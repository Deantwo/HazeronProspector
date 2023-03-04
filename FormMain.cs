using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace HazeronProspector
{
    public partial class FormMain : Form
    {
        HazeronStarMapReader _hStarMap;

        Dictionary<string, Galaxy> _galaxies = new Dictionary<string, Galaxy>();
        Dictionary<string, HSystem> _systems = new Dictionary<string, HSystem>();
        Dictionary<string, Sector> _sectors = new Dictionary<string, Sector>();

        Coordinate coord;

        Dictionary<string, bool> _columnVisability = new Dictionary<string, bool>();

        bool _errorCoordinate = true;

        bool _optionSystemWide = false;

        public FormMain()
        {
            InitializeComponent();

#if DEBUG
            this.Text += " (DEBUG MODE)";
#endif

            foreach (DataGridViewColumn column in dgvSurvey.Columns)
            {
                _columnVisability.Add(column.Name, true);
            }
            
            cobSelectionGalaxy.Enabled = false;
            rabSelectionDropdown.Enabled = false;
            cobSelectionSector.Enabled = false;
            cobSelectionSystem.Enabled = false;
            rabSelectionCoordinate.Enabled = false;
            tbxSelectionCoordinate.Enabled = false;
            rabFilterNone.Enabled = false;
            rabFilterRange.Enabled = false;
            nudFilterRange.Enabled = false;
            rabFilterWormhole.Enabled = false;
            nudFilterWormhole.Enabled = false;
            cbxOptionsSystemWide.Enabled = false;
            btnSearch.Enabled = false;

            toolStripProgressBar1.Visible = false;

            openFileDialog1.FileName = "<empire> Star Map";
            openFileDialog1.Filter = "Hazeron XML Star Map|*.xml|All files|*.*";
            //openFileDialog1.InitialDirectory = Application.StartupPath;

            toolStripStatusLabel1.Text = "No star map";
        }

        private void StarMapImport()
        {
            if (openFileDialog1.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
                return;
            _hStarMap = new HazeronStarMapReader(openFileDialog1.FileName);
            _hStarMap.ReadSectorsAndSystems(ref _galaxies, ref _sectors, ref _systems);

            if (_galaxies.Count == 0)
            {
                MessageBox.Show(this,
                    "The starmap XML export file does not contain any surveyed solar systems.",
                    "Starmap XML Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            cobSelectionGalaxy.Enabled = true;
            rabSelectionDropdown.Enabled = true;
            rabSelectionCoordinate.Enabled = true;
            rabSelection_Click(rabSelectionDropdown, null);
            rabFilterNone.Enabled = true;
            rabFilterRange.Enabled = true;
            rabFilterWormhole.Enabled = true;
            rabFilter_Click(rabFilterNone, null);
            cbxOptionsSystemWide.Enabled = true;
            btnSearch.Enabled = true;

            Galaxy[] galaxies = _galaxies.Values.OrderBy(x => x.Name).ToArray();
            cobSelectionGalaxy.Items.Clear();
            cobSelectionGalaxy.Items.AddRange(galaxies);
            cobSelectionGalaxy.SelectedIndex = 0;
            toolStripStatusLabel1.Text = "Star map loaded";
        }

        private void StarMapSearch()
        {
            toolStripStatusLabel1.Text = "Searching star map...";

            List<HSystem> selectedSystems = new List<HSystem>();

            // Selection of center
            if (rabSelectionDropdown.Checked) // Dropdown search
            {
                if (cobSelectionSector.SelectedIndex > 0) // Selected sector
                {
                    if (cobSelectionSystem.SelectedIndex > 0) // Selected system
                    {
                        HSystem system = (cobSelectionSystem.SelectedItem as HSystem);
                        selectedSystems.Add(system);
                        coord = system.Coord;
                    }
                    else // All systems in sector
                    {
                        Sector sector = (cobSelectionSector.SelectedItem as Sector);
                        selectedSystems.AddRange(sector.Systems.Values);
                        coord = sector.Coord;
                    }
                }
                else // All sectors in galaxy
                {
                    Galaxy galaxy = (cobSelectionGalaxy.SelectedItem as Galaxy);
                    foreach (Sector sector in galaxy.Sectors.Values)
                        selectedSystems.AddRange(sector.Systems.Values);
                    coord = new Coordinate();
                }
            }
            else if (rabSelectionCoordinate.Checked) // Coordinate search
            {
                if (!_errorCoordinate)
                {
                    string[] coordinateArray = tbxSelectionCoordinate.Text.Split(new string[] { ", ", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
                    coord = new Coordinate(
                        Double.Parse(coordinateArray[0], System.Globalization.NumberStyles.Number, Hazeron.NumberFormat),
                        Double.Parse(coordinateArray[1], System.Globalization.NumberStyles.Number, Hazeron.NumberFormat),
                        Double.Parse(coordinateArray[2], System.Globalization.NumberStyles.Number, Hazeron.NumberFormat));
                    HSystem system = _systems.Values.SingleOrDefault(x => x.Coord.Equals(coord));
                    if (system == null)
                    {
                        IEnumerable<Sector> sectors = _sectors.Values.Where(x => x.Coord.Distance(coord) < 10);
                        foreach (Sector sector in sectors)
                            selectedSystems.AddRange(sector.Systems.Values.Where(x => x.Coord.Distance(coord) < 0.5));
                    }
                }
                else
                {
                    MessageBox.Show(this, "The entered coordinate is not valid. Pleaese check it for errors or use another selection type.", "Invalid Coordinate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else // Error?
                throw new Exception("No selection type selected!?");

            // Filters for surounding systmes
            if (rabFilterRange.Checked && nudFilterRange.Value > 0) // Range filter
            {
                foreach (Sector sector in _sectors.Values)
                {
                    double dist = coord.Distance(sector.Coord);
                    if (dist <= (double)nudFilterRange.Value + 15)
                    {
                        foreach (HSystem system in sector.Systems.Values)
                        {
                            if (!selectedSystems.Contains(system))
                            {
                                dist = coord.Distance(system.Coord);
                                if (dist <= (double)nudFilterRange.Value)
                                {
                                    selectedSystems.Add(system);
                                }
                            }
                        }
                    }
                }
            }
            else if (rabFilterWormhole.Checked && nudFilterWormhole.Value > 0) // Wormhole filter
            {
                if (selectedSystems.Count == 0)
                {
                    MessageBox.Show(this, "The entered coordinate did not yield a system." + Environment.NewLine + "If no system is at or very close to the coordinate, then it is impossible to filter with wormhole jumps.", "No System at Coordinate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (rabSelectionDropdown.Checked && cobSelectionSector.SelectedIndex == 0)
                {
                    // If all sectors are selected, don't bother with this.
                }
                else
                {
                    List<HSystem> nextWormholeReach = selectedSystems.ToList();
                    for (int i = 0; i < nudFilterWormhole.Value; i++)
                    {
                        List<HSystem> currentWormholeReach = nextWormholeReach.ToList();
                        nextWormholeReach.Clear();
                        foreach (HSystem system in currentWormholeReach)
                        {
                            foreach (HSystem destinationSystem in system.WormholeLinks)
                            {
                                if (!selectedSystems.Contains(destinationSystem))
                                {
                                    nextWormholeReach.Add(destinationSystem);
                                    selectedSystems.Add(destinationSystem);
                                }
                            }
                        }
                    }
                }
            }


            // Initialize all selected systems that aren't already initialized.
            _hStarMap.InitializeSystems(selectedSystems.Where(x => !x.Initialized).ToList());

            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Step = 1;
            toolStripProgressBar1.Maximum = selectedSystems.Count;
            toolStripProgressBar1.Visible = true;
            // Clear and fill the table.
            TableClear();
            foreach (HSystem system in selectedSystems)
            {
                TableAddSystem(system);
                toolStripProgressBar1.PerformStep();
            }
            toolStripProgressBar1.Visible = false;
            dgvSurvey.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            toolStripStatusLabel1.Text = selectedSystems.Count + " systems found resulting in " + dgvSurvey.Rows.Count + " entries";
        }

        private void ResetTableSettings()
        {
            // Show all columns.
            foreach (DataGridViewColumn column in dgvSurvey.Columns)
            {
                _columnVisability[column.Name] = true;
                column.Visible = true;
            }
            // Unfreeze all columns.
            foreach (DataGridViewColumn column in dgvSurvey.Columns)
            {
                column.Frozen = false;
                column.DividerWidth = 0;
            }
            // Freeze specific column.
            if (!_optionSystemWide)
            {
                dgvSurvey.Columns["dgvSurveyColumnZone"].Frozen = true;
                dgvSurvey.Columns["dgvSurveyColumnZone"].DividerWidth = 2;
            }
            else
            {
                dgvSurvey.Columns["dgvSurveyColumnSystem"].Frozen = true;
                dgvSurvey.Columns["dgvSurveyColumnSystem"].DividerWidth = 2;
            }
        }

        #region Buttons
        private void btnImport_Click(object sender, EventArgs e)
        {
            StarMapImport();
        }

        private void rabSelection_Click(object sender, EventArgs e)
        {
            RadioButton reb = (sender as RadioButton);
            rabSelectionDropdown.Checked = (rabSelectionDropdown == reb);
            cobSelectionSector.Enabled = (rabSelectionDropdown == reb);
            cobSelectionSystem.Enabled = (rabSelectionDropdown == reb && cobSelectionSector.SelectedIndex > 0);
            rabSelectionCoordinate.Checked = (rabSelectionCoordinate == reb);
            tbxSelectionCoordinate.Enabled = (rabSelectionCoordinate == reb);
        }

        private void rabFilter_Click(object sender, EventArgs e)
        {
            RadioButton reb = (sender as RadioButton);
            rabFilterNone.Checked = (rabFilterNone == reb);
            rabFilterRange.Checked = (rabFilterRange == reb);
            nudFilterRange.Enabled = (rabFilterRange == reb);
            rabFilterWormhole.Checked = (rabFilterWormhole == reb);
            nudFilterWormhole.Enabled = (rabFilterWormhole == reb);
        }

        private void cobSelectionGalaxy_SelectedIndexChanged(object sender, EventArgs e)
        {
            cobSelectionSector.Items.Clear();
            cobSelectionSector.Items.Add("<All sectors>");
            cobSelectionSector.SelectedIndex = 0;
            Sector[] sectors = (cobSelectionGalaxy.SelectedItem as Galaxy).Sectors.Values.OrderBy(x => x.Name).ToArray();
            cobSelectionSector.Items.AddRange(sectors);
        }

        private void cobSelectionSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            cobSelectionSystem.Items.Clear();
            if (cobSelectionSector.SelectedIndex > 0)
            {
                cobSelectionSystem.Items.Add("<All systems>");
                cobSelectionSystem.SelectedIndex = 0;
                HSystem[] systems = (cobSelectionSector.SelectedItem as Sector).Systems.Values.OrderBy(x => x.Name).ToArray();
                cobSelectionSystem.Items.AddRange(systems);
                cobSelectionSystem.Enabled = true;
            }
            else
                cobSelectionSystem.Enabled = false;
        }

        private void tbxSelectionCoordinate_TextChanged(object sender, EventArgs e)
        {
            string[] coordinateArray = tbxSelectionCoordinate.Text.Split(new string[] { ", ", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            if (coordinateArray.Length != 3)
            {
                tbxSelectionCoordinate.ForeColor = Color.Red;
                _errorCoordinate = true;
                return;
            }
            foreach (string coordinate in coordinateArray)
            {
                double none;
                if (!Double.TryParse(coordinate, System.Globalization.NumberStyles.Number, Hazeron.NumberFormat, out none))
                {
                    tbxSelectionCoordinate.ForeColor = Color.Red;
                    _errorCoordinate = true;
                    return;
                }
            }
            tbxSelectionCoordinate.ForeColor = Color.Black;
            _errorCoordinate = false;
        }

        private void cbxOptionsSystemWide_Click(object sender, EventArgs e)
        {
            _optionSystemWide = !_optionSystemWide;
            cbxOptionsSystemWide.Checked = _optionSystemWide;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StarMapSearch();
        }
        #endregion

        #region DataGridView
        private void TableAddSystem(HSystem system)
        {
            dgvSurvey.Columns["dgvSurveyColumnZone"].Visible = _columnVisability["dgvSurveyColumnZone"] && !_optionSystemWide;
            dgvSurvey.Columns["dgvSurveyColumnDiameter"].Visible = _columnVisability["dgvSurveyColumnDiameter"] && !_optionSystemWide;
            dgvSurvey.Columns["dgvSurveyColumnBodyType"].Visible = _columnVisability["dgvSurveyColumnBodyType"] && !_optionSystemWide;

            if (_optionSystemWide)
            { // SystemWide mode
                dgvSurvey.Rows.Add();
                DataGridViewRow row = dgvSurvey.Rows[dgvSurvey.RowCount - 1];
                row.Cells["dgvSurveyColumnGalaxy"].Value = system.HostSector.HostGalaxy;
                row.Cells["dgvSurveyColumnSector"].Value = system.HostSector;
                row.Cells["dgvSurveyColumnSystem"].Value = system;
                row.Cells["dgvSurveyColumnSystemCatalog"].Value = system.CatalogName;
                row.Cells["dgvSurveyColumnWorld"].Value = system.CelestialBodies.Values.Count(x =>  x.Type != CelestialBodyType.Star && x.Type != CelestialBodyType.Ring) + " worlds (" + system.HabitbleCount() + " habitable)";
                row.Cells["dgvSurveyColumnOrbit"].Value = system.CelestialBodies.Values.Count(x => x.Type == CelestialBodyType.Star) + " Stars";
                row.Cells["dgvSurveyColumnCoordinates"].Value = system.Coord;
                foreach (Resource resource in system.BestResources().Values)
                {
                    TableAddResourceRow(row, resource);
                }
            }
            else
            { // Normal HazeronScouter mode
                foreach (CelestialBody planet in system.CelestialBodies.Values)
                {
                    foreach (Zone zone in planet.ResourceZones)
                    {
                        dgvSurvey.Rows.Add();
                        DataGridViewRow row = dgvSurvey.Rows[dgvSurvey.RowCount - 1];
                        row.Cells["dgvSurveyColumnGalaxy"].Value = system.HostSector.HostGalaxy;
                        row.Cells["dgvSurveyColumnSector"].Value = system.HostSector;
                        row.Cells["dgvSurveyColumnSystem"].Value = system;
                        row.Cells["dgvSurveyColumnSystemCatalog"].Value = system.CatalogName;
                        row.Cells["dgvSurveyColumnWorld"].Value = planet;
                        row.Cells["dgvSurveyColumnZone"].Value = zone;
                        switch (planet.Orbit)
                        {
                            case CelestialBodyOrbit.Inferno:
                                row.Cells["dgvSurveyColumnOrbit"].Value = "Inferno";
                                break;
                            case CelestialBodyOrbit.Inner:
                                row.Cells["dgvSurveyColumnOrbit"].Value = "Inner";
                                break;
                            case CelestialBodyOrbit.Habitable:
                                row.Cells["dgvSurveyColumnOrbit"].Value = "Habitable";
                                break;
                            case CelestialBodyOrbit.Outer:
                                row.Cells["dgvSurveyColumnOrbit"].Value = "Outer";
                                break;
                            case CelestialBodyOrbit.Frigid:
                                row.Cells["dgvSurveyColumnOrbit"].Value = "Frigid";
                                break;
                        }
                        row.Cells["dgvSurveyColumnCoordinates"].Value = system.Coord;
                        switch (planet.Type)
                        {
                            case CelestialBodyType.Star:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Star";
                                break;
                            case CelestialBodyType.NeutronStar:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Neutron Star";
                                break;
                            case CelestialBodyType.BlackHole:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Black Holes";
                                break;
                            case CelestialBodyType.Planet:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Planet";
                                break;
                            case CelestialBodyType.Moon:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Moon";
                                break;
                            case CelestialBodyType.GasGiant:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Gas Giant";
                                break;
                            case CelestialBodyType.Titan:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Titan";
                                break;
                            case CelestialBodyType.Ring:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Ring";
                                break;
                            case CelestialBodyType.Planetoid:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Planetoid";
                                break;
                            case CelestialBodyType.RingworldArc:
                                row.Cells["dgvSurveyColumnBodyType"].Value = "Ringworld Arc";
                                break;
                        }
                        if (planet.Diameter > 0 && planet.Type != CelestialBodyType.Ring)
                            row.Cells["dgvSurveyColumnDiameter"].Value = planet.Diameter;
                        foreach (Resource resource in zone.Resources.Values)
                        {
                            TableAddResourceRow(row, resource);
                        }
                    }
                }
            }
        }

        private void TableAddResourceRow(DataGridViewRow row, Resource resource)
        {
            DataGridViewCell cell = row.Cells["dgvSurveyColumnResource" + Enum.GetName(typeof(ResourceType), (int)resource.Type)];
            cell.Value = resource;
            cell.Style.ForeColor = resource.TechLevelColor;
            cell.Style.SelectionForeColor = resource.TechLevelColor;
            if (resource.HostZone.HostCelestialBody.ResourceZones.Length == 1
             || (resource.HostZone.HostCelestialBody.ResourceZones.Length > 1 && resource.AcrossZones))
                cell.ToolTipText = resource.HostZone.HostCelestialBody.Name;
            else
                cell.ToolTipText = resource.HostZone.HostCelestialBody.Name + ", " + resource.HostZone.Name;
        }

        private void TableClear()
        {
            dgvSurvey.Rows.Clear();
            dgvSurvey.Refresh();
        }

        private void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        { // Override of the DataGridView's normal SortCompare. This version converts some of the fields to numbers before sorting them.
            DataGridView dgv = (sender as DataGridView);
            string columnName = e.Column.Name;

            const int ColumnResourceNameLenght = 23; // "dgvSurveyColumnResource".Length
            if (columnName.Length > ColumnResourceNameLenght && columnName.Remove(ColumnResourceNameLenght) == "dgvSurveyColumnResource")
            {
                int value1 = -1;
                if ((e.CellValue1 as Resource) != null)
                    value1 = (e.CellValue1 as Resource).Quality;
                int value2 = -1;
                if ((e.CellValue2 as Resource) != null)
                    value2 = (e.CellValue2 as Resource).Quality;
                e.SortResult = CompareNumbers(value1, value2);

                if (e.SortResult == 0)
                {
                    value1 = -1;
                    if ((e.CellValue1 as Resource) != null)
                        value1 = (e.CellValue1 as Resource).Abundance;
                    value2 = -1;
                    if ((e.CellValue2 as Resource) != null)
                        value2 = (e.CellValue2 as Resource).Abundance;
                    e.SortResult = CompareNumbers(value1, value2);
                }
            }
            else
            {
                // Try to sort based on the cells in the current column as padded srtings.
                string value1 = (e.CellValue1 ?? "").ToString(), value2 = (e.CellValue2 ?? "").ToString();
                e.SortResult = CompareNumberStrings(value1, value2);
            }

            // If the cells are equal, sort based on the ID column.
            if (e.SortResult == 0)
            {
                e.SortResult = String.Compare(
                    dgv.Rows[e.RowIndex1].Cells["dgvSurveyColumnSector"].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells["dgvSurveyColumnSector"].Value.ToString());
            }
            if (e.SortResult == 0)
            {
                e.SortResult = String.Compare(
                    dgv.Rows[e.RowIndex1].Cells["dgvSurveyColumnSystem"].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells["dgvSurveyColumnSystem"].Value.ToString());
            }
            if (e.SortResult == 0)
            {
                e.SortResult = String.Compare(
                    dgv.Rows[e.RowIndex1].Cells["dgvSurveyColumnWorld"].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells["dgvSurveyColumnWorld"].Value.ToString());
            }
            if (e.SortResult == 0
             && dgv.Rows[e.RowIndex1].Cells["dgvSurveyColumnZone"].Value != null
             && dgv.Rows[e.RowIndex2].Cells["dgvSurveyColumnZone"].Value != null)
            {
                e.SortResult = String.Compare(
                    dgv.Rows[e.RowIndex1].Cells["dgvSurveyColumnZone"].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells["dgvSurveyColumnZone"].Value.ToString());
            }
            e.Handled = true;
        }

        /// <summary>
        /// Makes sure the string has one decimal separated with ".", and then pads the start of the string with spaces (" "s).
        /// </summary>
        private string Normalize(string s, int len)
        {
            s = s.Replace(',', '.');
            if (!s.Contains('.'))
                s += ".00";
            return s.PadLeft(len + 3);
        }

        private int CompareNumberStrings(string value1, string value2)
        {
            int maxLen = Math.Max(value1.Length, value2.Length);
            value1 = Normalize(value1, maxLen);
            value2 = Normalize(value2, maxLen);
            return String.Compare(value1, value2);
        }

        private int CompareNumbers(string value1, string value2)
        {
            double value1double = Double.Parse(value1, Hazeron.NumberFormat);
            double value2double = Double.Parse(value2, Hazeron.NumberFormat);
            return Math.Sign(value1double.CompareTo(value2double));
        }

        private int CompareNumbers(double value1, double value2)
        {
            return Math.Sign(value1.CompareTo(value2));
        }

        private int CompareNumbers(int value1, int value2)
        {
            return Math.Sign(value1.CompareTo(value2));
        }

        private void dgvSurvey_SelectionChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Entries " + dgvSurvey.Rows.Count;
            if (dgvSurvey.SelectedRows != null && dgvSurvey.SelectedRows.Count > 0)
                toolStripStatusLabel1.Text += " (" + dgvSurvey.SelectedRows.Count + " selected)";
        }
        #endregion

        #region DataGridView ContextMenu RightClick
        private void dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        { // Based on: http://stackoverflow.com/questions/1718389/right-click-context-menu-for-datagrid.
            if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                DataGridView dgv = (sender as DataGridView);
                dgv.ContextMenuStrip = cmsRightClick;
                DataGridViewCell currentCell = dgv[e.ColumnIndex, e.RowIndex];
                currentCell.DataGridView.ClearSelection();
                currentCell.DataGridView.CurrentCell = currentCell;
                currentCell.Selected = true;
                //Rectangle r = currentCell.DataGridView.GetCellDisplayRectangle(currentCell.ColumnIndex, currentCell.RowIndex, false);
                //Point p = new Point(r.X + r.Width, r.Y + r.Height);
                Point p = dgv.PointToClient(Control.MousePosition);
                dgv.ContextMenuStrip.Show(currentCell.DataGridView, p);
                dgv.ContextMenuStrip = null;
            }
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        { // Based on: http://stackoverflow.com/questions/1718389/right-click-context-menu-for-datagrid.
            DataGridView dgv = (sender as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;
            if (currentCell != null)
            {
                cmsRightClick_Opening(sender, null);
                if ((e.KeyCode == Keys.F10 && !e.Control && e.Shift) || e.KeyCode == Keys.Apps)
                {
                    dgv.ContextMenuStrip = cmsRightClick;
                    Rectangle r = currentCell.DataGridView.GetCellDisplayRectangle(currentCell.ColumnIndex, currentCell.RowIndex, false);
                    Point p = new Point(r.X + r.Width, r.Y + r.Height);
                    dgv.ContextMenuStrip.Show(currentCell.DataGridView, p);
                    dgv.ContextMenuStrip = null;
                }
                else if (e.KeyCode == Keys.C && e.Control && !e.Shift)
                    cmsRightClickCopy_Click(sender, null);
            }
        }

        private void cmsRightClick_Opening(object sender, CancelEventArgs e)
        {
            // Get table in question and currentCell.
            DataGridView dgv = (sender as DataGridView);
            if (dgv == null)
                dgv = ((sender as ContextMenuStrip).SourceControl as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;

            // Freeze Column.
            DataGridViewColumn nextCoumn = dgv.Columns.GetNextColumn(currentCell.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
            if (currentCell.OwningColumn.Frozen && (nextCoumn == null || !nextCoumn.Frozen))
                cmsRightClickFreezeColumn.Text = "Unfreeze Column";
            else
                cmsRightClickFreezeColumn.Text = "Freeze Column";

        }

        private void cmsRightClickCopy_Click(object sender, EventArgs e)
        { // http://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
            DataGridView dgv = (sender as DataGridView);
            if (dgv == null)
                dgv = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;
            if (currentCell != null)
            {
                // Check if the cell is empty.
                if (currentCell.Value != null && currentCell.Value.ToString() != String.Empty)
                { // If not empty, add to clipboard and inform the user.
                    Clipboard.SetText(currentCell.Value.ToString());
                    toolStripStatusLabel1.Text = "Cell content copied to clipboard (\"" + currentCell.Value.ToString() + "\")";
                }
                else
                { // Inform the user the cell was empty and therefor no reason to erase the clipboard.
                    toolStripStatusLabel1.Text = "Cell is empty";
#if DEBUG
                    // Debug code to see if the cell is null or "".
                    if (dgv.CurrentCell.Value == null)
                        toolStripStatusLabel1.Text += " (null)";
                    else
                        toolStripStatusLabel1.Text += " (\"\")";
#endif
                }
            }
        }

        private void cmsRightClickHideColumn_Click(object sender, EventArgs e)
        { // http://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
            DataGridView dgv = (sender as DataGridView);
            if (dgv == null)
                dgv = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;
            if (currentCell != null)
            {
                // Hide the column.
                currentCell.OwningColumn.Visible = false;
                toolStripStatusLabel1.Text = "\"" + currentCell.OwningColumn.HeaderText + "\" column hidden";
            }
        }

        private void cmsRightClickFreezeColumn_Click(object sender, EventArgs e)
        { // http://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
            DataGridView dgv = (sender as DataGridView);
            if (dgv == null)
                dgv = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;
            if (currentCell != null)
            {
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    column.DividerWidth = 0;
                }
                DataGridViewColumn nextCoumn = dgv.Columns.GetNextColumn(currentCell.OwningColumn, DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                // Is it frozen already?
                if (currentCell.OwningColumn.Frozen && (nextCoumn == null || !nextCoumn.Frozen))
                {
                    // Unfreeze all columns.
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        column.Frozen = false;
                    }
                    toolStripStatusLabel1.Text = "\"" + currentCell.OwningColumn.HeaderText + "\" column unfrozen";
                }
                else
                {
                    // Freeze the column.
                    currentCell.OwningColumn.Frozen = true;
                    if (nextCoumn != null)
                        nextCoumn.Frozen = false;
                    currentCell.OwningColumn.DividerWidth = 2;
                    toolStripStatusLabel1.Text = "\"" + currentCell.OwningColumn.HeaderText + "\" column frozen";
                }
            }
        }

        private void cmsRightClickHideRow_Click(object sender, EventArgs e)
        { // http://stackoverflow.com/questions/4886327/determine-what-control-the-contextmenustrip-was-used-on
            DataGridView dgv = (sender as DataGridView);
            if (dgv == null)
                dgv = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as DataGridView);
            DataGridViewCell currentCell = dgv.CurrentCell;
            if (currentCell != null)
            {
                // Hide the selected row(s).
                foreach (DataGridViewRow row in dgv.SelectedRows)
                    row.Visible = false;
                toolStripStatusLabel1.Text = dgv.SelectedRows.Count + " row(s) hidden";
            }
        }
        #endregion

        #region menuStrip1
        private void menuStrip1FileImport_Click(object sender, EventArgs e)
        {
            StarMapImport();
        }

        private void menuStrip1FileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuStrip1OptionsShowHidenRows_Click(object sender, EventArgs e)
        {
            // Show all rows.
            foreach (DataGridViewRow row in dgvSurvey.Rows)
            {
                row.Visible = true;
            }
        }

        private void menuStrip1OptionsResetTableSettings_Click(object sender, EventArgs e)
        {
            ResetTableSettings();
        }

        private void menuStrip1HelpGithub_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://github.com/Deantwo/HazeronProspector");
        }

        private void menuStrip1HelpThread_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://hazeron.com/mybb/showthread.php?tid=31");
        }

        private void menuStrip1HelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Program:" + Environment.NewLine +
                "   HazeronProspector" + Environment.NewLine +
                "" + Environment.NewLine +
                "Version:" + Environment.NewLine +
                "   " + Application.ProductVersion + Environment.NewLine +
                "" + Environment.NewLine +
                "Creator:" + Environment.NewLine +
                "   Deantwo" + Environment.NewLine +
                "" + Environment.NewLine +
                "Feedback, suggestions, and bug reports should be posted in the forum thread or PMed to Deantwo please."
                , "About HazeronProspector", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void menuStrip1HelpHowToUse_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Export StarMap XML:" + Environment.NewLine +
                "1.  Log into Hazeron with your character" + Environment.NewLine +
                "2.  Open the Locator window (F7)" + Environment.NewLine +
                "4.  Go to the Star Map tab" + Environment.NewLine +
                "5.  Click the \"Explored/Scanned/Detected Space\" button" + Environment.NewLine +
                "6.  Click the \"Save Starmap\" button" + Environment.NewLine +
                "7.  Choose a location for the .xml file that you will be able to find later"
                , "How to use HazeronProspector", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HazeronProspector
{
    public partial class FormSystemOptimizer : Form
    {
        HSystem _system;

        Dictionary<ResourceType, Resource> _resourceBest;

        public FormSystemOptimizer(HSystem system)
        {
            InitializeComponent();
            Initialize(system);
        }

        public void Initialize(HSystem system)
        {
            _system = system;

            comboBox1.SelectedIndex = 0;

            this.Text += " - " + _system.Name;
            tbxSystem.Text = _system.Name;
            tbxSector.Text = _system.HostSector.Name;
            tbxGalaxy.Text = _system.HostSector.HostGalaxy.Name;
            tbxCoordinates.Text = _system.Coord.ToString();

            CreateResourceTable(_system);
            CreateLocationTable(_system);
        }

        /// <summary>
        /// Resource filter option. Skip geosphere resources if "bioshpere resourcces" is selected, and so on.
        /// </summary>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>Returns true if the resource should be skipped.</returns>
        private bool FilterResource(ResourceType resourceType)
        {
            if ((comboBox1.SelectedIndex == 2
              && (Resource.GetCategory(resourceType) == ResourceCategory.Geosphere
               || Resource.GetCategory(resourceType) == ResourceCategory.Hydrophere
               || Resource.GetCategory(resourceType) == ResourceCategory.Atmosphere))
             || (comboBox1.SelectedIndex == 1
              && Resource.GetCategory(resourceType) == ResourceCategory.Biosphere)
             || Resource.GetCategory(resourceType) == ResourceCategory.Photophere)
                return true;
            return false;
        }

        #region Buttons
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
                UpdateTables();
        }

        private void btnSuggestCities_Click(object sender, EventArgs e)
        {
            // Remove all City checkmarks.
            foreach (DataGridViewRow row in dgvSuggentions.Rows)
            {
                bool city = (bool)row.Cells[nameof(dgvSuggentionsColumnCity)].Value;
                if (city)
                    row.Cells[nameof(dgvSuggentionsColumnCity)].Value = false;
            }

            // Mark all worlds with a best resource.
            Dictionary<ResourceType, Resource> bestResources = _system.BestResources(true);
            foreach (Resource resource in bestResources.Values)
            {
                if (FilterResource(resource.Type))
                    continue;

                foreach (DataGridViewRow row in dgvSuggentions.Rows)
                {
                    bool hasABest;
                    if (resource.AcrossZones)
                        hasABest = resource.HostZone.HostCelestialBody.Equals(((Zone)row.Cells[nameof(dgvSuggentionsColumnZone)].Value).HostCelestialBody);
                    else
                        hasABest = resource.HostZone.Equals((Zone)row.Cells[nameof(dgvSuggentionsColumnZone)].Value);
                    if (hasABest)
                    {
                        bool city = (bool)row.Cells[nameof(dgvSuggentionsColumnCity)].Value;
                        if (!city)
                            row.Cells[nameof(dgvSuggentionsColumnCity)].Value = true;
                    }
                }
            }
        }
        #endregion

        #region DataGridView
        private void CreateResourceTable(HSystem system)
        {
            Dictionary<ResourceType, Resource> bestResources = _system.BestResources(true);
            foreach (Resource resource in bestResources.Values)
            {
                DataGridViewRow row = dgvResources.Rows[dgvResources.Rows.Add()];
                row.Cells[nameof(dgvResourcesColumnResource)].Value = resource.Name;
                row.Cells[nameof(dgvResourcesColumnCurrent)].Value = null;
                row.Cells[nameof(dgvResourcesColumnBest)].Value = resource;
                if (resource.HostZone.HostCelestialBody.ResourceZones.Length == 1
                 || (resource.HostZone.HostCelestialBody.ResourceZones.Length > 1
                  && resource.AcrossZones))
                    row.Cells[nameof(dgvResourcesColumnBest)].ToolTipText = resource.HostZone.HostCelestialBody.Name;
                else
                    row.Cells[nameof(dgvResourcesColumnBest)].ToolTipText = $"{resource.HostZone.HostCelestialBody.Name}, {resource.HostZone.Name}";
                row.Cells[nameof(dgvResourcesColumnBest)].Style.ForeColor = resource.TechLevelColor;
            }
            dgvResources.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }

        private void CreateLocationTable(HSystem system)
        {
            foreach (CelestialBody world in system.CelestialBodies.Values)
            {
                foreach (Zone zone in world.ResourceZones)
                {
                    DataGridViewRow row = dgvSuggentions.Rows[dgvSuggentions.Rows.Add()];
                    row.Cells[nameof(dgvSuggentionsColumnWorld)].Value = world;
                    row.Cells[nameof(dgvSuggentionsColumnZone)].Value = zone;
                    row.Cells[nameof(dgvSuggentionsColumnOrbit)].Value = world.Orbit;
                    row.Cells[nameof(dgvSuggentionsColumnCity)].Value = false;
                    row.Cells[nameof(dgvSuggentionsColumnHarvest)].Value = string.Empty;

                    // Color row.
                    if (world.Type == CelestialBodyType.Star
                     || world.Type == CelestialBodyType.Ring)
                        row.DefaultCellStyle.BackColor = Color.LightPink;
                    else if (world.Type == CelestialBodyType.GasGiant)
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                    else if (world.IsHabitable)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
            }
            dgvSuggentions.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            dgvSuggentions.Sort(dgvSuggentionsColumnWorld, ListSortDirection.Ascending);
        }

        private void dgvSuggentions_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // End of edition on each click on column of checkbox
            if (e.ColumnIndex == dgvSuggentionsColumnCity.Index && e.RowIndex != -1)
            {
                dgvSuggentions.EndEdit();
            }
        }

        private void dgvSuggentions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvSuggentionsColumnCity.Index && e.RowIndex != -1)
                UpdateTables();
        }

        private void UpdateTables()
        {
            Dictionary<ResourceType, List<Resource>> resourceOrders = new Dictionary<ResourceType, List<Resource>>();
            _resourceBest = new Dictionary<ResourceType, Resource>();
            Dictionary<Zone, string> cityHarvest = new Dictionary<Zone, string>();
            foreach (DataGridViewRow row in dgvSuggentions.Rows)
            {
                bool city = (bool)row.Cells[nameof(dgvSuggentionsColumnCity)].Value;
                if (city)
                {
                    Zone zone = (Zone)row.Cells[nameof(dgvSuggentionsColumnZone)].Value;

                    cityHarvest.Add(zone, string.Empty);

                    foreach (KeyValuePair<ResourceType, Resource> resourceEntry in zone.Resources)
                    {
                        if (FilterResource(resourceEntry.Key))
                            continue;
        
                        if (!resourceOrders.ContainsKey(resourceEntry.Key))
                            resourceOrders.Add(resourceEntry.Key, new List<Resource>());
                        resourceOrders[resourceEntry.Key].Add(resourceEntry.Value);
                    }
                }
                else
                    row.Cells[nameof(dgvSuggentionsColumnHarvest)].Value = string.Empty;
            }
            if (cityHarvest.Count != 0)
            {
                foreach (KeyValuePair<ResourceType, List<Resource>> resourceEntry in resourceOrders)
                {
                    _resourceBest.Add(resourceEntry.Key, resourceEntry.Value.OrderByDescending(x => x.Quality).ToList()[0]);
                }
                foreach (KeyValuePair<ResourceType, Resource> resourceEntry in _resourceBest)
                {
                    Resource resource = resourceEntry.Value;
                    Zone zone = resource.HostZone;

                    // Ass the resouce to the 
                    if (!string.IsNullOrEmpty(cityHarvest[zone]))
                        cityHarvest[zone] += ", ";
                    cityHarvest[zone] += resource.Name;

                    // If the resource is world-wide, add it to all the zones on the world.
                    if (resourceEntry.Value.AcrossZones && zone.HostCelestialBody.ResourceZones.Length > 1)
                    {
                        foreach (Zone zoneSibling in zone.HostCelestialBody.ResourceZones)
                        {
                            if (zoneSibling != zone && cityHarvest.ContainsKey(zoneSibling))
                            {
                                if (!string.IsNullOrEmpty(cityHarvest[zoneSibling]))
                                    cityHarvest[zoneSibling] += ", ";
                                cityHarvest[zoneSibling] += resource.Name;
                            }
                        }
                    }
                }
                foreach (DataGridViewRow row in dgvSuggentions.Rows)
                {
                    Zone zone = (Zone)row.Cells[nameof(dgvSuggentionsColumnZone)].Value;
                    if (cityHarvest.ContainsKey(zone))
                    {
                        DataGridViewCell cell = row.Cells[nameof(dgvSuggentionsColumnHarvest)];
                        cell.Value = cityHarvest[zone];
                    }
                }
            }

            // Update the reources table.
            foreach (DataGridViewRow row in dgvResources.Rows)
            {
                ResourceType resourceType = (row.Cells[nameof(dgvResourcesColumnBest)].Value as Resource).Type;
                DataGridViewCell cell = row.Cells[nameof(dgvResourcesColumnCurrent)];

                // Update quality of each resource on the list and add colors.
                row.DefaultCellStyle.BackColor = Color.White;
                if (_resourceBest.ContainsKey(resourceType))
                {
                    cell.Value = _resourceBest[resourceType];
                    if (_resourceBest[resourceType].HostZone.HostCelestialBody.ResourceZones.Length == 1
                     || (_resourceBest[resourceType].HostZone.HostCelestialBody.ResourceZones.Length > 1
                      && Resource.IsAcrossZones(resourceType)))
                        cell.ToolTipText = _resourceBest[resourceType].HostZone.HostCelestialBody.Name;
                    else
                        cell.ToolTipText = $"{_resourceBest[resourceType].HostZone.HostCelestialBody.Name}, {_resourceBest[resourceType].HostZone.Name}";
                    cell.Style.ForeColor = _resourceBest[resourceType].TechLevelColor;
                    // Color the row green if the current is best.
                    if (((Resource)row.Cells[nameof(dgvResourcesColumnBest)].Value).Quality == _resourceBest[resourceType].Quality)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                    cell.Value = null;

                if (FilterResource(resourceType))
                    row.Visible = false;
                else
                    row.Visible = true;
            }
        }

        private void dgv_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        { // Override of the DataGridView's normal SortCompare. This version converts some of the fields to numbers before sorting them.
            DataGridView dgv = (sender as DataGridView);
            string columnName = e.Column.Name;

            if (columnName == nameof(dgvSuggentionsColumnWorld))
            {
                CelestialBody value1 = (CelestialBody)e.CellValue1;
                CelestialBody value2 = (CelestialBody)e.CellValue2;
                e.SortResult = value1.CompareTo(value2);
            }
            else if (columnName == nameof(dgvSuggentionsColumnCity))
            {
                bool value1 = (bool)e.CellValue1;
                bool value2 = (bool)e.CellValue2;
                e.SortResult = value1.CompareTo(value2);
            }
            else if (columnName == nameof(dgvSuggentionsColumnHarvest))
            {
                int value1 = -1;
                if ((string)e.CellValue1 != null)
                    value1 = e.CellValue1.ToString().Length;
                int value2 = -1;
                if ((string)e.CellValue2 != null)
                    value2 = e.CellValue2.ToString().Length;
                e.SortResult = CompareNumbers(value1, value2);

                if (e.SortResult == 0)
                {
                    e.SortResult = string.Compare(
                        e.CellValue1.ToString(),
                        e.CellValue2.ToString());
                }
            }
            else
            {
                // Try to sort based on the cells in the current column as srtings.
                e.SortResult = string.Compare((e.CellValue1 ?? string.Empty).ToString(), (e.CellValue2 ?? string.Empty).ToString());
            }

            // If the cells are equal, sort based on the ID column.
            if (e.SortResult == 0)
            {
                e.SortResult = string.Compare(
                    dgv.Rows[e.RowIndex1].Cells[nameof(dgvSuggentionsColumnWorld)].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells[nameof(dgvSuggentionsColumnWorld)].Value.ToString());
            }
            if (e.SortResult == 0)
            {
                e.SortResult = string.Compare(
                    dgv.Rows[e.RowIndex1].Cells[nameof(dgvSuggentionsColumnZone)].Value.ToString(),
                    dgv.Rows[e.RowIndex2].Cells[nameof(dgvSuggentionsColumnZone)].Value.ToString());
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
            return string.Compare(value1, value2);
        }

        private int CompareNumbers(string value1, string value2)
        {
            double value1double = double.Parse(value1, Hazeron.NumberFormat);
            double value2double = double.Parse(value2, Hazeron.NumberFormat);
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
                if (currentCell.Value != null && currentCell.Value.ToString() != string.Empty)
                { // If not empty, add to clipboard and inform the user.
                    Clipboard.SetText(currentCell.Value.ToString());
                    toolStripStatusLabel1.Text = $"Cell content copied to clipboard (\"{currentCell.Value}\")";
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
                toolStripStatusLabel1.Text = $"\"{currentCell.OwningColumn.HeaderText}\" column hidden";
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
                toolStripStatusLabel1.Text = $"{dgv.SelectedRows.Count} row(s) hidden";
            }
        }
        #endregion
    }
}

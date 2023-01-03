using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HazeronProspector
{
    public partial class FormSystemMapConvertor : Form
    {
        HSystem _solarSystem;
        public HSystem ReturnValue => _solarSystem;

        Dictionary<string, TreeNode> _treeNodes = new Dictionary<string, TreeNode>();

        public FormSystemMapConvertor()
        {
            InitializeComponent();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!ckbxAuto.Checked)
                return;

            TextBox tb = sender as TextBox;
            tb.TextChanged -= textBox1_TextChanged;
            btnSubmit_Click(sender, e);
            tb.TextChanged += textBox1_TextChanged;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(this, "No input. Please copy-paste the SystemMap survey details into the textbox on the left.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = textBox1.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            if (lines[1].StartsWith("Catalog Name "))
            {
                string catalogName = lines[1].Substring(lines[1].LastIndexOf(" ") + 1);
                string name = lines[0];
                Tuple<double, double, double> coordinates = Hazeron.ConvertCalalogNameToCoordinates(catalogName);

                _solarSystem = new HSystem(catalogName, name,
                    coordinates.Item1.ToString(System.Globalization.NumberFormatInfo.InvariantInfo),
                    coordinates.Item2.ToString(System.Globalization.NumberFormatInfo.InvariantInfo),
                    coordinates.Item3.ToString(System.Globalization.NumberFormatInfo.InvariantInfo), "Surveyed");

                TreeNode mainNode = new TreeNode();
                //mainNode.Name = "mainNode";
                mainNode.Text = name;
                treeView1.Nodes.Add(mainNode);
                _treeNodes.Clear();
            }
            else if (lines[1] == "Primary" || lines[1].StartsWith("Orbiting "))
            {
                // Read basic body information and generate celestial body.
                string name = lines[0];
                string type = lines[2];
                string orbit = "n/a";
                string size = "n/a";
                if (type.StartsWith("Type "))
                    type = "Photosphere";
                else if (type == "Stellar Black Hole")
                    type = "Photon Sphere";
                else if (type == "Neutron Star")
                    type = "Neutron Sphere";
                else if (type.EndsWith(" Zone") && lines[4] == "Geosphere")
                {
                    type = lines[5].Remove(lines[5].IndexOf(", "));
                    orbit = lines[2];
                    size = lines[5].Substring(lines[5].IndexOf(", ") + 2);
                }
                string patentName = lines[1];
                if (patentName == "Primary")
                    patentName = null;
                else
                {
                    patentName = patentName.Substring("Orbiting ".Length);
                    patentName = patentName.Remove(patentName.LastIndexOf(" at "));
                }

                // Read resource information and generate celestial body's zones.
                Zone[] zones = null;
                if (lines.Contains("Resource"))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i] == "Resource")
                        {
                            for (; i < lines.Length; i += 3)
                            {
                                if (lines[i + 1] == ""
                                    || lines[i + 1] == "Geosphere"
                                    || lines[i + 1] == "Hydrosphere"
                                    || lines[i + 1] == "Atmosphere"
                                    || lines[i + 1] == "Biosphere Potential")
                                    break;

                                if (lines[i] == "Resource")
                                {
                                    if (zones is null)
                                    {
                                        if (lines[i + 4] == "Zone 4")
                                            zones = new Zone[] { new Zone(1), new Zone(2), new Zone(3), new Zone(4) };
                                        else if (lines[i + 3] == "Zone 3")
                                            zones = new Zone[] { new Zone(1), new Zone(2), new Zone(3) };
                                        else if (lines[i + 2] == "Zone 2")
                                            zones = new Zone[] { new Zone(1), new Zone(2) };
                                        else if (lines[i + 1] == "Quality and Abundance")
                                            zones = new Zone[] { new Zone(1) };
                                    }
                                }
                                else
                                {
                                    string resourceName = lines[i];
                                    for (int z = 0; z < zones.Length; z++)
                                    {
                                        string line;
                                        if (zones.Length == 1 || lines[i + 2] == "￼" || lines[i + 2] == "")
                                            line = lines[i + 1];
                                        else
                                            line = lines[i + 1 + z];

                                        string q, a;
                                        if (line == "None")
                                        {
                                            q = "0";
                                            a = "0";
                                        }
                                        else if (line.Contains(' '))
                                        {
                                            q = line.Remove(line.IndexOf(' ')).TrimStart('Q');
                                            a = line.Substring(line.IndexOf(' ')).TrimEnd('%');
                                        }
                                        else
                                        {
                                            q = line.TrimStart('Q');
                                            a = "1";
                                        }

                                        zones[z].AddResource(new Resource(resourceName, q, a));
                                    }
                                }

                                if (lines[i + 2] == "")
                                    break;
                                else if (zones.Length != 1 && lines[i + 2] != "￼")
                                {
                                    if (zones.Length == 2)
                                        i += 3 - 1;
                                    else
                                        i += zones.Length - 1;
                                }
                            }
                        }
                    }
                }
                else
                    zones = new Zone[1];

                CelestialBody body = new CelestialBody(name, name, type, orbit, size, zones);

                _solarSystem.CelestialBodies.Add(name, body);

                TreeNode subNode = new TreeNode();
                subNode.Text = name;
                TreeNode patentNode;
                if (patentName is null)
                    patentNode = treeView1.Nodes[0];
                else
                    patentNode = _treeNodes[patentName];
                patentNode.Nodes.Add(subNode);
                patentNode.Expand();
                _treeNodes.Add(name, subNode);
            }

            textBox1.Clear();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}

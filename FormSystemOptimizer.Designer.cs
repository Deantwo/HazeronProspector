namespace HazeronProspector
{
    partial class FormSystemOptimizer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxCoordinates = new System.Windows.Forms.TextBox();
            this.tbxGalaxy = new System.Windows.Forms.TextBox();
            this.tbxSector = new System.Windows.Forms.TextBox();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.tbxSystem = new System.Windows.Forms.TextBox();
            this.lblGalaxy = new System.Windows.Forms.Label();
            this.lblSector = new System.Windows.Forms.Label();
            this.lblSystem = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvResources = new System.Windows.Forms.DataGridView();
            this.dgvResourcesColumnResource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvResourcesColumnCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvResourcesColumnBest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvSuggentions = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvSuggentionsColumnPlanet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSuggentionsColumnZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSuggentionsColumnOrbit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSuggentionsColumnCity = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvSuggentionsColumnHarvest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuggentions)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tbxCoordinates);
            this.groupBox1.Controls.Add(this.tbxGalaxy);
            this.groupBox1.Controls.Add(this.tbxSector);
            this.groupBox1.Controls.Add(this.lblCoordinates);
            this.groupBox1.Controls.Add(this.tbxSystem);
            this.groupBox1.Controls.Add(this.lblGalaxy);
            this.groupBox1.Controls.Add(this.lblSector);
            this.groupBox1.Controls.Add(this.lblSystem);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 123);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "System Details";
            // 
            // tbxCoordinates
            // 
            this.tbxCoordinates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxCoordinates.Location = new System.Drawing.Point(75, 19);
            this.tbxCoordinates.Name = "tbxCoordinates";
            this.tbxCoordinates.ReadOnly = true;
            this.tbxCoordinates.Size = new System.Drawing.Size(134, 20);
            this.tbxCoordinates.TabIndex = 2;
            // 
            // tbxGalaxy
            // 
            this.tbxGalaxy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxGalaxy.Location = new System.Drawing.Point(75, 71);
            this.tbxGalaxy.Name = "tbxGalaxy";
            this.tbxGalaxy.ReadOnly = true;
            this.tbxGalaxy.Size = new System.Drawing.Size(134, 20);
            this.tbxGalaxy.TabIndex = 2;
            // 
            // tbxSector
            // 
            this.tbxSector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxSector.Location = new System.Drawing.Point(75, 45);
            this.tbxSector.Name = "tbxSector";
            this.tbxSector.ReadOnly = true;
            this.tbxSector.Size = new System.Drawing.Size(134, 20);
            this.tbxSector.TabIndex = 2;
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.Location = new System.Drawing.Point(6, 22);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(63, 13);
            this.lblCoordinates.TabIndex = 1;
            this.lblCoordinates.Text = "Coordinates";
            // 
            // tbxSystem
            // 
            this.tbxSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxSystem.Location = new System.Drawing.Point(75, 97);
            this.tbxSystem.Name = "tbxSystem";
            this.tbxSystem.ReadOnly = true;
            this.tbxSystem.Size = new System.Drawing.Size(134, 20);
            this.tbxSystem.TabIndex = 2;
            // 
            // lblGalaxy
            // 
            this.lblGalaxy.AutoSize = true;
            this.lblGalaxy.Location = new System.Drawing.Point(6, 74);
            this.lblGalaxy.Name = "lblGalaxy";
            this.lblGalaxy.Size = new System.Drawing.Size(39, 13);
            this.lblGalaxy.TabIndex = 1;
            this.lblGalaxy.Text = "Galaxy";
            // 
            // lblSector
            // 
            this.lblSector.AutoSize = true;
            this.lblSector.Location = new System.Drawing.Point(6, 48);
            this.lblSector.Name = "lblSector";
            this.lblSector.Size = new System.Drawing.Size(38, 13);
            this.lblSector.TabIndex = 1;
            this.lblSector.Text = "Sector";
            // 
            // lblSystem
            // 
            this.lblSystem.AutoSize = true;
            this.lblSystem.Location = new System.Drawing.Point(6, 100);
            this.lblSystem.Name = "lblSystem";
            this.lblSystem.Size = new System.Drawing.Size(41, 13);
            this.lblSystem.TabIndex = 0;
            this.lblSystem.Text = "System";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgvResources);
            this.groupBox2.Location = new System.Drawing.Point(3, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 400);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Resource";
            // 
            // dgvResources
            // 
            this.dgvResources.AllowUserToAddRows = false;
            this.dgvResources.AllowUserToDeleteRows = false;
            this.dgvResources.AllowUserToResizeRows = false;
            this.dgvResources.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvResourcesColumnResource,
            this.dgvResourcesColumnCurrent,
            this.dgvResourcesColumnBest});
            this.dgvResources.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvResources.Location = new System.Drawing.Point(6, 19);
            this.dgvResources.Name = "dgvResources";
            this.dgvResources.RowHeadersVisible = false;
            this.dgvResources.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvResources.Size = new System.Drawing.Size(200, 375);
            this.dgvResources.TabIndex = 0;
            // 
            // dgvResourcesColumnResource
            // 
            this.dgvResourcesColumnResource.Frozen = true;
            this.dgvResourcesColumnResource.HeaderText = "Resource";
            this.dgvResourcesColumnResource.Name = "dgvResourcesColumnResource";
            this.dgvResourcesColumnResource.ReadOnly = true;
            // 
            // dgvResourcesColumnCurrent
            // 
            this.dgvResourcesColumnCurrent.FillWeight = 50F;
            this.dgvResourcesColumnCurrent.HeaderText = "Current";
            this.dgvResourcesColumnCurrent.Name = "dgvResourcesColumnCurrent";
            this.dgvResourcesColumnCurrent.ReadOnly = true;
            this.dgvResourcesColumnCurrent.Width = 50;
            // 
            // dgvResourcesColumnBest
            // 
            this.dgvResourcesColumnBest.FillWeight = 50F;
            this.dgvResourcesColumnBest.HeaderText = "Best";
            this.dgvResourcesColumnBest.Name = "dgvResourcesColumnBest";
            this.dgvResourcesColumnBest.ReadOnly = true;
            this.dgvResourcesColumnBest.Width = 50;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.dgvSuggentions);
            this.groupBox3.Location = new System.Drawing.Point(3, 39);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 493);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Locations";
            // 
            // dgvSuggentions
            // 
            this.dgvSuggentions.AllowUserToAddRows = false;
            this.dgvSuggentions.AllowUserToDeleteRows = false;
            this.dgvSuggentions.AllowUserToResizeRows = false;
            this.dgvSuggentions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSuggentions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSuggentions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvSuggentionsColumnPlanet,
            this.dgvSuggentionsColumnZone,
            this.dgvSuggentionsColumnOrbit,
            this.dgvSuggentionsColumnCity,
            this.dgvSuggentionsColumnHarvest});
            this.dgvSuggentions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSuggentions.Location = new System.Drawing.Point(6, 19);
            this.dgvSuggentions.Name = "dgvSuggentions";
            this.dgvSuggentions.RowHeadersVisible = false;
            this.dgvSuggentions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvSuggentions.Size = new System.Drawing.Size(425, 468);
            this.dgvSuggentions.TabIndex = 0;
            this.dgvSuggentions.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellMouseDown);
            this.dgvSuggentions.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSuggentions_CellMouseUp);
            this.dgvSuggentions.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuggentions_CellValueChanged);
            this.dgvSuggentions.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dgv_SortCompare);
            this.dgvSuggentions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_KeyDown);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 535);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(668, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(622, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmsRightClick
            // 
            this.cmsRightClick.Name = "cmsRightClick";
            this.cmsRightClick.Size = new System.Drawing.Size(61, 4);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "All Resources",
            "Geosphere Resources",
            "Biosphere Resources"});
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.comboBox1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(668, 535);
            this.splitContainer1.SplitterDistance = 443;
            this.splitContainer1.TabIndex = 8;
            // 
            // dgvSuggentionsColumnPlanet
            // 
            this.dgvSuggentionsColumnPlanet.Frozen = true;
            this.dgvSuggentionsColumnPlanet.HeaderText = "Planet";
            this.dgvSuggentionsColumnPlanet.Name = "dgvSuggentionsColumnPlanet";
            this.dgvSuggentionsColumnPlanet.ReadOnly = true;
            // 
            // dgvSuggentionsColumnZone
            // 
            this.dgvSuggentionsColumnZone.HeaderText = "Zone";
            this.dgvSuggentionsColumnZone.MinimumWidth = 20;
            this.dgvSuggentionsColumnZone.Name = "dgvSuggentionsColumnZone";
            this.dgvSuggentionsColumnZone.ReadOnly = true;
            this.dgvSuggentionsColumnZone.Width = 20;
            // 
            // dgvSuggentionsColumnOrbit
            // 
            this.dgvSuggentionsColumnOrbit.HeaderText = "Orbit";
            this.dgvSuggentionsColumnOrbit.Name = "dgvSuggentionsColumnOrbit";
            this.dgvSuggentionsColumnOrbit.ReadOnly = true;
            this.dgvSuggentionsColumnOrbit.Width = 50;
            // 
            // dgvSuggentionsColumnCity
            // 
            this.dgvSuggentionsColumnCity.HeaderText = "City";
            this.dgvSuggentionsColumnCity.MinimumWidth = 20;
            this.dgvSuggentionsColumnCity.Name = "dgvSuggentionsColumnCity";
            this.dgvSuggentionsColumnCity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvSuggentionsColumnCity.Width = 20;
            // 
            // dgvSuggentionsColumnHarvest
            // 
            this.dgvSuggentionsColumnHarvest.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvSuggentionsColumnHarvest.HeaderText = "Harvest";
            this.dgvSuggentionsColumnHarvest.Name = "dgvSuggentionsColumnHarvest";
            this.dgvSuggentionsColumnHarvest.ReadOnly = true;
            // 
            // FormSystemOptimizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 557);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.MinimumSize = new System.Drawing.Size(539, 323);
            this.Name = "FormSystemOptimizer";
            this.Text = "System Optimizer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResources)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuggentions)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxCoordinates;
        private System.Windows.Forms.TextBox tbxGalaxy;
        private System.Windows.Forms.TextBox tbxSector;
        private System.Windows.Forms.Label lblCoordinates;
        private System.Windows.Forms.TextBox tbxSystem;
        private System.Windows.Forms.Label lblGalaxy;
        private System.Windows.Forms.Label lblSector;
        private System.Windows.Forms.Label lblSystem;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvResources;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvSuggentions;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ContextMenuStrip cmsRightClick;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvResourcesColumnResource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvResourcesColumnCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvResourcesColumnBest;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSuggentionsColumnPlanet;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSuggentionsColumnZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSuggentionsColumnOrbit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvSuggentionsColumnCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvSuggentionsColumnHarvest;
    }
}
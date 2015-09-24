namespace HDisplayControl
{
    partial class HDisplayControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HDisplayControl));
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSetupROILabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRegionAdd = new System.Windows.Forms.ToolStripButton();
            this.btnRegionDiff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDeleteSelectedRegion = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.viewPort = new HalconDotNet.HWindowControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Enabled = false;
            this.vScrollBar1.Location = new System.Drawing.Point(320, 34);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(20, 233);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Enabled = false;
            this.hScrollBar1.Location = new System.Drawing.Point(1, 271);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(316, 20);
            this.hScrollBar1.TabIndex = 1;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSetupROILabel,
            this.toolStripSeparator1,
            this.btnRegionAdd,
            this.btnRegionDiff,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripSeparator2,
            this.toolStripDeleteSelectedRegion,
            this.toolStripSeparator3});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(340, 23);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSetupROILabel
            // 
            this.toolStripSetupROILabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripSetupROILabel.Name = "toolStripSetupROILabel";
            this.toolStripSetupROILabel.Size = new System.Drawing.Size(67, 15);
            this.toolStripSetupROILabel.Text = "Setup ROI:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // btnRegionAdd
            // 
            this.btnRegionAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRegionAdd.Image = global::HDisplayControl.Properties.Resources.roi_op_union;
            this.btnRegionAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegionAdd.Name = "btnRegionAdd";
            this.btnRegionAdd.Size = new System.Drawing.Size(23, 20);
            this.btnRegionAdd.Text = "regionAdd";
            this.btnRegionAdd.ToolTipText = "The drawn region will be added to the region of interest";
            this.btnRegionAdd.Click += new System.EventHandler(this.btnRegionAdd_Click);
            // 
            // btnRegionDiff
            // 
            this.btnRegionDiff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRegionDiff.Image = global::HDisplayControl.Properties.Resources.roi_op_difference;
            this.btnRegionDiff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegionDiff.Name = "btnRegionDiff";
            this.btnRegionDiff.Size = new System.Drawing.Size(23, 20);
            this.btnRegionDiff.Text = "regionDifference";
            this.btnRegionDiff.ToolTipText = "The drawn region will be excluded from region of interest";
            this.btnRegionDiff.Click += new System.EventHandler(this.btnRegionDiff_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::HDisplayControl.Properties.Resources.gen_rect1;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Draw axis-aligned rectangle";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::HDisplayControl.Properties.Resources.gen_rect2;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Draw rotated rectangle";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::HDisplayControl.Properties.Resources.gen_line;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.ToolTipText = "Draw line";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::HDisplayControl.Properties.Resources.gen_circle;
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton4.Text = "toolStripButton4";
            this.toolStripButton4.ToolTipText = "Draw circle";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::HDisplayControl.Properties.Resources.gen_circular_arc;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 20);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.ToolTipText = "Draw circular arc";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripDeleteSelectedRegion
            // 
            this.toolStripDeleteSelectedRegion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteSelectedRegion.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDeleteSelectedRegion.Image")));
            this.toolStripDeleteSelectedRegion.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteSelectedRegion.Name = "toolStripDeleteSelectedRegion";
            this.toolStripDeleteSelectedRegion.Size = new System.Drawing.Size(23, 20);
            this.toolStripDeleteSelectedRegion.Text = "Delete selected region";
            this.toolStripDeleteSelectedRegion.ToolTipText = "Delete the drawn activated region.";
            this.toolStripDeleteSelectedRegion.Click += new System.EventHandler(this.toolStripDeleteSelectedRegion_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // viewPort
            // 
            this.viewPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(1, 31);
            this.viewPort.MinimumSize = new System.Drawing.Size(320, 240);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(320, 240);
            this.viewPort.TabIndex = 5;
            this.viewPort.WindowSize = new System.Drawing.Size(320, 240);
            this.viewPort.HInitWindow += new HalconDotNet.HInitWindowEventHandler(this.viewPort_HInitWindow);
            this.viewPort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.viewPort_KeyDown);
            // 
            // HDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.viewPort);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.MinimumSize = new System.Drawing.Size(344, 293);
            this.Name = "HDisplayControl";
            this.Size = new System.Drawing.Size(340, 289);
            this.Load += new System.EventHandler(this.HDisplayControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HDisplayControl_Paint);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripDeleteSelectedRegion;
        private System.Windows.Forms.ToolStripLabel toolStripSetupROILabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private HalconDotNet.HWindowControl viewPort;
        private System.Windows.Forms.ToolStripButton btnRegionAdd;
        private System.Windows.Forms.ToolStripButton btnRegionDiff;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;

    }
}

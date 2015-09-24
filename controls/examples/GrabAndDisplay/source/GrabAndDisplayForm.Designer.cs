namespace GrabAndDisplay
{
    partial class Form1
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
         this.btnConnectDisconnect = new System.Windows.Forms.Button();
         this.btnImageAcquisition = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.btnSetIAParameter = new System.Windows.Forms.Button();
         this.button4 = new System.Windows.Forms.Button();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.rBFullImageSize = new System.Windows.Forms.RadioButton();
         this.rBFitToWindow = new System.Windows.Forms.RadioButton();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
         this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
         this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.chBUpdateROI = new System.Windows.Forms.CheckBox();
         this.hImageAcquisition1 = new HImageAcquisition.HImageAcquisition(this.components);
         this.hDisplayControl1 = new HDisplayControl.HDisplayControl();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.statusStrip1.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.SuspendLayout();
         // 
         // btnConnectDisconnect
         // 
         this.btnConnectDisconnect.Location = new System.Drawing.Point(17, 43);
         this.btnConnectDisconnect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnConnectDisconnect.Name = "btnConnectDisconnect";
         this.btnConnectDisconnect.Size = new System.Drawing.Size(188, 28);
         this.btnConnectDisconnect.TabIndex = 1;
         this.btnConnectDisconnect.Text = "Connect to device";
         this.btnConnectDisconnect.UseVisualStyleBackColor = true;
         this.btnConnectDisconnect.Click += new System.EventHandler(this.button1_Click);
         // 
         // btnImageAcquisition
         // 
         this.btnImageAcquisition.Location = new System.Drawing.Point(17, 159);
         this.btnImageAcquisition.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnImageAcquisition.Name = "btnImageAcquisition";
         this.btnImageAcquisition.Size = new System.Drawing.Size(188, 28);
         this.btnImageAcquisition.TabIndex = 2;
         this.btnImageAcquisition.Text = "Start Image Acquisition";
         this.btnImageAcquisition.UseVisualStyleBackColor = true;
         this.btnImageAcquisition.Click += new System.EventHandler(this.button2_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.groupBox1.Controls.Add(this.btnSetIAParameter);
         this.groupBox1.Controls.Add(this.button4);
         this.groupBox1.Controls.Add(this.btnImageAcquisition);
         this.groupBox1.Controls.Add(this.btnConnectDisconnect);
         this.groupBox1.Location = new System.Drawing.Point(827, 36);
         this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox1.MinimumSize = new System.Drawing.Size(241, 304);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox1.Size = new System.Drawing.Size(241, 304);
         this.groupBox1.TabIndex = 6;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Image Acquisition";
         // 
         // btnSetIAParameter
         // 
         this.btnSetIAParameter.Location = new System.Drawing.Point(19, 209);
         this.btnSetIAParameter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.btnSetIAParameter.Name = "btnSetIAParameter";
         this.btnSetIAParameter.Size = new System.Drawing.Size(187, 28);
         this.btnSetIAParameter.TabIndex = 5;
         this.btnSetIAParameter.Text = "Set IA Parameters";
         this.btnSetIAParameter.UseVisualStyleBackColor = true;
         this.btnSetIAParameter.Click += new System.EventHandler(this.btnSetIAParameter_Click);
         // 
         // button4
         // 
         this.button4.Location = new System.Drawing.Point(19, 103);
         this.button4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(188, 28);
         this.button4.TabIndex = 4;
         this.button4.Text = "Snap Image";
         this.button4.UseVisualStyleBackColor = true;
         this.button4.Click += new System.EventHandler(this.button4_Click);
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.rBFullImageSize);
         this.groupBox2.Controls.Add(this.rBFitToWindow);
         this.groupBox2.Location = new System.Drawing.Point(827, 359);
         this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox2.Size = new System.Drawing.Size(237, 132);
         this.groupBox2.TabIndex = 8;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Display Image Settings";
         // 
         // rBFullImageSize
         // 
         this.rBFullImageSize.AutoSize = true;
         this.rBFullImageSize.Location = new System.Drawing.Point(17, 85);
         this.rBFullImageSize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.rBFullImageSize.Name = "rBFullImageSize";
         this.rBFullImageSize.Size = new System.Drawing.Size(112, 21);
         this.rBFullImageSize.TabIndex = 1;
         this.rBFullImageSize.TabStop = true;
         this.rBFullImageSize.Text = "fullImageSize";
         this.rBFullImageSize.UseVisualStyleBackColor = true;
         this.rBFullImageSize.CheckedChanged += new System.EventHandler(this.rBFullImageSize_CheckedChanged);
         // 
         // rBFitToWindow
         // 
         this.rBFitToWindow.AutoSize = true;
         this.rBFitToWindow.Location = new System.Drawing.Point(17, 36);
         this.rBFitToWindow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.rBFitToWindow.Name = "rBFitToWindow";
         this.rBFitToWindow.Size = new System.Drawing.Size(106, 21);
         this.rBFitToWindow.TabIndex = 0;
         this.rBFitToWindow.TabStop = true;
         this.rBFitToWindow.Text = "fitToWindow";
         this.rBFitToWindow.UseVisualStyleBackColor = true;
         this.rBFitToWindow.CheckedChanged += new System.EventHandler(this.rBFitToWindow_CheckedChanged);
         // 
         // statusStrip1
         // 
         this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
         this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
         this.statusStrip1.Location = new System.Drawing.Point(0, 593);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
         this.statusStrip1.Size = new System.Drawing.Size(1111, 29);
         this.statusStrip1.TabIndex = 9;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // toolStripStatusLabel1
         // 
         this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
         this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
         this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
         this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
         this.toolStripStatusLabel1.Size = new System.Drawing.Size(31, 24);
         this.toolStripStatusLabel1.Text = "......";
         this.toolStripStatusLabel1.ToolTipText = "Number of images acquired since starting last live acquisition";
         // 
         // toolStripStatusLabel2
         // 
         this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
         this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
         this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
         this.toolStripStatusLabel2.Size = new System.Drawing.Size(22, 24);
         this.toolStripStatusLabel2.Text = "...";
         this.toolStripStatusLabel2.ToolTipText = "Time used for last image acquisition";
         // 
         // toolStripStatusLabel3
         // 
         this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
         this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 24);
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.chBUpdateROI);
         this.groupBox3.Location = new System.Drawing.Point(833, 498);
         this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.groupBox3.Size = new System.Drawing.Size(231, 58);
         this.groupBox3.TabIndex = 13;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "ROI Settings";
         // 
         // chBUpdateROI
         // 
         this.chBUpdateROI.AutoSize = true;
         this.chBUpdateROI.Location = new System.Drawing.Point(12, 30);
         this.chBUpdateROI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.chBUpdateROI.Name = "chBUpdateROI";
         this.chBUpdateROI.Size = new System.Drawing.Size(104, 21);
         this.chBUpdateROI.TabIndex = 0;
         this.chBUpdateROI.Text = "Update ROI";
         this.chBUpdateROI.UseVisualStyleBackColor = true;
         this.chBUpdateROI.CheckedChanged += new System.EventHandler(this.chBUpdateROI_CheckedChanged);
         // 
         // hImageAcquisition1
         // 
         this.hImageAcquisition1.CameraType = "coins";
         this.hImageAcquisition1.HorizontalResolution = 1;
         this.hImageAcquisition1.LineIn = -1;
         this.hImageAcquisition1.Port = -1;
         this.hImageAcquisition1.OnGrabbedImage += new HImageAcquisition.OnGrabbedImageHandler(this.hImageAcquisition1_OnGrabbedImage);
         // 
         // hDisplayControl1
         // 
         this.hDisplayControl1.AutoScroll = true;
         this.hDisplayControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.hDisplayControl1.BackColor = System.Drawing.SystemColors.Control;
         this.hDisplayControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.hDisplayControl1.ImageViewState = HDisplayControl.ImageViewStates.fitToWindow;
         this.hDisplayControl1.Location = new System.Drawing.Point(0, 0);
         this.hDisplayControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.hDisplayControl1.MinimumSize = new System.Drawing.Size(457, 360);
         this.hDisplayControl1.Name = "hDisplayControl1";
         this.hDisplayControl1.ShowROI = true;
         this.hDisplayControl1.Size = new System.Drawing.Size(807, 589);
         this.hDisplayControl1.TabIndex = 14;
         this.hDisplayControl1.WindowSize = new System.Drawing.Size(777, 524);
         this.hDisplayControl1.ZoomCenter = new System.Drawing.Point(213, 147);
         this.hDisplayControl1.ZoomOnMouseWheel = false;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1111, 622);
         this.Controls.Add(this.hDisplayControl1);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.Name = "Form1";
         this.Text = "GrabAndDisplay";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
         this.Load += new System.EventHandler(this.Form1_Load);
         this.Resize += new System.EventHandler(this.Form1_Resize);
         this.groupBox1.ResumeLayout(false);
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.statusStrip1.ResumeLayout(false);
         this.statusStrip1.PerformLayout();
         this.groupBox3.ResumeLayout(false);
         this.groupBox3.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnectDisconnect;
        private System.Windows.Forms.Button btnImageAcquisition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rBFitToWindow;
        private System.Windows.Forms.RadioButton rBFullImageSize;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private HImageAcquisition.HImageAcquisition hImageAcquisition1;
        private System.Windows.Forms.Button btnSetIAParameter;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chBUpdateROI;
        private HDisplayControl.HDisplayControl hDisplayControl1;
    }
}


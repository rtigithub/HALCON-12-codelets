using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ViewROI;
using HalconDotNet;


namespace SmartWindow2
{	

    /// <summary>
    /// This project demonstrates how to set up the window controller HWndCtrl
    /// to perform the move and zoom functions using the mouse device. 
    /// All you have to do is to change the view state of the 
    /// window controller to one of the constants
    /// <c>MODE_VIEW_MOVE</c> or <c>MODE_VIEW_ZOOM</c>. 
    /// To disable this function again, change the state back to 
    /// <c>MODE_VIEW_NONE</c>. 
    /// 
    /// As a fourth view state, you can use
    /// <c>MODE_VIEW_ZOOMWINDOW</c>. This mode creates a small zoom window on
    /// top of your application window when you click
    /// the left mouse button - this function isn't used in this
    /// project, though.
    /// </summary>
    public class SmartWindowForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton zoomButton;
		private System.Windows.Forms.RadioButton moveButton;
	    private System.Windows.Forms.RadioButton noneButton;
        private System.Windows.Forms.Button resetButton;
		private System.ComponentModel.Container components = null;
		
		private HWindowControl viewPort;
       	private HWndCtrl	   hWndControl;
		

		public SmartWindowForm()
		{
			InitializeComponent();
		}

		/**************************************************************************/
		/**************************************************************************/
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.viewPort = new HalconDotNet.HWindowControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.noneButton = new System.Windows.Forms.RadioButton();
            this.moveButton = new System.Windows.Forms.RadioButton();
            this.zoomButton = new System.Windows.Forms.RadioButton();
            this.resetButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(48, 48);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(600, 448);
            this.viewPort.TabIndex = 0;
            this.viewPort.WindowSize = new System.Drawing.Size(600, 448);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.noneButton,
                                                                                    this.moveButton,
                                                                                    this.zoomButton});
            this.groupBox1.Location = new System.Drawing.Point(688, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(144, 136);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " View Interaction ";
            // 
            // noneButton
            // 
            this.noneButton.Checked = true;
            this.noneButton.Location = new System.Drawing.Point(32, 96);
            this.noneButton.Name = "noneButton";
            this.noneButton.TabIndex = 2;
            this.noneButton.TabStop = true;
            this.noneButton.Text = "none";
            this.noneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noneButton.CheckedChanged += new System.EventHandler(this.noneButton_CheckedChanged);
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(32, 64);
            this.moveButton.Name = "moveButton";
            this.moveButton.TabIndex = 1;
            this.moveButton.Text = "move";
            this.moveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.moveButton.CheckedChanged += new System.EventHandler(this.moveButton_CheckedChanged);
            // 
            // zoomButton
            // 
            this.zoomButton.Location = new System.Drawing.Point(32, 32);
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.TabIndex = 0;
            this.zoomButton.Text = "zoom";
            this.zoomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zoomButton.CheckedChanged += new System.EventHandler(this.zoomButton_CheckedChanged);
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(688, 56);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(144, 48);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset View";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // SmartWindowForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(880, 557);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.resetButton,
                                                                          this.groupBox1,
                                                                          this.viewPort});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SmartWindowForm";
            this.Text = "SmartWindow";
            this.Load += new System.EventHandler(this.SmartWindowForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new SmartWindowForm());
		}

		/*************************************************************************
         * Use one of the following view modes to implement view functions
         * in respond to mouse events
        /*************************************************************************/
		private void noneButton_CheckedChanged(object sender, System.EventArgs e)
        {
            hWndControl.setViewState(HWndCtrl.MODE_VIEW_NONE);
		}

		private void moveButton_CheckedChanged(object sender, System.EventArgs e)
		{
			hWndControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);		
		}

		private void zoomButton_CheckedChanged(object sender, System.EventArgs e)
		{
			hWndControl.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
		}


        /* reset the view to the initial display dimensions */
		private void resetButton_Click(object sender, System.EventArgs e)
		{
			hWndControl.resetWindow();
			hWndControl.repaint();
		}

        /**************************************************************************/
        /* Setup the GUI for the SmartWindow application
         **************************************************************************/
		private void SmartWindowForm_Load(object sender, System.EventArgs e)
		{
			hWndControl = new HWndCtrl(viewPort);            
            String fileName = "patras";
            HImage image;

            try
            {
                image   = new HImage(fileName);
            }
            catch(HOperatorException)
            {
                MessageBox.Show("Problem occured while reading file!", 
                                "SmartWindow2",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                return;
            }
            
            hWndControl.addIconicVar(image);
            hWndControl.repaint();
		}
		
	}//end of class
}//end of namespace

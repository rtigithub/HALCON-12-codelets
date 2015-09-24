using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ViewROI;
using HalconDotNet;


namespace SmartWindow1
{

    /// <summary>
    /// This project demonstrates how to use the routines of the HWndCtrl class
    /// to control the functions like moving and zooming. There are two
    /// ways of accessing this feature. One is to use GUI Components like
    /// sliders and numeric devices to receive events for the motion and 
    /// scaling factor. 
    /// Before forwarding the GUI handle events to the HWndCtrl,
    /// you have to specify the range of values that is used for the GUI 
    /// components by providing an array containing
    /// the minimum and the maximum value of the GUI component, as well as
    /// its initial value. As an alternative, you can let the HWndCtrl use the
    /// mouse device as a trigger. The example SmartWindow2Form shows how to
    /// do this.
    /// </summary>
    public class SmartWindow1Form : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TrackBar XTrackBar;
		private System.Windows.Forms.Label label2;

		private System.ComponentModel.Container components = null;
		private HWndCtrl		hWndControl;
		private System.Windows.Forms.TrackBar YTrackBar;
		private System.Windows.Forms.NumericUpDown ZoomUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private HalconDotNet.HWindowControl viewPort;
		private System.Windows.Forms.Button resetButton;
        

		public SmartWindow1Form()
		{
			InitializeComponent();
		}

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
            this.XTrackBar = new System.Windows.Forms.TrackBar();
            this.YTrackBar = new System.Windows.Forms.TrackBar();
            this.ZoomUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.resetButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.viewPort = new HalconDotNet.HWindowControl();
            ((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // XTrackBar
            // 
            this.XTrackBar.Location = new System.Drawing.Point(48, 488);
            this.XTrackBar.Maximum = 100;
            this.XTrackBar.Name = "XTrackBar";
            this.XTrackBar.Size = new System.Drawing.Size(616, 42);
            this.XTrackBar.TabIndex = 1;
            this.XTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.XTrackBar.Value = 50;
            this.XTrackBar.Scroll += new System.EventHandler(this.XTrackBar_Scroll);
            // 
            // YTrackBar
            // 
            this.YTrackBar.Location = new System.Drawing.Point(680, 16);
            this.YTrackBar.Maximum = 100;
            this.YTrackBar.Name = "YTrackBar";
            this.YTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.YTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.YTrackBar.Size = new System.Drawing.Size(42, 464);
            this.YTrackBar.TabIndex = 2;
            this.YTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.YTrackBar.Value = 50;
            this.YTrackBar.Scroll += new System.EventHandler(this.YTrackBar_Scroll);
            // 
            // ZoomUpDown
            // 
            this.ZoomUpDown.Increment = new System.Decimal(new int[] {
                                                                         10,
                                                                         0,
                                                                         0,
                                                                         0});
            this.ZoomUpDown.Location = new System.Drawing.Point(744, 512);
            this.ZoomUpDown.Maximum = new System.Decimal(new int[] {
                                                                       1600,
                                                                       0,
                                                                       0,
                                                                       0});
            this.ZoomUpDown.Minimum = new System.Decimal(new int[] {
                                                                       1,
                                                                       0,
                                                                       0,
                                                                       0});
            this.ZoomUpDown.Name = "ZoomUpDown";
            this.ZoomUpDown.Size = new System.Drawing.Size(56, 20);
            this.ZoomUpDown.TabIndex = 4;
            this.ZoomUpDown.Value = new System.Decimal(new int[] {
                                                                     100,
                                                                     0,
                                                                     0,
                                                                     0});
            this.ZoomUpDown.ValueChanged += new System.EventHandler(this.ZoomUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(736, 488);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Zoomfactor";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(784, 24);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(96, 48);
            this.resetButton.TabIndex = 6;
            this.resetButton.Text = "Reset View";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(56, 536);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "X-Axis";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(728, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Y-Axis";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(808, 512);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "%";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(56, 24);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(608, 440);
            this.viewPort.TabIndex = 10;
            this.viewPort.WindowSize = new System.Drawing.Size(608, 440);
            // 
            // SmartWindow1Form
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(896, 573);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.viewPort,
                                                                          this.label4,
                                                                          this.label1,
                                                                          this.resetButton,
                                                                          this.label2,
                                                                          this.ZoomUpDown,
                                                                          this.YTrackBar,
                                                                          this.XTrackBar,
                                                                          this.label3});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SmartWindow1Form";
            this.Text = "SmartWindow";
            this.Load += new System.EventHandler(this.SmartWindow1Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.XTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.YTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomUpDown)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new SmartWindow1Form());
		}


		/**************************************************************************/
		/* Setup the GUI for the SmartWindow application in terms of
         * registering the Window Control class and setting up all values
         * and flags to use the GUI based viewfunctions (zoom and move)
         **************************************************************************/
		private void SmartWindow1Form_Load(object sender, System.EventArgs e)
		{
			hWndControl = new HWndCtrl(viewPort);
			Init();
		}

		
		private void Init()
		{		
			String fileName = "patras";
            HImage image;

            try
            {
                image   = new HImage(fileName);
            }
            catch(HOperatorException)
            {
                MessageBox.Show("Problem occured while reading file!", 
                                "SmartWindow1",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                return;
            }
            
            hWndControl.addIconicVar(image);
			hWndControl.repaint();
			
			hWndControl.setGUICompRangeX( new int[]{ XTrackBar.Minimum, 
					   							     XTrackBar.Maximum}, 
													 XTrackBar.Value );
			hWndControl.setGUICompRangeY( new int[]{ YTrackBar.Minimum,
					   							     YTrackBar.Maximum}, 
													 YTrackBar.Maximum-YTrackBar.Value); 
		}

		/**************************************************************************/
		/* Adjust the view using GUI components for moving along the x and y 
         * direction and for changing the zoom of the image displayed
         * *************************************************************************/
		private void XTrackBar_Scroll(object sender, System.EventArgs e)
		{
			hWndControl.moveXByGUIHandle(XTrackBar.Value);
		}

		private void YTrackBar_Scroll(object sender, System.EventArgs e)
		{
			hWndControl.moveYByGUIHandle(YTrackBar.Maximum-YTrackBar.Value);
		}

		private void ZoomUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			hWndControl.zoomByGUIHandle((int)ZoomUpDown.Value);
		}


		/**************************************************************************/
		/* Reset the view to its initial setting
         * *************************************************************************/
		private void resetButton_Click(object sender, System.EventArgs e)
		{
			XTrackBar.Value = 50;
			YTrackBar.Value = 50;
			ZoomUpDown.Value = 100;

			hWndControl.resetGUIInitValues(XTrackBar.Value, 
										   (YTrackBar.Maximum-YTrackBar.Value));
			hWndControl.resetWindow();
			hWndControl.repaint();
		}
       
	}//end of class
}//end of namespace

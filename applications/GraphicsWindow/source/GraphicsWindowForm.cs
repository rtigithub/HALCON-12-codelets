using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ViewROI;
using HalconDotNet;


namespace GraphicsWindow
{	

    /// <summary>
    /// This project implements an extended graphics window.
    /// To realize the various view functions similar to the ones provided 
    /// with HDevelop's graphics window, we use the  class HWndCtrl from the
    /// project ViewROI.
    ///
    /// For instance, by using the method setViewState() you tell 
    /// the HWndCtrl to perform changes to the visualization, like
    /// image translation or image scaling, in response to mouse events.
    /// Furthermore, you can use calls like zoomImage and scaleWindow to 
    /// manipulate the visible part of the image or the size of the HalconWindow
    /// HWindow, respectively. 
    /// 
    /// To get more ideas on how to use the HWndCtrl class, please also refer 
    /// to the example projects SmartWindow1 and SmartWindow2. 
    /// </summary>
    public class GraphicsWindowForm : System.Windows.Forms.Form
	{        
        private System.ComponentModel.Container components = null;
		
		private HalconDotNet.HWindowControl viewPort;
		private System.Windows.Forms.Panel wndFramePanel;
		private System.Windows.Forms.Panel ToolPanel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox zoomWndComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox zoomImgComboBox;
		private System.Windows.Forms.GroupBox viewFunctionGroup;
		private System.Windows.Forms.RadioButton zoomClipradioButton;
		private System.Windows.Forms.RadioButton zoomRadioButton;
		private System.Windows.Forms.RadioButton moveRadioButton;
        private System.Windows.Forms.RadioButton noneRadioButton;
		private System.Windows.Forms.Button ResetButton;

        /// <summary> 
        /// Window object, which takes care of all view functions 
        /// </summary>
		private HWndCtrl viewControl;
               
        /// <summary> Height of current screen </summary>
		private int		ScreenHeight;
        /// <summary> Width of current screen </summary>
        private int		ScreenWidth;
        
        /// <summary>
        /// Limiting parameter for the window height, to avoid that the window
        /// frame size becomes larger than the available screen resolution.
        /// </summary>
        private int		ScreenCastH;

        /// <summary>
        /// Limiting parameter for the window width to avoid that the window
        /// frame size becomes larger  than the available screen resolution.
        /// </summary>
        private int		ScreenCastW;

        private bool	locked;
        

        
        /// <summary> Constructor </summary>
		public GraphicsWindowForm()
		{
			InitializeComponent();
		}

        /*******************************************************************
        ********************************************************************/
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
		
		private void InitializeComponent()
		{
            this.wndFramePanel = new System.Windows.Forms.Panel();
            this.viewPort = new HalconDotNet.HWindowControl();
            this.ToolPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.zoomWndComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.zoomImgComboBox = new System.Windows.Forms.ComboBox();
            this.viewFunctionGroup = new System.Windows.Forms.GroupBox();
            this.zoomClipradioButton = new System.Windows.Forms.RadioButton();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.zoomRadioButton = new System.Windows.Forms.RadioButton();
            this.moveRadioButton = new System.Windows.Forms.RadioButton();
            this.ResetButton = new System.Windows.Forms.Button();
            this.wndFramePanel.SuspendLayout();
            this.ToolPanel.SuspendLayout();
            this.viewFunctionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // wndFramePanel
            // 
            this.wndFramePanel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.wndFramePanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                        this.viewPort});
            this.wndFramePanel.Location = new System.Drawing.Point(5, 80);
            this.wndFramePanel.Name = "wndFramePanel";
            this.wndFramePanel.Size = new System.Drawing.Size(720, 520);
            this.wndFramePanel.TabIndex = 15;
            this.wndFramePanel.Resize += new System.EventHandler(this.wndFramePanel_Resize);
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Brown;
            this.viewPort.BorderColor = System.Drawing.Color.Brown;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(5, 5);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(710, 510);
            this.viewPort.TabIndex = 15;
            this.viewPort.WindowSize = new System.Drawing.Size(710, 510);
            this.viewPort.Resize += new System.EventHandler(this.viewPort_Resize);
            // 
            // ToolPanel
            // 
            this.ToolPanel.AutoScroll = true;
            this.ToolPanel.AutoScrollMinSize = new System.Drawing.Size(600, 64);
            this.ToolPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.label2,
                                                                                    this.zoomWndComboBox,
                                                                                    this.label1,
                                                                                    this.zoomImgComboBox,
                                                                                    this.viewFunctionGroup,
                                                                                    this.ResetButton});
            this.ToolPanel.Location = new System.Drawing.Point(8, 8);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.Size = new System.Drawing.Size(624, 72);
            this.ToolPanel.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(496, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Scale Window";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoomWndComboBox
            // 
            this.zoomWndComboBox.ItemHeight = 13;
            this.zoomWndComboBox.Items.AddRange(new object[] {
                                                                 "Window",
                                                                 "400%",
                                                                 "200%",
                                                                 "100%",
                                                                 "  50%",
                                                                 "  25%"});
            this.zoomWndComboBox.Location = new System.Drawing.Point(504, 32);
            this.zoomWndComboBox.Name = "zoomWndComboBox";
            this.zoomWndComboBox.Size = new System.Drawing.Size(72, 21);
            this.zoomWndComboBox.TabIndex = 16;
            this.zoomWndComboBox.Text = "Window";
            this.zoomWndComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomWndComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(416, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "Scale Image";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoomImgComboBox
            // 
            this.zoomImgComboBox.ItemHeight = 13;
            this.zoomImgComboBox.Items.AddRange(new object[] {
                                                                 "Fit",
                                                                 "400%",
                                                                 "200%",
                                                                 "100%",
                                                                 "  50%",
                                                                 "  25%"});
            this.zoomImgComboBox.Location = new System.Drawing.Point(424, 32);
            this.zoomImgComboBox.Name = "zoomImgComboBox";
            this.zoomImgComboBox.Size = new System.Drawing.Size(72, 21);
            this.zoomImgComboBox.TabIndex = 14;
            this.zoomImgComboBox.Text = "Fit";
            this.zoomImgComboBox.SelectedIndexChanged += new System.EventHandler(this.zoomImgComboBox_SelectedIndexChanged);
            // 
            // viewFunctionGroup
            // 
            this.viewFunctionGroup.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                            this.zoomClipradioButton,
                                                                                            this.noneRadioButton,
                                                                                            this.zoomRadioButton,
                                                                                            this.moveRadioButton});
            this.viewFunctionGroup.Location = new System.Drawing.Point(120, 8);
            this.viewFunctionGroup.Name = "viewFunctionGroup";
            this.viewFunctionGroup.Size = new System.Drawing.Size(288, 48);
            this.viewFunctionGroup.TabIndex = 13;
            this.viewFunctionGroup.TabStop = false;
            this.viewFunctionGroup.Text = "View Interaction";
            // 
            // zoomClipradioButton
            // 
            this.zoomClipradioButton.Location = new System.Drawing.Point(216, 16);
            this.zoomClipradioButton.Name = "zoomClipradioButton";
            this.zoomClipradioButton.Size = new System.Drawing.Size(64, 24);
            this.zoomClipradioButton.TabIndex = 3;
            this.zoomClipradioButton.Text = "magnify";
            this.zoomClipradioButton.CheckedChanged += new System.EventHandler(this.zoomClipradioButton_CheckedChanged);
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.Checked = true;
            this.noneRadioButton.Location = new System.Drawing.Point(16, 16);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(64, 24);
            this.noneRadioButton.TabIndex = 2;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "none";
            this.noneRadioButton.CheckedChanged += new System.EventHandler(this.noneRadioButton_CheckedChanged);
            // 
            // zoomRadioButton
            // 
            this.zoomRadioButton.Location = new System.Drawing.Point(152, 16);
            this.zoomRadioButton.Name = "zoomRadioButton";
            this.zoomRadioButton.Size = new System.Drawing.Size(56, 24);
            this.zoomRadioButton.TabIndex = 1;
            this.zoomRadioButton.Text = "zoom";
            this.zoomRadioButton.CheckedChanged += new System.EventHandler(this.zoomRadioButton_CheckedChanged);
            // 
            // moveRadioButton
            // 
            this.moveRadioButton.Location = new System.Drawing.Point(88, 16);
            this.moveRadioButton.Name = "moveRadioButton";
            this.moveRadioButton.Size = new System.Drawing.Size(64, 24);
            this.moveRadioButton.TabIndex = 0;
            this.moveRadioButton.Text = "move";
            this.moveRadioButton.CheckedChanged += new System.EventHandler(this.moveRadioButton_CheckedChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(16, 16);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(80, 40);
            this.ResetButton.TabIndex = 12;
            this.ResetButton.Text = "Reset Window";
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // GraphicsWindowForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 605);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.ToolPanel,
                                                                          this.wndFramePanel});
            this.Name = "GraphicsWindowForm";
            this.Text = "GraphicsWindow";
            this.Resize += new System.EventHandler(this.GraphicsWindowForm_Resize);
            this.Load += new System.EventHandler(this.GraphicsWindowForm_Load);
            this.wndFramePanel.ResumeLayout(false);
            this.ToolPanel.ResumeLayout(false);
            this.viewFunctionGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new GraphicsWindowForm());
		}


        /*****************************************************************
         * During the load process the HWndCtrl instance is initialized -
         * besides some other variables, needed to manipulate the 
         * window panel.
         *****************************************************************/
    	private void GraphicsWindowForm_Load(object sender, System.EventArgs e)
		{
			locked = false;
			viewControl = new HWndCtrl(viewPort);
			
            // current screen resolution, height and width
			ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height; 
			ScreenWidth  = Screen.PrimaryScreen.WorkingArea.Width;
            
            ScreenCastH = (int)(ScreenHeight/4.0*3.0);
			ScreenCastW = (int)(ScreenWidth/4.0*3.0);

            Init();
		}

        
        /*******************************************************************/
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
                                "GraphicsWindow",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                return;
            }
            viewControl.addIconicVar(image);
			zoomWndComboBox.SelectedIndex = 3;
		}

        /*******************************************************************/
        private void noneRadioButton_CheckedChanged(object sender, System.EventArgs e)
        {
        	viewControl.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }
		
        /*******************************************************************/
        private void moveRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			viewControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
		}
		
        /*******************************************************************/
        private void zoomRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			viewControl.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
		}

        /*******************************************************************/
        private void zoomClipradioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            viewControl.setViewState(HWndCtrl.MODE_VIEW_ZOOMWINDOW);
        }

        /*******************************************************************/
        private void ResetButton_Click(object sender, System.EventArgs e)
		{
			locked = true;
			zoomImgComboBox.SelectedIndex = 3;
			viewControl.resetWindow();
			locked = false;
			
			if(zoomWndComboBox.SelectedIndex != 3)
				zoomWndComboBox.SelectedIndex = 3;
			else
				viewControl.repaint();
		}
		
        /*******************************************************************/
        private void zoomImgComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!locked)
			{
				resizeImage((string)zoomImgComboBox.SelectedItem);
			}
		}
		
        /*******************************************************************/
        private void zoomWndComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!locked)
			{
				resizeWindow((string)zoomWndComboBox.SelectedItem);
			}
		}
             

        /// <summary>
        /// Adjusts the graphical window to the new size of the window
        /// form. If the size exceeds the limitation parameters of the
        /// screen, then resizes the window form to the value of the 
        /// limitation parameters of the form.
        /// </summary>
        private void GraphicsWindowForm_Resize(object sender, System.EventArgs e)
		{
			if(!locked)
			{
				int valH = this.Height - this.wndFramePanel.Location.Y - 30;
				int valW = this.Width - this.wndFramePanel.Location.X - 10;

				if(valH < ScreenHeight)
					wndFramePanel.Height = this.Height - this.wndFramePanel.Location.Y - 30;
				else
					wndFramePanel.Height = ScreenCastH;

				if(valW < ScreenWidth)
					wndFramePanel.Width  = this.Width - this.wndFramePanel.Location.X - 10;
				else
					wndFramePanel.Width = ScreenCastW;

				this.ToolPanel.Width = this.Width;
			}
        }
		
        
        /// <summary>
        /// The graphical (HALCON) window is embedded in a frame panel 
        /// instance to obtain the correct values of the resizing step.  
        /// If the size of the frame panel exceeds the allowed screen size
        /// in height or width, an autoscrollbar is created additionally.
        /// The graphical window is adjusted accordingly, if it 
        /// has not been already by the calling method.
        /// </summary>
        private void wndFramePanel_Resize(object sender, System.EventArgs e)
		{
            HOperatorSet.SetSystem("flush_graphic", "false");
            if (!locked)
			{   // graphical HALCON window gets adjusted to the frame panel
				viewPort.Height = (int)wndFramePanel.Height-10; 
				viewPort.Width  = (int)wndFramePanel.Width-10;	
				viewControl.repaint();
			}
			else
			{   // ... otherwise the Height and Width of the viewPort have been 
                // adjusted already by using the HWndCtrl method scaleWindow()
				int valH, valW;
				bool resize = false;

				valH = wndFramePanel.Location.Y + wndFramePanel.Height + 30; 
				valW = wndFramePanel.Location.X + wndFramePanel.Width  + 10;	

				if(valH < ScreenHeight)
				{
					this.Height	= valH;
				}
				else
					resize = true;
				
				if(valW < ScreenWidth)
					this.Width	= valW;
				else 
					resize = true;


				if(resize)
				{
					System.Drawing.Size scrollSize = this.AutoScrollMinSize;
					scrollSize.Height = valH;
					scrollSize.Width  = valW;
					this.AutoScrollMinSize = scrollSize;
					this.AutoScroll = true;
					this.Height = ScreenCastH;
					this.Width  = ScreenCastW;
				}
				else
				{
					this.AutoScroll = false;
				}
				this.ToolPanel.Width = this.Width;
			}
            HOperatorSet.SetSystem("flush_graphic", "true");
            Invalidate();
        }

                
        /*********************************************************************
         * For any changes to the dimension of the HALCON window, that 
         * is performed outside of the HWndCtrl instance (by not using the
         * method scaleWindow()), the state of the internal parameter
         * ZoomWndFactor needs to be updated to the new size to assure a
         * correct scaling relation for the zoom view function
         * ******************************************************************/
        private void viewPort_Resize(object sender, System.EventArgs e)
		{
			if(!locked)
			{   
                // update of the internal parameter to the new size of 
                // the HALCON window
				viewControl.setZoomWndFactor();

				locked = true;
				zoomWndComboBox.SelectedIndex = 0;
				locked = false;
			}
        }
		       

        /// <summary> Zoom image to one the provided scaling factors. </summary>
        /// <param name="val">
        /// Scale factor for image resizing, which can be one of the 
        /// following values [400%, 200%, 100%, 50%, 25%, 'Fit']. 
        /// When choosing the parameter 'Fit', the image is fitted into
        /// the HALCON window, with the image origin in the upper left corner.
        /// </param>
        private void resizeImage(string val)
		{
			double scale = 1.0;

			switch(val)
			{
				case "400%":
					scale = 100.0/400.0; 
					break;
				case "200%":
					scale = 100.0/200.0; 
					break;
				case "100%":
					scale = 1.0; 
					break;
				case "  50%":
					scale = 100.0/50.0; 
					break;
				case "  25%":
					scale = 100.0/25.0; 
					break;
				case "Fit":
					viewControl.resetWindow();
					viewControl.repaint();
					return;
				default:
					return;
			}
			
			viewControl.zoomImage(scale);
		}
        
       
        /// <summary>
        /// Resize the window frame according to one of the provided
        /// scaling factors.
        /// </summary>
        /// <param name="val">
        /// Scale factor for window resizing, which can be one of the 
        /// following values [400%, 200%, 100%, 50%, 25%].
        /// </param>
        private void resizeWindow(string val)
		{
			double scale = 0.0;
			
			switch(val)
			{
				case "400%":
					scale =  4.0;
					break;
				case "200%":
					scale =  2.0;
					break;
				case "100%":
					scale =  1.0;
					break;
				case "  50%":
					scale =  0.5;
					break;
				case "  25%":
					scale =  0.25;
					break;
				default:
					return;
			}
			
		    viewControl.resetWindow();
			locked = true;
			viewControl.scaleWindow(scale);
			this.wndFramePanel.Width  = viewPort.Width  + 10;
			this.wndFramePanel.Height = viewPort.Height + 10;
			locked = false;

			if(zoomImgComboBox.SelectedIndex != 3)
				zoomImgComboBox.SelectedIndex = 3;
			else 
				viewControl.repaint();
		}

	}//end of class
}//end of namespace

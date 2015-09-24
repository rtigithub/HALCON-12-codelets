using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ViewROI;
using HalconDotNet;



namespace GraphicsStack
{
 
    /// <summary>
    /// This project demonstrates how to work with the graphics stack used
    /// in the window controller. 
    /// The graphics stack is a FIFO list which contains iconic objects, which
    /// can have individual graphical contexts. The graphics stack works
    /// similar to the stack used in HDevelop to display objects. When the
    /// application invokes a repaint of the window, all objects 
    /// contained on this stack are flushed to the screen. You can add one and 
    /// the same HALCON object more than once to the stack, each having
    /// different color and draw setups and being put successively
    /// into the window. You can try out this function with this 
    /// application by double clicking one iconic entry from the iconics
    /// list several times.
    /// This example application shows you how to add objects to the graphics
    /// stack and how to define/adjust the graphical context for 
    /// individual objects.
    ///	The graphical context for an HALCON object is supplied by the
    ///	class GraphicsContext. The different "modes" that can be adjusted are
    /// defined as constants of the GraphicsContext class, e.g.
    /// <c>GC_COLOR</c>, <c>GC_DRAWMODE</c> etc.
    /// You can apply most of the graphical 
    /// settings that are also available with the operators dev_set_* in
    /// HDevelop. More information about these operators can be found in the
    /// Reference Manual (HDevelop Syntax) in the chapter Develop.
    /// </summary>        
	public class GraphicsStackForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBoxViewFunction;
		private System.Windows.Forms.RadioButton MoveButton;
		private System.Windows.Forms.RadioButton ZoomButton;
        private System.Windows.Forms.RadioButton NoneButton;
		private System.Windows.Forms.ListBox IconiclistBox;
		private System.Windows.Forms.ComboBox ColorComboBox;
		private System.Windows.Forms.ComboBox LineWidthComboBox;
		private System.Windows.Forms.ComboBox DrawModeComboBox;
		private System.Windows.Forms.Button ResetButton;
		private System.Windows.Forms.Button ClearWindButton;
		private System.Windows.Forms.ListBox IconicDisplayStack;
		private System.Windows.Forms.Label label1;
		private System.ComponentModel.Container components = null;

		private HWindowControl viewPort;
		private HWndCtrl	   viewControl;
		private Hashtable	   procList;
        private HImage         image;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label StackCountLabel;
		private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
       
       

		bool locked;

		public GraphicsStackForm()
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
            this.groupBoxViewFunction = new System.Windows.Forms.GroupBox();
            this.NoneButton = new System.Windows.Forms.RadioButton();
            this.ZoomButton = new System.Windows.Forms.RadioButton();
            this.MoveButton = new System.Windows.Forms.RadioButton();
            this.IconiclistBox = new System.Windows.Forms.ListBox();
            this.ColorComboBox = new System.Windows.Forms.ComboBox();
            this.LineWidthComboBox = new System.Windows.Forms.ComboBox();
            this.DrawModeComboBox = new System.Windows.Forms.ComboBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ClearWindButton = new System.Windows.Forms.Button();
            this.IconicDisplayStack = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StackCountLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBoxViewFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(24, 80);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(616, 424);
            this.viewPort.TabIndex = 0;
            this.viewPort.WindowSize = new System.Drawing.Size(616, 424);
            // 
            // groupBoxViewFunction
            // 
            this.groupBoxViewFunction.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                               this.NoneButton,
                                                                                               this.ZoomButton,
                                                                                               this.MoveButton});
            this.groupBoxViewFunction.Location = new System.Drawing.Point(376, 16);
            this.groupBoxViewFunction.Name = "groupBoxViewFunction";
            this.groupBoxViewFunction.Size = new System.Drawing.Size(264, 48);
            this.groupBoxViewFunction.TabIndex = 1;
            this.groupBoxViewFunction.TabStop = false;
            this.groupBoxViewFunction.Text = "View Interaction";
            // 
            // NoneButton
            // 
            this.NoneButton.Checked = true;
            this.NoneButton.Location = new System.Drawing.Point(24, 16);
            this.NoneButton.Name = "NoneButton";
            this.NoneButton.Size = new System.Drawing.Size(72, 24);
            this.NoneButton.TabIndex = 2;
            this.NoneButton.TabStop = true;
            this.NoneButton.Text = "none";
            this.NoneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NoneButton.CheckedChanged += new System.EventHandler(this.NoneButton_CheckedChanged);
            // 
            // ZoomButton
            // 
            this.ZoomButton.Location = new System.Drawing.Point(176, 16);
            this.ZoomButton.Name = "ZoomButton";
            this.ZoomButton.Size = new System.Drawing.Size(72, 24);
            this.ZoomButton.TabIndex = 1;
            this.ZoomButton.Text = "zoom";
            this.ZoomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ZoomButton.CheckedChanged += new System.EventHandler(this.ZoomButton_CheckedChanged);
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(104, 16);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(64, 24);
            this.MoveButton.TabIndex = 0;
            this.MoveButton.Text = "move ";
            this.MoveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MoveButton.CheckedChanged += new System.EventHandler(this.MoveButton_CheckedChanged);
            // 
            // IconiclistBox
            // 
            this.IconiclistBox.Enabled = false;
            this.IconiclistBox.Location = new System.Drawing.Point(192, 544);
            this.IconiclistBox.Name = "IconiclistBox";
            this.IconiclistBox.Size = new System.Drawing.Size(144, 95);
            this.IconiclistBox.TabIndex = 2;
            this.IconiclistBox.DoubleClick += new System.EventHandler(this.IconiclistBox_DoubleClick);
            // 
            // ColorComboBox
            // 
            this.ColorComboBox.Items.AddRange(new object[] {
                                                               "red",
                                                               "yellow",
                                                               "green",
                                                               "blue",
                                                               "colored3",
                                                               "colored6",
                                                               "colored12"});
            this.ColorComboBox.Location = new System.Drawing.Point(86, 544);
            this.ColorComboBox.Name = "ColorComboBox";
            this.ColorComboBox.Size = new System.Drawing.Size(96, 21);
            this.ColorComboBox.TabIndex = 3;
            this.ColorComboBox.Text = "colored3";
            this.ColorComboBox.SelectedIndexChanged += new System.EventHandler(this.ColorComboBox_SelectedIndexChanged);
            // 
            // LineWidthComboBox
            // 
            this.LineWidthComboBox.Items.AddRange(new object[] {
                                                                   "1",
                                                                   "2",
                                                                   "3",
                                                                   "4",
                                                                   "5",
                                                                   "6",
                                                                   "7",
                                                                   "8",
                                                                   "9",
                                                                   "10"});
            this.LineWidthComboBox.Location = new System.Drawing.Point(86, 580);
            this.LineWidthComboBox.Name = "LineWidthComboBox";
            this.LineWidthComboBox.Size = new System.Drawing.Size(96, 21);
            this.LineWidthComboBox.TabIndex = 4;
            this.LineWidthComboBox.Text = "Linewidth";
            this.LineWidthComboBox.SelectedIndexChanged += new System.EventHandler(this.LineWidthComboBox_SelectedIndexChanged);
            // 
            // DrawModeComboBox
            // 
            this.DrawModeComboBox.Items.AddRange(new object[] {
                                                                  "fill",
                                                                  "margin"});
            this.DrawModeComboBox.Location = new System.Drawing.Point(86, 616);
            this.DrawModeComboBox.Name = "DrawModeComboBox";
            this.DrawModeComboBox.Size = new System.Drawing.Size(96, 21);
            this.DrawModeComboBox.TabIndex = 5;
            this.DrawModeComboBox.Text = "Drawmode";
            this.DrawModeComboBox.SelectedIndexChanged += new System.EventHandler(this.DrawModeComboBox_SelectedIndexChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.Location = new System.Drawing.Point(264, 24);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(88, 40);
            this.ResetButton.TabIndex = 7;
            this.ResetButton.Text = "Reset View";
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ClearWindButton
            // 
            this.ClearWindButton.Location = new System.Drawing.Point(24, 24);
            this.ClearWindButton.Name = "ClearWindButton";
            this.ClearWindButton.Size = new System.Drawing.Size(88, 40);
            this.ClearWindButton.TabIndex = 8;
            this.ClearWindButton.Text = "Clear Window";
            this.ClearWindButton.Click += new System.EventHandler(this.ClearWindButton_Click);
            // 
            // IconicDisplayStack
            // 
            this.IconicDisplayStack.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.IconicDisplayStack.ForeColor = System.Drawing.SystemColors.WindowText;
            this.IconicDisplayStack.HorizontalScrollbar = true;
            this.IconicDisplayStack.Location = new System.Drawing.Point(376, 544);
            this.IconicDisplayStack.Name = "IconicDisplayStack";
            this.IconicDisplayStack.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.IconicDisplayStack.Size = new System.Drawing.Size(264, 95);
            this.IconicDisplayStack.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label1.Location = new System.Drawing.Point(376, 520);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Displayed iconics:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 544);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 24);
            this.label2.TabIndex = 12;
            this.label2.Text = "Color:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 580);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Linewidth:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 616);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 24);
            this.label4.TabIndex = 14;
            this.label4.Text = "Drawmode:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StackCountLabel
            // 
            this.StackCountLabel.Location = new System.Drawing.Point(480, 648);
            this.StackCountLabel.Name = "StackCountLabel";
            this.StackCountLabel.Size = new System.Drawing.Size(24, 16);
            this.StackCountLabel.TabIndex = 15;
            this.StackCountLabel.Text = "0";
            this.StackCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label5.Location = new System.Drawing.Point(192, 520);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "List of iconics:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.label6.Location = new System.Drawing.Point(376, 648);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "Current stack size:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GraphicsStackForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(664, 685);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.label6,
                                                                          this.label5,
                                                                          this.StackCountLabel,
                                                                          this.label3,
                                                                          this.label2,
                                                                          this.label1,
                                                                          this.IconicDisplayStack,
                                                                          this.ClearWindButton,
                                                                          this.ResetButton,
                                                                          this.DrawModeComboBox,
                                                                          this.LineWidthComboBox,
                                                                          this.ColorComboBox,
                                                                          this.IconiclistBox,
                                                                          this.groupBoxViewFunction,
                                                                          this.viewPort,
                                                                          this.label4});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "GraphicsStackForm";
            this.Text = "Graphic Stack";
            this.Load += new System.EventHandler(this.GraphicsStackForm_Load);
            this.groupBoxViewFunction.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		[STAThread]
		static void Main() 
		{
			Application.Run(new GraphicsStackForm());
		}


		/**************************************************************************
         * Initialize the GraphicsStack application in terms of:
         * register the window controller and perform the image processing 
         * beforehand, to create a list of iconic objects to be added 
         * to the graphics stack by the user during the application.
		/**************************************************************************/
		private void GraphicsStackForm_Load(object sender, System.EventArgs e)
		{
			viewControl = new HWndCtrl(viewPort);
			viewControl.NotifyIconObserver = new IconicDelegate(UpdateHWnd);
			Init();
		}

		private void Init()
		{
           
            String fileName = "particle";
            
            try
            {
                image   = new HImage(fileName);
            }
            catch(HOperatorException)
            {
                MessageBox.Show("Problem occured while reading file!", 
                                "GraphicsStack",
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Information);
                return;
            }


           	locked   = false;
			procList = new Hashtable(10);
			
            IconiclistBox.Enabled = true;
            performIPTask();
            IconiclistBox.SelectedIndex = 1;
            viewControl.repaint();
		}


		/*************************************************************************
         * Eventhandles to reset the window to the initial view dimensions, as well  
         * as clearing the graphics stack from all iconic objects added - display 
         * will be black then. 
		/**************************************************************************/
		private void ResetButton_Click(object sender, System.EventArgs e)
		{
			viewControl.resetWindow();
			viewControl.repaint();
		}
		
		
		private void ClearWindButton_Click(object sender, System.EventArgs e)
		{
			viewControl.resetWindow();
			viewControl.clearList();
			IconicDisplayStack.Items.Clear();
			StackCountLabel.Text = "" + viewControl.getListCount();
			viewControl.repaint();
		}
	
        /*************************************************************************
         * Eventhandles for the GUI components mapping the view functions, like
         * move, zoom and the neutral state.
        /**************************************************************************/
		private void MoveButton_CheckedChanged(object sender, System.EventArgs e)
		{
			viewControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
		}

		private void ZoomButton_CheckedChanged(object sender, System.EventArgs e)
		{
			viewControl.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
		}

        private void NoneButton_CheckedChanged(object sender, System.EventArgs e)
        {
        	viewControl.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }
		
		/*************************************************************************
         * Change the graphical context regarding the color mode
		/**************************************************************************/
		private void ColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string val = (string)ColorComboBox.SelectedItem;

			if(!locked)
			{
				switch(val)
				{
					case "colored3":
						viewControl.changeGraphicSettings(GraphicsContext.GC_COLORED, 3);
						break;
					case "colored6":
						viewControl.changeGraphicSettings(GraphicsContext.GC_COLORED, 6);
						break;
					case "colored12":
						viewControl.changeGraphicSettings(GraphicsContext.GC_COLORED, 12);
						break;
					default:
						viewControl.changeGraphicSettings(GraphicsContext.GC_COLOR, val);
						break;
				}
			}
		}

        /*************************************************************************
         * Change the graphical context in terms of linewidth or the drawmode
        /**************************************************************************/
		private void LineWidthComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int val = Convert.ToInt32(LineWidthComboBox.SelectedItem);
			if(!locked)
			{
				viewControl.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, val);
			}
		}
		
		private void DrawModeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string val = (string)DrawModeComboBox.SelectedItem;
			if(!locked)
			{
				viewControl.changeGraphicSettings(GraphicsContext.GC_DRAWMODE, val);
			}
		}

        /*************************************************************************
         * Add iconical objects to the graphics stack by double clicking
         * single entries in the listbox
        /**************************************************************************/
		private void IconiclistBox_DoubleClick(object sender, System.EventArgs e)
		{
			string col, lw, dm;
			col = lw = dm = "";
			string name = (string)IconiclistBox.SelectedItem;
			
			HObject obj = (HObject)procList[name];
			
			viewControl.addIconicVar(obj);
			viewControl.repaint();
			
			if(viewControl.getListCount()==1)
				IconicDisplayStack.Items.Clear();

			if(ColorComboBox.SelectedIndex!=-1)
				col = (string)ColorComboBox.SelectedItem;

			if(LineWidthComboBox.SelectedIndex!=-1)
				lw = (string)LineWidthComboBox.SelectedItem;

			if(DrawModeComboBox.SelectedIndex!=-1)
				dm = (string)DrawModeComboBox.SelectedItem;

			addGraphicsStackDisplay(name, col, lw, dm);
		}

		/**************************************************************************/
		/**************************************************************************/
		private void performIPTask()
		{
			string color, drawMode, lineWidth;
			Hashtable lastState;
			HImage mean, particlesRed;
			HRegion large, largeDilation, notLarge, smallRaw, small, smallConnection;

			color = drawMode = lineWidth = "";
			IconiclistBox.Items.Clear();
			IconicDisplayStack.Items.Clear();

			lastState = viewControl.getGraphicContext();
			
			if(lastState.ContainsKey(GraphicsContext.GC_COLOR))
				color = (string)lastState[GraphicsContext.GC_COLOR];
			else if(lastState.ContainsKey(GraphicsContext.GC_COLORED))
				color =  "colored" + (int)lastState[GraphicsContext.GC_COLORED];

			if(lastState.ContainsKey(GraphicsContext.GC_DRAWMODE))
				drawMode = (string)lastState[GraphicsContext.GC_DRAWMODE];

			if(lastState.ContainsKey(GraphicsContext.GC_LINEWIDTH))
				lineWidth = "" + (int)lastState[GraphicsContext.GC_LINEWIDTH];
			
			/* ip task */
			large = image.Threshold(110.0, 255.0); 
			largeDilation = large.DilationCircle(7.5d);
			notLarge = largeDilation.Complement();
			particlesRed = image.ReduceDomain(notLarge);
			mean = particlesRed.MeanImage(31,31);
			smallRaw = particlesRed.DynThreshold(mean, 3.0, "light");
			small = smallRaw.OpeningCircle(2.5);
			smallConnection = small.Connection();

			/* add for display*/
			addToStack("Image", image, true, color, lineWidth, drawMode);				  
				
			viewControl.changeGraphicSettings(GraphicsContext.GC_COLOR, "green");
			color = "green";
			addToStack("Large", large, true, color, lineWidth, drawMode);				  

			viewControl.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 3);
			viewControl.changeGraphicSettings(GraphicsContext.GC_DRAWMODE, "margin");
			viewControl.changeGraphicSettings(GraphicsContext.GC_COLOR, "yellow");
			color = "yellow";
			lineWidth = "3";
			drawMode = "margin";
			addToStack("LargeDilation", largeDilation, true, color, lineWidth, drawMode); 
			
			viewControl.changeGraphicSettings(GraphicsContext.GC_COLOR, "red");
			color = "red";
			addToStack("Small", small, true, color, lineWidth, drawMode);				

			viewControl.changeGraphicSettings(GraphicsContext.GC_DRAWMODE, "fill");
			viewControl.changeGraphicSettings(GraphicsContext.GC_COLORED, 12);
			color = "colored12";
			drawMode = "fill";
			addToStack("SmallConnection", smallConnection, true, color, lineWidth, drawMode); 

			/* display last settings in combobox*/
			locked = true;
			LineWidthComboBox.SelectedIndex = LineWidthComboBox.Items.IndexOf("3");
			DrawModeComboBox.SelectedIndex = DrawModeComboBox.Items.IndexOf("fill"); 
			ColorComboBox.SelectedIndex = ColorComboBox.Items.IndexOf("colored12");
			locked=false;
		}


		/*************************************************************************
         * Auxiliary methods to manage the adding-process of iconical objects, in 
         * the sense that each object added to the stack gets an entry in another
         * listbox, describing the current graphical setup of this new object. So 
         * the user has a means to track the changes in the graphical context and the
         * effects for each object extending the stack.
		/**************************************************************************/
		private void addToStack(string Name, HObject obj, bool addToPL)
		{	
			string col, lw, dm;
			col = lw = dm = "";

			if(addToPL)
				procList.Add(Name, obj);
			
			IconiclistBox.Items.Add(Name);
			
			if(ColorComboBox.SelectedIndex!=-1)
				col = (string)ColorComboBox.SelectedItem;
			
			if(LineWidthComboBox.SelectedIndex!=-1)
				lw = (string)LineWidthComboBox.SelectedItem;

			if(DrawModeComboBox.SelectedIndex!=-1)
				dm = (string)DrawModeComboBox.SelectedItem;

			viewControl.addIconicVar(obj);
			addGraphicsStackDisplay(Name, col, lw, dm);
		}

		private void addToStack(string Name, HObject obj, bool addToPL, 
								string col, string lw, string dm)
		{	
			if(addToPL)
				procList.Add(Name, obj);
			
			viewControl.addIconicVar(obj);
		
			IconiclistBox.Items.Add(Name);
			addGraphicsStackDisplay(Name, col, lw, dm);
		}


		private void addGraphicsStackDisplay(string HObjName,
											 string Color,
											 string LineWidth,
											 string DrawMode)
		{
			string item = HObjName + "  (";
			
			if(Color!="")
				item += " C=" + Color + " ";

			if(LineWidth!="")
				item += " L=" + LineWidth + " ";

			if(DrawMode!="")
				item += " D=" + DrawMode + " ";
			
			item += ")  ";
			IconicDisplayStack.Items.Add(item);
			StackCountLabel.Text = "" + viewControl.getListCount();
		}

	    
		/// <summary>
		/// Update method invoked by the window controller for errors 
		/// that may occur during loading or processing
		/// </summary>
		public void UpdateHWnd(int mode)
		{
			switch(mode)
			{
				case HWndCtrl.ERR_READING_IMG:
					MessageBox.Show("Problem occured while reading file! \n" + 
						viewControl.exceptionText, 
						"GraphicsStack",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
					break;
				case HWndCtrl.ERR_DEFINING_GC:
					MessageBox.Show("Problem occured while setting up graphical context! \n " +
						viewControl.exceptionText,
						"GraphicsStack",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					break;
			}
		}
        
	}//end of class
}//end of namespace

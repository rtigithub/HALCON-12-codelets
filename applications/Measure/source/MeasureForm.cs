using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using ViewROI;
using HalconDotNet;
using MeasureModule;




namespace MeasureMain
{
	/// <summary>
	/// This project provides the main functionality of HDevelop's Measure Assistant,
	/// with the main exception of fuzzy measure.
	/// In the first tab you can adjust the parameters for edge 
	/// detection. The second tab displays the measure results and the 
	/// third tab plots the gray value profile of the selected ROI.
	/// The project is composed of several classes for paremeterizing 
	/// the measurements (like MeasurementEdge, MeasurementPair or 
	/// MeasurementResult) and the controller class MeasureAssistant,
	/// which is in charge of the communication between the GUI-frontend
	/// and the model classes.
	/// </summary>
	public class MeasureForm : System.Windows.Forms.Form
	{

		/* base variables */
		private HWindowControl      viewPort;
		private HWndCtrl            mView;
		public  ROIController       roiController;
		public  MeasureAssistant    mAssistant;
		public  HXLDCont            mShadow;
		private FunctionPlot        plotGraphWindow;
		private HImage              currImage;

		/* display parameters */
		private string regionColor;
		private string edgeColor;
		private int    lineWidth;

		private bool   displayRegion;
		private bool   useShadow;
		private bool   updateLineProfile;


		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button LoadImgButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton NoneButton;
		private System.Windows.Forms.RadioButton MoveButton;
		private System.Windows.Forms.RadioButton ZoomButton;
		private System.Windows.Forms.Button LineButton;
		private System.Windows.Forms.Button DeleteActRoiButton;
		private System.Windows.Forms.Button ResetROIButton;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageEdges;
		private System.Windows.Forms.TabPage tabPageResults;
		private System.Windows.Forms.OpenFileDialog openImageFileDialog;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox EdgeToPaircheckBox;
		private System.Windows.Forms.RadioButton MagnifyButton;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.ComboBox TransitionComboBox;
		private System.Windows.Forms.ComboBox PositionComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox RegionColorComboBox;
		private System.Windows.Forms.ComboBox EdgeColorComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown EdgeLengthUpDown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown LineWidthDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox ShowRegionCheckBox;
		private System.Windows.Forms.CheckBox UseShadowsCheckBox;
		private System.Windows.Forms.CheckBox UseROIWidthCheckBox;
		private System.Windows.Forms.NumericUpDown ROIWidthUpDown;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.NumericUpDown SmoothingUpDown;
		private System.Windows.Forms.NumericUpDown MinEdgAmplUpDown;
		private System.Windows.Forms.ComboBox InterpolationComboBox;
		private System.Windows.Forms.Button ResetViewButton;
		private System.Windows.Forms.TrackBar MinEdgAmplTrackBar;
		private System.Windows.Forms.TrackBar SmoothingTrackBar;
		private System.Windows.Forms.TrackBar ROIWidthTrackBar;
		private System.Windows.Forms.Button ResetMinEdgAmplButton;
		private System.Windows.Forms.Label StatusLabel;
		private System.Windows.Forms.Button ResetSmoothingButton;
		private System.Windows.Forms.Button ResetROIWidthButton;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.CheckBox PositionCheckBox;
		private System.Windows.Forms.CheckBox PairWidthCheckBox;
		private System.Windows.Forms.CheckBox AmplitudeCheckBox;
		private System.Windows.Forms.CheckBox DistanceCheckBox;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.CheckBox TransWCoordCheckBox;
		private System.Windows.Forms.Button LoadPoseButton;
		private System.Windows.Forms.Button LoadCalibButton;
		private System.Windows.Forms.TextBox CalibCamTextBox;
		private System.Windows.Forms.TextBox CalibPoseTextBox;
		private System.Windows.Forms.ListView EdgeResultListView;
		private System.Windows.Forms.OpenFileDialog openCamparFileDialog;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.ComboBox UnitComboBox;
		private System.Windows.Forms.Panel UnitPanel;
		private System.Windows.Forms.ListBox ActiveROIListBox;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.CheckBox ShowROIcheckBox;
		private System.Windows.Forms.Button CircArcButton;
		private System.Windows.Forms.TabPage tabPageLineProfile;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Panel panelAxis;
		private System.Windows.Forms.Label labelDeviation;
		private System.Windows.Forms.Label labelMean;
		private System.Windows.Forms.Label labelRange;
		private System.Windows.Forms.Label labelPeak;
		private System.Windows.Forms.Label labelRangeX;
		private System.Windows.Forms.Label labelPeakX;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.ComboBox CSScaleComboBox;
		private GroupBox groupBox3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MeasureForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Cleans up the used resources.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
            this.viewPort = new HalconDotNet.HWindowControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageEdges = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ShowROIcheckBox = new System.Windows.Forms.CheckBox();
            this.UseROIWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.UseShadowsCheckBox = new System.Windows.Forms.CheckBox();
            this.ShowRegionCheckBox = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LineWidthDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.EdgeLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.EdgeColorComboBox = new System.Windows.Forms.ComboBox();
            this.RegionColorComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PositionComboBox = new System.Windows.Forms.ComboBox();
            this.TransitionComboBox = new System.Windows.Forms.ComboBox();
            this.EdgeToPaircheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ResetROIWidthButton = new System.Windows.Forms.Button();
            this.ResetSmoothingButton = new System.Windows.Forms.Button();
            this.ResetMinEdgAmplButton = new System.Windows.Forms.Button();
            this.ROIWidthTrackBar = new System.Windows.Forms.TrackBar();
            this.SmoothingTrackBar = new System.Windows.Forms.TrackBar();
            this.MinEdgAmplTrackBar = new System.Windows.Forms.TrackBar();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MinEdgAmplUpDown = new System.Windows.Forms.NumericUpDown();
            this.SmoothingUpDown = new System.Windows.Forms.NumericUpDown();
            this.InterpolationComboBox = new System.Windows.Forms.ComboBox();
            this.ROIWidthUpDown = new System.Windows.Forms.NumericUpDown();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.ActiveROIListBox = new System.Windows.Forms.ListBox();
            this.EdgeResultListView = new System.Windows.Forms.ListView();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.UnitPanel = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.UnitComboBox = new System.Windows.Forms.ComboBox();
            this.CalibPoseTextBox = new System.Windows.Forms.TextBox();
            this.CalibCamTextBox = new System.Windows.Forms.TextBox();
            this.LoadPoseButton = new System.Windows.Forms.Button();
            this.LoadCalibButton = new System.Windows.Forms.Button();
            this.TransWCoordCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.DistanceCheckBox = new System.Windows.Forms.CheckBox();
            this.AmplitudeCheckBox = new System.Windows.Forms.CheckBox();
            this.PairWidthCheckBox = new System.Windows.Forms.CheckBox();
            this.PositionCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPageLineProfile = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.CSScaleComboBox = new System.Windows.Forms.ComboBox();
            this.panelAxis = new System.Windows.Forms.Panel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.labelDeviation = new System.Windows.Forms.Label();
            this.labelMean = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.labelPeak = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.labelRangeX = new System.Windows.Forms.Label();
            this.labelPeakX = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CircArcButton = new System.Windows.Forms.Button();
            this.LineButton = new System.Windows.Forms.Button();
            this.DeleteActRoiButton = new System.Windows.Forms.Button();
            this.ResetROIButton = new System.Windows.Forms.Button();
            this.LoadImgButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ResetViewButton = new System.Windows.Forms.Button();
            this.MagnifyButton = new System.Windows.Forms.RadioButton();
            this.ZoomButton = new System.Windows.Forms.RadioButton();
            this.MoveButton = new System.Windows.Forms.RadioButton();
            this.NoneButton = new System.Windows.Forms.RadioButton();
            this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.openCamparFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl.SuspendLayout();
            this.tabPageEdges.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LineWidthDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EdgeLengthUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ROIWidthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothingTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinEdgAmplTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinEdgAmplUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothingUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROIWidthUpDown)).BeginInit();
            this.tabPageResults.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.UnitPanel.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPageLineProfile.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(8, 51);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(632, 474);
            this.viewPort.TabIndex = 0;
            this.viewPort.WindowSize = new System.Drawing.Size(632, 474);
            // 
            // tabControl
            // 
            this.tabControl.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                     this.tabPageEdges,
                                                                                     this.tabPageResults,
                                                                                     this.tabPageLineProfile});
            this.tabControl.Location = new System.Drawing.Point(652, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(358, 516);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageEdges
            // 
            this.tabPageEdges.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                       this.groupBox6,
                                                                                       this.groupBox5,
                                                                                       this.groupBox4});
            this.tabPageEdges.Location = new System.Drawing.Point(4, 22);
            this.tabPageEdges.Name = "tabPageEdges";
            this.tabPageEdges.Size = new System.Drawing.Size(350, 490);
            this.tabPageEdges.TabIndex = 1;
            this.tabPageEdges.Text = "  Edges  ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.ShowROIcheckBox,
                                                                                    this.UseROIWidthCheckBox,
                                                                                    this.UseShadowsCheckBox,
                                                                                    this.ShowRegionCheckBox,
                                                                                    this.label6,
                                                                                    this.LineWidthDown,
                                                                                    this.label5,
                                                                                    this.EdgeLengthUpDown,
                                                                                    this.label4,
                                                                                    this.label3,
                                                                                    this.EdgeColorComboBox,
                                                                                    this.RegionColorComboBox});
            this.groupBox6.Location = new System.Drawing.Point(8, 320);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(336, 160);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Display Parameters";
            // 
            // ShowROIcheckBox
            // 
            this.ShowROIcheckBox.Location = new System.Drawing.Point(216, 128);
            this.ShowROIcheckBox.Name = "ShowROIcheckBox";
            this.ShowROIcheckBox.TabIndex = 11;
            this.ShowROIcheckBox.Text = "  Show ROIs";
            this.ShowROIcheckBox.CheckedChanged += new System.EventHandler(this.ShowROIcheckBox_CheckedChanged);
            // 
            // UseROIWidthCheckBox
            // 
            this.UseROIWidthCheckBox.Location = new System.Drawing.Point(216, 96);
            this.UseROIWidthCheckBox.Name = "UseROIWidthCheckBox";
            this.UseROIWidthCheckBox.TabIndex = 10;
            this.UseROIWidthCheckBox.Text = "Use ROI Width";
            this.UseROIWidthCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UseROIWidthCheckBox.CheckedChanged += new System.EventHandler(this.UseROIWidthCheckBox_CheckedChanged);
            // 
            // UseShadowsCheckBox
            // 
            this.UseShadowsCheckBox.Location = new System.Drawing.Point(216, 64);
            this.UseShadowsCheckBox.Name = "UseShadowsCheckBox";
            this.UseShadowsCheckBox.Size = new System.Drawing.Size(96, 24);
            this.UseShadowsCheckBox.TabIndex = 9;
            this.UseShadowsCheckBox.Text = "Use Shadows";
            this.UseShadowsCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.UseShadowsCheckBox.CheckedChanged += new System.EventHandler(this.UseShadowsCheckBox_CheckedChanged);
            // 
            // ShowRegionCheckBox
            // 
            this.ShowRegionCheckBox.Location = new System.Drawing.Point(216, 32);
            this.ShowRegionCheckBox.Name = "ShowRegionCheckBox";
            this.ShowRegionCheckBox.Size = new System.Drawing.Size(96, 24);
            this.ShowRegionCheckBox.TabIndex = 8;
            this.ShowRegionCheckBox.Text = "Show Region";
            this.ShowRegionCheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShowRegionCheckBox.CheckedChanged += new System.EventHandler(this.ShowRegionCheckBox_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 24);
            this.label6.TabIndex = 7;
            this.label6.Text = "Line Width";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LineWidthDown
            // 
            this.LineWidthDown.Location = new System.Drawing.Point(88, 128);
            this.LineWidthDown.Maximum = new System.Decimal(new int[] {
                                                                          9,
                                                                          0,
                                                                          0,
                                                                          0});
            this.LineWidthDown.Minimum = new System.Decimal(new int[] {
                                                                          1,
                                                                          0,
                                                                          0,
                                                                          0});
            this.LineWidthDown.Name = "LineWidthDown";
            this.LineWidthDown.Size = new System.Drawing.Size(112, 20);
            this.LineWidthDown.TabIndex = 6;
            this.LineWidthDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LineWidthDown.Value = new System.Decimal(new int[] {
                                                                        1,
                                                                        0,
                                                                        0,
                                                                        0});
            this.LineWidthDown.ValueChanged += new System.EventHandler(this.LineWidthDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 24);
            this.label5.TabIndex = 5;
            this.label5.Text = "Edge Length";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EdgeLengthUpDown
            // 
            this.EdgeLengthUpDown.Location = new System.Drawing.Point(88, 96);
            this.EdgeLengthUpDown.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.EdgeLengthUpDown.Name = "EdgeLengthUpDown";
            this.EdgeLengthUpDown.Size = new System.Drawing.Size(112, 20);
            this.EdgeLengthUpDown.TabIndex = 4;
            this.EdgeLengthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.EdgeLengthUpDown.Value = new System.Decimal(new int[] {
                                                                           1,
                                                                           0,
                                                                           0,
                                                                           0});
            this.EdgeLengthUpDown.ValueChanged += new System.EventHandler(this.EdgeLengthUpDown_ValueChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "Edge Color";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "Region Color";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EdgeColorComboBox
            // 
            this.EdgeColorComboBox.Items.AddRange(new object[] {
                                                                   "green",
                                                                   "red",
                                                                   "blue",
                                                                   "black",
                                                                   "white",
                                                                   "yellow",
                                                                   "magenta",
                                                                   "cyan",
                                                                   "gray"});
            this.EdgeColorComboBox.Location = new System.Drawing.Point(88, 64);
            this.EdgeColorComboBox.Name = "EdgeColorComboBox";
            this.EdgeColorComboBox.Size = new System.Drawing.Size(112, 21);
            this.EdgeColorComboBox.TabIndex = 1;
            this.EdgeColorComboBox.Text = "EdgeColor";
            this.EdgeColorComboBox.SelectedIndexChanged += new System.EventHandler(this.EdgeColorComboBox_SelectedIndexChanged);
            // 
            // RegionColorComboBox
            // 
            this.RegionColorComboBox.Items.AddRange(new object[] {
                                                                     "green",
                                                                     "red",
                                                                     "blue",
                                                                     "black",
                                                                     "white",
                                                                     "yellow",
                                                                     "magenta",
                                                                     "cyan",
                                                                     "gray"});
            this.RegionColorComboBox.Location = new System.Drawing.Point(88, 32);
            this.RegionColorComboBox.Name = "RegionColorComboBox";
            this.RegionColorComboBox.Size = new System.Drawing.Size(112, 21);
            this.RegionColorComboBox.TabIndex = 0;
            this.RegionColorComboBox.Text = "RegionColor";
            this.RegionColorComboBox.SelectedIndexChanged += new System.EventHandler(this.RegionColorComboBox_SelectedIndexChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.label2,
                                                                                    this.label1,
                                                                                    this.PositionComboBox,
                                                                                    this.TransitionComboBox,
                                                                                    this.EdgeToPaircheckBox});
            this.groupBox5.Location = new System.Drawing.Point(8, 188);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 120);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Edge Selection";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Position";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Transition";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PositionComboBox
            // 
            this.PositionComboBox.Items.AddRange(new object[] {
                                                                  "all",
                                                                  "first",
                                                                  "last"});
            this.PositionComboBox.Location = new System.Drawing.Point(88, 88);
            this.PositionComboBox.Name = "PositionComboBox";
            this.PositionComboBox.Size = new System.Drawing.Size(160, 21);
            this.PositionComboBox.TabIndex = 3;
            this.PositionComboBox.Text = "Position";
            this.PositionComboBox.SelectedIndexChanged += new System.EventHandler(this.PositionComboBox_SelectedIndexChanged);
            // 
            // TransitionComboBox
            // 
            this.TransitionComboBox.Items.AddRange(new object[] {
                                                                    "all",
                                                                    "positive",
                                                                    "negative"});
            this.TransitionComboBox.Location = new System.Drawing.Point(88, 56);
            this.TransitionComboBox.Name = "TransitionComboBox";
            this.TransitionComboBox.Size = new System.Drawing.Size(160, 21);
            this.TransitionComboBox.TabIndex = 2;
            this.TransitionComboBox.Text = "Transition";
            this.TransitionComboBox.SelectedIndexChanged += new System.EventHandler(this.TransitionComboBox_SelectedIndexChanged);
            // 
            // EdgeToPaircheckBox
            // 
            this.EdgeToPaircheckBox.Location = new System.Drawing.Point(88, 24);
            this.EdgeToPaircheckBox.Name = "EdgeToPaircheckBox";
            this.EdgeToPaircheckBox.Size = new System.Drawing.Size(144, 24);
            this.EdgeToPaircheckBox.TabIndex = 1;
            this.EdgeToPaircheckBox.Text = "Group Edges to Pairs";
            this.EdgeToPaircheckBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.EdgeToPaircheckBox.CheckedChanged += new System.EventHandler(this.EdgeToPaircheckBox_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.ResetROIWidthButton,
                                                                                    this.ResetSmoothingButton,
                                                                                    this.ResetMinEdgAmplButton,
                                                                                    this.ROIWidthTrackBar,
                                                                                    this.SmoothingTrackBar,
                                                                                    this.MinEdgAmplTrackBar,
                                                                                    this.label10,
                                                                                    this.label9,
                                                                                    this.label8,
                                                                                    this.label7,
                                                                                    this.MinEdgAmplUpDown,
                                                                                    this.SmoothingUpDown,
                                                                                    this.InterpolationComboBox,
                                                                                    this.ROIWidthUpDown});
            this.groupBox4.Location = new System.Drawing.Point(8, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(336, 160);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Edge Extraction";
            // 
            // ResetROIWidthButton
            // 
            this.ResetROIWidthButton.Location = new System.Drawing.Point(280, 88);
            this.ResetROIWidthButton.Name = "ResetROIWidthButton";
            this.ResetROIWidthButton.Size = new System.Drawing.Size(48, 24);
            this.ResetROIWidthButton.TabIndex = 14;
            this.ResetROIWidthButton.Text = "Reset";
            this.ResetROIWidthButton.Click += new System.EventHandler(this.ResetROIWidthButton_Click);
            // 
            // ResetSmoothingButton
            // 
            this.ResetSmoothingButton.Location = new System.Drawing.Point(280, 56);
            this.ResetSmoothingButton.Name = "ResetSmoothingButton";
            this.ResetSmoothingButton.Size = new System.Drawing.Size(48, 24);
            this.ResetSmoothingButton.TabIndex = 13;
            this.ResetSmoothingButton.Text = "Reset";
            this.ResetSmoothingButton.Click += new System.EventHandler(this.ResetSmoothingButton_Click);
            // 
            // ResetMinEdgAmplButton
            // 
            this.ResetMinEdgAmplButton.Location = new System.Drawing.Point(280, 24);
            this.ResetMinEdgAmplButton.Name = "ResetMinEdgAmplButton";
            this.ResetMinEdgAmplButton.Size = new System.Drawing.Size(48, 24);
            this.ResetMinEdgAmplButton.TabIndex = 12;
            this.ResetMinEdgAmplButton.Text = "Reset";
            this.ResetMinEdgAmplButton.Click += new System.EventHandler(this.ResetMinEdgAmplButton_Click);
            // 
            // ROIWidthTrackBar
            // 
            this.ROIWidthTrackBar.Location = new System.Drawing.Point(136, 86);
            this.ROIWidthTrackBar.Maximum = 255;
            this.ROIWidthTrackBar.Minimum = 1;
            this.ROIWidthTrackBar.Name = "ROIWidthTrackBar";
            this.ROIWidthTrackBar.Size = new System.Drawing.Size(144, 42);
            this.ROIWidthTrackBar.TabIndex = 11;
            this.ROIWidthTrackBar.TickFrequency = 10;
            this.ROIWidthTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ROIWidthTrackBar.Value = 1;
            this.ROIWidthTrackBar.Scroll += new System.EventHandler(this.ROIWidthTrackBar_Scroll);
            // 
            // SmoothingTrackBar
            // 
            this.SmoothingTrackBar.Location = new System.Drawing.Point(136, 54);
            this.SmoothingTrackBar.Maximum = 250;
            this.SmoothingTrackBar.Minimum = 4;
            this.SmoothingTrackBar.Name = "SmoothingTrackBar";
            this.SmoothingTrackBar.Size = new System.Drawing.Size(144, 42);
            this.SmoothingTrackBar.TabIndex = 10;
            this.SmoothingTrackBar.TickFrequency = 10;
            this.SmoothingTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SmoothingTrackBar.Value = 4;
            this.SmoothingTrackBar.Scroll += new System.EventHandler(this.SmoothingTrackBar_Scroll);
            // 
            // MinEdgAmplTrackBar
            // 
            this.MinEdgAmplTrackBar.Location = new System.Drawing.Point(136, 20);
            this.MinEdgAmplTrackBar.Maximum = 255;
            this.MinEdgAmplTrackBar.Minimum = 1;
            this.MinEdgAmplTrackBar.Name = "MinEdgAmplTrackBar";
            this.MinEdgAmplTrackBar.Size = new System.Drawing.Size(144, 42);
            this.MinEdgAmplTrackBar.TabIndex = 9;
            this.MinEdgAmplTrackBar.TickFrequency = 10;
            this.MinEdgAmplTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinEdgAmplTrackBar.Value = 1;
            this.MinEdgAmplTrackBar.Scroll += new System.EventHandler(this.MinEdgAmplTrackBar_Scroll);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 24);
            this.label10.TabIndex = 8;
            this.label10.Text = "ROI Width";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(8, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 24);
            this.label9.TabIndex = 7;
            this.label9.Text = "Smoothing*10";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 119);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 37);
            this.label8.TabIndex = 6;
            this.label8.Text = "Interpolation Method";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 24);
            this.label7.TabIndex = 5;
            this.label7.Text = "Min.EdgeAmpl";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinEdgAmplUpDown
            // 
            this.MinEdgAmplUpDown.Location = new System.Drawing.Point(88, 24);
            this.MinEdgAmplUpDown.Maximum = new System.Decimal(new int[] {
                                                                             255,
                                                                             0,
                                                                             0,
                                                                             0});
            this.MinEdgAmplUpDown.Minimum = new System.Decimal(new int[] {
                                                                             1,
                                                                             0,
                                                                             0,
                                                                             0});
            this.MinEdgAmplUpDown.Name = "MinEdgAmplUpDown";
            this.MinEdgAmplUpDown.Size = new System.Drawing.Size(48, 20);
            this.MinEdgAmplUpDown.TabIndex = 3;
            this.MinEdgAmplUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.MinEdgAmplUpDown.Value = new System.Decimal(new int[] {
                                                                           1,
                                                                           0,
                                                                           0,
                                                                           0});
            this.MinEdgAmplUpDown.ValueChanged += new System.EventHandler(this.MinEdgAmplUpDown_ValueChanged);
            // 
            // SmoothingUpDown
            // 
            this.SmoothingUpDown.Location = new System.Drawing.Point(88, 56);
            this.SmoothingUpDown.Maximum = new System.Decimal(new int[] {
                                                                            250,
                                                                            0,
                                                                            0,
                                                                            0});
            this.SmoothingUpDown.Minimum = new System.Decimal(new int[] {
                                                                            4,
                                                                            0,
                                                                            0,
                                                                            0});
            this.SmoothingUpDown.Name = "SmoothingUpDown";
            this.SmoothingUpDown.Size = new System.Drawing.Size(48, 20);
            this.SmoothingUpDown.TabIndex = 2;
            this.SmoothingUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SmoothingUpDown.Value = new System.Decimal(new int[] {
                                                                          4,
                                                                          0,
                                                                          0,
                                                                          0});
            this.SmoothingUpDown.ValueChanged += new System.EventHandler(this.SmoothingUpDown_ValueChanged);
            // 
            // InterpolationComboBox
            // 
            this.InterpolationComboBox.Items.AddRange(new object[] {
                                                                       "nearest_neighbor",
                                                                       "bilinear",
                                                                       "bicubic"});
            this.InterpolationComboBox.Location = new System.Drawing.Point(88, 128);
            this.InterpolationComboBox.Name = "InterpolationComboBox";
            this.InterpolationComboBox.Size = new System.Drawing.Size(160, 21);
            this.InterpolationComboBox.TabIndex = 1;
            this.InterpolationComboBox.Text = "Interpolation";
            this.InterpolationComboBox.SelectedIndexChanged += new System.EventHandler(this.InterpolationComboBox_SelectedIndexChanged);
            // 
            // ROIWidthUpDown
            // 
            this.ROIWidthUpDown.Location = new System.Drawing.Point(88, 88);
            this.ROIWidthUpDown.Maximum = new System.Decimal(new int[] {
                                                                           255,
                                                                           0,
                                                                           0,
                                                                           0});
            this.ROIWidthUpDown.Minimum = new System.Decimal(new int[] {
                                                                           1,
                                                                           0,
                                                                           0,
                                                                           0});
            this.ROIWidthUpDown.Name = "ROIWidthUpDown";
            this.ROIWidthUpDown.Size = new System.Drawing.Size(48, 20);
            this.ROIWidthUpDown.TabIndex = 0;
            this.ROIWidthUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ROIWidthUpDown.Value = new System.Decimal(new int[] {
                                                                         1,
                                                                         0,
                                                                         0,
                                                                         0});
            this.ROIWidthUpDown.ValueChanged += new System.EventHandler(this.ROIWidthUpDown_ValueChanged);
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                         this.label15,
                                                                                         this.ActiveROIListBox,
                                                                                         this.EdgeResultListView,
                                                                                         this.groupBox8,
                                                                                         this.groupBox7});
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(350, 490);
            this.tabPageResults.TabIndex = 2;
            this.tabPageResults.Text = "  Results  ";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(40, 192);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label15.Size = new System.Drawing.Size(72, 24);
            this.label15.TabIndex = 16;
            this.label15.Text = "Active ROI";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ActiveROIListBox
            // 
            this.ActiveROIListBox.Location = new System.Drawing.Point(114, 192);
            this.ActiveROIListBox.Name = "ActiveROIListBox";
            this.ActiveROIListBox.Size = new System.Drawing.Size(230, 82);
            this.ActiveROIListBox.TabIndex = 15;
            this.ActiveROIListBox.SelectedIndexChanged += new System.EventHandler(this.ActiveROIListBox_SelectedIndexChanged);
            // 
            // EdgeResultListView
            // 
            this.EdgeResultListView.AutoArrange = false;
            this.EdgeResultListView.FullRowSelect = true;
            this.EdgeResultListView.GridLines = true;
            this.EdgeResultListView.Location = new System.Drawing.Point(10, 290);
            this.EdgeResultListView.Name = "EdgeResultListView";
            this.EdgeResultListView.Size = new System.Drawing.Size(334, 190);
            this.EdgeResultListView.TabIndex = 14;
            this.EdgeResultListView.View = System.Windows.Forms.View.Details;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.UnitPanel,
                                                                                    this.CalibPoseTextBox,
                                                                                    this.CalibCamTextBox,
                                                                                    this.LoadPoseButton,
                                                                                    this.LoadCalibButton,
                                                                                    this.TransWCoordCheckBox});
            this.groupBox8.Location = new System.Drawing.Point(8, 72);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(336, 108);
            this.groupBox8.TabIndex = 12;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Feature Processing";
            // 
            // UnitPanel
            // 
            this.UnitPanel.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.label14,
                                                                                    this.UnitComboBox});
            this.UnitPanel.Location = new System.Drawing.Point(216, 16);
            this.UnitPanel.Name = "UnitPanel";
            this.UnitPanel.Size = new System.Drawing.Size(112, 32);
            this.UnitPanel.TabIndex = 16;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 24);
            this.label14.TabIndex = 4;
            this.label14.Text = "Unit";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UnitComboBox
            // 
            this.UnitComboBox.Items.AddRange(new object[] {
                                                              "m",
                                                              "cm",
                                                              "mm",
                                                              "µm"});
            this.UnitComboBox.Location = new System.Drawing.Point(56, 8);
            this.UnitComboBox.Name = "UnitComboBox";
            this.UnitComboBox.Size = new System.Drawing.Size(56, 21);
            this.UnitComboBox.TabIndex = 3;
            this.UnitComboBox.Text = "unit";
            this.UnitComboBox.SelectedIndexChanged += new System.EventHandler(this.UnitComboBox_SelectedIndexChanged);
            // 
            // CalibPoseTextBox
            // 
            this.CalibPoseTextBox.Location = new System.Drawing.Point(120, 78);
            this.CalibPoseTextBox.Name = "CalibPoseTextBox";
            this.CalibPoseTextBox.ReadOnly = true;
            this.CalibPoseTextBox.Size = new System.Drawing.Size(208, 20);
            this.CalibPoseTextBox.TabIndex = 15;
            this.CalibPoseTextBox.Text = " *.dat";
            // 
            // CalibCamTextBox
            // 
            this.CalibCamTextBox.Location = new System.Drawing.Point(120, 56);
            this.CalibCamTextBox.Name = "CalibCamTextBox";
            this.CalibCamTextBox.ReadOnly = true;
            this.CalibCamTextBox.Size = new System.Drawing.Size(208, 20);
            this.CalibCamTextBox.TabIndex = 14;
            this.CalibCamTextBox.Text = " *.cal";
            // 
            // LoadPoseButton
            // 
            this.LoadPoseButton.Location = new System.Drawing.Point(16, 78);
            this.LoadPoseButton.Name = "LoadPoseButton";
            this.LoadPoseButton.Size = new System.Drawing.Size(88, 20);
            this.LoadPoseButton.TabIndex = 13;
            this.LoadPoseButton.Text = "  Load Pose";
            this.LoadPoseButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoadPoseButton.Click += new System.EventHandler(this.LoadCamParamButton_Click);
            // 
            // LoadCalibButton
            // 
            this.LoadCalibButton.Location = new System.Drawing.Point(16, 56);
            this.LoadCalibButton.Name = "LoadCalibButton";
            this.LoadCalibButton.Size = new System.Drawing.Size(88, 20);
            this.LoadCalibButton.TabIndex = 12;
            this.LoadCalibButton.Text = "  Load CamPar";
            this.LoadCalibButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LoadCalibButton.Click += new System.EventHandler(this.LoadCamParamButton_Click);
            // 
            // TransWCoordCheckBox
            // 
            this.TransWCoordCheckBox.Location = new System.Drawing.Point(16, 24);
            this.TransWCoordCheckBox.Name = "TransWCoordCheckBox";
            this.TransWCoordCheckBox.Size = new System.Drawing.Size(195, 24);
            this.TransWCoordCheckBox.TabIndex = 0;
            this.TransWCoordCheckBox.Text = "Transform into World Coordinates";
            this.TransWCoordCheckBox.CheckedChanged += new System.EventHandler(this.TransWCoordCheckBox_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.DistanceCheckBox,
                                                                                    this.AmplitudeCheckBox,
                                                                                    this.PairWidthCheckBox,
                                                                                    this.PositionCheckBox});
            this.groupBox7.Location = new System.Drawing.Point(8, 16);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(336, 48);
            this.groupBox7.TabIndex = 11;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Feature Selection";
            // 
            // DistanceCheckBox
            // 
            this.DistanceCheckBox.Location = new System.Drawing.Point(172, 19);
            this.DistanceCheckBox.Name = "DistanceCheckBox";
            this.DistanceCheckBox.Size = new System.Drawing.Size(72, 24);
            this.DistanceCheckBox.TabIndex = 3;
            this.DistanceCheckBox.Text = "Distance";
            this.DistanceCheckBox.CheckedChanged += new System.EventHandler(this.DistanceCheckBox_CheckedChanged);
            // 
            // AmplitudeCheckBox
            // 
            this.AmplitudeCheckBox.Location = new System.Drawing.Point(90, 19);
            this.AmplitudeCheckBox.Name = "AmplitudeCheckBox";
            this.AmplitudeCheckBox.Size = new System.Drawing.Size(80, 24);
            this.AmplitudeCheckBox.TabIndex = 2;
            this.AmplitudeCheckBox.Text = "Amplitude";
            this.AmplitudeCheckBox.CheckedChanged += new System.EventHandler(this.AmplitudeCheckBox_CheckedChanged);
            // 
            // PairWidthCheckBox
            // 
            this.PairWidthCheckBox.Enabled = false;
            this.PairWidthCheckBox.Location = new System.Drawing.Point(250, 19);
            this.PairWidthCheckBox.Name = "PairWidthCheckBox";
            this.PairWidthCheckBox.Size = new System.Drawing.Size(80, 24);
            this.PairWidthCheckBox.TabIndex = 1;
            this.PairWidthCheckBox.Text = "Pair Width";
            this.PairWidthCheckBox.CheckedChanged += new System.EventHandler(this.PairWidthCheckBox_CheckedChanged);
            // 
            // PositionCheckBox
            // 
            this.PositionCheckBox.Location = new System.Drawing.Point(16, 19);
            this.PositionCheckBox.Name = "PositionCheckBox";
            this.PositionCheckBox.Size = new System.Drawing.Size(64, 24);
            this.PositionCheckBox.TabIndex = 0;
            this.PositionCheckBox.Text = "Position";
            this.PositionCheckBox.CheckedChanged += new System.EventHandler(this.PositionCheckBox_CheckedChanged);
            // 
            // tabPageLineProfile
            // 
            this.tabPageLineProfile.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                             this.label20,
                                                                                             this.CSScaleComboBox,
                                                                                             this.panelAxis,
                                                                                             this.groupBox9});
            this.tabPageLineProfile.Location = new System.Drawing.Point(4, 22);
            this.tabPageLineProfile.Name = "tabPageLineProfile";
            this.tabPageLineProfile.Size = new System.Drawing.Size(350, 490);
            this.tabPageLineProfile.TabIndex = 3;
            this.tabPageLineProfile.Text = " LineProfile ";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(160, 40);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(80, 24);
            this.label20.TabIndex = 6;
            this.label20.Text = "Y-axis scale:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CSScaleComboBox
            // 
            this.CSScaleComboBox.Items.AddRange(new object[] {
                                                                 "Adaptive",
                                                                 "Increasing",
                                                                 "Fixed"});
            this.CSScaleComboBox.Location = new System.Drawing.Point(248, 40);
            this.CSScaleComboBox.Name = "CSScaleComboBox";
            this.CSScaleComboBox.Size = new System.Drawing.Size(96, 21);
            this.CSScaleComboBox.TabIndex = 5;
            this.CSScaleComboBox.Text = "Scale Y-Axis";
            this.CSScaleComboBox.SelectedIndexChanged += new System.EventHandler(this.CSScaleComboBox_SelectedIndexChanged);
            // 
            // panelAxis
            // 
            this.panelAxis.Location = new System.Drawing.Point(8, 88);
            this.panelAxis.Name = "panelAxis";
            this.panelAxis.Size = new System.Drawing.Size(336, 232);
            this.panelAxis.TabIndex = 4;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.labelDeviation,
                                                                                    this.labelMean,
                                                                                    this.labelRange,
                                                                                    this.labelPeak,
                                                                                    this.label24,
                                                                                    this.label25,
                                                                                    this.labelRangeX,
                                                                                    this.labelPeakX,
                                                                                    this.label23,
                                                                                    this.label22,
                                                                                    this.label19,
                                                                                    this.label18,
                                                                                    this.label17,
                                                                                    this.label16});
            this.groupBox9.Location = new System.Drawing.Point(8, 344);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(336, 112);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Statistics";
            // 
            // labelDeviation
            // 
            this.labelDeviation.Location = new System.Drawing.Point(224, 88);
            this.labelDeviation.Name = "labelDeviation";
            this.labelDeviation.Size = new System.Drawing.Size(72, 16);
            this.labelDeviation.TabIndex = 18;
            this.labelDeviation.Text = "0";
            this.labelDeviation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMean
            // 
            this.labelMean.Location = new System.Drawing.Point(224, 72);
            this.labelMean.Name = "labelMean";
            this.labelMean.Size = new System.Drawing.Size(72, 16);
            this.labelMean.TabIndex = 17;
            this.labelMean.Text = "0";
            this.labelMean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRange
            // 
            this.labelRange.Location = new System.Drawing.Point(224, 56);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(96, 16);
            this.labelRange.TabIndex = 15;
            this.labelRange.Text = "0 ... 0";
            this.labelRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPeak
            // 
            this.labelPeak.Location = new System.Drawing.Point(224, 40);
            this.labelPeak.Name = "labelPeak";
            this.labelPeak.Size = new System.Drawing.Size(72, 16);
            this.labelPeak.TabIndex = 14;
            this.labelPeak.Text = "0";
            this.labelPeak.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(128, 88);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(72, 16);
            this.label24.TabIndex = 13;
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(128, 72);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(72, 16);
            this.label25.TabIndex = 12;
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRangeX
            // 
            this.labelRangeX.Location = new System.Drawing.Point(128, 56);
            this.labelRangeX.Name = "labelRangeX";
            this.labelRangeX.Size = new System.Drawing.Size(72, 16);
            this.labelRangeX.TabIndex = 9;
            this.labelRangeX.Text = "0 ... 0";
            this.labelRangeX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPeakX
            // 
            this.labelPeakX.Location = new System.Drawing.Point(128, 40);
            this.labelPeakX.Name = "labelPeakX";
            this.labelPeakX.Size = new System.Drawing.Size(72, 16);
            this.labelPeakX.TabIndex = 8;
            this.labelPeakX.Text = "0";
            this.labelPeakX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(16, 88);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(88, 16);
            this.label23.TabIndex = 7;
            this.label23.Text = "Deviation:";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(16, 72);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(88, 16);
            this.label22.TabIndex = 6;
            this.label22.Text = "Mean:";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(16, 56);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 16);
            this.label19.TabIndex = 3;
            this.label19.Text = "Range:";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(16, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 16);
            this.label18.TabIndex = 2;
            this.label18.Text = "Peak:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(224, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 16);
            this.label17.TabIndex = 1;
            this.label17.Text = "Gray Values";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(128, 16);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 16);
            this.label16.TabIndex = 0;
            this.label16.Text = "x-Value";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.CircArcButton,
                                                                                    this.LineButton,
                                                                                    this.DeleteActRoiButton,
                                                                                    this.ResetROIButton});
            this.groupBox2.Location = new System.Drawing.Point(8, 528);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(512, 48);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ROIs";
            // 
            // CircArcButton
            // 
            this.CircArcButton.Location = new System.Drawing.Point(128, 16);
            this.CircArcButton.Name = "CircArcButton";
            this.CircArcButton.Size = new System.Drawing.Size(108, 24);
            this.CircArcButton.TabIndex = 10;
            this.CircArcButton.Text = "Create Circular Arc";
            this.CircArcButton.Click += new System.EventHandler(this.CircArcButton_Click);
            // 
            // LineButton
            // 
            this.LineButton.Location = new System.Drawing.Point(16, 16);
            this.LineButton.Name = "LineButton";
            this.LineButton.Size = new System.Drawing.Size(100, 24);
            this.LineButton.TabIndex = 1;
            this.LineButton.Text = "Create Line";
            this.LineButton.Click += new System.EventHandler(this.LineButton_Click);
            // 
            // DeleteActRoiButton
            // 
            this.DeleteActRoiButton.Location = new System.Drawing.Point(304, 16);
            this.DeleteActRoiButton.Name = "DeleteActRoiButton";
            this.DeleteActRoiButton.Size = new System.Drawing.Size(96, 24);
            this.DeleteActRoiButton.TabIndex = 2;
            this.DeleteActRoiButton.Text = "Delete";
            this.DeleteActRoiButton.Click += new System.EventHandler(this.DeleteActRoiButton_Click);
            // 
            // ResetROIButton
            // 
            this.ResetROIButton.Location = new System.Drawing.Point(408, 16);
            this.ResetROIButton.Name = "ResetROIButton";
            this.ResetROIButton.Size = new System.Drawing.Size(88, 24);
            this.ResetROIButton.TabIndex = 3;
            this.ResetROIButton.Text = "Delete All";
            this.ResetROIButton.Click += new System.EventHandler(this.ResetROIButton_Click);
            // 
            // LoadImgButton
            // 
            this.LoadImgButton.Location = new System.Drawing.Point(56, 12);
            this.LoadImgButton.Name = "LoadImgButton";
            this.LoadImgButton.Size = new System.Drawing.Size(112, 26);
            this.LoadImgButton.TabIndex = 4;
            this.LoadImgButton.Text = "Load ...";
            this.LoadImgButton.Click += new System.EventHandler(this.LoadImgButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.ResetViewButton,
                                                                                    this.MagnifyButton,
                                                                                    this.ZoomButton,
                                                                                    this.MoveButton,
                                                                                    this.NoneButton});
            this.groupBox1.Location = new System.Drawing.Point(199, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(441, 45);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Interaction";
            // 
            // ResetViewButton
            // 
            this.ResetViewButton.Location = new System.Drawing.Point(336, 12);
            this.ResetViewButton.Name = "ResetViewButton";
            this.ResetViewButton.Size = new System.Drawing.Size(92, 26);
            this.ResetViewButton.TabIndex = 10;
            this.ResetViewButton.Text = "Reset ";
            this.ResetViewButton.Click += new System.EventHandler(this.ResetViewButton_Click);
            // 
            // MagnifyButton
            // 
            this.MagnifyButton.Location = new System.Drawing.Point(248, 19);
            this.MagnifyButton.Name = "MagnifyButton";
            this.MagnifyButton.Size = new System.Drawing.Size(64, 16);
            this.MagnifyButton.TabIndex = 4;
            this.MagnifyButton.Text = "magnify";
            this.MagnifyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MagnifyButton.CheckedChanged += new System.EventHandler(this.MagnifyButton_CheckedChanged);
            // 
            // ZoomButton
            // 
            this.ZoomButton.Location = new System.Drawing.Point(182, 19);
            this.ZoomButton.Name = "ZoomButton";
            this.ZoomButton.Size = new System.Drawing.Size(56, 16);
            this.ZoomButton.TabIndex = 2;
            this.ZoomButton.Text = "zoom";
            this.ZoomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ZoomButton.CheckedChanged += new System.EventHandler(this.ZoomButton_CheckedChanged);
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(104, 19);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(56, 16);
            this.MoveButton.TabIndex = 1;
            this.MoveButton.Text = "move";
            this.MoveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MoveButton.CheckedChanged += new System.EventHandler(this.MoveButton_CheckedChanged);
            // 
            // NoneButton
            // 
            this.NoneButton.Checked = true;
            this.NoneButton.Location = new System.Drawing.Point(24, 19);
            this.NoneButton.Name = "NoneButton";
            this.NoneButton.Size = new System.Drawing.Size(56, 16);
            this.NoneButton.TabIndex = 0;
            this.NoneButton.TabStop = true;
            this.NoneButton.Text = "none";
            this.NoneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.NoneButton.CheckedChanged += new System.EventHandler(this.NoneButton_CheckedChanged);
            // 
            // openImageFileDialog
            // 
            this.openImageFileDialog.Filter = "png (*.png)|*.png|tiff (*.tif)|*.tif|jpeg (*.jpg)| *.jpg|all files (*.*)|*.*";
            this.openImageFileDialog.RestoreDirectory = true;
            // 
            // StatusLabel
            // 
            this.StatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusLabel.Location = new System.Drawing.Point(652, 536);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(356, 32);
            this.StatusLabel.TabIndex = 9;
            this.StatusLabel.Text = "Status";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // openCamparFileDialog
            // 
            this.openCamparFileDialog.Filter = "camera parameters (*.cal)|*.cal|camera parameters (*.dat)|*dat|all files (*.*)|*." +
                "*";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                                    this.LoadImgButton});
            this.groupBox3.Location = new System.Drawing.Point(8, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(176, 45);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Image";
            // 
            // MeasureForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(1014, 583);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                          this.groupBox3,
                                                                          this.StatusLabel,
                                                                          this.groupBox1,
                                                                          this.groupBox2,
                                                                          this.tabControl,
                                                                          this.viewPort});
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MeasureForm";
            this.Text = "Measure Assistant";
            this.Load += new System.EventHandler(this.MeasureForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageEdges.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LineWidthDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EdgeLengthUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ROIWidthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothingTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinEdgAmplTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinEdgAmplUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothingUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ROIWidthUpDown)).EndInit();
            this.tabPageResults.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.UnitPanel.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.tabPageLineProfile.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Run(new MeasureForm());
		}

		/// <summary>
		/// Loads MeasureForm and initializes view functions
		/// to include in the MeasureAssistant.
		/// </summary>
		private void MeasureForm_Load(object sender, System.EventArgs e)
		{
			mView = new HWndCtrl(viewPort);
			roiController = new ROIController();
			mView.useROIController(roiController);
			mAssistant = new MeasureAssistant(roiController);
			mShadow = new HXLDCont();

			mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
			mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 1);

			roiController.NotifyRCObserver = new IconicDelegate(UpdateROIData);
			mAssistant.NotifyMeasureObserver = new MeasureDelegate(UpdateMeasureResults);

			plotGraphWindow = new FunctionPlot(panelAxis, true);

      openImageFileDialog.InitialDirectory =
        (string)(HSystem.GetSystem("image_dir").TupleSplit(";"));
			Init();

			mView.setDispLevel(HWndCtrl.MODE_EXCLUDE_ROI);
		}


		/// <summary>
		/// Initializes the parameters for the edges measurement and
		/// sets up the measure assistant.
		/// </summary>
		private void Init()
		{
			MinEdgAmplUpDown.Value = 40;
			SmoothingUpDown.Value = 10;
			ROIWidthUpDown.Value = 10;
			InterpolationComboBox.SelectedIndex = 0;
			EdgeToPaircheckBox.Checked = false;
			TransitionComboBox.SelectedIndex = 0;
			PositionComboBox.SelectedIndex = 0;
			EdgeLengthUpDown.Value = 30;
			UseROIWidthCheckBox.Checked = true;
			ShowRegionCheckBox.Checked = true;
			ShowROIcheckBox.Checked = true;
			UseShadowsCheckBox.Checked = false;
			ResetMinEdgAmplButton.ForeColor = System.Drawing.Color.Gray;
			ResetROIWidthButton.ForeColor = System.Drawing.Color.Gray;
			ResetSmoothingButton.ForeColor = System.Drawing.Color.Gray;
			
			PositionCheckBox.Checked = true;
			PairWidthCheckBox.Checked = true;
			AmplitudeCheckBox.Checked = true;
			DistanceCheckBox.Checked = true;
			TransWCoordCheckBox.Enabled = false;
			TransWCoordCheckBox.Checked = false;
			UnitComboBox.SelectedIndex = 1;
			UnitPanel.Enabled = false;

			mAssistant.mThresh = 40.0;
			mAssistant.mSigma = 1.0;
			mAssistant.mRoiWidth = 10;
			mAssistant.mInterpolation = "nearest_neighbor";
			mAssistant.mSelPair = false;
			mAssistant.mTransition = "all";
			mAssistant.mPosition = "all";
			mAssistant.mDispEdgeLength = 30;
			mAssistant.mDispROIWidth = true;
			mAssistant.setUnit(UnitComboBox.Text);

			mAssistant.mInitThresh = 40.0;
			mAssistant.mInitSigma = 1.0;
			mAssistant.mInitRoiWidth = 10;

			RegionColorComboBox.SelectedIndex = 2;
			EdgeColorComboBox.SelectedIndex = 7;
			LineWidthDown.Value = 1;

			CSScaleComboBox.SelectedIndex = 0;

			regionColor = "blue";
			edgeColor = "cyan";
			lineWidth = 1;
			useShadow = false;
		}



		/********************************************************************/
		/*                   Display Parameters                             */
		/********************************************************************/

		/// <summary>
		/// Loads image and passes it to the controller class. Only images of 
		/// type "byte" are allowed.
		/// </summary>
		private void LoadImgButton_Click(object sender, System.EventArgs e)
		{
			string file,info;

			int dispROI = HWndCtrl.MODE_INCLUDE_ROI;


			if (openImageFileDialog.ShowDialog() == DialogResult.OK)
			{
				file = openImageFileDialog.FileName;

				roiController.reset();
				mView.resetWindow();

				if (mAssistant.setImage(file))
				{
					currImage = mAssistant.getImage();
					info = currImage.GetChannelInfo("type", 1);
				}
				else
				{
					info = "wrong file type";
				}

				if (info != "byte")
				{
					currImage = null;
					mAssistant.setImage(currImage);
					dispROI = HWndCtrl.MODE_EXCLUDE_ROI;

					MessageBox.Show("Problem occured: The Image is of type \"" + info + "\".\nOnly byte images are supported by this Measure Assistant!",
									"Measure assistant",
									 MessageBoxButtons.OK,
									 MessageBoxIcon.Information);

				}
				mView.setDispLevel(dispROI);
				UpdateView();
			}
		}



		/* *******************************************************
		 *                  Window Interaction 
		 * *******************************************************/


		private void NoneButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
		}


		private void MoveButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_MOVE);
		}

		private void ZoomButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
		}

		private void MagnifyButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_ZOOMWINDOW);
		}

		private void ResetViewButton_Click(object sender, System.EventArgs e)
		{
			mView.resetWindow();
			UpdateView();
		}


		/* *******************************************************
		 *                  ROI Interaction 
		 * *******************************************************/


		/// <summary>
		/// Creates linear ROI.
		/// </summary>
		private void LineButton_Click(object sender, System.EventArgs e)
		{
			if (ShowROIcheckBox.Checked)
				roiController.setROIShape(new ROILine());
		}

		/// <summary>
		/// Creates circular ROI.
		/// </summary>
		private void CircArcButton_Click(object sender, System.EventArgs e)
		{
			if (ShowROIcheckBox.Checked)
				roiController.setROIShape(new ROICircularArc());
		}

		/// <summary>Deletes selected ROI.</summary>
		private void DeleteActRoiButton_Click(object sender, System.EventArgs e)
		{
			roiController.removeActive();
		}

		/// <summary>Deletes all ROIs.</summary>
		private void ResetROIButton_Click(object sender, System.EventArgs e)
		{
			roiController.reset();
			UpdateView();
		}

        /********************************************************************/
		/*                         Edges Tab                                */
		/********************************************************************/

		/*                        Edge Extraction                           */

		private void ROIWidthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)ROIWidthUpDown.Value;
			ROIWidthTrackBar.Value = val;

			setRoiWidth(val);
		}

		private void ROIWidthTrackBar_Scroll(object sender, System.EventArgs e)
		{
			ROIWidthUpDown.Value = ROIWidthTrackBar.Value;
			ROIWidthUpDown.Refresh();
		}

		private void ResetROIWidthButton_Click(object sender, System.EventArgs e)
		{
			ROIWidthUpDown.Value = (int)mAssistant.mInitRoiWidth;
			ResetROIWidthButton.ForeColor = System.Drawing.Color.Gray;
		}

		private void setRoiWidth(int val)
		{
			ResetROIWidthButton.ForeColor = System.Drawing.Color.Black;
			mAssistant.setRoiWidth((double)val);
		}


		/********************************************************************/
		private void SmoothingUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)SmoothingUpDown.Value;
			SmoothingTrackBar.Value = val;

			setSigma(val);
		}

		private void SmoothingTrackBar_Scroll(object sender, System.EventArgs e)
		{
			SmoothingUpDown.Value = SmoothingTrackBar.Value;
			SmoothingUpDown.Refresh();
		}

		private void ResetSmoothingButton_Click(object sender, System.EventArgs e)
		{
			SmoothingUpDown.Value = (int)mAssistant.mInitSigma * 10;
			ResetSmoothingButton.ForeColor = System.Drawing.Color.Gray;
		}

		private void setSigma(int val)
		{
			double valD = (double)val / 10.0;

			ResetSmoothingButton.ForeColor = System.Drawing.Color.Black;
			mAssistant.setSigma(valD);
		}

		/********************************************************************/
		private void MinEdgAmplUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)MinEdgAmplUpDown.Value;
			MinEdgAmplTrackBar.Value = val;

			setMinEdgeAmpl(val);
		}

		private void MinEdgAmplTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MinEdgAmplUpDown.Value = MinEdgAmplTrackBar.Value;
			MinEdgAmplUpDown.Refresh();
		}

		private void ResetMinEdgAmplButton_Click(object sender, System.EventArgs e)
		{
			MinEdgAmplUpDown.Value = (int)mAssistant.mInitThresh;
			ResetMinEdgAmplButton.ForeColor = System.Drawing.Color.Gray;
		}

		private void setMinEdgeAmpl(int val)
		{
			ResetMinEdgAmplButton.ForeColor = System.Drawing.Color.Black;
			mAssistant.setMinEdgeAmpl((double)val);
		}


		/********************************************************************/
		private void InterpolationComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mAssistant.setInterpolation((string)InterpolationComboBox.Text);
		}


		/********************************************************************/
		/********************************************************************/
		/*                     Define Edge Selection                        */

		private void EdgeToPaircheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.setSelPair(EdgeToPaircheckBox.Checked);
			UpdateMeasureResultComposition();

			if (EdgeToPaircheckBox.Checked)
			{
				PairWidthCheckBox.Enabled = true;
				TransitionComboBox.Items.AddRange(new object[] {"all_strongest",
                                                                "positive_strongest",
                                                                "negative_strongest"});


			}
			else
			{
				PairWidthCheckBox.Enabled = false;
				TransitionComboBox.Items.Clear();
				TransitionComboBox.Items.AddRange(new object[] { "all", "positive", "negative" });
				TransitionComboBox.SelectedIndex = 0;
			}
		}

		private void TransitionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mAssistant.setTransition((string)TransitionComboBox.Text);
		}

		private void PositionComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mAssistant.setPosition((string)PositionComboBox.Text);
		}

		/********************************************************************/
		/********************************************************************/
		/*                 Define Edge Display Parameters                   */

		private void RegionColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			regionColor = (string)RegionColorComboBox.SelectedItem;
			UpdateView();
		}

		private void EdgeColorComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			edgeColor = (string)EdgeColorComboBox.SelectedItem;
			UpdateView();
		}

		private void LineWidthDown_ValueChanged(object sender, System.EventArgs e)
		{
			lineWidth = (int)LineWidthDown.Value;
			UpdateView();
		}

		private void ShowRegionCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			displayRegion = ShowRegionCheckBox.Checked;
			UpdateView();
		}

		private void UseShadowsCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			useShadow = UseShadowsCheckBox.Checked;
			UpdateView();
		}

		/********************************************************************/
		/********************************************************************/
		/*              Definition for Result EdgeXLD Format                */

		private void EdgeLengthUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			mAssistant.setDispEdgeLength((int)EdgeLengthUpDown.Value);
		}

		private void UseROIWidthCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.setFlagDispROIWidth(UseROIWidthCheckBox.Checked);

			if (UseROIWidthCheckBox.Checked)
				EdgeLengthUpDown.Enabled = false;
			else
				EdgeLengthUpDown.Enabled = true;

		}

		private void ShowROIcheckBox_CheckedChanged(object sender, System.EventArgs e)
		{

			int mode;

			if (ShowROIcheckBox.Checked)
				mode = HWndCtrl.MODE_INCLUDE_ROI;
			else
				mode = HWndCtrl.MODE_EXCLUDE_ROI;

			mView.setDispLevel(mode);
			UpdateView();
		}

		/********************************************************************/
		/*                        Edges Results                             */
		/********************************************************************/

		private void PositionCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.showPosition(PositionCheckBox.Checked);
			UpdateMeasureResultComposition();
		}

		private void PairWidthCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.showPairWidth(PairWidthCheckBox.Checked);
			UpdateMeasureResultComposition();
		}

		private void AmplitudeCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.showAmplitude(AmplitudeCheckBox.Checked);
			UpdateMeasureResultComposition();
		}

		private void DistanceCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			mAssistant.showDistance(DistanceCheckBox.Checked);
			UpdateMeasureResultComposition();
		}

		private void TransWCoordCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (TransWCoordCheckBox.Checked)
				UnitPanel.Enabled = true;
			else
				UnitPanel.Enabled = false;

			mAssistant.setTransWorldCoord(TransWCoordCheckBox.Checked);

		}

		private void UnitComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mAssistant.setUnit((string)UnitComboBox.SelectedItem);
		}

		/// <summary>Loads HALCON calibration data.</summary>
		private void LoadCamParamButton_Click(object sender, System.EventArgs e)
		{
			HTuple buttonType, valType;
			string file;
			string [] val;
			bool   isCalFile;


			StatusLabel.Text = "";
			buttonType = new HTuple(sender.ToString());
            valType = buttonType.TupleStrrstr(new HTuple("CamPar"));

            if (valType[0].I > 0)
                openCamparFileDialog.FilterIndex = 1;
            else
                openCamparFileDialog.FilterIndex = 2;

            openCamparFileDialog.FileName = "";

			if (openCamparFileDialog.ShowDialog() == DialogResult.OK)
			{
				file = openCamparFileDialog.FileName;
				StatusLabel.Text = "";

				if ((isCalFile = file.EndsWith(".cal")) || file.EndsWith(".dat"))
				{
					
					try
					{
						if (valType[0].I > 0)
						{
							mAssistant.LoadCamParFile(file);
							val = file.Split(new Char[] { '\\' });
							file = val[val.Length - 1];
							CalibCamTextBox.Text = file;

						}
						else
						{
							mAssistant.LoadCamPoseFile(file);
							val = file.Split(new Char[] { '\\' });
							file = val[val.Length - 1];
							CalibPoseTextBox.Text = file;
						}
					}
					catch (HOperatorException)
					{
						if (valType[0].I > 0)
							CalibCamTextBox.Text = "*.cal";
						else
							CalibPoseTextBox.Text = "*.dat";

						StatusLabel.Text = mAssistant.exceptionText;
						valType = mAssistant.exceptionText.TupleSplit(":");
						MessageBox.Show("File is corrupted or has a wrong format:\nPlease check if you chose a valid calibration file (or format)!",
										 "Measure Assistant",
										 MessageBoxButtons.OK,
										 MessageBoxIcon.Information);

					}

				}
				else
				{
					MessageBox.Show("Fileformat is wrong: Data is not a calibration file!",
									 "Measure Assistant",
									 MessageBoxButtons.OK,
									 MessageBoxIcon.Information);
				}


				if (mAssistant.mIsCalibValid)
				{
					TransWCoordCheckBox.Enabled = true;
				}
				else
				{
					TransWCoordCheckBox.Enabled = false;
					TransWCoordCheckBox.Checked = false;
				}
			}
		}

		/// <summary>
		/// Changes index of the selected ROI displayed in the listbox. 
		/// </summary>
		private void ActiveROIListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			roiController.activeROIidx = ActiveROIListBox.SelectedIndex;
			mAssistant.mActRoiIdx = roiController.activeROIidx;
			UpdateCodeTable();
			UpdateView();
		}


		/********************************************************************/
		/*             Update methods invoked by delegates                  */
		/********************************************************************/

		/// <summary>
		/// Updates HALCON display and result table according to new measure 
		/// results.
		/// </summary>
		/// <param name="mode">Type of measure update</param>
		public void UpdateMeasureResults(int mode)
		{
			StatusLabel.Text = "";

			switch (mode)
			{
				case MeasureAssistant.ERR_READING_FILE:
					MessageBox.Show("Problem occured while reading file! \n" + mAssistant.exceptionText,
						"Measure assistant",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information);
					mAssistant.exceptionText = "";
					break;
				case MeasureAssistant.EVENT_UPDATE_MEASUREMENT:
				case MeasureAssistant.EVENT_UPDATE_RESULT_XLD:
				case MeasureAssistant.EVENT_UPDATE_REMOVE:
					UpdateView();
					break;
				default:
					break;
			}

			if (mAssistant.exceptionText != "")
			{
				HTuple segment = mAssistant.exceptionText.TupleSplit(":");
				StatusLabel.Text = " Measure failed: " + mAssistant.exceptionText;
			}
			UpdateCodeTable();
		}


		/// <summary>
		/// Responds to changes in the selected ROI.
		/// </summary>
		/// <param name="mode">Type of ROI update</param>
		public void UpdateROIData(int mode)
		{
			switch (mode)
			{
				case ROIController.EVENT_CREATED_ROI:
					mAssistant.AddMeasureObject();
					UpdateROIClickList("new");
					break;
				case ROIController.EVENT_ACTIVATED_ROI:
					mAssistant.ClickedActiveROI();
					UpdateROIClickList("active");
					break;
				case ROIController.EVENT_MOVING_ROI:
					mAssistant.UpdateMeasure(roiController.getActiveROIIdx());
					break;
				case ROIController.EVENT_DELETED_ACTROI:
					mAssistant.RemoveMeasureObjectActIdx();
					UpdateROIClickList("removeIdx");
					break;
				case ROIController.EVENT_DELETED_ALL_ROIS:
					mAssistant.RemoveAllMeasureObjects();
					UpdateROIClickList("removeAll");
					break;
			}

			if (updateLineProfile)
				PaintGraph();

		}


		/// <summary>
		/// Triggers an update of the HALCON window. Displays all valid iconic 
		/// objects.
		/// </summary>
		public void UpdateView()
		{
			HObject edges, regions;

			if (currImage == null)
			{
				mView.clearList();
			}
			else
			{
				mView.addIconicVar(currImage);

				if (roiController.getROIList().Count > 0)
				{
					edges = mAssistant.getMeasureResults();

					if (edges.IsInitialized())
					{
						if (useShadow)
						{
							UpdateShadowContours(edges);
							mView.changeGraphicSettings(GraphicsContext.GC_COLOR, "black");
							mView.addIconicVar(mShadow);
						}

                        mView.changeGraphicSettings(GraphicsContext.GC_COLOR, edgeColor);
                        mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, lineWidth);
                        mView.addIconicVar(edges);
					}

					regions = mAssistant.getMeasureRegions();

					if (displayRegion && regions.IsInitialized())
					{
						mView.changeGraphicSettings(GraphicsContext.GC_COLOR, regionColor);
						mView.addIconicVar(regions);
					}
				}
			}
			mView.repaint();
		}


		/// <summary>
		/// Adds visual effects (shadows)
		/// to the measure results.
		/// </summary>
		/// <param name="edges">
		/// Add shadows along the edges provided
		/// </param>
		public void UpdateShadowContours(HObject edges)
		{
			double shift;
			HXLDCont shadow1, shadow2;
			HHomMat2D hom2D = new HHomMat2D();

			mShadow.Dispose();
			mShadow.GenEmptyObj();

			shift = Math.Min(0.5 * lineWidth, 2.0);

			hom2D.HomMat2dIdentity();
			hom2D = hom2D.HomMat2dTranslate(shift, 1);
			shadow1 = ((HXLDCont)edges).AffineTransContourXld(hom2D);

			hom2D.HomMat2dIdentity();
			hom2D = hom2D.HomMat2dTranslate(1, shift);
			shadow2 = ((HXLDCont)edges).AffineTransContourXld(hom2D);

			mShadow = mShadow.ConcatObj(shadow1);
			mShadow = mShadow.ConcatObj(shadow2);

			shadow1.Dispose();
			shadow2.Dispose();
		}


		/// <summary>
		/// Adapts column header for measure result table. 
		/// </summary>
		public void UpdateMeasureResultComposition()
		{
			ArrayList composition;
			ColumnHeader  header;
			int length;
			int size = -1;

			composition = mAssistant.getMeasureResultComposition();
			length = composition.Count;
			EdgeResultListView.Columns.Clear();

			if (length <= 5 && length > 0)
				size = (EdgeResultListView.Size.Width / length) - 1;

			for (int i=0; i < length; i++)
			{
				header = new ColumnHeader();
				header.Text = (string)composition[i];
				header.Width = (size != -1) ? size : 20 + header.Text.Length * 5;
				EdgeResultListView.Columns.Add(header);
			}

			UpdateCodeTable();
		}


		/// <summary>Creates table for measure results.</summary>
		public void UpdateCodeTable()
		{
			ArrayList table;
			ListViewItem item;
			int rowCount,colCount;
			string val;
			rowCount = 0;

			table = mAssistant.getMeasureTableData();
			EdgeResultListView.Items.Clear();

			if ((colCount = table.Count) == 0 || (table[0] == null))
				return;

			for (int i=0; i < colCount; i++)
				rowCount = Math.Max(rowCount, ((HTuple)table[i]).Length);

			for (int i=0; i < rowCount; i++)
			{
				val = (i >= ((HTuple)table[0]).Length) ? "" : ((HTuple)table[0])[i].D.ToString("f6");
				item = new ListViewItem(val);

				for (int j=1; j < colCount; j++)
				{
					val = (i >= ((HTuple)table[j]).Length) ? "" : ((HTuple)table[j])[i].D.ToString("f6");
					item.SubItems.Add(val);
				}
				EdgeResultListView.Items.Add(item);
			}
		}


		/// <summary>
		/// Updates listbox displaying current set of ROIs.
		/// </summary>
		public void UpdateROIClickList(string mode)
		{
			int count = roiController.ROIList.Count;

			switch (mode)
			{
				case "active":
					ActiveROIListBox.SelectedIndex = mAssistant.mActRoiIdx;
					break;
				case "new":
					ActiveROIListBox.Items.Add(" InterActive ROI " + (mAssistant.mActRoiIdx + 1).ToString("d2"));
					ActiveROIListBox.SelectedIndex = mAssistant.mActRoiIdx;
					break;
				case "removeIdx":
					ActiveROIListBox.Items.Clear();
					for (int i=1; i <= count; i++)
						ActiveROIListBox.Items.Add(" InterActive ROI " + i.ToString("d2"));
					break;
				case "removeAll":
					ActiveROIListBox.Items.Clear();
					break;
			}
		}


		/// <summary>
		/// Updates function plot (line profile) of measure projection for the 
		/// selected ROI.
		/// </summary>
		public void PaintGraph()
		{
			double [] grayVals;

			grayVals = mAssistant.getMeasureProjection();
			plotGraphWindow.plotFunction(grayVals);
			ComputeStatistics(grayVals);
		}


		/// <summary>Adjusts statistics of measure projection (line profile).</summary>
		public void ComputeStatistics(double[] grayVals)
		{
			HTuple tuple, val;
			int max =0;

			if (grayVals != null)
			{
				tuple = new HTuple(grayVals);

				val = tuple.TupleMean();
				labelMean.Text = val[0].D.ToString("f2");
				val = tuple.TupleDeviation();
				labelDeviation.Text = val[0].D.ToString("f2");

				val = tuple.TupleSortIndex();
				labelPeakX.Text = val[val.Length - 1].I + "";
				max = (int)tuple[val[val.Length - 1].I].D;
				labelPeak.Text = max + "";

				labelRange.Text = (int)tuple[0].D + " ... " + (int)tuple[tuple.Length - 1].D;
				labelRangeX.Text = "0 ... " + (tuple.Length - 1);
			}
			else
			{
				labelMean.Text = "0";
				labelDeviation.Text = "0";

				labelPeakX.Text = "0";
				labelPeak.Text = "0";

				labelRange.Text = "0 ... 0";
				labelRangeX.Text = "0 ... 0";
			}
		}

		/// <summary>Activates update for function plot (line profile).</summary>
		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int tab = tabControl.SelectedIndex;
			updateLineProfile = false;

			if (tab == 2)
			{
				updateLineProfile = true;
				PaintGraph();
			}
		}


		/// <summary>
		/// Adapts mode for scaling the y-axis in the plot (line profile) window.
		/// </summary>
		private void CSScaleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (CSScaleComboBox.SelectedIndex)
			{
				case 0: // Adaptive
					plotGraphWindow.setAxisAdaption(FunctionPlot.AXIS_RANGE_ADAPTING);
					break;
				case 1: // Increasing
					plotGraphWindow.setAxisAdaption(FunctionPlot.AXIS_RANGE_INCREASING);
					break;
				case 2: // Fixed
					plotGraphWindow.setAxisAdaption(FunctionPlot.AXIS_RANGE_FIXED, 255.0f);
					break;
			}
			PaintGraph();
		}

	}//end of class
}//end of namespace

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using HalconDotNet;
using ViewROI;
using MatchingModule;



namespace MatchingMain
{
    /// <summary>
    /// This project provides you with the functionality of the HDevelop 
    /// Matching Assistant, as a stand alone application. It contains 
    /// most of the data structures and menus that are offered with the
    /// HDevelop Assistant. 
    /// It uses the ROI classes to create interactive ROI objects,
    /// as introduced with the project InteractiveROI. 
    /// MatchingForm provides the graphical user interface, i.e., it contains
    /// all display handles, GUI components, and event handles. It uses the
    /// ROI class to create interactive ROI objects (see also the project
    /// InteractiveROI). The class Matching Assistant is used as the
    /// controller, which connects the GUI instance with the models. Changes
    /// in the model are forwarded to the GUI by event delegates.
    /// </summary>    
    public class MatchingForm : System.Windows.Forms.Form
	{		
		//  Main Panel 
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageCreateModel;
		private System.Windows.Forms.TabPage tabPageFindModel;
		private System.Windows.Forms.TabPage tabPageInspectModel;
		private System.Windows.Forms.Button loadImagebutton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button saveModelbutton;
		private System.Windows.Forms.RadioButton addToROIButton;
		private System.Windows.Forms.RadioButton subFromROIButton;
		private System.Windows.Forms.Button rect1Button;
		private System.Windows.Forms.Button rect2Button;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton moveButton;
		private System.Windows.Forms.RadioButton zoomButton;
		private System.ComponentModel.IContainer components;
		
		
        /// <summary>HALCON window</summary>
		private HalconDotNet.HWindowControl viewPort;
		/// <summary>Window control that manages visualization</summary>
		public  HWndCtrl					mView;
        /// <summary>ROI control that manages ROI objects</summary>
		public  ROIController				roiController;

		private HImage						CurrentImg;
		private HRegion						ModelRegion;
		private	HXLD						ModelContour;
		private HXLD						DetectionContour;
		private	MatchingAssistant			mAssistant;
		private MatchingParam				parameterSet;
		private	MatchingOpt					optInstance;
		private	MatchingOptSpeed			speedOptHandler;
		private MatchingOptStatistics		inspectOptHandler;
        private Color                       createModelWindowMode;
        private Color                       trainModelWindowMode;


		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Button loadTestImgButton;
		private System.Windows.Forms.Button deleteTestImgButton;
		private System.Windows.Forms.Button deleteAllTestImgButton;
		private System.Windows.Forms.Button findModelButton;
		private System.Windows.Forms.Button displayTestImgButton;
		private System.Windows.Forms.TrackBar MinScoreTrackBar;
		private System.Windows.Forms.NumericUpDown MinScoreUpDown;
		private System.Windows.Forms.TrackBar NumMatchesTrackBar;
		private System.Windows.Forms.NumericUpDown NumMatchesUpDown;
		private System.Windows.Forms.TrackBar MaxOverlapTrackBar;
		private System.Windows.Forms.NumericUpDown MaxOverlapUpDown;
		private System.Windows.Forms.TrackBar GreedinessTrackBar;
		private System.Windows.Forms.NumericUpDown GreedinessUpDown;
		private System.Windows.Forms.ComboBox SubPixelBox;
		private System.Windows.Forms.TrackBar LastPyrLevTrackBar;
		private System.Windows.Forms.NumericUpDown LastPyrLevUpDown;
		private System.Windows.Forms.ListBox testImgListBox;
		private System.Windows.Forms.TabPage tabPageOptimRecognition;
		private System.Windows.Forms.CheckBox FindAlwaysCheckBox;
		private System.Windows.Forms.Button DisplayDataButton;
		private System.Windows.Forms.GroupBox groupBoxCreateROI;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.RadioButton FindNoOfInstButton;
		private System.Windows.Forms.RadioButton FindAtLeastOneModelButton;
		private System.Windows.Forms.RadioButton FindMaxNoOfModelButton;
		private System.Windows.Forms.TrackBar recogRateTrackBar;
		private System.Windows.Forms.NumericUpDown recogRateUpDown;
		private System.Windows.Forms.ComboBox RecognRateComboBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelRecogn21;
		private System.Windows.Forms.Label labelRecogn24;
		private System.Windows.Forms.Label labelRecogn14;
		private System.Windows.Forms.Label labelRecogn23;
		private System.Windows.Forms.Label labelRecogn13;
		private System.Windows.Forms.Label labelRecogn22;
		private System.Windows.Forms.Label labelRecogn12;
		private System.Windows.Forms.Label labelRecogn11;
		private System.Windows.Forms.Label labelOptStatus;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Button StatisticsButton;
		private System.Windows.Forms.NumericUpDown RecogNoManSelUpDown;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.Label label62;
		private System.Windows.Forms.Label label63;
		private System.Windows.Forms.Label label64;
		private System.Windows.Forms.Label label65;
		private System.Windows.Forms.Label label66;
		private System.Windows.Forms.Label label67;
		private System.Windows.Forms.Label label68;
		private System.Windows.Forms.Label label69;
		private System.Windows.Forms.Label label70;
		private System.Windows.Forms.Label label71;
		private System.Windows.Forms.Label label72;
		private System.Windows.Forms.Label labelInspect11;
		private System.Windows.Forms.Label labelInspect21;
		private System.Windows.Forms.Label labelInspect31;
		private System.Windows.Forms.Label labelInspect32;
		private System.Windows.Forms.Label labelInspect22;
		private System.Windows.Forms.Label labelInspect12;
		private System.Windows.Forms.Label labelInspect33;
		private System.Windows.Forms.Label labelInspect23;
		private System.Windows.Forms.Label labelInspect13;
		private System.Windows.Forms.Label labelInspect34;
		private System.Windows.Forms.Label labelInspect24;
		private System.Windows.Forms.Label labelInspect14;
		private System.Windows.Forms.Label labelInspect35;
		private System.Windows.Forms.Label labelInspect25;
		private System.Windows.Forms.Label labelInspect15;
		private System.Windows.Forms.Label labelInspect36;
		private System.Windows.Forms.Label labelInspect26;
		private System.Windows.Forms.Label labelInspect16;
		private System.Windows.Forms.Label labelInspect37;
		private System.Windows.Forms.Label labelInspect27;
		private System.Windows.Forms.Label labelInspect17;
		private System.Windows.Forms.Label labelInspect05;
		private System.Windows.Forms.Label labelInspect04;
		private System.Windows.Forms.Label labelInspect03;
		private System.Windows.Forms.Label labelInspect02;
		private System.Windows.Forms.Label labelInspect01;
		private System.Windows.Forms.NumericUpDown InspectMaxNoMatchUpDown;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Button OptimizeButton;
		private System.Windows.Forms.Button loadModelbutton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog3;
		private System.Windows.Forms.GroupBox groupBoxCreateModel;
		private System.Windows.Forms.NumericUpDown DispPyramidUpDown;
		private System.Windows.Forms.TrackBar DispPyramidTrackBar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ContrastAutoButton;
		private System.Windows.Forms.NumericUpDown ContrastUpDown;
		private System.Windows.Forms.TrackBar ContrastTrackBar;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown MinScaleUpDown;
		private System.Windows.Forms.TrackBar MinScaleTrackBar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown MaxScaleUpDown;
		private System.Windows.Forms.TrackBar MaxScaleTrackBar;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button ScaleStepAutoButton;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.NumericUpDown ScaleStepUpDown;
		private System.Windows.Forms.TrackBar ScaleStepTrackBar;
		private System.Windows.Forms.TrackBar StartingAngleTrackBar;
		private System.Windows.Forms.NumericUpDown StartingAngleUpDown;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TrackBar AngleExtentTrackBar;
		private System.Windows.Forms.NumericUpDown AngleExtentUpDown;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TrackBar AngleStepTrackBar;
		private System.Windows.Forms.Button AngleStepAutoButton;
		private System.Windows.Forms.NumericUpDown AngleStepUpDown;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TrackBar PyramidLevelTrackBar;
		private System.Windows.Forms.Button PyramidLevelAutoButton;
		private System.Windows.Forms.NumericUpDown PyramidLevelUpDown;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox MetricBox;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox OptimizationBox;
		private System.Windows.Forms.Button OptimizationAutoButton;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TrackBar MinContrastTrackBar;
		private System.Windows.Forms.Button MinContrastAutoButton;
		private System.Windows.Forms.NumericUpDown MinContrastUpDown;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button resetModelbutton;
		private System.Windows.Forms.Button delROIButton;
        private System.Windows.Forms.RadioButton noneButton;
        private System.Windows.Forms.Button circleButton;
        private System.Windows.Forms.Button delAllROIButton;
        private System.Windows.Forms.Button resetViewButton;
        private System.Windows.Forms.Panel WindowModePanel;
		public  bool locked;
		
		
        /// <summary>
        /// In the beginning, some initialization tasks are performed. 
        /// The control instances, like ROIController and WindowController,
        /// are initialized and registered with each other. Also, the
        /// delegates of the models are linked to update methods
        /// to get notified about changes.
        /// </summary>
		public MatchingForm()
		{
			InitializeComponent();
			
			mView = new HWndCtrl(viewPort);
            
            createModelWindowMode = Color.RoyalBlue;
            trainModelWindowMode  = Color.Chartreuse;
            
			roiController = new ROIController();
			mView.useROIController(roiController);
			
			roiController.setROISign(ROIController.MODE_ROI_POS); 
			
			mView.NotifyIconObserver = new IconicDelegate(UpdateViewData); 
			roiController.NotifyRCObserver = new IconicDelegate(UpdateViewData);

			mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
			
			locked = true;
			parameterSet = new MatchingParam();
			Init(parameterSet);
			locked = false;

			mAssistant = new MatchingAssistant(parameterSet);
			mAssistant.NotifyIconObserver  = new MatchingDelegate(UpdateMatching);
			mAssistant.NotifyParamObserver = new AutoParamDelegate(UpdateButton);
			
			speedOptHandler		= new MatchingOptSpeed(mAssistant, parameterSet);
			speedOptHandler.NotifyStatisticsObserver = new  StatisticsDelegate(UpdateStatisticsData);

			inspectOptHandler	= new MatchingOptStatistics(mAssistant, parameterSet);
			inspectOptHandler.NotifyStatisticsObserver = new  StatisticsDelegate(UpdateStatisticsData);
		}
		

		/********************************************************************/
		/********************************************************************/
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
		
		/********************************************************************/
		/********************************************************************/
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.viewPort = new HalconDotNet.HWindowControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCreateModel = new System.Windows.Forms.TabPage();
            this.groupBoxCreateModel = new System.Windows.Forms.GroupBox();
            this.MinContrastTrackBar = new System.Windows.Forms.TrackBar();
            this.MinContrastAutoButton = new System.Windows.Forms.Button();
            this.MinContrastUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.OptimizationBox = new System.Windows.Forms.ComboBox();
            this.OptimizationAutoButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.MetricBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.PyramidLevelTrackBar = new System.Windows.Forms.TrackBar();
            this.PyramidLevelAutoButton = new System.Windows.Forms.Button();
            this.PyramidLevelUpDown = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.AngleStepTrackBar = new System.Windows.Forms.TrackBar();
            this.AngleStepAutoButton = new System.Windows.Forms.Button();
            this.AngleStepUpDown = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.AngleExtentTrackBar = new System.Windows.Forms.TrackBar();
            this.AngleExtentUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.StartingAngleTrackBar = new System.Windows.Forms.TrackBar();
            this.StartingAngleUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.ScaleStepAutoButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.ScaleStepUpDown = new System.Windows.Forms.NumericUpDown();
            this.ScaleStepTrackBar = new System.Windows.Forms.TrackBar();
            this.MaxScaleUpDown = new System.Windows.Forms.NumericUpDown();
            this.MaxScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.MinScaleUpDown = new System.Windows.Forms.NumericUpDown();
            this.MinScaleTrackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.ContrastAutoButton = new System.Windows.Forms.Button();
            this.ContrastUpDown = new System.Windows.Forms.NumericUpDown();
            this.ContrastTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.DispPyramidUpDown = new System.Windows.Forms.NumericUpDown();
            this.DispPyramidTrackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageFindModel = new System.Windows.Forms.TabPage();
            this.FindAlwaysCheckBox = new System.Windows.Forms.CheckBox();
            this.testImgListBox = new System.Windows.Forms.ListBox();
            this.findModelButton = new System.Windows.Forms.Button();
            this.displayTestImgButton = new System.Windows.Forms.Button();
            this.deleteAllTestImgButton = new System.Windows.Forms.Button();
            this.deleteTestImgButton = new System.Windows.Forms.Button();
            this.LastPyrLevTrackBar = new System.Windows.Forms.TrackBar();
            this.LastPyrLevUpDown = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.SubPixelBox = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.MaxOverlapTrackBar = new System.Windows.Forms.TrackBar();
            this.MaxOverlapUpDown = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.GreedinessTrackBar = new System.Windows.Forms.TrackBar();
            this.GreedinessUpDown = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.NumMatchesTrackBar = new System.Windows.Forms.TrackBar();
            this.NumMatchesUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.MinScoreTrackBar = new System.Windows.Forms.TrackBar();
            this.MinScoreUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.loadTestImgButton = new System.Windows.Forms.Button();
            this.tabPageOptimRecognition = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RecogNoManSelUpDown = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.FindAtLeastOneModelButton = new System.Windows.Forms.RadioButton();
            this.FindMaxNoOfModelButton = new System.Windows.Forms.RadioButton();
            this.FindNoOfInstButton = new System.Windows.Forms.RadioButton();
            this.recogRateTrackBar = new System.Windows.Forms.TrackBar();
            this.recogRateUpDown = new System.Windows.Forms.NumericUpDown();
            this.RecognRateComboBox = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelOptStatus = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.labelRecogn21 = new System.Windows.Forms.Label();
            this.labelRecogn24 = new System.Windows.Forms.Label();
            this.labelRecogn14 = new System.Windows.Forms.Label();
            this.labelRecogn23 = new System.Windows.Forms.Label();
            this.labelRecogn13 = new System.Windows.Forms.Label();
            this.labelRecogn22 = new System.Windows.Forms.Label();
            this.labelRecogn12 = new System.Windows.Forms.Label();
            this.labelRecogn11 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.OptimizeButton = new System.Windows.Forms.Button();
            this.tabPageInspectModel = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.InspectMaxNoMatchUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label72 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.labelInspect37 = new System.Windows.Forms.Label();
            this.labelInspect27 = new System.Windows.Forms.Label();
            this.labelInspect17 = new System.Windows.Forms.Label();
            this.labelInspect36 = new System.Windows.Forms.Label();
            this.labelInspect26 = new System.Windows.Forms.Label();
            this.labelInspect16 = new System.Windows.Forms.Label();
            this.labelInspect35 = new System.Windows.Forms.Label();
            this.labelInspect25 = new System.Windows.Forms.Label();
            this.labelInspect15 = new System.Windows.Forms.Label();
            this.labelInspect34 = new System.Windows.Forms.Label();
            this.labelInspect24 = new System.Windows.Forms.Label();
            this.labelInspect14 = new System.Windows.Forms.Label();
            this.labelInspect33 = new System.Windows.Forms.Label();
            this.labelInspect23 = new System.Windows.Forms.Label();
            this.labelInspect13 = new System.Windows.Forms.Label();
            this.labelInspect32 = new System.Windows.Forms.Label();
            this.labelInspect22 = new System.Windows.Forms.Label();
            this.labelInspect12 = new System.Windows.Forms.Label();
            this.labelInspect31 = new System.Windows.Forms.Label();
            this.labelInspect21 = new System.Windows.Forms.Label();
            this.labelInspect11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.labelInspect05 = new System.Windows.Forms.Label();
            this.labelInspect04 = new System.Windows.Forms.Label();
            this.labelInspect03 = new System.Windows.Forms.Label();
            this.labelInspect02 = new System.Windows.Forms.Label();
            this.labelInspect01 = new System.Windows.Forms.Label();
            this.StatisticsButton = new System.Windows.Forms.Button();
            this.groupBoxCreateROI = new System.Windows.Forms.GroupBox();
            this.circleButton = new System.Windows.Forms.Button();
            this.delROIButton = new System.Windows.Forms.Button();
            this.delAllROIButton = new System.Windows.Forms.Button();
            this.rect2Button = new System.Windows.Forms.Button();
            this.subFromROIButton = new System.Windows.Forms.RadioButton();
            this.addToROIButton = new System.Windows.Forms.RadioButton();
            this.rect1Button = new System.Windows.Forms.Button();
            this.loadImagebutton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveModelbutton = new System.Windows.Forms.Button();
            this.loadModelbutton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.resetViewButton = new System.Windows.Forms.Button();
            this.noneButton = new System.Windows.Forms.RadioButton();
            this.zoomButton = new System.Windows.Forms.RadioButton();
            this.moveButton = new System.Windows.Forms.RadioButton();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.DisplayDataButton = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog3 = new System.Windows.Forms.OpenFileDialog();
            this.resetModelbutton = new System.Windows.Forms.Button();
            this.WindowModePanel = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.tabPageCreateModel.SuspendLayout();
            this.groupBoxCreateModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinContrastTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContrastUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PyramidLevelTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PyramidLevelUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleStepTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleStepUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleExtentTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleExtentUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingAngleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingAngleUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleStepUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleStepTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxScaleUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxScaleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScaleUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScaleTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DispPyramidUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DispPyramidTrackBar)).BeginInit();
            this.tabPageFindModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastPyrLevTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastPyrLevUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxOverlapTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxOverlapUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreedinessTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreedinessUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMatchesTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMatchesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScoreTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScoreUpDown)).BeginInit();
            this.tabPageOptimRecognition.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecogNoManSelUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recogRateTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recogRateUpDown)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPageInspectModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InspectMaxNoMatchUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxCreateROI.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(56, 40);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(448, 344);
            this.viewPort.TabIndex = 5;
            this.viewPort.WindowSize = new System.Drawing.Size(448, 344);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCreateModel);
            this.tabControl.Controls.Add(this.tabPageFindModel);
            this.tabControl.Controls.Add(this.tabPageOptimRecognition);
            this.tabControl.Controls.Add(this.tabPageInspectModel);
            this.tabControl.Location = new System.Drawing.Point(528, 40);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(400, 520);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageCreateModel
            // 
            this.tabPageCreateModel.Controls.Add(this.groupBoxCreateModel);
            this.tabPageCreateModel.Location = new System.Drawing.Point(4, 22);
            this.tabPageCreateModel.Name = "tabPageCreateModel";
            this.tabPageCreateModel.Size = new System.Drawing.Size(392, 494);
            this.tabPageCreateModel.TabIndex = 0;
            this.tabPageCreateModel.Text = "  Create Model  ";
            // 
            // groupBoxCreateModel
            // 
            this.groupBoxCreateModel.Controls.Add(this.MinContrastTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.MinContrastAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.MinContrastUpDown);
            this.groupBoxCreateModel.Controls.Add(this.label12);
            this.groupBoxCreateModel.Controls.Add(this.OptimizationBox);
            this.groupBoxCreateModel.Controls.Add(this.OptimizationAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.label10);
            this.groupBoxCreateModel.Controls.Add(this.MetricBox);
            this.groupBoxCreateModel.Controls.Add(this.label9);
            this.groupBoxCreateModel.Controls.Add(this.PyramidLevelTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.PyramidLevelAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.PyramidLevelUpDown);
            this.groupBoxCreateModel.Controls.Add(this.label8);
            this.groupBoxCreateModel.Controls.Add(this.AngleStepTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.AngleStepAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.AngleStepUpDown);
            this.groupBoxCreateModel.Controls.Add(this.label7);
            this.groupBoxCreateModel.Controls.Add(this.AngleExtentTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.AngleExtentUpDown);
            this.groupBoxCreateModel.Controls.Add(this.label6);
            this.groupBoxCreateModel.Controls.Add(this.StartingAngleTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.StartingAngleUpDown);
            this.groupBoxCreateModel.Controls.Add(this.label5);
            this.groupBoxCreateModel.Controls.Add(this.ScaleStepAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.label11);
            this.groupBoxCreateModel.Controls.Add(this.ScaleStepUpDown);
            this.groupBoxCreateModel.Controls.Add(this.ScaleStepTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.MaxScaleUpDown);
            this.groupBoxCreateModel.Controls.Add(this.MaxScaleTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.label4);
            this.groupBoxCreateModel.Controls.Add(this.MinScaleUpDown);
            this.groupBoxCreateModel.Controls.Add(this.MinScaleTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.label3);
            this.groupBoxCreateModel.Controls.Add(this.ContrastAutoButton);
            this.groupBoxCreateModel.Controls.Add(this.ContrastUpDown);
            this.groupBoxCreateModel.Controls.Add(this.ContrastTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.label2);
            this.groupBoxCreateModel.Controls.Add(this.DispPyramidUpDown);
            this.groupBoxCreateModel.Controls.Add(this.DispPyramidTrackBar);
            this.groupBoxCreateModel.Controls.Add(this.label1);
            this.groupBoxCreateModel.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCreateModel.Name = "groupBoxCreateModel";
            this.groupBoxCreateModel.Size = new System.Drawing.Size(392, 488);
            this.groupBoxCreateModel.TabIndex = 55;
            this.groupBoxCreateModel.TabStop = false;
            // 
            // MinContrastTrackBar
            // 
            this.MinContrastTrackBar.Location = new System.Drawing.Point(184, 440);
            this.MinContrastTrackBar.Maximum = 30;
            this.MinContrastTrackBar.Name = "MinContrastTrackBar";
            this.MinContrastTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MinContrastTrackBar.Size = new System.Drawing.Size(144, 45);
            this.MinContrastTrackBar.TabIndex = 80;
            this.MinContrastTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinContrastTrackBar.Value = 10;
            this.MinContrastTrackBar.Scroll += new System.EventHandler(this.MinContrastTrackBar_Scroll);
            // 
            // MinContrastAutoButton
            // 
            this.MinContrastAutoButton.Location = new System.Drawing.Point(328, 440);
            this.MinContrastAutoButton.Name = "MinContrastAutoButton";
            this.MinContrastAutoButton.Size = new System.Drawing.Size(48, 24);
            this.MinContrastAutoButton.TabIndex = 79;
            this.MinContrastAutoButton.Text = "Auto";
            this.MinContrastAutoButton.Click += new System.EventHandler(this.MinContrastAutoButton_Click);
            // 
            // MinContrastUpDown
            // 
            this.MinContrastUpDown.Location = new System.Drawing.Point(120, 440);
            this.MinContrastUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.MinContrastUpDown.Name = "MinContrastUpDown";
            this.MinContrastUpDown.Size = new System.Drawing.Size(48, 20);
            this.MinContrastUpDown.TabIndex = 78;
            this.MinContrastUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MinContrastUpDown.ValueChanged += new System.EventHandler(this.MinContrastUpDown_ValueChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(32, 432);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 40);
            this.label12.TabIndex = 77;
            this.label12.Text = "MinContrast";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OptimizationBox
            // 
            this.OptimizationBox.Items.AddRange(new object[] {
            "none",
            "point_reduction_low",
            "point_reduction_medium",
            "point_reduction_high"});
            this.OptimizationBox.Location = new System.Drawing.Point(120, 408);
            this.OptimizationBox.Name = "OptimizationBox";
            this.OptimizationBox.Size = new System.Drawing.Size(200, 21);
            this.OptimizationBox.TabIndex = 76;
            this.OptimizationBox.SelectedIndexChanged += new System.EventHandler(this.OptimizationBox_SelectedIndexChanged);
            // 
            // OptimizationAutoButton
            // 
            this.OptimizationAutoButton.Location = new System.Drawing.Point(328, 408);
            this.OptimizationAutoButton.Name = "OptimizationAutoButton";
            this.OptimizationAutoButton.Size = new System.Drawing.Size(48, 24);
            this.OptimizationAutoButton.TabIndex = 75;
            this.OptimizationAutoButton.Text = "Auto";
            this.OptimizationAutoButton.Click += new System.EventHandler(this.OptimizationAutoButton_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(32, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 40);
            this.label10.TabIndex = 74;
            this.label10.Text = "Optimization";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MetricBox
            // 
            this.MetricBox.Items.AddRange(new object[] {
            "use_polarity",
            "ignore_global_polarity",
            "ignore_local_polarity",
            "ignore_color_polarity"});
            this.MetricBox.Location = new System.Drawing.Point(120, 376);
            this.MetricBox.Name = "MetricBox";
            this.MetricBox.Size = new System.Drawing.Size(200, 21);
            this.MetricBox.TabIndex = 73;
            this.MetricBox.SelectedIndexChanged += new System.EventHandler(this.MetricBox_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(32, 368);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 40);
            this.label9.TabIndex = 72;
            this.label9.Text = "Metric";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PyramidLevelTrackBar
            // 
            this.PyramidLevelTrackBar.Location = new System.Drawing.Point(184, 312);
            this.PyramidLevelTrackBar.Maximum = 6;
            this.PyramidLevelTrackBar.Minimum = 1;
            this.PyramidLevelTrackBar.Name = "PyramidLevelTrackBar";
            this.PyramidLevelTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PyramidLevelTrackBar.Size = new System.Drawing.Size(144, 45);
            this.PyramidLevelTrackBar.TabIndex = 71;
            this.PyramidLevelTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.PyramidLevelTrackBar.Value = 5;
            this.PyramidLevelTrackBar.Scroll += new System.EventHandler(this.PyramidLevelTrackBar_Scroll);
            // 
            // PyramidLevelAutoButton
            // 
            this.PyramidLevelAutoButton.Location = new System.Drawing.Point(328, 312);
            this.PyramidLevelAutoButton.Name = "PyramidLevelAutoButton";
            this.PyramidLevelAutoButton.Size = new System.Drawing.Size(48, 24);
            this.PyramidLevelAutoButton.TabIndex = 70;
            this.PyramidLevelAutoButton.Text = "Auto";
            this.PyramidLevelAutoButton.Click += new System.EventHandler(this.PyramidLevelAutoButton_Click);
            // 
            // PyramidLevelUpDown
            // 
            this.PyramidLevelUpDown.Location = new System.Drawing.Point(120, 312);
            this.PyramidLevelUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.PyramidLevelUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PyramidLevelUpDown.Name = "PyramidLevelUpDown";
            this.PyramidLevelUpDown.Size = new System.Drawing.Size(48, 20);
            this.PyramidLevelUpDown.TabIndex = 69;
            this.PyramidLevelUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.PyramidLevelUpDown.ValueChanged += new System.EventHandler(this.PyramidLevelUpDown_ValueChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(32, 304);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 40);
            this.label8.TabIndex = 68;
            this.label8.Text = "PyramidLevel";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AngleStepTrackBar
            // 
            this.AngleStepTrackBar.Location = new System.Drawing.Point(184, 280);
            this.AngleStepTrackBar.Maximum = 112;
            this.AngleStepTrackBar.Name = "AngleStepTrackBar";
            this.AngleStepTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AngleStepTrackBar.Size = new System.Drawing.Size(144, 45);
            this.AngleStepTrackBar.TabIndex = 67;
            this.AngleStepTrackBar.TickFrequency = 10;
            this.AngleStepTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.AngleStepTrackBar.Value = 10;
            this.AngleStepTrackBar.Scroll += new System.EventHandler(this.AngleStepTrackBar_Scroll);
            // 
            // AngleStepAutoButton
            // 
            this.AngleStepAutoButton.Location = new System.Drawing.Point(328, 280);
            this.AngleStepAutoButton.Name = "AngleStepAutoButton";
            this.AngleStepAutoButton.Size = new System.Drawing.Size(48, 24);
            this.AngleStepAutoButton.TabIndex = 66;
            this.AngleStepAutoButton.Text = "Auto";
            this.AngleStepAutoButton.Click += new System.EventHandler(this.AngleStepAutoButton_Click);
            // 
            // AngleStepUpDown
            // 
            this.AngleStepUpDown.Location = new System.Drawing.Point(120, 280);
            this.AngleStepUpDown.Maximum = new decimal(new int[] {
            112,
            0,
            0,
            0});
            this.AngleStepUpDown.Name = "AngleStepUpDown";
            this.AngleStepUpDown.Size = new System.Drawing.Size(48, 20);
            this.AngleStepUpDown.TabIndex = 65;
            this.AngleStepUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.AngleStepUpDown.ValueChanged += new System.EventHandler(this.AngleStepUpDown_ValueChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(32, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 40);
            this.label7.TabIndex = 64;
            this.label7.Text = "AngleStep*10";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AngleExtentTrackBar
            // 
            this.AngleExtentTrackBar.Location = new System.Drawing.Point(184, 248);
            this.AngleExtentTrackBar.Maximum = 360;
            this.AngleExtentTrackBar.Name = "AngleExtentTrackBar";
            this.AngleExtentTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.AngleExtentTrackBar.Size = new System.Drawing.Size(144, 45);
            this.AngleExtentTrackBar.TabIndex = 63;
            this.AngleExtentTrackBar.TickFrequency = 20;
            this.AngleExtentTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.AngleExtentTrackBar.Value = 360;
            this.AngleExtentTrackBar.Scroll += new System.EventHandler(this.AngleExtentTrackBar_Scroll);
            // 
            // AngleExtentUpDown
            // 
            this.AngleExtentUpDown.Location = new System.Drawing.Point(120, 248);
            this.AngleExtentUpDown.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.AngleExtentUpDown.Name = "AngleExtentUpDown";
            this.AngleExtentUpDown.Size = new System.Drawing.Size(48, 20);
            this.AngleExtentUpDown.TabIndex = 62;
            this.AngleExtentUpDown.Value = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.AngleExtentUpDown.ValueChanged += new System.EventHandler(this.AngleExtentUpDown_ValueChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(32, 240);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 40);
            this.label6.TabIndex = 61;
            this.label6.Text = "AngleExtent";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StartingAngleTrackBar
            // 
            this.StartingAngleTrackBar.Location = new System.Drawing.Point(184, 216);
            this.StartingAngleTrackBar.Maximum = 180;
            this.StartingAngleTrackBar.Minimum = -180;
            this.StartingAngleTrackBar.Name = "StartingAngleTrackBar";
            this.StartingAngleTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartingAngleTrackBar.Size = new System.Drawing.Size(144, 45);
            this.StartingAngleTrackBar.TabIndex = 60;
            this.StartingAngleTrackBar.TickFrequency = 20;
            this.StartingAngleTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.StartingAngleTrackBar.Scroll += new System.EventHandler(this.StartingAngleTrackBar_Scroll);
            // 
            // StartingAngleUpDown
            // 
            this.StartingAngleUpDown.Location = new System.Drawing.Point(120, 216);
            this.StartingAngleUpDown.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.StartingAngleUpDown.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.StartingAngleUpDown.Name = "StartingAngleUpDown";
            this.StartingAngleUpDown.Size = new System.Drawing.Size(48, 20);
            this.StartingAngleUpDown.TabIndex = 59;
            this.StartingAngleUpDown.ValueChanged += new System.EventHandler(this.StartingAngleUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(32, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 40);
            this.label5.TabIndex = 58;
            this.label5.Text = "StartingAngle";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleStepAutoButton
            // 
            this.ScaleStepAutoButton.Location = new System.Drawing.Point(328, 184);
            this.ScaleStepAutoButton.Name = "ScaleStepAutoButton";
            this.ScaleStepAutoButton.Size = new System.Drawing.Size(48, 24);
            this.ScaleStepAutoButton.TabIndex = 50;
            this.ScaleStepAutoButton.Text = "Auto";
            this.ScaleStepAutoButton.Click += new System.EventHandler(this.ScaleStepAutoButton_Click);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(32, 176);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 40);
            this.label11.TabIndex = 49;
            this.label11.Text = "ScaleStep*1000";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ScaleStepUpDown
            // 
            this.ScaleStepUpDown.Location = new System.Drawing.Point(120, 184);
            this.ScaleStepUpDown.Maximum = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.ScaleStepUpDown.Name = "ScaleStepUpDown";
            this.ScaleStepUpDown.Size = new System.Drawing.Size(48, 20);
            this.ScaleStepUpDown.TabIndex = 48;
            this.ScaleStepUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.ScaleStepUpDown.ValueChanged += new System.EventHandler(this.ScaleStepUpDown_ValueChanged);
            // 
            // ScaleStepTrackBar
            // 
            this.ScaleStepTrackBar.Location = new System.Drawing.Point(184, 184);
            this.ScaleStepTrackBar.Maximum = 190;
            this.ScaleStepTrackBar.Name = "ScaleStepTrackBar";
            this.ScaleStepTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ScaleStepTrackBar.Size = new System.Drawing.Size(144, 45);
            this.ScaleStepTrackBar.TabIndex = 47;
            this.ScaleStepTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ScaleStepTrackBar.Value = 10;
            this.ScaleStepTrackBar.Scroll += new System.EventHandler(this.ScaleStepTrackBar_Scroll);
            // 
            // MaxScaleUpDown
            // 
            this.MaxScaleUpDown.Location = new System.Drawing.Point(120, 152);
            this.MaxScaleUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.MaxScaleUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MaxScaleUpDown.Name = "MaxScaleUpDown";
            this.MaxScaleUpDown.Size = new System.Drawing.Size(48, 20);
            this.MaxScaleUpDown.TabIndex = 46;
            this.MaxScaleUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxScaleUpDown.ValueChanged += new System.EventHandler(this.MaxScaleUpDown_ValueChanged);
            // 
            // MaxScaleTrackBar
            // 
            this.MaxScaleTrackBar.Location = new System.Drawing.Point(184, 152);
            this.MaxScaleTrackBar.Maximum = 200;
            this.MaxScaleTrackBar.Minimum = 1;
            this.MaxScaleTrackBar.Name = "MaxScaleTrackBar";
            this.MaxScaleTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MaxScaleTrackBar.Size = new System.Drawing.Size(144, 45);
            this.MaxScaleTrackBar.TabIndex = 45;
            this.MaxScaleTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MaxScaleTrackBar.Value = 100;
            this.MaxScaleTrackBar.Scroll += new System.EventHandler(this.MaxScaleTrackBar_Scroll);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 40);
            this.label4.TabIndex = 44;
            this.label4.Text = "MaxScale*100";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinScaleUpDown
            // 
            this.MinScaleUpDown.Location = new System.Drawing.Point(120, 120);
            this.MinScaleUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.MinScaleUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MinScaleUpDown.Name = "MinScaleUpDown";
            this.MinScaleUpDown.Size = new System.Drawing.Size(48, 20);
            this.MinScaleUpDown.TabIndex = 43;
            this.MinScaleUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MinScaleUpDown.ValueChanged += new System.EventHandler(this.MinScaleUpDown_ValueChanged);
            // 
            // MinScaleTrackBar
            // 
            this.MinScaleTrackBar.Location = new System.Drawing.Point(184, 120);
            this.MinScaleTrackBar.Maximum = 200;
            this.MinScaleTrackBar.Minimum = 1;
            this.MinScaleTrackBar.Name = "MinScaleTrackBar";
            this.MinScaleTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MinScaleTrackBar.Size = new System.Drawing.Size(144, 45);
            this.MinScaleTrackBar.TabIndex = 42;
            this.MinScaleTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinScaleTrackBar.Value = 100;
            this.MinScaleTrackBar.Scroll += new System.EventHandler(this.MinScaleTrackBar_Scroll);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(32, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 40);
            this.label3.TabIndex = 41;
            this.label3.Text = "MinScale*100";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ContrastAutoButton
            // 
            this.ContrastAutoButton.BackColor = System.Drawing.SystemColors.Control;
            this.ContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ContrastAutoButton.Location = new System.Drawing.Point(328, 88);
            this.ContrastAutoButton.Name = "ContrastAutoButton";
            this.ContrastAutoButton.Size = new System.Drawing.Size(48, 24);
            this.ContrastAutoButton.TabIndex = 40;
            this.ContrastAutoButton.Text = "Auto";
            this.ContrastAutoButton.UseVisualStyleBackColor = false;
            this.ContrastAutoButton.Click += new System.EventHandler(this.ContrastAutoButton_Click);
            // 
            // ContrastUpDown
            // 
            this.ContrastUpDown.Location = new System.Drawing.Point(120, 88);
            this.ContrastUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.ContrastUpDown.Name = "ContrastUpDown";
            this.ContrastUpDown.Size = new System.Drawing.Size(48, 20);
            this.ContrastUpDown.TabIndex = 39;
            this.ContrastUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.ContrastUpDown.ValueChanged += new System.EventHandler(this.ContrastUpDown_ValueChanged);
            // 
            // ContrastTrackBar
            // 
            this.ContrastTrackBar.Location = new System.Drawing.Point(184, 88);
            this.ContrastTrackBar.Maximum = 255;
            this.ContrastTrackBar.Name = "ContrastTrackBar";
            this.ContrastTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ContrastTrackBar.Size = new System.Drawing.Size(144, 45);
            this.ContrastTrackBar.TabIndex = 38;
            this.ContrastTrackBar.TickFrequency = 15;
            this.ContrastTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ContrastTrackBar.Value = 30;
            this.ContrastTrackBar.Scroll += new System.EventHandler(this.ContrastTrackBar_Scroll);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 40);
            this.label2.TabIndex = 37;
            this.label2.Text = "Contrast";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DispPyramidUpDown
            // 
            this.DispPyramidUpDown.Location = new System.Drawing.Point(128, 32);
            this.DispPyramidUpDown.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.DispPyramidUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DispPyramidUpDown.Name = "DispPyramidUpDown";
            this.DispPyramidUpDown.Size = new System.Drawing.Size(40, 20);
            this.DispPyramidUpDown.TabIndex = 30;
            this.DispPyramidUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DispPyramidUpDown.ValueChanged += new System.EventHandler(this.DispPyramidUpDown_ValueChanged);
            // 
            // DispPyramidTrackBar
            // 
            this.DispPyramidTrackBar.Enabled = false;
            this.DispPyramidTrackBar.Location = new System.Drawing.Point(184, 32);
            this.DispPyramidTrackBar.Maximum = 6;
            this.DispPyramidTrackBar.Minimum = 1;
            this.DispPyramidTrackBar.Name = "DispPyramidTrackBar";
            this.DispPyramidTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DispPyramidTrackBar.Size = new System.Drawing.Size(144, 45);
            this.DispPyramidTrackBar.TabIndex = 29;
            this.DispPyramidTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.DispPyramidTrackBar.Value = 1;
            this.DispPyramidTrackBar.Scroll += new System.EventHandler(this.DispPyramidTrackBar_Scroll);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 40);
            this.label1.TabIndex = 28;
            this.label1.Text = "Display Image Pyramid";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPageFindModel
            // 
            this.tabPageFindModel.Controls.Add(this.FindAlwaysCheckBox);
            this.tabPageFindModel.Controls.Add(this.testImgListBox);
            this.tabPageFindModel.Controls.Add(this.findModelButton);
            this.tabPageFindModel.Controls.Add(this.displayTestImgButton);
            this.tabPageFindModel.Controls.Add(this.deleteAllTestImgButton);
            this.tabPageFindModel.Controls.Add(this.deleteTestImgButton);
            this.tabPageFindModel.Controls.Add(this.LastPyrLevTrackBar);
            this.tabPageFindModel.Controls.Add(this.LastPyrLevUpDown);
            this.tabPageFindModel.Controls.Add(this.label18);
            this.tabPageFindModel.Controls.Add(this.SubPixelBox);
            this.tabPageFindModel.Controls.Add(this.label17);
            this.tabPageFindModel.Controls.Add(this.MaxOverlapTrackBar);
            this.tabPageFindModel.Controls.Add(this.MaxOverlapUpDown);
            this.tabPageFindModel.Controls.Add(this.label15);
            this.tabPageFindModel.Controls.Add(this.GreedinessTrackBar);
            this.tabPageFindModel.Controls.Add(this.GreedinessUpDown);
            this.tabPageFindModel.Controls.Add(this.label16);
            this.tabPageFindModel.Controls.Add(this.NumMatchesTrackBar);
            this.tabPageFindModel.Controls.Add(this.NumMatchesUpDown);
            this.tabPageFindModel.Controls.Add(this.label14);
            this.tabPageFindModel.Controls.Add(this.MinScoreTrackBar);
            this.tabPageFindModel.Controls.Add(this.MinScoreUpDown);
            this.tabPageFindModel.Controls.Add(this.label13);
            this.tabPageFindModel.Controls.Add(this.loadTestImgButton);
            this.tabPageFindModel.Location = new System.Drawing.Point(4, 22);
            this.tabPageFindModel.Name = "tabPageFindModel";
            this.tabPageFindModel.Size = new System.Drawing.Size(392, 494);
            this.tabPageFindModel.TabIndex = 0;
            this.tabPageFindModel.Text = "  Find Model  ";
            // 
            // FindAlwaysCheckBox
            // 
            this.FindAlwaysCheckBox.Location = new System.Drawing.Point(288, 212);
            this.FindAlwaysCheckBox.Name = "FindAlwaysCheckBox";
            this.FindAlwaysCheckBox.Size = new System.Drawing.Size(88, 32);
            this.FindAlwaysCheckBox.TabIndex = 68;
            this.FindAlwaysCheckBox.Text = "Always Find";
            this.FindAlwaysCheckBox.CheckedChanged += new System.EventHandler(this.findAlwaysCheckBox_CheckedChanged);
            // 
            // testImgListBox
            // 
            this.testImgListBox.HorizontalScrollbar = true;
            this.testImgListBox.Location = new System.Drawing.Point(32, 24);
            this.testImgListBox.Name = "testImgListBox";
            this.testImgListBox.Size = new System.Drawing.Size(240, 212);
            this.testImgListBox.TabIndex = 67;
            this.testImgListBox.SelectedIndexChanged += new System.EventHandler(this.testImgListBox_SelectedIndexChanged);
            // 
            // findModelButton
            // 
            this.findModelButton.Location = new System.Drawing.Point(288, 178);
            this.findModelButton.Name = "findModelButton";
            this.findModelButton.Size = new System.Drawing.Size(88, 28);
            this.findModelButton.TabIndex = 66;
            this.findModelButton.Text = "Find Model";
            this.findModelButton.Click += new System.EventHandler(this.findModelButton_Click);
            // 
            // displayTestImgButton
            // 
            this.displayTestImgButton.Location = new System.Drawing.Point(288, 146);
            this.displayTestImgButton.Name = "displayTestImgButton";
            this.displayTestImgButton.Size = new System.Drawing.Size(88, 28);
            this.displayTestImgButton.TabIndex = 65;
            this.displayTestImgButton.Text = "Display";
            this.displayTestImgButton.Click += new System.EventHandler(this.displayTestImgButton_Click);
            // 
            // deleteAllTestImgButton
            // 
            this.deleteAllTestImgButton.Location = new System.Drawing.Point(288, 88);
            this.deleteAllTestImgButton.Name = "deleteAllTestImgButton";
            this.deleteAllTestImgButton.Size = new System.Drawing.Size(88, 28);
            this.deleteAllTestImgButton.TabIndex = 64;
            this.deleteAllTestImgButton.Text = "Delete All";
            this.deleteAllTestImgButton.Click += new System.EventHandler(this.deleteAllTestImgButton_Click);
            // 
            // deleteTestImgButton
            // 
            this.deleteTestImgButton.Location = new System.Drawing.Point(288, 56);
            this.deleteTestImgButton.Name = "deleteTestImgButton";
            this.deleteTestImgButton.Size = new System.Drawing.Size(88, 28);
            this.deleteTestImgButton.TabIndex = 63;
            this.deleteTestImgButton.Text = "Delete Image";
            this.deleteTestImgButton.Click += new System.EventHandler(this.deleteTestImgButton_Click);
            // 
            // LastPyrLevTrackBar
            // 
            this.LastPyrLevTrackBar.Location = new System.Drawing.Point(192, 440);
            this.LastPyrLevTrackBar.Maximum = 5;
            this.LastPyrLevTrackBar.Minimum = 1;
            this.LastPyrLevTrackBar.Name = "LastPyrLevTrackBar";
            this.LastPyrLevTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LastPyrLevTrackBar.Size = new System.Drawing.Size(192, 45);
            this.LastPyrLevTrackBar.TabIndex = 62;
            this.LastPyrLevTrackBar.TickFrequency = 20;
            this.LastPyrLevTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.LastPyrLevTrackBar.Value = 1;
            this.LastPyrLevTrackBar.Scroll += new System.EventHandler(this.LastPyrLevTrackBar_Scroll);
            // 
            // LastPyrLevUpDown
            // 
            this.LastPyrLevUpDown.Location = new System.Drawing.Point(128, 440);
            this.LastPyrLevUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.LastPyrLevUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LastPyrLevUpDown.Name = "LastPyrLevUpDown";
            this.LastPyrLevUpDown.Size = new System.Drawing.Size(48, 20);
            this.LastPyrLevUpDown.TabIndex = 61;
            this.LastPyrLevUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LastPyrLevUpDown.ValueChanged += new System.EventHandler(this.LastPyrLevUpDown_ValueChanged);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(32, 432);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 40);
            this.label18.TabIndex = 60;
            this.label18.Text = "Last Pyramid Level";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SubPixelBox
            // 
            this.SubPixelBox.Items.AddRange(new object[] {
            "none",
            "interpolation",
            "least_squares",
            "least_squares_high",
            "least_squares_very_high"});
            this.SubPixelBox.Location = new System.Drawing.Point(128, 406);
            this.SubPixelBox.Name = "SubPixelBox";
            this.SubPixelBox.Size = new System.Drawing.Size(248, 21);
            this.SubPixelBox.TabIndex = 59;
            this.SubPixelBox.SelectedIndexChanged += new System.EventHandler(this.SubPixelBox_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(32, 398);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(89, 40);
            this.label17.TabIndex = 58;
            this.label17.Text = "Subpixel";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaxOverlapTrackBar
            // 
            this.MaxOverlapTrackBar.LargeChange = 10;
            this.MaxOverlapTrackBar.Location = new System.Drawing.Point(192, 360);
            this.MaxOverlapTrackBar.Maximum = 100;
            this.MaxOverlapTrackBar.Name = "MaxOverlapTrackBar";
            this.MaxOverlapTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MaxOverlapTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MaxOverlapTrackBar.TabIndex = 57;
            this.MaxOverlapTrackBar.TickFrequency = 20;
            this.MaxOverlapTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MaxOverlapTrackBar.Value = 50;
            this.MaxOverlapTrackBar.Scroll += new System.EventHandler(this.MaxOverlapTrackBar_Scroll);
            // 
            // MaxOverlapUpDown
            // 
            this.MaxOverlapUpDown.Location = new System.Drawing.Point(128, 360);
            this.MaxOverlapUpDown.Name = "MaxOverlapUpDown";
            this.MaxOverlapUpDown.Size = new System.Drawing.Size(48, 20);
            this.MaxOverlapUpDown.TabIndex = 56;
            this.MaxOverlapUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.MaxOverlapUpDown.ValueChanged += new System.EventHandler(this.MaxOverlapUpDown_ValueChanged);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(32, 352);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 40);
            this.label15.TabIndex = 55;
            this.label15.Text = "MaxOverlap*100";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GreedinessTrackBar
            // 
            this.GreedinessTrackBar.LargeChange = 10;
            this.GreedinessTrackBar.Location = new System.Drawing.Point(192, 336);
            this.GreedinessTrackBar.Maximum = 100;
            this.GreedinessTrackBar.Name = "GreedinessTrackBar";
            this.GreedinessTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.GreedinessTrackBar.Size = new System.Drawing.Size(192, 45);
            this.GreedinessTrackBar.TabIndex = 54;
            this.GreedinessTrackBar.TickFrequency = 20;
            this.GreedinessTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.GreedinessTrackBar.Value = 75;
            this.GreedinessTrackBar.Scroll += new System.EventHandler(this.GreedinessTrackBar_Scroll);
            // 
            // GreedinessUpDown
            // 
            this.GreedinessUpDown.Location = new System.Drawing.Point(128, 336);
            this.GreedinessUpDown.Name = "GreedinessUpDown";
            this.GreedinessUpDown.Size = new System.Drawing.Size(48, 20);
            this.GreedinessUpDown.TabIndex = 53;
            this.GreedinessUpDown.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.GreedinessUpDown.ValueChanged += new System.EventHandler(this.GreedinessUpDown_ValueChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(32, 328);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(89, 40);
            this.label16.TabIndex = 52;
            this.label16.Text = "Greediness*100";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NumMatchesTrackBar
            // 
            this.NumMatchesTrackBar.LargeChange = 10;
            this.NumMatchesTrackBar.Location = new System.Drawing.Point(192, 288);
            this.NumMatchesTrackBar.Maximum = 100;
            this.NumMatchesTrackBar.Name = "NumMatchesTrackBar";
            this.NumMatchesTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NumMatchesTrackBar.Size = new System.Drawing.Size(192, 45);
            this.NumMatchesTrackBar.TabIndex = 51;
            this.NumMatchesTrackBar.TickFrequency = 20;
            this.NumMatchesTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.NumMatchesTrackBar.Value = 1;
            this.NumMatchesTrackBar.Scroll += new System.EventHandler(this.NumMatchesTrackBar_Scroll);
            // 
            // NumMatchesUpDown
            // 
            this.NumMatchesUpDown.Location = new System.Drawing.Point(128, 288);
            this.NumMatchesUpDown.Name = "NumMatchesUpDown";
            this.NumMatchesUpDown.Size = new System.Drawing.Size(48, 20);
            this.NumMatchesUpDown.TabIndex = 50;
            this.NumMatchesUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumMatchesUpDown.ValueChanged += new System.EventHandler(this.NumMatchesUpDown_ValueChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(32, 280);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 40);
            this.label14.TabIndex = 49;
            this.label14.Text = "NumMatches";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinScoreTrackBar
            // 
            this.MinScoreTrackBar.LargeChange = 10;
            this.MinScoreTrackBar.Location = new System.Drawing.Point(192, 264);
            this.MinScoreTrackBar.Maximum = 100;
            this.MinScoreTrackBar.Name = "MinScoreTrackBar";
            this.MinScoreTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.MinScoreTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MinScoreTrackBar.TabIndex = 48;
            this.MinScoreTrackBar.TickFrequency = 20;
            this.MinScoreTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinScoreTrackBar.Value = 50;
            this.MinScoreTrackBar.Scroll += new System.EventHandler(this.MinScoreTrackBar_Scroll);
            // 
            // MinScoreUpDown
            // 
            this.MinScoreUpDown.Location = new System.Drawing.Point(128, 264);
            this.MinScoreUpDown.Name = "MinScoreUpDown";
            this.MinScoreUpDown.Size = new System.Drawing.Size(48, 20);
            this.MinScoreUpDown.TabIndex = 47;
            this.MinScoreUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.MinScoreUpDown.ValueChanged += new System.EventHandler(this.MinScoreUpDown_ValueChanged);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(32, 256);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 40);
            this.label13.TabIndex = 46;
            this.label13.Text = "MinScore*100";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // loadTestImgButton
            // 
            this.loadTestImgButton.Location = new System.Drawing.Point(288, 24);
            this.loadTestImgButton.Name = "loadTestImgButton";
            this.loadTestImgButton.Size = new System.Drawing.Size(88, 28);
            this.loadTestImgButton.TabIndex = 0;
            this.loadTestImgButton.Text = "Load Images";
            this.loadTestImgButton.Click += new System.EventHandler(this.LoadTestImgButton_Click);
            // 
            // tabPageOptimRecognition
            // 
            this.tabPageOptimRecognition.Controls.Add(this.groupBox1);
            this.tabPageOptimRecognition.Controls.Add(this.panel1);
            this.tabPageOptimRecognition.Controls.Add(this.OptimizeButton);
            this.tabPageOptimRecognition.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptimRecognition.Name = "tabPageOptimRecognition";
            this.tabPageOptimRecognition.Size = new System.Drawing.Size(392, 494);
            this.tabPageOptimRecognition.TabIndex = 1;
            this.tabPageOptimRecognition.Text = "Optimize Recognition";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RecogNoManSelUpDown);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.FindAtLeastOneModelButton);
            this.groupBox1.Controls.Add(this.FindMaxNoOfModelButton);
            this.groupBox1.Controls.Add(this.FindNoOfInstButton);
            this.groupBox1.Controls.Add(this.recogRateTrackBar);
            this.groupBox1.Controls.Add(this.recogRateUpDown);
            this.groupBox1.Controls.Add(this.RecognRateComboBox);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Location = new System.Drawing.Point(16, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 184);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            // 
            // RecogNoManSelUpDown
            // 
            this.RecogNoManSelUpDown.Location = new System.Drawing.Point(304, 36);
            this.RecogNoManSelUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RecogNoManSelUpDown.Name = "RecogNoManSelUpDown";
            this.RecogNoManSelUpDown.Size = new System.Drawing.Size(48, 20);
            this.RecogNoManSelUpDown.TabIndex = 54;
            this.RecogNoManSelUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RecogNoManSelUpDown.ValueChanged += new System.EventHandler(this.RecogNoManSelUpDown_ValueChanged);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(16, 40);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(56, 48);
            this.label19.TabIndex = 1;
            this.label19.Text = "Mode";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FindAtLeastOneModelButton
            // 
            this.FindAtLeastOneModelButton.Location = new System.Drawing.Point(72, 56);
            this.FindAtLeastOneModelButton.Name = "FindAtLeastOneModelButton";
            this.FindAtLeastOneModelButton.Size = new System.Drawing.Size(280, 20);
            this.FindAtLeastOneModelButton.TabIndex = 2;
            this.FindAtLeastOneModelButton.Text = "  Find at least one model instance per image";
            this.FindAtLeastOneModelButton.CheckedChanged += new System.EventHandler(this.FindAtLeastOneModelButton_CheckedChanged);
            // 
            // FindMaxNoOfModelButton
            // 
            this.FindMaxNoOfModelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindMaxNoOfModelButton.Location = new System.Drawing.Point(72, 80);
            this.FindMaxNoOfModelButton.Name = "FindMaxNoOfModelButton";
            this.FindMaxNoOfModelButton.Size = new System.Drawing.Size(288, 16);
            this.FindMaxNoOfModelButton.TabIndex = 3;
            this.FindMaxNoOfModelButton.Text = "  Find maximum number of model instances per image";
            this.FindMaxNoOfModelButton.CheckedChanged += new System.EventHandler(this.FindMaxNoOfModelButton_CheckedChanged);
            // 
            // FindNoOfInstButton
            // 
            this.FindNoOfInstButton.Checked = true;
            this.FindNoOfInstButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FindNoOfInstButton.Location = new System.Drawing.Point(72, 32);
            this.FindNoOfInstButton.Name = "FindNoOfInstButton";
            this.FindNoOfInstButton.Size = new System.Drawing.Size(248, 24);
            this.FindNoOfInstButton.TabIndex = 0;
            this.FindNoOfInstButton.TabStop = true;
            this.FindNoOfInstButton.Text = "  Find number of instances specified";
            this.FindNoOfInstButton.CheckedChanged += new System.EventHandler(this.FindNoOfInstButton_CheckedChanged);
            // 
            // recogRateTrackBar
            // 
            this.recogRateTrackBar.LargeChange = 10;
            this.recogRateTrackBar.Location = new System.Drawing.Point(184, 128);
            this.recogRateTrackBar.Maximum = 100;
            this.recogRateTrackBar.Name = "recogRateTrackBar";
            this.recogRateTrackBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.recogRateTrackBar.Size = new System.Drawing.Size(168, 45);
            this.recogRateTrackBar.TabIndex = 53;
            this.recogRateTrackBar.TickFrequency = 20;
            this.recogRateTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.recogRateTrackBar.Value = 100;
            this.recogRateTrackBar.Scroll += new System.EventHandler(this.recogRateTrackBar_Scroll);
            // 
            // recogRateUpDown
            // 
            this.recogRateUpDown.Location = new System.Drawing.Point(128, 128);
            this.recogRateUpDown.Name = "recogRateUpDown";
            this.recogRateUpDown.Size = new System.Drawing.Size(48, 20);
            this.recogRateUpDown.TabIndex = 52;
            this.recogRateUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.recogRateUpDown.ValueChanged += new System.EventHandler(this.recogRateUpDown_ValueChanged);
            // 
            // RecognRateComboBox
            // 
            this.RecognRateComboBox.Items.AddRange(new object[] {
            "=",
            ">="});
            this.RecognRateComboBox.Location = new System.Drawing.Point(72, 128);
            this.RecognRateComboBox.Name = "RecognRateComboBox";
            this.RecognRateComboBox.Size = new System.Drawing.Size(40, 21);
            this.RecognRateComboBox.TabIndex = 5;
            this.RecognRateComboBox.SelectedIndexChanged += new System.EventHandler(this.RecognRateComboBox_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(16, 120);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(48, 40);
            this.label20.TabIndex = 4;
            this.label20.Text = "Recogn Rate";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelOptStatus);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.label34);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Controls.Add(this.label31);
            this.panel1.Controls.Add(this.labelRecogn21);
            this.panel1.Controls.Add(this.labelRecogn24);
            this.panel1.Controls.Add(this.labelRecogn14);
            this.panel1.Controls.Add(this.labelRecogn23);
            this.panel1.Controls.Add(this.labelRecogn13);
            this.panel1.Controls.Add(this.labelRecogn22);
            this.panel1.Controls.Add(this.labelRecogn12);
            this.panel1.Controls.Add(this.labelRecogn11);
            this.panel1.Controls.Add(this.label29);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Location = new System.Drawing.Point(16, 216);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(368, 216);
            this.panel1.TabIndex = 55;
            // 
            // labelOptStatus
            // 
            this.labelOptStatus.BackColor = System.Drawing.Color.White;
            this.labelOptStatus.Location = new System.Drawing.Point(8, 16);
            this.labelOptStatus.Name = "labelOptStatus";
            this.labelOptStatus.Size = new System.Drawing.Size(352, 24);
            this.labelOptStatus.TabIndex = 63;
            this.labelOptStatus.Text = " Optimization Status: ";
            this.labelOptStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label35
            // 
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(8, 176);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(112, 32);
            this.label35.TabIndex = 62;
            this.label35.Text = "Average Time";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(8, 144);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(112, 32);
            this.label34.TabIndex = 61;
            this.label34.Text = "Recognition Rate";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(8, 112);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(112, 32);
            this.label33.TabIndex = 60;
            this.label33.Text = "Greediness";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(8, 40);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(88, 32);
            this.label32.TabIndex = 59;
            this.label32.Text = "Statistics";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(8, 80);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(112, 32);
            this.label31.TabIndex = 58;
            this.label31.Text = "Minimum Score";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelRecogn21
            // 
            this.labelRecogn21.BackColor = System.Drawing.Color.White;
            this.labelRecogn21.Location = new System.Drawing.Point(256, 72);
            this.labelRecogn21.Name = "labelRecogn21";
            this.labelRecogn21.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn21.TabIndex = 8;
            this.labelRecogn21.Text = "-";
            this.labelRecogn21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn24
            // 
            this.labelRecogn24.BackColor = System.Drawing.Color.White;
            this.labelRecogn24.Location = new System.Drawing.Point(256, 180);
            this.labelRecogn24.Name = "labelRecogn24";
            this.labelRecogn24.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn24.TabIndex = 7;
            this.labelRecogn24.Text = "-";
            this.labelRecogn24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn14
            // 
            this.labelRecogn14.BackColor = System.Drawing.Color.White;
            this.labelRecogn14.Location = new System.Drawing.Point(144, 180);
            this.labelRecogn14.Name = "labelRecogn14";
            this.labelRecogn14.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn14.TabIndex = 6;
            this.labelRecogn14.Text = "-";
            this.labelRecogn14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn23
            // 
            this.labelRecogn23.BackColor = System.Drawing.Color.White;
            this.labelRecogn23.Location = new System.Drawing.Point(256, 144);
            this.labelRecogn23.Name = "labelRecogn23";
            this.labelRecogn23.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn23.TabIndex = 5;
            this.labelRecogn23.Text = "-";
            this.labelRecogn23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn13
            // 
            this.labelRecogn13.BackColor = System.Drawing.Color.White;
            this.labelRecogn13.Location = new System.Drawing.Point(144, 144);
            this.labelRecogn13.Name = "labelRecogn13";
            this.labelRecogn13.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn13.TabIndex = 4;
            this.labelRecogn13.Text = "-";
            this.labelRecogn13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn22
            // 
            this.labelRecogn22.BackColor = System.Drawing.Color.White;
            this.labelRecogn22.Location = new System.Drawing.Point(256, 108);
            this.labelRecogn22.Name = "labelRecogn22";
            this.labelRecogn22.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn22.TabIndex = 3;
            this.labelRecogn22.Text = "-";
            this.labelRecogn22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn12
            // 
            this.labelRecogn12.BackColor = System.Drawing.Color.White;
            this.labelRecogn12.Location = new System.Drawing.Point(144, 108);
            this.labelRecogn12.Name = "labelRecogn12";
            this.labelRecogn12.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn12.TabIndex = 2;
            this.labelRecogn12.Text = "-";
            this.labelRecogn12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecogn11
            // 
            this.labelRecogn11.BackColor = System.Drawing.Color.White;
            this.labelRecogn11.Location = new System.Drawing.Point(144, 72);
            this.labelRecogn11.Name = "labelRecogn11";
            this.labelRecogn11.Size = new System.Drawing.Size(104, 32);
            this.labelRecogn11.TabIndex = 0;
            this.labelRecogn11.Text = "-";
            this.labelRecogn11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(136, 40);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(112, 32);
            this.label29.TabIndex = 56;
            this.label29.Text = "Last Test Run";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(248, 40);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(112, 32);
            this.label30.TabIndex = 57;
            this.label30.Text = "Optimum Test Run";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OptimizeButton
            // 
            this.OptimizeButton.Location = new System.Drawing.Point(272, 436);
            this.OptimizeButton.Name = "OptimizeButton";
            this.OptimizeButton.Size = new System.Drawing.Size(104, 32);
            this.OptimizeButton.TabIndex = 54;
            this.OptimizeButton.Text = "Optimize";
            this.OptimizeButton.Click += new System.EventHandler(this.runOptimizeSpeedButton_Click);
            // 
            // tabPageInspectModel
            // 
            this.tabPageInspectModel.Controls.Add(this.label21);
            this.tabPageInspectModel.Controls.Add(this.InspectMaxNoMatchUpDown);
            this.tabPageInspectModel.Controls.Add(this.groupBox4);
            this.tabPageInspectModel.Controls.Add(this.groupBox3);
            this.tabPageInspectModel.Controls.Add(this.StatisticsButton);
            this.tabPageInspectModel.Location = new System.Drawing.Point(4, 22);
            this.tabPageInspectModel.Name = "tabPageInspectModel";
            this.tabPageInspectModel.Size = new System.Drawing.Size(392, 494);
            this.tabPageInspectModel.TabIndex = 0;
            this.tabPageInspectModel.Text = "  Inspect Model  ";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(16, 448);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(160, 24);
            this.label21.TabIndex = 4;
            this.label21.Text = "Maximum Number of Matches";
            // 
            // InspectMaxNoMatchUpDown
            // 
            this.InspectMaxNoMatchUpDown.Location = new System.Drawing.Point(176, 448);
            this.InspectMaxNoMatchUpDown.Name = "InspectMaxNoMatchUpDown";
            this.InspectMaxNoMatchUpDown.Size = new System.Drawing.Size(48, 20);
            this.InspectMaxNoMatchUpDown.TabIndex = 3;
            this.InspectMaxNoMatchUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InspectMaxNoMatchUpDown.ValueChanged += new System.EventHandler(this.InspectMaxNoMatchUpDown_ValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label72);
            this.groupBox4.Controls.Add(this.label71);
            this.groupBox4.Controls.Add(this.label70);
            this.groupBox4.Controls.Add(this.label69);
            this.groupBox4.Controls.Add(this.label68);
            this.groupBox4.Controls.Add(this.label67);
            this.groupBox4.Controls.Add(this.label66);
            this.groupBox4.Controls.Add(this.label65);
            this.groupBox4.Controls.Add(this.label64);
            this.groupBox4.Controls.Add(this.label63);
            this.groupBox4.Controls.Add(this.label62);
            this.groupBox4.Controls.Add(this.label43);
            this.groupBox4.Controls.Add(this.labelInspect37);
            this.groupBox4.Controls.Add(this.labelInspect27);
            this.groupBox4.Controls.Add(this.labelInspect17);
            this.groupBox4.Controls.Add(this.labelInspect36);
            this.groupBox4.Controls.Add(this.labelInspect26);
            this.groupBox4.Controls.Add(this.labelInspect16);
            this.groupBox4.Controls.Add(this.labelInspect35);
            this.groupBox4.Controls.Add(this.labelInspect25);
            this.groupBox4.Controls.Add(this.labelInspect15);
            this.groupBox4.Controls.Add(this.labelInspect34);
            this.groupBox4.Controls.Add(this.labelInspect24);
            this.groupBox4.Controls.Add(this.labelInspect14);
            this.groupBox4.Controls.Add(this.labelInspect33);
            this.groupBox4.Controls.Add(this.labelInspect23);
            this.groupBox4.Controls.Add(this.labelInspect13);
            this.groupBox4.Controls.Add(this.labelInspect32);
            this.groupBox4.Controls.Add(this.labelInspect22);
            this.groupBox4.Controls.Add(this.labelInspect12);
            this.groupBox4.Controls.Add(this.labelInspect31);
            this.groupBox4.Controls.Add(this.labelInspect21);
            this.groupBox4.Controls.Add(this.labelInspect11);
            this.groupBox4.Location = new System.Drawing.Point(16, 200);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(360, 232);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Statistics";
            // 
            // label72
            // 
            this.label72.BackColor = System.Drawing.SystemColors.Control;
            this.label72.Location = new System.Drawing.Point(272, 24);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(64, 20);
            this.label72.TabIndex = 33;
            this.label72.Text = "Extent";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label71
            // 
            this.label71.BackColor = System.Drawing.SystemColors.Control;
            this.label71.Location = new System.Drawing.Point(192, 24);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(80, 20);
            this.label71.TabIndex = 32;
            this.label71.Text = "Maximum";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label70
            // 
            this.label70.BackColor = System.Drawing.SystemColors.Control;
            this.label70.Location = new System.Drawing.Point(112, 24);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(80, 20);
            this.label70.TabIndex = 31;
            this.label70.Text = "Minimum";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label69
            // 
            this.label69.BackColor = System.Drawing.SystemColors.Control;
            this.label69.Location = new System.Drawing.Point(16, 24);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(96, 20);
            this.label69.TabIndex = 30;
            this.label69.Text = "Recognition";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label68
            // 
            this.label68.BackColor = System.Drawing.SystemColors.Control;
            this.label68.Location = new System.Drawing.Point(16, 202);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(96, 20);
            this.label68.TabIndex = 29;
            this.label68.Text = "Column Scale";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label67
            // 
            this.label67.BackColor = System.Drawing.SystemColors.Control;
            this.label67.Location = new System.Drawing.Point(16, 180);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(96, 20);
            this.label67.TabIndex = 28;
            this.label67.Text = "Row Scale";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label66
            // 
            this.label66.BackColor = System.Drawing.SystemColors.Control;
            this.label66.Location = new System.Drawing.Point(16, 158);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(96, 20);
            this.label66.TabIndex = 27;
            this.label66.Text = "Angle";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label65
            // 
            this.label65.BackColor = System.Drawing.SystemColors.Control;
            this.label65.Location = new System.Drawing.Point(16, 136);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(96, 20);
            this.label65.TabIndex = 26;
            this.label65.Text = "Column";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label64
            // 
            this.label64.BackColor = System.Drawing.SystemColors.Control;
            this.label64.Location = new System.Drawing.Point(16, 114);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(96, 20);
            this.label64.TabIndex = 25;
            this.label64.Text = "Row";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.SystemColors.Control;
            this.label63.Location = new System.Drawing.Point(16, 92);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(96, 20);
            this.label63.TabIndex = 24;
            this.label63.Text = "Pose Bounds";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.BackColor = System.Drawing.SystemColors.Control;
            this.label62.Location = new System.Drawing.Point(16, 70);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(96, 20);
            this.label62.TabIndex = 23;
            this.label62.Text = "Time (ms)";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.BackColor = System.Drawing.SystemColors.Control;
            this.label43.Location = new System.Drawing.Point(16, 48);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(96, 20);
            this.label43.TabIndex = 22;
            this.label43.Text = "Score";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInspect37
            // 
            this.labelInspect37.BackColor = System.Drawing.Color.White;
            this.labelInspect37.Location = new System.Drawing.Point(272, 202);
            this.labelInspect37.Name = "labelInspect37";
            this.labelInspect37.Size = new System.Drawing.Size(76, 20);
            this.labelInspect37.TabIndex = 21;
            this.labelInspect37.Text = "-";
            this.labelInspect37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect27
            // 
            this.labelInspect27.BackColor = System.Drawing.Color.White;
            this.labelInspect27.Location = new System.Drawing.Point(192, 202);
            this.labelInspect27.Name = "labelInspect27";
            this.labelInspect27.Size = new System.Drawing.Size(76, 20);
            this.labelInspect27.TabIndex = 20;
            this.labelInspect27.Text = "-";
            this.labelInspect27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect17
            // 
            this.labelInspect17.BackColor = System.Drawing.Color.White;
            this.labelInspect17.Location = new System.Drawing.Point(112, 202);
            this.labelInspect17.Name = "labelInspect17";
            this.labelInspect17.Size = new System.Drawing.Size(76, 20);
            this.labelInspect17.TabIndex = 19;
            this.labelInspect17.Text = "-";
            this.labelInspect17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect36
            // 
            this.labelInspect36.BackColor = System.Drawing.Color.White;
            this.labelInspect36.Location = new System.Drawing.Point(272, 180);
            this.labelInspect36.Name = "labelInspect36";
            this.labelInspect36.Size = new System.Drawing.Size(76, 20);
            this.labelInspect36.TabIndex = 18;
            this.labelInspect36.Text = "-";
            this.labelInspect36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect26
            // 
            this.labelInspect26.BackColor = System.Drawing.Color.White;
            this.labelInspect26.Location = new System.Drawing.Point(192, 180);
            this.labelInspect26.Name = "labelInspect26";
            this.labelInspect26.Size = new System.Drawing.Size(76, 20);
            this.labelInspect26.TabIndex = 17;
            this.labelInspect26.Text = "-";
            this.labelInspect26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect16
            // 
            this.labelInspect16.BackColor = System.Drawing.Color.White;
            this.labelInspect16.Location = new System.Drawing.Point(112, 180);
            this.labelInspect16.Name = "labelInspect16";
            this.labelInspect16.Size = new System.Drawing.Size(76, 20);
            this.labelInspect16.TabIndex = 16;
            this.labelInspect16.Text = "-";
            this.labelInspect16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect35
            // 
            this.labelInspect35.BackColor = System.Drawing.Color.White;
            this.labelInspect35.Location = new System.Drawing.Point(272, 158);
            this.labelInspect35.Name = "labelInspect35";
            this.labelInspect35.Size = new System.Drawing.Size(76, 20);
            this.labelInspect35.TabIndex = 15;
            this.labelInspect35.Text = "-";
            this.labelInspect35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect25
            // 
            this.labelInspect25.BackColor = System.Drawing.Color.White;
            this.labelInspect25.Location = new System.Drawing.Point(192, 158);
            this.labelInspect25.Name = "labelInspect25";
            this.labelInspect25.Size = new System.Drawing.Size(76, 20);
            this.labelInspect25.TabIndex = 14;
            this.labelInspect25.Text = "-";
            this.labelInspect25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect15
            // 
            this.labelInspect15.BackColor = System.Drawing.Color.White;
            this.labelInspect15.Location = new System.Drawing.Point(112, 158);
            this.labelInspect15.Name = "labelInspect15";
            this.labelInspect15.Size = new System.Drawing.Size(76, 20);
            this.labelInspect15.TabIndex = 13;
            this.labelInspect15.Text = "-";
            this.labelInspect15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect34
            // 
            this.labelInspect34.BackColor = System.Drawing.Color.White;
            this.labelInspect34.Location = new System.Drawing.Point(272, 136);
            this.labelInspect34.Name = "labelInspect34";
            this.labelInspect34.Size = new System.Drawing.Size(76, 20);
            this.labelInspect34.TabIndex = 12;
            this.labelInspect34.Text = "-";
            this.labelInspect34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect24
            // 
            this.labelInspect24.BackColor = System.Drawing.Color.White;
            this.labelInspect24.Location = new System.Drawing.Point(192, 136);
            this.labelInspect24.Name = "labelInspect24";
            this.labelInspect24.Size = new System.Drawing.Size(76, 20);
            this.labelInspect24.TabIndex = 11;
            this.labelInspect24.Text = "-";
            this.labelInspect24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect14
            // 
            this.labelInspect14.BackColor = System.Drawing.Color.White;
            this.labelInspect14.Location = new System.Drawing.Point(112, 136);
            this.labelInspect14.Name = "labelInspect14";
            this.labelInspect14.Size = new System.Drawing.Size(76, 20);
            this.labelInspect14.TabIndex = 10;
            this.labelInspect14.Text = "-";
            this.labelInspect14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect33
            // 
            this.labelInspect33.BackColor = System.Drawing.Color.White;
            this.labelInspect33.Location = new System.Drawing.Point(272, 114);
            this.labelInspect33.Name = "labelInspect33";
            this.labelInspect33.Size = new System.Drawing.Size(76, 20);
            this.labelInspect33.TabIndex = 9;
            this.labelInspect33.Text = "-";
            this.labelInspect33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect23
            // 
            this.labelInspect23.BackColor = System.Drawing.Color.White;
            this.labelInspect23.Location = new System.Drawing.Point(192, 114);
            this.labelInspect23.Name = "labelInspect23";
            this.labelInspect23.Size = new System.Drawing.Size(76, 20);
            this.labelInspect23.TabIndex = 8;
            this.labelInspect23.Text = "-";
            this.labelInspect23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect13
            // 
            this.labelInspect13.BackColor = System.Drawing.Color.White;
            this.labelInspect13.Location = new System.Drawing.Point(112, 114);
            this.labelInspect13.Name = "labelInspect13";
            this.labelInspect13.Size = new System.Drawing.Size(76, 20);
            this.labelInspect13.TabIndex = 7;
            this.labelInspect13.Text = "-";
            this.labelInspect13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect32
            // 
            this.labelInspect32.BackColor = System.Drawing.Color.White;
            this.labelInspect32.Location = new System.Drawing.Point(272, 70);
            this.labelInspect32.Name = "labelInspect32";
            this.labelInspect32.Size = new System.Drawing.Size(76, 20);
            this.labelInspect32.TabIndex = 6;
            this.labelInspect32.Text = "-";
            this.labelInspect32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect22
            // 
            this.labelInspect22.BackColor = System.Drawing.Color.White;
            this.labelInspect22.Location = new System.Drawing.Point(192, 70);
            this.labelInspect22.Name = "labelInspect22";
            this.labelInspect22.Size = new System.Drawing.Size(76, 20);
            this.labelInspect22.TabIndex = 5;
            this.labelInspect22.Text = "-";
            this.labelInspect22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect12
            // 
            this.labelInspect12.BackColor = System.Drawing.Color.White;
            this.labelInspect12.Location = new System.Drawing.Point(112, 70);
            this.labelInspect12.Name = "labelInspect12";
            this.labelInspect12.Size = new System.Drawing.Size(76, 20);
            this.labelInspect12.TabIndex = 4;
            this.labelInspect12.Text = "-";
            this.labelInspect12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect31
            // 
            this.labelInspect31.BackColor = System.Drawing.Color.White;
            this.labelInspect31.Location = new System.Drawing.Point(272, 48);
            this.labelInspect31.Name = "labelInspect31";
            this.labelInspect31.Size = new System.Drawing.Size(76, 20);
            this.labelInspect31.TabIndex = 2;
            this.labelInspect31.Text = "-";
            this.labelInspect31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect21
            // 
            this.labelInspect21.BackColor = System.Drawing.Color.White;
            this.labelInspect21.Location = new System.Drawing.Point(192, 48);
            this.labelInspect21.Name = "labelInspect21";
            this.labelInspect21.Size = new System.Drawing.Size(76, 20);
            this.labelInspect21.TabIndex = 1;
            this.labelInspect21.Text = "-";
            this.labelInspect21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect11
            // 
            this.labelInspect11.BackColor = System.Drawing.Color.White;
            this.labelInspect11.Location = new System.Drawing.Point(112, 48);
            this.labelInspect11.Name = "labelInspect11";
            this.labelInspect11.Size = new System.Drawing.Size(76, 20);
            this.labelInspect11.TabIndex = 0;
            this.labelInspect11.Text = "-";
            this.labelInspect11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label39);
            this.groupBox3.Controls.Add(this.label38);
            this.groupBox3.Controls.Add(this.label37);
            this.groupBox3.Controls.Add(this.label36);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.label27);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.labelInspect05);
            this.groupBox3.Controls.Add(this.labelInspect04);
            this.groupBox3.Controls.Add(this.labelInspect03);
            this.groupBox3.Controls.Add(this.labelInspect02);
            this.groupBox3.Controls.Add(this.labelInspect01);
            this.groupBox3.Location = new System.Drawing.Point(16, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(360, 176);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Recognition Rate";
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(56, 136);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(120, 32);
            this.label39.TabIndex = 11;
            this.label39.Text = "in rel. to maximum No. of matches";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(56, 104);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(120, 32);
            this.label38.TabIndex = 10;
            this.label38.Text = "in rel. to specified No. of vis. models";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label37
            // 
            this.label37.Location = new System.Drawing.Point(56, 72);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(120, 32);
            this.label37.TabIndex = 9;
            this.label37.Text = "with the maximum number of matches";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(56, 40);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(120, 32);
            this.label36.TabIndex = 8;
            this.label36.Text = "with at least the spec.  number of models";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(56, 16);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(120, 24);
            this.label28.TabIndex = 7;
            this.label28.Text = "with at least one match";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(8, 112);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(56, 24);
            this.label27.TabIndex = 6;
            this.label27.Text = "Matches:";
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(8, 16);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(48, 24);
            this.label26.TabIndex = 5;
            this.label26.Text = "Images:";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect05
            // 
            this.labelInspect05.BackColor = System.Drawing.Color.White;
            this.labelInspect05.Location = new System.Drawing.Point(176, 140);
            this.labelInspect05.Name = "labelInspect05";
            this.labelInspect05.Size = new System.Drawing.Size(172, 24);
            this.labelInspect05.TabIndex = 4;
            this.labelInspect05.Text = "100.00 % (1 of 1 model)";
            this.labelInspect05.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect04
            // 
            this.labelInspect04.BackColor = System.Drawing.Color.White;
            this.labelInspect04.Location = new System.Drawing.Point(174, 112);
            this.labelInspect04.Name = "labelInspect04";
            this.labelInspect04.Size = new System.Drawing.Size(174, 24);
            this.labelInspect04.TabIndex = 3;
            this.labelInspect04.Text = "100.00 % (1 of 1 model)";
            this.labelInspect04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect03
            // 
            this.labelInspect03.BackColor = System.Drawing.Color.White;
            this.labelInspect03.Location = new System.Drawing.Point(174, 74);
            this.labelInspect03.Name = "labelInspect03";
            this.labelInspect03.Size = new System.Drawing.Size(174, 24);
            this.labelInspect03.TabIndex = 2;
            this.labelInspect03.Text = "100.00 % (1 of 1 image)";
            this.labelInspect03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect02
            // 
            this.labelInspect02.BackColor = System.Drawing.Color.White;
            this.labelInspect02.Location = new System.Drawing.Point(174, 46);
            this.labelInspect02.Name = "labelInspect02";
            this.labelInspect02.Size = new System.Drawing.Size(174, 24);
            this.labelInspect02.TabIndex = 1;
            this.labelInspect02.Text = "100.00 % (1 of 1 image)";
            this.labelInspect02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspect01
            // 
            this.labelInspect01.BackColor = System.Drawing.Color.White;
            this.labelInspect01.Location = new System.Drawing.Point(174, 18);
            this.labelInspect01.Name = "labelInspect01";
            this.labelInspect01.Size = new System.Drawing.Size(174, 24);
            this.labelInspect01.TabIndex = 0;
            this.labelInspect01.Text = "100.00 % (1 of 1  image)";
            this.labelInspect01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatisticsButton
            // 
            this.StatisticsButton.Location = new System.Drawing.Point(272, 440);
            this.StatisticsButton.Name = "StatisticsButton";
            this.StatisticsButton.Size = new System.Drawing.Size(104, 32);
            this.StatisticsButton.TabIndex = 0;
            this.StatisticsButton.Text = "Run";
            this.StatisticsButton.Click += new System.EventHandler(this.runStatisticsButton_Click);
            // 
            // groupBoxCreateROI
            // 
            this.groupBoxCreateROI.Controls.Add(this.circleButton);
            this.groupBoxCreateROI.Controls.Add(this.delROIButton);
            this.groupBoxCreateROI.Controls.Add(this.delAllROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect2Button);
            this.groupBoxCreateROI.Controls.Add(this.subFromROIButton);
            this.groupBoxCreateROI.Controls.Add(this.addToROIButton);
            this.groupBoxCreateROI.Controls.Add(this.rect1Button);
            this.groupBoxCreateROI.Enabled = false;
            this.groupBoxCreateROI.Location = new System.Drawing.Point(336, 416);
            this.groupBoxCreateROI.Name = "groupBoxCreateROI";
            this.groupBoxCreateROI.Size = new System.Drawing.Size(168, 144);
            this.groupBoxCreateROI.TabIndex = 2;
            this.groupBoxCreateROI.TabStop = false;
            this.groupBoxCreateROI.Text = "Create ROI";
            // 
            // circleButton
            // 
            this.circleButton.Location = new System.Drawing.Point(16, 110);
            this.circleButton.Name = "circleButton";
            this.circleButton.Size = new System.Drawing.Size(48, 24);
            this.circleButton.TabIndex = 12;
            this.circleButton.Text = "Circle";
            this.circleButton.Click += new System.EventHandler(this.circleButton_Click);
            // 
            // delROIButton
            // 
            this.delROIButton.Location = new System.Drawing.Point(80, 64);
            this.delROIButton.Name = "delROIButton";
            this.delROIButton.Size = new System.Drawing.Size(72, 24);
            this.delROIButton.TabIndex = 11;
            this.delROIButton.Text = "Del ActROI ";
            this.delROIButton.Click += new System.EventHandler(this.delROIButton_Click);
            // 
            // delAllROIButton
            // 
            this.delAllROIButton.Location = new System.Drawing.Point(80, 96);
            this.delAllROIButton.Name = "delAllROIButton";
            this.delAllROIButton.Size = new System.Drawing.Size(72, 24);
            this.delAllROIButton.TabIndex = 10;
            this.delAllROIButton.Text = "Del All ROI";
            this.delAllROIButton.Click += new System.EventHandler(this.delAllROIButton_Click);
            // 
            // rect2Button
            // 
            this.rect2Button.Location = new System.Drawing.Point(16, 84);
            this.rect2Button.Name = "rect2Button";
            this.rect2Button.Size = new System.Drawing.Size(48, 24);
            this.rect2Button.TabIndex = 9;
            this.rect2Button.Text = "Rect2";
            this.rect2Button.Click += new System.EventHandler(this.rect2Button_Click);
            // 
            // subFromROIButton
            // 
            this.subFromROIButton.Location = new System.Drawing.Point(104, 16);
            this.subFromROIButton.Name = "subFromROIButton";
            this.subFromROIButton.Size = new System.Drawing.Size(40, 40);
            this.subFromROIButton.TabIndex = 8;
            this.subFromROIButton.Text = "( - )";
            this.subFromROIButton.Click += new System.EventHandler(this.subFromROIButton_Click);
            // 
            // addToROIButton
            // 
            this.addToROIButton.Checked = true;
            this.addToROIButton.Location = new System.Drawing.Point(32, 16);
            this.addToROIButton.Name = "addToROIButton";
            this.addToROIButton.Size = new System.Drawing.Size(40, 40);
            this.addToROIButton.TabIndex = 7;
            this.addToROIButton.TabStop = true;
            this.addToROIButton.Text = "(+)";
            this.addToROIButton.Click += new System.EventHandler(this.addToROIButton_Click);
            // 
            // rect1Button
            // 
            this.rect1Button.Location = new System.Drawing.Point(16, 58);
            this.rect1Button.Name = "rect1Button";
            this.rect1Button.Size = new System.Drawing.Size(48, 24);
            this.rect1Button.TabIndex = 5;
            this.rect1Button.Text = "Rect1";
            this.rect1Button.Click += new System.EventHandler(this.rect1Button_Click);
            // 
            // loadImagebutton
            // 
            this.loadImagebutton.Location = new System.Drawing.Point(56, 420);
            this.loadImagebutton.Name = "loadImagebutton";
            this.loadImagebutton.Size = new System.Drawing.Size(88, 22);
            this.loadImagebutton.TabIndex = 0;
            this.loadImagebutton.Text = "Load Image";
            this.loadImagebutton.Click += new System.EventHandler(this.loadImage);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "png (*.png)|*.png|tiff (*.tif)|*.tif|jpeg (*.jpg)| *.jpg|all files (*.*)|*.*";
            this.openFileDialog1.FilterIndex = 4;
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // saveModelbutton
            // 
            this.saveModelbutton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.saveModelbutton.Location = new System.Drawing.Point(56, 482);
            this.saveModelbutton.Name = "saveModelbutton";
            this.saveModelbutton.Size = new System.Drawing.Size(88, 22);
            this.saveModelbutton.TabIndex = 3;
            this.saveModelbutton.Text = "Save Model";
            this.saveModelbutton.Click += new System.EventHandler(this.saveModelbutton_Click);
            // 
            // loadModelbutton
            // 
            this.loadModelbutton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.loadModelbutton.Location = new System.Drawing.Point(56, 506);
            this.loadModelbutton.Name = "loadModelbutton";
            this.loadModelbutton.Size = new System.Drawing.Size(88, 22);
            this.loadModelbutton.TabIndex = 4;
            this.loadModelbutton.Text = "Load Model";
            this.loadModelbutton.Click += new System.EventHandler(this.loadModelbutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.resetViewButton);
            this.groupBox2.Controls.Add(this.noneButton);
            this.groupBox2.Controls.Add(this.zoomButton);
            this.groupBox2.Controls.Add(this.moveButton);
            this.groupBox2.Location = new System.Drawing.Point(192, 416);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(112, 144);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "View Interaction";
            // 
            // resetViewButton
            // 
            this.resetViewButton.Location = new System.Drawing.Point(14, 116);
            this.resetViewButton.Name = "resetViewButton";
            this.resetViewButton.Size = new System.Drawing.Size(84, 20);
            this.resetViewButton.TabIndex = 3;
            this.resetViewButton.Text = "Reset View";
            this.resetViewButton.Click += new System.EventHandler(this.resetViewButton_Click);
            // 
            // noneButton
            // 
            this.noneButton.Checked = true;
            this.noneButton.Location = new System.Drawing.Point(16, 16);
            this.noneButton.Name = "noneButton";
            this.noneButton.Size = new System.Drawing.Size(72, 32);
            this.noneButton.TabIndex = 2;
            this.noneButton.TabStop = true;
            this.noneButton.Text = "none";
            this.noneButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noneButton.CheckedChanged += new System.EventHandler(this.noneButton_CheckedChanged);
            // 
            // zoomButton
            // 
            this.zoomButton.Location = new System.Drawing.Point(16, 80);
            this.zoomButton.Name = "zoomButton";
            this.zoomButton.Size = new System.Drawing.Size(72, 32);
            this.zoomButton.TabIndex = 1;
            this.zoomButton.Text = "zoom";
            this.zoomButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.zoomButton.CheckedChanged += new System.EventHandler(this.zoomButton_CheckedChanged);
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(16, 48);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(72, 32);
            this.moveButton.TabIndex = 0;
            this.moveButton.Text = "move";
            this.moveButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.moveButton.CheckedChanged += new System.EventHandler(this.moveButton_CheckedChanged);
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.Filter = "png (*.png)|*.png|tiff (*.tif)|*.tif|jpeg (*.jpg)| *.jpg|all files (*.*)|*.*";
            this.openFileDialog2.FilterIndex = 4;
            this.openFileDialog2.Multiselect = true;
            // 
            // DisplayDataButton
            // 
            this.DisplayDataButton.BackColor = System.Drawing.SystemColors.Control;
            this.DisplayDataButton.Location = new System.Drawing.Point(56, 444);
            this.DisplayDataButton.Name = "DisplayDataButton";
            this.DisplayDataButton.Size = new System.Drawing.Size(88, 22);
            this.DisplayDataButton.TabIndex = 7;
            this.DisplayDataButton.Text = "Display";
            this.DisplayDataButton.UseVisualStyleBackColor = false;
            this.DisplayDataButton.Click += new System.EventHandler(this.DisplayDataButton_Click);
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_ticked);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "HALCON ShapeModel File|*.shm|All files|*.*";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // openFileDialog3
            // 
            this.openFileDialog3.Filter = "HALCON ShapeModel File|*.shm|All files|*.*";
            // 
            // resetModelbutton
            // 
            this.resetModelbutton.Enabled = false;
            this.resetModelbutton.Location = new System.Drawing.Point(56, 530);
            this.resetModelbutton.Name = "resetModelbutton";
            this.resetModelbutton.Size = new System.Drawing.Size(88, 22);
            this.resetModelbutton.TabIndex = 8;
            this.resetModelbutton.Text = "Clear Model";
            this.resetModelbutton.Click += new System.EventHandler(this.resetModelbutton_Click);
            // 
            // WindowModePanel
            // 
            this.WindowModePanel.BackColor = System.Drawing.Color.RoyalBlue;
            this.WindowModePanel.Location = new System.Drawing.Point(52, 36);
            this.WindowModePanel.Name = "WindowModePanel";
            this.WindowModePanel.Size = new System.Drawing.Size(456, 352);
            this.WindowModePanel.TabIndex = 9;
            // 
            // MatchingForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(954, 580);
            this.Controls.Add(this.resetModelbutton);
            this.Controls.Add(this.DisplayDataButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.loadModelbutton);
            this.Controls.Add(this.saveModelbutton);
            this.Controls.Add(this.groupBoxCreateROI);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.viewPort);
            this.Controls.Add(this.loadImagebutton);
            this.Controls.Add(this.WindowModePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(960, 608);
            this.MinimumSize = new System.Drawing.Size(960, 608);
            this.Name = "MatchingForm";
            this.Text = "Matching Assistant";
            this.tabControl.ResumeLayout(false);
            this.tabPageCreateModel.ResumeLayout(false);
            this.groupBoxCreateModel.ResumeLayout(false);
            this.groupBoxCreateModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MinContrastTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContrastUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PyramidLevelTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PyramidLevelUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleStepTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleStepUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleExtentTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AngleExtentUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingAngleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartingAngleUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleStepUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleStepTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxScaleUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxScaleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScaleUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScaleTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ContrastTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DispPyramidUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DispPyramidTrackBar)).EndInit();
            this.tabPageFindModel.ResumeLayout(false);
            this.tabPageFindModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LastPyrLevTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastPyrLevUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxOverlapTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxOverlapUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreedinessTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GreedinessUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMatchesTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumMatchesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScoreTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinScoreUpDown)).EndInit();
            this.tabPageOptimRecognition.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RecogNoManSelUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recogRateTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recogRateUpDown)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPageInspectModel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InspectMaxNoMatchUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBoxCreateROI.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            try
            {
			    Application.Run(new MatchingForm());
            }
            catch (TypeLoadException e)
            {
                MessageBox.Show(e.StackTrace.ToString());
            }
		}


		/// <summary> 
        /// Updates all model parameters to the initial settings of the GUI 
        /// components.  
        /// </summary>
        /// <param name="parSet">holds all necessary parameter values</param>
        private void Init(MatchingParam parSet)
		{
			this.PyramidLevelUpDown.Value = 5;
			this.MinContrastUpDown.Maximum = this.ContrastUpDown.Value;
			this.MinContrastTrackBar.Maximum = (int)this.ContrastUpDown.Value;
			this.MetricBox.SelectedIndex = 0;
			this.OptimizationBox.SelectedIndex = 0;
			this.SubPixelBox.SelectedIndex = 2;
			this.RecognRateComboBox.SelectedIndex = 0;
			
			parSet.mAngleExtent	  = (double)(AngleExtentUpDown.Value)*Math.PI/180.0; 
			parSet.mAngleStep	  = ((double)AngleStepUpDown.Value /10.0)*Math.PI/180.0;
			parSet.mContrast      = (int) ContrastUpDown.Value;
			parSet.mMaxScale	  = (double)MaxScaleUpDown.Value/100.0;
			parSet.mMetric		  = (string)MetricBox.Items[MetricBox.SelectedIndex];
			parSet.mMinContrast   = (int) MinContrastUpDown.Value;
			parSet.mMinScale	  = (double)MinScaleUpDown.Value/100.0;
			parSet.mNumLevel      = (int)PyramidLevelUpDown.Value;
			parSet.mOptimization  = (string)OptimizationBox.Items[OptimizationBox.SelectedIndex];
			parSet.mScaleStep	  = (double)ScaleStepUpDown.Value/1000.0;
			parSet.mStartingAngle = (double)(StartingAngleUpDown.Value)*Math.PI/180.0; 
			
			parSet.mMinScore		= (double)MinScoreUpDown.Value/100.0;
			parSet.mNumMatches		= (int)NumMatchesUpDown.Value;
			parSet.mGreediness		= (double)GreedinessUpDown.Value/100.0;
			parSet.mMaxOverlap		= (double)MaxOverlapUpDown.Value/100.0;
			parSet.mSubpixel		= (string)SubPixelBox.Items[SubPixelBox.SelectedIndex];
			parSet.mLastPyramidLevel= (int)LastPyrLevUpDown.Value;

			parSet.mRecogRateOpt		=  (int)RecognRateComboBox.SelectedIndex;
			parSet.mRecogRate			=  (int)recogRateUpDown.Value;
			parSet.mRecogSpeedMode		=  MatchingParam.RECOGM_MANUALSELECT;
			parSet.mRecogManualSel		=  (int)RecogNoManSelUpDown.Value;
			parSet.mInspectMaxNoMatch	=  (int)InspectMaxNoMatchUpDown.Value;

      string imPathValue = (string)(HSystem.GetSystem("image_dir").TupleSplit(";"));
            openFileDialog1.InitialDirectory = imPathValue;

            openFileDialog2.InitialDirectory = imPathValue;

            openFileDialog3.InitialDirectory = 
            Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);

            saveFileDialog1.InitialDirectory =
            Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
		}

		/********************************************************************/
		/********************************************************************/
		private void loadImage(object sender, System.EventArgs e)
		{
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				locked = true;

				DetectionContour = null;
				DispPyramidTrackBar.Enabled = false;
				DispPyramidTrackBar.Value = 1;
				DispPyramidUpDown.Enabled = false;
				DispPyramidUpDown.Value = 1;

				locked = false;
                
                FindAlwaysCheckBox.Checked = false;
				
				if(!mAssistant.setImage(openFileDialog1.FileName))
                    return;

                mView.resetAll(); 
                
                if(mAssistant.onExternalModelID)
                    ModelContour = mAssistant.getLoadedModelContour();

                // to add ROI instances to the display as well                                
                if(tabControl.SelectedIndex != 0)
                {
                    tabControl.SelectedIndex = 0;
                }
                else
                {
                    changeWindowMode(1);
                    CreateModelGraphics();
                    mView.repaint();                   
                }				
			}
		}

		/********************************************************************/
		/********************************************************************/
		private void DisplayDataButton_Click(object sender, System.EventArgs e)
		{        
			changeWindowMode(1);
            CreateModelGraphics();
			mView.repaint(); 			
		}

		/********************************************************************/
		/********************************************************************/
		private void saveModelbutton_Click(object sender, System.EventArgs e)
		{
			string files;

			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				files = saveFileDialog1.FileName;
				
				if(!files.EndsWith(".shm") && !files.EndsWith(".SHM"))
					files += ".shm";
				
				mAssistant.saveShapeModel(files);
			}/* if */
		}

		/********************************************************************/
		/********************************************************************/
		private void loadModelbutton_Click(object sender, System.EventArgs e)
		{
			string file;
			
			if(openFileDialog3.ShowDialog() == DialogResult.OK)
			{
				file = openFileDialog3.FileName;

				if(!file.EndsWith(".shm") || !file.EndsWith(".shm"))
				{
					UpdateMatching(MatchingAssistant.ERR_NO_VALID_FILE);
					return;
				}

                roiController.reset();  
                DetectionContour = null;
                mAssistant.reset();
                FindAlwaysCheckBox.Checked = false;

				if(mAssistant.loadShapeModel(file))
                {
                    groupBoxCreateROI.Enabled   = false;
                    groupBoxCreateModel.Enabled = false;
                    resetModelbutton.Enabled    = true;
                    saveModelbutton.Enabled     = false;

                   
                    ModelContour = mAssistant.getLoadedModelContour();
                     
                    if(tabControl.SelectedIndex != 0)
                    {
                        tabControl.SelectedIndex = 0;
                    }
                    else
                    {
                        changeWindowMode(1);
                        CreateModelGraphics();
                        mView.repaint();
                    }
                }

			}/* if */
		}

		/********************************************************************/
		/********************************************************************/
		private void resetModelbutton_Click(object sender, System.EventArgs e)
		{
            
			groupBoxCreateModel.Enabled = true;			
			mAssistant.onExternalModelID = false;
			FindAlwaysCheckBox.Checked = false;
            resetModelbutton.Enabled   = false;
            saveModelbutton.Enabled    = true;

			DetectionContour = null;
            ModelContour     = null;

            if(mAssistant.getDispImage()!=null)
                groupBoxCreateROI.Enabled = true;
			
			speedOptHandler.reset();
			optInstance = speedOptHandler;
			UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_UPDATE_VALS);
			UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_OPTIMUM_VALS);
			UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_STATISTICS_STATUS);

			inspectOptHandler.reset();
			optInstance = inspectOptHandler;
			UpdateStatisticsData(MatchingOpt.UPDATE_INSP_STATISTICS);
			UpdateStatisticsData(MatchingOpt.UPDATE_INSP_RECOGRATE);
            
            //reset Model parameters
            mAssistant.resetCachedModelParams();
          
            if(tabControl.SelectedIndex != 0)
            {
                tabControl.SelectedIndex = 0;
            }
            else
            {
                changeWindowMode(1);
                CreateModelGraphics();
                mView.repaint();
            }
		}

		/********************************************************************/
		/********************************************************************/
		private void rect1Button_Click(object sender, System.EventArgs e)
		{
			roiController.setROIShape(new ROIRectangle1());
		}

		/********************************************************************/
		/********************************************************************/
		private void rect2Button_Click(object sender, System.EventArgs e)
		{
			roiController.setROIShape(new ROIRectangle2());
		}

        
        private void circleButton_Click(object sender, System.EventArgs e)
        {
            roiController.setROIShape(new ROICircle());
        }
		/********************************************************************/
		/********************************************************************/
		private void moveButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_MOVE);
		}

		/********************************************************************/
		/********************************************************************/
		private void zoomButton_CheckedChanged(object sender, System.EventArgs e)
		{
			mView.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
		}

		/********************************************************************/
		/********************************************************************/
		private void noneButton_CheckedChanged(object sender, System.EventArgs e)
        {
        	mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }

		/********************************************************************/
		/********************************************************************/
		private void delROIButton_Click(object sender, System.EventArgs e)
		{
			roiController.removeActive();            
		}
		
		/********************************************************************/
		/********************************************************************/
		private void delAllROIButton_Click(object sender, System.EventArgs e)
        {
          roiController.reset();  
          DetectionContour = null;
          mAssistant.reset();

          DispPyramidTrackBar.Value = 1;
          DispPyramidTrackBar.Enabled = false;
          DispPyramidUpDown.Value = 1;
          DispPyramidUpDown.Enabled = false;	
        }
     
        /********************************************************************/
        /********************************************************************/
        private void resetViewButton_Click(object sender, System.EventArgs e)
        {
            locked = true;			
            mView.resetWindow();            
            locked = false;
            mView.repaint();            

        }

		/********************************************************************/
		/********************************************************************/
		private void addToROIButton_Click(object sender, System.EventArgs e)
		{
            if(addToROIButton.Checked)
                roiController.setROISign(ROIController.MODE_ROI_POS);                
           
		}

		/********************************************************************/
		/********************************************************************/
		private void subFromROIButton_Click(object sender, System.EventArgs e)
		{
            if(subFromROIButton.Checked)
                roiController.setROISign(ROIController.MODE_ROI_NEG);              
           
		}


		/****************************************************************************/
		/******************  1. Tab: CREATE MODEL ***********************************/
		/********************** Eventhandling ***************************************/
		/****************************************************************************/

		/********************************************************************/
		/*                           DispPyramid                            */
		/********************************************************************/
		private void DispPyramidUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)DispPyramidUpDown.Value;
			DispPyramidTrackBar.Value = val;
			
			
			mView.setDispLevel(1);
            
			if(!locked)
				setDisplayLevel(val);
		}

		private void DispPyramidTrackBar_Scroll(object sender, System.EventArgs e)
		{
			DispPyramidUpDown.Value = DispPyramidTrackBar.Value;
			DispPyramidUpDown.Refresh();
		}

		private void setDisplayLevel(int val)
		{
			mAssistant.setDispLevel(val);
		}

		/********************************************************************/
		/*  Auto                      Contrast                              */
		/********************************************************************/
		private void ContrastUpDown_ValueChanged(object sender, System.EventArgs e)
		{

			int val = (int)ContrastUpDown.Value;
			ContrastTrackBar.Value = val;
			MinContrastTrackBar.Maximum = val;
			MinContrastUpDown.Maximum = val;

			if(!locked)
				setContrast(val);

			if(!parameterSet.isAuto(MatchingParam.AUTO_CONTRAST))
			{
				ContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				ContrastAutoButton.Text = "Auto";
			}
		}


		private void ContrastTrackBar_Scroll(object sender, System.EventArgs e)
		{
			ContrastUpDown.Value = ContrastTrackBar.Value;		
			ContrastUpDown.Refresh();
		}


		private void ContrastAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_CONTRAST))
			{
				ContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				ContrastAutoButton.Text = "Auto";
			}
			else 
			{
				ContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				ContrastAutoButton.Text		 = "Auto!";
				mAssistant.setAuto(MatchingParam.AUTO_CONTRAST);
			}
		}

		private void setContrast(int val)
		{
			mAssistant.setContrast(val);
		}

		/********************************************************************/
		/*                             MinScale                             */
		/********************************************************************/
		private void MinScaleUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)MinScaleUpDown.Value;
			MinScaleTrackBar.Value = val;
			
			if(val >= MaxScaleUpDown.Value)
				MaxScaleUpDown.Value = val;

			if(!locked)
				setMinScale(val);
		}

		private void MinScaleTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MinScaleUpDown.Value =  MinScaleTrackBar.Value;
			MinScaleUpDown.Refresh();
		}

		private void setMinScale(int val)
		{
			mAssistant.setMinScale((double)val/100);		
		}

		/********************************************************************/
		/*                          MaxScale                                */
		/********************************************************************/
		private void MaxScaleUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val  = (int)MaxScaleUpDown.Value; 
			MaxScaleTrackBar.Value = val;

			if(val<= MinScaleUpDown.Value)
				MinScaleUpDown.Value = val;

			if(!locked)
				setMaxScale(val);
		}

		private void MaxScaleTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MaxScaleUpDown.Value = MaxScaleTrackBar.Value;
			MaxScaleUpDown.Refresh();
		}

		private void setMaxScale(int val)
		{
			mAssistant.setMaxScale((double)val/100);
		}

		/********************************************************************/
		/* Auto                     scaleStep                               */
		/********************************************************************/
		private void ScaleStepUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val =  (int)ScaleStepUpDown.Value;
			ScaleStepTrackBar.Value = val;

			if(!locked)
				setScaleStep(val);

			if(!parameterSet.isAuto(MatchingParam.AUTO_SCALE_STEP))
			{
				ScaleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				ScaleStepAutoButton.Text = "Auto";
			}
		}

		private void ScaleStepTrackBar_Scroll(object sender, System.EventArgs e)
		{
			ScaleStepUpDown.Value = ScaleStepTrackBar.Value;
			ScaleStepUpDown.Refresh();
		}

		private void ScaleStepAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_SCALE_STEP))
			{
				ScaleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				ScaleStepAutoButton.Text = "Auto";
			}
			else
			{
				ScaleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				ScaleStepAutoButton.Text = "Auto!";	
				mAssistant.setAuto(MatchingParam.AUTO_SCALE_STEP);
			}
		}


		private void setScaleStep(int val)
		{
			mAssistant.setScaleStep((double)val/1000);
		}

		/********************************************************************/
		/*                          startingAngle                           */
		/********************************************************************/
		private void StartingAngleUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)StartingAngleUpDown.Value;
			StartingAngleTrackBar.Value = val;

			if(!locked)
				setStartingAngle(val);
		}

		private void StartingAngleTrackBar_Scroll(object sender, System.EventArgs e)
		{
			StartingAngleUpDown.Value = StartingAngleTrackBar.Value;
			StartingAngleUpDown.Refresh();
		}

		private void setStartingAngle(int val)
		{
			double rad = val*Math.PI/180;
			mAssistant.setStartingAngle(rad);
		}

		/********************************************************************/
		/*                          angleExtent                             */
		/********************************************************************/
		private void AngleExtentUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int) AngleExtentUpDown.Value;
			AngleExtentTrackBar.Value = val;

			if(!locked)
				setAngleExtent(val);
		}

		private void AngleExtentTrackBar_Scroll(object sender, System.EventArgs e)
		{
			AngleExtentUpDown.Value  = AngleExtentTrackBar.Value;
			AngleExtentUpDown.Refresh();
		}

		private void setAngleExtent(int val)
		{
			double rad = val*Math.PI/180;
			mAssistant.setAngleExtent(rad);
		}

		/********************************************************************/
		/* Auto                      angleStep                              */
		/********************************************************************/
		private void AngleStepUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)AngleStepUpDown.Value;
			AngleStepTrackBar.Value = val;

			if(!locked)
				setAngleStep(val);

			if(!parameterSet.isAuto(MatchingParam.AUTO_ANGLE_STEP))
			{
				AngleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				AngleStepAutoButton.Text = "Auto";
			}
		}

		private void AngleStepTrackBar_Scroll(object sender, System.EventArgs e)
		{	
			AngleStepUpDown.Value  = AngleStepTrackBar.Value; 
			AngleStepUpDown.Refresh();
		}

		private void AngleStepAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_ANGLE_STEP))
			{
				AngleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				AngleStepAutoButton.Text = "Auto";
			}
			else
			{
				AngleStepAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				AngleStepAutoButton.Text = "Auto!";	
				mAssistant.setAuto(MatchingParam.AUTO_ANGLE_STEP);
			}
		}

		private void setAngleStep(int val)
		{
			double rad = (double)(val/10.0)*Math.PI/180.0;
			mAssistant.setAngleStep(rad);
		}

		/********************************************************************/
		/*  Auto                  pyramidLevel                              */
		/********************************************************************/
		private void PyramidLevelUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)PyramidLevelUpDown.Value;
			PyramidLevelTrackBar.Value = val;

			if(!locked)
				setPyramidLevel(val);

			if(!parameterSet.isAuto(MatchingParam.AUTO_NUM_LEVEL))
			{
				PyramidLevelAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				PyramidLevelAutoButton.Text = "Auto";
			}
		}

		private void PyramidLevelTrackBar_Scroll(object sender, System.EventArgs e)
		{
			PyramidLevelUpDown.Value = PyramidLevelTrackBar.Value;
			PyramidLevelUpDown.Refresh();
		}

		private void PyramidLevelAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_NUM_LEVEL))
			{
				PyramidLevelAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				PyramidLevelAutoButton.Text = "Auto";
			}
			else
			{
				PyramidLevelAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				PyramidLevelAutoButton.Text = "Auto!";	
				mAssistant.setAuto(MatchingParam.AUTO_NUM_LEVEL);
			}
		}

		private void setPyramidLevel(int val)
		{
			mAssistant.setPyramidLevel(val);
		}

		/********************************************************************/
		/*                          metricBox                               */
		/********************************************************************/
		private void MetricBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!locked)
				mAssistant.setMetric(MetricBox.Text);
		}
		

		/********************************************************************/
		/*  Auto                    optimizationBox                         */
		/********************************************************************/
		private void OptimizationBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!locked)
				mAssistant.setOptimization((string)OptimizationBox.Text);

			if(!parameterSet.isAuto(MatchingParam.AUTO_OPTIMIZATION))
			{
				OptimizationAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				OptimizationAutoButton.Text = "Auto";
			}
		}
		private void OptimizationAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_OPTIMIZATION))
			{
				OptimizationAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				OptimizationAutoButton.Text = "Auto";
			}
			else
			{
				OptimizationAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				OptimizationAutoButton.Text = "Auto!";
				mAssistant.setAuto(MatchingParam.AUTO_OPTIMIZATION);
			}
			
		}

		/********************************************************************/
		/*  Auto                   minContrast                              */
		/********************************************************************/
		private void MinContrastUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)MinContrastUpDown.Value;		
			MinContrastTrackBar.Value = val;

			if(!locked)
				setMinContrast(val);

			if(!parameterSet.isAuto(MatchingParam.AUTO_MIN_CONTRAST))
			{
				MinContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				MinContrastAutoButton.Text = "Auto";
			}
		}

		private void MinContrastTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MinContrastUpDown.Value = MinContrastTrackBar.Value;		
			MinContrastUpDown.Refresh();
		}

		private void MinContrastAutoButton_Click(object sender, System.EventArgs e)
		{
			if(mAssistant.removeAuto(MatchingParam.AUTO_MIN_CONTRAST))
			{
				MinContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlText;	
				MinContrastAutoButton.Text = "Auto";
			}
			else
			{
				MinContrastAutoButton.ForeColor = System.Drawing.SystemColors.ControlLightLight;	
				MinContrastAutoButton.Text = "Auto!";
				mAssistant.setAuto(MatchingParam.AUTO_MIN_CONTRAST);
			}
		}
		
		
		private void setMinContrast(int val)
		{
			mAssistant.setMinContrast(val);
		}


		/**********************************************************************/
		/************  2. Tab: USE MODEL **************************************/
		/**************** Eventhandling ***************************************/
		/**********************************************************************/

		private void LoadTestImgButton_Click(object sender, System.EventArgs e)
		{
			string [] files;
			int count = 0;

			if(openFileDialog2.ShowDialog() == DialogResult.OK)
			{
				files = openFileDialog2.FileNames;
				count = files.Length;

				for(int i=0; i<count; i++)
				{
					if(mAssistant.addTestImages(files[i]))
						testImgListBox.Items.Add(files[i]);
				}
				
				if(testImgListBox.SelectedIndex<0 && testImgListBox.Items.Count != 0)
				{
					CurrentImg = mAssistant.getTestImage((string)testImgListBox.Items[0]);
                    testImgListBox.SelectedIndex = 0;
                    FindModelGraphics();
				}
			}//if 
		}
	
		/********************************************************************/
		/********************************************************************/
		private void deleteTestImgButton_Click(object sender, System.EventArgs e)
		{
			int count;
			if((count = testImgListBox.SelectedIndex) < 0)
				return;

			string fileName = (string)testImgListBox.SelectedItem;
			
			if((--count) < 0)
				count+=2;

            if((count < testImgListBox.Items.Count))
            {
                testImgListBox.SelectedIndex = count;
            }
           	
			mAssistant.removeTestImage(fileName);
			testImgListBox.Items.Remove(fileName);
		}


		/********************************************************************/
		/********************************************************************/
		private void deleteAllTestImgButton_Click(object sender, System.EventArgs e)
		{	
			if(testImgListBox.Items.Count>0)
			{
				testImgListBox.Items.Clear();
				mAssistant.removeTestImage();
                DetectionContour = null;
                
                mView.clearList();
                changeWindowMode(2);              
                mView.repaint();
			}
		}

		/********************************************************************/
		/********************************************************************/
		private void testImgListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string file;

            if(testImgListBox.SelectedIndex < 0)
            {
                mView.clearList();
                changeWindowMode(2);
                mView.repaint();
                return;
            }
			
            DetectionContour = null;
			file = (string)testImgListBox.SelectedItem;
			mAssistant.setTestImage(file);

            changeWindowMode(2);
			
			if(FindAlwaysCheckBox.Checked 
				&& (mAssistant.onExternalModelID || (ModelRegion != null)))
			{
				mAssistant.applyFindModel();
			}
			else
			{
				mView.repaint(); 
			}
		}

		/********************************************************************/
		/********************************************************************/
		private void displayTestImgButton_Click(object sender, System.EventArgs e)
		{	
			string file;
			
           	if(testImgListBox.Items.Count == 0)
			{
                mView.clearList();
                changeWindowMode(2);    
                mView.repaint();
             	
                UpdateMatching(MatchingAssistant.ERR_NO_TESTIMAGE);
				return;
			}


			file = (string)testImgListBox.SelectedItem;			
			CurrentImg = mAssistant.getTestImage(file);
            
            changeWindowMode(2, CurrentImg);
			
			if(!FindAlwaysCheckBox.Checked)
				DetectionContour = null;

			FindModelGraphics();
			
			if(ModelContour != null && FindAlwaysCheckBox.Checked)
			{
				mAssistant.applyFindModel(); 				
			}
			else
			{
				mView.repaint(); 
			}
		}

		/********************************************************************/
		/********************************************************************/
		private void findModelButton_Click(object sender, System.EventArgs e)
		{
			
			if(testImgListBox.Items.Count!=0)
			   changeWindowMode(2);
			
 			mAssistant.applyFindModel();                
		}

		private void findAlwaysCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			bool flag = FindAlwaysCheckBox.Checked;
            
            if(flag && testImgListBox.Items.Count!=0)
                changeWindowMode(2);

			mAssistant.setFindAlways(flag);
		}
		
		/********************************************************************/
		/*                            minScore                              */
		/********************************************************************/
		private void MinScoreUpDown_ValueChanged(object sender, System.EventArgs e)
		{	
			int val = (int)MinScoreUpDown.Value;
			MinScoreTrackBar.Value = val;

			if(!locked)
				setMinScore(val);
		}

		private void MinScoreTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MinScoreUpDown.Value = MinScoreTrackBar.Value;
			MinScoreUpDown.Refresh();
			
		}
		
		private void setMinScore(int val)
		{
            if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                changeWindowMode(2);

			mAssistant.setMinScore((double)val/100.0);			
		}

		/********************************************************************/
		/*                            numMatches                            */
		/********************************************************************/
		private void NumMatchesUpDown_ValueChanged(object sender, System.EventArgs e)
		{	
			int val = (int) NumMatchesUpDown.Value;
			NumMatchesTrackBar.Value = val;
			InspectMaxNoMatchUpDown.Value = val;

			if(!locked)
				setNumMatches(val);
		}

		private void NumMatchesTrackBar_Scroll(object sender, System.EventArgs e)
		{
			NumMatchesUpDown.Value = NumMatchesTrackBar.Value;
			NumMatchesUpDown.Refresh();
		}

		private void setNumMatches(int val)
		{
            if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                changeWindowMode(2);
            
            if(val==0)
            {
                if(FindMaxNoOfModelButton.Checked==true)
                    FindAtLeastOneModelButton.Checked=true;

                FindMaxNoOfModelButton.Enabled=false;
            }
            else
            {
                FindMaxNoOfModelButton.Enabled=true;
            }           
            

			mAssistant.setNumMatches(val);
		}

		/********************************************************************/
		/*                            greediness                            */
		/********************************************************************/
		private void GreedinessUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int) GreedinessUpDown.Value;
			GreedinessTrackBar.Value = val;

			if(!locked)
				setGreediness(val);
		}

		private void GreedinessTrackBar_Scroll(object sender, System.EventArgs e)
		{
			GreedinessUpDown.Value = GreedinessTrackBar.Value;
			GreedinessUpDown.Refresh();
		}

		private void setGreediness(int val)
		{
            if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                changeWindowMode(2);

			mAssistant.setGreediness((double)val/100.0);
		}

		/********************************************************************/
		/*                            maxOverlap                            */
		/********************************************************************/
		private void MaxOverlapUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int) MaxOverlapUpDown.Value;
			MaxOverlapTrackBar.Value = val;

			if(!locked)
				setMaxOverlap(val);
		}

		private void MaxOverlapTrackBar_Scroll(object sender, System.EventArgs e)
		{
			MaxOverlapUpDown.Value = MaxOverlapTrackBar.Value;
			MaxOverlapUpDown.Refresh();
		}
		
		private void setMaxOverlap(int val)
		{
            if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                changeWindowMode(2);

			mAssistant.setMaxOverlap((double)val/100.0);
		}

		/********************************************************************/
		/*                            subpixelBox                           */
		/********************************************************************/
		private void SubPixelBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(!locked)
			{
                if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                    changeWindowMode(2);

				mAssistant.setSubPixel(SubPixelBox.Text);
			}
		}

		/********************************************************************/
		/*                            lastPyramidLevel                      */
		/********************************************************************/
		private void LastPyrLevUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int) LastPyrLevUpDown.Value;
			LastPyrLevTrackBar.Value = val;

			if(!locked)
				setLastPyramidLevel(val);
		}

		private void LastPyrLevTrackBar_Scroll(object sender, System.EventArgs e)
		{
			LastPyrLevUpDown.Value = LastPyrLevTrackBar.Value;
			LastPyrLevUpDown.Refresh();
		}
		
		private void setLastPyramidLevel(int val)
		{
            if(testImgListBox.Items.Count!=0 && FindAlwaysCheckBox.Checked)
                changeWindowMode(2);

			mAssistant.setLastPyramLevel(val);	
		}


		/*************************************************************************/
		/********  3. Tab: Optimize Recognition Speed ****************************/
		/******************* Eventhandling ***************************************/
		/*************************************************************************/
		private void FindNoOfInstButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!locked && FindNoOfInstButton.Checked)
				mAssistant.setRecogSpeedMode(MatchingParam.RECOGM_MANUALSELECT);
		}

		private void FindAtLeastOneModelButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!locked && FindAtLeastOneModelButton.Checked)
				mAssistant.setRecogSpeedMode(MatchingParam.RECOGM_ATLEASTONE);
		}

		private void FindMaxNoOfModelButton_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!locked && FindMaxNoOfModelButton.Checked)
				mAssistant.setRecogSpeedMode(MatchingParam.RECOGM_MAXNUMBER);
		}

		/********************************************************************/
		/*                         RecognitionRate                          */
		/********************************************************************/
		private void RecognRateComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int val = RecognRateComboBox.SelectedIndex; 
			
			if(!locked)
				mAssistant.setRecogRateOption(val);
		}

		private void recogRateUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val  = (int)recogRateUpDown.Value;
			recogRateTrackBar.Value = val;
			mAssistant.setRecogitionRate(val);
		}

		private void recogRateTrackBar_Scroll(object sender, System.EventArgs e)
		{
			recogRateUpDown.Value = recogRateTrackBar.Value;
			recogRateUpDown.Refresh();
		}

		private void RecogNoManSelUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			int val = (int)RecogNoManSelUpDown.Value;
			if(!locked)
				mAssistant.setRecogManualSelection(val);
		}

		/*******************************************************************/
		/*******************************************************************/
		private void runOptimizeSpeedButton_Click(object sender, System.EventArgs e)
		{
            if(testImgListBox.Items.Count!=0)
                changeWindowMode(2);

			if(!timer.Enabled)
			{
				FindAlwaysCheckBox.Checked = false;
				optInstance = speedOptHandler;
				optInstance.reset();
				UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_UPDATE_VALS);
				UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_OPTIMUM_VALS);
				UpdateStatisticsData(MatchingOpt.UPDATE_RECOG_STATISTICS_STATUS);

				OptimizeButton.Text = "Stop";	
				mAssistant.onTimer = true;
				timer.Enabled = true;
			}
			else
			{
				timer.Enabled = false;
				OptimizeButton.Text = "Run Optimization";
				mAssistant.onTimer = false;
				UpdateStatisticsData(MatchingOpt.RUN_FAILED);
			}
		}

		/*******************************************************************/
		/*******************************************************************/
		private void runStatisticsButton_Click(object sender, System.EventArgs e)
		{
            if(testImgListBox.Items.Count!=0)
                changeWindowMode(2);

			if(!timer.Enabled)
			{
            	FindAlwaysCheckBox.Checked = false;
				optInstance = inspectOptHandler;
				optInstance.reset();
				UpdateStatisticsData(MatchingOpt.UPDATE_INSP_RECOGRATE);
				UpdateStatisticsData(MatchingOpt.UPDATE_INSP_STATISTICS);
		
				StatisticsButton.Text = "Stop";				
				timer.Enabled = true;
				mAssistant.onTimer = true;
			}
			else
			{
				timer.Enabled = false;
				StatisticsButton.Text = "Run Inspect";
				mAssistant.onTimer = false;
			}
		}
				
		private void InspectMaxNoMatchUpDown_ValueChanged(object sender, System.EventArgs e)
		{
			if(InspectMaxNoMatchUpDown.Value != NumMatchesUpDown.Value) 
				NumMatchesUpDown.Value = (int)InspectMaxNoMatchUpDown.Value;
		}
		
		/*******************************************************************/
		/*******************************************************************/
		private void timer_ticked(object sender, System.EventArgs e)
		{
			bool run;
			run = optInstance.ExecuteStep();

			if(!run)
			{
				timer.Enabled = false;
				optInstance.stop();
				mAssistant.onTimer = false;

				OptimizeButton.Text	  = "Run Optimization";	
				StatisticsButton.Text = "Run Inspect";
			}
		}

		/**********************************************************************/
		/**********************************************************************/
		/*                     Delegate Routines                              */
		/**********************************************************************/
		/**********************************************************************/

        /// <summary>
        /// This method is invoked if changes occur in the HWndCtrl instance
        /// or the ROIController. In either case, the HALCON 
        /// window needs to be updated/repainted.
        /// </summary>
        public void UpdateViewData(int val)
		{

			switch(val)
			{	
                case ROIController.EVENT_CHANGED_ROI_SIGN:                   
                case ROIController.EVENT_DELETED_ACTROI:                   
                case ROIController.EVENT_DELETED_ALL_ROIS:                
				case ROIController.EVENT_UPDATE_ROI:
					ModelContour = null;
					DetectionContour = null;
                    bool genROI = roiController.defineModelROI();
					ModelRegion = roiController.getModelRegion();
					mAssistant.setModelROI(ModelRegion);
                    CreateModelGraphics();
					if(!genROI)
                        mView.repaint();

					break;                   
                case HWndCtrl.ERR_READING_IMG:
					MessageBox.Show("Problem occured while reading file! \n" + mView.exceptionText, 
						"Matching assistant",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
					break;
				default: 
					break;
			}
		}

		/// <summary>
        /// This method is invoked for any changes in the 
        /// MatchingAssistant, concerning the model creation and
        /// the model finding. Also changes in the display mode 
        /// (e.g., pyramid level) are mapped here.
        /// </summary>
        public void UpdateMatching(int val)
		{
			bool paint=false;
			switch(val)
			{	
				case MatchingAssistant.UPDATE_XLD:
					ModelContour = mAssistant.getModelContour();
					CreateModelGraphics();
					paint = true;
					break;
				case MatchingAssistant.UPDATE_DISPLEVEL:
					CurrentImg = mAssistant.getDispImage(); 
					ModelContour = mAssistant.getModelContour();
					CreateModelGraphics();
					paint = true;
					break;
				case MatchingAssistant.UPDATE_PYRAMID:
					DispPyramidTrackBar.Enabled = true;
					DispPyramidUpDown.Enabled = true;
					break;
				case MatchingAssistant.UPDATE_DETECTION_RESULT:
                    DetectionContour = mAssistant.getDetectionResults();
					FindModelGraphics();
					paint = true;
					break;
				case MatchingAssistant.UPDATE_TESTVIEW:
					CurrentImg = mAssistant.getCurrTestImage(); 
					FindModelGraphics();
					break;
				case MatchingAssistant.ERR_WRITE_SHAPEMODEL:
					MessageBox.Show("Problem occured while writing into file \n" + mAssistant.exceptionText, 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					break;
				case MatchingAssistant.ERR_READ_SHAPEMODEL:
					MessageBox.Show("Problem occured while reading from file \n" + mAssistant.exceptionText, 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					break;
				case MatchingAssistant.ERR_NO_MODEL_DEFINED:
					MessageBox.Show("Please define a Model!", 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
					paint = true;
					break;
				case MatchingAssistant.ERR_NO_IMAGE:
					MessageBox.Show("Please load an image", 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
                    break;
				case MatchingAssistant.ERR_NO_TESTIMAGE:
					MessageBox.Show("Please load a testimage", 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
                    paint = true;
					break;
				case MatchingAssistant.ERR_NO_VALID_FILE:
					MessageBox.Show("Selected file is not a HALCON ShapeModel file .shm", 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information);
					break;
				case MatchingAssistant.ERR_READING_IMG:
					UpdateViewData(HWndCtrl.ERR_READING_IMG);
					break;
				default: 
					break;
			}
			if(paint)
				mView.repaint();
		}

		
        /// <summary>
        /// Calculates new optimal values for a parameter, if the parameter is
        /// in the auto-mode list. The new settings are forwarded to the GUI
        /// components to update the display.
        /// </summary>
        public void UpdateButton(string mode)
		{
			int [] r;
			locked = true;
			switch (mode)
			{		
				case MatchingParam.AUTO_ANGLE_STEP: 
					AngleStepUpDown.Value = (int)((parameterSet.mAngleStep*10.0)*180.0/Math.PI);
					break;
				case MatchingParam.AUTO_CONTRAST: 
					ContrastUpDown.Value  =  (int)parameterSet.mContrast;
					break;
				case MatchingParam.AUTO_MIN_CONTRAST: 
					MinContrastUpDown.Value = (int)parameterSet.mMinContrast;
					break;
				case MatchingParam.AUTO_NUM_LEVEL: 
					PyramidLevelUpDown.Value = (int)parameterSet.mNumLevel;
					break;
				case MatchingParam.AUTO_OPTIMIZATION: 
					OptimizationBox.Text = parameterSet.mOptimization;
					break;
				case MatchingParam.AUTO_SCALE_STEP: 
					ScaleStepUpDown.Value =  (int)(parameterSet.mScaleStep*1000);
					break;
				case MatchingParam.BUTTON_ANGLE_EXTENT:
					AngleExtentUpDown.Value = (int)(parameterSet.mAngleExtent*180.0/Math.PI);
					break;
				case MatchingParam.BUTTON_ANGLE_START:
					StartingAngleUpDown.Value = (int)(parameterSet.mStartingAngle*180.0/Math.PI);
					break;
				case MatchingParam.BUTTON_METRIC:
					MetricBox.Text = parameterSet.mMetric;
					break;
				case MatchingParam.BUTTON_SCALE_MAX:
					MaxScaleUpDown.Value = (int)(parameterSet.mMaxScale*100.0);
					break;
				case MatchingParam.BUTTON_SCALE_MIN:
					MinScaleUpDown.Value = (int)(parameterSet.mMinScale*100.0);
					break;
				case MatchingParam.BUTTON_MINSCORE:
					MinScoreUpDown.Value = (int)(parameterSet.mMinScore*100);
					break;
				case MatchingParam.BUTTON_GREEDINESS:
					GreedinessUpDown.Value = (int)(parameterSet.mGreediness*100);
					break;
				case MatchingParam.H_ERR_MESSAGE:
					MessageBox.Show("Bad Parameters! \n" + mAssistant.exceptionText, 
						"Matching Wizard",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					break;
				case MatchingParam.RANGE_ANGLE_STEP:
					r = mAssistant.getStepRange(MatchingParam.RANGE_ANGLE_STEP);
					AngleStepTrackBar.Minimum	= r[0];
					AngleStepUpDown.Minimum		= r[0];
					AngleStepTrackBar.Maximum	= r[1];
					AngleStepUpDown.Maximum		= r[1];

					AngleStepUpDown.Value = (int)((parameterSet.mAngleStep*10.0)*180.0/Math.PI);
					break;
				case MatchingParam.RANGE_SCALE_STEP:
					r = mAssistant.getStepRange(MatchingParam.RANGE_SCALE_STEP);
					ScaleStepTrackBar.Minimum	= r[0];
					ScaleStepUpDown.Minimum		= r[0];
					ScaleStepTrackBar.Maximum	= r[1];
					ScaleStepUpDown.Maximum		= r[1];

					ScaleStepUpDown.Value =  (int)(parameterSet.mScaleStep*1000);
					break;
				default:
					break;
			}
			locked = false;
		}

		
        /// <summary>
        /// This method is invoked when the inspection tab or the 
        /// recognition tab are triggered to compute the optimized values
        /// and to forward the results to the display.
        /// </summary>
        public void UpdateStatisticsData(int mode)
		{

			switch (mode)
			{
				case MatchingOpt.UPDATE_RECOG_STATISTICS_STATUS: 
					labelOptStatus.Text = optInstance.statusString;
					break;
				case MatchingOpt.UPDATE_RECOG_UPDATE_VALS:
					labelRecogn11.Text = optInstance.recogTabOptimizationData[0]; 
					labelRecogn12.Text = optInstance.recogTabOptimizationData[1]; 
					labelRecogn13.Text = optInstance.recogTabOptimizationData[2]; 
					labelRecogn14.Text = optInstance.recogTabOptimizationData[3]; 
					break;
				case MatchingOpt.UPDATE_RECOG_OPTIMUM_VALS:
					labelRecogn21.Text = optInstance.recogTabOptimizationData[4]; 
					labelRecogn22.Text = optInstance.recogTabOptimizationData[5]; 
					labelRecogn23.Text = optInstance.recogTabOptimizationData[6]; 
					labelRecogn24.Text = optInstance.recogTabOptimizationData[7];
					break;
				case MatchingOpt.UPDATE_INSP_RECOGRATE:
					labelInspect01.Text = optInstance.inspectTabRecogRateData[0];
					labelInspect02.Text = optInstance.inspectTabRecogRateData[1];
					labelInspect03.Text = optInstance.inspectTabRecogRateData[2];
					labelInspect04.Text = optInstance.inspectTabRecogRateData[3];
					labelInspect05.Text = optInstance.inspectTabRecogRateData[4];
					break;
				case MatchingOpt.UPDATE_INSP_STATISTICS:
					labelInspect11.Text = optInstance.inspectTabStatisticsData[0];
					labelInspect21.Text = optInstance.inspectTabStatisticsData[1];
					labelInspect31.Text = optInstance.inspectTabStatisticsData[2];

					labelInspect12.Text = optInstance.inspectTabStatisticsData[3];
					labelInspect22.Text = optInstance.inspectTabStatisticsData[4];
					labelInspect32.Text = optInstance.inspectTabStatisticsData[5];

					labelInspect13.Text = optInstance.inspectTabStatisticsData[6];
					labelInspect23.Text = optInstance.inspectTabStatisticsData[7];
					labelInspect33.Text = optInstance.inspectTabStatisticsData[8];

					labelInspect14.Text = optInstance.inspectTabStatisticsData[9];
					labelInspect24.Text = optInstance.inspectTabStatisticsData[10];
					labelInspect34.Text = optInstance.inspectTabStatisticsData[11];

					labelInspect15.Text = optInstance.inspectTabStatisticsData[12];
					labelInspect25.Text = optInstance.inspectTabStatisticsData[13];
					labelInspect35.Text = optInstance.inspectTabStatisticsData[14];

					labelInspect16.Text = optInstance.inspectTabStatisticsData[15];
					labelInspect26.Text = optInstance.inspectTabStatisticsData[16];
					labelInspect36.Text = optInstance.inspectTabStatisticsData[17];

					labelInspect17.Text = optInstance.inspectTabStatisticsData[18];
					labelInspect27.Text = optInstance.inspectTabStatisticsData[19];
					labelInspect37.Text = optInstance.inspectTabStatisticsData[20];
					break;
				case MatchingOpt.UPDATE_TEST_ERR:
					MessageBox.Show("Optimization failed! \n Please check if your model is well defined\n(e.g contains some model contours)!?",
						"Shapebases Matching Assistant",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					break;
				case MatchingOpt.UPDATE_RECOG_ERR:
					labelOptStatus.Text = "Optimization failed";
					MessageBox.Show("There was no appropriate set of parameters to match - \n" +
						"Check if your model is well defined and the parameters\nfor model creation are set appropriately!",
						"Shapebases Matching Assistant",
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					break;
				case MatchingAssistant.ERR_NO_TESTIMAGE:
					UpdateMatching(MatchingAssistant.ERR_NO_TESTIMAGE);
					break;
				case MatchingOpt.RUN_SUCCESSFUL:
					UpdateButton(MatchingParam.BUTTON_GREEDINESS);
					UpdateButton(MatchingParam.BUTTON_MINSCORE);
					break;
				case MatchingOpt.RUN_FAILED:
					setMinScore((int)MinScoreUpDown.Value);
					setGreediness((int)GreedinessUpDown.Value);
					break;
				default:
					break;
			}
		}
		

		/********************************************************************/
		/********************************************************************/
		private void CreateModelGraphics()
		{
            mView.clearList();
			mView.changeGraphicSettings(GraphicsContext.GC_LINESTYLE, new HTuple());
			mView.addIconicVar(CurrentImg);
			if(ModelRegion != null)
			{  
				mView.changeGraphicSettings(GraphicsContext.GC_COLOR, "blue"); 
				mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 3);
				mView.addIconicVar(ModelRegion); 
			}
			if(ModelContour != null)
			{
				mView.changeGraphicSettings(GraphicsContext.GC_COLOR, "red"); 
				mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 1); 
				mView.addIconicVar(ModelContour); 
			}
		}

		private void FindModelGraphics()
		{
            mView.clearList();
			mView.changeGraphicSettings(GraphicsContext.GC_LINESTYLE, new HTuple());
			mView.addIconicVar(CurrentImg);
			if(DetectionContour != null)
			{	
				mView.changeGraphicSettings(GraphicsContext.GC_COLOR, "green"); 
				mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 3); 
				mView.addIconicVar(DetectionContour); 
			}
		}


        /********************************************************************/
        /********************************************************************/
        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(tabControl.SelectedIndex == 0)
            {
                changeWindowMode(1);
                CreateModelGraphics();
            }
            else
            {
                changeWindowMode(2);
                FindModelGraphics();
            }
            mView.repaint();
        }

        /********************************************************************/
        /********************************************************************/
        private void changeWindowMode(int mode)
        {
            HImage img = null;

            if(mode==2)
                img = mAssistant.getCurrTestImage();  

            changeWindowMode(mode, img);
        }

        private void changeWindowMode(int mode, HImage testImage)
        {
            mView.setDispLevel(mode);
            
            switch(mode)
            {
                case 1:                    
                    CurrentImg = mAssistant.getDispImage();                            
                    if(mAssistant.onExternalModelID)
                    {
                        WindowModePanel.BackColor = System.Drawing.SystemColors.ControlDark;
                    }
                    else if(CurrentImg==null)
                    {
                        WindowModePanel.BackColor = createModelWindowMode;
                    }
                    else
                    {
                        WindowModePanel.BackColor = createModelWindowMode;
                        groupBoxCreateROI.Enabled = true;
                    }
                    break;

                case 2:                    
                    WindowModePanel.BackColor = trainModelWindowMode;
                    CurrentImg = testImage;
                    groupBoxCreateROI.Enabled = false;                    
                    break;
            }
        }

	}//end of class
}//end of namespace

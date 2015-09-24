using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ViewROI;
using CalibrationModule;
using HalconDotNet;



namespace CalibrationMain
{
    
    /// <summary>
    /// This project presents the functionalities of the HDevelop Calibration 
    /// Assistant in a stand-alone application. It contains most of the 
    /// data structures and working menus that are offered with the HDevelop
    /// Assistant. 
    /// 
    /// The Assistant includes the project ViewROI for its window handling and
    /// uses the class CalibrationAssistant of the project CalibrationModule
    /// as a controller between the GUI and the classes CalibImage and
    /// QualityIssue.
    /// </summary>
    public class CalibrationForm : System.Windows.Forms.Form
	{
        private HalconDotNet.HWindowControl viewPort;
        private HWndCtrl                    mView;
        private CalibrationAssistant        mAssistant;
        private int                         currIdx;
        private int                         currLineW;

        private double tThickness   = 0;
        private double tCellWidth   = 0;
        private double tCellHeight  = 0;
        private double tFocalLength = 0;
        private double tMotionX = 0;
        private double tMotionY = 0;
        private double tMotionZ = 0;
        
        private bool   locked;
        private bool   plateRegionDisp;
        private bool   markCentersDisp;
        private bool   coordSystemDisp;

        private string plateRegionColor;
        private string markCenterColor;
        private string coordSystemColor;

        // GUI elements
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonDeleteAll;
        private System.Windows.Forms.Button buttonSetReference;
        private System.Windows.Forms.Button buttonCalibrate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxCoordSys;
        private System.Windows.Forms.CheckBox checkBoxMarkCenter;
        private System.Windows.Forms.CheckBox checkBoxPlateRegion;
        private System.Windows.Forms.ComboBox comboBoxDraw;
        private System.Windows.Forms.ComboBox comboBoxCoordSys;
        private System.Windows.Forms.ComboBox comboBoxMarkCenters;
        private System.Windows.Forms.ComboBox comboBoxPlateRegion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPageQualityCheck;
        private System.Windows.Forms.TabPage tabPageCalib;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.RadioButton buttonMove;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.RadioButton buttonNone;
        private System.Windows.Forms.RadioButton buttonMagnify;
        private System.Windows.Forms.RadioButton buttonZoom;
        private System.Windows.Forms.OpenFileDialog openFileDialogImg;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Button buttonImportParams;
        private System.Windows.Forms.Button buttonLoadDescrFile;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.ListView ListCalibImg;
        private System.Windows.Forms.ColumnHeader ColRef;
        private System.Windows.Forms.ColumnHeader ColImg;
        private System.Windows.Forms.ColumnHeader ColStatus;
        private System.Windows.Forms.ListView ListQualityCheck;
        private System.Windows.Forms.ColumnHeader ColScope;
        private System.Windows.Forms.ColumnHeader ColDescr;
        private System.Windows.Forms.ColumnHeader Col2Status;
        private System.Windows.Forms.OpenFileDialog openFileDialogDescr;
        private System.Windows.Forms.TextBox textBoxDescr;
        private System.Windows.Forms.OpenFileDialog openFileDialogImportParams;
        private System.Windows.Forms.CheckBox TelecentricCheckBox;
        private System.Windows.Forms.NumericUpDown FocalLengthUpDown;
        private System.Windows.Forms.NumericUpDown SyUpDown;
        private System.Windows.Forms.NumericUpDown SxUpDown;
        private System.Windows.Forms.ComboBox CamTypComboBox;
        private System.Windows.Forms.NumericUpDown ThicknessUpDown;
        private System.Windows.Forms.TrackBar MaxDiamTrackBar;
        private System.Windows.Forms.NumericUpDown MaxDiamUpDown;
        private System.Windows.Forms.TrackBar MinContLTrackBar;
        private System.Windows.Forms.NumericUpDown MinContLUpDown;
        private System.Windows.Forms.TrackBar SmoothTrackBar;
        private System.Windows.Forms.NumericUpDown SmoothUpDown;
        private System.Windows.Forms.TrackBar MinThreshTrackBar;
        private System.Windows.Forms.NumericUpDown MinThreshUpDown;
        private System.Windows.Forms.TrackBar ThreshDecrTrackBar;
        private System.Windows.Forms.NumericUpDown ThreshDecrUpDown;
        private System.Windows.Forms.TrackBar InitThreshTrackBar;
        private System.Windows.Forms.NumericUpDown InitThreshUpDown;
        private System.Windows.Forms.TrackBar MinDiamTrackBar;
        private System.Windows.Forms.NumericUpDown MinDiamUpDown;
        private System.Windows.Forms.TrackBar MarkThreshTrackBar;
        private System.Windows.Forms.NumericUpDown MarkThreshUpDown;
        private System.Windows.Forms.ComboBox SeqTestsComboBox;
        private System.Windows.Forms.ComboBox ImgTestsComboBox;
        private System.Windows.Forms.NumericUpDown WarnlevelUpDown;
        private System.Windows.Forms.Button MaxDiamResetButton;
        private System.Windows.Forms.Button MinContLResetButton;
        private System.Windows.Forms.Button SmootingResetButton;
        private System.Windows.Forms.Button MinThreshResetButton;
        private System.Windows.Forms.Button ThreshDecrResetButton;
        private System.Windows.Forms.Button InitThreshResetButton;
        private System.Windows.Forms.Button MinDiamResetButton;
        private System.Windows.Forms.Button MarkThreshResetButton;
        private System.Windows.Forms.TrackBar FilterSizeTrackBar;
        private System.Windows.Forms.NumericUpDown FilterSizeUpDown;
        private System.Windows.Forms.Button FilterSizeResetButton;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.NumericUpDown MotionZUpDown;
        private System.Windows.Forms.NumericUpDown MotionYUpDown;
        private System.Windows.Forms.NumericUpDown MotionXUpDown;
        private System.Windows.Forms.Panel LineScanAddPanel;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.NumericUpDown upDownLineWidth;
        private System.Windows.Forms.Button buttonSaveCamPose;
        private System.Windows.Forms.Label CamGammaLabel;
        private System.Windows.Forms.Label CamBetaLabel;
        private System.Windows.Forms.Label CamAlphaLabel;
        private System.Windows.Forms.Label CamPoseZLabel;
        private System.Windows.Forms.Label CamPoseYLabel;
        private System.Windows.Forms.Label CamPoseXLabel;
        private System.Windows.Forms.Button buttonSaveCamParams;
        private System.Windows.Forms.Label CyResultLabel;
        private System.Windows.Forms.Label CxResultLabel;
        private System.Windows.Forms.Label SyResultLabel;
        private System.Windows.Forms.Label SxResultLabel;
        private System.Windows.Forms.Label StatusCalibLabel;
        private System.Windows.Forms.Label ImgHResultLabel;
        private System.Windows.Forms.Label ImgWResultLabel;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox checkBoxOrigImgCoord;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.RadioButton buttonRefImg;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label FocalLResultLabel;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label KappaResultLabel;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label P2Label;
        private System.Windows.Forms.Label P1Label;
        private System.Windows.Forms.Label K3Label;
        private System.Windows.Forms.Label K2Label;
        private System.Windows.Forms.Label K1Label;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label VzResultLabel;
        private System.Windows.Forms.Label VyResultLabel;
        private System.Windows.Forms.Label VxResultLabel;
        private System.Windows.Forms.Panel AreaScanPolynomPanel;
        private System.Windows.Forms.Panel KappaPanel;
        private System.Windows.Forms.Panel LineScanPanel;
        private System.Windows.Forms.SaveFileDialog saveParamFileDialog;
        private System.Windows.Forms.RadioButton buttonSimImg;
        private System.Windows.Forms.Button buttonDefaultParams;        
		private System.ComponentModel.Container components = null;

        
        /// <summary>Constructor</summary>
		public CalibrationForm()
		{
			InitializeComponent();
		}

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
		private void InitializeComponent()
		{
            this.viewPort = new HalconDotNet.HWindowControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageCalib = new System.Windows.Forms.TabPage();
            this.ListCalibImg = new System.Windows.Forms.ListView();
            this.ColRef = new System.Windows.Forms.ColumnHeader();
            this.ColImg = new System.Windows.Forms.ColumnHeader();
            this.ColStatus = new System.Windows.Forms.ColumnHeader();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonDefaultParams = new System.Windows.Forms.Button();
            this.LineScanAddPanel = new System.Windows.Forms.Panel();
            this.label72 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.MotionZUpDown = new System.Windows.Forms.NumericUpDown();
            this.MotionYUpDown = new System.Windows.Forms.NumericUpDown();
            this.MotionXUpDown = new System.Windows.Forms.NumericUpDown();
            this.label71 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TelecentricCheckBox = new System.Windows.Forms.CheckBox();
            this.FocalLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.SyUpDown = new System.Windows.Forms.NumericUpDown();
            this.SxUpDown = new System.Windows.Forms.NumericUpDown();
            this.buttonImportParams = new System.Windows.Forms.Button();
            this.textBoxDescr = new System.Windows.Forms.TextBox();
            this.CamTypComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ThicknessUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonLoadDescrFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCalibrate = new System.Windows.Forms.Button();
            this.buttonSetReference = new System.Windows.Forms.Button();
            this.buttonDeleteAll = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.tabPageQualityCheck = new System.Windows.Forms.TabPage();
            this.WarnlevelUpDown = new System.Windows.Forms.NumericUpDown();
            this.ListQualityCheck = new System.Windows.Forms.ListView();
            this.ColScope = new System.Windows.Forms.ColumnHeader();
            this.ColDescr = new System.Windows.Forms.ColumnHeader();
            this.Col2Status = new System.Windows.Forms.ColumnHeader();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.MaxDiamTrackBar = new System.Windows.Forms.TrackBar();
            this.MaxDiamResetButton = new System.Windows.Forms.Button();
            this.MaxDiamUpDown = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.MinContLResetButton = new System.Windows.Forms.Button();
            this.MinContLTrackBar = new System.Windows.Forms.TrackBar();
            this.MinContLUpDown = new System.Windows.Forms.NumericUpDown();
            this.label18 = new System.Windows.Forms.Label();
            this.SmootingResetButton = new System.Windows.Forms.Button();
            this.SmoothTrackBar = new System.Windows.Forms.TrackBar();
            this.SmoothUpDown = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.MinThreshResetButton = new System.Windows.Forms.Button();
            this.MinThreshTrackBar = new System.Windows.Forms.TrackBar();
            this.MinThreshUpDown = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.ThreshDecrResetButton = new System.Windows.Forms.Button();
            this.ThreshDecrTrackBar = new System.Windows.Forms.TrackBar();
            this.ThreshDecrUpDown = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.InitThreshResetButton = new System.Windows.Forms.Button();
            this.InitThreshTrackBar = new System.Windows.Forms.TrackBar();
            this.InitThreshUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.MinDiamResetButton = new System.Windows.Forms.Button();
            this.MinDiamTrackBar = new System.Windows.Forms.TrackBar();
            this.MinDiamUpDown = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.MarkThreshResetButton = new System.Windows.Forms.Button();
            this.MarkThreshTrackBar = new System.Windows.Forms.TrackBar();
            this.MarkThreshUpDown = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.FilterSizeResetButton = new System.Windows.Forms.Button();
            this.FilterSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.FilterSizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SeqTestsComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ImgTestsComboBox = new System.Windows.Forms.ComboBox();
            this.tabPageResults = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.buttonSimImg = new System.Windows.Forms.RadioButton();
            this.buttonRefImg = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.checkBoxOrigImgCoord = new System.Windows.Forms.CheckBox();
            this.buttonSaveCamPose = new System.Windows.Forms.Button();
            this.label59 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.CamGammaLabel = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.CamBetaLabel = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.CamAlphaLabel = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.CamPoseZLabel = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.CamPoseYLabel = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.CamPoseXLabel = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.AreaScanPolynomPanel = new System.Windows.Forms.Panel();
            this.P2Label = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.P1Label = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.K3Label = new System.Windows.Forms.Label();
            this.K2Label = new System.Windows.Forms.Label();
            this.K1Label = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.label63 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.FocalLResultLabel = new System.Windows.Forms.Label();
            this.KappaPanel = new System.Windows.Forms.Panel();
            this.LineScanPanel = new System.Windows.Forms.Panel();
            this.VzResultLabel = new System.Windows.Forms.Label();
            this.VyResultLabel = new System.Windows.Forms.Label();
            this.VxResultLabel = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label85 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label88 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.KappaResultLabel = new System.Windows.Forms.Label();
            this.buttonSaveCamParams = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.ImgHResultLabel = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.ImgWResultLabel = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.CyResultLabel = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.CxResultLabel = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.SyResultLabel = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.SxResultLabel = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.StatusCalibLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonMove = new System.Windows.Forms.RadioButton();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonNone = new System.Windows.Forms.RadioButton();
            this.buttonMagnify = new System.Windows.Forms.RadioButton();
            this.buttonZoom = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.upDownLineWidth = new System.Windows.Forms.NumericUpDown();
            this.label78 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.checkBoxCoordSys = new System.Windows.Forms.CheckBox();
            this.checkBoxMarkCenter = new System.Windows.Forms.CheckBox();
            this.checkBoxPlateRegion = new System.Windows.Forms.CheckBox();
            this.comboBoxDraw = new System.Windows.Forms.ComboBox();
            this.comboBoxCoordSys = new System.Windows.Forms.ComboBox();
            this.comboBoxMarkCenters = new System.Windows.Forms.ComboBox();
            this.comboBoxPlateRegion = new System.Windows.Forms.ComboBox();
            this.openFileDialogImg = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogDescr = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogImportParams = new System.Windows.Forms.OpenFileDialog();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.saveParamFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tabControl.SuspendLayout();
            this.tabPageCalib.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.LineScanAddPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MotionZUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MotionYUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MotionXUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FocalLengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SyUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SxUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThicknessUpDown)).BeginInit();
            this.tabPageQualityCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WarnlevelUpDown)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDiamTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDiamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContLTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContLUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreshDecrTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreshDecrUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitThreshTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitThreshUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinDiamTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinDiamUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkThreshTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkThreshUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSizeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSizeUpDown)).BeginInit();
            this.tabPageResults.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.AreaScanPolynomPanel.SuspendLayout();
            this.KappaPanel.SuspendLayout();
            this.LineScanPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upDownLineWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // viewPort
            // 
            this.viewPort.BackColor = System.Drawing.Color.Black;
            this.viewPort.BorderColor = System.Drawing.Color.Black;
            this.viewPort.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.viewPort.Location = new System.Drawing.Point(16, 40);
            this.viewPort.Name = "viewPort";
            this.viewPort.Size = new System.Drawing.Size(456, 368);
            this.viewPort.TabIndex = 0;
            this.viewPort.WindowSize = new System.Drawing.Size(456, 368);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageCalib);
            this.tabControl.Controls.Add(this.tabPageQualityCheck);
            this.tabControl.Controls.Add(this.tabPageResults);
            this.tabControl.Location = new System.Drawing.Point(496, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(432, 624);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPageCalib
            // 
            this.tabPageCalib.Controls.Add(this.ListCalibImg);
            this.tabPageCalib.Controls.Add(this.groupBox2);
            this.tabPageCalib.Controls.Add(this.buttonCalibrate);
            this.tabPageCalib.Controls.Add(this.buttonSetReference);
            this.tabPageCalib.Controls.Add(this.buttonDeleteAll);
            this.tabPageCalib.Controls.Add(this.buttonDelete);
            this.tabPageCalib.Controls.Add(this.buttonLoad);
            this.tabPageCalib.Location = new System.Drawing.Point(4, 22);
            this.tabPageCalib.Name = "tabPageCalib";
            this.tabPageCalib.Size = new System.Drawing.Size(424, 598);
            this.tabPageCalib.TabIndex = 0;
            this.tabPageCalib.Text = "Calibration";
            // 
            // ListCalibImg
            // 
            this.ListCalibImg.AutoArrange = false;
            this.ListCalibImg.BackColor = System.Drawing.SystemColors.Window;
            this.ListCalibImg.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColRef,
            this.ColImg,
            this.ColStatus});
            this.ListCalibImg.FullRowSelect = true;
            this.ListCalibImg.Location = new System.Drawing.Point(8, 32);
            this.ListCalibImg.MultiSelect = false;
            this.ListCalibImg.Name = "ListCalibImg";
            this.ListCalibImg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ListCalibImg.Size = new System.Drawing.Size(312, 216);
            this.ListCalibImg.TabIndex = 21;
            this.ListCalibImg.UseCompatibleStateImageBehavior = false;
            this.ListCalibImg.View = System.Windows.Forms.View.Details;
            this.ListCalibImg.SelectedIndexChanged += new System.EventHandler(this.ListCalibImg_SelectedIndexChanged);
            // 
            // ColRef
            // 
            this.ColRef.Text = "Ref";
            this.ColRef.Width = 30;
            // 
            // ColImg
            // 
            this.ColImg.Text = "Image";
            this.ColImg.Width = 152;
            // 
            // ColStatus
            // 
            this.ColStatus.Text = "Status";
            this.ColStatus.Width = 125;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.buttonDefaultParams);
            this.groupBox2.Controls.Add(this.LineScanAddPanel);
            this.groupBox2.Controls.Add(this.label71);
            this.groupBox2.Controls.Add(this.label70);
            this.groupBox2.Controls.Add(this.label69);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.TelecentricCheckBox);
            this.groupBox2.Controls.Add(this.FocalLengthUpDown);
            this.groupBox2.Controls.Add(this.SyUpDown);
            this.groupBox2.Controls.Add(this.SxUpDown);
            this.groupBox2.Controls.Add(this.buttonImportParams);
            this.groupBox2.Controls.Add(this.textBoxDescr);
            this.groupBox2.Controls.Add(this.CamTypComboBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.ThicknessUpDown);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonLoadDescrFile);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(8, 272);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(408, 320);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Setup";
            // 
            // buttonDefaultParams
            // 
            this.buttonDefaultParams.Location = new System.Drawing.Point(272, 288);
            this.buttonDefaultParams.Name = "buttonDefaultParams";
            this.buttonDefaultParams.Size = new System.Drawing.Size(120, 24);
            this.buttonDefaultParams.TabIndex = 33;
            this.buttonDefaultParams.Text = "Reset Parameters";
            this.buttonDefaultParams.Click += new System.EventHandler(this.buttonDefaultParams_Click);
            // 
            // LineScanAddPanel
            // 
            this.LineScanAddPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LineScanAddPanel.Controls.Add(this.label72);
            this.LineScanAddPanel.Controls.Add(this.label73);
            this.LineScanAddPanel.Controls.Add(this.label74);
            this.LineScanAddPanel.Controls.Add(this.label75);
            this.LineScanAddPanel.Controls.Add(this.label76);
            this.LineScanAddPanel.Controls.Add(this.label77);
            this.LineScanAddPanel.Controls.Add(this.MotionZUpDown);
            this.LineScanAddPanel.Controls.Add(this.MotionYUpDown);
            this.LineScanAddPanel.Controls.Add(this.MotionXUpDown);
            this.LineScanAddPanel.Location = new System.Drawing.Point(8, 210);
            this.LineScanAddPanel.Name = "LineScanAddPanel";
            this.LineScanAddPanel.Size = new System.Drawing.Size(344, 72);
            this.LineScanAddPanel.TabIndex = 32;
            this.LineScanAddPanel.Visible = false;
            // 
            // label72
            // 
            this.label72.Location = new System.Drawing.Point(8, 48);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(88, 24);
            this.label72.TabIndex = 40;
            this.label72.Text = "Motion Z  (Vz)";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label73
            // 
            this.label73.Location = new System.Drawing.Point(8, 24);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(88, 24);
            this.label73.TabIndex = 39;
            this.label73.Text = "Motion Y  (Vy)";
            this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label74
            // 
            this.label74.Location = new System.Drawing.Point(8, 0);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(88, 24);
            this.label74.TabIndex = 38;
            this.label74.Text = "Motion X  (Vx)";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label75
            // 
            this.label75.BackColor = System.Drawing.SystemColors.Control;
            this.label75.Location = new System.Drawing.Point(256, 48);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(48, 24);
            this.label75.TabIndex = 37;
            this.label75.Text = "µm/Pixel";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label76
            // 
            this.label76.BackColor = System.Drawing.SystemColors.Control;
            this.label76.Location = new System.Drawing.Point(256, 24);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(48, 24);
            this.label76.TabIndex = 36;
            this.label76.Text = "µm/Pixel";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label77
            // 
            this.label77.BackColor = System.Drawing.SystemColors.Control;
            this.label77.Location = new System.Drawing.Point(256, 0);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(48, 24);
            this.label77.TabIndex = 35;
            this.label77.Text = "µm/Pixel";
            this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MotionZUpDown
            // 
            this.MotionZUpDown.DecimalPlaces = 3;
            this.MotionZUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MotionZUpDown.Location = new System.Drawing.Point(104, 48);
            this.MotionZUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MotionZUpDown.Name = "MotionZUpDown";
            this.MotionZUpDown.Size = new System.Drawing.Size(136, 20);
            this.MotionZUpDown.TabIndex = 34;
            this.MotionZUpDown.ValueChanged += new System.EventHandler(this.MotionZUpDown_ValueChanged);
            this.MotionZUpDown.Leave += new System.EventHandler(this.MotionZUpDown_Leave);
            // 
            // MotionYUpDown
            // 
            this.MotionYUpDown.DecimalPlaces = 3;
            this.MotionYUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MotionYUpDown.Location = new System.Drawing.Point(104, 24);
            this.MotionYUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MotionYUpDown.Name = "MotionYUpDown";
            this.MotionYUpDown.Size = new System.Drawing.Size(136, 20);
            this.MotionYUpDown.TabIndex = 33;
            this.MotionYUpDown.ValueChanged += new System.EventHandler(this.MotionYUpDown_ValueChanged);
            this.MotionYUpDown.Leave += new System.EventHandler(this.MotionYUpDown_Leave);
            // 
            // MotionXUpDown
            // 
            this.MotionXUpDown.DecimalPlaces = 3;
            this.MotionXUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MotionXUpDown.Location = new System.Drawing.Point(104, 0);
            this.MotionXUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MotionXUpDown.Name = "MotionXUpDown";
            this.MotionXUpDown.Size = new System.Drawing.Size(136, 20);
            this.MotionXUpDown.TabIndex = 32;
            this.MotionXUpDown.ValueChanged += new System.EventHandler(this.MotionXUpDown_ValueChanged);
            this.MotionXUpDown.Leave += new System.EventHandler(this.MotionXUpDown_Leave);
            // 
            // label71
            // 
            this.label71.Location = new System.Drawing.Point(16, 184);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(88, 24);
            this.label71.TabIndex = 22;
            this.label71.Text = "Focal Length ";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label70
            // 
            this.label70.Location = new System.Drawing.Point(16, 160);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(88, 24);
            this.label70.TabIndex = 21;
            this.label70.Text = "Cell Height (Sy)";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label69
            // 
            this.label69.Location = new System.Drawing.Point(16, 136);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(88, 24);
            this.label69.TabIndex = 20;
            this.label69.Text = "Cell Width  (Sx)";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(264, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 24);
            this.label7.TabIndex = 19;
            this.label7.Text = "mm";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(264, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 24);
            this.label6.TabIndex = 18;
            this.label6.Text = "µm";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(264, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 24);
            this.label5.TabIndex = 17;
            this.label5.Text = "µm";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TelecentricCheckBox
            // 
            this.TelecentricCheckBox.Location = new System.Drawing.Point(312, 184);
            this.TelecentricCheckBox.Name = "TelecentricCheckBox";
            this.TelecentricCheckBox.Size = new System.Drawing.Size(88, 24);
            this.TelecentricCheckBox.TabIndex = 16;
            this.TelecentricCheckBox.Text = "Telecentric";
            this.TelecentricCheckBox.CheckedChanged += new System.EventHandler(this.checkBoxTelecentric_CheckedChanged);
            // 
            // FocalLengthUpDown
            // 
            this.FocalLengthUpDown.DecimalPlaces = 3;
            this.FocalLengthUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.FocalLengthUpDown.Location = new System.Drawing.Point(112, 184);
            this.FocalLengthUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.FocalLengthUpDown.Name = "FocalLengthUpDown";
            this.FocalLengthUpDown.Size = new System.Drawing.Size(136, 20);
            this.FocalLengthUpDown.TabIndex = 15;
            this.FocalLengthUpDown.ValueChanged += new System.EventHandler(this.numUpDownFocalLength_ValueChanged);
            this.FocalLengthUpDown.Leave += new System.EventHandler(this.FocalLengthUpDown_Leave);
            // 
            // SyUpDown
            // 
            this.SyUpDown.DecimalPlaces = 3;
            this.SyUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SyUpDown.Location = new System.Drawing.Point(112, 160);
            this.SyUpDown.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.SyUpDown.Name = "SyUpDown";
            this.SyUpDown.Size = new System.Drawing.Size(136, 20);
            this.SyUpDown.TabIndex = 14;
            this.SyUpDown.ValueChanged += new System.EventHandler(this.numUpDownSy_ValueChanged);
            this.SyUpDown.Leave += new System.EventHandler(this.SyUpDown_Leave);
            // 
            // SxUpDown
            // 
            this.SxUpDown.DecimalPlaces = 3;
            this.SxUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SxUpDown.Location = new System.Drawing.Point(112, 136);
            this.SxUpDown.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.SxUpDown.Name = "SxUpDown";
            this.SxUpDown.Size = new System.Drawing.Size(136, 20);
            this.SxUpDown.TabIndex = 13;
            this.SxUpDown.ValueChanged += new System.EventHandler(this.numUpDownSx_ValueChanged);
            this.SxUpDown.Leave += new System.EventHandler(this.SxUpDown_Leave);
            // 
            // buttonImportParams
            // 
            this.buttonImportParams.Location = new System.Drawing.Point(272, 96);
            this.buttonImportParams.Name = "buttonImportParams";
            this.buttonImportParams.Size = new System.Drawing.Size(120, 24);
            this.buttonImportParams.TabIndex = 9;
            this.buttonImportParams.Text = "Import Parameters ...";
            this.buttonImportParams.Click += new System.EventHandler(this.buttonImportParams_Click);
            // 
            // textBoxDescr
            // 
            this.textBoxDescr.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBoxDescr.Location = new System.Drawing.Point(112, 32);
            this.textBoxDescr.Name = "textBoxDescr";
            this.textBoxDescr.ReadOnly = true;
            this.textBoxDescr.Size = new System.Drawing.Size(144, 20);
            this.textBoxDescr.TabIndex = 8;
            this.textBoxDescr.Text = ".descr";
            // 
            // CamTypComboBox
            // 
            this.CamTypComboBox.Items.AddRange(new object[] {
            "Area Scan  (Division)",
            "Area Scan  (Polynomial)",
            "Line Scan"});
            this.CamTypComboBox.Location = new System.Drawing.Point(112, 96);
            this.CamTypComboBox.Name = "CamTypComboBox";
            this.CamTypComboBox.Size = new System.Drawing.Size(144, 21);
            this.CamTypComboBox.TabIndex = 7;
            this.CamTypComboBox.Text = "Area Scan  (Division)";
            this.CamTypComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBoxCamTyp_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Camera Type";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(272, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "mm";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ThicknessUpDown
            // 
            this.ThicknessUpDown.DecimalPlaces = 3;
            this.ThicknessUpDown.Location = new System.Drawing.Point(112, 56);
            this.ThicknessUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ThicknessUpDown.Name = "ThicknessUpDown";
            this.ThicknessUpDown.Size = new System.Drawing.Size(144, 20);
            this.ThicknessUpDown.TabIndex = 4;
            this.ThicknessUpDown.ValueChanged += new System.EventHandler(this.numUpDownThickness_ValueChanged);
            this.ThicknessUpDown.Leave += new System.EventHandler(this.ThicknessUpDown_Leave);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Thickness";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonLoadDescrFile
            // 
            this.buttonLoadDescrFile.Location = new System.Drawing.Point(272, 32);
            this.buttonLoadDescrFile.Name = "buttonLoadDescrFile";
            this.buttonLoadDescrFile.Size = new System.Drawing.Size(120, 24);
            this.buttonLoadDescrFile.TabIndex = 2;
            this.buttonLoadDescrFile.Text = "Load File";
            this.buttonLoadDescrFile.Click += new System.EventHandler(this.buttonLoadDescrFile_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Description File";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Enabled = false;
            this.buttonCalibrate.Location = new System.Drawing.Point(328, 224);
            this.buttonCalibrate.Name = "buttonCalibrate";
            this.buttonCalibrate.Size = new System.Drawing.Size(84, 24);
            this.buttonCalibrate.TabIndex = 19;
            this.buttonCalibrate.Text = "Calibrate";
            this.buttonCalibrate.Click += new System.EventHandler(this.buttonCalibrate_Click);
            // 
            // buttonSetReference
            // 
            this.buttonSetReference.Enabled = false;
            this.buttonSetReference.Location = new System.Drawing.Point(328, 192);
            this.buttonSetReference.Name = "buttonSetReference";
            this.buttonSetReference.Size = new System.Drawing.Size(84, 24);
            this.buttonSetReference.TabIndex = 18;
            this.buttonSetReference.Text = "Set Reference";
            this.buttonSetReference.Click += new System.EventHandler(this.buttonSetReference_Click);
            // 
            // buttonDeleteAll
            // 
            this.buttonDeleteAll.Location = new System.Drawing.Point(328, 96);
            this.buttonDeleteAll.Name = "buttonDeleteAll";
            this.buttonDeleteAll.Size = new System.Drawing.Size(84, 24);
            this.buttonDeleteAll.TabIndex = 15;
            this.buttonDeleteAll.Text = "Delete All";
            this.buttonDeleteAll.Click += new System.EventHandler(this.buttonDeleteAll_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(328, 64);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(84, 24);
            this.buttonDelete.TabIndex = 14;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(328, 32);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(84, 24);
            this.buttonLoad.TabIndex = 8;
            this.buttonLoad.Text = "Load ...";
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // tabPageQualityCheck
            // 
            this.tabPageQualityCheck.Controls.Add(this.WarnlevelUpDown);
            this.tabPageQualityCheck.Controls.Add(this.ListQualityCheck);
            this.tabPageQualityCheck.Controls.Add(this.groupBox4);
            this.tabPageQualityCheck.Controls.Add(this.label10);
            this.tabPageQualityCheck.Controls.Add(this.label9);
            this.tabPageQualityCheck.Controls.Add(this.SeqTestsComboBox);
            this.tabPageQualityCheck.Controls.Add(this.label8);
            this.tabPageQualityCheck.Controls.Add(this.ImgTestsComboBox);
            this.tabPageQualityCheck.Location = new System.Drawing.Point(4, 22);
            this.tabPageQualityCheck.Name = "tabPageQualityCheck";
            this.tabPageQualityCheck.Size = new System.Drawing.Size(424, 598);
            this.tabPageQualityCheck.TabIndex = 1;
            this.tabPageQualityCheck.Text = "Image Quality Check";
            // 
            // WarnlevelUpDown
            // 
            this.WarnlevelUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.WarnlevelUpDown.Location = new System.Drawing.Point(328, 204);
            this.WarnlevelUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.WarnlevelUpDown.Name = "WarnlevelUpDown";
            this.WarnlevelUpDown.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.WarnlevelUpDown.Size = new System.Drawing.Size(80, 20);
            this.WarnlevelUpDown.TabIndex = 23;
            this.WarnlevelUpDown.ValueChanged += new System.EventHandler(this.numUpDownWarnlevel_ValueChanged);
            // 
            // ListQualityCheck
            // 
            this.ListQualityCheck.AutoArrange = false;
            this.ListQualityCheck.BackColor = System.Drawing.SystemColors.Window;
            this.ListQualityCheck.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColScope,
            this.ColDescr,
            this.Col2Status});
            this.ListQualityCheck.FullRowSelect = true;
            this.ListQualityCheck.GridLines = true;
            this.ListQualityCheck.LabelWrap = false;
            this.ListQualityCheck.Location = new System.Drawing.Point(8, 32);
            this.ListQualityCheck.MultiSelect = false;
            this.ListQualityCheck.Name = "ListQualityCheck";
            this.ListQualityCheck.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ListQualityCheck.Size = new System.Drawing.Size(312, 192);
            this.ListQualityCheck.TabIndex = 22;
            this.ListQualityCheck.UseCompatibleStateImageBehavior = false;
            this.ListQualityCheck.View = System.Windows.Forms.View.Details;
            // 
            // ColScope
            // 
            this.ColScope.Text = "Scope";
            this.ColScope.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColScope.Width = 65;
            // 
            // ColDescr
            // 
            this.ColDescr.Text = "Description";
            this.ColDescr.Width = 192;
            // 
            // Col2Status
            // 
            this.Col2Status.Text = "Status";
            this.Col2Status.Width = 50;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.MaxDiamTrackBar);
            this.groupBox4.Controls.Add(this.MaxDiamResetButton);
            this.groupBox4.Controls.Add(this.MaxDiamUpDown);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.MinContLResetButton);
            this.groupBox4.Controls.Add(this.MinContLTrackBar);
            this.groupBox4.Controls.Add(this.MinContLUpDown);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.SmootingResetButton);
            this.groupBox4.Controls.Add(this.SmoothTrackBar);
            this.groupBox4.Controls.Add(this.SmoothUpDown);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.MinThreshResetButton);
            this.groupBox4.Controls.Add(this.MinThreshTrackBar);
            this.groupBox4.Controls.Add(this.MinThreshUpDown);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.ThreshDecrResetButton);
            this.groupBox4.Controls.Add(this.ThreshDecrTrackBar);
            this.groupBox4.Controls.Add(this.ThreshDecrUpDown);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.InitThreshResetButton);
            this.groupBox4.Controls.Add(this.InitThreshTrackBar);
            this.groupBox4.Controls.Add(this.InitThreshUpDown);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.MinDiamResetButton);
            this.groupBox4.Controls.Add(this.MinDiamTrackBar);
            this.groupBox4.Controls.Add(this.MinDiamUpDown);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.MarkThreshResetButton);
            this.groupBox4.Controls.Add(this.MarkThreshTrackBar);
            this.groupBox4.Controls.Add(this.MarkThreshUpDown);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.FilterSizeResetButton);
            this.groupBox4.Controls.Add(this.FilterSizeTrackBar);
            this.groupBox4.Controls.Add(this.FilterSizeUpDown);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(8, 256);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(410, 336);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Calibration Plate Extraction Parameters";
            // 
            // MaxDiamTrackBar
            // 
            this.MaxDiamTrackBar.LargeChange = 20;
            this.MaxDiamTrackBar.Location = new System.Drawing.Point(160, 288);
            this.MaxDiamTrackBar.Maximum = 500;
            this.MaxDiamTrackBar.Name = "MaxDiamTrackBar";
            this.MaxDiamTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MaxDiamTrackBar.SmallChange = 10;
            this.MaxDiamTrackBar.TabIndex = 34;
            this.MaxDiamTrackBar.TickFrequency = 20;
            this.MaxDiamTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MaxDiamTrackBar.Scroll += new System.EventHandler(this.trackBarMaxDiam_Scroll);
            // 
            // MaxDiamResetButton
            // 
            this.MaxDiamResetButton.ForeColor = System.Drawing.Color.Gray;
            this.MaxDiamResetButton.Location = new System.Drawing.Point(352, 288);
            this.MaxDiamResetButton.Name = "MaxDiamResetButton";
            this.MaxDiamResetButton.Size = new System.Drawing.Size(48, 24);
            this.MaxDiamResetButton.TabIndex = 35;
            this.MaxDiamResetButton.Text = "Reset";
            this.MaxDiamResetButton.Click += new System.EventHandler(this.buttonRMaxDiam_Click);
            // 
            // MaxDiamUpDown
            // 
            this.MaxDiamUpDown.Location = new System.Drawing.Point(96, 288);
            this.MaxDiamUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.MaxDiamUpDown.Name = "MaxDiamUpDown";
            this.MaxDiamUpDown.Size = new System.Drawing.Size(64, 20);
            this.MaxDiamUpDown.TabIndex = 33;
            this.MaxDiamUpDown.ValueChanged += new System.EventHandler(this.numUpDownMaxDiam_ValueChanged);
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(8, 288);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(88, 24);
            this.label19.TabIndex = 32;
            this.label19.Text = "Max Mark Diam";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinContLResetButton
            // 
            this.MinContLResetButton.ForeColor = System.Drawing.Color.Gray;
            this.MinContLResetButton.Location = new System.Drawing.Point(352, 256);
            this.MinContLResetButton.Name = "MinContLResetButton";
            this.MinContLResetButton.Size = new System.Drawing.Size(48, 24);
            this.MinContLResetButton.TabIndex = 31;
            this.MinContLResetButton.Text = "Reset";
            this.MinContLResetButton.Click += new System.EventHandler(this.buttonRMinContL_Click);
            // 
            // MinContLTrackBar
            // 
            this.MinContLTrackBar.LargeChange = 20;
            this.MinContLTrackBar.Location = new System.Drawing.Point(160, 256);
            this.MinContLTrackBar.Maximum = 500;
            this.MinContLTrackBar.Name = "MinContLTrackBar";
            this.MinContLTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MinContLTrackBar.SmallChange = 10;
            this.MinContLTrackBar.TabIndex = 30;
            this.MinContLTrackBar.TickFrequency = 20;
            this.MinContLTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinContLTrackBar.Scroll += new System.EventHandler(this.trackBarMinContL_Scroll);
            // 
            // MinContLUpDown
            // 
            this.MinContLUpDown.Location = new System.Drawing.Point(96, 256);
            this.MinContLUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.MinContLUpDown.Name = "MinContLUpDown";
            this.MinContLUpDown.Size = new System.Drawing.Size(64, 20);
            this.MinContLUpDown.TabIndex = 29;
            this.MinContLUpDown.ValueChanged += new System.EventHandler(this.numUpDownMinContL_ValueChanged);
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(8, 256);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(88, 24);
            this.label18.TabIndex = 28;
            this.label18.Text = "Min Cont Length";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmootingResetButton
            // 
            this.SmootingResetButton.ForeColor = System.Drawing.Color.Gray;
            this.SmootingResetButton.Location = new System.Drawing.Point(352, 224);
            this.SmootingResetButton.Name = "SmootingResetButton";
            this.SmootingResetButton.Size = new System.Drawing.Size(48, 24);
            this.SmootingResetButton.TabIndex = 27;
            this.SmootingResetButton.Text = "Reset";
            this.SmootingResetButton.Click += new System.EventHandler(this.buttonRSmooting_Click);
            // 
            // SmoothTrackBar
            // 
            this.SmoothTrackBar.LargeChange = 10;
            this.SmoothTrackBar.Location = new System.Drawing.Point(160, 224);
            this.SmoothTrackBar.Maximum = 200;
            this.SmoothTrackBar.Minimum = 1;
            this.SmoothTrackBar.Name = "SmoothTrackBar";
            this.SmoothTrackBar.Size = new System.Drawing.Size(192, 45);
            this.SmoothTrackBar.SmallChange = 5;
            this.SmoothTrackBar.TabIndex = 26;
            this.SmoothTrackBar.TickFrequency = 10;
            this.SmoothTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.SmoothTrackBar.Value = 1;
            this.SmoothTrackBar.Scroll += new System.EventHandler(this.trackBarSmooth_Scroll);
            // 
            // SmoothUpDown
            // 
            this.SmoothUpDown.Location = new System.Drawing.Point(96, 224);
            this.SmoothUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.SmoothUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SmoothUpDown.Name = "SmoothUpDown";
            this.SmoothUpDown.Size = new System.Drawing.Size(64, 20);
            this.SmoothUpDown.TabIndex = 25;
            this.SmoothUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SmoothUpDown.ValueChanged += new System.EventHandler(this.numUpDownSmooth_ValueChanged);
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(8, 224);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 24);
            this.label17.TabIndex = 24;
            this.label17.Text = "Smoothing*100";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinThreshResetButton
            // 
            this.MinThreshResetButton.ForeColor = System.Drawing.Color.Gray;
            this.MinThreshResetButton.Location = new System.Drawing.Point(352, 192);
            this.MinThreshResetButton.Name = "MinThreshResetButton";
            this.MinThreshResetButton.Size = new System.Drawing.Size(48, 24);
            this.MinThreshResetButton.TabIndex = 23;
            this.MinThreshResetButton.Text = "Reset";
            this.MinThreshResetButton.Click += new System.EventHandler(this.buttonRMinThresh_Click);
            // 
            // MinThreshTrackBar
            // 
            this.MinThreshTrackBar.LargeChange = 10;
            this.MinThreshTrackBar.Location = new System.Drawing.Point(160, 192);
            this.MinThreshTrackBar.Maximum = 100;
            this.MinThreshTrackBar.Name = "MinThreshTrackBar";
            this.MinThreshTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MinThreshTrackBar.SmallChange = 5;
            this.MinThreshTrackBar.TabIndex = 22;
            this.MinThreshTrackBar.TickFrequency = 10;
            this.MinThreshTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinThreshTrackBar.Scroll += new System.EventHandler(this.trackBarMinThresh_Scroll);
            // 
            // MinThreshUpDown
            // 
            this.MinThreshUpDown.Location = new System.Drawing.Point(96, 192);
            this.MinThreshUpDown.Name = "MinThreshUpDown";
            this.MinThreshUpDown.Size = new System.Drawing.Size(64, 20);
            this.MinThreshUpDown.TabIndex = 21;
            this.MinThreshUpDown.ValueChanged += new System.EventHandler(this.numUpDownMinThresh_ValueChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 192);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 24);
            this.label16.TabIndex = 20;
            this.label16.Text = "Min Thresh";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ThreshDecrResetButton
            // 
            this.ThreshDecrResetButton.ForeColor = System.Drawing.Color.Gray;
            this.ThreshDecrResetButton.Location = new System.Drawing.Point(352, 160);
            this.ThreshDecrResetButton.Name = "ThreshDecrResetButton";
            this.ThreshDecrResetButton.Size = new System.Drawing.Size(48, 24);
            this.ThreshDecrResetButton.TabIndex = 19;
            this.ThreshDecrResetButton.Text = "Reset";
            this.ThreshDecrResetButton.Click += new System.EventHandler(this.buttonRThreshDecr_Click);
            // 
            // ThreshDecrTrackBar
            // 
            this.ThreshDecrTrackBar.LargeChange = 10;
            this.ThreshDecrTrackBar.Location = new System.Drawing.Point(160, 160);
            this.ThreshDecrTrackBar.Maximum = 100;
            this.ThreshDecrTrackBar.Name = "ThreshDecrTrackBar";
            this.ThreshDecrTrackBar.Size = new System.Drawing.Size(192, 45);
            this.ThreshDecrTrackBar.SmallChange = 5;
            this.ThreshDecrTrackBar.TabIndex = 18;
            this.ThreshDecrTrackBar.TickFrequency = 10;
            this.ThreshDecrTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ThreshDecrTrackBar.Scroll += new System.EventHandler(this.trackBarThreshDecr_Scroll);
            // 
            // ThreshDecrUpDown
            // 
            this.ThreshDecrUpDown.Location = new System.Drawing.Point(96, 160);
            this.ThreshDecrUpDown.Name = "ThreshDecrUpDown";
            this.ThreshDecrUpDown.Size = new System.Drawing.Size(64, 20);
            this.ThreshDecrUpDown.TabIndex = 17;
            this.ThreshDecrUpDown.ValueChanged += new System.EventHandler(this.numUpDownThreshDecr_ValueChanged);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 160);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(72, 24);
            this.label15.TabIndex = 16;
            this.label15.Text = "Thresh Decr";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InitThreshResetButton
            // 
            this.InitThreshResetButton.ForeColor = System.Drawing.Color.Gray;
            this.InitThreshResetButton.Location = new System.Drawing.Point(352, 128);
            this.InitThreshResetButton.Name = "InitThreshResetButton";
            this.InitThreshResetButton.Size = new System.Drawing.Size(48, 24);
            this.InitThreshResetButton.TabIndex = 15;
            this.InitThreshResetButton.Text = "Reset";
            this.InitThreshResetButton.Click += new System.EventHandler(this.buttonRInitThresh_Click);
            // 
            // InitThreshTrackBar
            // 
            this.InitThreshTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InitThreshTrackBar.LargeChange = 10;
            this.InitThreshTrackBar.Location = new System.Drawing.Point(160, 128);
            this.InitThreshTrackBar.Maximum = 255;
            this.InitThreshTrackBar.Name = "InitThreshTrackBar";
            this.InitThreshTrackBar.Size = new System.Drawing.Size(192, 45);
            this.InitThreshTrackBar.SmallChange = 5;
            this.InitThreshTrackBar.TabIndex = 14;
            this.InitThreshTrackBar.TickFrequency = 10;
            this.InitThreshTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.InitThreshTrackBar.Scroll += new System.EventHandler(this.trackBarInitThresh_Scroll);
            // 
            // InitThreshUpDown
            // 
            this.InitThreshUpDown.Location = new System.Drawing.Point(96, 128);
            this.InitThreshUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.InitThreshUpDown.Name = "InitThreshUpDown";
            this.InitThreshUpDown.Size = new System.Drawing.Size(64, 20);
            this.InitThreshUpDown.TabIndex = 13;
            this.InitThreshUpDown.ValueChanged += new System.EventHandler(this.numUpDownInitThresh_ValueChanged);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(8, 128);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 24);
            this.label14.TabIndex = 12;
            this.label14.Text = "Init Thresh";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinDiamResetButton
            // 
            this.MinDiamResetButton.ForeColor = System.Drawing.Color.Gray;
            this.MinDiamResetButton.Location = new System.Drawing.Point(352, 96);
            this.MinDiamResetButton.Name = "MinDiamResetButton";
            this.MinDiamResetButton.Size = new System.Drawing.Size(48, 24);
            this.MinDiamResetButton.TabIndex = 11;
            this.MinDiamResetButton.Text = "Reset";
            this.MinDiamResetButton.Click += new System.EventHandler(this.buttonRMinDiam_Click);
            // 
            // MinDiamTrackBar
            // 
            this.MinDiamTrackBar.LargeChange = 10;
            this.MinDiamTrackBar.Location = new System.Drawing.Point(160, 96);
            this.MinDiamTrackBar.Maximum = 100;
            this.MinDiamTrackBar.Name = "MinDiamTrackBar";
            this.MinDiamTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MinDiamTrackBar.SmallChange = 5;
            this.MinDiamTrackBar.TabIndex = 10;
            this.MinDiamTrackBar.TickFrequency = 10;
            this.MinDiamTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MinDiamTrackBar.Scroll += new System.EventHandler(this.trackBarMinDiam_Scroll);
            // 
            // MinDiamUpDown
            // 
            this.MinDiamUpDown.Location = new System.Drawing.Point(96, 96);
            this.MinDiamUpDown.Name = "MinDiamUpDown";
            this.MinDiamUpDown.Size = new System.Drawing.Size(64, 20);
            this.MinDiamUpDown.TabIndex = 9;
            this.MinDiamUpDown.ValueChanged += new System.EventHandler(this.numUpDownMinDiam_ValueChanged);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(8, 96);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 24);
            this.label13.TabIndex = 8;
            this.label13.Text = "Min Mark Diam";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MarkThreshResetButton
            // 
            this.MarkThreshResetButton.ForeColor = System.Drawing.Color.Gray;
            this.MarkThreshResetButton.Location = new System.Drawing.Point(352, 64);
            this.MarkThreshResetButton.Name = "MarkThreshResetButton";
            this.MarkThreshResetButton.Size = new System.Drawing.Size(48, 24);
            this.MarkThreshResetButton.TabIndex = 7;
            this.MarkThreshResetButton.Text = "Reset";
            this.MarkThreshResetButton.Click += new System.EventHandler(this.buttonRMarkThresh_Click);
            // 
            // MarkThreshTrackBar
            // 
            this.MarkThreshTrackBar.LargeChange = 10;
            this.MarkThreshTrackBar.Location = new System.Drawing.Point(160, 64);
            this.MarkThreshTrackBar.Maximum = 255;
            this.MarkThreshTrackBar.Name = "MarkThreshTrackBar";
            this.MarkThreshTrackBar.Size = new System.Drawing.Size(192, 45);
            this.MarkThreshTrackBar.SmallChange = 5;
            this.MarkThreshTrackBar.TabIndex = 6;
            this.MarkThreshTrackBar.TickFrequency = 10;
            this.MarkThreshTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MarkThreshTrackBar.Scroll += new System.EventHandler(this.trackBarMarkThresh_Scroll);
            // 
            // MarkThreshUpDown
            // 
            this.MarkThreshUpDown.Location = new System.Drawing.Point(96, 64);
            this.MarkThreshUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.MarkThreshUpDown.Name = "MarkThreshUpDown";
            this.MarkThreshUpDown.Size = new System.Drawing.Size(64, 20);
            this.MarkThreshUpDown.TabIndex = 5;
            this.MarkThreshUpDown.ValueChanged += new System.EventHandler(this.numUpDownMarkThresh_ValueChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(8, 64);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 24);
            this.label12.TabIndex = 4;
            this.label12.Text = "Mark Thresh";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FilterSizeResetButton
            // 
            this.FilterSizeResetButton.ForeColor = System.Drawing.Color.Gray;
            this.FilterSizeResetButton.Location = new System.Drawing.Point(352, 32);
            this.FilterSizeResetButton.Name = "FilterSizeResetButton";
            this.FilterSizeResetButton.Size = new System.Drawing.Size(48, 24);
            this.FilterSizeResetButton.TabIndex = 3;
            this.FilterSizeResetButton.Text = "Reset";
            this.FilterSizeResetButton.Click += new System.EventHandler(this.buttonRFilterSize_Click);
            // 
            // FilterSizeTrackBar
            // 
            this.FilterSizeTrackBar.LargeChange = 1;
            this.FilterSizeTrackBar.Location = new System.Drawing.Point(160, 32);
            this.FilterSizeTrackBar.Maximum = 15;
            this.FilterSizeTrackBar.Minimum = 1;
            this.FilterSizeTrackBar.Name = "FilterSizeTrackBar";
            this.FilterSizeTrackBar.Size = new System.Drawing.Size(192, 45);
            this.FilterSizeTrackBar.TabIndex = 2;
            this.FilterSizeTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.FilterSizeTrackBar.Value = 1;
            this.FilterSizeTrackBar.Scroll += new System.EventHandler(this.trackBarFilterSize_Scroll);
            // 
            // FilterSizeUpDown
            // 
            this.FilterSizeUpDown.Location = new System.Drawing.Point(96, 32);
            this.FilterSizeUpDown.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.FilterSizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FilterSizeUpDown.Name = "FilterSizeUpDown";
            this.FilterSizeUpDown.Size = new System.Drawing.Size(64, 20);
            this.FilterSizeUpDown.TabIndex = 1;
            this.FilterSizeUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.FilterSizeUpDown.ValueChanged += new System.EventHandler(this.numUpDownFilterSize_ValueChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(8, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 24);
            this.label11.TabIndex = 0;
            this.label11.Text = "Filter Size";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(328, 180);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 24);
            this.label10.TabIndex = 6;
            this.label10.Text = "Warn Level (%)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(328, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 24);
            this.label9.TabIndex = 4;
            this.label9.Text = "Sequence Tests";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SeqTestsComboBox
            // 
            this.SeqTestsComboBox.Items.AddRange(new object[] {
            "All",
            "Quick",
            "None"});
            this.SeqTestsComboBox.Location = new System.Drawing.Point(328, 128);
            this.SeqTestsComboBox.Name = "SeqTestsComboBox";
            this.SeqTestsComboBox.Size = new System.Drawing.Size(80, 21);
            this.SeqTestsComboBox.TabIndex = 3;
            this.SeqTestsComboBox.Text = "All";
            this.SeqTestsComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeqTests_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(328, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(88, 24);
            this.label8.TabIndex = 2;
            this.label8.Text = "Image Tests";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImgTestsComboBox
            // 
            this.ImgTestsComboBox.Items.AddRange(new object[] {
            "All",
            "Quick",
            "None"});
            this.ImgTestsComboBox.Location = new System.Drawing.Point(328, 56);
            this.ImgTestsComboBox.Name = "ImgTestsComboBox";
            this.ImgTestsComboBox.Size = new System.Drawing.Size(80, 21);
            this.ImgTestsComboBox.TabIndex = 1;
            this.ImgTestsComboBox.Text = "All";
            this.ImgTestsComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBoxImgTests_SelectedIndexChanged);
            // 
            // tabPageResults
            // 
            this.tabPageResults.Controls.Add(this.label20);
            this.tabPageResults.Controls.Add(this.buttonSimImg);
            this.tabPageResults.Controls.Add(this.buttonRefImg);
            this.tabPageResults.Controls.Add(this.groupBox6);
            this.tabPageResults.Controls.Add(this.groupBox5);
            this.tabPageResults.Controls.Add(this.label22);
            this.tabPageResults.Controls.Add(this.ErrorLabel);
            this.tabPageResults.Controls.Add(this.label21);
            this.tabPageResults.Controls.Add(this.StatusCalibLabel);
            this.tabPageResults.Location = new System.Drawing.Point(4, 22);
            this.tabPageResults.Name = "tabPageResults";
            this.tabPageResults.Size = new System.Drawing.Size(424, 598);
            this.tabPageResults.TabIndex = 2;
            this.tabPageResults.Text = "Results";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(368, 52);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(40, 24);
            this.label20.TabIndex = 21;
            this.label20.Text = " pixels";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSimImg
            // 
            this.buttonSimImg.Location = new System.Drawing.Point(24, 568);
            this.buttonSimImg.Name = "buttonSimImg";
            this.buttonSimImg.Size = new System.Drawing.Size(168, 24);
            this.buttonSimImg.TabIndex = 9;
            this.buttonSimImg.Text = " Simulated Reference Image";
            this.buttonSimImg.CheckedChanged += new System.EventHandler(this.radioSimulatedImg_CheckedChanged);
            // 
            // buttonRefImg
            // 
            this.buttonRefImg.Checked = true;
            this.buttonRefImg.Location = new System.Drawing.Point(24, 544);
            this.buttonRefImg.Name = "buttonRefImg";
            this.buttonRefImg.Size = new System.Drawing.Size(168, 24);
            this.buttonRefImg.TabIndex = 8;
            this.buttonRefImg.TabStop = true;
            this.buttonRefImg.Text = " Original Reference Image";
            this.buttonRefImg.CheckedChanged += new System.EventHandler(this.buttonRefImg_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.checkBoxOrigImgCoord);
            this.groupBox6.Controls.Add(this.buttonSaveCamPose);
            this.groupBox6.Controls.Add(this.label59);
            this.groupBox6.Controls.Add(this.label60);
            this.groupBox6.Controls.Add(this.label61);
            this.groupBox6.Controls.Add(this.label62);
            this.groupBox6.Controls.Add(this.CamGammaLabel);
            this.groupBox6.Controls.Add(this.label64);
            this.groupBox6.Controls.Add(this.CamBetaLabel);
            this.groupBox6.Controls.Add(this.label66);
            this.groupBox6.Controls.Add(this.CamAlphaLabel);
            this.groupBox6.Controls.Add(this.label50);
            this.groupBox6.Controls.Add(this.label51);
            this.groupBox6.Controls.Add(this.label52);
            this.groupBox6.Controls.Add(this.label53);
            this.groupBox6.Controls.Add(this.CamPoseZLabel);
            this.groupBox6.Controls.Add(this.label55);
            this.groupBox6.Controls.Add(this.CamPoseYLabel);
            this.groupBox6.Controls.Add(this.label57);
            this.groupBox6.Controls.Add(this.CamPoseXLabel);
            this.groupBox6.Location = new System.Drawing.Point(8, 408);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(408, 128);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Camera Pose";
            // 
            // checkBoxOrigImgCoord
            // 
            this.checkBoxOrigImgCoord.Location = new System.Drawing.Point(16, 104);
            this.checkBoxOrigImgCoord.Name = "checkBoxOrigImgCoord";
            this.checkBoxOrigImgCoord.Size = new System.Drawing.Size(144, 16);
            this.checkBoxOrigImgCoord.TabIndex = 42;
            this.checkBoxOrigImgCoord.Text = "Origin at Image Corner";
            this.checkBoxOrigImgCoord.CheckedChanged += new System.EventHandler(this.checkBoxOrigImgCoord_CheckedChanged);
            // 
            // buttonSaveCamPose
            // 
            this.buttonSaveCamPose.Location = new System.Drawing.Point(336, 24);
            this.buttonSaveCamPose.Name = "buttonSaveCamPose";
            this.buttonSaveCamPose.Size = new System.Drawing.Size(64, 24);
            this.buttonSaveCamPose.TabIndex = 41;
            this.buttonSaveCamPose.Text = "Save ...";
            this.buttonSaveCamPose.Click += new System.EventHandler(this.buttonSaveCamPose_Click);
            // 
            // label59
            // 
            this.label59.Location = new System.Drawing.Point(296, 72);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(24, 24);
            this.label59.TabIndex = 40;
            this.label59.Text = "deg";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label60
            // 
            this.label60.Location = new System.Drawing.Point(296, 48);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(24, 24);
            this.label60.TabIndex = 39;
            this.label60.Text = "deg";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label61
            // 
            this.label61.Location = new System.Drawing.Point(296, 24);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(24, 24);
            this.label61.TabIndex = 38;
            this.label61.Text = "deg";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.Location = new System.Drawing.Point(160, 72);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(48, 24);
            this.label62.TabIndex = 37;
            this.label62.Text = "Gamma";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamGammaLabel
            // 
            this.CamGammaLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamGammaLabel.Location = new System.Drawing.Point(208, 72);
            this.CamGammaLabel.Name = "CamGammaLabel";
            this.CamGammaLabel.Size = new System.Drawing.Size(88, 24);
            this.CamGammaLabel.TabIndex = 36;
            this.CamGammaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label64
            // 
            this.label64.Location = new System.Drawing.Point(160, 48);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(32, 24);
            this.label64.TabIndex = 35;
            this.label64.Text = "Beta";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamBetaLabel
            // 
            this.CamBetaLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamBetaLabel.Location = new System.Drawing.Point(208, 48);
            this.CamBetaLabel.Name = "CamBetaLabel";
            this.CamBetaLabel.Size = new System.Drawing.Size(88, 24);
            this.CamBetaLabel.TabIndex = 34;
            this.CamBetaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label66
            // 
            this.label66.Location = new System.Drawing.Point(160, 24);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(40, 24);
            this.label66.TabIndex = 33;
            this.label66.Text = "Alpha";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamAlphaLabel
            // 
            this.CamAlphaLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamAlphaLabel.Location = new System.Drawing.Point(208, 24);
            this.CamAlphaLabel.Name = "CamAlphaLabel";
            this.CamAlphaLabel.Size = new System.Drawing.Size(88, 24);
            this.CamAlphaLabel.TabIndex = 32;
            this.CamAlphaLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label50
            // 
            this.label50.Location = new System.Drawing.Point(120, 72);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(24, 24);
            this.label50.TabIndex = 31;
            this.label50.Text = "mm";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label51
            // 
            this.label51.Location = new System.Drawing.Point(120, 48);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(24, 24);
            this.label51.TabIndex = 30;
            this.label51.Text = "mm";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.Location = new System.Drawing.Point(120, 24);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(24, 24);
            this.label52.TabIndex = 29;
            this.label52.Text = "mm";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label53
            // 
            this.label53.Location = new System.Drawing.Point(16, 72);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(16, 24);
            this.label53.TabIndex = 28;
            this.label53.Text = "Z";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamPoseZLabel
            // 
            this.CamPoseZLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamPoseZLabel.Location = new System.Drawing.Point(32, 72);
            this.CamPoseZLabel.Name = "CamPoseZLabel";
            this.CamPoseZLabel.Size = new System.Drawing.Size(88, 24);
            this.CamPoseZLabel.TabIndex = 27;
            this.CamPoseZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label55
            // 
            this.label55.Location = new System.Drawing.Point(16, 48);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(16, 24);
            this.label55.TabIndex = 26;
            this.label55.Text = "Y";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamPoseYLabel
            // 
            this.CamPoseYLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamPoseYLabel.Location = new System.Drawing.Point(32, 48);
            this.CamPoseYLabel.Name = "CamPoseYLabel";
            this.CamPoseYLabel.Size = new System.Drawing.Size(88, 24);
            this.CamPoseYLabel.TabIndex = 25;
            this.CamPoseYLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label57
            // 
            this.label57.Location = new System.Drawing.Point(16, 24);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(16, 24);
            this.label57.TabIndex = 24;
            this.label57.Text = "X";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CamPoseXLabel
            // 
            this.CamPoseXLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CamPoseXLabel.Location = new System.Drawing.Point(32, 24);
            this.CamPoseXLabel.Name = "CamPoseXLabel";
            this.CamPoseXLabel.Size = new System.Drawing.Size(88, 24);
            this.CamPoseXLabel.TabIndex = 23;
            this.CamPoseXLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.AreaScanPolynomPanel);
            this.groupBox5.Controls.Add(this.label44);
            this.groupBox5.Controls.Add(this.label30);
            this.groupBox5.Controls.Add(this.FocalLResultLabel);
            this.groupBox5.Controls.Add(this.KappaPanel);
            this.groupBox5.Controls.Add(this.buttonSaveCamParams);
            this.groupBox5.Controls.Add(this.label49);
            this.groupBox5.Controls.Add(this.label48);
            this.groupBox5.Controls.Add(this.label47);
            this.groupBox5.Controls.Add(this.label46);
            this.groupBox5.Controls.Add(this.label43);
            this.groupBox5.Controls.Add(this.label42);
            this.groupBox5.Controls.Add(this.label40);
            this.groupBox5.Controls.Add(this.ImgHResultLabel);
            this.groupBox5.Controls.Add(this.label38);
            this.groupBox5.Controls.Add(this.ImgWResultLabel);
            this.groupBox5.Controls.Add(this.label36);
            this.groupBox5.Controls.Add(this.CyResultLabel);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.CxResultLabel);
            this.groupBox5.Controls.Add(this.label28);
            this.groupBox5.Controls.Add(this.SyResultLabel);
            this.groupBox5.Controls.Add(this.label26);
            this.groupBox5.Controls.Add(this.SxResultLabel);
            this.groupBox5.Location = new System.Drawing.Point(8, 86);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(408, 322);
            this.groupBox5.TabIndex = 6;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Camera Paramters";
            // 
            // AreaScanPolynomPanel
            // 
            this.AreaScanPolynomPanel.Controls.Add(this.P2Label);
            this.AreaScanPolynomPanel.Controls.Add(this.label83);
            this.AreaScanPolynomPanel.Controls.Add(this.label84);
            this.AreaScanPolynomPanel.Controls.Add(this.P1Label);
            this.AreaScanPolynomPanel.Controls.Add(this.label80);
            this.AreaScanPolynomPanel.Controls.Add(this.label81);
            this.AreaScanPolynomPanel.Controls.Add(this.K3Label);
            this.AreaScanPolynomPanel.Controls.Add(this.K2Label);
            this.AreaScanPolynomPanel.Controls.Add(this.K1Label);
            this.AreaScanPolynomPanel.Controls.Add(this.label54);
            this.AreaScanPolynomPanel.Controls.Add(this.label56);
            this.AreaScanPolynomPanel.Controls.Add(this.label58);
            this.AreaScanPolynomPanel.Controls.Add(this.label63);
            this.AreaScanPolynomPanel.Controls.Add(this.label65);
            this.AreaScanPolynomPanel.Controls.Add(this.label67);
            this.AreaScanPolynomPanel.Location = new System.Drawing.Point(16, 192);
            this.AreaScanPolynomPanel.Name = "AreaScanPolynomPanel";
            this.AreaScanPolynomPanel.Size = new System.Drawing.Size(384, 120);
            this.AreaScanPolynomPanel.TabIndex = 47;
            this.AreaScanPolynomPanel.Visible = false;
            // 
            // P2Label
            // 
            this.P2Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P2Label.Location = new System.Drawing.Point(88, 96);
            this.P2Label.Name = "P2Label";
            this.P2Label.Size = new System.Drawing.Size(152, 24);
            this.P2Label.TabIndex = 49;
            this.P2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label83
            // 
            this.label83.Location = new System.Drawing.Point(0, 96);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(88, 24);
            this.label83.TabIndex = 48;
            this.label83.Text = "Tang 2nd  (P2)";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label84
            // 
            this.label84.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label84.Location = new System.Drawing.Point(248, 96);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(120, 24);
            this.label84.TabIndex = 47;
            this.label84.Text = "1/m²";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // P1Label
            // 
            this.P1Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.P1Label.Location = new System.Drawing.Point(88, 72);
            this.P1Label.Name = "P1Label";
            this.P1Label.Size = new System.Drawing.Size(152, 24);
            this.P1Label.TabIndex = 46;
            this.P1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label80
            // 
            this.label80.Location = new System.Drawing.Point(0, 72);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(88, 24);
            this.label80.TabIndex = 45;
            this.label80.Text = "Tang 2nd  (P1)";
            this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label81
            // 
            this.label81.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label81.Location = new System.Drawing.Point(248, 72);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(120, 24);
            this.label81.TabIndex = 44;
            this.label81.Text = "1/m²";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // K3Label
            // 
            this.K3Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.K3Label.Location = new System.Drawing.Point(88, 48);
            this.K3Label.Name = "K3Label";
            this.K3Label.Size = new System.Drawing.Size(152, 24);
            this.K3Label.TabIndex = 43;
            this.K3Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // K2Label
            // 
            this.K2Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.K2Label.Location = new System.Drawing.Point(88, 24);
            this.K2Label.Name = "K2Label";
            this.K2Label.Size = new System.Drawing.Size(152, 24);
            this.K2Label.TabIndex = 42;
            this.K2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // K1Label
            // 
            this.K1Label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.K1Label.Location = new System.Drawing.Point(88, 0);
            this.K1Label.Name = "K1Label";
            this.K1Label.Size = new System.Drawing.Size(152, 24);
            this.K1Label.TabIndex = 41;
            this.K1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label54
            // 
            this.label54.Location = new System.Drawing.Point(0, 48);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(88, 24);
            this.label54.TabIndex = 40;
            this.label54.Text = "Radial 6th (K3)";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label56
            // 
            this.label56.Location = new System.Drawing.Point(0, 24);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(88, 24);
            this.label56.TabIndex = 39;
            this.label56.Text = "Radial 4th (K2)";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label58
            // 
            this.label58.Location = new System.Drawing.Point(0, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(88, 24);
            this.label58.TabIndex = 38;
            this.label58.Text = "Radial 2nd (K1)";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label63
            // 
            this.label63.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label63.Location = new System.Drawing.Point(248, 48);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(120, 24);
            this.label63.TabIndex = 37;
            this.label63.Text = "m^ -6";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label65
            // 
            this.label65.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label65.Location = new System.Drawing.Point(248, 24);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(120, 24);
            this.label65.TabIndex = 36;
            this.label65.Text = "m^ -4";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label67
            // 
            this.label67.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label67.Location = new System.Drawing.Point(248, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(120, 24);
            this.label67.TabIndex = 35;
            this.label67.Text = "1/m²";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label44
            // 
            this.label44.Location = new System.Drawing.Point(264, 168);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(32, 24);
            this.label44.TabIndex = 46;
            this.label44.Text = "mm";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(16, 168);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(80, 24);
            this.label30.TabIndex = 43;
            this.label30.Text = "Focal Length";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FocalLResultLabel
            // 
            this.FocalLResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.FocalLResultLabel.Location = new System.Drawing.Point(104, 168);
            this.FocalLResultLabel.Name = "FocalLResultLabel";
            this.FocalLResultLabel.Size = new System.Drawing.Size(152, 24);
            this.FocalLResultLabel.TabIndex = 42;
            this.FocalLResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // KappaPanel
            // 
            this.KappaPanel.Controls.Add(this.LineScanPanel);
            this.KappaPanel.Controls.Add(this.label45);
            this.KappaPanel.Controls.Add(this.label32);
            this.KappaPanel.Controls.Add(this.KappaResultLabel);
            this.KappaPanel.Location = new System.Drawing.Point(16, 192);
            this.KappaPanel.Name = "KappaPanel";
            this.KappaPanel.Size = new System.Drawing.Size(344, 120);
            this.KappaPanel.TabIndex = 33;
            // 
            // LineScanPanel
            // 
            this.LineScanPanel.Controls.Add(this.VzResultLabel);
            this.LineScanPanel.Controls.Add(this.VyResultLabel);
            this.LineScanPanel.Controls.Add(this.VxResultLabel);
            this.LineScanPanel.Controls.Add(this.label82);
            this.LineScanPanel.Controls.Add(this.label85);
            this.LineScanPanel.Controls.Add(this.label86);
            this.LineScanPanel.Controls.Add(this.label87);
            this.LineScanPanel.Controls.Add(this.label88);
            this.LineScanPanel.Controls.Add(this.label89);
            this.LineScanPanel.Location = new System.Drawing.Point(0, 40);
            this.LineScanPanel.Name = "LineScanPanel";
            this.LineScanPanel.Size = new System.Drawing.Size(344, 72);
            this.LineScanPanel.TabIndex = 51;
            this.LineScanPanel.Visible = false;
            // 
            // VzResultLabel
            // 
            this.VzResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VzResultLabel.Location = new System.Drawing.Point(88, 48);
            this.VzResultLabel.Name = "VzResultLabel";
            this.VzResultLabel.Size = new System.Drawing.Size(152, 24);
            this.VzResultLabel.TabIndex = 43;
            this.VzResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VyResultLabel
            // 
            this.VyResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VyResultLabel.Location = new System.Drawing.Point(88, 24);
            this.VyResultLabel.Name = "VyResultLabel";
            this.VyResultLabel.Size = new System.Drawing.Size(152, 24);
            this.VyResultLabel.TabIndex = 42;
            this.VyResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VxResultLabel
            // 
            this.VxResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.VxResultLabel.Location = new System.Drawing.Point(88, 0);
            this.VxResultLabel.Name = "VxResultLabel";
            this.VxResultLabel.Size = new System.Drawing.Size(152, 24);
            this.VxResultLabel.TabIndex = 41;
            this.VxResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label82
            // 
            this.label82.Location = new System.Drawing.Point(0, 48);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(88, 24);
            this.label82.TabIndex = 40;
            this.label82.Text = "Motion Z  (Vz)";
            this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label85
            // 
            this.label85.Location = new System.Drawing.Point(0, 24);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(88, 24);
            this.label85.TabIndex = 39;
            this.label85.Text = "Motion Y  (Vy)";
            this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label86
            // 
            this.label86.Location = new System.Drawing.Point(0, 0);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(88, 24);
            this.label86.TabIndex = 38;
            this.label86.Text = "Motion X  (Vx)";
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label87
            // 
            this.label87.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label87.Location = new System.Drawing.Point(248, 48);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(48, 24);
            this.label87.TabIndex = 37;
            this.label87.Text = "µm/Pixel";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label88
            // 
            this.label88.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label88.Location = new System.Drawing.Point(248, 24);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(48, 24);
            this.label88.TabIndex = 36;
            this.label88.Text = "µm/Pixel";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label89
            // 
            this.label89.BackColor = System.Drawing.SystemColors.ControlLight;
            this.label89.Location = new System.Drawing.Point(248, 0);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(48, 24);
            this.label89.TabIndex = 35;
            this.label89.Text = "µm/Pixel";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(248, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(32, 24);
            this.label45.TabIndex = 50;
            this.label45.Text = "1/m²";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(0, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(64, 24);
            this.label32.TabIndex = 49;
            this.label32.Text = "Kappa";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // KappaResultLabel
            // 
            this.KappaResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.KappaResultLabel.Location = new System.Drawing.Point(88, 0);
            this.KappaResultLabel.Name = "KappaResultLabel";
            this.KappaResultLabel.Size = new System.Drawing.Size(152, 24);
            this.KappaResultLabel.TabIndex = 48;
            this.KappaResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonSaveCamParams
            // 
            this.buttonSaveCamParams.Location = new System.Drawing.Point(336, 24);
            this.buttonSaveCamParams.Name = "buttonSaveCamParams";
            this.buttonSaveCamParams.Size = new System.Drawing.Size(64, 24);
            this.buttonSaveCamParams.TabIndex = 28;
            this.buttonSaveCamParams.Text = "Save ...";
            this.buttonSaveCamParams.Click += new System.EventHandler(this.buttonSaveCamParams_Click);
            // 
            // label49
            // 
            this.label49.Location = new System.Drawing.Point(264, 144);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(40, 24);
            this.label49.TabIndex = 27;
            this.label49.Text = "pixels";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label48
            // 
            this.label48.Location = new System.Drawing.Point(264, 120);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(40, 24);
            this.label48.TabIndex = 26;
            this.label48.Text = "pixels";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(264, 96);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(40, 24);
            this.label47.TabIndex = 25;
            this.label47.Text = "pixels";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(264, 72);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(40, 24);
            this.label46.TabIndex = 24;
            this.label46.Text = "pixels";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.Location = new System.Drawing.Point(264, 48);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(32, 24);
            this.label43.TabIndex = 21;
            this.label43.Text = "µm";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label42
            // 
            this.label42.Location = new System.Drawing.Point(264, 24);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(32, 24);
            this.label42.TabIndex = 20;
            this.label42.Text = "µm";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.Location = new System.Drawing.Point(16, 144);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(72, 24);
            this.label40.TabIndex = 19;
            this.label40.Text = "Image Height";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImgHResultLabel
            // 
            this.ImgHResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ImgHResultLabel.Location = new System.Drawing.Point(104, 144);
            this.ImgHResultLabel.Name = "ImgHResultLabel";
            this.ImgHResultLabel.Size = new System.Drawing.Size(152, 24);
            this.ImgHResultLabel.TabIndex = 18;
            this.ImgHResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label38
            // 
            this.label38.Location = new System.Drawing.Point(16, 120);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(72, 24);
            this.label38.TabIndex = 17;
            this.label38.Text = "Image Width";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ImgWResultLabel
            // 
            this.ImgWResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ImgWResultLabel.Location = new System.Drawing.Point(104, 120);
            this.ImgWResultLabel.Name = "ImgWResultLabel";
            this.ImgWResultLabel.Size = new System.Drawing.Size(152, 24);
            this.ImgWResultLabel.TabIndex = 16;
            this.ImgWResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(16, 96);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(88, 24);
            this.label36.TabIndex = 15;
            this.label36.Text = "Center Row (Cy)";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CyResultLabel
            // 
            this.CyResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CyResultLabel.Location = new System.Drawing.Point(104, 96);
            this.CyResultLabel.Name = "CyResultLabel";
            this.CyResultLabel.Size = new System.Drawing.Size(152, 24);
            this.CyResultLabel.TabIndex = 14;
            this.CyResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(16, 72);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(88, 24);
            this.label34.TabIndex = 13;
            this.label34.Text = "Center Col (Cx)";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CxResultLabel
            // 
            this.CxResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CxResultLabel.Location = new System.Drawing.Point(104, 72);
            this.CxResultLabel.Name = "CxResultLabel";
            this.CxResultLabel.Size = new System.Drawing.Size(152, 24);
            this.CxResultLabel.TabIndex = 12;
            this.CxResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(16, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(83, 24);
            this.label28.TabIndex = 7;
            this.label28.Text = "Cell Height (Sy)";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SyResultLabel
            // 
            this.SyResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SyResultLabel.Location = new System.Drawing.Point(104, 48);
            this.SyResultLabel.Name = "SyResultLabel";
            this.SyResultLabel.Size = new System.Drawing.Size(152, 24);
            this.SyResultLabel.TabIndex = 6;
            this.SyResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(16, 24);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(80, 24);
            this.label26.TabIndex = 5;
            this.label26.Text = "Cell Width (Sx)";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SxResultLabel
            // 
            this.SxResultLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SxResultLabel.Location = new System.Drawing.Point(104, 24);
            this.SxResultLabel.Name = "SxResultLabel";
            this.SxResultLabel.Size = new System.Drawing.Size(152, 24);
            this.SxResultLabel.TabIndex = 4;
            this.SxResultLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(16, 52);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 24);
            this.label22.TabIndex = 3;
            this.label22.Text = "Mean Error";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ErrorLabel.Location = new System.Drawing.Point(80, 52);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(280, 24);
            this.ErrorLabel.TabIndex = 2;
            this.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(16, 24);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(64, 24);
            this.label21.TabIndex = 1;
            this.label21.Text = "Status";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusCalibLabel
            // 
            this.StatusCalibLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusCalibLabel.Location = new System.Drawing.Point(80, 24);
            this.StatusCalibLabel.Name = "StatusCalibLabel";
            this.StatusCalibLabel.Size = new System.Drawing.Size(328, 24);
            this.StatusCalibLabel.TabIndex = 0;
            this.StatusCalibLabel.Text = "No calibration data available";
            this.StatusCalibLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonMove);
            this.groupBox1.Controls.Add(this.buttonReset);
            this.groupBox1.Controls.Add(this.buttonNone);
            this.groupBox1.Controls.Add(this.buttonMagnify);
            this.groupBox1.Controls.Add(this.buttonZoom);
            this.groupBox1.Location = new System.Drawing.Point(360, 440);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(112, 160);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View Interaction";
            // 
            // buttonMove
            // 
            this.buttonMove.Location = new System.Drawing.Point(16, 24);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(88, 24);
            this.buttonMove.TabIndex = 4;
            this.buttonMove.Text = "   move";
            this.buttonMove.CheckedChanged += new System.EventHandler(this.buttonMove_CheckedChanged);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(16, 128);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(80, 22);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset";
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonNone
            // 
            this.buttonNone.Checked = true;
            this.buttonNone.Location = new System.Drawing.Point(16, 96);
            this.buttonNone.Name = "buttonNone";
            this.buttonNone.Size = new System.Drawing.Size(88, 24);
            this.buttonNone.TabIndex = 2;
            this.buttonNone.TabStop = true;
            this.buttonNone.Text = "   none";
            this.buttonNone.CheckedChanged += new System.EventHandler(this.buttonNone_CheckedChanged);
            // 
            // buttonMagnify
            // 
            this.buttonMagnify.Location = new System.Drawing.Point(16, 72);
            this.buttonMagnify.Name = "buttonMagnify";
            this.buttonMagnify.Size = new System.Drawing.Size(88, 24);
            this.buttonMagnify.TabIndex = 1;
            this.buttonMagnify.Text = "   magnify";
            this.buttonMagnify.CheckedChanged += new System.EventHandler(this.buttonMagnify_CheckedChanged);
            // 
            // buttonZoom
            // 
            this.buttonZoom.Location = new System.Drawing.Point(16, 48);
            this.buttonZoom.Name = "buttonZoom";
            this.buttonZoom.Size = new System.Drawing.Size(88, 24);
            this.buttonZoom.TabIndex = 0;
            this.buttonZoom.Text = "   zoom";
            this.buttonZoom.CheckedChanged += new System.EventHandler(this.buttonZoom_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.upDownLineWidth);
            this.groupBox3.Controls.Add(this.label78);
            this.groupBox3.Controls.Add(this.label68);
            this.groupBox3.Controls.Add(this.checkBoxCoordSys);
            this.groupBox3.Controls.Add(this.checkBoxMarkCenter);
            this.groupBox3.Controls.Add(this.checkBoxPlateRegion);
            this.groupBox3.Controls.Add(this.comboBoxDraw);
            this.groupBox3.Controls.Add(this.comboBoxCoordSys);
            this.groupBox3.Controls.Add(this.comboBoxMarkCenters);
            this.groupBox3.Controls.Add(this.comboBoxPlateRegion);
            this.groupBox3.Location = new System.Drawing.Point(16, 440);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(312, 160);
            this.groupBox3.TabIndex = 26;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Display Parameters";
            // 
            // upDownLineWidth
            // 
            this.upDownLineWidth.Location = new System.Drawing.Point(164, 128);
            this.upDownLineWidth.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.upDownLineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownLineWidth.Name = "upDownLineWidth";
            this.upDownLineWidth.Size = new System.Drawing.Size(128, 20);
            this.upDownLineWidth.TabIndex = 36;
            this.upDownLineWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownLineWidth.ValueChanged += new System.EventHandler(this.upDownLineWidth_ValueChanged);
            // 
            // label78
            // 
            this.label78.Location = new System.Drawing.Point(40, 128);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(64, 24);
            this.label78.TabIndex = 35;
            this.label78.Text = " LineWidth";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label68
            // 
            this.label68.Location = new System.Drawing.Point(40, 104);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(64, 24);
            this.label68.TabIndex = 33;
            this.label68.Text = " Draw";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxCoordSys
            // 
            this.checkBoxCoordSys.Checked = true;
            this.checkBoxCoordSys.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCoordSys.Location = new System.Drawing.Point(20, 72);
            this.checkBoxCoordSys.Name = "checkBoxCoordSys";
            this.checkBoxCoordSys.Size = new System.Drawing.Size(128, 24);
            this.checkBoxCoordSys.TabIndex = 32;
            this.checkBoxCoordSys.Text = "  Coordinate System";
            this.checkBoxCoordSys.CheckedChanged += new System.EventHandler(this.checkBoxCoordSys_CheckedChanged);
            // 
            // checkBoxMarkCenter
            // 
            this.checkBoxMarkCenter.Checked = true;
            this.checkBoxMarkCenter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMarkCenter.Location = new System.Drawing.Point(20, 48);
            this.checkBoxMarkCenter.Name = "checkBoxMarkCenter";
            this.checkBoxMarkCenter.Size = new System.Drawing.Size(128, 24);
            this.checkBoxMarkCenter.TabIndex = 31;
            this.checkBoxMarkCenter.Text = "  Mark Centers";
            this.checkBoxMarkCenter.CheckedChanged += new System.EventHandler(this.checkBoxMarkCenter_CheckedChanged);
            // 
            // checkBoxPlateRegion
            // 
            this.checkBoxPlateRegion.Checked = true;
            this.checkBoxPlateRegion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPlateRegion.Location = new System.Drawing.Point(20, 24);
            this.checkBoxPlateRegion.Name = "checkBoxPlateRegion";
            this.checkBoxPlateRegion.Size = new System.Drawing.Size(128, 24);
            this.checkBoxPlateRegion.TabIndex = 30;
            this.checkBoxPlateRegion.Text = "  Plate Region";
            this.checkBoxPlateRegion.CheckedChanged += new System.EventHandler(this.checkBoxPlateRegion_CheckedChanged);
            // 
            // comboBoxDraw
            // 
            this.comboBoxDraw.Items.AddRange(new object[] {
            "margin",
            "fill"});
            this.comboBoxDraw.Location = new System.Drawing.Point(164, 104);
            this.comboBoxDraw.Name = "comboBoxDraw";
            this.comboBoxDraw.Size = new System.Drawing.Size(128, 21);
            this.comboBoxDraw.TabIndex = 29;
            this.comboBoxDraw.Text = "margin";
            this.comboBoxDraw.SelectedIndexChanged += new System.EventHandler(this.comboBoxDraw_SelectedIndexChanged);
            // 
            // comboBoxCoordSys
            // 
            this.comboBoxCoordSys.Items.AddRange(new object[] {
            "green",
            "red",
            "blue",
            "black",
            "white",
            "yellow",
            "magenta",
            "cyan",
            "gray"});
            this.comboBoxCoordSys.Location = new System.Drawing.Point(164, 72);
            this.comboBoxCoordSys.Name = "comboBoxCoordSys";
            this.comboBoxCoordSys.Size = new System.Drawing.Size(128, 21);
            this.comboBoxCoordSys.TabIndex = 28;
            this.comboBoxCoordSys.Text = "yellow";
            this.comboBoxCoordSys.SelectedIndexChanged += new System.EventHandler(this.comboBoxCoordSys_SelectedIndexChanged);
            // 
            // comboBoxMarkCenters
            // 
            this.comboBoxMarkCenters.Items.AddRange(new object[] {
            "green",
            "red",
            "blue",
            "black",
            "white",
            "yellow",
            "magenta",
            "cyan",
            "gray"});
            this.comboBoxMarkCenters.Location = new System.Drawing.Point(164, 48);
            this.comboBoxMarkCenters.Name = "comboBoxMarkCenters";
            this.comboBoxMarkCenters.Size = new System.Drawing.Size(128, 21);
            this.comboBoxMarkCenters.TabIndex = 27;
            this.comboBoxMarkCenters.Text = "cyan";
            this.comboBoxMarkCenters.SelectedIndexChanged += new System.EventHandler(this.comboBoxMarkCenters_SelectedIndexChanged);
            // 
            // comboBoxPlateRegion
            // 
            this.comboBoxPlateRegion.Items.AddRange(new object[] {
            "green",
            "red",
            "blue",
            "black",
            "white",
            "yellow",
            "magenta",
            "cyan",
            "gray"});
            this.comboBoxPlateRegion.Location = new System.Drawing.Point(164, 24);
            this.comboBoxPlateRegion.Name = "comboBoxPlateRegion";
            this.comboBoxPlateRegion.Size = new System.Drawing.Size(128, 21);
            this.comboBoxPlateRegion.TabIndex = 26;
            this.comboBoxPlateRegion.Text = "green";
            this.comboBoxPlateRegion.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlateRegion_SelectedIndexChanged);
            // 
            // openFileDialogImg
            // 
            this.openFileDialogImg.Filter = "png (*.png)|*.png|tiff (*.tif)|*.tif|jpeg (*.jpg)| *.jpg|all files (*.*)|*.*";
            this.openFileDialogImg.FilterIndex = 4;
            this.openFileDialogImg.Multiselect = true;
            // 
            // openFileDialogDescr
            // 
            this.openFileDialogDescr.Filter = " Plate Description (*.descr)|*.descr| all files (*.*)|*.*";
            // 
            // openFileDialogImportParams
            // 
            this.openFileDialogImportParams.Filter = "camera parameters (*.cal)|*.cal|camera parameters (*.dat)|*dat| all files (*.*)|*" +
                ".*";
            this.openFileDialogImportParams.FilterIndex = 3;
            // 
            // StatusLabel
            // 
            this.StatusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.StatusLabel.Location = new System.Drawing.Point(16, 616);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(456, 24);
            this.StatusLabel.TabIndex = 27;
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // saveParamFileDialog
            // 
            this.saveParamFileDialog.Filter = "camera parameters (*.cal)|*.cal|camera parameters (*.dat)|*dat| all files (*.*)|*" +
                ".*";
            // 
            // CalibrationForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(942, 652);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.viewPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(948, 680);
            this.MinimumSize = new System.Drawing.Size(948, 680);
            this.Name = "CalibrationForm";
            this.Text = "Calibration Assistant";
            this.Load += new System.EventHandler(this.CalibrationForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPageCalib.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.LineScanAddPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MotionZUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MotionYUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MotionXUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FocalLengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SyUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SxUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThicknessUpDown)).EndInit();
            this.tabPageQualityCheck.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WarnlevelUpDown)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDiamTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxDiamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContLTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinContLUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SmoothUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinThreshUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreshDecrTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreshDecrUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitThreshTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InitThreshUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinDiamTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinDiamUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkThreshTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MarkThreshUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSizeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilterSizeUpDown)).EndInit();
            this.tabPageResults.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.AreaScanPolynomPanel.ResumeLayout(false);
            this.KappaPanel.ResumeLayout(false);
            this.LineScanPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upDownLineWidth)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion

		
        [STAThread]
        static void Main() 
        {
            Application.Run(new CalibrationForm());
        }

        /********************************************************************/
        private void CalibrationForm_Load(object sender, System.EventArgs e)
        {
            mView = new HWndCtrl(viewPort);
            mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
            mView.changeGraphicSettings(GraphicsContext.GC_DRAWMODE, comboBoxDraw.Text);
            mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, 1);
            
            mAssistant = new CalibrationAssistant();
            mAssistant.NotifyCalibObserver = new CalibDelegate(UpdateCalibResults);
            Init();
        }

        /*******************************************************************
         * Initialize the GUI front end with the values defined for the 
         * corresponding parameters in the Calibration Assistant 
         * ****************************************************************/
        private void Init()
        {
            currIdx   = 0;
            currLineW = 1;
            locked    = true;
            
            plateRegionDisp = checkBoxPlateRegion.Checked;
            markCentersDisp = checkBoxMarkCenter.Checked;
            coordSystemDisp = checkBoxCoordSys.Checked;
            
            plateRegionColor = comboBoxPlateRegion.Text;
            markCenterColor  = comboBoxMarkCenters.Text;
            coordSystemColor = comboBoxCoordSys.Text;
            
            CamTypComboBox.SelectedIndex = 0;
            SxUpDown.Value               = (decimal)mAssistant.getCellWidth();
            SxUpDown.Increment           = 0.100m;
            SyUpDown.Value               = (decimal)mAssistant.getCellHeight();
            SyUpDown.Increment           = 0.100m;
            FocalLengthUpDown.Value      = (decimal)mAssistant.getFocalLength();
            FocalLengthUpDown.Increment  = 0.100m;
            ThicknessUpDown.Value        = (decimal)(mAssistant.getThickness());
            ThicknessUpDown.Increment    = 0.100m;


            textBoxDescr.Text      =  mAssistant.mDescrFileName;

            FilterSizeUpDown.Value = (int)mAssistant.getFilterSize();
            MarkThreshUpDown.Value = (int)mAssistant.getMarkThresh();
            MinDiamUpDown.Value    = (int)mAssistant.getMinMarkDiam();
            InitThreshUpDown.Value = (int)mAssistant.getInitThresh();
            ThreshDecrUpDown.Value = (int)mAssistant.getThreshDecr();
            MinThreshUpDown.Value  = (int)mAssistant.getMinThresh();
            SmoothUpDown.Value     = (int)(mAssistant.getSmoothing()*100);
            MinContLUpDown.Value   = (int)mAssistant.getMinContLength();
            MaxDiamUpDown.Value    = (int)mAssistant.getMaxMarkDiam();
            WarnlevelUpDown.Value  = (int)mAssistant.getWarnLevel();

            MotionXUpDown.Value     = (decimal)mAssistant.getMotionX();
            MotionXUpDown.Increment = 0.100m;
            MotionYUpDown.Value     = (decimal)mAssistant.getMotionY();
            MotionYUpDown.Increment = 0.100m;
            MotionZUpDown.Value     = (decimal)mAssistant.getMotionZ();
            MotionZUpDown.Increment = 0.100m;

            string imPathValue =
              (string)HSystem.GetSystem("image_dir").TupleSplit(";");
            openFileDialogImg.InitialDirectory = imPathValue + "\\calib";
            string halconPathValue = Environment.GetEnvironmentVariable(
                                     "HALCONROOT");
            openFileDialogDescr.InitialDirectory = halconPathValue + "\\calib";
            // set initial directory to standard user's working directory
            openFileDialogImportParams.InitialDirectory = 
            Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
            // set initial directory to standard user's working directory
            saveParamFileDialog.InitialDirectory =
            Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);

            locked = false;           
        }

        /********************************************************************/
        /*                   Display Parameters                             */
        /********************************************************************/
        private void checkBoxPlateRegion_CheckedChanged(object sender, System.EventArgs e)
        {
            plateRegionDisp = checkBoxPlateRegion.Checked;  
            UpdateView();
        }

        /****************************************************/
        private void checkBoxMarkCenter_CheckedChanged(object sender, System.EventArgs e)
        {
            markCentersDisp = checkBoxMarkCenter.Checked;
            UpdateView();
        }

        /****************************************************/
        private void checkBoxCoordSys_CheckedChanged(object sender, System.EventArgs e)
        {
            coordSystemDisp = checkBoxCoordSys.Checked;
            UpdateView();
        }

        /****************************************************/
        private void comboBoxPlateRegion_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            plateRegionColor = comboBoxPlateRegion.Text;
            UpdateView();
        }

        /****************************************************/
        private void comboBoxMarkCenters_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            markCenterColor = comboBoxMarkCenters.Text;
            UpdateView();
        }

        /****************************************************/
        private void comboBoxCoordSys_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            coordSystemColor = comboBoxCoordSys.Text;
            UpdateView();
        }

        /****************************************************/
        private void comboBoxDraw_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            mView.changeGraphicSettings(GraphicsContext.GC_DRAWMODE, comboBoxDraw.Text);
            UpdateView();
        }

        private void upDownLineWidth_ValueChanged(object sender, System.EventArgs e)
        {
            currLineW = (int)upDownLineWidth.Value;
            mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, currLineW);
            UpdateView();
        }


        /********************************************************************/
        /*                  MouseInteraction Parameters                     */
        /********************************************************************/
        private void buttonMove_CheckedChanged(object sender, System.EventArgs e)
        {
            mView.setViewState(HWndCtrl.MODE_VIEW_MOVE);
        }

        /****************************************************/
        private void buttonZoom_CheckedChanged(object sender, System.EventArgs e)
        {
            mView.setViewState(HWndCtrl.MODE_VIEW_ZOOM);
        }

        /****************************************************/
        private void buttonMagnify_CheckedChanged(object sender, System.EventArgs e)
        {
            mView.setViewState(HWndCtrl.MODE_VIEW_ZOOMWINDOW);
        }

        /****************************************************/
        private void buttonNone_CheckedChanged(object sender, System.EventArgs e)
        {
            mView.setViewState(HWndCtrl.MODE_VIEW_NONE);
        }

        /****************************************************/
        private void buttonReset_Click(object sender, System.EventArgs e)
        {
            mView.resetWindow();
            UpdateView();
        }

        /********************************************************************/
        /********************************************************************/
        /*                       1. Tab                                     */
        /********************************************************************/
        /********************************************************************/
        private void buttonLoad_Click(object sender, System.EventArgs e)
        {
            string [] files;
            ListViewItem item;
            int  count      = 0;
            CalibImage data = null;
            
            int idx = StatusLabel.Text.Length;

            if(openFileDialogImg.ShowDialog() == DialogResult.OK)
            {
                files = openFileDialogImg.FileNames;
                count = files.Length;
                
                
                for(int i=0; i<count; i++)
                {
                    if((data = mAssistant.addImage(files[i]))!= null)
                    {
                        item = new ListViewItem("");
                        item.SubItems.Add(files[i]);
                        item.SubItems.Add(data.getPlateStatus());
                        ListCalibImg.Items.AddRange(new ListViewItem[]{item});
                    }
                }//for
			    
                mAssistant.UpdateSequenceIssues();
            
                buttonCalibrate.Enabled    = (mAssistant.mCanCalib && (mAssistant.mReferenceIndex != -1));
                buttonSetReference.Enabled = true;
                
                if(data!=null)
                {
                    ListCalibImg.Items[currIdx].BackColor = System.Drawing.SystemColors.Window;
                    currIdx = ListCalibImg.Items.Count-1;
                    UpdateCalibResults(CalibrationAssistant.UPDATE_MARKS_POSE);
                    UpdateCalibResults(CalibrationAssistant.UPDATE_QUALITY_TABLE);
                    UpdateCalibResults(CalibrationAssistant.UPDATE_CALIBRATION_RESULTS);  
                }
            }//if
        }

        /****************************************************/
        private void ListCalibImg_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string path = "";
            ListView.SelectedListViewItemCollection items = ListCalibImg.SelectedItems;
            
            foreach ( ListViewItem item in items )
            {
                path = item.SubItems[1].Text;
                if(path!= "")
                {
                    ListCalibImg.Items[currIdx].BackColor = System.Drawing.SystemColors.Window;
                    currIdx = ListCalibImg.Items.IndexOf(item);
                    ListCalibImg.Items[currIdx].BackColor = System.Drawing.SystemColors.Control;                    
                    
                    showLabelErrMessage(); 
                    UpdateView();
                    UpdateQualityIssueTable();                    
                    break;
                }//if
            }//foreach
        }

        /****************************************************/
        private void showLabelErrMessage()
        {
            string message = mAssistant.getCalibDataAt(currIdx).mErrorMessage;
            showLabelErrMessage(message);
        }

        /****************************************************/
        private void showLabelErrMessage(string message)
        {
            int idx  = message.IndexOf(":");
            string tmp;

            if(idx>0)
            {   
                message = "Error" + message.Remove(0,idx);
                
                if(message.Length>90)
                {
                    idx = message.LastIndexOf(" ", 80); 
                    tmp = message.Substring(0, idx) + "\n          " + message.Substring(idx+1);
                    message = tmp;
                }
                StatusLabel.Text = message;
            }
            else
            {
                StatusLabel.Text = "";
            }
        }

        /****************************************************/
        private void buttonDelete_Click(object sender, System.EventArgs e)
        {
            int cIdx = -1;
            int refIdx = -1;
            string path = "";
            
            ListView.SelectedListViewItemCollection items = ListCalibImg.SelectedItems;

            currIdx = 0;
            foreach ( ListViewItem item in items )
            {
                path = item.SubItems[1].Text;
                if(path!= "")
                {
                    refIdx = mAssistant.mReferenceIndex;

                    if((cIdx=ListCalibImg.Items.IndexOf(item)) == refIdx)
                        refIdx = -1; 
                    else if(cIdx < refIdx)
                        refIdx--;
                   
                    mAssistant.setReferenceIdx(refIdx);
                    mAssistant.removeImage(cIdx);
                    buttonCalibrate.Enabled = (mAssistant.mCanCalib && (refIdx!=-1));
                    ListCalibImg.Items.Remove(item);
                    ListQualityCheck.Items.Clear();
                    break;
                }//if
            }//foreach

            if(ListCalibImg.Items.Count==0)
               buttonSetReference.Enabled = false;
            else 
                UpdateView(); 
           
        }
        
        /****************************************************/
        private void buttonDeleteAll_Click(object sender, System.EventArgs e)
        {
            mAssistant.removeImage();
            mView.resetWindow();
            mView.clearList();
            currIdx = 0;

            ListCalibImg.Items.Clear();
            ListQualityCheck.Items.Clear();

            buttonSetReference.Enabled = false;
            buttonCalibrate.Enabled = false;
            mAssistant.setReferenceIdx(-1);
            mAssistant.resetCanCalib();

            UpdateView();
        }

        /****************************************************/
        private void buttonSetReference_Click(object sender, System.EventArgs e)
        {
            int val = mAssistant.mReferenceIndex;

            if( val > -1)
                ListCalibImg.Items[val].SubItems[0].Text = "";
            
            mAssistant.setReferenceIdx(currIdx);
            ListCalibImg.Items[currIdx].SubItems[0].Text = "   *";
            buttonCalibrate.Enabled = mAssistant.mCanCalib;   
        }
        
        /*************************************************************/
        private void buttonCalibrate_Click(object sender, System.EventArgs e)
        {
            StatusLabel.Text = " Calibrating ... ";
            StatusLabel.Refresh();

            mAssistant.applyCalibration();
            tabControl.SelectedTab = tabControl.TabPages[2];
            
        }

        /**************************************************************/
        private void buttonLoadDescrFile_Click(object sender, System.EventArgs e)
        {
            string file;
            string [] val;
            if(openFileDialogDescr.ShowDialog() == DialogResult.OK)
            {
               file = openFileDialogDescr.FileNames[0];
                if(file.EndsWith(".descr"))
                {
                    mAssistant.setDesrcFile(file);
                    val = file.Split(new Char [] {'\\'});
                    file = val[val.Length-1];
                    textBoxDescr.Text = file;
                }
                else 
                {   
                    MessageBox.Show("Fileformat is wrong, it's not a description file!", 
                                    "Calibration Assistant",
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);
                }
            }
        }

        /**************************************************************/
        private void buttonImportParams_Click(object sender, System.EventArgs e)
        {
            string file;
            bool   success;
           

            if(openFileDialogImportParams.ShowDialog() == DialogResult.OK)
            {
                file = openFileDialogImportParams.FileNames[0];
                if(file.EndsWith(".cal") || file.EndsWith(".dat"))
                {
                    success = mAssistant.importCamParams(file);
                    
                    if(success)
                    {
                        locked = true;
                        resetGUICameraSetup();
                        locked = false;
                    }
                }
                else 
                {
                    UpdateCalibResults(CalibrationAssistant.ERR_READING_FILE);
                }
            }
        }

        /**************************************************************/
        private void resetGUICameraSetup()
        {
            int camType, val;
          
            try
            {
                camType = mAssistant.getCameraType();
                if(camType == CalibrationAssistant.CAMERA_TYP_AREA_SCAN_DIV)
                    val = 0;
                else if(camType == CalibrationAssistant.CAMERA_TYP_AREA_SCAN_POLY)
                    val = 1;
                else if(camType == CalibrationAssistant.CAMERA_TYP_LINE_SCAN)
                    val = 2;
                else
                    throw(new ArgumentException());
               

                CamTypComboBox.SelectedIndex = val;
                SxUpDown.Value               =(decimal) mAssistant.getCellWidth();                   
                SyUpDown.Value               =(decimal) mAssistant.getCellHeight();
                FocalLengthUpDown.Value      =(decimal) mAssistant.getFocalLength();
                TelecentricCheckBox.Checked = mAssistant.isTelecentric;

                MotionXUpDown.Value     = (decimal)mAssistant.getMotionX();
                MotionYUpDown.Value     = (decimal)mAssistant.getMotionY();
                MotionZUpDown.Value     = (decimal)mAssistant.getMotionZ();
                        
            }
            catch(ArgumentException)
            {
                locked = false;
            }
        }


        /**************************************************************/
        private void buttonDefaultParams_Click(object sender, System.EventArgs e)
        {
            mAssistant.resetCameraSetup(false);
            locked = true;
            resetGUICameraSetup();
            locked = false;
        }

        /**************************************************************/
        private void numUpDownThickness_ValueChanged(object sender, System.EventArgs e)
        {
            tThickness   = (double)ThicknessUpDown.Value;
            mAssistant.setThickness((double)ThicknessUpDown.Value);  
        }

        /**************************************************************/
        private void comboBoxCamTyp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int val = 0;

            AreaScanPolynomPanel.Visible    = false;
            KappaPanel.Visible              = false;
            LineScanPanel.Visible           = false;
            
            LineScanAddPanel.Visible        = false;

            switch((int)CamTypComboBox.SelectedIndex)
            {
                case 0:
                    val = CalibrationAssistant.CAMERA_TYP_AREA_SCAN_DIV;
                    KappaPanel.Visible          = true;
                    TelecentricCheckBox.Enabled = true;
                    break;
                case 1:
                    val = CalibrationAssistant.CAMERA_TYP_AREA_SCAN_POLY;
                    AreaScanPolynomPanel.Visible = true;
                    TelecentricCheckBox.Enabled  = true;
                    break;
                case 2:
                    val = CalibrationAssistant.CAMERA_TYP_LINE_SCAN;
                    KappaPanel.Visible          = true;
                    LineScanPanel.Visible       = true;
                    LineScanAddPanel.Visible    = true;
                    TelecentricCheckBox.Checked = false;
                    TelecentricCheckBox.Enabled = false;
                    break;
            }
            
            if(!locked)
                mAssistant.setCameraType(val);
            
        }

        /**************************************************************/
        private void numUpDownSx_ValueChanged(object sender, System.EventArgs e)
        {
            tCellWidth = (double)SxUpDown.Value;

            if(!locked)
                mAssistant.setCellWidth((double)SxUpDown.Value);
        }

        /**************************************************************/
        private void numUpDownSy_ValueChanged(object sender, System.EventArgs e)
        {
            tCellHeight = (double)SyUpDown.Value;   

            if(!locked)
                mAssistant.setCellHeight((double)SyUpDown.Value);
        }

        /**************************************************************/
        private void numUpDownFocalLength_ValueChanged(object sender, System.EventArgs e)
        {
            tFocalLength = (double)FocalLengthUpDown.Value;

            if(!locked)
                mAssistant.setFocalLength((double)FocalLengthUpDown.Value);
        }

        /**************************************************************/
        private void checkBoxTelecentric_CheckedChanged(object sender, System.EventArgs e)
        {
            bool check = TelecentricCheckBox.Checked;
            
            if(check)
                FocalLengthUpDown.Enabled = false;
            else
                FocalLengthUpDown.Enabled = true;

            if(!locked)
                mAssistant.setIsTelecentric(check);
        }

        /**************************************************************/
        private void MotionXUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            tMotionX = (double)MotionXUpDown.Value;

            if(!locked)
                mAssistant.setMotionX((double)MotionXUpDown.Value);
        }

        /**************************************************************/
        private void MotionYUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            tMotionY = (double)MotionYUpDown.Value;

            if(!locked)
                mAssistant.setMotionY((double)MotionYUpDown.Value);
        }
        
        /**************************************************************/
        private void MotionZUpDown_ValueChanged(object sender, System.EventArgs e)
        {
            tMotionZ = (double)MotionZUpDown.Value;

            if(!locked)
                mAssistant.setMotionZ((double)MotionZUpDown.Value);
        }

        /*************************************************************************/
        /*************************************************************************/
        /*                            2. Tab                                     */
        /*************************************************************************/
        /*************************************************************************/
        private void comboBoxImgTests_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(!locked)
                mAssistant.setImageTests(ImgTestsComboBox.SelectedIndex);
        }

        /**************************************************************/
        private void comboBoxSeqTests_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if(!locked)
                mAssistant.setSequenceTests(SeqTestsComboBox.SelectedIndex);
        }

        /**************************************************************/
        private void numUpDownWarnlevel_ValueChanged(object sender, System.EventArgs e)
        {
            if(!locked)
                mAssistant.setWarnLevel((int)WarnlevelUpDown.Value);
        }

        /**************************************************************/
        /********************   FillSize    ***************************/
        private void numUpDownFilterSize_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)FilterSizeUpDown.Value;
            FilterSizeTrackBar.Value = val;
           
            if(!locked)
                setFilterSize(val);
         
        }
        private void trackBarFilterSize_Scroll(object sender, System.EventArgs e)
        {
            FilterSizeUpDown.Value = FilterSizeTrackBar.Value;
            FilterSizeUpDown.Refresh();
        }
        private void buttonRFilterSize_Click(object sender, System.EventArgs e)
        {
            FilterSizeUpDown.Value          = mAssistant.resetFilterSize;     
            FilterSizeResetButton.ForeColor = System.Drawing.Color.Gray;
        }                
        private void setFilterSize(int val)
        {
            FilterSizeResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setFilterSize((double)val);
        }

        /**************************************************************/
        /********************** Mark Threshold ************************/
        private void numUpDownMarkThresh_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)MarkThreshUpDown.Value;
            MarkThreshTrackBar.Value = val;
            
            if(!locked)
                setMarkThresh(val);
            
        }
        private void trackBarMarkThresh_Scroll(object sender, System.EventArgs e)
        {
            MarkThreshUpDown.Value = (int)MarkThreshTrackBar.Value;
            MarkThreshUpDown.Refresh();
        }
        private void buttonRMarkThresh_Click(object sender, System.EventArgs e)
        {
            MarkThreshUpDown.Value          = mAssistant.resetMarkThresh;
            MarkThreshResetButton.ForeColor = System.Drawing.Color.Gray;
            
        }
        private void setMarkThresh(int val)
        {
            MarkThreshResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setMarkThresh((double)val);
        }

        /**************************************************************/
        /***********************  Min Diameter  ***********************/
        private void numUpDownMinDiam_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)MinDiamUpDown.Value;
            MinDiamTrackBar.Value = val;
            
            if(!locked)
                setMinDiam(val);
            
        }
        private void trackBarMinDiam_Scroll(object sender, System.EventArgs e)
        {
            MinDiamUpDown.Value = MinDiamTrackBar.Value;
            MinDiamUpDown.Refresh();
        }
        private void buttonRMinDiam_Click(object sender, System.EventArgs e)
        {
            MinDiamUpDown.Value          = mAssistant.resetMinMarkDiam;
            MinDiamResetButton.ForeColor = System.Drawing.Color.Gray;
        }
        private void setMinDiam(int val)
        {
            MinDiamResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setMinMarkDiam((double)val);
        }        

        /**************************************************************/
        /********************  Init Threshold  ************************/        
        private void numUpDownInitThresh_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)InitThreshUpDown.Value;
            InitThreshTrackBar.Value = val;
            
            if(!locked)
                setInitThresh(val);
            
        }
        private void trackBarInitThresh_Scroll(object sender, System.EventArgs e)
        {
            InitThreshUpDown.Value = InitThreshTrackBar.Value;
            InitThreshUpDown.Refresh();
        }
        private void buttonRInitThresh_Click(object sender, System.EventArgs e)
        {
            InitThreshUpDown.Value          = mAssistant.resetInitThresh;        
            InitThreshResetButton.ForeColor = System.Drawing.Color.Gray;
        }
        private void setInitThresh(int val)
        {
            InitThreshResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setInitThresh((double)val);
        }

        /**************************************************************/
        /******************  Threshold Decrement **********************/        
        private void numUpDownThreshDecr_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)ThreshDecrUpDown.Value;
            ThreshDecrTrackBar.Value = val;
            
            if(!locked)
                setThreshDecr(val);
            
        }
        private void trackBarThreshDecr_Scroll(object sender, System.EventArgs e)
        {
            ThreshDecrUpDown.Value = (int)ThreshDecrTrackBar.Value;
            ThreshDecrUpDown.Refresh();
        }
        private void buttonRThreshDecr_Click(object sender, System.EventArgs e)
        {
            ThreshDecrUpDown.Value          = mAssistant.resetThreshDecr;
            ThreshDecrResetButton.ForeColor = System.Drawing.Color.Gray;
            
        }
        private void setThreshDecr(int val)
        {
            ThreshDecrResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setThreshDecr((double)val);
        }    
    
        /**************************************************************/
        /******************  Minimum Threshold ************************/        
        private void numUpDownMinThresh_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)MinThreshUpDown.Value;
            MinThreshTrackBar.Value = val;

            if(!locked)
                setMinThresh(val);
            
        }
        private void trackBarMinThresh_Scroll(object sender, System.EventArgs e)
        {
            MinThreshUpDown.Value = MinThreshTrackBar.Value;
            MinThreshUpDown.Refresh();
        }
        private void buttonRMinThresh_Click(object sender, System.EventArgs e)
        {
            MinThreshUpDown.Value          = mAssistant.resetMinThresh;
            MinThreshResetButton.ForeColor = System.Drawing.Color.Gray;
            
        }
        private void setMinThresh(int val)
        {
            MinThreshResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setMinThresh((double)val);
        }

        /**************************************************************/
        /********************  Smoothing  *****************************/        
        private void numUpDownSmooth_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)SmoothUpDown.Value;
            SmoothTrackBar.Value = val;
            
            if(!locked)
                setSmoothing(val);
            
        }
        private void trackBarSmooth_Scroll(object sender, System.EventArgs e)
        {
            SmoothUpDown.Value = SmoothTrackBar.Value;
            SmoothUpDown.Refresh();
        }
        private void buttonRSmooting_Click(object sender, System.EventArgs e)
        {
            SmoothUpDown.Value            = (int)(mAssistant.resetSmoothing*100);
            SmootingResetButton.ForeColor = System.Drawing.Color.Gray;
            
        }
        private void setSmoothing(int val)
        {
            SmootingResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setSmoothing((double)val/100.0);
        }

        /**************************************************************/
        /*****************  Min Contour Length ************************/        
        private void numUpDownMinContL_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)MinContLUpDown.Value;
            MinContLTrackBar.Value = val;
            
            if(!locked)
                setMinContLength(val);
            
        }
        private void trackBarMinContL_Scroll(object sender, System.EventArgs e)
        {
            MinContLUpDown.Value = (int)MinContLTrackBar.Value;
            MinContLUpDown.Refresh();
        }
        private void buttonRMinContL_Click(object sender, System.EventArgs e)
        {
            MinContLUpDown.Value          = mAssistant.resetMinContL;
            MinContLResetButton.ForeColor = System.Drawing.Color.Gray;

        }
        private void setMinContLength(int val)
        {
            MinContLResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setMinContLength((double)val);
        }

        /**************************************************************/
        /********************  Max Diameter ***************************/        
        private void numUpDownMaxDiam_ValueChanged(object sender, System.EventArgs e)
        {
            int val = (int)MaxDiamUpDown.Value;
            MaxDiamTrackBar.Value = val;
            
            if(!locked)
                setMaxDiam(val);
        }
        private void trackBarMaxDiam_Scroll(object sender, System.EventArgs e)
        {
            MaxDiamUpDown.Value = MaxDiamTrackBar.Value;
            MaxDiamUpDown.Refresh();
        }
        private void buttonRMaxDiam_Click(object sender, System.EventArgs e)
        {
            MaxDiamUpDown.Value          = mAssistant.resetMaxMarkDiam;
            MaxDiamResetButton.ForeColor = System.Drawing.Color.Gray;
        }
        private void setMaxDiam(int val)
        {
            MaxDiamResetButton.ForeColor = System.Drawing.Color.Black;
            mAssistant.setMaxMarkDiam((double)val);
        }

        /*************************************************************************/
        /*************************************************************************/
        /*                            3. Tab                                     */
        /*************************************************************************/
        /*************************************************************************/
        private void checkBoxOrigImgCoord_CheckedChanged(object sender, System.EventArgs e)
        {
            mAssistant.setAtImgCoord(checkBoxOrigImgCoord.Checked);
            UpdateView();
        }


        private void buttonSaveCamParams_Click(object sender, System.EventArgs e)
        {
            string files;

            if(saveParamFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = saveParamFileDialog.FileName;

                if(!files.EndsWith(".cal") && !files.EndsWith(".CAL"))
                    files += ".cal";
                
                if(mAssistant.mCalibValid)
                    mAssistant.saveCamParams(files);
            }
        }

        /*************************************************************************/
        /*************************************************************************/
        private void buttonSaveCamPose_Click(object sender, System.EventArgs e)
        {

            string files;

            if(saveParamFileDialog.ShowDialog() == DialogResult.OK)
            {
                files = saveParamFileDialog.FileName;

                if(!files.EndsWith(".dat") && !files.EndsWith(".DAT"))
                    files += ".dat";

                if(mAssistant.mCalibValid)
                    mAssistant.saveCamPose(files);
            }
        }
        
        /*************************************************************************/
        private void buttonRefImg_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateView();
        }

        /*************************************************************************/
        private void radioSimulatedImg_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateView();
        }

       

        /********************************************************************/
        /*             Update methods invoked by delegates                  */
        /********************************************************************/

        /// <summary>
        /// This update method is invoked for all changes occurring in the 
        /// CalibrationAssistant that need to be forwarded to the GUI. The
        /// referring delegate invokes an update for changes in the model
        /// data, which leads to an update in the graphics view. Also, errors
        /// that occur during IO functions and for single calibration steps
        /// are mapped here. 
        /// </summary>
        /// <param name="mode">
        /// Constant of the class CalibrationAssistant, which starts with 
        /// UPDATE_* or ERR_* and which describes an update of the model
        /// data or an HALCON error, respectively.
        /// </param>
        public void UpdateCalibResults(int mode)
        {
            switch(mode)
            {
                case CalibrationAssistant.UPDATE_MARKS_POSE:
                    UpdateView();
                   break;
                case CalibrationAssistant.UPDATE_QUALITY_TABLE:
                    UpdateQualityIssueTable();
                    break;
                case CalibrationAssistant.UPDATE_CALTAB_STATUS:
                    UpdateImageStatus();
                    break;
                case CalibrationAssistant.UPDATE_CALIBRATION_RESULTS:
                    UpdateResultTab(mAssistant.mCalibValid);
                    StatusLabel.Text = " ";
                    break;
                case CalibrationAssistant.ERR_READING_FILE:
                    MessageBox.Show("Problem occured while reading file! \n" + mAssistant.mErrorMessage, 
                                    "Calibration Assistant",
                                     MessageBoxButtons.OK, 
                                     MessageBoxIcon.Information);
                    break;
                case CalibrationAssistant.ERR_QUALITY_ISSUES:
                    MessageBox.Show("Error occured while testing for quality issues \n" + mAssistant.mErrorMessage, 
                                    "Calibration Assistant",
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);
                    ListQualityCheck.Items.Clear();
                    break;
                case CalibrationAssistant.ERR_IN_CALIBRATION:
                    MessageBox.Show("Problem occured while calibrating",
                                    "Calibration Assistant",
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);
                    UpdateResultTab(mAssistant.mCalibValid);
                    showLabelErrMessage(mAssistant.mErrorMessage);
                    break;
                case CalibrationAssistant.ERR_REFINDEX_INVALID:
                    MessageBox.Show("Problem occured: \n" + 
                                    "Please check, whether your reference index is valid", 
                                    "Calibration Assistant",
                                    MessageBoxButtons.OK, 
                                    MessageBoxIcon.Information);
                    break;
                case CalibrationAssistant.ERR_WRITE_CALIB_RESULTS:
                    MessageBox.Show("Problem occured while ! \n" + mAssistant.mErrorMessage, 
                        "Calibration Assistant",
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                    break;
                default:
                    break;
            }
        }
                

        /// <summary> 
        /// Update the graphical window, by adding all objects of either
        /// the training data set or the test data set, including the 
        /// reference world coordinate system and the calibrated world
        /// coordinate system.
        /// </summary>
        public void UpdateView()
        {
            HImage img = null;
            CalibImage data = mAssistant.getCalibDataAt(currIdx);
            

            if(data==null)
            {
                mView.clearList();
            }
            else
            {   
                if(tabControl.SelectedIndex==2 && mAssistant.mCalibValid)
                {
                    if(buttonRefImg.Checked)
                        img = mAssistant.getRefImage();
                    else 
                        img = (HImage)mAssistant.getSimulatedImage();

                   
                    mView.addIconicVar(img);   
                    if(coordSystemDisp && mAssistant.getReferenceWCS().IsInitialized())
                    {
                        mView.changeGraphicSettings(GraphicsContext.GC_COLOR, coordSystemColor);
                        mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, currLineW+1);
                        mView.addIconicVar(mAssistant.getReferenceWCS());
                    }
                }
                else
                {
                  
                    mView.addIconicVar(data.getImage());   
                    mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, currLineW);             

                    if(plateRegionDisp && data.getCaltabRegion().IsInitialized())
                    {
                        mView.changeGraphicSettings(GraphicsContext.GC_COLOR, plateRegionColor);
                        mView.addIconicVar(data.getCaltabRegion());
                    }

                    if(markCentersDisp && data.getMarkCenters().IsInitialized())
                    {
                        mView.changeGraphicSettings(GraphicsContext.GC_COLOR, markCenterColor);
                        mView.addIconicVar(data.getMarkCenters());
                    }
                    if(coordSystemDisp && data.getEstimatedWCS().IsInitialized())
                    {   
                        mView.changeGraphicSettings(GraphicsContext.GC_COLOR, coordSystemColor);
                        mView.changeGraphicSettings(GraphicsContext.GC_LINEWIDTH, currLineW+1);
                        mView.addIconicVar(data.getEstimatedWCS());
                        
                    }
                }
                
            }
            mView.repaint();
        }

        /// <summary>
        /// If the quality issues are recalculated for all calibration 
        /// images, the information table depicting the quality
        /// measure for each image must be updated. 
        /// </summary>
        public void UpdateQualityIssueTable()
        {
            ListViewItem item;
            QualityIssue issue;
            int count;   
            string text = "";
            ArrayList qList;

            qList = (mAssistant.getCalibDataAt(currIdx)).getQualityIssueList();
            count = qList.Count;
            ListQualityCheck.Items.Clear();

            for(int i=0; i<count; i++)
            {
                issue = (QualityIssue)qList[i];    
                item = new ListViewItem("Image");
                
                //Images
                switch(issue.getIssueType())
                {
                    case CalibrationAssistant.QUALITY_ISSUE_FAILURE:
                        text = "Quality assessment failed";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_IMG_CALTAB_SIZE:
                        text = "Plate in image is too small";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_IMG_CONTRAST:
                        text = "Contrast is too low";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_IMG_EXPOSURE:
                        text = "Plate is too overexposed";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_IMG_FOCUS:
                        text = "Marks on plate are out of focus";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_IMG_HOMOGENEITY:
                        text = "Illumination is inhomogeneous";
                        break;
                }

                item.SubItems.Add(text);
                item.SubItems.Add(((int)(issue.getScore()*100.0))+" %");
                ListQualityCheck.Items.AddRange(new ListViewItem[]{item});
            }//for
            

            qList = mAssistant.mSeqQualityList;
            count = qList.Count;
        
            for(int i=0; i<count; i++)
            {
                issue = (QualityIssue)qList[i];    
                item = new ListViewItem("Sequence");                

                //Sequences
                switch(issue.getIssueType())
                {
                    case CalibrationAssistant.QUALITY_ISSUE_FAILURE:
                        text = "Quality assessment failed";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_SEQ_ALL_OVER:
                        text = "Quality issues detected for some images";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_SEQ_CALTAB_TILT:
                        text = "Tilt angles are not covered by sequence";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_SEQ_MARKS_DISTR:
                        text = "Field of view is not covered by plate images";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_SEQ_NUMBER:
                        text = "Number of images is too low";
                        break;
                    case CalibrationAssistant.QUALITY_ISSUE_SEQ_ERROR:
                        text = "Mark extraction failed for some images";
                        break;
                    default:
                        text = "unknown issue";
                        break;
                }

                item.SubItems.Add(text);
                item.SubItems.Add(((int)(issue.getScore()*100.0))+" %");
                ListQualityCheck.Items.AddRange(new ListViewItem[]{item});
            }//for
            
        }//end of method


        /// <summary>
        /// For each change in the calibration parameter set the basic parts,
        /// like finding the calibration plate and the marks, need to be 
        /// recalculated.
        /// The success or failure for detecting each of these basic parts
        /// are described for each calibration image by one of the following 
        /// status messages:
        /// ["Plate not found", "Marks not found", 
        /// "Quality issues detected", "Ok"]
        /// </summary>
        public void UpdateImageStatus()
        {
            ListViewItem item;
            int count = ListCalibImg.Items.Count;   
            
            for(int i=0; i<count; i++)
            {
                item = ListCalibImg.Items[i];
                item.SubItems[2].Text = ((CalibImage)mAssistant.getCalibDataAt(i)).getPlateStatus();
            }

            showLabelErrMessage();
            buttonCalibrate.Enabled = (mAssistant.mCanCalib && (mAssistant.mReferenceIndex != -1));
        }


        /// <summary>Displays the calibration results</summary>
        /// <param name="CalibSuccess"> 
        /// Depicts success or failure of the calibration process
        /// </param>
        public void UpdateResultTab(bool CalibSuccess)
        {
            HTuple campar, reference;
            HTuple from = new HTuple(7*mAssistant.mReferenceIndex);
            HTuple to   = new HTuple(7*mAssistant.mReferenceIndex + 6);

            if(CalibSuccess)
            {
                int offset = 0;
                
                mAssistant.getCalibrationResult(out campar, out reference);

                StatusCalibLabel.Text = "Calibration successful";
                ErrorLabel.Text       = mAssistant.mErrorMean.ToString("f5");
                FocalLResultLabel.Text = (campar[0].D*1000.0).ToString("f4");

                if(mAssistant.getCameraType() == CalibrationAssistant.CAMERA_TYP_AREA_SCAN_POLY)
                {
                    offset = 4;
                    K1Label.Text       = campar[1].D.ToString("f2");
                    K2Label.Text       = campar[2].D.ToString("e10");
                    K3Label.Text       = campar[3].D.ToString("e10");
                    P1Label.Text       = campar[4].D.ToString("f6");
                    P2Label.Text       = campar[5].D.ToString("f6");   
                }
                else
                {
                    KappaResultLabel.Text  = campar[1].D.ToString("f2");
                }

                SxResultLabel.Text     = (campar[2+offset].D*1000000.0).ToString("f3");
                SyResultLabel.Text     = (campar[3+offset].D*1000000.0).ToString("f3");
                CxResultLabel.Text     =  campar[4+offset].D.ToString("f3");
                CyResultLabel.Text     =  campar[5+offset].D.ToString("f3");
                ImgWResultLabel.Text   =  campar[6+offset].I + "";
                ImgHResultLabel.Text   =  campar[7+offset].I + "";

                if(campar.Length == 11) 
                {
                    VxResultLabel.Text = (campar[8].D*1000000.0).ToString("f3");
                    VyResultLabel.Text = (campar[9].D*1000000.0).ToString("f3");
                    VzResultLabel.Text = (campar[10].D*1000000.0).ToString("f3");
                }
                
                
                if(reference.Length >=6)
                {
                    CamPoseXLabel.Text = (reference[0].D*1000).ToString("f3");
                    CamPoseYLabel.Text = (reference[1].D*1000).ToString("f3");
                    CamPoseZLabel.Text = (reference[2].D*1000).ToString("f3");
                    CamAlphaLabel.Text = (reference[3].D).ToString("f3");
                    CamBetaLabel.Text  = (reference[4].D).ToString("f3");
                    CamGammaLabel.Text = (reference[5].D).ToString("f3");
                }
            }
            else //leave all fields empty
            {
                StatusCalibLabel.Text  = "No calibration data available";
                ErrorLabel.Text        = "";
                SxResultLabel.Text     = "";
                SyResultLabel.Text     = "";
                FocalLResultLabel.Text = "";
                KappaResultLabel.Text  = "";
                CxResultLabel.Text     = "";
                CyResultLabel.Text     = "";
                ImgWResultLabel.Text   = "";
                ImgHResultLabel.Text   = "";
                VxResultLabel.Text     = "";
                VyResultLabel.Text     = "";
                VzResultLabel.Text     = "";
                CamPoseXLabel.Text     = "";
                CamPoseYLabel.Text     = "";
                CamPoseZLabel.Text     = "";
                CamAlphaLabel.Text     = "";
                CamBetaLabel.Text      = "";
                CamGammaLabel.Text     = "";
                K1Label.Text           = ""; 
                K2Label.Text           = "";
                K3Label.Text           = "";
                P1Label.Text           = "";
                P2Label.Text           = "";
            }
        }


        /**************************************************************/
        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateView();   
        }


        /**************************************************************/
        /*********  Event on leave - for not to miss on changes *******/
        /**************************************************************/
       private void FocalLengthUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)FocalLengthUpDown.Value != tFocalLength)
                mAssistant.setFocalLength((double)FocalLengthUpDown.Value);
        }

        private void ThicknessUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)ThicknessUpDown.Value != tThickness)
                mAssistant.setThickness((double)ThicknessUpDown.Value);
        }

        private void SxUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)SxUpDown.Value != tCellWidth)
                mAssistant.setCellWidth((double)SxUpDown.Value);
        }

        private void SyUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)SyUpDown.Value != tCellHeight)
                mAssistant.setCellHeight((double)SyUpDown.Value);
        }

        private void MotionXUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)MotionXUpDown.Value != tMotionX)
                mAssistant.setMotionX((double)MotionXUpDown.Value);
        }

        private void MotionYUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)MotionYUpDown.Value != tMotionY)
                mAssistant.setMotionY((double)MotionYUpDown.Value);
        }

        private void MotionZUpDown_Leave(object sender, System.EventArgs e)
        {
            if ((double)MotionZUpDown.Value != tMotionZ)
                mAssistant.setMotionZ((double)MotionZUpDown.Value);
        }

	}//end of class
}//end of namespace

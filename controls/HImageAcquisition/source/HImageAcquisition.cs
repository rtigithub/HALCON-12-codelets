using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;
using HalconDotNet;

namespace HImageAcquisition
{
    public delegate void OnGrabbedImageHandler(object sender, HImage image);
    public delegate void OnGrabbedFailureHandler(Object sender, EventArgs args);
    public delegate void OnParameterFailureHandler(Object sender, 
                                                   String parameterName, 
                                                   HTuple parameterValue);
    public delegate void OnParameterChangeHandler(Object sender, 
                                                   String parameterName, 
                                                   HTuple parameterValue);
  
    /// <summary>
    /// The new Codelet class HImageAcquisition is a .NET component 
    /// for easier intergration of an image acquisition functionality 
    /// into you .NET application. 
    /// This component can be integrated in the toolbox of Visual Studio and
    /// then you can add it to your application, by pulling it on the
    /// form. If the component is successfully added, one symbol is
    /// appearing in the Component Tray of the current form. 
    /// For configuration of the component double click the symbol
    /// in the component tray and then you can configure the properties
    /// in the property browser of Visual Studio.
    /// Parameters like HALCON image acquisition interface name (IAName) 
    /// and type of used camera (CameraType) should be set up
    /// in the designer mode in propety browser. 
    /// Further general parameter, known also from the operator
    /// open_framegrabber, can be set up in the source code before
    /// connection to the device.
    /// The class HImageAcquistion offers the function for acquiring
    /// the single images (GrabSingleImage) and for continuously image 
    /// acquisition in the external thread (StartGrabThread,StopGrabThread). 
    /// If the new image is acquired the acquisition thread notifies 
    /// the main thread by firing the OnGrabbedImage event.
    /// 
    /// If you work with multiple cameras, you have to place one
    /// HImageAcquisition component for each connection.
    /// 
    /// </summary>
    /// 
    [DefaultEvent("OnGrabbedImage")]
    [DefaultProperty("IAName")]
    public partial class HImageAcquisition : Component
    {
        private HImage img;
        public event OnGrabbedImageHandler OnGrabbedImage;
        public event OnParameterChangeHandler OnParameterChange;


        /*********************************************************************
         * Definition of private class members
         *********************************************************************/
        #region Definition of private class members



        private readonly string[] iaGeneralParamNames =
        { "horizontalResolution","verticalResolution",
          "imageWidth","imageHeight","startRow","startColumn",
          "field","bitsPerChannel","colorSpace","generic",
          "externalTrigger","cameraType", "device","port",
          "lineIn"
        };

        public ArrayList ListOfAdjustedDynamicParameters;


        // Image Acquisition parameters //


        // name of image acquisition interface
        // during the deklaration the values will be 
        // set to their default values
        private string iaName                = "File";
        private int    horizontalResolution  = 1;
        private int    verticalResolution    = 1;
        private int    imageWidth            = 0;
        private int    imageHeight           = 0;
        private int    startRow              = 0;
        private int    startColumn           = 0;
        private string field                 = "default";
        private int    bitsPerChannel        = -1;
        private string colorSpace            = "default";
        private double generic               = -1;
        private string externalTrigger       = "default";
        private string cameraType            = "default";
        private string device                = "default";
        private int    port                  = -1;
        private int    lineIn                = -1;

        private HFramegrabber   frameGrabber;
        private static object   locker;
        private bool            grabon;
        private int             imCounter   = 0;
        private HTuple          acqHandle;

        // Time measurement
        private HTuple startGrabTime;
        private HTuple endGrabTime;
        // calculated frame rate based on the last grab
        private double frameRate;
        // Time needed for Requiring of the last image 
        // from the device
        private double timeGrab;


        // Grab image properties
        bool asyncGrabbing = true;


        // image acqusition thread
        Thread grabThread;

        #endregion


        /*********************************************************************
         * Construction and Deconstruction
         *********************************************************************/
        #region Construction and Deconstruction


        /// <summary>
        /// Initializes a new instance of the HImageAcquisition.
        /// For connection to device the name of image acquisition
        /// interface and camera type should be intialized.
        /// </summary>
        public HImageAcquisition()
        {
            InitializeComponent();

            acqHandle = null;
            if (locker == null)
                locker = new object();
 
        }

        /// <summary>
        /// Initializes a new instance of the HImageAcquisition.
        /// For connection to device the name of image acquisition
        /// interface and camera type should be intialized.
        /// </summary>
        public HImageAcquisition(IContainer container)
        {
            container.Add(this);

            InitializeComponent();


            if (locker == null)
                locker = new object();
            acqHandle = null;
        }



        /// <summary>
        /// Initializes a new instance of the HImageAcquisition from
        /// already initialized frame grabber handler.
        /// </summary>
        public HImageAcquisition(HTuple acqHandle)
        {
            InitializeComponent();

            if (locker == null)
                locker = new object();

            if (acqHandle.Length != 1)
                throw new System.ArgumentOutOfRangeException("acqHandle",
                                                             acqHandle,
                 "Only one valid image acquisition handle"+
                 " is accepted as valid parameter!");

             using (frameGrabber = new HFramegrabber(acqHandle[0].IP))
             {
                 this.acqHandle = acqHandle[0];    
             }
        }



        /// <summary>
        /// Releases all resources used by the Component.
        /// Stops the grabbing of images, closes the connection
        /// to the device and release the object, that is responsible
        /// for the device connection and image acquisition.
        /// </summary>
        new public void Dispose()
        {
            try
            {
                if (GrabOn)
                    StopGrabThread();

                CloseDevice();

                if (frameGrabber != null)
                    frameGrabber.Dispose();
                if (img != null)
                    img.Dispose();
            }
            finally
            {
                if (frameGrabber != null)
                    frameGrabber.Dispose();
            }
            base.Dispose();
        }

        #endregion

        /*********************************************************************
         * Image acquistion parameters (Lists Initialization)
         *********************************************************************/

        #region Image acquistion parameters (List Initialization)


        /// <summary>
        /// Reads all current value of the connection parameters
        /// after initialization of IA device.
        /// </summary>
        private void GetCurrentValuesOfGeneralParameters()
        {
            // get values of connection parameters
            if ((frameGrabber != null) && (Connected == true))
            {
               horizontalResolution = frameGrabber.GetFramegrabberParam(
                                      "horizontal_resolution");
               verticalResolution   = frameGrabber.GetFramegrabberParam(
                                      "vertical_resolution");
               imageWidth           = frameGrabber.GetFramegrabberParam(
                                      "image_width");
               imageHeight          = frameGrabber.GetFramegrabberParam(
                                      "image_height");
               startRow             = frameGrabber.GetFramegrabberParam(
                                      "start_row");
               startColumn          = frameGrabber.GetFramegrabberParam(
                                      "start_column");
               field                = frameGrabber.GetFramegrabberParam(
                                      "field");
               bitsPerChannel       = frameGrabber.GetFramegrabberParam(
                                      "bits_per_channel");
               colorSpace           = frameGrabber.GetFramegrabberParam(
                                      "color_space");
               generic              = frameGrabber.GetFramegrabberParam(
                                      "generic");
               externalTrigger      = frameGrabber.GetFramegrabberParam(
                                      "external_trigger");
               device               = frameGrabber.GetFramegrabberParam(
                                      "device");
               port                 = frameGrabber.GetFramegrabberParam(
                                      "port");
               lineIn               = frameGrabber.GetFramegrabberParam(
                                      "line_in");                
            }

        }



        /// <summary>
        ///  Clear list of recorded adjustments of IA interface
        /// </summary>
        public void ResetListOfAdjustedDynamicParameters()
        {
            if (ListOfAdjustedDynamicParameters != null)
                ListOfAdjustedDynamicParameters.Clear();
        }


        #endregion

        #region Properties definition
        /*********************************************************************
         * Properties definition
         *********************************************************************/
        /// <summary>
        /// Gets or sets the state of the device if the
        /// images has to be grabbed or not
        /// </summary>
        private bool GrabOn
        {
            get
            {
                lock (locker)
                    return grabon;
            }
            set
            {
                lock (locker)
                    grabon = value;
            }
        }


        [Description("Time needed for requiring the last image" + 
                     "from the device")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double TimeGrab
        {
            get
            {
                return timeGrab;
            }
        }


        [Description("Contains the frame rate of image acquisition.")]        
        public double FrameRate
        {
            get
            {
                return frameRate;
            }
        }


        [Description("Contains the handle of image acquisition device.")]
        [Browsable(false)]
        public HTuple AcqHandle
        {
           get
            {
                acqHandle = new HTuple(frameGrabber.Handle);
                return acqHandle;
            }
        }

        
        /// <summary>
        /// Sets the name of the HALCON image acquistion interface
        /// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(true)]
        [Description("Contains the name of the HALCON image acquistion" + 
                     "interface.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Visible)]
        [Category("Action")]
        [System.ComponentModel.DefaultValue("File")]
        [TypeConverter(typeof(StringConverter))]
        public String IAName
        {
            get
            {
                return iaName;
            }
            set
            {
  
                if ((value != null) && (value.Length > 0))
                {
                   iaName = value;

                   // ATTENTION: This part of source code is executed 
                   // only to run time
                   if (!this.DesignMode)
                   {
                       HTuple ParametersValue;
                       HTuple ParametersInfo;

                       HTuple infoParameter = new HTuple("defaults");
                       HTuple IANameTuple = new HTuple(IAName);

                       if (!Connected)
                       {
                           try
                           {
                               //initialize default values for IA interface
                               HOperatorSet.InfoFramegrabber(IANameTuple,
                                                             infoParameter,
                                                             out ParametersInfo,
                                                             out ParametersValue);

                               horizontalResolution = ParametersValue[0].I;
                               verticalResolution = ParametersValue[1].I;
                               imageWidth = ParametersValue[2].I;
                               imageHeight = ParametersValue[3].I;
                               startRow = ParametersValue[4].I;
                               startColumn = ParametersValue[5].I;
                               field = ParametersValue[6].S;
                               bitsPerChannel = ParametersValue[7].I;
                               colorSpace = ParametersValue[8].S;
                               generic = ParametersValue[9].D;
                               externalTrigger = ParametersValue[10].S;
                               // cameraType will be adopted from the value 
                               // of property browser                      
                               device = ParametersValue[12].S;
                               port = ParametersValue[13].I;
                               lineIn = ParametersValue[14].I;
                           }
                           catch (HOperatorException exc)
                           {
                               throw exc;
                           }
                       }
                       else
                       {
                           throw new HalconException("It is not possible to " +
                                                      "change the image" +
                                                      "acquisition device," +
                                                      "if you are connected " +
                                                      "to other device.");
                       }
                   }
                }
            }
        }

        /// <summary>
        /// Desired horizontal resolution of image acquisition
        /// interface (absolute value or 1 for full resolution,
        /// 2 for half resolution, or 4 for quarter resolution).
        /// </summary>
        [Browsable(false)]
        [Description("Desired horizontal resolution of image acquisition" +
                     "interface (absolute value or 1 for full resolution," +
                     "2 for half resolution, or 4 for quarter resolution).")]
        [Category("Action")]
        [DefaultValue(-1)]
        public int HorizontalResolution
        {
            get
            {
                return horizontalResolution;
            }
            set
            {
                if (value >= 1)
                    horizontalResolution = value;
            }
        }


        /// <summary>
        /// Desired vertical resolution of image acquisition
        /// interface (absolute value or 1 for full resolution,
        /// 2 for half resolution, or 4 for quarter resolution).
        /// </summary>
        [Browsable(false)]
        [Description("Desired vertical resolution of image acquisition" +
                     "interface (absolute value or 1 for full resolution," +
                     "2 for half resolution, or 4 for quarter resolution).")]
       // [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue(1)]
        public int VerticalResolution
        {
            get
            {
                return verticalResolution;
            }
            set
            {
                if (value >= -1)
                    verticalResolution = value;
            }
        }

        /// <summary>
        /// Width of desired image part (absolute value or 0 for
        /// HorizontalResolution - 2*StartColumn).
        /// </summary>
        [Browsable(false)]
        [Description("Width of desired image part (absolute value or 0 for" +
                     "HorizontalResolution - 2*StartColumn).")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Action")]
        [DefaultValue(0)]
        public int ImageWidth
        {
            get
            {
                return imageWidth;
            }
            set
            {
                if (value >= -1)
                    imageWidth = value;
            }
        }

        /// <summary>
        /// Height of desired image part (absolute value or 0 for
        /// VerticalResolution - 2*StartRow).
        /// </summary>
        [Browsable(false)]
        [Description("Height of desired image part (absolute value or 0 for" +
                     "VerticalResolution - 2*StartRow).")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Action")]
        [DefaultValue(0)]
        public int ImageHeight
        {
            get
            {
                return imageHeight;
            }
            set
            {
                if (value >= -1)
                    imageHeight = value;
            }
        }

        /// <summary>
        /// Line number of upper left corner of desired image part 
        /// (or border height if ImageHeight = 0). 
        /// </summary>
        [Browsable(false)]
        [Description("Line number of upper left corner of desired image part " +
                      "(or border height if ImageHeight = 0).")]
        [Category("Action")]
        [DefaultValue(0)]
        public int StartRow
        {
            get
            {
                return startRow;
            }
            set
            {
                if (value >= -1)
                    startRow = value;
            }
        }


        /// <summary>
        /// Column number of upper left corner of desired image part 
        /// (or border height if ImageWidth = 0). 
        /// </summary>
        [Browsable(false)]
        [Description("Column number of upper left corner of desired image part " +
                     "(or border width if ImageWidth = 0).")]
        [Category("Action")]
        [DefaultValue(0)]
        public int StartColumn
        {
            get
            {
                return startColumn;
            }
            set
            {
                if (value >= -1)
                    startColumn = value;
            }
        }

        /// <summary>
        /// Column number of upper left corner of desired image part 
        /// (or border height if ImageWidth = 0). 
        /// </summary>
        [Browsable(false)]
        [Description("Desired half image (’first’, ’second’, or ’next’)" +
                     "or selection of a full image.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue("default")]
        public string Field
        {
            get
            {
                return field;
            }
            set
            {
                if (value != null)
                {
                    if (value.Length > 0)
                        field = value;
                }
            }
        }

        /// <summary>
        /// Number of transferred bits per pixel and image channel"
        /// (-1: device-specific default value). 
        /// </summary>
        [Browsable(false)]
        [Description("Number of transferred bits per pixel and image channel"+
                     "(-1: device-specific default value).")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue(-1)]
        public int BitsPerChannel
        {
            get
            {
                return bitsPerChannel;
            }
            set
            {
                if (value >= -1)
                    bitsPerChannel = value;
            }
        }

        /// <summary>
        ///Output color format of the grabbed images, typically 
        ///’gray’ or ’raw’ for single-channel or ’rgb’ or ’yuv’ for
        /// three-channel images (’default’: device-specific default value).
        /// </summary>
        [Browsable(false)]
        [Description("Output color format of the grabbed images, typically"+ 
                     "’gray’ or ’raw’ for single-channel or ’rgb’ or ’yuv’"+ 
                     "for three-channel images (’default’: device-specific "+
                     "default value).")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue("default")]
        public string ColorSpace
        {
            get
            {
                return colorSpace;
            }
            set
            {
                if ((value != null) &&
                   (value.Length > 0))
                {
                   colorSpace = value;
                }
                else
                    throw new System.ArgumentException("The value of parameter" + 
                        "ColorSpace property should be a valid string!");
            }
        }


        //private double generic;
        /// <summary>
        /// Generic parameter with device-specific meaning.
        /// </summary>
        [Browsable(false)]
        [Description("Generic parameter with device-specific meaning.")]
        [Category("Action")]
        [DefaultValue(-1.0)]
        public double Generic
        {
             get
            {
                return generic;
            }
            set
            {
                if (frameGrabber == null)
                    generic = value;
                else
                {
                    generic = value;
                }

            }
        }


        //private string externalTrigger;
        /// <summary>
        /// External triggering.
        /// </summary>
        [Browsable(false)]
        [Description("External triggering.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue("default")]
        public string ExternalTrigger
        {
            get
            {
                return externalTrigger;
            }
            set
            {
                if (value != null)
                {
                    if ((value == "default") ||
                        (value == "true") ||
                        (value == "false"))
                    {
                        externalTrigger = value;
                    }
                    else
                    {
                        throw new System.ArgumentException("Invalid string value " +
                            "of ExternalTrigger property!");
                    }
                }
            }
        }


        /// <summary>
        /// Type of used camera (’default’: device-specific default value).
        /// </summary>
        [Browsable(true)]
        [Description("Type of used camera (’default’: " +
                     "device-specific default value).")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Visible)]
        [Category("Action")]
        [DefaultValue("default")]
        public string CameraType
        {
            get
            {
                return cameraType;
            }
            set
            {
                if ((value != null) && 
                    (value.Length > 0))
                {
                    cameraType = value;
                }
                else
                {
                    throw new System.ArgumentException("Invalid string value " +
                        "for parameter CameraType!");
                }

            }
        }

        /// <summary>
        /// Device the image acquisition device is connected
        /// to (’default’: device-specific default value).
        /// </summary>
        [Browsable(false)]
        [Description("Device the image acquisition device is connected" +
                     " to (’default’: device-specific default value).")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        [DefaultValue("default")]
        public string Device
        {
            get
            {
                return device;
            }
            set
            {
                if ((value != null) &&
                    (value.Length > 0))
                        device = value;
                else
                {
                    throw new System.ArgumentException("Invalid string value " +
                        "for parameter Device!");
                }
            }
        }

        /// <summary>
        /// Port the image acquisition device is connected to 
        /// (-1: device-specific default value).
        /// </summary>
        [Browsable(false)]
        [Description("Port the image acquisition device is connected to" + 
                     " (-1: device-specific default value).")]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                if (value >= -1)
                    port = value;
                else
                {
                    throw new System.ArgumentException("Invalid value for " +
                                          "image acquistion parameter Port!");
                }
                
            }
        }

        /// <summary>
        /// Camera input line of multiplexer 
        /// (-1: device-specific default value).
        /// </summary>
        [Browsable(false)]
        [Description("Camera input line of multiplexer "+
                     "(-1: device-specific default value).")]
        //[EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Action")]
        public int LineIn
        {
            get
            {
                return lineIn;
            }
            set
            {
                if (value >= -1)
                    lineIn = value;
                else
                {
                    throw new System.ArgumentException("Invalid value for " +
                                          "image acquistion parameter LineIn!");
                }
            }
        }

        /// <summary>
        /// Returns the number of grabbed images
        /// </summary>
        public int ImageCounter
        {
            get
            {
                return this.imCounter;
            }
        }


        /// <summary>
        /// Returns "true", if the IA-Device is grabbing the images
        /// </summary>
        public bool Grabbing
        {
            get
            {
                return GrabOn;
            }
        }

        /// <summary>
        /// Returns "true", if the IA-device is initialized
        /// and connected with HImageAcquistion object
        /// </summary>
        public bool Connected
        {
            get
            {
                if (frameGrabber == null)
                    return false;

                else
                if (frameGrabber.IsInitialized())
                    return true;
                else
                    return false;
            }
        }

        #endregion


        private void InitIA()
        {
            HTuple sysParam = "clock_mode";
            HTuple sysParamValue = "elapsed_time";           

            HOperatorSet.SetSystem(sysParam,
                                   sysParamValue);
            timeGrab = 0;
            frameRate = 0;
        }

        /*********************************************************************
         * Open and Close Device
         *********************************************************************/

        /// <summary>
        /// Initialize the connection to the image acquisition interface and 
        /// initialization of its image acquisition parameters
        /// </summary>
        public void OpenDevice()
        {
            InitIA();
            img = null;
            imCounter = 0;
            grabThread = null;

            try
            {
                if (this.IAName != "File")
                {
                    OpenFrameGrabber();
                }
                else
                {
                    OpenOfflineIADevice();
                }

                // Initialize acqHandle 
                acqHandle = new HTuple(frameGrabber.Handle);

            }
            catch (HOperatorException exp)
            {
                acqHandle = null;
                throw exp;
            }

            // try to start asynchronous grab
            try
            {
                frameGrabber.GrabImageStart(-1.0);
            }
            catch (HOperatorException exc)
            {
                // Catch the error, if the device doesn't support
                // asynchronous mode: H_ERR_FGASYNC
                if (exc.GetErrorCode() == 5320)
                {
                    asyncGrabbing = false;
                }
                else
                {
                    throw exc;
                }
            }

        }


        /// <summary>
        /// Encapsulates the call of new framegrabber-Object with
        /// default parameters, that are already set up as class
        /// members.
        /// </summary>
        private void OpenFrameGrabber()
        {
            frameGrabber = new HFramegrabber(this.IAName,
                                        this.HorizontalResolution,
                                        this.VerticalResolution,
                                        this.ImageWidth,
                                        this.ImageHeight,
                                        this.StartRow,
                                        this.StartColumn,
                                        this.Field,
                                        this.BitsPerChannel,
                                        this.ColorSpace,
                                        this.Generic,
                                        this.ExternalTrigger,
                                        this.CameraType,
                                        this.Device,
                                        this.Port,
                                        this.LineIn);
        }


        private void OpenOfflineIADevice()
        {
            bool try_halcon_path = false;
            try
            {
                OpenFrameGrabber();

            }
            catch (HOperatorException)
            {
                try_halcon_path = true;    
            }

            if (try_halcon_path)
            {
                String halconImagesPath, imPath;
                halconImagesPath = CreateHalconImagesPath();

                imPath = halconImagesPath + "\\" + this.CameraType;
                imPath = imPath.Replace("\\", "/");

                try
                {
                    // try to create framegrabber with the path
                    // to the HALCON images directory
                    frameGrabber = new HFramegrabber(this.IAName,
                                               this.HorizontalResolution,
                                               this.VerticalResolution,
                                               this.ImageWidth,
                                               this.ImageHeight,
                                               this.StartRow,
                                               this.StartColumn,
                                               this.Field,
                                               this.BitsPerChannel,
                                               this.ColorSpace,
                                               this.Generic,
                                               this.ExternalTrigger,
                                               imPath,
                                               this.Device,
                                               this.Port,
                                               this.LineIn);

                }
                catch (HOperatorException exp)
                {
                    throw exp;
                }
            }

        }


        private String CreateHalconImagesPath()
        {
            String halconImagesPath;
            // Intialize HALCON image path
            try
            {
                halconImagesPath = Environment.GetEnvironmentVariable("HALCONIMAGES");
                if (halconImagesPath == null)
                    throw new Exception();
            }

            catch (Exception)
            {
                halconImagesPath = Environment.GetEnvironmentVariable("HALCONROOT") + @"\images";
            }
            return halconImagesPath;
        }


        /// <summary>
        /// Initialize the connection to the image acquisition interface and 
        /// initialization of its image acquisition parameters
        /// </summary>
        /// <param name="IAName"> Name of the HALCON interface. </param>
        public void OpenDevice(String IAName, String CameraType)
        {

            InitIA();
            
            img = null;
            imCounter = 0;
            grabThread = null;

            this.IAName = IAName;
            this.CameraType = CameraType;
            try
            {
                OpenFrameGrabber();

                // Initialize acqHandle 
                acqHandle = new HTuple(frameGrabber.Handle);
            }
            catch (HOperatorException exc)
            {
                acqHandle = null;
              
                throw exc;
            }

            // try to start asynchronous grab
            try
            {
                frameGrabber.GrabImageStart(-1.0);
            }
            catch (HOperatorException exc)
            {
                // Catch the error, if the device doesn't support
                // asynchronous mode: H_ERR_FGASYNC
                if (exc.GetErrorCode() == 5320)
                {
                    asyncGrabbing = false;                   
                }
                else
                {
                    throw exc;
                }                
            }
        }


        public void OpenDevice(HTuple acqHandle)
        {
            InitIA();
            img = null;
            imCounter = 0;
            grabThread = null;
            using (frameGrabber = new HFramegrabber(acqHandle[0].IP))
            {
                // Initialize acqHandle 
                acqHandle = new HTuple(frameGrabber.Handle);
            }

            // try to start asynchronous grab
            try
            {
                frameGrabber.GrabImageStart(-1.0);
            }
            catch (HOperatorException exc)
            {
                // Catch the error, if the device doesn't support
                // asynchronous mode: H_ERR_FGASYNC
                if (exc.GetErrorCode() == 5320)
                {
                    asyncGrabbing = false;
                }
                else
                {
                    throw exc;
                }
            }
        }


        public void CloseDevice()
        {
            if (frameGrabber != null)
            {
                if (frameGrabber.IsInitialized())
                {
                    // if device performs image acquisition, then stop it
                    if (GrabOn)
                        StopGrabThread();
                    imCounter = 0;
                    // close connection to IA device
                    frameGrabber.Dispose();
                    frameGrabber = null;
                }
            }
        }


        /*********************************************************************
         * Grab images
         *********************************************************************/

        /// <summary>
        /// The method grabs permanet new images from image aquisition device,
        /// until the property GrabOn is set to false or image acquisition 
        /// is stopped. Or the thread is in which the method is executed, will
        /// be discarded.
        /// </summary>
        private void GrabThread()
        {
          GrabOn = true;
          while (GrabOn)
          {
            GrabImage();
          }
        }


        /// <summary>
        /// The method grabs new image and sents an event, if the 
        /// image is grabbed.
        /// </summary>
        public void GrabImage()
        {
          //GrabOn = true;

          if ((GrabOn) && (frameGrabber != null))
          {
              HImage copimg = null;

              HOperatorSet.CountSeconds(out startGrabTime);
              try
              {
                  if (asyncGrabbing)
                      img = frameGrabber.GrabImageAsync(-1);
                  else
                      img = frameGrabber.GrabImage();
                  HOperatorSet.CountSeconds(out endGrabTime);
              }
              catch (HOperatorException exc)
              {
                  HOperatorSet.CountSeconds(out endGrabTime);
                  timeGrab = (endGrabTime.D - startGrabTime.D) * 1000.0;

                  // If failed grabbing of an image,
                  // If Time Out Exception occured, 
                  // then the image acquisition is interrupted
                  if ((exc.GetErrorCode() == 5306) || 
                      (exc.GetErrorCode() == 5322))
                      StopGrabThread();

                  
                  // If the connection to device is lost,then the image
                  // acquisition is interrupted
                  if (exc.GetErrorCode() == 5335)
                  {
                      StopGrabThread();
                      CloseDevice();
                  }
                  throw exc;
              }

              timeGrab = (endGrabTime.D - startGrabTime.D) * 1000.0;

              if (timeGrab > 0)
                  frameRate = 1000.0 / timeGrab;
              else
                  frameRate = 0.0;

              lock (locker)
              {
                  if (img != null)
                  {
                      copimg = img;
                      imCounter = imCounter + 1;
                  }
              }

              if ((OnGrabbedImage != null))
                OnGrabbedImage(this, copimg);
          }         
        }


        /// <summary>
        /// Synchronous grab of an image from the initialized 
        /// image acquisition device and fires an event, if the 
        /// image is grabbed.
        /// </summary>
        public void GrabSingleImage()
        {

            GrabOn = true;
            if ((GrabOn) && (frameGrabber != null))
            {
                try
                {
                    asyncGrabbing = false;
                    GrabImage();
                    GrabOn = false;
                }
                catch(HOperatorException excp)
                {
                    throw excp;
                }
            }
            else
            {
                new HalconException("The image acquisition device is not initialized!");
            }
        }


        /// <summary>
        /// The method creates new thread, if not yet created, for  
        /// image acquisition from camera using Interfaces provided 
        /// by HALCON.
        /// </summary>
        public void StartGrabThread()
        {
          if ((grabThread == null) || (grabThread.IsAlive == false))
          {
            GrabOn = true;
            asyncGrabbing = true;
            grabThread = new Thread((ThreadStart)GrabThread);
            grabThread.IsBackground = true;
            grabThread.Start();
          }
        }


        /// <summary>
        /// Stops the image acquisition and aborts the thread, that 
        /// is responsible for image acquisition.
        /// </summary>
        public void StopGrabThread()
        {
          GrabOn = false;          
          if (grabThread != null)
            grabThread.Abort();
        }



        /*********************************************************************
         * Set image acquisition parameters
         *********************************************************************/

        /// <summary>
        /// 
        /// </summary>        
        public void SetIADynamicParam(string paramName, 
                                      int paramValue)
        {
            HTuple paramNameTuple = new HTuple(paramName);
            HTuple paramValueTuple = new HTuple(paramValue);
            SetIADynamicParamTuple(paramNameTuple, paramValueTuple);
        }


        /// <summary>
        /// 
        /// </summary>        
        public void SetIADynamicParam(string paramName, 
                                      string paramValue)
        {
            HTuple paramNameTuple = new HTuple(paramName);
            HTuple paramValueTuple = new HTuple(paramValue);
            SetIADynamicParamTuple(paramNameTuple, paramValueTuple);
        }


        /// <summary>
        /// 
        /// </summary>        
        public void SetIADynamicParam(string paramName, 
                                      double paramValue)
        {
            HTuple paramNameTuple = new HTuple(paramName);
            HTuple paramValueTuple = new HTuple(paramValue);
            SetIADynamicParamTuple(paramNameTuple, paramValueTuple);
        }


        /// <summary>
        /// 
        /// </summary>        
        public void SetIADynamicParamTuple(HTuple paramName,
                                           HTuple paramValue)
        {
            string parameterName;
            int    indexOfdo_;            

            // Check if the input parameters are valid
            if ((paramName.TupleLength() > 0) &&
                (paramName.TupleLength() == paramValue.TupleLength()))
            {
                // get string value of parameter name
                parameterName = paramName[0].S;
                // get the index of the string "do_"
                indexOfdo_    = parameterName.IndexOf("do_");

                    try
                    {
                        frameGrabber.SetFramegrabberParam(paramName, 
                                                          paramValue);
                        // check if the parameter ist a "do_"-parameter
                        // these parameter cannot be read with the method
                        // get_framegrabber_param()
                        if (indexOfdo_ == -1)
                        {
                            // Acquire the changed value from IA Interface
                            paramValue =
                            frameGrabber.GetFramegrabberParam(paramName);
                        }
 
                        // fire event, to notify the main program,
                        // that the IA parameter has changed
                        if (OnParameterChange != null)
                            OnParameterChange(this, parameterName, 
                                              paramValue);
                    }
                    catch (HOperatorException exc)
                    {
                        throw exc;
                    }                        
                    // add the entry to the list to 
                    // record the order of parameter settings  
                    AddAdjustedParamToHistoryList(parameterName,paramValue);                                    
                }
                else
                {
                    throw new ArgumentException("Invalid name of IA" + 
                                                " parameter!");
                }
             
        }


        /// <summary>
        /// This method adds the new adjusted parameter to the history
        /// list
        /// </summary>        
        private void AddAdjustedParamToHistoryList(string parameterName,
                                                   HTuple paramValue)
        {
            IAParameterEntry adjustedDynamicParam;

            // Add the new parameter with the new value to the list
            adjustedDynamicParam = new IAParameterEntry(
                                                parameterName,
                                                paramValue);
            // if the tuple contains values
            if (adjustedDynamicParam.ParameterValue.TupleLength() > 0)
            {
                // Create the list for the history of adjustment of 
                // IA parameters, if the list of adjusted parameters 
                // is not created yet.
                if (ListOfAdjustedDynamicParameters == null)
                    ListOfAdjustedDynamicParameters = new ArrayList();

                ListOfAdjustedDynamicParameters.Add(adjustedDynamicParam);
            }
            else
                throw new ArgumentException("The parameter value" + 
                                            "contains no values!");
        }



        /// <summary>
        /// Each change of the dynamic IA parameters is recorded as an entry
        /// in the list ListOfAdjustedDynamicParameters.
        /// This list can be used to recover the last state of
        /// the parameters of an IA interface.
        /// ATTENTION: The best way to set the parameters is NOT
        /// during image acquisition! 
        /// 1). Connect to device/Stop IA
        /// 2). Set up parameters
        /// 3). Start IA
        /// </summary>
        public void SetIADynamicParametersFromList(ArrayList ParameterList)
        {

            if ((ParameterList.Capacity > 0) &&
                 (frameGrabber != null))
            {
                foreach (IAParameterEntry entry in ParameterList)
                {
                    try
                    {
                        frameGrabber.SetFramegrabberParam(
                                                      entry.ParameterNameTuple,
                                                      entry.ParameterValue);
                    }
                    catch (HOperatorException exc)
                    {
                        throw exc;
                    }
                }                    
            }// if
        }//end method



  
        /*********************************************************************
         * Get image acquisition parameter value
         *********************************************************************/
        public HTuple GetIAParameterValue(string parameterName)
        {
            HTuple paramValue;
            //TODO: Check if the parameter is not write-only
            if (Connected)
            {
                try
                {
                    paramValue = frameGrabber.GetFramegrabberParam(parameterName);
                }
                catch (HOperatorException exp)
                {
                    throw exp;
                }
            }
            else
                throw new HalconException("The device is not initialized!");
            return paramValue;
        }


        public HTuple GetIAParameterValue(HTuple parameterName)
        {
            HTuple paramValue;

            //TODO: Check if the parameter is not write-only
            if (Connected)
            {
                try
                {
                    paramValue = frameGrabber.GetFramegrabberParam(
                                                            parameterName.S);
                }
                catch (HOperatorException exp)
                {
                    throw exp;
                }

            }
            else
                throw new HalconException("The device is not initialized!");
            return paramValue;
        }
    }
}

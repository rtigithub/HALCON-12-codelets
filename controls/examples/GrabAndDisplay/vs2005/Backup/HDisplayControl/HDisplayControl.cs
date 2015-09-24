using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using HDisplayControl.ViewROI;


/// <summary>
/// The class HDisplayControl is a User Interface .NET Control
/// and is responsible for the visualization of iconic obejcts. The 
/// implementation of this class is based on the HWindowControl class 
/// and the adapted version of HWndCtrl class. 
/// Using the mouse interaction you can move and zoom the visible 
/// image part. If this interaction is available or not is configurable
/// through HDisplayControl properties. 
/// The images can be displayed in two modes. In default mode 
/// the image is zoomed to the window size in correct aspect ratio. 
/// The second mode displays the image in its real size.If the image is larger 
/// than the graphic window, the scroll bars will be displayed so 
/// you can scroll to view the remainder of the image.
/// The class provides also a tool bar for drawing and defining 
/// the region(s) of interest in displayed image. If the interaction 
/// with the tool bar is not desired, then the tool bar can be disabled.
/// The HDisplayControl can be resized during the execution. In this
/// case the size of graphical display and the displayed objects
/// will be adapted to the new size of the control.
/// The class HDisplayControl uses a graphics stack 
/// to manage the iconic objects for the display. Each object is 
/// linked to a graphical context, which determines how the object 
/// is to be drawn. 
/// The context can be changed by calling changeGraphicSettings(). 
/// The graphical "modes" are defined by the class GraphicsContext 
/// and map most of the dev_set_* operators provided in HDevelop.
/// </summary>
namespace HDisplayControl
{
    // delegate as type definition for ROI events 
    public delegate void OnROIChangedHandler(object sender, ROI NewROI);

    public enum ImageViewStates
    {
      fitToWindow = 1,
      fullSizeImage = 2
    }

    public partial class HDisplayControl : UserControl
    {

        #region Events and Variables for ROI Handling 

        /// <summary>
        /// This flag enables or disables the 
        /// functionality for defining the region of interest
        /// using draw operators.
        /// In default case this functionality is activated.
        /// If the property is set to "false" the the toolbar
        /// for ROI handling is deactivated and invisible
        /// </summary>
        /// [true,false]
        bool enabledROISetup;
        private bool showROI;
        /// <summary>
        /// Instance of ROIController, which manages the interactions
        /// user ROIs(regions)
        /// </summary>
        public ROIController roiController;
        
        /// <summary>
        /// The event OnROIChanged is fired, when the activated
        /// ROI (region) is changed. These changes can be 
        /// position changes, size changes or/and orientation
        /// changes.
        /// </summary>
        public event OnROIChangedHandler OnROIChanged;
        /// <summary>
        /// The event OnROICreated is fired, when an user
        /// draw a new ROI (region) in HDisplayControl. 
        /// </summary>
        public event OnROIChangedHandler OnROICreated;
        /// <summary>
        /// The event OnROISignChanged is fired, when the
        /// the sign of activated ROI (region) is changed. 
        /// </summary>
        public event OnROIChangedHandler OnROISignChanged;
        /// <summary>
        /// The event OnActiveROIDeleted is fired, when 
        /// an activated ROI (region) is deleted.
        /// </summary>
        public event OnROIChangedHandler OnActiveROIDeleted;

        #endregion

        /*-------------------------------------------------------------------*/

        #region Definition of private class members

        /*********************************************************************
         * Definition of private class members
         *********************************************************************/

        // a wrapper class for the HALCON window HWindow
        private HWndCtrl hWndControl;

        // The coordinates of HWindow in HDisplayControl
        private Rectangle windowExtents;

        // Currently displayed HALCON Image 
        private HImage hImage;

        // The region that is calculated from the 
        // all drawn ROIs 
        // Or the region of interest set manually by 
        // assigment a new region to the property (CurrentROI)
        private HRegion regionOfInterest;
        // The dimensions of HALCON Image
        // If any image is currently displayed, the imageWidth
        // and imageHeight are set to 0
        private int imageWidth, imageHeight;

        // This object is used to lock the code that the accesses 
        // the HALCON image and graphic stack. These prevents
        // That the image acquisition thread and GUI Thread
        // acess the graphic stack at the same time
        private readonly object locker;

        /*----------- Zoom --------------------------------*/
        // Coordinates of the point in the image, that is 
        // defined by the current mouse position in the image.
        // These coordinates are used as the zoom center.
        private Point zoomCenter;

        // The current value of the zoom state of the image 
        // given in per cent (%)
        private int   displayZoomValue;
        // The flag that signalize if the option for zooming
        // with mouse wheel is swithed on (true) or switched off
        private bool  zoomOnMouseWheel;

        /*-------------------------------------------------*/
        // This flag swithes the option for movement of the displayed image
        // by pressing the left mouse button and moving the mouse cursor in
        // HDisplayControl
        private bool moveOnPressedMouseButton;

        /*----------- Options for displaying the halcon image -------------*/
        /// <summary>If the state is set to true, then the image view 
        /// is adapted to the size of the window with correct aspect ration
        /// </summary>
        /// [fitToWindow,fullSizeImage]
        private ImageViewStates imageViewState;

        #endregion

        #region Construction and Deconstruction
        /*********************************************************************
         * Construction and Deconstruction
         *********************************************************************/
        public HDisplayControl()
        {

            InitializeComponent();

            //DoubleBuffer: 
            //UserPaint: 
            //AllPaintingInWmPaint: 
            //ResizeRedraw:
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw, true);

            HSystem.SetSystem("clip_region", "false");

            locker = new object();
            this.ImageViewState = ImageViewStates.fitToWindow;

            // Intialize ScrollBars and display modus of image
            hScrollBar1.Enabled = false;
            hScrollBar1.Value = 0;
            vScrollBar1.Enabled = false;
            vScrollBar1.Value = 0;


            // Initialize ToolBar
            toolStrip1.Width = viewPort.Location.X + viewPort.Width;

            // Region interaction
            this.EnabledROISetup = true;
 
        }

        #endregion


        #region Definition of HDisplayControl properties

        /*********************************************************************
         * Definition of HDisplayControl properties
         *********************************************************************/

        [Browsable(false)]
        public HWndCtrl HWndCtrl
        {
            get
            {
                return this.hWndControl;
            }         
        }

		[Browsable(true)]
		public bool ShowROI
		{
			get
			{
				return showROI;
			}
			set
			{
				if (hWndControl != null)
				{
					showROI = value;
					if (showROI)
					{
						hWndControl.ShowROI = HWndCtrl.MODE_INCLUDE_ROI;
						this.Invalidate();
					}
					else
					{
						hWndControl.ShowROI = HWndCtrl.MODE_EXCLUDE_ROI;
						this.Invalidate();
					}
				}
			}
		}

        /// <summary>
        /// Gets or sets the current size of the graphical display,
        /// (not the whole control!)
        /// </summary>
        [Browsable(true)]
        [Description("Gets the current size of the graphical display,"+
                     "(not the whole control!)")]
        [DesignerSerializationVisibility
         (DesignerSerializationVisibility.Visible)]
        public Size WindowSize
        {
          get
          {
            return this.viewPort.WindowSize;
          }
          set
          {
            this.viewPort.WindowSize = value;
          }
        }

        /// <summary>
        /// Gets the current image to display.
        /// </summary>
        [Browsable(false)]
        [Description("Get the reference to HWindow.")]
        public HWindow HalconWindow
        {
            get
            {
                return this.viewPort.HalconWindow;                
            }
        }


        /// <summary>
        /// Gets the current image to display.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the current image to display.")]        
        [DesignerSerializationVisibility
         (DesignerSerializationVisibility.Hidden)]
        public HImage Image
        {
          get
          {
            lock (locker)
            {
              return hImage;
            }
          }
          set
          {
            lock (locker)
            {
              hImage = value;
              if (hImage != null)
              {
                try
                {
                  hImage.GetImageSize(out imageWidth, out imageHeight);
                }
                catch
                {
                  imageWidth  = 0;
                  imageHeight = 0;
                }
              }
            }//lock
            this.AddObjectToGraphicStack(hImage);
          }
        }


        /// <summary>
        /// Gets or sets the state of the image view. The state impacts the 
        /// image view in graphic window. The values are fitToWindow 
        /// (the image is scaled so the whole image is displayed in 
        /// the graphic window), fullSizeImage (the image is dispalyed in 
        /// current image size. The Scorllbars apear, if the image exceeds the 
        /// limits of graphical window.)
        /// </summary>
        [Browsable(true)]
        [Description("Gets or sets the state of the image" + 
                     " view in graphic window.")]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Visible)] 
        public ImageViewStates ImageViewState
        {
          get
          {
            return imageViewState;
          }
          set
          {
            if (value == ImageViewStates.fitToWindow)
            {
              imageViewState = value;

              if (!this.DesignMode)
              {
                  if (hWndControl != null)
                      hWndControl.adaptSize = true;
                  if (this.Image != null)
                  {
                      // set the image of the Halcon window 
                      // to the size of current image
                      hWndControl.resetImagePart(imageWidth, imageHeight);
                      this.Invalidate();
                  }
              }
            }
            else
              if (value == ImageViewStates.fullSizeImage)
              {
                imageViewState = value;
                hWndControl.adaptSize = false;
                if (!this.DesignMode)
                {
                    if (this.Image != null)
                        setFullImageSize();
                }
              }
              else
                throw new InvalidEnumArgumentException("Invalid value of Property " +
                                                       "ImageViewState. " +
                                                       "The property can have to " +
                                                       "different values \"fitToWindow\"" +
                                                       "\"fullSizeImage\".");
          }
        }

        /*------------------------------------------------------------------*/
        /*-------------------------Zoom-------------------------------------*/
        /*------------------------------------------------------------------*/
        /// <summary>
        /// Coordinates of the image marked as zoom center. Initial value is 
        /// the center of the image. X-coordinate corresponds the column 
        /// coordinate and Y-coodrinate to the row coordinate of image. The 
        /// zoom center is changed if you click with the left mouse button 
        /// in the display image and then scroll the mouse wheel.
        /// </summary>
        [Browsable(false)]
        [Description("Coordinates of the image marked as zoom center. " +
                     "Initial value is the center of the image. X-coordinate" +
                     " corresponds the column coordinate and Y-coodrinate to" +
                     " the row coordinate of image. The zoom center is " +
                     " changed if you click with the left mouse button" + 
                     " in the display image and then scroll the mouse wheel")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [CategoryAttribute("Zoom")]
        public Point ZoomCenter
        {
          get 
          { 
            return zoomCenter; 
          }
          set
          {            
            if (value.IsEmpty)
            {
              zoomCenter = new Point((imageWidth / 2), imageHeight / 2);
            }
            else
            {
              if ((imageWidth > 0 ) && (imageHeight > 0))
              {
                if ((value.X <= imageWidth) && (value.X >= 0) && 
                    (value.Y <= imageHeight) && (value.Y >= 0))
                  this.zoomCenter = value;
                else
                {
                  string excString = "The coordinates of ZoomCenter should " +
                                     "be within image.";
                  throw new ArgumentOutOfRangeException(excString,
                                                        "ZoomCenter");
                }
              }
              else
              {
                zoomCenter = value;
              }
            }
          }
        }


         /// <summary>
        /// Gets the current zoom value of display expressed as a 
        /// percentage of original image size.
        /// </summary>
        [Browsable(false)]
        [Description("Gets the current zoom value of display expressed as a " +
                     "percentage of original image size.")]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
        [CategoryAttribute("Zoom")]
        [DefaultValue(100)]
        public int DisplayZoomValue
        {
          get
          {
            return this.displayZoomValue;
          }
        }


        /// <summary>
        /// Gets or sets the property to zoom the image by 
        /// scrolling the mouse wheel. The center of zoom is 
        /// set to the current image position of the mouse.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility
         (DesignerSerializationVisibility.Visible)]
        [Description("Specifies, if the zoom with mouse wheel is activated " +
                     "or not. The center of zoom is set to the current " +
                     "image position of the mouse.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [CategoryAttribute("Zoom")]
        [DefaultValue(true)]
        public bool ZoomOnMouseWheel
        {
          get
          {
            return this.zoomOnMouseWheel;
          }
          set
          {
            this.zoomOnMouseWheel = value;
          }
        }


        /// <summary>
        /// Specifies, if the moving of displayed objects
        /// by pressed mouse button is activated or not.
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility
         (DesignerSerializationVisibility.Visible)]
        [Description("Specifies, if the moving of displayed objects" +
                      "by pressed mouse button is activated or not.")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [CategoryAttribute("Move")]
        [DefaultValue(false)]
        public bool MoveOnPressedMouseButton
        {
            get
            {
                return this.moveOnPressedMouseButton;
            }
            set
            {
                this.moveOnPressedMouseButton = value;
                if (this.moveOnPressedMouseButton && hWndControl != null)
                  hWndControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
            }
        }


        /// <summary>
        /// Specifies, if the toolbar for setup of roi
        /// is activated and visible or not.
        /// </summary>
        [Browsable(true)]
        [Description("Specifies, if the toolbar for setup of roi"+
                     " is activated and visible or not.")]
        [DefaultValue(true)]
        public bool EnabledROISetup
        {
            get
            {
                return this.enabledROISetup;
            }
            set
            {
                this.enabledROISetup = value;
                if (this.enabledROISetup)
                {
                    toolStrip1.Enabled = true;
                    toolStrip1.Visible = true;
                }
                else
                {
                    toolStrip1.Enabled = false;
                    toolStrip1.Visible = false;
                }
            }
        }


        
        /// <summary>
        /// Gets or sets the current region of interest.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the current region of interest.")]
        public HRegion CurrentROI
        {
            get
            {
                if (!this.DesignMode)
                {
                    CalcCurrentROI();
                    return this.regionOfInterest;
                }
                else
                    return null;
            }
            set
            {
                if (!this.DesignMode)
                {
                    if (value != null)
                    {
                        this.regionOfInterest = value;
                        roiController.ModelROI = this.regionOfInterest;
                    }
                    else
                    {
                        if (roiController != null)
                        {
                            //Clears all variables managing ROI objects
                            roiController.getROIList().Clear();
                            roiController.defineModelROI();
                            this.regionOfInterest = roiController.getModelRegion();
                        }
                        else
                            this.regionOfInterest = null;
                        // set the image part to the whole image
                        if (this.Image != null)
                            hWndControl.resetImagePart(imageWidth, imageHeight);
                    }
                }
            }
        }


        /// <summary>
        /// Returns the reference to the object ROIController
        /// that is responsible for the management of the regions
        /// that belongs to region of interest.
        /// </summary>
        [Browsable(false)]
        [Description("Returns the reference to the object ROIController, "+
                     "that is responsible for the management of the regions "+
                     " that belongs to region of interest")]
        public ROIController ROIController
        {
            get
            {
                return this.roiController;
            }            
        }
    
       #endregion


        /// <summary>
        /// Reset window settings for zoom and pan 
        /// </summary>
        public void ResetDisplaySettings()
        {
          // clear all settings in graphic window
          //hWndControl.resetAll();
            hWndControl.resetWindow();
           
          // set the flag to display the full image 
          // in correct aspect ration in graphic window
          if ( imageViewState  == ImageViewStates.fitToWindow)
            hWndControl.adaptSize = true;
          else
            hWndControl.adaptSize = false;
          if ((imageWidth > 0) && (imageHeight > 0))
            hWndControl.resetImagePart(imageWidth, imageHeight);

          this.Invalidate();
        }


        /// <summary>
        /// Reset window settings including settings for drawing ROIs
        /// </summary>
        public void ResetDisplaySettingsInclROI()
        {
            // clear all settings in graphic window
            hWndControl.resetAll();
            // set the flag to display the full image 
            // in correct aspect ration in graphic window
            if (imageViewState == ImageViewStates.fitToWindow)
                hWndControl.adaptSize = true;
            else
                hWndControl.adaptSize = false;

            if ((imageWidth > 0) && (imageHeight > 0))
                hWndControl.resetImagePart(imageWidth, imageHeight);

            this.Invalidate();
        }

        /// <summary>
        /// Clears the content in the graphic stack 
        /// that is managed by HDisplayControl
        /// </summary>
        public void ClearGraphicStack()
        {
            hWndControl.clearList();
            // set the flag to display the full image 
            // in correct aspect ration in graphic window
            if (imageViewState == ImageViewStates.fitToWindow)
                hWndControl.adaptSize = true;
            else
                hWndControl.adaptSize = false;

            if ((imageWidth > 0) && (imageHeight > 0))
                hWndControl.resetImagePart(imageWidth, imageHeight);

            this.Invalidate();
        }

        /// <summary>
        /// Clears !only! the window
        /// </summary>
        public void ClearDisplay()
        {
            this.HalconWindow.ClearWindow();
        }

        /// <summary>
        /// Zooms the image around the image coordinate supplied 
        /// in [centerX, centerY] by value, that is provided
        /// by parameter zoomFactor. The zoomFactor describe the grade of zooming 
        /// in per cents.
        /// </summary>
        /// <param name="centerX"> Column coordinate of zoom center </param>
        /// <param name="centerY"> Row coordinate of zoom center </param>
        /// <param name="zoomFactor"> Zoom value in percenetage </param>
        public void ZoomImage(double centerX, double centerY, int zoomFactor)
        {
          this.displayZoomValue = zoomFactor;

          // zoom image 
          hWndControl.zoomByGUIHandle(displayZoomValue);

          // repaint the graphic window
          this.Invalidate();
        }


        /// <summary> 
        /// The object will be pushed to the graphic stack of image 
        /// display. The objects on the graphic stack will not be
        /// displayed automatically. To display the objects on the 
        /// graphic stack, please call method Refresh.
        /// </summary>
        /// <param name="obj"> HALCON iconic object </param>
        public void AddObjectToGraphicStack(HObject obj)
        {
            lock (locker)
            {
                if (obj != null)
                {
                    HObject copyObj = obj.CopyObj(1,-1);
                    hWndControl.addIconicVarKeepSettings(obj);
                }
            }

        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed. Actually 
        /// you can set up following properties of graphical context 
        /// using this function: 
        /// GraphicsContext.GC_COLOR    (see dev_set_color)
        /// GraphicsContext.GC_DRAWMODE (see set_draw)
        /// GraphicsContext.GC_SHAPE    (see set_shape)
        /// GraphicsContext.GC_LUT      (see set_lut)
        /// GraphicsContext.GC_PAINT    (see set_paint)
        /// </param>
        /// <param name="val">
        /// Value, provided as a string, 
        /// the mode is to be changed to, e.g., "blue" 
        /// </param>       
        public void ChangeGraphicSettings(string mode, string val)
        {
            hWndControl.changeGraphicSettings(mode, val);
        }


        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed. Actually you can 
        /// set up following properties of graphical context using this 
        /// function: 
        /// GraphicsContext.GC_LINEWIDTH (see set_line_width)
        /// GraphicsContext.GC_COLORED   (see dev_set_colored)
        /// </param>
        /// <param name="val">
        /// Value, provided as an integer, the mode is to be changed to, 
        /// e.g., 5 
        /// </param>
        public void ChangeGraphicSettings(string mode, int val)
        {
            hWndControl.changeGraphicSettings(mode, val);
        }


        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed.Actually you can 
        /// set up following properties of graphical context using this 
        /// function: 
        /// GraphicsContext.GC_LINESTYLE (see set_line_style)
        /// </param>
        /// <param name="val">
        /// Value, provided as an HTuple instance, the mode is 
        /// to be changed to, e.g., new HTuple(new int[]{2,2})
        /// </param>
        public void ChangeGraphicSettings(string mode, HTuple val)
        {
            hWndControl.changeGraphicSettings(mode, val);
        }


        /// <summary> 
        /// Repaint the content of the graphic window
        /// </summary>
        public void RefreshDisplay()
        {
            /*
             * repaint the graphic control including the 
             * actual graphic stack
            */
            this.Invalidate();
        }

        /// <summary> 
        /// Performs the initialization of the HDisplayControl
        /// during loading to the memory.
        /// </summary>
        private void HDisplayControl_Load(object sender, EventArgs e)
        {

          hWndControl = new HWndCtrl(viewPort);

          // Initialization graphic window size
          windowExtents = new Rectangle(0, 0, this.viewPort.WindowSize.Width,
                                                this.viewPort.WindowSize.Height);


          imageWidth = imageHeight = 0;

          displayZoomValue = 100;
          zoomCenter = new Point(windowExtents.Width / 2, windowExtents.Height / 2);

          if (MoveOnPressedMouseButton)
            hWndControl.setViewState(HWndCtrl.MODE_VIEW_MOVE);
          else
            hWndControl.setViewState(HWndCtrl.MODE_VIEW_NONE);  

          // add event handler after zooming the image
          hWndControl.OnImageZoomed += new OnIconicObjectZoomedHandler(
                                  this.hWndControl_IconicObjectZoomed);
          hWndControl.OnImageMoved  += new OnIconicObjectMovedHandler(
                                  this.hWndControl_IconicObjectMoved);
          // setup ROIController
          roiController = new ROIController();
          hWndControl.useROIController(roiController);

          // handle the changes of regions
          roiController.NotifyRCObserver = null;
          roiController.NotifyRCObserver = new IconicDelegate(UpdateViewData);
          hWndControl.clearList();
          //---------

          // set the sign of the draw region to the value "Add Region"
          roiController.setROISign(ROIController.MODE_ROI_POS);
          this.ShowROI = true;
        }


        /// <summary> 
        /// Performs event handling of the HMouseWheel event of
        /// HWindowControl, so that the dipslayed image  part and scroll bars 
        /// of HDisplayControl can be adapted to the current zoom value.
        /// </summary>
        private void viewPort_HMouseWheel(object sender, HMouseEventArgs e)
        {
          hWndControl.mouseWheel(sender, e);
          ManageScrollBars();
          hWndControl.repaint();

          this.Invalidate();
        }


        /// <summary> 
        /// Event handling of paint event. The methods takes care, that the 
        /// image part is displayed correctly and the scroll bars appear 
        /// if they are necessary.
        /// </summary>
       private void HDisplayControl_Paint(object sender, PaintEventArgs e)
       {
           if (!this.DesignMode)
           {
               try
               {                  
                   displayZoomValue = (int)hWndControl.ZoomFactor;
                   ManageScrollBars();
               }
               finally
               {
                   hWndControl.repaint();
                   HOperatorSet.SetSystem("flush_graphic", "true");
                   viewPort.HalconWindow.DispCircle(-100.0, -100.0, 1);

               }
           }
       }

       /// <summary>
       /// Event handling for zooming the displayed iconic objects 
       /// </summary>
       private void hWndControl_IconicObjectZoomed(object sender, 
                                                   double zoomCenterX,
                                                   double zoomCenterY,
                                                   double scaleFactor)
       {
         ZoomCenter = new Point((int)Math.Round(zoomCenterX), 
                                (int)Math.Round(zoomCenterY));
         displayZoomValue = (int)scaleFactor;
         this.Invalidate();
       }


       /// <summary>
       /// By resizing the controls the visualization of displayed iconic 
       /// objects should also be adapted to the new size of graphic window.
       /// </summary>
       new public void Resize(object sender, EventArgs e)
       {

            Rectangle imagePart;
            imagePart = viewPort.ImagePart;

           // adapt the displayed image part 
           // to the new size of display
           if (hWndControl.adaptSize)
           {
               if (this.Image != null)
               {
                   // set the image part of the Halcon window 
                   // to the size of current image
                   if ((imagePart.Width >= imageWidth) &&
                       (imagePart.Height >= imageHeight))
                   {
                       hWndControl.resetImagePart(imageWidth, imageHeight);
                   }
                   else
                   {        
                       // The window is resized and this impacts that the 
                       // visible image part has to be changed. Adapt
                       // the image part to new window size.
                       imagePart.Width = viewPort.Width;
                       imagePart.Height = viewPort.Height;
                       viewPort.ImagePart = imagePart;
                   }
               }
           }
           else
               if (!hWndControl.adaptSize)
               {
                   if (this.Image != null)
                       setFullImageSize();
               }

           this.Invalidate();
       }

       /// <summary>
       /// Event handling for resizing the graphic window
       /// </summary>
       private void HDisplayControl_Resize(object sender, EventArgs e)
       {
         // update the size of the HALCON window if the 
         // the whole component is resized
         UpdateHalconWindowExtents();

         // set the position of ScrollBars
         vScrollBar1.Location = new Point((viewPort.Location.X + 
                                           windowExtents.Width),
                                          viewPort.Location.Y);


         if (hWndControl.adaptSize)
         {
             if (this.Image != null)
                 // set the image of the Halcon window 
                 // to the size of current image
                 hWndControl.resetImagePart(imageWidth, imageHeight);
         }
         else
         {
             if (this.Image != null)
                setFullImageSize();
         }

         // this calls the Paint-Method
         this.Invalidate();
       }

       /// <summary>
       /// Event handling if the content of the graphic window is moved.
       /// </summary>
       private void hWndControl_IconicObjectMoved(object sender,
                                                  double moveX,
                                                  double moveY)
       {
           if (MoveOnPressedMouseButton)
            this.Invalidate();
       }


       /// <summary>
       /// Updates the size of HWindowControl during size changing of whole
       /// user control.
       /// </summary>
       private void UpdateHalconWindowExtents()
       {
         int windowWidth = this.ClientSize.Width - 
                           2 * this.viewPort.BorderWidth - 
                           vScrollBar1.Width - 2;
         int windowHeight = this.ClientSize.Height -
                            2 * this.viewPort.BorderWidth - 
                            hScrollBar1.Height - 2;

         windowExtents = new Rectangle(this.viewPort.BorderWidth, 
                                       this.viewPort.BorderWidth,
                                       windowWidth,
                                       windowHeight);
         // update extens of window
         this.viewPort.WindowSize = new Size(windowWidth, windowHeight);
       }



      /// <summary>
      /// This function analyzes the image part of current image and activates 
      /// the scroll bars in case the image part is larger than graphic window.
      /// </summary>
      private void ManageScrollBars()
      {
        int widthImagePart;
        int heightImagePart;
        int[] range = new int[2];

        Rectangle rect = viewPort.ImagePart;
        widthImagePart = rect.Width;
        heightImagePart = rect.Height;
        double wndWidth  = viewPort.Width;
        double wndHeight = viewPort.Height;

        if ((widthImagePart < imageWidth))
        {
          hScrollBar1.Enabled = true;
          hScrollBar1.Visible = true;
          hScrollBar1.Minimum = 0;
          hScrollBar1.Maximum = imageWidth - 1;
          hScrollBar1.LargeChange = (int)Math.Ceiling
                                     (wndWidth / 
                                     (imageWidth / widthImagePart));
          range[0] = hScrollBar1.Minimum;
          range[1] = hScrollBar1.Maximum;

          if (rect.X > 0)
            if (rect.X < (hScrollBar1.Maximum - hScrollBar1.LargeChange))
              hScrollBar1.Value = rect.X;
            else
              hScrollBar1.Value = hScrollBar1.Maximum;
          else
            hScrollBar1.Value = 0;

        }
        else
        {
          hScrollBar1.Enabled = false;
          hScrollBar1.Visible = false;
        }

        // analyse the height of image part
        if ((heightImagePart < imageHeight))
        {
          vScrollBar1.Enabled = true;
          vScrollBar1.Visible = true;
          vScrollBar1.Minimum = 0;
          vScrollBar1.Maximum = imageHeight - 1;            
          vScrollBar1.LargeChange = (int)Math.Ceiling(
                                     wndHeight / 
                                     (imageHeight / heightImagePart));
          range[0] = vScrollBar1.LargeChange;
          range[1] = vScrollBar1.Maximum;


          if (rect.Y > 0)
            if (rect.Y < (vScrollBar1.Maximum - vScrollBar1.LargeChange))
              vScrollBar1.Value = rect.Y;
            else
              vScrollBar1.Value = vScrollBar1.Maximum;
          else
            vScrollBar1.Value = 0;
        }
        else
        {
            if (vScrollBar1.Visible)
            {
                vScrollBar1.Enabled = false;
                vScrollBar1.Visible = false;
            }
        }              
      }


      /// <summary>
      /// Event handling for horizontal scroll bar. The image part will
      /// be adapted according to the position of horizontal scroll bar.
      /// </summary>
      private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
      {
        Rectangle rect = viewPort.ImagePart;
        rect.X = hScrollBar1.Value;
        hWndControl.setImagePart(rect.Y, rect.X, 
                                 rect.Y + rect.Height, 
                                 rect.X + rect.Width);
        hWndControl.repaint();  
      }

      /// <summary>
      /// Event handling for vertical scroll bar. The image part will
      /// be adapted according to the position of vertical scroll bar.
      /// </summary>
      private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
      {
        // set new image part during scrolling
        Rectangle rect = viewPort.ImagePart;
        rect.Y = vScrollBar1.Value;
        hWndControl.setImagePart(rect.Y, rect.X, 
                                 rect.Y + rect.Height, 
                                 rect.X + rect.Width);
        hWndControl.repaint();
      }


      /// <summary>
      /// Sets the mode of graphic window to display the 
      /// image in its full image size. If the image part
      /// of the displayed image is larger than the graphic
      /// window then the scroll bars appear. 
      /// </summary>
      private void setFullImageSize()
      {
        if (this.Image != null)
        {
          hWndControl.adaptSize = false;
          hWndControl.resetImagePart(imageWidth, imageHeight);
          this.Invalidate();
        }
      }

      /// <summary>
      /// Sets the shape of region to draw to axis-aligned rectangle
      /// </summary>
      private void toolStripButton1_Click(object sender, EventArgs e)
      {
          roiController.setROIShape(new ROIRectangle1());
          this.Invalidate();
      }

      /// <summary>
      /// Sets the shape of region to draw to rotated rectangle
      /// </summary>
      private void toolStripButton2_Click(object sender, EventArgs e)
      {
          roiController.setROIShape(new ROIRectangle2());
      }

      /// <summary>
      /// Sets the shape of region to draw to line
      /// </summary>
      private void toolStripButton3_Click(object sender, EventArgs e)
      {
          roiController.setROIShape(new ROILine());
      }

      /// <summary>
      /// Sets the shape of region to draw to circle
      /// </summary>
      private void toolStripButton4_Click(object sender, EventArgs e)
      {
          roiController.setROIShape(new ROICircle());
      }


      /// <summary>
      /// Sets the shape of region to draw to circular arc
      /// </summary>
      private void toolStripButton5_Click(object sender, EventArgs e)
      {
          try
          {
              roiController.setROIShape(new ROICircularArc());
          }
          catch (HOperatorException exception)
          {
              throw exception;
          }
      }


      /// <summary>
      /// Update the current ROI (region of interest) according to 
      /// the changes that were performed through user interaction.
      /// </summary>
      private void toolStripDeleteSelectedRegion_Click(object sender, EventArgs e)
      {
          int activeROIIdx = roiController.getActiveROIIdx();
          if (activeROIIdx > -1)
              roiController.removeActive();
      }


      /// <summary>
      /// Update the current ROI (region of interest) according to 
      /// the changes that were performed through user interaction.
      /// </summary>
      public void UpdateViewData(int val)
      {
        switch (val)
        {
            case ROIController.EVENT_CHANGED_ROI_SIGN:
               CalcCurrentROI();
               if (OnROISignChanged != null)
                   OnROICreated(this, roiController.getActiveROI());
               break;
            case ROIController.EVENT_DELETED_ACTROI:
            case ROIController.EVENT_DELETED_ALL_ROIS:
                CalcCurrentROI();
                if (OnActiveROIDeleted != null)
                    // if activated ROI is deleted or all ROIs
                    // are deleted, the event parameter for ROI 
                    // is set NULL
                    OnActiveROIDeleted(this, null);
                    break;                        
            case ROIController.EVENT_CREATED_ROI:
                CalcCurrentROI();
                if (OnROICreated != null)
                {
                    OnROICreated(this, roiController.getActiveROI());
                }
                break;
            case ROIController.EVENT_UPDATE_ROI:
                CalcCurrentROI();
                if (OnROIChanged != null)
                    OnROIChanged(this, roiController.getActiveROI());
                break;
            case ROIController.EVENT_REPAINT_ROI:
                this.Invalidate();
                break;
             default:
                    break;
          }
        this.Invalidate();
       }

       /// <summary>
       /// Update the current ROI (region of interest) according to 
       /// the changes that were performed through user interaction.
       /// </summary>
        private void CalcCurrentROI()
        {
            bool genROI = false;
            try
            {
                genROI = roiController.defineModelROI();
            }
            catch (HOperatorException exception)
            {
                MessageBox.Show("Error occured during calculating" +
                                " the region of interest:\n" +
                                exception.Message);
                hWndControl.repaint();
            }
            regionOfInterest = roiController.getModelRegion();
            if (!genROI)
                hWndControl.repaint();
        }


        /// <summary>
        /// Adds the new region to the region of interest (ROI)
        /// </summary>
        private void btnRegionAdd_Click(object sender, EventArgs e)
        {
            roiController.setROISign(ROIController.MODE_ROI_POS);
        }

        /// <summary>
        /// Exludes the area defined by new region from 
        /// the region of interest (ROI)
        /// </summary>
        private void btnRegionDiff_Click(object sender, EventArgs e)
        {
            roiController.setROISign(ROIController.MODE_ROI_NEG);
        }


        /// <summary>
        /// Event handling for entering the Delete-Button.
        /// If one of the drawn regions is activated then 
        /// the activated region will be deleted.
        /// </summary>
        private void viewPort_KeyDown(object sender, KeyEventArgs e)
        {
          Keys button = e.KeyCode;
          // if the pressed button is "Del"
          if (e.KeyCode == Keys.Delete)
              // if one region is activated, then delete it
              if (roiController.activeROIidx > -1)
                  roiController.removeActive();                
        }


        /// <summary>
        /// 
        /// </summary>
        private void viewPort_HInitWindow(object sender, EventArgs e)
        {
            hImage = null;
            regionOfInterest = new HRegion();
        }   
      
    }
}

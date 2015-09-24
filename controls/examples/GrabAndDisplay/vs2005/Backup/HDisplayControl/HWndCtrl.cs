using System;
using HDisplayControl.ViewROI;
using System.Collections;
using System.Drawing;
using HalconDotNet;



namespace HDisplayControl.ViewROI
{
	public delegate void IconicDelegate(int val);
	public delegate void FuncDelegate();
    public delegate void OnIconicObjectMovedHandler(Object sender, double moveX, double moveY);
    public delegate void OnIconicObjectZoomedHandler(Object sender, double zoomCenterX, double zoomCenterY, double scale);


	/// <summary>
	/// This class works as a wrapper class for the HALCON window
	/// HWindow. HWndCtrl is in charge of the visualization.
	/// You can move and zoom the visible image part by using GUI component 
	/// inputs or with the mouse. The class HWndCtrl uses a graphics stack 
	/// to manage the iconic objects for the display. Each object is linked 
	/// to a graphical context, which determines how the object is to be drawn.
	/// The context can be changed by calling changeGraphicSettings().
	/// The graphical "modes" are defined by the class GraphicsContext and 
	/// map most of the dev_set_* operators provided in HDevelop.
	/// </summary>
	public class HWndCtrl
	{
	  /// <summary>No action is performed on mouse events</summary>
	  public const int MODE_VIEW_NONE       = 10;

	  /// <summary>Zoom is performed on mouse events</summary>
	  public const int MODE_VIEW_ZOOM       = 11;

	  /// <summary>Move is performed on mouse events</summary>
	  public const int MODE_VIEW_MOVE       = 12;

	  /// <summary>Magnification is performed on mouse events</summary>
	  public const int MODE_VIEW_ZOOMWINDOW	= 13;


	  public const int MODE_INCLUDE_ROI     = 1;

	  public const int MODE_EXCLUDE_ROI     = 2;


        /// <summary>
        /// Constant describes delegate message to signal new image
        /// </summary>
        public const int EVENT_UPDATE_IMAGE   = 31;
        /// <summary>
        /// Constant describes delegate message to signal error
        /// when reading an image from file
        /// </summary>
        public const int ERR_READING_IMG      = 32;
        /// <summary> 
        /// Constant describes delegate message to signal error
        /// when defining a graphical context
        /// </summary>
        public const int ERR_DEFINING_GC      = 33;

        /// <summary> 
        /// Maximum number of HALCON objects that can be put on the graphics 
        /// stack without loss. For each additional object, the first entry 
        /// is removed from the stack again.
        /// </summary>
        private const int MAXNUMOBJLIST       = 50;


        private int      stateView;
        //private int   displayMode;
        private bool   mousePressed = false;
        private double startX,startY;

        /// <summary>HALCON window</summary>
        private HWindowControl viewPort;
        private HWindow    bufferWindow;

        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private ROIController roiManager;

        /* dispROI is a flag to know when to add the ROI models to the 
        paint routine and whether or not to respond to mouse events for 
        ROI objects */
        private int           dispROI;

        /* Basic parameters, like dimension of window and displayed image part */
        private int   windowWidth;
        private int   windowHeight;
        private int   widthImagePart;
        private int   heightImagePart;
        // width and height of the current image, after applying 
        // manipulation (zooming)
        private int   widthDisplayedImage;
        private int   heightDisplayedImage;
        // width and height of current image
        //private int   imageWidth;
        //private int   imageHeight;

        private int[] CompRangeX;
        private int[] CompRangeY;


        private int    prevCompX, prevCompY;
        private int    initValX, initValY;
        private double stepSizeX, stepSizeY;
        //int k;

        /* Image coordinates, which describe the image part that is displayed  
            in the HALCON window                                               */
        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        /* Initial image coordinates, which describe the image part that is displayed  
        in the HALCON window by adding new image to graphic stack */
        private double InitImgRow1, InitImgCol1, InitImgRow2, InitImgCol2;

        /// <summary>Error message when an exception is thrown</summary>
        public string  exceptionText = "";

        /* Delegates to send notification messages to other classes */
        /// <summary>
        /// Delegate to add information to the HALCON window after 
        /// the paint routine has finished
        /// </summary>
        public FuncDelegate   addInfoDelegate;

        /// <summary>
        /// Delegate to notify about failed tasks of the HWndCtrl instance
        /// </summary>
        public IconicDelegate NotifyIconObserver;

        /// <summary>
        /// This event is fired, if the displayed iconic objects 
        /// were moved
        /// </summary>
        public event OnIconicObjectMovedHandler OnImageMoved;

        /// <summary>
        /// This event is fired, if the displayed iconic objects 
        /// was zoomed
        /// </summary>
        public event OnIconicObjectZoomedHandler OnImageZoomed;

        // handle to the HALCON Window, that willbe temporarily opened,
        // when the "Zoom Window"- Function is activated
        private HWindow ZoomWindow;

        // relation between the size of graphic window and 
        // the image size
        private double  zoomWndFactor;

        // relation between the size of graphic window and 
        // the image size, when the full image is displayed
        // in the graphic window 
        // private double ratioImageSizeToWindowSize;

        // additional zoom grade for external zoom window
        private double  zoomAddOn;
        // size of the external zoom window 
        private int     zoomWndSize;
        private double  zoomImageFactor;

        /// <summary> 
        /// List of HALCON objects to be drawn into the HALCON window. 
        /// The list shouldn't contain more than MAXNUMOBJLIST objects, 
        /// otherwise the first entry is removed from the list.
        /// </summary>
        private ArrayList HObjList;

        /// <summary>
        /// Instance that describes the graphical context for the
        /// HALCON window. According on the graphical settings
        /// attached to each HALCON object, this graphical context list 
        /// is updated constantly.
        /// </summary>
        private GraphicsContext	mGC;

    // MODIFIED

    /// <summary>
    /// boolean variable set to false if image size shall not change with window size
    /// </summary>
    public bool adaptSize;

        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        public HWndCtrl(HWindowControl view)
        {
          viewPort = view;
          stateView = MODE_VIEW_NONE;
          windowWidth = viewPort.Size.Width;
          windowHeight = viewPort.Size.Height;

          // initialize the image part to window size
          initializeDisplayImagePart();
          resetImagePart(widthImagePart, heightImagePart);                        
            

          if (widthImagePart > 0)
            zoomWndFactor = (double)widthImagePart / viewPort.Width;
          else
            zoomWndFactor = 1;

          zoomAddOn = Math.Pow(0.9, 5);
          zoomWndSize = 150;

          /*Set the boundaries and steps for changes for the GUI elements*/
          /*default*/
          CompRangeX = new int[] { 0, 100 };
          CompRangeY = new int[] { 0, 100 };

          prevCompX = prevCompY = 0;
          initValX  = initValY  = 0;

          /* Initialize the values for value range, step for 
           * some GUI elements */
          setGUICompRangeX(CompRangeX, prevCompX);
          setGUICompRangeY(CompRangeY, prevCompY);

          //displayMode = MODE_VIEW_NONE;

          dispROI = MODE_INCLUDE_ROI;//1;

          viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
          viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
          viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);
          viewPort.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.mouseWheel);

          addInfoDelegate = new FuncDelegate(dummyV);
          NotifyIconObserver = new IconicDelegate(dummy);

          // graphical stack 
          HObjList = new ArrayList(20);
          mGC = new GraphicsContext();
          mGC.gcNotification = new GCDelegate(exceptionGC);

          // set the variable bufferWindow to null for repaint-Method        
          bufferWindow = null;
        }

        /// <summary>
        /// initialize the variables, that are used for image part descriotion, 
        /// with the initial values of HWindowControl
        /// </summary>
        private void initializeDisplayImagePart()
        {
            System.Drawing.Rectangle rect = viewPort.ImagePart;
            ImgCol1 = rect.X;
            ImgRow1 = rect.Y;
            ImgRow2 = rect.Height - 1;
            ImgCol2 = rect.Width - 1;

            widthImagePart = Convert.ToInt32(Math.Ceiling(ImgCol2 - ImgCol1));
            heightImagePart = Convert.ToInt32(Math.Ceiling(ImgRow2 - ImgRow1));
        }


        /// <summary>
        /// The Properties define the actual magnification value or diminution 
        /// factor of the image displayed in the graphic window in per cent [%]
        /// </summary>
        public double ZoomFactor
        {
          get
          {
            return (this.zoomWndFactor*100.0);
          }
        }

        public int ShowROI
        {
            get
            {
                return dispROI;
            }
            set
            {
                if ((value == MODE_INCLUDE_ROI) ||
                    (value == MODE_EXCLUDE_ROI))
                    dispROI = value;
            }
        }

        // MODIFIED

        /// <summary>
        /// get window width
        /// </summary>
        public void getWindowSize()
        {
          windowWidth = viewPort.Size.Width;
          windowHeight = viewPort.Size.Height;
        }

        /// <summary>
        /// Registers an instance of an ROIController with this window 
        /// controller (and vice versa).
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useROIController(ROIController rC)
        {
          roiManager = rC;
          rC.setViewController(this);
        }


        /// <summary>
        /// Read dimensions of the image to adjust own window settings
        /// </summary>
        /// <param name="image">HALCON image</param>
        private void setImagePart(HImage image)
        {
          string s;
          int w,h;

          image.GetImagePointer1(out s, out w, out h);
          setImagePart(0, 0, h, w);
        }


        /// <summary>
        /// Adjust window settings by the values supplied for the left 
        /// upper corner and the right lower corner
        /// </summary>
        /// <param name="r1">y coordinate of left upper corner</param>
        /// <param name="c1">x coordinate of left upper corner</param>
        /// <param name="r2">y coordinate of right lower corner</param>
        /// <param name="c2">x coordinate of right lower corner</param>
        public void setImagePart(int r1, int c1, int r2, int c2)
        {
          if (r1 <= r2 )
            ImgRow1 = r1;          
          ImgCol1 = c1;
          ImgRow2 = r2;
          heightImagePart = r2 - r1;
          ImgCol2 = c2;
          widthImagePart = c2 - c1;

          System.Drawing.Rectangle rect = viewPort.ImagePart;
          rect.X = (int)ImgCol1;
          rect.Y = (int)ImgRow1;
          rect.Height = (int)heightImagePart;
          rect.Width = (int)widthImagePart;       
          viewPort.ImagePart = rect;
        }


        /// <summary>
        /// Sets the view mode for mouse events in the HALCON window
        /// (zoom, move, magnify or none).
        /// </summary>
        /// <param name="mode">One of the MODE_VIEW_* constants</param>
        public void setViewState(int mode)
        {
          stateView = mode;

          if (roiManager != null)
            roiManager.resetROI();
        }

        /// <summary>
        /// Sets the view mode for displaying the image sequence in HALCON window
        /// with modified mode (zoom, move, magnify or none).
        /// </summary>
        /// <param name="mode">One of the MODE_VIEW_* constants</param>
 /*       public void setDisplayViewMode(int mode)
        {
          displayMode = mode;

          if (roiManager != null)
              roiManager.resetROI();
        }*/

        /********************************************************************/
        private void dummy(int val)
        {
        }

        private void dummyV()
        {
        }

        /*******************************************************************/
        private void exceptionGC(string message)
        {
          exceptionText = message;
          NotifyIconObserver(ERR_DEFINING_GC);
        }

        /// <summary>
        /// Paint or don't paint the ROIs into the HALCON window by 
        /// defining the parameter to be equal to 1 or not equal to 1.
        /// </summary>
        public void setDispLevel(int mode)
        {
          dispROI = mode;
        }

        /****************************************************************************/
        /*                          graphical element                               */
        /****************************************************************************/
        /// <summary>
        /// Scales the image in the HALCON window according to the 
        /// value scaleFactor and the zoom center are coordinates x and y
        /// </summary>
        private void zoomImage(double x, double y, double scale)
        {
          double lengthC, lengthR;
          double percentC, percentR;
          int    lenC, lenR;

          percentC = (x - ImgCol1) / (ImgCol2 - ImgCol1);
          percentR = (y - ImgRow1) / (ImgRow2 - ImgRow1);

          lengthC = (ImgCol2 - ImgCol1) * scale;
          lengthR = (ImgRow2 - ImgRow1) * scale;

          ImgCol1 = x - lengthC * percentC;
          ImgCol2 = x + lengthC * (1 - percentC);

          ImgRow1 = y - lengthR * percentR;
          ImgRow2 = y + lengthR * (1 - percentR);

          lenC = (int)Math.Round(lengthC);
          lenR = (int)Math.Round(lengthR);

          System.Drawing.Rectangle rect = viewPort.ImagePart;
          rect.X = (int)Math.Round(ImgCol1);
          rect.Y = (int)Math.Round(ImgRow1);
          rect.Width = (lenC > 0) ? lenC : 1;
          rect.Height = (lenR > 0) ? lenR : 1;
          viewPort.ImagePart = rect;

          // if the displayed image area is larger then image, 
          // then the zooming operation  will be restricted to show the 
          // whole image in the graphic window
          if ((rect.Width > widthDisplayedImage) && 
              (rect.Height > heightDisplayedImage))
          {
              // if the image was not full fitted (because of activated modus)
              // in the graphic window, then the modus for fully fitting in
              // the window is activated
              if (!adaptSize)
                  adaptSize = true;

              // set the image part to display the whole image 
              viewPort.ImagePart = new Rectangle(0, 0, widthDisplayedImage, 
                                                      heightDisplayedImage);
              // 
              resetImagePart(widthDisplayedImage, 
                             heightDisplayedImage);
          }
          else
          {
              zoomWndFactor *= scale;
              zoomImageFactor = (1.0 / ((double)lenC / widthImagePart)) * 
                                (100.0 / scale);
          }
          if (OnImageZoomed != null)
              OnImageZoomed(this, x, y, zoomImageFactor);

        }


        /// <summary>
        /// Scales the image in the HALCON window according to the 
        /// value scaleFactor
        /// </summary>
        public void zoomImage(double scaleFactor)
        {
          double midPointX, midPointY;

          if(((ImgRow2 - ImgRow1) == scaleFactor * heightImagePart) &&
            ((ImgCol2 - ImgCol1) == scaleFactor * widthImagePart))
          {
            return;
          }

          ImgRow2 = ImgRow1 + heightImagePart;
          ImgCol2 = ImgCol1 + widthImagePart;

          midPointX = ImgCol1;
          midPointY = ImgRow1;

          zoomWndFactor = (double)widthImagePart / viewPort.Width;
          zoomImage(midPointX, midPointY, scaleFactor);
        }


        /// <summary>
        /// Scales the HALCON window according to the value scale
        /// </summary>
        public void scaleWindow(double scale)
        {
          ImgRow1 = 0;
          ImgCol1 = 0;

          ImgRow2 = heightImagePart;
          ImgCol2 = widthImagePart;

          viewPort.Width = (int)(ImgCol2 * scale);
          viewPort.Height = (int)(ImgRow2 * scale);

          zoomWndFactor = ((double)widthImagePart / viewPort.Width);
        }


        /// <summary>
        /// Recalculates the image-window-factor, which needs to be added to 
        /// the scale factor for zooming an image. This way the zoom gets 
        /// adjusted to the window-image relation, expressed by the equation 
        /// imageWidth/viewPort.Width.
        /// </summary>
        public void setZoomWndFactor()
        {
          zoomWndFactor = ((double)widthImagePart / viewPort.Width);
        }


        public void setZoomWndFactor2(double imageWidth0)
        {
            zoomWndFactor = ((double) imageWidth0 / viewPort.Width);
        }

        /// <summary>
        /// Sets the image-window-factor to the value zoomF
        /// </summary>
        public void setZoomWndFactor(double zoomF)
        {
          zoomWndFactor = zoomF;
        }

        /*******************************************************************/
        public void moveImage(double motionX, double motionY)
        {
          ImgRow1 += -motionY;
          ImgRow2 += -motionY;

          ImgCol1 += -motionX;
          ImgCol2 += -motionX;

          System.Drawing.Rectangle rect = viewPort.ImagePart;
          rect.X = (int)Math.Round(ImgCol1);
          rect.Y = (int)Math.Round(ImgRow1);
          viewPort.ImagePart = rect;
        }

        /// <summary>
        /// Move image according the displacement values given in 
        /// motionX and motionY
        /// </summary>
        public void moveImageFromInit(double motionX, double motionY)
        {
          ImgRow1 = InitImgRow1 - motionY;
          ImgRow2 = InitImgRow2 - motionY;

          ImgCol1 = InitImgCol1 - motionX;
          ImgCol2 = InitImgCol2 - motionX;

          System.Drawing.Rectangle rect = viewPort.ImagePart;
          rect.X = (int)Math.Round(ImgCol1);
          rect.Y = (int)Math.Round(ImgRow1);
          viewPort.ImagePart = rect;
        }


        /// <summary>
        /// Move image according the displacement and axis 
        /// </summary>
        public void moveImageFromInit(double motion, string axis)
        {
          if (axis == "y")
          {
            ImgRow1 = InitImgRow1 - motion;
            ImgRow2 = InitImgRow2 - motion;
          }
          else if (axis == "x")
          {
            ImgCol1 = InitImgCol1 - motion;
            ImgCol2 = InitImgCol2 - motion;
          }
          else
            throw new 
              ArgumentException("Invalid name of the parameter axis.", 
                                 "axis");

          System.Drawing.Rectangle rect = viewPort.ImagePart;
          rect.X = (int)Math.Round(ImgCol1);
          rect.Y = (int)Math.Round(ImgRow1);
          viewPort.ImagePart = rect;
        }




        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void resetAll()
        {

          resetWindow();
          if (roiManager != null)
	          roiManager.reset();
        }


        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values.
        /// </summary>
        public void resetWindow()
        {
          System.Drawing.Rectangle newImagePart;

          ImgRow1 = 0;
          ImgCol1 = 0;
          ImgRow2 = heightImagePart;
          ImgCol2 = widthImagePart;

          zoomWndFactor = (double)widthImagePart / viewPort.Width;

          // set the image part to the actual image size 
          if ((heightImagePart > 0) && (widthImagePart > 0))
          {
            newImagePart   = viewPort.ImagePart;
            newImagePart.X = (int)ImgCol1;
            newImagePart.Y = (int)ImgRow1;
            newImagePart.Width = (int)widthImagePart;
            newImagePart.Height = (int)heightImagePart;
          }
          // if no image is displayed, set image part to 
          // the size of the display 
          else
          {
            newImagePart = viewPort.ImagePart;
            newImagePart.X = (int)0;
            newImagePart.Y = (int)0;
            newImagePart.Width = (int)viewPort.WindowSize.Width;
            newImagePart.Height = (int)viewPort.WindowSize.Height;
          }

          viewPort.ImagePart = newImagePart;
        }

       

        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
          mousePressed = true;
          int activeROIidx = -1;
          double scale;

          if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
          {
            activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
          }

          if (activeROIidx == -1)
          {
            switch (stateView)
            {
              case MODE_VIEW_MOVE:
                startX = e.X;
                startY = e.Y;
                break;
              case MODE_VIEW_ZOOM:
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                  scale = 0.9;
                else
                  scale = 1 / 0.9;
                zoomImage(e.X, e.Y, scale);
                break;
              case MODE_VIEW_NONE:
                break;
              case MODE_VIEW_ZOOMWINDOW:
                activateZoomWindow((int)e.X, (int)e.Y);
                break;
              default:
                break;
            }
          }
          //end of if
        }

        public void mouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {          
          if (e.Delta > 0)
            zoomImage(e.X, e.Y, 0.9);
          else
            zoomImage(e.X, e.Y, 1.1);
        }


        /*******************************************************************/
        /*******************************************************************/
        private void activateZoomWindow(int X, int Y)
        {
          double posX, posY;
          int zoomZone;

          if (ZoomWindow != null)
            ZoomWindow.Dispose();

          HOperatorSet.SetSystem("border_width", 10);
          ZoomWindow = new HWindow();

          posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
          posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

          zoomZone = (int)((zoomWndSize / 2) * zoomWndFactor * zoomAddOn);
          ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
					              zoomWndSize, zoomWndSize,
					              viewPort.HalconID, "visible", "");
          ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
          repaintUsingFlushGraphic(ZoomWindow);
          ZoomWindow.SetColor("black");
        }


        /*******************************************************************/
        private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
          mousePressed = false;

          if (roiManager != null
	          && (roiManager.activeROIidx != -1)
	          && (dispROI == MODE_INCLUDE_ROI))
          {
            roiManager.NotifyRCObserver(ROIController.EVENT_UPDATE_ROI);
          }
          else if (stateView == MODE_VIEW_ZOOMWINDOW)
          {
            ZoomWindow.Dispose();
          }
        }


        /*******************************************************************/
        private void mouseMoved(object sender, HalconDotNet.HMouseEventArgs e)
        {
          double motionX, motionY;
          double posX, posY;
          double zoomZone;

          if (!mousePressed)
          {
            return;
          }

          if (roiManager != null && (roiManager.activeROIidx != -1) && (dispROI == MODE_INCLUDE_ROI))
          {
            roiManager.mouseMoveAction(e.X, e.Y);
          }
          else if (stateView == MODE_VIEW_MOVE)
          {
            motionX = ((e.X - startX));
            motionY = ((e.Y - startY));

            if (((int)motionX != 0) || ((int)motionY != 0))
            {
              moveImage(motionX, motionY);
              startX = e.X - motionX;
              startY = e.Y - motionY;
              if (OnImageMoved != null)
                  OnImageMoved(this, motionX, motionY);
            }
          }
          else if (stateView == MODE_VIEW_ZOOMWINDOW)
          {
            HSystem.SetSystem("flush_graphic", "false");
            ZoomWindow.ClearWindow();


            posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
            posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
            zoomZone = (zoomWndSize / 2) * zoomWndFactor * zoomAddOn;

            ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
							            (int)posX - (zoomWndSize / 2),
							            zoomWndSize, zoomWndSize);
            ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
				                (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
            repaintUsingFlushGraphic(ZoomWindow);

            HSystem.SetSystem("flush_graphic", "true");
            ZoomWindow.DispLine(-1000.0, -1000.0, -1000.0, -1000.0);
          }
        }


        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrl
        /// first needs to know the range supplied by the GUI component. 
        /// For the x direction it is specified by xRange, which is 
        /// calculated as follows: GuiComponentX.Max()-GuiComponentX.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter Init
        /// </summary>
        public void setGUICompRangeX(int[] xRange, int Init)
        {
          int cRangeX;

          CompRangeX = xRange;
          cRangeX = xRange[1] - xRange[0];
          prevCompX = Init;
          initValX = Init;
          //
          if (widthImagePart > 0)
            stepSizeX = ((double)widthImagePart / cRangeX) * (1.0 * widthImagePart / windowWidth);
          else
            stepSizeX = (double)windowWidth /(1.0 * cRangeX);

        }

        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrl
        /// first needs to know the range supplied by the GUI component. 
        /// For the y direction it is specified by yRange, which is 
        /// calculated as follows: GuiComponentY.Max()-GuiComponentY.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter Init
        /// </summary>
        public void setGUICompRangeY(int[] yRange, int Init)
        {
          int cRangeY;

          CompRangeY = yRange;
          cRangeY = yRange[1] - yRange[0];
          prevCompY = Init;
          initValY = Init;
          if (heightImagePart > 0)
            stepSizeY = ((double)heightImagePart / cRangeY) * (1.0 * heightImagePart / windowHeight);
          else
            stepSizeY = (double)windowWidth / (1.0 * cRangeY);
        }


        /// <summary>
        /// Resets to the starting value of the GUI component.
        /// </summary>
        public void resetGUIInitValues(int xVal, int yVal)
        {
          prevCompX = xVal;
          prevCompY = yVal;
        }

        /// <summary>
        /// Moves the image by the value valX supplied by the GUI component
        /// </summary>
        public void moveXByGUIHandle(int valX)
        {
          double motionX;

          motionX = valX * stepSizeX;

          //moveImageFromInit(motionX, "x");
          moveImage(motionX, 0);
          prevCompX = valX;
        }

        /// <summary>
        /// Moves the image by the value valY supplied by the GUI component
        /// </summary>
        public void moveYByGUIHandle(int valY)
        {
	      double motionY;
			
          motionY = valY  * stepSizeY;
          //moveImageFromInit(motionY, "y");
          moveImage(0, motionY);
          prevCompY = valY;
        }

        /// <summary>
        /// Zooms the image by the value valF supplied by the GUI component
        /// </summary>
        public void zoomByGUIHandle(double valF)
        {
          double x, y, scale;
          double prevScaleC;


          x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
          y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);


          prevScaleC = (double)((ImgCol2 - ImgCol1) / widthImagePart);
          scale = ((double)1.0 / prevScaleC * (100.0 / valF));

          zoomImage(x, y, scale);
        }

        /// <summary>
        /// Zooms the image by the value valF supplied by the GUI component
        /// </summary>
        public void zoomByGUIHandleInitial(double valF, double prevScaleC)
        {
          double x, y, scale;
          //double prevScaleC;


          x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
          y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);

          //prevScaleC = (double)((ImgCol2 - ImgCol1) / imageWidth);
          scale = ((double)1.0 / prevScaleC * (100.0 / valF));

          zoomImage(x, y, scale);
        }
      

        /// <summary>
        /// Triggers a repaint of the HALCON window
        /// </summary>
        public void repaint()
        {
          repaintUsingFlushGraphic(viewPort.HalconWindow);
        }

        /// <summary>
        /// Repaints the HALCON window 'window' using BufferWindow
        /// </summary>
        public void repaintUsingBufferWindow(HalconDotNet.HWindow window)
        {
	      int count = HObjList.Count;
	      HObjectEntry entry;

          // modified
          if ((windowWidth > 0) && (windowHeight > 0))
          {
              if (bufferWindow == null)
              {
                  bufferWindow = new HWindow(0, 0, windowWidth, windowHeight, "root", "buffer", "");
                  // check the boundatries of the image part
                  if ((ImgRow2 > ImgRow1) && (ImgCol2 > ImgCol1))
                      bufferWindow.SetPart((int)ImgRow1, (int)ImgCol1, (int)ImgRow2, (int)ImgCol2);
                  // set the image part to window size
                  else
                      bufferWindow.SetPart(0, 0, windowHeight - 1, windowWidth - 1);
              }
              else
                  bufferWindow.ClearWindow();

            mGC.stateOfSettings.Clear();

            // display the graphic stack on the buffer window
            for (int i = 0; i < count; i++)
            {
              entry = ((HObjectEntry)HObjList[i]);
              if (entry != null)
              {
                  mGC.applyContext(window, entry.gContext);
                  mGC.applyContext(bufferWindow, entry.gContext);
                  bufferWindow.DispObj(entry.HObj);
              }
            }

            addInfoDelegate();

            // if any rois exists add this to buffer window
            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
              roiManager.paintData(bufferWindow);

            // copy the content of buffer window to HWindow
            bufferWindow.CopyRectangle(window, 0, 0, windowHeight - 1, windowWidth - 1, 0, 0);

            // dispose the buffer window
            bufferWindow.Dispose();
            bufferWindow = null;
          }

        }

        /// <summary>
        /// Repaints the HALCON window 'window'
        /// </summary>
        public void repaintUsingFlushGraphic(HalconDotNet.HWindow window)
        {
          int count = HObjList.Count;
          HObjectEntry entry;

          if (window != null)
          {
            HSystem.SetSystem("flush_graphic", "false");
            window.ClearWindow();
            // check the boundatries of the image part
            if ((ImgRow2 > ImgRow1) && (ImgCol2 > ImgCol1))
              window.SetPart((int)ImgRow1, (int)ImgCol1, (int)ImgRow2, (int)ImgCol2);
            // set the image part to window size
            else
              window.SetPart(0, 0, windowHeight - 1, windowWidth - 1);
            mGC.stateOfSettings.Clear();

            for (int i = 0; i < count; i++)
            {
              entry = ((HObjectEntry)HObjList[i]);
              if (entry != null)
              {
                  mGC.applyContext(window, entry.gContext);
                  window.DispObj(entry.HObj);
              }
            }

            addInfoDelegate();

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
              roiManager.paintData(window);

            HSystem.SetSystem("flush_graphic", "true");

            window.DispLine(-1000.0, -1000.0, -1001.0, -1001.0);
          }
        }



        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/
        /// <summary>
        /// Adds an iconic object to the graphics stack similar to the way
        /// it is defined for the HDevelop graphics stack.
        /// </summary>
        /// <param name="obj">Iconic object</param>
        public void addIconicVar(HObject obj)
        {
          HObjectEntry entry;

          if (obj == null)
	          return;

          if (obj is HImage)
          {
            double r, c;
            int h, w, area;
            string s;

            area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
            ((HImage)obj).GetImagePointer1(out s, out w, out h);

            if (area == (w * h))
            {
              clearList();
              // MODIFIED
              if ((h != heightImagePart) || (w != widthImagePart))
              {
                heightImagePart = h;
                widthImagePart = w;                  
                resetImagePart(widthImagePart, heightImagePart);                        
              }
	    }//if
          }//if

          entry = new HObjectEntry(obj, mGC.copyContextList());

          HObjList.Add(entry);

          if (HObjList.Count > MAXNUMOBJLIST)
	          HObjList.RemoveAt(1);
        }


        /********************************************************************/
        /*                      GRAPHICSSTACK     -KEEP                     */
        /********************************************************************/
        /// <summary>
        /// Adds an iconic object to the graphics stack similar to the way
        /// it is defined for the HDevelop graphics stack.
        /// </summary>
        /// <param name="obj">Iconic object</param>
        public void addIconicVarKeepSettings(HObject obj)
        {
          HObjectEntry entry;

          if (obj == null)
            return;

          if ((obj is HImage) && (obj != null))
          {
            double r, c;
            int h, w, area;
            string s;

            area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
            ((HImage)obj).GetImagePointer1(out s, out w, out h);

            if (HObjList.Count == 0)
            {
              heightImagePart = h;
              widthImagePart = w;
              //adaptSize = true;

              zoomWndFactor = (double)widthDisplayedImage / viewPort.Width;

              resetImagePart(widthImagePart, heightImagePart);
            }

            if (area == (w * h))
            {
              clearList();

              if ((h != heightImagePart) || (w != widthImagePart))
              {
                heightImagePart = h;
                widthImagePart  = w;
                
                if ((widthDisplayedImage != w) || (heightDisplayedImage != h))
                  resetImagePart(w, h);

                zoomWndFactor = (double)widthDisplayedImage / viewPort.Width;
              }            
            }//if
            widthDisplayedImage  = w;
            heightDisplayedImage = h;
          }//if
          entry = new HObjectEntry(obj, mGC.copyContextList());


          HObjList.Add(entry);

          if (HObjList.Count > MAXNUMOBJLIST)
            HObjList.RemoveAt(1);
        }


        /// <summary>
        /// Set the displayed image part of the 
        /// </summary>
        public void resetImagePart(int imageWidth0, int imageHeight0)
        {
          getWindowSize();
          if (adaptSize == true)
          {
            setImagePartAdaptSize(imageWidth0, imageHeight0);
          }
          else
          {
            setImagePartConst(imageWidth0, imageHeight0);
          }
        }

        // MODIFIED

        /// <summary>
        /// calculate and set image part according new dimensions of HalconWindow
        /// (the image is displayed with correct aspect ratio and one side is scaled to the 
        /// corresponding window size)
        /// </summary>
        /// <param name="imageWidth0"></param>
        /// <param name="imageHeight0"></param>
        public void setImagePartAdaptSize(int imWidth, int imHeight)
        {
            //
            // fit to best size with aspect ratio 1:1
            //
            int r1, r2, c1, c2;

            // The image part is calculated, so the displayed image is adapted
            // to the window size, but the aspect ratio of the image
            // is kept. 
            // The following condition decides, if the height or width
            // of the displayed image is adapted to window height/width and 
            // the value of width/height will be calculated and adapted 
            // so the image is displayed in correct aspect ratio.
            if ((windowWidth == 0) || (windowHeight == 0))
                return;

            if (1.0 * imWidth / imHeight < 1.0 * windowWidth / windowHeight)
            {
                // scale imageHeight to windowHeight
                c1 = (int)(-0.5 * (1.0 * imHeight * windowWidth / windowHeight - imWidth));
                c2 = (int)(imWidth + Math.Abs(c1)-1);
                r1 = 0;
                r2 = imHeight - 1;
            }
            else
            {
                // scale imageWidth to windowWidth
                r1 = (int)(-0.5 * (1.0 * imWidth * windowHeight / windowWidth - imHeight));
                r2 = (int)(imHeight + Math.Abs(r1)-1);
                c1 = 0;
                c2 = imWidth - 1;
            }

            // Check, if the values of image part coordinates
            // have valid values
            if (c2 < c1)
            {
                c1 = 0;
                c2 = imWidth;
            }
            if (r2 < r1)
            {
                r1 = 0;
                r2 = imHeight;
            }

            // adapt the current image part to the 
            // new size of window
            System.Drawing.Rectangle rect = viewPort.ImagePart;

            rect.X = c1;
            rect.Y = r1;

            rect.Width = c2 - c1+1;
            rect.Height = r2 - r1 + 1;

            InitImgRow1 = ImgRow1 = rect.Y;
            InitImgCol1 = ImgCol1 = rect.X;
            InitImgRow2 = ImgRow2 = rect.Y + rect.Height;
            InitImgCol2 = ImgCol2 = rect.X + rect.Width;
 

            viewPort.ImagePart = rect;

            heightImagePart = rect.Height;
            widthImagePart = rect.Width;

            zoomWndFactor = (double)widthImagePart / viewPort.Width;
            zoomImageFactor = 100.0;

        }

        // MODIFIED

        /// <summary>
        /// The image part is set so that the image size remains constant 
        /// independent from the window size (image is not scaled)
        /// </summary>
        /// <param name="imageWidth0"></param>
        /// <param name="imageHeight0"></param>
        public void setImagePartConst(int imWidth, int imHeight)
        {
            //
            // fit to best size with aspect ratio 1:1
            //
            int r1, r2, c1, c2;

            /*calculate the coordinates for displaying the image 
             * centered in the Halcon window*/
            c1 = (int)(0.5 * (imWidth - windowWidth));
            c2 = imWidth - c1;
            r1 = (int)(0.5 * (imHeight - windowHeight));
            r2 = imHeight - r1;


            // Check, if the values of image part coordinates
            // have valid values
            if (c2 <= c1)
            {
                c1 = 0;
                c2 = imWidth;
            }
            if (r2 <= r1)
            {
                r1 = 0;
                r2 = imHeight;
            }


            /* Assign the calculated values to image part of Halcon window*/
            System.Drawing.Rectangle rect = viewPort.ImagePart;

            rect.X = c1;
            rect.Y = r1;
            rect.Width = c2 - c1;
            rect.Height = r2 - r1 + 1;

            // set the image part values to member variables
            InitImgRow1 = ImgRow1 = rect.Y;
            InitImgCol1 = ImgCol1 = rect.X;
            InitImgRow2 = ImgRow2 = rect.Y + rect.Height;
            InitImgCol2 = ImgCol2 = rect.X + rect.Width;

            viewPort.ImagePart = rect;

            heightImagePart = rect.Height;
            widthImagePart = rect.Width;

            zoomWndFactor = (double)widthImagePart / viewPort.Width;
        }

        /// <summary>
        /// Clears all entries from the graphics stack 
        /// </summary>
        public void clearList()
        {
	        HObjList.Clear();
        }


        /// <summary>
        /// Returns the number of items on the graphics stack
        /// </summary>
        public int getListCount()
        {
	  return HObjList.Count;
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g., GraphicsContext.GC_COLOR
        /// </param>
        /// <param name="val">
        /// Value, provided as a string, 
        /// the mode is to be changed to, e.g., "blue" 
        /// </param>
        public void changeGraphicSettings(string mode, string val)
        {
          switch (mode)
          {
            case GraphicsContext.GC_COLOR:
              mGC.setColorAttribute(val);
              break;
            case GraphicsContext.GC_DRAWMODE:
              mGC.setDrawModeAttribute(val);
              break;
            case GraphicsContext.GC_LUT:
              mGC.setLutAttribute(val);
              break;
            case GraphicsContext.GC_PAINT:
              mGC.setPaintAttribute(val);
              break;
            case GraphicsContext.GC_SHAPE:
              mGC.setShapeAttribute(val);
              break;
            default:
              break;
          }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g., GraphicsContext.GC_LINEWIDTH
        /// </param>
        /// <param name="val">
        /// Value, provided as an integer, the mode is to be changed to, 
        /// e.g., 5 
        /// </param>
        public void changeGraphicSettings(string mode, int val)
        {
          switch (mode)
          {
            case GraphicsContext.GC_COLORED:
              mGC.setColoredAttribute(val);
              break;
            case GraphicsContext.GC_LINEWIDTH:
              mGC.setLineWidthAttribute(val);
              break;
            default:
              break;
          }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g.,  GraphicsContext.GC_LINESTYLE
        /// </param>
        /// <param name="val">
        /// Value, provided as an HTuple instance, the mode is 
        /// to be changed to, e.g., new HTuple(new int[]{2,2})
        /// </param>
        public void changeGraphicSettings(string mode, HTuple val)
        {
          switch (mode)
          {
            case GraphicsContext.GC_LINESTYLE:
              mGC.setLineStyleAttribute(val);
              break;
            default:
              break;
          }
        }

        /// <summary>
        /// Clears all entries from the graphical context list
        /// </summary>
        public void clearGraphicContext()
        {
          mGC.clear();
        }

        /// <summary>
        /// Returns a clone of the graphical context list (hashtable)
        /// </summary>
        public Hashtable getGraphicContext()
        {
          return mGC.copyContextList();
        }

  }//end of class
}//end of namespace

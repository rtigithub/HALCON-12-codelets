using System;
using System.Collections;
using HalconDotNet;


namespace CalibrationModule
{
    public delegate void CalibDelegate(int value);   

    /// <summary>
    /// The calibration assistant serves as a controller class  
    /// that coordinates the calibration process and maintains the
    /// communication between the GUI and the calibration data.
    /// 
    /// It keeps the set of calibration images <c>CalibData</c> updated and 
    /// tracks the calibration parameters. A change in the calibration 
    /// parameters provokes an update in the entire set of 
    /// <c>CalibImage</c> objects. 
    /// Changes in the images are then forwarded to the GUI by
    /// using delegates. Besides changes concerning the graphics window,
    /// also exceptions and errors occurring during processing get
    /// forwarded. 
	/// Note that only calibration plates with rectangularly arranged
	/// marks are supported for the Calibration Codelet. The new 
	/// HALCON calibration plates with hexagonally arranged marks are 
	/// not supported.
    /// </summary>
    public class CalibrationAssistant
	{
        /// <summary>
        /// Constant indicating the camera model to be of type 
        /// line scan camera.
        /// </summary>
        public const int  CAMERA_TYP_LINE_SCAN           = 3;
        /// <summary>
        /// Constant indicating the camera model to be of type 
        /// area scan camera with the division 
        /// model to describe the lens distortions
        /// </summary>
        public const int  CAMERA_TYP_AREA_SCAN_DIV       = 4;
        /// <summary>
        /// Constant indicating the camera model to be of type
        /// area scan camera with the polynomial 
        /// model to describe the lens distortions
        /// </summary>
        public const int  CAMERA_TYP_AREA_SCAN_POLY      = 5;

        /// <summary>
        /// Constant indicating a change in the set of 
        /// <c>CalibImage</c> instances regarding the parameters 
        /// marks and poses.
        /// </summary>
        public const int  UPDATE_MARKS_POSE  	         = 10;
        /// <summary>
        /// Constant indicating a change in the set of
        /// <c>CalibImage</c> instances regarding the 
        /// quality assessment.
        /// </summary>
        public const int  UPDATE_QUALITY_TABLE           = 12;
        /// <summary>
        /// Constant indicating a change in the evaluation grades 
        /// of the <c>CalibImage</c> set. The grades measure the results 
        /// of the calibration preparations prior to the calibration
        /// process itself.
        /// </summary>
        public const int  UPDATE_CALTAB_STATUS           = 13;
        /// <summary>
        /// Constant indicating an update of the calibration
        /// results.
        /// </summary>
        public const int  UPDATE_CALIBRATION_RESULTS     = 14;

        /// <summary>
        /// Constant indicating that the quality measurement
        /// includes all quality assessment operators for 
        /// evaluating the set of calibration images.
        /// </summary>
        public const int  QUALITY_ISSUE_TEST_ALL         = 0;
        /// <summary>
        /// Constant indicating that the quality measurement uses only the
        /// basic quality assessment operators for speedup purposes.
        /// </summary>
        public const int  QUALITY_ISSUE_TEST_QUICK       = 1;
        /// <summary>
        /// Constant indicating no quality measurement 
        /// for the set of calibration images.
        /// </summary>
        public const int  QUALITY_ISSUE_TEST_NONE        = 2;
        
        /// <summary>
        /// Constant indicating the image quality feature 'exposure'
        /// </summary>
        public const int  QUALITY_ISSUE_IMG_EXPOSURE     = 20;
        /// <summary>
        /// Constant indicating the image quality feature 'homogeneity'
        /// </summary>
        public const int  QUALITY_ISSUE_IMG_HOMOGENEITY  = 21;
        /// <summary>
        /// Constant indicating the image quality feature 'contrast'
        /// </summary>
        public const int  QUALITY_ISSUE_IMG_CONTRAST     = 22;
        /// <summary>
        /// Constant indicating the image quality feature 'sharpness'
        /// </summary>
        public const int  QUALITY_ISSUE_IMG_FOCUS        = 23;
        /// <summary>
        /// Constant indicating the size of the depicted  
        /// calibration plate in the calibration image, to evaluate
        /// the distance used to the camera
        /// </summary>
        public const int  QUALITY_ISSUE_IMG_CALTAB_SIZE  = 24;
        
        /// <summary>
        /// Constant indicating the coverage of the view field.
        /// This is assured only by a sufficient number of 
        /// calibration images and its correct distribution in the space.
        /// </summary>
        public const int  QUALITY_ISSUE_SEQ_MARKS_DISTR  = 25;
        /// <summary>
        /// Constant indicating the amount of distortions covered,
        /// described by the set of images showing tilted calibration
        /// plates
        /// </summary>
        public const int  QUALITY_ISSUE_SEQ_CALTAB_TILT  = 26;
        /// <summary>
        /// Constant indicating whether the number of provided calibration
        /// images is sufficient enough to obtain stable calibration results.
        /// </summary>
        public const int  QUALITY_ISSUE_SEQ_NUMBER       = 27;
        /// <summary>
        /// Constant indicating the all over quality performance,
        /// being best for a value close or equal to 1
        /// </summary>
        public const int  QUALITY_ISSUE_SEQ_ALL_OVER     = 28; 
        /// <summary>
        /// Constant that indicates an error in the calibration
        /// preprocessing step, which has to perform well for the
        /// whole sequence of calibration images in order to start 
        /// the calibration process. 
        /// </summary>
        public const int  QUALITY_ISSUE_SEQ_ERROR        = 29; 
        /// <summary>
        /// Constant indicating an error in the preprocessing step 
        /// for a single calibration image, i.e., that the
        /// marks and pose values might be missing or the region 
        /// plate couldn't be detected
        /// </summary>
        public const int  QUALITY_ISSUE_FAILURE          = 30;

        /// <summary>
        /// Constant describing an error while reading a 
        /// calibration image from file
        /// </summary>
        public const int  ERR_READING_FILE               = 31;
        /// <summary>
        /// Constant describing an error exception raised during
        /// the calibration process
        /// </summary>
        public const int  ERR_IN_CALIBRATION             = 32; 
        /// <summary>
        /// Constant indicating an invalid reference index. The
        /// index is needed to define the reference image for
        /// the camera calibration.
        /// </summary>
        public const int  ERR_REFINDEX_INVALID           = 33;
        /// <summary>
        /// Constant describing an error exception raised 
        /// during quality assessment.
        /// </summary>
        public const int  ERR_QUALITY_ISSUES             = 34;
        /// <summary>
        /// Constant describing an error that occurred while
        /// writing the calibration parameters into file
        /// </summary>
        public const int  ERR_WRITE_CALIB_RESULTS        = 35;
        
        
        /// <summary>
        /// Constant indicating the result status of the 
        /// calibration preparation step:
        /// Plate region couldn't be detected in the 
        /// calibration image.
        /// </summary>
        public const string PS_NOT_FOUND        = "Plate not found";
        /// <summary>
        /// Constant that describes the results of the 
        /// calibration preparation step:
        /// Plate region was detected, but the marks could not
        /// be extracted in the plate region.
        /// </summary>
        public const string PS_MARKS_FAILED     = "Marks not found";
        /// <summary>
        /// Constant indicating the result status of the 
        /// calibration preparation step:
        /// Plate region and marks were detected, 
        /// but the quality assessment delivered bad scores.
        /// </summary>
        public const string PS_QUALITY_ISSUES   = "Quality issues detected";
        /// <summary>
        /// Constant indicating the result status of the 
        /// calibration preparation step:
        /// The preprocessing step was successful.
        /// </summary>
        public const string PS_OK               = "Ok";
 
        /// <summary>
        /// List of calibration images that are used 
        /// to perform the camera calibration. 
        /// </summary>
        private ArrayList CalibData;
        /// <summary>
        /// Index to the reference image that is used to
        /// determine the initial values for the internal camera 
        /// parameters for the camera calibration
        /// </summary>
        public  int       mReferenceIndex;

        private QualityProcedures procedure;


        // CALIBRATION RESULTS -----------------------------------------
        /// <summary>
        /// Flag indicating that the calibration was successful and
        /// the present calibration results are up to date
        /// </summary>
        public  bool      mCalibValid;
        /// <summary>
        /// The average error give an impression of the accuracy of the
        /// calibration. The error (deviations in x and y coordinates) are 
        /// measured in pixels
        /// </summary>
        public  double    mErrorMean;
        
        /// <summary>
        /// Ordered tuple with the external camera parameters for all
        /// calibration images, i.e., the position and orientation of the
        /// calibration plate in camera coordinates.
        /// </summary>
        public  HTuple    mPoses;
        /// <summary>
        /// Internal camera parameters
        /// </summary>
        public  HTuple    mCameraParams;
        /// <summary>
        /// Error contents that caused an exception 
        /// </summary>
        public  string    mErrorMessage;

        /// <summary>
        /// Calibration image at index <c>mReferenceIndex</c>
        /// </summary>
        public  HImage   mReferenceImage;
        /// <summary>
        /// Synthetic calibration images with calibrated camera 
        /// parameters to test the quality of the calibration 
        /// algorithm
        /// </summary>
        public  HImage   mSimulatedImage;
        /// <summary>
        ///  Reference world coordinate system, based on 
        ///  <c>mPose</c> and the calibrated camera parameters 
        /// </summary>
        public  HObject   mReferenceWCS;

        /// <summary>
        /// Flag describing whether all calibration images
        /// have a sufficient quality, i.e. whether  the region plate and
        /// marks have been detected in all calibration images,
        /// so that a camera calibration can be invoked
        /// </summary>
        public  bool      mCanCalib;
        /// <summary>
        /// Flag indicating that the origin of the reference world coordinate
        /// system <c>mReferenceWCS</c> is mapped to the origin of the image
        /// coordinate system.
        /// </summary>
        public  bool      mAtImgCoord;
        

        // FIRST TAB  -----------------------------------------------
        /// <summary>
        /// Name of the calibration plate description file to read
        /// the mark center points from
        /// </summary>
        public  string    mDescrFileName;
        /// <summary>
        /// Thickness of the calibration plate that was used in the 
        /// calibration images
        /// </summary>
        public  double    mThickness;
        /// <summary>
        /// Camera type, which can either be an area scan camera 
        /// (using the division or polynomial model) or a linescan camera
        /// </summary>
        public  int       mCameraType;
        /// <summary>
        /// Horizontal distance between two neighboring CCD 
        /// sensor cells 
        /// </summary>
        public  double    mCellWidth;    // Sx 
        /// <summary>
        /// Vertical distance between two neighboring CCD 
        /// sensor cells 
        /// </summary>
        public  double    mCellHeight;   // Sy 
        /// <summary>
        /// Nominal focal length of the camera lense
        /// </summary>
        public  double    mFocalLength; 
        /// <summary>
        /// Parameter to model the radial distortion described by 
        /// the division model
        /// </summary>
        public  double    mKappa;  
        /// <summary>
        /// First parameter to model the radial distortion described by 
        /// the polynomial model
        /// </summary>
        public  double    mK1;
        /// <summary>
        /// Second parameter to model the radial distortion described by 
        /// the polynomial model
        /// </summary>
        public  double    mK2;
        /// <summary>
        /// Third parameter to model the radial distortion described by 
        /// the polynomial model
        /// </summary>
        public  double    mK3;
        /// <summary>
        /// First parameter to model the decentering distortion described by 
        /// the polynomial model
        /// </summary>
        public  double    mP1;
        /// <summary>
        /// Second parameter to model the decentering distortion described by 
        /// the polynomial model
        /// </summary>
        public  double    mP2;
        /// <summary>
        /// Flag indicating the type of camera lense used:
        /// telecentric, which means a parallel projection with the focal 
        /// length equal to 0, or  a perspective projection
        /// </summary>
        public  bool      isTelecentric;

        /// <summary>
        /// X component of the motion vector, which describes the motion
        /// between the linescan camera and the object.
        /// </summary>
        public double     mMotionVx;
        /// <summary>
        /// Y component of the motion vector, which describes the motion
        /// between the linescan camera and the object.
        /// </summary>
        public double     mMotionVy;
        /// <summary>
        /// Z component of the motion vector, which describes the motion
        /// between the linescan camera and the object.
        /// </summary>
        public double     mMotionVz;

        // SECOND TAB ---------------------------------------------
        private int       mWarnLevel;
        private int       mImageTests;
        private int       mSequenceTests;

        /// <summary>
        /// List of quality assessment scores of the whole set of calibration
        /// images.
        /// </summary>
        public ArrayList mSeqQualityList;

        /// <summary>
        /// Size of the filter mask that is used to smooth the 
        /// image before determining the region plate in the
        /// calibration image
        /// </summary>
        public  double    mFilterSize;
        /// <summary>
        /// Threshold value for mark extraction
        /// </summary>
        public  double    mMarkThresh;
        /// <summary>
        /// Expected minimum diameter of the marks on the 
        /// calibration plate
        /// </summary>
        public  double    mMinMarkDiam;
        /// <summary>
        /// Initial threshold value for contour detection
        /// </summary>
        public  double    mInitThresh;
        /// <summary>
        /// Loop value for successive reduction of 
        /// the initial threshold <c>mInitThresh</c>
        /// </summary>
        public  double    mThreshDecr;
        /// <summary>
        /// Minimum threshold for contour detection
        /// </summary>
        public  double    mMinThresh;
        /// <summary>
        /// Filter parameter for contour detection
        /// </summary>
        public  double    mSmoothing;
        /// <summary>
        /// Minimum length of the contours of the marks
        /// </summary>
        public  double    mMinContLength;
        /// <summary>
        /// Maximum expected diameter of the marks
        /// </summary>
        public  double    mMaxMarkDiam;
        
        // reset vals 
        public int     resetFilterSize  = 3;
        public int     resetMarkThresh  = 112;
        public int     resetMinMarkDiam = 5;
        public int     resetInitThresh  = 128;
        public int     resetThreshDecr  = 10;
        public int     resetMinThresh   = 18;
        public double  resetSmoothing   = 0.9; /* 90*0.01 */
        public int     resetMinContL    = 15;
        public int     resetMaxMarkDiam = 100;

        /// <summary>
        /// Delegate to notify the GUI about changes in the data models
        /// </summary>
        public CalibDelegate   NotifyCalibObserver;

        
      /* Constructor, in which all calibration parameters  
       * and  auxiliary variables, flags and lists are initilized */
		public CalibrationAssistant()
		{
            CalibData       = new ArrayList(15); 
            mReferenceIndex = -1;      
            mDescrFileName  = "caltab_30mm.descr";
            mCalibValid     = false;  
            mCanCalib       = true;
            mAtImgCoord     = false;

            mReferenceImage = new HImage();
            mSimulatedImage = new HImage();

            mFilterSize    = resetFilterSize;
            mMarkThresh    = resetMarkThresh;
            mMinMarkDiam   = resetMinMarkDiam;
            mInitThresh    = resetInitThresh;
            mThreshDecr    = resetThreshDecr;
            mMinThresh     = resetMinThresh;
            mSmoothing     = resetSmoothing;
            mMinContLength = resetMinContL;
            mMaxMarkDiam   = resetMaxMarkDiam;

            mWarnLevel      = 70;
            mImageTests     = QUALITY_ISSUE_TEST_ALL;
            mSequenceTests  = QUALITY_ISSUE_TEST_ALL;
            mSeqQualityList = new ArrayList(15);
            procedure       = new QualityProcedures();
            
            
            mThickness     = 1.00;       // millimeter
            mCameraType    = CAMERA_TYP_AREA_SCAN_DIV;
            mCellWidth     = 8.300;         // micrometer
            mCellHeight    = 8.300;        // micrometer
            mFocalLength   = 8.000;          // millimeter
            isTelecentric  = false;
            mKappa         = 0.0;   
            mK1            = 0.0;
            mK2            = 0.0;
            mK3            = 0.0;
            mP1            = 0.0;
            mP2            = 0.0;
            mMotionVx      = 0.0;
            mMotionVy      = 500.0;
            mMotionVz      = 0.0;
            
            NotifyCalibObserver = new CalibDelegate(dummy);
        }

        /*******************************************/
        /**********         Tab.1       ************/
        /*******************************************/

        /// <summary>
        /// Sets the reference index to the supplied value
        /// </summary>
        public void setReferenceIdx(int val)
        {
            mReferenceIndex = val;
        }
       
        /*******************************************/

        /// <summary>
        /// Sets the file path for the description file to 
        /// the supplied value
        /// </summary>
        /// <param name="fileName">
        /// Absolute path to the description file
        /// </param>
        public void setDesrcFile(string fileName)
        {
            mDescrFileName = fileName;
            Update();
        }
        /// <summary>
        /// Gets the path to the description file used
        /// with the calibration
        /// </summary>
        public string getDesrcFile()
        {
            return mDescrFileName;
        }
     
        /*******************************************/

        /// <summary>
        /// Sets the thickness parameter to the supplied value
        /// </summary>
        public void setThickness(double val)
        {
            mThickness = val;
            UpdateResultVisualization();
        }
        /// <summary>
        /// Returns the current value for the thickness of the calibration
        /// plate.
        /// </summary>
        public double getThickness()
        {
            return mThickness;
        }
     
        /*******************************************/

        /// <summary>
        /// Sets the camera type to the supplied value and 
        /// invokes an update to adjust data for the calibration images.
        /// 
        /// </summary>
        public void setCameraType(int mode)
        {
            mCameraType = mode;
            Update(true); 
        }
        /// <summary>
        /// Returns the current camera type.
        /// </summary>
        public int getCameraType()
        {
            return mCameraType;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for the CCD sensor cell width
        /// to the supplied value.
        /// </summary>
        public void setCellWidth(double val)
        {
            mCellWidth = val;
            Update();
        }
        /// <summary>
        /// Returns the value for the CCD sensor cell width
        /// </summary>
        public double getCellWidth()
        {
            return mCellWidth;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for the CCD sensor cell height
        /// to the supplied value.
        /// </summary>
        public void setCellHeight(double val)
        {
            mCellHeight = val;
            Update();
        }
        /// <summary>
        /// Returns the value for the CCD sensor cell height
        /// </summary>
        public double getCellHeight()
        {
            return mCellHeight;        
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for the focal length to the
        /// supplied value 
        /// </summary>
        public void setFocalLength(double val)
        {
            mFocalLength = val;
            Update();
        }
        /// <summary>
        /// Returns the current value for the focal length
        /// </summary>
        public double getFocalLength()
        {
            return mFocalLength;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the boolean flag to indicate the use of telecentric 
        /// lense
        /// </summary>
        public void setIsTelecentric(bool val)
        {
            isTelecentric = val;
            Update();
        }
        /// <summary>
        /// Returns whether the current camera lense is  defined to 
        /// be telecentric or not
        /// </summary>
        public bool IsTelecentric()
        {
            return isTelecentric;
        }

        /*******************************************/

        /// <summary>
        /// Sets the motion vector Vx to the supplied 
        /// value
        /// </summary>
        public void setMotionX(double val)
        {
            mMotionVx = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the motion vector Vx
        /// </summary>
        public double getMotionX()
        {
            return mMotionVx;
        }

        /*******************************************/

        /// <summary>
        /// Sets the motion vector Vy to the supplied 
        /// value 
        /// </summary>
        public void setMotionY(double val)
        {
            mMotionVy = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the motion vector Vy
        /// </summary>
        public double getMotionY()
        {
            return mMotionVy;
        }

        /*******************************************/

        /// <summary>
        /// Sets the motion vector Vz to the supplied 
        /// value 
        /// </summary>
        public void setMotionZ(double val)
        {
            mMotionVz = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the motion vector Vz
        /// </summary>
        public double getMotionZ()
        {
            return mMotionVz;
        }

        /*******************************************/
        /**********         Tab.2       ************/
        /*******************************************/
        /*******************************************/

        /// <summary>
        /// Sets the parameter for the warn level to 
        /// the supplied value.
        /// </summary>
        public void setWarnLevel(int val)
        {
            mWarnLevel = val;
            Update(true);
        }
        /// <summary>
        /// Gets the current value for the warn level
        /// </summary>
        public double getWarnLevel()
        {
            return mWarnLevel;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter to define the mode (accuracy) of
        /// the quality assessment for single
        /// calibration images
        /// </summary>
        /// <param name="mode">
        /// mode (accuracy) of the quality assessment; one of the constants
        /// starting with QUALITY_ISSUE_TEST_*.
        /// </param>
        public void setImageTests(int mode)
        {
            mImageTests = mode;
            Update(true);
        }
        /// <summary>
        /// Gets the current value that describes the
        /// accuracy of the quality assessment
        /// </summary>
        public double getImageTests()
        {
            return mImageTests;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter to define the mode (accuracy) of
        /// the quality assessment for the whole sequence
        /// of calibration images
        /// </summary>
        /// <param name="mode">
        /// mode (accuracy) of the quality assessment; one of the constants
        /// starting with QUALITY_ISSUE_TEST_*.
        /// </param>
        public void setSequenceTests(int mode)
        {
            mSequenceTests = mode;
            Update(true);
        }
        /// <summary>
        /// Gets the current value that describes the
        /// accuracy of the quality assessment
        /// </summary>
        public double getSequenceTests()
        {
            return mSequenceTests;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter for the filter size to
        /// the supplied value.
        /// </summary>
        public void setFilterSize(double val)
        {
            mFilterSize = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the filter size 
        /// </summary>
        public double getFilterSize()
        {
            return mFilterSize;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for mark threshold to
        /// the supplied value.
        /// </summary>
        public void setMarkThresh(double val)
        {
            mMarkThresh = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the mark threshold
        /// </summary>
        public double getMarkThresh()
        {
            return mMarkThresh;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for the minimum mark
        /// diameter to the supplied value.
        /// </summary>
        public void setMinMarkDiam(double val)
        {
            mMinMarkDiam = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the minimum mark diameter
        /// </summary>
        public double getMinMarkDiam()
        {
            return mMinMarkDiam;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter for the start threshold
        /// to the supplied value.
        /// </summary>
        public void setInitThresh(double val)
        {
            mInitThresh = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the start threshold
        /// </summary>
        public double getInitThresh()
        {
            return mInitThresh;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter for the threshold 
        /// decrement to the supplied value.
        /// </summary>
        public void setThreshDecr(double val)
        {
            mThreshDecr = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the threshold decrement
        /// </summary>
        public double getThreshDecr()
        {
            return mThreshDecr;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter for the minimum 
        /// threshold to the supplied value.
        /// </summary>
        public void setMinThresh(double val)
        {
            mMinThresh = val;
            Update();
        }
        /// <summary>
        /// Gets the current value for the parameter 
        /// describing the minimum threshold
        /// </summary>
        public double getMinThresh()
        {
            return mMinThresh;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter describing the smoothing
        /// factor to the supplied value.
        /// </summary>
        public void setSmoothing(double val)
        {
            mSmoothing = val;
            Update();
        }
        /// <summary>
        /// Gets the parameter describing the smoothing
        /// factor
        /// </summary>
        public double getSmoothing()
        {
            return mSmoothing;
        }
        
        /*******************************************/

        /// <summary>
        /// Sets the parameter describing the minimum
        /// contour length to the supplied value.
        /// </summary>
        public void setMinContLength(double val)
        {
            mMinContLength = val;
            Update();
        }
        /// <summary>
        /// Gets the parameter describing the minimum
        /// contour length
        /// </summary>
        public double getMinContLength()
        {
            return mMinContLength;
        }

        /*******************************************/

        /// <summary>
        /// Sets the parameter describing the maximum
        /// mark diameter to the supplied value.
        /// </summary>
        public void setMaxMarkDiam(double val)
        {
            mMaxMarkDiam = val;
            Update();
        }
        /// <summary>
        /// Gets the parameter describing the maximum
        /// mark diameter
        /// </summary>
        public double getMaxMarkDiam()
        {
            return mMaxMarkDiam;
        }

        /*******************************************/

        /// <summary>
        /// Sets the flag defining the origin of
        /// the reference world coordinate system
        /// to be in the image coordinate system
        /// </summary>
        public void setAtImgCoord(bool val)
        {
            mAtImgCoord = val;
            UpdateResultVisualization();
            
        }
        /// <summary>
        /// Gets the current value for the flag 
        /// </summary>
        public bool getAtImgCoord()
        {
            return mAtImgCoord;
        }


        public void dummy(int val){}

        /*******************************************/
        /*******************************************/
        /*******************************************/
        /*******************************************/        

        /// <summary>
        /// Gets the calibration image for the index <c>i</c>
        /// from the list <c>CalibData</c>
        /// </summary>
        public CalibImage getCalibDataAt(int i)
        {
            if(CalibData.Count>0)
                return (CalibImage)CalibData[i];
            else
                return null;
        }

        public int getCalibDataLength()
        {
            return CalibData.Count;
        }

        /// <summary>
        /// Add a new calibration image to the list <c>CalibData</c>.
        /// The image is read from the location <c>filename</c>
        /// and a new calibration image instance is then 
        /// generated, embedding this image.
        /// As a preparation step prior to the calibration process, the 
        /// basic information for the calibration image are determined,
        /// in terms of: detection of the region plate and the marks
        /// and pose.
        /// </summary>
        /// <returns>
        /// Instance of a calibration image model created for 
        /// the calibration image, supplied by <c>filename</c>
        /// </returns>
        public CalibImage addImage(string filename) 
        {
            HImage     image = null;
            CalibImage data = null;

            try
            {
                image = new HImage(filename);
                data  = new CalibImage(image, this);
                CalibData.Add(data);  
                data.UpdateCaltab(true);  
                mCanCalib = (mCanCalib && (data.mCanCalib==0));
                mCalibValid = false; 
            }                      
            catch(HOperatorException e)
            {
                mErrorMessage = e.Message;
                NotifyCalibObserver(CalibrationAssistant.ERR_READING_FILE);
            }
            
            return data;
        }

        /// <summary>
        /// Removes the instance of the calibration images
        /// at the index <c>index</c> from the list <c>CalibData</c>
        /// </summary>
        public void removeImage(int index)
        {
            ((CalibImage)CalibData[index]).Clear();
            CalibData.RemoveAt(index);
            mCalibValid = false;
            getCanCalibrate();
            NotifyCalibObserver(CalibrationAssistant.UPDATE_CALIBRATION_RESULTS);
        }
        
        /// <summary>
        /// Removes all instances of the calibration images
        /// from the list <c>CalibData</c>
        /// </summary>
        public void removeImage()
        {
            int count = CalibData.Count;
            for(int i=0; i<count;i++)
                ((CalibImage)CalibData[i]).Clear();
            
            CalibData.Clear();
            mCalibValid = false;
            mCanCalib = false;
            NotifyCalibObserver(CalibrationAssistant.UPDATE_CALIBRATION_RESULTS);
        }
		
        /// <summary>
        /// Gets the HALCON image with the index <c>index</c> in the list of
        /// calibration images <c>CalibData</c>.
        /// </summary>
        public HImage getImageAt(int index)
        {
            if(CalibData.Count>0)
                return ((CalibImage)CalibData[index]).getImage(); 
            return null;
        }

        /// <summary>
        /// Resets the flag <c>mCanCalib</c> to the boolean 'true'
        /// </summary>
        public void resetCanCalib()
        {
            mCanCalib = true;
        }
        
        /// <summary>
        /// Returns the reference image for the calibration process
        /// </summary>
        public HImage getRefImage()
        {
            return (HImage)mReferenceImage;
        }

        /// <summary>
        /// Returns the simulated image obtained from 
        /// the calibration process
        /// </summary>
        public HImage getSimulatedImage()
        {
            return mSimulatedImage;
        }

        /// <summary>
        /// Returns the reference world coordinate system
        /// obtained from the calibration process
        /// </summary>
        /// <returns></returns>
        public HObject getReferenceWCS()
        {
            return mReferenceWCS;
        }

        /*******************************************/

        /// <summary>
        /// Auxiliary method prior to the actual update 
        /// routine. Calls the actual update method 
        /// omitting the quality assessment for the 
        /// set of calibration image models.
        /// </summary>
        public void Update()
        {
            bool doQuality = false;
            Update(doQuality);
        }

        /// <summary>
        /// Updates the data of the calibration images
        /// if a change occurred in the calibration parameters.
        /// The quality assessment is performed if the 
        /// supplied value is positive; otherwise, it is omited.
        /// </summary>
        /// <param name="doQuality">
        /// If the flag is positive, an update of the 
        /// quality assessment is invoked, otherwise not.
        /// </param>
        public void Update(bool doQuality)
        {
            int  count;
            
            if((count=CalibData.Count) == 0)    
                return;
            
            try
            {
                for(int i=0; i<count;i++)
                    ((CalibImage)CalibData[i]).UpdateCaltab(doQuality); 

                if(doQuality)
                    UpdateSequenceIssues();


                mCanCalib = getCanCalibrate();

                NotifyCalibObserver(CalibrationAssistant.UPDATE_CALTAB_STATUS);
                NotifyCalibObserver(CalibrationAssistant.UPDATE_MARKS_POSE);

                if(doQuality)
                    NotifyCalibObserver(CalibrationAssistant.UPDATE_QUALITY_TABLE);
            }
            catch(HOperatorException e)
            {
                mErrorMessage = (string)e.Message;
                mCanCalib     = false;
                NotifyCalibObserver(CalibrationAssistant.ERR_QUALITY_ISSUES);
            }

            if(mCalibValid) 
            {               
                mCalibValid = false;
                NotifyCalibObserver(CalibrationAssistant.UPDATE_CALIBRATION_RESULTS);
            }
        }

        /*******************************************/

        /// <summary>
        /// Checks the whole set of calibration image models for
        /// the quality of the preprocessing step. If the basic
        /// information, i.e. the region plate and the marks
        /// and pose, was extracted in all images, then the
        /// flag <c>mCanCalib</c> is positive, which means
        /// the actual calibration process can be initiated 
        /// </summary>
        /// <returns>
        /// Flag indicating the feasibility of the calibration
        /// process
        /// </returns>
        public bool getCanCalibrate()
        {
            int count = CalibData.Count;
            int val   = 0;

            for(int i=0; i<count; i++)
                val+= ((CalibImage)CalibData[i]).mCanCalib;

            if(val==0 && count > 0)
                mCanCalib = true;
            else
                mCanCalib = false;

            return mCanCalib;
        }

        /// <summary>
        /// Gets the mark centers and the poses extracted from
        /// the set of calibration images 
        /// </summary>
        /// <param name="rows">
        /// Tuple of row coordinates of all marks from
        /// the entire set of calibration images
        /// </param>
        /// <param name="cols">
        /// Tuple of column coordinates of all marks from 
        /// the entire set of calibration images
        /// </param>
        /// <returns>
        /// Tuple of estimated poses for the entire set
        /// of calibration images
        /// </returns>
        public HTuple getCalibrationData(out HTuple rows, out HTuple cols)
        {
            int count   = CalibData.Count;
            HTuple pose = new HTuple();
            rows        = new HTuple();
            cols        = new HTuple();
            CalibImage image;
            
            for(int i = 0; i<count; i++)
            {
                image = (CalibImage)CalibData[i];                
                pose = pose.TupleConcat(image.getEstimatedPose());
                rows = rows.TupleConcat(image.getMarkCenterRows());
                cols = cols.TupleConcat(image.getMarkCenterColumns());
            }
            return pose;
        }

        /// <summary>
        /// Gets the camera parameters corresponding to
        /// the supplied calibration image.
        /// </summary>
        /// <returns>Camera parameters</returns>
        public HTuple getCameraParams(CalibImage image)
        {
            HTuple campar; 
            int  paramsListSize = 8; 
            int  offset         = 0;
            bool areaScanPoly   = false;
            
            if(mCameraType==CalibrationAssistant.CAMERA_TYP_AREA_SCAN_POLY)
            {
                paramsListSize  = 12;
                offset          = 4;                                                       
                areaScanPoly    = true;
            }

            paramsListSize += (mCameraType==CalibrationAssistant.CAMERA_TYP_LINE_SCAN)?3:0;
            
            campar    = new HTuple(paramsListSize);
            campar[0] = (isTelecentric? 0.0: ((double)mFocalLength/1000.0)); 

            if(areaScanPoly)
            {
                campar[1] =  mK1; 
                campar[2] =  mK2; 
                campar[3] =  mK3; 
                campar[4] =  mP1; 
                campar[5] =  mP2; 
            }
            else
            {
                campar[1] =  mKappa; 
            }
            
            campar[2+offset] = (double)mCellWidth/1000000.0;   // Sx -width   -> * 10^ -6 
            campar[3+offset] = (double)mCellHeight/1000000.0;  // Sy -height  -> * 10^ -6 
            campar[4 + offset] = (double)image.mWidth * 0.5;                  // x -principal point 
            campar[5 + offset] = (double)image.mHeight * 0.5;                 // y -principal point 
            campar[6+offset] = image.mWidth;                      // imagewidth 
            campar[7+offset] = image.mHeight;                     // imageheight 

            if(paramsListSize==11)
            {
                campar[8] = mMotionVx/1000000.0; 
                campar[9] = mMotionVy/1000000.0; 
                campar[10]= mMotionVz/1000000.0; 

                campar[5+offset] = 0;     // y -principal point = 0 for line scan camera 
            }

            return campar;       
        }
        
        /// <summary>
        /// Tests different quality features for the calibration image 
        /// <c>cImg</c>
        /// </summary>
        /// <returns>
        /// Returns a value indicating the success or failure
        /// of the quality assessment
        /// </returns>
        public bool testQualityIssues(CalibImage cImg)
        {
            ArrayList qList;
            
            HObject markContours;
            HObject plateRegion;
            HImage  mImg;
            HTuple  score, score2, contrast;
            int  numRegions, numContours;
            bool qualityFailure;

           
           
            mImg            = cImg.getImage();
            qList           = cImg.getQualityIssueList();
            procedure       = new QualityProcedures();
            contrast        = new HTuple();
            qualityFailure  = false;
            // DescriptionFileName = mDescrFileName;
            ;
                                     
            try
            {
                procedure.find_caltab_edges(mImg, out plateRegion,
                                            out markContours, 
                                            new HTuple(mDescrFileName));
                numRegions   = plateRegion.CountObj();
                numContours  = markContours.CountObj();

                if(mImageTests < QUALITY_ISSUE_TEST_NONE)
                {
                    if(numRegions == 0)
                    {
                        qualityFailure = true;
                    }
                    else
                    {
                        procedure.eval_caltab_overexposure(mImg, plateRegion, out score);  
                        addQualityIssue(qList, QUALITY_ISSUE_IMG_EXPOSURE, score.D);
                    }

                    if(numContours == 0)
                    {
                        qualityFailure = true;
                    }
                    else
                    {
                        procedure.eval_caltab_contrast_homogeneity(mImg, markContours,out contrast, out score, out score2);
                        addQualityIssue(qList, QUALITY_ISSUE_IMG_CONTRAST,    score.D);
                        addQualityIssue(qList, QUALITY_ISSUE_IMG_HOMOGENEITY, score2.D);

                        procedure.eval_caltab_size(mImg, plateRegion, markContours, out score);
                        addQualityIssue(qList, QUALITY_ISSUE_IMG_CALTAB_SIZE, score.D);
                    }

                    if(mImageTests == QUALITY_ISSUE_TEST_ALL)
                    {
                        procedure.eval_caltab_focus(mImg, markContours,contrast , out score);
                        addQualityIssue(qList, QUALITY_ISSUE_IMG_FOCUS, score.D);
                    }
                }
            }
            catch(HOperatorException e)
            {
                throw(e); 
            }

            return qualityFailure;
        }

        /// <summary>
        /// Tests for quality features concerning the performance 
        /// of the entire sequence of calibration images provided by
        /// the list <c>CalibData</c>
        /// </summary>
        public void UpdateSequenceIssues()
        {
            HTuple markRows, marksCols, startPose, width, height, hScore;
            bool hasIssue;
            bool hasError;
            double minScore, score;
            int count, countL;
            CalibImage imgC;
            ArrayList qList;
            
            mSeqQualityList.Clear();

            try
            {
                if(mSequenceTests < QUALITY_ISSUE_TEST_NONE)
                {
                    hasIssue = false;
                    hasError = false;
                    minScore   = 1.0;

                    if((count=CalibData.Count) == 0)    
                        return;
            
                    for(int i=0; i<count;i++)
                    {
                        imgC =(CalibImage)CalibData[i];
                  
                        if(imgC.getPlateStatus() == PS_QUALITY_ISSUES)
                        {
                            hasIssue = true;
                            qList = imgC.getQualityIssueList();
                            countL = qList.Count;

                            for(int j=0; j<countL;j++)
                            {
                                score = ((QualityIssue)qList[j]).getScore();

                                if(score < minScore)
                                    minScore = score;
                            }
                        }
                        else if(imgC.getPlateStatus() != PS_OK)
                        {
                            hasError = true;
                        }
                    }//for
                
                    if(hasError)
                    {
                        addQualityIssue(mSeqQualityList, QUALITY_ISSUE_SEQ_ERROR, 0.0);
                    }
                    else if(hasIssue)
                    {
                        addQualityIssue(mSeqQualityList, QUALITY_ISSUE_SEQ_ALL_OVER, minScore);
                    }

                    if(count < 20)
                    {
                        score = (count<=10)? 0.0:(0.1*((double)count-10.0));
                        addQualityIssue(mSeqQualityList, QUALITY_ISSUE_SEQ_NUMBER, score);
                    }  

                    if(mSequenceTests == QUALITY_ISSUE_TEST_ALL && count>0)
                    {
                        startPose = getCalibrationData(out markRows, out marksCols);
                        width     = new HTuple(((CalibImage)CalibData[0]).mWidth);
                        height    = new HTuple(((CalibImage)CalibData[0]).mHeight);
                        
                        procedure.eval_marks_distribution(markRows, marksCols, width, height, out hScore);
                        addQualityIssue(mSeqQualityList, QUALITY_ISSUE_SEQ_MARKS_DISTR, hScore[0].D);

                        procedure.eval_caltab_tilt(startPose, out hScore);
                        addQualityIssue(mSeqQualityList, QUALITY_ISSUE_SEQ_CALTAB_TILT, hScore[0].D);
                    }
                }//if!=None
            }
            catch(HOperatorException e)
            {
                mErrorMessage = e.Message;
            }
        }

        /// <summary>
        /// Adds the calculated score <c>score</c> for the quality feature 
        /// <c>type</c> to the supplied feature list <c>qList</c>
        /// </summary>
        /// <param name="qList">
        /// Quality feature list
        /// </param>
        /// <param name="type">
        /// Constant starting with QUALITY_*, describing one of the quality 
        /// features
        /// </param>
        /// <param name="score">
        /// Score determined for the quality feature
        /// </param>
        public void addQualityIssue(ArrayList qList, int type, double score)
        {
           if((int)(score*100)<= mWarnLevel)         
               qList.Add(new QualityIssue(type, score));
        }

        /// <summary>
        /// Calls the actual <c>addQualityIssue</c>
        /// method, with the feature list obtained from the
        /// calibration image <c>cImg</c>
        /// </summary>
        /// <param name="cImg">
        /// Calibration image model, which has been tested for
        /// the quality feature defined with <c>type</c>
        /// </param>
        /// <param name="type">
        /// Constant starting with QUALITY_* describing one of the quality 
        /// features
        /// </param>
        /// <param name="score">
        /// Score determined for the quality feature
        /// </param>
        public void addQualityIssue(CalibImage cImg, int type, double score)
        {
            ArrayList qList = cImg.getQualityIssueList();
            addQualityIssue(qList, type, score);
        }

        /// <summary>
        /// Applies the calibration on the set of calibration
        /// images <c>CalibData</c>
        /// </summary>
        public void applyCalibration()
        {
            HTuple       x,y,z,marksR,marksC;
            HTuple       error, startPoses, startCampar, parameters;
            CalibImage   refImg;
             
            mCalibValid = false;
            mErrorMean  = -1;
            
            if(mReferenceIndex >=0)
            {
                try
                {
                    refImg = (CalibImage)CalibData[mReferenceIndex];
                    HOperatorSet.CaltabPoints(mDescrFileName, out x, out y, out z);
                    startPoses  = getCalibrationData(out marksR, out marksC);
                    startCampar = getCameraParams(refImg);
                    parameters  = new HTuple("all");
                    
                    HOperatorSet.CameraCalibration(x, y, z,
                                                   marksR, marksC,
                                                   startCampar,
                                                   startPoses,
                                                   parameters,
                                                   out mCameraParams,
                                                   out mPoses,
                                                   out error);
                    mErrorMean = error[0].D;
                    mCalibValid = true;
                    
                    UpdateResultVisualization();
                }
                catch(HOperatorException e)
                {
                    mErrorMessage = e.Message;
                    NotifyCalibObserver(CalibrationAssistant.ERR_IN_CALIBRATION);
                }
            }
            else 
            {
                NotifyCalibObserver(CalibrationAssistant.ERR_REFINDEX_INVALID);
            }//if-else
        }

        /// <summary>
        /// Generates the iconic objects of the calibration results 
        /// for display
        /// </summary>
        public void UpdateResultVisualization()
        {
            HTuple refPose, correctedPose;
            double axisLen;
            HObject obj;
    
            if(!mCalibValid)
                return;

            mSimulatedImage.Dispose();

            mReferenceImage = ((CalibImage)CalibData[mReferenceIndex]).getImage();
            refPose         = getCalibratedPose(false);
        
            HOperatorSet.SimCaltab(out obj, new HTuple(mDescrFileName), 
                                   mCameraParams, refPose, new HTuple(128), 
                                   new HTuple(224), new HTuple(80), new HTuple(1));

            mSimulatedImage = new HImage(obj);

            correctedPose = getCalibratedPose(true);
            axisLen       = ((CalibImage)CalibData[mReferenceIndex]).getEstimatedPlateSize();
            
            procedure.get_3d_coord_system(mReferenceImage, out mReferenceWCS, mCameraParams, 
                                          correctedPose, new HTuple(axisLen/2));

            NotifyCalibObserver(CalibrationAssistant.UPDATE_CALIBRATION_RESULTS);
        }
  
        /// <summary>
        /// Saves the calibrated camera parameters in the file
        /// defined by <c>filename</c>
        /// </summary>
        public void saveCamParams(string filename)
        {
            if(mCalibValid)
            {
                try
                {
                    HOperatorSet.WriteCamPar(mCameraParams, new HTuple(filename));
                }
                catch(HOperatorException e)
                {
                    mErrorMessage = e.Message;
                    NotifyCalibObserver(CalibrationAssistant.ERR_WRITE_CALIB_RESULTS);  
                }
            }
        }

        /// <summary>
        /// Saves the pose obtained from the camera calibration
        /// in the file, defined by <c>filename</c>
        /// </summary>
        public void saveCamPose(string filename)
        {
            HTuple Pose;
            if(mCalibValid)
            {
                try
                {
                    Pose = getCalibratedPose(true);
                    HOperatorSet.WritePose(Pose, new HTuple(filename));
                }
                catch(HOperatorException e)
                {
                    mErrorMessage = e.Message;
                    NotifyCalibObserver(CalibrationAssistant.ERR_WRITE_CALIB_RESULTS);  
                }
            }
        }

        /// <summary>
        /// Returns calibration results
        /// </summary>
        /// <param name="camParams">
        /// Calibrated internal camera parameters 
        /// </param>
        /// <param name="refPose">
        /// Calibrated external camera parameters
        /// </param>
        public void getCalibrationResult(out HTuple camParams, 
                                           out HTuple refPose)
        {
            camParams = new HTuple(mCameraParams);
            refPose   = getCalibratedPose(true);
        }
        
        /// <summary>
        /// Returns the calibrated reference pose
        /// </summary>
        public HTuple getCalibratedPose(bool corrected)
        {
            HTuple tX, tY, tZ, correctedPose, refPose=null; 

            if(!mCalibValid)
                return new HTuple(1.0, -1.0, 0.0);
           
           
            if(mPoses.Length >= 7*(mReferenceIndex+1))
               refPose =  mPoses.TupleSelectRange(new HTuple(7*mReferenceIndex), 
                                                  new HTuple(7*mReferenceIndex+6));
               
            if(!corrected)
                return refPose;

            tX = new HTuple(0);
            tY = new HTuple(0);
            tZ = new HTuple(this.mThickness / 1000.0); 

            if(mAtImgCoord)
            {
                HOperatorSet.ImagePointsToWorldPlane(mCameraParams, refPose, 
                                                    new HTuple(0), new HTuple(0), 
                                                    new HTuple("m"), out tX, out tY);
            }
            HOperatorSet.SetOriginPose(refPose, tX, tY, tZ, out correctedPose); 

            return correctedPose;
        }

        /// <summary>Loads camera parameters from file</summary>
        /// <param name="camParFile">File name</param>
        /// <returns>Success or failure of load process</returns>
        public bool importCamParams(string camParFile)
        {
            HTuple campar;
            int    offset = 0;
            bool   areaScanPoly = false;
            

            try
            {
                HOperatorSet.ReadCamPar(new HTuple(camParFile), out campar);
                
                // -- load camera parameters --
                switch(campar.Length)
                {
                    case 8:
                        mCameraType = CAMERA_TYP_AREA_SCAN_DIV;
                        break;
                    case 11:
                        mCameraType = CAMERA_TYP_LINE_SCAN;
                        break;
                    case 12:
                        mCameraType = CAMERA_TYP_AREA_SCAN_POLY;
                        offset = 4;
                        areaScanPoly = true;
                        break;
                    default:
                        mCameraType = -1;
                        break;
                }

                mFocalLength  = campar[0]*1000.0;
                
                if(mFocalLength==0.0)
                    isTelecentric = true;
                else
                    isTelecentric = false;

                if(areaScanPoly)
                {
                    mK1 = campar[1]; 
                    mK2 = campar[2];  
                    mK3 = campar[3]; 
                    mP1 = campar[4]; 
                    mP2 = campar[5]; 
                }
                else
                {
                    mKappa =  campar[1]; 
                }
            
                mCellWidth  = campar[2+offset]*1000000.0;   
                mCellHeight = campar[3+offset]*1000000.0;   
                
                // line scan camera
                if(campar.Length==11)
                {
                    mMotionVx = campar[8].D *1000000.0; 
                    mMotionVy = campar[9].D *1000000.0; 
                    mMotionVz = campar[10].D*1000000.0; 
                }
                
                Update(true);
            }
            catch(HOperatorException e)
            {
                mErrorMessage = e.Message;
                NotifyCalibObserver(CalibrationAssistant.ERR_READING_FILE);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets the camera parameters to the default settings.
        /// </summary>
        /// <param name="camTyp">
        /// Flag to change camera type to the default as well
        /// </param>
        public void resetCameraSetup(bool camTyp)
        {
            if(camTyp)
                mCameraType    = CAMERA_TYP_AREA_SCAN_DIV;
       
            mThickness     = 1.00;       
            mCellWidth     = 8.300;    
            mCellHeight    = 8.300;    
            mFocalLength   = 8.000;      
            isTelecentric  = false;
            mKappa         = 0.0;   
            mK1            = 0.0;
            mK2            = 0.0;
            mK3            = 0.0;
            mP1            = 0.0;
            mP2            = 0.0;
            mMotionVx      = 0.0;
            mMotionVy      = 500.0;
            mMotionVz      = 0.0;

            Update(true);
        }

    }//end of class
}//end of namespace
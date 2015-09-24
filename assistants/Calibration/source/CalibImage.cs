using System;
using System.Collections;
using HalconDotNet;

   

namespace CalibrationModule
{
    
    /// <summary>
    /// This class contains all 
    /// information about its calibration image. 
    /// Besides the basic information for the calibration process, like
    /// the plate region and the marks, the calibration results are also 
    /// stored here. 
    /// 
	/// Please note that the HALCON Codelets does not support the 
	/// new HALCON calibration plate with hexagonally arranged marks.
	/// Only calibration plates with rectangularly
	/// arranged marks are supported for the Calibration Codelet.
	///
    /// Each CalibImage instance has a status <c>mCanCalib</c>, which 
    /// describes the mode of  "being ready for a calibration", depending 
    /// on the validity and completeness of the parameters marks, pose 
    /// and the plate region. 
    /// If these basics can not be extracted from the calibration image
    /// <c>mImage</c> using the current set of calibration parameters, 
    /// the flag <c>mCanCalib</c> remains 1 and indicates that a calibration 
    /// process is not feasible using this calibration image.
    /// </summary>
    public class CalibImage
	{
		
        /// <summary>
        /// Reference to the controller class that performs all 
        /// calibration operations and interacts with the GUI.
        /// </summary>
        private CalibrationAssistant mAssistant;
        /// <summary>Calibration image</summary>
        private HImage               mImage;
        /// <summary>
        /// Width of calibration image
        /// </summary>
        public int                   mWidth;
        /// <summary>
        /// Height of calibration image
        /// </summary>
        public int                   mHeight;
        

        /// <summary>
        /// Tuple with row coordinates of the detected marks
        /// </summary>
        private HTuple  mMarkCenterRows;
        /// <summary>
        /// Tuple with column coordinates of the detected marks
        /// </summary>
        private HTuple  mMarkCenterCols;
        /// <summary>
        /// Estimation for the external camera parameters (position and
        /// orientation)
        /// </summary>
        private HPose   mEstimatedPose;
        
      
        /// <summary>
        /// HALCON error message that occurs when calculating the 
        /// basic information for the calibration image 
        /// (plate region, marks and pose).
        /// </summary>
        public  string  mErrorMessage;
        /// <summary>
        /// Flag that describes the degree of success or failure 
        /// after an update of the basic information.
        /// </summary>
        public  string  mPlateStatus;
        /// <summary>
        /// Flag that permits or forbids this calibration image
        /// to be part of the calibration process
        /// </summary>
        public  int     mCanCalib; // true  = 0  ||  false = 1
        
        
        /// <summary>
        /// Region of the plane calibration plate in the calibration image
        /// </summary>
        private HRegion  mCaltabRegion;
        /// <summary>
        /// XLD contour points of the marks detected in 
        /// the calibration image, generated from the row and 
        /// column values <c>mMarkCenterRows</c> and 
        /// <c>mMarkCenterCols</c> 
        /// </summary>
        private HXLDCont mMarkCenter;
        /// <summary>
        /// Estimated world coordinate system (pose of the calibration plate
        /// in camera coordinates), based on the
        /// <c>mEstimatedPose</c> and the camera parameters 
        /// for this calibration image
        /// </summary>
        private HObject  mEstimatedWCS;
        
        private double   mEstimatedPlateSize;

        // for quality measurement 
        private ArrayList mQualityIssuesList;

        /// <summary>
        /// Initializes all status flags and objects to set up 
        /// this image for the calibration process
        /// </summary>
        /// <param name="img">Calibration image</param>
        /// <param name="assist">Reference to the Calibration Assistant</param>
        public CalibImage(HImage img, CalibrationAssistant assist)
		{
            string tmp;
            mImage     = img;
           
            mAssistant = assist;
            mCanCalib  = 1;  //labeled as 'for not having been evaluated'
            mPlateStatus = CalibrationAssistant.PS_NOT_FOUND;// "No Plate found" yet
            mImage.GetImagePointer1(out tmp, out mWidth, out mHeight);
            mEstimatedPlateSize = 0;
            mErrorMessage       = "";
            
            // initialize all instances
            mCaltabRegion      = new HRegion();
            mMarkCenter        = new HXLDCont();
            mEstimatedWCS      = new HObject();
            mQualityIssuesList = new ArrayList(15);

            mMarkCenterRows = new HTuple(); 
            mMarkCenterCols = new HTuple(); 
            mEstimatedPose  = new HPose (); 
		}  


        /************** getter-methods  *************/
        /********************************************/
        public HImage getImage()
        {
            return mImage;
        }
      
        public HTuple getMarkCenterRows() 
        {
            return mMarkCenterRows;
        }
        public HTuple getMarkCenterColumns() 
        {
            return mMarkCenterCols;
        }
        public HXLDCont getMarkCenters()
        {
            return mMarkCenter;
        }
        public HTuple getEstimatedPose()
        {
            return mEstimatedPose;
        }
        public HObject getEstimatedWCS()
        {
            return mEstimatedWCS;
        }
        public double getEstimatedPlateSize()
        {
            return mEstimatedPlateSize;
        }
        public HObject getCaltabRegion()
        {
            return mCaltabRegion;
        }
        public ArrayList getQualityIssueList()
        {
            return mQualityIssuesList;
        }
        public string getPlateStatus()
        {
            return mPlateStatus;
        }

     

        /// <summary>
        /// Determine s(or updates) the basic information for this 
        /// calibration image, which are the values for the region 
        /// plate, the center marks, and the estimated pose. 
        /// The flag <c>mPlateStatus</c> describes the evaluation 
        /// of the computation process.
        /// If desired the quality assessment can be recalculated 
        /// as well.
        /// </summary>
        /// <param name="updateQuality">
        /// Triggers the recalculation of the quality assessment for
        /// this calibration image 
        /// </param>
        public void UpdateCaltab(bool updateQuality) 
        {
            HTuple worldX, worldY;
            HTuple unit = new HTuple("m");  
 
            bool failed = false;
            QualityProcedures proc = new QualityProcedures();
            string descrFile;
            HTuple startCamp;
            mErrorMessage = "";
           
            
            mCaltabRegion.Dispose();
            mMarkCenter.Dispose();
            mEstimatedWCS.Dispose();

            //reset this variable
            mMarkCenterRows = new HTuple(); 
            
            mPlateStatus =  CalibrationAssistant.PS_NOT_FOUND ;
            
            descrFile = mAssistant.getDesrcFile();

            try
            {
                mCaltabRegion = mImage.FindCaltab(descrFile, 
                                                 (int)mAssistant.mFilterSize,
                                                 (int)mAssistant.mMarkThresh, 
                                                 (int)mAssistant.mMinMarkDiam);
            
                mPlateStatus = CalibrationAssistant.PS_MARKS_FAILED; 
             
                //-- Quality issue measurements --
                if(updateQuality)
                {   
                    mQualityIssuesList.Clear();
                    failed = mAssistant.testQualityIssues(this);
                }

                startCamp = mAssistant.getCameraParams(this);
                mMarkCenterRows = mImage.FindMarksAndPose(mCaltabRegion, 
                                                          descrFile, 
                                                          startCamp, 
                                                          (int)mAssistant.mInitThresh,
                                                          (int)mAssistant.mThreshDecr, 
                                                          (int)mAssistant.mMinThresh,
                                                          mAssistant.mSmoothing,
                                                          mAssistant.mMinContLength,
                                                          mAssistant.mMaxMarkDiam,
                                                          out mMarkCenterCols, 
                                                          out mEstimatedPose);     


                mMarkCenter.GenCrossContourXld(mMarkCenterRows, 
                                               mMarkCenterCols, 
                                               new HTuple(6.0), 
                                               0.785398);

                if(failed)
                    mAssistant.addQualityIssue(this, CalibrationAssistant.QUALITY_ISSUE_FAILURE, 0.0);

               
               HOperatorSet.ImagePointsToWorldPlane(startCamp, mEstimatedPose, 
                                                    mMarkCenterRows, mMarkCenterCols, 
                                                    unit, out worldX, out worldY);                      
               mEstimatedPlateSize = HMisc.DistancePp(worldY[0].D, worldX[0].D, 
                                                      worldY[1].D, worldX[1].D);
               mEstimatedPlateSize *=10.0;
               proc.get_3d_coord_system(mImage, out mEstimatedWCS, 
                                        startCamp, mEstimatedPose, 
                                        new HTuple(mEstimatedPlateSize/2.0));     
                
               mPlateStatus = mQualityIssuesList.Count>0 ? CalibrationAssistant.PS_QUALITY_ISSUES:CalibrationAssistant.PS_OK; // "Quality Issues found": "OK";
               mCanCalib   = 0;
            }
            catch(HOperatorException e)
            {
                this.mErrorMessage = e.Message; 
                mCanCalib = 1;
                
                /* if exception was raised due to lack of memory, 
                 * forward the error to the calling method */
                if(e.Message.IndexOf("not enough")!=-1) 
                    throw(e);
            }
        }        
        
        
        /// <summary>
        /// Releases the memory for all iconic HALCON objects contained in
        /// this instance.
        /// </summary>
        public void Clear()
        {
            mImage.Dispose();
            mCaltabRegion.Dispose();
        }
       
	}//end of class
}//end of namespace

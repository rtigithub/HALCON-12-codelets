using System;
using System.Collections;
using HalconDotNet;


namespace MatchingModule
{
	public delegate void MatchingDelegate(int value);
	public delegate void AutoParamDelegate(string value);
    
    /// <summary>
    /// This is a controller class, which receives the
    /// GUI component changes made by the user and forwards 
    /// the actions according to the internal settings.
    /// To notify the GUI about semantic changes, we use two delegates. One
    /// notifies about changes regarding the display objects
    /// (like a change of image or change in model or contour). It is also used
    /// to forward errors to the user, which might occur during a processing step.
    /// The second delegate notifies the GUI to correct its sliders and trackbars
    /// according to the new parameter setting, computed along with the 
    /// 'auto'-mechanism. 
    /// </summary>
    public class MatchingAssistant
	{
        /// <summary>
        /// Constant indicating that the model contour display should be
        /// updated.
        /// </summary>
		public const int UPDATE_XLD				= 0;
        /// <summary>
        /// Constant indicating that the pyramid (display-)level should be
        /// updated.
        /// </summary>
		public const int UPDATE_DISPLEVEL		= 1;
        /// <summary>
        /// Constant indicating that the pyramid should be updated.
        /// </summary>
		public const int UPDATE_PYRAMID			= 2;
        /// <summary>
        /// Constant indicating that the detection results should be updated.
        /// </summary>
		public const int UPDATE_DETECTION_RESULT= 3;
        /// <summary>
        /// Constant indicating that the test image display should be updated.
        /// </summary>
		public const int UPDATE_TESTVIEW		= 4;

        /// <summary>
        /// Constant indicating an error if a wrong file extension is used for
        /// a model file.
        /// </summary>
		public const int ERR_NO_VALID_FILE		= 5; 
        /// <summary>
        /// Constant indicating an error when writing a model file 
        /// </summary>
		public const int ERR_WRITE_SHAPEMODEL	= 6; 
        /// <summary>
        /// Constant indicating an error when reading from model file
        /// </summary>
		public const int ERR_READ_SHAPEMODEL	= 7; 
        /// <summary>
        /// Constant indicating an error if operations are performed that 
        /// need a shape-based model, though no model has been created, yet
        /// </summary>
		public const int ERR_NO_MODEL_DEFINED	= 8; 
        /// <summary>
        /// Constant indicating an error if operations are performed that 
        /// need a model image, though no model image has been loaded, yet
        /// </summary>
		public const int ERR_NO_IMAGE		    = 9;  
        /// <summary> 
        /// Constant indicating an error if operations are performed that
        /// need test images, though no test image has been loaded, yet
        /// </summary>
		public const int ERR_NO_TESTIMAGE	    = 10; 
        /// <summary>
        /// Constant indicating an error when reading an image file.
        /// </summary>
		public const int ERR_READING_IMG		= 11; 
		

		/// <summary>
		/// Region of interest defined by the sum of
		/// the positive and negative ROIs
		/// </summary>
		private HRegion         mROIModel; 
		
        /// <summary>Training image with full domain</summary>
		private HImage			oImage;	

        /// <summary> 
        /// The model image is the training image with  
        /// a domain reduced by the region mROIModel
        /// </summary>
		private HImage			mImage;

        /// <summary> 
        /// Test image in which the model is searched.
        /// </summary>
		private HImage			tImage;	
		
        /// <summary>List of test images</summary>
        public	Hashtable	TestImages;
        
        //matching parameters
		private MatchingParam   parameterSet;
        //matching result
		private MatchingResult	tResult;
        //model handle
		private HShapeModel		ModelID;
				
		// flags to control processing
		private bool	findAlways;
		private bool	createNewModelID;

        //flags to control the inspection and recognition process
		public  bool	onExternalModelID;
		public  bool	onTimer = false;
		
		// display purposes 
		private int		oWidth;
		private int		oHeight;
		private double	scaleW = 1.0;
		private double	scaleH = 1.0;

		private HImage		PyramidImages;  
		private HRegion		PyramidROIs;   
		private int			currentImgLevel = 1;	//mNumLevel
		private int			maxPyramidLevel = 6;
		private HHomMat2D	homSc2D;

        /// <summary>Stores exception message for display</summary>
		public string		exceptionText = "";
		
		
		// upper and lower range
		private int		contrastLowB;
		private int		contrastUpB;
		private double	scaleStepLowB;
		private double	scaleStepUpB;
		private double	angleStepLowB;
		private double  angleStepUpB;
		private int		pyramLevLowB;
		private int		pyramLevUpB;
		private int		minContrastLowB;
		private int		minContrastUpB;

        // auxiliary value table to store intermediate states
        private int    mCachedNumLevel;
        private double mCachedStartAng;
        private double mCachedAngExt;
        private double mCachedAngStep;
        private double mCachedMinScale;
        private double mCachedMaxScale;
        private double mCachedScaleStep;
        private string mCachedMetric;
        private int    mCachedMinCont;
        
      

        /// <summary>
        /// Delegate to forward changes for the display, which means changes 
        /// in the model contour, image level etc.
        /// </summary>
		public MatchingDelegate   NotifyIconObserver;
        /// <summary>
        /// Delegate to forward changes in the matching parameters determined 
        /// with the 'auto-' mechanism
        /// </summary>
		public AutoParamDelegate  NotifyParamObserver;

        
        /// <summary>
        /// Initializes flags, lists, and delegates to have a valid
        /// starting point to start the assistant.
        /// </summary>
       	public MatchingAssistant(MatchingParam  parSet)
		{
			parameterSet	= parSet;
			NotifyIconObserver  = new MatchingDelegate(dummy);
			NotifyParamObserver = new AutoParamDelegate(dummyS);
			homSc2D    = new HHomMat2D();
			TestImages = new Hashtable(10);
			tResult	   = new MatchingResult();

			contrastLowB	= 0;
			contrastUpB		= 255;
			scaleStepLowB	= 0.0;
			scaleStepUpB	= (double)19.0/1000.0;
			angleStepLowB	= 0.0;
			angleStepUpB	= (double)(112.0/10.0)*Math.PI/180.0;
			pyramLevLowB	= 1;
			pyramLevUpB		= 6;
			minContrastLowB = 0;
			minContrastUpB	= 30;

			findAlways		 = false;
			createNewModelID = true;
			ModelID			  = new HShapeModel();
			onExternalModelID = false;
		}
		
        /// <summary>Sets <c>image</c> to be the training image <c>oImage</c> </summary>
        public void setImage(HImage image)
		{
			string tmp;
			image.GetImagePointer1(out tmp, 
								   out oWidth, 
								   out oHeight);

			oImage	= image;
			reset();
		}
       
        /// <summary>
        /// Loads the training image from the file supplied by <c>filename</c>
        /// </summary>
        public bool setImage(string filename)
        {
            HImage image;

            try
            {
                image   = new HImage(filename);
                setImage(image);
            }
            catch(HOperatorException e)
            {
                exceptionText = e.Message;
                NotifyIconObserver(MatchingAssistant.ERR_READING_IMG);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Compute the model image and the model contour for the applied ROI.
        /// </summary>
        public void setModelROI(HRegion roi)
		{
			mROIModel	= roi;
			PyramidROIs = null;
			createNewModelID = true;
			
			if(mROIModel==null)
			{	
				mImage = null;
				return;
			}

			mImage = oImage.ReduceDomain(mROIModel);
			
			determineStepRanges();

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			inspectShapeModel();
		}

		/********************************************************************/
		/*					methods for test image      					*/
		/********************************************************************/

        /// <summary>
        /// Loads and adds test image files to the list of test images
        /// </summary>
        public bool addTestImages(string fileKey)
		{
			if(TestImages.ContainsKey(fileKey))
				return false;
			
			try
			{
				HImage image   = new HImage(fileKey);
				TestImages.Add(fileKey, image);
			}
			catch(HOperatorException e)
			{
				exceptionText = e.Message;
				NotifyIconObserver(MatchingAssistant.ERR_READING_IMG);
				return false;
			}
			return true;
		}

        /// <summary>
        /// Sets an image from the list of test images to be the current
        /// test image. The filename is given by the value fileKey.
        /// </summary>
        public void setTestImage(string fileKey)
		{
			tImage = (HImage)TestImages[fileKey];
			NotifyIconObserver(MatchingAssistant.UPDATE_TESTVIEW);
		}

        /// <summary>
        /// Removes an image from the list of test images, 
        /// using the file name
        /// </summary>
        public void removeTestImage(string fileName)
		{
            if(TestImages.ContainsKey(fileName))
                TestImages.Remove(fileName);

            if(TestImages.Count==0)
                tImage = null;
		}

        /// <summary>
        /// Removes all images from the list of test images
        /// </summary>
		public void removeTestImage()
		{
      		TestImages.Clear();
			tImage = null;
		}
		
        /// <summary>
        /// Sets the current test image to the image defined by 
        /// <c>fileName</c> and return it
        /// </summary>
        public HImage getTestImage(string fileName)
		{
			if((tImage == null) && (TestImages.ContainsKey(fileName)))
				tImage = (HImage)TestImages[fileName]; 
			
			return tImage;
		}

        /// <summary>
        /// Returns the current test image
        /// </summary>
        public HImage getCurrTestImage() 
		{
			return tImage;
		}
        

		/******************************************************************/
		/*							  auto options						  */
		/******************************************************************/

        /// <summary>
        /// Adds the parameter name given by <c>mode</c> 
        /// to the list of matching parameters that are 
        /// set to be determined automatically
        /// </summary>
        public void setAuto(string mode)
		{
			if(parameterSet.setAuto(mode))
				determineShapeParameter();
		}
		
        /// <summary>
        /// Removes the parameter name given by <c>mode</c> from the
        /// list of matching parameters that are determined 
        /// automatically
        /// </summary>
        public bool removeAuto(string mode)
		{
			return parameterSet.removeAuto(mode);
		}
		

		/************************  set matching values ********************/
		/******************************************************************/
        
        /// <summary>
        /// Sets Contrast to the input value <c>val</c> 
        /// and recreates the model.
        /// </summary>
        public void setContrast(int val)
		{
			parameterSet.setContrast(val);
			minContrastUpB = (int)val;
			
			inspectShapeModel();

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}
        
        /// <summary>
        /// Sets ScaleStep to the input value <c>val</c> 
        /// </summary>
        public void setScaleStep(double val)
		{
			parameterSet.setScaleStep(val);

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}

        /// <summary>
        /// Sets AngleStep to the input value <c>val</c> 
        /// </summary>
        public void setAngleStep(double val)
		{
			parameterSet.setAngleStep(val);

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}

        /// <summary>
        /// Sets PyramidLevel to the input value <c>val</c> 
        /// </summary>
        public void setPyramidLevel(double val)
		{
			parameterSet.setNumLevel(val);

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}

        /// <summary>
        /// Sets Optimization to the input value <c>val</c> 
        /// </summary>
       	public void setOptimization(string val)
		{
			parameterSet.setOptimization(val);

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}

        /// <summary>
        /// Sets MinContrast to the input value <c>val</c> 
        /// </summary>
        public void setMinContrast(int val)
		{
			parameterSet.setMinContrast(val);

			if(parameterSet.isOnAuto())
				determineShapeParameter();

			createNewModelID = true;
		}

		/*******************************************************/
  
        /// <summary>
        /// Sets Metric to the input value <c>val</c> 
        /// </summary>
        public void setMetric(string val)
		{
			parameterSet.setMetric(val);
			createNewModelID = true;
		}

        /// <summary>
        /// Sets MinScale to the input value <c>val</c> 
        /// </summary>
        public void setMinScale(double val)
		{
			parameterSet.setMinScale(val);
			createNewModelID = true;
		}
		
        /// <summary>
        /// Sets MaxScale to the input value <c>val</c> 
        /// </summary>
        public void setMaxScale(double val)
		{
			parameterSet.setMaxScale(val);
			createNewModelID = true;
		}

        /// <summary>
        /// Sets StartingAngle to the input value <c>val</c> 
        /// </summary>
        public void setStartingAngle(double val)
		{
			parameterSet.setStartingAngle(val);
			createNewModelID = true;
		}

        /// <summary>
        /// Sets AngleExtent to the input value <c>val</c> 
        /// </summary>
        public void setAngleExtent(double val)
		{
			parameterSet.setAngleExtent(val);
			createNewModelID = true;
		}

        /// <summary>
        /// Changes the currentImgLevel to the input parameter
        /// <c>val</c> and triggers an update in the GUI display
        /// </summary>
        public void setDispLevel(int val)
		{
			this.currentImgLevel = val;
			NotifyIconObserver(MatchingAssistant.UPDATE_DISPLEVEL);
		}

		/******************  set findmodel parameters *****************/
		/**************************************************************/
        /// <summary>
        /// Sets MinScore to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setMinScore(double val)
		{
			parameterSet.setMinScore(val);
			
			if(findAlways)
				detectShapeModel();
		}
		
        /// <summary>
        /// Sets NumMatches to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setNumMatches(int val)
		{
			parameterSet.setNumMatches(val);
			
			if(findAlways)
				detectShapeModel();
		}

        /// <summary>
        /// Sets Greediness to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setGreediness(double val)
		{
			parameterSet.setGreediness(val);
			
			if(findAlways)
				detectShapeModel();
		}

        /// <summary>
        /// Sets MaxOverlap to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setMaxOverlap(double val)
		{
			parameterSet.setMaxOverlap(val);
			
			if(findAlways)
				detectShapeModel();
		}

        /// <summary>
        /// Sets SubPixel to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setSubPixel(string val)
		{
			parameterSet.setSubPixel(val);
			
			if(findAlways)
				detectShapeModel();
		}

        /// <summary>
        /// Sets LastPyramLevel to the input value and starts model detection
        /// if the flag <c>findAlways</c> is checked
        /// </summary>
        public void setLastPyramLevel(int val)
		{
			parameterSet.setLastPyramLevel(val);
			
			if(findAlways)
				detectShapeModel();
		}

        /// <summary>
        /// Sets the flag <c>findAlways</c> to the input value <c>flag</c>
        /// </summary>
        public void setFindAlways(bool flag)
		{
			findAlways = flag;

			if(findAlways && tImage != null)
				detectShapeModel();
		}

        /// <summary>
        /// Triggers model detection, if a test image is defined
        /// </summary>
        public bool applyFindModel()
		{
			bool success = false;

			if(tImage != null)
				success = detectShapeModel();
			else
				NotifyIconObserver(MatchingAssistant.ERR_NO_TESTIMAGE);

			return success;
		}

        /// <summary>
        /// Gets detected model contours
        /// </summary>
        public HXLD getDetectionResults()
		{
			return tResult.getDetectionResults();
		}

        /// <summary>
        /// Gets matching results
        /// </summary>
        public MatchingResult getMatchingResults()
		{
			return tResult;
		}
		
		/********************  optimize recognition speed *****************/
		/******************************************************************/

        /// <summary>
        /// Sets the RecognitionRateOption to the input  
        /// value <c>idx</c>
        /// </summary>
        public void setRecogRateOption(int idx)
		{
			parameterSet.setRecogRateOption(idx);
		}

        /// <summary>
        /// Sets the RecognitionRate to the input value <c>val</c>
        /// </summary>
        public void setRecogitionRate(int val)
		{
			parameterSet.setRecogitionRate(val);
		}

        /// <summary>
        /// Sets the RecognitionSpeedMode to the input 
        /// value <c>val</c>
        /// </summary>
        public void setRecogSpeedMode(string val)
		{
			parameterSet.setRecogSpeedMode(val);
		}

        /// <summary>
        /// Sets the RecognitionManualSelection to the input  .
        /// value <c>val</c>
        /// </summary>
        public void setRecogManualSelection(int val)
		{	
			parameterSet.setRecogManualSelection(val);
		}
		

		/********************************************************************/
		/*                        getter methods                            */
		/********************************************************************/

        /// <summary>
        /// Gets the model for the current image level
        /// </summary>
        public HXLD getModelContour()
		{
			if(PyramidROIs==null)
				return null;

			homSc2D.HomMat2dIdentity();
			homSc2D = homSc2D.HomMat2dScaleLocal(scaleW, scaleH);
			
			return ((PyramidROIs.SelectObj(currentImgLevel)).
										   GenContourRegionXld("center")).
										   AffineTransContourXld(homSc2D); 
		}

        /// <summary>
        /// Gets the model supplied by a loaded shapebased model file (.shm)
        /// </summary>
        public HXLD getLoadedModelContour()
        {
            HTuple row1, col1, row2, col2, row, col;
            HHomMat2D homMat2D = new HHomMat2D();

            try
            {
                tResult.mContour.SmallestRectangle1Xld(out row1, out col1, out row2, out col2);
                row2 = row1.TupleMin();     
                col2 = col1.TupleMin();     
                row  = row2.TupleFloor()-5; 
                col  = col2.TupleFloor()-5; 
                homMat2D.HomMat2dIdentity();
                homMat2D = homMat2D.HomMat2dTranslate(-row,-col);
                
                return homMat2D.AffineTransContourXld(tResult.mContour);
            }
            catch(HOperatorException e)
            {
                exceptionText = e.Message;
                NotifyIconObserver(MatchingAssistant.ERR_READ_SHAPEMODEL);
                return null;
            }
        }

		/// <summary>
        /// Gets the model region for the current image level.
        /// </summary>
        public HRegion getModelRegion()
		{
			if(PyramidROIs==null)
				return null;

			HRegion reg = PyramidROIs.SelectObj(currentImgLevel);
			return reg.ZoomRegion(scaleW, scaleH);
		}

		/// <summary>
        /// Gets the model image for the current image level.
        /// </summary>
        public HImage getDispImage()
		{
			int fW, fH;
			string tmp;
			
			if (PyramidImages ==null)
				return oImage;
			
			HImage img = PyramidImages.SelectObj(currentImgLevel);
			img.GetImagePointer1(out tmp, out fW, out fH);

			scaleW = (double)oWidth/fW;
			scaleH = (double)oHeight/fH;

			return img.ZoomImageFactor(scaleW, scaleH, "nearest_neighbor");
		}

		/// <summary>
        /// Clears all model creation and detection results 
        /// and resets flags to their initial values.
        /// </summary>
		public void reset()
		{
			mROIModel	= null;
			currentImgLevel = 1;
			PyramidROIs = null;
			PyramidImages = null;
			mImage = null;
			scaleH = 1;
			scaleW = 1;
			createNewModelID = true;

			tResult.reset();
			NotifyIconObserver(MatchingAssistant.UPDATE_XLD);
		}


		/********************************************************************/
        /********************************************************************/

		/// <summary>
        /// Writes model to the file specified by <c>fileName</c>.
        /// </summary>
        public void saveShapeModel(string fileName)
		{
			if(mImage == null)
			{
				NotifyIconObserver(MatchingAssistant.ERR_NO_MODEL_DEFINED);
				return;
			}
				
			if (createNewModelID)
				if (!createShapeModel())	
					return;					
			try
			{
				ModelID.WriteShapeModel(fileName);
			}
			catch(HOperatorException e)
			{
				exceptionText = e.Message;
				NotifyIconObserver(MatchingAssistant.ERR_WRITE_SHAPEMODEL);
			}	
		}
        
        /// <summary>
        /// Loads model from the file specified by <c>fileName</c>
        /// </summary>
        public bool loadShapeModel(string fileName)
		{
            onExternalModelID = false;

			try
			{
				ModelID.ReadShapeModel(fileName);
			}
			catch(HOperatorException e)
			{
				exceptionText = e.Message;
            	NotifyIconObserver(MatchingAssistant.ERR_READ_SHAPEMODEL);
				return false;
			}
			
			onExternalModelID = true;
			tResult.mContour  = ModelID.GetShapeModelContours(1);
            
            cacheModelParams();
            
			parameterSet.mNumLevel = ModelID.GetShapeModelParams(out parameterSet.mStartingAngle, 
									                             out parameterSet.mAngleExtent, 
										                         out parameterSet.mAngleStep,
										                         out parameterSet.mMinScale,
										                         out parameterSet.mMaxScale,
 										                         out parameterSet.mScaleStep, 
										                         out parameterSet.mMetric,       
										                         out parameterSet.mMinContrast); 			
            return true;
		}
        
        /***************************************************************/
        /***************************************************************/
        private void cacheModelParams()
        {        
            mCachedNumLevel  = parameterSet.mNumLevel;
            mCachedStartAng  = parameterSet.mStartingAngle;
            mCachedAngExt    = parameterSet.mAngleExtent;
            mCachedAngStep   = parameterSet.mAngleStep;
            mCachedMinScale  = parameterSet.mMinScale;
            mCachedMaxScale  = parameterSet.mMaxScale;
            mCachedScaleStep = parameterSet.mScaleStep;
            mCachedMetric    = parameterSet.mMetric;
            mCachedMinCont   = parameterSet.mMinContrast;
        }


        public void resetCachedModelParams()
        {
            parameterSet.mNumLevel      = mCachedNumLevel;
            parameterSet.mStartingAngle = mCachedStartAng;
            parameterSet.mAngleExtent   = mCachedAngExt;
            parameterSet.mAngleStep     = mCachedAngStep;
            parameterSet.mMinScale      = mCachedMinScale;
            parameterSet.mMaxScale      = mCachedMaxScale;
            parameterSet.mScaleStep     = mCachedScaleStep;
            parameterSet.mMetric        = mCachedMetric;
            parameterSet.mMinContrast   = mCachedMinCont;
            createNewModelID            = true;
        }
		
        /// <summary>
        /// Creates the shape-based model. If the region of interest 
        /// <c>mROIModel</c> is missing or not well defined using the 
        /// interactive ROIs, then an error message is returned.
        /// </summary>
        public bool createShapeModel()
		{
			if(mImage==null)
			{
				if(!onTimer)
					NotifyIconObserver(MatchingAssistant.ERR_NO_MODEL_DEFINED);
				return false;
			}

			try
			{
				ModelID = mImage.CreateScaledShapeModel(parameterSet.mNumLevel,
														parameterSet.mStartingAngle,
														parameterSet.mAngleExtent,
														parameterSet.mAngleStep,
														parameterSet.mMinScale,
														parameterSet.mMaxScale,
														parameterSet.mScaleStep,
														parameterSet.mOptimization,
														parameterSet.mMetric,
														parameterSet.mContrast,
														parameterSet.mMinContrast);
			}
			catch(HOperatorException e)
			{
				
				if(!onTimer)
				{
					exceptionText = e.Message;
					NotifyParamObserver(MatchingParam.H_ERR_MESSAGE);
				}
				return false;
			}

			tResult.mContour = ModelID.GetShapeModelContours(1);
			
			createNewModelID = false;
			return true;
		}

		/// <summary>
        /// Finds the model in the test image. If the model
        /// hasn't been created or needs to be recreated (due to 
        /// user changes made to the GUI components), 
        /// then the model is created first.
        /// </summary>
       	public bool detectShapeModel()
		{	
			HTuple levels, rtmp;
			rtmp = new HTuple();
			double t2, t1;

			if(tImage == null)
				return false;
			
			if(createNewModelID && !onExternalModelID)
				if(!createShapeModel()) 
					return false;		

			try
			{	
				levels = new HTuple(new int [] {parameterSet.mNumLevel, 
												parameterSet.mLastPyramidLevel});
				t1 = HSystem.CountSeconds();
				tImage.FindScaledShapeModel(ModelID,
											parameterSet.mStartingAngle,
											parameterSet.mAngleExtent,
											parameterSet.mMinScale,
											parameterSet.mMaxScale,
											parameterSet.mMinScore,
											parameterSet.mNumMatches,
											parameterSet.mMaxOverlap,
											new HTuple(parameterSet.mSubpixel),
											levels, 
											parameterSet.mGreediness,
											out tResult.mRow,
											out tResult.mCol,
											out tResult.mAngle,
											out tResult.mScaleRow,
											out tResult.mScore);
				t2 = HSystem.CountSeconds();
				tResult.mTime = 1000.0 * (t2 - t1);
				tResult.count = tResult.mRow.Length;
				tResult.mScaleCol = tResult.mScaleRow;
			}	
			catch(HOperatorException e)
			{	
				if(!onTimer)
				{
					exceptionText = e.Message;
					NotifyParamObserver(MatchingParam.H_ERR_MESSAGE);
				}
					
				return false;
			}
			
			NotifyIconObserver(MatchingAssistant.UPDATE_DETECTION_RESULT);
			return true;
		}


		/// <summary>
        /// Creates the model contour.
        /// </summary>
		public void inspectShapeModel()
		{
			HRegion tmpReg;
			HImage  tmpImg;

			if(mImage == null)
			{
				NotifyIconObserver(MatchingAssistant.ERR_NO_MODEL_DEFINED);
				return;
			}

			PyramidImages  = oImage.InspectShapeModel(out tmpReg, 
													 (int)maxPyramidLevel, 
													  parameterSet.mContrast);
			tmpImg  = mImage.InspectShapeModel(out PyramidROIs, 
											  (int)maxPyramidLevel, 
											   parameterSet.mContrast);

			NotifyIconObserver(MatchingAssistant.UPDATE_XLD);
			NotifyIconObserver(MatchingAssistant.UPDATE_PYRAMID);
		}
		
		/// <summary>
        /// Adjusts the range of ScaleStep and AngleStep according to the
        /// current set of matching parameters.
        /// </summary>
		public void determineStepRanges()
		{
			double vald = 0.0;
			HTuple paramValue = new HTuple();
			HTuple paramList  = new HTuple();
			string [] paramRange = {"scale_step", "angle_step"};
			
			if(mImage == null)
			{
				NotifyIconObserver(MatchingAssistant.ERR_NO_MODEL_DEFINED);
				return;
			}

			try
			{
				paramList = mImage.DetermineShapeModelParams(parameterSet.mNumLevel,				
															(double)parameterSet.mStartingAngle,
															(double)parameterSet.mAngleExtent,	
															parameterSet.mMinScale,				
															parameterSet.mMaxScale,				
															parameterSet.mOptimization,			
															parameterSet.mMetric,				
															(int)parameterSet.mContrast,		
															(int)parameterSet.mMinContrast,		
															paramRange,		
															out paramValue);
			}
			catch(HOperatorException e)
			{
				exceptionText = e.Message;
				NotifyParamObserver(MatchingParam.H_ERR_MESSAGE);
				return;
			}

			for(int i =0; i<paramList.Length; i++) 
			{
				switch ((string)paramList[i])
				{
					case MatchingParam.AUTO_ANGLE_STEP: 
						vald =  (double)paramValue[i];		

						angleStepUpB  = vald * 3.0;
						angleStepLowB = vald / 3.0;
						parameterSet.mAngleStep = vald;
						NotifyParamObserver(MatchingParam.RANGE_ANGLE_STEP);
						break;
					case MatchingParam.AUTO_SCALE_STEP: 
						vald = (double)paramValue[i];

						scaleStepUpB  = vald * 3.0;
						scaleStepLowB = vald / 3.0;
						parameterSet.mScaleStep = vald; 
						NotifyParamObserver(MatchingParam.RANGE_SCALE_STEP);
						break;
					default: 
						break;
				}//end of switch
			}//end of for
		}

		
        /// <summary>
        /// Determines the values for the matching parameters
        /// contained in the auto-list automatically.
        /// </summary>
		public void determineShapeParameter()
		{
			double	vald;
			int		vali, count;
			HTuple paramValue = new HTuple();
			HTuple paramList = new HTuple();
			
			if(mImage == null)
			{
				NotifyIconObserver(MatchingAssistant.ERR_NO_MODEL_DEFINED);
				return;
			}
			
			try
			{
				paramList = mImage.DetermineShapeModelParams(parameterSet.mNumLevel,				
															 (double)parameterSet.mStartingAngle,
															 (double)parameterSet.mAngleExtent,	
															 parameterSet.mMinScale,				
															 parameterSet.mMaxScale,				
															 parameterSet.mOptimization,			
															 parameterSet.mMetric,				
															 (int)parameterSet.mContrast,		
															 (int)parameterSet.mMinContrast,		
															 parameterSet.getAutoParList(),		
															 out paramValue);
			}
			catch(HOperatorException e)
			{
				exceptionText = e.Message;
				NotifyParamObserver(MatchingParam.H_ERR_MESSAGE);
				return;
			}

			count = paramList.Length;
			
			for(int i =0; i<count; i++)
			{
				switch ((string)paramList[i])
				{
					case MatchingParam.AUTO_ANGLE_STEP: 
						vald =  (double)paramValue[i];			

						if(vald > angleStepUpB)
							vald = angleStepUpB;
						else if(vald < angleStepLowB)
							vald = angleStepLowB;

						parameterSet.mAngleStep = vald;
						break;
					case MatchingParam.AUTO_CONTRAST: 
						vali = (int)paramValue[i];
			
						if(vali > contrastUpB)
							vali = contrastUpB;
						else if (vali < contrastLowB)
							vali = contrastLowB;

						minContrastUpB = vali;
						parameterSet.mContrast= vali;
						
						inspectShapeModel();
						break;
					case MatchingParam.AUTO_MIN_CONTRAST: 
						vali = (int)paramValue[i];

						if(vali > minContrastUpB)
							vali = minContrastUpB;
						else if(vali < minContrastLowB)
							vali = minContrastLowB;

						parameterSet.mMinContrast = vali;
						break;
					case MatchingParam.AUTO_NUM_LEVEL: 
						vali = (int)paramValue[i];

						if(vali > pyramLevUpB)
							vali = pyramLevUpB;
						else if(vali < pyramLevLowB)
							vali = pyramLevLowB;

						parameterSet.mNumLevel = vali;
						break;
					case MatchingParam.AUTO_OPTIMIZATION: 
						parameterSet.mOptimization = (string)paramValue[i];
						break;
					case MatchingParam.AUTO_SCALE_STEP: 
						vald = (double)paramValue[i];

						if(vald > scaleStepUpB)
							vald = scaleStepUpB;
						else if (vald < scaleStepLowB)
							vald = scaleStepLowB;

						parameterSet.mScaleStep = vald; 
						break;
					default: 
						break;
				}
				NotifyParamObserver((string)paramList[i]);
			}//end of for

			if (count!=0)
				createNewModelID = true;
		}

		/// <summary>
        /// Gets the range of either one of the parameters  
        /// AngleStep and ScaleStep, defined by <c>param</c>
        /// </summary>
        public int [] getStepRange(string param)
		{
			int [] range = new int[2];
		
			switch (param)
			{
				case MatchingParam.RANGE_ANGLE_STEP:
					range [0] = (int)(angleStepLowB * 10.0 *180.0/Math.PI); //low
					range [1] = (int)(angleStepUpB * 10.0 *180.0/Math.PI);  //up
					break;
				case MatchingParam.RANGE_SCALE_STEP:
					range [0] = (int) (scaleStepLowB * 1000.0); //low
					range [1] = (int) (scaleStepUpB * 1000.0);  //up
					break;
				default:
					break;
			}
			return range;
		}

		/********************************************************************/
		/********************************************************************/
		private void dummy(int val)
		{
			return;
		}
		private void dummyS(string val)
		{
			return;
		}

	}//end of class
}//end of namespace


using System;
using System.Collections;
using HalconDotNet;


namespace MatchingModule
{
    
    /// <summary>
    /// This class contains the parameters that are used to
    /// create and detect the shape-based model. 
    /// Besides auxiliary methods to set and get the parameters, there 
    /// is a set of methods to handle the parameters for 'automatic' 
    /// determination.
    /// </summary>
    public class MatchingParam
	{

		// --------------- create model ---------------------

        /// <summary>
        /// Defines the maximum number of pyramid levels
        /// </summary>
		public int	  mNumLevel;		
        /// <summary>
        /// Measure for local gray value differences between
        /// the object and the background and between 
        /// different parts of the object
        /// </summary>
		public int	  mContrast; 
        /// <summary>
        /// Minimum scale of the model
        /// </summary>
		public double mMinScale;		
        /// <summary>
        /// Maximum scale of the model
        /// </summary>
		public double mMaxScale;		
        /// <summary>
        /// Step length within the selected range of scales
        /// </summary>
		public double mScaleStep;		
        /// <summary>
        /// Smallest rotation of the model
        /// </summary>
		public double mStartingAngle;	
        /// <summary>
        /// Extent of the rotation angles
        /// </summary>
		public double mAngleExtent;		
        /// <summary>
        /// Step length within the selected range of angles
        /// </summary>
		public double mAngleStep;		
        /// <summary>
        /// Used to separate the model from the noise in the image
        /// </summary>
		public int	  mMinContrast;		
        /// <summary>
        /// Conditions determining how the model is supposed to be 
        /// recognized in the image
        /// </summary>
		public string mMetric;			
        /// <summary>
        /// Defines the kind of optimization and optionally the 
        /// kind of method used for generating the model
        /// </summary>
		public string mOptimization;	


		// -------------------- find model -----------------------

        /// <summary>
        /// Defines the score a potential match must have at least 
        /// to be accepted as an instance of the model in the image
        /// </summary>
		public double mMinScore;
        /// <summary>
        /// Number of instances of the model to be found
        /// </summary>
		public int	  mNumMatches;
        /// <summary>
        /// 'Greediness' of the search heuristic (0 means safe but slow - 
        /// 1 means fast but matches may be missed).
        /// </summary>
		public double mGreediness;
        /// <summary>
        /// Defines fraction two instances may at most overlap in order
        /// to consider them as different instances, and hence
        /// to be returned separately
        /// </summary>
		public double mMaxOverlap;
        /// <summary>
        /// Determines whether the instances should be extracted with 
        /// subpixel accuracy
        /// </summary>
		public string mSubpixel;
        /// <summary>
        /// Determines the lowest pyramid level to which the 
        /// found matches are tracked. Mechanism is used to 
        /// speed up the matching.
        /// </summary>
		public int    mLastPyramidLevel;

		//----------------- optimize recognition speed ----------------

        /// <summary>
        /// Defines heuristic for evaluating the detection results
        /// </summary>
		public int	  mRecogRateOpt; /* opt=0 => '=' und opt=1 => '>='*/
        /// <summary>
        /// Defines rate for the model recognition
        /// </summary>
		public int	  mRecogRate;
        /// <summary>
        /// Defines mode to determine accuracy of recognition
        /// </summary>
		public string mRecogSpeedMode;
        /// <summary>
        /// Manual selection of number of matches to be found.
        /// </summary>
		public int	  mRecogManualSel;

		// ---------------------- inspect vals -----------------------

        /// <summary>
        /// Is equal to the value of the parameter mNumMatches
        /// </summary>
		public int mInspectMaxNoMatch;
        /// <summary>
        /// List of parameters to be determined automatically
        /// </summary>
		public ArrayList paramAuto;
		

        /// <summary>
        /// Constant defining the auto-mode for the parameter NumLevels
        /// </summary>
		public const string AUTO_NUM_LEVEL		= "num_levels";
        /// <summary>
        /// Constant defining the auto-mode for the parameter Contrast
        /// </summary>
		public const string AUTO_CONTRAST		= "contrast"; 
        /// <summary>
        /// Constant defining the auto-mode for the parameter ScaleStep
        /// </summary>
		public const string AUTO_SCALE_STEP		= "scale_step";
        /// <summary>
        /// Constant defining the auto-mode for the parameter AngleStep
        /// </summary>
		public const string AUTO_ANGLE_STEP		= "angle_step";
        /// <summary>
        /// Constant defining the auto-mode for the parameter MinContrast
        /// </summary>
		public const string AUTO_MIN_CONTRAST	= "min_contrast";
        /// <summary>
        /// Constant defining the auto-mode for the parameter Optimization
        /// </summary>
		public const string AUTO_OPTIMIZATION	= "optimization";
		
        /// <summary>
		/// Constant indicating an update for the button representation of
        /// AngleStart.
		/// </summary>
		public const string BUTTON_ANGLE_START	= "angle_start";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// AngleExtent.
        /// </summary>
		public const string BUTTON_ANGLE_EXTENT	= "angle_extent";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// ScaleMin.
        /// </summary>
		public const string BUTTON_SCALE_MIN	= "scale_min";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// ScaleMax.
        /// </summary>
		public const string BUTTON_SCALE_MAX	= "scale_max";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// Metric.
        /// </summary>
		public const string BUTTON_METRIC		= "metric";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// MinScore.
        /// </summary>
		public const string BUTTON_MINSCORE		= "min_score";
        /// <summary>
        /// Constant indicating an update for the button representation of
        /// Greediness.
        /// </summary>
		public const string BUTTON_GREEDINESS	= "greediness";

        /// <summary>
        /// Constant defining the number of instances to be detected:
        /// Find number of models specified by the user
        /// </summary>
		public const string RECOGM_MANUALSELECT	= "RecognFindSpecifiedNumber";
        /// <summary>
        /// Constant defining the number of instances to be detected:
        /// Find at least one model instance per image
        /// </summary>
		public const string RECOGM_ATLEASTONE	= "RecognAtLeast";
        /// <summary>
        /// Constant defining the number of instances to be detected:
        /// Find maximum number of model instances per image
        /// </summary>
		public const string RECOGM_MAXNUMBER	= "RecognFindMaximum";
        
        /// <summary>
        /// Constant indicating a change of ScaleStep for
        /// its GUI component representation 
        /// </summary>
		public const string RANGE_SCALE_STEP	= "RangeScaleStep";
        /// <summary>
        /// Constant indicating a change of AngleStep for
        /// its GUI component representation 
        /// </summary>
		public const string RANGE_ANGLE_STEP	= "RangeAngleStep";
        
        /// <summary>
        /// Constant indicating an error regarding the parameter set. 
        /// It is forwarded for HALCON errors that occur during the
        /// creation of the shape-based model or detection of instances 
        /// of the model
        /// </summary>
		public const string H_ERR_MESSAGE		= "Halcon Error";


        /// <summary>Constructor</summary>
		public MatchingParam()
		{
			paramAuto = new ArrayList(10);
		}
		
		/*******************************************************************/
		/*    Setter-methods for the set of values, that can be determined 
         *    automatically. If a parameter gets assigned a new value
         *    it can be only caused by user interaction, which means, the
         *    auto-modus for these particular parameters needs to be 
         *    canceled, to avoid further automatic adjustment              
		/*******************************************************************/
  
        /// <summary>
        /// Sets the parameter <c>NumLevel</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void setNumLevel(double val)
		{
			mNumLevel = (int)val;
			if(paramAuto.Contains(AUTO_NUM_LEVEL))
				paramAuto.Remove(AUTO_NUM_LEVEL);
		}
	
		/// <summary>
		/// Sets the parameter <c>Contrast</c> to the supplied value;
		/// if the parameter has been in auto-mode, cancel this option
		/// </summary>
		public void setContrast(int val)
		{
			mContrast = val;

			if(paramAuto.Contains(AUTO_CONTRAST))
				paramAuto.Remove(AUTO_CONTRAST);
		}
	
		/// <summary>
		/// Sets the parameter <c>MinScale</c> to the supplied value;
		/// if the parameter has been in auto-mode, cancel this option
		/// </summary>
		public void setMinScale(double val)
		{
			mMinScale = val;
		}
	
        /// <summary>
        /// Sets the parameter <c>MaxScale</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void setMaxScale(double val)
		{
			mMaxScale = val;
		}
		
		/// <summary>
		/// Sets the parameter <c>ScaleStep</c> to the supplied value;
		/// if the parameter has been in auto-mode, cancel this option
		/// </summary>
		public void setScaleStep(double val)
		{
			mScaleStep = val;

			if(paramAuto.Contains(AUTO_SCALE_STEP))
				paramAuto.Remove(AUTO_SCALE_STEP);

		}
        
        /// <summary>
        /// Sets the parameter <c>AngleStep</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void setAngleStep(double val)
        {
            mAngleStep = val;

            if(paramAuto.Contains(AUTO_ANGLE_STEP))
                paramAuto.Remove(AUTO_ANGLE_STEP);
        }
	
        /// <summary>
        /// Sets the parameter <c>MinContrast</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        public void setMinContrast(int val)
        {
            mMinContrast = val;

            if(paramAuto.Contains(AUTO_MIN_CONTRAST))
                paramAuto.Remove(AUTO_MIN_CONTRAST);
        }

        /// <summary>
        /// Sets the parameter <c>Optimization</c> to the supplied value;
        /// if the parameter has been in auto-mode, cancel this option
        /// </summary>
        /// <param name="val"></param>
        public void setOptimization(string val)
        {
            mOptimization = val;

            if(paramAuto.Contains(AUTO_OPTIMIZATION))
                paramAuto.Remove(AUTO_OPTIMIZATION);
        }		

		
        /*******************************************************************/
        /*        Setter-methods for the other values                      */
        /*******************************************************************/

		/// <summary>
		/// Sets the parameter <c>StartingAngle</c> to the supplied value
		/// </summary>
		public void setStartingAngle(double val)
		{
			mStartingAngle = val;
		}
		
		/// <summary>
		/// Sets the parameter <c>AngleExtent</c> to the supplied value
		/// </summary>
		public void setAngleExtent(double val)
		{
			mAngleExtent = val;
		}
	
		/// <summary>
		/// Sets the parameter <c>Metric</c> to the supplied value
		/// </summary>
		public void setMetric(string val)
		{
			mMetric = val;
		}
		
		/// <summary>
		/// Sets the parameter <c>MinScore</c> to the supplied value
		/// </summary>
		public void setMinScore(double val)
		{
			mMinScore = val;
		}
		
		/// <summary>
		/// Sets  the parameter <c>NumMatches</c> to the supplied value
		/// </summary>
		public void setNumMatches(int val)
		{
			mNumMatches = val;
		}

		
		/// <summary>
		/// Sets the parameter <c>Greediness</c> to the supplied value
		/// </summary>
		public void setGreediness(double val)
		{
			mGreediness = val;
		}

		/// <summary>
		/// Sets the parameter <c>MaxOverlap</c> to the supplied value
		/// </summary>
		public void setMaxOverlap(double val)
		{
			mMaxOverlap = val;
		}

		/// <summary>
		/// Sets the parameter <c>Subpixel</c> to the supplied value
		/// </summary>
		public void setSubPixel(string val)
		{
			mSubpixel = val;
		}

		/// <summary>
		/// Sets the parameter <c>LastPyramidLevel</c> to the supplied value
		/// </summary>
		public void setLastPyramLevel(int val)
		{
			mLastPyramidLevel = val;
		}

		/*******************************************************************/
		/*******************************************************************/
		/*******************************************************************/

        /// <summary>
        /// Sets the parameter defining the options for the recognition rate
        /// to the supplied value
        /// </summary>
        public void setRecogRateOption(int val)
		{
			mRecogRateOpt = val;
		}

        /// <summary>
        /// Sets the parameter defining the rate for the recognition to the
        /// supplied value.
        /// </summary>
        public void setRecogitionRate(int val)
		{
			mRecogRate = val;
		}

        /// <summary>
        /// Sets the parameter to define the mode of accuracy
        /// </summary>
        public void setRecogSpeedMode(string val)
		{
			mRecogSpeedMode = val;
		}

        /// <summary>
        /// Sets the number of matches to be recognized to the supplied value.
        /// </summary>
        public void setRecogManualSelection(int val)
		{
			mRecogManualSel = val;
		}

        /// <summary>
        /// Sets the parameter <c>NumMatches</c> to the supplied value.
        /// </summary>
        public void setInspectMaxNoMatchValue(int val)
		{
			mInspectMaxNoMatch = val;
		}


		/*******************************************************************/
		/*******************************************************************/           
		

        /// <summary>
        /// Checks if the parameter referenced by <c>mode</c> is 
        /// in the auto-mode list, i.e., that it is determined automatically
        /// </summary>
        /// <param name="mode">
        /// Constant starting with AUTO_*, describing one of the parameters
        /// for the auto-mode.
        /// </param>
        public bool isAuto(string mode)
		{
			bool isAuto = false;

			switch (mode)
			{
				case AUTO_ANGLE_STEP: 
					isAuto = paramAuto.Contains(AUTO_ANGLE_STEP);
					break;
				case AUTO_CONTRAST: 
					isAuto = paramAuto.Contains(AUTO_CONTRAST);
					break;
				case AUTO_MIN_CONTRAST: 
					isAuto = paramAuto.Contains(AUTO_MIN_CONTRAST);
					break;
				case AUTO_NUM_LEVEL: 
					isAuto = paramAuto.Contains(AUTO_NUM_LEVEL);
					break;
				case AUTO_OPTIMIZATION: 
					isAuto = paramAuto.Contains(AUTO_OPTIMIZATION);
					break;
				case AUTO_SCALE_STEP: 
					isAuto = paramAuto.Contains(AUTO_SCALE_STEP);
					break;
				default: break;
			}
			
			return isAuto;
		}

		/// <summary>
		/// Checks if any parameter is registered for automatic 
		/// determination. If not, the call for automatic
		/// determination can be skipped
		/// </summary>
		public bool isOnAuto()
		{
			if( paramAuto.Count > 0 )
				return true;
			else 
				return false;
		}

		/// <summary>
        /// Adds the parameter <c>val</c> to the list of parameters that 
        /// will be determined automatically before the application.
		/// </summary>
		/// <param name="val">
		/// Constant starting with AUTO_*, describing one of the parameters
        /// for the auto-mode.
		/// </param>
		/// <returns>
		/// Indicates whether the variable is already in auto-mode or
		/// was added to the auto-list successfully.
		/// </returns>
		public bool setAuto(string val)
		{
			string mode = "";

			switch (val)
			{
				case AUTO_ANGLE_STEP: 
					if(!paramAuto.Contains(AUTO_ANGLE_STEP))
						mode = AUTO_ANGLE_STEP;
					break;
				case AUTO_CONTRAST: 
					if(!paramAuto.Contains(AUTO_CONTRAST))
						mode = AUTO_CONTRAST;
					break;
				case AUTO_MIN_CONTRAST: 
					if(!paramAuto.Contains(AUTO_MIN_CONTRAST))
						mode = AUTO_MIN_CONTRAST;
					break;
				case AUTO_NUM_LEVEL: 
					if(!paramAuto.Contains(AUTO_NUM_LEVEL))
						mode = AUTO_NUM_LEVEL;
					break;
				case AUTO_OPTIMIZATION: 
					if(!paramAuto.Contains(AUTO_OPTIMIZATION))
						mode = AUTO_OPTIMIZATION;
					break;
				case AUTO_SCALE_STEP: 
					if(!paramAuto.Contains(AUTO_SCALE_STEP))
						mode = AUTO_SCALE_STEP;
					break;
				default: break;
			}
			
			if(mode == "")
				return false;
			
			paramAuto.Add(mode);
			return true;
		}

		/// <summary>
        /// Removes the parameter <c>val</c> from the list of parameters that 
        /// will be determined automatically.
        /// </summary>
        /// <param name="val">
        /// Constant starting with AUTO_*, describing one of the parameters for
        /// the auto-mode.
        /// </param>
        /// <returns>
        /// Indicates if the variable was removed from the 
        /// auto-list successfully.
        /// </returns>
		public bool removeAuto(string val)
		{
			string mode = "";

			switch (val)
			{
				case AUTO_ANGLE_STEP: 
					if(paramAuto.Contains(AUTO_ANGLE_STEP))
						mode = AUTO_ANGLE_STEP;
					break;
				case AUTO_CONTRAST: 
					if(paramAuto.Contains(AUTO_CONTRAST))
						mode = AUTO_CONTRAST;
					break;
				case AUTO_MIN_CONTRAST: 
					if(paramAuto.Contains(AUTO_MIN_CONTRAST))
						mode = AUTO_MIN_CONTRAST;
					break;
				case AUTO_NUM_LEVEL: 
					if(paramAuto.Contains(AUTO_NUM_LEVEL))
						mode = AUTO_NUM_LEVEL;
					break;
				case AUTO_OPTIMIZATION: 
					if(paramAuto.Contains(AUTO_OPTIMIZATION))
						mode = AUTO_OPTIMIZATION;
					break;
				case AUTO_SCALE_STEP: 
					if(paramAuto.Contains(AUTO_SCALE_STEP))
						mode = AUTO_SCALE_STEP;
					break;
				default: break;
			}
			
			if(mode == "")
				return false;
			
			paramAuto.Remove(mode);
			return true;
		}

		/// <summary>
		/// Gets the names of the parameters to be determined
		/// automatically
		/// </summary>
		/// <returns>
		/// List of parameter names being in auto-mode.
		/// </returns>
		public string [] getAutoParList()
		{
			int count = paramAuto.Count;
			string [] paramList = new string[count];

			for(int i=0; i<count; i++)
				paramList[i] = (string)paramAuto[i]; 
		
			return paramList;
		}

	}//end of class
}//end of namespace

using System;
using System.Collections;



namespace MatchingModule
{
	public delegate void StatisticsDelegate(int val);
	
    /// <summary>
    /// This class and its derived classes MatchingOptSpeed and 
    /// MatchingOptStatistics implement the optimization process for the 
    /// matching parameters in terms of the recognition speed and the 
    /// recognition rate. Similar to the processing in HDevelop, a timer 
    /// is used to be able to abort the processing during a run.
    /// </summary>
    public class MatchingOpt
	{
       
        /// <summary>
        /// Delegate to notify about the state of  the optimization process
        /// </summary>
		public	StatisticsDelegate NotifyStatisticsObserver;
		/// <summary>
		/// Information about the optimization process 
		/// (e.g. Success or Failure) to be displayed in the GUI
		/// </summary>
        public	string  statusString;		
        /// <summary>
        /// Statistics for the parameter optimization
        /// </summary>
		public string [] recogTabOptimizationData = new string[8];
		/// <summary>
		/// Statistics for the recognition rate
		/// </summary>
        public string [] inspectTabRecogRateData  = new string[5];
		/// <summary>
		/// Statistics of detection results for the optimal 
		/// recognition rate
		/// </summary>
        public string [] inspectTabStatisticsData = new string[21];

        /// <summary>
        /// Reference to instance of MatchingAssistant, 
        /// which triggers the optimization performance.
        /// </summary>
		public MatchingAssistant	mAssistant;
        /// <summary>
        /// Result of detection 
        /// </summary>
		public MatchingResult		mResults;
        /// <summary>
        /// Set of matching parameters
        /// </summary>
		public MatchingParam		mParams;
        /// <summary>
        /// Number of all test images to be inspected
        /// </summary>
		public int					tImageCount;
        /// <summary>
        /// Index of test image, being inspected currently
        /// </summary>
		public int				 	mCurrentIndex;
        /// <summary>
        /// Flag, indicating success or failure of optimization process
        /// </summary>
		public bool					mOptSuccess;

		public IEnumerator iterator;
		
        /// <summary>
        /// Constant describing a change in the status line
        /// </summary>
		public	const int UPDATE_RECOG_STATISTICS_STATUS= 21;
        /// <summary>
        /// Constant describing a change in the statistics of 
        /// the last recognition run
        /// </summary>
		public	const int UPDATE_RECOG_UPDATE_VALS		= 22;
        /// <summary>
        /// Constant describing a change in the statistics of 
        /// the optimal recognition run
        /// </summary>
		public  const int UPDATE_RECOG_OPTIMUM_VALS		= 23;
        /// <summary>
        /// Constant describing an error during the optimization 
        /// run, concerning the test image data or matching model
        /// </summary>
		public	const int UPDATE_TEST_ERR				= 24;
        /// <summary>
        /// Constant describing an error, which says that there is 
        /// no possible combination of matching parameters to obtain
        /// a detection result
        /// </summary>
		public	const int UPDATE_RECOG_ERR				= 25;

        /// <summary>
        /// Constant describing a change in the statistics of
        /// the recognition rate
        /// </summary>
		public	const int UPDATE_INSP_RECOGRATE			= 26;
        /// <summary>
        /// Constant describing a change in the statics of
        /// the average recognition results 
        /// </summary>
		public	const int UPDATE_INSP_STATISTICS		= 27;
        /// <summary>
        /// Constant describing an update of the 
        /// detection results
        /// </summary>
		public	const int UPDATE_TESTVIEW				= 28;
        /// <summary>
        /// Constant describing the success of the optimization 
        /// process and triggering the adjustment of the GUI
        /// components to the optimal parameter setting
        /// </summary>
		public  const int RUN_SUCCESSFUL				= 29;
        /// <summary>
        /// Constant describing the failure of the optimization
        /// process and reseting the matching parameters to the 
        /// initial setup
        /// </summary>
		public  const int RUN_FAILED					= 30;

		/// <summary> 
		/// Constructor 
		/// </summary>
		public MatchingOpt(){}

		/// <summary>  
		/// Performs an optimization step.
		/// </summary>
        public virtual bool ExecuteStep(){ return true;	}

		/// <summary>  
		/// Resets all parameters for evaluating the performance to their initial values.
		/// </summary>
		public virtual void reset(){}

        /// <summary>
        /// 
        /// </summary>
		public virtual void stop(){}
        
        public void dummy(int val) { }
	}//class
}//end of namespace

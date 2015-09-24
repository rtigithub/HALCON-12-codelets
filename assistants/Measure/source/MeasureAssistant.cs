using System;
using HalconDotNet;
using System.Collections;
using ViewROI;



namespace MeasureModule
{
	public delegate void MeasureDelegate(int value);

	/// <summary>
	/// The controller class MeasureAssistant controls
	/// the communication between the GUI and the model data that 
	/// is used to parameterize the measuring.
	/// </summary>
	public class MeasureAssistant
	{
		/// <summary>Index of the selected ROI.</summary>
		public  int             mActRoiIdx;

		/// <summary>Reference to the ROI controller instance.</summary>
		private  ROIController  roiController;

		/// <summary>List of measure handles created for the list of ROIs.</summary>
		private  ArrayList      mMeasureList;

		/// <summary>Reference to list of ROI instances.</summary>
		private  ArrayList      mROIList;

		/// <summary>HALCON image used for measuring.</summary>
		public  HImage      mImage;
		public  int         mWidth;
		public  int         mHeight;

		/// <summary>Flag indicating whether measurements are performed between 
		/// individual edges or edge pairs.</summary>
		public   bool       mSelPair;

		/// <summary>Minimum edge amplitude.</summary>
		public   double     mThresh;

		/// <summary>Sigma of Gaussian smoothing.</summary>
		public   double     mSigma;

		/// <summary>
		/// Width of the measure ROI (circular ROI: radius (half width) of the 
		/// annulus; linear ROI: half height of the rectangle).
		/// </summary>
		public   double     mRoiWidth;

		/// <summary>Default value for the minimum edge amplitude.</summary>
		public   double     mInitThresh;

		/// <summary>Default value for the sigma of Gaussian smoothing.</summary>
		public   double     mInitSigma;

		/// <summary>Default value for the ROI width.</summary>
		public   double     mInitRoiWidth;

		/// <summary>
		/// Type of gray-value transition; determines how edges are
		/// selected (dark-light or light-dark transition).
		/// </summary>
		public   string     mTransition;

		/// <summary>Position of edges to select.</summary>
		public   string     mPosition;

		/// <summary>Type of interpolation to be used.</summary>
		public   string     mInterpolation;

		/// <summary>
		/// Length of the displayed edges.
		/// </summary>
		public   int        mDispEdgeLength;

		/// <summary>Error messages for failed measure actions.</summary>
		public   HTuple     exceptionText;

		/// <summary>
		/// Flag indicating whether files with internal camera parameters
		/// and camera pose are present and valid.
		/// </summary>
		public    bool      mIsCalibValid;

		/// <summary>
		/// Flag indicating whether measure results should be transformed
		/// into world coordinates.
		/// </summary>
		public    bool      mTransWorldCoord;

		/// <summary>
		/// Flag indicating whether the ROI width should be used for
		/// displaying the extracted edges (instead of mDispEdgeLength).
		/// </summary>
		public    bool      mDispROIWidth;

		/// <summary>
		/// Flag to add position coordinates of the detected (and 
		/// selected) edges to the result table.
		/// </summary>
		public    bool      mDispPosition;

		/// <summary>
		/// Flag to add the pair width of the detected edge (and 
		/// selected) pairs to the result table.
		/// </summary>
		public    bool      mDispPairWidth;

		/// <summary>
		/// Flag to add the edge amplitude of the detected (and 
		/// selected) edge pairs to the result table.
		/// </summary>
		public    bool      mDispAmplitude;

		/// <summary>
		/// Flag to add the distance between consecutive detected (and
		/// selected) edges to the result table.
		/// </summary>
		public    bool      mDispDistance;

		/// <summary>File with internal camera parameters.</summary>
		public    HTuple    mCamParameter;

		/// <summary>Camera pose file.</summary>
		public    HTuple    mCamPose;

		/// <summary>Unit for transformation into world coordinates.</summary>
		public    string    mUnit;

		/// <summary>Constant indicating an update of measure instances.</summary>
		public const int    EVENT_UPDATE_MEASUREMENT = 2;

		/// <summary>
		/// Constant indicating the removal of measure instances from the
		/// measure list mMeasureList.
		/// </summary>
		public const int    EVENT_UPDATE_REMOVE      = 3;

		/// <summary>Constant indicating an update of measure results.</summary>
		public const int    EVENT_UPDATE_RESULT_XLD  = 4;

		/// <summary>Auxiliary constant.</summary>
		public const int    ALL_ROI                  =-1;

		/// <summary>
		/// Constant indicating an error while reading an image file.</summary>
		public const int    ERR_READING_FILE         = 10;


		public MeasureDelegate NotifyMeasureObserver;


		/* To create the measure controller class the ROI 
		   controller has to be provided for initialization */
		public MeasureAssistant(ROIController CRoi)
		{
			exceptionText = "";
			roiController = CRoi;
			mROIList = roiController.getROIList();
			mMeasureList = new ArrayList(15);
			mIsCalibValid = false;
			mActRoiIdx = -1;
		}

		/*********************************************************/
		/*                  setter-methods                       */
		/*********************************************************/

		/// <summary>
		/// Sets a flag to determine if measure detection is calculated
		/// for edges or edge pairs
		/// </summary>
		public void setSelPair(bool mode)
		{
			mSelPair = mode;
			UpdateMeasure();
			mActRoiIdx = roiController.getActiveROIIdx();
		}

		/// <summary>
		/// Sets the variable mThresh to the provided value and
		/// triggers an update for the measure results
		/// </summary>
		public void setMinEdgeAmpl(double val)
		{
			mThresh = val;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Sets the variable mSigma to the provided value and
		/// triggers an update for the measure results
		/// </summary>
		public void setSigma(double val)
		{
			mSigma = val;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Sets the variable mRoiWidth to the provided value and
		/// triggers an update for the measure results
		/// </summary>
		public void setRoiWidth(double val)
		{
			mRoiWidth = val;
			UpdateMeasure(ALL_ROI);
		}

		/// <summary>
		/// Sets the variable mInterpolation to the provided value and
		/// triggers an update for the measure results
		/// </summary>
		public void setInterpolation(string mode)
		{
			mInterpolation = mode;
			UpdateMeasure(ALL_ROI);
		}

		/// <summary>
		/// Sets the variable mTransition to the provided value and
		/// triggers an update for the measure results
		/// </summary>
		public void setTransition(string mode)
		{
			mTransition = mode;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Sets the position of edges to select to the provided value and
		/// triggers an update od the measure results.
		/// </summary>
		public void setPosition(string mode)
		{
			mPosition = mode;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Sets the length of the displayed edges to the provided value 
		/// and triggers an update of the measure results.
		/// </summary>
		public void setDispEdgeLength(int val)
		{
			mDispEdgeLength = val;
			UpdateXLD();
		}

		/// <summary>
		/// Sets the width of the displayed ROI to the provided value 
		/// and triggers an update for the measure results
		/// </summary>
		public void setFlagDispROIWidth(bool mode)
		{
			mDispROIWidth = mode;
			UpdateXLD();
		}

		/********************************************************/
		/* determine list of result coordinates to be displayed */
		/********************************************************/

		/// <summary>
		/// Sets a flag to determine if the edge positions of 
		/// measure results are added and displayed on the 
		/// measure result table
		/// </summary>
		public void showPosition(bool mode)
		{
			mDispPosition = mode;
		}

		/// <summary>
		/// Sets a flag to determine if the intra distance for 
		/// measure edge pairs are added and displayed on the 
		/// measure result table
		/// </summary>            
		public void showPairWidth(bool mode)
		{
			mDispPairWidth = mode;
		}

		/// <summary>
		/// Sets a flag to determine if the edge amplitude of 
		/// measure results are added and displayed on the 
		/// measure result table
		/// </summary>
		public void showAmplitude(bool mode)
		{
			mDispAmplitude = mode;
		}

		/// <summary>
		/// Sets a flag to determine if the edge distance of
		/// measure results are added and displayed on the 
		/// measure result table
		/// </summary>
		public void showDistance(bool mode)
		{
			mDispDistance = mode;
		}

		/// <summary>
		/// Sets the unit for world coordinate transformation to
		/// the provided value and triggers an update of the 
		/// measure results
		/// </summary>
		public void setUnit(string val)
		{
			mUnit = val;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Sets the variable mTransWorldCoord to the provided 
		/// value and triggers an update for the measure results
		/// </summary>
		public void setTransWorldCoord(bool mode)
		{
			mTransWorldCoord = mode;
			UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Load an HALCON image from the file filename and
		/// delete the list of measure handles created for the
		/// previous image setup
		/// </summary>
		/// <param name="filename">Location of image file</param>
		/// <returns>Flag depicting success of the load process</returns>
		public bool setImage(string filename)
		{
			string tmp;

			RemoveAllMeasureObjects();
			exceptionText = "";
			try
			{
				mImage = new HImage(filename);
				mImage.GetImagePointer1(out tmp, out mWidth, out mHeight);
			}
			catch (HOperatorException e)
			{
				mImage = null;
				exceptionText = e.Message;
				NotifyMeasureObserver(MeasureAssistant.ERR_READING_FILE);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Sets the variable mImage to the HImage provided 
		/// </summary>
		/// <param name="img">HALCON image instance</param>
		public void setImage(HImage img)
		{
			string tmp;

			RemoveAllMeasureObjects();
			mImage = img;

			if (mImage != null)
				mImage.GetImagePointer1(out tmp, out mWidth, out mHeight);

		}

		/// <summary>
		/// Returns the image currently used for measurement, described 
		/// by mImage
		/// </summary>
		public HImage getImage()
		{
			return mImage;
		}

		/// <summary>Load camera calibration data from file</summary>
		/// <param name="file">Location of *.cal file</param>
		public void LoadCamParFile(string file)
		{
			exceptionText = "";
			try
			{
				mCamParameter = null;
				mCamParameter = HMisc.ReadCamPar(file);
			}
			catch (HOperatorException e)
			{
				mIsCalibValid = false;
				exceptionText = e.Message;
				throw (e);
			}

			if (mCamParameter == null || !(mCamParameter.Length > 0) || mCamPose == null || !(mCamPose.Length > 0))
				mIsCalibValid = false;
			else
				mIsCalibValid = true;

			if (mIsCalibValid)
				UpdateExecute(ALL_ROI);
		}

		/// <summary>Load camera pose data from file</summary>
		/// <param name="file">Location of *.dat file</param>
		public void LoadCamPoseFile(string file)
		{
			exceptionText = "";
			try
			{
				HOperatorSet.ReadPose(new HTuple(file), out mCamPose);
			}
			catch (HOperatorException e)
			{
				mIsCalibValid = false;
				exceptionText = e.Message;
				throw (e);
			}

			if (mCamParameter == null || !(mCamParameter.Length > 0) || mCamPose == null || !(mCamPose.Length > 0))
				mIsCalibValid = false;
			else
				mIsCalibValid = true;


			if (mIsCalibValid)
				UpdateExecute(ALL_ROI);
		}

		/// <summary>
		/// Create a measure object for the new instance of an interactive 
		/// ROI at index mActRoiIdx
		/// </summary>
		public void AddMeasureObject()
		{
			mActRoiIdx = roiController.getActiveROIIdx();

			if (mActRoiIdx != -1)
				mMeasureList.Add(CreateMeasure((ROI)mROIList[mActRoiIdx], this));

			NotifyMeasureObserver(EVENT_UPDATE_MEASUREMENT);
		}

		/// <summary>
		/// Update measure object for the selected interactive ROI 
		/// at index or update measure handles for the 
		/// entire set of interactive ROIs
		/// </summary>
		public void UpdateMeasure(int index)
		{

			if (index == -1)
			{
				for (int i =0; i < mMeasureList.Count; i++)
					((Measurement)mMeasureList[i]).UpdateROI();
			}
			else
			{
				((Measurement)mMeasureList[index]).UpdateROI();
			}

			NotifyMeasureObserver(EVENT_UPDATE_MEASUREMENT);
		}

		/// <summary>
		/// Recreate the entire measure object list, if the  
		/// measure edge type changes from edge pose to
		/// edge pairs or vice versa
		/// </summary>
		public void UpdateMeasure()
		{
			RemoveAllMeasureObjects();
			for (int i =0; i < mROIList.Count; i++)
			{
				ROI roi = (ROI)mROIList[i];
				mMeasureList.Add(CreateMeasure((ROI)mROIList[i], this));
			}
			NotifyMeasureObserver(EVENT_UPDATE_MEASUREMENT);
		}

		/// <summary>
		/// Update measure results for a selected measure object instance
		/// at index or the entire set of measure handles.
		/// </summary>
		public void UpdateExecute(int index)
		{
			if (index == -1)
			{
				for (int i =0; i < mMeasureList.Count; i++)
					((Measurement)mMeasureList[i]).UpdateResults();
			}
			else
			{
				((Measurement)mMeasureList[index]).UpdateResults();
			}

			NotifyMeasureObserver(EVENT_UPDATE_MEASUREMENT);
		}

		/// <summary>
		/// Update display objects of the measure results 
		/// for the entire set of measure handles
		/// </summary>
		public void UpdateXLD()
		{
			for (int i =0; i < mMeasureList.Count; i++)
				((Measurement)mMeasureList[i]).UpdateXLD();

			NotifyMeasureObserver(EVENT_UPDATE_RESULT_XLD);
		}

		/// <summary>
		/// Clear the entire list of measure handles and 
		/// releases the resources used
		/// </summary>
		public void RemoveAllMeasureObjects()
		{
			int count = mMeasureList.Count;
			for (int i=0; i < count; i++)
				RemoveMeasureObjectIdx(0);

			NotifyMeasureObserver(EVENT_UPDATE_REMOVE);
		}

		/// <summary>
		/// Clear the  measure object corresponding to the deleted
		/// interactive ROI and release the resources used
		/// </summary>
		public void RemoveMeasureObjectActIdx()
		{
			int DelIdx = roiController.getDelROIIdx();
			RemoveMeasureObjectIdx(DelIdx);

			NotifyMeasureObserver(EVENT_UPDATE_REMOVE);
		}

		/// <summary>
		/// Clear the measure object at the index DelIdx from
		/// the measure object list and release the resources used
		/// </summary>
		private void RemoveMeasureObjectIdx(int DelIdx)
		{
			Measurement m = (Measurement)mMeasureList[DelIdx];
			mMeasureList.RemoveAt(DelIdx);
			m.ClearMeasurement();
			mActRoiIdx = -1;
		}

		/// <summary>
		/// Factory method to create measure objects
		/// </summary>
		/// <param name="roi">Interactive ROI</param>
		/// <param name="parent">
		/// Reference to measure controller class
		/// </param>
		/// <returns>New measure object</returns>
		public static Measurement CreateMeasure(ROI roi, MeasureAssistant parent)
		{
			if (parent.mSelPair)
				return new MeasurementPair(roi, parent);
			else
				return new MeasurementEdge(roi, parent);
		}

		/// <summary>
		/// Composes set of HALCON iconic objects to display 
		/// measure edge results
		/// </summary>
		/// <returns>Detected measure edges</returns> 
		public HXLDCont getMeasureResults()
		{
			HXLDCont val;
			HXLDCont obj = new HXLDCont();
			obj.GenEmptyObj();

			for (int i=0; i < mMeasureList.Count; i++)
			{
				val = ((Measurement)mMeasureList[i]).getMeasureResults();
				if (val.IsInitialized())
					obj = obj.ConcatObj(val);
			}
			return obj;
		}

		/// <summary>
		/// Composes set of HALCON iconic objects to display 
		/// dimension of measure object
		/// </summary>
		/// <returns>Surrounding contour of measure object</returns>
		public HObject getMeasureRegions()
		{
			HObject val;
			HObject obj = new HObject();
			obj.GenEmptyObj();

			for (int i=0; i < mMeasureList.Count; i++)
			{
				val = ((Measurement)mMeasureList[i]).getMeasureRegion();
				if (val.IsInitialized())
					obj = obj.ConcatObj(val);
			}
			return obj;
		}

		/// <summary>Returns number of measure objects</summary>
		/// <returns>Length of mMeasureList</returns>
		public int getMeasureListCount()
		{
			return mMeasureList.Count;
		}

		/// <summary>Change index for selected interactive ROI</summary>
		public void ClickedActiveROI()
		{
			mActRoiIdx = roiController.getActiveROIIdx();
		}

		/// <summary>
		/// Compose set of measure result data to display in the result table
		/// </summary>
		/// <returns>Composition of measure result information</returns>
		public ArrayList getMeasureTableData()
		{
			MeasureResult resultData;
			ArrayList table = new ArrayList(10);

			if (mMeasureList.Count == 0 || mActRoiIdx == -1)
				return table;

			resultData = ((Measurement)mMeasureList[mActRoiIdx]).getMeasureResultData();

			if (mSelPair)
			{
				if (mDispPosition)
				{
					table.Add(((PairResult)resultData).rowEdgeFirst);
					table.Add(((PairResult)resultData).colEdgeFirst);
					table.Add(((PairResult)resultData).rowEdgeSecond);
					table.Add(((PairResult)resultData).colEdgeSecond);
				}
				if (mDispAmplitude)
				{
					table.Add(((PairResult)resultData).amplitudeFirst);
					table.Add(((PairResult)resultData).amplitudeSecond);
				}
				if (mDispDistance)
					table.Add(((PairResult)resultData).interDistance);

				if (mDispPairWidth)
					table.Add(((PairResult)resultData).intraDistance);
			}
			else
			{
				if (mDispPosition)
				{
					table.Add(((EdgeResult)resultData).rowEdge);
					table.Add(((EdgeResult)resultData).colEdge);
				}
				if (mDispAmplitude)
					table.Add(((EdgeResult)resultData).amplitude);
				if (mDispDistance)
					table.Add(((EdgeResult)resultData).distance);
			}
			return table;
		}

		/// <summary>
		/// Compose the set of measure result types to be displayed 
		/// in the result table. The types are determined by the user,
		/// who can check the corresponding flags in the GUI frontend
		/// </summary>
		/// <returns>Composition of measure result types</returns>
		public ArrayList getMeasureResultComposition()
		{
			ArrayList composition = new ArrayList(5);

			if (mSelPair)
			{
				if (mDispPosition)
				{
					composition.Add("Row 1st");
					composition.Add("Col 1st");
					composition.Add("Row 2nd");
					composition.Add("Col 2nd");
				}
				if (mDispAmplitude)
				{
					composition.Add("Amplitude 1st");
					composition.Add("Amplitude 2nd");
				}
				if (mDispDistance)
					composition.Add("Distance");
				if (mDispPairWidth)
					composition.Add("Width");
			}
			else
			{
				if (mDispPosition)
				{
					composition.Add("Row");
					composition.Add("Column");
				}
				if (mDispAmplitude)
					composition.Add("Amplitude");
				if (mDispDistance)
					composition.Add("Distance");
			}
			return composition;
		}

		/// <summary>
		/// Determine the gray value projection for the measure object
		/// corresponding to the interactive ROI currently selected. 
		/// If no ROI is selected then return a null-object.
		/// </summary>
		/// <returns>Gray value profile</returns>
		public double[] getMeasureProjection()
		{
			if (mActRoiIdx == -1)
				return null;
			else
				return ((Measurement)mMeasureList[mActRoiIdx]).getGrayValueProj();
		}

	}//end of class
}//end of namespace

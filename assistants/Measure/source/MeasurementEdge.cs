using System;
using ViewROI;
using HalconDotNet;

namespace MeasureModule
{

	/// <summary>
	/// The class MeasurementEdge describes single-edge measurement
	/// and inherits from the base class Measurement. Virtual methods 
	/// defined in the base class are customized here to apply
	/// HALCON operators for single-edge extraction.
	/// </summary>
	public class MeasurementEdge : Measurement
	{
		/// <summary>
		/// Result container for the edge information returned
		/// by the HALCON measure operator.
		/// </summary>
		private EdgeResult mResult;
		/// <summary>
		/// Result container for the edge information converted
		/// into world coordinates. If calibration data is not available,
		/// the variable contains the same information as mResult.
		/// </summary>
		private EdgeResult mResultWorld;


		/// <summary>
		/// Creates a measurement object for the provided ROI instance.
		/// </summary>
		/// <param name="roi">ROI instance</param>
		/// <param name="mAssist">Reference to controller class</param>
		public MeasurementEdge(ROI roi, MeasureAssistant mAssist)
			: base(roi, mAssist)
		{
			mResult = new EdgeResult();
			mResultWorld = new EdgeResult();
			UpdateMeasure();
		}

		/// <summary>
		/// Triggers an update of the measure results because of  
		/// changes in the parameter setup or a recreation of the measure 
		/// object caused by an update in the ROI model.
		/// </summary>               
		public override void UpdateResults()
		{
			if (mHandle == null)
				return;

			mMeasAssist.exceptionText = "";

			try
			{
				mHandle.MeasurePos(mMeasAssist.mImage,
								   mMeasAssist.mSigma, mMeasAssist.mThresh,
								   mMeasAssist.mTransition, mMeasAssist.mPosition,
								   out mResult.rowEdge, out mResult.colEdge,
								   out mResult.amplitude, out mResult.distance);

				if (mMeasAssist.mIsCalibValid && mMeasAssist.mTransWorldCoord)
				{
					Rectify(mResult.rowEdge, mResult.colEdge, out mResultWorld.rowEdge, out mResultWorld.colEdge);
					mResultWorld.distance = Distance(mResult.rowEdge, mResult.colEdge, mResult.rowEdge, mResult.colEdge, 1);
					mResultWorld.amplitude = mResult.amplitude;
				}
				else
				{
					mResultWorld = new EdgeResult(mResult);
				}
			}
			catch (HOperatorException e)
			{
				mEdgeXLD.Dispose();
				mMeasAssist.exceptionText = e.Message;
				mResultWorld = new EdgeResult();
				return;
			}
			UpdateXLD();
		}

		/// <summary>Updates display object for measured edge results</summary>
		public override void UpdateXLD()
		{
			double width, phi, cRow, cCol, radius;

			if (mHandle == null && ((int)mHandle.Handle < 0))
				return;

			mMeasAssist.exceptionText = "";
			width = mMeasAssist.mDispROIWidth ? mMeasAssist.mRoiWidth : mMeasAssist.mDispEdgeLength;
			mEdgeXLD.Dispose();
			mEdgeXLD.GenEmptyObj();

			try
			{
				if (mROIType == ROI.ROI_TYPE_LINE)
				{
					phi = mMeasROI[2].D;

					for (int i = 0; i < mResult.rowEdge.Length; i++)
						mEdgeXLD = mEdgeXLD.ConcatObj(DetermineEdgeLine(mResult.rowEdge[i].D, mResult.colEdge[i].D, phi, width));

				}
				else if (mROIType == ROI.ROI_TYPE_CIRCLEARC)
				{
					cRow = mROICoord[0].D;
					cCol = mROICoord[1].D;
					radius = mROICoord[2].D;

					for (int i = 0; i < mResult.rowEdge.Length; i++)
						mEdgeXLD = mEdgeXLD.ConcatObj(DetermineEdgeCircularArc(mResult.rowEdge[i].D, mResult.colEdge[i].D, cRow, cCol, radius, width));
				}
			}
			catch (HOperatorException e)
			{
				mMeasAssist.exceptionText = e.Message;
			}
		}

		/// <summary>Returns measurement result.</summary>
		public override MeasureResult getMeasureResultData()
		{
			return mResultWorld;
		}

		/// <summary>Clears measurement result.</summary>
		public override void ClearResultData()
		{
			mResultWorld = new EdgeResult();
		}

	}//end of class
}//end of namespace

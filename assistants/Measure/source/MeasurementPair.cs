using System;
using ViewROI;
using HalconDotNet;

namespace MeasureModule
{

	/// <summary>
	/// The class MeasurementPair describes edge pair measurement
	/// and inherits from the base class Measurement. Virtual methods 
	/// defined in the base class are customized here to apply
	/// HALCON operators for edge pair measurement.
	/// </summary>
	public class MeasurementPair : Measurement
	{
		/// <summary>
		/// Result container for the edge information returned
		/// by the HALCON measure operator.
		/// </summary>
		private PairResult mResult;

		/// <summary>
		/// Result container for the edge information converted
		/// into world coordinates. If calibration data is not available,
		/// the variable contains the same information as mResult.
		/// </summary>
		private PairResult mResultWorld;

		/// <summary>
		/// Creates a measurement object for the provided ROI instance.
		/// </summary>
		/// <param name="roi">ROI instance</param>
		/// <param name="mAssist">Reference to controller class</param>
		public MeasurementPair(ROI roi, MeasureAssistant mAssist)
			: base(roi, mAssist)
		{
			mResult = new PairResult();
			mResultWorld = new PairResult();
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
				mHandle.MeasurePairs(mMeasAssist.mImage,
									 mMeasAssist.mSigma, mMeasAssist.mThresh,
									 mMeasAssist.mTransition, mMeasAssist.mPosition,
									 out mResult.rowEdgeFirst, out mResult.colEdgeFirst, out mResult.amplitudeFirst,
									 out mResult.rowEdgeSecond, out mResult.colEdgeSecond, out mResult.amplitudeSecond,
									 out mResult.intraDistance, out mResult.interDistance);

				if (mMeasAssist.mIsCalibValid && mMeasAssist.mTransWorldCoord)
				{
					Rectify(mResult.rowEdgeFirst, mResult.colEdgeFirst, out mResultWorld.rowEdgeFirst, out mResultWorld.colEdgeFirst);
					Rectify(mResult.rowEdgeSecond, mResult.colEdgeSecond, out mResultWorld.rowEdgeSecond, out mResultWorld.colEdgeSecond);

					mResultWorld.intraDistance = Distance(mResult.rowEdgeFirst, mResult.colEdgeFirst,
														  mResult.rowEdgeSecond, mResult.colEdgeSecond, 0);
					mResultWorld.interDistance = Distance(mResult.rowEdgeSecond, mResult.colEdgeSecond,
														  mResult.rowEdgeFirst, mResult.colEdgeFirst, 1);

					mResultWorld.amplitudeFirst = mResult.amplitudeFirst;
					mResultWorld.amplitudeSecond = mResult.amplitudeSecond;
				}
				else
				{
					mResultWorld = new PairResult(mResult);
				}
			}
			catch (HOperatorException e)
			{
				mEdgeXLD.Dispose();
				mMeasAssist.exceptionText = e.Message;
				mResultWorld = new PairResult();
				return;
			}
			UpdateXLD();
		}

		/// <summary>Updates display object for measured edges.</summary>
		public override void UpdateXLD()
		{
			HXLDCont val;
			double width, phi, cRow, cCol, radius, extent;

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

					for (int i = 0; i < mResult.rowEdgeFirst.Length; i++)
					{
						val = DetermineEdgeLine(mResult.rowEdgeFirst[i].D, mResult.colEdgeFirst[i].D, phi, width);
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
						val = DetermineEdgeLine(mResult.rowEdgeSecond[i].D, mResult.colEdgeSecond[i].D, phi, width);
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
						val = DetermineLine(mResult.rowEdgeFirst[i].D, mResult.colEdgeFirst[i].D,
											mResult.rowEdgeSecond[i].D, mResult.colEdgeSecond[i].D);
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
					}
				}
				else if (mROIType == ROI.ROI_TYPE_CIRCLEARC)
				{
					cRow = mROICoord[0].D;
					cCol = mROICoord[1].D;
					radius = mROICoord[2].D;
					extent = mROICoord[4].D;

					for (int i = 0; i < mResult.rowEdgeFirst.Length; i++)
					{
						val = DetermineEdgeCircularArc(mResult.rowEdgeFirst[i].D,
													   mResult.colEdgeFirst[i].D,
													   cRow, cCol, radius, width);
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
						val = DetermineEdgeCircularArc(mResult.rowEdgeSecond[i].D,
													   mResult.colEdgeSecond[i].D,
													   cRow, cCol, radius, width);
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
						val = DeterminePairCircularArc(mResult.rowEdgeFirst[i].D, mResult.colEdgeFirst[i].D,
														mResult.rowEdgeSecond[i].D, mResult.colEdgeSecond[i].D,
														cRow, cCol, radius, width, (extent >= 0));
						mEdgeXLD = mEdgeXLD.ConcatObj(val);
					}
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
			mResultWorld = new PairResult();
		}

	}//end of class
}//end of namespace

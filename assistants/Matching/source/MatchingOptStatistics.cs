using System;
using HalconDotNet;

namespace MatchingModule
{
	/// <summary>
	/// To determine the performance of a shape-based model, given
	/// a parameter setup for model creation and detection, 
	/// this class applies a model detection for the whole set
	/// of test images loaded and computes an all-over statistics.
	/// </summary>
    public class MatchingOptStatistics: MatchingOpt
	{
		// recognize - group 
		private	int 	mMatchesNumProb;
		private	bool	mModelFound;
		private	int		mSpecMatchesNum;
		private	int		mMaxMatchesNum;
		private	int		mFoundMatchesNum;
		private	int		mImagesWithOneMatchNum;
		private	int		mImagesWithSpecMatchesNum;
		private	int		mImagesWithMaxMatchesNum;

		// statistic - group 
		private double mScoreMin;	
		private double mScoreMax;
		private double mTimeMin;	
		private double mTimeMax;	
		private double mRowMin;		
		private double mRowMax;
		private double mColMin;		
		private double mColMax;
		private double mAngleMin;	
		private double mAngleMax;
		private double mScaleRowMin;
		private double mScaleRowMax;
		private double mScaleColMin;
		private double mScaleColMax;
	

        /// <summary>Constructor</summary>
        /// <param name="mAss">MatchingAssistant that created this instance</param>
        /// <param name="mPars">Current set of matching parameters</param>
		public MatchingOptStatistics(MatchingAssistant mAss, MatchingParam mPars)
		{
			mAssistant	= mAss;
			mParams		= mPars;
			NotifyStatisticsObserver = new StatisticsDelegate(dummy);	

			reset();
			tImageCount	= mAssistant.TestImages.Count;
		}

        /// <summary>
        /// With each execution step the shape-based model is searched in
        /// the current test image. The detection result is then compared 
        /// with the previous results and the overall statistics is adjusted.
        /// </summary>
		public override bool ExecuteStep()
		{	
			string fileName, imgNumStr;
			int val, i;
			string matchFormatStr;  
			int actualMatches;		
			int expectedMatches;	
			int maxNumMatches;		
			double score, time, row, col, angle, angleB, scaleR, scaleC;

			if(!iterator.MoveNext())
				return false;
			
			fileName = (string)iterator.Current;
			mAssistant.setTestImage(fileName);

			if(!(mOptSuccess = mAssistant.applyFindModel())) 
				return false;
			
			mResults	    = mAssistant.getMatchingResults();
			actualMatches   = mResults.count;

			// determine recognition rate ------------------ 
			expectedMatches = mParams.mRecogManualSel;
			maxNumMatches	= mParams.mNumMatches;
			
			mSpecMatchesNum	 += expectedMatches;
			mMaxMatchesNum   += maxNumMatches;
			mFoundMatchesNum +=	actualMatches;

			if(actualMatches > 0)
				mImagesWithOneMatchNum++;

			if(actualMatches >= expectedMatches)
				mImagesWithSpecMatchesNum++;

			if(actualMatches == maxNumMatches)
				mImagesWithMaxMatchesNum++;

			mCurrentIndex++;
			
			this.inspectTabRecogRateData[2] = "-";
			this.inspectTabRecogRateData[3] = "-";
			this.inspectTabRecogRateData[4] = "-";

			imgNumStr = " of " + mCurrentIndex + " images)"; 
			
			val = mImagesWithOneMatchNum;
			this.inspectTabRecogRateData[0] = Math.Round(100.0* ((double)val / mCurrentIndex), 2)  
														 + " %  ("  + val + imgNumStr;
			val = mImagesWithSpecMatchesNum;
			this.inspectTabRecogRateData[1] = Math.Round(100.0* ((double) val / mCurrentIndex), 2) 
														 + " %  ("  + val + imgNumStr;
			if(mMaxMatchesNum > 0)
			{
				matchFormatStr	=  " of " + mMaxMatchesNum + " models)";

				val = mImagesWithMaxMatchesNum;
				this.inspectTabRecogRateData[2] = Math.Round(100.0* ((double)val / mCurrentIndex), 2) 
															 + " %  (" + val + imgNumStr;
				val = mFoundMatchesNum;
				this.inspectTabRecogRateData[3] = Math.Round(100.0*((double) val / mMaxMatchesNum), 2) 
															 + " %  (" + val + matchFormatStr;
			}
			
			if(mSpecMatchesNum  > 0)
			{	
				matchFormatStr	=  " of " + mSpecMatchesNum + " models)";
				val = mFoundMatchesNum;
				this.inspectTabRecogRateData[4] = Math.Round(100.0* ((double)val / mSpecMatchesNum), 2) 
															 + " %  (" + val + matchFormatStr;
			}
			NotifyStatisticsObserver(MatchingOpt.UPDATE_INSP_RECOGRATE);

			// determine statistics data ------------ 
			if(actualMatches > 0)
			{
				i=0;
				if(!mModelFound)
				{
					mScoreMin	=	mScoreMax	= mResults.mScore[0].D;
					mTimeMin	=	mTimeMax	= mResults.mTime;
					mRowMin		=	mRowMax		= mResults.mRow[0].D;
					mColMin		=	mColMax		= mResults.mCol[0].D;
					mAngleMin	=	mAngleMax	= mResults.mAngle[0].D;
					mScaleRowMin=	mScaleRowMax= mResults.mScaleRow[0].D; 
					mScaleColMin=	mScaleColMax= mResults.mScaleCol[0].D; 
					mModelFound	=	true;
					i++;
				}
				
				for(; i < actualMatches; i++)
				{
					score	= mResults.mScore[i].D;
					if(score < mScoreMin)
						mScoreMin = score;
					else if(score > mScoreMax)
						mScoreMax = score;

					row		= mResults.mRow[i].D;
					if(row < mRowMin)
						mRowMin = row;
					else if(row > mRowMax)
						mRowMax	= row;

					col		= mResults.mCol[i].D;
					if(col < mColMin)
						mColMin = col;
					else if(col > mColMax)
						mColMax	= col;

					angle	= mResults.mAngle[i].D;
					if(angle < mAngleMin)
						mAngleMin = angle;
					else if(angle > mAngleMax)
						mAngleMax = angle;

					scaleR	= mResults.mScaleRow[i].D;
					if(scaleR < mScaleRowMin)
						mScaleRowMin = scaleR;
					else if(scaleR > mScaleRowMax)
						mScaleRowMax = scaleR;

					scaleC	= mResults.mScaleCol[i].D;
					if(scaleC < mScaleColMin)
						mScaleColMin = scaleC;
					else if(scaleC > mScaleColMax)
						mScaleColMax = scaleC;
				}//end of for

				time	= mResults.mTime;
				if(time < mTimeMin)
					mTimeMin = time;
				else if(time > mTimeMax)
					mTimeMax = time;
			}//end of if

			
			if(mModelFound)
			{	
				this.inspectTabStatisticsData[0]	= "" + Math.Round(mScoreMin, 2); 
				this.inspectTabStatisticsData[1]	= "" + Math.Round(mScoreMax, 2); 
				this.inspectTabStatisticsData[2]	= "" + Math.Round((mScoreMax - mScoreMin),2); 

				this.inspectTabStatisticsData[3]	= "" + Math.Round(mTimeMin, 2); 
				this.inspectTabStatisticsData[4]	= "" + Math.Round(mTimeMax, 2); 
				this.inspectTabStatisticsData[5]	= "" + Math.Round((mTimeMax - mTimeMin),2); 

				this.inspectTabStatisticsData[6]	= "" + Math.Round(mRowMin, 2);  
				this.inspectTabStatisticsData[7]	= "" + Math.Round(mRowMax, 2);  
				this.inspectTabStatisticsData[8]	= "" + Math.Round((mRowMax - mRowMin), 2); 

				this.inspectTabStatisticsData[9]	= "" + Math.Round(mColMin, 2);  
				this.inspectTabStatisticsData[10]	= "" + Math.Round(mColMax, 2); 
				this.inspectTabStatisticsData[11]	= "" + Math.Round((mColMax - mColMin), 2); 

				angle = (double)mAngleMin*180.0/Math.PI;
				angleB = (double)mAngleMax*180.0/Math.PI;
				this.inspectTabStatisticsData[12]	= "" + Math.Round(angle, 2)+ "°";  
				this.inspectTabStatisticsData[13]	= "" + Math.Round(angleB, 2) + "°"; 
				this.inspectTabStatisticsData[14]	= "" + Math.Round((angleB - angle), 2) + "°"; 

				this.inspectTabStatisticsData[15]	= "" + Math.Round(mScaleRowMin, 2); 
				this.inspectTabStatisticsData[16]	= "" + Math.Round(mScaleRowMax, 2); 
				this.inspectTabStatisticsData[17]	= "" + Math.Round((mScaleRowMax - mScaleRowMin),2);  

				this.inspectTabStatisticsData[18]	= "" + Math.Round(mScaleColMin, 2);  
				this.inspectTabStatisticsData[19]	= "" + Math.Round(mScaleColMax, 2); 
				this.inspectTabStatisticsData[20]	= "" + Math.Round((mScaleColMax - mScaleColMin), 2); 

				NotifyStatisticsObserver(MatchingOpt.UPDATE_INSP_STATISTICS);
			}
			return (mCurrentIndex < tImageCount);
		}


        /// <summary>
        /// Resets all parameters for evaluating the performance to their initial values.
        /// </summary>
		public override void reset()
		{
			mMatchesNumProb		= mParams.mNumMatches;
			mModelFound			= false;
			mSpecMatchesNum		= 0;
			mMaxMatchesNum		= 0;
			mFoundMatchesNum	= 0;
			mImagesWithOneMatchNum 		= 0;
			mImagesWithSpecMatchesNum	= 0;	
			mImagesWithMaxMatchesNum	= 0;
			mOptSuccess  = false;
		
			for(int i=0; i < 21;i++)
				this.inspectTabStatisticsData[i]="-";

			inspectTabRecogRateData[0] = "100.00 % (1 of 1  image)";
			inspectTabRecogRateData[1] = "100.00 % (1 of 1  image)";
			inspectTabRecogRateData[2] = "100.00 % (1 of 1  image)";
			inspectTabRecogRateData[3] = "100.00 % (1 of 1 model)";
			inspectTabRecogRateData[4] = "100.00 % (1 of 1 model)";

			tImageCount		= mAssistant.TestImages.Count;
			iterator		= mAssistant.TestImages.Keys.GetEnumerator();
			mCurrentIndex	= 0;
		}

        /// <summary>
        /// If the optimization has stopped, then check whether it was
        /// completed successfully or whether it was aborted 
        /// due to errors or to user interaction.
        /// Depending on the failure or success of the run, the GUI is notified
        /// for visual update of the results and obtained statistics.
        /// </summary>
		public override void stop()
		{
			if(tImageCount==0)
			{
				NotifyStatisticsObserver(MatchingAssistant.ERR_NO_TESTIMAGE);
			}
			else if(!mOptSuccess)
			{
				NotifyStatisticsObserver(MatchingOpt.UPDATE_TEST_ERR);
			}
		}
	}//end of class
}//end of namespace

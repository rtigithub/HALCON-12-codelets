using System;
using HalconDotNet;


namespace MatchingModule
{
    
    /// <summary>
    /// This class holds the result data from a model detection. For any new
    /// detection run, it also remembers the time needed
    /// for the model detection. 
    /// </summary>
    public class MatchingResult
	{
		
        /// <summary>
        /// Model contour applied for model detection
        /// </summary>
		public HXLDCont  mContour;
        /// <summary>
        /// All model contours detected
        /// </summary>
		public HXLDCont  mContResults;

        /// <summary>
        /// Row coordinate of the found instances of the model
        /// </summary>
		public HTuple mRow;
        /// <summary>
        /// Column coordinate of the found instances of the model
        /// </summary>
		public HTuple mCol;

        /// <summary>
        /// Rotation angle of the found instances of the model
        /// </summary>
		public HTuple mAngle;
        /// <summary>
        /// Scale of the found instances of the model in the row direction
        /// </summary>
		public HTuple mScaleRow;
        /// <summary>
        /// Scale of the found instances of the model in the column direction
        /// </summary>
		public HTuple mScaleCol;
        /// <summary>
        /// Score of the found instances of the model
        /// </summary>
		public HTuple mScore;
        /// <summary>
        /// Time needed to detect <c>count</c> numbers of model instances
        /// </summary>
		public double mTime;
        /// <summary>
        /// Number of model instances found
        /// </summary>
		public int	  count;
        /// <summary>
        /// 2D homogeneous transformation matrix that can be used to transform
        /// data from the model into the test image.
        /// </summary>
		public HHomMat2D hmat;
        
        /// <summary>Constructor</summary>
		public MatchingResult()
		{
			hmat = new HHomMat2D();
			mContResults = new HXLDCont();
		}


        /// <summary>
        /// Gets the detected contour.
        /// </summary>
        /// <returns>Detected contour</returns>
		public HXLDCont getDetectionResults()
		{
			HXLDCont rContours = new HXLDCont();
			hmat.HomMat2dIdentity();
			mContResults.GenEmptyObj();

			for(int i = 0; i<count; i++) 
			{
				hmat.VectorAngleToRigid(0, 0, 0, mRow[i].D, mCol[i].D, mAngle[i].D);
				rContours = hmat.AffineTransContourXld(mContour);
				mContResults =  mContResults.ConcatObj(rContours);
			}
			return mContResults;
		}


        /// <summary>
        /// Resets the detection results and sets count to 0.
        /// </summary>
		public void reset()
		{
			count = 0;
		}

	}//end of class
}//end of namespace

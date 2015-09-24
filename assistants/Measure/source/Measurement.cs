using System;
using HalconDotNet;
using ViewROI;

namespace MeasureModule
{
	/// <summary>
	/// This model data class holds general information about a measure
	/// object. To define more specialized measure data objects the
	/// classes MeasurementEdge and MeasurementPair are derived from this
	/// class to be able to distinguish the measure
	/// types and results for simple edges and for edge pairs. The base
	/// class contains the methods to create measure handles and to create
	/// the display results of a measure action. The derived classes
	/// implement the specialized methods to perform a measure operation
	/// and generate the measure results.
	/// </summary>
	public class Measurement
	{
		/// <summary>HALCON measure object (handle).</summary>
		protected HMeasure    mHandle;

		/// <summary>
		/// Flag to distinguish between a linear and circular measure object.
		/// </summary>
		protected   int       mROIType;

		/// <summary>
		/// Model coordinates obtained from the ROI instance.
		/// This coordinates are the base to calculate the measuring field.
		/// </summary>
		protected   HTuple    mROICoord;

		/// <summary>Reference to the ROI.</summary>
		protected   ROI       mRoi;

		/// <summary>
		/// Auxiliary information about the
		/// measuring field calculated using the ROI coordinates.
		/// </summary>
		protected   HTuple    mMeasROI;

		/// <summary>Iconic object to display measure edge results.</summary>
		protected   HXLDCont  mEdgeXLD;

		/// <summary>Iconic object to display mMeasROI.</summary>
		protected   HRegion   mMeasureRegion;

		/// <summary>
		/// Reference to controller class to obtain information about
		/// the context of the measurement, e.g. image, parameter setups etc.
		/// </summary>
		protected MeasureAssistant mMeasAssist;

		/// <summary>
		/// Creates and initializes a measure object based on information the ROI object
		/// about the ROI.
		/// </summary>
		/// <param name="roi">ROI instance</param>
		/// <param name="mAssist">Reference to controller class</param>
		public Measurement(ROI roi, MeasureAssistant mAssist)
		{
			mRoi = roi;
			mMeasAssist = mAssist;
			mROICoord = mRoi.getModelData();
			mEdgeXLD = new HXLDCont();
			mMeasureRegion = new HRegion();

			if (mRoi is ROICircularArc)
				mROIType = ROI.ROI_TYPE_CIRCLEARC;
			else
				mROIType = ROI.ROI_TYPE_LINE;
		}

		/// <summary>
		/// Triggers an update of the measure object for all 
		/// changes concerning the shape of the ROI or the measurement
		/// parameters.
		/// </summary>
		public void UpdateROI()
		{
			mROICoord = mRoi.getModelData();
			UpdateMeasure();
		}

		/// <summary>
		/// Creates a measure object based on the model data
		/// defined by the ROI instance and the parameters
		/// describing the measure context.
		/// </summary>
		protected void UpdateMeasure()
		{
			double extent, sPhi, radius;

			if (mHandle != null)
				mHandle.Dispose();

			mMeasAssist.exceptionText = "";

			try
			{
				switch (mROIType)
				{
					case ROI.ROI_TYPE_CIRCLEARC:

						radius = mROICoord[2].D;
						sPhi = mROICoord[3].D;
						extent = mROICoord[4].D;


						mMeasROI = GenSurCircle(mROICoord, mMeasAssist.mRoiWidth);
						mHandle = new HMeasure(mROICoord[0].D, mROICoord[1].D, radius,
												sPhi, extent,
												mMeasAssist.mRoiWidth,
												mMeasAssist.mWidth, mMeasAssist.mHeight,
												mMeasAssist.mInterpolation);

						break;
					case ROI.ROI_TYPE_LINE:

						mMeasROI = GenSurRect2(mROICoord, mMeasAssist.mRoiWidth);
						mHandle = new HMeasure(mMeasROI[0].D, mMeasROI[1].D,
											   mMeasROI[2].D, mMeasROI[3].D, mMeasROI[4].D,
											   mMeasAssist.mWidth, mMeasAssist.mHeight,
											   mMeasAssist.mInterpolation);
						break;
				}
			}
			catch (HOperatorException e)
			{
				mEdgeXLD.Dispose();
				mMeasureRegion.Dispose();
				mMeasAssist.exceptionText = e.Message;
				ClearResultData();
				return;
			}
			UpdateResults();
			UpdateMeasureRegion();
		}

		/// <summary>
		/// Defines the measuring field of a linear ROI.
		/// </summary>
		/// <param name="line">
		/// Model data for a linear interactive ROI
		/// </param>
		/// <param name="width">Half width of (rectangular) measure ROI</param>
		/// <returns>Model data describing a linear measuring field</returns>
		private HTuple GenSurRect2(HTuple line, double width)
		{
			double row1 = line[0];
			double col1 = line[1];
			double row2 = line[2];
			double col2 = line[3];

			double phi     = HMisc.AngleLx(row1, col1, row2, col2);
			double length1 = (HMisc.DistancePp(row1, col1, row2, col2)) / 2.0;
			double length2 = width;
			double rowM    = (row1 + row2) / 2;
			double colM    = (col1 + col2) / 2;

			return new HTuple(new double[] { rowM, colM, phi, length1, length2 });
		}

		/// <summary>
		/// Defines the measuring field of a circular ROI.
		/// </summary>
		/// <param name="circle">
		/// Model data for a circular ROI
		/// </param>
		/// <param name="width">Radius (half width) of the circular (ring-shaped) ROI</param>
		/// <returns>Model data describing circular measuring field</returns>
		private HTuple GenSurCircle(HTuple circle, double width)
		{
			double row1   = circle[0];
			double col1   = circle[1];
			double radius = circle[2];
			double startPhi  = circle[3];
			double extent    = circle[4];

			if (radius <= width)
				return new HTuple(new double[] { row1, col1, startPhi, extent, (radius + width), 0.0 });
			else
				return new HTuple(new double[] { row1, col1, startPhi, extent, (radius + width), (radius - width) });
		}

		/// <summary>Releases resources used for this measure object.</summary>
		public void ClearMeasurement()
		{
			if (mHandle != null)
			{
				mHandle.Dispose();
				mEdgeXLD.Dispose();
				mMeasureRegion.Dispose();
			}
		}

		/// <summary>
		/// Creates the iconic object for displaying a measured
		/// edge in a linear ROI.
		/// </summary>
		/// <returns>Contour depicting the measured edge</returns>
		public static HXLDCont DetermineEdgeLine(double row, double col, double phi, double width)
		{
			double row1, row2, col1, col2;

			row1 = row - width * Math.Sin(phi + 0.5 * Math.PI);
			col1 = col + width * Math.Cos(phi + 0.5 * Math.PI);
			row2 = row - width * Math.Sin(phi + 1.5 * Math.PI);
			col2 = col + width * Math.Cos(phi + 1.5 * Math.PI);
			return DetermineLine(row1, col1, row2, col2);
		}

		/// <summary>
		/// Creates the iconic object for displaying a measured
		/// edge from the coordinates of the end points.
		/// </summary>
		/// <returns>Contour depicting the measured edge</returns>
		public static HXLDCont DetermineLine(double row1, double col1, double row2, double col2)
		{
			HXLDCont edge = new HXLDCont();
			HTuple rows, cols;

			rows = new HTuple(new double[] { row1, row2 });
			cols = new HTuple(new double[] { col1, col2 });

			edge.GenContourPolygonXld(rows, cols);
			return edge;
		}

		/// <summary>
		/// Creates the iconic object for displaying a measured
		/// edge in a circular ROI.
		/// </summary>
		/// <returns>Edge contour depicting a measured edge</returns>
		public static HXLDCont DetermineEdgeCircularArc(double row, double col, double center_row, double center_col, double radius, double width)
		{
			double row1, row2, col1, col2;

			row1 = row + (center_row - row) / radius * width;
			col1 = col + (center_col - col) / radius * width;
			row2 = row - (center_row - row) / radius * width;
			col2 = col - (center_col - col) / radius * width;

			return DetermineLine(row1, col1, row2, col2);
		}

		/// <summary>
		/// Creates the iconic object for displaying
		/// the arc between edges of a pair.
		/// </summary>
		/// <returns>Edge contour depicting the arc between edges of a pair</returns>
		public static HXLDCont DeterminePairCircularArc(double row1, double col1, double row2, double col2, double cRow, double cCol, double radius, double width, bool positive)
		{
			HXLDCont arc = new HXLDCont();
			double startPhi, endPhi;

			startPhi = Math.Atan2(cRow - row1, col1 - cCol);
			endPhi = Math.Atan2(cRow - row2, col2 - cCol);

			arc.GenEllipseContourXld(cRow, cCol, 0.0, radius, radius, startPhi, endPhi, (positive ? "positive" : "negative"), 1.5);
			return arc;
		}

		/// <summary>
		/// Creates an iconic object depicting the 
		/// measuring field.
		/// </summary>
		public void UpdateMeasureRegion()
		{

			mMeasureRegion.Dispose();
			mMeasureRegion.GenEmptyObj();

			if (mROIType == ROI.ROI_TYPE_CIRCLEARC)
			{
				double sPhi, extent, innerRad, outerRad;
				HTuple innerR, outerR, innerC, outerC;
				HXLDCont outCont, innerCont, contour;

				outCont = new HXLDCont();
				innerCont = new HXLDCont();

				sPhi = mMeasROI[2].D;
				extent = mMeasROI[3].D;
				outerRad = mMeasROI[4].D;
				innerRad = mMeasROI[5].D;


				innerCont.GenCircleContourXld(mMeasROI[0].D, mMeasROI[1].D, innerRad, sPhi, (sPhi + extent), (extent > 0) ? "positive" : "negative", 1.0);
				outCont.GenCircleContourXld(mMeasROI[0].D, mMeasROI[1].D, outerRad, (sPhi + extent), sPhi, (extent > 0) ? "negative" : "positive", 1.0);

				innerCont.GetContourXld(out innerR, out innerC);
				outCont.GetContourXld(out outerR, out outerC);
				innerR = innerR.TupleConcat(outerR);
				innerC = innerC.TupleConcat(outerC);

				contour = new HXLDCont(innerR, innerC);
				contour = contour.CloseContoursXld();

				mMeasureRegion = contour.GenRegionContourXld("margin");

				contour.Dispose();
				innerCont.Dispose();
				outCont.Dispose();
			}
			else
			{
				mMeasureRegion.GenRectangle2(mMeasROI[0].D, mMeasROI[1].D,
											 mMeasROI[2].D, mMeasROI[3].D,
											 mMeasROI[4].D);
			}
		}

		/// <summary>
		/// If calibration data is available and valid, then rectify 
		/// measure result coordinates, otherwise leave them the same
		/// </summary>
		public void Rectify(HTuple row, HTuple col, out HTuple rowRect, out HTuple colRect)
		{
			double unitScale=0.0;

			if (mMeasAssist.mIsCalibValid)
			{
				switch (mMeasAssist.mUnit)
				{
					case "µm":
						unitScale = 0.000001;
						break;
					case "mm":
						unitScale = 0.001;
						break;
					case "cm":
						unitScale = 0.01;
						break;
					case "m":
						unitScale = 1.0;
						break;
					default:
						break;
				}

				HOperatorSet.ImagePointsToWorldPlane(mMeasAssist.mCamParameter,
													 mMeasAssist.mCamPose,
													 row, col, new HTuple(unitScale),
													 out colRect, out rowRect);
			}
			else
			{
				rowRect = row;
				colRect = col;
			}

		}

		/// <summary>
		/// If calibration data is available and valid, then transform the
		/// distance between measure result edges into world coordinates,
		/// else leave them the same.
		/// </summary>
		public HTuple Distance(HTuple row1, HTuple col1, HTuple row2, HTuple col2, int shift)
		{
			HTuple rows, cols, rowRect, colRect;
			HTuple distance = new HTuple();
			HXLDCont contour;


			if (shift == 0)
			{
				if (mROIType == ROI.ROI_TYPE_CIRCLEARC)
				{
					double cRow, cCol, radius, extent, phi1, phi2, phi, res, length, tmp;

					cRow = mROICoord[0].D;
					cCol = mROICoord[1].D;
					radius = mROICoord[2].D;
					extent = mROICoord[4].D;

					HOperatorSet.TupleGenConst(new HTuple(row1.Length), 0.0, out distance);

					for (int i=0; i < distance.Length; i++)
					{
						phi1 = HMisc.AngleLx(cRow, cCol, row1[i].D, col1[i].D);
						phi2 = HMisc.AngleLx(cRow, cCol, row2[i].D, col2[i].D);

						if (extent < 0)
						{
							tmp = phi1;
							phi1 = phi2;
							phi2 = tmp;
						}

						phi = phi2 - phi1;

						if (phi < 0)
							phi += 2 * Math.PI;

						res = 0.05 * 24.0 / (radius * phi);

						contour = new HXLDCont();
						contour.GenEllipseContourXld(cRow, cCol, 0, radius, radius, phi1, phi2, "positive", res);
						contour.GetContourXld(out rows, out cols);
						Rectify(rows, cols, out rowRect, out colRect);
						contour.Dispose();
						contour.GenContourPolygonXld(rowRect, colRect);
						length = contour.LengthXld();
						distance[i].D = length;
						contour.Dispose();
					}

				}
				else if (mROIType == ROI.ROI_TYPE_LINE)
				{
					HTuple rRect1, cRect1, rRect2, cRect2;
					Rectify(row1, col1, out rRect1, out cRect1);
					Rectify(row2, col2, out rRect2, out cRect2);
					distance = HMisc.DistancePp(rRect1, cRect1, rRect2, cRect2);
				}
				return distance;

			}
			else
			{
				HTuple rClip1, cClip1, rShift2, cShift2;

				rClip1 = row1.TupleSelectRange(new HTuple(0), new HTuple(row1.Length - shift - 1));
				cClip1 = col1.TupleSelectRange(new HTuple(0), new HTuple(col1.Length - shift - 1));
				rShift2 = row2.TupleSelectRange(new HTuple(shift), new HTuple(row2.Length - 1));
				cShift2 = col2.TupleSelectRange(new HTuple(shift), new HTuple(col2.Length - 1));

				return this.Distance(rClip1, cClip1, rShift2, cShift2, 0);
			}
		}

		/// <summary>
		/// Returns the iconic object describing the measuring field
		/// of the measure object.
		/// </summary>
		public HRegion getMeasureRegion()
		{
			return mMeasureRegion;
		}

		/// <summary>
		/// Returns the iconic object depicting the measured edges.
		/// </summary>
		public HXLDCont getMeasureResults()
		{
			return mEdgeXLD;
		}

		/// <summary>
		/// Returns the gray-value profile obtained by the measure projection
		/// operation.
		/// </summary>
		/// <returns>Gray-value profile</returns>
		public double[] getGrayValueProj()
		{
			HTuple grayVal;

			if (mHandle == null || (int)mHandle.Handle < 0)
				return null;

			grayVal = mHandle.MeasureProjection(mMeasAssist.mImage);
			return grayVal.ToDArr();
		}

		/********************************************************/
		/********************************************************/
		/// <summary>
		/// Virtual method to be implemented by the derived classes.
		/// </summary>
		public virtual void UpdateResults() { }
		/// <summary>
		/// Virtual method to be implemented by the derived classes.
		/// </summary>
		public virtual void UpdateXLD() { }
		/// <summary>
		/// Virtual method to be implemented by the derived classes.
		/// </summary>
		public virtual void ClearResultData() { }
		/// <summary>
		/// Virtual method to be implemented by the derived classes.
		/// </summary>
		public virtual MeasureResult getMeasureResultData()
		{
			return null;
		}

	}//end of class
}//end of namespace



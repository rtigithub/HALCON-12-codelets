using System;
using HalconDotNet;

/// <summary>
/// This class contains all procedures computing the quality of the 
/// calibration images. At first the region plate and the marks must
/// be determined using the procedure call <c>find_caltab_edges</c>.
/// If a plate region and marks were detected in the calibration image,
/// the quality of the specified calibration plate can be measured for 
/// each image in terms of:
/// - homogeneity of illumination
/// - contrast and homogeneity of the marks
/// - the area covered by the plate in the image
/// - the sharpness of the plate
/// 
/// An all over quality of the sequence of calibration images
/// can also be determined using the following properties:
/// - number of calibration images used 
/// - distribution of the calibration marks in the volume
///   determined by the camera
/// - amount of tilt used for the calibration plates
/// - all over quality performance described by the
///   lowest quality score of the image set
/// </summary>
public class QualityProcedures
{
  /// <summary>
  /// Evaluates the sharpness of the calibration plate in the calibration
  /// image.
  /// </summary>
  public void eval_caltab_focus (HObject ho_Image, 
                                 HObject ho_Marks,
                                 HTuple hv_Contrast,
                                 out HTuple hv_FocusScore)
  {
    // Local iconic variables 

    HObject ho_Region, ho_RegionUnion, ho_ImageReduced;
    HObject ho_DerivGauss;

    // Local control variables 

    HTuple hv_Number, hv_MeanGradient, hv_Deviation;
    HTuple hv_MinScore, hv_RawResult;

    // Initialize local and output iconic variables 
    HOperatorSet.GenEmptyObj(out ho_Region);
    HOperatorSet.GenEmptyObj(out ho_RegionUnion);
    HOperatorSet.GenEmptyObj(out ho_ImageReduced);
    HOperatorSet.GenEmptyObj(out ho_DerivGauss);

    hv_FocusScore = 0.0;
    if ((int)(new HTuple(hv_Contrast.TupleEqual(0))) != 0)
    {
      ho_Region.Dispose();
      ho_RegionUnion.Dispose();
      ho_ImageReduced.Dispose();
      ho_DerivGauss.Dispose();

      return;
    }
    HOperatorSet.CountObj(ho_Marks, out hv_Number);
    if ((int)(new HTuple(hv_Number.TupleLess(3))) != 0)
    {
      ho_Region.Dispose();
      ho_RegionUnion.Dispose();
      ho_ImageReduced.Dispose();
      ho_DerivGauss.Dispose();

      return;
    }
    ho_Region.Dispose();
    HOperatorSet.GenRegionContourXld(ho_Marks, out ho_Region, "margin");
    ho_RegionUnion.Dispose();
    HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
    ho_ImageReduced.Dispose();
    HOperatorSet.ReduceDomain(ho_Image, ho_RegionUnion, out ho_ImageReduced);
    ho_DerivGauss.Dispose();
    HOperatorSet.DerivateGauss(ho_ImageReduced, out ho_DerivGauss, 0.7, "gradient");
    HOperatorSet.Intensity(ho_Region, ho_DerivGauss, out hv_MeanGradient, out hv_Deviation);
    hv_MinScore = 0.25;
    //Normalize the Gradient with the contrast
    hv_RawResult = hv_MeanGradient / hv_Contrast;
    hv_FocusScore = (((hv_RawResult * 4.5)).TupleSort())[(new HTuple((new HTuple(hv_RawResult.TupleLength()
        )) / 20.0)).TupleRound()];
    hv_FocusScore = ((((((((hv_FocusScore - hv_MinScore)).TupleConcat(0.0))).TupleMax()
        )).TupleConcat(1.0))).TupleMin();
    ho_Region.Dispose();
    ho_RegionUnion.Dispose();
    ho_ImageReduced.Dispose();
    ho_DerivGauss.Dispose();

    return;
  }



  /// <summary>
  /// Extracts the calibration plate and the marks on this plate
  /// for the supplied image
  /// </summary>
  public void find_caltab_edges (HObject ho_Image, 
                                 out HObject ho_Caltab, 
                                 out HObject ho_Marks,
                                 HTuple hv_DescriptionFileName)
  {


      // Stack for temporary objects 
      HObject[] OTemp = new HObject[20];

      // Local iconic variables 

      HObject ho_ImageMean, ho_RegionDynThresh, ho_RegionBorder;
      HObject ho_RegionOpening1, ho_ConnectedRegions1, ho_SelectedRegions4;
      HObject ho_SelectedRegions5, ho_RegionBorder2, ho_RegionTrans;
      HObject ho_RegionErosion, ho_RegionBorder1, ho_RegionDilation2;
      HObject ho_RegionDifference1, ho_RegionOpening, ho_ConnectedRegions;
      HObject ho_SelectedRegions2, ho_SelectedRegions, ho_RegionFillUp;
      HObject ho_SelectedRegions1, ho_RegionIntersection, ho_RegionFillUp1;
      HObject ho_RegionDifference, ho_CaltabCandidates, ho_ObjectSelected = null;
      HObject ho_ConnectedMarks = null, ho_ObjectSelectedCaltab = null;
      HObject ho_RegionFillUpCand, ho_MarksCand, ho_RegionDilation1;
      HObject ho_ImageReduced, ho_DefaultEdges, ho_UnionContours;
      HObject ho_SelectedXLD, ho_SelectedXLD1;


      // Local control variables 

      HTuple hv_Width, hv_Height, hv_EstimatedCaltabSize;
      HTuple hv_EstimatedMarkSize, hv_Number, hv_X, hv_Y, hv_Z;
      HTuple hv_NumDescrMarks, hv_Index, hv_NumberMarks = new HTuple();
      HTuple hv_Anisometry = new HTuple(), hv_Bulkiness = new HTuple();
      HTuple hv_StructureFactor = new HTuple(), hv_AreaMarks = new HTuple();
      HTuple hv_Row = new HTuple(), hv_Column = new HTuple(), hv_Rectangularity;
      HTuple hv_SortedIndex, hv_IndexBest, hv_MinContrast, hv_NumberCand;
      HTuple hv_Area, hv_Dummy, hv_DummyS, hv_AreaMedian;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_Caltab);
      HOperatorSet.GenEmptyObj(out ho_Marks);
      HOperatorSet.GenEmptyObj(out ho_ImageMean);
      HOperatorSet.GenEmptyObj(out ho_RegionDynThresh);
      HOperatorSet.GenEmptyObj(out ho_RegionBorder);
      HOperatorSet.GenEmptyObj(out ho_RegionOpening1);
      HOperatorSet.GenEmptyObj(out ho_ConnectedRegions1);
      HOperatorSet.GenEmptyObj(out ho_SelectedRegions4);
      HOperatorSet.GenEmptyObj(out ho_SelectedRegions5);
      HOperatorSet.GenEmptyObj(out ho_RegionBorder2);
      HOperatorSet.GenEmptyObj(out ho_RegionTrans);
      HOperatorSet.GenEmptyObj(out ho_RegionErosion);
      HOperatorSet.GenEmptyObj(out ho_RegionBorder1);
      HOperatorSet.GenEmptyObj(out ho_RegionDilation2);
      HOperatorSet.GenEmptyObj(out ho_RegionDifference1);
      HOperatorSet.GenEmptyObj(out ho_RegionOpening);
      HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
      HOperatorSet.GenEmptyObj(out ho_SelectedRegions2);
      HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
      HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
      HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
      HOperatorSet.GenEmptyObj(out ho_RegionIntersection);
      HOperatorSet.GenEmptyObj(out ho_RegionFillUp1);
      HOperatorSet.GenEmptyObj(out ho_RegionDifference);
      HOperatorSet.GenEmptyObj(out ho_CaltabCandidates);
      HOperatorSet.GenEmptyObj(out ho_ObjectSelected);
      HOperatorSet.GenEmptyObj(out ho_ConnectedMarks);
      HOperatorSet.GenEmptyObj(out ho_ObjectSelectedCaltab);
      HOperatorSet.GenEmptyObj(out ho_RegionFillUpCand);
      HOperatorSet.GenEmptyObj(out ho_MarksCand);
      HOperatorSet.GenEmptyObj(out ho_RegionDilation1);
      HOperatorSet.GenEmptyObj(out ho_ImageReduced);
      HOperatorSet.GenEmptyObj(out ho_DefaultEdges);
      HOperatorSet.GenEmptyObj(out ho_UnionContours);
      HOperatorSet.GenEmptyObj(out ho_SelectedXLD);
      HOperatorSet.GenEmptyObj(out ho_SelectedXLD1);

      //
      ho_Marks.Dispose();
      HOperatorSet.GenEmptyObj(out ho_Marks);
      ho_Caltab.Dispose();
      HOperatorSet.GenEmptyObj(out ho_Caltab);
      HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
      hv_EstimatedCaltabSize = (((((hv_Width.TupleConcat(hv_Height))).TupleMax()) / 2.5)).TupleRound()
          ;
      hv_EstimatedMarkSize = ((hv_EstimatedCaltabSize / 12.0)).TupleRound();
      ho_ImageMean.Dispose();
      HOperatorSet.MeanImage(ho_Image, out ho_ImageMean, hv_EstimatedMarkSize * 3, hv_EstimatedMarkSize * 3);
      ho_RegionDynThresh.Dispose();
      HOperatorSet.DynThreshold(ho_Image, ho_ImageMean, out ho_RegionDynThresh, 3,
          "light");
      ho_RegionBorder.Dispose();
      HOperatorSet.DynThreshold(ho_Image, ho_ImageMean, out ho_RegionBorder, 20, "dark");
      ho_RegionOpening1.Dispose();
      HOperatorSet.OpeningCircle(ho_RegionBorder, out ho_RegionOpening1, 1.5);
      ho_ConnectedRegions1.Dispose();
      HOperatorSet.Connection(ho_RegionOpening1, out ho_ConnectedRegions1);
      ho_SelectedRegions4.Dispose();
      HOperatorSet.SelectShape(ho_ConnectedRegions1, out ho_SelectedRegions4, "compactness",
          "and", 17, 50);
      ho_SelectedRegions5.Dispose();
      HOperatorSet.SelectShape(ho_SelectedRegions4, out ho_SelectedRegions5, "anisometry",
          "and", 1, 1.4);
      ho_RegionBorder2.Dispose();
      HOperatorSet.Boundary(ho_SelectedRegions5, out ho_RegionBorder2, "outer");
      ho_SelectedRegions5.Dispose();
      HOperatorSet.SelectShape(ho_RegionBorder2, out ho_SelectedRegions5, "circularity",
          "and", 0.006, 1);
      ho_RegionTrans.Dispose();
      HOperatorSet.ShapeTrans(ho_SelectedRegions5, out ho_RegionTrans, "rectangle2");
      ho_RegionErosion.Dispose();
      HOperatorSet.ErosionCircle(ho_RegionTrans, out ho_RegionErosion, (hv_Width / 640.0) * 5.5);
      ho_RegionBorder1.Dispose();
      HOperatorSet.Boundary(ho_RegionErosion, out ho_RegionBorder1, "inner");
      ho_RegionDilation2.Dispose();
      HOperatorSet.DilationCircle(ho_RegionBorder1, out ho_RegionDilation2, 3.5);
      ho_RegionDifference1.Dispose();
      HOperatorSet.Difference(ho_RegionDynThresh, ho_RegionDilation2, out ho_RegionDifference1
          );
      ho_RegionOpening.Dispose();
      HOperatorSet.OpeningCircle(ho_RegionDifference1, out ho_RegionOpening, (hv_Width / 640) * 1.5);
      ho_ConnectedRegions.Dispose();
      HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
      ho_SelectedRegions2.Dispose();
      HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions2, "area",
          "and", (hv_EstimatedCaltabSize.TuplePow(2)) / 10, (hv_EstimatedCaltabSize.TuplePow(
          2)) * 5);
      ho_SelectedRegions.Dispose();
      HOperatorSet.SelectShape(ho_SelectedRegions2, out ho_SelectedRegions, "compactness",
          "and", 1.4, 10);
      ho_RegionFillUp.Dispose();
      HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
      ho_SelectedRegions1.Dispose();
      HOperatorSet.SelectShape(ho_RegionFillUp, out ho_SelectedRegions1, "rectangularity",
          "and", 0.6, 1);
      ho_RegionIntersection.Dispose();
      HOperatorSet.Intersection(ho_SelectedRegions1, ho_RegionDynThresh, out ho_RegionIntersection
          );
      ho_RegionFillUp1.Dispose();
      HOperatorSet.FillUp(ho_RegionIntersection, out ho_RegionFillUp1);
      ho_RegionDifference.Dispose();
      HOperatorSet.Difference(ho_RegionFillUp1, ho_RegionIntersection, out ho_RegionDifference
          );
      HOperatorSet.CountObj(ho_RegionDifference, out hv_Number);
      ho_CaltabCandidates.Dispose();
      HOperatorSet.GenEmptyObj(out ho_CaltabCandidates);
      HOperatorSet.CaltabPoints(hv_DescriptionFileName, out hv_X, out hv_Y, out hv_Z);
      hv_NumDescrMarks = new HTuple(hv_X.TupleLength());
      for (hv_Index = 1; hv_Index.Continue(hv_Number, 1); hv_Index = hv_Index.TupleAdd(1))
      {
          ho_ObjectSelected.Dispose();
          HOperatorSet.SelectObj(ho_RegionDifference, out ho_ObjectSelected, hv_Index);
          ho_ConnectedMarks.Dispose();
          HOperatorSet.Connection(ho_ObjectSelected, out ho_ConnectedMarks);
          HOperatorSet.CountObj(ho_ConnectedMarks, out hv_NumberMarks);
          HOperatorSet.Eccentricity(ho_ConnectedMarks, out hv_Anisometry, out hv_Bulkiness,
              out hv_StructureFactor);
          HOperatorSet.AreaCenter(ho_ConnectedMarks, out hv_AreaMarks, out hv_Row, out hv_Column);
          ho_ObjectSelectedCaltab.Dispose();
          HOperatorSet.SelectObj(ho_RegionIntersection, out ho_ObjectSelectedCaltab,
              hv_Index);
          if ((int)((new HTuple((new HTuple((new HTuple(hv_NumberMarks.TupleGreaterEqual(
              10))).TupleAnd(new HTuple(hv_NumberMarks.TupleLess(hv_NumDescrMarks + 20))))).TupleAnd(
              new HTuple(((((hv_Anisometry.TupleSort())).TupleSelect((new HTuple(hv_Anisometry.TupleLength()
              )) / 2))).TupleLess(2))))).TupleAnd(new HTuple((new HTuple(hv_AreaMarks.TupleMean()
              )).TupleGreater(20)))) != 0)
          {
              HOperatorSet.ConcatObj(ho_CaltabCandidates, ho_ObjectSelectedCaltab, out OTemp[0]
                  );
              ho_CaltabCandidates.Dispose();
              ho_CaltabCandidates = OTemp[0];
          }
      }
      ho_RegionFillUpCand.Dispose();
      HOperatorSet.FillUp(ho_CaltabCandidates, out ho_RegionFillUpCand);
      HOperatorSet.Rectangularity(ho_RegionFillUpCand, out hv_Rectangularity);
      if ((int)(new HTuple((new HTuple(hv_Rectangularity.TupleLength())).TupleEqual(
          0))) != 0)
      {
          ho_ImageMean.Dispose();
          ho_RegionDynThresh.Dispose();
          ho_RegionBorder.Dispose();
          ho_RegionOpening1.Dispose();
          ho_ConnectedRegions1.Dispose();
          ho_SelectedRegions4.Dispose();
          ho_SelectedRegions5.Dispose();
          ho_RegionBorder2.Dispose();
          ho_RegionTrans.Dispose();
          ho_RegionErosion.Dispose();
          ho_RegionBorder1.Dispose();
          ho_RegionDilation2.Dispose();
          ho_RegionDifference1.Dispose();
          ho_RegionOpening.Dispose();
          ho_ConnectedRegions.Dispose();
          ho_SelectedRegions2.Dispose();
          ho_SelectedRegions.Dispose();
          ho_RegionFillUp.Dispose();
          ho_SelectedRegions1.Dispose();
          ho_RegionIntersection.Dispose();
          ho_RegionFillUp1.Dispose();
          ho_RegionDifference.Dispose();
          ho_CaltabCandidates.Dispose();
          ho_ObjectSelected.Dispose();
          ho_ConnectedMarks.Dispose();
          ho_ObjectSelectedCaltab.Dispose();
          ho_RegionFillUpCand.Dispose();
          ho_MarksCand.Dispose();
          ho_RegionDilation1.Dispose();
          ho_ImageReduced.Dispose();
          ho_DefaultEdges.Dispose();
          ho_UnionContours.Dispose();
          ho_SelectedXLD.Dispose();
          ho_SelectedXLD1.Dispose();

          return;
      }
      hv_SortedIndex = hv_Rectangularity.TupleSortIndex();
      hv_IndexBest = (((hv_SortedIndex.TupleInverse())).TupleSelect(0)) + 1;
      ho_Caltab.Dispose();
      HOperatorSet.SelectObj(ho_RegionFillUpCand, out ho_Caltab, hv_IndexBest);
      ho_RegionFillUp.Dispose();
      HOperatorSet.FillUp(ho_Caltab, out ho_RegionFillUp);
      ho_MarksCand.Dispose();
      HOperatorSet.Difference(ho_RegionFillUp, ho_RegionDynThresh, out ho_MarksCand
          );
      ho_RegionBorder.Dispose();
      HOperatorSet.Boundary(ho_MarksCand, out ho_RegionBorder, "inner");
      ho_RegionDilation1.Dispose();
      HOperatorSet.DilationCircle(ho_RegionBorder, out ho_RegionDilation1, 9.5);
      ho_ImageReduced.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_RegionDilation1, out ho_ImageReduced);
      hv_MinContrast = 10;
      ho_DefaultEdges.Dispose();
      HOperatorSet.EdgesSubPix(ho_ImageReduced, out ho_DefaultEdges, "canny", 2, hv_MinContrast / 2,
          hv_MinContrast);
      HOperatorSet.CountObj(ho_DefaultEdges, out hv_NumberCand);
      if ((int)(new HTuple(hv_NumberCand.TupleLess(10))) != 0)
      {
          ho_ImageMean.Dispose();
          ho_RegionDynThresh.Dispose();
          ho_RegionBorder.Dispose();
          ho_RegionOpening1.Dispose();
          ho_ConnectedRegions1.Dispose();
          ho_SelectedRegions4.Dispose();
          ho_SelectedRegions5.Dispose();
          ho_RegionBorder2.Dispose();
          ho_RegionTrans.Dispose();
          ho_RegionErosion.Dispose();
          ho_RegionBorder1.Dispose();
          ho_RegionDilation2.Dispose();
          ho_RegionDifference1.Dispose();
          ho_RegionOpening.Dispose();
          ho_ConnectedRegions.Dispose();
          ho_SelectedRegions2.Dispose();
          ho_SelectedRegions.Dispose();
          ho_RegionFillUp.Dispose();
          ho_SelectedRegions1.Dispose();
          ho_RegionIntersection.Dispose();
          ho_RegionFillUp1.Dispose();
          ho_RegionDifference.Dispose();
          ho_CaltabCandidates.Dispose();
          ho_ObjectSelected.Dispose();
          ho_ConnectedMarks.Dispose();
          ho_ObjectSelectedCaltab.Dispose();
          ho_RegionFillUpCand.Dispose();
          ho_MarksCand.Dispose();
          ho_RegionDilation1.Dispose();
          ho_ImageReduced.Dispose();
          ho_DefaultEdges.Dispose();
          ho_UnionContours.Dispose();
          ho_SelectedXLD.Dispose();
          ho_SelectedXLD1.Dispose();

          return;
      }
      ho_UnionContours.Dispose();
      HOperatorSet.UnionCocircularContoursXld(ho_DefaultEdges, out ho_UnionContours,
          0.5, 0.1, 0.2, 30, 10, 10, "true", 1);
      ho_SelectedXLD.Dispose();
      HOperatorSet.SelectShapeXld(ho_UnionContours, out ho_SelectedXLD, "area", "and",
          30, 10000);
      ho_SelectedXLD1.Dispose();
      HOperatorSet.SelectShapeXld(ho_SelectedXLD, out ho_SelectedXLD1, "circularity",
          "and", 0.4, 1);
      ho_MarksCand.Dispose();
      HOperatorSet.SelectShapeXld(ho_SelectedXLD1, out ho_MarksCand, "compactness",
          "and", 1, 1.5);
      HOperatorSet.AreaCenterXld(ho_MarksCand, out hv_Area, out hv_Dummy, out hv_Dummy,
          out hv_DummyS);
      HOperatorSet.CountObj(ho_MarksCand, out hv_Number);
      if ((int)(new HTuple(hv_Number.TupleLess(4))) != 0)
      {
          ho_ImageMean.Dispose();
          ho_RegionDynThresh.Dispose();
          ho_RegionBorder.Dispose();
          ho_RegionOpening1.Dispose();
          ho_ConnectedRegions1.Dispose();
          ho_SelectedRegions4.Dispose();
          ho_SelectedRegions5.Dispose();
          ho_RegionBorder2.Dispose();
          ho_RegionTrans.Dispose();
          ho_RegionErosion.Dispose();
          ho_RegionBorder1.Dispose();
          ho_RegionDilation2.Dispose();
          ho_RegionDifference1.Dispose();
          ho_RegionOpening.Dispose();
          ho_ConnectedRegions.Dispose();
          ho_SelectedRegions2.Dispose();
          ho_SelectedRegions.Dispose();
          ho_RegionFillUp.Dispose();
          ho_SelectedRegions1.Dispose();
          ho_RegionIntersection.Dispose();
          ho_RegionFillUp1.Dispose();
          ho_RegionDifference.Dispose();
          ho_CaltabCandidates.Dispose();
          ho_ObjectSelected.Dispose();
          ho_ConnectedMarks.Dispose();
          ho_ObjectSelectedCaltab.Dispose();
          ho_RegionFillUpCand.Dispose();
          ho_MarksCand.Dispose();
          ho_RegionDilation1.Dispose();
          ho_ImageReduced.Dispose();
          ho_DefaultEdges.Dispose();
          ho_UnionContours.Dispose();
          ho_SelectedXLD.Dispose();
          ho_SelectedXLD1.Dispose();

          return;
      }
      hv_AreaMedian = ((hv_Area.TupleSort())).TupleSelect(hv_Number / 2);
      ho_Marks.Dispose();
      HOperatorSet.SelectShapeXld(ho_MarksCand, out ho_Marks, "area", "and", hv_AreaMedian - (hv_AreaMedian * 0.5),
          hv_AreaMedian + (hv_AreaMedian * 0.5));
      ho_ImageMean.Dispose();
      ho_RegionDynThresh.Dispose();
      ho_RegionBorder.Dispose();
      ho_RegionOpening1.Dispose();
      ho_ConnectedRegions1.Dispose();
      ho_SelectedRegions4.Dispose();
      ho_SelectedRegions5.Dispose();
      ho_RegionBorder2.Dispose();
      ho_RegionTrans.Dispose();
      ho_RegionErosion.Dispose();
      ho_RegionBorder1.Dispose();
      ho_RegionDilation2.Dispose();
      ho_RegionDifference1.Dispose();
      ho_RegionOpening.Dispose();
      ho_ConnectedRegions.Dispose();
      ho_SelectedRegions2.Dispose();
      ho_SelectedRegions.Dispose();
      ho_RegionFillUp.Dispose();
      ho_SelectedRegions1.Dispose();
      ho_RegionIntersection.Dispose();
      ho_RegionFillUp1.Dispose();
      ho_RegionDifference.Dispose();
      ho_CaltabCandidates.Dispose();
      ho_ObjectSelected.Dispose();
      ho_ConnectedMarks.Dispose();
      ho_ObjectSelectedCaltab.Dispose();
      ho_RegionFillUpCand.Dispose();
      ho_MarksCand.Dispose();
      ho_RegionDilation1.Dispose();
      ho_ImageReduced.Dispose();
      ho_DefaultEdges.Dispose();
      ho_UnionContours.Dispose();
      ho_SelectedXLD.Dispose();
      ho_SelectedXLD1.Dispose();

      return;
  }




  /// <summary>
  /// Determines whether the calibration image is overexposed
  /// </summary>
  public void eval_caltab_overexposure (HObject ho_Image, 
                                        HObject ho_Caltab, 
                                        out HTuple hv_OverexposureScore)
  {

      // Local iconic variables 

      HObject ho_ImageReduced, ho_Region;


      // Local control variables 

      HTuple hv_AreaCaltab, hv_Row, hv_Column, hv_AreaOverExp;
      HTuple hv_Thresh, hv_Ratio;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_ImageReduced);
      HOperatorSet.GenEmptyObj(out ho_Region);

      //returns a measure of the amount of saturation of the plate
      hv_OverexposureScore = 0.0;
      HOperatorSet.AreaCenter(ho_Caltab, out hv_AreaCaltab, out hv_Row, out hv_Column);
      if ((int)((new HTuple(hv_AreaCaltab.TupleEqual(0))).TupleOr(new HTuple(hv_AreaCaltab.TupleEqual(
          new HTuple())))) != 0)
      {
          ho_ImageReduced.Dispose();
          ho_Region.Dispose();

          return;
      }
      ho_ImageReduced.Dispose();
      HOperatorSet.ReduceDomain(ho_Image, ho_Caltab, out ho_ImageReduced);
      ho_Region.Dispose();
      HOperatorSet.Threshold(ho_ImageReduced, out ho_Region, 255, 255);
      HOperatorSet.AreaCenter(ho_Region, out hv_AreaOverExp, out hv_Row, out hv_Column);
      hv_Thresh = 0.15;
      hv_Ratio = (hv_AreaOverExp.TupleReal()) / hv_AreaCaltab;
      if ((int)(new HTuple(hv_Ratio.TupleLess(hv_Thresh))) != 0)
      {
          hv_OverexposureScore = (((new HTuple(1.0)).TupleConcat(1 - (hv_Ratio / hv_Thresh)))).TupleMin()
              ;
      }
      ho_ImageReduced.Dispose();
      ho_Region.Dispose();

      return;
  }


  /// <summary>
  /// Evaluates the gray value contrast between the marks and the calibration 
  /// plate and the homogeneity of the used illumination.
  /// </summary>
  public void eval_caltab_contrast_homogeneity (HObject ho_Image, 
                                                HObject ho_Marks,
                                                out HTuple hv_Contrast, 
                                                out HTuple hv_ContrastScore, 
                                                out HTuple hv_HomogeneityScore)
  {

      // Local iconic variables 

      HObject ho_Region, ho_RegionDilation;


      // Local control variables 

      HTuple hv_Number, hv_Min, hv_Max, hv_Range;
      HTuple hv_MinContrast, hv_MaxContrast, hv_DeviationMax;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_Region);
      HOperatorSet.GenEmptyObj(out ho_RegionDilation);

      hv_ContrastScore = 0.0;
      hv_Contrast = 0.0;
      hv_HomogeneityScore = 0.0;
      HOperatorSet.CountObj(ho_Marks, out hv_Number);
      if ((int)(new HTuple(hv_Number.TupleLess(4))) != 0)
      {
          ho_Region.Dispose();
          ho_RegionDilation.Dispose();

          return;
      }
      ho_Region.Dispose();
      HOperatorSet.GenRegionContourXld(ho_Marks, out ho_Region, "margin");
      ho_RegionDilation.Dispose();
      HOperatorSet.DilationCircle(ho_Region, out ho_RegionDilation, 5.5);
      HOperatorSet.MinMaxGray(ho_RegionDilation, ho_Image, 3, out hv_Min, out hv_Max,
          out hv_Range);
      //Calculate contrast score
      hv_Contrast = hv_Range.TupleMean();
      hv_MinContrast = 70;
      hv_MaxContrast = 160;
      if ((int)(new HTuple(hv_Contrast.TupleGreater(hv_MinContrast))) != 0)
      {
          hv_ContrastScore = (hv_Contrast - hv_MinContrast) / (hv_MaxContrast - hv_MinContrast);
          hv_ContrastScore = ((hv_ContrastScore.TupleConcat(1.0))).TupleMin();
      }
      //Now for the homogeneity score
      HOperatorSet.TupleDeviation(hv_Max, out hv_DeviationMax);
      hv_HomogeneityScore = 1.1 - (hv_DeviationMax / 40.0);
      hv_HomogeneityScore = ((((((hv_HomogeneityScore.TupleConcat(1.0))).TupleMin()
          )).TupleConcat(0.0))).TupleMax();
      ho_Region.Dispose();
      ho_RegionDilation.Dispose();

      return;

  }

  /// <summary>
  /// Evaluates the area covered by the calibration plate in the calibration
  /// image.
  /// </summary>
  public void eval_caltab_size (HObject ho_Image, 
                                HObject ho_Caltab, 
                                HObject ho_Marks,
                                out HTuple hv_SizeScore)
  {


      // Local iconic variables 

      HObject ho_Region = null, ho_RegionUnion = null;


      // Local control variables 

      HTuple hv_Width, hv_Height, hv_Number, hv_Row1 = new HTuple();
      HTuple hv_Column1 = new HTuple(), hv_Phi1 = new HTuple(), hv_Length1 = new HTuple();
      HTuple hv_Length2 = new HTuple(), hv_Area = new HTuple(), hv_Row = new HTuple();
      HTuple hv_Column = new HTuple(), hv_MinRatio, hv_MaxRatio;
      HTuple hv_Ratio;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_Region);
      HOperatorSet.GenEmptyObj(out ho_RegionUnion);

      hv_SizeScore = 0.0;
      HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
      HOperatorSet.CountObj(ho_Marks, out hv_Number);
      if ((int)(new HTuple(hv_Number.TupleGreaterEqual(4))) != 0)
      {
          //Best approach: Use the surrounding box of the marks as reference size
          ho_Region.Dispose();
          HOperatorSet.GenRegionContourXld(ho_Marks, out ho_Region, "filled");
          ho_RegionUnion.Dispose();
          HOperatorSet.Union1(ho_Region, out ho_RegionUnion);
          HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_Row1, out hv_Column1,
              out hv_Phi1, out hv_Length1, out hv_Length2);
          hv_Area = (hv_Length2 * hv_Length1) * 4;
      }
      else
      {
          //If no marks could be found: use the caltab as reference size
          HOperatorSet.AreaCenter(ho_Caltab, out hv_Area, out hv_Row, out hv_Column);
          if ((int)((new HTuple(hv_Area.TupleEqual(0))).TupleOr(new HTuple(hv_Area.TupleEqual(
              new HTuple())))) != 0)
          {
              ho_Region.Dispose();
              ho_RegionUnion.Dispose();

              return;
          }
      }
      hv_MinRatio = 0.015;
      hv_MaxRatio = 0.075;
      hv_Ratio = (hv_Area.TupleReal()) / (hv_Width * hv_Height);
      if ((int)(new HTuple(hv_Ratio.TupleGreater(hv_MinRatio))) != 0)
      {
          hv_SizeScore = (hv_Ratio - hv_MinRatio) / (hv_MaxRatio - hv_MinRatio);
          hv_SizeScore = (((new HTuple(1.0)).TupleConcat(hv_SizeScore))).TupleMin();
      }
      ho_Region.Dispose();
      ho_RegionUnion.Dispose();

      return;
  }


  /// <summary>
  /// Display the axes of a 3d coordinate system.
  /// </summary>
  private void disp_3d_coord_system (HTuple hv_WindowHandle, 
                                    HTuple hv_CamParam, 
                                    HTuple hv_Pose,
                                    HTuple hv_CoordAxesLength)
  {

    // Local iconic variables 

    HObject ho_ContX, ho_ContY, ho_ContZ;


    // Local control variables 

    HTuple hv_TransWorld2Cam, hv_OrigCamX, hv_OrigCamY;
    HTuple hv_OrigCamZ, hv_Row0, hv_Column0, hv_X, hv_Y, hv_Z;
    HTuple hv_RowAxX, hv_ColumnAxX, hv_RowAxY, hv_ColumnAxY;
    HTuple hv_RowAxZ, hv_ColumnAxZ;

    // Initialize local and output iconic variables 
    HOperatorSet.GenEmptyObj(out ho_ContX);
    HOperatorSet.GenEmptyObj(out ho_ContY);
    HOperatorSet.GenEmptyObj(out ho_ContZ);

    if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
    {
      ho_ContX.Dispose();
      ho_ContY.Dispose();
      ho_ContZ.Dispose();

      return;
    }
    if ((int)(new HTuple(((hv_Pose.TupleSelect(5))).TupleEqual(0.0))) != 0)
    {
      ho_ContX.Dispose();
      ho_ContY.Dispose();
      ho_ContZ.Dispose();

      return;
    }
    HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
    //Project the world origin into the image
    HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
        out hv_OrigCamY, out hv_OrigCamZ);
    HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
        out hv_Row0, out hv_Column0);
    //Project the coordinate axes into the image
    HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
        out hv_X, out hv_Y, out hv_Z);
    HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
    HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
        out hv_X, out hv_Y, out hv_Z);
    HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
    HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
        out hv_X, out hv_Y, out hv_Z);
    HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
    ho_ContX.Dispose();
    gen_arrow_cont(out ho_ContX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX);
    ho_ContY.Dispose();
    gen_arrow_cont(out ho_ContY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY);
    ho_ContZ.Dispose();
    gen_arrow_cont(out ho_ContZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ);
    if (HDevWindowStack.IsOpen())
    {
      //dev_display (ContX)
    }
    if (HDevWindowStack.IsOpen())
    {
      //dev_display (ContY)
    }
    if (HDevWindowStack.IsOpen())
    {
      //dev_display (ContZ)
    }
    HOperatorSet.DispObj(ho_ContX, hv_WindowHandle);
    HOperatorSet.DispObj(ho_ContY, hv_WindowHandle);
    HOperatorSet.DispObj(ho_ContZ, hv_WindowHandle);
    HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxX + 3, hv_ColumnAxX + 3);
    HOperatorSet.WriteString(hv_WindowHandle, "X");
    HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxY + 3, hv_ColumnAxY + 3);
    HOperatorSet.WriteString(hv_WindowHandle, "Y");
    HOperatorSet.SetTposition(hv_WindowHandle, hv_RowAxZ + 3, hv_ColumnAxZ + 3);
    HOperatorSet.WriteString(hv_WindowHandle, "Z");
    ho_ContX.Dispose();
    ho_ContY.Dispose();
    ho_ContZ.Dispose();

    return;
  }

  /// <summary>
  /// Generate a contour in form of an arrow.
  /// </summary>
  private void gen_arrow_cont(out HObject ho_Arrow, 
                              HTuple hv_Row1, 
                              HTuple hv_Column1,
                              HTuple hv_Row2, 
                              HTuple hv_Column2)
  {

    // Local iconic variables 

    HObject ho_Cross1, ho_Cross2, ho_CrossP1, ho_CrossP2;


    // Local control variables 

    HTuple hv_Length, hv_Angle, hv_MinArrowLength;
    HTuple hv_DRow, hv_DCol, hv_ArrowLength, hv_Phi, hv_P1R;
    HTuple hv_P1C, hv_P2R, hv_P2C;

    // Initialize local and output iconic variables 
    HOperatorSet.GenEmptyObj(out ho_Arrow);
    HOperatorSet.GenEmptyObj(out ho_Cross1);
    HOperatorSet.GenEmptyObj(out ho_Cross2);
    HOperatorSet.GenEmptyObj(out ho_CrossP1);
    HOperatorSet.GenEmptyObj(out ho_CrossP2);

    //Generate a contour in form of a arrow
    hv_Length = 7;
    hv_Angle = 40;
    hv_MinArrowLength = 2;
    hv_DRow = hv_Row2 - hv_Row1;
    hv_DCol = hv_Column2 - hv_Column1;
    hv_ArrowLength = (((hv_DRow * hv_DRow) + (hv_DCol * hv_DCol))).TupleSqrt();
    if ((int)(new HTuple(hv_ArrowLength.TupleLess(hv_MinArrowLength))) != 0)
    {
      hv_Length = 0;
    }
    HOperatorSet.TupleAtan2(hv_DRow, -hv_DCol, out hv_Phi);
    hv_P1R = hv_Row2 - (hv_Length * (((hv_Phi - (hv_Angle.TupleRad()))).TupleSin()));
    hv_P1C = hv_Column2 + (hv_Length * (((hv_Phi - (hv_Angle.TupleRad()))).TupleCos()));
    hv_P2R = hv_Row2 - (hv_Length * (((hv_Phi + (hv_Angle.TupleRad()))).TupleSin()));
    hv_P2C = hv_Column2 + (hv_Length * (((hv_Phi + (hv_Angle.TupleRad()))).TupleCos()));
    ho_Cross1.Dispose();
    HOperatorSet.GenCrossContourXld(out ho_Cross1, hv_Row1, hv_Column1, 6, 0.785398);
    ho_Cross2.Dispose();
    HOperatorSet.GenCrossContourXld(out ho_Cross2, hv_Row2, hv_Column2, 6, 0.785398);
    ho_CrossP1.Dispose();
    HOperatorSet.GenCrossContourXld(out ho_CrossP1, hv_P1R, hv_P1C, 6, 0.785398);
    ho_CrossP2.Dispose();
    HOperatorSet.GenCrossContourXld(out ho_CrossP2, hv_P2R, hv_P2C, 6, 0.785398);
    ho_Arrow.Dispose();
    HOperatorSet.GenContourPolygonXld(out ho_Arrow, ((((((hv_Row1.TupleConcat(hv_Row2))).TupleConcat(
        hv_P1R))).TupleConcat(hv_Row2))).TupleConcat(hv_P2R), ((((((hv_Column1.TupleConcat(
        hv_Column2))).TupleConcat(hv_P1C))).TupleConcat(hv_Column2))).TupleConcat(
        hv_P2C));
    ho_Cross1.Dispose();
    ho_Cross2.Dispose();
    ho_CrossP1.Dispose();
    ho_CrossP2.Dispose();

    return;
  }

  private void tuple_equal_greater(HTuple hv_Tuple, 
                                   HTuple hv_Threshold, 
                                   out HTuple hv_Selected,
                                   out HTuple hv_Indices)
  {

    // Local control variables 

    HTuple hv_Sgn;

    // Initialize local and output iconic variables 

    hv_Selected = new HTuple();
    HOperatorSet.TupleSgn(hv_Tuple - hv_Threshold, out hv_Sgn);
    HOperatorSet.TupleFind(hv_Sgn, 1, out hv_Indices);
    if ((int)((new HTuple((new HTuple(hv_Indices.TupleLength())).TupleGreater(1))).TupleOr(
        new HTuple(((hv_Indices.TupleSelect(0))).TupleNotEqual(-1)))) != 0)
    {
      HOperatorSet.TupleSelect(hv_Tuple, hv_Indices, out hv_Selected);
    }

    return;
  }

  private void gen_fuzzy_weight_funct (HTuple hv_NPoints, 
                                      HTuple hv_Min, 
                                      HTuple hv_Max,
                                      HTuple hv_LowPass, 
                                      HTuple hv_HighPass, 
                                      out HTuple hv_FuzzyFunct)
  {

      // Local control variables 

      HTuple hv_Ones, hv_Index, hv_X, hv_Y, hv_Dummy;
      HTuple hv_IndicesTrans, hv_IndicesHigh, hv_i, hv_NTransPoints;

      // Initialize local and output iconic variables 

      //generates a function which is 0.0 for values lower than LowPass, 1.0 for
      //values over HighPass, and grows linearly for values in between the two
      HOperatorSet.TupleGenConst(hv_NPoints, 1, out hv_Ones);
      hv_Index = ((hv_Ones.TupleCumul()) - 1) / ((new HTuple(hv_Ones.TupleLength())).TupleReal()
          );
      hv_X = (hv_Index * (hv_Max - hv_Min)) + hv_Min;
      HOperatorSet.TupleGenConst(new HTuple(hv_X.TupleLength()), 0.0, out hv_Y);
      tuple_equal_greater(hv_X, hv_LowPass, out hv_Dummy, out hv_IndicesTrans);
      tuple_equal_greater(hv_X, hv_HighPass, out hv_Dummy, out hv_IndicesHigh);
      //ramp from LowPass (0.0) to Highpass (1.0)
      hv_i = 0;
      while ((int)(new HTuple(((hv_IndicesTrans.TupleSelect(hv_i))).TupleLess(hv_IndicesHigh.TupleSelect(
          0)))) != 0)
      {
          hv_i = hv_i + 1;
      }
      hv_NTransPoints = hv_i.Clone();
      for (hv_i = 0; hv_i.Continue(hv_NTransPoints - 1, 1); hv_i = hv_i.TupleAdd(1))
      {
          if (hv_Y == null)
              hv_Y = new HTuple();
          hv_Y[hv_IndicesTrans.TupleSelect(hv_i)] = (hv_i.TupleReal()) / hv_NTransPoints;
      }
      for (hv_i = hv_IndicesTrans.TupleSelect(hv_NTransPoints); hv_i.Continue((new HTuple(hv_Y.TupleLength()
          )) - 1, 1); hv_i = hv_i.TupleAdd(1))
      {
          if (hv_Y == null)
              hv_Y = new HTuple();
          hv_Y[hv_i] = 1.0;
      }
      HOperatorSet.CreateFunct1dPairs(hv_X, hv_Y, out hv_FuzzyFunct);

      return;
  }


  /// <summary>
  /// Measures the tilt that is used for the plates in the set
  /// of calibration images. The more tilted plates are used
  /// in the image set, the better you can correct the radial 
  /// distortion of the lense by performing the calibration.
  /// </summary>
  public void eval_caltab_tilt (HTuple hv_FinalPoses, 
                                out HTuple hv_TiltScore)
  {

      // Local control variables 

      HTuple hv_NImages, hv_Ones, hv_Index, hv_Slant;
      HTuple hv_Pan, hv_FuzzyFunct, hv_SlantWeight, hv_PanWeight;
      HTuple hv_TmpPan, hv_TmpSlant;

      // Initialize local and output iconic variables 

      hv_NImages = (new HTuple(hv_FinalPoses.TupleLength())) / 7;
      HOperatorSet.TupleGenConst(hv_NImages, 1, out hv_Ones);
      hv_Index = (hv_Ones.TupleCumul()) - 1;
      HOperatorSet.TupleSelect(hv_FinalPoses, (7 * hv_Index) + 3, out hv_Slant);
      HOperatorSet.TupleSelect(hv_FinalPoses, (7 * hv_Index) + 4, out hv_Pan);
      for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Slant.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
      {
          if ((int)(new HTuple(((hv_Slant.TupleSelect(hv_Index))).TupleGreater(180))) != 0)
          {
              if (hv_Slant == null)
                  hv_Slant = new HTuple();
              hv_Slant[hv_Index] = 360 - (hv_Slant.TupleSelect(hv_Index));
          }
      }
      for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Pan.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
      {
          if ((int)(new HTuple(((hv_Pan.TupleSelect(hv_Index))).TupleGreater(180))) != 0)
          {
              if (hv_Pan == null)
                  hv_Pan = new HTuple();
              hv_Pan[hv_Index] = 360 - (hv_Pan.TupleSelect(hv_Index));
          }
      }
      hv_Pan = hv_Pan.TupleRad();
      hv_Slant = hv_Slant.TupleRad();
      //function acting as a fuzzy weighting
      gen_fuzzy_weight_funct(256, (new HTuple(0.0)).TupleRad(), (new HTuple(90.0)).TupleRad()
          , (new HTuple(15.0)).TupleRad(), (new HTuple(40.0)).TupleRad(), out hv_FuzzyFunct);
      HOperatorSet.GetYValueFunct1d(hv_FuzzyFunct, hv_Slant, "constant", out hv_SlantWeight);
      HOperatorSet.GetYValueFunct1d(hv_FuzzyFunct, hv_Pan, "constant", out hv_PanWeight);
      //Calculate score value
      hv_TmpPan = (hv_PanWeight.TupleSum()) / 6;
      hv_TmpPan = ((hv_TmpPan.TupleConcat(0.5))).TupleMin();
      hv_TmpSlant = (hv_SlantWeight.TupleSum()) / 6;
      hv_TmpSlant = ((hv_TmpSlant.TupleConcat(0.5))).TupleMin();
      hv_TiltScore = hv_TmpSlant + hv_TmpPan;

      return;
  }


  /// <summary>
  /// Evaluates the distribution of the marks and hence the plates
  /// used for the calibration images. Precise measurements can only be
  /// achieved if the field view of the camera is covered well by the
  /// calibration plate in the images.
  /// </summary>
  public void eval_marks_distribution (HTuple hv_NRCoord, 
                                       HTuple hv_NCCoord, 
                                       HTuple hv_Width,
                                       HTuple hv_Height, 
                                       out HTuple hv_MarksDistributionScore)
  {
      // Local iconic variables 

      HObject ho_Region, ho_DistanceImage, ho_Mask;


      // Local control variables 

      HTuple hv_Border, hv_Min, hv_Max, hv_Range;
      HTuple hv_ImageDiagonal, hv_MinThresh, hv_MaxThresh, hv_Ratio;
      HTuple hv_Tmp1, hv_Tmp2;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_Region);
      HOperatorSet.GenEmptyObj(out ho_DistanceImage);
      HOperatorSet.GenEmptyObj(out ho_Mask);

      //Determine the distances between the marks centers
      ho_Region.Dispose();
      HOperatorSet.GenRegionPoints(out ho_Region, hv_NRCoord, hv_NCCoord);
      ho_DistanceImage.Dispose();
      HOperatorSet.DistanceTransform(ho_Region, out ho_DistanceImage, "octagonal",
          "false", hv_Width, hv_Height);
      //A clipping is needed because the marks cannot come close to the border
      hv_Border = (((hv_Width.TupleConcat(hv_Height))).TupleMax()) / 15;
      ho_Mask.Dispose();
      HOperatorSet.GenRectangle1(out ho_Mask, hv_Border, hv_Border, (hv_Height - 1) - hv_Border,
          (hv_Width - 1) - hv_Border);
      HOperatorSet.MinMaxGray(ho_Mask, ho_DistanceImage, 0, out hv_Min, out hv_Max,
          out hv_Range);
      HOperatorSet.DistancePp(0, 0, hv_Height - 1, hv_Width - 1, out hv_ImageDiagonal);
      hv_MinThresh = 0.3;
      hv_MaxThresh = 0.85;
      hv_Ratio = (hv_Max / hv_ImageDiagonal) * 2.5;
      hv_Tmp1 = 1 - hv_Ratio;
      hv_Tmp2 = (hv_Tmp1 - hv_MinThresh) / (hv_MaxThresh - hv_MinThresh);
      hv_MarksDistributionScore = ((((((hv_Tmp2.TupleConcat(1.0))).TupleMin())).TupleConcat(
          0.0))).TupleMax();
      ho_Region.Dispose();
      ho_DistanceImage.Dispose();
      ho_Mask.Dispose();

      return;

  }

  /// <summary>
  /// Auxiliary method for display purposes. Returns the coordinate system
  /// described by the parameters <c>hv_CamParam</c> and <c>hv_Pose</c>
  /// as an iconic object.
  /// </summary>
  public void get_3d_coord_system (HObject ho_ImageReference, 
                                   out HObject ho_CoordSystemRegion,
                                   HTuple hv_CamParam, 
                                   HTuple hv_Pose, 
                                   HTuple hv_CoordAxesLength)
  {



      // Local iconic variables 

      HObject ho_CoordSystemImage;


      // Local control variables 

      HTuple hv_Width, hv_Height, hv_OldBG, hv_WindowHandle;

      // Initialize local and output iconic variables 
      HOperatorSet.GenEmptyObj(out ho_CoordSystemRegion);
      HOperatorSet.GenEmptyObj(out ho_CoordSystemImage);

      HOperatorSet.GetImageSize(ho_ImageReference, out hv_Width, out hv_Height);
      HOperatorSet.GetWindowAttr("background_color", out hv_OldBG);
      HOperatorSet.SetWindowAttr("background_color", "black");
      HOperatorSet.OpenWindow(0, 0, hv_Width, hv_Height, 0, "buffer", "", out hv_WindowHandle);
      HOperatorSet.SetWindowAttr("background_color", hv_OldBG);
      HOperatorSet.SetColor(hv_WindowHandle, "white");
      HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_Height - 1, hv_Width - 1);
      disp_3d_coord_system(hv_WindowHandle, hv_CamParam, hv_Pose, hv_CoordAxesLength);
      ho_CoordSystemImage.Dispose();
      HOperatorSet.DumpWindowImage(out ho_CoordSystemImage, hv_WindowHandle);
      HOperatorSet.CloseWindow(hv_WindowHandle);
      ho_CoordSystemRegion.Dispose();
      HOperatorSet.Threshold(ho_CoordSystemImage, out ho_CoordSystemRegion, 255, 255);
      ho_CoordSystemImage.Dispose();

      return;
  }


}//end of class


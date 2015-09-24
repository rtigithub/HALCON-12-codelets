'*****************************************************************************
'This VB/.NET project demonstrates how to use the new Codelet controls
'in Windows Forms application only using the pre-compiled DLLs.
'To execute this project, please, rebuild on your system the following
'provided projects:
' - $(HALCONEXAMPLES)/codelets/controls/HDisplayControl
' - $(HALCONEXAMPLES)/codelets/controls/HImageAcquisition
'The build proces will generate the necessary DLLs for this project. 
'After the DLLs are generated, you can build and execute this project.
'*****************************************************************************/
'
' (c) 2014 by MVTec Software GmbH
'             www.mvtec.com
'****************************************************************************/

Imports HalconDotNet
Imports HImageAcquisition
Imports HDisplayControl




Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Call HImageAcquisition1.OpenDevice()        
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        If HImageAcquisition1.Connected Then
            Call HImageAcquisition1.StartGrabThread()
        End If
    End Sub

    Private Sub HImageAcquisition1_OnGrabbedImage(ByVal sender As System.Object, ByVal image As HalconDotNet.HImage) Handles HImageAcquisition1.OnGrabbedImage

        HDisplayControl1.Image = image
        HDisplayControl1.Invalidate()
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Call HImageAcquisition1.StopGrabThread()
    End Sub
End Class

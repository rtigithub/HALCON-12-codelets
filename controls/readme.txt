/*****************************************************************************
*
* Readme to codelet controls HDisplayControl and HImageAcquisition
*****************************************************************************/
*
* (c) 2014 by MVTec Software GmbH
*             www.mvtec.com
*****************************************************************************/

Note that HALCON Codelets are now legacy. Its modules and classes are still
available for reasons of compatibility, but they are marked as legacy and 
have not been updated to new features of HALCON 12. 

1. Introduction

The codelet classes HDisplayControl and HImageAcquisition are ready-to-use .NET
controls that simplify the integration of image visualization and acquisition
in a .NET application, respectively.
They are provided as Visual Studio 2005 projects.
You can integrate these codelets as Visual Studio projects into your .NET
solution, or you can just add a reference to the compiled DLLs.
The codelets are provided as example projects and can be extended by new
functionality as needed.



2. Supported operating systems

The codelet classes can be used in applications that run on Windows platforms
and can be integrated in each Windows Forms application.


3. Description of HDisplayControl and HImageAcquisition Codelets

3.1 HDisplayControl

The class HDisplayControl is a .NET control for the visualization of iconic
objects.
The implementation of this class is based on the HWindowControl class and on an
adapted version of HWndCtrl class.
Using the mouse interaction you can move and zoom the visible image part.
Whether this interaction is available for the user or not is configured through
HDisplayControl properties (MoveOnPressedMouseButton, ZoomOnMouseWheel).
The images can be displayed in two modes.
In the default mode, the image is zoomed to the window size in correct aspect
ratio.
The second mode displays the image in its real size.
If the image is larger than the graphics window, scroll bars will be displayed
so you can scroll to view the remainder of the image.
The class also provides a tool bar for drawing and defining the region(s)
of interest in the displayed image.
If the interaction with the tool bar is not desired, the tool bar can be
disabled by setting the property EnabledROISetup to "false".
The HDisplayControl can be resized during the execution.
In this case, the size of the graphical display and the displayed objects will
be adapted to the new size of the control.
The class HDisplayControl uses a graphics stack to manage the displayed iconic
objects.
Each object is linked to a graphical context, which determines how the object
is to be drawn.
The context of each object can be changed by calling the function
changeGraphicSettings().
The graphical "modes" are defined by the class GraphicsContext and map most of
the dev_set_* operators provided in HDevelop.


3.2 HImageAcquisition

The class HImageAcquisition is a .NET component for image acquisition.
This component can be integrated in the toolbox of Visual Studio and then be
added to your application by dragging it onto the application form.
If the component is successfully added, one symbol is appearing in the
component tray of the current form.
To set the component properties, double-click the corresponding symbol in the
component tray and configure the properties in the property browser of Visual
Studio.
Parameters like the HALCON image acquisition interface name (IAName) and the
type of the used camera (CameraType) should be set up in the designer mode in
property browser.
Further general parameters from the operator open_framegrabber(), can be set up
in the source code before connecting to the device.
The class HImageAcquistion supports the acquisition of single images
(GrabSingleImage) as well as continuous image acquisition in the external
thread (StartGrabThread, StopGrabThread).
If a new image is acquired, the acquisition thread notifies the main thread by
firing the event OnGrabbedImage.
If you work with multiple cameras, you will have to add one HImageAcquisition
component for each image acquisition device.


4. Provided Examples

There are two examples which show the use of HDisplayControl and
HImageAcquisition codelets:

- GrabAndDisplay/C#.NET-Project
- SimpleGrabApplication/VB.NET-Project


5. Guideline for Integrating Codelet Controls in .NET Windows Forms Application

This explanation includes the code from the provided VB.NET-Project
SimpleGrabApplication.

Step 1: Create a Windows Forms Visual Studio project.

Step 2: Load the HDisplayControl and HImageAcquisition to the Visual Studio
        toolbox.

Step 3: Place HDisplayControl and HImageAcquisition on the form from the Visual
        Studio toolbox.

Step 4: Set up the properties of HImageAcquisition in the property browser.

Step 5: Set up the properties of HDisplayControl in the property browser as
        needed.

Step 6: Adapt the program to open the connection to an image acquisition
        device.

To open the connection to an image acquisition device, please make sure that
all necessary connection parameters are set up (in the property browser and/or
in the source code).
Then call the function OpenDevice() of the class HImageAcquistion.
In case the connection to the image acquisition device already exists and you
have a valid framegrabber handle, you can call the function OpenDevice() with
the framegrabber handle as input parameter.
This function intializes the class HImageAcquistion from the existing handle
and you can use the functionality from HImageAcquisition without restrictions.

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
Private Sub Button1_Click(ByVal sender As System.Object,
                          ByVal e As System.EventArgs) Handles Button1.Click

        Call HImageAcquisition1.OpenDevice()

End Sub
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

Step 7: Start the image acquisition in a separate thread.

To start the image acquisition in a separate thread, call the function
StartGrabThread() that is implemented in the class HImageAcquistion.

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
Private Sub Button2_Click(ByVal sender As System.Object,
                          ByVal e As System.EventArgs) Handles Button2.Click

        If HImageAcquisition1.Connected Then
            Call HImageAcquisition1.StartGrabThread()
        End If

End Sub
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

Step 8: Stop the image acquisition in the separate thread.

To stop the image acquisition call the function StopGrabThread().

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
Private Sub Button3_Click(ByVal sender As System.Object,
                          ByVal e As System.EventArgs) Handles Button3.Click
        Call HImageAcquisition1.StopGrabThread()
End Sub
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

Step 9: Display the obtained images in the graphical window HDisplayControl.

To display the obtained images in the graphical window HDisplayControl, set the
property Image of this class to the image that has to be displayed, and call
the method Invalidate() to refresh the content of graphical window.
If the new image is obtained from the image acquisition device, the event
OnGrabbedImage will be fired.
In our example application, the image is displayed just after receiving it from
the device by handling the event OnGrabbedImage().

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
Private Sub HImageAcquisition1_OnGrabbedImage(ByVal sender As System.Object,
 ByVal image As HalconDotNet.HImage) Handles HImageAcquisition1.OnGrabbedImage

        HDisplayControl1.Image = image
        HDisplayControl1.Invalidate()
End Sub
\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
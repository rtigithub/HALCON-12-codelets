using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HDisplayControl;
using HImageAcquisition;
using HalconDotNet;


/// <summary>
/// The application GrabAndDisplay shows, how the new codelets
/// controls HDisplayControl and HImageAcquisition can be used
/// in a .NET application.
/// HDisplayControl and HImageAcquisition are Ready-to-use .NET 
/// controls that make the integration of visualization and 
/// image acquistion in a .NET application easier. 
/// For further information to development see also the delivered
/// documentation in "readme.txt"-file.
/// </summary>
namespace GrabAndDisplay
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Lock for SomeEvent delegate access.
        /// </summary>
        readonly object someEventLock;

        public Form1()
        {
            InitializeComponent();

            someEventLock = new object();
    
        }

        private void Form1_Load(object sender, EventArgs e)
        {            

            if (hDisplayControl1.ImageViewState == ImageViewStates.fitToWindow)
                rBFitToWindow.Checked = true;
            else
                rBFullImageSize.Checked = true;

            if (hDisplayControl1.ShowROI)
                chBUpdateROI.Checked = true;
            else
                chBUpdateROI.Checked = false;

                setStatusInformation("      ", "       ", "");
        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        /// <summary>
        /// Estimate connection to image acquisition device
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {            

            // If the connection to IA-Device already exists,
            // then the button is resonsible for disconnection
            //1). Stop IA
            //2). Disconnect device
            if ((hImageAcquisition1.Connected))
            {
                if (hImageAcquisition1.Grabbing)
                    hImageAcquisition1.StopGrabThread();

                DisconnectDevice();
                btnConnectDisconnect.Text = "Connect to device";
            }
            else
            {
                ConnectToDevice();
            }

            if (hImageAcquisition1.Connected)
            {
                try
                {
                    hImageAcquisition1.GrabImage();
                }
                catch (HalconException ex3)
                {
                    setStatusInformation(hImageAcquisition1.ImageCounter.ToString(),
                                hImageAcquisition1.TimeGrab.ToString("F2") + " ms",
                                "Grab of first image failed with error message: " +
                                ex3.Message);
                }
            }
            hDisplayControl1.Invalidate();

        }


        /// <summary>
        /// Connects to image acquisition device
        /// </summary>
        private void ConnectToDevice()
        {
            try
            {
                if ((hImageAcquisition1.Connected == false) &&
                    (hImageAcquisition1.IAName == "File"))
                {
                    hImageAcquisition1.OpenDevice();
                }
                else
                {
                    setStatusInformation("", "", "Connecting to device ... ");

                    hImageAcquisition1.OpenDevice(hImageAcquisition1.IAName, 
                                               hImageAcquisition1.CameraType);
                    
                    // set specific camera parameters, if there are some saved in the 
                    // corresponding list.
                    if (hImageAcquisition1.ListOfAdjustedDynamicParameters.Capacity > 0)
                        hImageAcquisition1.SetIADynamicParametersFromList(
                           hImageAcquisition1.ListOfAdjustedDynamicParameters);
                }

                setStatusInformation("","","Connected to device " +
                                           hImageAcquisition1.CameraType+ ".");

                btnConnectDisconnect.Text = "Disconnect";

            }
            catch (HalconException ex)
            {
                MessageBox.Show("The connection to image acquisition device " +
                                "\'" + hImageAcquisition1.CameraType + "\'" + " " +
                                "failed with error message: \n " +
                                ex.Message, "Connection error!",
                                MessageBoxButtons.OK);

                toolStripStatusLabel3.Text = "The connection to image" +
                                             " acquisition device " +
                                             "failed!";
            }
        }


        /// <summary>
        /// Writes status information to status line
        /// </summary>
        private void setStatusInformation(string statusInfoLeft, 
                                          string statusInfoRight,
                                          string statusInfoMessage)
        {
            if (statusInfoLeft.Length > 0)
                toolStripStatusLabel1.Text = statusInfoLeft;
            else
                toolStripStatusLabel1.Text = "    ";
            if (statusInfoRight.Length > 0)
                toolStripStatusLabel2.Text = statusInfoRight;
            else
                toolStripStatusLabel2.Text = "    ";

            toolStripStatusLabel3.Text = statusInfoMessage;
        }


        /// <summary>
        /// Writes the new image to graphic window
        /// </summary>
        public void UpdateImage(object sender, HImage newimage)
        {
            // display the new received image 
            // lock with act, because the grabbing thread
            // can overwrite the image
            lock (someEventLock)
            {
                HImage image = newimage;
                hDisplayControl1.Image = image;
            }

            if (hDisplayControl1 != null)
                hDisplayControl1.Invalidate();

        }


        /// <summary>
        /// Event handling after acquiring new image from device
        /// </summary>
        private void hImageAcquisition1_OnGrabbedImage(object sender, HImage image)
        {
            UpdateImage(sender,image);

            setStatusInformation(hImageAcquisition1.ImageCounter.ToString(),
                     hImageAcquisition1.TimeGrab.ToString("F2") + " ms",
                     "Acquiring image.");
        }


        /// <summary>
        /// Starts and stops image acquisition in "live" mode
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hImageAcquisition1.Connected)
                {
                    setStatusInformation("", "", "There is no connection " +
                                               "to image acquisition device!");
                }
                else
                {
                    if (!hImageAcquisition1.Grabbing)
                    {
                        hImageAcquisition1.StartGrabThread();
                        btnImageAcquisition.Text = "Stop Image Acquisition";
                    }
                    else
                    {
                        hImageAcquisition1.StopGrabThread();
                        btnImageAcquisition.Text = "Start Image Acquisition";
                    }
                }
           }
           catch (HalconException ex)
           {
               MessageBox.Show(ex.Message);
           }
        }


        /// <summary>
        /// Acquires single image from device
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            if (hImageAcquisition1.Connected)
            {
                try
                {
                    if (hImageAcquisition1.Grabbing)
                    {
                        hImageAcquisition1.StopGrabThread();
                        btnImageAcquisition.Text = "Start Image Acquisition";
                    }

                    hImageAcquisition1.GrabSingleImage();
                }
                catch (HalconException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                setStatusInformation("", "", "There is no connection " +
                                               "to image acquisition device!");
        }


        /// <summary>
        /// By resizing the form of the application the graphic window
        /// should be resized also.
        /// </summary>
        private void Form1_Resize(object sender, EventArgs e)
        {
            hDisplayControl1.Resize(sender,e);
        }


        /// <summary>
        /// Changes the mode of displaying the image in the graphic window
        /// </summary>
        private void rBFitToWindow_CheckedChanged(object sender, EventArgs e)
        {
            if (rBFitToWindow.Checked)
              hDisplayControl1.ImageViewState = ImageViewStates.fitToWindow;
        }


        /// <summary>
        /// Changes the mode of displaying the image in the graphic window
        /// </summary>
        private void rBFullImageSize_CheckedChanged(object sender, EventArgs e)
        {
            if (rBFullImageSize.Checked)
              hDisplayControl1.ImageViewState = ImageViewStates.fullSizeImage;
        }

        /// <summary>
        /// Closes connection to image acquistion device
        /// </summary>
        private void DisconnectDevice()
        {
            try
            {
                hImageAcquisition1.CloseDevice();
                btnImageAcquisition.Text = "Start Image Acquisition";
                setStatusInformation("","","Device is disconnected.");
            }
            catch (HalconException ex)
            {
                MessageBox.Show(ex.Message);
            }
            hDisplayControl1.ClearGraphicStack();
        }


        /// <summary>
        /// Sets some dynamic image acquisition parameters.
        /// These parameters will be saved in the list, that
        /// is managed by HImageAcquisition class. After losing
        /// the connection to the device the changed parameters can be 
        /// resonstructed, because the list exists as long as the class 
        /// HImageAcquisition is not disposed.
        /// ATTENTION: Works only for cameras that support listed 
        /// specific parameters.
        /// </summary>
        private void btnSetIAParameter_Click(object sender, EventArgs e)
        {
            try
            {
                hImageAcquisition1.SetIADynamicParam("exposure", 55);
                hImageAcquisition1.SetIADynamicParam("auto_brightness_speed", 
                                                     29);
                hImageAcquisition1.SetIADynamicParam("edge_enhancement", 
                                                     "weak");
                hImageAcquisition1.SetIADynamicParam("exposure", 24.2915);
                hImageAcquisition1.SetIADynamicParam("exposure", 55);
            }
            catch (ArgumentException aEcxp)
            {
                setStatusInformation("   ","   ",aEcxp.Message);
            }
            catch (HOperatorException hEcxp)
            {
                setStatusInformation("   ","   ",hEcxp.Message);
            }
        }

        /// <summary>
        /// Stops image acquisition and closes connection to image acquisition
        /// device.
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hImageAcquisition1.Grabbing)
                DisconnectDevice();
        }

        private void chBUpdateROI_CheckedChanged(object sender, EventArgs e)
        {
            if (chBUpdateROI.Checked)
            {
                hDisplayControl1.ShowROI = true;
            }
            else
            {
                hDisplayControl1.ShowROI = false;
            }
            hDisplayControl1.Invalidate();
        }   
    }
}
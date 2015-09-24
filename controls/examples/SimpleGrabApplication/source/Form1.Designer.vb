<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.HImageAcquisition1 = New HImageAcquisition.HImageAcquisition(Me.components)
        Me.HDisplayControl1 = New HDisplayControl.HDisplayControl
        Me.Button3 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(541, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(109, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Connect"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(541, 75)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(109, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Start Grab"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'HImageAcquisition1
        '
        Me.HImageAcquisition1.CameraType = "coins"
        Me.HImageAcquisition1.HorizontalResolution = 1
        Me.HImageAcquisition1.LineIn = -1
        Me.HImageAcquisition1.Port = -1
        '
        'HDisplayControl1
        '
        Me.HDisplayControl1.AutoScroll = True
        Me.HDisplayControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.HDisplayControl1.BackColor = System.Drawing.SystemColors.Control
        Me.HDisplayControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.HDisplayControl1.CurrentROI = Nothing
        Me.HDisplayControl1.Image = Nothing
        Me.HDisplayControl1.ImageViewState = HDisplayControl.ImageViewStates.fitToWindow
        Me.HDisplayControl1.Location = New System.Drawing.Point(12, 28)
        Me.HDisplayControl1.MinimumSize = New System.Drawing.Size(344, 293)
        Me.HDisplayControl1.MoveOnPressedMouseButton = True
        Me.HDisplayControl1.Name = "HDisplayControl1"
        Me.HDisplayControl1.Size = New System.Drawing.Size(512, 395)
        Me.HDisplayControl1.TabIndex = 0
        Me.HDisplayControl1.WindowSize = New System.Drawing.Size(488, 342)
        Me.HDisplayControl1.ZoomCenter = New System.Drawing.Point(160, 120)
        Me.HDisplayControl1.ZoomOnMouseWheel = False
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(541, 124)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(109, 23)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Stop Grab"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(662, 489)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.HDisplayControl1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HImageAcquisition1 As HImageAcquisition.HImageAcquisition
    Friend WithEvents HDisplayControl1 As HDisplayControl.HDisplayControl
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button

End Class

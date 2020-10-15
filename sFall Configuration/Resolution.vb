Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO

Friend Class Resolution

    Private Declare Function EnumDisplaySettings Lib "user32" Alias "EnumDisplaySettingsA" _
            (ByVal lpszDeviceName As Integer, ByVal iModeNum As Integer, ByRef lpdmode As DEVMODE) As Boolean

    Const ENUM_CURRENT_SETTINGS As Integer = -1
    Const CDS_UPDATEREGISTRY As Integer = &H1
    Const CDS_TEST As Long = &H2
    Const CCDEVICENAME As Integer = 32
    Const CCFORMNAME As Integer = 32
    Const DISP_CHANGE_SUCCESSFUL As Integer = 0
    Const DISP_CHANGE_RESTART As Integer = 1
    Const DISP_CHANGE_FAILED As Integer = -1

    <StructLayout(LayoutKind.Sequential)>
    Private Structure DEVMODE
        <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=CCDEVICENAME)> Public dmDeviceName As String
        Public dmSpecVersion As Short
        Public dmDriverVersion As Short
        Public dmSize As Short
        Public dmDriverExtra As Short
        Public dmFields As Integer

        Public dmOrientation As Short
        Public dmPaperSize As Short
        Public dmPaperLength As Short
        Public dmPaperWidth As Short

        Public dmScale As Short
        Public dmCopies As Short
        Public dmDefaultSource As Short
        Public dmPrintQuality As Short
        Public dmColor As Short
        Public dmDuplex As Short
        Public dmYResolution As Short
        Public dmTTOption As Short
        Public dmCollate As Short
        <MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst:=CCFORMNAME)> Public dmFormName As String
        Public dmUnusedPadding As Short
        Public dmBitsPerPel As Short
        Public dmPelsWidth As Integer
        Public dmPelsHeight As Integer

        Public dmDisplayFlags As Integer
        Public dmDisplayFrequency As Integer
    End Structure

    Shared Function GetResolution() As ArrayList
        Dim DevM As DEVMODE
        DevM.dmDeviceName = New [String](New Char(32) {})
        DevM.dmFormName = New [String](New Char(32) {})
        DevM.dmSize = CShort(Marshal.SizeOf(GetType(DEVMODE)))
        Dim dMode = 0
        Dim Res As New ArrayList

        Do While EnumDisplaySettings(Nothing, dMode, DevM) = True
            If DevM.dmPelsWidth >= 640 And DevM.dmDisplayFrequency = 60 And DevM.dmBitsPerPel = 32 And DevM.dmDefaultSource = 0 Then
                Res.Add(CStr(DevM.dmPelsWidth) & " X " & CStr(DevM.dmPelsHeight))
#If DEBUG Then
                'DebugRes(DevM)
#End If
            End If
            dMode += 1
        Loop

        Res.Reverse()
        Return Res
    End Function

#If DEBUG Then
    Private Shared Sub DebugRes(ByVal DevM As DEVMODE)
        FileSystem.WriteAllText("sConf.dbg", "DevM.dmBitsPerPel:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmBitsPerPel, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmCollate:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmCollate, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmColor:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmColor, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmCopies:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmCopies, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDefaultSource:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDefaultSource, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDeviceName:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDeviceName, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDisplayFlags:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDisplayFlags, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDisplayFrequency:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDisplayFrequency, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDriverExtra:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDriverExtra, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDriverVersion:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDriverVersion, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDuplex:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmDuplex, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmFields:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmFields, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmFormName:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmFormName, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmOrientation:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmOrientation, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperLength:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperLength, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperSize:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperSize, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperWidth:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperWidth, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPelsHeight:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPelsHeight, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPelsWidth:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPelsWidth, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPrintQuality:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmPrintQuality, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmScale:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmScale, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmSize:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmSize, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmSpecVersion:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmSpecVersion, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmTTOption:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmTTOption, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmUnusedPadding:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmUnusedPadding, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmYResolution:", True)
        FileSystem.WriteAllText("sConf.dbg", DevM.dmYResolution, True)
        FileSystem.WriteAllText("sConf.dbg", vbCrLf & vbCrLf, True)
    End Sub
#End If

End Class

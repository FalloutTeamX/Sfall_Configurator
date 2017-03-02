'Imports System
'Imports System.Linq
Imports System.Runtime.InteropServices

Public Class MainForm
    Private FormReady As Boolean
    Private Changed As Boolean
    Private Value As Byte, sValue As SByte
    Private OnlyOnce As Boolean

    Private Declare Function EnumDisplaySettings Lib "user32" Alias "EnumDisplaySettingsA" (ByVal lpszDeviceName As Integer, ByVal iModeNum As Integer, ByRef lpdmode As DEVMODE) As Boolean

    Const ENUM_CURRENT_SETTINGS As Integer = -1
    Const CDS_UPDATEREGISTRY As Integer = &H1
    Const CDS_TEST As Long = &H2
    Const CCDEVICENAME As Integer = 32
    Const CCFORMNAME As Integer = 32
    Const DISP_CHANGE_SUCCESSFUL As Integer = 0
    Const DISP_CHANGE_RESTART As Integer = 1
    Const DISP_CHANGE_FAILED As Integer = -1

    <StructLayout(LayoutKind.Sequential)> Public Structure DEVMODE
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

    'Private Sub DebugRes(ByVal DevM As DEVMODE)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", "DevM.dmBitsPerPel:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmBitsPerPel, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmCollate:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmCollate, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmColor:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmColor, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmCopies:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmCopies, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDefaultSource:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDefaultSource, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDeviceName:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDeviceName, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDisplayFlags:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDisplayFlags, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDisplayFrequency:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDisplayFrequency, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDriverExtra:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDriverExtra, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDriverVersion:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDriverVersion, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmDuplex:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmDuplex, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmFields:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmFields, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmFormName:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmFormName, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmOrientation:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmOrientation, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperLength:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperLength, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperSize:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperSize, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPaperWidth:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPaperWidth, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPelsHeight:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPelsHeight, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPelsWidth:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPelsWidth, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmPrintQuality:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmPrintQuality, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmScale:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmScale, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmSize:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmSize, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmSpecVersion:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmSpecVersion, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmTTOption:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmTTOption, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmUnusedPadding:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmUnusedPadding, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & "DevM.dmYResolution:", True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", DevM.dmYResolution, True)
    '    My.Computer.FileSystem.WriteAllText("sConf.dbg", vbCrLf & vbCrLf, True)
    'End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If IO.File.Exists(Main_Path & "\ddraw.ini") = False Then
            For i = 0 To 3
                TabControl1.TabPages.RemoveAt(0)
            Next
            ReDim Ddraw_ini(0)
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Exit Sub
        Else : Ddraw_ini = IO.File.ReadAllLines(Main_Path & "\ddraw.ini", System.Text.Encoding.Default) : End If
        If IO.File.Exists(Main_Path & "\f2_res.ini") Then F2res_ini = IO.File.ReadAllLines(Main_Path & "\f2_res.ini", System.Text.Encoding.Default)
        '
        Dim DevM As DEVMODE
        DevM.dmDeviceName = New [String](New Char(32) {})
        DevM.dmFormName = New [String](New Char(32) {})
        DevM.dmSize = CShort(Marshal.SizeOf(GetType(DEVMODE)))
        Dim dMode = 0
        Dim Res As New ArrayList
        Do While EnumDisplaySettings(Nothing, dMode, DevM) = True
            If DevM.dmPelsWidth >= 640 And DevM.dmDisplayFrequency = 60 And DevM.dmBitsPerPel = 32 And DevM.dmDefaultSource = 0 Then
                Res.Add(CStr(DevM.dmPelsWidth) & " X " & CStr(DevM.dmPelsHeight))
                'DebugRes(DevM)
            End If
            dMode += 1
        Loop
        Res.Reverse()
        ListBox1.Items.AddRange(Res.ToArray)
    End Sub

    Private Sub MainForm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Dim chBool As Boolean, str() As String
        Me.Text &= My.Application.Info.Version.ToString
        If Ddraw_ini.GetLength(0) = 1 Then
            DatDesc()
            MsgBox("File ddraw.ini not found.")
            Exit Sub
        End If
        If GetIni_Param("sFallConfigurator") = "ENG" Then
            ComboBox8.SelectedIndex = 1
        Else
            ComboBox8.SelectedIndex = 0
        End If
        On Error Resume Next
        CheckBox1.Checked = CBool(GetIni_Param("DisplayKarmaChanges"))
        CheckBox4.Checked = CBool(GetIni_Param("UseScrollingQuestsList"))
        CheckBox6.Checked = CBool(GetIni_Param("StackEmptyWeapons"))
        CheckBox11.Checked = CBool(GetIni_Param("ToggleItemHighlightsKey"))
        'v3.7
        If GetIni_Param("DisplayBonusDamage") <> Nothing Then
            CheckBox26.Enabled = True : CheckBox26.CheckState = CheckState.Unchecked
            CheckBox26.Checked = CBool(GetIni_Param("DisplayBonusDamage"))
        End If
        '
        CheckBox10.Checked = CBool(GetIni_Param("ControlCombat"))
        '
        Dim n As Integer = GetIni_Section_Line("[Speed]")
        For n = n + 1 To UBound(Ddraw_ini)
            If GetIni_NameParam(Ddraw_ini(n)) = "Enable" Then
                chBool = CBool(GetIni_ParamValue(Ddraw_ini(n)))
                Exit For
            End If
        Next
        If GetIni_Param("SpeedMultiInitial") = "200" And chBool Then CheckBox13.Checked = True
        '
        CheckBox23.Checked = CBool(GetIni_Param("CheckWeaponAmmoCost"))
        CheckBox25.Checked = CBool(GetIni_Param("ReloadWeaponKey"))
        CheckBox2.Checked = CBool(GetIni_Param("ExtraSaveSlots"))
        If GetIni_Param("MotionScannerFlags") = "4" Then
            ComboBox2.SelectedIndex = 3
        Else : ComboBox2.SelectedIndex = GetIni_Param("MotionScannerFlags") : End If
        ComboBox5.SelectedIndex = GetIni_Param("DamageFormula")
        ComboBox4.SelectedIndex = GetIni_Param("FastShotFix")
        Select Case GetIni_Param("TimeLimit")
            Case "0"
                ComboBox7.SelectedIndex = 0
            Case "-1"
                ComboBox7.SelectedIndex = 1
            Case "-2"
                ComboBox7.SelectedIndex = 2
            Case "-3"
                ComboBox7.SelectedIndex = 3
            Case Else
                ComboBox7.SelectedIndex = 4
        End Select
        '
        If GetIni_Param("Mode") > 0 Then
            ComboBox1.SelectedIndex = GetIni_Param("Mode") - 3
        Else : ComboBox1.SelectedIndex = 0 : End If
        NumericUpDown4.Value = GetIni_Param("GraphicsWidth")
        NumericUpDown3.Value = GetIni_Param("GraphicsHeight")
        Set_ListBoxRes()
        CheckBox14.Checked = CBool(GetIni_Param("Use32BitHeadGraphics"))
        CheckBox15.Checked = CBool(GetIni_Param("GPUBlt"))
        CheckBox16.Checked = CBool(GetIni_Param("SkipOpeningMovies"))
        CheckBox17.Checked = CBool(GetIni_Param("SpeedInterfaceCounterAnims"))
        CheckBox18.Checked = CBool(GetIni_Param("ExplosionsEmitLight"))
        NumericUpDown1.Value = GetIni_Param("DialogPanelAnimDelay")
        NumericUpDown2.Value = GetIni_Param("CombatPanelAnimDelay")
        '
        If GetIni_Param("DebugMode") <> Nothing Then
            CheckBox19.Checked = CBool(GetIni_Param("DebugMode"))
        Else : CheckBox19.Enabled = False : CheckBox19.CheckState = CheckState.Indeterminate : End If
        If GetIni_Param("SkipSizeCheck") <> Nothing Then
            CheckBox20.Checked = CBool(GetIni_Param("SkipSizeCheck"))
        Else : CheckBox20.Enabled = False : CheckBox20.CheckState = CheckState.Indeterminate : End If
        If GetIni_Param("ProcessorIdle") > 0 Then CheckBox21.Checked = True 'CBool(GetIni_Param("ProcessorIdle"))
        CheckBox22.Checked = CBool(GetIni_Param("SingleCore"))
        If GetIni_Param("ExtraCRC") <> Nothing Then
            TextBox1.Text = GetIni_Param("ExtraCRC")
        Else : TextBox1.Enabled = False : End If
        CheckBox24.Checked = CBool(GetIni_Param("ReverseMouseButtons"))
        'Advanced
        CheckBox32.Checked = CBool(GetIni_Param("ObjCanSeeObj_ShootThru_Fix"))
        CheckBox34.Checked = CBool(GetIni_Param("CorpseLineOfFireFix"))
        CheckBox38.Checked = CBool(GetIni_Param("RemoveCriticalTimelimits"))
        ComboBox11.SelectedIndex = GetIni_Param("SaveInCombatFix")
        NumericUpDown6.Value = GetIni_Param("NPCsTryToSpendExtraAP")
        '
        If GetIni_Param("HighlightContainers") <> Nothing Then
            CheckBox12.Enabled = True : CheckBox12.CheckState = CheckState.Unchecked
            CheckBox12.Checked = CBool(GetIni_Param("HighlightContainers"))
        End If
        'Crafty
        If GetIni_Param("TurnHighlightContainers") <> Nothing Then
            CheckBox12.Enabled = True : CheckBox12.CheckState = CheckState.Unchecked
            CheckBox12.Checked = CBool(GetIni_Param("TurnHighlightContainers"))
        End If
        If GetIni_Param("FreeWeight") <> Nothing Then
            CheckBox7.Enabled = True : CheckBox7.CheckState = CheckState.Unchecked
            CheckBox7.Checked = CBool(GetIni_Param("FreeWeight"))
        End If
        If GetIni_Param("EquipArmor") <> Nothing Then
            CheckBox8.Enabled = True : CheckBox8.CheckState = CheckState.Unchecked
            CheckBox8.Checked = CBool(GetIni_Param("EquipArmor"))
        End If
        If GetIni_Param("AutoReloadWeapon") <> Nothing Then
            CheckBox9.Enabled = True : CheckBox9.CheckState = CheckState.Unchecked
            CheckBox9.Checked = CBool(GetIni_Param("AutoReloadWeapon"))
        End If
        If GetIni_Param("DontTurnOffSneakIfYouRun") <> Nothing Then
            CheckBox3.Enabled = True : CheckBox3.CheckState = CheckState.Unchecked
            CheckBox3.Checked = CBool(GetIni_Param("DontTurnOffSneakIfYouRun"))
        End If
        If GetIni_Param("EnableMusicInDialogue") <> Nothing Then
            CheckBox5.Enabled = True : CheckBox5.CheckState = CheckState.Unchecked
            CheckBox5.Checked = CBool(GetIni_Param("EnableMusicInDialogue"))
        End If
        If GetIni_Param("UsePartySkills") <> Nothing Then
            CheckBox35.Enabled = True : CheckBox35.CheckState = CheckState.Unchecked
            CheckBox35.Checked = CBool(GetIni_Param("UsePartySkills"))
        End If
        If GetIni_Param("NumbersInDialogue") <> Nothing Then
            CheckBox37.Enabled = True : CheckBox37.CheckState = CheckState.Unchecked
            CheckBox37.Checked = CBool(GetIni_Param("NumbersInDialogue"))
        End If
        If GetIni_Param("AutoQuickSave") <> Nothing Then
            ComboBox6.Enabled = True
            ComboBox6.SelectedIndex = GetIni_Param("AutoQuickSave")
        End If
        Select Case GetIni_Param("ReloadReserve")
            Case "-1"
                ComboBox3.SelectedIndex = 0
                ComboBox3.Enabled = True
            Case "0"
                ComboBox3.SelectedIndex = 1
                ComboBox3.Enabled = True
            Case "1"
                ComboBox3.SelectedIndex = 2
                ComboBox3.Enabled = True
            Case Not (Nothing)
                ComboBox3.SelectedText = GetIni_Param("ReloadReserve")
                ComboBox3.Enabled = True
        End Select
        If GetIni_Param("InstanWeaponEquip") <> Nothing Or GetIni_Param("InstantWeaponEquip") <> Nothing Then
            CheckBox29.Enabled = True : CheckBox29.CheckState = CheckState.Unchecked
            CheckBox29.Checked = CBool(GetIni_Param("InstanWeaponEquip") Or GetIni_Param("InstantWeaponEquip"))
        End If
        If GetIni_Param("PipboyTimeAnimDelay") <> Nothing Then
            CheckBox30.Enabled = True : CheckBox30.CheckState = CheckState.Unchecked
            If CInt(GetIni_Param("PipboyTimeAnimDelay")) <= 25 Then CheckBox30.Checked = True
        End If
        If GetIni_Param("XltKey") <> Nothing Then
            ComboBox9.Enabled = True
            ComboBox10.Enabled = True
            Select Case GetIni_Param("XltKey")
                Case "2"
                    ComboBox9.SelectedIndex = 1
                Case "4"
                    ComboBox9.SelectedIndex = 2
                Case Else
                    ComboBox9.SelectedIndex = 0
            End Select
            str = GetIni_Param("XltTable").Split(",")
            If str(2) = 221 Then
                ComboBox10.SelectedIndex = 0
            Else
                ComboBox10.SelectedIndex = 1
            End If
        End If
        If GetIni_Param("DrugExploitFix") <> Nothing Then
            CheckBox31.Enabled = True : CheckBox31.CheckState = CheckState.Unchecked
            CheckBox31.Checked = CBool(GetIni_Param("DrugExploitFix"))
        End If
        If GetIni_Param("CanSellUsedGeiger") <> Nothing Then
            CheckBox36.Enabled = True : CheckBox36.CheckState = CheckState.Unchecked
            CheckBox36.Checked = CBool(GetIni_Param("CanSellUsedGeiger"))
        End If
        If GetIni_Param("RiflescopePenalty") <> Nothing Then
            NumericUpDown5.Enabled = True
            NumericUpDown5.Value = GetIni_Param("RiflescopePenalty")
        End If
        'HRP
        If F2res_ini.Length > 1 Then
            CheckBox27.Enabled = True : CheckBox27.CheckState = CheckState.Unchecked
            CheckBox27.Checked = CBool(GetIni_Param_HRP("IS_GRAY_SCALE"))
            CheckBox28.Enabled = True : CheckBox28.CheckState = CheckState.Unchecked
            CheckBox28.Checked = CBool(GetIni_Param_HRP("ALTERNATE_AMMO_METRE"))
        End If
        '
        FormReady = True
    End Sub

    Private Sub CheckBox25_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox25.CheckedChanged
        If FormReady Then
            Value = CheckBox25.Checked
            If Value > 0 Then Value = 17 'DIK_W
            SetIni_ParamValue("ReloadWeaponKey", Value)
        End If
    End Sub

    Private Sub CheckBox23_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox23.CheckedChanged
        If FormReady Then
            Value = CheckBox23.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CheckWeaponAmmoCost", Value)
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If FormReady Then
            Value = CheckBox1.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplayKarmaChanges", Value)
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox4.CheckedChanged
        If FormReady Then
            Value = CheckBox4.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("UseScrollingQuestsList", Value)
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox3.CheckedChanged
        If FormReady Then
            Value = CheckBox3.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DontTurnOffSneakIfYouRun", Value)
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox7.CheckedChanged
        If FormReady Then
            Value = CheckBox7.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("FreeWeight", Value)
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox8.CheckedChanged
        If FormReady Then
            Value = CheckBox8.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("EquipArmor", Value)
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox6.CheckedChanged
        If FormReady Then
            Value = CheckBox6.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("StackEmptyWeapons", Value)
        End If
    End Sub

    Private Sub CheckBox9_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox9.CheckedChanged
        If FormReady Then
            Value = CheckBox9.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("AutoReloadWeapon", Value)
        End If
    End Sub

    Private Sub CheckBox11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox11.CheckedChanged
        If FormReady Then
            Value = CheckBox11.Checked
            If Value > 0 Then Value = 42 'код кл. шифта
            SetIni_ParamValue("ToggleItemHighlightsKey", Value)
        End If
    End Sub

    Private Sub CheckBox12_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox12.CheckedChanged
        If FormReady Then
            Value = CheckBox12.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("HighlightContainers", Value)
            SetIni_ParamValue("TurnHighlightContainers", Value)
        End If
    End Sub

    Private Sub CheckBox26_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox26.CheckedChanged
        If FormReady Then
            Value = CheckBox26.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplayBonusDamage", Value)
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If FormReady Then
            Value = CheckBox5.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("EnableMusicInDialogue", Value)
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox10.CheckedChanged
        If FormReady Then
            Value = CheckBox10.Checked
            If Value > 1 Then Value = 2 'Set to control all party members
            SetIni_ParamValue("ControlCombat", Value)
        End If
    End Sub

    Private Sub CheckBox13_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox13.CheckedChanged
        If FormReady Then
            Value = CheckBox13.Checked
            If Value > 1 Then Value = 200 Else Value = 100
            Dim n As Integer = GetIni_Section_Line("[Speed]")
            If n = -1 Then Exit Sub
            For n = n + 1 To UBound(Ddraw_ini)
                If GetIni_NameParam(Ddraw_ini(n)) = "Enable" Then Ddraw_ini(n) = "Enable=1"
            Next
            'SetIni_ParamValue("Enable", 1)
            SetIni_ParamValue("SpeedMultiInitial", Value)
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.CheckedChanged
        If FormReady Then
            Value = CheckBox2.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ExtraSaveSlots", Value)
        End If
    End Sub

    Private Sub ComboBox6_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox6.SelectedIndexChanged
        If FormReady Then SetIni_ParamValue("AutoQuickSave", ComboBox6.SelectedIndex)
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox3.TextChanged
        If FormReady Then
            If ComboBox3.SelectedIndex = 0 Then
                SetIni_ParamValue("ReloadReserve", -1)
            ElseIf ComboBox3.SelectedIndex = 1 Then
                SetIni_ParamValue("ReloadReserve", 0)
            ElseIf ComboBox3.SelectedIndex = 2 Then
                SetIni_ParamValue("ReloadReserve", 1)
            Else : SetIni_ParamValue("ReloadReserve", ComboBox3.Text) : End If
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox2.SelectedIndexChanged
        If FormReady Then
            Value = ComboBox2.SelectedIndex
            If Value = 3 Then Value = 4
            SetIni_ParamValue("MotionScannerFlags", Value)
        End If
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox5.SelectedIndexChanged
        If FormReady Then
            Value = ComboBox5.SelectedIndex
            If Value = 3 Then Value = 0
            SetIni_ParamValue("DamageFormula", Value)
        End If
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox7.SelectedIndexChanged
        If FormReady Then
            Value = ComboBox7.SelectedIndex
            sValue = 0
            If Value = 1 Then
                sValue = -1
            ElseIf Value = 2 Then
                sValue = -2
            ElseIf Value = 3 Then
                sValue = -3
            ElseIf Value = 4 Then
                sValue = 13
            End If
            SetIni_ParamValue("TimeLimit", sValue)
        End If
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox4.SelectedIndexChanged
        If FormReady Then SetIni_ParamValue("FastShotFix", ComboBox4.SelectedIndex)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If FormReady Then
            Value = ComboBox1.SelectedIndex
            If Value > 0 Then Value += 3
            SetIni_ParamValue("Mode", Value)
        End If
    End Sub

    Private Sub Resolution_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown3.ValueChanged, NumericUpDown4.ValueChanged
        If FormReady Then
            SetIni_ParamValue("GraphicsHeight", NumericUpDown3.Value)
            SetIni_ParamValue("GraphicsWidth", NumericUpDown4.Value)
            If F2res_ini.Length > 1 Then
                SetIni_ParamValue_HRP("SCR_HEIGHT", NumericUpDown3.Value)
                SetIni_ParamValue_HRP("SCR_WIDTH", NumericUpDown4.Value)
            End If
        End If
    End Sub

    Private Sub CheckBox14_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox14.CheckedChanged
        If FormReady Then
            Value = CheckBox14.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("Use32BitHeadGraphics", Value)
        End If
    End Sub

    Private Sub CheckBox15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox15.CheckedChanged
        If FormReady Then
            Value = CheckBox15.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("GPUBlt", Value)
        End If
    End Sub

    Private Sub CheckBox16_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox16.CheckedChanged
        If FormReady Then
            Value = CheckBox16.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SkipOpeningMovies", Value)
        End If
    End Sub

    Private Sub CheckBox17_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox17.CheckedChanged
        If FormReady Then
            Value = CheckBox17.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SpeedInterfaceCounterAnims", Value)
        End If
    End Sub

    Private Sub CheckBox18_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox18.CheckedChanged
        If FormReady Then
            Value = CheckBox18.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ExplosionsEmitLight", Value)
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown1.ValueChanged
        If FormReady Then SetIni_ParamValue("DialogPanelAnimDelay", NumericUpDown1.Value)
    End Sub

    Private Sub NumericUpDown2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown2.ValueChanged
        If FormReady Then SetIni_ParamValue("CombatPanelAnimDelay", NumericUpDown2.Value)
    End Sub

    Private Sub CheckBox19_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox19.CheckedChanged
        If FormReady Then
            Value = CheckBox19.Checked
            If Value > 1 Then Value = 2
            SetIni_ParamValue("DebugMode", Value)
            EnableDebug()
        End If
    End Sub

    Private Sub CheckBox20_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox20.CheckedChanged
        If FormReady Then
            Value = CheckBox20.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SkipSizeCheck", Value)
            EnableDebug()
        End If
    End Sub

    Private Sub CheckBox21_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox21.CheckedChanged
        If FormReady Then
            If CheckBox21.Checked = 0 Then SetIni_ParamValue("ProcessorIdle", -1) Else SetIni_ParamValue("ProcessorIdle", 1)
        End If
    End Sub

    Private Sub CheckBox22_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox22.CheckedChanged
        If FormReady Then
            Value = CheckBox22.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SingleCore", Value)
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        If FormReady Then
            Changed = True
        End If
    End Sub

    Private Sub TextBox1_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.Leave
        If Changed Then
            StoreIniCrc()
        End If
        Changed = False
    End Sub

    Friend Sub StoreIniCrc()
        If TextBox1.Enabled Then
            SetIni_ParamValue("ExtraCRC", TextBox1.Text)
        Else
            Dim n As Integer = GetIni_Param_Line("ExtraCRC")
            If n = -1 Then n = GetIni_Param_Line(";ExtraCRC")
            If n <> -1 Then
                Ddraw_ini(n) = "ExtraCRC=" & TextBox1.Text
                TextBox1.Enabled = True
            End If
        End If
        EnableDebug()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        'Process.Start("notepad.exe", "ddraw.ini")
        Save_ini()
        Process.Start("ddraw.ini")
        Application.Exit()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        On Error GoTo CUSTEXE
        Check_Exe()
        Save_ini()
        Process.Start("fallout2.exe")
        GoTo EXITAPP
CUSTEXE:
        On Error GoTo -1
        On Error GoTo SELEXE
        Process.Start(GetIni_Param("sFallConfigatorGameExe"))
        GoTo EXITAPP
SELEXE:
        OpenFileDialog1.Filter = "Exe files|*.exe"
        OpenFileDialog1.InitialDirectory = Main_Path
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        SetGameExe_Ini(OpenFileDialog1.SafeFileName)
        Check_Exe()
        Save_ini()
        Process.Start(GetIni_Param("sFallConfigatorGameExe")) 'GoTo CUSTEXE
EXITAPP:
        Application.Exit()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Save_ini()
        Application.Exit()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim game_exe As String = GetIni_Param("sFallConfigatorGameExe")
        If game_exe <> Nothing And IO.File.Exists(Main_Path & "\" & game_exe) Then
            Check_CRC(game_exe)
        Else
            If IO.File.Exists(Main_Path & "\fallout2.exe") Then
                Check_CRC("fallout2.exe")
            Else
                MsgBox("Required file fallout2.exe", , "Select Game Exe")
                OpenFileDialog1.Filter = "Exe files|*.exe"
                OpenFileDialog1.InitialDirectory = Main_Path
                If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
                SetGameExe_Ini(OpenFileDialog1.SafeFileName)
                Check_CRC(OpenFileDialog1.SafeFileName)
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://yadi.sk/d/6S_zZOpKjZcxg")
    End Sub

    Private Sub CheckBox24_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox24.CheckedChanged
        If FormReady Then
            Value = CheckBox24.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ReverseMouseButtons", Value)
        End If
    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox8.SelectedIndexChanged
        If ComboBox8.SelectedIndex Then
            ToolTip2.Active = False
            ToolTip1.Active = True
        Else
            ToolTip2.Active = True
            ToolTip1.Active = False
        End If
        If FormReady Then
            SetIni_ParamValue("sFallConfigurator", ComboBox8.Text)
            If GetIni_Param("sFallConfigurator") = Nothing Then
                ReDim Preserve Ddraw_ini(UBound(Ddraw_ini) + 3)
                Ddraw_ini(UBound(Ddraw_ini) - 1) = ";Setting language for sFallConfigurator"
                Ddraw_ini(UBound(Ddraw_ini)) = "sFallConfigurator=" & ComboBox8.Text
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        If FormReady Then
            Dim str As String = ListBox1.SelectedItem
            Dim res() As String = str.Split("X")
            NumericUpDown4.Value = res(0).Trim
            NumericUpDown3.Value = res(1).Trim
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Dim strprm As String, strval As String, lineprm As Integer
        OpenFileDialog1.Filter = "Ini files|*.ini"
        OpenFileDialog1.InitialDirectory = Main_Path
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        Dim Ddraw_old() As String = IO.File.ReadAllLines(OpenFileDialog1.FileName, System.Text.Encoding.Default)
        For n = 0 To UBound(Ddraw_old)
            strprm = GetIni_NameParam(Ddraw_old(n))
            If strprm <> Nothing Then
                strval = GetIni_ParamValue(Ddraw_old(n))
                If strval <> Nothing Then
                    If GetIni_Param(strprm) <> Nothing Then
                        SetIni_ParamValue(strprm, strval)
                    Else
                        lineprm = GetIni_Param_Line(";" & strprm)
                        If lineprm <> -1 Then
                            Ddraw_ini(lineprm) = strprm & "=" & strval
                        End If
                    End If
                End If
            End If
        Next
        Call MainForm_Shown(sender, e)
        MsgBox("Ready")
    End Sub

    Private Sub CheckBox27_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox27.CheckedChanged
        If FormReady Then
            Value = CheckBox27.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue_HRP("IS_GRAY_SCALE", Value)
        End If
    End Sub

    Private Sub CheckBox28_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox28.CheckedChanged
        If FormReady Then
            Value = CheckBox28.Checked
            If Value > 1 Then Value = 2
            SetIni_ParamValue_HRP("ALTERNATE_AMMO_METRE", Value)
        End If
    End Sub

    Private Sub CheckBox29_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox29.CheckedChanged
        If FormReady Then
            Value = CheckBox29.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("InstantWeaponEquip", Value)
            SetIni_ParamValue("InstanWeaponEquip", Value)
        End If
    End Sub

    Private Sub CheckBox30_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox30.CheckedChanged
        If FormReady Then
            If CheckBox30.Checked = True Then
                SetIni_ParamValue("PipboyTimeAnimDelay", 10)
            Else
                SetIni_ParamValue("PipboyTimeAnimDelay", 50)
            End If
        End If
    End Sub

    Private Sub ComboBox9_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox9.SelectedIndexChanged
        If FormReady Then
            Value = ComboBox9.SelectedIndex
            If Value < 2 Then
                SetIni_ParamValue("XltKey", Value + 1)
            Else
                SetIni_ParamValue("XltKey", 4)
            End If
        End If
    End Sub

    Private Sub ComboBox10_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox10.SelectedIndexChanged
        If FormReady Then
            If ComboBox10.SelectedIndex = 0 Then
                SetIni_ParamValue("XltTable", "32,33,221,35,36,37,38,253,40,41,42,43,225,45,254,47,48,49,50,51,52,53,54,55,56,57,198,230,193,61,222,63,64,212,200,209,194,211,192,207,208,216,206,203,196,220,210,217,199,201,202,219,197,195,204,214,215,205,223,245,92,250,94,95,96,244,232,241,226,243,224,239,240,248,238,235,228,252,242,249,231,233,234,251,229,227,236,246,247,237,255,213,124,218")
            Else
                SetIni_ParamValue("XltTable", "32,33,157,35,36,37,38,237,40,41,42,43,161,45,238,47,48,49,50,51,52,53,54,55,56,57,134,166,129,61,158,63,64,148,136,145,130,147,128,143,144,152,142,139,132,156,146,153,135,137,138,155,133,131,140,150,151,141,159,229,92,234,94,95,96,228,168,225,162,227,160,175,224,232,174,171,164,236,226,233,167,169,170,235,165,163,172,230,231,173,239,240,124,154")
            End If
        End If
    End Sub

    Private Sub CheckBox36_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox36.CheckedChanged
        If FormReady Then
            Value = CheckBox36.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CanSellUsedGeiger", Value)
        End If
    End Sub

    Private Sub CheckBox38_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox38.CheckedChanged
        If FormReady Then
            Value = CheckBox38.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("RemoveCriticalTimelimits", Value)
        End If
    End Sub

    Private Sub CheckBox37_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox37.CheckedChanged
        If FormReady Then
            Value = CheckBox37.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("NumbersInDialogue", Value)
        End If
    End Sub

    Private Sub CheckBox34_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox34.CheckedChanged
        If FormReady Then
            Value = CheckBox34.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CorpseLineOfFireFix", Value)
        End If
    End Sub

    Private Sub CheckBox35_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox35.CheckedChanged
        If FormReady Then
            Value = CheckBox35.Checked
            If Value >= 1 Then Value = 2
            SetIni_ParamValue("UsePartySkills", Value)
        End If
    End Sub

    Private Sub ComboBox11_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox11.SelectedIndexChanged
        If FormReady Then SetIni_ParamValue("SaveInCombatFix", ComboBox11.SelectedIndex)
    End Sub

    Private Sub CheckBox32_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox32.CheckedChanged
        If FormReady Then
            Value = CheckBox32.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ObjCanSeeObj_ShootThru_Fix", Value)
        End If
    End Sub

    Private Sub NumericUpDown6_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown6.ValueChanged
        If FormReady Then SetIni_ParamValue("NPCsTryToSpendExtraAP", NumericUpDown6.Value)
    End Sub

    Private Sub CheckBox31_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox31.CheckedChanged
        If FormReady Then
            Value = CheckBox31.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DrugExploitFix", Value)
        End If
    End Sub

    Private Sub NumericUpDown5_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumericUpDown5.ValueChanged
        If FormReady Then SetIni_ParamValue("RiflescopePenalty", NumericUpDown5.Value)
    End Sub

    '
    Private Sub TabControl1_Selecting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Selecting
        If e.TabPageIndex = 4 And Not (OnlyOnce) Then
            OnlyOnce = True
            DatDesc()
        End If
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ListView1.MouseDoubleClick
        Dim promt() As String = {"Введите описание для ", "Enter description for "}
        Dim indx As Byte = CByte(ToolTip1.Active)
        If indx > 1 Then indx = 1
        Dim Name As String = ListView1.FocusedItem.Text 'ListView1.FocusedItem.SubItems(1).Text
        Dim desc As String = InputBox(promt(indx) & Name, , ListView1.Items(ListView1.FocusedItem.Index).SubItems(1).Text) '
        If desc <> Nothing AndAlso desc <> ListView1.FocusedItem.SubItems(1).Text Then
            ListView1.FocusedItem.SubItems(1).Text = desc
            Dim desc_dat() As String = {Name, desc}
            IO.File.WriteAllLines(Main_Path & "\desc.id", desc_dat)
            Shell(Main_Path & "\dat.unp d " & Name & " desc.id", AppWinStyle.Hide, True, 1000)
            Shell(Main_Path & "\dat.unp a " & Name & " desc.id", AppWinStyle.Hide, True, 1000)
            IO.File.Delete(Main_Path & "\desc.id")
        End If
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        IO.File.Delete(Main_Path & "\dat.unp")
    End Sub

End Class

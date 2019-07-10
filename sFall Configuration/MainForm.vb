Imports System.IO
Imports System.Text

Public Class MainForm

    Private OnlyOnce, Changed, FormReady As Boolean
    Private Value As Byte, sValue As SByte

    Public Sub New()

        InitializeComponent()
        Me.Text &= My.Application.Info.Version.ToString

        If Not (File.Exists(App_Path & "\ddraw.ini")) Then
            For i = 0 To 3
                TabControl1.TabPages.RemoveAt(0)
            Next
            Button1.Enabled = False
            Button2.Enabled = False
            Button3.Enabled = False
            Exit Sub
        Else
            Ddraw_ini = File.ReadAllLines(App_Path & "\ddraw.ini", Encoding.Default).ToList
        End If

        If File.Exists(App_Path & "\f2_res.ini") Then
            F2res_ini = File.ReadAllLines(App_Path & "\f2_res.ini", Encoding.Default)
        End If
        If File.Exists(App_Path & "\sfall-mods.ini") Then
            mods_ini = File.ReadAllLines(App_Path & "\sfall-mods.ini", Encoding.Default)
        End If

        ListBox1.Items.AddRange(Resolution.GetResolution)
        Initialization()
    End Sub

    Private Sub MainForm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        If Ddraw_ini.Count = 0 Then
            DatDesc()
            MsgBox("File ddraw.ini not found.")
        End If
        FormReady = True
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If ListBox1.Items.Count Then Set_ListBoxRes()
    End Sub

    Private Sub Initialization()
        Dim valueStr As String, str() As String

        If GetIni_Param("sFallConfigurator") = "ENG" Then
            ComboBox8.SelectedIndex = 1
        Else
            ComboBox8.SelectedIndex = 0
        End If

        On Error Resume Next

        cbDisplayKarma.Checked = CBool(GetIni_Param("DisplayKarmaChanges"))
        cbScrollingQuestsList.Checked = CBool(GetIni_Param("UseScrollingQuestsList"))

        valueStr = GetIni_Param("StackEmptyWeapons")
        If valueStr <> Nothing Then
            cbStackEmptyWeapons.Enabled = True
            cbStackEmptyWeapons.CheckState = CheckState.Unchecked
            cbStackEmptyWeapons.Checked = CBool(valueStr)
        End If

        valueStr = GetIni_Param("ToggleItemHighlightsKey")
        If valueStr = Nothing Then
            valueStr = GetIni_Param(mods_ini, "Key")
        End If
        If valueStr <> Nothing Then
            cbItemHighlightsKey.Enabled = True
            cbItemHighlightsKey.Checked = CBool(valueStr)
        End If

        'v3.7
        valueStr = GetIni_Param("DisplayBonusDamage")
        If valueStr <> Nothing Then
            cbBonusDamage.Enabled = True
            cbBonusDamage.CheckState = CheckState.Unchecked
            cbBonusDamage.Checked = CBool(valueStr)
        End If
        '
        valueStr = GetIni_Param(mods_ini, "Mode")
        If valueStr = Nothing Then
            valueStr = GetIni_Param("ControlCombat")
            'ElseIf GetIni_Param(mods_ini, "DisplayName") <> Nothing Then
            '    nudShowNameBar.Enabled = True
        End If
        If valueStr <> Nothing Then
            cbControlCombat.Enabled = True
            cbControlCombat.Checked = CBool(valueStr)
        End If
        '
        Dim chBool As Boolean
        Dim n As Integer = Get_Section_Line("[Speed]")
        For n = n + 1 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)).ToLower = "enable" Then
                chBool = CBool(GetIni_ValueParam(Ddraw_ini(n)))
                Exit For
            End If
        Next
        If GetIni_Param("SpeedMultiInitial") = "200" And chBool Then cbSpeedMultiInit.Checked = True
        '
        valueStr = GetIni_Param("CheckWeaponAmmoCost")
        If valueStr <> Nothing Then
            cbWeaponAmmoCost.Enabled = True
            cbWeaponAmmoCost.CheckState = CheckState.Unchecked
            cbWeaponAmmoCost.Checked = CBool(valueStr)
        End If
        cbExtraSaveSlots.Checked = CBool(GetIni_Param("ExtraSaveSlots"))

        valueStr = GetIni_Param("ReloadWeaponKey")
        If valueStr <> Nothing Then
            cbReloadWeapon.Enabled = True
            cbReloadWeapon.CheckState = CheckState.Unchecked
            cbReloadWeapon.Checked = CBool(valueStr)
        End If

        valueStr = GetIni_Param("MotionScannerFlags")
        If valueStr = "4" Then
            cmbMotionScanner.SelectedIndex = 3
        Else
            cmbMotionScanner.SelectedIndex = CInt(valueStr)
        End If

        cmbDamageFormula.SelectedIndex = GetIni_Param("DamageFormula")

        valueStr = GetIni_Param("FastShotFix")
        If valueStr <> Nothing Then
            cmbFastShotFix.SelectedIndex = CInt(valueStr)
        Else
            cmbFastShotFix.Enabled = False
        End If

        Select Case GetIni_Param("TimeLimit")
            Case "0"
                cmbTimeLimit.SelectedIndex = 0
            Case "-1"
                cmbTimeLimit.SelectedIndex = 1
            Case "-2"
                cmbTimeLimit.SelectedIndex = 2
            Case "-3"
                cmbTimeLimit.SelectedIndex = 3
            Case Else
                cmbTimeLimit.SelectedIndex = 4
        End Select
        '
        valueStr = GetIni_Param("Mode")
        If CInt(valueStr) > 0 Then
            cmbMode.SelectedIndex = CInt(valueStr) - 3
        Else
            cmbMode.SelectedIndex = 0
        End If

        NumericUpDown4.Value = GetIni_Param("GraphicsWidth")
        NumericUpDown3.Value = GetIni_Param("GraphicsHeight")
        cbGPUBlt.Checked = CBool(GetIni_Param("GPUBlt"))

        CheckBox14.Checked = CBool(GetIni_Param("Use32BitHeadGraphics"))

        cbSkipOpeningMovies.Checked = CBool(GetIni_Param("SkipOpeningMovies"))
        cbSpeedInterfaceCounter.Checked = CBool(GetIni_Param("SpeedInterfaceCounterAnims"))
        cbExplosionsEmitLight.Checked = CBool(GetIni_Param("ExplosionsEmitLight"))
        NumericUpDown1.Value = GetIni_Param("DialogPanelAnimDelay")
        NumericUpDown2.Value = GetIni_Param("CombatPanelAnimDelay")
        '
        valueStr = GetIni_Param("DebugMode")
        If valueStr <> Nothing Then
            cmbDebugLog.SelectedIndex = 1
            Value = CByte(valueStr)
            cbDebugMode.Checked = CBool(Value)
            If cbDebugMode.Checked Then
                cmbDebugLog.SelectedIndex = Value - 1
            End If
            cmbDebugLog.Enabled = cbDebugMode.Checked
        Else
            cbDebugMode.Enabled = False
            cbDebugMode.CheckState = CheckState.Indeterminate
            cmbDebugLog.Enabled = False
        End If

        valueStr = GetIni_Param("SkipSizeCheck")
        If valueStr <> Nothing Then
            cbSkipSize.Checked = CBool(valueStr)
        Else
            cbSkipSize.Enabled = False
            cbSkipSize.CheckState = CheckState.Indeterminate
        End If

        valueStr = GetIni_Param("AllowUnsafeScripting")
        If valueStr <> Nothing Then
            cbAllowUnsafe.Checked = CBool(valueStr)
        Else
            cbAllowUnsafe.Enabled = False
            cbAllowUnsafe.CheckState = CheckState.Indeterminate
        End If

        If CInt(GetIni_Param("ProcessorIdle")) > 0 Then
            cbProcessorIdle.Checked = True
        End If
        cbSingleCore.Checked = CBool(GetIni_Param("SingleCore"))

        valueStr = GetIni_Param("ExtraCRC")
        If valueStr <> Nothing Then
            tbExtraCRC.Text = valueStr
        Else
            tbExtraCRC.Enabled = False
        End If

        cbReverseMouseButtons.Checked = CBool(GetIni_Param("ReverseMouseButtons"))
        numMouseSensitivity.Value = CDec(GetIni_Param("MouseSensitivity"))

        'Advanced
        cbObjCanSeeObj.Checked = CBool(GetIni_Param("ObjCanSeeObj_ShootThru_Fix"))
        cbCorpseLine.Checked = CBool(GetIni_Param("CorpseLineOfFireFix"))
        cbRemoveCriticalTime.Checked = CBool(GetIni_Param("RemoveCriticalTimelimits"))
        cbSaveInCombat.SelectedIndex = GetIni_Param("SaveInCombatFix")
        NumericUpDown6.Value = GetIni_Param("NPCsTryToSpendExtraAP")
        cbHeroAppearMod.Checked = CBool(GetIni_Param("EnableHeroAppearanceMod"))
        '
        valueStr = GetIni_Param("HighlightContainers")
        If valueStr = Nothing Then
            valueStr = GetIni_Param("TurnHighlightContainers") 'Crafty 
            If valueStr = Nothing Then
                valueStr = GetIni_Param(mods_ini, "Containers")
            End If
        End If
        If valueStr <> Nothing Then
            cbHighlightContainers.Enabled = True
            cbHighlightContainers.CheckState = CheckState.Unchecked
            cbHighlightContainers.Checked = CBool(valueStr)
        End If

        valueStr = GetIni_Param("FreeWeight")
        If valueStr <> Nothing Then
            cbFreeWeight.Enabled = True
            cbFreeWeight.CheckState = CheckState.Unchecked
            cbFreeWeight.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("EquipArmor")
        If valueStr <> Nothing Then
            cbEquipArmor.Enabled = True
            cbEquipArmor.CheckState = CheckState.Unchecked
            cbEquipArmor.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("AutoReloadWeapon")
        If valueStr <> Nothing Then
            cbAutoReloadWeapon.Enabled = True
            cbAutoReloadWeapon.CheckState = CheckState.Unchecked
            cbAutoReloadWeapon.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("DontTurnOffSneakIfYouRun")
        If valueStr <> Nothing Then
            cbDontTurnOffSneak.Enabled = True
            cbDontTurnOffSneak.CheckState = CheckState.Unchecked
            cbDontTurnOffSneak.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("EnableMusicInDialogue")
        If valueStr <> Nothing Then
            cbMusicInDialogue.Enabled = True
            cbMusicInDialogue.CheckState = CheckState.Unchecked
            cbMusicInDialogue.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("UsePartySkills")
        If valueStr <> Nothing Then
            cbPartySkills.Enabled = True
            cbPartySkills.CheckState = CheckState.Unchecked
            cbPartySkills.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("NumbersInDialogue")
        If valueStr <> Nothing Then
            cbNumbersInDialogue.Enabled = True
            cbNumbersInDialogue.CheckState = CheckState.Unchecked
            cbNumbersInDialogue.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("AutoQuickSave")
        If valueStr <> Nothing Then
            cmbQuickSave.Enabled = True
            cmbQuickSave.SelectedIndex = CInt(valueStr)
        End If
        valueStr = GetIni_Param("CanSeeAndHearFix")
        If valueStr <> Nothing Then
            cbCanSeeHear.Enabled = True
            cbCanSeeHear.CheckState = CheckState.Unchecked
            cbCanSeeHear.Checked = CBool(valueStr)
        End If

        valueStr = GetIni_Param("ReloadReserve")
        Select Case valueStr
            Case "-1"
                cmbReloadReserve.SelectedIndex = 0
                cmbReloadReserve.Enabled = True
            Case "0"
                cmbReloadReserve.SelectedIndex = 1
                cmbReloadReserve.Enabled = True
            Case "1"
                cmbReloadReserve.SelectedIndex = 2
                cmbReloadReserve.Enabled = True
            Case Not (Nothing)
                cmbReloadReserve.SelectedText = valueStr
                cmbReloadReserve.Enabled = True
        End Select

        valueStr = GetIni_Param("InstantWeaponEquip")
        If valueStr <> Nothing Then
            cbInstanWeaponEquip.Enabled = True
            cbInstanWeaponEquip.CheckState = CheckState.Unchecked
            cbInstanWeaponEquip.Checked = CBool(valueStr)
        End If

        valueStr = GetIni_Param("PipboyTimeAnimDelay")
        If valueStr <> Nothing Then
            cbPipboyTimeAnim.Enabled = True
            cbPipboyTimeAnim.CheckState = CheckState.Unchecked
            If CInt(valueStr) <= 25 Then cbPipboyTimeAnim.Checked = True
        End If

        valueStr = GetIni_Param("XltKey")
        If valueStr <> Nothing Then
            cmbXltKey.Enabled = True
            cmbXltTable.Enabled = True
            Select Case valueStr
                Case "2"
                    cmbXltKey.SelectedIndex = 1
                Case "4"
                    cmbXltKey.SelectedIndex = 2
                Case Else
                    cmbXltKey.SelectedIndex = 0
            End Select
            str = GetIni_Param("XltTable").Split(",")
            If str(2) = 221 Then
                cmbXltTable.SelectedIndex = 0
            Else
                cmbXltTable.SelectedIndex = 1
            End If
        End If

        valueStr = GetIni_Param("DrugExploitFix")
        If valueStr <> Nothing Then
            cbDrugExploit.Enabled = True
            cbDrugExploit.CheckState = CheckState.Unchecked
            cbDrugExploit.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("CanSellUsedGeiger")
        If valueStr <> Nothing Then
            cbCanSellGeiger.Enabled = True
            cbCanSellGeiger.CheckState = CheckState.Unchecked
            cbCanSellGeiger.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("RiflescopePenalty")
        If valueStr <> Nothing Then
            NumericUpDown5.Enabled = True
            NumericUpDown5.Value = valueStr
        End If
        valueStr = GetIni_Param("OpenAndPassUnlockedDoors")
        If valueStr <> Nothing Then
            cbOpenAndPassUnlockedDoors.Enabled = True
            cbOpenAndPassUnlockedDoors.CheckState = CheckState.Unchecked
            cbOpenAndPassUnlockedDoors.Checked = CBool(valueStr)
        End If

        'for v4
        valueStr = GetIni_Param("FullItemDescInBarter")
        If valueStr <> Nothing Then
            cbFullItemDescInBarter.Enabled = True
            cbFullItemDescInBarter.CheckState = CheckState.Unchecked
            cbFullItemDescInBarter.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("DisplayElectricalResist")
        If valueStr <> Nothing Then
            cbDisplayElectricalDmgStat.Enabled = True
            cbDisplayElectricalDmgStat.CheckState = CheckState.Unchecked
            cbDisplayElectricalDmgStat.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("DisplaySecondWeaponRange")
        If valueStr <> Nothing Then
            cbDisplayWeaponRange.Enabled = True
            cbDisplayWeaponRange.CheckState = CheckState.Unchecked
            cbDisplayWeaponRange.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("PartyMemberTakeOffItem")
        If valueStr <> Nothing Then
            cbPartyMemberTakeOffItem.Enabled = True
            cbPartyMemberTakeOffItem.CheckState = CheckState.Unchecked
            cbPartyMemberTakeOffItem.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("DisablePunchKnockback")
        If valueStr <> Nothing Then
            cbPunchKnockback.Enabled = True
            cbPunchKnockback.CheckState = CheckState.Unchecked
            cbPunchKnockback.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("TakeBetterWeapons")
        If valueStr <> Nothing Then
            cbTakeBetterWeapons.Enabled = True
            cbTakeBetterWeapons.CheckState = CheckState.Unchecked
            cbTakeBetterWeapons.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("SmartBehavior")
        If valueStr <> Nothing Then
            cbAISmartBehavior.Enabled = True
            cbAISmartBehavior.CheckState = CheckState.Unchecked
            cbAISmartBehavior.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("TryToFindTargets")
        If valueStr <> Nothing Then
            cbTryFindTarget.Enabled = True
            cbTryFindTarget.CheckState = CheckState.Unchecked
            cbTryFindTarget.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("ItemPickUpFix")
        If valueStr <> Nothing Then
            cbItemPickUpFix.Enabled = True
            cbItemPickUpFix.CheckState = CheckState.Unchecked
            cbItemPickUpFix.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("SkipLoadingGameSetting")
        If valueStr <> Nothing Then
            cbSkipLoadingGameSetting.Enabled = True
            cbSkipLoadingGameSetting.CheckState = CheckState.Unchecked
            cbSkipLoadingGameSetting.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("UseScrollWheel")
        If valueStr <> Nothing Then
            cbUseScrollWheel.Enabled = True
            cbUseScrollWheel.CheckState = CheckState.Unchecked
            cbUseScrollWheel.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("ItemFastMoveKey")
        If valueStr <> Nothing Then
            cbItemFastMoveKey.Enabled = True
            cbItemFastMoveKey.CheckState = CheckState.Unchecked
            If valueStr.StartsWith("0x") Then
                valueStr = "&H" & valueStr.Remove(0, 2)
            End If
            If Val(valueStr) > 0 Then
                cbItemFastMoveKey.Checked = True
            End If
        End If
        valueStr = GetIni_Param("FastMoveFromContainer")
        If valueStr <> Nothing Then
            cbFastMoveContainer.Enabled = True
            cbFastMoveContainer.CheckState = CheckState.Unchecked
            cbFastMoveContainer.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("AutoQuickSavePage")
        If valueStr <> Nothing Then
            If cmbQuickSave.SelectedIndex > 0 Then cmbQuickSavePage.Enabled = cbExtraSaveSlots.Checked
            cmbQuickSavePage.Text = valueStr
        End If
        valueStr = GetIni_Param("WorldMapFontPatch")
        If valueStr <> Nothing Then
            cbWorldmapFontFix.Enabled = True
            cbWorldmapFontFix.CheckState = CheckState.Unchecked
            cbWorldmapFontFix.Checked = CBool(valueStr)
        End If
        valueStr = GetIni_Param("ExpandWorldMap")
        If valueStr <> Nothing Then
            cmbExpWorldmap.SelectedIndex = CInt(valueStr)
        Else
            cmbExpWorldmap.Enabled = False
        End If
        valueStr = GetIni_Param("ActionPointsBar")
        If valueStr <> Nothing Then
            cbActionPointsBar.Enabled = True
            cbActionPointsBar.CheckState = CheckState.Unchecked
            cbActionPointsBar.Checked = CBool(valueStr)
        End If

        'for HRP
        If F2res_ini IsNot Nothing Then
            cbIsGrayScale.Enabled = True
            cbIsGrayScale.CheckState = CheckState.Unchecked
            cbIsGrayScale.Checked = CBool(GetIni_Param(F2res_ini, "IS_GRAY_SCALE"))
            cbAmmoMetre.Enabled = True
            cbAmmoMetre.CheckState = CheckState.Unchecked
            cbAmmoMetre.Checked = CBool(GetIni_Param(F2res_ini, "ALTERNATE_AMMO_METRE"))
        End If
    End Sub

    Private Sub cbReloadWeapon_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbReloadWeapon.CheckedChanged
        If FormReady Then
            cbReloadWeapon.ForeColor = Color.MediumVioletRed
            Value = cbReloadWeapon.Checked
            If Value > 0 Then Value = 17 'DIK_W
            SetIni_ParamValue("ReloadWeaponKey", Value)
        End If
    End Sub

    Private Sub cbWeaponAmmoCost_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbWeaponAmmoCost.CheckedChanged
        If FormReady Then
            cbWeaponAmmoCost.ForeColor = Color.MediumVioletRed
            Value = cbWeaponAmmoCost.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CheckWeaponAmmoCost", Value)
        End If
    End Sub

    Private Sub cbDisplayKarma_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbDisplayKarma.CheckedChanged
        If FormReady Then
            cbDisplayKarma.ForeColor = Color.MediumVioletRed
            Value = cbDisplayKarma.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplayKarmaChanges", Value)
        End If
    End Sub

    Private Sub cbScrollingQuestsList_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbScrollingQuestsList.CheckedChanged
        If FormReady Then
            cbScrollingQuestsList.ForeColor = Color.MediumVioletRed
            Value = cbScrollingQuestsList.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("UseScrollingQuestsList", Value)
        End If
    End Sub

    Private Sub cbDontTurnOffSneak_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbDontTurnOffSneak.CheckedChanged
        If FormReady Then
            cbDontTurnOffSneak.ForeColor = Color.MediumVioletRed
            Value = cbDontTurnOffSneak.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DontTurnOffSneakIfYouRun", Value)
        End If
    End Sub

    Private Sub cbFreeWeight_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbFreeWeight.CheckedChanged
        If FormReady Then
            cbFreeWeight.ForeColor = Color.MediumVioletRed
            Value = cbFreeWeight.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("FreeWeight", Value)
        End If
    End Sub

    Private Sub cbEquipArmor_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbEquipArmor.CheckedChanged
        If FormReady Then
            cbEquipArmor.ForeColor = Color.MediumVioletRed
            Value = cbEquipArmor.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("EquipArmor", Value)
        End If
    End Sub

    Private Sub cbStackEmptyWeapons_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbStackEmptyWeapons.CheckedChanged
        If FormReady Then
            cbStackEmptyWeapons.ForeColor = Color.MediumVioletRed
            Value = cbStackEmptyWeapons.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("StackEmptyWeapons", Value)
        End If
    End Sub

    Private Sub cbAutoReloadWeapon_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbAutoReloadWeapon.CheckedChanged
        If FormReady Then
            cbAutoReloadWeapon.ForeColor = Color.MediumVioletRed
            Value = cbAutoReloadWeapon.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("AutoReloadWeapon", Value)
        End If
    End Sub

    Private Sub cbItemHighlightsKey_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbItemHighlightsKey.CheckedChanged
        If FormReady Then
            cbItemHighlightsKey.ForeColor = Color.MediumVioletRed
            Value = cbItemHighlightsKey.Checked
            If Value > 0 Then Value = 42 'shift key
            SetIni_ParamValue("ToggleItemHighlightsKey", Value)

            SetIni_ParamValue(mods_ini, "Key", Value)
        End If
    End Sub

    Private Sub cbHighlightContainers_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbHighlightContainers.CheckedChanged
        If FormReady Then
            cbHighlightContainers.ForeColor = Color.MediumVioletRed
            Value = cbHighlightContainers.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("HighlightContainers", Value)
            SetIni_ParamValue("TurnHighlightContainers", Value)

            SetIni_ParamValue(mods_ini, "Containers", Value)
            SetIni_ParamValue(mods_ini, "Corpses", Value)
        End If
    End Sub

    Private Sub cbBonusDamage_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbBonusDamage.CheckedChanged
        If FormReady Then
            cbBonusDamage.ForeColor = Color.MediumVioletRed
            Value = cbBonusDamage.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplayBonusDamage", Value)
        End If
    End Sub

    Private Sub cbMusicInDialogue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbMusicInDialogue.CheckedChanged
        If FormReady Then
            cbMusicInDialogue.ForeColor = Color.MediumVioletRed
            Value = cbMusicInDialogue.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("EnableMusicInDialogue", Value)
        End If
    End Sub

    Private Sub cbControlCombat_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbControlCombat.CheckedChanged
        If FormReady Then
            cbControlCombat.ForeColor = Color.MediumVioletRed
            Value = cbControlCombat.Checked
            If Value > 1 Then Value = 2 'Set to control all party members
            SetIni_ParamValue("ControlCombat", Value)

            SetIni_ParamValue(mods_ini, "Mode", Value)
        End If
    End Sub

    Private Sub cbSpeedMultiInit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbSpeedMultiInit.CheckedChanged
        If FormReady Then
            cbSpeedMultiInit.ForeColor = Color.MediumVioletRed
            Value = cbSpeedMultiInit.Checked
            If Value > 1 Then Value = 150 Else Value = 100
            Dim n As Integer = Get_Section_Line("[Speed]")
            If n = -1 Then Exit Sub
            For n = n + 1 To Ddraw_ini.Count - 1
                If GetIni_NameParam(Ddraw_ini(n)) = "enable" Then Ddraw_ini(n) = "Enable=1"
            Next
            'SetIni_ParamValue("Enable", 1)
            SetIni_ParamValue("SpeedMultiInitial", Value)
        End If
    End Sub

    Private Sub ExtraSaveSlots(ByVal sender As Object, ByVal e As EventArgs) Handles cbExtraSaveSlots.CheckedChanged
        If FormReady Then
            cbExtraSaveSlots.ForeColor = Color.MediumVioletRed
            If cmbQuickSave.SelectedIndex > 0 AndAlso cmbQuickSavePage.Text.Length > 0 Then cmbQuickSavePage.Enabled = cbExtraSaveSlots.Checked
            Value = cbExtraSaveSlots.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ExtraSaveSlots", Value)
        End If
    End Sub

    Private Sub QuickSave(ByVal sender As Object, ByVal e As EventArgs) Handles cmbQuickSave.SelectedIndexChanged
        If FormReady Then
            If cbExtraSaveSlots.Checked AndAlso cmbQuickSavePage.Text.Length > 0 Then cmbQuickSavePage.Enabled = (cmbQuickSave.SelectedIndex > 0)
            SetIni_ParamValue("AutoQuickSave", cmbQuickSave.SelectedIndex)
        End If
    End Sub

    Private Sub ReloadReserve(ByVal sender As Object, ByVal e As EventArgs) Handles cmbReloadReserve.TextChanged
        If FormReady Then
            If cmbReloadReserve.SelectedIndex = 0 Then
                SetIni_ParamValue("ReloadReserve", -1)
            ElseIf cmbReloadReserve.SelectedIndex = 1 Then
                SetIni_ParamValue("ReloadReserve", 0)
            ElseIf cmbReloadReserve.SelectedIndex = 2 Then
                SetIni_ParamValue("ReloadReserve", 1)
            Else : SetIni_ParamValue("ReloadReserve", cmbReloadReserve.Text) : End If
        End If
    End Sub

    Private Sub MotionScanner(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMotionScanner.SelectedIndexChanged
        If FormReady Then
            Value = cmbMotionScanner.SelectedIndex
            If Value = 3 Then Value = 4
            SetIni_ParamValue("MotionScannerFlags", Value)
        End If
    End Sub

    Private Sub DamageFormula(ByVal sender As Object, ByVal e As EventArgs) Handles cmbDamageFormula.SelectedIndexChanged
        If FormReady Then
            Value = cmbDamageFormula.SelectedIndex
            If Value = 3 Then Value = 0
            SetIni_ParamValue("DamageFormula", Value)
        End If
    End Sub

    Private Sub TimeLimit(ByVal sender As Object, ByVal e As EventArgs) Handles cmbTimeLimit.SelectedIndexChanged
        If FormReady Then
            Value = cmbTimeLimit.SelectedIndex
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

    Private Sub FastShotFix(ByVal sender As Object, ByVal e As EventArgs) Handles cmbFastShotFix.SelectedIndexChanged
        If FormReady Then
            SetIni_ParamValue("FastShotFix", cmbFastShotFix.SelectedIndex)
        End If
    End Sub

    Private Sub Mode(ByVal sender As Object, ByVal e As EventArgs) Handles cmbMode.SelectedIndexChanged
        If FormReady Then
            Value = cmbMode.SelectedIndex
            If Value > 0 Then Value += 3
            SetIni_ParamValue("Mode", Value)
        End If
    End Sub

    Private Sub ResolutionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles NumericUpDown3.Leave, NumericUpDown4.Leave
        SetIni_ParamValue("GraphicsHeight", NumericUpDown3.Value)
        SetIni_ParamValue("GraphicsWidth", NumericUpDown4.Value)

        SetIni_ParamValue(F2res_ini, "SCR_HEIGHT", NumericUpDown3.Value)
        SetIni_ParamValue(F2res_ini, "SCR_WIDTH", NumericUpDown4.Value)
    End Sub

    Private Sub CheckBox14_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CheckBox14.CheckedChanged
        If FormReady Then
            Value = CheckBox14.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("Use32BitHeadGraphics", Value)
        End If
    End Sub

    Private Sub GPUBlt(ByVal sender As Object, ByVal e As EventArgs) Handles cbGPUBlt.CheckedChanged
        If FormReady Then
            cbGPUBlt.ForeColor = Color.MediumVioletRed
            Value = cbGPUBlt.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("GPUBlt", Value)
        End If
    End Sub

    Private Sub SkipOpeningMovies(ByVal sender As Object, ByVal e As EventArgs) Handles cbSkipOpeningMovies.CheckedChanged
        If FormReady Then
            cbSkipOpeningMovies.ForeColor = Color.MediumVioletRed
            Value = cbSkipOpeningMovies.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SkipOpeningMovies", Value)
        End If
    End Sub

    Private Sub SpeedInterfaceCounter(ByVal sender As Object, ByVal e As EventArgs) Handles cbSpeedInterfaceCounter.CheckedChanged
        If FormReady Then
            cbSpeedInterfaceCounter.ForeColor = Color.MediumVioletRed
            Value = cbSpeedInterfaceCounter.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SpeedInterfaceCounterAnims", Value)
        End If
    End Sub

    Private Sub ExplosionsEmitLight(ByVal sender As Object, ByVal e As EventArgs) Handles cbExplosionsEmitLight.CheckedChanged
        If FormReady Then
            cbExplosionsEmitLight.ForeColor = Color.MediumVioletRed
            Value = cbExplosionsEmitLight.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ExplosionsEmitLight", Value)
        End If
    End Sub

    Private Sub DialogPanelAnimDelay(ByVal sender As Object, ByVal e As EventArgs) Handles NumericUpDown1.Leave
        SetIni_ParamValue("DialogPanelAnimDelay", NumericUpDown1.Value)
    End Sub

    Private Sub CombatPanelAnimDelay(ByVal sender As Object, ByVal e As EventArgs) Handles NumericUpDown2.Leave
        SetIni_ParamValue("CombatPanelAnimDelay", NumericUpDown2.Value)
    End Sub

    Private Sub DebugMode(ByVal sender As Object, ByVal e As EventArgs) Handles cbDebugMode.CheckedChanged
        If FormReady Then
            cmbDebugLog.Enabled = cbDebugMode.Checked
            If cbDebugMode.Checked Then
                SetIni_ParamValue("DebugMode", cmbDebugLog.SelectedIndex + 1)
                SetDebugSection()
            Else
                SetIni_ParamValue("DebugMode", 0)
            End If
            EnableDebug()
        End If
    End Sub

    Private Sub DebugLog(ByVal sender As Object, ByVal e As EventArgs) Handles cmbDebugLog.SelectedIndexChanged
        If FormReady Then
            SetIni_ParamValue("DebugMode", cmbDebugLog.SelectedIndex + 1)
        End If
    End Sub

    Private Sub SkipSize(ByVal sender As Object, ByVal e As EventArgs) Handles cbSkipSize.CheckedChanged
        If FormReady Then
            cbSkipSize.ForeColor = Color.MediumVioletRed
            Value = cbSkipSize.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SkipSizeCheck", Value)
            EnableDebug()
        End If
    End Sub

    Private Sub AllowUnsafe(ByVal sender As Object, ByVal e As EventArgs) Handles cbAllowUnsafe.CheckedChanged
        If FormReady Then
            cbAllowUnsafe.ForeColor = Color.MediumVioletRed
            Value = cbAllowUnsafe.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("AllowUnsafeScripting", Value)
            EnableDebug()
        End If
    End Sub

    Private Sub ProcessorIdle(ByVal sender As Object, ByVal e As EventArgs) Handles cbProcessorIdle.CheckedChanged
        If FormReady Then
            cbProcessorIdle.ForeColor = Color.MediumVioletRed
            If cbProcessorIdle.Checked = 0 Then SetIni_ParamValue("ProcessorIdle", -1) Else SetIni_ParamValue("ProcessorIdle", 1)
        End If
    End Sub

    Private Sub SingleCore(ByVal sender As Object, ByVal e As EventArgs) Handles cbSingleCore.CheckedChanged
        If FormReady Then
            cbSingleCore.ForeColor = Color.MediumVioletRed
            Value = cbSingleCore.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SingleCore", Value)
        End If
    End Sub

    Private Sub ExtraCRC_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbExtraCRC.TextChanged
        If FormReady Then
            Changed = True
        End If
    End Sub

    Private Sub ExtraCRC_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles tbExtraCRC.Leave
        If Changed Then
            StoreIniCrc()
        End If
        Changed = False
    End Sub

    Friend Sub StoreIniCrc()
        If tbExtraCRC.Enabled Then
            SetIni_ParamValue("ExtraCRC", tbExtraCRC.Text)
        Else
            Dim n As Integer = GetIni_Param_Line("ExtraCRC")
            If n = -1 Then n = GetIni_Param_Line(";ExtraCRC")
            If n <> -1 Then
                Ddraw_ini(n) = "ExtraCRC=" & tbExtraCRC.Text
                tbExtraCRC.Enabled = True
            End If
        End If
        EnableDebug()
    End Sub

    Private Sub ReverseMouseButtons(ByVal sender As Object, ByVal e As EventArgs) Handles cbReverseMouseButtons.CheckedChanged
        If FormReady Then
            cbReverseMouseButtons.ForeColor = Color.MediumVioletRed
            Value = cbReverseMouseButtons.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ReverseMouseButtons", Value)
        End If
    End Sub

    Private Sub numMouseSensitivity_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles numMouseSensitivity.Leave
        SetIni_ParamValue("MouseSensitivity", numMouseSensitivity.Value)
    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ComboBox8.SelectedIndexChanged
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
                Ddraw_ini.Add(Nothing)
                Ddraw_ini.Add(";Setting language for sFallConfigurator")
                Ddraw_ini.Add("sFallConfigurator=" & ComboBox8.Text)
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If FormReady Then
            Dim str As String = ListBox1.SelectedItem
            Dim res() As String = str.Split("X")
            NumericUpDown4.Value = res(0).Trim
            NumericUpDown3.Value = res(1).Trim
            ResolutionChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub IsGrayScale(ByVal sender As Object, ByVal e As EventArgs) Handles cbIsGrayScale.CheckedChanged
        If FormReady Then
            cbIsGrayScale.ForeColor = Color.MediumVioletRed
            Value = cbIsGrayScale.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue(F2res_ini, "IS_GRAY_SCALE", Value)
        End If
    End Sub

    Private Sub AmmoMetre(ByVal sender As Object, ByVal e As EventArgs) Handles cbAmmoMetre.CheckedChanged
        If FormReady Then
            cbAmmoMetre.ForeColor = Color.MediumVioletRed
            Value = cbAmmoMetre.Checked
            If Value > 1 Then Value = 2
            SetIni_ParamValue(F2res_ini, "ALTERNATE_AMMO_METRE", Value)
        End If
    End Sub

    Private Sub InstanWeaponEquip(ByVal sender As Object, ByVal e As EventArgs) Handles cbInstanWeaponEquip.CheckedChanged
        If FormReady Then
            cbInstanWeaponEquip.ForeColor = Color.MediumVioletRed
            Value = cbInstanWeaponEquip.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("InstantWeaponEquip", Value)
            SetIni_ParamValue("InstanWeaponEquip", Value)
        End If
    End Sub

    Private Sub PipboyTimeAnim(ByVal sender As Object, ByVal e As EventArgs) Handles cbPipboyTimeAnim.CheckedChanged
        If FormReady Then
            cbPipboyTimeAnim.ForeColor = Color.MediumVioletRed
            If cbPipboyTimeAnim.Checked = True Then
                SetIni_ParamValue("PipboyTimeAnimDelay", 10)
            Else
                SetIni_ParamValue("PipboyTimeAnimDelay", 50)
            End If
        End If
    End Sub

    Private Sub XltKey(ByVal sender As Object, ByVal e As EventArgs) Handles cmbXltKey.SelectedIndexChanged
        If FormReady Then
            Value = cmbXltKey.SelectedIndex
            If Value < 2 Then
                SetIni_ParamValue("XltKey", Value + 1)
            Else
                SetIni_ParamValue("XltKey", 4)
            End If
        End If
    End Sub

    Private Sub XltTable(ByVal sender As Object, ByVal e As EventArgs) Handles cmbXltTable.SelectedIndexChanged
        If FormReady Then
            If cmbXltTable.SelectedIndex = 0 Then
                SetIni_ParamValue("XltTable", "32,33,221,35,36,37,38,253,40,41,42,43,225,45,254,47,48,49,50,51,52,53,54,55,56,57,198,230,193,61,222,63,64,212,200,209,194,211,192,207,208,216,206,203,196,220,210,217,199,201,202,219,197,195,204,214,215,205,223,245,92,250,94,95,96,244,232,241,226,243,224,239,240,248,238,235,228,252,242,249,231,233,234,251,229,227,236,246,247,237,255,213,124,218")
            Else
                SetIni_ParamValue("XltTable", "32,33,157,35,36,37,38,237,40,41,42,43,161,45,238,47,48,49,50,51,52,53,54,55,56,57,134,166,129,61,158,63,64,148,136,145,130,147,128,143,144,152,142,139,132,156,146,153,135,137,138,155,133,131,140,150,151,141,159,229,92,234,94,95,96,228,168,225,162,227,160,175,224,232,174,171,164,236,226,233,167,169,170,235,165,163,172,230,231,173,239,240,124,154")
            End If
        End If
    End Sub

    Private Sub CanSellGeiger(ByVal sender As Object, ByVal e As EventArgs) Handles cbCanSellGeiger.CheckedChanged
        If FormReady Then
            cbCanSellGeiger.ForeColor = Color.MediumVioletRed
            Value = cbCanSellGeiger.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CanSellUsedGeiger", Value)
        End If
    End Sub

    Private Sub RemoveCriticalTime(ByVal sender As Object, ByVal e As EventArgs) Handles cbRemoveCriticalTime.CheckedChanged
        If FormReady Then
            cbRemoveCriticalTime.ForeColor = Color.MediumVioletRed
            Value = cbRemoveCriticalTime.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("RemoveCriticalTimelimits", Value)
        End If
    End Sub

    Private Sub NumbersInDialogue(ByVal sender As Object, ByVal e As EventArgs) Handles cbNumbersInDialogue.CheckedChanged
        If FormReady Then
            cbNumbersInDialogue.ForeColor = Color.MediumVioletRed
            Value = cbNumbersInDialogue.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("NumbersInDialogue", Value)
        End If
    End Sub

    Private Sub CorpseLine(ByVal sender As Object, ByVal e As EventArgs) Handles cbCorpseLine.CheckedChanged
        If FormReady Then
            cbCorpseLine.ForeColor = Color.MediumVioletRed
            Value = cbCorpseLine.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CorpseLineOfFireFix", Value)
        End If
    End Sub

    Private Sub PartySkills(ByVal sender As Object, ByVal e As EventArgs) Handles cbPartySkills.CheckedChanged
        If FormReady Then
            cbPartySkills.ForeColor = Color.MediumVioletRed
            Value = cbPartySkills.Checked
            If Value >= 1 Then Value = 2
            SetIni_ParamValue("UsePartySkills", Value)
        End If
    End Sub

    Private Sub SaveInCombat(ByVal sender As Object, ByVal e As EventArgs) Handles cbSaveInCombat.SelectedIndexChanged
        If FormReady Then SetIni_ParamValue("SaveInCombatFix", cbSaveInCombat.SelectedIndex)
    End Sub

    Private Sub ObjCanSeeObj(ByVal sender As Object, ByVal e As EventArgs) Handles cbObjCanSeeObj.CheckedChanged
        If FormReady Then
            cbObjCanSeeObj.ForeColor = Color.MediumVioletRed
            Value = cbObjCanSeeObj.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ObjCanSeeObj_ShootThru_Fix", Value)
        End If
    End Sub

    Private Sub NPCsTryToSpendExtraAP(ByVal sender As Object, ByVal e As EventArgs) Handles NumericUpDown6.Leave
        SetIni_ParamValue("NPCsTryToSpendExtraAP", NumericUpDown6.Value)
    End Sub

    Private Sub DrugExploit(ByVal sender As Object, ByVal e As EventArgs) Handles cbDrugExploit.CheckedChanged
        If FormReady Then
            cbDrugExploit.ForeColor = Color.MediumVioletRed
            Value = cbDrugExploit.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DrugExploitFix", Value)
        End If
    End Sub

    Private Sub RiflescopePenalty(ByVal sender As Object, ByVal e As EventArgs) Handles NumericUpDown5.Leave
        SetIni_ParamValue("RiflescopePenalty", NumericUpDown5.Value)
    End Sub

    Private Sub CanSeeHear(ByVal sender As Object, ByVal e As EventArgs) Handles cbCanSeeHear.CheckedChanged
        If FormReady Then
            cbCanSeeHear.ForeColor = Color.MediumVioletRed
            Value = cbCanSeeHear.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("CanSeeAndHearFix", Value)
        End If
    End Sub

    Private Sub HeroAppearMod(ByVal sender As Object, ByVal e As EventArgs) Handles cbHeroAppearMod.CheckedChanged
        If FormReady Then
            cbHeroAppearMod.ForeColor = Color.MediumVioletRed
            Value = cbHeroAppearMod.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("EnableHeroAppearanceMod", Value)
        End If
    End Sub

    '
    Private Sub TabControl1_Selecting(ByVal sender As Object, ByVal e As Windows.Forms.TabControlCancelEventArgs) Handles TabControl1.Selecting
        If e.TabPageIndex = 4 AndAlso Not (OnlyOnce) Then
            OnlyOnce = True
            DatDesc()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Save_ini()
        Process.Start("ddraw.ini") 'Process.Start("notepad.exe", "ddraw.ini")
        Application.Exit()
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Save_ini()
        Application.Exit()
    End Sub

    Private Sub Button4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button4.Click
        OpenFileDialog1.Filter = "Exe files|*.exe"
        OpenFileDialog1.InitialDirectory = App_Path
        OpenFileDialog1.FileName = "fallout2.exe" 'default exe
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        Check_CRC(OpenFileDialog1.SafeFileName)
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        On Error GoTo FO2EXE
        Check_Exe()
        Save_ini()
        Process.Start(GetIni_Param("sFallConfigatorGameExe"))
        GoTo EXITAPP
FO2EXE:
        On Error GoTo -1
        On Error GoTo SELEXE
        Check_CRC("fallout2.exe")
        Save_ini()
        Process.Start("fallout2.exe")
        GoTo EXITAPP
SELEXE:
        OpenFileDialog1.Filter = "Exe files|*.exe"
        OpenFileDialog1.InitialDirectory = App_Path
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub
        SetGameExe_Ini(OpenFileDialog1.SafeFileName)
        Check_Exe()
        Save_ini()
        Process.Start(GetIni_Param("sFallConfigatorGameExe"))
EXITAPP:
        Application.Exit()
    End Sub

    Private Sub Button5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button5.Click
        Dim strprm, strval As String
        Dim lineprm As Integer

        OpenFileDialog1.Filter = "Ini files|*.ini"
        OpenFileDialog1.InitialDirectory = App_Path
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then Exit Sub

        Dim Ddraw_old() As String = File.ReadAllLines(OpenFileDialog1.FileName, Encoding.Default)
        For Each line As String In Ddraw_old
            strprm = GetIni_NameParam(line)
            If strprm <> String.Empty Then
                strval = GetIni_ValueParam(line)
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
        Initialization()
        MsgBox("Done!")
    End Sub

    Private Sub ListView1_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ListView1.MouseDoubleClick
        InputDesc()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://yadi.sk/d/6S_zZOpKjZcxg")
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        File.Delete(App_Path & "\dat.unp")
    End Sub

    Private Sub FullItemDescInBarter(ByVal sender As Object, ByVal e As EventArgs) Handles cbFullItemDescInBarter.CheckedChanged
        If FormReady Then
            cbFullItemDescInBarter.ForeColor = Color.MediumVioletRed
            Value = cbFullItemDescInBarter.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("FullItemDescInBarter", Value)
        End If
    End Sub

    Private Sub DisplayElectricalDmgStat(ByVal sender As Object, ByVal e As EventArgs) Handles cbDisplayElectricalDmgStat.CheckedChanged
        If FormReady Then
            cbDisplayElectricalDmgStat.ForeColor = Color.MediumVioletRed
            Value = cbDisplayElectricalDmgStat.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplayElectricalResist", Value)
        End If
    End Sub

    Private Sub DisplayWeaponRange(ByVal sender As Object, ByVal e As EventArgs) Handles cbDisplayWeaponRange.CheckedChanged
        If FormReady Then
            cbDisplayWeaponRange.ForeColor = Color.MediumVioletRed
            Value = cbDisplayWeaponRange.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisplaySecondWeaponRange", Value)
        End If
    End Sub

    Private Sub PartyMemberTakeOffItem(ByVal sender As Object, ByVal e As EventArgs) Handles cbPartyMemberTakeOffItem.CheckedChanged
        If FormReady Then
            cbPartyMemberTakeOffItem.ForeColor = Color.MediumVioletRed
            Value = cbPartyMemberTakeOffItem.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("PartyMemberTakeOffItem", Value)
        End If
    End Sub

    Private Sub ItemFastMoveKey(ByVal sender As Object, ByVal e As EventArgs) Handles cbItemFastMoveKey.CheckedChanged
        If FormReady Then
            cbItemFastMoveKey.ForeColor = Color.MediumVioletRed
            Value = cbItemFastMoveKey.Checked
            If Value > 1 Then Value = 29 'Ctrl Key
            SetIni_ParamValue("ItemFastMoveKey", Value)
        End If
    End Sub

    Private Sub TakeBetterWeapons(ByVal sender As Object, ByVal e As EventArgs) Handles cbTakeBetterWeapons.CheckedChanged
        If FormReady Then
            cbTakeBetterWeapons.ForeColor = Color.MediumVioletRed
            Value = cbTakeBetterWeapons.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("TakeBetterWeapons", Value)
        End If
    End Sub

    Private Sub AISmartBehavior(ByVal sender As Object, ByVal e As EventArgs) Handles cbAISmartBehavior.CheckedChanged
        If FormReady Then
            cbAISmartBehavior.ForeColor = Color.MediumVioletRed
            Value = cbAISmartBehavior.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("SmartBehavior", Value)
        End If
    End Sub

    Private Sub ItemPickUpFix(ByVal sender As Object, ByVal e As EventArgs) Handles cbItemPickUpFix.CheckedChanged
        If FormReady Then
            cbItemPickUpFix.ForeColor = Color.MediumVioletRed
            Value = cbItemPickUpFix.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ItemPickUpFix", Value)
        End If
    End Sub

    Private Sub SkipLoadingGameSetting(ByVal sender As Object, ByVal e As EventArgs) Handles cbSkipLoadingGameSetting.CheckedChanged
        If FormReady Then
            cbSkipLoadingGameSetting.ForeColor = Color.MediumVioletRed
            Value = cbSkipLoadingGameSetting.Checked
            If Value > 1 Then Value = 2
            SetIni_ParamValue("SkipLoadingGameSetting", Value)
            SetIni_ParamValue("SkipLoadingGameSettings", Value)
        End If
    End Sub

    Private Sub cbTryFindTarget_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTryFindTarget.CheckedChanged
        If FormReady Then
            cbTryFindTarget.ForeColor = Color.MediumVioletRed
            Value = cbTryFindTarget.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("TryToFindTargets", Value)
        End If
    End Sub

    Private Sub UseScrollWheel(ByVal sender As Object, ByVal e As EventArgs) Handles cbUseScrollWheel.CheckedChanged
        If FormReady Then
            cbUseScrollWheel.ForeColor = Color.MediumVioletRed
            Value = cbUseScrollWheel.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("UseScrollWheel", Value)
        End If
    End Sub

    Private Sub QuickSavePage(ByVal sender As Object, ByVal e As EventArgs) Handles cmbQuickSavePage.SelectedIndexChanged, cmbQuickSavePage.TextUpdate
        If FormReady Then SetIni_ParamValue("AutoQuickSavePage", cmbQuickSavePage.Text)
    End Sub

    Private Sub OpenAndPassUnlockedDoors(ByVal sender As Object, ByVal e As EventArgs) Handles cbOpenAndPassUnlockedDoors.CheckedChanged
        If FormReady Then
            cbOpenAndPassUnlockedDoors.ForeColor = Color.MediumVioletRed
            Value = cbOpenAndPassUnlockedDoors.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("OpenAndPassUnlockedDoors", Value)
        End If
    End Sub

    Private Sub bReloadSetKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bReloadSetKey.Click

    End Sub

    Private Sub bItemFastMoveSetKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bItemFastMoveSetKey.Click

    End Sub

    Private Sub cbPunchKnockback_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPunchKnockback.CheckedChanged
        If FormReady Then
            cbPunchKnockback.ForeColor = Color.MediumVioletRed
            Value = cbPunchKnockback.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("DisablePunchKnockback", Value)
        End If
    End Sub

    Private Sub cbWorldmapFontFix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWorldmapFontFix.CheckedChanged
        If FormReady Then
            cbWorldmapFontFix.ForeColor = Color.MediumVioletRed
            Value = cbWorldmapFontFix.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("WorldMapFontPatch", Value)
        End If
    End Sub

    Private Sub cbFastMoveContainer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFastMoveContainer.CheckedChanged
        If FormReady Then
            cbFastMoveContainer.ForeColor = Color.MediumVioletRed
            Value = cbFastMoveContainer.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("FastMoveFromContainer", Value)
        End If
    End Sub

    Private Sub cmbExpWorldmap_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbExpWorldmap.SelectedIndexChanged
        If FormReady Then
            SetIni_ParamValue("ExpandWorldMap", cmbExpWorldmap.SelectedIndex)
        End If
    End Sub

    Private Sub cbActionPointsBar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbActionPointsBar.CheckedChanged
        If FormReady Then
            cbActionPointsBar.ForeColor = Color.MediumVioletRed
            Value = cbActionPointsBar.Checked
            If Value > 1 Then Value = 1
            SetIni_ParamValue("ActionPointsBar", Value)
        End If
    End Sub
End Class

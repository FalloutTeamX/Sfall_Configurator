Imports System.IO
Imports System.Text

Module M_Module

    Friend Ddraw_ini As List(Of String) = New List(Of String)
    Friend F2res_ini As String()
    Friend mods_ini As String()

    Friend App_Path As String = Application.StartupPath

    Friend f2_cfg As List(Of String) = New List(Of String)
    Friend f2_cfgFile As String = "\fallout2.cfg"
    Friend f2cfgIsPatch = False

    Private debugSection() As String = {
            "[debug]",
            "mode=environment",
            "output_map_data_info=0",
            "show_load_info=0",
            "show_script_messages=1",
            "show_tile_num=0" & vbNewLine
    }

    Friend Sub DatDesc()
        Dim path As String = App_Path & "\dat.unp"

        File.Delete(path)
        File.WriteAllBytes(path, My.Resources.dat2)
        File.SetAttributes(path, FileAttributes.Hidden)

        For Each datFile In Directory.GetFiles(App_Path, "*.dat", SearchOption.TopDirectoryOnly)
            Dim z As Integer = datFile.LastIndexOf("\") + 1
            datFile = datFile.Substring(z)

            Shell(App_Path & "\dat.unp x " & datFile & " desc.id", AppWinStyle.Hide, True, 1000)

            Dim descID As String() = Nothing
            path = App_Path & "\desc.id"
            If File.Exists(path) Then
                descID = File.ReadAllLines(path)
            End If
            If descID Is Nothing Then
                MainForm.ListView1.Items.Add(New ListViewItem(New String() {datFile, "<no description>"}))
            Else
                MainForm.ListView1.Items.Add(New ListViewItem(descID))
            End If
            File.Delete(path)
        Next
    End Sub

    Friend Sub InputDesc()
        Dim promt As String() = {"Введите описание для ", "Input description for "}

        Dim indx As Byte = CByte(MainForm.ToolTip1.Active)
        If indx > 1 Then indx = 1

        Dim Name As String = MainForm.ListView1.FocusedItem.Text
        Dim desc As String = InputBox(promt(indx) & Name, , MainForm.ListView1.Items(
                                      MainForm.ListView1.FocusedItem.Index).SubItems(1).Text)

        If desc <> String.Empty AndAlso desc <> MainForm.ListView1.FocusedItem.SubItems(1).Text Then
            MainForm.ListView1.FocusedItem.SubItems(1).Text = desc
            Dim desc_dat As String() = {Name, desc}
            File.WriteAllLines(App_Path & "\desc.id", desc_dat)
            Shell(App_Path & "\dat.unp d " & Name & " desc.id", AppWinStyle.Hide, True, 1000)
            Shell(App_Path & "\dat.unp a " & Name & " desc.id", AppWinStyle.Hide, True, 1000)
            File.Delete(App_Path & "\desc.id")
        End If
    End Sub

    Friend Sub Save_ini()
        File.WriteAllLines(App_Path & "\ddraw.ini", Ddraw_ini.ToArray, Encoding.Default)
        If F2res_ini IsNot Nothing Then File.WriteAllLines(App_Path & "\f2_res.ini", F2res_ini, Encoding.Default)
        If mods_ini IsNot Nothing Then File.WriteAllLines(App_Path & "\sfall-mods.ini", mods_ini, Encoding.Default)
        If f2cfgIsPatch Then File.WriteAllLines(App_Path & f2_cfgFile, f2_cfg.ToArray, Encoding.Default)
    End Sub

    Friend Sub Set_ListBoxRes()
        Dim index As Integer = -1
        Dim res As String = MainForm.NumericUpDown4.Value.ToString + " X " + MainForm.NumericUpDown3.Value.ToString
        For i As Integer = 0 To MainForm.ListBox1.Items.Count - 1
            If MainForm.ListBox1.Items(i).ToString.Contains(res) Then
                index = i
                Exit For
            End If
        Next
        If index <> -1 Then
            MainForm.ListBox1.SelectedIndex = index
        End If
    End Sub

    Friend Sub Check_Exe()
        Dim game_exe As String = vbNullString
        game_exe = GetIni_Param("sFallConfigatorGameExe")
        If game_exe = vbNullString Then game_exe = "fallout2.exe"
        If File.Exists(App_Path & "\" & game_exe) Then Check_CRC(game_exe)
    End Sub

    Friend Sub Check_CRC(ByVal game_exe As String)
        Dim equal As Boolean
        Dim crc() As String = Get_CRC(App_Path & "\" & game_exe)
        Dim crc_line() As String = MainForm.tbExtraCRC.Text.Split(",")

        For Each crcVal As String In crc
            For Each line As String In crc_line
                If line.Trim.ToUpper = crcVal OrElse line.Trim.ToUpper = "0X" & crcVal Then
                    equal = True
                    Exit For
                End If
            Next
            If Not (equal) Then
                If MainForm.tbExtraCRC.Text.Length > 0 Then
                    MainForm.tbExtraCRC.Text &= ", " & "0x" & crcVal 'заносим в тексбох
                Else
                    MainForm.tbExtraCRC.Text = "0x" & crcVal
                End If
                MainForm.StoreIniCrc() 'записать изменения в буффер
            End If
            equal = False
        Next
    End Sub

    Friend Function Get_CRC(ByVal path As String) As String()
        Dim Bytes() As Byte = File.ReadAllBytes(path)
        Dim LineCrc(1) As String

        LineCrc(0) = Hex(CalcCRC(Bytes, &HEDB88320UI)).ToUpper
        LineCrc(1) = Hex(CalcCRC(Bytes, &H1EDC6F41)).ToUpper

        Return LineCrc
    End Function

    Private Function CalcCRC(ByRef Bytes() As Byte, ByVal Polynomial As UInteger) As UInteger
        Dim Size As UInteger = Bytes.Length
        Dim Table(255) As UInteger
        Dim crc As UInteger = &HFFFFFFFFUI
        Dim r As UInteger
        '
        For i As UShort = 0 To 255
            r = i
            For j As Byte = 0 To 7
                If (r And 1) <> 0 Then : r = ((r >> 1) Xor Polynomial) : Else : r >>= 1 : End If
            Next
            Table(i) = r
        Next
        For i As UInteger = 0 To Size - 1
            crc = Table(CByte(crc And &HFF) Xor Bytes(i)) Xor (crc >> 8)
        Next
        Return crc Xor &HFFFFFFFFUI
    End Function

    Friend Sub SetGameExe_Ini(ByVal game_exe As String)
        'проверяем
        If GetIni_Param("sFallConfigatorGameExe") = Nothing Then
            Ddraw_ini.Add(Nothing)
            Ddraw_ini.Add(";Setting for sFallConfigurator")
            Ddraw_ini.Add("sFallConfigatorGameExe=" & game_exe)
        Else
            SetIni_ParamValue("sFallConfigatorGameExe", game_exe)
        End If
    End Sub

    Friend Function GetIni_Param(ByRef iniData() As String, ByVal param As String) As String
        If iniData Is Nothing Then
            Return Nothing
        End If

        param = param.ToLower
        For n As Integer = 0 To UBound(iniData)
            If GetIni_NameParam(iniData(n)) = param Then
                Return GetIni_ValueParam(iniData(n))
            End If
        Next

        Return Nothing
    End Function

    Friend Function GetIni_Param(ByVal param As String) As String
        param = param.ToLower
        For Each line As String In Ddraw_ini
            If GetIni_NameParam(line) = param Then
                Return GetIni_ValueParam(line)
            End If
        Next
        Return Nothing
    End Function

    Friend Function GetIni_NameParam(ByVal str As String) As String
        Dim m As Integer = InStr(2, str, "=")
        If m = 0 Then Return Nothing
        'возвращаем
        Return str.Substring(0, m - 1).ToLower
    End Function

    Friend Function GetIni_ValueParam(ByVal str As String) As String
        Dim m As Integer = InStr(2, str, "=")
        If m <= 0 Then Return Nothing
        'возвращаем
        Return str.Substring(m, str.Length - m)
    End Function

    Friend Sub SetIni_ParamValue(ByRef iniData() As String, ByVal param As String, ByVal value As String)
        If iniData Is Nothing Then
            Exit Sub
        End If

        param = param.ToLower
        For n As Integer = 0 To UBound(iniData)
            If GetIni_NameParam(iniData(n)) = param Then
                Dim m As Integer = iniData(n).IndexOf("=", 2)
                If m <= 0 Then Exit Sub
                'записываем
                iniData(n) = iniData(n).Substring(0, m + 1) & value
                Exit Sub
            End If
        Next
    End Sub

    Friend Sub SetIni_ParamValue(ByVal param As String, ByVal value As String)
        param = param.ToLower
        For n As Integer = 0 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Set_Value(n, value)
                Exit Sub
            End If
        Next
    End Sub

    Private Sub Set_Value(ByVal z As Integer, ByRef value As String)
        Dim m As Integer = Ddraw_ini(z).IndexOf("=", 2)
        If m <= 0 Then Exit Sub
        'записываем
        Ddraw_ini(z) = Ddraw_ini(z).Remove(m + 1) & value
    End Sub

    Friend Function GetIni_Param_Line(ByVal param As String) As Integer
        param = param.ToLower
        For n As Integer = 0 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Return n
            End If
        Next
        Return -1
    End Function

    Friend Function Get_Section_Line(ByVal section As String) As Integer
        Return GetIni_Section_Line(section, Ddraw_ini)
    End Function

    Friend Function GetIni_Section_Line(ByVal section As String, ByRef ini As List(Of String)) As Integer
        section = section.ToLower()
        For n As Integer = 0 To ini.Count - 1
            If ini(n).ToLower = section Then
                Return n
            End If
        Next
        Return -1
    End Function

    'for sfall v3.7+
    Friend Sub EnableDebug()
        Dim n As Integer = Get_Section_Line("[Debugging]")
        If n = -1 Then Exit Sub

        Dim val As Byte = (MainForm.tbExtraCRC.Enabled _
                          OrElse MainForm.cbSkipSize.Checked _
                          OrElse MainForm.cbDebugMode.Checked _
                          OrElse MainForm.cbAllowUnsafe.Checked)

        If val > 1 Then val = 1

        For n = n + 1 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)) = "enable" Then
                Ddraw_ini(n) = "Enable=" + CStr(val)
            End If
        Next
    End Sub

    Friend Sub SetDebugSection()
        Dim cfgFile As String = GetIni_Param("ConfigFile")
        If cfgFile <> Nothing Then
            f2_cfgFile = "\" & cfgFile
        End If
        If File.Exists(App_Path & f2_cfgFile) = False Then Exit Sub

        f2_cfg = File.ReadAllLines(App_Path & f2_cfgFile, Encoding.Default).ToList
        If GetIni_Section_Line("[Debug]", f2_cfg) = -1 Then
            f2_cfg.InsertRange(0, debugSection)
            f2cfgIsPatch = True
        End If
    End Sub
End Module

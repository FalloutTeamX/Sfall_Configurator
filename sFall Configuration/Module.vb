Imports System.IO
Imports System.Text

Module M_Module

    Friend Ddraw_ini As List(Of String) = New List(Of String)
    Friend F2res_ini As String()
    Friend mods_ini As String()

    Friend App_Path As String = Application.StartupPath

    Friend Sub DatDesc()
        Dim n As Integer

        File.WriteAllBytes(App_Path & "\dat.unp", My.Resources.dat2)
        File.SetAttributes(App_Path & "\dat.unp", FileAttributes.Hidden)

        Dim ListDat() As String = Directory.GetFiles(App_Path, "*.dat", SearchOption.TopDirectoryOnly)
        For Each dat In ListDat
            Dim z As Integer = dat.LastIndexOf("\") + 1
            dat = dat.Substring(z, dat.Length - z)
            'If dat.ToLower <> "master.dat" And dat <> "critter.dat" Then
            Shell(App_Path & "\dat.unp x " & dat & " desc.id", AppWinStyle.Hide, True)
            Dim desc() As String = File.ReadAllLines(App_Path & "\desc.id")
            MainForm.ListView1.Items.Add(dat)
            If desc(1) = Nothing Then
                MainForm.ListView1.Items(n).SubItems.Add("<unknown>") '& desc(1)
            Else
                MainForm.ListView1.Items(n).SubItems.Add(desc(1))
            End If
            File.Delete(App_Path & "\desc.id")
            n += 1
            'End If
        Next
    End Sub

    Friend Sub Save_ini()
        File.WriteAllLines(App_Path & "\ddraw.ini", Ddraw_ini.ToArray, Encoding.Default)
        If F2res_ini IsNot Nothing Then File.WriteAllLines(App_Path & "\f2_res.ini", F2res_ini, Encoding.Default)
        If mods_ini IsNot Nothing Then File.WriteAllLines(App_Path & "\sfall-mods.ini", mods_ini, Encoding.Default)
    End Sub

    Friend Sub Set_ListBoxRes()
        Dim indx As Integer = MainForm.ListBox1.Items.IndexOf(CStr(MainForm.NumericUpDown4.Value) + " X " _
                                                            + CStr(MainForm.NumericUpDown3.Value))
        MainForm.ListBox1.SelectedIndex = indx
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

        For n As Integer = 0 To UBound(iniData)
            If GetIni_NameParam(iniData(n)) = param Then
                Return GetIni_ValueParam(iniData(n))
            End If
        Next

        Return Nothing
    End Function

    Friend Function GetIni_Param(ByVal param As String) As String
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
        Return str.Substring(0, m - 1)
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
        For n As Integer = 0 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Return n
            End If
        Next
        Return -1
    End Function

    Friend Function GetIni_Section_Line(ByVal section As String) As Integer
        For n As Integer = 0 To Ddraw_ini.Count - 1
            If Ddraw_ini(n) = section Then
                Return n
            End If
        Next
        Return -1
    End Function

    'for sfall v3.7+
    Friend Sub EnableDebug()
        Dim n As Integer = GetIni_Section_Line("[Debugging]")
        If n = -1 Then Exit Sub

        Dim val As Byte = CByte(MainForm.tbExtraCRC.Enabled _
                          OrElse MainForm.cbSkipSize.Checked _
                          OrElse MainForm.cbDebugMode.Checked _
                          OrElse MainForm.cbAllowUnsafe.Checked)

        For n = n + 1 To Ddraw_ini.Count - 1
            If GetIni_NameParam(Ddraw_ini(n)) = "Enable" Then
                Ddraw_ini(n) = "Enable=" + CStr(val)
            End If
        Next
    End Sub

End Module

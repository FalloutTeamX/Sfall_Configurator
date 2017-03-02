Module M_Module

    'Private CrcTable() As UInteger = {
    '&H0, &H77073096, &HEE0E612CUI, &H990951BAUI,
    '&H76DC419, &H706AF48F, &HE963A535UI, &H9E6495A3UI,
    '&HEDB8832, &H79DCB8A4, &HE0D5E91EUI, &H97D2D988UI,
    '&H9B64C2B, &H7EB17CBD, &HE7B82D07UI, &H90BF1D91UI,
    '&H1DB71064, &H6AB020F2, &HF3B97148UI, &H84BE41DEUI,
    '&H1ADAD47D, &H6DDDE4EB, &HF4D4B551UI, &H83D385C7UI,
    '&H136C9856, &H646BA8C0, &HFD62F97AUI, &H8A65C9ECUI,
    '&H14015C4F, &H63066CD9, &HFA0F3D63UI, &H8D080DF5UI,
    '&H3B6E20C8, &H4C69105E, &HD56041E4UI, &HA2677172UI,
    '&H3C03E4D1, &H4B04D447, &HD20D85FDUI, &HA50AB56BUI,
    '&H35B5A8FA, &H42B2986C, &HDBBBC9D6UI, &HACBCF940UI,
    '&H32D86CE3, &H45DF5C75, &HDCD60DCFUI, &HABD13D59UI,
    '&H26D930AC, &H51DE003A, &HC8D75180UI, &HBFD06116UI,
    '&H21B4F4B5, &H56B3C423, &HCFBA9599UI, &HB8BDA50FUI,
    '&H2802B89E, &H5F058808, &HC60CD9B2UI, &HB10BE924UI,
    '&H2F6F7C87, &H58684C11, &HC1611DABUI, &HB6662D3DUI,
    '&H76DC4190, &H1DB7106, &H98D220BCUI, &HEFD5102AUI,
    '&H71B18589, &H6B6B51F, &H9FBFE4A5UI, &HE8B8D433UI,
    '&H7807C9A2, &HF00F934, &H9609A88EUI, &HE10E9818UI,
    '&H7F6A0DBB, &H86D3D2D, &H91646C97UI, &HE6635C01UI,
    '&H6B6B51F4, &H1C6C6162, &H856530D8UI, &HF262004EUI,
    '&H6C0695ED, &H1B01A57B, &H8208F4C1UI, &HF50FC457UI,
    '&H65B0D9C6, &H12B7E950, &H8BBEB8EAUI, &HFCB9887CUI,
    '&H62DD1DDF, &H15DA2D49, &H8CD37CF3UI, &HFBD44C65UI,
    '&H4DB26158, &H3AB551CE, &HA3BC0074UI, &HD4BB30E2UI,
    '&H4ADFA541, &H3DD895D7, &HA4D1C46DUI, &HD3D6F4FBUI,
    '&H4369E96A, &H346ED9FC, &HAD678846UI, &HDA60B8D0UI,
    '&H44042D73, &H33031DE5, &HAA0A4C5FUI, &HDD0D7CC9UI,
    '&H5005713C, &H270241AA, &HBE0B1010UI, &HC90C2086UI,
    '&H5768B525, &H206F85B3, &HB966D409UI, &HCE61E49FUI,
    '&H5EDEF90E, &H29D9C998, &HB0D09822UI, &HC7D7A8B4UI,
    '&H59B33D17, &H2EB40D81, &HB7BD5C3BUI, &HC0BA6CADUI,
    '&HEDB88320UI, &H9ABFB3B6UI, &H3B6E20C, &H74B1D29A,
    '&HEAD54739UI, &H9DD277AFUI, &H4DB2615, &H73DC1683,
    '&HE3630B12UI, &H94643B84UI, &HD6D6A3E, &H7A6A5AA8,
    '&HE40ECF0BUI, &H9309FF9DUI, &HA00AE27, &H7D079EB1,
    '&HF00F9344UI, &H8708A3D2UI, &H1E01F268, &H6906C2FE,
    '&HF762575DUI, &H806567CBUI, &H196C3671, &H6E6B06E7,
    '&HFED41B76UI, &H89D32BE0UI, &H10DA7A5A, &H67DD4ACC,
    '&HF9B9DF6FUI, &H8EBEEFF9UI, &H17B7BE43, &H60B08ED5,
    '&HD6D6A3E8UI, &HA1D1937EUI, &H38D8C2C4, &H4FDFF252,
    '&HD1BB67F1UI, &HA6BC5767UI, &H3FB506DD, &H48B2364B,
    '&HD80D2BDAUI, &HAF0A1B4CUI, &H36034AF6, &H41047A60,
    '&HDF60EFC3UI, &HA867DF55UI, &H316E8EEF, &H4669BE79,
    '&HCB61B38CUI, &HBC66831AUI, &H256FD2A0, &H5268E236,
    '&HCC0C7795UI, &HBB0B4703UI, &H220216B9, &H5505262F,
    '&HC5BA3BBEUI, &HB2BD0B28UI, &H2BB45A92, &H5CB36A04,
    '&HC2D7FFA7UI, &HB5D0CF31UI, &H2CD99E8B, &H5BDEAE1D,
    '&H9B64C2B0UI, &HEC63F226UI, &H756AA39C, &H26D930A,
    '&H9C0906A9UI, &HEB0E363FUI, &H72076785, &H5005713,
    '&H95BF4A82UI, &HE2B87A14UI, &H7BB12BAE, &HCB61B38,
    '&H92D28E9BUI, &HE5D5BE0DUI, &H7CDCEFB7, &HBDBDF21,
    '&H86D3D2D4UI, &HF1D4E242UI, &H68DDB3F8, &H1FDA836E,
    '&H81BE16CDUI, &HF6B9265BUI, &H6FB077E1, &H18B74777,
    '&H88085AE6UI, &HFF0F6A70UI, &H66063BCA, &H11010B5C,
    '&H8F659EFFUI, &HF862AE69UI, &H616BFFD3, &H166CCF45,
    '&HA00AE278UI, &HD70DD2EEUI, &H4E048354, &H3903B3C2,
    '&HA7672661UI, &HD06016F7UI, &H4969474D, &H3E6E77DB,
    '&HAED16A4AUI, &HD9D65ADCUI, &H40DF0B66, &H37D83BF0,
    '&HA9BCAE53UI, &HDEBB9EC5UI, &H47B2CF7F, &H30B5FFE9,
    '&HBDBDF21CUI, &HCABAC28AUI, &H53B39330, &H24B4A3A6,
    '&HBAD03605UI, &HCDD70693UI, &H54DE5729, &H23D967BF,
    '&HB3667A2EUI, &HC4614AB8UI, &H5D681B02, &H2A6F2B94,
    '&HB40BBE37UI, &HC30C8EA1UI, &H5A05DF1B, &H2D02EF8D}

    Friend Ddraw_ini() As String
    Friend F2res_ini(0) As String
    Friend Main_Path As String = Application.StartupPath

    Friend Sub DatDesc()
        Dim desc() As String
        Dim n As Integer
        On Error Resume Next
        IO.File.WriteAllBytes(Main_Path & "\dat.unp", My.Resources.dat2)
        IO.File.SetAttributes(Main_Path & "\dat.unp", IO.FileAttributes.Hidden)
        Dim ListDat() As String = IO.Directory.GetFiles(Main_Path, "*.dat", IO.SearchOption.TopDirectoryOnly)
        For Each dat In ListDat
            Dim z As Integer = dat.LastIndexOf("\") + 1
            dat = dat.Substring(z, dat.Length - z)
            'If dat.ToLower <> "master.dat" And dat <> "critter.dat" Then
            Shell(Main_Path & "\dat.unp x " & dat & " desc.id", AppWinStyle.Hide, True)
            desc = IO.File.ReadAllLines(Main_Path & "\desc.id")
            MainForm.ListView1.Items.Add(dat)
            If desc(1) = Nothing Then
                MainForm.ListView1.Items(n).SubItems.Add("<unknown>") '& desc(1)
            Else
                MainForm.ListView1.Items(n).SubItems.Add(desc(1))
            End If
            IO.File.Delete(Main_Path & "\desc.id")
            ReDim desc(0)
            n += 1
            'End If
        Next
    End Sub

    Friend Sub Save_ini()
        IO.File.WriteAllLines(Main_Path & "\ddraw.ini", Ddraw_ini, System.Text.Encoding.Default)
        If F2res_ini.Length > 1 Then IO.File.WriteAllLines(Main_Path & "\f2_res.ini", F2res_ini, System.Text.Encoding.Default)
    End Sub

    Friend Sub Set_ListBoxRes()
        Dim indx As Integer = MainForm.ListBox1.Items.IndexOf(CStr(MainForm.NumericUpDown4.Value) + " X " + CStr(MainForm.NumericUpDown3.Value))
        MainForm.ListBox1.SelectedIndex = indx
    End Sub

    Friend Sub Check_Exe()
        Dim game_exe As String = vbNullString
        If Not (IO.File.Exists(Main_Path & "\fallout2.exe")) Then game_exe = GetIni_Param("sFallConfigatorGameExe")
        If game_exe = vbNullString Then game_exe = "fallout2.exe"
        Check_CRC(game_exe)
    End Sub

    Friend Sub Check_CRC(ByVal game_exe As String)
        Dim equal As Boolean
        Dim crc() As String = Get_CRC(Main_Path & "\" & game_exe).Split("|")
        Dim crc_line() As String = MainForm.TextBox1.Text.Split(",")
        For n = 0 To 1
            For i = 0 To crc_line.Length - 1
                If crc_line(i).Trim.ToUpper = crc(n) Or crc_line(i).Trim.ToUpper = "0X" & crc(n) Then
                    equal = True
                    Exit For
                End If
            Next
            If Not (equal) Then
                If MainForm.TextBox1.Text.Length > 0 Then
                    MainForm.TextBox1.Text = "0x" & crc(n) & ", " & MainForm.TextBox1.Text  'заносим в тексбох
                Else
                    MainForm.TextBox1.Text = "0x" & crc(n)
                End If
                MainForm.StoreIniCrc() 'записать изменения в буффер
            End If
            equal = False
        Next
    End Sub

    Friend Function Get_CRC(ByVal path As String) As String
        Dim Bytes() As Byte = IO.File.ReadAllBytes(path)
        Dim LineCrc As String = Hex(CalcCRC(Bytes, &H1EDC6F41)).ToUpper
        LineCrc &= "|" & Hex(CalcCRC(Bytes, &HEDB88320UI)).ToUpper
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
        SetIni_ParamValue("sFallConfigatorGameExe", game_exe)
        'проверяем
        If GetIni_Param("sFallConfigatorGameExe") = Nothing Then
            ReDim Preserve Ddraw_ini(UBound(Ddraw_ini) + 3)
            Ddraw_ini(UBound(Ddraw_ini) - 1) = ";Setting for sFallConfigurator"
            Ddraw_ini(UBound(Ddraw_ini)) = "sFallConfigatorGameExe=" & game_exe
        End If
    End Sub

    Friend Function GetIni_Param_HRP(ByVal param As String) As String
        Dim n As Integer
        For n = 0 To UBound(F2res_ini)
            If GetIni_NameParam(F2res_ini(n)) = param Then
                Return GetIni_ParamValue(F2res_ini(n))
            End If
        Next
        Return Nothing
    End Function

    Friend Function GetIni_Param(ByVal param As String) As String
        Dim n As Integer
        For n = 0 To UBound(Ddraw_ini)
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Return GetIni_ParamValue(Ddraw_ini(n))
            End If
        Next
        Return vbNullString
    End Function

    Friend Function GetIni_NameParam(ByVal str As String) As String
        Dim m As UShort = InStr(1, str, "=", CompareMethod.Text)
        If m = 0 Then Return Nothing
        'возвращаем
        Return str.Substring(0, m - 1)
    End Function

    Friend Function GetIni_ParamValue(ByVal str As String) As String
        Dim m As UShort = InStr(1, str, "=", CompareMethod.Text)
        If m = 0 Then Return Nothing
        'возвращаем
        Return str.Substring(m, str.Length - m)
    End Function

    Friend Sub SetIni_ParamValue_HRP(ByVal param As String, ByVal value As String)
        Dim n As Integer
        For n = 0 To UBound(F2res_ini)
            If GetIni_NameParam(F2res_ini(n)) = param Then
                Dim m As UShort = InStr(1, F2res_ini(n), "=", CompareMethod.Text)
                If m = 0 Then Exit Sub
                'записываем
                F2res_ini(n) = F2res_ini(n).Substring(0, m) & value
                Exit Sub
            End If
        Next
    End Sub

    Friend Sub SetIni_ParamValue(ByVal param As String, ByVal value As String)
        Dim n As Integer
        For n = 0 To UBound(Ddraw_ini)
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Set_Value(n, value)
                Exit Sub
            End If
        Next
    End Sub

    Friend Sub Set_Value(ByVal z As Integer, ByRef value As String)
        Dim m As UShort = InStr(1, Ddraw_ini(z), "=", CompareMethod.Text)
        If m = 0 Then Exit Sub
        'записываем
        Ddraw_ini(z) = Ddraw_ini(z).Substring(0, m) & value
    End Sub

    Friend Function GetIni_Param_Line(ByVal param As String) As Integer
        Dim n As Integer
        For n = 0 To UBound(Ddraw_ini)
            If GetIni_NameParam(Ddraw_ini(n)) = param Then
                Return n
            End If
        Next
        Return -1
    End Function

    Friend Function GetIni_Section_Line(ByVal section As String) As Integer
        Dim n As Integer
        For n = 0 To UBound(Ddraw_ini)
            If Ddraw_ini(n) = section Then
                Return n
            End If
        Next
        Return -1
    End Function

    'for v3.7
    Friend Sub EnableDebug()
        Dim n As Integer = GetIni_Section_Line("[Debugging]")
        If n = -1 Then Exit Sub
        For n = n + 1 To UBound(Ddraw_ini)
            If GetIni_NameParam(Ddraw_ini(n)) = "Enable" Then Ddraw_ini(n) = "Enable=1"
        Next
    End Sub

End Module

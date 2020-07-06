<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InputKeyCodeForm
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
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

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Button1.Location = New System.Drawing.Point(61, 40)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(68, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Done"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Items.AddRange(New Object() {"1   DIK_ESCAPE", "2   DIK_1", "3   DIK_2", "4   DIK_3", "5   DIK_4", "6   DIK_5", "7   DIK_6", "8   DIK_7", "9   DIK_8", "10  DIK_9", "11  DIK_0", "12  DIK_MINUS", "13  DIK_EQUALS", "14  DIK_BACKSPACE", "15  DIK_TAB", "16  DIK_Q", "17  DIK_W", "18  DIK_E", "19  DIK_R", "20  DIK_T", "21  DIK_Y", "22  DIK_U", "23  DIK_I", "24  DIK_O", "25  DIK_P", "26  DIK_LBRACKET", "27  DIK_RBRACKET", "28  DIK_ENTER", "29  DIK_LCONTROL", "30  DIK_A", "31  DIK_S", "32  DIK_D", "33  DIK_F", "34  DIK_G", "35  DIK_H", "36  DIK_J", "37  DIK_K", "38  DIK_L", "39  DIK_SEMICOLON", "40  DIK_APOSTROPHE", "41  DIK_GRAVE", "42  DIK_LSHIFT", "43  DIK_BACKSLASH", "44  DIK_Z", "45  DIK_X", "46  DIK_C", "47  DIK_V", "48  DIK_B", "49  DIK_N", "50  DIK_M", "51  DIK_COMMA", "52  DIK_PERIOD", "53  DIK_SLASH", "54  DIK_RSHIFT", "55  DIK_MULTIPLY", "56  DIK_LALT", "57  DIK_SPACE", "58  DIK_CAPITAL", "59  DIK_F1", "60  DIK_F2", "61  DIK_F3", "62  DIK_F4", "63  DIK_F5", "64  DIK_F6", "65  DIK_F7", "66  DIK_F8", "67  DIK_F9", "68  DIK_F10", "69  DIK_NUMLOCK", "70  DIK_SCROLLLOCK", "71  DIK_NUMPAD7", "72  DIK_NUMPAD8", "73  DIK_NUMPAD9", "74  DIK_SUBTRACT", "75  DIK_NUMPAD4", "76  DIK_NUMPAD5", "77  DIK_NUMPAD6", "78  DIK_ADD", "79  DIK_NUMPAD1", "80  DIK_NUMPAD2", "81  DIK_NUMPAD3", "82  DIK_NUMPAD0", "83  DIK_DECIMAL", "87  DIK_F11", "88  DIK_F12", "141 DIK_NUMPADEQUALS", "145 DIK_AT", "146 DIK_COLON", "147 DIK_UNDERLINE", "149 DIK_STOP", "150 DIK_AX", "151 DIK_UNLABELED", "156 DIK_NUMPADENTER", "157 DIK_RCONTROL", "179 DIK_NUMPADCOMMA", "181 DIK_DIVIDE", "183 DIK_SYSRQ", "184 DIK_RALT", "199 DIK_HOME", "200 DIK_UP", "201 DIK_PRIOR", "203 DIK_LEFT", "205 DIK_RIGHT", "207 DIK_END", "208 DIK_DOWN", "209 DIK_NEXT", "210 DIK_INSERT", "211 DIK_DELETE", "219 DIK_LWIN", "220 DIK_RWIN", "221 DIK_APPS"})
        Me.ComboBox1.Location = New System.Drawing.Point(12, 12)
        Me.ComboBox1.MaxDropDownItems = 10
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(165, 22)
        Me.ComboBox1.TabIndex = 2
        '
        'InputKeyCodeForm
        '
        Me.AcceptButton = Me.Button1
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(188, 71)
        Me.ControlBox = False
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.Button1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InputKeyCodeForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
End Class

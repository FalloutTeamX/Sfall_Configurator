Public Class InputKeyCodeForm

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub InputKeyCodeForm_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        If e.KeyData = Keys.Escape Then Me.Close()
    End Sub

    Private Sub ComboBox1_DropDownClosed(sender As Object, e As EventArgs) Handles ComboBox1.DropDownClosed
        Button1.Focus()
    End Sub
End Class
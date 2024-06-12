
Partial Class AsynCallBack
    Inherits System.Web.UI.Page

    Private Sub AsynCallBack_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim sMensaje As String = ""
            For Each key As String In Request.Form.AllKeys
                sMensaje = sMensaje & Request.Form(key)
            Next
            Request.InputStream.Position = 0
            Dim jsonString As String = New System.IO.StreamReader(Request.InputStream).ReadToEnd()

            TextBox1.Text = sMensaje
            Label1.Text = "Mensaje: " & sMensaje & "->" & jsonString
        End If
    End Sub
End Class

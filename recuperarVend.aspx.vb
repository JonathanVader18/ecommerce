
Imports System.Data
Imports System.Security.Cryptography

Partial Class recuperarVend
    Inherits System.Web.UI.Page
    Public objdatos As New Cls_Funciones
    Public ssql As String = ""
    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        If txtCorreo.Text = "" Then
            objdatos.Mensaje("Ingrese la dirección de correo con la que registró su cuenta", Me.Page)
            Exit Sub
        End If

        ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,cvMail from config.Usuarios where cvMail=" & "'" & txtCorreo.Text & "'"
        Dim dtAcceso As New DataTable
        dtAcceso = objdatos.fnEjecutarConsulta(ssql)
        If dtAcceso.Rows.Count = 0 Then
            objdatos.Mensaje("Cuenta de correo no registrada", Me.Page)
            Exit Sub
        End If

        Dim sLiga As String = ""

        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If

        sLiga = sLigaSitio & "/cambiar-contraseñaVend.aspx?usr=" & Encriptar(txtCorreo.Text)

        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objdatos.fnEjecutarConsulta(ssql)


        Dim Mensaje As String = ""
        If dtcliente.Rows.Count > 0 Then

            Mensaje = "Hola! " & dtAcceso.Rows(0)(0) & vbCrLf & dtcliente.Rows(0)(0) & " te saluda y te proporciona el siguiente enlace dónde podrás reestablecer tu contraseña:" & vbCrLf _
                    & sLiga


            objdatos.fnEnviarCorreo(txtCorreo.Text, Mensaje, dtcliente.Rows(0)(0) & "- Recupera tu contraseña")
            objdatos.Mensaje("Se ha enviado un correo a la cuenta especifícada, con un enlace para que pueda reestablecer su contraseña", Me.Page)
        End If

        ' Response.Redirect("aviso.aspx")
    End Sub



    Function ENCRIPTAR(ByVal string_encriptar As String) As String
        Dim R As Integer
        Dim I As Integer
        R = Len(Trim(string_encriptar))
        For I = 1 To R
            Mid(string_encriptar, I, 1) = Chr(Asc(Mid(string_encriptar, I, 1)) - 1)
        Next I
        Return string_encriptar
    End Function

    Public Function Desencriptar(ByVal Input As String) As String

        Dim IV() As Byte = ASCIIEncoding.ASCII.GetBytes("qualityi") 'La clave debe ser de 8 caracteres
        Dim EncryptionKey() As Byte = Convert.FromBase64String("rpaSPvIvVLlrcmtzPU9/c67Gkj7yL1S5") 'No se puede alterar la cantidad de caracteres pero si la clave
        Dim buffer() As Byte = Convert.FromBase64String(Input)
        Dim des As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider
        des.Key = EncryptionKey
        des.IV = IV
        Return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))

    End Function


End Class

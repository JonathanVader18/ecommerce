Imports System.Data
Partial Class informacion
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub informacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString.Count > 0 Then
                Dim opc As String = ""
                opc = Request.QueryString(0)
                fnCargaPagina(opc)
            End If
        End If
    End Sub

    Public Sub fnCargaPagina(opc As String)
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""

        ssql = "SELECT cvHTML from config.paginas where cvParametro=" & "'" & opc & "'"
        Dim dtHTML As New DataTable
        dtHTML = objDatos.fnEjecutarConsulta(ssql)
        If dtHTML.Rows.Count > 0 Then
            sHTMLEncabezado = dtHTML.Rows(0)(0)
        Else
            objDatos.fnLog("Pagina info", "Sin datos opc:" & opc)
        End If


        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlContenido.Controls.Clear()
        pnlContenido.Controls.Add(literal)
    End Sub
End Class

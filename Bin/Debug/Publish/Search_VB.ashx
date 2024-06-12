<%@ WebHandler Language="VB" Class="Search_VB" %>

Imports System
Imports System.Web
Imports System.Data

Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Text

Public Class Search_VB : Implements IHttpHandler

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim prefixText As String = context.Request.QueryString("q")
        Dim ssql As String
        Dim objDatos As New Cls_Funciones


        Dim conn As SqlConnection = New SqlConnection
        conn = New Data.SqlClient.SqlConnection

        ''Cargamos la configuracion a SAP
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            conn.ConnectionString = "Data Source=" & dtConfSAP.Rows(0)("cvServidorSQL") & ";Initial Catalog=" & dtConfSAP.Rows(0)("cvBD") & ";User ID=" & dtConfSAP.Rows(0)("cvUserSQL") & ";Password=" & dtConfSAP.Rows(0)("cvPwdSQL")
        End If

        ssql = objDatos.fnObtenerQuery("QueryBusqueda")
        Dim cmd As SqlCommand = New SqlCommand
        cmd.CommandText = (ssql)
        'cmd.CommandText = ("select ItemName as Descripcion from OITM where " & _
        '              "ItemName like @SearchText + '%'")
        '    cmd.CommandText = "SELECT ItemCode,ItemName,(ItemCode +  '-' + ISNULL(ItemName,'') as Descripcion FROM OITM WHERE ValidFor='Y' AND (ItemCode like @SearchText + '%' OR ItemName like @SearchText + '%')"
        cmd.Parameters.AddWithValue("@SearchText", prefixText)
        cmd.Connection = conn
        Dim sb As StringBuilder = New StringBuilder
        conn.Open()
        Dim sdr As SqlDataReader = cmd.ExecuteReader
        While sdr.Read
            sb.Append(sdr("Descripcion")) _
                .Append(Environment.NewLine)
        End While
        conn.Close()
        context.Response.Write(sb.ToString)
    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class
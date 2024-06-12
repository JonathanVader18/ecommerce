
Imports System.Drawing.Printing
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class VistaPrevia
    Inherits System.Web.UI.Page

    Private Sub VistaPrevia_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables


        reporte.Load(Server.MapPath("~") & "\Pedido.rpt")
        reporte.SetParameterValue("@DocKey", Session("DocEntry"))
        reporte.SetDatabaseLogon("sa", "o2#/P\uW", "ZEYCOSERVER3", "PruebaADZ")

        crConnectionInfo.ServerName = "ZEYCOSERVER3"
        crConnectionInfo.DatabaseName = "PruebaADZ"
        crConnectionInfo.UserID = "sa"
        crConnectionInfo.Password = "o2#/P\uW"
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
        reporte.Refresh()
        reporte.SetParameterValue("@DocKey", Session("DocEntry"))
        '    crpReporte.ReportSource = reporte
        Dim settings As New PrinterSettings
        '  Dim pdialog = New PrintDialog()

    End Sub
End Class

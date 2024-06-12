
Imports System.Data
Imports System.IO
Imports Telerik.Web.UI

Partial Class configDescuentos
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub configDescuentos_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("loginAdmin") = "" Then
                Response.Redirect("loginadmin.aspx")
            End If
            Session("IdArticuloDesc") = ""

            fnCargaDescuentos()

            If Request.QueryString.Count > 0 Then
                Session("IdArticuloDesc") = Request.QueryString("Id")
                ''Obtenemos los datos para rellenar
                ssql = "Select cvItemCode as Articulo, cfDescto as Descuento,CONVERT(varchar(10),cdfechaini,120) as FechaIni,CONVERT(varchar(10),cdfechaFin,120) as FechaFin from config.DesctosArticulo  WHERE cvItemCode= " & "'" & Request.QueryString("Id") & "'"
                Dim dtBanner As New DataTable
                dtBanner = objDatos.fnEjecutarConsulta(ssql)

                If dtBanner.Rows.Count > 0 Then
                    txtArticulo.Text = dtBanner.Rows(0)("Articulo")
                    txtDescuento.Text = CDbl(dtBanner.Rows(0)("Descuento"))
                    txtFechaIni.Text = dtBanner.Rows(0)("FechaIni")
                    txtFechaFin.Text = dtBanner.Rows(0)("FechaFin")


                    btnAgregar.Text = "Actualizar"
                End If
            End If

        End If
    End Sub

    Public Sub fnCargaDescuentos()
        ''Los banners activos
        ssql = "Select cvItemCode as Articulo, cfDescto as Descuento,CONVERT(varchar(10),cdfechaini,120) as FechaIni,CONVERT(varchar(10),cdfechaFin,120) as FechaFin from config.DesctosArticulo Order by cvItemCode "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtBanners.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtBanners.Rows(i)("Articulo") Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If

                Else
                    sClass = ""

                End If

            Catch ex As Exception

            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configDescuentos.aspx?Id=" & dtBanners.Rows(i)("Articulo") & "'>" & dtBanners.Rows(i)("Articulo") & "</a></td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Descuento") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("FechaIni") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("FechaFin") & "</td>"

            sHTML = sHTML & "</tr>"
        Next
        Dim literal As New LiteralControl(sHTML)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If txtArticulo.Text = "" Then
            objDatos.Mensaje("Indique el código de artículo", Me.Page)
            Exit Sub
        End If

        If txtDescuento.Text = "" Then
            objDatos.Mensaje("Indique el descuento del artículo", Me.Page)
            Exit Sub
        End If

        If txtFechaIni.Text = "" Then
            objDatos.Mensaje("Indique la fecha inicial del descuento del artículo", Me.Page)
            Exit Sub
        End If
        If txtFechaFin.Text = "" Then
            objDatos.Mensaje("Indique la fecha final del descuento del artículo", Me.Page)
            Exit Sub
        End If

        If btnAgregar.Text = "Actualizar" Then
            ''Eliminamos y dejamos el flujo natural de una inserción
            ssql = "DELETE FROM config.DesctosArticulo  where cvItemCode=" & "'" & Session("IdArticuloDesc") & "'"
            objDatos.fnEjecutarInsert(ssql)
        End If
        ssql = "INSERT INTO config.DesctosArticulo  (cvItemCode,cfDescto,cdFechaini,cdFechaFin) VALUES(" _
             & "'" & txtArticulo.Text & "'," _
             & "'" & txtDescuento.Text & "'," _
             & "'" & txtFechaIni.Text & "'," _
             & "'" & txtFechaFin.Text & "')"

        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Descuento Guardado!", Me.Page)
        txtArticulo.Text = ""
        txtDescuento.Text = ""

        Session("IdArticuloDesc") = ""
        fnCargaDescuentos()

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM config.DesctosArticulo  where cvItemCode=" & "'" & Session("IdArticuloDesc") & "'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Descuento Eliminado!", Me.Page)
        txtArticulo.Text = ""
        txtDescuento.Text = ""

        Session("IdArticuloDesc") = ""
        fnCargaDescuentos()
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("menuconfig.aspx")
    End Sub
End Class

Imports System.Data
Imports System.Web.Services

Partial Class elegir_favoritos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub elegir_favoritos_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Cargamos las plantillas que tiene el usuario
        HttpContext.Current.Session("ItemActual") = Request.QueryString("code")
        If CInt(HttpContext.Current.Session("slpCode")) = 0 Then
            '''Lo cargamos al carrito de favoritos
            'Dim oDeseo As New Cls_Pedido.Partidas
            'oDeseo.ItemCode = Request.QueryString("code")
            'oDeseo.ItemName = Request.QueryString("name")
            'Dim dPrecioActual As Double = 0
            'If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            '    dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), HttpContext.Current.Session("ListaPrecios"))
            'Else
            '    dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
            'End If
            'oDeseo.Cantidad = 1
            'oDeseo.Precio = dPrecioActual
            'Session("WishList").add(oDeseo)


            Dim sNombre As String = ""
            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Request.QueryString("code") & "'")
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count > 0 Then
                sNombre = dtItemName.Rows(0)(0)
            End If
            If (Session("RazonSocial") = "") And CInt(Session("slpCode")) = "0" Then
                lblTitulo.Text = "Se agregará el artículo " & sNombre & " a tu lista de favoritos"
                ddlFavoritos.Visible = False
                btnSeleccionar.Visible = False
                btnConfirmar.Visible = False
                pnlAgregar.Visible = True
                pnlAgregarLista.Visible = False

            Else
                ''Es B2B
                btnConfirmar.Visible = False

                ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE cvUsuario=" & "'" & Session("UserTienda") & "' "
                Dim dtPlantillas As New DataTable
                dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
                If dtPlantillas.Rows.Count > 0 Then
                    ddlFavoritos.DataSource = dtPlantillas
                    ddlFavoritos.DataTextField = "Plantilla"
                    ddlFavoritos.DataValueField = "No"
                    ddlFavoritos.DataBind()
                    btnSeleccionar.Visible = False
                    btnConfirmar.Visible = False
                    pnlAgregar.Visible = False
                    pnlAgregarLista.Visible = True

                Else
                    pnlAgregar.Visible = False
                    btnConfirmar.Visible = False
                    btnSeleccionar.Visible = False
                    ddlFavoritos.Visible = False
                    lblTitulo.Text = "No tiene creada ninguna lista aún para agregar este producto"
                End If
            End If


            'Exit Sub
        Else
            btnConfirmar.Visible = False
            ''B2b o vendedores
            ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' "
            Dim dtPlantillas As New DataTable
            dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
            If dtPlantillas.Rows.Count > 0 Then
                ddlFavoritos.DataSource = dtPlantillas
                ddlFavoritos.DataTextField = "Plantilla"
                ddlFavoritos.DataValueField = "No"
                ddlFavoritos.DataBind()
                btnSeleccionar.Visible = False
                btnConfirmar.Visible = False
                pnlAgregar.Visible = False
                pnlAgregarLista.Visible = True
            Else
                pnlAgregar.Visible = False
                btnConfirmar.Visible = False
                btnSeleccionar.Visible = False
                ddlFavoritos.Visible = False
                lblTitulo.Text = "No tiene creada ninguna lista aún para agregar este producto"
            End If

        End If


    End Sub

    <WebMethod>
    Public Shared Function CargarFavorito(Cantidad As String, Articulo As String) As String
        Articulo = HttpContext.Current.Session("ItemActual")
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        Dim dPrecioActual As Double = 0


        partida.ItemCode = Articulo
        partida.Cantidad = 1
        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
        Else
            dPrecioActual = objDatos.fnPrecioActual(Articulo)
        End If

        ' partida.Descuento = desc
        partida.Precio = dPrecioActual
        partida.TotalLinea = partida.Cantidad * partida.Precio

        ''Ahora el itemName
        Try
            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count > 0 Then
                partida.ItemName = dtItemName.Rows(0)(0)
            End If

        Catch ex As Exception

        End Try


        HttpContext.Current.Session("WishList").add(partida)





        Dim result As String = "Entró:" & Articulo

        Return result
    End Function



    <WebMethod>
    Public Shared Function CargarALista(Lista As String, Articulo As String) As String
        Articulo = HttpContext.Current.Session("ItemActual")
        Dim sDescripcion As String = ""
        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones
        ''Ahora el itemName
        Try
            ssql = objdatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtItemName As New DataTable
            dtItemName = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count > 0 Then
                sDescripcion = dtItemName.Rows(0)(0)
            End If

        Catch ex As Exception

        End Try



        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Plantilla_det"
        Dim dtIdLineas As New DataTable

        dtIdLineas = objDatos.fnEjecutarConsulta(ssql)

        Dim dPrecioActual As Double = 0
        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            dPrecioActual = objdatos.fnPrecioActual(HttpContext.Current.Session("ItemActual"), HttpContext.Current.Session("ListaPrecios"))
        Else
            dPrecioActual = objdatos.fnPrecioActual(HttpContext.Current.Session("ItemActual"))
        End If

        ssql = "INSERT INTO Tienda.Plantilla_det (ciIdRelacion,ciIdPlantilla,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
          & "'" & dtIdLineas.Rows(0)(0) & "'," _
          & "'" & Lista & "'," _
          & "'" & HttpContext.Current.Session("UserTienda") & "'," _
          & "'" & Articulo & "'," _
          & "'" & sDescripcion & "'," _
          & "'1'," _
          & "'" & dPrecioActual & "'," _
          & "'0')"
        objdatos.fnEjecutarInsert(ssql)
        '  objDatos.Mensaje("Agregado", Me.Page)





        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Protected Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click

        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Plantilla_det"
        Dim dtIdLineas As New DataTable
        dtIdLineas = objDatos.fnEjecutarConsulta(ssql)

        Dim dPrecioActual As Double = 0
        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), HttpContext.Current.Session("ListaPrecios"))
        Else
            dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
        End If

        ssql = "INSERT INTO Tienda.Plantilla_det (ciIdRelacion,ciIdPlantilla,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
          & "'" & dtIdLineas.Rows(0)(0) & "'," _
          & "'" & ddlFavoritos.SelectedValue & "'," _
          & "'" & Session("UserTienda") & "'," _
          & "'" & Request.QueryString("code") & "'," _
          & "'" & Request.QueryString("name") & "'," _
          & "'1'," _
          & "'" & dPrecioActual & "'," _
          & "'0')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Agregado", Me.Page)
    End Sub
    Protected Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Page.ClientScript.RegisterOnSubmitStatement(GetType(Page), "closePage", "window.onunload = CloseWindow();")
    End Sub
End Class

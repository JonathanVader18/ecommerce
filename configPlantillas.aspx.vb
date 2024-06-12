Imports System.Data
Imports Telerik.Web.UI
Partial Class configPlantillas
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ''Cargamos los lenguajes
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            rcbLenguaje.DataSource = dtLenguajes
            rcbLenguaje.DataTextField = "cvLenguaje"
            rcbLenguaje.DataValueField = "ciIdLenguaje"
            rcbLenguaje.DataBind()

            ''Los atributos
            ssql = "select T0.ciIdAtributo as Id,T1.cvTipoAtributo as Tipo, T0.cvDescripcion as Atributo,T0.cvCampoSAP as CampoSAP,T0.cvLenguaje as Lenguaje, T0.ciOrden as Orden from config.atributos T0 " _
                & " INNER JOIN config.TipoAtributos T1 ON T0.ciIdTipo=T1.ciIdTipo WHERE T0.cvEstatus='ACTIVO' AND T0.cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' Order by T1.CiOrden,T0.ciOrden "
            Dim dtAtributos As New DataTable
            dtAtributos = objDatos.fnEjecutarConsulta(ssql)
            rcbAtributo.DataSource = dtAtributos
            rcbAtributo.DataTextField = "Atributo"
            rcbAtributo.DataValueField = "Id"
            rcbAtributo.DataBind()

            ssql = "SELECT T0.ciIdPlantilla as Id,T1.ciIdDetalle as IdHijo,T0.cvDescripcion as Plantilla,T2.cvDescripcion as Atributo " _
           & " ,T0.cvLenguaje as Lenguaje from config.plantillas T0 " _
           & " INNER JOIN config.plantillas_detalle T1 ON T0.ciIdPlantilla = T1.CiIdPLantilla " _
           & " INNER JOIN config.atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
           & " WHERE T0.cvEstatus ='ACTIVO' "
            Dim dtPlantilla As New DataTable
            dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
            rgvPlantilla.DataSource = dtPlantilla
            rgvPlantilla.DataBind()


            ''Combo plantillas
            ssql = "Select ciIdPlantilla,cvDescripcion FROM config.Plantillas WHERE cvEstatus='ACTIVO'"
            Dim dtPlantillas As New DataTable
            dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
            rcbPlantilla.DataSource = dtPlantillas
            rcbPlantilla.DataTextField = "cvDescripcion"
            rcbPlantilla.DataValueField = "ciIdPlantilla"
            rcbPlantilla.DataBind()

        End If
    End Sub
    Protected Sub rcbLenguaje_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles rcbLenguaje.SelectedIndexChanged
        Try
            ''Los atributos
            ssql = "select T0.ciIdAtributo as Id,T1.cvTipoAtributo as Tipo, T0.cvDescripcion as Atributo,T0.cvCampoSAP as CampoSAP,T0.cvLenguaje as Lenguaje, T0.ciOrden as Orden from config.atributos T0 " _
                & " INNER JOIN config.TipoAtributos T1 ON T0.ciIdTipo=T1.ciIdTipo WHERE T0.cvEstatus='ACTIVO' AND T0.cvLenguaje=" & "'" & rcbLenguaje.SelectedValue & "' Order by T1.CiOrden,T0.ciOrden "
            Dim dtAtributos As New DataTable
            dtAtributos = objDatos.fnEjecutarConsulta(ssql)
            rcbAtributo.DataSource = dtAtributos
            rcbAtributo.DataTextField = "Atributo"
            rcbAtributo.DataValueField = "Id"
            rcbAtributo.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click


        If btnAgregar.Text = "Actualizar" Then
            ssql = "DELETE FROM config.plantillas_detalle WHERE ciIdDetalle=" & "'" & Session("IdDetallePlantilla") & "'"
            objDatos.fnEjecutarInsert(ssql)
            btnAgregar.Text = "Agregar"
        End If

        ''Insertamos el HDR
        Dim dtId As New DataTable
        'If lblIdPlantilla.Text = "" Then
        '    ssql = "SELECT ISNULL(MAX(ciIdPlantilla),0) + 1 fROM config.plantillas "

        '    dtId = objDatos.fnEjecutarConsulta(ssql)

        '    ssql = "INSERT INTO config.plantillas (ciIdPlantilla,cvDescripcion,cvLenguaje,cvEstatus) VALUES(" _
        '         & "'" & dtId.Rows(0)(0) & "'," _
        '         & "'" & txtNombre.Text & "'," _
        '         & "'" & rcbLenguaje.SelectedItem.Text & "','ACTIVO')"
        '    objDatos.fnEjecutarInsert(ssql)
        '    lblIdPlantilla.Text = dtId.Rows(0)(0)

        '    ''La Linea
        '    ssql = "SELECT ISNULL(MAX(ciIdDetalle),0) + 1 fROM config.plantillas_detalle "
        '    dtId = New DataTable
        '    dtId = objDatos.fnEjecutarConsulta(ssql)
        '    ssql = "INSERT INTO config.plantillas_detalle (ciIdDetalle,ciIdPlantilla,ciIdAtributo)VALUES(" _
        '        & "'" & dtId.Rows(0)(0) & "'," _
        '        & "'" & lblIdPlantilla.Text & "'," _
        '        & "'" & rcbAtributo.SelectedValue & "')"
        '    objDatos.fnEjecutarInsert(ssql)

        'Else
        '    ''Sólo la Linea
        '    ssql = "SELECT ISNULL(MAX(ciIdDetalle),0) + 1 fROM config.plantillas_detalle "
        '    dtId = New DataTable
        '    dtId = objDatos.fnEjecutarConsulta(ssql)
        '    ssql = "INSERT INTO config.plantillas_detalle (ciIdDetalle,ciIdPlantilla,ciIdAtributo)VALUES(" _
        '        & "'" & dtId.Rows(0)(0) & "'," _
        '        & "'" & rcbPlantilla.SelectedValue & "'," _
        '        & "'" & rcbAtributo.SelectedValue & "')"
        '    objDatos.fnEjecutarInsert(ssql)

        'End If

        'Sólo la Linea
        ssql = "SELECT ISNULL(MAX(ciIdDetalle),0) + 1 fROM config.plantillas_detalle "
        dtId = New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)
        ssql = "INSERT INTO config.plantillas_detalle (ciIdDetalle,ciIdPlantilla,ciIdAtributo)VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & rcbPlantilla.SelectedValue & "'," _
                & "'" & rcbAtributo.SelectedValue & "')"
        objDatos.fnEjecutarInsert(ssql)



        ssql = "SELECT T0.ciIdPlantilla as Id,T1.ciIdDetalle as IdHijo,T0.cvDescripcion as Plantilla,T2.cvDescripcion as Atributo " _
            & " ,T0.cvLenguaje as Lenguaje from config.plantillas T0 " _
            & " INNER JOIN config.plantillas_detalle T1 ON T0.ciIdPlantilla = T1.CiIdPLantilla " _
            & " INNER JOIN config.atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
            & " WHERE T0.cvEstatus ='ACTIVO' AND T0.ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
        Dim dtPlantilla As New DataTable
        dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
        rgvPlantilla.DataSource = dtPlantilla
        rgvPlantilla.DataBind()
        btnAgregar.Text = "Agregar"
        objDatos.Mensaje("Registrado", Me.Page)

    End Sub
    Protected Sub rgvPlantilla_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgvPlantilla.ItemCommand
        Try
            Dim Item As GridEditableItem
            Item = e.Item
            Session("IdDetallePlantilla") = rgvPlantilla.Items(Item.ItemIndex)("IdHijo").Text
            ssql = "SELECT  ciIdAtributo,ciIdPlantilla FROM config.Plantillas_detalle WHERE ciIdDetalle=" & "'" & Session("IdDetallePlantilla") & "'"
            Dim dtAtributo As New DataTable
            dtAtributo = objDatos.fnEjecutarConsulta(ssql)
            rcbAtributo.SelectedValue = dtAtributo.Rows(0)(0)

            ssql = "SELECT * from config.plantillas Where ciIdPlantilla=" & "'" & dtAtributo.Rows(0)("ciIdPlantilla") & "'"
            Dim dtPlantilla As New DataTable
            dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
            txtNombre.Text = dtPlantilla.Rows(0)("cvDescripcion")

            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & rgvPlantilla.Items(Item.ItemIndex)("Lenguaje").Text & "'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            If dtLenguajes.Rows.Count > 0 Then
                rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
            End If
            btnAgregar.Text = "Actualizar"
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM config.plantillas_detalle WHERE ciIdDetalle=" & "'" & Session("IdDetallePlantilla") & "'"
        objDatos.fnEjecutarInsert(ssql)
        ssql = "SELECT T0.ciIdPlantilla as Id,T1.ciIdDetalle as IdHijo,T0.cvDescripcion as Plantilla,T2.cvDescripcion as Atributo " _
            & " ,T0.cvLenguaje as Lenguaje from config.plantillas T0 " _
            & " INNER JOIN config.plantillas_detalle T1 ON T0.ciIdPlantilla = T1.CiIdPLantilla " _
            & " INNER JOIN config.atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
            & " WHERE T0.cvEstatus ='ACTIVO' AND T0.ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
        Dim dtPlantilla As New DataTable
        dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
        rgvPlantilla.DataSource = dtPlantilla
        rgvPlantilla.DataBind()
        btnAgregar.Text = "Agregar"
        objDatos.Mensaje("Eliminado", Me.Page)

    End Sub
    Protected Sub btnAgregar0_Click(sender As Object, e As EventArgs) Handles btnAgregarP.Click
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique a la plantilla", Me.Page)
            Exit Sub
        End If
        If btnAgregarP.Text = "Actualizar Plantilla" Then


            ssql = "UPDATE config.plantillas SET " _
                 & "cvDescripcion='" & txtNombre.Text & "'," _
                 & "cvLenguaje='" & rcbLenguaje.SelectedItem.Text & "' " _
                 & " WHERE ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
            objDatos.fnEjecutarInsert(ssql)
            btnAgregarP.Text = "Agregar Plantilla"
        Else
            ssql = "SELECT ISNULL(MAX(ciIdPlantilla),0) + 1 fROM config.plantillas "
            Dim dtId As New DataTable
            dtId = objDatos.fnEjecutarConsulta(ssql)

            ssql = "INSERT INTO config.plantillas (ciIdPlantilla,cvDescripcion,cvLenguaje,cvEstatus) VALUES(" _
                 & "'" & dtId.Rows(0)(0) & "'," _
                 & "'" & txtNombre.Text & "'," _
                 & "'" & rcbLenguaje.SelectedItem.Text & "','ACTIVO')"
            objDatos.fnEjecutarInsert(ssql)
            lblIdPlantilla.Text = dtId.Rows(0)(0)
        End If

        objDatos.Mensaje("Registrado", Me.Page)
        ssql = "Select ciIdPlantilla,cvDescripcion FROM config.Plantillas WHERE cvEstatus='ACTIVO'"
        Dim dtPlantillas As New DataTable
        dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
        rcbPlantilla.DataSource = dtPlantillas
        rcbPlantilla.DataTextField = "cvDescripcion"
        rcbPlantilla.DataValueField = "ciIdPlantilla"
        rcbPlantilla.DataBind()
        txtNombre.Text = ""
    End Sub
    Protected Sub btnEliminar0_Click(sender As Object, e As EventArgs) Handles btnEliminar0.Click
        ssql = "DELETE FROM config.plantillas_detalle WHERE ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        ssql = "DELETE FROM config.plantillas WHERE ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        objDatos.Mensaje("Eliminado", Me.Page)

        ''Combo plantillas
        ssql = "Select ciIdPlantilla,cvDescripcion FROM config.Plantillas WHERE cvEstatus='ACTIVO'"
        Dim dtPlantillas As New DataTable
        dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
        rcbPlantilla.DataSource = dtPlantillas
        rcbPlantilla.DataTextField = "cvDescripcion"
        rcbPlantilla.DataValueField = "ciIdPlantilla"
        rcbPlantilla.DataBind()


        ssql = "SELECT T0.ciIdPlantilla as Id,T1.ciIdDetalle as IdHijo,T0.cvDescripcion as Plantilla,T2.cvDescripcion as Atributo " _
           & " ,T0.cvLenguaje as Lenguaje from config.plantillas T0 " _
           & " INNER JOIN config.plantillas_detalle T1 ON T0.ciIdPlantilla = T1.CiIdPLantilla " _
           & " INNER JOIN config.atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
           & " WHERE T0.cvEstatus ='ACTIVO' AND T0.ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
        Dim dtPlantilla As New DataTable
        dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
        rgvPlantilla.DataSource = dtPlantilla
        rgvPlantilla.DataBind()

    End Sub
    Protected Sub rcbPlantilla_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles rcbPlantilla.SelectedIndexChanged
        Try
            ssql = "SELECT T0.ciIdPlantilla as Id,T1.ciIdDetalle as IdHijo,T0.cvDescripcion as Plantilla,T2.cvDescripcion as Atributo " _
           & " ,T0.cvLenguaje as Lenguaje from config.plantillas T0 " _
           & " INNER JOIN config.plantillas_detalle T1 ON T0.ciIdPlantilla = T1.CiIdPLantilla " _
           & " INNER JOIN config.atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
           & " WHERE T0.cvEstatus ='ACTIVO' AND T0.ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
            Dim dtPlantilla As New DataTable
            dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
            rgvPlantilla.DataSource = dtPlantilla
            rgvPlantilla.DataBind()
            btnAgregar.Text = "Agregar"


            ssql = "SELECT * from config.plantillas Where ciIdPlantilla=" & "'" & rcbPlantilla.SelectedValue & "'"
            dtPlantilla = New DataTable
            dtPlantilla = objDatos.fnEjecutarConsulta(ssql)
            txtNombre.Text = dtPlantilla.Rows(0)("cvDescripcion")

            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & dtPlantilla.Rows(0)("cvLenguaje") & "'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            If dtLenguajes.Rows.Count > 0 Then
                rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
            End If
            btnAgregarP.Text = "Actualizar Plantilla"
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAgregarP0_Click(sender As Object, e As EventArgs) Handles btnAgregarP0.Click
        btnAgregarP.Text = "Agregar Plantilla"
        txtNombre.Text = ""
    End Sub
End Class

Imports System.Data
Partial Class direccionesla
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub direccionesla_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()
        If Not IsPostBack Then

            ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,cvPredeterminado,ISNULL(cvCP,'') as cvCP  FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ISNULL(cvPredeterminado,'NO') ='SI' "
            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsulta(ssql)
            If dtDirecciones.Rows.Count > 0 Then
                ''Nos traemos la predeterminada
                'Else
                '    ''Traemos la ultima que utilizó para una compra
                '    ssql = "SELECT TOP 1 [ciNoPedido],[cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais] FROM Tienda.Pedido_Envio WHERE cvUsuario='" & Session("UserB2C") & "' order by ciNoPedido desc"
                '    dtDirecciones = New DataTable
                '    dtDirecciones = objDatos.fnEjecutarConsulta(ssql)
            End If

            Dim sHtml As String = ""
            Dim sHTMLEncabezado As String = ""
            For i = 0 To dtDirecciones.Rows.Count - 1 Step 1
                sHTMLEncabezado = "<div class='singular-content col-xs-12 col-sm-12'>"
                sHtml = sHtml & "<div><p>" & Session("NombreUserB2C") & "<br>"
                sHtml = sHtml & dtDirecciones.Rows(i)("cvCalle") & " No " & dtDirecciones.Rows(i)("cvNumExt") & " " & dtDirecciones.Rows(i)("cvNumInt") & " <br>"
                If dtDirecciones.Rows(0)("cvColonia") = "" Then
                    sHtml = sHtml & dtDirecciones.Rows(i)("cvCiudad") & ",  " & dtDirecciones.Rows(i)("cvMunicipio") & " <br>"
                Else
                    sHtml = sHtml & dtDirecciones.Rows(i)("cvColonia") & ", " & dtDirecciones.Rows(i)("cvCiudad") & " <br>"
                End If
                sHtml = sHtml & dtDirecciones.Rows(i)("cvPais") & " , " & dtDirecciones.Rows(i)("cvCP") & " </p></div> "

                sHtml = sHtml & "<div class='blk-action-btn'>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnEditar' href='editar-direccionla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnQuitar'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'> quitar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnPred'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'> selecionar como predeterminado</a>"
                sHtml = sHtml & "</div>"

                sHTMLEncabezado = sHTMLEncabezado & sHtml
                sHTMLEncabezado = sHTMLEncabezado & "</div>"

                Dim literal As New LiteralControl(sHTMLEncabezado)
                pnlDirecciones.Controls.Add(literal)
                sHTMLEncabezado = ""
                sHtml = ""

            Next




            ''Ahora todas las direcciones
            ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'  "
            dtDirecciones = New DataTable
            dtDirecciones = objDatos.fnEjecutarConsulta(ssql)

            For i = 0 To dtDirecciones.Rows.Count - 1 Step 1
                sHTMLEncabezado = "<div class='singular-content col-xs-12 col-sm-12'>"
                sHtml = sHtml & "<div><p>" & Session("NombreUserB2C") & "<br>"
                sHtml = sHtml & dtDirecciones.Rows(i)("cvCalle") & "No " & dtDirecciones.Rows(i)("cvNumExt") & " " & dtDirecciones.Rows(i)("cvNumInt") & " <br>"
                If dtDirecciones.Rows(0)("cvColonia") = "" Then
                    sHtml = sHtml & dtDirecciones.Rows(i)("cvCiudad") & ", " & dtDirecciones.Rows(i)("cvMunicipio") & " <br>"
                Else
                    sHtml = sHtml & dtDirecciones.Rows(i)("cvColonia") & ", " & dtDirecciones.Rows(i)("cvCiudad") & " <br>"
                End If
                sHtml = sHtml & dtDirecciones.Rows(i)("cvPais") & ", " & dtDirecciones.Rows(i)("cvCP") & " </p></div> "

                sHtml = sHtml & "<div class='blk-action-btn'>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnEditar' href='editar-direccionla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnQuitar'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnPred'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
                sHtml = sHtml & "</div>"


                sHTMLEncabezado = sHTMLEncabezado & sHtml
                sHTMLEncabezado = sHTMLEncabezado & "</div>"

                Dim literal As New LiteralControl(sHTMLEncabezado)
                pnlDirecciones_lista.Controls.Add(literal)
                sHTMLEncabezado = ""
                sHtml = ""

            Next


            Dim fila As DataRow

            ''Cargamos el pais
            ssql = objDatos.fnObtenerQuery("Paises")
            Dim dtPais As New DataTable
            dtPais = objDatos.fnEjecutarConsultaSAP(ssql)
            fila = dtPais.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtPais.Rows.Add(fila)
            ddlPais.DataSource = dtPais
            ddlPais.DataTextField = "Descripcion"
            ddlPais.DataValueField = "Clave"
            ddlPais.DataBind()
            ' ddlPais.SelectedValue = "0"

            Try
                ''Cargamos los estados

                ssql = objDatos.fnObtenerQuery("Estados")
                ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
                Dim dtEstado As New DataTable
                dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)

                objDatos.fnLog("pais index", "")

                fila = dtEstado.NewRow
                fila("Clave") = "0"
                fila("Descripcion") = "-Seleccione-"
                dtEstado.Rows.Add(fila)
                ddlEstados.DataSource = dtEstado
                ddlEstados.DataTextField = "Descripcion"
                ddlEstados.DataValueField = "Clave"
                ddlEstados.DataBind()
                ddlEstados.SelectedValue = "0"
            Catch ex As Exception
                objDatos.fnLog("Cargar estados", ex.Message)
            End Try

            ''Revisamos si hay una acción por realizar

            If Request.QueryString.Count > 0 Then
                objDatos.fnLog("Direcciones", "Request")
                Dim IdDireccion As String = ""
                Dim Action As String = ""

                IdDireccion = Request.QueryString("dir")
                Action = Request.QueryString("action")
                objDatos.fnLog("Direcciones", "Lee Request")
                If Action = "e" Then
                    objDatos.fnLog("Direcciones", "Action e")
                    ''Editamos la direccion, la cargamos abajo
                    ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel= " & "'" & IdDireccion & "'"
                    Dim dtDireccion As New DataTable
                    dtDireccion = objDatos.fnEjecutarConsulta(ssql)
                    objDatos.fnLog("Direcciones", dtDireccion.Rows.Count)
                    If dtDireccion.Rows.Count > 0 Then
                        txtCalle.Text = dtDireccion.Rows(0)("cvCalle")
                        txtNumExt.Text = dtDireccion.Rows(0)("cvNumExt")
                        txtMunicipio.Text = dtDireccion.Rows(0)("cvMunicipio")
                        txtCP.Text = dtDireccion.Rows(0)("cvCP")
                        ddlEstados.SelectedValue = dtDireccion.Rows(0)("cvEstado")
                        ddlPais.SelectedValue = dtDireccion.Rows(0)("cvPais")

                    End If
                End If

                If Action = "p" Then
                    ''La establecemos como predeterminada
                    ssql = "UPDATE Tienda.Direcciones_Envio  SET cvPredeterminado='NO' WHERE cvUsuario='" & Session("UserB2C") & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    ssql = "UPDATE Tienda.Direcciones_Envio  SET cvPredeterminado='SI' WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel=" & "'" & IdDireccion & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    fnCargaDirecciones()
                End If

                If Action = "q" Then
                    ''La eliminamos
                    ssql = "DELETE Tienda.Direcciones_Envio  WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel=" & "'" & IdDireccion & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    fnCargaDirecciones()
                End If

            Else



            End If
        End If
    End Sub
    Public Sub fnCargaDirecciones()
        pnlDirecciones.Controls.Clear()

        ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ISNULL(cvPredeterminado,'NO') ='SI' "
        Dim dtDirecciones As New DataTable
        dtDirecciones = objDatos.fnEjecutarConsulta(ssql)
        If dtDirecciones.Rows.Count > 0 Then
            ''Nos traemos la predeterminada
            'Else
            '    ''Traemos la ultima que utilizó para una compra
            '    ssql = "SELECT TOP 1 [ciNoPedido],[cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais] FROM Tienda.Pedido_Envio WHERE cvUsuario='" & Session("UserB2C") & "' order by ciNoPedido desc"
            '    dtDirecciones = New DataTable
            '    dtDirecciones = objDatos.fnEjecutarConsulta(ssql)
        End If

        Dim sHtml As String = ""
        Dim sHTMLEncabezado As String = ""
        For i = 0 To dtDirecciones.Rows.Count - 1 Step 1
            sHTMLEncabezado = "<div class='singular-content col-xs-12 col-sm-12'>"
            sHtml = sHtml & "<div><p>" & Session("NombreUserB2C") & "<br>"
            sHtml = sHtml & dtDirecciones.Rows(i)("cvCalle") & "No " & dtDirecciones.Rows(i)("cvNumExt") & " " & dtDirecciones.Rows(i)("cvNumInt") & " <br>"
            If dtDirecciones.Rows(0)("cvColonia") = "" Then
                sHtml = sHtml & dtDirecciones.Rows(i)("cvCiudad") & ", " & dtDirecciones.Rows(i)("cvMunicipio") & " <br>"
            Else
                sHtml = sHtml & dtDirecciones.Rows(i)("cvColonia") & ", " & dtDirecciones.Rows(i)("cvCiudad") & " <br>"
            End If
            sHtml = sHtml & dtDirecciones.Rows(i)("cvPais") & ", " & dtDirecciones.Rows(i)("cvCP") & " </p></div> "

            sHtml = sHtml & "<div class='blk-action-btn'>"
            sHtml = sHtml & "   <a class='btn-act-blok' id='btnEditar' href='editar-direccionla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
            sHtml = sHtml & "   <a class='btn-act-blok' id='btnQuitar'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
            sHtml = sHtml & "   <a class='btn-act-blok' id='btnPred'  href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
            sHtml = sHtml & "</div>"


            sHTMLEncabezado = sHTMLEncabezado & sHtml
            sHTMLEncabezado = sHTMLEncabezado & "</div>"

            Dim literal As New LiteralControl(sHTMLEncabezado)

            pnlDirecciones.Controls.Add(literal)
            sHTMLEncabezado = ""
            sHtml = ""

        Next

        pnlDirecciones_lista.Controls.Clear()

        ''Ahora todas las direcciones
        ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'  "
        dtDirecciones = New DataTable
        dtDirecciones = objDatos.fnEjecutarConsulta(ssql)

        For i = 0 To dtDirecciones.Rows.Count - 1 Step 1
            sHTMLEncabezado = "<div class='singular-content col-xs-12 col-sm-6'>"
            sHtml = sHtml & "<div><p>" & Session("NombreUserB2C") & "<br>"
            sHtml = sHtml & dtDirecciones.Rows(i)("cvCalle") & "No " & dtDirecciones.Rows(i)("cvNumExt") & " " & dtDirecciones.Rows(i)("cvNumInt") & " <br>"
            If dtDirecciones.Rows(0)("cvColonia") = "" Then
                sHtml = sHtml & dtDirecciones.Rows(i)("cvCiudad") & ", " & dtDirecciones.Rows(i)("cvMunicipio")
            Else
                sHtml = sHtml & dtDirecciones.Rows(i)("cvColonia") & ", " & dtDirecciones.Rows(i)("cvCiudad")
            End If
            sHtml = sHtml & dtDirecciones.Rows(i)("cvPais") & ", " & dtDirecciones.Rows(i)("cvCP") & " </p></div> "

            sHtml = sHtml & "<div class='blk-action-btn'>"
            sHtml = sHtml & " <a class='btn-act-blok ' href='editar-direccionla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
            sHtml = sHtml & " <a class='btn-act-blok ' href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
            sHtml = sHtml & " <a class='btn-act-blok ' href='direccionesla.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
            sHtml = sHtml & "</div>"

            sHTMLEncabezado = sHTMLEncabezado & sHtml
            sHTMLEncabezado = sHTMLEncabezado & "</div>"

            Dim literal As New LiteralControl(sHTMLEncabezado)
            pnlDirecciones_lista.Controls.Add(literal)
            sHTMLEncabezado = ""
            sHtml = ""

        Next


    End Sub
    Public Sub fnMenuPreferencias()
        Dim ssql As String
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Preferencias' "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""

        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuPref.Controls.Clear()
        pnlMenuPref.Controls.Add(literal)

    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Request.QueryString.Count = 0 Then
            ''Insertamos
            ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
                & "'" & Session("UserB2C") & "'," _
                & "'" & txtCalle.Text & "'," _
                & "''," _
                & "'" & txtNumExt.Text & "'," _
                & "''," _
                & "''," _
                & "'" & txtMunicipio.Text & "'," _
                & "'" & ddlEstados.SelectedValue & "'," _
                & "'" & ddlPais.SelectedValue & "','N'," _
                & "'" & txtCP.Text & "')"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.Mensaje("Dirección registrada", Me.Page)

        Else
            ''Editamos
            Dim IdDireccion As String = ""
            Dim Action As String = ""

            IdDireccion = Request.QueryString("dir")
            Action = Request.QueryString("action")

            If Action = "p" Then
                ssql = "UPDATE Tienda.Direcciones_Envio SET cvPredeterminado='S' where ciIdRel=" & "'" & IdDireccion & "'"
                objDatos.fnEjecutarInsert(ssql)

            End If
            If Action = "q" Then
                ssql = "DELETE Tienda.Direcciones_Envio where ciIdRel=" & "'" & IdDireccion & "'"
                objDatos.fnEjecutarInsert(ssql)

            End If

            If Action = "e" Then
                ssql = "UPDATE Tienda.Direcciones_Envio SET " _
                    & " cvCalle=" & "'" & txtCalle.Text & "'," _
                    & " cvNumExt=" & "'" & txtNumExt.Text & "'," _
                    & " cvMunicipio=" & "'" & txtMunicipio.Text & "'," _
                    & " cvEstado=" & "'" & ddlEstados.SelectedValue & "'," _
                    & " cvCP=" & "'" & txtCP.Text & "'," _
                    & " cvPais=" & "'" & ddlPais.SelectedValue & "' " _
                    & " where ciIdRel=" & "'" & IdDireccion & "'"
                objDatos.fnEjecutarInsert(ssql)

            End If
            objDatos.Mensaje("Dirección actualizada", Me.Page)
        End If
        fnCargaDirecciones()
    End Sub
    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            ''Cargamos los estados

            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)

            objDatos.fnLog("pais index", "")
            Dim fila As DataRow
            fila = dtEstado.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtEstado.Rows.Add(fila)
            ddlEstados.DataSource = dtEstado
            ddlEstados.DataTextField = "Descripcion"
            ddlEstados.DataValueField = "Clave"
            ddlEstados.DataBind()
            ddlEstados.SelectedValue = "0"
        Catch ex As Exception
            objDatos.fnLog("Cargar estados", ex.Message)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("preferencias.aspx")
    End Sub
End Class

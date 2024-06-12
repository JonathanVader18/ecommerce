
Imports System.Data

Partial Class direcciones
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub direcciones_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()

        If Not IsPostBack Then

            ssql = "SELECT ISNULL(cvClienteLAT,'NO') FROM config.Parametrizaciones "
            Dim dtClienteLAT As New DataTable
            dtClienteLAT = objDatos.fnEjecutarConsulta(ssql)
            If dtClienteLAT.Rows.Count = 0 Then
                '   Response.Redirect("pagoindex.aspx")
            Else
                If dtClienteLAT.Rows(0)(0) = "SI" Then
                    ssql = "select ISNULL(cvLigaPagoIndex,'') from config.parametrizaciones"
                    Dim dtOtro As New DataTable
                    dtOtro = objDatos.fnEjecutarConsulta(ssql)
                    If dtOtro.Rows.Count > 0 Then
                        If dtOtro.Rows(0)(0) <> "" Then
                            Response.Redirect(dtOtro.Rows(0)(0))
                        Else
                            Response.Redirect("direccionesla.aspx")
                        End If

                    Else
                        Response.Redirect("direccionesla.aspx")
                    End If

                Else
                    '    Response.Redirect("pagoindex.aspx")
                End If
            End If

            ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvColonia, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ISNULL(cvPredeterminado,'NO') ='SI' "
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
                sHtml = sHtml & " <a class='btn-act-blok' id='btnEditar' href='editar-direccion.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnQuitar'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'> quitar</a>"
                '     sHtml = sHtml & " <a class='btn-act-blok' id='btnPred'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'> selecionar como predeterminado</a>"
                sHtml = sHtml & "</div>"

                sHTMLEncabezado = sHTMLEncabezado & sHtml
                sHTMLEncabezado = sHTMLEncabezado & "</div>"

                Dim literal As New LiteralControl(sHTMLEncabezado)
                pnlDirecciones.Controls.Add(literal)
                sHTMLEncabezado = ""
                sHtml = ""

            Next

            '''La última dirección, la ponemos arriba
            'lblNombre.Text = Session("NombreUserB2C")
            'lblDomicilio.Text = dtDirecciones.Rows(0)("cvCalle") & "No " & dtDirecciones.Rows(0)("cvNumExt") & " " & dtDirecciones.Rows(0)("cvNumInt")
            'If dtDirecciones.Rows(0)("cvColonia") = "" Then
            '    lblColoniaCiudad.Text = dtDirecciones.Rows(0)("cvCiudad") & ", " & dtDirecciones.Rows(0)("cvMunicipio")
            'Else
            '    lblColoniaCiudad.Text = dtDirecciones.Rows(0)("cvColonia") & ", " & dtDirecciones.Rows(0)("cvCiudad")
            'End If
            'lblPaisCP.Text = dtDirecciones.Rows(0)("cvPais") & " " & dtDirecciones.Rows(0)("cvCP")


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
                sHtml = sHtml & " <a class='btn-act-blok' id='btnEditar' href='editar-direccion.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
                sHtml = sHtml & " <a class='btn-act-blok' id='btnQuitar'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
                ' sHtml = sHtml & " <a class='btn-act-blok' id='btnPred'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
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
                        txtColonia.Text = dtDireccion.Rows(0)("cvColonia")
                        txtNumExt.Text = dtDireccion.Rows(0)("cvNumExt")
                        txtNumInt.Text = dtDireccion.Rows(0)("cvNumInt")
                        txtCiudad.Text = dtDireccion.Rows(0)("cvCiudad")
                        txtLocalidad.Text = dtDireccion.Rows(0)("cvMunicipio")
                        txtCp.Text = dtDireccion.Rows(0)("cvCP")
                        ddlEstados.SelectedValue = dtDireccion.Rows(0)("cvEstado")
                        ddlPais.SelectedValue = dtDireccion.Rows(0)("cvPais")

                    End If
                End If

                If Action = "p" Then
                    ''La establecemos como predeterminada
                    ssql = "UPDATE Tienda.Direcciones_Envio  SET cvPredeterminado='SI' WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel=" & "'" & IdDireccion & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    fnCargaDirecciones()
                End If

                If Action = "q" Then

                    ''Cargamos los valores claves del domicilio, por si tenemos que quitarlos de SAP
                    ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel= " & "'" & IdDireccion & "'"
                    Dim dtDireccion As New DataTable
                    dtDireccion = objDatos.fnEjecutarConsulta(ssql)
                    objDatos.fnLog("Direcciones", dtDireccion.Rows.Count)
                    If dtDireccion.Rows.Count > 0 Then
                        Session("CalleEnvio") = dtDireccion.Rows(0)("cvCalle")
                        Session("ColoniaEnvio") = dtDireccion.Rows(0)("cvColonia")
                        Session("NumExtEnvio") = dtDireccion.Rows(0)("cvNumExt")
                        Session("MunicipioEnvio") = dtDireccion.Rows(0)("cvMunicipio")

                    End If

                    ''La eliminamos
                    ssql = "DELETE Tienda.Direcciones_Envio  WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel=" & "'" & IdDireccion & "'"
                    objDatos.fnEjecutarInsert(ssql)

                    ''Revisamos si tenemos que ir a SAP a editar
                    ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
                    Dim dtCreaSAP As New DataTable
                    dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
                    If dtCreaSAP.Rows.Count > 0 Then
                        If dtCreaSAP.Rows(0)(0) = "SI" Then

                            ssql = "SELECT ISNULL(cvCardCode,'') FROM Config.usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'"
                            Dim dtClienteSAP As New DataTable
                            dtClienteSAP = objDatos.fnEjecutarConsulta(ssql)
                            Dim sCardCodeSAP As String = ""
                            If dtClienteSAP.Rows.Count > 0 Then

                                sCardCodeSAP = dtClienteSAP.Rows(0)(0)
                                Dim sIdDireccion As String = ""
                                If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
                                    sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
                                Else
                                    sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
                                End If
                                Dim sAdressName As String = ""
                                sAdressName = "Envio-" & sIdDireccion

                                ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                                ssql = ssql.Replace("[%0]", sCardCodeSAP).Replace("[%1]", sAdressName)
                                Dim dtDetalleDireccion As New DataTable
                                dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtDetalleDireccion.Rows.Count > 0 Then
                                    ''Ya existe
                                    Dim oCompany As SAPbobsCOM.Company
                                    oCompany = objDatos.fnConexion_SAP
                                    fnEliminaDireccionCliente(sCardCodeSAP, sAdressName, oCompany)
                                    objDatos.fnLog("dirección Decimal entrega", "Ya existe:" & sAdressName.Replace("'", ""))
                                End If
                            End If
                        End If
                    End If

                    fnCargaDirecciones()
                End If

            Else



            End If
        End If
    End Sub

    Public Function fnEliminaDireccionCliente(CardCode As String, Direccion As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion

        objDatos.fnLog("Actualizando direccion", sAdressName)


        Dim sQueryDirecciones As String = ""

        sQueryDirecciones = objDatos.fnObtenerQuery("DireccionesSN")

        If sQueryDirecciones <> "" Then
            sQueryDirecciones = sQueryDirecciones.Replace("[%0]", CardCode)


            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsultaSAP(sQueryDirecciones)

            If dtDirecciones.Rows.Count > 0 Then
                Dim oProspecto As SAPbobsCOM.BusinessPartners
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.GetByKey(CardCode)
                For i = 0 To dtDirecciones.Rows.Count - 1 Step 1

                    oProspecto.Addresses.SetCurrentLine(i)
                    If oProspecto.Addresses.AddressName = sAdressName Then
                        oProspecto.Addresses.Delete()
                        If oProspecto.Update() <> 0 Then
                            sIdDireccion = "ERROR"

                        End If
                        Exit For
                        sIdDireccion = sAdressName
                    End If



                Next
            End If


        End If



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function
    Public Sub fnCargaDirecciones()

        pnlDirecciones_lista.Controls.Clear()
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
            sHtml = sHtml & "   <a class='btn-act-blok' id='btnEditar' href='editar-direccion.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
            sHtml = sHtml & "   <a class='btn-act-blok' id='btnQuitar'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
            '    sHtml = sHtml & "   <a class='btn-act-blok' id='btnPred'  href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
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
            sHtml = sHtml & " <a class='btn-act-blok ' href='editar-direccion.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=e'>editar</a>"
            sHtml = sHtml & " <a class='btn-act-blok ' href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=q'>quitar</a>"
            ' sHtml = sHtml & " <a class='btn-act-blok ' href='direcciones.aspx?dir=" & dtDirecciones.Rows(i)("ciIdRel") & "&action=p'>selecionar como predeterminado</a>"
            sHtml = sHtml & "</div>"

            sHTMLEncabezado = sHTMLEncabezado & sHtml
            sHTMLEncabezado = sHTMLEncabezado & "</div>"

            Dim literal As New LiteralControl(sHTMLEncabezado)
            pnlDirecciones_lista.Controls.Add(literal)
            sHTMLEncabezado = ""
            sHtml = ""

        Next
    End Sub

    Public Function fnAgregarDireccionCliente(CardCode As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion

        objDatos.fnLog("Agregar direccion", sAdressName)



        Dim oProspecto As SAPbobsCOM.BusinessPartners

        oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        oProspecto.GetByKey(CardCode)
        oProspecto.Addresses.Add()
        oProspecto.Addresses.TypeOfAddress = "S"
        oProspecto.Addresses.AddressName = sAdressName
        oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
        oProspecto.Addresses.Block = Session("ColoniaEnvio")
        oProspecto.Addresses.County = Session("MunicipioEnvio")
        oProspecto.Addresses.ZipCode = Session("CPEnvio")
        oProspecto.Addresses.StreetNo = Session("NumExtEnvio")
        oProspecto.Addresses.Street = CStr(Session("CalleEnvio"))
        oProspecto.Addresses.State = Session("EstadoEnvio")
        '  oProspecto.Addresses.Country = ddlPais.SelectedValue
        If oProspecto.Update() <> 0 Then
            sIdDireccion = "ERROR"

        End If
        sIdDireccion = sAdressName



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Request.QueryString.Count = 0 Then
            ''Insertamos
            ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
                & "'" & Session("UserB2C") & "'," _
                & "'" & txtCalle.Text & "'," _
                & "'" & txtColonia.Text & "'," _
                & "'" & txtNumExt.Text & "'," _
                & "'" & txtNumInt.Text & "'," _
                & "'" & txtCiudad.Text & "'," _
                & "'" & txtLocalidad.Text & "'," _
                & "'" & ddlEstados.SelectedValue & "'," _
                & "'" & ddlPais.SelectedValue & "','N'," _
                & "'" & txtCp.Text & "')"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.Mensaje("Dirección registrada", Me.Page)

            ''Validamos si tenemos que meterla a SAP
            ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
            Dim dtCreaSAP As New DataTable
            dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
            If dtCreaSAP.Rows.Count > 0 Then
                If dtCreaSAP.Rows(0)(0) = "SI" Then

                    ssql = "SELECT ISNULL(cvCardCode,'') FROM Config.usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'"
                    Dim dtClienteSAP As New DataTable
                    dtClienteSAP = objDatos.fnEjecutarConsulta(ssql)

                    If dtClienteSAP.Rows.Count > 0 Then
                        ''Metemos la dirección

                        Session("CalleEnvio") = txtCalle.Text
                        Session("NumExtEnvio") = txtNumExt.Text
                        Session("ColoniaEnvio") = txtColonia.Text
                        Session("EstadoEnvio") = ddlEstados.SelectedValue
                        Session("MunicipioEnvio") = txtLocalidad.Text
                        Session("ColoniaEnvio") = txtColonia.Text
                        Session("CPEnvio") = txtCp.Text
                        Dim oCompany As SAPbobsCOM.Company
                        oCompany = objDatos.fnConexion_SAP
                        fnAgregarDireccionCliente(dtClienteSAP.Rows(0)(0), oCompany)

                        fnCargaDirecciones()
                    End If


                End If
            End If
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
                    & " cvColonia=" & "'" & txtColonia.Text & "'," _
                    & " cvNumExt=" & "'" & txtNumExt.Text & "'," _
                    & " cvNumInt=" & "'" & txtNumInt.Text & "'," _
                    & " cvCiudad=" & "'" & txtCiudad.Text & "'," _
                    & " cvMunicipio=" & "'" & txtLocalidad.Text & "'," _
                    & " cvEstado=" & "'" & ddlEstados.SelectedValue & "'," _
                    & " cvCP=" & "'" & txtCp.Text & "'," _
                    & " cvPais=" & "'" & ddlPais.SelectedValue & "' " _
                    & " where ciIdRel=" & "'" & IdDireccion & "'"
                objDatos.fnEjecutarInsert(ssql)

            End If
            objDatos.Mensaje("Dirección actualizada", Me.Page)
        End If
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

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("preferencias.aspx")
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
End Class

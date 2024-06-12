Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.Globalization
Imports System
Imports System.Net.Mime
Imports System.Net.Mail
Imports System.IO
Imports System.Net.Http
Imports System.Data.Odbc
Imports System.Windows.Forms


Public Class Cls_Funciones




    Private _Conexion As SqlConnection
    Public Shared vdtDatos As New DataTable

    Public oCompany As SAPbobsCOM.Company
    Public Function fnEjecutarSPConsulta(ByVal pStrSP As String) As DataTable
        Dim cmdSQL As New SqlCommand
        Dim daPuente As New SqlDataAdapter
        Dim dtDatos As New DataTable
        Dim vStrSQL As String
        vStrSQL = "Exec " & pStrSP
        Try
            Me._Conexion = New Data.SqlClient.SqlConnection
            Me._Conexion.ConnectionString = ""

            If _Conexion.State = ConnectionState.Closed Then
                _Conexion.Open()
            End If
            cmdSQL = New SqlCommand(vStrSQL, _Conexion)
            cmdSQL.CommandTimeout = 2000
            daPuente = New SqlDataAdapter(cmdSQL)
            daPuente.Fill(dtDatos)
            cmdSQL.Connection.Close()
        Catch ex As Exception

        End Try
        Return dtDatos
    End Function
    Public Function fnEjecutarConsulta(ByVal pStrSP As String) As DataTable
        Dim cmdSQL As New SqlCommand
        Dim daPuente As New SqlDataAdapter
        Dim dtDatos As New DataTable
        Dim vStrSQL As String
        vStrSQL = pStrSP
        Try
            Me._Conexion = New Data.SqlClient.SqlConnection

            Me._Conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnSat").ConnectionString

            If _Conexion.State = ConnectionState.Closed Then
                _Conexion.Open()
            End If
            cmdSQL = New SqlCommand(vStrSQL, _Conexion)
            cmdSQL.CommandTimeout = 200000
            daPuente = New SqlDataAdapter(cmdSQL)
            daPuente.Fill(dtDatos)
            cmdSQL.Connection.Close()
            cmdSQL.Connection.Dispose()
            cmdSQL.Dispose()

            _Conexion.Dispose()
        Catch ex As Exception
            cmdSQL.Connection.Close()
            cmdSQL.Connection.Dispose()
            cmdSQL.Dispose()

            Try
                fnLog("Error", ex.Message.Replace("'", "") & " " & pStrSP.Replace("'", ""))
                Me._Conexion.Close()
                _Conexion.Dispose()
                cmdSQL.Dispose()
            Catch ex2 As Exception

            End Try
        End Try
        Return dtDatos
    End Function


    Public Function fnEjecutarConsultaSAP(ByVal pStrSP As String) As DataTable
        Dim cmdSQL As New SqlCommand
        Dim daPuente As New SqlDataAdapter
        Dim dtDatos As New DataTable
        Dim vStrSQL As String
        vStrSQL = pStrSP
        Try

            If fnObtenerDBMS() = "HANA" Then
                fnLog("SAP DBMS", "HANA")
                dtDatos = fnEjecutarConsultaHANA(vStrSQL)
            Else
                Me._Conexion = New Data.SqlClient.SqlConnection
                ''Cargamos la configuracion a SAP
                Dim ssql As String
                ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
                Dim dtConfSAP As New DataTable
                dtConfSAP = fnEjecutarConsulta(ssql)
                If dtConfSAP.Rows.Count > 0 Then
                    Me._Conexion.ConnectionString = "Data Source=" & dtConfSAP.Rows(0)("cvServidorSQL") & ";Initial Catalog=" & dtConfSAP.Rows(0)("cvBD") & ";User ID=" & dtConfSAP.Rows(0)("cvUserSQL") & ";Password=" & dtConfSAP.Rows(0)("cvPwdSQL")
                End If


                If _Conexion.State = ConnectionState.Closed Then
                    _Conexion.Open()
                End If
                cmdSQL = New SqlCommand(vStrSQL, _Conexion)
                cmdSQL.CommandTimeout = 20000
                daPuente = New SqlDataAdapter(cmdSQL)
                daPuente.Fill(dtDatos)
                cmdSQL.Connection.Close()

                cmdSQL.Dispose()
                _Conexion.Dispose()
            End If

        Catch ex As Exception
            GC.Collect()
            cmdSQL.Connection.Close()

            cmdSQL.Dispose()
            _Conexion.Dispose()
            Try
                fnLog("Error", ex.Message.Replace("'", "") & " " & pStrSP.Replace("'", ""))
                Me._Conexion.Close()
                _Conexion.Dispose()
                cmdSQL.Dispose()
            Catch ex2 As Exception

            End Try
        End Try
        Return dtDatos
    End Function
    Public Function fnEjecutarInsertSAP(ByVal pStrSP As String) As DataTable
        Dim cmdSQL As New SqlCommand
        Dim daPuente As New SqlDataAdapter
        Dim dtDatos As New DataTable
        Dim vStrSQL As String
        vStrSQL = pStrSP
        Try
            Me._Conexion = New Data.SqlClient.SqlConnection
            ''Cargamos la configuracion a SAP
            Dim ssql As String
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                Me._Conexion.ConnectionString = "Data Source=" & dtConfSAP.Rows(0)("cvServidorSQL") & ";Initial Catalog=" & dtConfSAP.Rows(0)("cvBD") & ";User ID=" & dtConfSAP.Rows(0)("cvUserSQL") & ";Password=" & dtConfSAP.Rows(0)("cvPwdSQL")
            End If


            If _Conexion.State = ConnectionState.Closed Then
                _Conexion.Open()
            End If
            cmdSQL = New SqlCommand(vStrSQL, _Conexion)
            cmdSQL.CommandTimeout = 2000
            cmdSQL.ExecuteNonQuery()
            cmdSQL.Connection.Close()

            cmdSQL.Dispose()
            _Conexion.Dispose()
        Catch ex As Exception
            Try
                fnLog("Error", ex.Message.Replace("'", "") & " " & pStrSP.Replace("'", ""))
                Me._Conexion.Close()
                _Conexion.Dispose()
                cmdSQL.Dispose()
            Catch ex2 As Exception

            End Try
        End Try
        Return dtDatos
    End Function

    Public Function fnEjecutarConsultaNOI(ByVal pStrSP As String) As DataTable
        Dim cmdSQL As New SqlCommand
        Dim daPuente As New SqlDataAdapter
        Dim dtDatos As New DataTable
        Dim vStrSQL As String
        vStrSQL = pStrSP
        Try
            Me._Conexion = New Data.SqlClient.SqlConnection
            Me._Conexion.ConnectionString = "Data Source=PDGDL-NOMINA\SQLNOMINA;Initial Catalog=ctNomIP;User ID=sa;Password=electronica"

            If _Conexion.State = ConnectionState.Closed Then
                _Conexion.Open()
            End If
            cmdSQL = New SqlCommand(vStrSQL, _Conexion)
            cmdSQL.CommandTimeout = 2000
            daPuente = New SqlDataAdapter(cmdSQL)
            daPuente.Fill(dtDatos)
            cmdSQL.Connection.Close()
        Catch ex As Exception
        End Try
        Return dtDatos
    End Function

    Public Function fnArticuloOferta(ItemCode As String) As String
        ''Promociones
        Dim ssql As String = ""
        Dim EsOferta As String = "NO"


        Dim dtCampoPromo As New DataTable
        ssql = fnObtenerQuery("Promociones")
        If ssql <> "" Then
            dtCampoPromo = fnEjecutarConsultaSAP(ssql)
            If dtCampoPromo.Rows.Count > 0 Then
                ''Evaluamos si existen articulos en promoción
                ssql = ssql & " and T0.itemCode=" & "'" & ItemCode & "'"
                Dim dtArticulosPromo As New DataTable
                dtArticulosPromo = fnEjecutarConsultaSAP(ssql)
                If dtArticulosPromo.Rows.Count > 0 Then
                    EsOferta = "SI"
                End If
            End If
        End If

        Return EsOferta
    End Function

    Public Function fnObtenerCliente() As String
        Dim ssql As String = ""
        Dim sCliente As String = ""
        ssql = "select cvCliente from config.datoscliente"
        Dim dtCliente As New DataTable
        dtCliente = fnEjecutarConsulta(ssql)

        If dtCliente.Rows.Count > 0 Then
            sCliente = dtCliente.Rows(0)(0)
        End If
        Return sCliente
    End Function

    Public Function fnDescuentoEspecial(ItemCode As String, Cliente As String) As Double
        ''Promociones
        Dim ssql As String = ""
        Dim Descuento As Double = 0
        fnLog("Descuento Especial", "Entra")

        Dim dtCampoPromo As New DataTable
        ssql = fnObtenerQuery("DescuentoEspecial")

        If ssql <> "" And Cliente <> "" Then
            ''Evaluamos si existen articulos en promoción
            ssql = ssql.Replace("[%0]", ItemCode)
            ssql = ssql.Replace("[%1]", Cliente)

            fnLog("Descuento Especial", ssql.Replace("'", ""))
            Dim dtArticulosPromo As New DataTable
            dtArticulosPromo = fnEjecutarConsultaSAP(ssql)
            If dtArticulosPromo.Rows.Count > 0 Then
                Descuento = dtArticulosPromo.Rows(0)(0)
            Else
                ssql = fnObtenerQuery("ObtenerGrupoItemCode")
                ssql = ssql.Replace("[%0]", ItemCode)
                Dim dtGrupo As New DataTable
                dtGrupo = fnEjecutarConsultaSAP(ssql)
                If dtGrupo.Rows.Count > 0 Then
                    ssql = "select Discount  from EDG1 where AbsEntry = (select AbsEntry from OEDG where ObjCode ='" & Cliente & "') and ObjKey='" & dtGrupo.Rows(0)(0) & "'"
                    fnLog("Descuento:", ssql.Replace("'", ""))
                    Dim dtDescPorGrupo As New DataTable
                    dtDescPorGrupo = fnEjecutarConsultaSAP(ssql)
                    If dtDescPorGrupo.Rows.Count > 0 Then
                        Descuento = dtDescPorGrupo.Rows(0)(0)
                    End If
                End If


            End If
        End If


        Return Descuento
    End Function

    Public Function fnEjecutarInsert(ByVal pStrSP As String)
        Dim cmdSQL As New SqlCommand
        Dim vStrSQL As String
        Dim mensaje As String = ""
        vStrSQL = pStrSP
        Try
            Me._Conexion = New Data.SqlClient.SqlConnection
            Me._Conexion.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("ConnSat").ConnectionString
            'Me._Conexion.ConnectionString = "Data Source=PDGDL-SAP;Initial Catalog=IP;User ID=sa;Password=SA"

            If _Conexion.State = ConnectionState.Closed Then
                _Conexion.Open()
            End If
            cmdSQL = New SqlCommand(vStrSQL, _Conexion)
            cmdSQL.ExecuteNonQuery()
            cmdSQL.CommandTimeout = 20000
            cmdSQL.Connection.Close()
            cmdSQL.Connection.Dispose()
            cmdSQL.Dispose()
            Me._Conexion.Close()
            _Conexion.Dispose()
        Catch ex As Exception
            fnLog("EjecutarInsert", ex.Message.Replace("'", ""))
            mensaje = ex.Message
            Try
                _Conexion.Close()
                _Conexion.Dispose()
                cmdSQL.Dispose()
            Catch ex2 As Exception

            End Try
        End Try
        Return mensaje
    End Function
    Public Sub Mensaje(ByVal Mensaje As String, ByVal MyPage As Web.UI.Page)
        MyPage.ClientScript.RegisterStartupScript(MyPage.GetType(), "Ventana", "alert('" & Mensaje & "','mywindow','');", True)
    End Sub
    Public Sub fnEnviarCorreo(Destinatario As String, Body As String, asunto As String)
        Try
            Dim ssql As String
            ssql = "select cvServerSMTP,cvUsuario,cvPass,cvSSL,cvCorreo,cvPuerto from config.correos  "
            Dim dtConf As New DataTable
            Dim objDatos As New Cls_Funciones
            dtConf = objDatos.fnEjecutarConsulta(ssql)

            Dim mimeType As New ContentType("text/html")
            Dim alternate As AlternateView = AlternateView.CreateAlternateViewFromString(Body, mimeType)

            Dim Mailmsg As New System.Net.Mail.MailMessage
            Dim archivos As String()
            Dim obj As New Net.Mail.SmtpClient
            Mailmsg.From = New System.Net.Mail.MailAddress(dtConf.Rows(0)("cvCorreo"))


            If Destinatario.Contains(";") Then
                Dim sCorreosVarios As String() = Destinatario.Split(";")
                For Each correo As String In sCorreosVarios
                    Mailmsg.To.Add(correo)
                Next
            Else
                Mailmsg.To.Add(Destinatario) '
            End If

            ' Mailmsg.To.Add(Destinatario) '
            Mailmsg.Subject = asunto
            Mailmsg.IsBodyHtml = True
            Mailmsg.Body = Body
            obj.Host = dtConf.Rows(0)("cvServerSMTP")
            obj.Credentials = New NetworkCredential(CStr(dtConf.Rows(0)("cvUsuario")), CStr(dtConf.Rows(0)("cvPass")))
            'obj.Port = CInt(dtEnvio.Rows(0).Item("U_Puerto"))
            If dtConf.Rows(0)("cvSSL") = "SI" Then
                obj.EnableSsl = True
            Else
                obj.EnableSsl = False
            End If

            obj.Port = dtConf.Rows(0)("cvPuerto")
            obj.Send(Mailmsg)
            Mailmsg = Nothing
        Catch ex As Exception
            fnLog("Envio de Correo", ex.Message)
            '  MsgBox("Ha ocurrido un problema: " & ex.Message)
        End Try

    End Sub

    Public Sub fnEnviarCorreo(Destinatario As String, Body As String, Archivo As String, asunto As String)
        Try
            Dim ssql As String
            ssql = "select cvServerSMTP,cvUsuario,cvPass,cvSSL,cvCorreo,cvPuerto from config.correos  "
            Dim dtConf As New DataTable
            Dim objDatos As New Cls_Funciones
            dtConf = objDatos.fnEjecutarConsulta(ssql)

            Dim mimeType As New ContentType("text/html")
            Dim alternate As AlternateView = AlternateView.CreateAlternateViewFromString(Body, mimeType)

            Dim Mailmsg As New System.Net.Mail.MailMessage
            Dim archivos As String()
            Dim obj As New Net.Mail.SmtpClient
            Mailmsg.From = New System.Net.Mail.MailAddress(dtConf.Rows(0)("cvCorreo"))

            If Archivo <> "" Then
                Dim oAtach As System.Net.Mail.Attachment
                oAtach = New System.Net.Mail.Attachment(Archivo)
                Mailmsg.Attachments.Add(oAtach)
            End If

            If Destinatario.Contains(";") Then
                Dim sCorreosVarios As String() = Destinatario.Split(";")
                For Each correo As String In sCorreosVarios
                    Mailmsg.To.Add(correo) '
                Next
            Else
                Mailmsg.To.Add(Destinatario) '
            End If

            Mailmsg.Subject = asunto
            Mailmsg.IsBodyHtml = True
            Mailmsg.Body = Body
            obj.Host = dtConf.Rows(0)("cvServerSMTP")
            obj.Credentials = New NetworkCredential(CStr(dtConf.Rows(0)("cvUsuario")), CStr(dtConf.Rows(0)("cvPass")))
            'obj.Port = CInt(dtEnvio.Rows(0).Item("U_Puerto"))
            If dtConf.Rows(0)("cvSSL") = "SI" Then
                obj.EnableSsl = True
            Else
                obj.EnableSsl = False
            End If

            obj.Port = dtConf.Rows(0)("cvPuerto")
            obj.Send(Mailmsg)
            Mailmsg = Nothing
        Catch ex As Exception
            fnLog("Envio de Correo", ex.Message.Replace("'", ""))
            '  MsgBox("Ha ocurrido un problema: " & ex.Message)
        End Try

    End Sub

    Public Function fnPrecioActual(itemCode As String) As Double
        Dim ssql As String
        Dim Precio As Double = 0
        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If

        ssql = "SELECT ciIdListaPrecios FROM config.DatosCliente "
        Dim dtLista As New DataTable
        dtLista = fnEjecutarConsulta(ssql)

        If dtLista.Rows.Count > 0 Then
            'ListaPrecios = dtLista.Rows(0)(0)
            If iBandRedondeo = 1 Then
                ssql = "SELECT ROUND(ISNULL(price,'0')," & DecRedondeo & ") FROM ITM1 with(nolock) WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & dtLista.Rows(0)(0) & "'"
            Else
                ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & dtLista.Rows(0)(0) & "'"
            End If


            If fnObtenerDBMS() = "HANA" Then
                ssql = fnObtenerQuery("GetPrice")
                ssql = ssql.Replace("[%0]", itemCode)
                ssql = ssql.Replace("[%1]", dtLista.Rows(0)(0))
            Else
                ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode like " & "'" & itemCode & "%' AND pricelist=" & "'" & dtLista.Rows(0)(0) & "'"
            End If

            Dim dtPrecio As New DataTable
            dtPrecio = fnEjecutarConsultaSAP(ssql)
            If dtPrecio.Rows.Count > 0 Then
                Precio = dtPrecio.Rows(0)(0)
            Else
                Precio = 0
            End If
            ''Revisamos si se mostrarán precios más IVA
            ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
            Dim dtTipoPrecio As New DataTable
            dtTipoPrecio = fnEjecutarConsulta(ssql)
            If dtTipoPrecio.Rows.Count > 0 Then
                If dtTipoPrecio.Rows(0)(0) = "SI" Then
                    ''Obtenemos el porcentaje de IVA
                    ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                    Dim dtPorcIVA As New DataTable
                    dtPorcIVA = fnEjecutarConsulta(ssql)
                    If dtPorcIVA.Rows.Count > 0 Then
                        Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                    End If
                End If
            End If
        End If

        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If

        Return Precio
    End Function

    Public Function fnPrecioActual(itemCode As String, Moneda As String) As Double
        Dim ssql As String
        Dim Precio As Double = 0

        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If


        ssql = "SELECT ciIdListaPrecios FROM config.DatosCliente "
        Dim dtLista As New DataTable
        dtLista = fnEjecutarConsulta(ssql)

        If dtLista.Rows.Count > 0 Then

            If iBandRedondeo = 1 Then
                ssql = "SELECT ROUND(ISNULL(price,'0')," & DecRedondeo & ")) FROM ITM1 with(nolock) WHERE ItemCode like " & "'" & itemCode & "%' AND pricelist=" & "'" & dtLista.Rows(0)(0) & "' and currency=" & "'" & Moneda & "'"
            Else
                ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode like " & "'" & itemCode & "%' AND pricelist=" & "'" & dtLista.Rows(0)(0) & "' and currency=" & "'" & Moneda & "'"
            End If

        End If


        Dim dtPrecio As New DataTable
        dtPrecio = fnEjecutarConsultaSAP(ssql)
        If dtPrecio.Rows.Count > 0 Then
            Precio = dtPrecio.Rows(0)(0)
        Else
            Precio = 0
        End If

        ''Revisamos si se mostrarán precios más IVA
        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "SI" Then
                ''Obtenemos el porcentaje de IVA
                ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                Dim dtPorcIVA As New DataTable
                dtPorcIVA = fnEjecutarConsulta(ssql)
                If dtPorcIVA.Rows.Count > 0 Then
                    Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                End If
            End If
        End If
        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If
        Return Precio
    End Function

    Public Function fnPrecioActual(itemCode As String, IdListaPrecios As Int16) As Double
        Dim ssql As String
        Dim Precio As Double = 0

        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If



        If iBandRedondeo = 1 Then
            ssql = "SELECT ROUND(ISNULL(price,'0')," & DecRedondeo & ")) FROM ITM1 with(nolock) WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"

        Else
            ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"
        End If




        If fnObtenerDBMS() = "HANA" Then
            ssql = fnObtenerQuery("GetPrice")
            ssql = ssql.Replace("[%0]", itemCode)
            ssql = ssql.Replace("[%1]", IdListaPrecios)
        Else
            ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode like " & "'" & itemCode & "%' AND pricelist=" & "'" & IdListaPrecios & "'"
        End If



        Dim dtPrecio As New DataTable
        dtPrecio = fnEjecutarConsultaSAP(ssql)
        If dtPrecio.Rows.Count > 0 Then
            Precio = dtPrecio.Rows(0)(0)
        Else
            Precio = 0
        End If



        ''Revisamos si se mostrarán precios más IVA
        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "SI" Then
                ''Obtenemos el porcentaje de IVA
                ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                Dim dtPorcIVA As New DataTable
                dtPorcIVA = fnEjecutarConsulta(ssql)
                If dtPorcIVA.Rows.Count > 0 Then
                    Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                End If
            End If
        End If


        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If



        Return Precio
    End Function
    Public Function fnArticuloDescuentoDelta(itemCode As String) As Int16
        Dim EsRebaja As Int16 = 0
        Dim ssql As String
        ssql = "SELECT Discount FROM OSPP WHERE ItemCode = '" & itemCode & "' AND Discount  > 0 and ListNum in('14') and CardCode='304'"
        Dim dtEsDescuento As New DataTable
        dtEsDescuento = fnEjecutarConsultaSAP(ssql)
        If dtEsDescuento.Rows.Count > 0 Then
            EsRebaja = 1
        End If
        fnLog("Descuentos", itemCode & "- Rebaja:" & EsRebaja)
        Return EsRebaja
    End Function

    Public Function fnObtenerDescuentoEspecialDelta(itemCode As String) As Double
        Dim DesctoEspecial As Double = 0
        Dim ssql As String
        ssql = "SELECT Discount FROM OSPP WHERE ItemCode = '" & itemCode & "' AND Discount  > 0 and ListNum in('14') and CardCode='304'"
        Dim dtEsDescuento As New DataTable
        dtEsDescuento = fnEjecutarConsultaSAP(ssql)
        If dtEsDescuento.Rows.Count > 0 Then
            DesctoEspecial = dtEsDescuento.Rows(0)(0)
        End If


        Return DesctoEspecial
    End Function

    Public Function fnObtenerDescuentoPorCantidad(itemcode As String, ListaPrecios As Int32, Cantidad As Double) As Double
        Dim DesctoEspecial As Double = 0
        Dim ssql As String
        If fnObtenerDBMS().ToUpper = "HANA" Then
            ssql = "SELECT TOP 1 ""Discount"" FROM ""#BDSAP#"".""SPP2"" where ""ItemCode"" ='" & itemcode & "' and ""Amount""<='" & Cantidad & "' and ""CardCode"" ='*" & ListaPrecios & "' Order by ""Discount"" desc"
            Dim dtdescuento As New DataTable
            dtdescuento = fnEjecutarConsultaSAP(ssql)
            If dtdescuento.Rows.Count > 0 Then
                DesctoEspecial = dtdescuento.Rows(0)(0)
            Else
                fnLog("Consulta descuentos:", ssql.Replace("'", ""))
            End If
        End If
        Return DesctoEspecial
    End Function



    Public Function fnCalculaIVA(Precio As Double) As Double
        Dim IVA As Double = 0
        Dim ssql As String = ""
        ''Obtenemos el porcentaje de IVA
        ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
        Dim dtPorcIVA As New DataTable
        dtPorcIVA = fnEjecutarConsulta(ssql)
        If dtPorcIVA.Rows.Count > 0 Then
            IVA = Precio * (dtPorcIVA.Rows(0)(0))

        End If





        Return IVA
    End Function

    Public Function fnPrecioActual(itemCode As String, IdListaPrecios As Int16, Moneda As String) As Double
        Dim ssql As String
        Dim Precio As Double = 0

        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If

        If fnObtenerDBMS() = "HANA" Then
            ssql = fnObtenerQuery("GetPriceCur")
            ssql = ssql.Replace("[%0]", itemCode)
            ssql = ssql.Replace("[%1]", IdListaPrecios)
            ssql = ssql.Replace("[%2]", Moneda)
        Else
            ssql = "SELECT ISNULL(price,'0') FROM ITM1 with(nolock) WHERE ItemCode like " & "'" & itemCode & "%' AND pricelist=" & "'" & IdListaPrecios & "' and Currency=" & "'" & Moneda & "'"
        End If


        Dim dtPrecio As New DataTable
        dtPrecio = fnEjecutarConsultaSAP(ssql)
        If dtPrecio.Rows.Count > 0 Then
            Precio = dtPrecio.Rows(0)(0)
        Else
            Precio = 0
        End If

        ''Revisamos si se mostrarán precios más IVA
        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "SI" Then
                ''Obtenemos el porcentaje de IVA
                ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                Dim dtPorcIVA As New DataTable
                dtPorcIVA = fnEjecutarConsulta(ssql)
                If dtPorcIVA.Rows.Count > 0 Then
                    Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                End If
            End If
        End If

        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If

        Return Precio
    End Function


    Public Function fnObtenerDBMS() As String
        Dim ssql As String
        ssql = "select cvDBMS from config.conexionSAP "
        Dim dtDAtos As New DataTable
        dtDAtos = fnEjecutarConsulta(ssql)
        Return dtDAtos.Rows(0)(0)
    End Function

    Public Function fnObtenerConfiguracionSAP() As DataTable
        Dim ssql As String
        ssql = "select * from config.conexionSAP "
        Dim dtDAtos As New DataTable
        dtDAtos = fnEjecutarConsulta(ssql)
        Return dtDAtos
    End Function
    Public Function fnObtenerQuery(Tipo As String) As String
        Dim sQuery As String = ""
        Dim ssql As String
        ssql = "SELECT cvQuery FROM config.Querys WHERE cvTipo=" & "'" & Tipo & "' AND cvDBMS=" & "'" & fnObtenerDBMS() & "'"
        Dim dtQuery As New DataTable
        dtQuery = fnEjecutarConsulta(ssql)
        If dtQuery.Rows.Count > 0 Then
            sQuery = dtQuery.Rows(0)(0)
        End If

        Return sQuery
    End Function

    Class Semana
        Private fi As DateTime
        Private ff As DateTime

        Public Property FechaInicial() As DateTime
            Get
                Return fi
            End Get
            Private Set
                fi = Value
            End Set
        End Property

        Public Property FechaFinal() As DateTime
            Get
                Return ff
            End Get
            Private Set
                ff = Value
            End Set
        End Property

        Public Sub New(num As Integer)
            If num < 1 OrElse num > 53 Then
                Throw New ArgumentException("En número de la semana debe estar comprendido en el rango [1..53]")
            End If

            FechaInicial = New DateTime(2016, 1, 8)
            FechaInicial = FechaInicial.AddDays(DayOfWeek.Monday - FechaInicial.DayOfWeek)

            Dim sem As Integer = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(FechaInicial, CalendarWeekRule.FirstDay, DayOfWeek.Monday)

            If sem <> num Then
                FechaInicial = FechaInicial.AddDays(((num - sem) * 7))
            End If

            FechaFinal = FechaInicial.AddDays(6)
        End Sub
    End Class

    Public Sub fnLog(Modulo As String, Mensaje As String)
        Dim ssql As String
        ssql = "INSERT INTO Log (Tipo,Error,Fecha) VALUES(" & "'" & Modulo & "'," & "'" & Mensaje & "',getdate())"
        fnEjecutarInsert(ssql)

    End Sub

    Public Sub fnLogMoneta(OTP As String, Mensaje As String)
        Dim ssql As String
        ssql = "INSERT INTO LogMoneta (OTP,Mensaje,Fecha) VALUES(" & "'" & OTP & "'," & "'" & Mensaje.Replace("'", "") & "',getdate())"
        fnEjecutarInsert(ssql)

    End Sub

    Public Function fnCreaFichaProductoPMK(Articulo As String) As String
        Dim sHTML As String = ""
        Dim ssql As String = "SELECT HTML FROM tablaModelos6 where articulo=" & "'" & Articulo & "'"
        Dim dtDatos As New DataTable
        dtDatos = fnEjecutarConsulta(ssql)

        If dtDatos.Rows.Count = 0 Then
            fnLog("Crea ficha especial PMK", "SIN DATOS PARA: " & Articulo & " " & ssql.Replace("'", ""))

        Else
            sHTML = dtDatos.Rows(0)(0)
        End If

        sHTML = sHTML.Replace("@", "'")

        Return sHTML

    End Function

    Public Function fnPromoFleteDeltaSocios() As Double
        Dim iMontoFleteGratis As Double = 20000000
        Dim ssql As String = ""
        ssql = fnObtenerQuery("FletePromoClientes")
        Dim dtMontoFlete As New DataTable
        dtMontoFlete = fnEjecutarConsultaSAP(ssql)

        If dtMontoFlete.Rows.Count > 0 Then
            iMontoFleteGratis = dtMontoFlete.Rows(0)(0)
        End If


        Return iMontoFleteGratis

    End Function

    Public Function fnConexion_SAP() As SAPbobsCOM.Company
        Dim viConexion As Integer = 0
        Dim dtConfiguracion As New DataTable
        Dim ssql As String = ""
        dtConfiguracion = fnObtenerConfiguracionSAP()
        fnLog("Conexion SAP", "Obtiene Configuracion")
        oCompany = New SAPbobsCOM.Company
        Try
            oCompany.CompanyDB = dtConfiguracion.Rows(0)("cvBD")


            'oCompany.Server = dtConfiguracion.Rows(0)("cvServidorSQL")
            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2008" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
            End If
            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2012" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012
            End If
            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2014" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014
            End If

            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2016" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016
            End If
            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2017" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017
            End If
            If dtConfiguracion.Rows(0)("cvVersionSQL") = "2019" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019
            End If

            If dtConfiguracion.Rows(0)("cvVersionSQL") = "HANA" Then
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.Server = dtConfiguracion.Rows(0)("cvServidorLicencias")
            End If

            'oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
            'oCompany.LicenseServer = dtConfiguracion.Rows(0)("cvServidorLicencias")
            oCompany.UserName = dtConfiguracion.Rows(0)("cvUserSAP")
            oCompany.Password = dtConfiguracion.Rows(0)("cvPWDSAP")
            oCompany.DbUserName = dtConfiguracion.Rows(0)("cvUserSQL")
            oCompany.DbPassword = dtConfiguracion.Rows(0)("cvPWDSQL")
            'oCompany.language = SAPbobsCOM.BoSuppLangs.ln_English

            fnLog("Conexion SAP", "Carga Datos antes de Connect")

            If oCompany.Connect <> 0 Then
                viConexion = 0
                fnLog("Conexion SAP", "No Conecta: " & oCompany.GetLastErrorDescription & " " & dtConfiguracion.Rows(0)("cvVersionSQL"))
            Else
                fnLog("Conexion SAP", "Si se conecta")
                viConexion = 1
            End If

        Catch ex As Exception
            fnLog("Conexion SAP", ex.Message)
        End Try

        Return oCompany
    End Function
    Public Function fnCreaFichaProducto(itemCode As String, RutaServer As String, slpCode As String, ListaPrecios As Short, UserB2B As String, UserB2C As String, Cliente As String)


        fnLog("CreaFicha", itemCode & "-" & slpCode & "-" & ListaPrecios & "-" & UserB2B & "-" & UserB2C & "-" & Cliente)
        Dim sHtmlBanner As String = ""
        Dim sImagen As String = "ImagenPal"
        Dim dtProductos As New DataTable
        Dim dPrecioActual As Double = 0
        Dim ssql As String = ""
        ssql = fnObtenerQuery("Info-Producto")
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtProductos = fnEjecutarConsultaSAP(ssql)
        Dim i As Int16 = 0
        Dim objDatos As New Cls_Funciones
        Dim sCaracterMoneda As String = ""
        ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
        Dim dtCaracter As New DataTable
        dtCaracter = fnEjecutarConsulta(ssql)
        If dtCaracter.Rows.Count > 0 Then
            sCaracterMoneda = dtCaracter.Rows(0)(0)
        End If

        Dim sCliente As String = ""
        Dim sStyle As String = ""
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            sCliente = CStr(dtclienteSeg.Rows(0)(0)).ToUpper
        End If
        Dim B2CActivo As String ="SI"
        If Cliente = "" And slpCode = 0 Then
            ssql = "SELECT ISNULL(cvB2CActivo,'SI') from Config.Parametrizaciones"
            Dim dtB2C As New DataTable
            dtB2C = objDatos.fnEjecutarConsulta(ssql)
            If dtB2C.Rows.Count > 0 Then
                If dtB2C.Rows(0)(0) = "NO" Then

                    B2CActivo = "NO"

                End If
            End If

        End If


        If UserB2B = "" And Cliente <> "" Then
            UserB2B = Cliente
        End If
        Dim sLeyendaPrecio As String = ""

        If sCliente.Contains("STOP CAT") Then
            sLeyendaPrecio = "  "
        End If
        '''Evaluamos el stock
        'Dim existencia As Double = 0
        '''Existencia 
        'ssql = objDatos.fnObtenerQuery("ExistenciaSAPHijos")
        'Dim dtExistencia As New DataTable
        'ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        'objDatos.fnLog("existencia", ssql.Replace("'", ""))
        'If ssql <> "" Then
        '    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
        '    If dtExistencia.Rows.Count > 0 Then
        '        existencia = CDbl(dtExistencia.Rows(0)(0))
        '        If existencia = 0 Then
        '            sStyle = "style='-webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */  filter: grayscale(100%);'"
        '        End If
        '    End If
        'End If


        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)


        sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
        sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
        sHtmlBanner = sHtmlBanner & " <div class='preview'>"
        sHtmlBanner = sHtmlBanner & "  <div class='img'>"
        sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive' " & sStyle & ">"

        If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
        End If

        Dim sPintaPrev As String = "SI"
        Dim sPintaFav As String = "SI"
        Dim sPintaCompra As String = "SI"

        ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
        Dim dtPintaCat As New DataTable
        dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
        If dtPintaCat.Rows.Count > 0 Then
            sPintaPrev = dtPintaCat.Rows(0)("Interior")
            sPintaFav = dtPintaCat.Rows(0)("Favorito")
            sPintaCompra = dtPintaCat.Rows(0)("Comprar")
        End If

        If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
        Else
            sHtmlBanner = sHtmlBanner & "     <div>"
        End If

        If sPintaCompra = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
        End If
        If sPintaFav = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
        End If
        If sPintaCompra = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
        End If
        sHtmlBanner = sHtmlBanner & "     </div>"
        sHtmlBanner = sHtmlBanner & "  </div>"

        If CInt(slpCode) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
        Else
            'If UserB2C = "" Then
            If UserB2C = "" And UserB2B = "" Then
                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            Else
                If UserB2C <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                End If

            End If

            ' Else
            '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            'End If

        End If
        Dim descuentoPromo As Double = 0
        Dim precioLleno As Double = 0

        If Cliente <> "" Then
            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
            Dim descEspecial As Double = 0
            descEspecial = objDatos.fnDescuentoEspecial(dtProductos.Rows(i)("ItemCode"), Cliente)
            If descEspecial > 0 Then

                dPrecioActual = (dPrecioActual * (1 - (descEspecial / 100)))

            End If

            If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                'B2B sin IVA
                dPrecioActual = dPrecioActual / 1.16
            End If

        End If

        If CStr(fnObtenerCliente()).ToUpper.Contains("STOP CAT") Then
            precioLleno = dPrecioActual
            descuentoPromo = fnObtenerDescuentoEspecialDelta(dtProductos.Rows(i)("ItemCode"))
            If descuentoPromo > 0 Then

                dPrecioActual = dPrecioActual * (1 - (descuentoPromo / 100))

            End If

        Else

        End If


        Dim sMoneda As String = ""
        sMoneda = fnObtenerMoneda(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
        Dim fDescuento As Double = 0
        fDescuento = fnDesctoB2C(dtProductos.Rows(i)("ItemCode"))

        If B2CActivo = "SI" Or CStr(fnObtenerCliente()).ToUpper.Contains("STOP CAT") Then
            If fDescuento > 0 Or descuentoPromo > 0 Then
                If fDescuento = 0 Then
                    fDescuento = descuentoPromo
                    dPrecioActual = precioLleno
                End If
                'Precio original
                sHtmlBanner = sHtmlBanner & "   <span class='precio-original2'>" & sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda & "</span>"
                'Precio tras descto

                sHtmlBanner = sHtmlBanner & "   <span class='precio-descto'>" & sCaracterMoneda & " " & (dPrecioActual * (1 - (fDescuento / 100))).ToString("###,###,###.#0") & " " & sMoneda & "</span>"
            Else
                If dPrecioActual > 0 Then
                    sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda & sLeyendaPrecio & " </span>"
                End If

            End If
        End If


        ''Fichas colores disponibles
        sHtmlBanner = sHtmlBanner & fnCargaFichasColoresMini(itemCode, RutaServer)

        sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
        ''img/home/producto-1.png
        For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                    sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                Else
                    sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                End If
            Else
                If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then

                    If CInt(slpCode) <> 0 Then

                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                    Else
                        'If UserB2C = "" Then
                        If UserB2C = "" And UserB2B = "" Then
                            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                        Else
                            If UserB2C <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                            End If

                        End If

                        ' Else
                        '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                        'End If

                    End If
                    If Cliente <> "" Then
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                    End If

                    ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))

                End If
                If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
                    Dim iband As Int16 = 0
                    sImagen = "images/no-image.png"

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
                    End If

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & ".jpg"
                    End If



                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
                    End If

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
                    End If
                    If iband = 0 Then
                        Try
                            ' sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                            If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value And iband = 0 Then
                                sImagen = "images/no-image.png"
                            Else
                                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" And iband = 0 Then
                                    sImagen = "images/no-image.png"
                                Else
                                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))) Then

                                        sImagen = "images/products/" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                                    Else
                                        sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                                    End If

                                End If
                            End If

                        Catch ex As Exception

                        End Try
                    End If

                    sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
                End If
            End If

        Next


        sHtmlBanner = sHtmlBanner & "  </a>"

        ''Si es BOOS, metemos el control para comprar
        If sCliente.ToUpper.Contains("BOSS") Then
            sHtmlBanner = sHtmlBanner & fnControlCompraRapida(dtProductos.Rows(i)("ItemCode"))
        End If

        sHtmlBanner = sHtmlBanner & " </div>"

        sHtmlBanner = sHtmlBanner & "  </a>"

        sHtmlBanner = sHtmlBanner & "</div>"

        Return sHtmlBanner
    End Function

    Public Function fnControlCompraRapida(ItemCode As String) As String
        Dim sHTML As String = ""
        Const Comillas As String = """"
        sHTML = sHTML & fnControlesPorCaja(ItemCode)
        sHTML = sHTML & " <div class='col-xs-12'>"
        sHTML = sHTML & "  <div class='controles'>"
        sHTML = sHTML & "   <div class='btn-down min-shopping' data-row='" & ItemCode & "'  data-operacion='min'><i class='fa fa-minus' aria-hidden='true'></i></div>"
        sHTML = sHTML & "   <input type='text' class='total-row cantidad" & ItemCode & "' name='cantidad' id='#cantidad" & ItemCode & "'  value='1'></input>"
        sHTML = sHTML & "   <div class='btn-down max-shopping' data-row='" & ItemCode & "' data-operacion='plus'>"
        sHTML = sHTML & "     <i class='fa fa-plus' aria-hidden='true'></i>"
        sHTML = sHTML & "   </div>"
        sHTML = sHTML & "  </div>"
        '  sHTML = sHTML & "  <div><a class='btn btn-general-2' id='#btnAgregar" & ItemCode.Replace(" ", "") & "' onclick=" & Comillas & "PageMethods.CargarCarrito(document.getElementById('#cantidad-" & ItemCode & "').value, '" & ItemCode.ToUpper & "', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','',event); } function onError(result) {}" & Comillas & ">agregar</a></div>"
        sHTML = sHTML & "  <div><a class='btn btn-general-2' id='#btnAgregar" & ItemCode.Replace(" ", "") & "' onclick=" & Comillas & "fnClick('" & "#cantidad" & ItemCode & "', '" & ItemCode & "');" & Comillas & ">agregar</a></div>"
        sHTML = sHTML & " </div>"

        Return sHTML
    End Function

    Public Function fnControlesPorCaja(ItemCode As String) As String
        Dim sHTML As String = ""
        Dim ssql As String = ""
        ssql = fnObtenerQuery("GetInfoCaja")

        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", ItemCode)
            Dim dtPorCaja As New DataTable
            dtPorCaja = fnEjecutarConsultaSAP(ssql)
            If dtPorCaja.Rows.Count > 0 Then
                If CInt(dtPorCaja.Rows(0)(0)) > 0 Then
                    ''Se compra por caja también, generamos el HTML para pintar los radios
                    sHTML = sHTML & "  <input type='radio'  id='pieza" & ItemCode & "' name='tipoProducto" & ItemCode & "' value='1' checked>"
                    sHTML = sHTML & "    <label for='pieza" & ItemCode & "' class='descripcion' style='font-weight:1;'>Pieza</label>"

                    sHTML = sHTML & "  <input type='radio'   id='caja" & ItemCode & "' name='tipoProducto" & ItemCode & "' value='" & CInt(dtPorCaja.Rows(0)(1)) & "'>"
                    sHTML = sHTML & "  <label for='caja" & ItemCode & "' class='descripcion' style='font-weight:1;'>Caja con " & CInt(dtPorCaja.Rows(0)(1)) & "</label>"

                Else
                    sHTML = ""
                End If
            End If
        End If




        Return sHTML
    End Function

    Public Function fnCreaFichaProductoPLUS(itemCode As String, RutaServer As String, slpCode As String, ListaPrecios As Short, UserB2B As String, UserB2C As String, Cliente As String)

        fnLog("CreaFicha", itemCode & "-" & slpCode & "-" & ListaPrecios & "-" & UserB2B & "-" & UserB2C & "-" & Cliente)
        Dim sHtmlBanner As String = ""
        Dim sImagen As String = "ImagenPal"
        Dim dtProductos As New DataTable
        Dim dPrecioActual As Double = 0
        Dim ssql As String = ""
        ssql = fnObtenerQuery("Info-Producto")
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtProductos = fnEjecutarConsultaSAP(ssql)
        Dim i As Int16 = 0
        Dim objDatos As New Cls_Funciones
        Dim sCaracterMoneda As String = ""
        ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
        Dim dtCaracter As New DataTable
        dtCaracter = fnEjecutarConsulta(ssql)
        If dtCaracter.Rows.Count > 0 Then
            sCaracterMoneda = dtCaracter.Rows(0)(0)
        End If

        Dim sCliente As String = ""
        Dim sStyle As String = ""
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            sCliente = CStr(dtclienteSeg.Rows(0)(0)).ToUpper
        End If

        '''Evaluamos el stock
        'Dim existencia As Double = 0
        '''Existencia 
        'ssql = objDatos.fnObtenerQuery("ExistenciaSAPHijos")
        'Dim dtExistencia As New DataTable
        'ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        'objDatos.fnLog("existencia", ssql.Replace("'", ""))
        'If ssql <> "" Then
        '    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
        '    If dtExistencia.Rows.Count > 0 Then
        '        existencia = CDbl(dtExistencia.Rows(0)(0))
        '        If existencia = 0 Then
        '            sStyle = "style='-webkit-filter: grayscale(100%); /* Safari 6.0 - 9.0 */  filter: grayscale(100%);'"
        '        End If
        '    End If
        'End If


        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)


        sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
        sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
        sHtmlBanner = sHtmlBanner & " <div class='preview'>"
        sHtmlBanner = sHtmlBanner & "  <div class='img'>"
        sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive' " & sStyle & ">"

        If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
        End If

        Dim sPintaPrev As String = "SI"
        Dim sPintaFav As String = "SI"
        Dim sPintaCompra As String = "SI"

        ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
        Dim dtPintaCat As New DataTable
        dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
        If dtPintaCat.Rows.Count > 0 Then
            sPintaPrev = dtPintaCat.Rows(0)("Interior")
            sPintaFav = dtPintaCat.Rows(0)("Favorito")
            sPintaCompra = dtPintaCat.Rows(0)("Comprar")
        End If

        If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
        Else
            sHtmlBanner = sHtmlBanner & "     <div>"
        End If

        If sPintaCompra = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
        End If
        If sPintaFav = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
        End If
        If sPintaCompra = "SI" Then
            sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
        End If
        sHtmlBanner = sHtmlBanner & "     </div>"
        sHtmlBanner = sHtmlBanner & "  </div>"

        If CInt(slpCode) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
        Else
            'If UserB2C = "" Then
            If UserB2C = "" And UserB2B = "" Then
                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            Else
                If UserB2C <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                End If

            End If

            ' Else
            '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            'End If

        End If
        If Cliente <> "" Then
            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
        End If

        Dim sMoneda As String = ""
        sMoneda = fnObtenerMoneda(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
        Dim fDescuento As Double = 0
        fDescuento = fnDesctoB2C(dtProductos.Rows(i)("ItemCode"))

        If fDescuento > 0 Then
            'Precio original
            sHtmlBanner = sHtmlBanner & "   <span class='precio-original2'>" & sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda & "</span>"
            'Precio tras descto

            sHtmlBanner = sHtmlBanner & "   <span class='precio-descto'>" & sCaracterMoneda & " " & (dPrecioActual * (1 - (fDescuento / 100))).ToString("###,###,###.#0") & " " & sMoneda & "</span>"
        Else
            If dPrecioActual > 0 Then
                sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda & "</span>"
            End If

        End If

        ''Fichas colores disponibles
        sHtmlBanner = sHtmlBanner & fnCargaFichasColoresMini(itemCode, RutaServer)

        sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
        ''img/home/producto-1.png
        For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                    sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                Else
                    sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                End If
            Else
                If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then

                    If CInt(slpCode) <> 0 Then

                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                    Else
                        'If UserB2C = "" Then
                        If UserB2C = "" And UserB2B = "" Then
                            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                        Else
                            If UserB2C <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                            End If

                        End If

                        ' Else
                        '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                        'End If

                    End If
                    If Cliente <> "" Then
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), ListaPrecios)
                    End If

                    ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))

                End If
                If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
                    Dim iband As Int16 = 0
                    sImagen = "images/no-image.png"

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "PLUS-1.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "PLUS-1.jpg"
                    End If

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "PLUS.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "PLUS.jpg"
                    End If



                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "PLUS-2.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "PLUS-2.jpg"
                    End If

                    If File.Exists(RutaServer & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "PLUS-3.jpg") And iband = 0 Then
                        iband = 1
                        sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "PLUS-3.jpg"
                    End If
                    If iband = 0 Then
                        Try
                            ' sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                            If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value And iband = 0 Then
                                sImagen = "images/no-image.png"
                            Else
                                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" And iband = 0 Then
                                    sImagen = "images/no-image.png"
                                Else
                                    sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                                End If

                            End If

                        Catch ex As Exception

                        End Try
                    End If

                    sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
                End If
            End If

        Next


        sHtmlBanner = sHtmlBanner & "  </a>"
        sHtmlBanner = sHtmlBanner & " </div>"

        sHtmlBanner = sHtmlBanner & "  </a>"

        sHtmlBanner = sHtmlBanner & "</div>"

        Return sHtmlBanner
    End Function
    Public Function fnCargaFichasColoresMini(itemcode As String, RutaServer As String)

        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim iband As Int16 = 0
        Dim ItemCodeFoto As String = ""
        Dim ssql As String = ""
        Dim dtColores As New DataTable
        Exit Function
        ssql = fnObtenerQuery("Color")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", itemcode)
            dtColores = fnEjecutarConsultaSAP(ssql)
        End If
        ' Dim sStyleItem As String = "style='position: relative;  width: 50px;  height: 50px; overflow:hidden;'"
        Dim sStyleItem As String = " class='mini-item-color'"
        sHtmlBanner = sHtmlBanner & "<div class='select-colores-mini'>"
        If dtColores.Rows.Count > 0 Then

            For i = 0 To dtColores.Rows.Count - 1 Step 1
                Dim sItemCode As String()
                ssql = fnObtenerQuery("ItemColorHijoFoto")
                Dim dtItemColorHijo As New DataTable

                If ssql <> "" Then

                    ssql = ssql.Replace("[%0]", itemcode).Replace("[%1]", dtColores.Rows(i)(0))
                    fnLog("Fichas Colores", ssql.Replace("'", ""))
                    dtItemColorHijo = fnEjecutarConsultaSAP(ssql)
                    If dtItemColorHijo.Rows.Count > 0 Then
                        Dim ItemCodeHijo As String = ""
                        ItemCodeHijo = dtItemColorHijo.Rows(0)(0)
                        sItemCode = ItemCodeHijo.Split("-")
                        If sItemCode.Count = 4 Then
                            ItemCodeFoto = sItemCode(0) & "-" & sItemCode(1) & sItemCode(3)

                            fnLog("Ficha colores existe", RutaServer & "\images\products\" & ItemCodeFoto & "-4.jpg")
                            If File.Exists(RutaServer & "\images\products\" & ItemCodeFoto & "-4.jpg") Then
                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                sHtmlBanner = sHtmlBanner & "</div>"
                            Else
                                fnLog("Ficha colores existe", RutaServer & "\images\products\" & ItemCodeFoto & "-3.jpg")
                                If File.Exists(RutaServer & "\images\products\" & ItemCodeFoto & "-3.jpg") Then
                                    sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                    sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-3.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                    sHtmlBanner = sHtmlBanner & "</div>"
                                Else

                                    fnLog("Ficha colores existe sin 3", RutaServer & "\images\products\" & ItemCodeFoto & "-2.jpg")
                                    If File.Exists(RutaServer & "\images\products\" & ItemCodeFoto & "-2.jpg") Then
                                        sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                        sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-2.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                        sHtmlBanner = sHtmlBanner & "</div>"
                                    Else
                                        fnLog("Ficha colores existe sin 2", RutaServer & "\images\products\" & ItemCodeFoto & "-1.jpg")
                                        If File.Exists(RutaServer & "\images\products\" & ItemCodeFoto & "-1.jpg") Then
                                            sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                            sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-1.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                            sHtmlBanner = sHtmlBanner & "</div>"
                                        Else
                                            If File.Exists(RutaServer & "\images\products\" & itemcode & "-4.jpg") Then
                                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                                sHtmlBanner = sHtmlBanner & "<img id='clip-1" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                                sHtmlBanner = sHtmlBanner & "</div>"
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                        Else
                            If File.Exists(RutaServer & "\images\products\" & itemcode & "-4.jpg") Then
                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                sHtmlBanner = sHtmlBanner & "<img id='clip-1" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                sHtmlBanner = sHtmlBanner & "</div>"
                            End If
                        End If
                    End If
                End If




            Next

        Else
            If File.Exists(RutaServer & "\images\products\" & itemcode & "-4.jpg") Then
                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                sHtmlBanner = sHtmlBanner & "<img id='clip-" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                sHtmlBanner = sHtmlBanner & "</div>"
            End If

        End If
        sHtmlBanner = sHtmlBanner & "</div>"
        'pnlFichasColor.Visible = True
        'Dim literalImagen = New LiteralControl(sHtmlBanner)
        'pnlFichasColor.Controls.Clear()
        'pnlFichasColor.Controls.Add(literalImagen)
        'sHtmlEncabezado = ""
        'sHtmlBanner = ""
        Return sHtmlBanner
    End Function
    Public Function fnObtenerMoneda(ItemCode As String, ListaPrecios As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & ListaPrecios & "'")
        dtMonedas = fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""

        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
            If dtMonedas.Rows.Count > 1 Then
                ''El articulo se puede vender en mas de una moneda
                ''Llenamos y mostramos combo de moneda



            End If
        End If

        Return sMoneda
    End Function

    Public Function fnPestañaKits(ItemCode As String)
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim z As Int16 = 999
        Dim ssql As String = ""

        ssql = fnObtenerQuery("ComponentesKit")
        ssql = ssql.Replace("[%0]", ItemCode)
        Dim dtEsKit As New DataTable
        dtEsKit = fnEjecutarConsultaSAP(ssql)

        If dtEsKit.Rows.Count > 0 Then
            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-12 no-padding info-producto-int'>"
            sHtmlEncabezado = sHtmlEncabezado & " <div class='Caracteristicas'> "
            'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'> "
            sHtmlEncabezado = sHtmlEncabezado & "  <div class='filtos-catalogo panel-group tymnyce' id='accordioninfo' role='tablist' aria-multiselectable='true'> "
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='panel'> "
            sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-heading' role='tab' id='heading" & z & "'> "
            sHtmlEncabezado = sHtmlEncabezado & "     <h4 class='categoria'> "
            sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thir" & z & "' aria-expanded='true' aria-controls='thir" & z & "'> Componentes </a>"
            sHtmlEncabezado = sHtmlEncabezado & "     </h4> </div>"
            sHtmlEncabezado = sHtmlEncabezado & "   <div id='thir" & z & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='thir" & z & "'>"
            sHtmlBanner = ""
            sHtmlBanner = sHtmlBanner & "<div class='panel-body'>"

            Dim dtDatosPestaña As New DataTable
            Dim sContenido As String = ""
            dtDatosPestaña = fnEjecutarConsultaSAP(ssql)



            If dtDatosPestaña.Rows.Count > 0 Then
                sHtmlBanner = sHtmlBanner & "<table class='table table-striped table-bordered' style='width:100%' id='componentes'>" 'class='table table-sm'
                sHtmlBanner = sHtmlBanner & "<thead><tr>"
                For col = 0 To dtDatosPestaña.Columns.Count - 1 Step 1
                    sHtmlBanner = sHtmlBanner & "<th>" & dtDatosPestaña.Columns(col).ColumnName & "</th>"
                Next
                sHtmlBanner = sHtmlBanner & "</tr></thead><tbody>"


                For atr = 0 To dtDatosPestaña.Rows.Count - 1 Step 1
                    sHtmlBanner = sHtmlBanner & "<tr>"
                    For colRow = 0 To dtDatosPestaña.Columns.Count - 1 Step 1

                        If dtDatosPestaña.Rows(atr)(colRow) Is DBNull.Value Then
                            sContenido = ""

                        Else
                            sContenido = dtDatosPestaña.Rows(atr)(colRow)

                        End If
                        sHtmlBanner = sHtmlBanner & "<td>" & sContenido & "</td>"
                    Next
                    sHtmlBanner = sHtmlBanner & "</tr>"
                Next


                sHtmlBanner = sHtmlBanner & "</tbody></table>"
            Else
                If dtDatosPestaña.Rows.Count = 1 Then
                    sHtmlBanner = sHtmlBanner & dtDatosPestaña.Rows(0)(0) & " </br>"
                End If
            End If

            sHtmlBanner = sHtmlBanner & "</div>"
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        End If


        Return sHtmlEncabezado
    End Function

    Public Function fnDesctoB2C(ItemCode As String) As Double
        Dim fDesc As Double = 0
        Dim ssql As String = ""
        Dim dtDescArticulo As New DataTable
        Dim dtDescNivel As New DataTable
        fnLog("Fn descto - Leyendo:", ItemCode)

        ''Validamos si primero hay un descuento directo x artículo
        ssql = "SELECT ISNULL(cfDescto,0) FROM config.DesctosArticulo WHERE cvItemCode=" & "'" & ItemCode & "' and getdate() between cdFechaIni and cdFechaFin "
        dtDescArticulo = fnEjecutarConsulta(ssql)
        If dtDescArticulo.Rows.Count > 0 Then
            fDesc = dtDescArticulo.Rows(0)(0)
            fnLog("Fn descto - desc x articulo:", ItemCode & "-" & fDesc)
        Else
            ''Revisamos si por alguno de los niveles
            ssql = fnObtenerQuery("Info-Producto")
            ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
            Dim dtInfoArticulo As New DataTable
            dtInfoArticulo = fnEjecutarConsultaSAP(ssql)

            If dtInfoArticulo.Rows.Count > 0 Then
                Dim ssqlNiv As String
                ssqlNiv = "SELECT * FROM config.NivelesDesctos "
                Dim dtNiveles As New DataTable
                dtNiveles = fnEjecutarConsulta(ssqlNiv)
                For i = 0 To dtNiveles.Rows.Count - 1 Step 1
                    If dtInfoArticulo.Rows(0)(dtNiveles.Rows(i)("cvNivel")) = dtNiveles.Rows(i)("cvValor") Then
                        fDesc = dtNiveles.Rows(i)("cfDescto")
                        fnLog("Fn descto - desc x nivel:", dtNiveles.Rows(i)("cvNivel") & "-" & dtNiveles.Rows(i)("cvValor") & "-" & fDesc)
                    End If
                Next

                If dtDescNivel.Rows.Count > 0 Then

                End If
            End If

        End If


        Return fDesc
    End Function


    Public Function conexionSAPHANA()

        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = fnEjecutarConsulta(ssql)

        Dim _strServerName As String = dtConfSAP.Rows(0)("cvServidorSQL")
        Const _strLoginName As String = "SYSTEM"
        Dim _strPassword As String = dtConfSAP.Rows(0)("cvPwdSQL") 'dtConf.Rows(0)("fcPassSAP") 'S!0nH@n@
        Dim strConnectionString As String = String.Empty
        'Does NOT require to create an odbc connection in windows system
        If IntPtr.Size = 8 Then
            strConnectionString = String.Concat(strConnectionString, "Driver={HDBODBC};")
        Else
            strConnectionString = String.Concat(strConnectionString, "Driver={HDBODBC32};")
        End If
        strConnectionString = String.Concat(strConnectionString, "ServerNode=", _strServerName, ";")
        strConnectionString = String.Concat(strConnectionString, "UID=", _strLoginName, ";")
        strConnectionString = String.Concat(strConnectionString, "PWD=", _strPassword, ";")

        Dim hanaConn As New OdbcConnection(strConnectionString.ToString())
        Try
            hanaConn.Open()
            fnLog("conexion HANA:", "Si conecta")
        Catch ex As Exception
            fnLog("conexion HANA:", ex.Message.Replace("'", ""))
        Finally
        End Try


        Return hanaConn
    End Function
    Public Function fnEjecutarConsultaHANA(ByVal ssql As String) As DataTable

        Dim dtClientes As New DataTable
        Dim da As OdbcDataAdapter
        Dim cmd As OdbcCommand

        Dim ssqlConf = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = fnEjecutarConsulta(ssqlConf)

        Try
            If dtConfSAP.Rows.Count > 0 Then
                ssql = ssql.Replace("#BDSAP#", dtConfSAP.Rows(0)("cvBD"))
            End If
            fnLog("SAP consulta", "Antes de conexion query config:" & ssqlConf.Replace("'", ""))
            cmd = New OdbcCommand(ssql, conexionSAPHANA)
            cmd.CommandTimeout = 90000
            da = New OdbcDataAdapter(cmd)
            da.Fill(dtClientes)
            cmd.Connection.Close()
        Catch ex As Exception
            fnLog("ConsultaHANa ex", ex.Message.Replace("'", "") & " " & ssql.Replace("'", ""))
        End Try

        Return dtClientes
    End Function

    Public Function fnCalculaDescuentoDelta(importe As Double) As DataTable
        Dim dtDescuento As New DataTable
        Dim ssql As String = ""
        ssql = "DECLARE @MontoCompra as float " _
            & " SET @MontoCompra= " & importe _
            & " SELECT TOP 1 T0.U_Descuento as DescActual,ISNULL((SELECT TOP 1 U_Descuento FROM [@ecom_descuentos] where u_descuento > " _
            & " T0.U_Descuento order by u_Descuento),0) as 'SigDescto', '' + CAST( ((  round(T0.U_MontoFinal -@MontoCompra,2) ) ) " _
            & " as varchar) as Leyenda from [@ecom_descuentos] T0 where u_montoFinal >= @MontoCompra order by u_montofinal "

        dtDescuento = fnEjecutarConsultaSAP(ssql)

        Return dtDescuento
    End Function

End Class

Imports System.Net.Http
Imports System.Threading.Tasks
Imports Microsoft.VisualBasic
Imports Newtonsoft.Json
Imports CotizacionRequest
Imports System.IO
Imports System.Data

Public Class Cls_Paqueteria
    Private ReadOnly url = "http://webserver.pegaduro.com.mx/api-paqueteria/api"
    Dim client As New HttpClient()

    Public objDatos As New Cls_Funciones
    Public Function FnObtenerToken() As String
        '{.User = "BOSSFOOD", .Password = "b@s3f00d", .Modo = "QA", .Paqueteria = "RedPack"} 'Paqueteria(UPS,RedPack); Modo(QA,PROD)
        Dim Usuario As New UserInfo() With
                {.User = "ALDO", .Password = "@ld02020", .Modo = "PROD", .Paqueteria = "RedPack"} 'Paqueteria(UPS,RedPack); Modo(QA,PROD)
        ' {.User = "BOSSFOOD", .Password = "b@s3f00d", .Modo = "QA", .Paqueteria = "RedPack"} 'Paqueteria(UPS,RedPack); Modo(QA,PROD)
        Dim Content As New StringContent(JsonConvert.SerializeObject(Usuario), Encoding.UTF8, "application/json")
        Dim _URL As String = url & "/Access"
        Dim RespuestaHttp = client.PostAsync(_URL, Content).Result
        If RespuestaHttp.IsSuccessStatusCode Then
            Dim UsuarioToken = JsonConvert.DeserializeObject(Of UserToken)(RespuestaHttp.Content.ReadAsStringAsync().Result)
            Return UsuarioToken.BearerToken
        End If
        Return ""
    End Function

    Public Function FnCreaModeloCotizacion(destino As Address, ListaPaquetes As List(Of Paquete)) As CotizacionRequest

        Dim objdatos As New Cls_Funciones

        objdatos.fnLog("Entra a modelo cotizacion", "")

        Dim ssql As String = ""
        Dim Origen As New Address

        ssql = "SELECT cvCalle,cvColonia,cvCiudad,cvNumExt,cvNumInt,cvPais,cvEstado, cvCP from config.DatosCliente"
        Dim dtDatosOrigen As New DataTable
        dtDatosOrigen = objdatos.fnEjecutarConsulta(ssql)

        If dtDatosOrigen.Rows.Count > 0 Then
            Origen.addressLine = dtDatosOrigen.Rows(0)("cvCalle")
            Origen.city = dtDatosOrigen.Rows(0)("cvCiudad")
            Origen.colonia = dtDatosOrigen.Rows(0)("cvColonia")
            Origen.countryCode = dtDatosOrigen.Rows(0)("cvPais")
            Origen.estado = dtDatosOrigen.Rows(0)("cvEstado")
            '  Origen.estado = "CDMX"
            Origen.numInt = dtDatosOrigen.Rows(0)("cvNumInt")
            Origen.numExt = dtDatosOrigen.Rows(0)("cvNumExt")
            '  Origen.postalCode = "01900"
            Origen.postalCode = dtDatosOrigen.Rows(0)("cvCP")
            Origen.stateProvinceCode = ""
        Else
            Origen.addressLine = "Anillo Periferico"
            Origen.city = "Ciudad de México"
            Origen.colonia = "Álvaro Obregón"
            Origen.countryCode = "MX"
            Origen.estado = "JAL"
            '  Origen.estado = "CDMX"
            Origen.numInt = ""
            Origen.numExt = "3720"
            '  Origen.postalCode = "01900"
            Origen.postalCode = "45134"
            Origen.stateProvinceCode = ""
        End If



        Dim Cotizacion As New CotizacionRequest
        Dim _Origen = New Origen
        _Origen.address = Origen
        Cotizacion.origen = _Origen

        objdatos.fnLog("modelo cotizacion", "Asigna origen")

        Try
            Dim _destino = New Destino
            _destino.address = destino
            Cotizacion.destino = _destino

        Catch ex As Exception
            objdatos.fnLog("Destino", ex.Message.Replace("'", ""))
        End Try
        Try
            Cotizacion.paquetes = ListaPaquetes
        Catch ex As Exception
            objdatos.fnLog("Paquetes", ex.Message.Replace("'", ""))
        End Try


        Return Cotizacion



        'Dim LstPaquetes = New List(Of Paquete)
        'LstPaquetes = ListaPaquetes

        'LstPaquetes.Add(
        '    New Paquete With
        '    {.description = "Descripcion",
        '    .dimensions = New Dimensions With {.height = "20", .length = "20", .width = "20", .unitOfMeasurement = New UnitOfMeasurement With {.code = "CM"}},
        '    .packageWeight = New PackageWeight With {.unitOfMeasurement = New UnitOfMeasurement With {.code = "KGS"}, .weight = "2"},
        '    .packagingType = New PackagingType With {.code = "02", .description = "Paquete"}
        '    })


        'packagingType
        'Valid values: 00 = UNKNOWN 01 = UPS Letter 02 =Package 3 = Tube 04 = Pak 21 = Express Box 24 =25KG Box 25 = 10KG Box 30 = Pallet 2a = Small Express
        'Box 2b = Medium Express Box 2c = Large Express Box.
        'For FRS rating requests the only valid value Is customer supplied packaging “02”.

    End Function

    Public Function FnCotizar(cotizacion As CotizacionRequest, bearerToken As String) As CotizacionResponse
        Dim objdatos As New Cls_Funciones
        Dim Respuesta As New CotizacionResponse
        Try
            client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", bearerToken)

            Dim Content As New StringContent(JsonConvert.SerializeObject(cotizacion), Encoding.UTF8, "application/json")

            Dim _URL As String = url & "/Cotizacion"
            Dim RespuestaHttp = client.PostAsync(_URL, Content).Result

            '      Dim RespuestaHttp As HttpResponseMessage = client.PostAsync(url + "/Cotizacion", Content)


            objdatos.fnLog("Json", JsonConvert.SerializeObject(cotizacion).Replace("'", ""))
            If RespuestaHttp.IsSuccessStatusCode Then
                objdatos.fnLog("IsSuccessStatusCode", "true")
                Dim _Respuesta = RespuestaHttp.Content.ReadAsStringAsync().Result

                objdatos.fnLog("Json2", _Respuesta.Replace("'", ""))
                Respuesta = JsonConvert.DeserializeObject(Of CotizacionResponse)(_Respuesta)
                Return Respuesta
            End If

            'If RespuestaHttp.iss Then
            '    Dim _Respuesta = RespuestaHttp.Content.ReadAsStringAsync()
            '    Dim Respuesta = JsonConvert.DeserializeObject(Of CotizacionResponse)(_Respuesta)
            '    Return Respuesta
            'End If
        Catch ex As Exception
            objdatos.fnLog("ex paqueteria", ex.Message.Replace("'", ""))
        End Try
        Return Respuesta
    End Function

    Public Function FnRespuestaCotizacion(respuestaCotizacion As CotizacionResponse) As List(Of Cotizacion_Response)
        Dim sMensaje As New Cotizacion_Response
        Dim Cotizaciones As New List(Of Cotizacion_Response)

        ''Metemos uba por default, que sea recoger en almacen
        Dim cotizacionRespDef As New Cotizacion_Response
        cotizacionRespDef.codigo = "Acordar"
        cotizacionRespDef.descripcion = "Acordar entrega con centro de distribución"
        cotizacionRespDef.monto = 0
        cotizacionRespDef.moneda = "MXN"
        Cotizaciones.Add(cotizacionRespDef)

        ''Ahora revisamos lo que responde la paquetería
        If respuestaCotizacion IsNot Nothing Then
            '   Console.WriteLine(respuestaCotizacion.mensaje)
            For Each _Cotizacion As Cotizacion_Response In respuestaCotizacion.cotizaciones
                '  sMensaje = "Codigo:" & _Cotizacion.codigo & "-Desc:" & _Cotizacion.descripcion & "-Monto:" & _Cotizacion.monto & "-" & _Cotizacion.moneda
                ''Revisamos si la descripcion debe llevar una máscara o mas detalle
                _Cotizacion.descripcion = fnConceptoPaqueteria(_Cotizacion.descripcion)
                Cotizaciones.Add(_Cotizacion)
            Next

        Else

            '   sMensaje = "Error en la consulta"
        End If
        Return Cotizaciones
    End Function

    Public Function fnConceptoPaqueteria(concepto As String)
        Dim sConcepto As String = concepto
        Dim ssql As String
        Dim objdatos As New Cls_Funciones
        ssql = "SELECt cvConceptoDesc FROM config.ConceptosPaqueteria where cvConcepto=" & "'" & concepto & "'"
        Dim dtConcepto As New DataTable
        dtConcepto = objdatos.fnejecutarconsulta(ssql)
        If dtConcepto.Rows.Count > 0 Then
            sConcepto = dtConcepto.Rows(0)(0)
        End If


        Return sConcepto
    End Function
    Public Function FnRespuestaGuia(guiaRespuesta As GuiaResponse, NumPedido As String, GuiaSolicitada As String, Concepto As String) As List(Of ResultadoPaquete)
        Dim GuiasPaquetes As New List(Of ResultadoPaquete)
        If guiaRespuesta IsNot Nothing Then
            ' Console.WriteLine(guiaRespuesta.mensaje)
            ' Console.WriteLine($"NoGuia:{guiaRespuesta.numeroGuia} - Peso Total:{guiaRespuesta.pesoTotal} Kgs. - Monto:{guiaRespuesta.totalCargos:C2} - {guiaRespuesta.moneda}")
            Dim iCons As Int32 = 1
            Dim ssql As String = ""
            For Each _Paquete As ResultadoPaquete In guiaRespuesta.resultadoPaquetes
                GuiasPaquetes.Add(_Paquete)
                Dim Ruta = "c:\inetpub\wwwroot\ecommerce\guias\" & iCons & DateTime.Now.ToString("yyyyMMdd_HH_mm_ss") & _Paquete.extension
                File.WriteAllBytes(Ruta, Convert.FromBase64String(_Paquete.archivo))

                ''---Insertamos en la tabla, los archivos generados para el pedido

                ssql = "INSERT INTO config.Guias_Paqueteria(cvPedido,cvArchivo,cvGuia,cvGuiaPaquete) VALUES(" _
                    & "'" & NumPedido & "'," _
                    & "'" & Ruta & "'," _
                    & "'" & GuiaSolicitada & "'," _
                    & "'" & _Paquete.numero & "')"
                objDatos.fnEjecutarInsert(ssql)

                'Console.WriteLine($"NoGuiaPaquete: {_Paquete.numero}")
                'Console.WriteLine($"ImagenBase64: {_Paquete.imagen}")
                'Console.WriteLine($"Se creo el paquete {Ruta}")
                'Console.ReadLine()
                iCons += 1
            Next
            ''Aumentamos el folio del consecutivo
            ssql = "UPDATE config.Folios_Paqueteria set ciFolioSiguiente = ciFolioSiguiente + 1 where ciConcepto= " & "'" & Concepto & "'"
            objDatos.fnEjecutarInsert(ssql)
        Else
            Console.WriteLine("Error en la consulta")
        End If
        Return GuiasPaquetes
    End Function

    Public Function FnCrearGuia(guias As GuiaRequest, bearerToken As String) As GuiaResponse
        Try
            client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Bearer", bearerToken)

            Dim Content As New StringContent(JsonConvert.SerializeObject(guias), Encoding.UTF8, "application/json")
            Dim _URL As String = url & "/Generar"
            objDatos.fnLog("Crear guia content:", JsonConvert.SerializeObject(guias).Replace("'", ""))
            Dim RespuestaHttp = client.PostAsync(_URL, Content).Result
            If RespuestaHttp.IsSuccessStatusCode Then
                Dim _Respuesta = RespuestaHttp.Content.ReadAsStringAsync().Result
                Dim Respuesta = JsonConvert.DeserializeObject(Of GuiaResponse)(_Respuesta)
                If Respuesta.error Then
                    ''error, guardar en tabla temporal para enviar despues. Hay que guardar el Json

                End If
                Return Respuesta
            Else
                objDatos.fnLog("Crear guia content: NO ", RespuestaHttp.Content.ReadAsStringAsync().Result.Replace("'", ""))
            End If
        Catch ex As Exception
            objDatos.fnLog("Crear guias ex", ex.Message.Replace("'", ""))
        End Try
        Return Nothing
    End Function

    Public Function FnCrearModeloGuia(cotizacion As CotizacionRequest, codigo As String) As GuiaRequest
        Try
            ''Obtenemos la descripcion del codigo
            Dim sDescripcion As String = ""
            Dim ssql As String = ""

            ssql = "SELECT cvConcepto FROM config.ConceptosPaqueteria WHERE ciIdConcepto=" & CInt(codigo)
            Dim dtDescripcion As New DataTable
            dtDescripcion = objDatos.fnEjecutarConsulta(ssql)
            If dtDescripcion.Rows.Count > 0 Then
                sDescripcion = dtDescripcion.Rows(0)(0)
            End If

            Dim Guia As New GuiaRequest With {
                .descripcion = sDescripcion,
                .destino = cotizacion.destino,
                .origen = cotizacion.origen,
                .paquetes = cotizacion.paquetes,
                .guia = cotizacion.guia,
                .servicio = New Servicio With {.code = codigo, .description = sDescripcion}
            }
            Return Guia
        Catch ex As Exception
            objDatos.fnLog("Modelo Guia ex", ex.Message.Replace("'", ""))
            Return Nothing
        End Try
    End Function

    Public Class UserInfo
        Public Property User As String
        Public Property Password As String
        Public Property Modo As String
        Public Property Paqueteria As String
    End Class
    Public Class UserToken
        Public Property BearerToken As String
        Public Property Expiracion As DateTime
    End Class
End Class

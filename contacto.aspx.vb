Imports System.Data
Partial Class contacto
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub contacto_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("Page") = "contacto.aspx"


        Session("correoInfo") = ""

        ssql = "SELECT * from config.DatosContacto"
        Dim dtDatos As New DataTable
        dtDatos = objDatos.fnEjecutarConsulta(ssql)
        Dim sHTMLEncabezado As String = ""
        For i = 0 To dtDatos.Rows.Count - 1 Step 1

            sHTMLEncabezado = sHTMLEncabezado & "<div class='col-xs-12'>"
            If dtDatos.Rows(i)("cvTipoDato") = "Direccion" Then
                sHTMLEncabezado = sHTMLEncabezado & "<div class='back-icon dir'>" & dtDatos.Rows(i)("cvInformacion") & "</div>"
            End If
            If dtDatos.Rows(i)("cvTipoDato") = "Telefono" Then
                sHTMLEncabezado = sHTMLEncabezado & "<div class='back-icon tel'>" & dtDatos.Rows(i)("cvInformacion") & "</div>"
            End If
            If dtDatos.Rows(i)("cvTipoDato") = "TelefonoW" Then
                sHTMLEncabezado = sHTMLEncabezado & "<div class='back-icon was'>" & dtDatos.Rows(i)("cvInformacion") & "</div>"
            End If
            If dtDatos.Rows(i)("cvTipoDato") = "Correo" Then
                Session("correoInfo") = dtDatos.Rows(i)("cvInformacion")
                sHTMLEncabezado = sHTMLEncabezado & "<div class='back-icon mail'>" & dtDatos.Rows(i)("cvInformacion") & "</div>"
            End If
            sHTMLEncabezado = sHTMLEncabezado & "</div>"
        Next

        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlContacto.Controls.Clear()
        pnlContacto.Controls.Add(literal)

        ''Cargamos las coordenadas
        ssql = "SELECT ISNULL(cvUbicacion,'') from config.Parametrizaciones "
        Dim dtubicacion As New DataTable
        dtubicacion = objDatos.fnEjecutarConsulta(ssql)
        If dtubicacion.Rows.Count > 0 Then
            Dim script As String = ""
            script = script & "<script type='text/javascript'>"
            script = script & " var image = 'images/marker.png'; "
            script = script & " var map; "
            script = script & " var markers = [];"
            script = script & " function initMap() {"
            script = script & " var mexico = {" & dtubicacion.Rows(0)(0) & "};"
            script = script & " map = new google.maps.Map(document.getElementById('map'), {"
            script = script & "  zoom: 17,"
            script = script & " center: mexico,"
            script = script & " mapTypeId: 'roadmap',"
            script = script & "  styles:[{'featureType':'water','elementType':'geometry','stylers':[{'color':'#e9e9e9'},{'lightness':17}]},{'featureType':'landscape','elementType':'geometry','stylers':[{'color':'#f5f5f5'},{'lightness':20}]},{'featureType':'road.highway','elementType':'geometry.fill','stylers':[{'color':'#ffffff'},{'lightness':17}]},{'featureType':'road.highway','elementType':'geometry.stroke','stylers':[{'color':'#ffffff'},{'lightness':29},{'weight':0.2}]},{'featureType':'road.arterial','elementType':'geometry','stylers':[{'color':'#ffffff'},{'lightness':18}]},{'featureType':'road.local','elementType':'geometry','stylers':[{'color':'#ffffff'},{'lightness':16}]},{'featureType':'poi','elementType':'geometry','stylers':[{'color':'#f5f5f5'},{'lightness':21}]},{'featureType':'poi.park','elementType':'geometry','stylers':[{'color':'#dedede'},{'lightness':21}]},{'elementType':'labels.text.stroke','stylers':[{'visibility':'on'},{'color':'#ffffff'},{'lightness':16}]},{'elementType':'labels.text.fill','stylers':[{'saturation':36},{'color':'#333333'},{'lightness':40}]},{'elementType':'labels.icon','stylers':[{'visibility':'off'}]},{'featureType':'transit','elementType':'geometry','stylers':[{'color':'#f2f2f2'},{'lightness':19}]},{'featureType':'administrative','elementType':'geometry.fill','stylers':[{'color':'#fefefe'},{'lightness':20}]},{'featureType':'administrative','elementType':'geometry.stroke','stylers':[{'color':'#fefefe'},{'lightness':17},{'weight':1.2}]}],"
            script = script & " });"
            script = script & " var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';"
            script = script & " var beachMarker = new google.maps.Marker({"
            script = script & " position: mexico,"
            script = script & " map: map,"
            script = script & " icon: image"
            script = script & " });"
            script = script & " }"
            script = script & "</script> "
            Response.Write(script)
        End If
    End Sub

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        If txtCorreo.Text = "" Then
            objDatos.Mensaje("Debe proporcionar su cuenta de correo", Me.Page)
            Exit Sub

        End If
        If txtMensaje.Text = "" Then
            objDatos.Mensaje("Índiquenos en el campo <<Mensaje>> cómo le podemos ayudar", Me.Page)
            Exit Sub
        End If
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Debe proporcionar su nombre", Me.Page)
            Exit Sub

        End If
        Try
            objDatos.fnEnviarCorreo(Session("correoInfo"), txtNombre.Text & " nos ha escrito: " & vbCrLf & txtMensaje.Text & vbCrLf & " Correo:" & txtCorreo.Text, "Contacto desde Ecommerce")
            objDatos.Mensaje("Su mensaje ha sido enviado!", Me.Page)
            txtNombre.Text = ""
            txtMensaje.Text = ""
            txtCorreo.Text = ""
        Catch ex As Exception
            objDatos.Mensaje("Ha ocurrido un problema al enviar el correo: " & ex.Message, Me.Page)
            Exit Sub
        End Try

    End Sub
End Class

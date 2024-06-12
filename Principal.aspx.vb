
Imports System.Data

Partial Class Principal
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim sHtmlEncabezado As String
        'sHtmlEncabezado = " <ul class='cd-cart-items'> "
        'Dim sCantidadEntregas As String = "220"
        'Dim sNombreMenu As String = "PEDIDOS Otros"
        'Dim sHtmlMenu As String
        'sHtmlMenu = " <div class='flex-sep'> "

        'sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 pedidos'> "
        'sHtmlMenu = sHtmlMenu & " <div class='col-xs-12 tit-pedidos'>" & sNombreMenu & " </div> "
        'sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        'sHtmlMenu = sHtmlMenu & " <img src='img/pedidos.png' class='img-responsive'> "
        'sHtmlMenu = sHtmlMenu & " <span class='data'> " & sCantidadEntregas & "</span>"
        'sHtmlMenu = sHtmlMenu & " </div>"
        'sHtmlMenu = sHtmlMenu & " </li>"

        'sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 facturas'> "
        'sHtmlMenu = sHtmlMenu & " <div class='col-xs-12 tit-pedidos'>" & sNombreMenu & " </div> "
        'sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        'sHtmlMenu = sHtmlMenu & " <img src='img/pedidos.png' class='img-responsive'> "
        'sHtmlMenu = sHtmlMenu & " <span class='data'> " & sCantidadEntregas & "</span>"
        'sHtmlMenu = sHtmlMenu & " </div>"
        'sHtmlMenu = sHtmlMenu & " </li>"

        'sHtmlMenu = sHtmlMenu & " </div>"



        'sHtmlMenu = sHtmlMenu & " <div class='flex-sep'> "

        'sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 pedidos'> "
        'sHtmlMenu = sHtmlMenu & "<a href='#'> <div class='col-xs-12 tit-pedidos'>" & sNombreMenu & " </div> "
        'sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        'sHtmlMenu = sHtmlMenu & " <img src='img/pedidos.png' class='img-responsive'> "
        'sHtmlMenu = sHtmlMenu & " <span class='data'> " & sCantidadEntregas & "</span>"
        'sHtmlMenu = sHtmlMenu & " </div></a>"
        'sHtmlMenu = sHtmlMenu & " </li>"

        'sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 facturas'> "
        'sHtmlMenu = sHtmlMenu & " <div class='col-xs-12 tit-pedidos'>" & sNombreMenu & " </div> "
        'sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        'sHtmlMenu = sHtmlMenu & " <img src='img/pedidos.png' class='img-responsive'> "
        'sHtmlMenu = sHtmlMenu & " <span class='data'> " & sCantidadEntregas & "</span>"
        'sHtmlMenu = sHtmlMenu & " </div> "
        'sHtmlMenu = sHtmlMenu & " </li>"

        'sHtmlMenu = sHtmlMenu & " </div>"



        'sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        'sHtmlEncabezado = sHtmlEncabezado & " </ul>"

        'Dim literal As New LiteralControl(sHtmlEncabezado)
        'pnlMenu.Controls.Clear()
        'pnlMenu.Controls.Add(literal)

        ' fnCargaMenu()

    End Sub


    'Public Sub fnCargaMenuB2B()
    '    ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Lateral'"
    '    Dim dtMenus As New DataTable
    '    dtMenus = objDatos.fnEjecutarConsulta(ssql)

    '    Dim sHtmlEncabezado As String
    '    sHtmlEncabezado = " <ul class='cd-cart-items'> "


    '    Dim iCuantosFila As Int16 = 0
    '    Dim sHtmlMenu As String = ""
    '    For i = 0 To dtMenus.Rows.Count - 1 Step 1

    '        If iCuantosFila = 0 Then
    '            sHtmlMenu = sHtmlMenu & " <div class='flex-sep'> "
    '        End If

    '        iCuantosFila = iCuantosFila + 1
    '        sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenuB2B(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), i, dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))
    '        If iCuantosFila = 2 Then
    '            iCuantosFila = 0
    '            sHtmlMenu = sHtmlMenu & " </div> "
    '        End If
    '    Next
    '    If iCuantosFila > 0 Then
    '        sHtmlMenu = sHtmlMenu & " </div> "
    '    End If

    '    sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
    '    sHtmlEncabezado = sHtmlEncabezado & " </ul>"

    '    Dim literal As New LiteralControl(sHtmlEncabezado)
    '    pnlMenu.Controls.Clear()
    '    pnlMenu.Controls.Add(literal)
    'End Sub
    'Public Function fnGeneraOpcionMenuB2B(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
    '    Dim sHtmlMenu As String = ""
    '    sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 " & Estilo.ToLower & "'> "
    '    sHtmlMenu = sHtmlMenu & "<div class='col-xs-12 tit-pedidos'>" & Nombre & " </div> "
    '    sHtmlMenu = sHtmlMenu & " <div class='icono '> "
    '    sHtmlMenu = sHtmlMenu & " <img src='" & imagen & "' class='img-responsive'> "
    '    sHtmlMenu = sHtmlMenu & " <span class='data'> " & valor & "</span>"
    '    sHtmlMenu = sHtmlMenu & " </div>"
    '    sHtmlMenu = sHtmlMenu & " </li>"
    '    Return sHtmlMenu
    'End Function
End Class

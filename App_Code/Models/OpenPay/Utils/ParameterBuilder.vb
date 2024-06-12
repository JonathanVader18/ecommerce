Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web

Namespace Openpay.Utils
    Friend Module ParameterBuilder
        Function ApplyParameterToUrl(ByVal url As String, ByVal argument As String, ByVal value As String) As String
            Dim token = "&"
            If Not url.Contains("?") Then token = "?"
            Return String.Format("{0}{1}{2}={3}", url, token, argument, HttpUtility.UrlEncode(value))
        End Function
    End Module
End Namespace


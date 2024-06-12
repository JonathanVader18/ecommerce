Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class OpenpayResourceObject
        Inherits JsonObject

        <JsonProperty(PropertyName:="id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Id As String
    End Class
End Namespace


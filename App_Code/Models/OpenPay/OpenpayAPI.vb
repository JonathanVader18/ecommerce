Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay
    Public Class OpenpayAPI
        Public Property CustomerService As CustomerService
        'Public Property CardService As CardService
        'Public Property BankAccountService As BankAccountService
        Public Property ChargeService As ChargeService
        'Public Property PayoutReportService As PayoutReportService
        'Public Property TransferService As TransferService
        'Public Property PayoutService As PayoutService
        'Public Property FeeService As FeeService
        'Public Property PlanService As PlanService
        'Public Property SubscriptionService As SubscriptionService
        'Public Property OpenpayFeesService As OpenpayFeesService
        'Public Property WebhooksService As WebhookService
        '  Public Property MerchantService As MerchantService
        Private httpClient As OpenpayHttpClient

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            Me.httpClient = New OpenpayHttpClient(api_key, merchant_id, production)
            CustomerService = New CustomerService(Me.httpClient)
            '  CardService = New CardService(Me.httpClient)
            'BankAccountService = New BankAccountService(Me.httpClient)
            ChargeService = New ChargeService(Me.httpClient)
            'PayoutService = New PayoutService(Me.httpClient)
            'TransferService = New TransferService(Me.httpClient)
            'FeeService = New FeeService(Me.httpClient)
            'PlanService = New PlanService(Me.httpClient)
            'SubscriptionService = New SubscriptionService(Me.httpClient)
            'OpenpayFeesService = New OpenpayFeesService(Me.httpClient)
            'WebhooksService = New WebhookService(Me.httpClient)
            'PayoutReportService = New PayoutReportService(Me.httpClient)
            ' MerchantService = New MerchantService(Me.httpClient)
        End Sub

        Public Property Production As Boolean
            Get
                Return Me.httpClient.Production
            End Get
            Set(ByVal value As Boolean)
                Me.httpClient.Production = value
            End Set
        End Property
    End Class
End Namespace


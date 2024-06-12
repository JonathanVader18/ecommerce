Imports System

Namespace Openpay.Entities
    Public Enum TransactionStatus
        IN_PROGRESS
        COMPLETED
        REFUNDED
        CHARGEBACK_PENDING
        CHARGEBACK_ACCEPTED
        CHARGEBACK_ADJUSTMENT
        CHARGE_PENDING
        CANCELLED
        FAILED
    End Enum
End Namespace


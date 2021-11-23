
Public Class geocoderesponse
        Public Property country As String = ""
        Public Property words As String = ""
        Public Property language As String = ""
        Public Property map As String = ""
        Public Property nearestPlace As String = ""
        Public Property square As square
        Public Property coordinates As coordinates
    End Class
    Public Class square
        Public Property southwest As coordinates
        Public Property northeast As coordinates
    End Class

    Public Class coordinates
        Public Property lng As Decimal
        Public Property lat As Decimal
    End Class



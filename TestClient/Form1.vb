Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim api As New eureka.w3w.API
        api.Settings.APIKey = "7I642GZD"
        api.Geocode("prescription.gloomy.margins")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim api As New eureka.w3w.API
        api.Settings.APIKey = "7I642GZD"
        Dim x As New eureka.w3w.models.coordinates
        x.lat = 52.342672
        x.lng = -6.54138
        api.ReverseGeocode(x)
    End Sub
End Class

Imports RestSharp
Imports eureka.w3w.models
Public Class API
    Public Property Settings As New settings
    Private Function GetClient() As RestClient
        Dim Client = New RestClient("https://api.what3words.com/v3/")
        Client.AddDefaultParameter("key", Settings.APIKey, ParameterType.QueryString)
        Return Client
    End Function

    Public Function Geocode(words As String) As Result
        Dim result As New Result
        Try
            Dim Client = GetClient()
            Dim request As New RestRequest("convert-to-coordinates")
            request.Method = Method.Get
            request.AddParameter("words", words, ParameterType.QueryString)
            request.RequestFormat = DataFormat.Json
            Dim Response As RestResponse(Of geocoderesponse) = Client.Execute(Of geocoderesponse)(request)
            If Response.StatusCode <> Net.HttpStatusCode.OK Then Throw New Exception(Response.StatusCode.ToString & Response.Content)
            result.data = Response.Data
            result.Success = True
        Catch ex As Exception
            result.Message = ex.Message
        End Try
        Return result
    End Function
    Public Function ReverseGeocode(coordinates As coordinates) As Result
        Dim result As New Result
        Try
            Dim Client = GetClient()
            Dim request As New RestRequest("convert-to-3wa")
            request.Method = Method.Get
            request.AddParameter("coordinates", String.Format("{0},{1}", coordinates.lat, coordinates.lng), ParameterType.QueryString)
            request.RequestFormat = DataFormat.Json
            Dim Response As RestResponse(Of geocoderesponse) = Client.Execute(Of geocoderesponse)(request)
            If Response.StatusCode <> Net.HttpStatusCode.OK Then Throw New Exception(Response.StatusCode.ToString & Response.Content)
            result.data = Response.Data
            result.Success = True
        Catch ex As Exception
            result.Message = ex.Message
        End Try
        Return result
    End Function
End Class

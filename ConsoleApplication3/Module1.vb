Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Module Module1

    Sub Main()

        Try
            Dim a As New Ambiente

            Console.WriteLine(JsonConvert.SerializeObject(a, Formatting.Indented))

        Catch ex As Exception

            Dim output As String = JsonConvert.SerializeObject(ex)

            Console.WriteLine(output)

        End Try

    End Sub

End Module

Public Class Ambiente

    Public ReadOnly Property Dominio As String
        Get
            Return Environment.UserDomainName
        End Get
    End Property

    Public ReadOnly Property NomeMaquina As String
        Get
            Return Environment.MachineName
        End Get
    End Property

    Public ReadOnly Property Usuario As String
        Get
            Return Environment.UserName
        End Get
    End Property

    Public ReadOnly Property SistemaOperacional As String
        Get
            Return New Devices.ComputerInfo().OSFullName
        End Get
    End Property

    Public ReadOnly Property Versao As String
        Get
            Return New Devices.ComputerInfo().OSVersion
        End Get
    End Property

    Public ReadOnly Property Plataforma As String
        Get
            Return New Devices.ComputerInfo().OSPlatform
        End Get
    End Property

    Public ReadOnly Property Cultura As String
        Get
            Return New Devices.ComputerInfo().InstalledUICulture.DisplayName
        End Get
    End Property

    Public ReadOnly Property MemoriaDisponivel As String
        Get
            Return New Devices.ComputerInfo().AvailablePhysicalMemory
        End Get
    End Property

    Public ReadOnly Property MemoriaTotal As String
        Get
            Return New Devices.ComputerInfo().TotalPhysicalMemory
        End Get
    End Property

    Public ReadOnly Property AcessoInternet As Boolean
        Get
            Return CheckForInternetConnection()
        End Get
    End Property

    Public ReadOnly Property Drives As List(Of Drive)
        Get

            Dim a = DriveInfo.GetDrives().ToList

            Dim k = a.Where(Function(x) x.IsReady).Select(Function(y) Drive.Create(y))

            k = k.ToList

            Return k

        End Get
    End Property

    Private Shared Function CheckForInternetConnection() As Boolean

        Try

            Using client As WebClient = New WebClient()

                Using stream = client.OpenRead("http://www.google.com")

                    Return True

                End Using

            End Using

        Catch

            Return False

        End Try

    End Function

End Class

Public Class Drive

    Public Property Name As String
    Public Property DriveType As String
    Public Property VolumeLabel As String
    Public Property DriveFormat As String
    Public Property AvailableFreeSpace As String
    Public Property TotalFreeSpace As String
    Public Property TotalSize As String


    Private Sub New(name As String, driveType As String, volumeLabel As String, driveFormat As String, availableFreeSpace As String, totalFreeSpace As String, totalSize As String)

        Me.Name = name
        Me.DriveType = driveType
        Me.VolumeLabel = volumeLabel
        Me.DriveFormat = driveFormat
        Me.AvailableFreeSpace = availableFreeSpace
        Me.TotalFreeSpace = totalFreeSpace
        Me.TotalSize = totalSize

    End Sub

    Public Shared Function Create(driveInfo As DriveInfo) As Drive

        Return New Drive(driveInfo.Name, driveInfo.DriveType.ToString, driveInfo.VolumeLabel, driveInfo.DriveFormat, driveInfo.AvailableFreeSpace.ToString, driveInfo.TotalFreeSpace.ToString, driveInfo.TotalSize.ToString)

    End Function

End Class



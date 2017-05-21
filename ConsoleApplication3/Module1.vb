Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports ConsoleApplication3
Imports Newtonsoft.Json

Module Module1

    Sub Main()

        Try
            Dim a As New Ambiente

            Dim log As New UsuarioLog() With {.UsuarioId = 1, .Acao = Acao.Inserir, .Tela = 1, .Objeto = JsonConvert.SerializeObject(a, Formatting.Indented)}

            Dim repo As New UsuarioLogRepository()

            repo.Inserir(log)

            ' Console.WriteLine(JsonConvert.SerializeObject(a, Formatting.Indented))

        Catch ex As Exception

            Dim output As String = JsonConvert.SerializeObject(ex)

            Console.WriteLine(output)

        End Try

    End Sub

End Module

Public Interface IUsuarioLogRepository

    Sub Inserir(usuarioLog As UsuarioLog)

End Interface

Public Class UsuarioLogRepository
    Implements IUsuarioLogRepository

    Private Property _connectionString As String = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TESTE;Integrated Security=True"

    Public Sub Inserir(usuarioLog As UsuarioLog) Implements IUsuarioLogRepository.Inserir

        Dim commandText As String = "INSERT INTO USUARIO_LOG (" &
                                    " USR_LOG_USR_ID, " &
                                    " USR_LOG_DATAHORA, " &
                                    " USR_LOG_TELA, " &
                                    " USR_LOG_ACAO, " &
                                    " USR_LOG_OBJETO ) " &
                                    " VALUES ( " &
                                    " @USR_LOG_USR_ID, " &
                                    " @USR_LOG_DATAHORA, " &
                                    " @USR_LOG_TELA, " &
                                    " @USR_LOG_ACAO, " &
                                    " @USR_LOG_OBJETO ) "

        Dim conn As New SqlConnection(_connectionString)

        Dim cmd As New SqlCommand(commandText, conn)

        cmd.Parameters.AddWithValue("@USR_LOG_USR_ID", usuarioLog.UsuarioId)
        cmd.Parameters.AddWithValue("@USR_LOG_DATAHORA", Now)
        cmd.Parameters.AddWithValue("@USR_LOG_TELA", usuarioLog.Tela)
        cmd.Parameters.AddWithValue("@USR_LOG_ACAO", usuarioLog.Acao)
        cmd.Parameters.AddWithValue("@USR_LOG_OBJETO", usuarioLog.Objeto)

        Try
            conn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception

        Finally
            If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then
                conn.Close()
            End If
        End Try

    End Sub

End Class

Public Class UsuarioLog

    Public Property UsuarioLogId As Integer
    Public Property UsuarioId As Integer
    Public ReadOnly Property DataHora As DateTime
    Public Property Tela As String
    Public Property Acao As Acao
    Public Property Objeto As String

End Class

Public Enum Acao

    Inserir = 1
    Alterar = 2
    Excluir = 3
    Inativar = 4

End Enum

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



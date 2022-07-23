' imports
Imports System.Data
Imports System.Data.OleDb
Public Class Form3
    ' connection string
    Dim con As New OleDb.OleDbConnection(My.Settings.QuestionsConnectionString)
    'Dim con As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\DOKTORHUSAY\Documents\Questions.accdb")
    Dim highscore As Integer
    Public Function highest()
        Dim sql As String
        Dim cmd As New OleDb.OleDbCommand
        Dim myreader As OleDbDataReader

        ' Open connection between database
        con.Open()
        ' This will get the highscore from the database
        sql = "Select HighScore from HighestScore where id = 1"
        cmd.Connection = con
        cmd.CommandText = sql

        ' read
        myreader = cmd.ExecuteReader
        myreader.Read()

        ' convert to int to compare 
        highscore = Convert.ToInt32(myreader("HighScore"))
        con.Close()
        Label3.Text = highscore.ToString
        Return Nothing
    End Function
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Exit the game
        Me.Close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' open the form 1 and hide the main menu
        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' get the highscore and print it to label
        highest()

    End Sub
End Class
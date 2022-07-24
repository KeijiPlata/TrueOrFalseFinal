' imports
Imports System.Data
Imports System.Data.OleDb
Public Class Form1
    ' Connection
    'Dim con As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\DOKTORHUSAY\Documents\Questions.accdb")
    'Dim con As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & System.Environment.CurrentDirectory & "\Questions.accdb")
    Dim con As New OleDb.OleDbConnection(My.Settings.QuestionsConnectionString)
    Dim convtf As Boolean
    Dim score As Integer
    Dim highscore As Integer
    Dim count1 As Integer
    Dim timee As Integer = 5 ' 5 seconds to answer

    Private Function ChangeButton()
        ' Generate random number
        Randomize()
        Dim randomNum As Decimal = Rnd() * 10
        Dim conv As Integer = Int(randomNum) + 1

        ' Generate random number again
        Randomize()
        Dim randomNum2 As Decimal = Rnd() * 10
        Dim conv2 As Integer = Int(randomNum2) + 1

        ' if the random number is an even number, it will change location
        If conv Mod 2 = 0 Then
            Button1.Location = New Point(365, 203)
            Button2.Location = New Point(63, 203)
        Else
            Button1.Location = New Point(63, 203)
            Button2.Location = New Point(365, 203)
        End If

        ' if the randomnumber is an even number, it will change color
        If conv2 Mod 2 = 0 Then
            Button1.BackColor = Color.Red
            Button2.BackColor = Color.DarkTurquoise
        Else
            Button1.BackColor = Color.DarkTurquoise
            Button2.BackColor = Color.Red
        End If
        Return Nothing
    End Function
    ' I created here a function to avoid redundancy
    Private Function TFsyntax()
        ' Declaring variables
        Dim sql As String
        Dim TF As String
        Dim cmd As New OleDb.OleDbCommand
        Dim myreader As OleDbDataReader

        ' generate random number 
        Randomize()
        Dim randomNum As Decimal = Rnd() * count1
        Dim conv As Integer = Int(randomNum) + 1

        ' Open connection between database
        con.Open()

        ' select from database
        sql = "Select Question, tf from QuestionsBool where id=" & Val(conv.ToString) & ""
        cmd.Connection = con
        cmd.CommandText = sql

        ' read
        myreader = cmd.ExecuteReader
        myreader.Read()

        ' print to label
        Label1.Text = myreader("Question")

        ' Get the value of TF and convert it into boolean
        TF = myreader("tf")
        convtf = Convert.ToBoolean(TF)

        ' close connection from database
        con.Close()
        Return Nothing
    End Function

    ' updates the score if there is a highscore
    Private Function Highest()
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

        ' get the highscore and pass it to form2
        Form2.highscore2 = highscore

        If score > highscore Then
            ' if the score is greater than highscore, it will be replaced
            highscore = score

            'get the new highscore and pass it to form2
            Form2.newhighscore = highscore

            ' Open connection between database
            con.Open()

            ' We only have one highscore so ID 1 only updates when there is a new highscore
            sql = "Update HighestScore set HighScore=" & highscore & " where id = 1" & ""
            cmd.Connection = con
            cmd.CommandText = sql
            cmd.ExecuteNonQuery()

            'close the connection
            con.Close()
        End If
        Return Nothing
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Start timer
        Timer1.Start()

        ' This will load the first question 
        TFsyntax()

        Dim sql As String
        Dim cmd As New OleDb.OleDbCommand

        ' This will get the count of the questions inside the database
        con.Open()
        sql = "Select count(Question) from QuestionsBool"
        cmd.Connection = con
        cmd.CommandText = sql
        count1 = Convert.ToInt32(cmd.ExecuteScalar)
        con.Close()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If convtf = True Then
            TFsyntax()
            ChangeButton() ' change the button location randomly
            score = score + 1
            timee = 5 ' reset time
            Label3.ForeColor = Color.Green
            Label3.Text = timee.ToString
            Label4.Text = score.ToString
        Else
            Highest()
            Form2.displayScore = score
            Form2.Show()
            score = 0 'reset
            Me.Close()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If convtf = False Then
            TFsyntax()
            ChangeButton() ' change the button location randomly
            score = score + 1
            timee = 5 'reset time
            Label3.ForeColor = Color.Green
            Label3.Text = timee.ToString
            Label4.Text = score.ToString
        Else
            Highest()
            Form2.displayScore = score
            Form2.Show()
            score = 0 'reset
            Me.Close()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' this will countdown the timer
        timee = timee - 1
        Label3.Text = timee.ToString

        If timee = 0 Then
            ' Stops the timer so it will not go  negative
            Timer1.Stop()

            ' get the score and pass it to next form
            Highest()
            Form2.displayScore = score
            Form2.Show()
            score = 0 'reset
            Me.Close()
        End If

        ' change the color 
        If timee <= 5 And timee >= 4 Then
            Label3.ForeColor = Color.Green
        Else
            Label3.ForeColor = Color.Red
        End If
    End Sub
End Class

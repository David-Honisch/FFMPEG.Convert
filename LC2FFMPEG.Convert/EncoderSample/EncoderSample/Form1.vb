Public Class Form1
    Public WithEvents newcoder As New FFLib.Encoder
    Private Sub ConOut(ByVal prog As String, ByVal tl As String) Handles newcoder.Progress
        ProgressBar.Value = prog
        Application.DoEvents()
    End Sub
    Private Sub stat(ByVal status) Handles newcoder.Status
        StatusLbl.Text = status
        Application.DoEvents()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim outpath As New FolderBrowserDialog
        If outpath.ShowDialog = Windows.Forms.DialogResult.OK Then
            outfolder = outpath.SelectedPath
            TextBox1.Text = outpath.SelectedPath
        End If
    End Sub
    Dim outfolder
    Private Sub ListBox1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then 'makes sure its a file or folder
            e.Effect = DragDropEffects.Copy 'copys the item to the "e" handler for later use 
            'in the drag drop event
        Else
            e.Effect = DragDropEffects.None 'if its not a file or folder, do nothing
            'actually it will display a circle with line "not valid"
        End If
    End Sub
    Private Sub ListBox1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListBox1.DragDrop
        Dim file_names As String() = DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
        'declares list of strings called "file_names". contains all the items dropped into list box       
        Dim listofiles As New ArrayList
        For Each file_name As String In file_names
            If My.Computer.FileSystem.FileExists(file_name) = True Then
                'a file was dragged
                listofiles.Add(file_name)
            ElseIf My.Computer.FileSystem.DirectoryExists(file_name) = True Then
                'a folder was added..
                Try
                    listofiles.AddRange(My.Computer.FileSystem.GetFiles(file_name, FileIO.SearchOption.SearchAllSubDirectories))
                Catch ex As Exception
                End Try
            End If
        Next file_name
        For Each file In listofiles
            'Create a file info object
            Dim FileInfo As New System.IO.FileInfo(file)
            If My.Settings.KnownFiles.Contains(FileInfo.Extension) = True Then
                'trim extension off of filename.
                ListBox1.Items.Add(file)
            End If
        Next
    End Sub


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Newarray As New ArrayList
        My.Settings.KnownFiles = Newarray
        My.Settings.KnownFiles.Add(".VOB")
        My.Settings.KnownFiles.Add(".wmv")
        My.Settings.KnownFiles.Add(".avi")
        My.Settings.KnownFiles.Add(".m2ts")
        My.Settings.KnownFiles.Add(".MP4")
        My.Settings.KnownFiles.Add(".ASF")
        My.Settings.KnownFiles.Add(".flac")
        My.Settings.KnownFiles.Add(".vob")
        My.Settings.KnownFiles.Add(".flv")
        My.Settings.KnownFiles.Add(".mp3")
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        For index = 0 To ListBox1.Items.Count - 1
            newcoder.OverWrite = False
            newcoder.SourceFile = ListBox1.Items.Item(index)
            newcoder.Format = newcoder.Format_MKV
            newcoder.AudioCodec = newcoder.AudioCodec_mp3
            newcoder.AudioBitrate = "128k"
            newcoder.Video_Codec = newcoder.Vcodec_h264
            newcoder.Threads = 0
            newcoder.OverWrite = True
            '  newcoder.ConstantRateFactor = 22
            newcoder.RateControl = newcoder.RateControl_ABR
            newcoder.VideoBitrate = "1000k"

            newcoder.Libx264_Preset_pass1 = newcoder.libx264_fast

            newcoder.OutputPath = outfolder
            newcoder.AnalyzeFile()

            newcoder.Encode()
            ListBox1.SetItemCheckState(index, CheckState.Checked)
        Next
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        For index = 0 To ListBox1.Items.Count - 1
            newcoder.OverWrite = False
            newcoder.SourceFile = ListBox1.Items.Item(index)
            newcoder.Format = newcoder.Format_MP3
            newcoder.AudioCodec = newcoder.AudioCodec_mp3
            newcoder.Video_Codec = newcoder.Vcodec_NONE
            newcoder.Threads = 0
            newcoder.OverWrite = True
            newcoder.ConstantRateFactor = 17
            newcoder.RateControl = newcoder.RateControl_CRF
            newcoder.Libx264_Preset_pass1 = newcoder.libx264_fast
            newcoder.OutputPath = outfolder


            newcoder.Encode()
            ListBox1.SetItemCheckState(index, CheckState.Checked)
        Next
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim outpath As New FolderBrowserDialog
        If outpath.ShowDialog = Windows.Forms.DialogResult.OK Then
            outfolder = outpath.SelectedPath
            TextBox1.Text = outpath.SelectedPath
        End If
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs)

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If (File.Exists("LC2Update.exe")) Then
            Process.Start("LC2Update.exe http://www.letztechance.org/download/LC2FFMPEGEncoder.xml 1000")
            Close()
        Else
            MessageBox.Show("Error: " + Environment.NewLine + "L2Update.exe " + Environment.NewLine + Environment.NewLine + "LC Update Manager not found.", "Error")


        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Close()

    End Sub
End Class

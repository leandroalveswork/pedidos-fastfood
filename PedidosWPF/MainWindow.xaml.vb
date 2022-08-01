Public Class MainWindow 

    Public Sub New()
        InitializeComponent()
        DataContext = New MainWindowViewModel()
    End Sub

End Class
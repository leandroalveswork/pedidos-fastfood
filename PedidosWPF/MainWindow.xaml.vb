Public Class MainWindow 

    Public Property PaginaAtual As EnPaginaAtual

    Public Sub New()
        InitializeComponent()
        PaginaAtual = EnPaginaAtual.Home_Index
        Console.WriteLine(PaginaAtual)
    End Sub

End Class
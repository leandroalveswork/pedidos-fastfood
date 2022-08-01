Public Class MainWindowViewModel

    Public Sub New()
        ModelDaUi = New MainWindowModel()
        ModelDaUi.ViewModelAtual = New HomeIndexViewModel()
    End Sub

    Public Property ModelDaUi As MainWindowModel

End Class
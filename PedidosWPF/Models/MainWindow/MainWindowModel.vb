Public Class MainWindowModel
    Inherits ModelBase

    Private _viewModelAtual As Object
    Public Property ViewModelAtual As Object
        Get
            Return _viewModelAtual
        End Get
        Set(ByVal Value As Object)
            _viewModelAtual = Value
            RaisePropertyChanged("ViewModelAtual")
        End Set
    End Property

    Public Sub Click_NavHome(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ViewModelAtual = New HomeIndexViewModel()
    End Sub

    Public Sub Click_NavBebida(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ViewModelAtual = New HomeIndexViewModel()
    End Sub

    Public Sub Click_NavLanche(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ViewModelAtual = New HomeIndexViewModel()
    End Sub

    Public Sub Click_NavPedido(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ViewModelAtual = New HomeIndexViewModel()
    End Sub

End Class
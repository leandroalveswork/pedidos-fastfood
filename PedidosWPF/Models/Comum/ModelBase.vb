Imports System.ComponentModel
Imports System.Runtime.CompilerServices

Public Class ModelBase
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub RaisePropertyChanged(<CallerMemberName()> Optional ByVal propertyName As String = Nothing)
        If PropertyChangedEvent IsNot Nothing Then
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))  
        End If
    End Sub

End Class
using System.Windows;
using FriendStorage.UI.ViewModel;

namespace FriendStorage.UI.View
{
  public partial class MainWindow : Window
  {
    public MainWindow(MainViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}

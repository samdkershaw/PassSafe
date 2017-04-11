using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PassSafe.Models;
using PassSafe.ViewModels;
using MahApps.Metro.Controls;

namespace PassSafe.Views
{
    /// <summary>
    /// Interaction logic for EditServiceWindow.xaml
    /// </summary>
    public partial class EditServiceWindow : MetroWindow
    {
        public EditServiceWindow(Service _SelectedService)
        {
            DataContext = new ViewModelEditServiceWindow(_SelectedService);
            InitializeComponent();
        }
    }
}

using MahApps.Metro.Controls;
using PassSafe.Models;
using PassSafe.ViewModels;
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

namespace PassSafe.Views
{
    /// <summary>
    /// Interaction logic for ViewDetailsWindow.xaml
    /// </summary>
    public partial class ViewDetailsWindow : MetroWindow
    {
        public ViewDetailsWindow(Service selectedService)
        {
            InitializeComponent();
            ViewModelViewDetailsWindow vm = new ViewModelViewDetailsWindow(selectedService);
            this.DataContext = vm;
        }
    }
}

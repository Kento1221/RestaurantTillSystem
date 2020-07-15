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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TillApp.Source;

namespace TillApp
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}

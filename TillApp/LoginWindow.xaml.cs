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
using TillApp.Source;

namespace TillApp
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Username_Box.Text = "";
            Password_Box.Password = "";
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Authentication of Login and Password
            //TODO: Success: Proceed to MainWindow
        }
    }
}

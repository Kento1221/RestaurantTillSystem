using System;
using System.Collections.Generic;
using System.Data.Linq;
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
            Username_Box.Text = String.Empty;
            Password_Box.Password = String.Empty;
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            using(Connection connection = new Connection("RestaurantMainDatabase"))
            {
                DataContext db = new DataContext(@"Database\RestaurantDatabase.mdf");
                Table<Employees> employees = db.GetTable<Employees>();//Needs Entity
                var query = from results in employees
                            where (employees.Username = Username_Box.Text) && (employees.Password = Password_Box.Password)
                            select (Name, Surname)//Is it correct?
                //TODO: Success: Proceed to MainWindow
            }
        }
    }
}

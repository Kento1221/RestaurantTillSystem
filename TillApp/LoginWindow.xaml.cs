using EFDataAccess.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TillApp.Source;

namespace TillApp
{
    public partial class LoginWindow : Window
    {
        readonly RestaurantDatabaseEntities entities = new RestaurantDatabaseEntities();

        public LoginWindow()
        {
            InitializeComponent();
            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();

            //Without the following line, the app experiences a brief freeze when pressing Enter button with correct username and password typed in. It doesn't take any data from database.
            entities.Employees.Where(x=>x.Id == -1).Select(x => x.Id).ToArray();
        }

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username_Box.Text != String.Empty && Password_Box.Password != String.Empty)
            {
                if (Logger(Username_Box.Text, Password_Box.Password))
                {
                    var v = new MainWindow();
                    v.Show();
                    Close();
                }
                else
                {
                    Error_Label.Visibility = Visibility.Visible;
                    ClearBoxes();
                }
            }
        }


        /// <summary>
        /// Matches username and password values in database.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns><para>
        /// True: Match has been found
        /// </para><para>
        /// False: Match hasn't beed found
        /// </para></returns>
        private bool Logger(string username, string password)
        {
            return entities.Employees
                        .Where(x => x.Username == username && x.Password == password)
                        .Select(x => x.Username)
                        .ToArray()
                        .Length == 1;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e) => ClearBoxes();

        /// <summary>
        /// Clears username and password textboxes.
        /// </summary>
        private void ClearBoxes()
        {
            Username_Box.Text = String.Empty;
            Password_Box.Password = String.Empty;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void Maximize_Button_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Maximized;
        private void About_Button_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Created by Kamil Orkisz. \n\nUsername: Kento1221\nPassword: Kamil");
    }
}

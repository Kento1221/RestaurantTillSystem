using EFDataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
    public partial class LoginWindow : Window
    {

        RestaurantDatabaseEntities entities = new RestaurantDatabaseEntities();

        public LoginWindow()
        {
            InitializeComponent();
            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e) => ClearBoxes();

        private void Enter_Button_Click(object sender, RoutedEventArgs e)
        {

            if (Username_Box.Text != String.Empty && Password_Box.Password != String.Empty)
            {

                var employees = entities.Employees
                        .Where(x => x.Username == Username_Box.Text && x.Password == Password_Box.Password)
                        .ToList();


                if(employees.Count == 1)
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

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();


        private void ClearBoxes()
        {
            Username_Box.Text = String.Empty;
            Password_Box.Password = String.Empty;
        }

        private void Maximize_Button_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Maximized;

        private void About_Button_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Created by Kamil Orkisz.");
    }
}

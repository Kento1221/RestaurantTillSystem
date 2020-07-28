using EFDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TillApp.Source;

namespace TillApp
{

    public partial class MainWindow : Window
    {
        public enum DishTypes { Pho, Starters, Curry, Noodles, Kids, Sides }
        readonly RestaurantDatabaseEntities entities = new RestaurantDatabaseEntities();
        private readonly char[] buttonConstName = "Dish_Menu_Button_".ToArray();

        //Stores all dishes from database
        List<Dish> dishList = new List<Dish>();
        //Stores dishes of type X
        List<Dish> selectedDishList = new List<Dish>();
        //Stores ordered dishes
        List<Dish> orderList = new List<Dish>();
        //Stores dish types
        DishTypes currentlySelectedType = new DishTypes();

        private double Total = 0f;

        public MainWindow()
        {
            InitializeComponent();

            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();

            PopulateListOfDishes();
            SelectDishes(DishTypes.Starters);
        }

        /// <summary>
        /// Populates the list of dishes with dishes of all types.
        /// </summary>
        private void PopulateListOfDishes()
        {
            dishList.Clear();
            var dishes = entities.Dishes.ToArray();

            foreach (var dish in dishes)
            {
                Dish _dish = new Dish
                {
                    Name = dish.DishName,
                    Type = dish.DishType,
                    Price = dish.Price,
                    Allergens = dish.Allergens,
                    Button = null
                };
                dishList.Add(_dish);
            }
        }

        /// <summary>
        /// Selects a certain type of dish from populated dishList and prints it on the screen.
        /// </summary>
        /// <param name="dishType">String of dish type.</param>
        public void SelectDishes(DishTypes dishType)
        {
            if (dishType != currentlySelectedType)
            {
                selectedDishList.Clear();
                ClearButtons();

                foreach (var dish in dishList)
                    if (dish.Type == dishType.ToString()) selectedDishList.Add(dish);

                PrintButtons();
                currentlySelectedType = dishType;
            }
        }

        /// <summary>
        /// Creates buttons. Each of them is assigned a dish from selectedDishList.
        /// </summary>
        private void PrintButtons()
        {
            for (int i = 0; i < selectedDishList.Count; i++)
            {
                Button button = new Button();
                button.Name = "Dish_Menu_Button_"+i;
                button.Click += Menu_Dish_Button_Click;
                button.IsEnabled = true;
                button.Visibility = Visibility.Visible;
                button.Height = 80f;
                button.Width = 300f;
                button.Foreground = Brushes.Black;
                button.Background = Brushes.LightGray;
                button.Content = selectedDishList[i].Name;

                Dish_Menu_WrapPanel.Children.Add(button);
            }
        }

        /// <summary>
        /// Removes all buttons from Dish_Menu_WrapPanel element.
        /// </summary>
        private void ClearButtons() => Dish_Menu_WrapPanel.Children.Clear();

        /// <summary>
        /// Prints order items and prices from orderList list to the Order_List_Items and Order_List_Price textblock elements.
        /// </summary>
        private void PrintOrderList( )
        {
            Order_List_Items.Text = String.Empty;
            Order_List_Price.Text = String.Empty;
            int id = 1;

            foreach (var item in orderList)
            {                
                string price = item.Price.ToString();
                if (price.Length == 2 || price.Length == 1) price += ",00";
               
                Order_List_Items.Text += id++ + ". " + item.Name.Trim() + "\n";
                Order_List_Price.Text += price + "\n";
            }
        }

        #region button click events

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Maximize_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                WindowState_Button_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                WindowState_Button_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
            }
        }

        private void Starters_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Starters);

        private void Pho_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Pho); 

        private void Curry_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Curry);

        private void Noodles_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Noodles);

        private void Kids_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Kids);

        private void Sides_button_Click(object sender, RoutedEventArgs e) => SelectDishes(DishTypes.Sides);

        private void Menu_Dish_Button_Click(object sender, RoutedEventArgs e)
        {
            //retrieving index from the button clicked.
            Button button = (Button)sender;
            string dishId = button.Name.Trim(buttonConstName);

            //Adding dish to Order_List text block and updating total.
            Dish selectedDish = selectedDishList[Int32.Parse(dishId)];
            orderList.Add(selectedDish);

            PrintOrderList();

            //Updating total to pay label
            Total +=selectedDish.Price;
            Total_Amount.Text = Total.ToString() + " zł";
        }

        #endregion button click events


        #region mouse events

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        #endregion mouse events
    }


}

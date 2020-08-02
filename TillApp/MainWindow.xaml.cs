using EFDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TillApp.Source;

namespace TillApp
{
    public partial class MainWindow : Window
    {
        #region properties
        public enum DishTypes { Pho, Starters, Curry, Noodles, Kids, Sides }
        readonly RestaurantDatabaseEntities entities = new RestaurantDatabaseEntities();
        private readonly char[] buttonConstName = "Dish_Menu_Button_".ToArray();

        //Stores all dishes from database
        List<Dish> dishList = new List<Dish>();
        //Stores dishes of type X
        List<Dish> selectedDishList = new List<Dish>();
        //Stores dish types
        DishTypes currentlySelectedType = new DishTypes();
        //Stores all tables created
        private List<Table> listOfTables = new List<Table>();
        //Stores currently selected table
        private Table currentTable;
        //Doesn't allow adding items when table is not selected
        private bool tableIsSelected = false;

        #endregion properties

        public MainWindow()
        {
            InitializeComponent();

            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();

            Page_Frame.Content = new Payment();
            PopulateListOfDishes();
            SelectDishes(DishTypes.Starters);
        }

        #region methods

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
        /// Selects a certain type of a dish from populated dishList and prints it on the screen.
        /// </summary>
        /// <param name="dishType">String naming the dish type.</param>
        public void SelectDishes(DishTypes dishType)
        {
            if (Page_Frame.Visibility == Visibility.Visible)
            {
                Page_Frame.Visibility = Visibility.Hidden;
                Dish_Menu_ScrollViever.Visibility = Visibility.Visible;
            }

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
        /// Creates buttons. Each of them is assigned to a dish from selectedDishList.
        /// </summary>
        private void PrintButtons()
        {
            for (int i = 0; i < selectedDishList.Count; i++)
            {
                Button button = new Button();
                button.Name = "Dish_Menu_Button_" + i;
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
        /// Prints order items and prices from currentTable.OrderedDishes list to the Order_List_Items and Order_List_Price textblock elements.
        /// </summary>
        private void PrintOrderList()
        {
            ClearItemDisplayList();
            int id = 1;

            foreach (var item in currentTable.OrderedDishes)
            {
                string price = item.Price.ToString();
                if (price.Length == 2 || price.Length == 1) price += ",00";

                Order_List_Items.Text += id++ + ". " + item.Name.Trim() + "\n";
                Order_List_Price.Text += price + "\n";
            }
        }

        /// <summary>
        /// Clears the view of ordered dishes and total amount to pay.
        /// </summary>
        private void ClearItemDisplayList()
        {
            Order_List_Items.Text = String.Empty;
            Order_List_Price.Text = String.Empty;
        }

        /// <summary>
        /// Creates a new instance of a Table.
        /// </summary>
        private void CreateNewTable()
        {
            var window = new GenericAlertWindow();
            window.CreateLabelValueView(
                new string[] { "Table Number:", "Table Name:" }, 
                new Type[] { typeof(int), typeof(string) }
                );
            
            window.ShowDialog();
            if (window.Values.Count != 0)
            {
                Table table = new Table(int.Parse(window.Values[0]), window.Values[1], "Kamil");
                listOfTables.Add(table); //TODO: Proper WaiterName download
                currentTable = listOfTables.Last();
                tableIsSelected = true;
                UpdateTableInfo();
                ClearItemDisplayList();
            }
        }

        /// <summary>
        /// Updates name, number, time and date of the table displayed using data from the currentTable.
        /// </summary>
        /// <param name="table">Table to display info from.</param>
        private void UpdateTableInfo()
        {
            Customer_Table_Name.Text = currentTable.TableName;
            Customer_Table_Number.Text = "T" + currentTable.TableNumber;
            Customer_Time.Text = currentTable.CreationDateTime.ToString("HH:mm:ss");
            Customer_Date.Text = currentTable.CreationDateTime.ToString("dd.MM.yyyy");
        }

        #endregion methods

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
            if (tableIsSelected)
            {

                //retrieving index from the button clicked.
                Button button = (Button)sender;
                string dishId = button.Name.Trim(buttonConstName);

                //Adding dish to Order_List text block and updating total.
                Dish selectedDish = selectedDishList[Int32.Parse(dishId)];
                var _table = listOfTables.Find(x => x.TableNumber == currentTable.TableNumber);
                _table.OrderedDishes.Add(selectedDish);
                _table.TotalPrice+=selectedDish.Price;

                PrintOrderList();

                //Updating total to pay label
                Total_Amount.Text = _table.TotalPrice + " zł"; 
            }
            else
            {
                MessageBoxResult messageBox = MessageBox.Show("No table is currently assigned. \nWould you like to create a new table?", "Empty Table", MessageBoxButton.YesNo);
                if (messageBox == MessageBoxResult.Yes)
                {
                    CreateNewTable();
                }
            }
        }

        private void Payment_Button_Click(object sender, RoutedEventArgs e)
        {
            Dish_Menu_ScrollViever.Visibility = Visibility.Hidden;
            Page_Frame.Visibility = Visibility.Visible;
        }

        private void New_Table_Click(object sender, RoutedEventArgs e) => CreateNewTable();
        
        private void Change_Table_Click(object sender, RoutedEventArgs e)
        {
            GenericAlertWindow window = new GenericAlertWindow();
            window.CreateSelectionView(listOfTables);
            window.ShowDialog();
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

using EFDataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TillApp.Source;

namespace TillApp
{

    public partial class MainWindow : Window
    {
        RestaurantDatabaseEntities entities = new RestaurantDatabaseEntities();
        List<Dish> dishList = new List<Dish>();
        List<Dish> selectedDishList = new List<Dish>();
        public enum dishType {Starters, Pho, Curry, Noodles, Kids, Sides}


        public MainWindow()
        {
            InitializeComponent();
            
            Clock clock = new Clock(Time_Label, Date_Label);
            clock.StartClock();
            
            PopulateListOfDishes();
            SelectDishes(dishType.Curry);
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();


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
        /// Selects a certain type of dish from populated dishList.
        /// </summary>
        /// <param name="dishType">String of dish type.</param>
        private void SelectDishes(dishType dishType)
        {
            selectedDishList.Clear();
            foreach(var dish in dishList)
            {
                if (dish.Type == dishType.ToString())
                    selectedDishList.Add(dish);
            }
            PrintButtons();
        }

        /// <summary>
        /// Assignes buttons to each element of selectedDishList.
        /// </summary>
        private void PrintButtons()
        {
            for (int i = 0; i<selectedDishList.Count;i++)
            {
                Button _button = (Button)FindName("dish_menu_button" + i);
                if (_button == null) 
                    throw new System.Exception("_button is NULL");
                else
                {
                    _button.Content = selectedDishList[i].Name;
                    _button.Visibility = Visibility.Visible;
                    _button.IsEnabled = true;
                    selectedDishList[i].Button = _button;
                }
            }
        }


    
    }


}

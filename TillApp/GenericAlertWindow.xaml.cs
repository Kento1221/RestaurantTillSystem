using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TillApp.Source;

namespace TillApp
{
    /// <summary>
    /// Logika interakcji dla klasy GenericAllertWindow.xaml
    /// </summary>
    public partial class GenericAlertWindow : Window
    {
        #region Parameters
        const double MARGIN_VALUE = 15f;
        const double CHILDREN_HEIGHT = 25f;

        public List<string> Values { get; } = new List<string>();
        public Type[] types;
        private int fieldNumber = 0;

        #endregion

        public GenericAlertWindow()
        {
            InitializeComponent();
        }

        #region Methods

        public void CreateLabelValueView(string[] fields, Type[] fieldTypes)
        {
            if (fields.Length == fieldTypes.Length)
            {
                fieldNumber = fields.Length;
                types = fieldTypes;
            }
            else
            {
                throw new Exception("Invalid number of fields and types!");
            }


            var margin = new Thickness(MARGIN_VALUE);

            var dockPanel = new DockPanel();
            dockPanel.Children.Add(new StackPanel { Name = "Labels", Orientation = Orientation.Vertical });
            dockPanel.Children.Add(new StackPanel { Name = "Values", Orientation = Orientation.Vertical });

            foreach (var field in fields)
            {
                //Labels on the left
                ((StackPanel)dockPanel.Children[0]).Children.Add(new TextBlock { Text = field, Margin = margin, Height = CHILDREN_HEIGHT });
                //Textboxes with value on the right
                ((StackPanel)dockPanel.Children[1]).Children.Add(new TextBox { Margin = margin, Height = CHILDREN_HEIGHT });
            }

            AddElement(dockPanel);
        }

        private void AddElement(UIElement element)
        {
            Main_Grid.Children.Add(element);
        }

        public void CreateSelectionView(List<Table> listOfTables)
        {
            var margin = new Thickness(MARGIN_VALUE);

            DockPanel dockPanel = new DockPanel();
            dockPanel.Children.Add(new StackPanel { Name = "Labels", Orientation = Orientation.Horizontal, Background = Brushes.BlueViolet });
            dockPanel.Children.Add(new StackPanel { Name = "TableNumber", Orientation = Orientation.Vertical });
            dockPanel.Children.Add(new StackPanel { Name = "DateTime", Orientation = Orientation.Vertical });
            dockPanel.Children.Add(new StackPanel { Name = "TableName", Orientation = Orientation.Vertical });
            dockPanel.Children.Add(new StackPanel { Name = "Select", Orientation = Orientation.Vertical });//TODO: Add state
            DockPanel.SetDock(dockPanel.Children[0], Dock.Top);
            var orderedList = listOfTables
                .OrderBy(x => x.TableNumber)
                .ThenBy(x => x.CreationDateTime)
                .ToList();

            ((StackPanel)dockPanel.Children[0]).Children.Add(new TextBlock { Background = Brushes.BlueViolet, Margin = margin, Height = CHILDREN_HEIGHT, Text = "Table Number" });
            ((StackPanel)dockPanel.Children[0]).Children.Add(new TextBlock { Background = Brushes.BlueViolet, Margin = margin, Height = CHILDREN_HEIGHT, Text = "Date and Time" });
            ((StackPanel)dockPanel.Children[0]).Children.Add(new TextBlock { Background = Brushes.BlueViolet, Margin = margin, Height = CHILDREN_HEIGHT, Text = "Table Name" });
            ((StackPanel)dockPanel.Children[0]).Children.Add(new TextBlock { Background = Brushes.BlueViolet, Margin = margin, Height = CHILDREN_HEIGHT, Text = "Select by clicking" });

            int i = 0;
            foreach (var table in orderedList)
            {
                ((StackPanel)dockPanel.Children[1]).Children.Add(new TextBlock { Background = Brushes.LightGray, Margin = margin, Height = CHILDREN_HEIGHT, Text = table.TableNumber.ToString() });
                ((StackPanel)dockPanel.Children[2]).Children.Add(new TextBlock { Margin = margin, Height = CHILDREN_HEIGHT, Text = table.CreationDateTime.ToString("HH:mm:ss dd.MM.yyyy") });
                ((StackPanel)dockPanel.Children[3]).Children.Add(new TextBlock { Background = Brushes.LightGray, Margin = margin, Height = CHILDREN_HEIGHT, Text = table.TableName });
                ((StackPanel)dockPanel.Children[4]).Children.Add(new Button { Margin = margin, Height = CHILDREN_HEIGHT, Name = "Select_" + i, Content = "Select" });
                i++;
            }
            AddElement(dockPanel);
        }

        public bool CheckValueAndType(Type type, string value)
        {
            try
            {
                TypeDescriptor.GetConverter(type).ConvertFromString(value);
                return true;
            }
            catch { return false; }
        }

        #endregion

        #region Button events
        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            StackPanel textBoxes = (StackPanel)(((DockPanel)Main_Grid.Children[0]).Children[1]);
            for (int i = 0; i < fieldNumber; i++)
            {
                string value = ((TextBox)textBoxes.Children[i]).Text;
                if (CheckValueAndType(types[i], value))
                {
                    Values.Add(value);
                }
                else
                {
                    //TODO: *Optional* Make it look better down below.
                    ((TextBox)((StackPanel)(((DockPanel)Main_Grid.Children[0]).Children[1])).Children[i]).Background = Brushes.LightPink;
                    Error_Label.Text = "Invalid value in " + ((TextBlock)((StackPanel)(((DockPanel)Main_Grid.Children[0]).Children[0])).Children[i]).Text.Trim(':');
                    Error_Label.Visibility = Visibility.Visible;
                    return;
                }
            }
            Close();
        }

        #endregion
    }
}

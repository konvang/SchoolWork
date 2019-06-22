using OrderEntryDataAccess;
using OrderEntryEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace OrderEntrySystem
{
    /// <summary>
    /// Interaction logic for EntityView.xaml
    /// </summary>
    public partial class EntityView : UserControl
    {
        private Grid propertyGrid;

        public StackPanel commandPanel;

        public EntityView()
        {

            this.InitializeComponent();

            this.propertyGrid = new Grid();

            this.Content = this.propertyGrid;

            this.Loaded += this.UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.commandPanel = new StackPanel();
            this.commandPanel.Orientation = Orientation.Horizontal;
            this.commandPanel.HorizontalAlignment = HorizontalAlignment.Right;
            this.commandPanel.Margin = new Thickness(0, 0, 15, 5);

            Grid grid = new Grid();
            RowDefinition rowDefinition = new RowDefinition();
            Grid.SetRow(this.commandPanel, 1);
            grid.Children.Add(this.commandPanel);
            this.propertyGrid.Children.Add(grid);

            PropertyInfo[] propertyInfos = typeof(ProductViewModel).GetProperties();

            propertyInfos = (from s in propertyInfos.AsEnumerable()
                             where DisplayUtil.HasControl(s)
                             orderby DisplayUtil.GetControlSequence(s)
                             select s).ToArray();

            foreach (PropertyInfo p in propertyInfos)
            {
                if (DisplayUtil.HasControl(p))
                {
                    this.BuildLabeledControl(p);

                }
            }
        }

        public Binding CreateBinding(PropertyInfo propertyInfo, ControlType controlType, object source)
        {
            Binding binding = new Binding(propertyInfo.Name);
            binding.Source = source;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            if (propertyInfo.CanWrite || (controlType != ControlType.Label && controlType != ControlType.Button))
            {
                binding.Mode = BindingMode.TwoWay;
            }
            else
            {
                binding.Mode = BindingMode.OneWay;
            }

            if (controlType == ControlType.TextBox && binding.Mode == BindingMode.TwoWay)
            {
                switch (propertyInfo.PropertyType.Name)
                {
                    case "Decimal":
                        binding.Converter = new DecimalToStringConverter();
                        break;
                }
            }

            return binding;
        }

        private void BuildLabeledControl(PropertyInfo propertyInfo)
        {
            Grid grid = new Grid();

            grid.Width = 270;
            grid.Height = 23;
            grid.Margin = new Thickness(0, 0, 15, 5);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

            ControlType controlType = DisplayUtil.GetControlType(propertyInfo);

            Binding binding = CreateBinding(propertyInfo, controlType, this.DataContext);

            switch (controlType)
            {
                case ControlType.None:
                    break;

                case ControlType.TextBox:
                    TextBox textBox = new TextBox();
                    Grid.SetColumn(textBox, 1);
                    grid.Children.Add(textBox);
                    break;

                case ControlType.CheckBox:
                    CheckBox checkbox = new CheckBox();
                    checkbox.SetBinding(CheckBox.IsCheckedProperty, binding);
                    checkbox.Margin = new Thickness(0, 0, 15, 5);
                    Grid.SetColumn(checkbox, 1);
                    grid.Children.Add(checkbox);
                    break;

                case ControlType.ComboBox:
                    ComboBox comboBox = new ComboBox();
                    if (propertyInfo.PropertyType.IsEnum)
                    {
                       PopulateLookupComboBox(comboBox, Enum.GetValues(propertyInfo.PropertyType), null);
                    }
                    else
                    {
                        ILookupRepository LookUpRepo = RepositoryManager.GetLookupRepository(propertyInfo.PropertyType);
                        comboBox.ItemsSource = LookUpRepo.LookupList;
                    }
                    Grid.SetColumn(comboBox, 1);
                    grid.Children.Add(comboBox);
                    break;

                case ControlType.DateBox:
                    break;

                case ControlType.Label:
                    TextBox texboxLabel = new TextBox();
                    texboxLabel.IsEnabled = false;
                    Grid.SetColumn(texboxLabel, 1);
                    grid.Children.Add(texboxLabel);
                    break;

                case ControlType.Button:
                    Button button = new Button();
                    button.Content = DisplayUtil.GetControlDescription(propertyInfo);
                    button.SetBinding(Button.CommandProperty, binding);
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    button.Margin = new Thickness(0, 0, 15, 5);
                    this.commandPanel.Children.Add(button);
                    break;

                default:
                    break;
            }

            if (controlType != ControlType.Button)
            {
                Label label = new Label();
                label.Content = DisplayUtil.GetControlDescription(propertyInfo);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);
            }

            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = GridLength.Auto;
            this.propertyGrid.RowDefinitions.Add(rowDefinition);

            Grid.SetRow(grid, this.propertyGrid.RowDefinitions.Count - 1);
            this.propertyGrid.Children.Add(grid);
        }

        private static void PopulateLookupComboBox(ComboBox comboBox, Array lookupObjects, IValueConverter converter)
        {
            comboBox.Items.Clear();

            foreach (var a in lookupObjects)
            {
                Binding binding = new Binding();
                binding.Path = new PropertyPath("Path");
                binding.Mode = BindingMode.OneWay;
                binding.Converter = converter;

                DataTemplate dtp = new DataTemplate();
                FrameworkElementFactory textBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
                textBlockFactory.SetBinding(TextBlock.TextProperty, binding);
                
                dtp.VisualTree = textBlockFactory;
                comboBox.ItemTemplate = dtp;
                comboBox.Items.Add(a);

            }

        }
    }
}

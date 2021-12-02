using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Final_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int workStart = 8;
        public int workEnd = 11;
        public enum BrandEnum { Lenovo, MSI, Razer, AlienWare }
        public enum LenovoEnum { Legion5i, Legion7i}
        public enum MsiEnum { GE, GF }
        public enum RazerEnum { Blade15, Blade14 }
        public enum AlienwareEnum { M15, M17 }

        public enum WorkDoneEnum { Cleaning, Installing }

        ObservableCollection<Appointment> displayAppointment = null;

        public ObservableCollection<string> BrandList { get; set; }
        public ObservableCollection<string> ModelList { get; set; }
        public ObservableCollection<string> WorkDoneList { get; set; }


        public ObservableCollection<Appointment> DisplayAppointment { get => displayAppointment; set => displayAppointment = value; }

        public MainWindow()
        {
            InitializeComponent();
            DisplayAppointment = new ObservableCollection<Appointment>();
            BrandList = new ObservableCollection<string>();
            ModelList = new ObservableCollection<string>();
            WorkDoneList = new ObservableCollection<string>();
            DataContext = this;
            
        }

        private void AppTimeCmbBox_Init(object sender, EventArgs e)
        {
            for (int i = workStart; i <= workEnd;  i++)
            {
                appTimeCmbBox.Items.Add(i);
            }
        }

        private void BrandCmbBox_Init(object sender, EventArgs e)
        {
            
            brandCmbBox.ItemsSource = Enum.GetNames(typeof(BrandEnum));
        }

        private void BrandCmbBox_Change(object sender, SelectionChangedEventArgs e)
        {
            BrandList = new ObservableCollection<string>(Enum.GetNames(typeof(BrandEnum)));
            string selectedText = (string)brandCmbBox.SelectedItem;
            modelCmbBox.ItemsSource = null;
            modelCmbBox.Items.Clear();

            LaptopModel(selectedText);

            
        }

        private void WorkDone_Init(object sender, EventArgs e)
        {
            workDoneCmbBox.ItemsSource = Enum.GetNames(typeof(WorkDoneEnum));
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            MyDataGrid.ItemsSource = null;
            MyDataGrid.Columns.Clear();
            MyDataGrid.Items.Clear();
            MyDataGrid.Items.Refresh();

            //customerNameTxt.Clear();
            ////appTimeCmbBox.SelectedIndex = -1;
            //creditCardNoTxt.Clear();
            //brandCmbBox.SelectedIndex = -1;
            //modelCmbBox.SelectedIndex = -1;
            //workDoneCmbBox.SelectedIndex = -1;
            //technicianNameTxt.Clear();

            Appointment appointment = new();

            appointment.AppointmentTime = (int)appTimeCmbBox.SelectedItem;
            appointment.CustomerName = customerNameTxt.Text;
            appointment.CreditCardNo = creditCardNoTxt.Text;
            appointment.Brand = (string)brandCmbBox.SelectedItem;
            appointment.Model = (string)modelCmbBox.SelectedItem;
            appointment.WorkDone = (string)workDoneCmbBox.SelectedItem;
            appointment.TechnicianName = technicianNameTxt.Text;
         

            DisplayAppointment.Add(appointment);

            MyDataGrid.ItemsSource = DisplayAppointment;

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("AppointmentTime"), Header = "Time" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerName"), Header = "Name" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CreditCardNo"), Header = "Credit Card" });

            var style = new Style(typeof(ComboBox));
            style.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.BrandList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            style.Setters.Add(new EventSetter(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(BrandChange)));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Brand", SelectedValueBinding = new Binding("Brand") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = style, EditingElementStyle = style });

            var modelStyle = new Style(typeof(ComboBox));
            modelStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.ModelList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Model", SelectedValueBinding = new Binding("Model") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = modelStyle, EditingElementStyle = modelStyle });

            var workStyle = new Style(typeof(ComboBox));
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.ModelList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Model", SelectedValueBinding = new Binding("Model") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

        }

        private void BrandChange(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            LaptopModel(text);
        }

        private void ClearGrid()
        {
            MyDataGrid.ItemsSource = null;
            MyDataGrid.Columns.Clear();
            MyDataGrid.Items.Clear();
            MyDataGrid.Items.Refresh();
        }

        private void LaptopModel(string selectedText)
        {
            if (selectedText == "Lenovo")
            {
                ModelList = new ObservableCollection<string>(Enum.GetNames(typeof(LenovoEnum)));
                modelCmbBox.ItemsSource = Enum.GetNames(typeof(LenovoEnum));
            }
            else if (selectedText == "MSI")
            {
                ModelList = new ObservableCollection<string>(Enum.GetNames(typeof(MsiEnum)));
                modelCmbBox.ItemsSource = Enum.GetNames(typeof(MsiEnum));
            }
            else if (selectedText == "Razer")
            {
                ModelList = new ObservableCollection<string>(Enum.GetNames(typeof(RazerEnum)));
                modelCmbBox.ItemsSource = Enum.GetNames(typeof(RazerEnum));
            }
            else if (selectedText == "AlienWare")
            {
                ModelList = new ObservableCollection<string>(Enum.GetNames(typeof(AlienwareEnum)));
                modelCmbBox.ItemsSource = Enum.GetNames(typeof(AlienwareEnum));
            }
        }

        private void WorkDone_Change(object sender, SelectionChangedEventArgs e)
        {
            WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
        }
    }
}

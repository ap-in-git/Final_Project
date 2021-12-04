using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Serialization;

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
        public enum LenovoEnum { Legion5i, Legion7i }
        public enum MsiEnum { GE, GF }
        public enum RazerEnum { Blade15, Blade14 }
        public enum AlienwareEnum { M15, M17 }

        public enum WorkDoneEnum { Cleaning, Installing }

        AppointmentList appList = new();

        ObservableCollection<Appointment> temporaryAppointment = null;
        ObservableCollection<Appointment> permanentAppointment = null;

        public ObservableCollection<string> BrandList { get; set; }
        public ObservableCollection<string> ModelList { get; set; }
        public ObservableCollection<string> WorkDoneList { get; set; }


        public ObservableCollection<Appointment> TemporaryAppointment { get => temporaryAppointment; set => temporaryAppointment = value; }
        public ObservableCollection<Appointment> PermanentAppointment { get => permanentAppointment; set => permanentAppointment = value; }

        public MainWindow()
        {
            InitializeComponent();
            TemporaryAppointment = new ObservableCollection<Appointment>();
            BrandList = new ObservableCollection<string>();
            ModelList = new ObservableCollection<string>();
            WorkDoneList = new ObservableCollection<string>();
            DataContext = this;

        }

        //Appointment time combo box initialization
        private void AppTimeCmbBox_Init(object sender, EventArgs e)
        {
            for (int i = workStart; i <= workEnd; i++)
            {
                appTimeCmbBox.Items.Add(i);
            }
        }

        //Brand combo box initialization
        private void BrandCmbBox_Init(object sender, EventArgs e)
        {

            brandCmbBox.ItemsSource = Enum.GetNames(typeof(BrandEnum));
        }

        //Brand combo box upon changing of value will call LaptopModel to change the list depending on the Brand
        private void BrandCmbBox_Change(object sender, SelectionChangedEventArgs e)
        {
            BrandList = new ObservableCollection<string>(Enum.GetNames(typeof(BrandEnum)));
            string selectedText = (string)brandCmbBox.SelectedItem;
            modelCmbBox.ItemsSource = null;
            modelCmbBox.Items.Clear();

            LaptopModel(selectedText);
        }

        //Models of every laptop brands. Will change list depending on laptop brand.
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


        //Initialization of work done combo box
        private void WorkDone_Init(object sender, EventArgs e)
        {
            workDoneCmbBox.ItemsSource = Enum.GetNames(typeof(WorkDoneEnum));
        }

        //Add button. This button is responsible for transferring data from the fields to the datagrid.
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            MyDataGrid.ItemsSource = null;
            MyDataGrid.Columns.Clear();
            MyDataGrid.Items.Clear();
            MyDataGrid.Items.Refresh();

            Appointment appointment = new();

            appointment.AppointmentTime = (int)appTimeCmbBox.SelectedItem;
            appointment.CustomerName = customerNameTxt.Text;
            appointment.CreditCardNo = creditCardNoTxt.Text;
            appointment.Brand = (string)brandCmbBox.SelectedItem;
            appointment.Model = (string)modelCmbBox.SelectedItem;
            appointment.WorkDone = (string)workDoneCmbBox.SelectedItem;
            appointment.TechnicianName = technicianNameTxt.Text;

            TemporaryAppointment.Add(appointment);

            MyDataGrid.ItemsSource = TemporaryAppointment;

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
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.WorkDoneList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Work Done", SelectedValueBinding = new Binding("WorkDone") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("TechnicianName"), Header = "Technician" });

            ClearFields();
        }

        //This is an event that will be called when the Brand is edited in the data grid.
        private void BrandChange(object sender, SelectionChangedEventArgs e)
        {
            string text = (sender as ComboBox).SelectedItem as string;
            LaptopModel(text);
        }

        //private void WorkDone_Change(object sender, SelectionChangedEventArgs e)
        //{
        //    WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
        //}

        //Save Button. Responsible for saving the values in the datagrid to XML.
        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TemporaryAppointment.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save them all?", "Save all client", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveAll();
                    ClearGrid();
                }
            }
        }

        //Saving the data into XML format
        private void SaveAll()
        {
            appList.Clear();
            foreach (Appointment appointment in TemporaryAppointment)
            {
                appList.Add(appointment);
            }
            XmlSerializer serializer = new(typeof(AppointmentList));
            TextWriter tw = new StreamWriter("Appointment.xml");
            serializer.Serialize(tw, appList);
            tw.Close();

        }

        //Retrieve button. Responsible for retrieving data from XML format to the datagrid.
        private void GetAllButton_Click(object sender, RoutedEventArgs e)
        {
            //This is for the datagrid if it contains unsave items
            MessageBoxResult result = MessageBoxResult.No;
            if (TemporaryAppointment.Count > 0)
            {
                result = MessageBox.Show("You have items in the grid. Do you want to save them all?", "Save all client", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveAll();
                }
            }
            ClearGrid();

            //Open the file written below and read values from it.
            XmlSerializer serializer = new(typeof(AppointmentList));
            TextReader tr = new StreamReader("Appointment.xml");
            appList = (AppointmentList)serializer.Deserialize(tr);
            tr.Close();

            for (int i = 0; i < appList.Count(); i++)
            {
                Appointment retrieveAppointment = appList[i];
                Appointment appointment = new();
                appointment.AppointmentTime = retrieveAppointment.AppointmentTime;
                appointment.CustomerName = retrieveAppointment.CustomerName;
                appointment.CreditCardNo = retrieveAppointment.CreditCardNo;
                appointment.Brand = retrieveAppointment.Brand;
                appointment.Model = retrieveAppointment.Model;
                appointment.WorkDone = retrieveAppointment.WorkDone;
                appointment.TechnicianName = retrieveAppointment.TechnicianName;

                TemporaryAppointment.Add(appointment);
            }

            MyDataGrid.ItemsSource = TemporaryAppointment;

            FillDataContext();

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
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.WorkDoneList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Work Done", SelectedValueBinding = new Binding("WorkDone") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("TechnicianName"), Header = "Technician" });
        }

        //Need to fill datacontext for combobox value to load in datagrid upon retrieving.
        private void FillDataContext()
        {
            BrandList = new ObservableCollection<string>(Enum.GetNames(typeof(BrandEnum)));
            WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
        }

        //Clearing of fields.
        private void ClearFields()
        {
            customerNameTxt.Clear();
            appTimeCmbBox.SelectedIndex = -1;
            creditCardNoTxt.Clear();
            brandCmbBox.SelectedIndex = -1;
            modelCmbBox.SelectedIndex = -1;
            workDoneCmbBox.SelectedIndex = -1;
            technicianNameTxt.Clear();
        }

        //Clearing the datagrid
        private void ClearGrid()
        {
            MyDataGrid.ItemsSource = null;
            MyDataGrid.Columns.Clear();
            MyDataGrid.Items.Clear();
            MyDataGrid.Items.Refresh();
        }


    }
}

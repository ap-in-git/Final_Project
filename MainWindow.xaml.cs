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
        public enum LaptopTypeEnum { GamingLaptop, NetBook, UltraBook, NoteBook }
        public enum BrandEnum { Lenovo, Asus, MSI, Alienware, Dell, Gigabyte }

        public enum WorkDoneEnum { Cleaning, Installing, Fixing }

        AppointmentList appList = new();

        ObservableCollection<Appointment> appointmentMade = null;

        public ObservableCollection<string> LaptopTypeList { get; set; }
        public ObservableCollection<string> BrandList { get; set; }


        public ObservableCollection<string> WorkDoneList { get; set; }
        public ObservableCollection<int> AppTimeList { get; set; }

        Laptop laptop = null;


        public ObservableCollection<Appointment> AppointmentMade { get => appointmentMade; set => appointmentMade = value; }

        public MainWindow()
        {
            InitializeComponent();
            AppointmentMade = new ObservableCollection<Appointment>();
            LaptopTypeList = new ObservableCollection<string>(Enum.GetNames(typeof(LaptopTypeEnum)));
            WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
            BrandList = new ObservableCollection<string>();
            addBtn.IsEnabled = false;

            AppTimeList = new ObservableCollection<int>();
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
        private void LaptopCmbBox_Init(object sender, EventArgs e)
        {
            laptopCmbBox.ItemsSource = Enum.GetNames(typeof(LaptopTypeEnum));
        }

        private void BrandCmbBox_Init(object sender, EventArgs e)
        {
            brandCmbBox.ItemsSource = Enum.GetNames(typeof(BrandEnum));
        }



        //Brand of every laptop type. TODO: Add possible overriding method here
        private void LaptopModel(string selectedText)
        {
            BrandList = new ObservableCollection<string>(Enum.GetNames(typeof(BrandEnum)));
            if (selectedText == "GamingLaptop")
            {
                laptop = new GamingLaptop();
                //BrandList = new ObservableCollection<string>(laptop.BrandModelList());
                //modelCmbBox.ItemsSource = BrandList;
            }
            else if (selectedText == "UltraBook")
            {
                laptop = new UltraBook();
                //UltraBookList = new ObservableCollection<string>(laptop.BrandModelList());
                //modelCmbBox.ItemsSource = UltraBookList;
            }
            else if (selectedText == "NoteBook")
            {
                laptop = new NoteBook();
                //NoteBookList = new ObservableCollection<string>(laptop.BrandModelList());
                //modelCmbBox.ItemsSource = NoteBookList;
            }
            else if (selectedText == "NetBook")
            {
                laptop = new NetBook();
                //NetBookList = new ObservableCollection<string>(laptop.BrandModelList());
                //modelCmbBox.ItemsSource = NetBookList;
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
            var arr = appTimeCmbBox.Items.Cast<int>().Select(item => item).ToArray();

            AppTimeList = new ObservableCollection<int>(arr);
            Appointment appointment = new();
            LaptopModel((string)laptopCmbBox.SelectedItem);
            appointment.CustomerLaptop = laptop;
            appointment.AppointmentTime = (int)appTimeCmbBox.SelectedItem;
            appointment.CustomerName = customerNameTxt.Text;
            appointment.CreditCardNo = creditCardNoTxt.Text;
            appointment.CustomerLaptop.LaptopType = (string)laptopCmbBox.SelectedItem;
            appointment.CustomerLaptop.Brand = (string)brandCmbBox.SelectedItem;
            appointment.CustomerLaptop.Model = modelTxt.Text;
            appointment.WorkDone = (string)workDoneCmbBox.SelectedItem;
            appointment.TechnicianName = technicianNameTxt.Text;

            appTimeCmbBox.Items.Remove(appointment.AppointmentTime);

            string selectedText = appointment.CustomerLaptop.LaptopType;
            AppointmentMade.Add(appointment);

            MyDataGrid.ItemsSource = AppointmentMade;

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("AppointmentTime"), Header = "Time", IsReadOnly = true });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerName"), Header = "Name" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CreditCardNo"), Header = "Credit Card" });

            var style = new Style(typeof(ComboBox));
            style.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.LaptopTypeList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Brand", SelectedValueBinding = new Binding("CustomerLaptop.LaptopType") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = style, EditingElementStyle = style });

            var modelStyle = new Style(typeof(ComboBox));
            modelStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.BrandList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Model", SelectedValueBinding = new Binding("CustomerLaptop.Brand") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = modelStyle, EditingElementStyle = modelStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerLaptop.Model"), Header = "Model" });

            var workStyle = new Style(typeof(ComboBox));
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.WorkDoneList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Work Done", SelectedValueBinding = new Binding("WorkDone") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("TechnicianName"), Header = "Technician" });

             ClearFields();

        }



        //Save Button. Responsible for saving the values in the datagrid to XML.
        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AppointmentMade.Count > 0)
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
            foreach (Appointment appointment in AppointmentMade)
            {
                appList.Add(appointment);
            }
            XmlSerializer serializer = new(typeof(AppointmentList), new Type[] { typeof(Appointment) });
            TextWriter tw = new StreamWriter("Appointment.xml");
            serializer.Serialize(tw, appList);
            tw.Close();

        }

        //Retrieve button. Responsible for retrieving data from XML format to the datagrid.
        private void GetAllButton_Click(object sender, RoutedEventArgs e)
        {
            //This is for the datagrid if it contains unsave items
            MessageBoxResult result = MessageBoxResult.No;
            if (AppointmentMade.Count > 0)
            {
                result = MessageBox.Show("You have items in the grid. Do you want to save them all?", "Save all client", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveAll();
                }
            }
            ClearGrid();

            //Open the file written below and read values from it.
            XmlSerializer serializer = new(typeof(AppointmentList), new Type[] { typeof(Appointment) });
            TextReader tr = new StreamReader("Appointment.xml");
            appList = (AppointmentList)serializer.Deserialize(tr);
            tr.Close();

            for (int i = 0; i < appList.Count(); i++)
            {
                Appointment retrieveAppointment = appList[i];
                Appointment appointment = new();
                LaptopModel(retrieveAppointment.CustomerLaptop.LaptopType);
                appointment.CustomerLaptop = laptop;
                appointment.AppointmentTime = retrieveAppointment.AppointmentTime;
                appointment.CustomerName = retrieveAppointment.CustomerName;
                appointment.CreditCardNo = retrieveAppointment.CreditCardNo;
                appointment.CustomerLaptop.LaptopType = retrieveAppointment.CustomerLaptop.LaptopType;
                appointment.CustomerLaptop.Brand = retrieveAppointment.CustomerLaptop.Brand;
                appointment.CustomerLaptop.Model = retrieveAppointment.CustomerLaptop.Model;
                appointment.WorkDone = retrieveAppointment.WorkDone;
                appointment.TechnicianName = retrieveAppointment.TechnicianName;

                AppointmentMade.Add(appointment);

                appTimeCmbBox.Items.Remove(appointment.AppointmentTime);
            }

            MyDataGrid.ItemsSource = AppointmentMade;

            var arr = appTimeCmbBox.Items.Cast<Object>().Select(item => item.ToString()).ToArray();

            List<string> list = new List<string>(arr);

            FillDataContext();

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("AppointmentTime"), Header = "Time" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerName"), Header = "Name" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CreditCardNo"), Header = "Credit Card" });



            var style = new Style(typeof(ComboBox));
            style.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.LaptopTypeList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Brand", SelectedValueBinding = new Binding("CustomerLaptop.LaptopType") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = style, EditingElementStyle = style });

            var modelStyle = new Style(typeof(ComboBox));
            modelStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.BrandList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Model", SelectedValueBinding = new Binding("CustomerLaptop.Brand") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = modelStyle, EditingElementStyle = modelStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerLaptop.Model"), Header = "Model" });

            var workStyle = new Style(typeof(ComboBox));
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.WorkDoneList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Work Done", SelectedValueBinding = new Binding("WorkDone") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("TechnicianName"), Header = "Technician" });
        }

        //Need to fill datacontext for combobox value to load in datagrid upon retrieving.
        private void FillDataContext()
        {
            WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
        }

        //Clearing of fields.
        private void ClearFields()
        {
            customerNameTxt.Clear();
            appTimeCmbBox.SelectedIndex = -1;
            creditCardNoTxt.Clear();
            brandCmbBox.SelectedIndex = -1;
            laptopCmbBox.SelectedIndex = -1;
            workDoneCmbBox.SelectedIndex = -1;
            technicianNameTxt.Clear();
            laptop = null;
        }

        //Clearing the datagrid
        private void ClearGrid()
        {
            MyDataGrid.ItemsSource = null;
            MyDataGrid.Columns.Clear();
            MyDataGrid.Items.Clear();
            MyDataGrid.Items.Refresh();
            AppointmentMade = new ObservableCollection<Appointment>();

        }

        private void appTimeCmbBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            if(appTimeCmbBox.SelectedIndex == -1)
            {
                addBtn.IsEnabled = false;
            }
            else
            {
                addBtn.IsEnabled = true;
            }
        }
    }
}

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

        public List<string> searchList = new() { "All", "Customer Name", "Technician Name", "Laptop Brand" };

        public enum WorkDoneEnum { Cleaning, Installing, Fixing }

        AppointmentList appList = new();
        Appointment appointment = new();

        public Appointment Appointment { get => appointment; set => appointment = value; }

        ObservableCollection<Appointment> appointmentMade = null;
        ObservableCollection<Appointment> tempAppointmentMade = null;

        public ObservableCollection<string> LaptopTypeList { get; set; }
        public ObservableCollection<string> BrandList { get; set; }

        public ObservableCollection<string> WorkDoneList { get; set; }
        public ObservableCollection<string> AppTimeList { get; set; }

        Laptop laptop = null;
        public ObservableCollection<Appointment> AppointmentMade { get => appointmentMade; set => appointmentMade = value; }
        public ObservableCollection<Appointment> TempAppointmentMade { get => tempAppointmentMade; set => tempAppointmentMade = value; }

        public MainWindow()
        {
            InitializeComponent();
            AppointmentMade = new ObservableCollection<Appointment>();
            TempAppointmentMade = new ObservableCollection<Appointment>();
            LaptopTypeList = new ObservableCollection<string>(Enum.GetNames(typeof(LaptopTypeEnum)));
            WorkDoneList = new ObservableCollection<string>(Enum.GetNames(typeof(WorkDoneEnum)));
            BrandList = new ObservableCollection<string>();
            addBtn.IsEnabled = false;
            AppTimeList = new ObservableCollection<string>();
            DataContext = this;

        }

 

        //Appointment time combo box initialization
        private void AppTimeCmbBox_Init(object sender, EventArgs e)
        {
            for (int i = workStart; i <= workEnd; i++)
            {
                string displayString = i.ToString() + ":00-" + i.ToString() + ":59 ";
                appTimeCmbBox.Items.Add(displayString);
            }

        }

        //Laptop Type combo box initialization
        private void LaptopCmbBox_Init(object sender, EventArgs e)
        {
            laptopCmbBox.ItemsSource = Enum.GetNames(typeof(LaptopTypeEnum));
        }

        //Brand combo box initialization
        private void BrandCmbBox_Init(object sender, EventArgs e)
        {
            brandCmbBox.ItemsSource = Enum.GetNames(typeof(BrandEnum));
        }

        //Work done combo box initialization
        private void WorkDone_Init(object sender, EventArgs e)
        {
            workDoneCmbBox.ItemsSource = Enum.GetNames(typeof(WorkDoneEnum));

        }

        //Search combo box initialization
        private void SearchCmbBox_Init(object sender, EventArgs e)
        {
            searchCmbBox.ItemsSource = searchList;
        }

        //Brand of every laptop type.
        private void LaptopModel(string selectedText, bool flag)
        {
            BrandList = new ObservableCollection<string>(Enum.GetNames(typeof(BrandEnum)));
            if (selectedText == "GamingLaptop")
            {
                laptop = new GamingLaptop();
            }
            else if (selectedText == "UltraBook")
            {
                laptop = new UltraBook();
            }
            else if (selectedText == "NoteBook")
            {
                laptop = new NoteBook();
            }
            else if (selectedText == "NetBook")
            {
                laptop = new NetBook();
            }

            //Flag is for disabling the text field if user use the grid for retrieving values.
            if (flag)
            {
                specialWorkTxt.Text = laptop.SpecialWorkDone();
            }
            specialWorkTxt.IsEnabled = false;
        }

        //Event trigger for the appointment time combo box.
        private void AppTimeCmbBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            addBtn.IsEnabled = appTimeCmbBox.SelectedIndex != -1 ? true : false;
        }

        //Event trigger for the Laptop combo box.
        private void LaptopCmbBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            LaptopModel((string)laptopCmbBox.SelectedItem, true);
        }

        //Event trigger for the search combo box.
        private void SearchCmbBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            searchlbl.IsEnabled = false;
            searchlbl.Content = "";
            searchTxt.IsEnabled = searchCmbBox.SelectedIndex != 0;
        } 

        //Add button. This button is responsible for transferring data from the fields to the datagrid.
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            AppointmentMade = new ObservableCollection<Appointment>();
            if (!Validation.GetHasError(creditCardNoTxt) && ValidateFields())
            {
                var arr = appTimeCmbBox.Items.Cast<string>().Select(item => item).ToArray();

                AppTimeList = new ObservableCollection<string>(arr);
                Appointment appointment = new();
                LaptopModel((string)laptopCmbBox.SelectedItem, true);
                appointment.CustomerLaptop = laptop;
                appointment.AppointmentTime = (string)appTimeCmbBox.SelectedItem;
                appointment.CustomerName = customerNameTxt.Text;
                appointment.CreditCardNo = creditCardNoTxt.Text;
                appointment.CustomerLaptop.LaptopType = (string)laptopCmbBox.SelectedItem;
                appointment.CustomerLaptop.Brand = (string)brandCmbBox.SelectedItem;
                appointment.CustomerLaptop.Model = modelTxt.Text;
                appointment.WorkDone = (string)workDoneCmbBox.SelectedItem;
                appointment.TechnicianName = technicianNameTxt.Text;

                appTimeCmbBox.Items.Remove(appointment.AppointmentTime);

                string selectedText = appointment.CustomerLaptop.LaptopType;
                TempAppointmentMade.Add(appointment);

                MyDataGrid.ItemsSource = TempAppointmentMade;
                SetDataGrid();
                ClearFields();
            }
        }

        //Save Button. Responsible for saving the values in the datagrid to XML.
        private void SaveAllBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TempAppointmentMade.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save them all?", "Save all client", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveAll(TempAppointmentMade);
                    ClearGrid();
                }
            }
            else if (AppointmentMade.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to update your data?", "Save all client", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    SaveAll(AppointmentMade);
                    ClearGrid();
                }
            }
        }

        //Saving the data into XML format
        private void SaveAll(ObservableCollection<Appointment> app)
        {
            appList.Clear();
            foreach (Appointment appointment in app)
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
            if (null != TempAppointmentMade && TempAppointmentMade.Count > 0)
            {
                result = MessageBox.Show("You have items in the grid. Do you want to save them all?", "Save all client", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    SaveAll(TempAppointmentMade);
                }
            }
            ClearGrid();

            string searchText = searchTxt.Text;
            if (searchCmbBox.SelectedIndex == -1)
            {
                searchlbl.IsEnabled = true;
                searchlbl.Content = "Please select one";
                searchlbl.Foreground = Brushes.Red;
            }
            else
            {
                //Open the file written below and read values from it.
                XmlSerializer serializer = new(typeof(AppointmentList), new Type[] { typeof(Appointment) });
                TextReader tr = new StreamReader("Appointment.xml");
                appList = (AppointmentList)serializer.Deserialize(tr);
                tr.Close();

                for (int i = 0; i < appList.Count(); i++)
                {
                    Appointment retrieveAppointment = appList[i];
                    Appointment appointment = new();
                    LaptopModel(retrieveAppointment.CustomerLaptop.LaptopType, false);
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
                }

                //Using LINQ to search via field
                if (searchCmbBox.SelectedIndex == 1)
                {

                    var query = from a in AppointmentMade
                                where a.CustomerName.Equals(searchText, StringComparison.OrdinalIgnoreCase)
                                select a;
                    MyDataGrid.ItemsSource = query;
                }
                else if (searchCmbBox.SelectedIndex == 2)
                {
                    var query = from a in AppointmentMade
                                where a.TechnicianName.Equals(searchText, StringComparison.OrdinalIgnoreCase)
                                select a;
                    MyDataGrid.ItemsSource = query;
                }
                else if (searchCmbBox.SelectedIndex == 3)
                {
                    var query = from a in AppointmentMade
                                where a.CustomerLaptop.Brand.Equals(searchText, StringComparison.OrdinalIgnoreCase)
                                select a;
                    MyDataGrid.ItemsSource = query;
                }
                else
                {
                    MyDataGrid.ItemsSource = AppointmentMade;
                }

                FillDataContext();
                SetDataGrid();
            }
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
            specialWorkTxt.Clear();
            modelTxt.Clear();
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
            TempAppointmentMade = new ObservableCollection<Appointment>();

        }

        //Refactored setting of data in the datagrid via add button or search button
        private void SetDataGrid()
        {
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("AppointmentTime"), Header = "Time", IsReadOnly = true });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerName"), Header = "Name" });
            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CreditCardNo"), Header = "Credit Card" });

            var style = new Style(typeof(ComboBox));
            style.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.LaptopTypeList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Type", SelectedValueBinding = new Binding("CustomerLaptop.LaptopType") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = style, EditingElementStyle = style });

            var modelStyle = new Style(typeof(ComboBox));
            modelStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.BrandList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Brand", SelectedValueBinding = new Binding("CustomerLaptop.Brand") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = modelStyle, EditingElementStyle = modelStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("CustomerLaptop.Model"), Header = "Model" });

            var workStyle = new Style(typeof(ComboBox));
            workStyle.Setters.Add(new Setter(ComboBox.ItemsSourceProperty, new Binding("DataContext.WorkDoneList") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) } }));
            MyDataGrid.Columns.Add(new DataGridComboBoxColumn { Header = "Work Done", SelectedValueBinding = new Binding("WorkDone") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, ElementStyle = workStyle, EditingElementStyle = workStyle });

            MyDataGrid.Columns.Add(new DataGridTextColumn { Binding = new Binding("TechnicianName"), Header = "Technician" });
        }

        //TODO: Add an error message in UI
        private bool ValidateFields()
        {
            bool validate = customerNameTxt.Text != String.Empty  && laptopCmbBox.SelectedIndex != -1 && brandCmbBox.SelectedIndex != -1
                && modelTxt.Text != String.Empty && workDoneCmbBox.SelectedIndex != -1 && technicianNameTxt.Text != String.Empty;

            return validate;
        }

    }
}

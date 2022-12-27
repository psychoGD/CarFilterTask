using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using CarFilterTask.Entities;
using Dapper;
namespace CarFilterTask
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Car> CarsDatas { get; set; }
        public List<string> Colors { get; set; } = new List<string>();

        private string selectedCarBrand;

        public string SelectedCarBrand
        {
            get { return selectedCarBrand; }
            set { selectedCarBrand = value; }
        }

        private string selectedCarType;

        public string SelectedCarType
        {
            get { return selectedCarType; }
            set { selectedCarType = value; }
        }

        private string selectedCarColor;

        public string SelectedCarColor
        {
            get { return selectedCarColor; }
            set { selectedCarColor = value; }
        }


        public int minKilometer { get; set; }
        public int maxKilometer { get; set; }

        public int minYear { get; set; }
        public int maxYear { get; set; }

        public double minPrice { get; set; }
        public double maxPrice { get; set; }

        public bool IsNew { get; set; } 
        public bool IsOld { get; set; } 
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext= this;
            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var query = @"
SELECT * 
FROM Car 
INNER JOIN Brand
ON Car.BrandId=Brand.Id
INNER JOIN CarType
ON CarType.Id=Car.TypeId";

                CarsDatas = connection.Query<Car, Brand, Cartype, Car>(query,
                    (car, brand, type) =>
                    {
                        car.BrandId = brand.Id;
                        car.Brand = brand;
                        car.TypeId = type.Id;
                        car.Cartype = type;
                        return car;
                    }).ToList();
                foreach (var item in CarsDatas)
                {
                    Marka.Items.Add(item.Brand.Name);
                    BanNovu.Items.Add(item.Cartype.Name);
                    var exist = Colors.Contains(item.Color);
                    if (!exist)
                    {
                        Colors.Add(item.Color);
                    }
                }
                ColorComboBox.ItemsSource = Colors;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {




            var Uc = new Cars();
            var DatasCopy = new List<Car>(CarsDatas);


            if (IsNew == true)
            {
                DatasCopy = DatasCopy.Where(c => c.IsNew == IsNew).ToList();
            }
            else if (IsOld == true)
            {
                DatasCopy = CarsDatas.Where(c => c.IsNew == false).ToList();
            }
            

            if (SelectedCarBrand != null)
                DatasCopy = DatasCopy.Where(c => c.Brand.Name == SelectedCarBrand).ToList();
            if (SelectedCarType != null)
                DatasCopy = DatasCopy.Where(c => c.Cartype.Name == SelectedCarType).ToList();
            
            //Price Filter
            if (minPrice != 0)
                DatasCopy = DatasCopy.Where(c => c.Price >= minPrice).ToList();
            if (maxPrice != 0)
                DatasCopy = DatasCopy.Where(c => c.Price <= maxPrice).ToList();

            //Year Filter
            if (minYear != 0)
                DatasCopy = DatasCopy.Where(c => c.Year >= minYear).ToList();
            if (maxYear != 0)
                DatasCopy = DatasCopy.Where(c => c.Year <= maxYear).ToList();

            //Kilometer Filter
            if(minKilometer != 0)
                DatasCopy = DatasCopy.Where(c => c.Kilometers >= minKilometer).ToList();
            if (maxKilometer != 0)
                DatasCopy = DatasCopy.Where(c => c.Kilometers <= maxKilometer).ToList();

            if(SelectedCarColor!=null)
                DatasCopy = DatasCopy.Where(c => c.Color == SelectedCarColor).ToList();
            //var datasCopy = CarsDatas.Where(c => c.Brand.Name == SelectedCarBrand || c.Cartype.Name == SelectedCarType || c.IsNew == IsNewCB.IsChecked ||
            //c.Price <= minPrice || c.Price >= maxPrice ||
            //c.Year >= minYear || c.Year <= maxYear ||
            //c.Kilometers >= minKilometer || c.Kilometers <= maxKilometer
            //);

            //Uc.CarsDatas = datasCopy.ToList();
            //Uc.CarsDatas = CarsDatas;
            Uc.MyDataGrid.ItemsSource = DatasCopy;
            MainDataGrid.Children.Add(Uc);

        }

        
    }
}

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

        public MainWindow()
        {
            InitializeComponent();
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
                        car.BrandId= brand.Id;
                        car.Brand = brand;
                        car.TypeId = type.Id;
                        car.Cartype = type;
                        return car;
                    }).ToList();
                foreach (var item in CarsDatas)
                {
                    Marka.Items.Add(item.Brand.Name);
                    BanNovu.Items.Add(item.Cartype.Name);

                    ColorComboBox.Items.Add(item.Color);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          

                var Uc = new Cars();

                Uc.MyDataGrid.ItemsSource = CarsDatas;

                MainDataGrid.Children.Add(Uc);
        }
    }
}

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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var conn = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            using (var connection = new SqlConnection(conn))
            {
                var query = @"
SELECT * 
FROM Car 
INNER JOIN Brand
ON Car.BrandId=Brand.Id
INNER JOIN Type
ON Type.Id=Car.TypeId";
                var cars = connection.Query<Car,Brand,Type,Car>(query,
                    (car, brand, type) =>
                    {
                        car.Brand = brand;
                        car.Type= type;
                        return car;
                    },splitOn:"BrandId,TypeId").ToList();
                var Uc = new Cars();
                Uc.CarsDatas= cars;
                MainDataGrid.Children.Add(Uc);
            }
        }
    }
}

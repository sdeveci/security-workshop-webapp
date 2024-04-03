using BuildSecureNorthwind.DataAccess.Entities;
using BuildSecureNorthwind.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BuildSecureNorthwind.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration configuration;

        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            var products = GetProducts();
            var model = new ProductIndexViewModel
            {
                Products = products
            };

            return View(model);
        }

        public IActionResult Search(string q)
        {
            var products = SearchProducts(q);
            var model = new ProductIndexViewModel
            {
                Products = products
            };

            return View("Index", model);
        }

        private IEnumerable<Product> GetProducts()
        {
            using var connection = new SqlConnection(this.configuration.GetConnectionString("NorthwindConnStr"));
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            using var command = new SqlCommand("select * from Products", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var product = new Product()
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Stock = int.Parse(reader["Stock"].ToString()),
                    UnitPrice = decimal.Parse(reader["UnitPrice"].ToString()),
                };

                yield return product;
            }

        }

        private IEnumerable<Product> SearchProducts(string expression)
        {
            using var connection = new SqlConnection(this.configuration.GetConnectionString("NorthwindConnStr"));
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            //SQL Injection zaafiyeti oluşur
            //using var command = new SqlCommand("select * from Products where Name like '%"+expression+"%'", connection);

            using var command = new SqlCommand("select * from Products where Name like '%@productName%'", connection);
            command.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@productName",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                SqlValue = expression
            });

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var product = new Product()
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Stock = int.Parse(reader["Stock"].ToString()),
                    UnitPrice = decimal.Parse(reader["UnitPrice"].ToString()),
                };

                yield return product;
            }
        }
    }
}

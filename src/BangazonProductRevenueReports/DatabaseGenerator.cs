using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonProductRevenueReports
{
    public class DatabaseGenerator
    {
        Random rnd = new Random();

        string[] customers = new[] { "Carys", "Emmett", "Latoya", "Trina", "Kade", "Torin", "Aggie", "Caelan", "Patsy", "Bettina", "Hans", "Leda", "Clair", "Evan", "Roscoe", "Sondra", "Dixon", "Gail" };
        string[] products = new[] { "Rug", "Wine Glasses", "Book Ends", "Picture Frame", "Blu-Ray Player", "Digital Camera", "Stuffed Animal - Monkey", "Stuffed Animal - Sloth", "Spatula", "Crayons", "Headphones", "Lawn Furniture", "Hammer", "Computer Monitor", "Golf Clubs", "Raspberry Pi", "eReader" };
        int[] productprice = new[] { 57, 21, 16, 22, 95, 257, 4, 5, 10, 2, 53, 150, 25, 950, 860, 45, 250 };
        int[] productrevenue = new[] { 3, 1, 4, 2, 15, 52, 1, 1, 1, 1, 5, 53, 3, 24, 169, 10, 9 };
        string[] customerAddressNumbers = new[] { "123", "435", "44", "283a", "6b", "1440", "7723", "289", "7564", "985-b", "33922", "23", "546", "5692", "6780", "9362", "121", "74567", "18", "9" };
        string[] customerAddressStreet = new[] { "Mallory Lane", "Carothers Pkwy", "Claybrook Lane", "Bending Creek Drive", "Old Hickory Blvd", "Harris Ave", "21st Ave N", "Plus Park Blvd", "Interstate Blvd S", "Whitney Ave", "Bell Rd", "Harding Pky", "Nolesville Road", "Charlotte Ave" };

        public string RandomizeCustomerProducts()
        {
            var rnd1 = rnd.Next(customers.Length);
            var rnd2 = rnd.Next(products.Length);
            var rnd3 = rnd.Next(customerAddressNumbers.Length);
            var rnd4 = rnd.Next(customerAddressStreet.Length);

            DateTime start = DateTime.Today.AddDays(-200);
            int range = (DateTime.Today - start).Days;

            return string.Format("INSERT INTO [dbo].[Revenue] VALUES('{0}', {1}, {2}, '{3}', '{4} {5}', '{6}') ", products[rnd2], productprice[rnd2], productrevenue[rnd2], customers[rnd1], customerAddressNumbers[rnd3], customerAddressStreet[rnd4], start.AddDays(rnd.Next(range)));
        }

        public string RandomizeCustomerProducts(int numOfEntries)
        {
            string returnstring = "";
            for (var i = 0; i < numOfEntries; i++) { returnstring += RandomizeCustomerProducts(); }
            return returnstring;
        }

        public void CreateDatabase()
        {
            string sql1 = "USE Master " +
                            "IF EXISTS (" +
                            "SELECT name FROM sys.databases WHERE name = N'BangProductRevenueReports' " +
                            ") DROP DATABASE BangProductRevenueReports ";
            string sql2 = "Use Master " +
                            "CREATE DATABASE BangProductRevenueReports ";
            string sql3 = "Use BangProductRevenueReports " +
                            "CREATE TABLE [dbo].[Revenue] (" +
                                "[Id] INT PRIMARY KEY NOT NULL IDENTITY, " +
                                "[ProductName] varchar(50) NOT NULL, " +
                                "[ProductCost] int not null," +
                                "[ProductRevenue] int not null, " +
                                "[CustomerName] varchar(50) not null, " +
                                "[CustomerAddress] varchar(100) not null, " +
                                "[PurchaseDate] Datetime not null " +
                            ") "
                            + RandomizeCustomerProducts(1000);

            SqlConnection connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB");
            using (connection)
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql1, connection);
                SqlCommand command2 = new SqlCommand(sql2, connection);
                SqlCommand command3 = new SqlCommand(sql3, connection);
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
            }
        }
    }
}

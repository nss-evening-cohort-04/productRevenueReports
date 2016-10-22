using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonProductRevenueReports
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Comment out these two lines for speed purposes after the initial db creation 
            //Uncomment them and run to generate fresh data
            DatabaseGenerator gen = new DatabaseGenerator();
            gen.CreateDatabase();

            SqlCommand cs = new SqlCommand();
            cs.Connection = new SqlConnection(@"server=(localdb)\MSSQLLocalDB");
            cs.CommandType = CommandType.Text;
            SqlDataReader reader;
            List<KeyValuePair<string, int>> reportValues = new List<KeyValuePair<string, int>>();

            Console.WriteLine("Bangazon Reports");

            while (true)
            {
                Console.WriteLine("1 - Last Week Report");
                Console.WriteLine("2 - Last Month Report");
                Console.WriteLine("3 - Last 3 months Report");
                Console.WriteLine("4 - Rev by customer");
                Console.WriteLine("5 - Rev by product");

                var stuff = Console.ReadLine();

                switch (stuff)
                {
                    case "1":
                        cs.CommandText = string.Format("SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue] WHERE PurchaseDate >= '{0}'", DateTime.Today.AddDays(-7));
                        cs.Connection.Open();
                        reader = cs.ExecuteReader();
                        while (reader.Read())
                        {
                            reportValues.Add(new KeyValuePair<string, int>(reader[1].ToString(), int.Parse(reader[3].ToString())));
                        }
                        
                        foreach (var val in reportValues)
                        {
                            Console.WriteLine(string.Format("{0} was purchased with ${1}.00 in revenue.", val.Key, val.Value));
                        }
                        break;
                    case "2":
                        cs.CommandText = string.Format("SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue] WHERE PurchaseDate >= '{0}'", DateTime.Today.AddDays(-30));
                        cs.Connection.Open();
                        reader = cs.ExecuteReader();
                        while (reader.Read())
                        {
                            reportValues.Add(new KeyValuePair<string, int>(reader[1].ToString(), int.Parse(reader[3].ToString())));
                        }

                        foreach (var val in reportValues)
                        {
                            Console.WriteLine(string.Format("{0} was purchased with ${1}.00 in revenue.", val.Key, val.Value));
                        }
                        break;
                    case "3":
                        cs.CommandText = string.Format("SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue] WHERE PurchaseDate >= '{0}'", DateTime.Today.AddDays(-90));
                        cs.Connection.Open();
                        reader = cs.ExecuteReader();
                        while (reader.Read())
                        {
                            reportValues.Add(new KeyValuePair<string, int>(reader[1].ToString(), int.Parse(reader[3].ToString())));
                        }

                        foreach (var val in reportValues)
                        {
                            Console.WriteLine(string.Format("{0} was purchased with ${1}.00 in revenue.", val.Key, val.Value));
                        }
                        break;
                    case "4":
                        cs.CommandText = string.Format("SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue]");
                        cs.Connection.Open();
                        reader = cs.ExecuteReader();
                        Dictionary<string, int> customerReportValues = new Dictionary<string, int>();
                        while (reader.Read())
                        {
                            if(customerReportValues.ContainsKey(reader[4].ToString()))
                            {
                                customerReportValues[reader[4].ToString()] += int.Parse(reader[3].ToString());
                            }
                            else
                            {
                                customerReportValues.Add(reader[4].ToString(), int.Parse(reader[3].ToString()));
                            }
                        }

                        foreach (var val in customerReportValues)
                        {
                            Console.WriteLine(string.Format("{0} purchased items with a total of ${1}.00 in revenue.", val.Key, val.Value));
                        }
                        break;
                    case "5":
                        cs.CommandText = string.Format("SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue]");
                        cs.Connection.Open();
                        reader = cs.ExecuteReader();
                        Dictionary<string, int> productsReportValues = new Dictionary<string, int>();
                        while (reader.Read())
                        {
                            if (productsReportValues.ContainsKey(reader[1].ToString()))
                            {
                                productsReportValues[reader[1].ToString()] += int.Parse(reader[3].ToString());
                            }
                            else
                            {
                                productsReportValues.Add(reader[1].ToString(), int.Parse(reader[3].ToString()));
                            }
                        }

                        foreach (var val in productsReportValues)
                        {
                            Console.WriteLine(string.Format("{0} brought in a total of ${1}.00 in revenue.", val.Key, val.Value));
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Try Again.");
                        break;
                }
                Console.ReadKey();
            }
        }
    }
}

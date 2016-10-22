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
            
            //List<string> Names = new List<string>();
            //List<string> Values = new List<string>()
            List<KeyValuePair<string, int>> reportValues = new List<KeyValuePair<string, int>>();

            Console.WriteLine("Bangazon Reports");
            bool go_on = true;

            while (go_on)
            {
                try
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
                            cs.CommandText = "SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue]";
                            cs.Connection.Open();
                            reader = cs.ExecuteReader();
                            //var proDict = new Dictionary<string, int>();
                            while (reader.Read())
                            {
                                var i = reader[1];
                                var hh = i.ToString();
                                var a = reader[3];
                                var t = a.ToString();
                                var e = int.Parse(t);
                                var r = reader[9];
                                //  e
                                var p = r.ToString();
                                var o = DateTime.Parse(p);
                                //  r
                                //  t
                                var s = DateTime.Today.AddDays(-7);
                                var straightupbull = new KeyValuePair<string, int>(hh, e);
                                if (o > s)
                                {
                                    //proDict.Add(h, e); throws error GRRRRRR      
                                    reportValues.Add(straightupbull);
                                }
                            }

                            foreach (var y in reportValues)
                            {
                                Console.WriteLine(string.Format("{0} was purchased with ${1}.00 in revenue.", y.Key, y.Value));
                            }
                            break;
                        case "2":
                            cs.CommandText = "SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue]";
                            //
                            cs.Connection.Open();
                            reader = cs.ExecuteReader();
                            while (reader.Read())
                            {
                                var hhh = reader[1];
                                var o = hhh.ToString();
                                var t = reader[3];


                                var c = t.ToString();
                                //  h
                                var i = int.Parse(c);
                                var k = reader[9];
                                var e = k.ToString();
                                var n = DateTime.Parse(e);
                                var s = DateTime.Today.AddDays(-30);



                                if (n > s)
                                {
                                    reportValues.Add(new KeyValuePair<string, int>(o, i));
                                }
                            }

                            foreach (var y in reportValues)
                            {
                                var z = y.Key;
                                var x = y.Value;
                                var str = string.Format("{0} was purchased with ${1}.00 in revenue.", z, x);
                                Console.WriteLine(str);
                            }
                            break;
                        case "3":
                                var h = DateTime.Today.AddDays(-90);
                            
                            cs.CommandText = "SELECT * FROM [BangProductRevenueReports].[dbo].[Revenue] WHERE PurchaseDate >= " + h;
                            cs.Connection.Open();
                            reader = cs.ExecuteReader();
                            while (reader.Read())
                            {
                                var i = reader[1];
                                var l = i.ToString();
                                var o = reader[3];
                                var v = o.ToString();
                                var e = int.Parse(v);
                                    reportValues.Add(new KeyValuePair<string, int>(l, e));
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
                            //LIST DOESN'T WORK NEED DICTIONARY TO CHANGE VALUES
                            Dictionary<string, int> customerReportValues = new Dictionary<string, int>();
                            while (reader.Read())
                            {
                                if (customerReportValues.ContainsKey(reader[4].ToString()))
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
                            
                            //THERE HAS TO BE A BETTER WAY TO SORT
                               Dictionary<string, int> productRevenue = new Dictionary<string, int>();
                               SortedList<int, string> sortProductRevenue = new SortedList<int, string>();
                               while (reader.Read())
                               {
                                   if (productRevenue.ContainsKey(reader[1].ToString()))
                                   {
                                       productRevenue[reader[1].ToString()] += int.Parse(reader[3].ToString());
                                   }
                                   else
                                   {
                                       productRevenue.Add(reader[1].ToString(), int.Parse(reader[3].ToString()));
                                   }
                               }
                               foreach (var entry in productRevenue)
                               {
                                   sortProductRevenue.Add(entry.Value, entry.Key);
                               }
                               foreach (var entry in sortProductRevenue)
                               {
                                   Console.WriteLine(string.Format("Product: {0} Revenue: {1}", entry.Value, entry.Key));
                               }

                               //JUST IN CASE SORTING DOESN"T WORK
                            /*Dictionary<string, int> productsReportValues = new Dictionary<string, int>();
                            while (reader.Read())
                            {
                               
                                //Dictionary<string, int> productsReportValues = new Dictionary<string, int>();
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
                            }*/
                            break;
                        default:
                            Console.WriteLine("Invalid input. Try Again.");
                            break;
                    }
                    Console.ReadKey();
                }
                catch (Exception e)
                {
                    //ADDING ERROR HANDLING
                    Console.WriteLine("Sorry an error has occcured. Please try agin ");
                    go_on = false;
                    Console.ReadKey();
                }

            }       
        }
    }
}

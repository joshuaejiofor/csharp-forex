using Forex.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Forex
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Orders.txt");
            List<Order> orders = new List<Order>();
            int count = 0;

            // Load data
            foreach (var line in lines)
            {
                var content = line.Split(',');
                var order = new Order
                {
                    OrderNo = ++count, 
                    Type = content[0],
                    Quantity = Convert.ToDecimal(content[1]),
                    Price = Convert.ToDecimal(content[2]),
                    Date = Convert.ToDateTime(content[3])
                };

                var matchList = orders.Where(c => c.Type != order.Type && c.Quantity > 0 &&
                                        ((order.Type == "buy" && order.Price >= c.Price) ||
                                        (order.Type == "sell" && order.Price <= c.Price))
                                    ).ToList();
                if (!matchList.Any())
                {
                    orders.Add(order);
                }
                else
                {
                    foreach (var match in matchList)
                    {
                        if (order.Quantity == 0) break;

                        if (match.Quantity >= order.Quantity)
                        {
                            match.Quantity -= order.Quantity;
                            order.Quantity = 0;
                        }
                        else
                        {
                            order.Quantity -= match.Quantity;
                            match.Quantity = 0;
                        }
                    }

                    orders.Add(order);
                }
            }

            //process data.
            Console.WriteLine("Successfully Executed Orders");
            Log(orders.Where(c => c.Quantity == 0).ToList());

            Console.WriteLine("\n\n\nPending Orders");
            Log(orders.Where(c => c.Quantity > 0).ToList());
        }

        static void Log(List<Order> orders)
        {
            foreach (var order in orders)
            {                
                Console.WriteLine(JsonConvert.SerializeObject(order));
            }
        }
    }
}

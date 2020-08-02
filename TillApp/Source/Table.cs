using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TillApp.Source
{
    public class Table
    {
        public int TableNumber { get; set; }

        public string TableName { get; set; }

        public List<Dish> OrderedDishes { get; set; }
        
        public string WaiterName { get; }

        public DateTime CreationDateTime { get; }

        public double TotalPrice { get; set; } = 0;


        public Table(int TableNumber, string TableName, string WaiterName)
        {
            CreationDateTime = DateTime.Now;
            OrderedDishes = new List<Dish>();
            this.TableName = TableName;
            this.TableNumber = TableNumber;
            this.WaiterName = WaiterName;
        }
    }
}

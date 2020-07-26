using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TillApp.Source
{
    class OrderItem
    {
        public int Id { get; }
        public string Name { get; }
        public float Price { get; }

        public OrderItem(int id, string name, float price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }

    class OrderList
    {
        private List<OrderItem> _OrderList;

        public OrderList()
        {
            _OrderList = new List<OrderItem>();
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _OrderList.Add(orderItem);
        }

    }
}

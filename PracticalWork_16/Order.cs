using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork_16
{
    internal class Order
    {
        public int Id { get;}
        public string Mail { get; }
        public string ProductCode { get; }
        public string ProductName { get; }

        public Order(int id, string mail, string productCode, string productName)
        {
            Id = id;
            Mail = mail;
            ProductCode = productCode;
            ProductName = productName;
        }
    }
}

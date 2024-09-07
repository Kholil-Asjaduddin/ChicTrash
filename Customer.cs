using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicTrash
{
    public class Customer : User
    {
        //Properties
        private string _customerId;
        private string? _requestItem;
        public string SellerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public string RequestItem
        {
            get { return _requestItem; }
            set { _requestItem = value; }
        }

        //Method
        public string SetRequest(string requestItem)
        {
            requestItem = "Limbah plastik";
            return requestItem;
        }
        public bool BuyItem(double userMoney, double itemPrice, int itemQuantity)
        {
            if (itemQuantity > 0)
                if (userMoney >= itemPrice)
                    return true;
                else
                    return false;
            else
                return false;
        }
    }
}

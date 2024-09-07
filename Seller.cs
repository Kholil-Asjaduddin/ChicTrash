using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicTrash
{
    public class Seller : User
    {
        //Properties
        private string _sellerId;

        public string SellerId
        {
            get { return _sellerId; }
            set { _sellerId = value; }
        }

        //Method
        public bool SellItem(string itemName, string itemCategory, string itemDescription, double itemPrice, int itemQuantity)
        {
            if ((itemName != null) && (itemCategory != null) && (itemDescription != null) && (itemPrice != 0) && (itemQuantity != 0))
                return true;
            else
                return false;
        }
    }
}

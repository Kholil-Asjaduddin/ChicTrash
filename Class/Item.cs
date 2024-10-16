using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicTrash
{
    public class Item
    {
        //Properties
        private int _itemId;
        private string _itemName;
        private string _category;
        private string _description;
        private double _price;
        private int _quantity;

        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicTrash
{
    public class User
    {
        //Properties
        private int _userId;
        private string _userName;
        private string _userEmail;
        private string _userPassword;
        private string _userPhone;
        private string _userAdress;
        private double _userMoney;
        private string? _sellerId;
        private string? _customerId;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }
        public string UserPhone
        {
            get { return _userPhone; }
            set { _userPhone = value; }
        }
        public double UserMoney
        {
            get { return _userMoney; }
            set { _userMoney = value; }
        }
        public string UserAdress
        {
            get { return _userAdress; }
            set { _userAdress = value; }
        }

        public int UserUserId
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string? SellerId
        {
            get { return _sellerId; }
            set { _sellerId = value; }
        }

        public string? CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }
        public double GetMoney()
        {
            return _userMoney;
        }

    }
}

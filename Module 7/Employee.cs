using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicTrash.Module_7
{
    public class Employee
    {
        private int _employeeId;
        private string _loginName;
        private string _password;

        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public int EmployeeID
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public bool Login(string loginName, string password)
        {
            if (loginName != null && password != null) 
                return true;
            else
                return false;
        }
    }
}

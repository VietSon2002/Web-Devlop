using SV20T1020242.DataLayers.SQLServer;
using SV20T1020242.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020242.BusinessLayers
{
    public static class UserAccountService
    {
        public static UserAccount Authorize(string userName, string password)
        {
            EmployeeAccountDAL employeeDAL = new EmployeeAccountDAL("server=Viet-Son;user id=sa;password=123;database=LiteCommerceDB_2023;TrustServerCertificate=true");
            return employeeDAL.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            EmployeeAccountDAL employeeDAL = new EmployeeAccountDAL("server=Viet-Son;user id=sa;password=123;database=LiteCommerceDB_2023;TrustServerCertificate=true");
            return employeeDAL.ChangePassword(userName, oldPassword, newPassword);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using TransferObject;
namespace BusinessLayer
{
    public class Account_BL
    {
        private Account_DAL accountDAL;
        public Account_BL()
        {
            accountDAL = new Account_DAL();
        }
        //kiem tra dang nhap
        public Account_DTO Login(Account_DTO account)
        {
            try
            {
                return accountDAL.Login(account);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //kiem tra va thuc hien dang ky
        public bool Register(Account_DTO account)
        {
            try
            {
                if (accountDAL.CheckAccountExists(account.Username))
                {
                    return false; // Tai khoan da ton tai
                }
                return accountDAL.AddAccount(account);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //kiem tra ton tai tai khoan
        public bool CheckAccountExists(string username)
        {
            try
            {
                return accountDAL.CheckAccountExists(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

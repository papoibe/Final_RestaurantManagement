using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransferObject;

namespace PresentationLayer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Account_DTO admin = new Account_DTO("admin", "123456");
            //Application.Run(new FrmAdmin(admin));
            //Account_DTO nhanvien = new Account_DTO("nhanvien1", "123456");
            //Application.Run(new FrmWorkers(nhanvien));

            Application.Run(new RestaurantManagement());
        }
    }
}

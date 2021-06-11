using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor_Test2
{
    public class Global_Variables
    {
        //Data Source=127.0.0.1,1433;Initial Catalog=Test; Integrated Security=False;User ID=sa;Password=123456;
        public static volatile string SQL_CONFIG = "Data Source=127.0.0.1,1433;Initial Catalog=Test; Integrated Security=False;User ID=sa;Password=123456;";
        //public static volatile string SQL_CONFIG = "Server=.;Database=Test;Trusted_Connection=True;";

        public static volatile string isDevice = "";
        public static volatile bool mobile = false;
    }
}

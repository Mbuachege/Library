using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Library
{
    public class connection
    {
        public static SqlConnection CONN()
        {
            SqlConnection sql = new SqlConnection(Properties.Settings.Default.Conn);
            sql.Open();

            return sql;
        }

        public string Name { get; set; }
        public string RegNo { get; set; }
        public string Form { get; set; }
    }
}

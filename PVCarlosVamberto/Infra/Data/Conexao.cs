using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVCarlosVamberto.Infra.Data
{
    public class Conexao
    {
        public System.Data.SqlClient.SqlConnection Connection { get; set; }

        public Conexao()
        {
            // Pega o ConnectionString do App.config
            string _connString = System.Configuration
                .ConfigurationManager
                .ConnectionStrings["PVConnectionString"]
                .ConnectionString;

            Connection = new System.Data.SqlClient.SqlConnection(_connString);
        }

        public void Open()
        {
            Connection.Open();
        }

        public void Close()
        {
            Connection.Close();
        }
    }
}
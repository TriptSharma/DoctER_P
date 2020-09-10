using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DoctER_P.Models
{
    public class SqlUtility
    {
        private SqlConnection cnDb = default;
        private SqlDataAdapter dsAdp = default;
        private string ConnectionStringKey = "ConnectionString";

        public SqlUtility()
        {
            string strConnString = ConfigurationManager.AppSettings[ConnectionStringKey];
            cnDb = new SqlConnection(strConnString);
        }

        public string commenceConnection()
        {
            try
            {
                cnDb.Open();
                return "Connected";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void closeConnection()
        {
            if (cnDb.State == ConnectionState.Open)
            {
                cnDb.Close();
            }
        }
        public DataSet executeQuery(string query)
        {
            commenceConnection();

            dsAdp = new SqlDataAdapter(query, cnDb);
            DataSet dsTemp = new DataSet();
            dsAdp.Fill(dsTemp);

            closeConnection();

            return dsTemp;
        }

        public DataTable executeSproc(string sprocName, List<SqlParameter> sqlParameters)
        {
            commenceConnection();

            SqlCommand command = new SqlCommand(sprocName, cnDb);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 0;

            foreach (SqlParameter param in sqlParameters)
            {
                command.Parameters.Add(param);
            }

            dsAdp = new SqlDataAdapter(command);
            DataTable dsTemp = new DataTable();
            dsAdp.Fill(dsTemp);

            closeConnection();
            return dsTemp;
        }
    }
}
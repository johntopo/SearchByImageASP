using System;
using System.Data;
using Npgsql;

namespace retrieveImageFeatures.UtilitiesClasses.Database
{
   
    public class SqlServerSetUp
    {
        //set up server
        
        private string host = "localhost";
        private string port = "5432";
        private string user = "topo";
        private string password = "topo";
        private string dbname = "ImagesDB";
        //

        public NpgsqlConnection connection;
        public DataSet ds;
        public DataTable dt;



        public SqlServerSetUp() 
        {
            ds=new DataSet();
            dt=new DataTable();
        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (Exception msg)
            {


            }
        }

        public void OpenConnection()
        {
            try
            {


                // PostgeSQL-style connection string
                string connstring = new SqlQuerriesAdapter().SqlConnection(host, port, user, password, dbname);
                // Making connection with Npgsql provider
                connection = new NpgsqlConnection(connstring);
                connection.Open();
                // quite complex sql statement

               


                




            }
            catch (Exception msg)
            {
                // something went wrong


            }
        }


    }
}
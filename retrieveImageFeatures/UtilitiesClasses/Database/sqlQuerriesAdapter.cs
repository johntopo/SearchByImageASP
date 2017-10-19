namespace retrieveImageFeatures.UtilitiesClasses.Database
{
    /// <summary>
    ///     Summary description for SqlQuerriesAdapter
    /// </summary>
    public class SqlQuerriesAdapter
    {


        //select top 10 similar images with custom function 'distance' that was created in postgresql .
        public string SelectByDistance(string tableName,string columnName,string columnForQuery, double[] queryArray)
        {
            return "SELECT " +columnName +","+columnForQuery+ " FROM "+tableName+" ORDER BY distance("+tableName+"."+columnForQuery+", ARRAY["+string.Join(",", queryArray)+
                   "]) LIMIT 10;";
        }

        public string SelectByDistance(string tableName, string columnName, string columnForQuery, double[] queryArray, int numberOfImages)
        {
            return "SELECT " + columnName + "," + columnForQuery + " FROM " + tableName + " ORDER BY distance(" + tableName + "." + columnForQuery + ", ARRAY[" + string.Join(",", queryArray) +
                   "]) LIMIT "+numberOfImages+";";
        }


        public string CreateTable(string tableName, string id, string fileName, string destriptor)
        {
            return "CREATE TABLE "+tableName+"("+id+" SERIAL PRIMARY KEY," +fileName+" TEXT    NOT NULL,"+destriptor+" real[]);";
        }
  

        public string Select(string table, string[] columns)
        {
            return "SELECT " + string.Join(",", columns) + " FROM " + table + ";";
        }

        public string InsertInto(string table, string[] columns, string[] values)//adistixia gia column-value
        {
            return "INSERT INTO " + table + "(" + string.Join(",", columns) + ") VALUES ('" + string.Join("','", values) +
                   "');";
        }
        public string InsertInto(string table, string[] columns, string fileName, double[] values)//2 columns(Name-descriptor(double[]))
        {
            return "INSERT INTO " + table + "(" + string.Join(",", columns) + ") VALUES ('"+fileName+"','{" + string.Join(",", values) +
                   "}');";
        }
    

        public string SqlConnection(string host, string port, string user, string password, string dbname)
        {
            var res = string.Format("Server={0};Port={1};" +
                                    "User Id={2};Password={3};Database={4};",
                host, port, user,
                password, dbname);

            return res;
        }

    
    }
}
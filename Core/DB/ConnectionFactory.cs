using System;
using System.Configuration;


namespace Core.DB
{
    public class ConnectionFactory
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString; }
        }


        public static DBCoreDataContext GetDBCoreDataContext()
        {
            return new DBCoreDataContext(ConnectionString);
        }        
    }
}

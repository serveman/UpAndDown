using System;
using System.Data.SqlClient;

namespace UpAndDown.History
{
    class History
    {
        public History()
        {
            OnConnect();
        }

        void OnConnect()
        {
            //Sql 연결정보 server=Server's IP Address,Server's PortNum;uid = User ID;pwd = User Password; database = Database name
            string connectionString = "server = 127.0.0.1,1433; uid = sa; pwd = 1qaz@WSX; database = test;";

            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                sqlConn.Open();
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlComm.CommandText = "SELECT * FROM test_table";

                using (SqlDataReader _result = sqlComm.ExecuteReader())
                {
                    while(_result.Read())
                    {
                        Console.WriteLine(_result[0].ToString());
                        Console.WriteLine(_result[1].ToString());
                    }
                }
            }
        }
    }
}

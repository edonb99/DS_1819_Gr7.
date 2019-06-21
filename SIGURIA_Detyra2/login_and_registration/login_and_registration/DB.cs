using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace login_and_registration
{
    class DB
    {
        // Create a connection with mysql

        private MySqlConnection connection = new MySqlConnection("Database=user_db;Data Source=localhost;User Id=root;Password=1234");
        
        //krijo nje funksion me hap konektimin
        public void openConnection()
        {
            if(connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }


        //krijo nje funksion me mbyll konektimin
        public void closeConnection() 
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        // krijo nje funksion me kthy konektimin
        public MySqlConnection getConnection()
        {
            return connection;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MALEX_Project_1._0
{
    class MySQL
    {
        //-------------------------------------------------------------------------------------
        //-- Объявление переменных
        //-------------------------------------------------------------------------------------
        //-- строка соединения 
        static string Connect = "Database=market_exmo;Data Source=localhost;User Id=root;Password=q1w2e3r4;SslMode=none;";

        public void InsertMysql(string sqltext)
        {
            //-- инициализируем соединение
            MySqlConnection myConnection = new MySqlConnection(Connect);
            //-- инициализируем SQL команду
            MySqlCommand myCommand = new MySqlCommand(sqltext, myConnection);

            //-- открываем соединения
            myConnection.Open();
            //-- выполняем запрос 
            myCommand.ExecuteNonQuery();
            //-- закрываем соединение
            myConnection.Close();
        }
    
    }
}

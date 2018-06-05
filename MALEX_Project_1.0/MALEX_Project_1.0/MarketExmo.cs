using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MALEX_Project_1._0
{
    class MarketExmo
    {
        //-------------------------------------------------------------------------------------
        //-- Объявление переменных
        //-------------------------------------------------------------------------------------
        //-- строка Json
        private string json = "";
        //-- строка Url
        private string url_trades        = "https://api.exmo.com/v1/trades/?pair=BTC_USD";      // Книга ордеров по валютной паре
        private string url_order_book    = "https://api.exmo.com/v1/order_book/?pair=BTC_USD";  // Книга ордеров по валютной паре
        private string url_ticker        = "https://api.exmo.com/v1/ticker/";                   // Cтатистика цен и объемов торгов по валютным парам
        private string url_pair_settings = "https://api.exmo.com/v1/pair_settings/";            // Настройки валютных пар
        private string url_currency      = "https://api.exmo.com/v1/currency/";                 // Cписок валют биржи


        MySQL sql = new MySQL();

        //-------------------------------------------------------------------------------------
        //-- Получение тикеров пар
        //-------------------------------------------------------------------------------------
        public void GetTicker()
        {
            // получаем Json файл
            GetJson(url_pair_settings);            
            // получаем структуру Json файла
            JObject obj = JObject.Parse(json);
            // получаем верхний уровень заголовков
            var children = obj.Children().ToArray();

            string ticker = "";

            // циклом перебираем заголовки (в данном случае название пар)
            foreach (var i in children)
            {
                ticker = i.Path;

                sql.InsertMysql("INSERT INTO market_exmo.ticker(ticker) values('"+ ticker + "')");                               
            }


            //dynamic result = JsonConvert.DeserializeObject<dynamic>(json);
        }

        //-------------------------------------------------------------------------------------
        //-- Метод для получения информации по Url
        //-------------------------------------------------------------------------------------
        private void GetJson(string url)
        {
            using (var webClient = new WebClient())
            {
                json = webClient.DownloadString(url);
            }
        }
    }
}

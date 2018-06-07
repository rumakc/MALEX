using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Market
{
    public class Exmo
    {
        //-------------------------------------------------------------------------------------
        //-- Объявление переменных
        //-------------------------------------------------------------------------------------

        //-- Ключ и Secret-код
        private string _key;
        private string _secret;
        //-- Уникальный идентификатор запроса
        private static long _nounce;
        //-- Входная точка (Url-адрес) для работы с API
        private string _url = "http://api.exmo.com/v1/{0}";
        //-- Тип выполнения методов (по умолчанию асинхронное выполнение - true)
        private bool _async = true;

        //-- Тип запроса
        string[] _query = new string[]
        {
                // Get запрос

                "currency"             , // { 0} - Cписок валют биржи
                "pair_settings"        , // { 1} - Настройки валютных пар
                "ticker"               , // { 2} - Cтатистика цен и объемов торгов по валютным парам
                "order_book"           , // { 3} - Книга ордеров по валютной паре
                "trades"               , // { 4} - Список сделок по валютной паре                               
                                                
                // Post запросы

                "user_info"            , // { 5} - Получение информации об аккаунте пользователя
                "user_open_orders"     , // { 6} - Получение списока открытых ордеров пользователя
                "user_cancelled_orders", // { 7} - Получение отмененных ордеров пользователя
                "order_trades"         , // { 8} - Получение истории сделок ордера
                "user_trades"          , // { 9} - Получение сделок пользователя

                "order_create"         , // {10} - Создание ордера
                "order_cancel"         , // {11} - Отмена ордера
                
                "deposit_address"      , // {12} - Получнение списка адресов для депозита криптовалют
                "withdraw_crypt"       , // {13} - Создание задачи на вывод криптовалют                
                
                "required_amount"      , // {14} - Подсчет в какую сумму обойдется покупка определенного кол-ва валюты по конкретной валютной паре                
                "withdraw_get_txid"      // {15} - Получение ИД транзакции криптовалюты для отслеживания на blockchain
        };

        //-------------------------------------------------------------------------------------
        //-- Конструкторы класса
        //-------------------------------------------------------------------------------------

        //-- Конструктор для инициализации данных дл параметра "nonce"

        static Exmo()
        {
            _nounce = Helpers.GetTimestamp();
        }

        //-- Конструктор для инициализации API Key и API Secret

        public Exmo(string key, string secret, bool async = true)
        {
            _key = key;
            _secret = secret;
            _async = async;
        }

        //-------------------------------------------------------------------------------------
        //-- Метод запроса данных из API (Синхронный)
        //-------------------------------------------------------------------------------------

        private string ApiQuery(string apiName, IDictionary<string, string> req)
        {
            using (var wb = new WebClient())
            {
                req.Add("nonce", Convert.ToString(_nounce++));
                var message = Helpers.ToQueryString(req);

                var sign = Helpers.Sign(_secret, message);

                wb.Headers.Add("Sign", sign);
                wb.Headers.Add("Key", _key);

                var data = req.ToNameValueCollection();

                var response = wb.UploadValues(string.Format(_url, apiName), "POST", data);
                return Encoding.UTF8.GetString(response);
            }
        }

        //-------------------------------------------------------------------------------------
        //-- Метод запроса данных из API (Асинхронный, возвращающий Json)
        //-------------------------------------------------------------------------------------

        private async Task<string> ApiQueryAsync(string apiName, IDictionary<string, string> req)
        {
            using (var client = new HttpClient())
            {
                var n = Interlocked.Increment(ref _nounce);
                req.Add("nonce", Convert.ToString(n));
                var message = Helpers.ToQueryString(req);

                var sign = Helpers.Sign(_secret, message);

                var content = new FormUrlEncodedContent(req);
                content.Headers.Add("Sign", sign);
                content.Headers.Add("Key", _key);

                var response = await client.PostAsync(string.Format(_url, apiName), content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        //-------------------------------------------------------------------------------------
        //-- Метод запроса данных из API (Асинхронный, возвращающий статус)
        //-------------------------------------------------------------------------------------

        private async Task<HttpStatusCode> ApiQueryAsyncEx(string apiName, IDictionary<string, string> req)
        {
            using (var client = new HttpClient())
            {
                var n = Interlocked.Increment(ref _nounce);
                req.Add("nonce", Convert.ToString(n));
                var message = Helpers.ToQueryString(req);

                var sign = Helpers.Sign(_secret, message);

                var content = new FormUrlEncodedContent(req);
                content.Headers.Add("Sign", sign);
                content.Headers.Add("Key", _key);

                var response = await client.PostAsync(string.Format(_url, apiName), content);

                await Task.Factory.StartNew(async () =>
                {
                    var data = await response.Content.ReadAsStringAsync();
                });

                return response.StatusCode;
            }
        }

        //-------------------------------------------------------------------------------------
        //-- Методы получения данных
        //-------------------------------------------------------------------------------------

        public string AccountInfo()
        {
            //-- Объявление переменных
            string type_property = _query[5];
            var result = "";

            //-- Выбор метода выполнения (синхронное/асинхронное)
            if (_async)
            {
                result = ApiQueryAsync(type_property, new Dictionary<string, string>()).Result;
            }
            else
            {
                result = ApiQuery(type_property, new Dictionary<string, string>());
            }

            return result;
        }

    }

}
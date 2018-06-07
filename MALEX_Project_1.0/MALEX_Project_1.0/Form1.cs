using OEC.Data;
using StockSharp.Binance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Market;

namespace MALEX_Project_1._0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MarketExmo ex = new MarketExmo();

            //ex.GetTicker();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // создаем подключение
            // Trader = new BinanceTrader();// { LogLevel = LogLevels.Debug };

            // Trader.Key = Key.Text;
            // Trader.Secret = Secret.Password;
        }
    }
}

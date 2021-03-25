using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockRepository.Tests
{
    [TestClass()]
    public class StockPriceTests
    {
        [TestMethod]
        public void GetStockClosePriceTest()
        {
            StockPrice sp = new StockPrice();
            var stockId = 2330;
            var date = "20210304";
            
            var price = sp.getStockReportPrice(stockId, date, "收盤價");
            Assert.AreEqual(price, 601);
        }

        [TestMethod]
        public void GetStockOpenPriceTest()
        {
            StockPrice sp = new StockPrice();
            var stockId = 2330;
            var date = "20210304";
            
            var price = sp.getStockReportPrice(stockId, date, "開盤價");
            Assert.AreEqual(price, 609);
        }

        [TestMethod]
        public void GetStockNotTradeDatePriceTest()
        {
            //假日
            StockPrice sp = new StockPrice();
            var stockId = 2330;
            var date = "20210314";
            Assert.ThrowsException<Exception>(() => sp.getStockReportPrice(stockId, date, "開盤價"), "找不到對應日期的資料");
        }
    }
}
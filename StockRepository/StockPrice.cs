using System;
using System.Globalization;
using Newtonsoft.Json;
using RestSharp;

namespace StockRepository
{
    public class StockPrice
    {

        public double getStockReportPrice(in int stockId, string date, string field_name)
        {
            var twseStockDayReportRequestUrl = "https://www.twse.com.tw/exchangeReport/STOCK_DAY?response=json&date={0}&stockNo={1}&_=1615738116281";
            
            var queryDate = DateTime.ParseExact(date, "yyyyMMdd", null);

            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();

            var queryDateStr = string.Empty; 
            queryDateStr = queryDate.ToString("yyy/MM/dd",culture);

            RestSharp.RestClient rc = new RestClient(String.Format(twseStockDayReportRequestUrl, date, stockId));
            var rcResponse = rc.Get(new RestRequest());

            DayTradeReportResponse response = JsonConvert.DeserializeObject<DayTradeReportResponse>(rcResponse.Content);

            if (response.stat != "OK") throw new Exception("回應錯誤");
            if (response.fields.Length == 0) throw new Exception("fields回應錯誤");
            if (response.data.Length == 0) throw new Exception("data回應錯誤");


            var dateIndex = Array.IndexOf(response.fields, "日期");
            var dataIndex = Array.IndexOf(response.fields, field_name);
            if (dateIndex < 0) throw new Exception("日期欄位不存在");
            if (dataIndex < 0) throw new Exception(field_name + "欄位不存在");

            var findDataStr = string.Empty;
            foreach (var dataRow in response.data)
            {
                if (dataRow[dateIndex] == queryDateStr)
                {
                    var data = dataRow[dataIndex];
                    if (data.Length > 0)
                    {
                        findDataStr = data;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(findDataStr))
                throw new Exception("找不到對應日期的資料");

            double stockClosingPrice = -1;
            double.TryParse(findDataStr, out stockClosingPrice);

            return stockClosingPrice;
        }
    }

    public class DayTradeReportResponse
    {
        public string stat { get; set; }

        public string date { get; set; }
        public string[] fields { get; set; }
        public string[][] data { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;

namespace StockGeneralApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockPriceController : ControllerBase
    {

        private readonly ILogger<StockPriceController> _logger;
        private readonly IHttpContextAccessor _accessor;

        public StockPriceController(ILogger<StockPriceController> logger, IHttpContextAccessor accessor)
        {
            _logger = logger;
            _accessor = accessor;
        }

        [HttpGet]
        [Route("Info")]
        public StockGeneralMessage Info()
        {
            var ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            _logger.LogInformation("呼叫Info, 來源:"+ip);
            return new StockGeneralMessage() { success = true, message = "request pattern:api/StockPrice/GetPrice/{yyyymmdd}/{stockNo}" };
        }


        //test example https://localhost:44315/api/StockPrice/GetPrice/20210331/2330
        [HttpGet]
        [Route("GetPrice/{dt}/{sid}")]
        public StockGeneralMessage GetPrice(string dt, string sid)
        {
            if (string.IsNullOrEmpty(dt) || string.IsNullOrEmpty(sid))
                return new StockGeneralMessage()
                { success = false, message = "arguments invalid, dt or stockNumber is empty!" };


            var sr = new StockRepository.StockPrice();
            try
            {
                int stockId = int.Parse(sid);
                var price = (decimal)sr.getStockReportPrice(stockId, dt, "收盤價");
                return new StockPrice() { success = true, message = string.Empty, price = price };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetPrice發生錯誤");
                return new StockGeneralMessage() { success = false, message = e.Message };
            }
        }


    }
}

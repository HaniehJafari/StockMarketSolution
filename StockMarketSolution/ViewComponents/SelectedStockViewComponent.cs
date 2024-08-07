using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace StockMarketSolution.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            Dictionary<string, object>? companyProfileDic = null;

            if (stockSymbol != null)
            {
                companyProfileDic = await _finnhubService.GetCompanyProfile(stockSymbol);
                Dictionary<string, object>? stockPriceDict = await _finnhubService.GetStockPriceQuote(stockSymbol);
                if (stockPriceDict != null && companyProfileDic != null)
                {
                    companyProfileDic.Add("price", stockPriceDict["c"]);
                }


            }
            if (companyProfileDic != null && companyProfileDic.ContainsKey("logo"))
                return View(companyProfileDic);
            else
                return Content("");

        }
    }
}

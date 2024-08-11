using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using StockMarketSolution;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;


namespace Tests.ControllerTests
{
    public class StocksControllerTest
    {
        private readonly IFinnhubService _finnhubService;
        private readonly Mock<IFinnhubService> _finnhubSrviceMock;
        private readonly Mock<ILogger<StocksController>> _loggerMock;
        private readonly Fixture _fixture;

        public StocksControllerTest()
        {
            _fixture = new Fixture();
            _finnhubSrviceMock = new Mock<IFinnhubService>();
            _finnhubService = _finnhubSrviceMock.Object;
            _loggerMock = new Mock<ILogger<StocksController>>();

        }



        [Fact]
        public async Task Explor_stockIsNull_ShouldReturnStockViewWithStocksList()
        {
            IOptions<TradingOptions> tradingOptions = Options.Create(new TradingOptions()
            {
                DefaultOrderQuantity = 100,
                DefaultStockSymbol = "MSFT",
                Top25PopularStocks = "AAPL,MSFT,AMZN,TSLA,GOOGL,GOOG,NVDA,BRK.B,META,UNH,JNJ,JPM,V,PG,XOM,HD,CVX,MA,BAC,ABBV,PFE,AVGO,COST,DIS,KO"
            });
            //Arrange
            StocksController stocksController = new StocksController(tradingOptions, _finnhubService, _loggerMock.Object);





            List<Dictionary<string, string>>? stocksDict = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(@"[{'currency':'USD','description':'APPLE INC','displaySymbol':'AAPL','figi':'BBG000B9XRY4','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5N8V8','symbol':'AAPL','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'MICROSOFT CORP','displaySymbol':'MSFT','figi':'BBG000BPH459','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5TD05','symbol':'MSFT','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'AMAZON.COM INC','displaySymbol':'AMZN','figi':'BBG000BVPV84','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001S5PQL7','symbol':'AMZN','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'TESLA INC','displaySymbol':'TSLA','figi':'BBG000N9MNX3','isin':null,'mic':'XNAS','shareClassFIGI':'BBG001SQKGD7','symbol':'TSLA','symbol2':'','type':'Common Stock'}, {'currency':'USD','description':'ALPHABET INC-CL A','displaySymbol':'GOOGL','figi':'BBG009S39JX6','isin':null,'mic':'XNAS','shareClassFIGI':'BBG009S39JY5','symbol':'GOOGL','symbol2':'','type':'Common Stock'}]");

            //Mock
            _finnhubSrviceMock.Setup(temp => temp.GetStocks()).ReturnsAsync(stocksDict);

            var expectedStocks = stocksDict!.Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) }).ToList();

            //Act
            IActionResult actionResult = await stocksController.Explore(null, false);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(actionResult);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<Stock>>();
            viewResult.ViewData.Model.Should().BeEquivalentTo(expectedStocks);
        }
    }
}

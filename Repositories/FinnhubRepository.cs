using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;
using System.Net.Http;
using Microsoft.Identity.Client;
using System;

using Microsoft.Extensions.Configuration;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {

            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            HttpClient httpClient= _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
            };
            HttpResponseMessage httpResponseMessage =await httpClient.SendAsync(httpRequestMessage);

            StreamReader streamReader = new StreamReader( await httpResponseMessage.Content.ReadAsStreamAsync());

            string responseBody= await streamReader.ReadToEndAsync();

            Dictionary<string, Object>? responseDictionary= JsonSerializer.Deserialize<Dictionary<string, Object>>(responseBody);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from Finnhub server");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error {responseDictionary["error"]}"); 
            }

            return responseDictionary;


        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"),
            };
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync());

            string responseBody = await streamReader.ReadToEndAsync();

            Dictionary<string, Object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, Object>>(responseBody);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from Finnhub server");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error {responseDictionary["error"]}");
            }

            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}"),
            };
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync());

            string responseBody = await streamReader.ReadToEndAsync();

            List<Dictionary<string, string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from Finnhub server");
            }

            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbolToSearch}&token={_configuration["FinnhubToken"]}"),
            };
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            StreamReader streamReader = new StreamReader(await httpResponseMessage.Content.ReadAsStreamAsync());

            string responseBody = await streamReader.ReadToEndAsync();

            Dictionary<string, Object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, Object>>(responseBody);

            if (responseDictionary == null)
            {
                throw new InvalidOperationException("No response from Finnhub server");
            }

            if (responseDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException($"Error {responseDictionary["error"]}");
            }

            return responseDictionary;
        }
    }
}

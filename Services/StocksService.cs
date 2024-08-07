
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class StocksService : IStocksService
    {

        private readonly IStocksRepository _stocksRepository;


        /// <summary>
        /// Constructor of StocksService class that executes when a new object is created for the class
        /// </summary>
        public StocksService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;

        }


        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            //Validation: buyOrderRequest can't be null
            if (buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));

            //Model validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();


            //add buy order object to buyorders List
            BuyOrder buyOrderFromRepo = await _stocksRepository.CreateBuyOrder(buyOrder);



            //convert the BuyOrder object into BuyOrderResponse type
            return buyOrderFromRepo.ToBuyOrderResponse();
        }


        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            //Validation: sellOrderRequest can't be null
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            //Model validation
            ValidationHelper.ModelValidation(sellOrderRequest);

            //convert sellOrderRequest into SellOrder type
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();

            //generate SellOrderID
            sellOrder.SellOrderID = Guid.NewGuid();

            //add sell order object to sell orders list
            SellOrder sellOrderFromRepo = await _stocksRepository.CreateSellOrder(sellOrder);
            

            //convert the SellOrder object into SellOrderResponse type
            return sellOrderFromRepo.ToSellOrderResponse();
        }


        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            List<BuyOrder> buyOrdersList = await _stocksRepository.GetBuyOrders();
            return buyOrdersList.Select(temp=>temp.ToBuyOrderResponse()).ToList();     
               
             
        }


        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            //Convert all SellOrder objects into SellOrderResponse objects
            List<SellOrder> sellOrdersList=await _stocksRepository.GetSellOrders();
            return  sellOrdersList
             .Select(temp => temp.ToSellOrderResponse()).ToList();
        }
    }
}



﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.DTO;

namespace RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>> GetBuyOrders();
        Task<List<SellOrder>> GetSellOrders();
    }
}

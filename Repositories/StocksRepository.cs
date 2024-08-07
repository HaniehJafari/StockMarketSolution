using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext _db;
        public StocksRepository(StockMarketDbContext db)
        {
            _db = db;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
        {
            List<BuyOrder> list = await _db.BuyOrders
                .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                .ToListAsync();
            return list;
        }

        public async Task<List<SellOrder>> GetSellOrders()
        {
            List<SellOrder> list = await _db.SellOrders
                .OrderByDescending(temp => temp.DateAndTimeOfOrder)
                .ToListAsync();
            return list;
        }
    }
}

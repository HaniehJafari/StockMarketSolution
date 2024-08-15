using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using StockMarketSolution.Controllers;
using StockMarketSolution.Models;

namespace StockMarketSolution.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {

        public CreateOrderActionFilter()
        {

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            if (context.Controller is TradeController tradeController)
            {
                IOrderRequest? orderRequest = (IOrderRequest?)context.ActionArguments["orderRequest"];

                if (orderRequest != null)
                {
                    //update date of order
                    orderRequest.DateAndTimeOfOrder = DateTime.Now;

                    //re-validate the model object after updating the date
                    tradeController.ModelState.Clear();

                    tradeController.TryValidateModel(orderRequest);

                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                        StockTrade stockTrade = new StockTrade() { Price = orderRequest.Price, Quantity = orderRequest.Quantity, StockName = orderRequest.StockName, StockSymbol = orderRequest.StockSymbol };

                        context.Result = tradeController.View(nameof(tradeController.Index), stockTrade);
                    }
                    else
                    {
                        await next();
                    }

                }

                else
                {
                    await next();
                }

            }

            await next();
        }
    }
}

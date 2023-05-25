using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using WebAPI.Helpers.Repositories;
using WebAPI.Models.Entities;
using WebAPI.Models.Schemas;

namespace WebAPI.Helpers.Services;

public class OrderService
{
    //Create order
    //delete/cancel order

    private readonly ProductRepo _productRepo;
    
    public async Task<bool> CreateOrderAsync(OrderSchema schema)
    {
        
        //var address = 
        var items = schema.Items;

        try
        {
            var order = new OrderEntity
            {
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                Items = new List<OrderItemEntity>(),
                //userId
            };

            foreach (var item in items)
            {
                OrderItemEntity OrderItem = item;
                order.Items.Add(OrderItem);
            }

            //create/save etc

            return true;
        }
        catch { }
        return false;
    }
    
}
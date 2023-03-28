using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities.Orders;
using WebStore.Interfaces.Services;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(HttpClient Client) : base(Client, WebApiAddresses.Orders)
        {

        }

        public async Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel, CancellationToken Cancel = default)
        {
            {
                var model = new CreateOrderDTO
                {
                    Items = Cart.ToDTO(),
                    Order = OrderModel,
                };

                var response = await PostAsync($"{Address}/{UserName}", model).ConfigureAwait(false);
                var order = await response
                   .EnsureSuccessStatusCode()
                   .Content
                   .ReadFromJsonAsync<OrderDTO>(cancellationToken: Cancel)
                   .ConfigureAwait(false);

                return order.FromDTO()!;
            }
        }
    

        public async Task<Order?> GerOrderByIdAsync(int Id, CancellationToken Cancel = default)
        {
            var order = await GetAsync<OrderDTO>($"{Address}/{Id}").ConfigureAwait(false);
            return order.FromDTO();
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string UserName, CancellationToken Cancel = default)
        {
            var orders = await GetAsync<IEnumerable<OrderDTO>>($"{Address}/user/{UserName}").ConfigureAwait(false);
            return orders!.FromDTO()!;
        }
    }
}

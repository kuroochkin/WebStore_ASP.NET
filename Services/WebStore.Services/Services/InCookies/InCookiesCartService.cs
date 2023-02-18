using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Mapping;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Services.InCookies
{
    public class InCookiesCartService : ICartService
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IProductData _ProductData;

        private readonly string _CartName; // имя cookies

        private Cart Cart
        {
            get
            {
                //Нужно взять cookies и десериализвать из Json
                var context = _HttpContextAccessor.HttpContext; // берем контекст
                var cookies = context!.Response.Cookies; // берем cookie из контекста

                var cart_cookie = context.Request.Cookies[_CartName]; // пытаемся извлечь собственную cookies
                if (cart_cookie is null) // если ее нет, то надо создать
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart; // возвращаем пустую корзину
                }

                // если cookies уже до этого был
                ReplaceCart(cookies, cart_cookie); 
                return JsonConvert.DeserializeObject<Cart>(cart_cookie)!;
            }
            // Cart сериализуем в Json и помещаем в Cookie
            set => ReplaceCart(_HttpContextAccessor.HttpContext!.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCart(IResponseCookies cookies, string cart)
        {
            cookies.Delete(_CartName); // удаляем сперва существующую корзину
            cookies.Append(_CartName, cart); // добавляем новую
        }

        public InCookiesCartService(IHttpContextAccessor HttpContextAccessor, IProductData ProductData)
        {
            _HttpContextAccessor = HttpContextAccessor;
            _ProductData = ProductData;

            var user = HttpContextAccessor.HttpContext!.User;
            var user_name = user.Identity!.IsAuthenticated ? $"-{user.Identity.Name}" : null;

            _CartName = $"WebStore.GB.Cart{user_name}";
        }
        public void Add(int Id)
        {
            var cart = Cart; // Десериализация корзины(get)

            var item = cart.Items.FirstOrDefault( i => i.ProductId == Id);
            if (item == null) // если объекта нет, то добавляем его в корзину
                cart.Items.Add(new CartItem { ProductId = Id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart; // Сериализуем обратнно
        }

        public void Clear()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void Decrement(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null)
                return; // если не найден, то ничего не делаем

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void Remove(int Id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == Id);
            if (item is null)
                return; 

            cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel GetViewModel()
        {
            var products = _ProductData.GetProducts(new()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToArray()
            }); ;

            var products_views = products.ToView().ToDictionary(p => p.Id);

            return new()
            {
                Items = Cart.Items.Where(item => products_views.ContainsKey(item.ProductId))
                .Select(item => (products_views[item.ProductId], item.Quantity))!
            };
        }
    }
}

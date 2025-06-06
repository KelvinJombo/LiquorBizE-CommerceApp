﻿using System.Net;

namespace LiquorSales.Web.Services
{
    public interface ICartServices
    {
        [Get("/cart-service/cart/{userName}")]
        Task<GetCartResponse> GetCart(string userName, [Header("Authorization")] string authorization);

        [Post("/cart-service/cart")]
        Task<StoreCartResponse> StoreCart(StoreCartRequest request, [Header("Authorization")] string authorization);

        [Delete("/cart-service/cart/{userName}")]
        Task<DeleteCartResponse> DeleteCart(string userName, [Header("Authorization")] string authorization);

        [Post("/cart-service/cart/checkout")]
        Task<CheckoutCartResponse> CheckoutCart(CheckoutCartRequest request, [Header("Authorization")] string authorization);

        [Put("/cart-service/cart")]
        Task<UpdateCartResponse> UpdateCart([Body] UpdateCartRequest request, [Header("Authorization")] string authorization);


    }
}

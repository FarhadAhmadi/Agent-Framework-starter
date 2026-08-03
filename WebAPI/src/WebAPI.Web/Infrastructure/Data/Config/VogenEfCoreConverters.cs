using Vogen;
using WebAPI.Web.Domain.CartAggregate;
using WebAPI.Web.Domain.GuestUserAggregate;
using WebAPI.Web.Domain.OrderAggregate;
using WebAPI.Web.Domain.ProductAggregate;

namespace WebAPI.Web.Infrastructure.Data.Config;

[EfCoreConverter<ProductId>]
[EfCoreConverter<CartId>]
[EfCoreConverter<CartItemId>]
[EfCoreConverter<GuestUserId>]
[EfCoreConverter<OrderId>]
[EfCoreConverter<OrderItemId>]
[EfCoreConverter<Quantity>]
[EfCoreConverter<Price>]
internal partial class VogenEfCoreConverters;

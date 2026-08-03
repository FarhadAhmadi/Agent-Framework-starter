using WebAPI.Web.Domain.ProductAggregate;

namespace WebAPI.Web.ProductFeatures;

public record ProductDto(ProductId Id, string Name, decimal UnitPrice);

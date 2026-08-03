using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebAPI.Web.Domain.CartAggregate;
using WebAPI.Web.Domain.GuestUserAggregate;
using WebAPI.Web.Domain.OrderAggregate;
using WebAPI.Web.Domain.ProductAggregate;

namespace WebAPI.Web.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) :
  DbContext(options)
{
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Cart> Carts => Set<Cart>();
  public DbSet<CartItem> CartItems => Set<CartItem>();
  public DbSet<GuestUser> GuestUsers => Set<GuestUser>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<OrderItem> OrderItems => Set<OrderItem>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }

  public override int SaveChanges() =>
        SaveChangesAsync().GetAwaiter().GetResult();
}

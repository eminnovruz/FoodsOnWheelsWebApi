using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Worker>().ToContainer("Worker");
        modelBuilder.Entity<User>().ToContainer("Users");
        modelBuilder.Entity<Order>().ToContainer("Orders");
        modelBuilder.Entity<Courier>().ToContainer("Couriers");
        modelBuilder.Entity<Restaurant>().ToContainer("Restaurants");
        modelBuilder.Entity<Category>().ToContainer("Categories");
        modelBuilder.Entity<Food>().ToContainer("Foods");
        modelBuilder.Entity<CourierComment>().ToContainer("CourierComments");
        modelBuilder.Entity<RestaurantComment>().ToContainer("RestaurantComments");
        modelBuilder.Entity<BankCard>().ToContainer("BankCards");
        modelBuilder.Entity<OrderRating>().ToContainer("OrderRatings");

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Worker> Workers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BankCard> BankCards { get; set; }
    public DbSet<CourierComment> CourierComments { get; set; }
    public DbSet<OrderRating> OrderRatings { get; set; }
    public DbSet<RestaurantComment> RestaurantComments { get; set; }
}

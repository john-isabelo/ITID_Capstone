using Microsoft.EntityFrameworkCore;
using WeeklyTask.Models;

public class FoodDbContext : DbContext
{
    public FoodDbContext(DbContextOptions<FoodDbContext> options)
        : base(options)
    { }

    public DbSet<Food> Foods { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Reservation> Reservations { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Food>().HasData(
            new Food { Id = 1, Name = "Pizza", Price = 10.99m, Ingredients = "ham, pineapple, tomatoes", ImagePath = "/images/pizza.jpg" },
            new Food { Id = 2, Name = "Hamburger", Price = 8.99m, Ingredients = "beef, egg, cheese", ImagePath = "/images/burger.jpg" },
            new Food { Id = 3, Name = "Fried Chicken", Price = 12.99m, Ingredients = "flour, egg, sauce", ImagePath = "/images/friedchicken.jpg" },
            new Food { Id = 4, Name = "Spaghetti", Price = 9.99m, Ingredients = "tomatoes, sausage, cheese", ImagePath = "/images/spaghetti.jpg" }
        );    
        
    }
}


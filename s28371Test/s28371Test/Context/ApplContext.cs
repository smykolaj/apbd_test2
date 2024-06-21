using Microsoft.EntityFrameworkCore;
using s28371Test.Models;

namespace s28371Test.Context;

public class ApplContext : DbContext
{
    protected ApplContext()
    {
    }

    public ApplContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Title> Titles { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


       
        modelBuilder.Entity<Character>().HasData(new List<Character>()
        {
            new()
            {
                Id = 1,
                FirstName = "Mykola",
                LastName = "Sukonnik",
                CurrentWeight = 55,
                MaxWeight = 100
            },
            new ()
            {
                Id = 2,
                FirstName = "Michal",
                LastName = "Pazio",
                CurrentWeight = 56,
                MaxWeight = 90
            }
            
        });
        modelBuilder.Entity<Item>().HasData(new List<Item>()
        {
            new()
            {
                Id = 1,
                Name = "Item1",
                Weight = 6,
            },
            new()
            {
                Id = 2,
                Name = "Item2",
                Weight = 20,
            },
            new()
            {
                Id = 3,
                Name = "Item3",
                Weight = 50
                
            }
            
            
        });
        modelBuilder.Entity<Title>().HasData(new List<Title>()
        {
            new()
            {
                Id = 1,
                Name = "Title1"
            },
            new ()
            {
                Id = 2,
                Name = "Title2"
            }
            
        });
        modelBuilder.Entity<Backpack>().HasData(new List<Backpack>()
        {
            new ()
            {
                ItemId = 1,
                CharacterId = 1,
                Amount = 1,
            }
        });
        modelBuilder.Entity<CharacterTitle>().HasData(new List<CharacterTitle>()
        {
            new()
            {
                TitleId = 1,
                CharacterId = 1,
                AcquiredAt = DateTime.Now
            }
        });
    }
}
using Microsoft.EntityFrameworkCore;

namespace Lab3
{
    public class ModelDB:DbContext
    {
        public ModelDB(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Investor>? Investors { get; set; }
        public DbSet<Percent>? Percents { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Percent>().HasData(
                new Percent { Id = 1, DepositNumber = 1, DepositName = "Росбанк", InterestRate = 15, },
                new Percent { Id = 2, DepositNumber = 2, DepositName = "Тинькофф", InterestRate = 20 },
                new Percent { Id = 3, DepositNumber = 3, DepositName = "Рыбхоз", InterestRate = 18 }
                );
            modelBuilder.Entity<Investor>().HasData(
                new Investor
                {
                    Id = 1,
                    DepositNumber = 1,
                    DepositName = "Росбанк",
                    FullName = "Зубенко Михаил Петрович",
                    DepositAmount = 150000000,
                    DepositDate = new DateTime(2022, 6, 10),
                    InterestRate = 15,
                    TotalAmount = 300000000,
                },
                new Investor
                {
                    Id = 2,
                    DepositNumber = 2,
                    DepositName = "Тинькофф",
                    FullName = "Попов Максим Фёдорович",
                    DepositAmount = 666789,
                    DepositDate = new DateTime(2023, 8, 12),
                    InterestRate = 20,
                    TotalAmount = 1000000,
                },
                new Investor
                {
                    Id = 3,
                    DepositNumber = 3,
                    DepositName = "Рыбхоз",
                    FullName = "Савельев Марк Алексеевич",
                    DepositAmount = 99045678,
                    DepositDate = new DateTime(2022, 6, 10),
                    InterestRate = 18,
                    TotalAmount = 100000000,
                });

    //        modelBuilder.Entity<User>().HasData(
    //            new User { id = 1, EMail = "zahem@mail.ru", Password = "123456" },
    //new User { id = 1, EMail = "loxi@mail.ru", Password = "11111" }
    //            );
        }
    }
}

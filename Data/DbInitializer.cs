using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Data;
using GameStore.Models;

namespace GameStore.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoreContext context)
        {
            context.Database.EnsureCreated();
            if (context.Games.Any())
            {
                return; // BD a fost creata anterior
            }
            var games = new Game[]
            {
            new Game{Title="Cyberpunk 2077",Platform="Playstation 5",Price=Decimal.Parse("69")},
            new Game{Title="Fifa 21",Platform="Xbox Series X",Price=Decimal.Parse("59")},
            new Game{Title="The Witcher 3: Wild Hunt",Platform="PC",Price=Decimal.Parse("29")},
            new Game{Title="The Legend of Zelda: Breath of The Wild",Platform="Nintendo Switch",Price=Decimal.Parse("59")},
            new Game{Title="Watch Dogs: Legion",Platform="PC",Price=Decimal.Parse("59")},
            new Game{Title="Assassins Creed: Valhalla",Platform="Playstation 5",Price=Decimal.Parse("59")},

            };
            foreach (Game b in games)
            {
                context.Games.Add(b);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {
                new Customer{CustomerID=1050,Name="Popescu Ion",BirthDate=DateTime.Parse("1996-09-01")},
                new Customer{CustomerID=1045,Name="Crișan Valentin",BirthDate=DateTime.Parse("1999-07-08")},
            };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
                new Order{GameID=1,CustomerID=1050,OrderDate=DateTime.Parse("2020-02-02")},
                new Order{GameID=3,CustomerID=1045,OrderDate=DateTime.Parse("2020-03-03")},
                new Order{GameID=1,CustomerID=1045,OrderDate=DateTime.Parse("2020-04-04")},
                new Order{GameID=2,CustomerID=1050,OrderDate=DateTime.Parse("2020-05-05")},
                new Order{GameID=4,CustomerID=1050,OrderDate=DateTime.Parse("2020-06-06")},
                new Order{GameID=6,CustomerID=1050,OrderDate=DateTime.Parse("2020-07-07")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var publishers = new Publisher[]
            {
                new Publisher{PublisherName="Ubisoft",Adress="Franta"},
                new Publisher{PublisherName="CD Projekt Red",Adress="Polonia"},
                new Publisher{PublisherName="Electronic Arts",Adress="Statele Unite"},
                new Publisher{PublisherName="Nintendo",Adress="Japonia"},
            };
            foreach (Publisher p in publishers)
            {
                context.Publishers.Add(p);
            }
            context.SaveChanges();

            var publishedgames = new PublishedGame[]
            {
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "Cyberpunk 2077").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "CD Projekt Red").ID

                },
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "The Witcher 3: Wild Hunt").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "CD Projekt Red").ID

                },
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "Assassins Creed: Valhalla").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "Ubisoft").ID

                },
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "Watch Dogs: Legion").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "Ubisoft").ID

                },
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "The Legend of Zelda: Breath of The Wild").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "Nintendo").ID

                },
                new PublishedGame
                {
                    GameID = games.Single(c => c.Title == "Fifa 21").ID,
                    PublisherID = publishers.Single(i => i.PublisherName == "Electronic Arts").ID

                },
            };

            foreach (PublishedGame pb in publishedgames)
            {
                context.PublishedGames.Add(pb);
            }
            context.SaveChanges();
        }
    }
}

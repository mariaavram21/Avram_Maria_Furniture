using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avram_Maria_Furniture.Models;

namespace Avram_Maria_Furniture.Data
{
    public class DbInitializer
    {
        public static void Initialize(FurnitureStoreContext context)
        {
            context.Database.EnsureCreated();
            if (context.Furnitures.Any())
            {
                return; // BD a fost creata anterior
            }
            var furniture = new Furniture[]
            {
 new Furniture{Title="Virgo",Description="Excelent for your bathroom",Color="Green",Price=Decimal.Parse("2200")},
 new Furniture{Title="Anabelle",Description="Scary to not have it",Color="Black",Price=Decimal.Parse("3200")},
 new Furniture{Title="Santal",Description="Santal wood - bedroom",Color="Maroon",Price=Decimal.Parse("4200")},
 new Furniture{Title="Fuzia",Description="Moon",Color="Yellow",Price=Decimal.Parse("200")},
 new Furniture{Title="Griffin",Description="Stars",Color="Green",Price=Decimal.Parse("600")},
 new Furniture{Title="Magic Bed",Description="BedroomSuper",Color="White",Price=Decimal.Parse("1200")},
            };
            foreach (Furniture f in furniture)
            {
                context.Furnitures.Add(f);
            }
            context.SaveChanges();
            var customers = new Customer[]
            {

 new Customer{CustomerID=1050,Name="Popescu Marcela",Address="Str. Laleleor, nr. 40, Bucuresti"},
 new Customer{CustomerID=1045,Name="Mihailescu Cornel",Address="Str. Cascadelor, nr. 22, Cluj-Napoca"},

 };
            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            context.SaveChanges();
            var orders = new Order[]
            {
 new Order{FurnitureID=1,CustomerID=1050,OrderDate=DateTime.Parse("2020-10-02")},
 new Order{FurnitureID=3,CustomerID=1045,OrderDate=DateTime.Parse("2020-07-12")},
 new Order{FurnitureID=1,CustomerID=1045,OrderDate=DateTime.Parse("2020-12-05")},
 new Order{FurnitureID=2,CustomerID=1050,OrderDate=DateTime.Parse("2020-11-24")},
            };
            foreach (Order e in orders)
            {
                context.Orders.Add(e);
            }
            context.SaveChanges();

            var factories = new Factory[]
 {

 new Factory{FactoryName="Dumas",Address="Str. Aviatorilor, nr. 40, Bucuresti"},
 new Factory{FactoryName="Lima",Address="Str. Plopilor, nr. 35, Ploiesti"},
 new Factory{FactoryName="Fist",Address="Str. Liza, nr. 785, Cluj-Napoca"},
 };
            foreach (Factory p in factories)
            {
                context.Factories.Add(p);
            }
            context.SaveChanges();

            var manufacture = new ManufacturedFurniture[]
            {
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Dumas" ).ID, FurnitureID = furniture.Single(i => i.Title =="Virgo").ID},
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Lima" ).ID, FurnitureID = furniture.Single(i => i.Title =="Santal").ID},
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Dumas" ).ID, FurnitureID = furniture.Single(i => i.Title =="Anabelle").ID},
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Fist" ).ID, FurnitureID = furniture.Single(i => i.Title =="Magic Bed").ID},
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Lima" ).ID, FurnitureID = furniture.Single(i => i.Title =="Griffin").ID},
 new ManufacturedFurniture {FactoryID = factories.Single(c => c.FactoryName == "Fist" ).ID, FurnitureID = furniture.Single(i => i.Title =="Fuzia").ID},
            };
            foreach (ManufacturedFurniture pb in manufacture)
            {
                context.ManufacturedFurnitures.Add(pb);
            }
            context.SaveChanges();
        }
    }
}

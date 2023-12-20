using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Server.Data
{
    public class DbInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            
            if (context.Roles.Any())
            {
                return;
            }
            
            var roles = new Role[]
            {
                new() { Name = "Admin" },
                new() { Name = "Patient" },
                new() { Name = "Doctor" },
                new() { Name = "Clinic Manager" },
                new() { Name = "Moderator" }
            };
            foreach (var r in roles)
            {
                context.Roles.Add(r);
            }

            context.SaveChanges();

            var genders = new Gender[]
            {
                new() { Name = "Man" },
                new() { Name = "Woman" },
            };

            foreach (var g in genders)
            {
                context.Genders.Add(g);
            }

            var doctors = new User[]
            {
                new()
                {
                    BirthDate = DateTime.ParseExact("15.03.1980 00:00:00", "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture), GenderId = 1, FirstName = "Олександр",
                    SecondName = "Іванович", LastName = "Карпенко", Phone = "+380969379992",
                    Avatar = "/img/doctor_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Admin")).Id,
                    Login = "admin", Password = "admin"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("20.05.1970 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 2, FirstName = "Ірина",
                    SecondName = "Василівна", LastName = "Іванова", Phone = "+380974649565",
                    Avatar = "/img/doctor_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Doctor")).Id,
                    Login = "doctor", Password = "doctor"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("11.10.1985 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 2, FirstName = "Валентина",
                    SecondName = "Зіновіївна", LastName = "Карпіна", Phone = "+380503182509",
                    Avatar = "/img/doctor_avatar.png",
                    RoleId = context.Roles.First(x => x.Name.Equals("Clinic Manager")).Id, Login = "manager",
                    Password = "manager"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("21.06.1990 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 1, FirstName = "Сергій",
                    SecondName = "Ігорович", LastName = "Капсамун", Phone = "+380961322552",
                    Avatar = "/img/doctor_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Moderator")).Id,
                    Login = "moderator", Password = "moderator"
                },
            };

            foreach (var u in doctors)
            {
                context.Users.Add(u);
            }

            var clients = new User[]
            {
                new()
                {
                    BirthDate = DateTime.ParseExact("11.04.1983 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 1, FirstName = "Олександр",
                    SecondName = "Іванович", LastName = "Карпенко", Phone = "+380989875214",
                    Avatar = "/img/men_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Patient")).Id,
                    Login = "client1", Password = "client1"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("13.02.1981 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 2, FirstName = "Ірина",
                    SecondName = "Василівна", LastName = "Іванова", Phone = "+380978523697",
                    Avatar = "/img/women_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Patient")).Id,
                    Login = "client2", Password = "client2"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("21.10.1990 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 2, FirstName = "Валентина",
                    SecondName = "Зіновіївна", LastName = "Карпіна", Phone = "+380962365142",
                    Avatar = "/img/women_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Patient")).Id,
                    Login = "client3", Password = "client3", Email = "ribibo9770@bayxs.com"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("28.06.1985 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 1, FirstName = "Сергій",
                    SecondName = "Ігорович", LastName = "Капсамун", Phone = "+380961478536",
                    Avatar = "/img/men_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Patient")).Id,
                    Login = "client4", Password = "client4"
                },
                new()
                {
                    BirthDate = DateTime.ParseExact("10.04.1984 00:00:00", "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture), GenderId = 2, FirstName = "Марія",
                    SecondName = "Юріївна", LastName = "Картакай", Phone = "+380503588856",
                    Avatar = "/img/women_avatar.png", RoleId = context.Roles.First(x => x.Name.Equals("Patient")).Id,
                    Login = "client5", Password = "client5"
                }
            };

            foreach (var u in clients)
            {
                context.Users.Add(u);
            }


            var results = new Result[]
            {
                new() { Name = "ALL RIGHT" },
                new() { Name = "WILL REQUIRE HOSPITALIZATION" },
                new() { Name = "YOU WILL NEED RE-QUITING" },
                new() { Name = "TREATMENT ADVICE PRESCRIBED" }
            };

            foreach (var r in results)
            {
                context.Results.Add(r);
            }

            context.SaveChanges();
        }
    }
}
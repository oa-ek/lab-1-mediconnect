using Shared.Models;
using Server.Data;
using Shared.Models;
using System;
using System.Linq;

namespace Server.Data
{
    public class DbInitializer
    {
        public static void Initialize(Context context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any roles
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            //Gender
            var genders = new Gender[]
            {
                new Gender{ Name = "Man"},
                new Gender{ Name = "Woman"},
            };

            foreach (Gender g in genders)
            {
                context.Genders.Add(g);
            }
            context.SaveChanges();

            //спеціальності
            var roles = new Role[]
            {
                new Role{Name = "Admin"},
                new Role{Name = "Patient"},
                new Role{Name = "Doctor"},
                new Role{Name = "Clinic Manager"},
                new Role{Name = "Moderator"}
            };
            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            var doctors = new User[]
            {
                new User{BitrhDate = DateTime.Parse("15.03.1980 00:00:00"), GenderID = 1, FirstName = "Олександр" , SecondName = "Іванович", LastName = "Карпенко",  Phone ="+380969379992", Avatar = "/img/doctor_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Admin")).ID,  Login = "admin", Password = "admin"},
                new User{BitrhDate = DateTime.Parse("20.05.1970 00:00:00"), GenderID = 2, FirstName = "Ірина" , SecondName = "Василівна", LastName = "Іванова",  Phone ="+380974649565", Avatar = "/img/doctor_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Doctor")).ID,  Login = "doctor", Password = "doctor"},
                new User{BitrhDate = DateTime.Parse("11.10.1985 00:00:00"), GenderID = 2, FirstName = "Валентина" , SecondName = "Зіновіївна", LastName = "Карпіна",  Phone ="+380503182509", Avatar = "/img/doctor_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Clinic Manager")).ID, Login = "manager", Password = "manager"},
                new User{BitrhDate = DateTime.Parse("21.06.1990 00:00:00"), GenderID = 1, FirstName = "Сергій" , SecondName = "Ігорович", LastName = "Капсамун", Phone ="+380961322552", Avatar = "/img/doctor_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Moderator")).ID,  Login = "moderator", Password = "moderator"},
            };

            foreach (User u in doctors)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var clients = new User[]
            {
                new User{BitrhDate = DateTime.Parse("11.04.1983 00:00:00"), GenderID = 1, FirstName = "Олександр" , SecondName = "Іванович", LastName = "Карпенко",  Phone ="+380989875214", Avatar = "/img/men_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Patient")).ID,  Login = "client1", Password = "client1"},
                new User{BitrhDate = DateTime.Parse("13.02.1981 00:00:00"), GenderID = 2, FirstName = "Ірина" , SecondName = "Василівна", LastName = "Іванова",  Phone ="+380978523697", Avatar = "/img/women_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Patient")).ID,  Login = "client2", Password = "client2"},
                new User{BitrhDate = DateTime.Parse("21.10.1990 00:00:00"), GenderID = 2, FirstName = "Валентина" , SecondName = "Зіновіївна", LastName = "Карпіна",  Phone ="+380962365142", Avatar = "/img/women_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Patient")).ID, Login = "client3", Password = "client3"},
                new User{BitrhDate = DateTime.Parse("28.06.1985 00:00:00"), GenderID = 1, FirstName = "Сергій" , SecondName = "Ігорович", LastName = "Капсамун", Phone ="+380961478536", Avatar = "/img/men_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Patient")).ID,  Login = "client4", Password = "client4"},
                new User{BitrhDate = DateTime.Parse("10.04.1984 00:00:00"), GenderID = 2, FirstName = "Марія" , SecondName = "Юріївна", LastName = "Картакай",  Phone ="+380503588856", Avatar = "/img/women_avatar.png" ,  RoleID =  context.Roles.First(x=>x.Name.Equals("Patient")).ID, Login = "client5", Password = "client5"}
            };

            foreach (User u in clients)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();


            var results = new Result[]
            {
                new Result { Name = "ALL RIGHT"},
                new Result { Name = "WILL REQUIRE HOSPITALIZATION"},
                new Result { Name = "YOU WILL NEED RE-QUITING"},
                new Result { Name = "TREATMENT ADVICE PRESCRIBED"}
            };

            foreach (Result r in results)
            {
                context.Results.Add(r);
            }
            context.SaveChanges();

        }
    }
}

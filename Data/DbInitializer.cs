using MediConnect.Models;
using System;
using System.Linq;

namespace MediConnect.Data
{
    public class DbInitializer
    {
        public static void Initialize(Context context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Roles.Any())
            {
                return;   // DB has been seeded
            }

            //стать
            var genders = new Gender[]
            {
                new Gender{ Name = "Чоловіча"},
                new Gender{ Name = "Жіноча"},
            };

            foreach (Gender g in genders)
            {
                context.Genders.Add(g);
            }
            context.SaveChanges();

            //спеціальності
            var roles = new Role[]
            {
                new Role{ Name = "Директор"},
                new Role{ Name = "Реєстратура"},
                new Role{ Name = "Клієнт"},
                new Role{ Name ="акушер-гінеколог"},
                new Role{ Name ="акушер-гінеколог цехової лікарської ділянки"},
                new Role{ Name ="алерголог-імунолог"},
                new Role{ Name ="анестезіолог-реаніматолог"},
                new Role{ Name ="бактеріолог"},
                new Role{ Name ="вірусолог"},
                new Role{ Name ="гастроентеролог"},
                new Role{ Name ="гематолог"},
                new Role{ Name ="генетик"},
                new Role{ Name ="геріатр"},
                new Role{ Name ="дезінфектолог"},
                new Role{ Name ="дерматовенеролог"},
                new Role{ Name ="дитячий кардіолог"},
                new Role{ Name ="дитячий онколог"},
                new Role{ Name ="дитячий уролог-андролог"},
                new Role{ Name ="дитячий хірург"},
                new Role{ Name ="дитячий ендокринолог"},
                new Role{ Name ="діабетолог"},
                new Role{ Name ="дієтолог"},
                new Role{ Name ="здравпункта"},
                new Role{ Name ="інфекціоніст"},
                new Role{ Name ="кардіолог"},
                new Role{ Name ="клінічної лабораторної діагностики"},
                new Role{ Name ="клінічний міколог"},
                new Role{ Name ="клінічний фармаколог"},
                new Role{ Name ="колопроктолог"},
                new Role{ Name ="косметолог"},
                new Role{ Name ="лаборант"},
                new Role{ Name ="лабораторний генетик"},
                new Role{ Name ="лабораторний міколог"},
                new Role{ Name ="мануальної терапії"},
                new Role{ Name ="методист"},
                new Role{ Name ="невролог"},
                new Role{ Name ="нейрохірург"},
                new Role{ Name ="неонатолог"},
                new Role{ Name ="нефролог"},
                new Role{ Name ="загальної практики (сімейний лікар)"},
                new Role{ Name ="онколог"},
                new Role{ Name ="ортодонт"},
                new Role{ Name ="оториноларинголог"},
                new Role{ Name ="офтальмолог"},
                new Role{ Name ="офтальмолог-протезист"},
                new Role{ Name ="паразитолог"},
                new Role{ Name ="патологоанатом"},
                new Role{ Name ="педіатр"},
                new Role{ Name ="педіатр міський (районний)"},
                new Role{ Name ="педіатр дільничний"},
                new Role{ Name ="пластичний хірург"},
                new Role{ Name ="з водолазної медицини"},
                new Role{ Name ="із відновлювальної медицини"},
                new Role{ Name ="з гігієни дітей та підлітків"},
                new Role{ Name ="з гігієни харчування"},
                new Role{ Name ="з гігієни праці"},
                new Role{ Name ="з гігієнічного виховання"},
                new Role{ Name ="із комунальної гігієни"},
                new Role{ Name ="із лікувальної фізкультури"},
                new Role{ Name ="з медико-соціальної експертизи"},
                new Role{ Name ="із загальної гігієни"},
                new Role{ Name ="з радіаційної гігієни"},
                new Role{ Name ="з рентгенендоваскулярної діагностики та лікування"},
                new Role{ Name ="із санітарно-гігієнічних лабораторних досліджень"},
                new Role{ Name ="зі спортивної медицини"},
                new Role{ Name ="приймального відділення"},
                new Role{ Name ="профпатолог"},
                new Role{ Name ="психіатр"},
                new Role{ Name ="психіатр дільничний"},
                new Role{ Name ="психіатр дитячий"},
                new Role{ Name ="психіатр дитячий дільничний"},
                new Role{ Name ="психіатр підлітковий"},
                new Role{ Name ="психіатр підлітковий дільничний"},
                new Role{ Name ="психіатр-нарколог"},
                new Role{ Name ="психіатр-нарколог дільничний"},
                new Role{ Name ="психотерапевт"},
                new Role{ Name ="пульмонолог"},
                new Role{ Name ="радіолог"},
                new Role{ Name ="радіотерапевт"},
                new Role{ Name ="ревматолог"},
                new Role{ Name ="рентгенолог"},
                new Role{ Name ="рефлексотерапевт"},
                new Role{ Name ="сексолог"},
                new Role{ Name ="серцево-судинний хірург"},
                new Role{ Name ="швидкої медичної допомоги"},
                new Role{ Name ="статистик"},
                new Role{ Name ="стоматолог"},
                new Role{ Name ="стоматолог дитячий"},
                new Role{ Name ="стоматолог-ортопед"},
                new Role{ Name ="стоматолог-терапевт"},
                new Role{ Name ="стоматолог-хірург"},
                new Role{ Name ="судово-медичний експерт"},
                new Role{ Name ="судово-психіатричний експерт"},
                new Role{ Name ="сурдолог-оториноларинголог"},
                new Role{ Name ="сурдолог-протезист"},
                new Role{ Name ="терапевт"},
                new Role{ Name ="терапевт підлітковий"},
                new Role{ Name ="терапевт дільничний"},
                new Role{ Name ="терапевт дільничний цехової лікарської ділянки"},
                new Role{ Name ="токсиколог"},
                new Role{ Name ="торакальний хірург"},
                new Role{ Name ="травматолог-ортопед"},
                new Role{ Name ="трансфузіолог"},
                new Role{ Name ="ультразвукової діагностики"},
                new Role{ Name ="уролог"},
                new Role{ Name ="фізіотерапевт"},
                new Role{ Name ="фтизіатр"},
                new Role{ Name ="фтизіатр дільничний"},
                new Role{ Name ="функціональної діагностики"},
                new Role{ Name ="хірург"},
                new Role{ Name ="щелепно-лицьовий хірург"},
                new Role{ Name ="ендокринолог"},
                new Role{ Name ="ендоскопіст"},
                new Role{ Name ="епідеміолог"}
            };
            foreach (Role r in roles)
            {
                context.Roles.Add(r);
            }
            context.SaveChanges();

            var doctors = new User[]
            {
                new User{BitrhDate = DateTime.Parse("15.03.1980 00:00:00"), GenderID = 1, FirstName = "Олександр" , SecondName = "Іванович", LastName = "Карпенко",  Phone ="+380969379992", Avatar = "/img/doctor_avatar.png" ,  RoleID =  83,  Login = "director", Password = "director"},
                new User{BitrhDate = DateTime.Parse("20.05.1970 00:00:00"), GenderID = 2, FirstName = "Ірина" , SecondName = "Василівна", LastName = "Іванова",  Phone ="+380974649565", Avatar = "/img/doctor_avatar.png" ,  RoleID =  1,  Login = "register", Password = "register"},
                new User{BitrhDate = DateTime.Parse("11.10.1985 00:00:00"), GenderID = 2, FirstName = "Валентина" , SecondName = "Зіновіївна", LastName = "Карпіна",  Phone ="+380503182509", Avatar = "/img/doctor_avatar.png" ,  RoleID =  10, Login = "doctor1", Password = "doctor1"},
                new User{BitrhDate = DateTime.Parse("21.06.1990 00:00:00"), GenderID = 1, FirstName = "Сергій" , SecondName = "Ігорович", LastName = "Капсамун", Phone ="+380961322552", Avatar = "/img/doctor_avatar.png" ,  RoleID =  4,  Login = "doctor2", Password = "doctor2"},
                new User{BitrhDate = DateTime.Parse("14.08.1983 00:00:00"), GenderID = 2, FirstName = "Марія" , SecondName = "Юріївна", LastName = "Картакай",  Phone ="+380978998523", Avatar = "/img/doctor_avatar.png" ,  RoleID =  5, Login = "doctor3", Password = "doctor3"},
                new User{BitrhDate = DateTime.Parse("22.07.1992 00:00:00"), GenderID = 2, FirstName = "Єкатерина" , SecondName = "Іванівна", LastName = "Казимирская",  Phone ="+380932221121", Avatar = "/img/doctor_avatar.png" ,  RoleID =  6,  Login = "doctor4", Password = "doctor4"},
                new User{BitrhDate = DateTime.Parse("28.12.1991 00:00:00"), GenderID = 2, FirstName = "Ольга" , SecondName = "Петрівна", LastName = "Шелестюк", Phone ="+380958525317", Avatar = "/img/doctor_avatar.png" ,  RoleID =  7, Login = "doctor5", Password = "doctor5"},
                new User{BitrhDate = DateTime.Parse("10.01.1989 00:00:00"), GenderID = 1, FirstName = "Вадим" , SecondName = "Олександрович", LastName = "Кушнір", Phone ="+380961597538", Avatar = "/img/doctor_avatar.png" ,  RoleID =  8, Login = "doctor6", Password = "doctor6"},
                new User{BitrhDate = DateTime.Parse("22.10.1990 00:00:00"), GenderID = 1, FirstName = "Микола" , SecondName = "Сергійович", LastName = "Чернявский",  Phone ="+380978526542", Avatar = "/img/doctor_avatar.png" ,  RoleID =  9, Login = "doctor7", Password = "doctor7"},
                new User{BitrhDate = DateTime.Parse("01.04.1987 00:00:00"), GenderID = 1, FirstName = "Борис" , SecondName = "Миколайович", LastName = "Поліщук",  Phone ="+380963219875", Avatar = "/img/doctor_avatar.png" ,  RoleID =  11, Login = "doctor8", Password = "doctor8"},
                new User{BitrhDate = DateTime.Parse("13.09.1995 00:00:00"), GenderID = 1, FirstName = "Денис" , SecondName = "Сергійович", LastName = "Нестеренко",  Phone ="+380961235698", Avatar = "/img/doctor_avatar.png" ,  RoleID =  12,  Login = "doctor9", Password = "doctor9"},
                new User{BitrhDate = DateTime.Parse("24.11.1981 00:00:00"), GenderID = 2, FirstName = "Юлія" , SecondName = "Іванівна", LastName = "Кальчева", Phone ="+380985528532", Avatar = "/img/doctor_avatar.png" ,  RoleID =  13, Login = "doctor10", Password = "doctor10"},
                new User{BitrhDate = DateTime.Parse("12.02.1991 00:00:00"), GenderID = 2, FirstName = "Вікторія" , SecondName = "Петрівна", LastName = "Голобродько", Phone ="+380967418525", Avatar = "/img/doctor_avatar.png" ,  RoleID =  14, Login = "doctor11", Password = "doctor11"},
                new User{BitrhDate = DateTime.Parse("21.10.1993 00:00:00"), GenderID = 2, FirstName = "Галина" , SecondName = "Петрівна", LastName = "Гульченко",  Phone ="+380963579876", Avatar = "/img/doctor_avatar.png" ,  RoleID =  15, Login = "doctor12", Password = "doctor12"},
                new User{BitrhDate = DateTime.Parse("15.07.1981 00:00:00"),GenderID = 2, FirstName = "Лідія" , SecondName = "Георгіївна", LastName = "Андронакі",  Phone ="+380969856321", Avatar = "/img/doctor_avatar.png" ,  RoleID =  16, Login = "doctor13", Password = "doctor13"},
                new User{BitrhDate = DateTime.Parse("11.01.1991 00:00:00"),GenderID = 2, FirstName = "Олена" , SecondName = "Іванівна", LastName = "Галкіна",  Phone ="+380961586245", Avatar = "/img/doctor_avatar.png" ,  RoleID =  17, Login = "doctor14", Password = "doctor14"}
            };

            foreach (User u in doctors)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var clients = new User[]
            {
                new User{BitrhDate = DateTime.Parse("11.04.1983 00:00:00"), GenderID = 1, FirstName = "Олександр" , SecondName = "Іванович", LastName = "Карпенко",  Phone ="+380989875214", Avatar = "/img/men_avatar.png" ,  RoleID =  82,  Login = "client1", Password = "client1"},
                new User{BitrhDate = DateTime.Parse("13.02.1981 00:00:00"), GenderID = 2, FirstName = "Ірина" , SecondName = "Василівна", LastName = "Іванова",  Phone ="+380978523697", Avatar = "/img/women_avatar.png" ,  RoleID =  82,  Login = "client2", Password = "client2"},
                new User{BitrhDate = DateTime.Parse("21.10.1990 00:00:00"), GenderID = 2, FirstName = "Валентина" , SecondName = "Зіновіївна", LastName = "Карпіна",  Phone ="+380962365142", Avatar = "/img/women_avatar.png" ,  RoleID =  82, Login = "client3", Password = "client3"},
                new User{BitrhDate = DateTime.Parse("28.06.1985 00:00:00"), GenderID = 1, FirstName = "Сергій" , SecondName = "Ігорович", LastName = "Капсамун", Phone ="+380961478536", Avatar = "/img/men_avatar.png" ,  RoleID =  82,  Login = "client4", Password = "client4"},
                new User{BitrhDate = DateTime.Parse("10.04.1984 00:00:00"), GenderID = 2, FirstName = "Марія" , SecondName = "Юріївна", LastName = "Картакай",  Phone ="+380503588856", Avatar = "/img/women_avatar.png" ,  RoleID =  82, Login = "client5", Password = "client5"}
            };

            foreach (User u in clients)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();


            var results = new Result[]
            {
                new Result { Name = "НОРМАЛЬНО"},
                new Result { Name = "ПОТРЕБУЄ ГОСПІТАЛІЗАЦІЇ"},
                new Result { Name = "ПОТРЕБУЄ ПОВТОРНОГО ОБСТЕЖЕННЯ"},
                new Result { Name = "ВИПИСАНІ ПОРАДИ ЩОДО ЛІКУВАННЯ"}
            };

            foreach (Result r in results)
            {
                context.Results.Add(r);
            }
            context.SaveChanges();

        }
    }
}

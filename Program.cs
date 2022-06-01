using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace dailywords_Robot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        

        public static void MyUsers(User user)
        {
            string path = "users.json";
            var temp = File.ReadAllText(path);
            var readUseres = JsonConvert.DeserializeObject<AllUsers>(temp);
            if (readUseres is null)
            {
                var newUsers = new AllUsers();
                newUsers.Myusers.Add(user);
                var data = JsonConvert.SerializeObject(newUsers);
                File.WriteAllText(path, data);
            }
            else if (!readUseres.Myusers.Exists(usr => usr.userID == user.userID))
            {
                readUseres.Myusers.Add(user);
                var data = JsonConvert.SerializeObject(readUseres);
                File.WriteAllText(path, data);
            }
        }

        public class User
        {
            public string fullName { get; set; }

            public long userID { get; set; }

            public string date { get; set; }

            public User(string fullName, long userID, string date)
            {
                this.fullName = fullName;
                this.userID = userID;
                this.date = date;
            }
        }

        public class AllUsers
        {
            public List<User> Myusers { get; set; }
            public AllUsers()
            {
                this.Myusers = new List<User>();
            }
        }


        public static int Count()
        {
            string path = "users.json";
            string temp = File.ReadAllText(path);
            var myusers = JsonConvert.DeserializeObject<AllUsers>(temp);
            if (myusers is null) return 0;
            return myusers.Myusers.Count;
        }
    }
}

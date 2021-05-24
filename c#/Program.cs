using System;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace c_
{
    public struct EmailRecord
    {
        public string email { get; set; }
        public static EmailRecord FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            EmailRecord emailRecord = new EmailRecord();
            emailRecord.email = values[0].ToUpper();
            return emailRecord;
        }
    }
    
    public struct SignInUser
    {
        public string email { get; set; }
        public string ipaddress { get; set; }
        public static SignInUser FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            SignInUser signInUser = new SignInUser();
            signInUser.email = values[1].ToUpper();
            signInUser.ipaddress = values[5];
            return signInUser;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int blah = 0;
            List<SignInUser> phone_users_with_ip = new List<SignInUser>();
            List<EmailRecord> phone_users = new List<EmailRecord>();

            var df = @"..\\data\\csv\\DevicesWithInventoryUsernameOnly.csv";
            var sf = @"..\\data\\csv\\InteractiveSignIns.csv";

            List<EmailRecord> email_list = File.ReadAllLines(df)
                                           .Skip(1)
                                           .Select(v => EmailRecord.FromCsv(v))
                                           .ToList();
            List<SignInUser> sign_in_users = File.ReadAllLines(sf)
                                           .Skip(1)
                                           .Select(v => SignInUser.FromCsv(v))
                                           .ToList();

            for (int i = 0; i < sign_in_users.Count; i++)
            {
                if (sign_in_users[i].ipaddress == "1211.2223.3374.6644" || sign_in_users[i].ipaddress == "2250.23331.4221.9822" || sign_in_users[i].ipaddress == "1332.1398.2110.2022")
                {
                    blah++;
                }
                else if (!email_list.Select(x => x.email).Contains(sign_in_users[i].email))
                {
                    phone_users.Add(new EmailRecord { email = sign_in_users[i].email });
                    phone_users_with_ip.Add(new SignInUser { email = sign_in_users[i].email, ipaddress = sign_in_users[i].ipaddress });
                    blah++;
                }
                else
                {
                    blah++;
                }
            }

            Console.WriteLine("Total records processed: " + blah);

            var unique_phone_users = phone_users.Select(x => x.email).Distinct();
            var unique_phone_users_with_ip = phone_users_with_ip.Distinct();

            Console.WriteLine(unique_phone_users.Count());
            Console.WriteLine(unique_phone_users_with_ip.Count());

            using (StreamWriter file = new StreamWriter(@"..\\data\\output\\usersoutputcsharp.csv"))
            {
                foreach (var line in unique_phone_users)
                {
                    file.WriteLine(line);
                }
            }
            using (StreamWriter file = new StreamWriter(@"..\\data\\output\\usersoutputwithipcsharp.csv"))
            {
                foreach (var line in unique_phone_users_with_ip)
                {
                    file.WriteLine(line.email + "," + line.ipaddress);
                }
            }

            stopWatch.Stop();
            Console.WriteLine(stopWatch.Elapsed);
        }
    }
}

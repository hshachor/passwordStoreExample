using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace passwordStore
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public int Salt { get; set; }
        public string HashedPassword { get; set; }
    }

    class Program
    {
        static Random rnd = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("what do you want to do?\n1)register\n2)login");
            int choice = int.Parse(Console.ReadLine());
            string password = readPassword();
            switch (choice)
            {
                case 1:
                    int salt = rnd.Next();
                    User u = new User { Id = 1, Salt = salt, HashedPassword = hashPassword(password + salt) };
                    FileStream wfs = new FileStream("users.xml", FileMode.Create); 
                    XmlSerializer x = new XmlSerializer(u.GetType()); 
                    x.Serialize(wfs, u);
                    break;
                case 2:
                    FileStream rfs = new FileStream("users.xml", FileMode.Open); 
                    XmlSerializer rx = new XmlSerializer(typeof(User)); 
                    User ru = rx.Deserialize(rfs) as User;
                    if (hashPassword(password + ru.Salt) == ru.HashedPassword)
                    {
                        Console.WriteLine("congratulation, hello user ");
                    } else
                    {
                        Console.WriteLine("wrong password");
                    }
                    break;
            }
        }

        private static string hashPassword(string passwordWithSalt)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt)));
        }

        private static string readPassword()
        {
            Console.WriteLine("Please enter password");
            return Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ISOFromCountry
{
    class Program
    {
        static void Main(string[] args)
        {
            var country = Country.FromString("Tampere, Pirkanmaa, Finland");
            Console.WriteLine(country.code);
        }



        public class Country
        {
            private static readonly List<Country> Countries;

            static Country()
            {  
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith("countries.csv"));
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null) throw new Exception("Cannot find embedded resource countries.csv");
                var sr = new StreamReader(stream);
                var strAllCsv = sr.ReadToEnd();
                var lines = strAllCsv.Split('\n');
                var twoDimensions = lines.Select(l => l.Split(',')).ToArray();
                Countries = twoDimensions.Select(p => new Country { name = p[0], code = p[1], numericCode = Convert.ToInt16(p[2]) }).ToList();
            }

            public static Country FromString(string address)
            {
                return Countries.FirstOrDefault(country => address.ToLower().Contains(country.name.ToLower()));
            }

            public string name { get; set; }
            public string code { get; set; }
            public int numericCode { get; set; }
        }

    }
}

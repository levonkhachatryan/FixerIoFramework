using FixerI0.Library;
using System;

namespace FixerIo.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please Enter Your API Access Key: ");
            string accessKey = Console.ReadLine();
            var fixerClient = new FixerIoClient(accessKey);
            Currency response = fixerClient.GetLatest(Rates.RUB);
            //Currency response = fixerClient.GetLatest(Rates.AED,Rates.USD);
            //Currency response = fixerClient.Get(DateTime.Parse("2013,03,16"));
            //Currency response = fixerClient.Get(DateTime.Parse("2013,03,16"), Rates.AED, Rates.USD, Rates.RUB);


            Console.ReadKey();
        }
    }
}

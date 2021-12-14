//OCR June 2020 and June 2021 series GCSE (9-1) Computer Science
//Written by Nourdin Ibrahim, no copypaste!
// .NET 6.0
// Newtonsoft.Json


using System;

namespace Task1MusicQuiz
{
    public class Program
    {
        //Default function to run on start.
        public static void Main(string[] args)
        {
            //Initialize seperate class instance
            Application application = new Application();
            //Runs the asynchrnous start function. Does not quit the app until the 
            //app is finished .GetAwaiter().GetResult()
            application.RunAsync().GetAwaiter().GetResult();
        }
    }
}
using System;
using System.Collections.Generic;

namespace DriveVidStore_Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Worker started!");
            var worker = new AzureWorker();
            while (true) // TODO: Add some type of kill switch to gracefully shut down
            {
                worker.PollAndProcessJob();
                System.Threading.Thread.Sleep(50);
            }
        }
    }
}

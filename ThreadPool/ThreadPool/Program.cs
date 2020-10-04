using System;
using System.IO;
using System.Linq;
using ThreadPool.FileService;
using ThreadPool.Pool;

namespace ThreadPool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Count() < 2)
            {
                Console.WriteLine("Not enough arguments.");
                return;
            }
            
            if (!Directory.Exists(args[0])) return;
            
            FileCopyService fileCopyService = new FileCopyService( new TaskQueue(100));
            fileCopyService.StartCopying(Path.GetFullPath(args[0]), Path.GetFullPath(args[1]));
        }
    }
}
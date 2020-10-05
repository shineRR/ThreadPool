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
            if (args.Count() != 3)
            {
                Console.WriteLine("Not enough arguments.");
                return;
            }
            string src = args[0];
            string dest = args[1];
            int threads = 0;
            int.TryParse(args[2], out threads);
          
            if ((!Directory.Exists(src) || !Directory.Exists(Directory.GetDirectoryRoot(dest)) || 
                src == dest) || threads < 1) return;
            Console.WriteLine(threads);
            FileCopyService fileCopyService = new FileCopyService(new TaskQueue(threads));
            fileCopyService.StartCopying(Path.GetFullPath(src), Path.GetFullPath(dest));
        }
    }
}
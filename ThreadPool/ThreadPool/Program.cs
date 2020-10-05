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
            string src = args[0];
            string dest = args[1];
            
            if (!Directory.Exists(src) || !Directory.Exists(Directory.GetDirectoryRoot(dest)) || 
                src == dest) return;
            FileCopyService fileCopyService = new FileCopyService( new TaskQueue(100));
            fileCopyService.StartCopying(Path.GetFullPath(src), Path.GetFullPath(dest));
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ThreadPool.Pool;

namespace ThreadPool.FileService
{
    public class FileCopyService : IFileCopyService
    {
        private static int _copiedFiles = 0;
        private readonly TaskQueue _taskQueue;

        public FileCopyService(TaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }
        
        private static List<string> GetDirs(string path)
        {
            string searchPattern = "*.*";
            List<string> list = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            DirectoryInfo[] directories =
                di.GetDirectories(searchPattern, SearchOption.AllDirectories);

            foreach (var dir in directories)
            {
                list.Add(dir.FullName);
            }
            return list;
        } 
        
        private static List<string> GetFiles(string path)
        {
            string searchPattern = "*.*";
            List<string> list = new List<string>();
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files =
                di.GetFiles(searchPattern, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                list.Add(file.FullName);
            }
            return list;
        }

        private static void PrintCopiedFiles(ref Thread[] threads)
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }
            Console.WriteLine("Total Copied: " + _copiedFiles);
        }
        public void CreateMissingDirs(string src, string dest)
        {
            foreach (var dir in GetDirs(src))
            {
                try
                {
                    Directory.CreateDirectory(dir.Replace(src, dest));

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                } 
            }
        }

        public void StartCopying(string src, string dest)
        {
            CreateMissingDirs(src, dest);
            List<String> dirFileList = GetFiles(src);
            FileAttributes attr = File.GetAttributes(dest);
            if (!attr.HasFlag(FileAttributes.Directory)) return;
            Directory.CreateDirectory(dest);
            
            foreach (var file in dirFileList)
            {
                string fileName = file.Replace(src, dest);

                _taskQueue.EnqueueTask(delegate {
                    try
                    {
                        File.Copy(file, fileName, true);
                        Interlocked.Increment(ref _copiedFiles);
                        Console.WriteLine("from: " + file + ", to: " + fileName + " by " + Thread.CurrentThread.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
                
            }
            _taskQueue.Dispose();
            PrintCopiedFiles(ref _taskQueue.ThreadQueue);
        }
    }
}
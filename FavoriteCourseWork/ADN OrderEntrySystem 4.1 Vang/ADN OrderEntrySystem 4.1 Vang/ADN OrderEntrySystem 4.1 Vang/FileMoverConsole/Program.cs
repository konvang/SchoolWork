using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileMoverConsole
{
    public class Program
    {
        private static FileSystemWatcher fileSystemWatcher;

        private static string source = @"C:\\temp";

        private static string destination = @"C:\\end";

        static void Main(string[] args)
        {
            fileSystemWatcher = new FileSystemWatcher(source);

            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Created += OnCreate;
            fileSystemWatcher.Renamed += OnRename;
            fileSystemWatcher.Changed += OnChange;

            Console.ReadLine();
        }

        private static void OnChange(object sender, FileSystemEventArgs e)
        {
            EventLog log = new EventLog();
            log.Source = "Application";

            try
            {
                log.WriteEntry(string.Format("File was moved {0}", e.FullPath), EventLogEntryType.Information);
            }

            catch
            {
                log.WriteEntry(string.Format("File was not moved {0}", e.FullPath), EventLogEntryType.Error);
                Console.WriteLine(" The file could not be moved from the starting folder to the destination folder.");
            }

            OnCreate(sender, e);
        }

        private static void OnCreate(object sender, FileSystemEventArgs e)
        {
            try
            {
                if (File.Exists(e.FullPath))
                {
                    Thread.Sleep(1000);

                    File.Move(e.FullPath, $"{destination}\\{e.Name}");
                    Console.WriteLine($"{e.FullPath} was moved to {destination}\\");
                }
            }
            catch
            {
                Console.WriteLine($"{e.FullPath} was not moved to {destination}\\");
            }
        }

        private static void OnRename(object sender, RenamedEventArgs e)
        {
            OnCreate(sender, e);
        }
    }
}

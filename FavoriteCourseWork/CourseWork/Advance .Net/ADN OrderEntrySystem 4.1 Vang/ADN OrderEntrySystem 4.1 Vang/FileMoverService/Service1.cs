using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileMoverService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private static FileSystemWatcher fileSystemWatcher;

        private static string source = @"C:\\temp";

        private static string destination = @"C:\\end";

        protected override void OnStart(string[] args)
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "Application";

            try
            {
                eventLog.WriteEntry("The service started.", EventLogEntryType.Information);
            }
            catch
            {

                eventLog.WriteEntry("The service could not start.", EventLogEntryType.Error);
            }


            fileSystemWatcher = new FileSystemWatcher(source);
            fileSystemWatcher.EnableRaisingEvents = true;
            fileSystemWatcher.Created += OnCreate;
            fileSystemWatcher.Renamed += OnRename;
            fileSystemWatcher.Changed += OnChange;

            eventLog.Source = "Applicaton";
            eventLog.WriteEntry("Service was started.");


        }

        public void Debug()
        {
            OnStart(null);
        }

        protected override void OnStop()
        {

            EventLog eventLog = new EventLog();
            eventLog.Source = "Application";

            try
            {
                eventLog.WriteEntry("The service stopped.", EventLogEntryType.Information);
            }
            catch
            {

                eventLog.WriteEntry("The service could not stop.", EventLogEntryType.Error);
            }
        }


        private static void OnChange(object sender, FileSystemEventArgs e)
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "Application";

            try
            {
                eventLog.WriteEntry(string.Format("A file was moved {0}", e.FullPath), EventLogEntryType.Information);
            }
            catch
            {

                eventLog.WriteEntry(string.Format("The file was not moved {0}", e.FullPath), EventLogEntryType.Error);
                Console.WriteLine("File was not moved from the source folder to the destination folder.");
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

                    EventLog eventLog = new EventLog();
                    eventLog.Source = "Applicaton";
                    eventLog.WriteEntry("Information");
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChitChit
{
    public class MonitorFile
    {
        private String FilePath;
        private bool let;
        private bool FileHasChanged;

        public bool FileChange
        {
            get { return FileHasChanged; }
            set { FileHasChanged = value; }
        }

        public MonitorFile(String path)
        {
            FilePath = path;
            let = false;
            FileChange = false;
        }
        public void Watch()
        {
            string filePath1 = FilePath;// @"d:\opengl\image\1.txt";
            FileSystemWatcher fwatcher = new FileSystemWatcher();
            fwatcher.Path = Path.GetDirectoryName(filePath1);
            fwatcher.Filter = Path.GetFileName(filePath1);
            fwatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            fwatcher.Changed += new FileSystemEventHandler(Changed);
            fwatcher.Created += new FileSystemEventHandler(FileCreated);
            fwatcher.Deleted += new FileSystemEventHandler(Deleted);
            fwatcher.Renamed += new RenamedEventHandler(Renamed);
            fwatcher.EnableRaisingEvents = true;
        }
        private void Changed(object sender, FileSystemEventArgs e)
        {
            if (!let)
            {
                Console.WriteLine(e.FullPath.ToString() + " is changed!");
                let = true;
                FileChange = true;


            }
            else
            {
                let = false;
            }
        }
        private void Deleted(object sender, FileSystemEventArgs e)
        {
            if (!let)
            {
                Console.WriteLine(e.FullPath.ToString() + " is delete!");
                let = true;
                FileChange = true;


            }
            else
            {
                let = false;
            }
        }
        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            if (!let)
            {
                Console.WriteLine(e.FullPath.ToString() + " is FileCreated!");
                let = true;
                FileChange = true;


            }
            else
            {
                let = false;
            }
        }
        private void Renamed(object sender, FileSystemEventArgs e)
        {
            if (!let)
            {
                Console.WriteLine(e.FullPath.ToString() + " is Renamed!");
                let = true;
                FileChange = true;


            }
            else
            {
                let = false;
            }
        }
    }
}

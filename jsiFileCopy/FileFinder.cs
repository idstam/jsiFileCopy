using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace jsiFileCopy
{
    public class FileFinder
    {
        private readonly string _source;
        private readonly List<string> _skipFolders;
        private readonly List<string> _skipFiles;

        public int Count { get; set; }
        public ConcurrentQueue<string> FoundFiles { get; }

        public bool Done { get; set; }

        public FileFinder(string source, string skipFolders, string skipFiles)
        {
            _source = source;
            _skipFolders =new List<string>( skipFolders.Split('|'));
            _skipFiles = new List<string>(skipFiles.Split('|'));
            FoundFiles = new ConcurrentQueue<string>();
        }

        public void Find()
        {
            findFiles(_source);
            Done = true;
        }

        private void findFiles(string root)
        {
            foreach(var folder in System.IO.Directory.GetDirectories(root))
            {
                var d = new DirectoryInfo(folder);
                if (!_skipFolders.Contains(d.Name))
                {
                    findFiles(d.FullName);
                }
            }
            foreach (var file in System.IO.Directory.GetFiles(root))
            {
                var f = new FileInfo(file);
                if (!_skipFiles.Contains(f.Extension))
                {
                    FoundFiles.Enqueue(file);
                    Count++;
                }
            }
        }
    }
}

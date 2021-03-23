using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace jsiFileCopy
{
    public class FileCopyer
    {
        private readonly string _source;
        private readonly string _dest;
        private FileFinder _fileFinder;
        private readonly bool _overwrite;
        public bool Done { get; set; }
        public int Count { get; set; }
        public List<string> FileNames { get; }


        public FileCopyer(string source, string dest,  FileFinder fileFinder, bool overwrite)
        {
            _source = source;
            _dest = dest;
            _fileFinder = fileFinder;
            _overwrite = overwrite;
        }

        public void Copy()
        {
            var destRoot = new DirectoryInfo(_dest).FullName;
            var sourceRoot = new DirectoryInfo(_source).FullName;

            while (true)
            {
                string sourceFile;
                var success= _fileFinder.FoundFiles.TryDequeue(out sourceFile);
                if (success)
                {
                    var destFile = sourceFile.Replace(sourceRoot, destRoot);
                    var f = new FileInfo(destFile);
                    Directory.CreateDirectory(f.Directory.FullName);

                    File.Copy(sourceFile, destFile, _overwrite);
                    Count++;
                    continue;
                }else
                {
                    Done = true;
                    return;
                }
            }
        }
    }
}

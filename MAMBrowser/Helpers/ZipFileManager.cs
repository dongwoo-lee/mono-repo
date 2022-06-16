using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace MAMBrowser.Helpers
{
    public class ZipFileManager
    {
        private static readonly Lazy<ZipFileManager> lazy = new Lazy<ZipFileManager>(() => new ZipFileManager());
        public static ZipFileManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public void CreateZIPFile(string backupFolder, string zipFilePath)
        {
            using (FileStream fileStream = new FileStream(zipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    foreach (string filePath in Directory.EnumerateFiles(backupFolder, "*.*", SearchOption.AllDirectories))
                    {
                        string relativePath = filePath.Substring(backupFolder.Length + 1);
                        if (!Path.GetFileName(relativePath).Equals(Path.GetFileName(zipFilePath)))
                        {
                            try
                            {
                                zipArchive.CreateEntryFromFile(filePath, relativePath);
                            }
                            catch (PathTooLongException)
                            {
                            }
                        }

                    }
                }
            }
        }

    }
}

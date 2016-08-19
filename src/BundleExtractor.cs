using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TextmateBundleInstaller
{
    public class BundleExtractor
    {
        public static async Task<bool> CopyBundles()
        {
            string src = GetSourceFolder();
            string dest = GetDestinationFolder();

            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    if (Directory.Exists(dest))
                        Directory.Delete(dest, true);

                    CopyDirectory(src, dest);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex);
                    return false;
                }

                return true;
            });
        }

        public static bool HasFilesBeenCopied()
        {
            string src = GetSourceFolder();
            string dest = GetDestinationFolder();

            if (!Directory.Exists(dest))
                return false;

            return Directory.GetDirectories(src).Count() == Directory.GetDirectories(dest).Count();
        }

        // from http://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp
        private static void CopyDirectory(string src, string dest)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(src);
            FileInfo[] files = dirInfo.GetFiles();

            foreach (FileInfo tempfile in files)
            {
                tempfile.CopyTo(Path.Combine(dest, tempfile.Name));
            }

            DirectoryInfo[] directories = dirInfo.GetDirectories();

            foreach (DirectoryInfo tempdir in directories)
            {
                CopyDirectory(Path.Combine(src, tempdir.Name), Path.Combine(dest, tempdir.Name));
            }
        }

        private static string GetDestinationFolder()
        {
            return Environment.ExpandEnvironmentVariables("%userprofile%\\.vs\\Extensions");
        }

        private static string GetSourceFolder()
        {
            string assembly = Assembly.GetExecutingAssembly().Location;
            return Path.Combine(Path.GetDirectoryName(assembly), "Bundles");
        }
    }
}

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

            try
            {
                bool success = CleanDirectory(dest) & CopyDirectory(src, dest);

                if (success)
                    await WriteLogFile(dest);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return false;
            }

            return true;
        }

        private static bool CleanDirectory(string directory)
        {
            bool success = true;

            if (Directory.Exists(directory))
            {
                foreach (var childDir in Directory.GetDirectories(directory))
                {
                    try
                    {
                        Directory.Delete(childDir, true);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        public static async Task<bool> IsLogFileCurrent()
        {
            try
            {
                string dest = GetDestinationFolder();

                if (!Directory.Exists(dest))
                    return false;

                string src = GetSourceFolder();

                if (Directory.GetDirectories(dest).Count() < Directory.GetDirectories(src).Count())
                    return false;

                string logFile = Path.Combine(dest, Vsix.Name + ".log");

                if (!File.Exists(logFile))
                    return false;

                using (var reader = new StreamReader(logFile))
                {
                    return await reader.ReadToEndAsync() == Vsix.Version;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex);
                return false;
            }
        }

        private static async Task WriteLogFile(string dest)
        {
            string logFile = Path.Combine(dest, Vsix.Name + ".log");

            using (var writer = new StreamWriter(logFile, false))
            {
                await writer.WriteAsync(Vsix.Version);
            }
        }

        // from http://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp
        private static bool CopyDirectory(string src, string dest)
        {
            try
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dest);
                }

                DirectoryInfo dirInfo = new DirectoryInfo(src);
                FileInfo[] files = dirInfo.GetFiles();

                foreach (FileInfo tempfile in files)
                {
                    tempfile.CopyTo(Path.Combine(dest, tempfile.Name), true);
                }

                DirectoryInfo[] directories = dirInfo.GetDirectories();

                foreach (DirectoryInfo tempdir in directories)
                {
                    CopyDirectory(Path.Combine(src, tempdir.Name), Path.Combine(dest, tempdir.Name));
                }

                return true;
            }
            catch (Exception)
            {
                return false;
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

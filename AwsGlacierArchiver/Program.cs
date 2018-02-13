using System;
using System.IO;
using Amazon.Glacier;
using Amazon.Glacier.Transfer;
using Amazon.Runtime;

namespace AwsGlacierArchiver
{
    public class Program
    {
        private const string VaultName = "BrookeStapletonPhotography";
        private const string ArchiveToUpload = "C:\\Users\\i58740\\Documents\\Xactware";

        public static void Main(string[] args)
        {
            var directories = Directory.EnumerateDirectories(ArchiveToUpload);
            foreach (var directory in directories)
            { 
                if (DateTime.Now.Subtract(Directory.GetLastWriteTime(directory)) > TimeSpan.FromDays(60))
                {
                    try
                    {
                        var manager = new ArchiveTransferManager(Amazon.RegionEndpoint.USWest2);
                        var archiveId = manager.UploadAsync(VaultName, "upload archive test", directory).Result.ArchiveId;
                        var archiveName = Path.Combine(ArchiveToUpload, directory + "-ARCHIVE");

                        File.WriteAllText(archiveName, archiveId);
                        Directory.Delete(Path.Combine(ArchiveToUpload, directory), true);
                    }
                    catch (AmazonGlacierException e) { Console.WriteLine(e.Message); }
                    catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
                    catch (Exception e) { Console.WriteLine(e.Message); }
                }
            }

        }
    }
}

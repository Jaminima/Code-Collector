using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeCollector
{
    class Program
    {
        static List<string> FileExt = new List<string> { "cs" },
            PathExceptions = new List<string> { "bin","obj", "Properties" };
        static void Main(string[] args)
        {
            Dictionary<string,string> CodeCollection = GetCodeCollection("D:/Programming/ComputerScienceCoursework/Code/Twitch-Discord-Reward-Bot/Twitch-Discord-Reward-Bot/Twitch-Discord-Reward-Bot");
            WriteCodeCollectionToFile(CodeCollection, "./Bot.txt");
            CodeCollection = GetCodeCollection("D:/Programming/ComputerScienceCoursework/Code/Twitch-Discord-Reward-Bot/Twitch-Discord-Reward-API/Twitch-Discord-Reward-API");
            WriteCodeCollectionToFile(CodeCollection, "./API.txt");
        }

        static void WriteCodeCollectionToFile(Dictionary<string,string> CodeCollection,string FilePath)
        {
            string FileContents="";
            foreach (KeyValuePair<string,string> CodeFilePair in CodeCollection)
            {
                FileContents += CodeFilePair.Key + "\n\n" + CodeFilePair.Value + "\n";
            }
            System.IO.File.WriteAllText(FilePath,FileContents);
        }

        static Dictionary<string,string> GetCodeCollection(string DirectoryPath)
        {
            Dictionary<string, string> FileCodePairs = new Dictionary<string, string> { };
            TraverseDirectoryFolders(DirectoryPath,DirectoryPath,FileCodePairs);
            return FileCodePairs;
        }

        static void TraverseDirectoryFolders(string DirectoryPath,string SourcePath, Dictionary<string, string> FileCodePairs)
        {
            foreach (string ChildPaths in System.IO.Directory.GetDirectories(DirectoryPath).Where(x=>PathExceptions.Where(y=>x.Contains(y)).Count()==0))
            {
                TraverseDirectoryFolders(ChildPaths, SourcePath, FileCodePairs);
            }
            TraverseDirectoryFiles(DirectoryPath, SourcePath, FileCodePairs);
        }

        static void TraverseDirectoryFiles(string DirectoryPath, string SourcePath, Dictionary<string, string> FileCodePairs)
        {
            foreach (string FilePath in System.IO.Directory.GetFiles(DirectoryPath).Where(x=>FileExt.Where(y=>x.EndsWith(y)).Count()!=0))
            {
                FileCodePairs.Add(FilePath.Remove(0,SourcePath.Length+1).Replace("\\","/"), System.IO.File.ReadAllText(FilePath).Replace("\r\n","\r"));
            }
        }
    }
}

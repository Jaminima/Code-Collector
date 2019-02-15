using System.Collections.Generic;
using System.Linq;

namespace CodeCollector
{
    class Program
    {
        static List<string> FileExt = new List<string> { ".cs" }, //File extensions that will be placed in the Collection
            PathExceptions = new List<string> { "bin","obj", "Properties" };//Folder paths that wont be traversed
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
            //Add every File path and its corresponding contents into the cumilative file content
            foreach (KeyValuePair<string,string> CodeFilePair in CodeCollection)
            {
                FileContents += CodeFilePair.Key + "\n\n" + CodeFilePair.Value + "\n";
            }
            System.IO.File.WriteAllText(FilePath,FileContents);//Write it to the fucking file
        }

        static Dictionary<string,string> GetCodeCollection(string DirectoryPath)
        {
            Dictionary<string, string> FileCodePairs = new Dictionary<string, string> { };//Stores the file path along with the files contents
            TraverseDirectoryFolders(DirectoryPath,DirectoryPath,FileCodePairs);
            return FileCodePairs;
        }

        static void TraverseDirectoryFolders(string DirectoryPath,string SourcePath, Dictionary<string, string> FileCodePairs)
        {
            //Traverse Every Child Directory Where the Path doesnt contain an excluded path
            foreach (string ChildPaths in System.IO.Directory.GetDirectories(DirectoryPath).Where(x=>PathExceptions.Where(y=>x.Contains(y)).Count()==0))
            {
                TraverseDirectoryFolders(ChildPaths, SourcePath, FileCodePairs);
            }
            TraverseDirectoryFiles(DirectoryPath, SourcePath, FileCodePairs);
        }

        static void TraverseDirectoryFiles(string DirectoryPath, string SourcePath, Dictionary<string, string> FileCodePairs)
        {
            //Add every file in the directory where the file extension matches a valid file extension
            foreach (string FilePath in System.IO.Directory.GetFiles(DirectoryPath).Where(x=>FileExt.Where(y=>x.EndsWith(y)).Count()!=0))
            {
                //This improves the dumped files beauty, and reduces space used if pasted into word
                FileCodePairs.Add(FilePath.Remove(0,SourcePath.Length+1).Replace("\\","/"), System.IO.File.ReadAllText(FilePath).Replace("\r\n","\r"));
            }
        }
    }
}

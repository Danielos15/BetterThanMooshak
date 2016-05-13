using System.Configuration;
using System.IO;

namespace BetterThanMooshak.Utilities
{
    public class FileHandler
    {

        public string GetFileContentByUserAndProblem(string userId, int problemId, string filename)
        {
            var baseFolder = ConfigurationManager.AppSettings.Get("baseDir");
            var workingFolder = baseFolder + userId + "\\" + problemId + "\\";
            string content = "";
            if (Directory.Exists(workingFolder)) {
                if (File.Exists(workingFolder + filename))
                {
                    content = File.ReadAllText(workingFolder + filename);
                }
            }
            return content;
        }
    }
}
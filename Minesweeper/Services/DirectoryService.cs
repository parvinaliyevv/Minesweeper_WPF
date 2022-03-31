using System.IO;

namespace Minesweeper.Services
{
    public static class DirectoryService
    {
        public static string GetProjectParentFolder()
        {
            var parentDirectory = Directory.GetCurrentDirectory().Split("\\bin")[0] + '\\';

            return parentDirectory;
        }
    }
}

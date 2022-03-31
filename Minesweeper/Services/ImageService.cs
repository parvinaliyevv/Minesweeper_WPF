using System.Windows.Media;

namespace Minesweeper.Services
{
    public static class ImageService
    {
        private static ImageSourceConverter Converter { get; } = new();


        public static ImageSource? GetImageFromFullPath(string path)
        {
            var image = Converter.ConvertFromString(path) as ImageSource;

            return image;
        }

        public static ImageSource? GetImageFromThisPath(string path)
        {
            var image = Converter.ConvertFromString(string.Format("{0}{1}", DirectoryService.GetProjectParentFolder(),  path)) as ImageSource;

            return image;
        }
    }
}

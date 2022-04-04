using System.Windows.Media;

namespace Minesweeper.Services
{
    public static class ColorService
    {
        private static BrushConverter Converter { get; } = new();


        public static SolidColorBrush? GetColorFromHexCode(string hexCode)
        {
            var color = Converter.ConvertFromString(hexCode) as SolidColorBrush;

            return color;
        }
    }
}

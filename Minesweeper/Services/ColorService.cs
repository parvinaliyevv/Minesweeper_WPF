using System.Collections.Generic;
using System.Windows.Media;

namespace Minesweeper.Services
{
    public static class ColorService
    {
        private static BrushConverter Converter { get; } = new();

        private static Dictionary<int, SolidColorBrush> Colors { get; } = new();


        static ColorService()
        {
            Colors.Add(1, Brushes.Blue);
            Colors.Add(2, Brushes.Green);
            Colors.Add(3, Brushes.Red);
            Colors.Add(4, Brushes.Indigo);
            Colors.Add(5, Brushes.Brown);
            Colors.Add(6, Brushes.Tomato);
            Colors.Add(7, Brushes.Chocolate);
        }


        public static SolidColorBrush? GetColorFromString(string hexcode)
        {
            var color = Converter.ConvertFromString(hexcode) as SolidColorBrush;

            return color;
        }

        public static SolidColorBrush? GetNumericColor(int number)
        {
            if (Colors.ContainsKey(number)) return Colors.GetValueOrDefault(number);
            else return Brushes.Red;
        }
    }
}

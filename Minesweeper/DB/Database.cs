using System.Windows.Media;
using System.Collections.Generic;
using Minesweeper.Models;

namespace Minesweeper.DB
{
    public static class Database
    {
        private static Dictionary<int, SolidColorBrush> NumericColors { get; set; }

        private static Dictionary<GameDifficulty, GameTime> BestTimes { get; set; }

        private static Dictionary<GameDifficulty, Models.Matrix> MatrixTemplates { get; set; }


        static Database()
        {
            NumericColors = new(7);
            BestTimes = new(4);
            MatrixTemplates = new(4);

            NumericColors.Add(1, Brushes.Blue);
            NumericColors.Add(2, Brushes.Green);
            NumericColors.Add(3, Brushes.Red);
            NumericColors.Add(4, Brushes.Indigo);
            NumericColors.Add(5, Brushes.Brown);
            NumericColors.Add(6, Brushes.Tomato);
            NumericColors.Add(7, Brushes.Chocolate);

            BestTimes.Add(GameDifficulty.Easy, new GameTime());
            BestTimes.Add(GameDifficulty.Medium, new GameTime());
            BestTimes.Add(GameDifficulty.Hard, new GameTime());
            BestTimes.Add(GameDifficulty.Custom, new GameTime());

            MatrixTemplates.Add(GameDifficulty.Easy, new Models.Matrix(8, 10, 10));
            MatrixTemplates.Add(GameDifficulty.Medium, new Models.Matrix(14, 18, 40));
            MatrixTemplates.Add(GameDifficulty.Hard, new Models.Matrix(20, 24, 100));
            MatrixTemplates.Add(GameDifficulty.Custom, new Models.Matrix(20, 24, 100));
        }


        public static SolidColorBrush? GetNumericColor(int number)
        {
            if (NumericColors.ContainsKey(number)) return NumericColors.GetValueOrDefault(number);

            return Brushes.Red;
        }

        public static GameTime GetBestTime(GameDifficulty gameDifficulty)
        {
            var bestScore = BestTimes.GetValueOrDefault(gameDifficulty);

            return bestScore; 
        }

        public static Models.Matrix GetMatrixTemplate(GameDifficulty gameDifficulty)
        {
            var matrixTemplate = MatrixTemplates.GetValueOrDefault(gameDifficulty);

            return matrixTemplate;
        }
    }
}

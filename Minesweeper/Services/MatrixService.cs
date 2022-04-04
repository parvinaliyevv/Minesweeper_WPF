using System.Collections.Generic;
using System.Windows.Media;
using Minesweeper.Models;

namespace Minesweeper.Services
{
    public static class MatrixService
    {
        public static void FillMatrix(Models.Matrix matrix)
        {
            matrix.Cells.Clear();
            
            SolidColorBrush? evenColor = ColorService.GetColorFromHexCode("#BAE86C"), oddColor = ColorService.GetColorFromHexCode("#A2D149");

            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    matrix.Cells.Add(new MatrixDetails(i, j, (i + j) % 2 == 0 ? evenColor : oddColor));
                }
            }
        }

        public static void SpawnMines(Models.Matrix matrix, int index)
        {
            var exceptions = new List<int>();
            byte tryCount = default;

            // Exceptions
            for (int i = 0, result = default, startIndex = index - matrix.Columns - 1; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    result = startIndex + (i * matrix.Columns) + j;

                    if (result < 0 || result >= matrix.Rows * matrix.Columns) continue;
                    else exceptions.Add(result);
                }
            }

            // Spawn mines
            for (int i = 0, result = default; i < matrix.MineCount; i++)
            {
                result = RandomService.GetRandomNumber(1, max: matrix.Rows * matrix.Columns);

                if (result < 0 || result >= matrix.Rows * matrix.Columns || exceptions.Contains(result)) { tryCount++; i--; }
                else if (result >= index - matrix.Columns && result <= index + matrix.Columns || matrix.Cells[result].Mode == MatrixCellMode.BombCell) { tryCount++; i--; }
                else { matrix.Cells[result].Mode = MatrixCellMode.BombCell; tryCount = default; }

                if (tryCount > 30)
                {
                    foreach (var item in matrix.Cells)
                    {
                        if (i == matrix.MineCount - 1) break;
                        else if (item.Mode == MatrixCellMode.EmptyCell && (item.Row * matrix.Columns) + item.Column != index)
                        {
                            item.Mode = MatrixCellMode.BombCell;
                            i++;
                        }
                    }
                }
            }

            CalculateMines(matrix);
        }

        public static void CalculateMines(Models.Matrix matrix)
        {
            for (int i = 0, startIndex = default, result = default; i < matrix.Rows * matrix.Columns; i++)
            {
                if (matrix.Cells[i].Mode == MatrixCellMode.BombCell || matrix.Cells[i].MineCount != -1) continue;

                startIndex = i - matrix.Columns - 1;
                matrix.Cells[i].MineCount = 0;

                for (int n = 0; n < 3; n++)
                {
                    for (int m = 0; m < 3; m++)
                    {
                        result = startIndex + (n * matrix.Columns) + m;

                        if (result == i || result < 0 || result >= matrix.Rows * matrix.Columns) continue;
                        else if (result / matrix.Columns != (result - m + 1) / matrix.Columns || result / matrix.Columns != (result - m + 1) / matrix.Columns) continue;

                        if (matrix.Cells[result].Mode == MatrixCellMode.BombCell) matrix.Cells[i].MineCount++;
                    }
                }
            }
        }

        public static bool IsGameWon(Models.Matrix matrix)
        {
            foreach (var item in matrix.Cells)
                if (item.Mode == MatrixCellMode.EmptyCell && item.IsEnable) return false;

            return true;
        }
    }
}

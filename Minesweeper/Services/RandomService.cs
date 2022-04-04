using System;

namespace Minesweeper.Services
{
    public static class RandomService
    {
        private static Random Random { get; set; } = new();


        public static void SetSeed(int seed) => Random = new(seed);

        public static int GetRandomNumber(int min = 0, int max = int.MaxValue) => Random.Next(min, max);
    }
}

namespace Minesweeper.Models
{
    public class GameDetails
    {
        public GameDifficulty Difficulty { get; set; }

        public GameTime Time { get; set; }

        public bool SoundOff { get; set; }

        public bool MinesInitialized { get; set; }


        public GameDetails()
        {
            Difficulty = GameDifficulty.Medium;
            Time = new GameTime();

            SoundOff = false;
            MinesInitialized = false;
        }
    }
}

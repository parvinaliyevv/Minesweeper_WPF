using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class OverViewModel
    {
        public const string GameWonTitleContent = "You are won!";
        public const string GameOverTitleContent = "You are lose!";

        public const string GameWonButtonContent = "Play Again!";
        public const string GameOverButtonContent = "Try Again!";

        public string TitleContent { get; set; }
        public string ButtonContent { get; set; }

        public GameTime GameTime { get; set; }
        public GameTime BestTime { get; set; }


        public OverViewModel(GameTime gameTime = null, GameTime bestTime = null)
        {
            if (gameTime is null)
            {
                GameTime = new();
                GameTime.ViewTime = "---";

                BestTime = new();
                BestTime.ViewTime = "---";

                TitleContent = GameOverTitleContent;
                ButtonContent = GameOverButtonContent;
            }
            else
            {
                GameTime = gameTime;
                BestTime = bestTime;

                TitleContent = GameWonTitleContent;
                ButtonContent = GameWonButtonContent;
            }
        }
    }
}

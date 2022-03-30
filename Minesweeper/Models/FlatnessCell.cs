namespace Minesweeper.Models
{
    public class FlatnessCell
    {
        public FlatnessCellMode Mode { get; set; }

        public int BombCount { get; set; }

        public bool HasFlag { get; set; }
        

        public FlatnessCell()
        {
            Mode = FlatnessCellMode.EmptyCell; 
            BombCount = -1;
            HasFlag = false;
        }
    }
}

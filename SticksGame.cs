using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_sticks
{
    class SticksGame
    {
        public readonly Random randomizer;
        public int InitialSticksNumber { get; }
        public Player Turn { get; private set; }
        public int RemainingSticks { get; private set; }
        public GameStatus GameStatus { get; private set; }

        public event Action<int> MachinePlayed;
        public event EventHandler<int> HumanTurnToMakeMove;
        public event Action<Player> EndOfGame;
        
        public SticksGame(int initialSticksNumber, Player whoMakesFirstMove)
        {
            if (initialSticksNumber<7 || initialSticksNumber> 30){
                throw new ArgumentException("Initial number of sticks should be >7 and <30");
            }

            randomizer = new Random();
            GameStatus = GameStatus.NotStarted;
            InitialSticksNumber = initialSticksNumber;
            RemainingSticks = InitialSticksNumber;
            Turn = whoMakesFirstMove;
        }

        public void Start()
        {
            if (GameStatus == GameStatus.GameIsOver)
            {
                RemainingSticks = InitialSticksNumber;
            }

            if (GameStatus == GameStatus.InProgress)
            {
                throw new InvalidOperationException("Can`t call Start while the game is Inprogress");
            }
            GameStatus = GameStatus.InProgress;
            while (GameStatus == GameStatus.InProgress)
            {
                if(Turn == Player.Computer)
                {
                    CompMakesMove();
                }
                else
                {

                    HumanMakesMove();
                }
                FireEndOfGameIfRequired();

                Turn = Turn == Player.Computer ? Player.Human : Player.Computer;
            }
        }

        private void FireEndOfGameIfRequired()
        {
            if (RemainingSticks == 0)
            {
                GameStatus = GameStatus.GameIsOver;
                if (EndOfGame != null)
                {
                    EndOfGame(Turn = Turn == Player.Computer ? Player.Human : Player.Computer);
                }
            }
        }

        private void HumanMakesMove()
        {
            if (HumanTurnToMakeMove != null)
                HumanTurnToMakeMove(this, RemainingSticks);
        }

        public void HumanTakes(int sticks)
        {
            if(sticks<1 || sticks > 3)
            {
                throw new ArgumentException("You can take from 1 to 3 ");
            }
            if (sticks > RemainingSticks)
            {
                throw new ArgumentException($"You can`t take more then {RemainingSticks}");
            }
            TakeSticks(sticks);
        }

        private void CompMakesMove()
        {
            int maxNumber = RemainingSticks >= 3 ? 3 : RemainingSticks;
            int sticks = randomizer.Next(1, maxNumber);

            TakeSticks(sticks);
            if (MachinePlayed != null)
                MachinePlayed(sticks);
        }

        private void TakeSticks(int sticks)
        {
            RemainingSticks -= sticks;
        }
    }
}

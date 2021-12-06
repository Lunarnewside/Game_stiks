using System;

namespace Game_sticks
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new SticksGame(29, Player.Computer);
            game.MachinePlayed += Game_MachinePlayed;
            game.HumanTurnToMakeMove += Game_HumanTurnToMakeMove;
            game.EndOfGame += Game_EndOfGame;

            game.Start();
        }

        private static void Game_EndOfGame(Player player)
        {
            Console.WriteLine($"Winner:{player}");
        }

        private static void Game_HumanTurnToMakeMove(object sender, int remainingSticks)
        {
            Console.WriteLine($"RemainingSticks:{remainingSticks}");
            Console.WriteLine("Take some sticks");

            bool takenCorrectly = false;
            while(takenCorrectly!=true)
                if(int.TryParse(Console.ReadLine(),out int takenSticks)){
                    var game = (SticksGame)sender;

                    try
                    {
                        game.HumanTakes(takenSticks);
                        takenCorrectly = true;
                    }
                    catch(ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
        }

        private static void Game_MachinePlayed(int sticksTaken)
        {
            Console.WriteLine($"Machine took: {sticksTaken}");
        }
    }
}

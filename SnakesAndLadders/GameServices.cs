using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakesAndLadders
{
    public class GameServices
    {
        Player currentPlayer; // current player rolling dice
        Player[] playerQueue; // helps keep track of who's next to roll
        const int totalPlayers = 2;
        const int boradSize = 100;
        Cell[] board;


        public GameServices(int s, int l) // contructor with 2 int params(number of snakes and ladders passed in from user input)
        {
            board = CreateBoard(boradSize, s, l);
            playerQueue = CreatePlayers(totalPlayers);
        }

        // Board setup
        // Creates a board according to size and randomly places snakes, ladders and golden cells on board
        private Cell[] CreateBoard(int size, int snakes, int ladders)
        {
            Cell[] board = new Cell[size];

            // fill board with cell numbers
            for(int i = 0; i < size; i++)
            {
                var c = new Cell();
                c.CellNumber = i + 1;
                board[i] = c;
            }


            // Position Snakes, Ladders and Golden cell on board
            
            // Snakes
            for (var i = 0; i < snakes; i++)
            {
                Random rand = new Random();
                int randomSnakeCell = rand.Next(30, size - 4); // snake head between 30 and 96
                int snakeTailCell = rand.Next(6, randomSnakeCell - 10); // snake tail between 6 and 84
                                                                        // -10 to ensure snake tail is not on same line as snake head

                // places snake on board
                var penalty = new Snake();
                penalty.CellNumber = randomSnakeCell;
                penalty.TailCell = snakeTailCell;
                board[randomSnakeCell - 1] = penalty;

            }

            // Ladders
            for (int i = 0; i < ladders; i++)
            {
                Random rand = new Random();

                int topOfLadderCell = rand.Next(30, size - 1); // top of ladder between 30 and 96
                int randomLadderCell = rand.Next(1, topOfLadderCell - 10); // start of ladder between 2 and 89

                // places ladder on board
                var reward = new Ladder();
                reward.CellNumber = randomLadderCell;
                reward.TopCell = topOfLadderCell;
                board[randomLadderCell - 1] = reward;
            }

            // Golden cells
            for (int i = 0; i < 2; i++)
            {
                Random rand = new Random();
                int randomGoldCell = rand.Next(30, 75);

                var gold = new GoldenCell();
                gold.CellNumber = randomGoldCell;
                board[randomGoldCell - 1] = gold;
            }

            return board;

        } // end CreateBoard

        
        // creates players array to access players by number(1 or 2) and adjust current positions
        private Player[] CreatePlayers(int amountOfplayers) 
        {
           var players = new Player[amountOfplayers]; // create players array

            // set each players number and gives them their current position
            for (int i = 0; i < amountOfplayers; i++)
            {
                players[i] = new Player();
                players[i].CurrentPlayerPosition = 0;
                players[i].PlayerNumber = i + 1;
            }

            return players; // returns players array with all players current position and player number

        } // end CreatePlayers

        private int RollDice()
        {
            var sum = 0;
            int dice1, dice2;

            Random rand = new Random();

            dice1 = rand.Next(1, 6);
            Thread.Sleep(1);
            dice2 = rand.Next(1, 6);

            sum = dice1 + dice2;

            return sum;
        }


        // ensures next player rolls and print each players postion after each round 
        private void NextPlayerRoll()
        {
            if(currentPlayer.PlayerNumber < totalPlayers)
            {
                currentPlayer = playerQueue[(currentPlayer.PlayerNumber - 1) + 1]; // sets current player to next player(in this case to player 2)
            }
            else // round is over,all players have rolled and displays each players current position
            {
                Console.WriteLine($"Player 1 location is {playerQueue[0].CurrentPlayerPosition}\nPlayer 2 location is {playerQueue[1].CurrentPlayerPosition}\n");
                currentPlayer = playerQueue[0];
            }
        }


        // adjust players positions accordingly after roll
        private void MovePlayer(int rolledNumber)
        {
            Console.WriteLine($"Player {currentPlayer.PlayerNumber} you rolled {rolledNumber}.");
            int moveTo = currentPlayer.CurrentPlayerPosition;

            if ((moveTo + rolledNumber) < board.Length)
            {
                moveTo += rolledNumber; // new location on board to be moved to
            }
            else
            {
                moveTo = 100; // if above condition is not met, it means player landed on block 100 or greater, Hense we have a winner
            }

            // CHECKS IF PLAYER LANDED ON SNAKE, LADDER OR GOLDEN CELL

            // Checks if player landed on snake. If yes, moves player to snake tail
            if (board[moveTo - 1].GetType() == typeof(Snake))
            {
                //var s = new Snake();
                //moveTo = s.TailCell;
                moveTo = (board[moveTo - 1] as Snake).TailCell; //??
                Console.WriteLine("Player " + currentPlayer.PlayerNumber + " You ladded on a SNAKE!! Moving to " + moveTo);
            }

            // Checks if player landed on ladder. If yes, moves current player to top of ladder
            if (board[moveTo - 1].GetType() == typeof(Ladder))
            {
                moveTo = (board[moveTo - 1] as Ladder).TopCell; //??
                Console.WriteLine("Player " + currentPlayer.PlayerNumber + " You have a LADDER!! Moving to " + moveTo);
            }

            // checks if current player landed on gold cell and if other player is leading
            if(board[moveTo - 1].GetType() == typeof(GoldenCell))
            {
                // checks if player 1 position is less than players 2 and if yes, swap positions
                if(currentPlayer.PlayerNumber == 1 && currentPlayer.CurrentPlayerPosition < playerQueue[1].CurrentPlayerPosition)
                {
                    var leadingCell = playerQueue[1].CurrentPlayerPosition;
                    playerQueue[1].CurrentPlayerPosition = currentPlayer.CurrentPlayerPosition;
                    playerQueue[0].CurrentPlayerPosition = leadingCell;

                    Console.WriteLine("GOLDEN BLOCK :) NEW LEADER! Player " + currentPlayer.PlayerNumber + " you have been moved to block " + leadingCell);
                    Console.WriteLine("Sorry Player " + playerQueue[1].PlayerNumber + " you've dropped to block " + playerQueue[1].CurrentPlayerPosition);

                    moveTo = playerQueue[0].CurrentPlayerPosition;

                }

                //checks if player 2 position is less than players 1 and if yes, swap positions
                if (currentPlayer.PlayerNumber == 2 && currentPlayer.CurrentPlayerPosition < playerQueue[0].CurrentPlayerPosition)
                {
                    var leadingCell = playerQueue[0].CurrentPlayerPosition;
                    playerQueue[0].CurrentPlayerPosition = currentPlayer.CurrentPlayerPosition;
                    playerQueue[1].CurrentPlayerPosition = leadingCell;

                    Console.WriteLine("NEW LEADER!! Player " + currentPlayer.PlayerNumber + " you have been moved to block " + leadingCell);
                    Console.WriteLine("Sorry Player " + playerQueue[0].PlayerNumber + " you've dropped to block " + playerQueue[0].CurrentPlayerPosition);

                    moveTo = playerQueue[1].CurrentPlayerPosition;

                }
            }

            currentPlayer.CurrentPlayerPosition = moveTo;

        } // end MovePlayer


        public void Play()
        {
            currentPlayer = playerQueue[0];
            bool FirstPlayerRoll = true;

            while(currentPlayer.CurrentPlayerPosition != board.Length)
            {
                if(!FirstPlayerRoll)
                {
                    NextPlayerRoll();
                }
                FirstPlayerRoll = false;
                MovePlayer(RollDice());
            }
            Console.WriteLine($"\nPlayer {currentPlayer.PlayerNumber} location {currentPlayer.CurrentPlayerPosition}");
            Console.WriteLine($"PLAYER {currentPlayer.PlayerNumber} WINS!!!!");
            Console.WriteLine("GAME OVER!!!");
        }

    }
}

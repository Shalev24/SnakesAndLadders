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
        Player currentPlayer;
        Player[] playerQueue; //array of Player ojects called playerQ ?? 
        const int totalPlayers = 2;
        const int BoardSize = 100;
        Cell[] board; // is this here so that all methods can have access to the same board and manipulate array respectivaly ??


        public GameServices()
        {
            //totalPlayers = NumberOfPlayers;
            board = CreateBoard(BoardSize);
            playerQueue = CreatePlayers(totalPlayers);
        }


        private Cell[] CreateBoard(int size)
        {
            //?? why here and also in line 16
            Cell[] board = new Cell[size];

            for(int i = 0; i < size; i++)
            {
                var c = new Cell();
                c.CellNumber = i + 1;
                board[i] = c;
            }


            // Position Snakes and Ladders

            // Snakes

            Random rand = new Random();
            int randomSnakeCell = rand.Next(92, 97);
            int snakeTailCell = rand.Next(21, 30);

            var s = new Snake();
            s.CellNumber = randomSnakeCell;
            s.TailCell = snakeTailCell;
            board[randomSnakeCell - 1] = s;


            // Ladders
            int topOfLadderCell = rand.Next(85, 90);
            int randomLadderCell = rand.Next(12, 20);

            var l = new Ladder();
            l.CellNumber = randomLadderCell; // start of ladder cell
            l.TopCell = topOfLadderCell;
            board[randomLadderCell - 1] = l;

            //Golden Cell
            int randomGoldCell = rand.Next(60, 65);

            var gold = new GoldenCell();
            gold.CellNumber = randomGoldCell;
            board[randomGoldCell - 1] = gold;

            return board;
        }

        private Player[] CreatePlayers(int amountOfplayers) // does this mean I'm returning an array of type players object ? 
        {
           var players = new Player[amountOfplayers]; // ?? what is this initilization 

            for(int i = 0; i < amountOfplayers; i++)
            {
                players[i] = new Player();
                players[i].CurrentPlayerPosition = 0;
                players[i].PlayerNumber = i + 1;
            }

            return players;
        }

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


        private void NextPlayerRoll()
        {
            if(currentPlayer.PlayerNumber < totalPlayers)
            {
                currentPlayer = playerQueue[(currentPlayer.PlayerNumber - 1) + 1];
            }
            else
            {
                Console.WriteLine($"Player 1 location is {playerQueue[0].CurrentPlayerPosition}\nPlayer 2 location is {playerQueue[1].CurrentPlayerPosition}\n");
                currentPlayer = playerQueue[0];
            }
        }


        private void MovePlayer(int rolledNumber)
        {
            Console.WriteLine($"Player {currentPlayer.PlayerNumber} you rolled {rolledNumber}.");
            int moveTo = currentPlayer.CurrentPlayerPosition;

            if ((moveTo + rolledNumber) <= board.Length)
            {
                moveTo += rolledNumber;
            }
            else
            {
                moveTo = 100;
            }

            //only if a player landed on a ladder, snake or golden cell enter loop otherwise no need to check conditions
            while (board[moveTo - 1].GetType() == typeof(GoldenCell) || board[moveTo - 1].GetType() == typeof(Ladder) || board[moveTo - 1].GetType() == typeof(Snake))
            {
                // Checks if player ladded on snake and moves current player accordingly
                if (board[moveTo - 1].GetType() == typeof(Snake))
                {
                    moveTo = (board[moveTo - 1] as Snake).TailCell;
                    Console.WriteLine("Player " + currentPlayer.PlayerNumber + " You ladded on a SNAKE!! Moving to " + moveTo);
                }

                // Checks if player ladded on ladder and moves current player accordingly
                if (board[moveTo - 1].GetType() == typeof(Ladder))
                {
                    moveTo = (board[moveTo - 1] as Ladder).TopCell;
                    Console.WriteLine("Player " + currentPlayer.PlayerNumber + " You have a LADDER!! Moving to " + moveTo);
                }

                // checkes if  player landed on Gold cell and if other player is leading
                if(board[moveTo - 1].GetType() == typeof(GoldenCell))
                {
                    if(currentPlayer.PlayerNumber == 1 && currentPlayer.CurrentPlayerPosition < playerQueue[1].CurrentPlayerPosition)
                    {
                        var leadingCell = playerQueue[1].CurrentPlayerPosition;
                        playerQueue[1].CurrentPlayerPosition = currentPlayer.CurrentPlayerPosition;
                        playerQueue[0].CurrentPlayerPosition = leadingCell;

                        Console.WriteLine("GOLDEN BLOCK :) NEW LEADER! Player " + currentPlayer.PlayerNumber + " you have been moved to block " + leadingCell);
                        Console.WriteLine("Sorry Player " + playerQueue[1].PlayerNumber + " you've dropped to block " + playerQueue[1].CurrentPlayerPosition);

                        moveTo = playerQueue[0].CurrentPlayerPosition;

                    }
                    else if (currentPlayer.PlayerNumber == 2 && currentPlayer.CurrentPlayerPosition < playerQueue[0].CurrentPlayerPosition)
                    {
                        var leadingCell = playerQueue[0].CurrentPlayerPosition;
                        playerQueue[0].CurrentPlayerPosition = currentPlayer.CurrentPlayerPosition;
                        playerQueue[1].CurrentPlayerPosition = leadingCell;

                        Console.WriteLine("NEW LEADER!! Player " + currentPlayer.PlayerNumber + " you have been moved to block " + leadingCell);
                        Console.WriteLine("Sorry Player " + playerQueue[0].PlayerNumber + " you've dropped to block " + playerQueue[0].CurrentPlayerPosition);

                        moveTo = playerQueue[1].CurrentPlayerPosition;

                    }
                }
            } // end while loop

            currentPlayer.CurrentPlayerPosition = moveTo;
        }


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

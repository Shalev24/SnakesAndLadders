using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakesAndLadders
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter desired number of snakes: ");
            int snakes = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Please enter desired number of ladders: ");
            int ladders = Int32.Parse(Console.ReadLine());

            var game = new GameServices(snakes, ladders);

            game.Play();
        }
    }
}

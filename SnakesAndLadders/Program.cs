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
            var game = new GameServices();

            game.Play();
        }
    }
}

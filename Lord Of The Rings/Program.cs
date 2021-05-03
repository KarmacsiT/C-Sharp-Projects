using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lord_Of_The_Rings
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MiddleEarth.PathFinder(MiddleEarth.map, 0, 8); //Finds the Shortest Path for our Heroes
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("The Shortest Path [Minimizing Risk / Number of Orcs encountered]:" + Environment.NewLine);
                MiddleEarth.ShortestPathOutput(MiddleEarth.shortestpath, MiddleEarth.Territories);
                Location.UserInteraction(); //Asks a Territory Name from the User to check whether or not it is in Sauron's Hand
            }

            catch (IndexOutOfRangeException)
            {
                bool FormatFailSafe = MiddleEarth.IsExcecuted(MiddleEarth.map); //Helps selecting the appropriate message for Exception by checking whether the destination is reachable or not

                if (FormatFailSafe == false)
                {
                    Console.WriteLine("The given graph has no route to destination, please try again.");
                }
                
                else
                {
                    Console.WriteLine("The inputed territory does not exist or the input is invalid.");
                }
            }


            finally
            {
                Console.ReadLine();
            }

        }
    }
}

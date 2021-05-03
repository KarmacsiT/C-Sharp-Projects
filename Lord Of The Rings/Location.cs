using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lord_Of_The_Rings
{
    class Location
    {
        bool isrightpath;
        int distancefromstart;
        int parent;

        public bool IsRightPath { get { return isrightpath; } set { isrightpath = value; } }
        public int DistanceFromStart { get { return distancefromstart; } set { distancefromstart = value; } }
        public int Parent { get { return parent; } set { parent = value; } }

        public Location(bool isrightpath, int distancefromstart, int parent)
        {
            this.isrightpath = isrightpath;
            this.distancefromstart = distancefromstart;
            this.parent = parent;
        }
        public Location()
        {

        }

        public static void UserInteraction() //Asks a TerritoryName from the User to check whether or not it is under Sauron's control
        {

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please input a territory name to check whether the location is under Sauron's control:" + Environment.NewLine);
            Console.WriteLine("You can choose from the following options:");

            for (int i = 0; i < MiddleEarth.Territories.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"~ {MiddleEarth.Territories[i]}");
            }

            Console.WriteLine(Environment.NewLine);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            string input = Console.ReadLine();
            Console.WriteLine(Environment.NewLine);

            int index = Array.IndexOf(MiddleEarth.Territories, input); //Finds the Index of the User Input

            if (IsInSauronHand(index) == true) //Inputed Territory in Sauron's Hand
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Territory is under Sauron's control.");
            }

            else if (IsInSauronHand(index) == false) //Inputed Territory is free
            {
                Console.ForegroundColor = ConsoleColor.Green;

                int[] ConnectedEdges = new int[MiddleEarth.map.GetLength(0)];

                Console.WriteLine("Territory is currently free." + Environment.NewLine);
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"Neighbours of {input} are:" + Environment.NewLine);

                for (int i = 0; i < MiddleEarth.map.GetLength(0); i++)
                {
                    ConnectedEdges[i] = MiddleEarth.map[index, i]; //Collects to connections of the indexed Territory
                }

                for (int i = 0; i < ConnectedEdges.Length; i++)
                {
                    if (ConnectedEdges[i] != 0) //Leaves out not connected Terrytories
                    {
                        Console.WriteLine($"- {MiddleEarth.Territories[Array.IndexOf(ConnectedEdges, ConnectedEdges[i])]}" + Environment.NewLine); //Prints out connected Territories to the Input Territory which are considered "Neighbours"

                        ConnectedEdges[i] = 0; //Nullifies the value of already printed Territories to help Array.IndexOf work correctly 
                                               //Eg.: In case of two elements having the same value, then Array.IndexOf will take the index of the first such element.
                    }

                }

            }
        }
        static bool IsInSauronHand(int TerritoryIndex) //Determines whether a Territory is in Sauron's Hand or not
        {
            bool InSauronHand = true;
            int AllPath = 0;
            int FreePath = 0;

            for (int i = 0; i < MiddleEarth.map.GetLength(0); i++)
            {
                if (MiddleEarth.map[TerritoryIndex, i] != 0)
                {
                    AllPath++;

                    if (MiddleEarth.map[TerritoryIndex, i] == 1)
                    {
                        FreePath++;
                    }
                }

            }
            if (FreePath >= AllPath / 2) //Teritory is in Sauron's Hand if more than half of all paths are being patroled by Orcs
            {
                InSauronHand = false;
            }

            return InSauronHand;

        }

    }
}

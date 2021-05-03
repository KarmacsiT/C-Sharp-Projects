using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lord_Of_The_Rings
{

    class MiddleEarth
    {
        public static Random rng = new Random();
        public static List<int> shortestpath = new List<int>();
        public static string[] Territories = new string[] { "Hobbiton (County)", "Minhiriath", "Rhudaur", "Enedwaith", "Lorien", "Gondor", "Rohan", "South Gondor", "Mordor" };

        static int[,] CreateGraph(int height, int width) //Creates Graph using only a given width and height, relly ONLY on a given LinkedList that generates Patrols on our paths (here OrcLinkedList) => Loosly Coupled
        {
            int[,] graph = new int[height, width];

            for (int i = 0; i < graph.GetLength(0); i++) //Initializing Array values with a starting value -1 for Fail Safe purposes
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    graph[i, j] = -1;
                }
            }

            int PatrolIndex = 0;
            int dice = 0;

            int[] OrcPatrol = GuardSpawn<int>.GeneratePatrol(height, width); //Spawns Orc Patrols, determines the number of Orcs on Sauron controlled paths

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[i, j] == -1)
                    {
                        dice = rng.Next(0, 11);

                        if (dice <= 7) //70% chance to create a connection beetwen two nodes
                        {
                            if (graph[j, i] != 0 && i != j)
                            {
                                graph[i, j] = OrcPatrol[PatrolIndex];
                                graph[j, i] = graph[i, j]; //Connecting Edges
                                PatrolIndex++;
                            }

                            else if (graph[j, i] == 0 || i == j) // //Coded in Bottom-Up Approach which means there isn't a double chance for Territories to have conncection beetween them & Eliminates Circles in Graph which wouldnt make sense in our application
                                                                 //Eg.: [0,1] has a chance to be connected to [1,0] BUT [1,0] has no chance to be connected to [0,1] IF [0,1] is already 0
                            {
                                graph[i, j] = 0;
                            }

                        }
                        else //30% chance that there is no path between two Location
                        {
                            graph[i, j] = 0;
                        }

                    }
                }
            }

            for (int i = 0; i < 3; i++) //Making sure that our heroes can't reach their destination and the two nodes before it from the first three territories
            {
                for (int j = 1; j < 4; j++) //Nullifying potential connections between the first three and the last three nodes
                {
                    if (graph[i, graph.GetLength(0) - j] != 0) //Because Array.GetLength(0) would get 9 but we have only 8 elements
                    {
                        graph[i, graph.GetLength(0) - j] = 0;
                        graph[graph.GetLength(0) - j, i] = 0;

                    }
                }

            }

            return graph;
        }

        public static int[,] map = CreateGraph(9, 9);

        public static bool IsExcecuted(int[,] graph) //Helps selecting the appropriate message for Exception by checking whether the destination is reachable or not
        {
            bool FormatFailSafe = true;
            bool StartingColumnHasValue = true;
            bool DestinationColumnHasValue = true;
            bool ConnectionBeetweenStartandDestination = false;
            int counter = 0;

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                counter = 0;

                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    if (graph[i, j] == 0)
                    {
                        counter++;
                    }

                    if (i == 0 && counter == graph.GetLength(0))
                    {
                        StartingColumnHasValue = false;
                    }

                    if (i == graph.GetLength(0) - 1 && counter == graph.GetLength(0))
                    {
                        DestinationColumnHasValue = false;
                    }

                    if (i > 0 && i < graph.GetLength(0) - 1 && counter >= 2)
                    {
                        ConnectionBeetweenStartandDestination = true;
                    }

                }
            }

            if (StartingColumnHasValue == false || DestinationColumnHasValue == false || ConnectionBeetweenStartandDestination == false) //The easisest connection beetwen start and finish is an edge going out from start connecting to a random vertice and from their reaching its destination
                                                                                                                                         //These three conditions are the fundamental conditions to determine whether a path is possible on a graph or not
            {
                FormatFailSafe = false;
            }

            return FormatFailSafe;
        }

        //Territories:
        //1.Hobbiton (County)       0 => no connection between two nodes
        //2.Minhiriath              1 => Free Path
        //3.Rhudaur                 else => Patroled Area
        //4.Enedwaith
        //5.Lorien
        //6.Gondor
        //7.Rohan
        //8.South Gondor
        //9.Mordor

        static int nodeCount;

        public static void PathFinder(int[,] graph, int startpoint, int destination) //Finds the Shortes Path with Dijkstra's Algortihm (Works with any undirected graph [as long as it is a square matrix], any startpoint, any destination)
        {
            nodeCount = graph.GetLength(0); //It's a square matrix it doesn't matter which dimension I take to measure 

            Location[] locations = new Location[nodeCount];
            for (int i = 0; i < locations.Length; i++) //Initializing Array
            {
                locations[i] = new Location();
            }

            //Dijkstra Step 1.

            for (int i = 0; i < nodeCount; i++)
            {
                locations[i].IsRightPath = false;
                locations[i].DistanceFromStart = int.MaxValue;

            }
            locations[startpoint].DistanceFromStart = 0;
            locations[startpoint].Parent = -1; //The startpoint has no parent because it is the source

            for (int i = 0; i < nodeCount; i++)
            {
                //Dijkstra Step 2.
                int NearestLocationFromStart = minDistance(locations); //Current closest node to startpoint

                locations[NearestLocationFromStart].IsRightPath = true; //Makes currently mentioned node the part of the choosen path

                //Dijkstra Step 3.
                for (int j = 0; j < nodeCount; j++)
                {
                    if (locations[j].IsRightPath == false && graph[NearestLocationFromStart, j] != 0 && locations[NearestLocationFromStart].DistanceFromStart != int.MaxValue && locations[NearestLocationFromStart].DistanceFromStart + graph[NearestLocationFromStart, j] < locations[j].DistanceFromStart)
                    {
                        locations[j].DistanceFromStart = locations[NearestLocationFromStart].DistanceFromStart + graph[NearestLocationFromStart, j]; //If we find a path with less guards we change the previous not fixed path to current path
                        locations[j].Parent = NearestLocationFromStart; //Setting Currently Assigned Node as the parent of the upcoming node
                    }
                }
            }

            ReturnPath(locations, destination);
            shortestpath.Reverse(); //List needs to be reversed because it was evaluated from the destination to startpoint and it's needed the otherway around
        }

        static int minDistance(Location[] locations) //Checks for minimum distance to startpoint for those points that aren't already decided
        {
            int min = int.MaxValue;
            int minindex = -1; //Fail Safe

            for (int i = 0; i < nodeCount; i++)
            {
                if (locations[i].IsRightPath == false && locations[i].DistanceFromStart < min)
                {
                    min = locations[i].DistanceFromStart;
                    minindex = i;
                }
            }
            return minindex;
        }

        //Storing Dijkstra Data (Shortest Path)
        static void ReturnPath(Location[] locations, int destination)
        {
            if (locations[destination].Parent != -1) //If it is not the startpoint add to shortestpath
            {
                shortestpath.Add(destination);
                ReturnPath(locations, locations[destination].Parent); //Recursing through parent nodes until we reach the startpoint of our graph and then it breaks
            }

            else
            {
                shortestpath.Add(destination); //Adding startpoint to shortestpath
            }
        }

        public static void ShortestPathOutput(List<int> GivenShortestPath, string[] GivenLocationNameArray) //Prints the shortest path piece by piece
        {
            foreach (int item in shortestpath)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine($"{Territories[item]}" /*+ Environment.NewLine*/);

                if (Territories[item] != "Mordor")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(" ~||~");
                    Console.WriteLine(" ~||~");
                    Console.WriteLine(" ~||~");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("          _^_");
                    Console.WriteLine("         (⁰÷⁰)");
                    Console.WriteLine("         \\/ \\/-o ==[]::::::>");
                    Console.WriteLine("          | |  ");
                    Console.WriteLine("          ---");
                    Console.WriteLine("          / \\ ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine(Environment.NewLine);
                }
            }

            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("  o ==[]::::::::::::::::>   o ==[]::::::::::::::::>   o ==[]::::::::::::::::> " + Environment.NewLine);

        }

    }
}

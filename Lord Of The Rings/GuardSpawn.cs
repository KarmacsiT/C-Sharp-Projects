using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lord_Of_The_Rings
{
    class GuardSpawn<T>
    {

        static Random rng = new Random();
        private GuardItem head;
        class GuardItem
        {
            public int content; //We will only store here the number of Patrol Guards
            public GuardItem next;
        }
        public void SpawnPatrolGuards() //Generate random number of guards
        {
            int dice = rng.Next(1, 11);
            int NumberOfGuards = -1; // Fail safe

            if (dice <= 3) //30% chance
            {
                NumberOfGuards = rng.Next(5, 11);
            }

            else if (dice > 3 && dice <= 7) //40% chance
            {
                NumberOfGuards = rng.Next(50, 101);
            }

            else //30% chance [Disclaimer: NumberOfGuards = 1 means that two territory is connected and there are NO Guards on the path]
            {
                NumberOfGuards = 1;
            }

            GuardItem PatrolGroup= new GuardItem();
            PatrolGroup.content = NumberOfGuards;
            PatrolGroup.next = head;
            head = PatrolGroup;

        }
        int Count()
        {
            int count = 0;
            GuardItem h = head;
            while (h != null)
            {
                count++;
                h = h.next;
            }

            return count;
        }
        public int[] GuardListToArray()
        {
            int[] GuardArray = new int[Count()];
            GuardItem h = head;

            for (int i = 0; i < GuardArray.Length && head != null; i++)
            {
                GuardArray[i] = h.content;
                h = h.next;
            }
            return GuardArray;
        }

        public static int[] GeneratePatrol(int height, int width)
        {
            GuardSpawn<int> OrcLinkedList = new GuardSpawn<int>();
            for (int i = 0; i < height * width; i++)
            {
                OrcLinkedList.SpawnPatrolGuards(); //Generate Orc Patrols
            }
            int[] EnemyPatrol = OrcLinkedList.GuardListToArray();
            return EnemyPatrol;
        }

    }
}

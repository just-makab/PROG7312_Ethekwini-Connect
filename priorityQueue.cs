using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_Ethekwini_Connect
{
    public class PriorityQueue
    {
        private SortedList<int, Queue<EventActivity>> queue;

        public PriorityQueue()
        {
            queue = new SortedList<int, Queue<EventActivity>>(new DescendingComparer());
        }

        //Populate que based on its priority(most clicked to least clicked item)
        public void Enqueue(EventActivity activity, int priority)
        {
            if (!queue.ContainsKey(priority))
            {
                queue[priority] = new Queue<EventActivity>();
            }
            queue[priority].Enqueue(activity);
        }

        // Retrieves and removes the highest priority activity
        public EventActivity Dequeue()
        {
            if (queue.Count == 0)
                return null;

            var firstKey = queue.Keys[0];
            var activity = queue[firstKey].Dequeue();

            // Remove the queue if it's empty
            if (queue[firstKey].Count == 0)
                queue.Remove(firstKey);

            return activity;
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }

        // Sorts keys in decending order
        private class DescendingComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return y.CompareTo(x); 
            }
        }
    }
}

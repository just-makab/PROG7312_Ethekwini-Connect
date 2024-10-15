using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_Ethekwini_Connect
{
    public class EventActivity : IComparable<EventActivity>
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public int CompareTo(EventActivity other)
        {
            // Sort by Date (earliest date first)
            return this.Date.CompareTo(other.Date);
        }
    }
}

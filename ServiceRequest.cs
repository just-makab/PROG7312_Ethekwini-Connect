using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG7312_Ethekwini_Connect
{
    public class ServiceRequest
    {
        public int RequestID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public DateTime RequestedDate { get; set; }

        public override string ToString()
        {
            return $"{RequestID} - {Title} ({Status})";
        }
    }
}

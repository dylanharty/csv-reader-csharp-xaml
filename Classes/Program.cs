using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SaitProjectCSVDesktop.Classes
{
    // Define Program class to bind each row of CSV data
    public class Program
    {
        public string Title { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }

        public Program(string title, int priority, string status)
        {
            Title = title;
            Priority = priority;
            Status = status;
        }
    }
}

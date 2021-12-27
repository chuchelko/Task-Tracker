using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task_Tracker_Proj.Services.Sorting
{
    public class ProjectParameters : BaseParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ProjectParameters()
        {
        }
    }
}

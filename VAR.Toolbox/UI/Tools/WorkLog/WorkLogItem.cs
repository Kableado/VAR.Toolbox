using System;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public class WorkLogItem
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Activity { get; set; }
        public string Description { get; set; }
    }
}

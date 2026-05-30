using System;

namespace VAR.Toolbox.Code.WorkLog;

public class WorkLogItem
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public string Activity { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;

    public bool Overlaps(WorkLogItem other)
    {
        return (other.DateEnd < DateStart || other.DateStart > DateEnd) == false;
    }
}
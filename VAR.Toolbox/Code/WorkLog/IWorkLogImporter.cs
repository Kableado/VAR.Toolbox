using System.Collections.Generic;
using Avalonia.Controls;

namespace VAR.Toolbox.Code.WorkLog;

public interface IWorkLogImporter : INamed
{
    List<WorkLogItem> Import(Window window);
    bool Export(List<WorkLogItem> items, Window window);
}
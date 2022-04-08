using System.Collections.Generic;
using System.Windows.Forms;

namespace VAR.Toolbox.Code.WorkLog
{
    public interface IWorkLogImporter : INamed
    {
        List<WorkLogItem> Import(Form form);
        bool Export(List<WorkLogItem> items, Form form);
    }
}
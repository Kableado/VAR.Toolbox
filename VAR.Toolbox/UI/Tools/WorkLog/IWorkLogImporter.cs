using System.Collections.Generic;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public interface IWorkLogImporter : INamed
    {
        List<WorkLogItem> Import(Form form);
        bool Export(List<WorkLogItem> items, Form form);
    }
}

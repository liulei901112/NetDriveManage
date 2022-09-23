using System.Collections.Generic;

namespace NetDriveManage.SingleInstance
{
    public interface ISingleInstanceApp
    {
        bool SignalExternalCommandLineArgs(IList<string> args);
    }
}

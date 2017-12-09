using System;
using System.Runtime.InteropServices;

namespace VAR.Toolbox.Code
{
    public class Win32API
    {
        [DllImport("PowrProf.dll")]
        public static extern Boolean SetSuspendState(Boolean Hibernate, Boolean ForceCritical, Boolean DisableWakeEvent);

        public static uint GetLastInputTime()
        {
            uint idleTime = 0;
            User32.LASTINPUTINFO lastInputInfo = new User32.LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;
            uint envTicks = (uint)Environment.TickCount;
            if (User32.GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }
            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
    }
}
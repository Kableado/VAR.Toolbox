using System;
using System.Runtime.InteropServices;

namespace VAR.Toolbox.Code.Platform;

public static class Platform
{
    private static readonly IPlatformService _current = CreatePlatformService();

    public static IPlatformService Current => _current;

    private static IPlatformService CreatePlatformService()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new WindowsPlatformService();

        // For Linux and other platforms use LinuxPlatformService (may be stubbed)
        return new LinuxPlatformService();
    }
}


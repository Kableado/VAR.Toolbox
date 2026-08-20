using System.Runtime.InteropServices;

using VAR.Toolbox.Code.Platforms.Linux;
using VAR.Toolbox.Code.Platforms.Stub;
using VAR.Toolbox.Code.Platforms.Windows;

namespace VAR.Toolbox.Code.Platforms;

public static class Platform
{
    public static IPlatform Current { get; } = CreatePlatformService();

    private static IPlatform CreatePlatformService()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) { return new WindowsPlatform(); }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) { return new LinuxPlatform(); }

        return new StubPlatform();
    }
    
}
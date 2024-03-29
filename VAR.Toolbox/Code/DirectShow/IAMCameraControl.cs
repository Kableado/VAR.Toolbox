﻿namespace VAR.Toolbox.Code.DirectShow
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The IAMCameraControl interface controls camera settings such as zoom, pan, aperture adjustment,
    /// or shutter speed. To obtain this interface, query the filter that controls the camera.
    /// </summary>
    [ComImport,
     Guid("C6E13370-30AC-11d0-A18C-00A0C9118956"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAMCameraControl
    {
        /// <summary>
        /// Gets the range and default value of a specified camera property.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to query.</param>
        /// <param name="pMin">Receives the minimum value of the property.</param>
        /// <param name="pMax">Receives the maximum value of the property.</param>
        /// <param name="pSteppingDelta">Receives the step size for the property.</param>
        /// <param name="pDefault">Receives the default value of the property. </param>
        /// <param name="pCapsFlags">Receives a member of the CameraControlFlags enumeration, indicating whether the property is controlled automatically or manually.</param>
        /// 
        /// <returns>Return's <b>HRESULT</b> error code.</returns>
        /// 
        [PreserveSig]
        int GetRange(
            [In] CameraControlProperty property,
            [Out] out int pMin,
            [Out] out int pMax,
            [Out] out int pSteppingDelta,
            [Out] out int pDefault,
            [Out] out CameraControlFlags pCapsFlags
        );

        /// <summary>
        /// Sets a specified property on the camera.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to set.</param>
        /// <param name="lValue">Specifies the new value of the property.</param>
        /// <param name="flags">Specifies the desired control setting, as a member of the CameraControlFlags enumeration.</param>
        /// 
        /// <returns>Return's <b>HRESULT</b> error code.</returns>
        /// 
        [PreserveSig]
        int Set(
            [In] CameraControlProperty property,
            [In] int lValue,
            [In] CameraControlFlags flags
        );

        /// <summary>
        /// Gets the current setting of a camera property.
        /// </summary>
        /// 
        /// <param name="property">Specifies the property to retrieve.</param>
        /// <param name="lValue">Receives the value of the property.</param>
        /// <param name="flags">Receives a member of the CameraControlFlags enumeration.
        /// The returned value indicates whether the setting is controlled manually or automatically.</param>
        /// 
        /// <returns>Return's <b>HRESULT</b> error code.</returns>
        /// 
        [PreserveSig]
        int Get(
            [In] CameraControlProperty property,
            [Out] out int lValue,
            [Out] out CameraControlFlags flags
        );
    }
}
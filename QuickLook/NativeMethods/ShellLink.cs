﻿// Copyright © 2017-2025 QL-Win Contributors
//
// This file is part of QuickLook program.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable InconsistentNaming

namespace QuickLook.NativeMethods;

[Flags]
internal enum SLGP_FLAGS
{
    /// <summary>Retrieves the standard short (8.3 format) file name</summary>
    SLGP_SHORTPATH = 0x1,

    /// <summary>Retrieves the Universal Naming Convention (UNC) path name of the file</summary>
    SLGP_UNCPRIORITY = 0x2,

    /// <summary>
    ///     Retrieves the raw path name. A raw path is something that might not exist and may include environment
    ///     variables that need to be expanded
    /// </summary>
    SLGP_RAWPATH = 0x4
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
internal struct WIN32_FIND_DATAW
{
    public uint dwFileAttributes;
    public long ftCreationTime;
    public long ftLastAccessTime;
    public long ftLastWriteTime;
    public uint nFileSizeHigh;
    public uint nFileSizeLow;
    public uint dwReserved0;
    public uint dwReserved1;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string cFileName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
    public string cAlternateFileName;
}

[Flags]
internal enum SLR_FLAGS
{
    /// <summary>
    ///     Do not display a dialog box if the link cannot be resolved. When SLR_NO_UI is set,
    ///     the high-order word of fFlags can be set to a time-out value that specifies the
    ///     maximum amount of time to be spent resolving the link. The function returns if the
    ///     link cannot be resolved within the time-out duration. If the high-order word is set
    ///     to zero, the time-out duration will be set to the default value of 3,000 milliseconds
    ///     (3 seconds). To specify a value, set the high word of fFlags to the desired time-out
    ///     duration, in milliseconds.
    /// </summary>
    SLR_NO_UI = 0x1,

    /// <summary>Obsolete and no longer used</summary>
    SLR_ANY_MATCH = 0x2,

    /// <summary>
    ///     If the link object has changed, update its path and list of identifiers.
    ///     If SLR_UPDATE is set, you do not need to call IPersistFile::IsDirty to determine
    ///     whether or not the link object has changed.
    /// </summary>
    SLR_UPDATE = 0x4,

    /// <summary>Do not update the link information</summary>
    SLR_NOUPDATE = 0x8,

    /// <summary>Do not execute the search heuristics</summary>
    SLR_NOSEARCH = 0x10,

    /// <summary>Do not use distributed link tracking</summary>
    SLR_NOTRACK = 0x20,

    /// <summary>
    ///     Disable distributed link tracking. By default, distributed link tracking tracks
    ///     removable media across multiple devices based on the volume name. It also uses the
    ///     Universal Naming Convention (UNC) path to track remote file systems whose drive letter
    ///     has changed. Setting SLR_NOLINKINFO disables both types of tracking.
    /// </summary>
    SLR_NOLINKINFO = 0x40,

    /// <summary>Call the Microsoft Windows Installer</summary>
    SLR_INVOKE_MSI = 0x80
}

/// <summary>The IShellLink interface allows Shell links to be created, modified, and resolved</summary>
[ComImport]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("000214F9-0000-0000-C000-000000000046")]
internal interface IShellLinkW
{
    /// <summary>Retrieves the path and file name of a Shell link object</summary>
    public void GetPath([Out] [MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATAW pfd, SLGP_FLAGS fFlags);

    /// <summary>Retrieves the list of item identifiers for a Shell link object</summary>
    public void GetIDList(out nint ppidl);

    /// <summary>Sets the pointer to an item identifier list (PIDL) for a Shell link object.</summary>
    public void SetIDList(nint pidl);

    /// <summary>Retrieves the description string for a Shell link object</summary>
    public void GetDescription([Out] [MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszName, int cchMaxName);

    /// <summary>Sets the description for a Shell link object. The description can be any application-defined string</summary>
    public void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

    /// <summary>Retrieves the name of the working directory for a Shell link object</summary>
    public void GetWorkingDirectory([Out] [MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszDir, int cchMaxPath);

    /// <summary>Sets the name of the working directory for a Shell link object</summary>
    public void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

    /// <summary>Retrieves the command-line arguments associated with a Shell link object</summary>
    public void GetArguments([Out] [MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszArgs, int cchMaxPath);

    /// <summary>Sets the command-line arguments for a Shell link object</summary>
    public void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

    /// <summary>Retrieves the hot key for a Shell link object</summary>
    public void GetHotkey(out short pwHotkey);

    /// <summary>Sets a hot key for a Shell link object</summary>
    public void SetHotkey(short wHotkey);

    /// <summary>Retrieves the show command for a Shell link object</summary>
    public void GetShowCmd(out int piShowCmd);

    /// <summary>Sets the show command for a Shell link object. The show command sets the initial show state of the window.</summary>
    public void SetShowCmd(int iShowCmd);

    /// <summary>Retrieves the location (path and index) of the icon for a Shell link object</summary>
    public void GetIconLocation([Out] [MarshalAs(UnmanagedType.LPWStr)]
        StringBuilder pszIconPath,
        int cchIconPath, out int piIcon);

    /// <summary>Sets the location (path and index) of the icon for a Shell link object</summary>
    public void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

    /// <summary>Sets the relative path to the Shell link object</summary>
    public void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

    /// <summary>Attempts to find the target of a Shell link, even if it has been moved or renamed</summary>
    public void Resolve(nint hwnd, SLR_FLAGS fFlags);

    /// <summary>Sets the path and file name of a Shell link object</summary>
    public void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
}

// CLSID_ShellLink from ShlGuid.h
[ComImport]
[Guid("00021401-0000-0000-C000-000000000046")]
public class ShellLink;

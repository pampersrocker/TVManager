using System;
using System.Runtime.InteropServices;

namespace TVManager.WDDM
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Region2D
    {
        public uint X;
        public uint Y;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }


    public enum ModeInfoType : uint
    {
        Source = 1,
        Target = 2,
        DesktopImage = 3
    }

    public enum PixelFormat : uint
    {
        E8BPP = 1,
        E16BPP = 2,
        E24BPP = 3,
        E32BPP = 4,
        ENONGDI = 5,
    }

    public enum ScanlineOrdering : uint
    {
        Unspecified = 0,
        Progressive = 1,
        Interlaced = 2,
        InterlacedUpperFieldFirst = Interlaced,
        InterlacedLowerFieldFirst = 3,
    }

    [Flags]
    public enum PathSourceStatusFlags : uint
    {
        InUse = 1
    }

    [Flags]
    public enum PathTargetStatusFlags : uint
    {
        InUse = 0x00000001,
        Forcible = 0x00000002,
        ForcedAvailabilityBoot = 0x00000004,
        ForcedAvailabilityPath = 0x00000008,
        ForcedAvailabilitySystem = 0x00000010,
    }

    [Flags]
    public enum PathStatusFlags : uint
    {
        Active = 1,
    }

    public enum VideoOutputTechnology : uint
    {
        Other = unchecked((uint) -1),
        HD15 = 0,
        SVideo = 1,
        CompositeVideo = 2,
        ComponentVideo = 3,
        DVI = 4,
        HDMI = 5,
        LVDS = 6,
        D_jpn = 8,
        SDI = 9,
        DisplayportExternal = 10,
        DisplayportEmbedded = 11,
        UDIExternal = 12,
        UDIEmbedded = 13,
        SDTVDongle = 14,
        Miracast = 15,
        Internal = 0x80000000,
    }

    public enum RotationType : uint
    {
        Identity = 1,
        Rotate90 = 2,
        Rotate180 = 3,
        Rotate270 = 4,
    }

    public enum ScalingType : uint
    {
        Identity = 1,
        Centered = 2,
        Stretched = 3,
        AspectRatioCenteredMax = 4,
        Custom = 5,
        Preferred = 128,
    }

    [Flags]
    public enum SetDisplayConfigFlags : uint
    {
        TopologyInternal = 0x00000001,
        TopologyClone = 0x00000002,
        TopologyExtend = 0x00000004,
        TopologyExternal = 0x00000008,
        TopologySupplied = 0x00000010,
        UseDatabaseCurrent = (TopologyInternal | TopologyClone | TopologyExtend | TopologyExternal),
        UseSuppliedDisplayConfig = 0x00000020,
        Validate = 0x00000040,
        Apply = 0x00000080,
        NoOptimization = 0x00000100,
        SaveToDatabase = 0x00000200,
        AllowChanges = 0x00000400,
        PathPersistIfRequired = 0x00000800,
        ForceModeEnumeration = 0x00001000,
        AllowOathOrderChanges = 0x00002000,
    }

    [Flags]
    public enum TopologyId : uint
    {
        Internal = 0x00000001,
        Clone = 0x00000002,
        Extend = 0x00000004,
        External = 0x00000008,
    }

    [Flags]
    public enum QueryDisplayFlags : uint
    {
        AllPaths = 0x00000001,
        OnlyActivePaths = 0x00000002,
        DatabaseCurrent = 0x00000004,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public uint HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SourceMode
    {
        public uint Width;
        public uint Height;
        public PixelFormat PixelFormat;
        public Point Position;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Rational
    {
        public uint Numerator;
        public uint Denominator;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct VideoSignalInfo
    {
        public ulong PixelRate;
        public Rational HorizontalSyncFrequency;
        public Rational VerticalSyncFrequency;
        public Region2D ActiveSize;
        public Region2D TotalSize;
        public uint VideoStandard;
        public ScanlineOrdering ScanlineOrdering;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TargetMode
    {
        public VideoSignalInfo TargetVideoSignalInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DesktopImageInfo
    {
        public Point PathSourceSize;
        public Rect DesktopImageRegion;
        public Rect DesktopImageClip;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ModeInfo
    {
        [FieldOffset(0)]
        public ModeInfoType InfoType;
        [FieldOffset(4)]
        public uint Id;
        [FieldOffset(8)]
        public LUID AdapterId;
        // Next 3 Fields Form a Union
        [FieldOffset(16)]
        public SourceMode SourceMode;
        [FieldOffset(16)]
        public TargetMode TargetMode;
        [FieldOffset(16)]
        public DesktopImageInfo DesktopImageInfo;


    }

    [StructLayout(LayoutKind.Explicit)]
    public struct PathSourceInfo
    {
        [FieldOffset(0)]
        public LUID AdapterId;
        [FieldOffset(8)]
        public uint Id;
        [FieldOffset(12)]
        public uint ModeInfoIdx;
        [FieldOffset(12)]
        public ushort CloneGroupId;
        [FieldOffset(14)]
        public ushort SourceModeInfoIdx;
        [FieldOffset(16)]
        public PathSourceStatusFlags StatusFlags;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DesktopTargetModeIndexes
    {
        public ushort DesktopModeInfoIdx;
        public ushort TargetModeInfoIdx;
    }
    
    [StructLayout(LayoutKind.Sequential)]
    public struct PathTargetInfo
    {
        public LUID AdapterId;
        public uint Id;
        public DesktopTargetModeIndexes ModeInfoIdx;
        public VideoOutputTechnology OutputTechnology;
        public RotationType Rotation;
        public ScalingType Scaling;
        public Rational RefreshRate;
        public ScanlineOrdering ScanlineOrdering;
        public bool TargetAvailable;
        public PathTargetStatusFlags StatusFlags;

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PathInfo
    {
        public PathSourceInfo SourceInfo;
        public PathTargetInfo TargetInfo;
        public PathStatusFlags StatusFlags;
    }

    public class CCD
    {
        public static int QueryDisplayConfig(QueryDisplayFlags Flags, out PathInfo[] PathInfos, out ModeInfo[] ModeInfos, out TopologyId Topology)
        {
            uint NumPathArrayElements, NumModeInfoArrayElements;
            CCD_Internal.GetDisplayConfigBufferSizes(Flags, out NumPathArrayElements, out NumModeInfoArrayElements);

            PathInfos = new PathInfo[NumPathArrayElements];
            ModeInfos = new ModeInfo[NumModeInfoArrayElements];

            return CCD_Internal.QueryDisplayConfig(Flags, ref NumPathArrayElements, PathInfos, ref NumModeInfoArrayElements, ModeInfos, out Topology);

        }

        public static int SetDisplayConfig(SetDisplayConfigFlags Flags, PathInfo[] PathInfos, ModeInfo[] ModeInfos)
        {
            return CCD_Internal.SetDisplayConfig((uint)PathInfos.Length, PathInfos, (uint)ModeInfos.Length, ModeInfos, Flags);
        }
    }

    class CCD_Internal
    {
        [DllImport("User32.dll")]
        public static extern int SetDisplayConfig(
           uint NumPathArrayElements,
           [In] PathInfo[] PathArray,
           uint NumModeInfoArrayElements,
           [In] ModeInfo[] ModeInfoArray,
           SetDisplayConfigFlags Flags
        );

        [DllImport("User32.dll")]
        public static extern int QueryDisplayConfig(
            QueryDisplayFlags flags,
            ref uint numPathArrayElements,
            [Out] PathInfo[] pathInfoArray,
            ref uint modeInfoArrayElements,
            [Out] ModeInfo[] modeInfoArray,
            out TopologyId Topology
        );

        [DllImport("User32.dll")]
        public static extern int GetDisplayConfigBufferSizes(
            QueryDisplayFlags Flags,
            out uint NumPathArrayElements,
            out uint NumModeInfoArrayElements
            );
    }

   
}

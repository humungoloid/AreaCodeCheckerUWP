using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AreaCodeCheckerUWP
{
    public class CStructures
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            [MarshalAs(UnmanagedType.U1)]
            public byte BlendOp;
            [MarshalAs(UnmanagedType.U1)]
            public byte BlendFlags;
            [MarshalAs(UnmanagedType.U1)]
            public byte SourceConstantAlpha;
            [MarshalAs(UnmanagedType.U1)]
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            [MarshalAs(UnmanagedType.I4)]
            public int x;
            [MarshalAs(UnmanagedType.I4)]
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SIZE
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cx;
            [MarshalAs(UnmanagedType.I4)]
            public int cy;
        }
    }
}


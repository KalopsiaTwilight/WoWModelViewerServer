using System.Runtime.InteropServices;

namespace WoWFileFormats.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C2Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C2iVector
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C3Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public readonly C3Vector Scale(float scalar)
        {
            return new C3Vector
            {
                X = X * scalar,
                Y = Y * scalar,
                Z = Z * scalar
            };
        }

        public float Dot(C3Vector other)
        {
            var result = X * other.X;
            result += Y * other.Y;
            result += Z * other.Z;
            return result;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C3iVector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C4Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C4iVector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int W { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C33Matrix
    {
        public C3Vector Col1 { get; set; }
        public C3Vector Col2 { get; set; }
        public C3Vector Col3 { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct C34Matrix
    {
        public C3Vector Col1 { get; set; }
        public C3Vector Col2 { get; set; }
        public C3Vector Col3 { get; set; }
        public C3Vector Col4 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C44Matrix
    {
        public C4Vector Col1 { get; set; }
        public C4Vector Col2 { get; set; }
        public C4Vector Col3 { get; set; }
        public C4Vector Col4 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C4Plane
    {
        public C3Vector Normal { get; set; }
        public float Distance { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Quat32
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
    }



    /** https://wowdev.wiki/Quaternion_values_and_2.x */
    [StructLayout(LayoutKind.Sequential)]
    public struct Quat16
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public short W { get; set; }

        public Quat32 ToQuat32()
        {
            return new Quat32()
            {
                X = (X <= 0 ? X + 32768 : X - 32767) / 32767.0f,
                Y = (Y <= 0 ? Y + 32768 : Y - 32767) / 32767.0f,
                Z = (Z <= 0 ? Z + 32768 : Z - 32767) / 32767.0f,
                W = (W <= 0 ? W + 32768 : W - 32767) / 32767.0f,
            };
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CRange
    {
        public float Min { get; set; }
        public float Max { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CAxisAlignedBox
    {
        public C3Vector Min { get; set; }
        public C3Vector Max { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CAxisAlignedSphere
    {
        public C3Vector Position { get; set; }
        public float Radius { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CArgb
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }



    [StructLayout(LayoutKind.Sequential)]
    public struct CImVector
    {
        public byte B { get; set; }
        public byte G { get; set; }
        public byte R { get; set; }
        public byte A { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct C3sVector
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct C3Segment
    {
        public C3Vector Start { get; set; }
        public C3Vector End { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CFacet
    {
        public C4Plane Plane { get; set; }
        public C3Vector Vertex1 { get; set; }
        public C3Vector Vertex2 { get; set; }
        public C3Vector Vertex3 { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct C3Ray
    {
        public C3Vector Origin { get; set; }
        public C3Vector Dir { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CRect
    {
        public float MinY { get; set; }
        public float MinX { get; set; }
        public float MaxY { get; set; }
        public float MaxX { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct CiRect
    {
        public int MinY { get; set; }
        public int MinX { get; set; }
        public int MaxY { get; set; }
        public int MaxX { get; set; }
    }

    public struct FixedPoint16
    { 
        public ushort Raw { get; set; }
        public byte IntegerBits { get; set; }
        public byte DecimalBits { get; set; }

        public FixedPoint16(ushort raw, byte integerBits, byte decimalBits)
        {
            Raw = raw;
            IntegerBits = integerBits;
            DecimalBits = decimalBits;
        }

        public readonly float ToFloat()
        {
            var integerAndDecimalBits = ((1 << (IntegerBits + DecimalBits)) - 1) & Raw;
            var sign = ((1 << (IntegerBits + DecimalBits)) & Raw) > 0;
            float factor = IntegerBits > 0
                ? (1 << DecimalBits)
                : (1 << (DecimalBits + 1)) - 1;
            return (sign ? -1 : 1) * integerAndDecimalBits / factor;
        }
    }

    public struct FixedPoint8
    {
        public byte Raw { get; set; }
        public byte IntegerBits { get; set; }
        public byte DecimalBits { get; set; }

        public FixedPoint8(byte raw, byte integerBits, byte decimalBits)
        {
            Raw = raw;
            IntegerBits = integerBits;
            DecimalBits = decimalBits;
        }

        public readonly float ToFloat()
        {
            var integerAndDecimalBits = ((1 << (IntegerBits + DecimalBits)) - 1) & Raw;
            var sign = ((1 << (IntegerBits + DecimalBits)) & Raw) > 0;
            float factor = IntegerBits > 0
                ? (1 << DecimalBits)
                : (1 << (DecimalBits + 1)) - 1;
            return (sign ? -1 : 1) * integerAndDecimalBits / factor;
        }
    }

    public struct Fixed16
    {
        public ushort Raw { get; set; }

        public Fixed16(ushort raw) { Raw = raw; }

        public readonly float ToFloat()
        {
            return Raw / (float)0x7fff;
        }
    }
}

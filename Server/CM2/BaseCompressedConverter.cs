using System.Numerics;
using System.Runtime.InteropServices;
using WoWFileFormats.Common;

namespace Server.CM2
{
    using Color = (byte R, byte G, byte B, byte A);
    using Float2 = (float X, float Y);
    using Float3 = (float X, float Y, float Z);
    using Float4 = (float X, float Y, float Z, float W);
    using Int2 = (int X, int Y);

    public class BaseCompressedConverter
    {

        public static Float2 Convert(CRange range)
        {
            return (range.Min, range.Max);
        }

        public static Float2 Convert(C2Vector vector)
        {
            return (vector.X, vector.Y);
        }

        public static Float3 Convert(C3Vector vector)
        {
            return (vector.X, vector.Y, vector.Z);
        }

        public static Float4 Convert(C4Vector vector)
        {
            return (vector.X, vector.Y, vector.Z, vector.W);
        }

        public static Float4 Convert(Quat32 quat)
        {
            return (quat.X, quat.Y, quat.Z, quat.W);
        }

        public static Float4 Convert(Quat16 quat)
        {
            return Convert(quat.ToQuat32());
        }

        public static float[] Convert(C44Matrix matrix)
        {
            return [
                matrix.Col1.X, matrix.Col1.Y, matrix.Col1.Z, matrix.Col1.W,
                matrix.Col2.X, matrix.Col2.Y, matrix.Col2.Z, matrix.Col2.W,
                matrix.Col3.X, matrix.Col3.Y, matrix.Col3.Z, matrix.Col3.W,
                matrix.Col4.X, matrix.Col4.Y, matrix.Col4.Z, matrix.Col4.W,
            ];
        }

        public static Color Convert(CImVector vector)
        {
            return (vector.R, vector.G, vector.B, vector.A);
        }

        public static Color Convert(CArgb argb)
        {
            return (argb.R, argb.G, argb.B, argb.A);
        }

        public static Int2 Convert(C2iVector vector)
        {
            return (vector.X, vector.Y);
        }
    }
}

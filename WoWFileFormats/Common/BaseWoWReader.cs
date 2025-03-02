using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WoWFileFormats.Common
{
    public abstract class BaseWoWReader : IDisposable
    {
        internal BinaryReader _reader;
        internal Stream _stream;

        internal BaseWoWReader(Stream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(_stream);
        }

        public void Dispose()
        {
            _reader.Dispose();
            _stream.Dispose();
        }

        internal C2Vector ReadC2Vector()
        {
            return new C2Vector()
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
            };
        }
        internal C2iVector ReadC2iVector()
        {
            return new C2iVector()
            {
                X = _reader.ReadInt32(),
                Y = _reader.ReadInt32(),
            };
        }

        internal C3Vector ReadC3Vector()
        {
            return new C3Vector()
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle(),
            };
        }
        internal C3iVector ReadC3iVector()
        {
            return new C3iVector()
            {
                X = _reader.ReadInt32(),
                Y = _reader.ReadInt32(),
                Z = _reader.ReadInt32(),
            };
        }
        internal C4Vector ReadC4Vector()
        {
            return new C4Vector()
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle(),
                W = _reader.ReadSingle(),
            };
        }

        internal C4iVector ReadC4iVector()
        {
            return new C4iVector()
            {
                X = _reader.ReadInt32(),
                Y = _reader.ReadInt32(),
                Z = _reader.ReadInt32(),
                W = _reader.ReadInt32()
            };
        }

        internal C33Matrix ReadC33Matrix()
        {
            return new C33Matrix()
            {
                Col1 = ReadC3Vector(),
                Col2 = ReadC3Vector(),
                Col3 = ReadC3Vector(),
            };
        }

        internal C34Matrix ReadC34Matrix()
        {
            return new C34Matrix()
            {
                Col1 = ReadC3Vector(),
                Col2 = ReadC3Vector(),
                Col3 = ReadC3Vector(),
                Col4 = ReadC3Vector(),
            };
        }

        internal C44Matrix ReadC44Matrix()
        {
            return new C44Matrix()
            {
                Col1 = ReadC4Vector(),
                Col2 = ReadC4Vector(),
                Col3 = ReadC4Vector(),
                Col4 = ReadC4Vector(),
            };
        }

        internal C4Plane ReadC4Plane()
        {
            return new C4Plane()
            {
                Normal = ReadC3Vector(),
                Distance = _reader.ReadSingle()
            };
        }

        internal Quat32 ReadQuat32()
        {
            return new Quat32()
            {
                X = _reader.ReadSingle(),
                Y = _reader.ReadSingle(),
                Z = _reader.ReadSingle(),
                W = _reader.ReadSingle(),
            };
        }

        internal Quat16 ReadQuat16()
        {
            return new Quat16()
            {
                X = _reader.ReadInt16(),
                Y = _reader.ReadInt16(),
                Z = _reader.ReadInt16(),
                W = _reader.ReadInt16(),
            };
        }

        internal CRange ReadCRange()
        {
            return new CRange()
            {
                Min = _reader.ReadSingle(),
                Max = _reader.ReadSingle()
            };
        }

        internal CAxisAlignedBox ReadCAxisAlignedBox()
        {
            return new CAxisAlignedBox()
            {
                Min = ReadC3Vector(),
                Max = ReadC3Vector(),
            };
        }

        internal CAxisAlignedSphere ReadCAxisAlignedSphere()
        {
            return new CAxisAlignedSphere()
            {
                Position = ReadC3Vector(),
                Radius = _reader.ReadSingle(),
            };
        }

        internal CArgb ReadCArgb()
        {
            return new CArgb()
            {
                R = _reader.ReadByte(),
                G = _reader.ReadByte(),
                B = _reader.ReadByte(),
                A = _reader.ReadByte(),
            };
        }

        internal CImVector ReadCImVector()
        {
            return new CImVector()
            {
                B = _reader.ReadByte(),
                G = _reader.ReadByte(),
                R = _reader.ReadByte(),
                A = _reader.ReadByte(),
            };
        }

        internal C3sVector ReadC3sVector()
        {
            return new C3sVector()
            {
                X = _reader.ReadInt16(),
                Y = _reader.ReadInt16(),
                Z = _reader.ReadInt16(),
            };
        }

        internal C3Segment ReadC3Segment()
        {
            return new C3Segment()
            {
                Start = ReadC3Vector(),
                End = ReadC3Vector(),
            };
        }

        internal CFacet ReadCFacet()
        {
            return new CFacet()
            {
                Plane = ReadC4Plane(),
                Vertex1 = ReadC3Vector(),
                Vertex2 = ReadC3Vector(),
                Vertex3 = ReadC3Vector(),
            };
        }

        internal C3Ray ReadC3Ray()
        {
            return new C3Ray()
            {
                Origin = ReadC3Vector(),
                Dir  = ReadC3Vector(),
            };
        }

        internal CRect ReadCRect()
        {
            return new CRect()
            {
                MinY = _reader.ReadSingle(),
                MinX = _reader.ReadSingle(),
                MaxY = _reader.ReadSingle(),
                MaxX = _reader.ReadSingle(),
            };
        }

        internal CiRect ReadCiRect()
        {
            return new CiRect()
            {
                MinY = _reader.ReadInt32(),
                MinX = _reader.ReadInt32(),
                MaxY = _reader.ReadInt32(),
                MaxX = _reader.ReadInt32(),
            };
        }

        internal FixedPoint16 ReadFixedPoint16(byte integerBits, byte decimalBits)
        {
            return new FixedPoint16(_reader.ReadUInt16(), integerBits, decimalBits);
        }

        internal FixedPoint8 ReadFixedPoint8(byte integerBits, byte decimalBits)
        {
            return new FixedPoint8(_reader.ReadByte(), integerBits, decimalBits);
        }

        internal Fixed16 ReadFixed16()
        {
            return new Fixed16(_reader.ReadUInt16());
        }

        private T Read<T>() where T : unmanaged
        {
            byte[] result = _reader.ReadBytes(Unsafe.SizeOf<T>());

            return Unsafe.ReadUnaligned<T>(ref result[0]);
        }
    }
}

using System.Text;
using WoWFileFormats.Common;

namespace WoWFileFormats.WMO
{
    public class WMOTypesReader : BaseWoWReader
    {
        public WMOTypesReader(Stream stream) : base(stream)
        {
        }

        public WMODoodadDef ReadWMODoodadDef()
        {
            var nameOffsetBytes = _reader.ReadBytes(3);
            return new WMODoodadDef()
            {
                NameOffset = (uint)(nameOffsetBytes[0] | nameOffsetBytes[1] << 8 | nameOffsetBytes[2] << 16),
                Flags = (WMODoodadFlags)_reader.ReadByte(),
                Position = ReadC3Vector(),
                Rotation = ReadQuat32(),
                Scale = _reader.ReadSingle(),
                Color = ReadCImVector(),
            };
        }

        public WMODoodadSet ReadWMODoodadSet()
        {
            return new WMODoodadSet()
            {
                Name = Encoding.UTF8.GetString(_reader.ReadBytes(20)),
                StartIndex = _reader.ReadUInt32(),
                Count = _reader.ReadUInt32(),
                Unused = _reader.ReadUInt32(),
            };
        }

        public WMOMaterial ReadWMOMaterial()
        {
            return new WMOMaterial()
            {
                Flags = (WMOMaterialFlags)_reader.ReadUInt32(),
                Shader = (WMOShader)_reader.ReadUInt32(),
                BlendMode = _reader.ReadUInt32(),
                Texture1 = _reader.ReadUInt32(),
                SidnColor = ReadCImVector(),
                FrameSidnColor = ReadCImVector(),
                Texture2 = _reader.ReadUInt32(),
                DiffColor = ReadCImVector(),
                GroundTypeId = _reader.ReadUInt32(),
                Texture3 = _reader.ReadUInt32(),
                Color2 = _reader.ReadUInt32(),
                Flags2 = _reader.ReadUInt32(),
                RunTimeData = [
                    _reader.ReadUInt32(),
                    _reader.ReadUInt32(),
                    _reader.ReadUInt32(),
                    _reader.ReadUInt32(),
                ]
            };
        }

        public WMOGroupInfo ReadWMOGroupInfo()
        {
            return new WMOGroupInfo()
            {
                Flags = (WMOGroupFileFlags)_reader.ReadUInt32(),
                BoundingBox = ReadCAxisAlignedBox(),
                NameOffset = _reader.ReadInt32(),
            };
        }

        public WMOLodInfo ReadWMOLodInfo()
        {
            return new WMOLodInfo()
            {
                Flags2 = (WmoGroupFileFlags2)_reader.ReadUInt32(),
                LodIndex = _reader.ReadUInt32(),
            };
        }

        public WMOFog ReadWMOFog()
        {
            return new WMOFog()
            {
                Flags = _reader.ReadUInt32(),
                Position = ReadC3Vector(),
                SmallerRadius = _reader.ReadSingle(),
                LargerRadius = _reader.ReadSingle(),
                FogEnd = _reader.ReadSingle(),
                FogStartScalar = _reader.ReadSingle(),
                FogColor = ReadCImVector(),
                UwFogEnd = _reader.ReadSingle(),
                UwFogStartScalar = _reader.ReadSingle(),
                UwFogColor = ReadCImVector(),
            };
        }

        public WMOPortal ReadWMOPortal()
        {
            return new WMOPortal()
            {
                StartVertex = _reader.ReadUInt16(),
                VertexCount = _reader.ReadUInt16(),
                Plane = ReadC4Plane(),
            };
        }

        public WMOPortalRef ReadWMOPortalRef()
        {
            return new WMOPortalRef()
            {
                PortalIndex = _reader.ReadUInt16(),
                GroupIndex = _reader.ReadUInt16(),
                Side = _reader.ReadInt16(),
                Filler = _reader.ReadUInt16()
            };
        }

        public WMOAmbientVolume ReadWMOAmbientVolume()
        {
            return new WMOAmbientVolume()
            {
                Position = ReadC3Vector(),
                Start = _reader.ReadSingle(),
                End = _reader.ReadSingle(),
                Color1 = ReadCImVector(),
                Color2 = ReadCImVector(),
                Color3 = ReadCImVector(),
                Flags = _reader.ReadUInt32(),
                DoodadSetId = _reader.ReadUInt16(),
                Unknown = _reader.ReadBytes(10)
            };
        }

        public WMOLiquidVertex ReadWMOLiquidVertex()
        {
            return new WMOLiquidVertex()
            {
                Data = _reader.ReadUInt32(),
                Height = _reader.ReadSingle()
            };
        }

        public WMOLiquidTile ReadWMOLiquidTile()
        {
            var data = _reader.ReadByte();
            return new WMOLiquidTile()
            {
                Unknown1 = GetBits(data, 0, 1),
                Unknown2 = GetBits(data, 1, 1),
                Fishable = GetBits(data, 2, 1),
                Shared = GetBits(data, 3, 1),
                LegacyLiquidType = GetBits(data, 4, 4),
            };
        }

        public WMOLiquid ReadWMOLiquid()
        {
            var result = new WMOLiquid()
            {
                LiquidVertices = ReadC2iVector(),
                LiquidTiles = ReadC2iVector(),
                Position = ReadC3Vector(),
                MaterialId = _reader.ReadUInt16(),
            };
            var numVertices = result.LiquidVertices.X * result.LiquidVertices.Y;
            result.Vertices = new WMOLiquidVertex[numVertices];
            for(var i = 0; i < numVertices; i++)
            {
                result.Vertices[i] = ReadWMOLiquidVertex();
            }

            var numTiles = result.LiquidTiles.X * result.LiquidTiles.Y;
            result.Tiles = new WMOLiquidTile[numTiles];
            for(var i = 0; i < numTiles; i++)
            {
                result.Tiles[i] = ReadWMOLiquidTile();
            }
            return result;
        }

        public WMOBatch ReadWMOBatch()
        {
            return new WMOBatch()
            {
                Unknown = _reader.ReadBytes(10),
                MaterialIdLarge = _reader.ReadUInt16(),
                StartIndex = _reader.ReadUInt32(),
                IndexCount = _reader.ReadUInt16(),
                FirstVertex = _reader.ReadUInt16(),
                LastVertex = _reader.ReadUInt16(),
                UseMaterialIdLarge = _reader.ReadByte(),
                MaterialId = _reader.ReadByte(),
            };
        }

        private byte GetBits(byte input, int start, int numBits)
        {
            return (byte)(input >> (8 - start - numBits) & ((1 << numBits) - 1));
        }
    }
}

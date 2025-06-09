using System.IO.Compression;

namespace ModelViewer.Core.CM2
{
    public class CWMOWriter : BaseCompressedWriter
    {
        public CWMOWriter(Stream stream) : base(stream)
        {
        }

        public void Write(CWMOFile file)
        {
            using var memory = new MemoryStream(GetOutputSize(file));
            _writer = new BinaryWriter(memory);
            
            var startPos = 0;
            memory.Position = startPos;

            _writer.Write(file.FileDataID);
            _writer.Write(file.Flags);
            _writer.Write(file.WMOId);
            _writer.Write(file.SkyboxFileId);
            Write(file.AmbientColor);
            Write(file.MinBoundingBox);
            Write(file.MaxBoundingBox);

            var materialsPos = (uint)memory.Position;
            WriteArray(file.Materials, Write);
            var groupInfoPos = (uint)memory.Position;
            WriteArray(file.GroupInfo, Write);
            var doodadDefPos = (uint)memory.Position;
            WriteArray(file.DoodadDefs, Write);
            var doodadIdsPos = (uint)memory.Position;
            WriteArray(file.DoodadIds, _writer.Write);
            var fogsPos = (uint)memory.Position;
            WriteArray(file.Fogs, Write);
            var doodadSetsPos = (uint)memory.Position;
            WriteArray(file.DoodadSets, Write);
            var portalRefsPos = (uint)memory.Position;
            WriteArray(file.PortalRefs, Write);
            var portalPos = (uint)memory.Position;
            WriteArray(file.Portals, Write);
            var globalAmbientVolumePos = (uint)memory.Position;
            WriteArray(file.GlobalAmbientVolumes, Write);
            var ambientVolumesPos = (uint)memory.Position;
            WriteArray(file.AmbientVolumes, Write);
            var portalVerticesPos = (uint)memory.Position;
            WriteArray(file.PortalVertices, Write);
            var groupsPos = (uint)memory.Position;
            WriteArray(file.Groups, Write);
            var dataEndPos = (uint)memory.Position;

            _writer = new BinaryWriter(_stream);
            _writer.Write((uint)0x43574D4F);
            _writer.Write(file.Version);
            _writer.Write(startPos);
            _writer.Write(materialsPos);
            _writer.Write(groupInfoPos);
            _writer.Write(doodadDefPos);
            _writer.Write(doodadIdsPos);
            _writer.Write(fogsPos);
            _writer.Write(doodadSetsPos);
            _writer.Write(portalRefsPos);
            _writer.Write(portalPos);
            _writer.Write(globalAmbientVolumePos);
            _writer.Write(ambientVolumesPos);
            _writer.Write(portalVerticesPos);
            _writer.Write(groupsPos);
            _writer.Write(dataEndPos);

            memory.Position = 0;
            using var compressor = new ZLibStream(_stream, CompressionMode.Compress);
            memory.CopyTo(compressor);
        }

        internal void Write(CWMOMaterial material)
        {
            _writer.Write(material.Flags);
            _writer.Write(material.Shader);
            _writer.Write(material.BlendMode);
            _writer.Write(material.Texture1);
            Write(material.SidnColor);
            Write(material.FrameSidnColor);
            _writer.Write(material.Texture2);
            Write(material.DiffColor);
            _writer.Write(material.GroundTypeId);
            _writer.Write(material.Texture3);
            _writer.Write(material.Color2);
            _writer.Write(material.Flags2);
            _writer.Write(material.RunTimeData[0]);
            _writer.Write(material.RunTimeData[1]);
            _writer.Write(material.RunTimeData[2]);
            _writer.Write(material.RunTimeData[3]);
        }

        internal void Write(CWMOGroupInfo group)
        {
            _writer.Write(group.Flags);
            Write(group.MinBoundingBox);
            Write(group.MaxBoundingBox);
        }

        internal void Write(CWMODoodadDef def)
        {
            _writer.Write(def.NameOffset);
            _writer.Write(def.Flags);
            Write(def.Position);
            Write(def.Rotation);
            _writer.Write(def.Scale);
            Write(def.Color);
        }

        internal void Write(CWMOFog fog)
        {
            _writer.Write(fog.Flags);
            Write(fog.Position);
            _writer.Write(fog.SmallerRadius);
            _writer.Write(fog.LargerRadius);
            _writer.Write(fog.FogEnd);
            _writer.Write(fog.FogStartScalar);
            Write(fog.FogColor);
            _writer.Write(fog.UwFogEnd);
            _writer.Write(fog.UwFogStartScalar);
            Write(fog.UwFogColor);
        }

        internal void Write(CWMODoodadSet doodadSet)
        {
            _writer.Write(doodadSet.StartIndex);
            _writer.Write(doodadSet.Count);
        }

        internal void Write(CWMOPortalRef portalRef)
        {
            _writer.Write(portalRef.PortalIndex);
            _writer.Write(portalRef.GroupIndex);
            _writer.Write(portalRef.Side);
        }

        internal void Write(CWMOPortal portal)
        {
            _writer.Write(portal.StartVertex);
            _writer.Write(portal.VertexCount);
            Write(portal.PlaneNormal);
            _writer.Write(portal.PlaneDistance);
        }

        internal void Write(CWMOAmbientVolume volume)
        {
            Write(volume.Position);
            _writer.Write(volume.Start);
            _writer.Write(volume.End);
            Write(volume.Color1);
            Write(volume.Color2);
            Write(volume.Color3);
            _writer.Write(volume.Flags);
            _writer.Write(volume.DoodadSetId);
        }

        internal void Write(CWMOGroup group)
        {
            _writer.Write(group.FileDataID);
            _writer.Write(group.Lod);
            _writer.Write(group.Flags);
            Write(group.BoundingBoxMin);
            Write(group.BoundingBoxMax);
            _writer.Write(group.PortalsOffset);
            _writer.Write(group.PortalCount);
            _writer.Write(group.TransBatchCount);
            _writer.Write(group.IntBatchCount);
            _writer.Write(group.ExtBatchCount);
            _writer.Write(group.UnknownBatchCount);
            _writer.Write(group.FogIndices[0]);
            _writer.Write(group.FogIndices[1]);
            _writer.Write(group.FogIndices[2]);
            _writer.Write(group.FogIndices[3]);
            _writer.Write(group.GroupLiquid);
            _writer.Write(group.GroupId);
            _writer.Write(group.Flags2);
            _writer.Write(group.SplitGroupindex);
            _writer.Write(group.NextSplitChildIndex);
            Write(group.HeaderReplacementColor);
            WriteArray(group.Indices, _writer.Write);
            WriteArray(group.LiquidData, Write);
            WriteArray(group.BspIndices, _writer.Write);
            WriteArray(group.BspNodes, Write);
            WriteArray(group.Vertices, Write);
            WriteArray(group.Normals, Write);
            WriteArray(group.UVList, Write);
            WriteArray(group.VertexColors, Write);
            WriteArray(group.Batches, Write);
            WriteArray(group.DoodadReferences, _writer.Write);
        }

        internal void Write(CWMOLiquid liquid)
        {
            Write(liquid.LiquidVertices);
            Write(liquid.LiquidTiles);
            Write(liquid.Position);
            _writer.Write(liquid.MaterialId);
            WriteArray(liquid.Vertices, Write);
            WriteArray(liquid.Tiles, Write);
        }

        internal void Write(CWMOBspNode node)
        {
            _writer.Write(node.Flags);
            _writer.Write(node.NegChild);
            _writer.Write(node.PosChild);
            _writer.Write(node.Faces);
            _writer.Write(node.FaceStart);
            _writer.Write(node.PlaneDistance);
        }

        internal void Write(CWMOBatch batch)
        {
            _writer.Write(batch.MaterialId);
            _writer.Write(batch.StartIndex);
            _writer.Write(batch.IndexCount);
            _writer.Write(batch.FirstVertex);
            _writer.Write(batch.LastVertex);
        }

        internal void Write(CWMOLiquidVertex vertex)
        {
            _writer.Write(vertex.Data);
            _writer.Write(vertex.Height);
        }

        internal void Write(CWMOLiquidTile tile)
        {
            _writer.Write(tile.LegacyLiquidType);
            _writer.Write(tile.Fishable);
            _writer.Write(tile.Shared);
        }


        #region GetOutputSizes
        private static int GetOutputSize(CWMOFile file) {
            return 42 +
                4 + file.Materials.Sum(GetOutputSize) +
                4 + file.GroupInfo.Sum(GetOutputSize) +
                4 + file.DoodadDefs.Sum(GetOutputSize) +
                4 + file.DoodadIds.Length * 4 +
                4 + file.Fogs.Sum(GetOutputSize) +
                4 + file.DoodadSets.Sum(GetOutputSize) +
                4 + file.PortalRefs.Sum(GetOutputSize) +
                4 + file.Portals.Sum(GetOutputSize) +
                4 + file.GlobalAmbientVolumes.Sum(GetOutputSize) +
                4 + file.AmbientVolumes.Sum(GetOutputSize) +
                4 + file.PortalVertices.Length * 12 +
                4 + file.Groups.Sum(GetOutputSize);
        }

        private static int GetOutputSize(CWMOMaterial material)
        {
            return 64;
        }

        private static int GetOutputSize(CWMOGroupInfo group)
        {
            return 28;
        }

        private static int GetOutputSize(CWMODoodadDef doodadDef)
        {
            return 44;
        }

        private static int GetOutputSize(CWMOFog fog)
        {
            return 48;
        }

        private static int GetOutputSize(CWMODoodadSet doodadSet)
        {
            return 8;
        }

        private static int GetOutputSize(CWMOPortalRef portalRef)
        {
            return 6;
        }

        private static int GetOutputSize(CWMOPortal portal)
        {
            return 20;
        }

        private static int GetOutputSize(CWMOAmbientVolume volume)
        {
            return 38;
        }

        private static int GetOutputSize(CWMOGroup group)
        {
            return 68 +
                4 + group.Indices.Length * 2 +
                4 + group.LiquidData.Sum(GetOutputSize) +
                4 + group.BspIndices.Length * 2 +
                4 + group.BspNodes.Sum(GetOutputSize) +
                4 + group.Vertices.Length * 12 +
                4 + group.Normals.Length * 12 +
                4 + group.UVList.Length * 8 +
                4 + group.VertexColors.Length * 4 +
                4 + group.Batches.Sum(GetOutputSize) +
                4 + group.DoodadReferences.Length * 2;
        }

        private static int GetOutputSize(CWMOLiquid liquid)
        {
            return 30 +
                4 + liquid.Tiles.Sum(GetOutputSize) +
                4 + liquid.Vertices.Sum(GetOutputSize);
        }

        private static int GetOutputSize(CWMOBspNode node)
        {
            return 16;
        }

        private static int GetOutputSize(CWMOBatch batch)
        {
            return 12;
        }

        private static int GetOutputSize(CWMOLiquidVertex vertex)
        {
            return 8;
        }

        private static int GetOutputSize(CWMOLiquidTile tile)
        {
            return 3;
        }
        #endregion
    }
}

using WoWFileFormats.WMO;

namespace Server.CM2
{
    public class CWMOConverter : BaseCompressedConverter
    {
        public static CWMOFile Convert(WMORootFile wmoFile)
        {
            return new CWMOFile()
            {
                AmbientColor = Convert(wmoFile.AmbientColor),
                AmbientVolumes = wmoFile.AmbientVolumes.Select(Convert).ToArray(),
                DoodadDefs = wmoFile.DoodadDefList.Select(Convert).ToArray(),
                DoodadSets = wmoFile.DoodadSetList.Select(Convert).ToArray(),
                DoodadIds = wmoFile.DoodadIdList.ToArray(),
                FileDataID = wmoFile.FileDataID,
                Flags = (short) wmoFile.Flags,
                Fogs = wmoFile.FogList.Select(Convert).ToArray(),
                GlobalAmbientVolumes = wmoFile.GlobalAmbientVolumes.Select(Convert).ToArray(),
                GroupInfo = wmoFile.GroupInfoList.Select(Convert).ToArray(),
                Groups = wmoFile.GroupFiles.Select(Convert).ToArray(),
                Materials = wmoFile.MaterialList.Select(Convert).ToArray(),
                MaxBoundingBox = Convert(wmoFile.BoundingBox.Max),
                MinBoundingBox = Convert(wmoFile.BoundingBox.Min),
                PortalRefs = wmoFile.PortalRefList.Select(Convert).ToArray(),
                Portals = wmoFile.PortalList.Select(Convert).ToArray(),
                PortalVertices = wmoFile.PortalVertexList.Select(Convert).ToArray(),
                SkyboxFileId = wmoFile.SkyboxFileId,
                WMOId = wmoFile.WMOId,
            };
        }

        public static CWMOGroup Convert(WMOGroupFile groupFile)
        {
            return new CWMOGroup()
            {
                Batches = groupFile.Batches.Select(Convert).ToArray(),
                BoundingBoxMin = Convert(groupFile.BoundingBox.Min),
                BoundingBoxMax = Convert(groupFile.BoundingBox.Max),
                BspIndices = groupFile.BspIndices.ToArray(),
                BspNodes = groupFile.BspNodes.Select(Convert).ToArray(),
                DoodadReferences = groupFile.DoodadReferences.ToArray(),
                ExtBatchCount = groupFile.ExtBatchCount,
                FileDataID = groupFile.FileDataID,
                Flags = (uint) groupFile.Flags,
                Flags2 = (uint) groupFile.Flags2,
                FogIndices = groupFile.FogIndices,
                GroupId = groupFile.GroupId,
                GroupLiquid = groupFile.GroupLiquid,
                HeaderReplacementColor = Convert(groupFile.HeaderReplacementColor),
                Indices = groupFile.Indices.ToArray(),
                IntBatchCount = groupFile.IntBatchCount,
                LiquidData = groupFile.LiquidData.Select(Convert).ToArray(),
                NextSplitChildIndex = groupFile.NextSplitChildIndex,
                Normals = groupFile.Normals.Select(Convert).ToArray(),
                PortalCount = groupFile.PortalCount,
                PortalsOffset = groupFile.PortalsOffset,
                SplitGroupindex = groupFile.SplitGroupindex,
                TransBatchCount = groupFile.TransBatchCount,
                UnknownBatchCount = groupFile.UnknownBatchCount,
                UVList = groupFile.UVList.Select(Convert).ToArray(),
                VertexColors = groupFile.VertexColors.Select(Convert).ToArray(),
                Vertices = groupFile.Vertices.Select(Convert).ToArray()
            };
        }

        public static CWMODoodadDef Convert(WMODoodadDef doodadDef)
        {
            return new CWMODoodadDef
            {
                Flags = (uint)doodadDef.Flags,
                Position = Convert(doodadDef.Position),
                Rotation = Convert(doodadDef.Rotation),
                Scale = doodadDef.Scale,
                Color = Convert(doodadDef.Color)
            };
        }

        public static CWMODoodadSet Convert(WMODoodadSet doodadSet)
        {
            return new CWMODoodadSet()
            {
                StartIndex = doodadSet.StartIndex,
                Count = doodadSet.Count
            };
        }

        public static CWMOMaterial Convert(WMOMaterial material)
        {
            return new CWMOMaterial()
            {
                Flags = (uint)material.Flags,
                Shader = (uint)material.Shader,
                BlendMode = material.BlendMode,
                Texture1 = material.Texture1,
                Texture2 = material.Texture2,
                Texture3 = material.Texture3,
                SidnColor = Convert(material.SidnColor),
                FrameSidnColor = Convert(material.FrameSidnColor),
                DiffColor = Convert(material.DiffColor),
                GroundTypeId = material.GroundTypeId,
                Color2 = material.Color2,
                Flags2 = material.Flags2,
                RunTimeData = material.RunTimeData,
            };
        }

        public static CWMOGroupInfo Convert(WMOGroupInfo group)
        {
            return new CWMOGroupInfo()
            {
                Flags = (uint)group.Flags,
                MinBoundingBox = Convert(group.BoundingBox.Min),
                MaxBoundingBox = Convert(group.BoundingBox.Max)
            };
        }

        public static CWMOLodInfo Convert(WMOLodInfo info)
        {
            return new CWMOLodInfo()
            {
                Flags2 = (uint)info.Flags2,
                LodIndex = info.LodIndex
            };
        }

        public static CWMOFog Convert(WMOFog fog)
        {
            return new CWMOFog()
            {
                Flags = (uint)fog.Flags,
                Position = Convert(fog.Position),
                SmallerRadius = fog.SmallerRadius,
                LargerRadius = fog.LargerRadius,
                FogEnd = fog.FogEnd,
                FogStartScalar = fog.FogStartScalar,
                FogColor = Convert(fog.FogColor),
                UwFogEnd = fog.UwFogEnd,
                UwFogStartScalar = fog.UwFogStartScalar,
                UwFogColor = Convert(fog.UwFogColor),
            };
        }

        public static CWMOPortal Convert(WMOPortal portal)
        {
            return new CWMOPortal()
            {
                StartVertex = portal.StartVertex,
                VertexCount = portal.VertexCount,
                PlaneNormal = Convert(portal.Plane.Normal),
                PlaneDistance = portal.Plane.Distance
            };
        }

        public static CWMOPortalRef Convert(WMOPortalRef portalRef)
        {
            return new CWMOPortalRef()
            {
                PortalIndex = portalRef.PortalIndex,
                GroupIndex = portalRef.GroupIndex,
                Side = portalRef.Side,
            };
        }

        public static CWMOAmbientVolume Convert(WMOAmbientVolume volume)
        {
            return new CWMOAmbientVolume()
            {
                Position = Convert(volume.Position),
                Start = volume.Start,
                End = volume.End,
                Color1 = Convert(volume.Color1),
                Color2 = Convert(volume.Color2),
                Color3 = Convert(volume.Color3),
                Flags = volume.Flags,
                DoodadSetId = volume.DoodadSetId,
            };
        }

        public static CWMOLiquidVertex Convert(WMOLiquidVertex vertex)
        {
            return new CWMOLiquidVertex()
            {
                Data = vertex.Data,
                Height = vertex.Height,
            };
        }

        public static CWMOLiquidTile Convert(WMOLiquidTile tile)
        {
            return new CWMOLiquidTile()
            {
                Fishable = tile.Fishable,
                LegacyLiquidType = tile.LegacyLiquidType,
                Shared = tile.Shared,
            };
        }

        public static CWMOLiquid Convert(WMOLiquid liquid)
        {
            return new CWMOLiquid()
            {
                LiquidVertices = Convert(liquid.LiquidVertices),
                LiquidTiles = Convert(liquid.LiquidTiles),
                Position = Convert(liquid.Position),
                MaterialId = liquid.MaterialId,
                Vertices = liquid.Vertices.Select(Convert).ToArray(),
                Tiles = liquid.Tiles.Select(Convert).ToArray()
            };
        }

        public static CWMOBspNode Convert(WMOBspNode node) 
        {
            return new CWMOBspNode()
            {
                Flags = (ushort)node.Flags,
                NegChild = node.NegChild,
                PosChild = node.PosChild,
                Faces = node.Faces,
                FaceStart = node.FaceStart,
                PlaneDistance = node.PlaneDistance
            };
        }

        public static CWMOBatch Convert(WMOBatch batch)
        {
            return new CWMOBatch()
            {
                StartIndex = batch.StartIndex,
                IndexCount = batch.IndexCount,
                FirstVertex = batch.FirstVertex,
                LastVertex = batch.LastVertex,
                MaterialId = batch.UseMaterialIdLarge > 0 ? batch.MaterialIdLarge : batch.MaterialId,
            };
        }
    }
}

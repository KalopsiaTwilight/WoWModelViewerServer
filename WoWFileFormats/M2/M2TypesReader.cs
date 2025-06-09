using System.Text;
using WoWFileFormats.Common;

namespace WoWFileFormats.M2
{
    public abstract class M2TypesReader : BaseWoWReader
    {
        internal long _chunkOffSet;
        internal long chunkSize;
        internal M2Sequence[] _sequences = [];
        internal ushort[] _sequenceLookup = [];

        internal M2TypesReader(Stream inputStream) : base(inputStream)
        {
        }
       
        internal M2TextureUnit ReadM2TextureUnit()
        {
            return new M2TextureUnit
            {
                Flags = _reader.ReadByte(),
                Priority = _reader.ReadByte(),
                ShaderId = _reader.ReadUInt16(),
                SkinSectionIndex = _reader.ReadUInt16(),
                Flags2 = _reader.ReadUInt16(),
                ColorIndex = _reader.ReadInt16(),
                MaterialIndex = _reader.ReadUInt16(),
                MaterialLayer = _reader.ReadUInt16(),
                TextureCount = _reader.ReadUInt16(),
                TextureComboIndex = _reader.ReadUInt16(),
                TextureCoordComboIndex = _reader.ReadUInt16(),
                TextureWeightComboIndex = _reader.ReadUInt16(),
                TextureTransformComboIndex = _reader.ReadUInt16(),
            };
        }

        internal M2SubMesh ReadM2SubMesh()
        {
            return new M2SubMesh()
            {
                SubmeshId = _reader.ReadUInt16(),
                Level = _reader.ReadUInt16(),
                VertexStart = _reader.ReadUInt16(),
                VertexCount = _reader.ReadUInt16(),
                TriangleStart = _reader.ReadUInt16(),
                TriangleCount = _reader.ReadUInt16(),
                BoneCount = _reader.ReadUInt16(),
                BoneStart = _reader.ReadUInt16(),
                BoneInfluences = _reader.ReadUInt16(),
                CenterBoneIndex = _reader.ReadUInt16(),
                CenterPosition = ReadC3Vector(),
                SortCenterPosition = ReadC3Vector(),
                SortRadius = _reader.ReadSingle()
            };
        }

        internal M2Light ReadM2Light()
        {
            return new M2Light()
            {
                Type = _reader.ReadUInt16(),
                Bone = _reader.ReadInt16(),
                Position = ReadC3Vector(),
                AmbientColor = ReadM2Track(ReadC3Vector),
                AmbientIntensity = ReadM2Track(_reader.ReadSingle),
                DiffuseColor = ReadM2Track(ReadC3Vector),
                DiffuseIntensity = ReadM2Track(_reader.ReadSingle),
                AttenuationStart = ReadM2Track(_reader.ReadSingle),
                AttenuationEnd = ReadM2Track(_reader.ReadSingle),
                Visibility = ReadM2Track(_reader.ReadByte),
            };
        }

        internal M2Attachment ReadM2Attachment()
        {
            return new M2Attachment()
            {
                Id = _reader.ReadInt32(),
                Bone = _reader.ReadUInt16(),
                Unknown = _reader.ReadUInt16(),
                Position = ReadC3Vector(),
                AnimateAttached = ReadM2Track(_reader.ReadByte)
            };
        }

        internal M2TextureTransform ReadM2TextureTransform()
        {
            return new M2TextureTransform()
            {
                Translation = ReadM2Track(ReadC3Vector),
                Rotation = ReadM2Track(ReadQuat32),
                Scale = ReadM2Track(ReadC3Vector),
            };
        }

        internal M2TextureWeight ReadM2TextureWeight()
        {
            return new M2TextureWeight()
            {
                Weight = ReadM2Track(ReadFixed16),
            };
        }

        internal M2Color ReadM2Color()
        {
            return new M2Color()
            {
                Color = ReadM2Track(ReadC3Vector),
                Alpha = ReadM2Track(ReadFixed16),
            };
        }

        internal M2CompBone ReadM2Bone()
        {
            return new M2CompBone()
            {
                KeyBoneId = _reader.ReadInt32(),
                Flags = (M2CompBoneFlags)_reader.ReadUInt32(),
                ParentBone = _reader.ReadInt16(),
                SubmeshId = _reader.ReadInt16(),
                BoneNameCRC = _reader.ReadUInt32(),
                Translation = ReadM2Track(ReadC3Vector),
                Rotation = ReadM2Track(ReadQuat16),
                Scale = ReadM2Track(ReadC3Vector),
                Pivot = ReadC3Vector()
            };
        }

        internal M2ExtendedParticle ReadM2Particle()
        {
            return new M2ExtendedParticle()
            {
                ZSource = _reader.ReadSingle(),
                ColorMult = _reader.ReadSingle(),
                AlphaMult = _reader.ReadSingle(),
                AlphaCutoff = ReadM2LocalTrack(ReadFixed16)
            };
        }

        internal M2ParticleEmitter ReadM2ParticleEmitter()
        {
            var result = new M2ParticleEmitter()
            {
                ParticleId = _reader.ReadInt32(),
                Flags = (M2ParticleFlags)_reader.ReadUInt32(),
                Position = ReadC3Vector(),
                Bone = _reader.ReadInt16(),
                Texture = _reader.ReadInt16(),
                GeometryModelFilename = ReadM2String(),
                RecursionModelFilename = ReadM2String(),
                BlendingType = _reader.ReadByte(),
                EmitterType = _reader.ReadByte(),
                ParticleColorIndex = _reader.ReadUInt16(),
                MultiTextureParamX = (ReadFixedPoint8(2, 5), ReadFixedPoint8(2, 5)),
                TextureTileRotation = _reader.ReadUInt16(),
                TextureDimensionsRows = _reader.ReadUInt16(),
                TextureDimensionsColumns = _reader.ReadUInt16(),
                EmissionSpeed = ReadM2Track(_reader.ReadSingle),
                SpeedVariation = ReadM2Track(_reader.ReadSingle),
                VerticalRange = ReadM2Track(_reader.ReadSingle),
                HorizontalRange = ReadM2Track(_reader.ReadSingle),
            };
            result.Gravity = ReadM2Track(() => ReadM2ParticleEmitterGravity(result.Flags));
            result.Lifespan = ReadM2Track(_reader.ReadSingle);
            result.LifespanVary = _reader.ReadSingle();
            result.EmissionRate = ReadM2Track(_reader.ReadSingle);
            result.EmissionRateVary = _reader.ReadSingle();
            result.EmissionAreaLength = ReadM2Track(_reader.ReadSingle);
            result.EmissionAreaWidth = ReadM2Track(_reader.ReadSingle);
            result.ZSource = ReadM2Track(_reader.ReadSingle);
            result.ColorTrack = ReadM2LocalTrack(ReadC3Vector);
            result.AlphaTrack = ReadM2LocalTrack(ReadFixed16);
            result.ScaleTrack = ReadM2LocalTrack(ReadC2Vector);
            result.ScaleVary = ReadC2Vector();
            result.HeadCellTrack = ReadM2LocalTrack(_reader.ReadUInt16);
            result.TailCellTrack = ReadM2LocalTrack(_reader.ReadUInt16);
            result.TailLength = _reader.ReadSingle();
            result.TwinkleSpeed = _reader.ReadSingle();
            result.TwinklePercent = _reader.ReadSingle();
            result.TwinkleScale = ReadCRange();
            result.BurstMultiplier = _reader.ReadSingle();
            result.Drag = _reader.ReadSingle();
            result.BaseSpin = _reader.ReadSingle();
            result.BaseSpinVary = _reader.ReadSingle();
            result.Spin = _reader.ReadSingle();
            result.SpinVary = _reader.ReadSingle();
            result.Tumble = ReadM2Box();
            result.WindVector = ReadC3Vector();
            result.WindTime = _reader.ReadSingle();
            result.FollowSpeed1 = _reader.ReadSingle();
            result.FollowScale1 = _reader.ReadSingle();
            result.FollowSpeed2 = _reader.ReadSingle();
            result.FollowScale2 = _reader.ReadSingle();
            result.SplinePoints = ReadM2Array(ReadC3Vector);
            result.EnabledIn = ReadM2Track(_reader.ReadByte);
            result.MultiTextureParam0 = (ReadM2ParticleMultiTextureParameter(), ReadM2ParticleMultiTextureParameter());
            result.MultiTextureParam1 = (ReadM2ParticleMultiTextureParameter(), ReadM2ParticleMultiTextureParameter());
            return result;
        }

        internal C3Vector ReadM2ParticleEmitterGravity(M2ParticleFlags flags)
        {
            if (!flags.HasFlag(M2ParticleFlags.GravityValuesAreCompressedVectors))
            {
                return new C3Vector
                {
                    X = 0,
                    Y = 0,
                    Z = -_reader.ReadSingle()
                };
            }

            var dir = new C3Vector()
            {
                X = _reader.ReadSByte(),
                Y = _reader.ReadSByte(),
                Z = 0
            };
            dir = dir.Scale(1.0f / 128.0f);
            var z = (float)Math.Sqrt(1 - dir.Dot(dir));
            var mag = _reader.ReadInt16() * 0.04238648f;
            if (mag < 0)
            {
                z = -z;
                mag = -mag;
            }
            dir.Z = z;
            dir = dir.Scale(mag);
            return dir;
        }

        internal M2ParticleMultiTextureParameter ReadM2ParticleMultiTextureParameter()
        {
            return new M2ParticleMultiTextureParameter
            {
                X = ReadFixedPoint16(6, 9),
                Y = ReadFixedPoint16(6, 9),
            };
        }

        internal M2Ribbon ReadM2RibbonEmitter()
        {
            return new M2Ribbon()
            {
                RibbonId = _reader.ReadInt32(),
                BoneIndex = _reader.ReadInt32(),
                Position = ReadC3Vector(),
                TextureIndices = ReadM2Array(_reader.ReadInt16),
                MaterialIndices = ReadM2Array(_reader.ReadInt16),
                ColorTrack = ReadM2Track(ReadC3Vector),
                AlphaTrack = ReadM2Track(ReadFixed16),
                HeightAboveTrack = ReadM2Track(_reader.ReadSingle),
                HeightBelowTrack = ReadM2Track(_reader.ReadSingle),
                EdgesPerSecond = _reader.ReadSingle(),
                EdgeLifetime = _reader.ReadSingle(),
                Gravity = _reader.ReadSingle(),
                TextureRows = _reader.ReadInt16(),
                TextureCols = _reader.ReadInt16(),
                TexSlotTrack = ReadM2Track(_reader.ReadUInt16),
                VisibilityTrack = ReadM2Track(_reader.ReadByte),
                PriorityPlane = _reader.ReadInt16(),
                RibbonColorIndex = _reader.ReadByte(),
                TextureTransformLookupIndex = _reader.ReadByte()
            };
        }

        internal M2Camera ReadM2Camera()
        {
            return new M2Camera()
            {
                Type = _reader.ReadUInt32(),
                FarClip = _reader.ReadSingle(),
                NearClip = _reader.ReadSingle(),
                Positions = ReadM2Track(() => ReadM2SplineKey(ReadC3Vector)),
                PositionBase = ReadC3Vector(),
                TargetPosition = ReadM2Track(() => ReadM2SplineKey(ReadC3Vector)),
                TargetPositionBase = ReadC3Vector(),
                Roll = ReadM2Track(() => ReadM2SplineKey(_reader.ReadSingle)),
                FOV = ReadM2Track(() => ReadM2SplineKey(_reader.ReadSingle))
            };
        }

        internal M2Event ReadM2Event()
        {
            return new M2Event()
            {
                Identifier = _reader.ReadUInt32(),
                Data = _reader.ReadUInt32(),
                Bone = _reader.ReadUInt32(),
                Position = ReadC3Vector(),
                Enabled = ReadM2TrackBase()
            };
        }

        internal M2Material ReadM2Material()
        {
            return new M2Material()
            {
                Flags = (M2MaterialFlags)_reader.ReadUInt16(),
                BlendingMode = _reader.ReadUInt16(),
            };
        }

        internal M2Texture ReadM2Texture()
        {
            return new M2Texture()
            {
                Type = (M2TextureType)_reader.ReadUInt32(),
                Flags = (M2TextureFlags)_reader.ReadUInt32(),
                Filename = ReadM2String()
            };
        }

        internal M2Vertex ReadM2Vertex()
        {
            return new M2Vertex
            {
                Position = ReadC3Vector(),
                BoneWeights = _reader.ReadBytes(4),
                BoneIndices = _reader.ReadBytes(4),
                Normal = ReadC3Vector(),
                TexCoords = (ReadC2Vector(), ReadC2Vector())
            };
        }

        internal M2Loop ReadM2Loop()
        {
            return new M2Loop()
            {
                Timestamp = _reader.ReadUInt32(),
            };
        }

        internal M2Box ReadM2Box()
        {
            return new M2Box()
            {
                ModelRotationSpeedMin = ReadC3Vector(),
                ModelRotationSpeedMax = ReadC3Vector()
            };
        }

        internal M2LocalTrack<T> ReadM2LocalTrack<T>(Func<T> deserializeTFn)
        {
            return new M2LocalTrack<T>()
            {
                Keys = ReadM2Array(_reader.ReadInt16),
                Values = ReadM2Array(deserializeTFn),
            };
        }

        internal M2SplineKey<T> ReadM2SplineKey<T>(Func<T> deserializeTFn) where T : struct
        {
            return new M2SplineKey<T>()
            {
                Value = deserializeTFn(),
                InTan = deserializeTFn(),
                OutTan = deserializeTFn(),
            };
        }

        internal M2Track<T> ReadM2Track<T>(Func<T> deserializeTFn)
        {
            var result = new M2Track<T>();
            result.InterpolationType = _reader.ReadInt16();
            result.GlobalSequence = _reader.ReadInt16();
            result.TimeStamps = ReadM2Array((i) =>
            {
                var anim = i < _sequences.Length ? _sequences[i] : null;
                var flags = anim?.Flags ?? 0;
                while ((flags & 0x40) > 0 && anim != _sequences[anim!.AliasNext])
                {
                    anim = _sequences[anim!.AliasNext];
                    flags = anim?.Flags ?? 0;
                }
                if ((flags & 0x20) > 0)
                {
                    return ReadM2Array(_reader.ReadInt32);
                }
                else
                {
                    result.TimeStampsOutOfSequence.Add(i, (_reader.ReadUInt32(), _reader.ReadUInt32()));
                    return [];
                }
            });
            result.Values = ReadM2Array((i) =>
            {
                var anim = i < _sequences.Length ? _sequences[i] : null;
                var flags = anim?.Flags ?? 0;
                while ((flags & 0x40) > 0 && anim != _sequences[anim!.AliasNext])
                {
                    anim = _sequences[anim!.AliasNext];
                    flags = anim?.Flags ?? 0;
                }
                if ((flags & 0x20) > 0)
                {
                    return ReadM2Array(deserializeTFn);
                }
                else
                {
                    result.ValuesOutOfSequence.Add(i, (_reader.ReadUInt32(), _reader.ReadUInt32()));
                    return [];
                }
            });
            return result;
        }

        internal M2TrackBase ReadM2TrackBase()
        {
            return new M2TrackBase
            {
                InterpolationType = _reader.ReadInt16(),
                GlobalSequence = _reader.ReadInt16(),
                TimeStamps = ReadM2Array(() => ReadM2Array(_reader.ReadInt32))
            };
        }

        internal M2Sequence ReadM2Sequence()
        {
            return new M2Sequence
            {
                Id = _reader.ReadUInt16(),
                VariationIndex = _reader.ReadUInt16(),
                Duration = _reader.ReadUInt32(),
                Movespeed = _reader.ReadSingle(),
                Flags = _reader.ReadUInt32(),
                Frequency = _reader.ReadUInt16(),
                Padding = _reader.ReadUInt16(),
                Replay = ReadM2Range(),
                BlendTimeIn = _reader.ReadUInt16(),
                BlendTimeOut = _reader.ReadUInt16(),
                Bounds = ReadM2Bounds(),
                VariationNext = _reader.ReadInt16(),
                AliasNext = _reader.ReadUInt16()
            };
        }

        internal M2Bounds ReadM2Bounds()
        {
            return new M2Bounds()
            {
                Extent = ReadCAxisAlignedBox(),
                Radius = _reader.ReadSingle()
            };
        }

        internal M2Range ReadM2Range()
        {
            return new M2Range()
            {
                Minimum = _reader.ReadUInt32(),
                Maximum = _reader.ReadUInt32(),
            };
        }


        internal T[] ReadM2Array<T>(Func<T> deserializeFn)
        {
            var arrayLength = _reader.ReadUInt32();
            var arrayOffset = _reader.ReadUInt32();

            T[] result = new T[arrayLength];

            var currentOffset = _stream.Position;
            _stream.Position = _chunkOffSet + arrayOffset;
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = deserializeFn();
            }
            _stream.Position = currentOffset;
            return result;
        }

        internal T[] ReadM2Array<T>(Func<int, T> deserializeFn)
        {
            var arrayLength = _reader.ReadUInt32();
            var arrayOffset = _reader.ReadUInt32();

            T[] result = new T[arrayLength];

            if (arrayLength == 0)
                return result;

            var currentOffset = _stream.Position;
            var arrayPos = _chunkOffSet + arrayOffset;

            if (arrayPos > _stream.Length)
            {
                throw new InvalidArrayOffsetException(arrayPos, _stream.Length); 
            }
            _stream.Position = arrayPos;
            for (var i = 0; i < arrayLength; i++)
            {
                result[i] = deserializeFn(i);
            }
            _stream.Position = currentOffset;
            return result;
        }

        internal string ReadM2String()
        {
            var stringLength = _reader.ReadUInt32();
            var stringOffset = _reader.ReadUInt32();

            var currentOffset = _stream.Position;
            _stream.Position = _chunkOffSet + stringOffset;
            var result = Encoding.UTF8.GetString(_reader.ReadBytes(checked((int)stringLength)));
            _stream.Position = currentOffset;
            return result;
        }
    }
}

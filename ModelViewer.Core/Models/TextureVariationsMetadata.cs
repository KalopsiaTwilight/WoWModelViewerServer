using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.Core.Models
{

    public enum DisplayType
    {
        Item,
        Creature
    }

    public class TextureVariation
    {
        public int DisplayId { get; set; }
        public DisplayType DisplayType { get; set; }
        public int[] TextureIds { get; set; } = [];
    }

    public class TextureVariationsMetadata
    {
        public List<TextureVariation> TextureVariations { get; set; } = [];
    }
}

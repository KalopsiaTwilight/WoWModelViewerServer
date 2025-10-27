using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelViewer.Core.Models
{

    public class TextureVariation
    {
        public int[] TextureIds { get; set; } = [];
    }

    public class TextureVariationsMetadata
    {
        public List<TextureVariation> TextureVariations { get; set; } = [];
    }
}

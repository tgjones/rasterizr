using System.IO;
using Rasterizr.Resources;

namespace Rasterizr.Toolkit.Models
{
    public delegate Texture2D TextureLoadHandler(Device device, Stream stream);
}
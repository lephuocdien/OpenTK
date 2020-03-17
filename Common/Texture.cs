using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using System.IO;

namespace Common
{
    public class Texture
    {
        public readonly int Handle;
        private string  TextuePath;

        public  string  PathTexture
        {
            get { return TextuePath; }
            set { TextuePath = value; }
        }

        public Texture(String path)
        {
            PathTexture = path;
             //generate handle texture 
             Handle = GL.GenTexture();
            //active this texture
            Use();

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            byte[] buffer = File.ReadAllBytes(path);
          //  StbImage.stbi_set_flip_vertically_on_load(1);
             ImageResult image = ImageResult.FromMemory(buffer, ColorComponents.RedGreenBlueAlpha);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        public  void reloadtexture(string texturePath)
        {
            byte[] buffer = File.ReadAllBytes(texturePath);
             StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromMemory(buffer, ColorComponents.RedGreenBlueAlpha);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
        
    }
}

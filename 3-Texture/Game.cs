using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.ES30;
using Common;
using ChitChit;
namespace _3_Texture
{
    class Game : GameWindow
    {

        ChitChitObject a;
        Shader shader, shaderColor;
        public Game(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            base.OnUpdateFrame(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
           // shader = new Shader(@"Shaders\shader.vert", @"Shaders\shader.frag");
          //  shaderColor = new Shader(@"Shaders\shaderColor.vert", @"Shaders\shaderColor.frag");
            a = new ChitChitObject("Vertices/Triangle.txt");
           //b = new ChitChitObject("Vertices/Rectangle.txt", ref shader);
           // c = new ChitChitObject("Vertices/RectangleEBO.txt");
            base.OnLoad(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
          a.Draw();
          //  b.Draw();
           // c.Draw();
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
        protected override void OnUnload(EventArgs e)
        {


           // b.DestroyAll();
          a.DestroyAll();
           // c.DestroyAll();
            base.OnUnload(e);
        }
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "LearnOpenTK"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }
        }
    }
}

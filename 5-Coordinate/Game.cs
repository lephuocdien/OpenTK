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


namespace _5_Coordinate
{
    class Game : GameWindow
    {

        ChitChitObject a;

        
        private Matrix4 _view;
        private Matrix4 _projection;
        private double _time;
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
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            _view =  Matrix4.CreateTranslation(0.0f, 0.0f, -5.0f);
            _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Width / (float)Height, 0.1f, 100.0f);
            
            a = new ChitChitObject("Vertices/RectangleEBO.txt");
           
           // c = new ChitChitObject("Vertices/pyramic.txt");

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            //  b.Draw();


            _time += 10.0 * e.Time;


            //  var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));
            //  Matrix4 model = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-55.0f));
            Matrix4 model =  Matrix4.CreateRotationY ((float)MathHelper.DegreesToRadians(_time));
            //model *= Matrix4.CreateScale(0.5f);
            //c.UpdateMVP(model, _view, _projection);
            a.UpdateMVP(model, _view, _projection);
           // c.Draw();
            a.Draw();
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
            //c.DestroyAll();
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

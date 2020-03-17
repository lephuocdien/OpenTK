using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL4;
using System.IO;
using Common;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace ChitChit
{
    public class ChitChitObject
    {
        int VAO;//Use to drall all information of object
        int VBOVertex;

        bool UseIndicate;
        int EBO;//for indicate to draw object

        bool UseColor;
        int VBOColor;

        bool UseTextCoord;
        int VBOTextCoord;
     

 
        
        List<float> vertices;
        List<int> Indicate;
        List<float> ColorList;
        List<float> TextCoord;    
        List<Texture> ArrayListTexture;
        int Amountvertex;
        List<String> ShadersList; // get full path of vertex and fragment shader
        Shader shader;
        /// 
        MonitorFile reloadvertex;
        MonitorFile reloadfragment;
        MonitorFile reloadNewTexture;
        //

        public void ReadShaders(string[] shaders)
        {
           
            foreach (var item in shaders)
            {
                if (item.Equals("//shader"))
                {
                    continue;

                }
                if (item.Equals("//end shader"))
                {

                    break;
                }
                string[] everyline = item.Split(':');
                if (everyline[0].Equals("vertex"))
                    ShadersList.Add(everyline[1]);
                if (everyline[0].Equals("fragment"))
                    ShadersList.Add(everyline[1]);
            }
            //create shader here
            shader = new Shader(ShadersList[0], ShadersList[1]);
        }
        List<string> getShader()
        {
            return ShadersList;
        }
        public void ReadHeader(string[] Header)
        {
            foreach (string item in Header)
            {
                if (item.Equals("//header"))
                {
                    continue;

                }
                if (item.Equals("//end header"))
                {

                    break;
                }
                //line 1 :ebo
                string[] ebo = item.Split(':');
                if(ebo[0].Equals("EBO"))
                    UseIndicate = bool.Parse(ebo[1]);
                //line 2 :color
                if (ebo[0].Equals("UseColor"))
                    UseColor = bool.Parse(ebo[1]);
                //line 3 : textcoord
                if (ebo[0].Equals("UseTextCoord"))
                    UseTextCoord = bool.Parse(ebo[1]);
                //line4 : normal

            }
        }
        public void ReadVertex(string[] ListVertex)
        {

            foreach (string item in ListVertex)
            {
                if (item.Equals("//vertex"))
                {
                    continue;

                }
                if (item.Equals("//end vertex"))
                {

                    break;
                }
                Amountvertex++;
                Console.WriteLine(item);
                float[] floatData = Array.ConvertAll(item.Split(' '), float.Parse);
                vertices.AddRange(floatData);
               
            }
        }
        public void ReadIndicate(string [] indicate)
        {
            foreach (var item in indicate)
            {
                if (item.Equals("//indicate"))
                {
                    continue;

                }
                if (item.Equals("//end indicate"))
                {

                    break;
                }
                int[] intData = Array.ConvertAll(item.Split(' '), int.Parse);
                Indicate.AddRange(intData);
            }
        }
        public void ReadColor(string[] color)
        {
            foreach (var item in color)
            {
                if (item.Equals("//color"))
                {
                    continue;

                }
                if (item.Equals("//end color"))
                {

                    break;
                }
                float[] floatData = Array.ConvertAll(item.Split(' '), float.Parse);
                ColorList.AddRange(floatData);
            }
        }
        public void ReadTextCoord(string[] textcoord)
        {

            
            foreach (var item in textcoord)
            {
                if (item.Equals("//textcoord"))
                {
                    continue;

                }
                if (item.Equals("//end textcoord"))
                {

                    break;
                }
                if (!item.Contains(@"//path:"))
                {
                    float[] floatData = Array.ConvertAll(item.Split(' '), float.Parse);
                    TextCoord.AddRange(floatData);
                }
                else
                {

                    
                    string[] epath = item.Split(':');

                    string path = epath[1];
                    ArrayListTexture.Add(new Texture(path));
                    string cc = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    string v = cc + "\\" + path;
                    reloadNewTexture = new MonitorFile(v);
                    reloadNewTexture.Watch();


                }
            }
            shader.Use();
            for (int i = 0; i < ArrayListTexture.Count(); i++)
            {

                switch (i)
                {
                    case 0:
                        shader.SetInt("texture1", 0);
                        break;
                    case 1:
                        shader.SetInt("texture2", 1);
                        break;
                    default:
                        break;
                }
            }
        }
       /// <summary>
       ///modify shader ealtime
       /// </summary>
        public void WatchingShader(List<string>_shader)
        {
            string cc = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string v = cc+"\\"+_shader[0];
            reloadvertex = new MonitorFile(v);
            string f = cc + "\\" + _shader[1];
            reloadfragment = new MonitorFile(f);
            reloadvertex.Watch();
            reloadfragment.Watch();
        }
        /// <summary>
        /// read all information of object (color, texcoord,shader...)
        /// </summary>
        /// <param name="VerticesPath"> Location of vertices file </param>
        public void ReadObject(string VerticesPath)
        {
            
            string VertexShaderSource;
            vertices = new List<float>();
            Indicate = new List<int>();
            ColorList = new List<float>();
            TextCoord = new List<float>();         
            ArrayListTexture = new List<Texture>();
            ShadersList = new List<string>();

            //
          
            //
            using (StreamReader reader = new StreamReader(VerticesPath, Encoding.UTF8))
            {

         
                VertexShaderSource = reader.ReadToEnd();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = VertexShaderSource.Split(stringSeparators, StringSplitOptions.None);
              
                //begin #information of object
                ReadHeader(lines.Skip(0).Take(Array.IndexOf(lines, "//end header")+1).ToArray());
                ReadVertex(lines.Skip(Array.IndexOf(lines, "//vertex")).Take((Array.IndexOf(lines, "//end vertex") - Array.IndexOf(lines, "//vertex"))+1 ).ToArray());
                //get shader
                ReadShaders(lines.Skip(Array.IndexOf(lines, "//shader")).Take((Array.IndexOf(lines, "//end shader") - Array.IndexOf(lines, "//vertex")) + 1).ToArray());
                    //watching shader here
                    WatchingShader(getShader());
                    //
                if (UseIndicate)
                {
                    //Read indicate
                    ReadIndicate(lines.Skip(Array.IndexOf(lines, "//indicate")).Take((Array.IndexOf(lines, "//end indicate") - Array.IndexOf(lines, "//indicate")) + 1).ToArray());
                }
                if(UseColor)
                {
                    ReadColor(lines.Skip(Array.IndexOf(lines, "//color")).Take((Array.IndexOf(lines, "//end color") - Array.IndexOf(lines, "//color")) + 1).ToArray());
                }
                if (UseTextCoord)
                {

                    ReadTextCoord(lines.Skip(Array.IndexOf(lines, "//textcoord")).Take((Array.IndexOf(lines, "//end textcoord") - Array.IndexOf(lines, "//textcoord")) + 1).ToArray());
                }
                //end  #information of object


            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vbo"></param>
        /// <param name="array"></param>
        /// <param name="layout">position :0, Color :1 ,TexCooerd:2,Normal:3</param>
        /// <param name="maxvertex"></param>
        public void InitVBO(ref int vbo,List<float>array, int layout, int maxvertex)
        {
            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, array.ToArray<float>().Length * sizeof(float), array.ToArray<float>(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(layout, maxvertex, VertexAttribPointerType.Float, false, maxvertex * sizeof(float), 0);
            GL.EnableVertexAttribArray(layout);
        }      
        
        /// <summary>
        /// rule for shader 
        ///layout (location = 0) Position
        ///layout (location = 1) Color
        /// 
        /// </summary>
        /// <param name="VerticesPath"></param>
        public ChitChitObject(string VerticesPath)
        {
           
             //init amount of vertex of object
             Amountvertex = 0;
            UseColor = false;
            UseTextCoord = false;
            UseIndicate = false;
            ///Read vertice, all information in this file
            ReadObject(VerticesPath);
            
            //Generate VAO      
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            InitVBO(ref VBOVertex, vertices, 0,3);
            if (UseIndicate)
            {
                EBO = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indicate.ToArray<int>().Length * sizeof(uint), Indicate.ToArray<int>(), BufferUsageHint.StaticDraw);
            }
            if(UseColor)
            {
                InitVBO(ref VBOColor, ColorList, 1,3);
            }
            if(UseTextCoord)
            {
                InitVBO(ref VBOTextCoord, TextCoord, 2, 2);
            }


        }      
        public int GetVAO()
        {
            return VAO;
        }
        public void UpdateMVP(Matrix4 model, Matrix4 view, Matrix4 projective)
        {
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projective);

        }
        public async void Draw()
        {
           
            if (reloadvertex.FileChange || reloadfragment.FileChange)
            {
                shader.Dispose();
                Console.WriteLine("Pre combine shader");
                shader = new Shader(ShadersList[0], ShadersList[1]);
                if (reloadfragment.FileChange == true)
                    reloadfragment.FileChange = false;
                if (reloadvertex.FileChange == true)
                    reloadvertex.FileChange = false;
            }
            if (UseTextCoord)
            {
                if (reloadNewTexture.FileChange)
                {
                    Texture a = ArrayListTexture[0];
                    a.reloadtexture(a.PathTexture);
                    reloadNewTexture.FileChange = false;
                }
            }
            shader.Use();
            GL.BindVertexArray(GetVAO());
            if (UseTextCoord)
            {
                for( int i =0; i < ArrayListTexture.Count();i++)
                {
                    switch (i)
                    {
                        case 0:
                           
                            ArrayListTexture[0].Use(TextureUnit.Texture0);
                            break;
                        case 1:
                            ArrayListTexture[1].Use(TextureUnit.Texture1);
                            break;
                        default:
                            break;
                    }
                  
                }
            }
            if (UseIndicate)
            {
                GL.DrawElements(PrimitiveType.Triangles, Indicate.ToArray<int>().Length, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, Amountvertex);
            }
        }

       public  void DestroyAll()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
           
            GL.DeleteVertexArray(VAO);
            GL.DeleteBuffer(VBOVertex);
            shader.Dispose();           
        }



    }
}

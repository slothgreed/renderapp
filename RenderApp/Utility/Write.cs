using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace RenderApp.Utility
{
    /// <summary>
    /// コンソールに出力するクラス
    /// </summary>
    public static class CWrite
    {
        /// <summary>
        /// 4次元行列
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="name"></param>
        public static void Matrix4(Matrix4 matrix,string name = "")
        {
            Console.WriteLine(name);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    
                    Console.Write(CCalc.Round(matrix[i, j]) + ":");
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 3次元行列
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="name"></param>
        public static void Matrix3(Matrix3 matrix, string name = "")
        {
            Console.WriteLine(name);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {

                    Console.Write(CCalc.Round(matrix[i, j]) + ":");
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        /// <summary>
        /// 3次元座標値出力
        /// </summary>
        public static void XYZ(float x,float y,float z = 0,string name = "")
        {
            Console.WriteLine(name + " X:" + x.ToString() + " Y:" + y.ToString() + " Z:" + z.ToString());
        }
        /// <summary>
        /// 2Dベクトル出力
        /// </summary>
        public static void Vector(Vector2 vector,string name = "")
        {
            Console.WriteLine(name + " X:" + vector.X.ToString() + " Y:" + vector.Y.ToString());
        }
        /// <summary>
        /// 3Dベクトル出力
        /// </summary>
        public static void Vector(Vector3 vector,string name = "")
        {
            
            Console.WriteLine(name + " X:" + CCalc.Round(vector.X).ToString() + " Y:" + CCalc.Round(vector.Y).ToString() + " Z:" + CCalc.Round(vector.Z).ToString());
        }

        /// <summary>
        /// 値の出力
        /// </summary>
        public static void Value(float value,string name = "")
        {
            Console.WriteLine(name + "Value:" + value);
        }

        /// <summary>
        /// 値の出力
        /// </summary>
        public static void Value(string value, string name = "")
        {
            Console.WriteLine(name + "Value:" + value);
        }


    }
}

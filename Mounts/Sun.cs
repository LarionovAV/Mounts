// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
using System;

namespace Mounts
{
    class Sun
    {
        private static int rotAngle = 5; //угол поворота солнца
        /*
         * Позиция и параметры свечения Солнца
         * */
        public static float[] Position {get; set;}
        public static float[] Diffuse { get; set; }
        public static float[] Ambient { get; set; }
        public static float[] Specular { get; set; }

        public static void Init(
            float[] pos, float[] diff, float[] amb, float[] spec
        )
        {
            Position = pos;
            Diffuse = diff;
            Ambient = amb;
            Specular = spec;
        }
        public static void Init() {
            Position = new float[] { 0.0f, 0.0f, -600.0f, 0.0f };
            Diffuse = new float[] { 0.9f, 0.9f, 0.9f, 1.0f };
            Specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };
            Ambient = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };
        }
        /*
         * Движение Солнца - вращение вокруг оси Х
         * */
        public static void Move() {
            float tmpZ = Position[2];
            float tmpY = Position[1];
            double angle = Math.PI * rotAngle / 180;
            Position[1] = (float)(Math.Cos(angle) * tmpY - Math.Sin(angle) * tmpZ);
            Position[2] = (float)(Math.Sin(angle) * tmpY + Math.Cos(angle) * tmpZ);
        }

        public static string ToString() {
            return Position[0].ToString() + "; " + Position[1].ToString() + "; " + Position[2].ToString();
        }
    }
}

// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
using System;

namespace Mounts
{
    class Camera
    {
        //угол поворота камеры в градусах
        public static float rotCamDegree = 1.5f;                       
        //Позиция камеры
        public static double X { get; set; }
        public static double Y { get; set; }
        public static double Z { get; set; }

        //Угол обзора
        public static float ViewAngle { get; set; }

        //Направление обзора
        public static double ViewX { get; set; }
        public static double ViewY { get; set; }
        public static double ViewZ { get; set; }

        //Верх камеры
        public static double UpX { get; set; }
        public static double UpY { get; set; }
        public static double UpZ { get; set; }

        public static void Init(
            float angle = 60.0f,
            double x = 0, double y = 0, double z = 300,
            double vx = 75, double vy = 75, double vz = 255,
            double upx = 0, double upy = 0, int upz = 300)
        {
            ViewAngle = angle;
            X = x;
            Y = y;
            Z = z;
            UpX = upx;
            UpY = upy;
            UpZ = upz;
            ViewX = vx;
            ViewY = vy;
            ViewZ = vz;
        }

        public static void CamRotate()
        {
            double tmpX = ViewX;
            double tmpY = ViewY;
            double angle = Math.PI * rotCamDegree / 180;
            ViewX = tmpX * Math.Cos(angle) - tmpY * Math.Sin(angle);
            ViewY = tmpX * Math.Sin(angle) + tmpY * Math.Cos(angle);
        }
    }
}

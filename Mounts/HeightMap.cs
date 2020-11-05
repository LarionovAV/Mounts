// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System.Drawing;

namespace Mounts
{
    class HeightMap
    {
        private Bitmap perlinNoiseImage;
        public int[,] Map { get; private set; }
        public HeightMap(string fileName)
        {
            perlinNoiseImage = new Bitmap(fileName);
            CreateMap();
            perlinNoiseImage = null;
        }
        /*
         * Считывание карты высот
         * Чем светлее пиксель, тем выше точка
         * */
        private void CreateMap() {
            Map = new int[perlinNoiseImage.Width, perlinNoiseImage.Height];
            for (int y = 0; y < perlinNoiseImage.Height; y++) {
                for (int x = 0; x < perlinNoiseImage.Width; x++) {
                    Map[y, x] = perlinNoiseImage.GetPixel(x, y).R; // т.к. карта выполнена в серых тонах, достаточно взять цвет по одному каналу
                }
            }
        }
    }
}

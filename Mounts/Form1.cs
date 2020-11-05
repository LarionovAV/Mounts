// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com
using System;
using System.Drawing;
using System.Windows.Forms;
using Mounts;
using SharpGL.SceneGraph.Assets;

namespace SharpGL
{
    public partial class MountsForm : Form
    {
        const int STEP = 10;                        //шаг между точками карты
       
        readonly float textureBitX;                 //единичный отрехок текстуры по X
        readonly float textureBitY;                 //единичный отрехок текстуры по Y
        bool textureChange = false;                 //флаг смены текстуры
        HeightMap hMap;                             //карта высот
        Texture texture = new Texture();            //текстура

        /*
         * Конструктор
         * */
        public MountsForm()
        {
            InitializeComponent();
            
            this.KeyPreview = true;                         //Включимвозможность обрабатывать прерывания от клавиатуры
            textureBitX = 1.0f / hMap.Map.GetLength(1);     //Расчитаем единичные отрезки тексуры
            textureBitY = 1.0f / hMap.Map.GetLength(0);
        }


        private void openGLControl1_OpenGLInitialized(object sender, EventArgs e)
        {
            OpenGL gl = this.openGLControl1.OpenGL;         //Сохраним контекст для отрисовки
            Camera.Init();                                  //Проинициализируем камеру стандартными настройками
            Sun.Init();                                     //Проинициализируем Солнце стандартными настройками 
            hMap = new HeightMap("Map.png");                //Получим карту высот из файла, сожержащего шум Перлина
            /*
             * Настроим свет
             * */
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, Sun.Ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, Sun.Diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, Sun.Specular);
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_TWO_SIDE, OpenGL.GL_TRUE);
            gl.Enable(OpenGL.GL_LIGHTING);
            gl.Enable(OpenGL.GL_LIGHT0);
            gl.ShadeModel(OpenGL.GL_SMOOTH);

            gl.ClearColor(
                1.0f * Color.SkyBlue.R / 255,
                1.0f * Color.SkyBlue.G / 255,
                1.0f * Color.SkyBlue.B / 255, 
                0
            );


            gl.Enable(OpenGL.GL_DEPTH_TEST);
            /*
             * Включим обработку текстур
             * */
            gl.Enable(OpenGL.GL_TEXTURE_2D);
            texture.Create(gl, new Bitmap("Rock.jpg"));     //зададим текстуру (по стандарту - камень)

            gl.MatrixMode(OpenGL.GL_PROJECTION);            //установим матрицу проекции

            //  Единичная матрица для последующих преобразований
            gl.LoadIdentity();

            //  Преобразование
            gl.Perspective(
                60.0f, 
                (double)Width / (double)Height, 
                0.01, 
                Math.Max(hMap.Map.GetLength(0), hMap.Map.GetLength(1)) * STEP);

            //  Зададим модель отображения
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            texture.Bind(gl);
        }


        /*
         * Метод вызываемый при рендере
         * */
        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            Draw();
        }

        private void Draw() {
            // Создаем экземпляр
            OpenGL gl = this.openGLControl1.OpenGL;
            float[] mat_amb_def_front = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] mat_amb_def_back = new float[] { 0.0f, 0.0f, 0.0f, 1.0f };

            // Очистка экрана и буфера глубин
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            /*
             * Установим камеру
             * */
            gl.LookAt(Camera.X, Camera.Y, Camera.Z,    // Позиция самой камеры
                       Camera.ViewX, Camera.ViewY, Camera.ViewZ,     // Направление, куда мы смотрим
                       Camera.UpX, Camera.UpY, Camera.UpZ);    // Верх камеры

            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, Sun.Position);

            /*
             * Установим свойства материала
             * */
            gl.Material(OpenGL.GL_FRONT, OpenGL.GL_AMBIENT_AND_DIFFUSE, mat_amb_def_front);
            gl.Material(OpenGL.GL_BACK, OpenGL.GL_AMBIENT_AND_DIFFUSE, mat_amb_def_back);

             
            /*
             * Отрисовка карты
             * */
            for (int i = 0; i < hMap.Map.GetLength(0) - 1; i++)
            {
                int y = i - hMap.Map.GetLength(0) / 2;
                for (int j = 0; j < hMap.Map.GetLength(1) - 1; j++)
                {
                    int x = j - hMap.Map.GetLength(1) / 2;

                    /*
                     * Проходми по карте высот и для каждого квадрата 2х2 рисуем 2 треугольника,
                     * на которые накладываем текстуру
                     * */
                    gl.Begin(OpenGL.GL_TRIANGLE_STRIP);
                        gl.TexCoord(i * textureBitX, j * textureBitY);  gl.Vertex(x * STEP, y * STEP, hMap.Map[i, j]);
                        gl.TexCoord(i * textureBitX, (j + 1) * textureBitY);  gl.Vertex((x + 1) * STEP, y * STEP, hMap.Map[i, j + 1]);
                        gl.TexCoord((i + 1) * textureBitX, j * textureBitY);  gl.Vertex(x * STEP, (y + 1) * STEP, hMap.Map[i + 1, j]);
                        gl.TexCoord((i + 1) * textureBitX, (j + 1) * textureBitY);  gl.Vertex((x + 1) * STEP, (y + 1) * STEP, hMap.Map[i + 1, j + 1]);
                    gl.End();
                }
            }

            gl.Flush();


            Sun.Move();             //Сдвигаем Солнце
            Camera.CamRotate();            //Вращаем камеру
        }

        /**
         * Метод вызывается при изменении размеров окна
         * */
        private void openGLControl1_Resized(object sender, EventArgs e)
        {
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)   //при нажатии на Escape  закрываем окно
            {
                Close();
            }
            else if (e.KeyCode == Keys.Space)   //при нажатии на пробел останавливаем рендеринг (запускается при повторном нажатии)
            {
                if (openGLControl1.RenderTrigger == RenderTrigger.TimerBased)
                {
                    openGLControl1.RenderTrigger = RenderTrigger.Manual;
                }
                else
                    openGLControl1.RenderTrigger = RenderTrigger.TimerBased;
            }
            else if (e.KeyCode == Keys.Z)   //при нажатии на Z меняем направление вращения камеры
            {
                Camera.rotCamDegree *= -1;
            }
            else if (e.KeyCode == Keys.Enter)   //при нажатии на Enter меняем текстуру
            {
                textureChange = !textureChange;
                if (textureChange)
                {
                    texture.Destroy(openGLControl1.OpenGL);
                    texture.Create(openGLControl1.OpenGL, "Grass.jpg");
                }
                else
                {
                    texture.Destroy(openGLControl1.OpenGL);
                    texture.Create(openGLControl1.OpenGL, "Rock.jpg");
                }
            }
        }

        /*
         * Вращение камеры вокруг оси Y
         * */
        
    }
}
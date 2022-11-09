using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.Windows.Threading; // добавляем потоки для таймера 

namespace _1Flappy_Bird_Game
{
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTimer = new DispatcherTimer(); // создаем новый экземпляр таймера

        double score; // строчка для сохранения счета 
        int gravity = 8; // обычное число гравитации 
        bool gameOver; // новое логическое значение для проверки, закончена игра или нет
        Rect flappyBirdHitBox; // добавляем rect, для поиска расхождений 

        public MainWindow()
        {
            InitializeComponent();
// установливаем настройки по умолчанию для таймера
            gameTimer.Tick += MainEventTimer; // свяжите отметку таймера с событием игрового движка
            gameTimer.Interval = TimeSpan.FromMilliseconds(20); // установим интеревал в 20 секунд 
            StartGame();   //  функция для запуска игры

        }

        private void MainEventTimer(object sender, EventArgs e) // игровой ивент
        {
            txtScore.Content = "Score: " + score;  // счетчик счета 
                                                   // связываем изображение flappy bird с классом flappy rect
            flappyBirdHitBox = new Rect(Canvas.GetLeft(flappyBird), Canvas.GetTop(flappyBird), flappyBird.Width - 5, flappyBird.Height);
           
            Canvas.SetTop(flappyBird, Canvas.GetTop(flappyBird) + gravity);  // устанавливаем значение тяжести для изображения flappy bird 

            if (Canvas.GetTop(flappyBird) < -30 || Canvas.GetTop(flappyBird) > 460)  // if проверяет, исчезла ли птица с экрана сверху или снизу
            {
                EndGame();
            }
            // ниже приведен основной цикл, этот цикл будет проходить через каждое изображение на холсте
            // если он найдет какое-либо изображение с тегами, то будет следовать инструкциям с ними
            foreach (var x in MyCanvas.Children.OfType<Image>())
            {
                if ((string)x.Tag == "obs1" || (string)x.Tag == "obs2" || (string)x.Tag == "obs3") 
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);// если мы нашли изображение с тегом obs1, obs2 или obs3, мы переместим его влево от столба, то есть obs(1,2,3)

                    if (Canvas.GetLeft(x) < -100)  // если  труба покинет сцену, то перейдет на -100 пикселей на лево
                    {
                        Canvas.SetLeft(x, 800); // сброс столбных изображений до 800 пикселей

                        score += .5;  // добавляет 1 к счету
                    }
                    // создайте новый прямоугольник с именем pillars и привязываем к нему прямоугольник, то есть rect
                    Rect PillarHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (flappyBirdHitBox.IntersectsWith(PillarHitBox))
                    {
                        EndGame();
                    }

                    if ((string)x.Tag == "cloud")
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - 2); // если мы нашли изображение с тегом cloud,то мы постепенно перемещаем облака влево 
                    {

                        if(Canvas.GetLeft(x) < -250) //  если  труба покинет сцену, то перейдет на -250 пикселей на лево
                            {
                            Canvas.SetLeft(x, 550);  // сброс облачных изображений до 550 пикселей
                            }
                    }

                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space) // если нажат пробел то:
            {
                flappyBird.RenderTransform = new RotateTransform(-20,flappyBird.Width /2, flappyBird.Height /2); // - поворачивается изображение птицы на -20 градусов от центрального положения
                gravity = -8; // изменяем гравитацию, чтобы птичка двигалась вверх
            }

            if( e.Key == Key.R && gameOver == true)
            {
                StartGame();
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            flappyBird.RenderTransform = new RotateTransform(5, flappyBird.Width / 2, flappyBird.Height / 2); // если клавиша будет отпущена, мы изменим поворот flappy bird на 5 градусов от центра
            gravity = 8;  // меняем гравитацию на 8, чтобы птица летела вниз, а не вверх
        }
        private void StartGame() // это функция запускускает игру
        {
            MyCanvas.Focus(); // эта функция загрузит все значения по умолчанию для запуска этой игры
          
            int temp = 300; // сначала создайте целое число temp со значением 300
            score = 0; // устанавливаем значение 0
            
            gameOver = false; 
            Canvas.SetTop(flappyBird, 190);// устанавливаем верхнюю позицию flappy bird равной 190 пикселям

            foreach (var x in MyCanvas.Children.OfType<Image>())  // приведенный ниже цикл просто проверит каждое изображение в этой игре и установит их на позиции по умолчанию
            {
                if ((string)x.Tag=="obs1") // устанавливаем каналы obs1 в положение по умолчанию
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "obs2") // устанавливаем каналы obs2 в положение по умолчанию
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "obs3") // устанавливаем каналы obs3 в положение по умолчанию
                {
                    Canvas.SetLeft(x, 1100);
                }

                if((string)x.Tag=="cloud") // устанавливаем облака в положение по умолчанию
                {
                    Canvas.SetLeft(x, 300+temp);
                    temp = 800;
                }
            }

            gameTimer.Start();



        }

        private void EndGame()
        {
            gameTimer.Stop(); // мы завершаем игру и показываем текст reset game
            gameOver = true;
            txtScore.Content += "Game Over || Press to R to try again!";
        }
    }
}

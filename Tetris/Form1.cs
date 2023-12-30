using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Tetris.Tetris;

namespace Tetris
{
    public partial class Tetris : Form
    {
        public Tetris()
        {
            InitializeComponent();
        }

        List<Panel[]> Pixels;
        Dictionary<char, Coordinate[]> Shapes;
        public class Coordinate
        {
            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public int x;
            public int y;
        }
        void Rotate()
        {
            Task.Run(() => Console.Beep(900, 250));
            if (CurrentShape == 'I') //tamam
            {
                if (CurrentAngle == 0) Change("-1,+2:+0,+1:+1,+0:+2,-1");
                else if (CurrentAngle == 90) Change("+2,-2:+1,-1:+0,+0:-1,+1");
                else if (CurrentAngle == 180) Change("-2,+1:-1,+0:+0,-1:+1,-2");
                else if (CurrentAngle == 270) Change("+1,-1:-0,+0:-1,+1:-2,+2");
            }

            else if (CurrentShape == 'J') //tamam
            {
                if (CurrentAngle == 0) Change("0,+3:-1,+2:0,+1:+1,0");
                else if (CurrentAngle == 90) Change("+2,0:+1,+1:+0,+0:-1,-1");
                else if (CurrentAngle == 180) Change("+1,-2:+2,-1:+1,0:+0,+1");
                else if (CurrentAngle == 270) Change("-3,-1:-2,-2:-1,-1:0,0");
            }

            else if (CurrentShape == 'L') //tamam
            {
                if (CurrentAngle == 0) Change("+3,0:+0,+1:+1,+0:+2,-1");
                else if (CurrentAngle == 90) Change("0,-3:+1,0:+0,-1:-1,-2");
                else if (CurrentAngle == 180) Change("-3,+0:0,-1:-1,0:-2,+1");
                else if (CurrentAngle == 270) Change("0,+3:-1,0:0,+1:+1,+2");
            }

            else if (CurrentShape == 'Z') //tamam
            {
                if (CurrentAngle == 0) Change("+1,+1:+2,0:+1,-1:+2,-2");
                else if (CurrentAngle == 90) Change("+1,0:0,-1:-1,+0:-2,-1");
                else if (CurrentAngle == 180) Change("0,-1:-1,+0:0,+1:-1,+2");
                else if (CurrentAngle == 270) Change("-2,0:-1,+1:0,0:+1,+1");
            }

            else if (CurrentShape == 'S') //tamam
            {
                if (CurrentAngle == 0) Change("+2,0:+3,-1:+0,-0:+1,-1");
                else if (CurrentAngle == 90) Change("0,-1:-1,-2:+0,+1:-1,0");
                else if (CurrentAngle == 180) Change("-1,0:-2,1:1,0:0,1");
                else if (CurrentAngle == 270) Change("-1,+1:0,+2:-1,-1:0,0");
            }

            else if (CurrentShape == 'T') //tamam
            {
                if (CurrentAngle == 0) Change("+1,0:-1,0:0,-1:+1,-2");
                else if (CurrentAngle == 90) Change("+1,0:+1,+2:+0,+1:-1,0");
                else if (CurrentAngle == 180) Change("-1,-1:+1,-1:0,0:-1,+1");
                else if (CurrentAngle == 270) Change("-1,+1:-1,-1:0,0:+1,+1");
            }

        }
        private int ScoreBack;

        public int Score
        {
            get { return ScoreBack; }
            set
            {
                ScoreBack = value;
                scoreLBL.Text = $"Score: {ScoreBack}";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Score = 0;
            Shapes = new Dictionary<char, Coordinate[]>();

            Shapes.Add('I', new Coordinate[] { new Coordinate(1, 0), new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3) });
            Shapes.Add('J', new Coordinate[] { new Coordinate(0, 0), new Coordinate(1, 0), new Coordinate(1, 1), new Coordinate(1, 2) });
            Shapes.Add('L', new Coordinate[] { new Coordinate(0, 3), new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3) });
            Shapes.Add('S', new Coordinate[] { new Coordinate(0, 2), new Coordinate(0, 3), new Coordinate(1, 1), new Coordinate(1, 2) });
            Shapes.Add('O', new Coordinate[] { new Coordinate(0, 1), new Coordinate(0, 2), new Coordinate(1, 1), new Coordinate(1, 2) });
            Shapes.Add('T', new Coordinate[] { new Coordinate(0, 2), new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3) });
            Shapes.Add('Z', new Coordinate[] { new Coordinate(0, 1), new Coordinate(0, 2), new Coordinate(1, 2), new Coordinate(1, 3) });


            Pixels = new List<Panel[]>();
            int xpay = 40;
            int ypay = 40;
            int pixel_shape_x = 25;
            int pixel_shape_y = 25;
            for (int x = 0; x < 20; x++)
            {
                Pixels.Add(new Panel[10]);
                for (int y = 0; y < 10; y++)
                {
                    Pixels[x][y] = new Panel();
                    Pixels[x][y].Size = new Size(pixel_shape_x, pixel_shape_y);
                    Pixels[x][y].BorderStyle = BorderStyle.FixedSingle;
                    Pixels[x][y].Location = new Point(y*pixel_shape_y+ypay, x*pixel_shape_x+xpay);
                    Pixels[x][y].BackColor = Color.Black;
                    this.Controls.Add(Pixels[x][y]);
                }
            }

            timer1.Start();
            RandomSummon();
        }
        Coordinate[] SummonedCoordinates;
        bool isGameStarted = false;
        void Summon(char Shape, Color clr)
        {
            CurrentShape = Shape;
            int soldanpay = 3;
            CurrentAngle = 0;

            SummonedCoordinates = (Coordinate[])Shapes[Shape].Clone();

            int a = 0;
            foreach (Coordinate coordinate in Shapes[Shape])
            {
                ChangePixelsColor(Pixels[coordinate.x][coordinate.y+ soldanpay], clr);
                SummonedCoordinates[a] = new Coordinate(coordinate.x, coordinate.y+soldanpay);
                a++;
            }

        }

        void Gravity()
        {
            foreach (Coordinate coordinate in SummonedCoordinates) { DeletePixel(coordinate.x, coordinate.y); }
            foreach (Coordinate coordinate in SummonedCoordinates) { ChangePixelsColor(Pixels[++coordinate.x][coordinate.y], CurrentColor); }

        }


        void DeletePixel(int x, int y)
        {
            Pixels[x][y].BackColor = Color.Black;
        }



        void ChangePixelsColor(Panel pixel, Color color)
        {
            pixel.BackColor = color;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!timer1.Enabled) return;
            if (e.KeyCode == Keys.Space)
            {
                while (!GameOverBool && !newSummoned) { timer1_Tick(sender, e); } // oyun bitmediyse ve yeni yaratılmadığı sürece zamanı ileri sar
                newSummoned= false;
            }
            else if (e.KeyCode == Keys.R)
            {
                Restart();
            }
            else if (e.KeyCode == Keys.Up) { Rotate(); }
            else if (e.KeyCode == Keys.Down) { timer1_Tick(sender, e); }
            else if (e.KeyCode == Keys.Left) 
       
            {
                foreach (Coordinate coordinate in SummonedCoordinates) { if (coordinate.y-1 < 0) return; }
                foreach (Coordinate coordinate in SummonedCoordinates) { if (isPixelColored(Pixels[coordinate.x][coordinate.y-1]) && !Contains(SummonedCoordinates, coordinate.x, coordinate.y-1)) return; } //solda renkli pixel var mı
                foreach (Coordinate coordinate in SummonedCoordinates) { DeletePixel(coordinate.x, coordinate.y); }
                foreach (Coordinate coordinate in SummonedCoordinates) { ChangePixelsColor(Pixels[coordinate.x][--coordinate.y], CurrentColor); }
            }
            else if (e.KeyCode == Keys.Right)
            {
                foreach (Coordinate coordinate in SummonedCoordinates) { if (coordinate.y+1 > 9) return; }
                foreach (Coordinate coordinate in SummonedCoordinates) { if (isPixelColored(Pixels[coordinate.x][coordinate.y+1]) && !Contains(SummonedCoordinates, coordinate.x, coordinate.y+1)) return; } //sağda renkli pixel var mı
                foreach (Coordinate coordinate in SummonedCoordinates) { DeletePixel(coordinate.x, coordinate.y); }
                foreach (Coordinate coordinate in SummonedCoordinates) { ChangePixelsColor(Pixels[coordinate.x][++coordinate.y], CurrentColor); }
            }
        }

        bool isPixelColored(Panel pnl)
        {
            return (pnl.BackColor != Color.Black) ? true : false;
        }
        void DrawShape(Coordinate[] lst)
        {
            foreach (Coordinate coordinate in lst)
            {
                ChangePixelsColor(Pixels[coordinate.x][coordinate.y], CurrentColor);
            }
        }
        bool GameOverBool = false;
        void DeleteShape(Coordinate[] lst)
        {
            foreach (var pixel in lst)
            {
                DeletePixel(pixel.x, pixel.y);
            }
        }

        Coordinate[] CloneCoordinateArray(Coordinate[] array)
        {
            Coordinate[] newCoordinateArray = new Coordinate[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newCoordinateArray[i] = new Coordinate(array[i].x, array[i].y);
            }
            return newCoordinateArray;
        }
        void Change(string changes) // changes : "-1,+2:+0,+1:+1,+0:+2,-1"
        {
            bool tryChange = true;
            Coordinate[] backup = CloneCoordinateArray(SummonedCoordinates);

            for (int i = 0; i <= 3; i++)
            {
                SummonedCoordinates[i].x+= int.Parse(changes.Split(':')[i].Split(',')[0]);
                SummonedCoordinates[i].y+= int.Parse(changes.Split(':')[i].Split(',')[1]);
            }

            foreach (Coordinate coordinate in SummonedCoordinates)
                if (coordinate.x > 19 ||  coordinate.x < 0 || coordinate.y > 9 || coordinate.y < 0 || !Contains(backup, coordinate.x, coordinate.y) && isPixelColored(Pixels[coordinate.x][coordinate.y]))
                {
                    SummonedCoordinates = CloneCoordinateArray(backup);
                    tryChange = false;
                    break;
                }
            if (tryChange)
            {
                DeleteShape(backup);
                DrawShape(SummonedCoordinates);
                CurrentAngle += 90;
                CurrentAngle%=360;
            }
        }




        void CheckGameOver()
        {
            if (newSummoned==false) return;
            for (int i = 0; i <= 1; i++) //bu satırlar aralığında ve
            {
                for (int j = 3; j <= 6; j++) //bu sütunlar aralığındaki
                {
                    if (Pixels[i][j].BackColor != Color.Black) GameOver(); //pixeller renkli ise oyunu bitir
                }
            }
        }


        Color CurrentColor = Color.Red;
        char CurrentShape = 'X';
        int CurrentAngle = 0;

        Random Random = new Random();
        void RandomSummon()
        {
            int rnd = Random.Next(0, 7);
            char[] chars = new char[] { 'O', 'I', 'L', 'J', 'S', 'T', 'Z' };
            Color[] colors = new Color[] { Color.FromArgb(255,255,0), Color.FromArgb(64, 224, 208), Color.FromArgb(255, 165, 0), Color.FromArgb(0, 0, 255), Color.FromArgb(0, 255, 0), Color.FromArgb(255, 110, 255), Color.FromArgb(255, 255, 0,0) };
            CurrentColor = colors[rnd];
            CurrentShape = chars[rnd];
            Summon(CurrentShape, CurrentColor);
        }
        bool isJustStarted()
        {
            foreach (var pixel in Pixels[19])
            {
                if (pixel.BackColor != Color.Black) return false;
            }
            return true;
        }
        void CarryLine(int line, int to)
        {
            int index = 0;
            foreach (var pixel in Pixels[line])
            {
                Pixels[to][index].BackColor = pixel.BackColor;
                index++;
            }
        }

        void DeleteLine(int line, bool effect)
        {
            if (effect)
                foreach (var pixel in Pixels[line])
                {
                    Task.Run(() => Console.Beep(600, 50));
                    pixel.BackColor = Color.White;
                    System.Threading.Thread.Sleep(20);
                    this.Refresh();
                }
            foreach (var pixel in Pixels[line])
            {
                pixel.BackColor = Color.Black;
                if (effect)
                {
                    Task.Run(() => Console.Beep(600, 100));
                    System.Threading.Thread.Sleep(20);
                    this.Refresh();
                }
            }
        }
        //SAĞ GİDERKEN BLOK KONTROL ET

        bool newSummoned = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Coordinate coord in SummonedCoordinates)
            {
                if (coord.x == 19 || (Pixels[coord.x+1][coord.y].BackColor != Color.Black) && !Contains(SummonedCoordinates, coord.x+1, coord.y))
                //en alt satırdaysak ya da altta herhangi bir şey varsa yeni summon ya da oyun bitir
                {
                    for (int x = 0; x < 20; x++) //her satırın
                    {
                        for (int y = 0; y < 10; y++) //her sütununu kontrol et
                        {
                            if (Pixels[x][y].BackColor == Color.Black) break; // boş pixel varsa break
                            if (y == 9) //tüm satır kontrol edildiyse
                            {
                                int tempx = x;
                                timer1.Stop();
                                DeleteLine(tempx, true);
                                timer1.Start();
                                Score += 100;
                                CarryLine(tempx-1, tempx);
                                tempx--;
                                while (tempx-1>=0)
                                {
                                    DeleteLine(tempx, false); //satırı sil
                                    CarryLine(tempx-1, tempx); //tüm satırları bir alta taşı
                                    tempx--;
                                }
                            }
                        }
                    }
                    // Summon('S', Color.Red);
                    Score += 10;
                    Task.Run(() => Console.Beep(300, 300));
                    RandomSummon();
                    CheckGameOver();
                    newSummoned = true;
                    return;
                }
            }
            Gravity();
            newSummoned = false;
        }
        void GameOver()
        {
            
            if (isJustStarted()) return;
            if (GameOverBool) return; //messagebox spamını önlemek için
            Task.Run(() => Console.Beep(900, 500));
            Task.Run(() => Console.Beep(600, 500));
            timer1.Stop();
            GameOverBool = true;
            MessageBox.Show($"Skor:{Score}", "Oyun Bitti.",MessageBoxButtons.OK,MessageBoxIcon.Error);
            Restart();
        }
        void Restart()
        {
            Score = 0;
            timer1.Start(); //oyun bitince duran timerı geri başlat
            for (int i = 0; i < Pixels.Count; i++)
            {
                for (int j = 0; j < Pixels[i].Length; j++)
                {
                    Pixels[i][j].BackColor = Color.Black; //tüm pixelleri siyah yap
                }
            }
           
            RandomSummon();
            GameOverBool = false;
        }
        bool Contains(Coordinate[] clist, int x, int y)
        {
            foreach (Coordinate coord in clist)
            {
                if (coord.x == x && coord.y == y) return true;
            }
            return false;
        }


    }
}

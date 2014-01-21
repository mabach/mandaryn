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

namespace Labirynt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public class Pole
        {
            public bool jestSciezka = false;
            public int odleglosc = 5000;
            public int X;
            public int Y;
            public bool[] kierunki = new bool[4];
            public char hanzi = (char)16000;

            public bool jestPolaczenie(Kierunek kierunek)
            {
                return kierunki[(int)kierunek];
            }

            public void polacz(Kierunek kierunek)
            {
                kierunki[(int)kierunek] = true;
            }

            public Pole(int x, int y)
            {
                X = x;
                Y = y;
                for (int k = 0; k < 4; ++k)
                {
                    kierunki[k] = false;
                }
            }

        }

        public enum Kierunek { Polnoc = 0, Wschod, Zachod, Poludnie };

        string[] lines;
        string line;

        static int szerokoscLabiryntu = 16;
        static int wysokoscLabiryntu = 12;

        Point player = new Point();

        BitmapImage source = new BitmapImage(new Uri("/Labirynt;component/Images/panda.png", UriKind.RelativeOrAbsolute));

        Pole wyjscie;

        Random random = new Random();

        // false - sciana
        // true - sciezka
        // liczba - odleglosc od poczatku (do generowania labiryntu)
        Pole[,] PolaLabiryntu = new Pole[szerokoscLabiryntu, wysokoscLabiryntu];

        public void znajdzWyjscie()
        {
            wyjscie = PolaLabiryntu[0,0];
            for (int x = 0; x < szerokoscLabiryntu; ++x)
            {
                for (int y = 0; y < wysokoscLabiryntu; ++y)
                {
                    Pole p = PolaLabiryntu[x, y];
                    if (p.odleglosc > wyjscie.odleglosc && 
                        (p.kierunki[0] == true || 
                        p.kierunki[1] == true || 
                        p.kierunki[2] == true || 
                        p.kierunki[3] == true))
                    {
                        wyjscie = PolaLabiryntu[x, y];
                    }
                }
            }
        }

        public void wczytajSlownik()
        {
            lines = System.IO.File.ReadAllLines(@"c:\users\macias\documents\visual studio 2012\Projects\Labirynt\Labirynt\Data\ccc.txt");
        }

        public MainWindow()
        {
            InitializeComponent();
            inicjalizujPolaLabiryntu();
            wygenerujLabiryntDFS();
            znajdzWyjscie();
            wczytajSlownik();

            

            for (int x = 0; x < szerokoscLabiryntu; ++x)
            {
                for (int y = 0; y < wysokoscLabiryntu; ++y)
                {
                    int rand = random.Next() % lines.Length;
                    char[] separator = new char[] { ',' };
                    string[] splitLine = lines[rand].Split(separator);
                    PolaLabiryntu[x, y].hanzi = splitLine[1][0];
                }
            }
        }

        private bool poleWGranicach(int x, int y)
        {
            if (x >= 0 &&
                x < szerokoscLabiryntu &&
                y >= 0 &&
                y < wysokoscLabiryntu)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Pole sasiadPola(int x, int y, Kierunek kierunek)
        {
            int xSasiada = 0;
            int ySasiada = 0;

            if (kierunek == Kierunek.Polnoc)
            {
                xSasiada = x;
                ySasiada = y - 1;
            }
            else if (kierunek == Kierunek.Wschod)
            {
                xSasiada = x + 1;
                ySasiada = y;
            }
            else if (kierunek == Kierunek.Poludnie)
            {
                xSasiada = x;
                ySasiada = y + 1;
            }
            else if (kierunek == Kierunek.Zachod)
            {
                xSasiada = x - 1;
                ySasiada = y;
            }

            if ( poleWGranicach(xSasiada, ySasiada) )
            {
                return PolaLabiryntu[xSasiada, ySasiada];
            }

            return null;
        }

        private void wygenerujLabiryntDFS()
        {
            int x = 0;
            int y = 0;
            int wartoscPoprzednika = 0;
            PolaLabiryntu[0, 0].odleglosc = 0;
            PolaLabiryntu[0, 0].jestSciezka = true;

            do
            {
                List<int> pozostaleKierunki = new List<int>();
                pozostaleKierunki.Add(0);
                pozostaleKierunki.Add(1);
                pozostaleKierunki.Add(2);
                pozostaleKierunki.Add(3);

                while (pozostaleKierunki.Count > 0)
                {
                    int wylosowanaPozycjaListy = random.Next() % pozostaleKierunki.Count;
                    Kierunek kierunek = (Kierunek)(pozostaleKierunki[wylosowanaPozycjaListy]);
                    pozostaleKierunki.RemoveAt(wylosowanaPozycjaListy);

                    Pole sasiad = sasiadPola(x, y, kierunek);
                    if (sasiad != null &&
                        !sasiad.jestSciezka &&
                        (PolaLabiryntu[x, y].jestPolaczenie(kierunek) == false) &&
                        (sasiad.odleglosc > PolaLabiryntu[x, y].odleglosc))
                    {
                        PolaLabiryntu[x, y].polacz(kierunek);
                        x = sasiad.X;
                        y = sasiad.Y;
                        sasiad.polacz((Kierunek)(3 - (int)kierunek));
                        PolaLabiryntu[x, y].jestSciezka = true;
                        PolaLabiryntu[x, y].odleglosc = wartoscPoprzednika + 1;
                        wartoscPoprzednika = PolaLabiryntu[x, y].odleglosc;

                        break;
                    }
                }

                if (pozostaleKierunki.Count == 0)
                {
                    for (int k = 0; k < 4; ++k)
                    {
                        Pole sasiad = sasiadPola(x, y, (Kierunek)k);
                        if (sasiad != null && 
                            sasiad.jestSciezka && 
                            PolaLabiryntu[x, y].jestPolaczenie((Kierunek)k) && 
                            (sasiad.odleglosc < PolaLabiryntu[x, y].odleglosc))
                        {
                            x = sasiad.X;
                            y = sasiad.Y;
                            wartoscPoprzednika = sasiad.odleglosc;
                            break;
                        }
                    }
                }

            }
            while ((PolaLabiryntu[x, y].X != 0) ||
                (PolaLabiryntu[x, y].Y != 0));
        }

        private void inicjalizujPolaLabiryntu()
        {
            for (int x = 0; x < szerokoscLabiryntu; ++x)
            {
                for (int y = 0; y < wysokoscLabiryntu; ++y)
                {
                    PolaLabiryntu[x, y] = new Pole(x,y);
                }
            }
        }

        private void canvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            rysujWszystko();
        }

        private void rysujWszystko()
        {
            rysujLabirynt();

            Image panda = new Image();

            Point[] punktyNaroznikiPola = new Point[4];

            //polnoc-zach
            punktyNaroznikiPola[0] = transformToCanvasCoordinates(new Point(player.X, player.Y));
            //polnoc-wsch
            punktyNaroznikiPola[1] = transformToCanvasCoordinates(new Point(player.X + 1, player.Y));
            //polud-wsch
            punktyNaroznikiPola[2] = transformToCanvasCoordinates(new Point(player.X + 1, player.Y + 1));
            //polud-zach
            punktyNaroznikiPola[3] = transformToCanvasCoordinates(new Point(player.X, player.Y + 1));

            panda.Source = source;
            panda.Width = punktyNaroznikiPola[2].X - punktyNaroznikiPola[0].X;
            panda.Height = punktyNaroznikiPola[2].Y - punktyNaroznikiPola[0].Y;
            panda.Stretch = Stretch.Uniform;

            canvas.Children.Add(panda);
            Canvas.SetLeft(panda, punktyNaroznikiPola[0].X);
            Canvas.SetTop(panda, punktyNaroznikiPola[0].Y);
        }

        public double gruboscLinii()
        {
            int min = Math.Min((int)canvas.ActualWidth, (int)canvas.ActualHeight);
            return min / 360.0;
        }

        private Point transformToCanvasCoordinates(Point coordinates)
        {
            //Stosunki wymiarow do rozpietosci wartosci.
            coordinates.X /= (szerokoscLabiryntu);
            coordinates.Y /= (wysokoscLabiryntu);

            //Przeskaluj wartosci do skali wymiarow Canvas
            coordinates.X *= canvas.ActualWidth;
            coordinates.Y *= canvas.ActualHeight;

            return coordinates;
        }

        private void rysujSciane(int x, int y, Kierunek kierunek)
        {
            if (PolaLabiryntu[x,y].jestPolaczenie(kierunek) == false) 
            {
                Point[] punktyNaroznikiPola = new Point[4];

                //polnoc-zach
                punktyNaroznikiPola[0] = transformToCanvasCoordinates(new Point(x, y));
                //polnoc-wsch
                punktyNaroznikiPola[1] = transformToCanvasCoordinates(new Point(x + 1, y));
                //polud-wsch
                punktyNaroznikiPola[2] = transformToCanvasCoordinates(new Point(x + 1, y + 1));
                //polud-zach
                punktyNaroznikiPola[3] = transformToCanvasCoordinates(new Point(x, y + 1));

                SolidColorBrush brush = new SolidColorBrush(Colors.Black);
                Line line = new Line();
                line.Stroke = brush;

                //line.Clip = new RectangleGeometry(getGraphBounds());
                line.StrokeThickness = gruboscLinii();
                line.StrokeStartLineCap = PenLineCap.Round;
                line.StrokeEndLineCap = PenLineCap.Round;

                if (kierunek == Kierunek.Polnoc)
                {
                    line.X1 = punktyNaroznikiPola[0].X;
                    line.Y1 = punktyNaroznikiPola[0].Y;
                    line.X2 = punktyNaroznikiPola[1].X;
                    line.Y2 = punktyNaroznikiPola[1].Y;
                }
                else if (kierunek == Kierunek.Wschod)
                {
                    line.X1 = punktyNaroznikiPola[1].X;
                    line.Y1 = punktyNaroznikiPola[1].Y;
                    line.X2 = punktyNaroznikiPola[2].X;
                    line.Y2 = punktyNaroznikiPola[2].Y;
                }
                else if (kierunek == Kierunek.Poludnie)
                {
                    line.X1 = punktyNaroznikiPola[2].X;
                    line.Y1 = punktyNaroznikiPola[2].Y;
                    line.X2 = punktyNaroznikiPola[3].X;
                    line.Y2 = punktyNaroznikiPola[3].Y;
                }
                else if (kierunek == Kierunek.Zachod)
                {
                    line.X1 = punktyNaroznikiPola[3].X;
                    line.Y1 = punktyNaroznikiPola[3].Y;
                    line.X2 = punktyNaroznikiPola[0].X;
                    line.Y2 = punktyNaroznikiPola[0].Y;
                }

                canvas.Children.Add(line);
            }
        }

        private void rysujSciezke(int x, int y)
        {
            Color color = new Color();
            if (PolaLabiryntu[x, y].jestSciezka)
            {
                color = Colors.LightYellow;
            }
            else
            {
                color = Colors.Yellow;
            }

            if (x == 0 && y == 0)
            {
                color = Colors.Red;
            }

            if (x == wyjscie.X && y == wyjscie.Y)
            {
                color = Colors.LightBlue;
            }

            Point[] punktyNaroznikiPola = new Point[4];

            //polnoc-zach
            punktyNaroznikiPola[0] = transformToCanvasCoordinates(new Point(x, y));
            //polnoc-wsch
            punktyNaroznikiPola[1] = transformToCanvasCoordinates(new Point(x + 1, y));
            //polud-wsch
            punktyNaroznikiPola[2] = transformToCanvasCoordinates(new Point(x + 1, y + 1));
            //polud-zach
            punktyNaroznikiPola[3] = transformToCanvasCoordinates(new Point(x, y + 1));

            Rectangle rect = new Rectangle
            {
                Stroke = new SolidColorBrush(color),
                Fill = new SolidColorBrush(color),
                StrokeThickness = 1
            };

            rect.Width = punktyNaroznikiPola[2].X - punktyNaroznikiPola[0].X;
            rect.Height = punktyNaroznikiPola[2].Y - punktyNaroznikiPola[0].Y;

            if ((x == 0 && y == 0) || (x == wyjscie.X && y == wyjscie.Y) )
            {
                Canvas.SetLeft(rect, punktyNaroznikiPola[0].X);
                Canvas.SetTop(rect, punktyNaroznikiPola[0].Y);
                canvas.Children.Add(rect);
            }      
        }


        private void wypiszHanziPola(int x, int y)
        {
            Pole p = PolaLabiryntu[x,y];

            if ((int)p.hanzi != 16000)
            {
                Label znak = new Label();
                //znak.IsReadOnly = true;
                znak.Content = "" + p.hanzi;
                //znak.Text = "" + p.odleglosc;
                znak.Background = Brushes.Transparent;
                znak.Foreground = new SolidColorBrush(Colors.Black);
                znak.BorderBrush = Brushes.Transparent; ;

                Point pos = transformToCanvasCoordinates(new Point(x, y));
                Canvas.SetLeft(znak, pos.X);
                Canvas.SetTop(znak, pos.Y);

                canvas.Children.Add(znak);
            }
        }

        private void rysujLabirynt()
        {
            for (int x = 0; x < szerokoscLabiryntu; ++x)
            {
                for (int y = 0; y < wysokoscLabiryntu; ++y)
                {
                    rysujSciezke(x,y);
                    rysujSciane(x, y, Kierunek.Polnoc);
                    rysujSciane(x, y, Kierunek.Zachod);

                    if (y == (wysokoscLabiryntu - 1))
                    {
                        rysujSciane(x, y, Kierunek.Poludnie);
                    }

                    if (x == (szerokoscLabiryntu - 1))
                    {
                        rysujSciane(x, y, Kierunek.Wschod);
                    }

                    wypiszHanziPola(x, y);
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int x = (int)player.X;
            int y = (int)player.Y;

            Kierunek kierunek = new Kierunek();

            if (e.Key == Key.Up)
            {
                kierunek = Kierunek.Polnoc;
            }
            if (e.Key == Key.Right)
            {
                kierunek = Kierunek.Wschod;
            }
            if (e.Key == Key.Down)
            {
                kierunek = Kierunek.Poludnie;
            }
            if (e.Key == Key.Left)
            {
                kierunek = Kierunek.Zachod;
            }

            Pole sasiad = sasiadPola(x, y, kierunek);
            if (sasiad != null &&
                    sasiad.jestSciezka &&
                    (PolaLabiryntu[x, y].jestPolaczenie(kierunek) == true))
            {
                player.X = sasiad.X;
                player.Y = sasiad.Y;
                canvas.Children.Clear();
                rysujWszystko();
            }

            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}

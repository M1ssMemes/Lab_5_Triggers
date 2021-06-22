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
namespace lab_5
{
    /// <summary>
    /// Логика взаимодействия для JK.xaml
    /// </summary>
    public partial class JK : Window
    {
        List<double> XCoord = new List<double> { };
        List<double> YCoord = new List<double> { };

        List<int> listJ = new List<int> { 0 };
        List<int> listK = new List<int> { 0 };
        List<int> listC = new List<int> { 0 };
        List<int> listQ = new List<int> { 0 };

        Window mainWin;

        string sr = " JKCQ";
        public JK(Window main)
        {
            mainWin = main;
            InitializeComponent();
            this.Closed += new EventHandler(Window_Closed);
            PrepareCanvas();
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            mainWin.Show();
        }

        public void PrepareCanvas()
        {
            SolidColorBrush color = new SolidColorBrush();
            color.Color = Colors.Green;

            double x1 = 20;
            double x2 = Graph.Width - x1;

            var i = 0;

            for (var y = 20; y < Graph.Height; y += 70)
            {

                var line = CreateLine(x1, y, x2, y, color);
                line.StrokeDashArray = new DoubleCollection() { 5 };
                Graph.Children.Add(line);
                YCoord.Add(y);
                if (sr[i] != ' ')
                {
                    Label label = new Label();
                    label.Content = sr[i];
                    label.Margin = new Thickness(x1 - 20, y - 15, 0, 0);
                    Graph.Children.Add(label);
                }
                i++;
            }

            double y1 = 20;
            double y2 = Graph.Height - y1;

            for (var x = 20; x < Graph.Width; x += 35)
            {
                var line = CreateLine(x, y1, x, y2, color);
                line.StrokeDashArray = new DoubleCollection() { 5 };
                Graph.Children.Add(line);
                XCoord.Add(x);
            }
        }

        public Line CreateLine(double x1, double y1, double x2, double y2, SolidColorBrush color)
        {
            var line = new Line();
            line.Stroke = color;
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Tag = color.Color.ToString();
            return line;
        }

        public void DrawGraph(List<int> data, int index)
        {
            SolidColorBrush color = new SolidColorBrush();
            color.Color = Colors.Blue;

            var y1 = YCoord.ElementAt(index + 1);
            var y2 = y1 - 35;
            var prev = data.ElementAt(0);

            for (var i = 1; i < data.Count; i++)
            {
                var elem = data.ElementAt(i);
                if (prev == 0)
                {
                    var x1 = XCoord.ElementAt(i - 1);
                    var x2 = XCoord.ElementAt(i);
                    Graph.Children.Add(CreateLine(x1, y1, x2, y1, color));
                }
                else
                {
                    var x1 = XCoord.ElementAt(i - 1);
                    var x2 = XCoord.ElementAt(i);
                    Graph.Children.Add(CreateLine(x1, y2, x2, y2, color));
                }
                if (prev != elem)
                {
                    var x2 = XCoord.ElementAt(i);
                    Graph.Children.Add(CreateLine(x2, y1, x2, y2, color));
                }
                prev = elem;
            }
        }

        private void C_Click(object sender, RoutedEventArgs e)
        {
            C.Content = C.IsChecked == true ? "1" : "0";
        }

        private void K_Click(object sender, RoutedEventArgs e)
        {
            K.Content = K.IsChecked == true ? "1" : "0";
        }

        private void J_Click(object sender, RoutedEventArgs e)
        {
            J.Content = J.IsChecked == true ? "1" : "0";
        }

        public void Calculate()
        {
            if (listQ.Count() == XCoord.Count())
            {
                var lastelem = listQ.Last();

                listQ.Clear();
                listQ.Add(lastelem);

                lastelem = listJ.Last();
                listJ.Clear();
                listJ.Add(lastelem);

                lastelem = listK.Last();
                listK.Clear();
                listK.Add(lastelem);

                lastelem = listC.Last();
                listC.Clear();
                listC.Add(lastelem);
            }

            listJ.Add(J.IsChecked == true ? 1 : 0);
            listK.Add(K.IsChecked == true ? 1 : 0);
            listC.Add(C.IsChecked == true ? 1 : 0);

            if ((listC.Last() == 0) && (listC.ElementAt(listC.Count() - 2) == 1))
            {
                if ((J.IsChecked == true) && (K.IsChecked == true))
                {
                    if (listQ.Last() == 0)
                    {
                        listQ.Add(1);
                    }
                    else
                    {
                        listQ.Add(0);
                    }
                }
                else if ((J.IsChecked == false) && (K.IsChecked == false))
                {
                    listQ.Add(listQ.Last());
                }
                else if ((J.IsChecked == true) && (K.IsChecked == false))
                {
                    listQ.Add(1);
                }
                else
                {
                    listQ.Add(0);
                }

                Q.Text = listQ.Last() == 1 ? "1" : "0";
                NeQ.Text = listQ.Last() == 0 ? "1" : "0";
            }
            else
            {
                listQ.Add(listQ.Last());
            }
        }

        private void DeleteLines()
        {
            SolidColorBrush color = new SolidColorBrush();
            color.Color = Colors.Blue;

            var remove = Graph.Children.OfType<Line>().Where(i => i.Tag.ToString() == color.Color.ToString()).ToList();

            foreach (var i in remove)
            {
                Graph.Children.Remove(i);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteLines();
            Calculate();
            DrawGraph(listJ, 0);
            DrawGraph(listK, 1);
            DrawGraph(listC, 2);
            DrawGraph(listQ, 3);
        }
    }
}

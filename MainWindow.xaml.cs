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

namespace MetodoMinimosCuadrados
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private IList<Puntos> _puntos;

        public MainWindow()
        {
            InitializeComponent();

            DgIteraciones.ItemsSource = _puntos;
        }

        private double[,] ObtenerMatrizInversa(double[,] matriz)
        {
            double a, b, c, d;
            double[,] matrizInversa = new double[2, 2];

            a = matriz[0, 0];
            b = matriz[0, 1];
            c = matriz[1, 0];
            d = matriz[1, 1];

            double multiplicando = 1 / ((a * d) - (b - c));

            matriz[0, 0] = d;
            matriz[1, 1] = a;
            matriz[0, 1] = -b;
            matriz[1, 0] = -c;

            for (int i = 0; i < matrizInversa.GetLength(0); i++)
            {
                for (int j = 0; j < matrizInversa.GetLength(1); j++)
                {
                    matrizInversa[i, j] = matriz[i, j] * multiplicando;
                }
            }

            return matrizInversa;
        }

        private double[,] ObtenerMatrizMultiplicada(double[,] matrizA, double[,] matrizB)
        {
            double[,] matrizMultiplicada = new double[2, 1];

            matrizMultiplicada[0, 0] = (matrizA[0, 0] * matrizB[0, 0]) + (matrizA[0, 1] * matrizB[1, 0]);
            matrizMultiplicada[1, 0] = (matrizA[1, 0] * matrizB[0, 0]) + (matrizA[1, 1] * matrizB[1, 0]);

            return matrizMultiplicada;
        }

        private void ImprimirMatriz(double[,] matriz)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    builder.Append($"[{matriz[i, j]}] ");
                }

                builder.Append(Environment.NewLine);
            }

            MessageBox.Show(builder.ToString());
        }

        private void BtnCalcular_Click(object sender, RoutedEventArgs e)
        {
            int n = _puntos.Count();
            double sumatoriaXi = 0, sumatoriaYi = 0, sumatoriaXiCuadrado = 0, xiYi = 0;

            foreach (Puntos puntos in _puntos)
            {
                sumatoriaXi += puntos.Xi;
                sumatoriaYi += puntos.Yi;
                sumatoriaXiCuadrado += Math.Pow(puntos.Xi, 2);
                xiYi += (puntos.Xi * puntos.Yi);
            }

            double[,] matrizA = new double[2, 2] { { n, sumatoriaXi }, { sumatoriaXi, sumatoriaXiCuadrado } };
            double[,] matrizB = new double[2, 1] { { sumatoriaYi }, { xiYi } };
            double[,] matrizC = new double[2, 1];

            matrizC = ObtenerMatrizMultiplicada(ObtenerMatrizInversa(matrizA), matrizB);

            double a0 = matrizC[0, 0];
            double a1 = matrizC[1, 0];

            MessageBox.Show($"a0 = {a0}, a1 = {a1}");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            TxtValorXi.Clear();
            TxtValorYi.Clear();
            _puntos.Clear();
            TxtValorXi.Focus();
            DgIteraciones.Items.Refresh();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(TxtValorXi.Text, out double xi) && double.TryParse(TxtValorYi.Text, out double yi))
            {
                Puntos puntos = new Puntos();
                puntos.Xi = xi;
                puntos.Yi = yi;

                _puntos.Add(puntos);

                DgIteraciones.Items.Refresh();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _puntos = new List<Puntos>()
            {
                new Puntos(){Xi = 1, Yi = 0.5},
                new Puntos(){Xi = 2, Yi = 2.5},
                new Puntos(){Xi = 3, Yi = 2},
                new Puntos(){Xi = 4, Yi = 4},
                new Puntos(){Xi = 5, Yi = 3.5},
                new Puntos(){Xi = 6, Yi = 6},
                new Puntos(){Xi = 7, Yi = 5.5},
            };

            DgIteraciones.ItemsSource = _puntos;
        }
    }
}

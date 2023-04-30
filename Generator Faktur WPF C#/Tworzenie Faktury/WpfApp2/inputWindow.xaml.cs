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
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class inputWindow : Window
    {
        public inputWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Pobieranie wartości wprowadzonych przez użytkownika
                int licznik = MainWindow.incr;
                string nazwa = nazwaTb.Text;
                int ilosc = Convert.ToInt32(iloscTb.Text);
                int cena_netto = Convert.ToInt32(cenaNettoTb.Text);
                double stawka_vat = Convert.ToDouble(stawkaVatTb.Text);

                if (nazwa == "")
                {
                    throw new Exception("Nie wprowadzono nazwy produktu!");
                }

                if (ilosc <= 0 || cena_netto <= 0 || stawka_vat <= 0)
                {
                    throw new Exception("Wprowadzona wartość jest ujemna!");
                }

                if (stawka_vat > 100)
                {
                    throw new Exception("Wprowadzona stawka VAT jest większa niż 100%!");
                }

                //Obliczenia wartości
                double wartosc_netto = ilosc * cena_netto;
                double wartosc_vat = wartosc_netto * (stawka_vat / 100);
                double wartosc_brutto = wartosc_netto + wartosc_vat;

                // Utworzenie obiektu klasy Produkt z wartościami, które wprowadził użytkownik
                Produkt produkt = new Produkt(licznik, nazwa, Convert.ToString(ilosc), Convert.ToString(cena_netto), Convert.ToString(stawka_vat), Convert.ToString(wartosc_netto), Convert.ToString(wartosc_vat), Convert.ToString(wartosc_brutto));
                // Dodanie tego obiektu do tabeli
                MainWindow.env.dg.Items.Add(produkt);
                // Zwiększenie licznika o 1
                MainWindow.incr++;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Wprowadzona wartość jest niepoprawna. Wprowadź poprawną wartość.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
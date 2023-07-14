using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public static MainWindow env;
        public static int incr;
        public MainWindow()
        {
            InitializeComponent();
            env = this;
            incr = 1;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            // Utworzenie okna "inputWindow" i wyświetlenie go po kliknięciu przycisku
            inputWindow input = new inputWindow();
            input.Show();
        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // Utowrzenie okna dialogowego, gdzie użytkownik wskazuje plik
                var dialog = new Microsoft.Win32.OpenFileDialog();
                // Ustawienie domyślnej nazwy pliku i rozszerzenia
                dialog.FileName = "Nazwa pliku";
                dialog.DefaultExt = ".txt";
                // Ustawienie filtrowania dla plików, które mają widoczne dla użytkownika
                dialog.Filter = "Text documents (.txt)|*.txt";
                string path = "";

                // Wyświetlenie okna dialogowego i czekanie na reakcję użytkownika
                bool? result = dialog.ShowDialog();

                // Sprawdzenie, czy użytkownik wybrał plik i przypisanie ścieżki do zmiennej path
                if (result == true)
                {
                    path = dialog.FileName;
                }
                // Przypisanie wszystkich linii z pliku do tablicy
                string[] linie = File.ReadAllLines(path);
                int i = 0;

                // Iteracja po wszystkich liniach
                while (i < linie.Length)
                {
                    // Pobranie nazw i ich przypisanie
                    string nazwa = linie[i];
                    int ilosc = int.Parse(linie[i + 1]);
                    double cena_netto = double.Parse(linie[i + 2]);
                    double stawka_vat = double.Parse(linie[i + 3]);
                    double wartosc_netto = ilosc * cena_netto;
                    double wartosc_vat = wartosc_netto * (stawka_vat / 100);
                    double wartosc_brutto = wartosc_netto + wartosc_vat;
                    Produkt produkt = new Produkt(incr, Convert.ToString(nazwa), Convert.ToString(ilosc), Convert.ToString(cena_netto), Convert.ToString(stawka_vat), Convert.ToString(wartosc_netto), Convert.ToString(wartosc_vat), Convert.ToString(wartosc_brutto));

                    // Dodanie produktu do DataGrid
                    dg.Items.Add(produkt);
                    incr++;
                    i += 4;
                    // Przeskakiwanie po pustych liniach
                    while (i < linie.Length && linie[i] == "")
                    {
                        i++;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Wystąpił błąd podczas odczytywania danych z pliku: {0}", ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Nieprawidłowe rozszerzenie pliku: {0}", ex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bt3_Click(object sender, RoutedEventArgs e)
        {
            double sum_wartosc_netto = 0;
            double sum_wartosc_brutto = 0;
            foreach (Produkt p in env.dg.Items)
            {
                sum_wartosc_netto += Convert.ToDouble(p.wartosc_netto);
                sum_wartosc_brutto += Convert.ToDouble(p.wartosc_brutto);
            }

            // add total row
            Produkt total = new Produkt(incr, "SUMA", "", "", "", Convert.ToString(sum_wartosc_netto), Convert.ToString(sum_wartosc_brutto - sum_wartosc_netto), Convert.ToString(sum_wartosc_brutto));
            env.dg.Items.Add(total);

        }

        private void bt4_Click(object sender, RoutedEventArgs e)
        {
            string sIm = sImie.Text;
            string sNa = sNazwisko.Text;
            string sN = sNIP.Text;
            string sUl = sUlica.Text;
            string sM = sMiasto.Text;

            string kIm = kImie.Text;
            string kNa = kNazwisko.Text;
            string kN = kNIP.Text;
            string kUl = kUlica.Text;
            string kM = kMiasto.Text;

            // Utowrzenie okna dialogowego, gdzie użytkownik wskazuje plik
            var dialog = new Microsoft.Win32.SaveFileDialog();
            // Ustawienie domyślnej nazwy pliku i rozszerzenia
            dialog.FileName = "Nazwa pliku";
            dialog.DefaultExt = ".pdf";
            // Ustawienie filtrowania dla plików, które mają widoczne dla użytkownika
            dialog.Filter = "PDF documents (.pdf)|*.pdf";
            string path = "";

            // Wyświetlenie okna dialogowego i czekanie na reakcję użytkownika
            bool? result = dialog.ShowDialog();

            // Sprawdzenie, czy użytkownik wybrał plik i przypisanie ścieżki do zmiennej path
            if (result == true)
            {
                path = dialog.FileName;
            }

            // Utworzenie dokumentu PDF i otwarcie go do zapisu
            Document doc = new Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.EMBEDDED);
            Font font = new Font(bf, 12);
            doc.Open();

            PdfPTable table1 = new PdfPTable(2);
            PdfPTable table = new PdfPTable(8);

            PdfPCell leftCell = new PdfPCell(new Phrase("Sprzedawca"));
            PdfPCell leftCell1 = new PdfPCell(new Phrase($"Imię: {sIm},", font));
            PdfPCell leftCell2 = new PdfPCell(new Phrase($"Nazwisko: {sNa},", font));
            PdfPCell leftCell3 = new PdfPCell(new Phrase($"NIP: {sN},", font));
            PdfPCell leftCell4 = new PdfPCell(new Phrase($"Ulica: {sUl},", font));
            PdfPCell leftCell5 = new PdfPCell(new Phrase($"Miasto: {sM}", font));

            leftCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
            leftCell.PaddingBottom = 20f;
            leftCell5.PaddingBottom = 20f;

            // Dodanie pustej komórki po środku tabeli
            PdfPCell emptyCell = new PdfPCell();
            emptyCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //for (int i = 0; i <= 1; i++)
            //{
            //table1.AddCell(emptyCell);
            //}

            PdfPCell rightCell = new PdfPCell(new Phrase("Klient"));
            PdfPCell rightCell1 = new PdfPCell(new Phrase($"Imię: {kIm},", font));
            PdfPCell rightCell2 = new PdfPCell(new Phrase($"Nazwisko: {kNa},", font));
            PdfPCell rightCell3 = new PdfPCell(new Phrase($"NIP: {kN},", font));
            PdfPCell rightCell4 = new PdfPCell(new Phrase($"Ulica: {kUl},", font));
            PdfPCell rightCell5 = new PdfPCell(new Phrase($"Miasto: {kM}", font));

            rightCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
            rightCell.PaddingBottom = 20f;
            rightCell5.PaddingBottom = 20f;

            table1.AddCell(leftCell);
            table1.AddCell(rightCell);
            table1.AddCell(leftCell1);
            table1.AddCell(rightCell1);
            table1.AddCell(leftCell2);
            table1.AddCell(rightCell2);
            table1.AddCell(leftCell3);
            table1.AddCell(rightCell3);
            table1.AddCell(leftCell4);
            table1.AddCell(rightCell4);
            table1.AddCell(leftCell5);
            table1.AddCell(rightCell5);

            table1.WidthPercentage = 100;
            // Utworzenie tabeli i dodanie wierszy z danymi produktów
            table.WidthPercentage = 100;
            table.AddCell("Lp.");
            table.AddCell("Nazwa");
            PdfPCell ilosc = new PdfPCell(new Phrase("Ilość", font));
            table.AddCell(ilosc);
            table.AddCell("Cena netto");
            table.AddCell("VAT");
            PdfPCell wart_net = new PdfPCell(new Phrase("Wartość netto", font));
            table.AddCell(wart_net);
            PdfPCell wart_vat = new PdfPCell(new Phrase("Wartość VAT", font));
            table.AddCell(wart_vat);
            PdfPCell wart_brut = new PdfPCell(new Phrase("Wartość brutto", font));
            table.AddCell(wart_brut);

            foreach (Produkt p in MainWindow.env.dg.Items)
            {
                table.AddCell(p.lp.ToString());
                table.AddCell(p.nazwa);
                table.AddCell(p.ilosc.ToString());
                table.AddCell(p.cena_netto);
                table.AddCell(p.stawka_vat + "%");
                table.AddCell(p.wartosc_netto);
                table.AddCell(p.wartosc_vat);
                table.AddCell(p.wartosc_brutto);
            }

            // Dodanie tabeli do dokumentu i zamknięcie dokumentu
            doc.Add(table1);
            doc.Add(table);
            doc.Close();

        }

        private void bt5_Click(object sender, RoutedEventArgs e)
        {
            sImie.Clear();
            sNazwisko.Clear();
            sNIP.Clear();
            sUlica.Clear();
            sMiasto.Clear();

            kImie.Clear();
            kNazwisko.Clear();
            kNIP.Clear();
            kUlica.Clear();
            kMiasto.Clear();

            dg.Items.Clear();
        }

    }
}
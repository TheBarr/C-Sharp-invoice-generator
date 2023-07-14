using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public class Produkt
    {
        public int lp { get; set; }
        public string nazwa { get; set; }
        public string ilosc { get; set; }
        public string cena_netto { get; set; }
        public string stawka_vat { get; set; }
        public string wartosc_netto { get; set; }
        public string wartosc_vat { get; set; }
        public string wartosc_brutto { get; set; }

        public Produkt(int _lp, string _nazwa, string _ilosc, string _cena_netto, string _stawka_vat, string _wartosc_netto, string _wartosc_vat, string _wartosc_brutto)
        {
            lp = _lp;
            nazwa = _nazwa;
            ilosc = _ilosc;
            cena_netto = _cena_netto;
            stawka_vat = _stawka_vat;
            wartosc_netto = _wartosc_netto;
            wartosc_vat = _wartosc_vat;
            wartosc_brutto = _wartosc_brutto;
        }
    }
}
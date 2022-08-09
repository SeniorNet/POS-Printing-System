using POS_PrintingServer_API.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.Tring
{
   public class Tring_Invoice
    {
        public static KasaOdgovor PrintInvoice(InvoiceViewModel obj)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                if (init)
                {
                    Racun _racun = new Racun();

                    if (!string.IsNullOrEmpty(obj.partner_name))
                    {
                        Kupac kup = new Kupac();
                        kup.Naziv = obj.partner_name;
                        kup.Adresa = obj.partner_address;
                        kup.PostanskiBroj = obj.partner_postal_code;
                        kup.IDbroj = obj.partner_uin;
                        kup.Grad = obj.partner_city;
                        _racun.Kupac = kup;
                    }

                    if (obj.cash > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Gotovina, obj.cash);
                    }
                    if (obj.check > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Cek, obj.check);
                    }
                    if (obj.card > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Kartica, obj.card);
                    }
                    if (obj.virman > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Virman, obj.virman);
                    }

                    foreach (var item in obj.articles)
                    {
                        RacunStavka _stavka = new RacunStavka();
                        _stavka.Kolicina = item.quantity;
                        _stavka.Rabat = item.discount;

                        Artikal _artikal = new Artikal();
                        _artikal.Naziv = item.article_name;
                        _artikal.Sifra = item.article_code;
                        _artikal.JM = item.article_unit_id;
                        _artikal.Cijena = item.price;
                        _stavka.artikal = _artikal;
                        if (item.vat_category == "E")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.E_Opca_poreska_stopa_PDV;
                        }
                        else if (item.vat_category == "K")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.K_Poreska_stopa_PDV_za_artikle_oslobodjene_PDV;
                        }
                        else if (item.vat_category == "A")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.A_Nulta_stopa_za_neregistrirane_obveznike;
                        }
                        _racun.DodajStavkuRacuna(_stavka);
                    }
                    KasaOdgovor odgovor = printer.StampatiFiskalniRacun(_racun);
                    return odgovor;
                }

            }
            return new KasaOdgovor() { VrstaOdgovora = VrsteOdgovora.Greska, Odgovori = new List<Odgovor> { new Odgovor() { Naziv = "Greska", Vrijednost = 999 } } };
        }
        public static KasaOdgovor ReclaimInvoice(InvoiceViewModel obj)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                if (init)
                {
                    Racun _racun = new Racun();
                    _racun.BrojRacuna = obj.fiscal_number;
                    if (!string.IsNullOrEmpty(obj.partner_name))
                    {
                        Kupac kup = new Kupac();
                        kup.Naziv = obj.partner_name;
                        kup.Adresa = obj.partner_address;
                        kup.PostanskiBroj = obj.partner_postal_code;
                        kup.IDbroj = obj.partner_uin;
                        kup.Grad = obj.partner_city;
                        _racun.Kupac = kup;
                    }

                    if (obj.cash > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Gotovina, obj.cash);
                    }
                    if (obj.check > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Cek, obj.check);
                    }
                    if (obj.card > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Kartica, obj.card);
                    }
                    if (obj.virman > 0)
                    {
                        _racun.DodajVrstuPlacanja(VrstePlacanja.Virman, obj.virman);
                    }

                    foreach (var item in obj.articles)
                    {
                        RacunStavka _stavka = new RacunStavka();
                        _stavka.Kolicina = item.quantity;
                        _stavka.Rabat = item.discount;

                        Artikal _artikal = new Artikal();
                        _artikal.Naziv = item.article_name;
                        _artikal.Sifra = item.article_code;
                        _artikal.JM = item.article_unit_id;
                        _artikal.Cijena = item.price;
                        _stavka.artikal = _artikal;
                        if (item.vat_category == "E")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.E_Opca_poreska_stopa_PDV;
                        }
                        else if (item.vat_category == "K")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.K_Poreska_stopa_PDV_za_artikle_oslobodjene_PDV;
                        }
                        else if (item.vat_category == "A")
                        {
                            _artikal.Stopa = VrstePoreskihStopa.A_Nulta_stopa_za_neregistrirane_obveznike;
                        }
                        _racun.DodajStavkuRacuna(_stavka);
                    }
                    KasaOdgovor odgovor = printer.StampatiReklamiraniRacun(_racun);
                    return odgovor;
                }

            }
            return new KasaOdgovor() { VrstaOdgovora = VrsteOdgovora.Greska, Odgovori = new List<Odgovor> { new Odgovor() { Naziv = "Greska", Vrijednost = 999 } } };
        }
        public static KasaOdgovor PrintInvoiceDuplicate(InvoiceViewModel obj)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                if (init)
                {
                    KasaOdgovor odgovor = printer.StampatiDuplikatFiskalnogRacuna(int.Parse(obj.fiscal_number));
                    return odgovor;
                }

            }
            return new KasaOdgovor() { VrstaOdgovora = VrsteOdgovora.Greska, Odgovori = new List<Odgovor> { new Odgovor() { Naziv = "Greska", Vrijednost = 999 } } };
        }
        public static KasaOdgovor PrintReclaimDuplicate(InvoiceViewModel obj)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                if (init)
                {
                    KasaOdgovor odgovor = printer.StampatiDuplikatReklamiranogRacuna(int.Parse(obj.stormed_number));
                    return odgovor;
                }

            }
            
            return new KasaOdgovor() { VrstaOdgovora = VrsteOdgovora.Greska, Odgovori = new List<Odgovor> { new Odgovor() { Naziv = "Greska", Vrijednost = 999 } } };
        }
    }
}

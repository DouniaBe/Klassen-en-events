using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klassen_en_events
{
    // Enum voor Verschijningsperiode
    public enum Verschijningsperiode
    {
        Dagelijks,
        Wekelijks,
        Maandelijks
    }

    // Basisklasse Boek
    public class Boek
    {
        private string isbn;
        private string naam;
        private string uitgever;
        private decimal prijs;

        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public string Naam
        {
            get { return naam; }
            set { naam = value; }
        }

        public string Uitgever
        {
            get { return uitgever; }
            set { uitgever = value; }
        }

        public decimal Prijs
        {
            get { return prijs; }
            set
            {
                // Prijs moet tussen 5€ en 50€ liggen
                if (value >= 5 && value <= 50)
                {
                    prijs = value;
                }
                else
                {
                    throw new ArgumentException("Prijs moet tussen 5€ en 50€ liggen.");
                }
            }
        }

        public Boek(string isbn, string naam, string uitgever, decimal prijs)
        {
            ISBN = isbn;
            Naam = naam;
            Uitgever = uitgever;
            Prijs = prijs;
        }

        public override string ToString()
        {
            return $"ISBN: {ISBN}, Naam: {Naam}, Uitgever: {Uitgever}, Prijs: {Prijs:C}";
        }

        public void Lees()
        {
            Console.WriteLine($"Je leest het boek '{Naam}'.");
        }
    }

    // Afgeleide klasse Tijdschrift
    public class Tijdschrift : Boek
    {
        public Verschijningsperiode VerschijningsPeriode { get; set; }

        public Tijdschrift(string isbn, string naam, string uitgever, decimal prijs, Verschijningsperiode periode)
            : base(isbn, naam, uitgever, prijs)
        {
            VerschijningsPeriode = periode;
        }

        public override string ToString()
        {
            return base.ToString() + $", Verschijningsperiode: {VerschijningsPeriode}";
        }
    }

    // Generieke klasse Bestelling<T>
    public class Bestelling<T>
    {
        private static int volgnummerCounter = 1;

        public int Id { get; private set; }
        public T Item { get; private set; }
        public DateTime Datum { get; private set; }
        public int Aantal { get; private set; }
        public Verschijningsperiode? Periode { get; private set; }

        public Bestelling(T item, DateTime datum, int aantal, Verschijningsperiode? periode = null)
        {
            Id = volgnummerCounter++;
            Item = item;
            Datum = datum;
            Aantal = aantal;
            Periode = periode;
        }

        public Tuple<string, int, decimal> Bestel()
        {
            decimal totalePrijs = 0;

            if (Item is Boek boek)
            {
                totalePrijs = boek.Prijs * Aantal;
                Tuple<string, int, decimal> bestelInfo = new Tuple<string, int, decimal>(boek.ISBN, Aantal, totalePrijs);

                // Trigger event
                OnBoekBesteld($"Boek {boek.Naam} is besteld. ISBN: {boek.ISBN}, Aantal: {Aantal}, Totale Prijs: {totalePrijs:C}");

                return bestelInfo;
            }
            else if (Item is Tijdschrift tijdschrift)
            {
                totalePrijs = tijdschrift.Prijs * Aantal;
                Console.WriteLine($"Tijdschrift {tijdschrift.Naam} is besteld. Aantal: {Aantal}, Totale Prijs: {totalePrijs:C}");
                return null; // Tuple is niet van toepassing op tijdschriften
            }

            //Console.WriteLine($"Bestelling {Id}: {Aantal} exemplaren van {Item.ToString()} voor een totaalbedrag van {totalePrijs:C}.");
            // return new Tuple<string, int, decimal>(Id.ToString(), Aantal, totalePrijs
            return null;
        }

        public static event EventHandler<string> BoekBesteld;

        public static void OnBoekBesteld(string boodschap)
        {
            BoekBesteld?.Invoke(null, boodschap);
        }

    }
}

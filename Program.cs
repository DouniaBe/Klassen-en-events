using System;

namespace Klassen_en_events
{
    internal class Program
    {
        static List<Boek> boeken = new List<Boek>();
        static List<Tijdschrift> tijdschriften = new List<Tijdschrift>();
        static List<Bestelling<object>> bestellingen = new List<Bestelling<object>>();
        //static List<Boek> boeken
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1. Voeg een nieuw boek toe");
                Console.WriteLine("2. Voeg een nieuw tijdschrift toe");
                Console.WriteLine("3. Plaats een bestelling");
                Console.WriteLine("4. Toon bestellingen");
                Console.WriteLine("5. Stoppen");

                Console.Write("Selecteer een optie (1-5): ");
                string keuze = Console.ReadLine();

                switch (keuze)
                {
                    case "1":
                        VoegBoekToe();
                        break;
                    case "2":
                        VoegTijdschriftToe();
                        break;
                    case "3":
                        PlaatsBestelling();
                        break;
                    case "4":
                        ToonBestellingen();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");
                        break;
                }
            }
        }

        static void VoegBoekToe()
        {
            Console.Write("Voer ISBN in: ");
            string isbn = Console.ReadLine();

            Console.Write("Voer boeknaam in: ");
            string naam = Console.ReadLine();

            Console.Write("Voer uitgever in: ");
            string uitgever = Console.ReadLine();

            Console.Write("Voer prijs in: ");
            decimal prijs = decimal.Parse(Console.ReadLine());

            Boek boek = new Boek(isbn, naam, uitgever, prijs);
            boeken.Add(boek);

            Console.WriteLine("Boek is toegevoegd.");
        }

        static void VoegTijdschriftToe()
        {
            Console.Write("Voer ISBN in: ");
            string isbn = Console.ReadLine();

            Console.Write("Voer tijdschriftnaam in: ");
            string naam = Console.ReadLine();

            Console.Write("Voer uitgever in: ");
            string uitgever = Console.ReadLine();

            Console.Write("Voer prijs in: ");
            decimal prijs = decimal.Parse(Console.ReadLine());

            Console.Write("Voer verschijningsperiode in (Dagelijks, Wekelijks, Maandelijks): ");
            Verschijningsperiode periode = (Verschijningsperiode)Enum.Parse(typeof(Verschijningsperiode), Console.ReadLine(), true);

            Tijdschrift tijdschrift = new Tijdschrift(isbn, naam, uitgever, prijs, periode);
            tijdschriften.Add(tijdschrift);

            Console.WriteLine("Tijdschrift is toegevoegd.");
        }

        static void PlaatsBestelling()
        {
            Console.Write("Selecteer een item (Boek of Tijdschrift): ");
            string itemType = Console.ReadLine();

            if (itemType.Equals("Boek", StringComparison.OrdinalIgnoreCase))
            {
                ToonBeschikbareBoeken();
            }
            else if (itemType.Equals("Tijdschrift", StringComparison.OrdinalIgnoreCase))
            {
                ToonBeschikbareTijdschriften();
            }
            else
            {
                Console.WriteLine("Ongeldig itemtype. Probeer opnieuw.");
                return;
            }

            Console.Write("Voer het nummer van het geselecteerde item in: ");
            int itemIndex = int.Parse(Console.ReadLine());

            if (itemType.Equals("Boek", StringComparison.OrdinalIgnoreCase) && itemIndex >= 0 && itemIndex < boeken.Count)
            {
                Boek geselecteerdBoek = boeken[itemIndex];
                VoegBestellingToe(geselecteerdBoek);
            }
            else if (itemType.Equals("Tijdschrift", StringComparison.OrdinalIgnoreCase) && itemIndex >= 0 && itemIndex < tijdschriften.Count)
            {
                Tijdschrift geselecteerdTijdschrift = tijdschriften[itemIndex];
                VoegBestellingToe(geselecteerdTijdschrift);
            }
            else
            {
                Console.WriteLine("Ongeldige selectie. Probeer opnieuw.");
            }
        }

        static void VoegBestellingToe(object item)
        {
            Console.Write("Voer besteldatum in (dd/mm/jjjj): ");
            DateTime datum = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

            Console.Write("Voer aantal in: ");
            int aantal = int.Parse(Console.ReadLine());

            if (item is Boek boek)
            {
                Bestelling<object> bestelling = new Bestelling<object>(boek, datum, aantal);
                bestellingen.Add(bestelling);
                Console.WriteLine($"Boek \"{boek.Naam}\" is besteld.");
            }
            else if (item is Tijdschrift tijdschrift)
            {
                Console.Write("Voer verschijningsperiode in (Dagelijks, Wekelijks, Maandelijks): ");
                Verschijningsperiode periode = (Verschijningsperiode)Enum.Parse(typeof(Verschijningsperiode), Console.ReadLine(), true);

                Bestelling<object> bestelling = new Bestelling<object>(tijdschrift, datum, aantal, periode);
                bestellingen.Add(bestelling);
                Console.WriteLine($"Tijdschrift \"{tijdschrift.Naam}\" is besteld.");
            }
        }

        static void ToonBeschikbareBoeken()
        {
            Console.WriteLine("Beschikbare boeken:");
            for (int i = 0; i < boeken.Count; i++)
            {
                Console.WriteLine($"{i}. {boeken[i].Naam}");
            }
        }

        static void ToonBeschikbareTijdschriften()
        {
            Console.WriteLine("Beschikbare tijdschriften:");
            for (int i = 0; i < tijdschriften.Count; i++)
            {
                Console.WriteLine($"{i}. {tijdschriften[i].Naam}");
            }
        }

        static void ToonBestellingen()
        {
            Console.WriteLine("Geplaatste bestellingen:");
            foreach (var bestelling in bestellingen)
            {
                Console.WriteLine($"ID: {bestelling.Id}, Item: {bestelling.Item.ToString()}, Datum: {bestelling.Datum.ToShortDateString()}, Aantal: {bestelling.Aantal}");
            }

        }

    }
}
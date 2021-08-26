using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PragueParkingv2Finished
{
    class Automobile
    {
        private readonly string regnr;
        private DateTime entry;
        private Automobilegroup group;
        public enum Automobilegroup
        {
            Car,
            Motorbike,
            TOM
        }

        public Automobile(Automobilegroup group, string regnr="",DateTime entry = new DateTime())
        {
            this.group = group;
            this.regnr = regnr;
            this.entry = entry;
        }

        public string RegNr
        { get { return regnr; } }
        public DateTime Entry
        { get { return entry; } }
        public Automobilegroup Group
        { get { return group; } }

        public override string ToString() { return entry.ToString() + "," + group + "," + regnr; }
        class InterpretCompose // Läser  fordon/Read vehicle.
        {
            public List<Automobile> Interpret()
            {
                if (!File.Exists("exempeldata.csv"))
                {
                    throw new FileNotFoundException("Filen 'exempeldata.csv' Kan ej läsas i hårddisken (Var vänlig och försök hitta den den manuellt.");
                }
                StreamReader stxt = new StreamReader("exempeldata.csv");
                List<Automobile> motorVehicle = new List<Automobile>();
                string signOf = "";
                using (stxt)
                {
                    signOf = stxt.ReadLine();
                    do
                    {
                        motorVehicle.Add(ConfigureAutomobile(signOf));
                        signOf = stxt.ReadLine();

                    } while (signOf != null);
                }
                return motorVehicle;
            }
            public void Compose(List<Automobile> carPark) // Skriver ut fordon/Write vehicle.
            {
                if (!File.Exists("exempeldata.csv"))
                {
                    Console.WriteLine("Filen 'exempeldata.csv' existerar inte. Skapar en ny kopia omedelbart!");
                }
                StreamWriter s = new StreamWriter("exempeldata.csv");
                using (s)
                {
                    foreach (Automobile motorVehicle in carPark)
                    {
                        s.WriteLine(motorVehicle.ToString());
                    }
                    s.Flush();
                }
            }
            private Automobile ConfigureAutomobile(string inData)
            {
                string[] arr = inData.Split(',');
                DateTime Date1 = DateTime.Parse(arr[0]);
                Automobile.Automobilegroup group;
                if (arr[1] == "Car")
                {
                    group = Automobile.Automobilegroup.Car;
                }
                else if (arr[1] == "Motorbike")
                {
                    group = Automobile.Automobilegroup.Motorbike;
                }
                else
                {
                    group = Automobile.Automobilegroup.TOM;
                }
                Automobile tempVehicle = new Automobile(group, arr[2], Date1);
                return tempVehicle;

            }

            class Menyn
            {
                private List<Automobile> carPark = new List<Automobile>();
                public void Meny()
                {
                    InterpretCompose ic = new InterpretCompose();
                    try
                    {
                        carPark = ic.Interpret();
                    }
                    catch (FileNotFoundException e)
                    {

                        Console.WriteLine(e.Message);
                        Console.Write("Vill du skapa en ny tom exempeldatafil eller avsluta programmet och söka i filen \n Visst skapa mig en ny fil: Tryck på [S] för att skapa.\n [N] för att avsluta!");
                        ConsoleKey inData = Console.ReadKey().Key;
                        Console.WriteLine();
                        if (inData == ConsoleKey.S)
                        {
                            FileStream fileStream = File.Create("exempeldata.csv");
                            fileStream.Close();
                            for (int parkingspot = 0; parkingspot < 200; parkingspot++)
                            {
                                carPark.Add
                               (new Automobile(Automobile.Automobilegroup.TOM, "TOM", DateTime.MinValue));
                            }
                            ic.Compose(carPark);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("STÅLANDE! Det skapades en ny fil med namet: exempeldata.csv .");
                            Console.WriteLine("Klicka på enter för att fortsätta");
                            Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.White;

                        }
                        else
                        {
                            System.Environment.Exit(0);
                        }
                    }
                    while (true)
                    {
                        Console.Clear();
                        Console.Write("\n**Välj ett Alternativ**:\n");
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("Välkommen till Parkeringshuset");
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[1] Lägg till en bil eller motorcykel"); // Lägger till fordon bil eller motorcykel.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[2] Flytta ett fordon till en annan parkeringsplats "); // Flytta fordon med regnr till annan plats.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[3] Ta bort fordon"); // Tar bort ett fordon via regnummer.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[4] Sök på ett fordon med registreringsnummber"); // Söker på fördon via regnr + får upp pris.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[5] Sök på fordon via platsnummer");    // Söker fordon via platsnummer + prislista, får även upp två MC:s på samma plats.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[6] Möblera om PHUSET(MC)");   // Gör om motorcyklarnas plats till bästa möjliga.
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[7] Möblera om PHUSET(Bil)"); // Gör om hela phuset till bästa möjliga
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[8] Översikt");       //Översikt över alla platser i phuset.
                        Console.WriteLine("----------------------------------");
                         Console.WriteLine("[9] Avsluta programmet");
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[0] Töm PHUS");
                        Console.WriteLine("----------------------------------");//Avslutar programmet.

                        // Console.WriteLine("[0] TÖM PHUS");


                        ConsoleKey inData = Console.ReadKey().Key; // Docs.microsoft / console.readkey & docs microsoft console.readkey
                        switch (inData)
                        {
                            case ConsoleKey.D1:

                                AttachAutomobile();
                                break;
                            case ConsoleKey.D2:

                                NewParkingspot();
                                break;
                            case ConsoleKey.D3:

                                WithdrawAutomobile();
                                break;
                            case ConsoleKey.D4:

                                ExploreAutomobile();
                                break;
                            case ConsoleKey.D5:

                                Exploreparkingspace();
                                break;
                            case ConsoleKey.D6:

                                ImprovecarParkMC();
                                break;
                            case ConsoleKey.D7:

                                ImprovecarPark();
                                break;

                            case ConsoleKey.D8:
                                GetinfoMenu();
                                break;
                             case ConsoleKey.D0:
                                   Clear();
                                   break;

                            case ConsoleKey.D9:
                            case ConsoleKey.Escape:
                                QUIT();
                                break;
                            default:
                                break;
                        }
                    }
                }
                private string ObtainRegNr()
                {
                    bool inData;
                    string regNum;

                    do
                    {
                        inData = true;
                        regNum = "";
                        Console.Write("Var vänlig och fyll i ett registreringsnummer: ");
                        while (regNum.Length < 1)
                        {
                            regNum = Console.ReadLine().ToUpper();
                        }
                        if (regNum.Contains(" "))
                        {
                            Console.WriteLine("Registreringsnummer får ej innehålla mellanslag .");
                            inData = false;
                        }


                    } while (!inData);
                    return regNum;
                } // Matar in ett regnr som sedan registreras i AttachAutomobile
                private void AttachAutomobile() // Här så lägger vi till både motorcykel och bil
                {
                    Console.Clear();
                    Console.WriteLine("Lägg till ett fordon:");
                    Console.WriteLine();

                    bool binData = false;
                    DateTime dt = DateTime.Now;
                    string inData = ObtainRegNr();
                    Automobile.Automobilegroup autoType = ObtainAutomobileGroup();


                    for (int i = 0; i < carPark.Count; i++)
                    {

                        if (carPark.Any(a => a.RegNr == inData))

                        {

                            Console.WriteLine("Registreringsnummret {0} registreringsnummer finns redan i systemet", inData);
                            Console.WriteLine("Tryck på enter för att återvända till menyn");
                            Console.ReadLine();
                            return;

                        }

                    }
                    if (autoType == Automobile.Automobilegroup.Car)
                    {
                        for (int parkspot = 0; parkspot < 100; parkspot++)
                        {
                            
                                    if (carPark[parkspot].Group == Automobile.Automobilegroup.TOM)
                                    {
                                        carPark.RemoveAt(parkspot);
                                        carPark.Insert(parkspot, new Automobile(autoType, inData, dt));
                                        Console.WriteLine("Bilen {0} fick platsen {1}.",
                                        carPark[parkspot].RegNr, parkspot + 1);
                                        binData = true;
                                        break;
                                    }
                                }
                            
                    }
                    else
                    {
                        for (int parkspot = 0; parkspot < 100; parkspot++)
                        {
                          
                                    if (carPark[parkspot].Group == Automobile.Automobilegroup.Motorbike)
                                    {

                                        if (carPark[parkspot + 100].Group == Automobile.Automobilegroup.TOM)
                                        {
                                            carPark.RemoveAt(parkspot + 100);
                                            carPark.Insert(parkspot + 100, new Automobile(autoType, inData, dt));
                                            Console.WriteLine("Motorcykeln {0} tilldelades parkeringsplatsen {1}. OBS här finns även en annan motorcykel med regnummer: {2}",
                                            carPark[parkspot + 100].RegNr, parkspot + 1, carPark[parkspot].RegNr);
                                            binData = true;
                                            break;
                                        }
                                    }
                                
                        }
                        if (!binData)
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                if (carPark[i].Group == Automobile.Automobilegroup.TOM)
                                {
                                    carPark.RemoveAt(i);
                                    carPark.Insert(i, new Automobile(autoType, inData, dt));
                                    Console.WriteLine("Motorcykeln {0} tilldelades parkeringsplatsen: {1}.",
                                    carPark[i].RegNr, i + 1);
                                    binData = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!binData)
                    {

                        Console.WriteLine("Tyvärr så finns det ingen {0} ledig plats för tillfället .", autoType.ToString().ToLower());
                    }
                    else
                    {
                        InterpretCompose ic = new InterpretCompose();
                        try
                        {
                            ic.Compose(carPark);
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    Console.WriteLine("Tryck på enter för att återvända till huvudmenyn.");
                    Console.Read();
                }

                private Automobile.Automobilegroup ObtainAutomobileGroup()
                {
                    while (true)
                    {
                        Console.WriteLine("Registreringsnumret du angav...Är det en bil eller motorcykel? Svara med [B] för Bil och [M] för Motorcykel: ");
                        ConsoleKey inData = Console.ReadKey().Key;
                        Console.WriteLine();
                        if (inData == ConsoleKey.B)
                        {
                            return Automobile.Automobilegroup.Car;
                        }
                        else if (inData == ConsoleKey.M)
                        {
                            return Automobile.Automobilegroup.Motorbike;
                        }
                        else
                        {
                            Console.WriteLine("Du angav fel tecken, får endast innehålla B eller M, stor eller små bokstäver. Tack för förståelsen.");
                        }
                    }
                } //  Frågar användaren vilket typ av fordon hen vill mata in.
                private int FindParkingspace()
                {
                    bool binData = false;
                    string inData;
                    int outData;
                    do
                    {
                        binData = true;
                        Console.Write("Var vänlig och fyll i siffran till parkeringsplatsen: ");
                        inData = "";
                        while (inData.Length < 1)
                        {
                            inData = Console.ReadLine();
                        }
                        bool validInt = int.TryParse(inData, out outData);
                        if (!validInt)
                        {
                            Console.WriteLine("OBS Får endast innehålla siffror.");
                            binData = false;
                        }
                        if (outData < 1 || outData > 100)
                        {
                            Console.WriteLine("OBS Ingen siffra över 100, då vi endast har 100 platser.");
                            binData = false;
                        }
                    } while (!binData);
                    return outData - 1;




                } // Användaren skriver in ett parkeringsnummer och får tillbaka info om specika P platsen. Så som regnummer som står på platsen, tid och pris.
                private int SearchForSpot(string regnr) // Metod som loopar igenom alla platser med  regnr.
                {
                    for (int parkingspot = 0; parkingspot < 200; parkingspot++) // Går ej med 100 här eftersom MC tar 2x i loopen 1 mc 2 mc 100st -> 200 i loopen
                    {
                        if (carPark[parkingspot].RegNr == regnr)
                        {
                            return parkingspot;
                        }
                    }
                    return -1;
                }
                private int Automobileprices(Automobile automobile)
                {
                    TimeSpan ts = DateTime.Now - automobile.Entry;
                    int cost;
                    if (ts.TotalMinutes < 5)
                    {
                        cost = 0;
                    }
                    else if (ts.TotalMinutes < 125)
                    { cost = NewMethod(automobile);
                    }
                    else
                    {
                        int time;
                        if (ts.TotalMinutes - 5 % 60 == 0)
                        {
                            time = (int)(ts.TotalMinutes - 5) / 60;
                        }
                        else
                        {
                            time = (int)(ts.TotalMinutes - 5) / 60;
                            time++;
                        }
                        if (automobile.Group == Automobile.Automobilegroup.Motorbike)
                        { cost = time * 10; }
                        else
                        { cost = time * 20; }
                    }
                    return cost;

                } // Kostnaden första 5 min är gratis. Bil 20CZK MC 10CZK

                private static int NewMethod(Automobile automobile) // Metod för kostnaden.
                {
                    int cost;
                    if (automobile.Group == Automobile.Automobilegroup.Motorbike)
                    { cost = 20; }
                    else
                    { cost = 40; }

                    return cost;
                }

                private void AquireSearchedAuto(int inData)
                {
                    if (carPark[inData].Group == Automobile.Automobilegroup.TOM)
                    {
                        Console.Write("\nPlatsen du angav är ledig.");
                    }
                    else if (inData < 100)
                    {
                        int cost = Automobileprices(carPark[inData]);
                        TimeSpan ts = DateTime.Now - carPark[inData].Entry;
                        Console.WriteLine("{0} ({1}), som är parkerad på {2}, anlände {3}.\nParkerad tid: {4:0,0} timmar {5} minuter. Kostnad: {6:C}.",
                         carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), inData + 1,
                         carPark[inData].Entry, ts.TotalHours, ts.Minutes, cost);
                        if (carPark[inData + 100].Group == Automobile.Automobilegroup.Motorbike)
                        {
                            inData += 100;
                            cost = Automobileprices(carPark[inData]);
                            ts = DateTime.Now - carPark[inData].Entry;
                            Console.WriteLine("{0} ({1}), är också parkerad {2}, anlände {3}.\nParkerad tid: {4:0,0} timmar {5} minuter. Kostnad: {6:C}.",
                            carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), inData - 99,
                            carPark[inData].Entry, ts.TotalHours, ts.Minutes, cost);
                        }
                    }
                    else
                    {
                        int price = Automobileprices(carPark[inData]);
                        TimeSpan span = DateTime.Now - carPark[inData].Entry;
                        Console.WriteLine("{0} ({1}), som är parkerad på {2}, anlände {3}.\nParkerad tid: {4:0,0} timmar {5} minuter. Kostnad: {6:C}.",
                        carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), inData - 99,
                        carPark[inData].Entry, span.TotalHours, span.Minutes, price);
                        inData -= 100;
                        price = Automobileprices(carPark[inData]);
                        span = DateTime.Now - carPark[inData].Entry;
                        Console.WriteLine("{0} ({1}), är också parkerad på {2}, anlände {3}.\nParkerad tid: {4:0,0} timmar {5} minuter. Kostnad: {6:C}.",
                        carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), inData + 1,
                        carPark[inData].Entry, span.TotalHours, span.Minutes, price);
                    }
                } // Metod som vi sedan lägger in för att få reda på regnr och kostnader för både MC och Bil via platsnummer

                private void NewParkingspot()
                {
                    Console.Clear();
                    Console.WriteLine("Transportera fordon:");
                    Console.WriteLine();
                    string RegNum = ObtainRegNr();
                    int inData = SearchForSpot(RegNum);

                    if (inData < 0)
                    {
                        Console.WriteLine("Kan ej hitta {0} på prague parkings parkeringshus.", RegNum);
                    }
                    else
                    {
                        int chooseParking = FindParkingspace();

                        if (carPark[inData].
                            Group == Automobile.Automobilegroup.Motorbike && carPark[chooseParking].Group == Automobile.Automobilegroup.Motorbike && carPark[chooseParking + 100].Group == Automobile.Automobilegroup.TOM)
                        {
                            Automobile inData1 = carPark[chooseParking + 100];
                            carPark[chooseParking + 100] = carPark[inData];
                            carPark[inData] = inData1;
                            if (inData < 100)
                            {
                                if (carPark[inData + 100].Group == Automobile.Automobilegroup.Motorbike)
                                {
                                    inData1 = carPark[inData + 100];
                                    carPark[inData + 100] = carPark[inData];
                                    carPark[inData] = inData1;
                                }
                            }
                            Console.WriteLine("{0} flyttades till platsen {1}, OBS här finns också en annan motorcykel {2}.",
                              carPark[chooseParking + 100].RegNr, chooseParking + 1, carPark[chooseParking].RegNr);
                        }

                        else if (carPark[chooseParking].Group == Automobile.Automobilegroup.TOM)
                        {
                            Automobile inData1 = carPark[chooseParking];
                            carPark[chooseParking] = carPark[inData];
                            carPark[inData] = inData1;
                            if (inData < 100)
                            {
                                if (carPark[inData + 100].Group == Automobile.Automobilegroup.Motorbike)
                                {
                                    inData1 = carPark[inData + 100];
                                    carPark[inData + 100] = carPark[inData];
                                    carPark[inData] = inData1;
                                }
                            }
                            Console.WriteLine("{0} ({1}) tilldelades platsen {2}.", carPark[chooseParking].RegNr,
                             carPark[chooseParking].Group.ToString().ToLower(), chooseParking + 1);
                        }
                        else
                        {
                            Console.WriteLine("Tyvärr så  finns det ingen plats för {0} på plats {1}.",
                             carPark[inData].RegNr, chooseParking);
                        }

                    }
                    InterpretCompose ic = new InterpretCompose();
                    try
                    {
                        ic.Compose(carPark);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    Console.WriteLine("Tryck på enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                } // Flytta fordon till ny plats
                private void WithdrawAutomobile() // Ta bort fordon
                {
                    Console.Clear();
                    Console.WriteLine("Ta bort fordon:");
                    Console.WriteLine();
                    string RegNum = ObtainRegNr();
                    int inData = SearchForSpot(RegNum);

                    if (inData >= 0 && inData < 100)
                    {

                        int cost = Automobileprices(carPark[inData]);
                        Console.WriteLine("{0} ({1}) anlände till parkeringshuset {2}. den totala kostnaden är {3:C}.\n {1} finns på plats {4}",
                            carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), carPark[inData].Entry, cost, inData + 1);
                        if (carPark[inData].Group == Automobile.Automobilegroup.Motorbike && carPark[inData + 100].Group == Automobile.Automobilegroup.Motorbike)
                        {
                            carPark[inData] = carPark[inData + 100];
                            carPark[inData + 100] = new Automobile(Automobile.Automobilegroup.TOM, "TOM", DateTime.MinValue);
                        }
                        else
                        {
                            carPark[inData] = new Automobile(Automobile.Automobilegroup.TOM, "TOM", DateTime.MinValue);
                        }
                    }
                    else if (inData >= 100)
                    {
                        int cost1 = Automobileprices(carPark[inData]);
                        Console.WriteLine("{0} ({1}) anlände till parkeringshuset {2}. den totala kostnaden {3:C}.\n {1} finns på plats {4}",
                        carPark[inData].RegNr, carPark[inData].Group.ToString().ToLower(), carPark[inData].Entry, cost1, inData - 99);
                        carPark[inData] = new Automobile(Automobile.Automobilegroup.TOM, "TOM", DateTime.MinValue);
                    }
                    else
                    {
                        Console.WriteLine("Fordonet ({0}) finns tyvärr inte parkerad här.", RegNum);
                    }
                    InterpretCompose ic = new InterpretCompose();
                    try
                    {
                        ic.Compose(carPark);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Console.WriteLine("Tryck på enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                }
                private void ExploreAutomobile()
                {
                    Console.Clear();
                    Console.WriteLine("Sök på fordon:");
                    Console.WriteLine();

                    string RegNr = ObtainRegNr();
                    int inData = SearchForSpot(RegNr);
                    if (inData < 0)
                    {
                        Console.WriteLine("Fordonet ({0}) kan ej hittas i systemet.", RegNr);
                    }
                    else
                    {
                        AquireSearchedAuto(inData);
                    }
                    Console.WriteLine("Tryck på enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                } // sök på regnummer
                private void Exploreparkingspace() // sök på platsnummer
                {
                    Console.Clear();
                    Console.WriteLine("Sök i en parkeringsplats för information:");
                    Console.WriteLine();
                    int inData1 = FindParkingspace();
                    AquireSearchedAuto(inData1);
                    Console.WriteLine("Tryck på enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                }
                private void ImprovecarParkMC() // gör om motorcyklarnas platser till bästa möjliga platser.
                {
                    Console.Clear();
                    Console.WriteLine("Vidta åtgärder för bästa möjliga alternativ :");
                    Console.WriteLine();
                    for (int i = 0; i < 100; i++)
                    {
                        if (carPark[i].Group == Automobile.Automobilegroup.Motorbike && carPark[i + 100].Group == Automobile.Automobilegroup.TOM)
                        {
                            for (int parkingspot = i + 1; parkingspot < 100; parkingspot++)
                            {
                                if (carPark[parkingspot].Group == Automobile.Automobilegroup.Motorbike && carPark[parkingspot + 100].Group == Automobile.Automobilegroup.TOM)
                                {
                                    Automobile inData = carPark[i + 100];
                                    carPark[i + 100] = carPark[parkingspot];
                                    carPark[parkingspot] = inData;
                                    Console.WriteLine("Flytta {0} tilldelades från platsen {1} till platsen {2}.", carPark[i + 100].RegNr, parkingspot + 1, i + 1);
                                    break;
                                }
                            }
                        }
                    }
                    InterpretCompose ic = new InterpretCompose();
                    try
                    {
                        ic.Compose(carPark);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Console.WriteLine("Åtgårder åttågs.Tryck enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                } // Flyttar över mc som exempelvis är ensamstående till närmaste ruta med en annan mc
                private void ImprovecarPark() // gör om hela phuset för bästa möjliga platse.
                {
                    Console.Clear();
                    Console.WriteLine("Vidta åtgärder för bästa möjliga parkeringsplats:");
                    Console.WriteLine();
                    for (int k = 0; k < 100; k++)
                    {
                        if (carPark[k].Group == Automobile.Automobilegroup.TOM)
                        {
                            for (int parkingspot = 99; parkingspot > k; parkingspot--)
                            {
                                if (carPark[parkingspot].Group == Automobile.Automobilegroup.Car)
                                {
                                    Automobile inData = carPark[k];
                                    carPark[k] = carPark[parkingspot];
                                    carPark[parkingspot] = inData;
                                    Console.WriteLine("Flytta (Bil) {0} från platsen {1} till platsen {2}.",
                                    carPark[k].RegNr, parkingspot + 1, k + 1);
                                    break;
                                }
                                else if (carPark[parkingspot].Group == Automobile.Automobilegroup.Motorbike)
                                {
                                    if (carPark[parkingspot + 100].Group == Automobile.Automobilegroup.Motorbike)
                                    {
                                        Automobile inData = carPark[k];
                                        carPark[k] = carPark[parkingspot];
                                        carPark[k + 100] = carPark[parkingspot + 100];
                                        carPark[parkingspot] = inData;
                                        carPark[parkingspot + 100] = inData;
                                        Console.WriteLine("Flytta (motorcykel) {0} och {1}, från plats {2} till plats {3}.",
                                            carPark[k].RegNr, carPark[k + 100].RegNr, parkingspot + 1, k + 1);
                                        break;
                                    }
                                    else
                                    {
                                        Automobile temp = carPark[k];
                                        carPark[k] = carPark[parkingspot];
                                        carPark[parkingspot] = temp;
                                        Console.WriteLine("Flytta (motorcykel) {0} från plats {1} till plats {2}.",
                                        carPark[k].RegNr, parkingspot + 1, k + 1);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    InterpretCompose ic = new InterpretCompose();
                    try
                    {
                        ic.Compose(carPark);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Console.WriteLine("Åtgårder åttågs.Tryck enter för att återvända till huvudmenyn.");
                    Console.ReadLine();
                } // Flyttar över bil som tex står på plats 90 till närmaste lediga

                private void GetinfoMenu() // meny för Översikt av phus
                {
                    bool remainInGetinfo = true;
                    while (remainInGetinfo)
                    {
                        Console.Clear();
                        Console.WriteLine("\nÖversikts menyn");
                        Console.WriteLine("----------------------------------");
                        Console.WriteLine("[1]    Visa alla parkerade  fordon");
                        Console.WriteLine("[2]    Återvänd till menyn");
                        ConsoleKey input = Console.ReadKey().Key;
                        switch (input)
                        {
                            case ConsoleKey.D1:
                                GetInfo();
                                break;
                            case ConsoleKey.D2:
                                remainInGetinfo = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
                private void GetInfo() // översikt av alla platser.
                {
                    Console.Clear();
                    Console.WriteLine("***ÖVERSIKT PARKERADE PLATSER****");
                    Console.WriteLine("");


                    for (int i = 0; i < 100; i++)

                    { 
                                if (carPark[i].Group == Automobile.Automobilegroup.Car)
                                {
                                    int plats = i + 1;
                                    Console.Write("Plats " + plats + " är tagen av fordon med registreringsnummer: " + carPark[i] + " *<-REGNR* " + "\n");


                                }
                                else if (carPark[i].Group == Automobile.Automobilegroup.Motorbike)
                                {
                                    int plats = i + 1;
                                    Console.Write("Plats " + plats + " är tagen av fordon med registreringsnummer: " + carPark[i] + " *<-REGNR* " + carPark[i + 100] + " *<-REGNR* " + "\n");
                                }

                        
                        else if (carPark[i].Group == Automobile.Automobilegroup.TOM)
                        {

                            Console.WriteLine($"{i+1} LEDIG PLATS");
                        }


                    }
                    Console.WriteLine("\nTryck på enter för att återvända  till huvudmenyn.");

                    Console.ReadLine();
                }





                public void Clear() 
                {

                    
                    
                     carPark.Clear();
                   

                    for (int i = 0; i < 100; i++)
                    {
                        Automobile Tom = new Automobile(Automobilegroup.TOM);

                        carPark.Add(Tom);
                        carPark.Add(Tom);
                        
                    }
                    Console.Clear();
                    Console.WriteLine("Nu är phuset tomt\n");
                    Console.ReadLine();

                }

                    private void QUIT()
                {
                    Console.Clear();
                    Console.Write("\n**Välkommen till menyn för att avsluta programmet**.");
                    Console.Write("\n ------------------------------------------------------------------");
                    Console.Write("\n");
                    Console.Write("Vill du Avsluta programmet? Mata in (J) för JA och (N) för NEJ. Tack och välkommen tillbaka.");
                    Console.Write("\n ------------------------------------------------------------------");
                    ConsoleKey avsluta_exit = Console.ReadKey().Key;
                    if (avsluta_exit == ConsoleKey.J || default == ConsoleKey.N || avsluta_exit == ConsoleKey.Escape)
                    { System.Environment.Exit(0); }
                } // Avslutar program med hjälp av Consolekey

              
                


                class Program
                {
                    static void Main(string[] args)// VIKTIGT! Kolla upp hur man får fram ConsoleEncoding, Unicode För att programmet ska kunna hantera ett alfabet mer än latinska *GLÖM EJ*
                    {
                        Console.OutputEncoding = Encoding.Unicode; Console.InputEncoding = Encoding.Unicode;
                        CultureInfo.CurrentCulture = new CultureInfo("cs-CZ"); Menyn meny = new Menyn(); meny.Meny();
                    }
                }
            }
        }
    }
}
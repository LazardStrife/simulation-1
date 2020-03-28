using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationModel;

namespace Evolving_Settlement
{
    class Program
    {
        static int year = 1, month = 1;
        static bool automaticShowInfo = false;
        static Simulation simulation;
        static string[] months = { "", "enero", "febrero", "marzo" ,"abril" ,"mayo" ,"junio" ,"julio" ,"agosto" ,"septiembre" ,"octubre" ,"noviembre", "diciembre" };
        static void Main(string[] args)
        {
            Start();
        }

        // monte carlo algorithm
        static void CalculateEspectedValues()
        {
            double birthsPerMonth = 0, deceasesPerMonth = 0, populationYear100 = 0, maleYear100 = 0, femaleYear100 = 0;
            Console.WriteLine("Introduzca la cantidad de simulaciones a analizar por el metodo de Monte Carlo");
            int numberOfSimulations = int.Parse(Console.ReadLine());

            Simulation testSimulation;
            for(int i = 0; i < numberOfSimulations; ++i)
            {
                testSimulation = new Simulation();
                birthsPerMonth += testSimulation.births;
                deceasesPerMonth += testSimulation.deceases;
                maleYear100 += testSimulation.GetMaleCount(100, 1);
                femaleYear100 += testSimulation.GetFemaleCount(100, 1);
                populationYear100 += (testSimulation.GetMaleCount(100, 1) + testSimulation.GetFemaleCount(100, 1));
            }

            birthsPerMonth /= numberOfSimulations;
            deceasesPerMonth /= numberOfSimulations;
            maleYear100 /= numberOfSimulations;
            femaleYear100 /= numberOfSimulations;
            populationYear100 /= numberOfSimulations;

            Console.WriteLine("Valor esperado de la poblacion en el año 100: " + populationYear100);
            Console.WriteLine("Valor esperado de la cantidad de varones en el año 100: " + maleYear100);
            Console.WriteLine("Valor esperado de la cantidad de hembras en el año 100: " + femaleYear100);
            Console.WriteLine("Valor esperado de la cantidad de nacidos hasta el año 100: " + birthsPerMonth);
            Console.WriteLine("Valor esperado de la cantidad de fallecidos hasta el año 100: " + deceasesPerMonth);

        }

        static void Start()
        {
            simulation = new Simulation();

            Console.WriteLine(">>> BIENVENIDO A LA SIMULACION DE UN POBLADO EN EVOLUCION <<<");
            Console.WriteLine("");
            Console.WriteLine("Escribe 'h' para consultar los diferentes comandos si es tu");
            Console.WriteLine("primera vez.");

            ReadInput();

        }

        static void ReadInput()
        {
            startingLabel:
            Console.Write("-> ");
            string[] input = Console.ReadLine().Split();
            Console.WriteLine();

            switch (input[0])
            {
                case "r":
                    simulation = new Simulation();
                    year = 1;
                    month = 1;
                    goto startingLabel;
                case "a":
                    CalculateEspectedValues();
                    goto startingLabel;
                case "c":
                    Console.Clear();
                    goto startingLabel;
                case "d":
                    ShowDateInfo();
                    goto startingLabel;
                case "q":
                    break;
                case "h":
                    ShowHelp();
                    goto startingLabel;
                case "i":
                    ShowInfo();
                    goto startingLabel;
                case "n":
                    AdvanceMonth();
                    goto startingLabel;
                case "m":
                    int y = 0, m = 0;        
                    y = int.Parse(input[1]);
                    m = int.Parse(input[2]);
                    MoveTo(y, m);
                    goto startingLabel;
                case "s":
                    ShowCurrentTimeInfo();
                    goto startingLabel;
                case "t":
                    if(input.Length == 2)
                    {
                        switch (input[1])
                        {
                            case "auto_muestra":
                                automaticShowInfo = !automaticShowInfo;
                                break;
                            case "id":
                                idON = !idON;
                                break;
                            case "edad":
                                ageON = !ageON;
                                break;
                            case "genero":
                                genderON = !genderON;
                                break;
                            case "pareja":
                                mateON = !mateON;
                                break;
                            case "cumpleaños":
                                birthdayON = !birthdayON;
                                break;
                            case "hijos_deseados":
                                desiredON = !desiredON;
                                break;
                            case "hijos_cantidad":
                                childrenON = !childrenON;
                                break;
                            case "embarazadas":
                                isPregnantON = !isPregnantON;
                                break;
                        }
                    }
                    goto startingLabel;
                default:
                    Console.WriteLine("Comando '" + input[0] + "' no existe. Escriba 'h' para ver lista de comandos.");
                    goto startingLabel;
            }
        }

        private static void ShowDateInfo()
        {
            Console.WriteLine("Fecha Actual: " + months[month] + " del año " + year);
        }

        static bool idON = true, ageON = true, genderON = true, mateON = false, birthdayON = false, desiredON = false, childrenON = false, isPregnantON = false;
        static void ShowDateInfo(int y, int m)
        {
            Tuple<List<Person>, List<Person>, List<Person>, List<Person>> info = simulation.DateInfo(y, m);
            List<Person> deceased = info.Item1;
            List<Person> borned = info.Item2;
            List<Person> male = info.Item3;
            List<Person> female = info.Item4;

            
            Console.WriteLine("Informacion de " + months[m] + " del año " + y + " :");
            Console.WriteLine(deceased.Count + " personas murieron:\n");
            foreach(Person p in deceased)
            {
                if(idON)
                    Console.WriteLine("Identificador: " + p.id);
                if(ageON)
                    Console.WriteLine("Edad: " + p.age);
                if(genderON)
                    Console.WriteLine("Genero: " + (p.gender == Gender.Male ? "Masculino" : "Femenino"));
                if(mateON && p.mate_id != "")
                    Console.WriteLine("Pareja: " + p.mate_id);
                if (birthdayON)
                    Console.WriteLine("Mes de cumpleaños: " + months[p.birthday_month]);
                if (desiredON)
                    Console.WriteLine("Cantidad de hijos deseados: " + (p.desired_children == -1 ? "infinito" : p.desired_children.ToString()));
                if (childrenON)
                    Console.WriteLine("Cantidad de hijos: " + p.children_count);
                if (isPregnantON && p.gender == Gender.Female && p.pregnancy_months_left > -1)
                    Console.WriteLine("Estaba embarazada");
            }

            Console.WriteLine(borned.Count + " personas nacieron:\n");
            foreach (Person p in borned)
            {
                if (idON)
                    Console.WriteLine("Identificador: " + p.id);
                if (genderON)
                    Console.WriteLine("Genero: " + (p.gender == Gender.Male ? "Masculino" : "Femenino"));
                if (birthdayON)
                    Console.WriteLine("Mes de cumpleaños: " + months[p.birthday_month]);
                if (desiredON)
                    Console.WriteLine("Cantidad de hijos deseados: " + (p.desired_children == -1 ? "infinito" : p.desired_children.ToString()));
            }

            Console.WriteLine(male.Count + " personas del sexo masculino:\n");
            foreach (Person p in male)
            {
                if (idON)
                    Console.WriteLine("Identificador: " + p.id);
                if (ageON)
                    Console.WriteLine("Edad: " + p.age);
                if (mateON && p.mate_id != "")
                    Console.WriteLine("Pareja: " + p.mate_id);
                if (birthdayON)
                    Console.WriteLine("Mes de cumpleaños: " + months[p.birthday_month]);
                if (desiredON)
                    Console.WriteLine("Cantidad de hijos deseados: " + (p.desired_children == -1 ? "infinito" : p.desired_children.ToString()));
                if (childrenON)
                    Console.WriteLine("Cantidad de hijos: " + p.children_count);
            }

            Console.WriteLine(female.Count + " personas del sexo femenino:\n");
            foreach (Person p in female)
            {
                if (idON)
                    Console.WriteLine("Identificador: " + p.id);
                if (ageON)
                    Console.WriteLine("Edad: " + p.age);
                if (mateON && p.mate_id != "")
                    Console.WriteLine("Pareja: " + p.mate_id);
                if (birthdayON)
                    Console.WriteLine("Mes de cumpleaños: " + months[p.birthday_month]);
                if (desiredON)
                    Console.WriteLine("Cantidad de hijos deseados: " + (p.desired_children == -1 ? "infinito" : p.desired_children.ToString()));
                if (childrenON)
                    Console.WriteLine("Cantidad de hijos: " + p.children_count);
                if (isPregnantON && p.pregnancy_months_left > -1)
                    Console.WriteLine("Esta embarazada");
            }
        }

        static void ShowCurrentTimeInfo()
        {
            ShowDateInfo(year, month);
        }

        static void MoveTo(int y, int m)
        {
            bool wrongDate = y < 1 || y > 100 || m < 1 || m > 12 || (y == 100 && m != 1);

            if (wrongDate)
            {
                Console.WriteLine("Fecha incorrecta");
                return;
            }

            year = y;
            month = m;

            if (automaticShowInfo)
            {
                ShowDateInfo(y, m);
            }
        }

        static void AdvanceMonth()
        {
            if (year == 100 && month == 1)
            {
                Console.WriteLine("No se puede avanzar mas alla de enero del año 100");
                return;
            }

            ++month;
            if(month == 13)
            {
                ++year;
                month = 1;
            }

            if (automaticShowInfo)
            {
                ShowDateInfo(year, month);
            }
            
        }

        static void ShowInfo()
        {
            Console.WriteLine("En este simulador se muestra la evolucion al cabo de 100 años");
            Console.WriteLine("de una poblacion inicialmente constituida por 50 hombres y 50");
            Console.WriteLine("mujeres, que se reproducen atendiendo a variables bastante cer");
            Console.WriteLine("canas a las reales.");
            Console.WriteLine("La simulacion se llevo a cabo en el momento en que se ejecuto");
            Console.WriteLine("este programa, de modo que se podran analizar los fallecidos, ");
            Console.WriteLine("nacidos y las demas personas de cada mes dentro de los años del");
            Console.WriteLine("estudio.");
            Console.WriteLine("Se comienza por el mes de enero del año 1 y se puede avanzar mes");
            Console.WriteLine("por mes o ir directamente al mes de un año determinado (para mas");
            Console.WriteLine("informacion de como navegar por las fechas y mostrar la informacion");
            Console.WriteLine("de las mismas consultar la lista de comandos escribiendo 'h').");
            Console.WriteLine("Inicialmente solo se ve la informacion del identificador y la edad");
            Console.WriteLine("de las personas de la fecha actual si se escribe 's'. (Para cambiar");
            Console.WriteLine("la informacion que se muestra de las personas o mostrar automaticamente");
            Console.WriteLine("la informacion al cambiar de fecha consultar la lista de comandos");
            Console.WriteLine("escribiendo 'h').");
            Console.WriteLine("");
        }

        static void ShowHelp()
        {
            Console.WriteLine("\nComandos:");
            Console.WriteLine("q                    salir del simulador");
            Console.WriteLine("h                    consultar los comandos");
            Console.WriteLine("i                    mostrar informacion sobre el simulador");
            Console.WriteLine("r                    reiniciar simulacion");
            Console.WriteLine("a                    analizar aproximaciones de valores esperados");
            Console.WriteLine("d                    mostrar fecha actual");
            Console.WriteLine("c                    limpiar la consola");
            Console.WriteLine("n                    avanzar un mes");
            Console.WriteLine("m x y                avanzar al mes y del año x");
            Console.WriteLine("s                    mostrar informacion del actual mes seleccinado");
            Console.WriteLine("t x                  activar/desactivar opcion x de observacion");
            Console.WriteLine("  auto_muestra       mostrar o no mostrar automaticamente la infor");
            Console.WriteLine("                     macion cuando se avanza a una fecha");
            Console.WriteLine("  id                 mostrar los identificadores de las personas");
            Console.WriteLine("  edad               mostrar la edad de las personas");
            Console.WriteLine("  genero             mostrar el sexo/genero de las personas");
            Console.WriteLine("  pareja             mostrar los identificadores de las parejas");
            Console.WriteLine("                     de las personas (si tiene en el momento)");
            Console.WriteLine("  cumpleaños         mostrar el mes en que cumplen año las personas");
            Console.WriteLine("  hijos_deseados     mostrar la cantidad de hijos deseados de las");
            Console.WriteLine("                     personas");
            Console.WriteLine("  hijos_cantidad     mostrar la cantidad de hijos de las personas");
            Console.WriteLine("  embarazadas        mostrar mujeres embarazadas");
        }
    }
}

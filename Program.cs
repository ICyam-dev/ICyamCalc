using System;
using System.Collections.Generic;
using ICyamCalc.Objets;

namespace ICyamCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            PresICyamCalc(); //Présentation de ICyamCalc
            List<string> memFormule = new List<string>(); //Enregistrer les formule en mémoire
            int nbMem = 0; //Nombre de formule en mémoire
            do
            {
                Console.Write(nbMem + 1 + ">");
                Formule formule = new Formule(Console.ReadLine());
                bool exitOk = false;
                // Instance Formule
                string maFormule = formule.TexteFormule();
                //-----------------
                switch (maFormule)
                {
                    case "cls": //Efface l'écran
                        Console.Clear();
                        PresICyamCalc();
                        break;
                    case "list":
                        //Code pour la liste des formules en mémoire
                        int indexFormule = 0;
                        foreach (string memoireFormule in memFormule)
                        {
                            indexFormule++;
                            Console.WriteLine("#" + indexFormule + "# :" + memoireFormule);
                        }
                        break;
                    case "quit": //Sortie de la boucle de saisie
                    case "exit": //Sortie de la boucle de saisie
                        exitOk = true;
                        break;
                    default : //Traitement du calcul de la formule
                        maFormule = maFormule.Replace('.', ','); //Remplace les '.' par des ','
                        memFormule.Add(maFormule); //enregistrement de la formule
                        nbMem ++;//incrémentation de l'index de la mémoire
                        int c = 50;
                        int l = Console.CursorTop - 1;
                        string newFormule = formule.AnalyseDieses(); // remplace les #xxx# par les formule correcpondantes si présent dans la chaine
                        formule.RemplaceFormule(newFormule); // remplace le la nouvelle formule dans l'objet formule.
                        string resultat = formule.CalculFormule(); //Calcul de la formule
                        Console.SetCursorPosition(c, l);
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.WriteLine("= " + resultat);
                        Console.ResetColor();
                        break;
                }
                if (exitOk) // sortir de la boucle
                    break;
            }
            while (true);
        }

        //Fonction de présentation de l'appication
        static void PresICyamCalc()
        {
            Console.Clear();
            Console.WriteLine("ICyamCalc v 0.04a");
            Console.WriteLine("----------------------------------------------------------------------------------");
        }
    }
}

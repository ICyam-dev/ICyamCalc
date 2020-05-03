using System;
using System.Collections.Generic;

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
                string formule = Console.ReadLine();
                bool exitOk = false;
                switch (formule)
                {
                    case "cls": //Efface l'écran
                        Console.Clear();
                        PresICyamCalc();
                        break;
                    case "quit": //Sortie de la boucle de saisie
                    case "exit": //Sortie de la boucle de saisie
                        exitOk = true;
                        break;
                    default : //Traitement du calcul de la formule
                        formule = formule.Replace('.', ','); //Remplace les '.' par des ','
                        memFormule.Add(formule); //enregistrement de la formule
                        nbMem ++;//incrémentation de l'index de la mémoire
                        int c = 30;
                        int l = Console.CursorTop - 1;
                        string resultat = CalculFormule(formule); //Calcul de la formule
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

        //Fonction de calcul de formule
        static string CalculFormule(string maFormule)
        {
            //Déclaration des varibales
            string formuleGauche, formuleDroite;
            int pos;

            //Traitement des parenthesess
            //---------------------------
            //---------------------------
            if (NbCaracChaine('(', maFormule) == NbCaracChaine(')',maFormule) && NbCaracChaine('(',maFormule) != 0)
            {
                int posParaOuverte = maFormule.IndexOf('(') + 1;
                int posParaFerme = maFormule.LastIndexOf(')') +1;
                string formuleG = maFormule.Substring(0, posParaOuverte - 1);
                string formuleM = maFormule.Substring(posParaOuverte, posParaFerme - posParaOuverte-1);
                string formuleD = maFormule.Substring(posParaFerme);
                maFormule = formuleG + CalculFormule(formuleM) + formuleD;
            }
            //Traitement de opérateurs
            //------------------------
            //------------------------

            //Opérateurs "-"
            pos = maFormule.IndexOf('-', 1) + 1;
            if (pos > 1)
            {
                //Gére les situation du genre 2*-4 ou 7/-4 où le signe "-" suit un opérateur et ne doit être traiter comme un opérateur mathèmatique
                string t = "0";
                if (pos>2)
                    t = maFormule.Substring(pos-2, 1);
                if (t != "*" && t != "/" && t != "+" && t != "-")
                {
                    formuleGauche = maFormule.Substring(0, pos - 1);
                    formuleDroite = maFormule.Substring(pos);
                    maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) - Convert.ToDouble(CalculFormule(formuleDroite))));
                }
            }

            //Opérateurs "+"
            pos = maFormule.IndexOf('+') + 1;
            if (pos > 0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                //Console.WriteLine(pos + " " + formuleGauche + " " + formuleDroite);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) + Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateurs "*"
            pos = maFormule.IndexOf('*') + 1;
            if (pos > 0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) * Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateurs "/"
            pos = maFormule.IndexOf('/') + 1;
            if (pos > 0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) / Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateur "sqr" : racine carrée
            pos = maFormule.IndexOf("sqr") + 1;
            if (pos > 0)
            {
                //formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos + 2);
                maFormule = CalculFormule(Convert.ToString(Math.Sqrt(Convert.ToDouble(CalculFormule(formuleDroite)))));
            }
            return maFormule;
        }

        //Fonction qui compte le nombre ou le caractère charSeach est présent dans la chaineRef
        static int NbCaracChaine(char charSeach, string chaineRef)
        {
            int nbCarac = 0;
            for (int i = 0; i < chaineRef.Length; i++)
            {
                if (chaineRef[i] == charSeach)
                    nbCarac++;
            }
            return nbCarac;
        }

        //Fonction de présentation de l'appication
        static void PresICyamCalc()
        {
            Console.Clear();
            Console.WriteLine("ICyamCalc v 0.03a");
            Console.WriteLine("----------------------------------------------------------------------------------");
        }
    }
}

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
                    case "exit": //Sortie de la boucle de saisie
                        exitOk = true;
                        break;
                    default : //Traitement du calcul de la formule
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



            //Traitement de opérateurs
            //------------------------
            //------------------------
            //Opérateurs "+"
            pos = PosChar('+', maFormule);
            if (pos>0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                //Console.WriteLine(pos + " " + formuleGauche + " " + formuleDroite);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) + Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateurs "-"
            pos = PosChar('-', maFormule);
            if (pos > 1)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) - Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateurs "*"
            pos = PosChar('*', maFormule);
            if (pos > 0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) * Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            //Opérateurs "/"
            pos = PosChar('/', maFormule);
            if (pos > 0)
            {
                formuleGauche = maFormule.Substring(0, pos - 1);
                formuleDroite = maFormule.Substring(pos);
                maFormule = CalculFormule(Convert.ToString(Convert.ToDouble(CalculFormule(formuleGauche)) / Convert.ToDouble(CalculFormule(formuleDroite))));
            }

            return maFormule;
        }

        //Fonction de position d'une caractère dans une chaine
        static int PosChar(char charSeach, string chaineRef)
        {
            int position = 0;
            //Boucle sur la longueur de la chaine
            for (int i=0;i<chaineRef.Length;i++)
            {
                if (chaineRef[i] == charSeach)
                    position = i +1;
            }
            return position;
        }

        //Fonction de position d'une caractère dans une chaine Inverse
        static int PosCharInv(char charSeach, string chaineRef)
        {
            int position = chaineRef.Length;
            //Boucle sur la longueur de la chaine
            for (int i = chaineRef.Length; i < 0; i--)
            {
                if (chaineRef[i] == charSeach)
                    position = i + 1;
            }
            return position;
        }

        //Fonction de présentation de l'appication
        static void PresICyamCalc()
        {
            Console.WriteLine("ICyamCalc v 0.01a");
            Console.WriteLine("----------------------------------------------------------------------------------");
        }
    }
}

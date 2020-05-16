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
            List<string> memInstruction = new List<string>(); //Enregistrer les instructions en mémoire
            int nbMem = 0; //Nombre d'instruction en mémoire
            do
            {
                Console.Write(nbMem + 1 + ">");
                string instructionBrute = Console.ReadLine();

                Instruction instruction = new Instruction(instructionBrute); //Instance de l'instruction brute
                //instruction.Analyse(); //Analyse de la chaine
                instruction.MemoireInFormule(memInstruction); //Inclusion des mémoire eventuelle

                bool exitOk = false;
                string maFormule = instruction.FormuleACalculer();
                switch (maFormule)
                {
                    case "cls": //Efface l'écran
                        Console.Clear();
                        PresICyamCalc();
                        break;
                    case "quit": //Sortie de la boucle de saisie
                    case "exit": //Sortie de la boucle de saisie
                        exitOk = true;
                        break;
                    case "Err001":
                        Console.WriteLine("Erreur : Fin d'instruction \";\" manquante");
                        break;
                    case "Err002":
                        Console.WriteLine("Erreur : Erreur d'écriture dans les inclusions de mémoire");
                        break;
                    default : //Traitement du calcul de la formule
                        maFormule = maFormule.Replace('.', ','); //Remplace les '.' par des ','
                        memInstruction.Add(instructionBrute); //enregistrement de l'instruction saisie
                        nbMem ++;//incrémentation de l'index de la mémoire
                        int c = 50;
                        int l = Console.CursorTop - 1;
                        string resultat = instruction.CalculFormule(); //Calcul de la formule
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
            Console.WriteLine("ICyamCalc v 0.05a");
            Console.WriteLine("--------------------------------------------------------------------------------");
        }
    }
}

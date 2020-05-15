using System;
using System.Collections.Generic;
using System.Text;

namespace ICyamCalc.Objets
{
    class Instruction
    {

        //Propriétés
        //************************************************************************************
        private string chaineInstruction { get; set; }
        private string chaineFormule { get; set; }
        private string chaineParametre { get; set; }
        private string chaineNote { get; set; }

        //constructeur
        //*************************************************************************************
        public Instruction(string monInstruction)
        {
            this.chaineInstruction = monInstruction.ToLower();
            this.Analyse();
        }

        //Methodes
        //*************************************************************************************

        //Méthode pour lire l'instruction a calculer les propriété
        public String FormuleACalculer()
        {
            return this.chaineFormule;
        }

        /*
         Cette méthode vas analyser la chaine de caratére que représente toute l'instruction
         et extraire 3 éléments
         1. L'instruction de calcul en premier dans la chaine et qui fini par ';'
         2. le paramétres de calcul que se trouve juste après l'instruction et qui est entouré de '*' Ex *xxx*
         3. la remarque de l'instruction, qui commence par "'"
        */
        private void Analyse()
        {
            //L'instruction a calculer
            int posEndFormule = chaineInstruction.IndexOf(';');
            if (posEndFormule > 0)
                chaineFormule = chaineInstruction.Substring(0, posEndFormule);
            else
                chaineFormule = "Err001";

            //Le paramètre de calcul
            int posFirstDollar = chaineInstruction.IndexOf('$');
            chaineParametre = "";
            if (posFirstDollar > 0)
            {
                int posSecondDollar = chaineInstruction.IndexOf('$', posFirstDollar + 1);
                chaineParametre = chaineInstruction.Substring(posFirstDollar, posSecondDollar - posFirstDollar);
            }

            //Note de l'instruction
            int posApostrophe = chaineInstruction.IndexOf('\'');
            chaineNote = "";
            if (posApostrophe > 0)
                chaineNote = chaineInstruction.Substring(posApostrophe, chaineInstruction.Length - posApostrophe);
        }

        /*
         Cette méthode va simplement remplacer les #xxx# dans l'instruction de calcul par les formules
         correspondantes en mémoire
         rq : faire appel a une nouvelle instance de l'objet Instruction pour extraire l'instruction de calcul seule "chaineFormule"
        */
        public void MemoireInFormule(List<string> memInstructions)
        {   
            int nbDiese = NbCaracChaine('#');
            if (nbDiese > 0)
            {
                if (nbDiese % 2 != 0) // Nombre de diesse pas divisible par deux
                    chaineFormule = "Err002";
                else
                {
                    //Code d'inclusion
                    //****************
                    int posFirstDiese = chaineFormule.IndexOf('#');
                    int posSecondDiese = chaineFormule.IndexOf('#', posFirstDiese + 1);
                    string indexInMemoire = chaineFormule.Substring(posFirstDiese + 1, posSecondDiese - (posFirstDiese + 1));
                    int memoireIn = Convert.ToInt32(indexInMemoire);
                    string instructionInMemoire = memInstructions[memoireIn - 1];
                    var instructionAInclure = new Instruction(instructionInMemoire);
                    //instructionAInclure.Analyse();
                    string formuleAInclure = instructionAInclure.FormuleACalculer();
                    if (memoireIn <= 0 || memoireIn > memInstructions.Count)
                        chaineFormule = "Err003"; //si la memoire n'existe pas
                    else
                    {
                        chaineFormule = chaineFormule.Substring(0, posFirstDiese) + "(" + formuleAInclure + ")" + chaineFormule.Substring(posSecondDiese + 1, chaineFormule.Length - (posSecondDiese + 1))+";";
                        var instruction = new Instruction(chaineFormule); // On recréer un instance de l'objet avec la nouvelle chaine formule
                        instruction.MemoireInFormule(memInstructions); // on relance la méthode
                    }
                }
                this.chaineFormule = this.FormuleACalculer();
            }
        }

        /*Fonction de calcul de formule
         Cette fonvtion permet d'analyser et de calculer l'opération présente dans 'chaineForme' de l'objet.
         Elle utilise la recursivité et analyse opérateur par opérateur.
         et renvois le résultat sous la forme d'un string
        */
        public string CalculFormule()
        {
            //Déclaration des varibales
            //string formuleGauche, formuleDroite;
            int pos;
            string maFormule = chaineFormule;

            //Traitement des parenthesess
            //---------------------------
            //---------------------------
            if (NbCaracChaine('(') == NbCaracChaine(')') && NbCaracChaine('(') != 0)
            {
                int posParenOpen = maFormule.IndexOf('(');
                int posParenClose = 0;
                int testParenOpen = 1;
                for (int i = posParenOpen + 1; i < maFormule.Length; i++)
                {
                    if (maFormule[i] == '(')
                        testParenOpen++;
                    if (maFormule[i] == ')')
                        testParenOpen--;
                    if (testParenOpen == 0)
                    {
                        posParenClose = i;
                        break;
                    }
                }
                Formule formuleG = new Formule(maFormule.Substring(0, posParenOpen));
                Formule formuleM = new Formule(maFormule.Substring(posParenOpen + 1, posParenClose - posParenOpen - 1));
                Formule formuleD = new Formule(maFormule.Substring(posParenClose + 1));
                maFormule = formuleG.TexteFormule() + formuleM.CalculFormule() + formuleD.TexteFormule();
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
                if (pos > 2)
                    t = maFormule.Substring(pos - 2, 1);
                if (t != "*" && t != "/" && t != "+" && t != "-" && t.ToUpper() != "E")
                {
                    Formule formuleGauche = new Formule(maFormule.Substring(0, pos - 1));
                    Formule formuleDroite = new Formule(maFormule.Substring(pos));
                    Formule newFormule = new Formule(Convert.ToString(Convert.ToDouble(formuleGauche.CalculFormule()) - Convert.ToDouble(formuleDroite.CalculFormule())));
                    maFormule = newFormule.CalculFormule();
                }
            }

            //Opérateurs "+"
            pos = maFormule.IndexOf('+') + 1;
            if (pos > 0)
            {
                string t = "0";
                if (pos > 2)
                    t = maFormule.Substring(pos - 2, 1);
                if (t.ToUpper() != "E")
                {
                    Formule formuleGauche = new Formule(maFormule.Substring(0, pos - 1));
                    Formule formuleDroite = new Formule(maFormule.Substring(pos));
                    Formule newFormule = new Formule(Convert.ToString(Convert.ToDouble(formuleGauche.CalculFormule()) + Convert.ToDouble(formuleDroite.CalculFormule())));
                    maFormule = newFormule.CalculFormule();
                }
            }

            //Opérateurs "*"
            pos = maFormule.IndexOf('*') + 1;
            if (pos > 0)
            {
                Formule formuleGauche = new Formule(maFormule.Substring(0, pos - 1));
                Formule formuleDroite = new Formule(maFormule.Substring(pos));
                Formule newFormule = new Formule(Convert.ToString(Convert.ToDouble(formuleGauche.CalculFormule()) * Convert.ToDouble(formuleDroite.CalculFormule())));
                maFormule = newFormule.CalculFormule();
            }

            //Opérateurs "/"
            pos = maFormule.IndexOf('/') + 1;
            if (pos > 0)
            {
                Formule formuleGauche = new Formule(maFormule.Substring(0, pos - 1));
                Formule formuleDroite = new Formule(maFormule.Substring(pos));
                Formule newFormule = new Formule(Convert.ToString(Convert.ToDouble(formuleGauche.CalculFormule()) / Convert.ToDouble(formuleDroite.CalculFormule())));
                maFormule = newFormule.CalculFormule();
            }

            //Opérateurs "^" - Opérateur de Puissance
            pos = maFormule.IndexOf('^') + 1;
            if (pos > 0)
            {
                Formule formuleGauche = new Formule(maFormule.Substring(0, pos - 1));
                Formule formuleDroite = new Formule(maFormule.Substring(pos));
                Formule newFormule = new Formule(Convert.ToString(Math.Pow(Convert.ToDouble(formuleGauche.CalculFormule()), Convert.ToDouble(formuleDroite.CalculFormule()))));
                maFormule = newFormule.CalculFormule();
            }

            //Opérateur "sqr" : racine carrée
            if (maFormule.Length >= 4)
            {
                pos = maFormule.IndexOf("sqr") + 1;
                if (pos > 0)
                {
                    Formule formuleDroite = new Formule(maFormule.Substring(pos + 2));
                    Formule newFormule = new Formule(Convert.ToString(Math.Sqrt(Convert.ToDouble(formuleDroite.CalculFormule()))));
                    maFormule = newFormule.CalculFormule();
                }
            }
            return maFormule;
        }

        //Fonction qui compte le nombre ou le caractère charSeach est présent dans la chaineRef
        public int NbCaracChaine(char charSeach)
        {
            int nbCarac = 0;
            for (int i = 0; i < this.chaineFormule.Length; i++)
            {
                if (this.chaineFormule[i] == charSeach)
                    nbCarac++;
            }
            return nbCarac;
        }
    }
}

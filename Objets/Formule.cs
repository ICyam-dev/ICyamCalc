using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ICyamCalc.Objets
{
    class Formule
    {
        //Propriété
        //************************************************************************************
        private string chaineFormule { get; set; }
        private string uniteAngle { get; set; }
        
        //Constructeur
        //************************************************************************************
        public Formule(string texteFormule, string angleTrigo = "rad")
        {
            chaineFormule = texteFormule.ToLower();
            uniteAngle = angleTrigo;
        }

        //Methodes
        //************************************************************************************

        //Methode pour récupérer l'unité angulaire pour les calculs trigo
        public string UniteAngle()
        {
            return this.uniteAngle;
        }

        //Methode pour modifier l'unite angulaire pour les calculs trigo
        public void NewUniteAngle(string newUnitAngle)
        {
            this.uniteAngle = newUnitAngle;
        }

        //Methode pour récupérer le texte de la formule
        public string TexteFormule()
        {
            return this.chaineFormule;
        }

        //Methode pour remplacer la formule
        public void RemplaceFormule(string nouvelleFormule)
        {
            this.chaineFormule = nouvelleFormule;
        }

        //Méthode d'analyse du diese présent dans la formule
        public string AnalyseDieses()
        {
            string maFormule = chaineFormule;

            return maFormule;
        }

        //Methode de calcul de formule
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
                if (t.ToUpper() != "E" )
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

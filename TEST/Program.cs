using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace testMiniProjet
{
    class Program
    {
        static void Main(string[] args)
        {
            Etudiant std1 = new Etudiant(10000, 120, "nom1", "prenom1", 10, "ginf");
            Etudiant std2 = new Etudiant(20000, 150, "nom2", "prenom2", 77, "ginf");

            std1.save();
            std2.save("ajouter_etudiant");

            /*Etudiant.ConvertToEtudiant(std1.find()).afficher();
            Etudiant.ConvertToEtudiant(Etudiant.find<Etudiant>(20000)).afficher();*/




            /*std2.update("nom", "XXXX");
            Etudiant.ConvertToEtudiant(Etudiant.find<Etudiant>(20000)).afficher();*/





            /*List<dynamic> L = Etudiant.All<Etudiant>();
            List<Etudiant> Etudiants = Etudiant.ListConvertToEtudiant(L);
            foreach (var et in Etudiants)
            {
                et.afficher();
            }*/




            Dictionary<string, object> dico = new Dictionary<string, object>();
            dico.Add("id", 10000);





            /*List<dynamic> L1 = std1.Select(dico);
            List<Etudiant> Etudiants1 = Etudiant.ListConvertToEtudiant(L1);

            foreach (var et in Etudiants1)
            {
                et.afficher();
            }*/





           /* List<dynamic> L2 = Etudiant.Select<Etudiant>(dico);
            List<Etudiant> Etudiants2 = Etudiant.ListConvertToEtudiant(L2);
            foreach (var et in Etudiants2)
            {
                et.afficher();
            }*/




            /*std1.delete();
            std2.delete("supprimer_etudiant");*/



            Console.ReadKey();
    }
    }
}

using DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMiniProjet
{
    public class Etudiant : Model
    {

        public Etudiant()
        {

        }

        public int code { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public int niveau { get; set; }
        public string code_Fil { get; set; }


        public void setCode(int code)
        {
            this.code = code;
        }
        public Etudiant(int id, int code, string nom, string prenom, int niveau, string code_Fil)
        {
            this.id = id;

            this.code = code;
            this.code_Fil = code_Fil;
            this.niveau = niveau;
            this.nom = nom;
            this.prenom = prenom;

        }

        public override string ToString()
        {
            return "nom: " + nom + " code " + code + "  id " + id;
        }

        public void afficher()
        {
            Console.WriteLine("Id: " + id + "  Code: " + code + "  nom: " + nom + "   prenom: " + prenom + "niveau: " + niveau + "   Code_Fil: " + code_Fil);
        }

        public static Etudiant ConvertToEtudiant(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Etudiant etudiant = JsonConvert.DeserializeObject<Etudiant>(json);

            return etudiant;
        }

        public static List<Etudiant> ListConvertToEtudiant(List<dynamic> L)
        {

            List<Etudiant> Result = new List<Etudiant>();
            //Console.WriteLine(L.Count);
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Etudiant.ConvertToEtudiant(student));
                
            }
            

            return Result;
        }
    }
}

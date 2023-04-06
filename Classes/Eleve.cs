using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;

namespace Test_MiniProjet
{
    public class Eleve : Model
    {
     
        public Eleve()
        {

        }
      
     
        public string nom { get; set; }
        public string prenom { get; set; }
        public int niveau { get; set; }
        public string code_fil { get; set; }


        public void setCode(string code)
        {
            this.code = code;
        }
        public Eleve(string code, string nom, string prenom, int niveau, string code_fil)
        {
                this.code = code;
                this.niveau = niveau;
                this.nom = nom;
                this.prenom = prenom;
                this.code_fil = code_fil;

        }

        

        public static Eleve ConvertToEleve(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Eleve Eleve = JsonConvert.DeserializeObject<Eleve>(json);

            return Eleve;
        }

        public static List<Eleve> ListConvertToEleve(List<dynamic> L)
        {

            List<Eleve> Result = new List<Eleve>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Eleve.ConvertToEleve(student));

            }

            return Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;
namespace Test_MiniProjet
{
    class Moyennes :Model
    {
        //Moyennes(id, #code_eleve,#code_fil, niveau, moyenne
        public string code_eleve { get; set; }
        public string code_fil { get; set; }
        public int niveau { get; set; }
        public double moyenne { get; set; }

        public Moyennes() { }

        public Moyennes(int id, string code_eleve, string code_fil, int niveau, float moyenne )
        {
            this.id = id;
            this.code_eleve = code_eleve;
            this.code_fil = code_fil;
            this.niveau = niveau;
            this.moyenne = moyenne;
        }
        public static Moyennes ConvertToMoyennes(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Moyennes Moyennes = JsonConvert.DeserializeObject<Moyennes>(json);

            return Moyennes;
        }

        public static List<Moyennes> ListConvertToMoyennes(List<dynamic> L)
        {

            List<Moyennes> Result = new List<Moyennes>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Moyennes.ConvertToMoyennes(student));

            }

            return Result;
        }
    }
}

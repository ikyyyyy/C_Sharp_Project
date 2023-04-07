using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;
namespace Test_MiniProjet
{
    class Moyennes : Model
    {
        static int n = 100;
        //Moyennes(id, #code_eleve,#code_fil, niveau, moyenne
        public string code_eleve { get; set; }
        public string code_fil { get; set; }
        public int niveau { get; set; }
        public double moyenne { get; set; }

        public Moyennes() { }

        public Moyennes(string code_eleve, string code_fil, int niveau, float moyenne )
        {
            if (this.getMaxCode() == 0)
            {
                this.code = n;
                n++;

            }
            else this.code = this.getMaxCode() + 1;


            this.code_eleve = code_eleve;
            this.code_fil = code_fil;
            this.niveau = niveau;
            this.moyenne = moyenne;
            this.code_string = -1;
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

using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;


namespace Test_MiniProjet
{
    class Filiere : Model
    {
        
        public string designation { get; set; }
        
        public Filiere() { }
        public Filiere(string code, string nom) {
            

            this.code = code;
            this.designation = nom;
        }

        

        public static Filiere ConvertToFiliere(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Filiere Filiere = JsonConvert.DeserializeObject<Filiere>(json);

            return Filiere;
        }

        public static List<Filiere> ListConvertToFiliere(List<dynamic> L)
        {

            List<Filiere> Result = new List<Filiere>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Filiere.ConvertToFiliere(student));

            }

            return Result;
        }
    }
}

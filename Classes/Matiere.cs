using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;

namespace Test_MiniProjet
{
    class Matiere :Model
    {
        static int n = 100;
        //Matiere (id, code,designation, VH, #code_module)
        public string code { get; set; }
        public string designation { get; set; }
        public int VH { get; set; }
        public string code_module { get; set; }

        public Matiere() { }

        public Matiere(int id, string code, string designation, int VH, string code_module)
        {
            if (this.getMaxId() == 0)
            {
                this.id = n;
                n++;

            }

            else this.id = this.getMaxId() + 1;

            this.code = code;
            this.designation = designation;
            this.VH = VH;
            this.code_module = code_module;
            
        }
        public static Matiere ConvertToMatiere(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Matiere Matiere = JsonConvert.DeserializeObject<Matiere>(json);

            return Matiere;
        }

        public static List<Matiere> ListConvertToMatiere(List<dynamic> L)
        {

            List<Matiere> Result = new List<Matiere>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Matiere.ConvertToMatiere(student));

            }

            return Result;
        }
    }
}

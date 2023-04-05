using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json;

namespace Test_MiniProjet
{
    class Module :Model
    {
        public string code { get; set; }
        public string designation { get; set; }
        public int niveau { get; set; }
        public string semestre { get; set; }
        public string code_fil { get; set; }

        public Module()
        {

        }

        public Module(int id, string code, string designation, int niveau, string semestre, string code_fil)
        {
            this.id = id;
            this.code = code;
            this.designation = designation;
            this.niveau = niveau;
            this.semestre = semestre;
            this.code_fil = code_fil;
        }

        public static Module ConvertToModule(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Module Module = JsonConvert.DeserializeObject<Module>(json);

            return Module;
        }

        public static List<Module> ListConvertToModule(List<dynamic> L)
        {

            List<Module> Result = new List<Module>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Module.ConvertToModule(student));

            }

            return Result;
        }
    }
}

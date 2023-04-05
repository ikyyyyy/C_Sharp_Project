﻿using System;
using System.Collections.Generic;
using System.Text;
using MiniProjet;
using Newtonsoft.Json; 

namespace Test_MiniProjet
{
    class Notes : Model
    {
        static int n = 100;
        //Notes (id, #code_eleve,#code_mat, note)
        public string code_eleve { get; set; }
        public string code_mat { get; set; }
        public double note { get; set; }
      
        public Notes()
        {

        }

        public Notes(int id, string code_eleve, string code_mat, float note)
        {
            if (this.getMaxId() == 0)
            {
                this.id = n;
                n++;

            }

            else this.id = this.getMaxId() + 1;

            this.code_eleve = code_eleve;
            this.code_mat = code_mat;
            this.note = note;
        }
        public static Notes ConvertToNotes(dynamic obj)
        {

            string json = JsonConvert.SerializeObject(obj);

            Notes Notes = JsonConvert.DeserializeObject<Notes>(json);

            return Notes;
        }

        public static List<Notes> ListConvertToNotes(List<dynamic> L)
        {

            List<Notes> Result = new List<Notes>();
            if (L.Count == 0)
            {
                Console.WriteLine("List is empty");
            }
            foreach (var student in L)
            {

                Result.Add(Notes.ConvertToNotes(student));

            }

            return Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamExamTwoAIHR
{
    class Overworld
    {
        private List<Applicant> applicants;
        private const double ExperienceStretch = 20;
        private const int CoursesStretch = 7;

        private const string CurrentlyPresent =
            "currently available applicants:";


        private string[] Names =
        {
            "John Smith", "John Doe", "Jane Smith",
            "Jane Doe", "Mark Fishbach", "Sean McLoughlin", "Adam Montoya",
            "Brad Colburn", "Brooke Lawson", "John Bain"
        };

        public Overworld(int amount_of_applicants)
        {
            Random gen = new Random();
            applicants = new List<Applicant>();

            for (int i = 0; i < amount_of_applicants; i++)
            {
                string name = Names[gen.Next(Names.Length)];
                bool knowsHTML = (gen.Next(2) == 1);
                bool knowsSQL = (gen.Next(2) == 1);
                bool knowsJS = (gen.Next(2) == 1);
                double yearsOfExperience = gen.NextDouble()*ExperienceStretch;
                uint course = (uint)gen.Next(CoursesStretch);
                applicants.Add(new Applicant(name,
                    knowsHTML,
                    knowsSQL,
                    knowsJS,
                    yearsOfExperience,
                    course));
            }
        }

        public List<string> ListCVs()
        {
            List<string> CVs = new List<string>();
            foreach (Applicant person in applicants)
            {
                CVs.Add(person.GenerateCV());
            }
            return CVs;
        }

        public void OutputCVs()
        {
            Console.WriteLine(CurrentlyPresent);
            foreach (string cv in ListCVs())
            {
                Console.WriteLine(cv);
            }
        }
    }
}

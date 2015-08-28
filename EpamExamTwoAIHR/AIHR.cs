using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EpamExamTwoAIHR
{
    internal delegate T ParseNumber<T>(string input);
    class AIHR
    {

        private const string NoVacancy = "not hired due to lack of vacancies";
        private const string NotEnoughApplicants = "not enough applicants";
        private const string Hired = "hired:";
        private const string NotHired = "not hired:";
        private const string Unranked = "failed to reach any rank:";
        private const string ExperienceKeyword = "experience";
        private const string ExperienceRegex = @"(\d+,?\d?)\s";
        private const string CourseKeyword = "course";
        private const string CourseRegex = @"([1-6])(th|nd|rd|st)";

        private List<Ranks> priorities;
        private Dictionary<Ranks, Criterion> criteriaMap;
        private List<Tuple<string, Applicant>> hiredApplicants;
        private Dictionary<Ranks, List<Tuple<string, Applicant>>> rankedApplicants;
        private List<Tuple<string, Applicant>> unrankedApplicants;
        private List<Tuple<string, Applicant>> rejectedApplicants;

        public AIHR(List<Ranks> priorities_, Dictionary<Ranks, Criterion> criteria_map)  //TODO priority map, criteria map
        {
            priorities = priorities_;
            criteriaMap = criteria_map;
            hiredApplicants = new List<Tuple<string, Applicant>>();
            rankedApplicants = new Dictionary<Ranks, List<Tuple<string, Applicant>>>();
            foreach (Ranks rank in priorities)
            {
                rankedApplicants.Add(rank, new List<Tuple<string, Applicant>>());
            }
            unrankedApplicants = new List<Tuple<string, Applicant>>();
            rejectedApplicants = new List<Tuple<string, Applicant>>();
        }

        public void ProcessCVs(List<string> CVs)
        {
            foreach (string cv in CVs)
            {
                ProcessCV(cv);
            }
        }

        private void ProcessCV(string cv)
        {
            string name = cv.Split('\n')[0];
            bool knowsHTML = cv.Contains(Applicant.knowsHTMLCVEntry);
            bool knowsSQL = cv.Contains(Applicant.knowsSQLCVEntry);
            bool knowsJS = cv.Contains(Applicant.knowsJSCVEntry);
            double yearsOfExperience = InitVariableByRegex(cv, CourseKeyword, CourseRegex,
                double.Parse);
            uint course = InitVariableByRegex(cv, CourseKeyword, CourseRegex,
                uint.Parse);
            RankApplicant(name, knowsHTML, knowsSQL, knowsJS,
                yearsOfExperience, course);

        }

        private T InitVariableByRegex<T>(string cv, string keyword,
            string regex, ParseNumber<T> parser) where T: new()
        {
            T value = new T();
            if (cv.Contains(CourseKeyword))
            {
                Regex capturer = new Regex(CourseRegex);
                Match match = capturer.Match(cv);
                if (match.Success)
                {
                    value = parser(match.Groups[1].Captures[0].Value);
                }
            }
            return value;
        }

        private void RankApplicant(string name, bool knowsHTML, bool knowsSQL,
            bool knowsJS, double yearsOfExperience, uint course)
        {
            List<Ranks> ranks = new List<Ranks>();
            foreach (Ranks rank in criteriaMap.Keys)
            {
                if (criteriaMap[rank].CheckCriterion(knowsHTML, knowsSQL,
                    knowsJS, yearsOfExperience, course)) 
                {
                    ranks.Add(rank);
                }
            }
            Applicant applicant = new Applicant(name, knowsHTML, knowsSQL,
                knowsJS, yearsOfExperience, course);
            if (ranks.Count > 0)
            {
                Ranks max_rank =
                    (Ranks)ranks.Select(item => (int)item).Max();
                string report = criteriaMap[max_rank].GenerateReport(
                    knowsHTML, knowsSQL, knowsJS, yearsOfExperience, course);
                rankedApplicants[max_rank].Add(Tuple.Create(report, applicant));
            }
            else
            {
                Ranks min_rank = (Ranks)0;

                string report = criteriaMap[min_rank].GenerateReport(
                    knowsHTML, knowsSQL, knowsJS, yearsOfExperience, course);
                unrankedApplicants.Add(Tuple.Create(report, applicant));
            }
        }

        public void HireApplicants(int amount)
        {
            int places_left = amount;

            foreach (Ranks rank in priorities)
            {
                for (; places_left > 0; places_left--)
                {
                    try
                    {
                        hiredApplicants.Add(rankedApplicants[rank].Last());
                        rankedApplicants[rank].RemoveAt(
                            rankedApplicants[rank].Count - 1);
                    }
                    catch (InvalidOperationException)
                    {
                        
                        break;
                    }
                }
                if (places_left == 0) break;
            }

            if (places_left == 0)
            {
                RegenerateRejectReports();
            }
            else
            {
                Console.WriteLine(NotEnoughApplicants);
            }
        }

        private void RegenerateRejectReports()
        {
            foreach (Ranks rank in rankedApplicants.Keys)
            {
                foreach (var applicant in rankedApplicants[rank])
                {
                    StringBuilder report =
                        new StringBuilder(applicant.Item1);
                    report.AppendLine(NoVacancy);
                    rejectedApplicants.Add(Tuple.Create(
                        report.ToString(), applicant.Item2));
                }
            }
        }

        public void AnnounceEmployees()
        {
            Console.WriteLine(Hired);
            foreach (var applicant in hiredApplicants)
            {
                Console.WriteLine(applicant.Item2.name);
            }
        }

        public void ReportRejects()
        {
            Console.WriteLine(NotHired);
            foreach (var applicant in rejectedApplicants)
            {
                Console.WriteLine(applicant.Item2.name);
                Console.WriteLine(applicant.Item1);
            }
            Console.WriteLine(Unranked);
            foreach (var applicant in unrankedApplicants)
            {
                Console.WriteLine(applicant.Item2.name);
                Console.WriteLine(applicant.Item1);
            }
        }
    }

    enum Ranks
    {
        Senior = 3,
        Middle = 2,
        Junior = 1,
        Student = 0,
    } //arbitrary integer ranks
}

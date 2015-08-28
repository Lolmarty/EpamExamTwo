using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamExamTwoAIHR
{
    class Program
    {
        static void Main(string[] args)
        {
            Overworld world = new Overworld(15);

            world.OutputCVs();

            Dictionary<Ranks,Criterion> criteria = new Dictionary<Ranks, Criterion>();
            criteria.Add(Ranks.Student,
                new Criterion( true, false, false, 0, 5));
            criteria.Add(Ranks.Junior,
                new Criterion( true, true, false, 0, 0));
            criteria.Add(Ranks.Middle,
                new Criterion( true, true, true, 2, 0));
            criteria.Add(Ranks.Senior,
                new Criterion( true, true, true, 5, 0));

            List<Ranks> priorities = new List<Ranks>(){Ranks.Middle,Ranks.Senior,Ranks.Junior,Ranks.Student};
            
            AIHR hr = new AIHR(priorities, criteria);
            hr.ProcessCVs(world.ListCVs());
            hr.HireApplicants(3);
            hr.AnnounceEmployees();
            hr.ReportRejects();

            Console.ReadLine();
        }
    }
}

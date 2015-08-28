using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamExamTwoAIHR
{
    class HumanBaseClass
    {
        
        public bool knowsHTML { get; set; }
        public bool knowsSQL { get; set; }
        public bool knowsJS { get; set; }
        public double yearsOfExperience { get; set; }
        public uint course { get; set; }

        public HumanBaseClass()
        {
            
        }

        public HumanBaseClass(bool knows_html, bool knows_sql,
            bool knows_js, double years_of_experience, uint course_)
        {
                knowsHTML = knows_html;
                knowsSQL = knows_sql;
                knowsJS = knows_js;
                yearsOfExperience = Math.Max(years_of_experience, 0);
                course = course_;
        }
        
    }

    class Applicant: HumanBaseClass
    {
        public string name { get; set; }

        public const string languagesEntry = "available languages:";
        public const string knowsHTMLCVEntry = "HTML";
        public const string knowsSQLCVEntry = "SQL";
        public const string knowsJSCVEntry = "JS";
        private const string yearsOfExperienceCVEntry =
            "{0:f1} years of experience";
        private const string courseCVEnrty = "currently on {0}th course";

        public Applicant(string name_, bool knows_html, bool knows_sql,
            bool knows_js, double years_of_experience, uint course_)
            : base(
                knows_html, knows_sql, knows_js, years_of_experience,
                course_)
        {
            name = name_;
        }
        

        public string GenerateCV()
        {
            StringBuilder cvBuilder = new StringBuilder(name);
            cvBuilder.AppendLine();
            if (knowsHTML || knowsSQL || knowsJS)
                cvBuilder.AppendLine(languagesEntry);
            if (knowsHTML) cvBuilder.AppendLine(knowsHTMLCVEntry); 
            if (knowsSQL) cvBuilder.AppendLine(knowsSQLCVEntry);
            if (knowsJS) cvBuilder.AppendLine(knowsJSCVEntry);
            if (yearsOfExperience > 0)
                cvBuilder.AppendLine(String.Format(yearsOfExperienceCVEntry,
                    yearsOfExperience));
            if (course > 0)
                cvBuilder.AppendLine(String.Format(courseCVEnrty, course));
            return cvBuilder.ToString();
        }
    }

    class Criterion : HumanBaseClass
    {
        private const string reportHTMLEntryFail = "doesn\'t know HTML";
        private const string reportHTMLEntry = "knows HTML";
        private const string reportSQLEntryFail = "doesn\'t know SQL";
        private const string reportSQLEntry = "knows SQL";
        private const string reportJSEntryFail = "doesn\'t know JS";
        private const string reportJSEntry = "knows JS";
        private const string reportExperienceEntryFail = "insufficient experience";
        private const string reportExperienceEntry = "sufficient experience";
        private const string reportCourseEntryFail = "currently on a too early course";
        private const string reportCourseEntry = "currently on an appropriate course";

        public Criterion(bool knows_html, bool knows_sql,
            bool knows_js, double years_of_experience, uint course_)
            : base(
                knows_html, knows_sql, knows_js, years_of_experience,
                course_)
        {
            
        }

        public bool CheckCriterion(bool knows_html, bool knows_sql,
            bool knows_js, double years_of_experience, uint course_)
        {
            return (this.knowsHTML == knows_html &&
                this.knowsSQL == knows_sql &&
                this.knowsJS == knows_js &&
                this.yearsOfExperience <= years_of_experience &&
                this.course <= course_);
        }

        public string GenerateReport(bool knows_html, bool knows_sql,
            bool knows_js, double years_of_experience, uint course_)
        {
            StringBuilder report = new StringBuilder();
            if (this.knowsHTML == knows_html)
            {
                report.AppendLine(reportHTMLEntry);
            }
            else
            {
             report.AppendLine(reportHTMLEntryFail);   
            }
            if (this.knowsSQL == knows_sql)
            {
                report.AppendLine(reportSQLEntry);
            }
            else
            {
             report.AppendLine(reportSQLEntryFail);   
            }
            if (this.knowsJS == knows_js)
            {
                report.AppendLine(reportJSEntry);
            }
            else
            {
             report.AppendLine(reportJSEntryFail);   
            }
            if (this.yearsOfExperience <= years_of_experience)
            {
                report.AppendLine(reportExperienceEntry);
            }
            else
            {
             report.AppendLine(reportExperienceEntryFail);   
            }
            if (this.course <= course_)
            {
                report.AppendLine(reportCourseEntry);
            }
            else
            {
             report.AppendLine(reportCourseEntryFail);   
            }
            return report.ToString();
        }
    }
}

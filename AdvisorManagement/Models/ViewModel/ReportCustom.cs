using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace AdvisorManagement.Models.ViewModel
{
    public class ReportCustom
    {
        public PlanClass PlanClass { get; set; }
        public string group_file { get; set; }
        public int  count { get; set; }
        public string status { get; set; }
        public int id { get; set; }
        public Nullable<int> year { get; set; }
        public Nullable<int> number_title { get; set; }
        public string content { get; set; }
        public string hk1 { get; set; }
        public string hk2 { get; set; }
        public string hk3 { get; set; }
        public string describe { get; set; }
        public string source { get; set; }
        public string note { get; set; }
        public Nullable<int> id_class { get; set; }
        public string evaluate { get; set; }

        public ReportCustom(PlanClass plan , int cou, string file, string statusMess) {
            this.group_file = file;
            this.PlanClass = plan;
            this.count = cou;
            this.status = statusMess;
            // base

            this.number_title = plan.number_title;
            this.content = plan.content;
            this.describe= plan.describe;
            this.source = plan.source;
            this.note = plan.note;
            this.evaluate = plan.evaluate;

        }
    }
}
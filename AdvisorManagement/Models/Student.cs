//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AdvisorManagement.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public string student_code { get; set; }
        public string id_class { get; set; }
        public string training_point { get; set; }
        public string grade_training_point { get; set; }
        public Nullable<int> status_id { get; set; }
        public Nullable<int> account_id { get; set; }
    
        public virtual AccountUser AccountUser { get; set; }
    }
}

using AdvisorManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using AdvisorManagement.Middleware;
using System.Web;
using System.Data.Entity;

namespace AdvisorManagement.Middleware
{
    public class StudentsMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();
        private AccountMiddleware serviceAccount = new AccountMiddleware();
        public string getRoles(string user_email)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_email);
            if (sql != null)
            {
                return db.Role.SingleOrDefault(x => x.id == sql.id_role).role_name;
            }
            return db.Role.SingleOrDefault(x => x.id == 3).role_name;
        }

        public int getRolesUser(int? id)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.id == id);
            if (sql != null)
            {
                return db.Role.SingleOrDefault(x => x.id == sql.id_role).id;
            }
            return db.Role.SingleOrDefault(x => x.id == 3).id;
        }
        public string getClassCode(string user_email)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_email);
            if (sql != null)
            {
                return db.Student.SingleOrDefault(x => x.student_code == sql.user_code).id_class;
            }
            return null;
        }
        public object getClass(string user_email)
        {
            var classAdvisor = (from pq in db.AccountUser
                                join ad in db.Advisor on pq.user_code equals ad.advisor_code
                                join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                                where pq.email == user_email && ad.advisor_code == cl.advisor_code
                                select new Models.ViewModel.AdvisorClass
                                {
                                    idClass = cl.class_code,
                                    idAdvisor = ad.advisor_code,
                                    name = pq.user_name,
                                    amount = 20
                                }).OrderBy(x => x.idClass).ToList();
            if (classAdvisor.Count() != 0)
            {
                return classAdvisor;
            }
            return null;
        }
        public object getStudent(string class_code)
        {
            var classStudent = (from std in db.Student
                                join acc in db.AccountUser on std.student_code equals acc.user_code
                                join cl in db.VLClass on std.id_class equals cl.class_code
                                join st in db.StudentStatus on std.status_id equals st.id
                                where std.id_class == class_code && std.student_code == acc.user_code
                                select new Models.ViewModel.ListStudent
                                {
                                    id = acc.id,
                                    idStudent = std.student_code,
                                    name = acc.user_name,
                                    email = acc.email,
                                    phone = acc.phone,
                                    course = 25,
                                    status = st.status_name
                                }).OrderBy(x => x.name).ToList();
            return classStudent;
        }
        public object getInfoClass(string class_code)
        {
            var classInfo = (from pq in db.AccountUser
                             join ad in db.Advisor on pq.user_code equals ad.advisor_code
                             join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                             where ad.advisor_code == cl.advisor_code && cl.class_code == class_code
                             select new Models.ViewModel.AdvisorClass
                             {
                                 idClass = cl.class_code,
                                 idAdvisor = ad.advisor_code,
                                 name = pq.user_name,
                                 email = pq.email,
                                 phone = pq.phone,
                             }).OrderBy(x => x.idClass).ToList();
            return classInfo;
        }

        public bool getPermission(string classCode, string user_email)
        {
            if (db.VLClass.Where(x => x.class_code.Equals(classCode)).Count() == 0)
            {
                return false;
            }
            else
            {
                var user_code = db.AccountUser.FirstOrDefault(x => x.email == user_email).user_code;
                var advisor_code = db.VLClass.FirstOrDefault(x => x.class_code.Equals(classCode)).advisor_code;
                if (advisor_code == user_code)
                {
                    return true;
                }
            }
            return false;
        }

        public bool saveAccount(String mssv, String name, String phone, String email, String address, Object gender, CP25Team09Entities db)
        {
            var result = false;
            try
            {
                var item = new AccountUser();
                item.id = serviceAccount.getID() + 1;
                item.id_role = 3;
                item.user_code = mssv;
                item.user_name = name;
                item.phone = phone;
                item.email = email;
                item.address = address;
                item.gender = getGender(gender.ToString());
                db.AccountUser.Add(item);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {


            }
            return result;
        }

        public bool saveStudent(String mssv, String id_class, String status, CP25Team09Entities db)
        {
            var result = false;
            try
            {
                var account_id = db.AccountUser.FirstOrDefault(x => x.user_code.Equals(mssv)).id;
                var status_id = db.StudentStatus.FirstOrDefault(x => x.status_name.Equals(status)).id;
                var item = new Student();
                item.student_code = mssv;
                item.id_class = id_class;
                item.status_id = status_id;
                item.account_id = account_id;
                db.Student.Add(item);
                db.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {


            }
            return result;
        }

        public void UpdateAccount(String mssv, String name, String phone, String address, String gender, CP25Team09Entities db)
        {
            try
            {
                var account_id = db.AccountUser.FirstOrDefault(x => x.user_code.Equals(mssv)).id;
                AccountUser user = db.AccountUser.Find(account_id);
                user.user_name = name;
                user.phone = phone;
                user.address = address;
                user.gender = getGender(gender.ToString());
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateStudentImport(String mssv, String status, CP25Team09Entities db)
        {
            try
            {
                var status_id = db.StudentStatus.FirstOrDefault(x => x.status_name.Equals(status)).id;
                Student user = db.Student.Find(mssv);
                user.status_id = status_id;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }
        public string getGender(String gender)
        {
            if (gender.ToUpper() == "TRUE")
            {
                return "Nam";
            }

            return "Nữ";
        }
    }
}
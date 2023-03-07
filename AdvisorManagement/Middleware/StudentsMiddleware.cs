using AdvisorManagement.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using AdvisorManagement.Middleware;
using System.Web;
using System.Data.Entity;
using System.IdentityModel.Protocols.WSTrust;

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
        public int getClassCode(string user_email)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.email == user_email);
            if (sql != null)
            {
                return (int)db.Student.SingleOrDefault(x => x.student_code == sql.user_code).id_class;
            }
            return 0;
        }
        public object getClass(string user_email, int year)
        {
            var classAdvisor = (from pq in db.AccountUser
                                join ad in db.Advisor on pq.user_code equals ad.advisor_code
                                join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                                where pq.email == user_email && ad.advisor_code == cl.advisor_code && cl.semester_name == year.ToString()
                                select new Models.ViewModel.AdvisorClass
                                {
                                    ID = cl.id,
                                    idClass = cl.class_code,
                                    idAdvisor = ad.advisor_code,
                                    name = pq.user_name,
                                    course = (int)cl.course,
                                    semester = cl.semester_name
                                }).OrderBy(x => x.idClass).ToList();
            if (classAdvisor.Count() != 0)
            {
                return classAdvisor;
            }
            return null;
        }

        public object getClassAdmin( int year)
        {            
            if(year != 0)
            {
                var classList = (from pq in db.AccountUser
                                 join ad in db.Advisor on pq.user_code equals ad.advisor_code
                                 join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                                 where ad.advisor_code == cl.advisor_code && cl.semester_name == year.ToString()
                                 select new Models.ViewModel.AdvisorClass
                                 {
                                     ID = cl.id,
                                     idClass = cl.class_code,
                                     idAdvisor = ad.advisor_code,
                                     name = pq.user_name,
                                     course = (int)cl.course,
                                     semester = cl.semester_name
                                 }).OrderBy(x => x.idClass).ToList();
                return classList;
            }
            else
            {
                var classList = (from pq in db.AccountUser
                                 join ad in db.Advisor on pq.user_code equals ad.advisor_code
                                 join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                                 where ad.advisor_code == cl.advisor_code
                                 select new Models.ViewModel.AdvisorClass
                                 {
                                     ID = cl.id,
                                     idClass = cl.class_code,
                                     idAdvisor = ad.advisor_code,
                                     name = pq.user_name,
                                     course = (int)cl.course,
                                     semester = cl.semester_name
                                 }).OrderBy(x => x.idClass).ToList();
                return classList;
            }                      
        }

        public object getStudentList(int id_class)
        {
            var classStudent = (from std in db.Student
                                join acc in db.AccountUser on std.student_code equals acc.user_code
                                join st in db.StudentStatus on std.status_id equals st.id
                                join lstd in db.ListStudents on std.student_code equals lstd.student_code
                                where lstd.id_class == id_class && std.student_code == lstd.student_code
                                select new Models.ViewModel.ListStudent
                                {
                                    id = lstd.id,    
                                    idAcc = acc.id,
                                    idStudent = std.student_code,
                                    name = acc.user_name,
                                    email = acc.email,
                                    phone = acc.phone,
                                    course = 25,
                                    status = st.status_name
                                }).OrderBy(x => x.name).ToList();
            return classStudent;
        }
        public object getInfoClass(int id)
        {
            var classInfo = (from pq in db.AccountUser
                             join ad in db.Advisor on pq.user_code equals ad.advisor_code
                             join cl in db.VLClass on ad.advisor_code equals cl.advisor_code
                             where ad.advisor_code == cl.advisor_code && cl.id == id
                             select new Models.ViewModel.AdvisorClass
                             {
                                 ID = cl.id,
                                 idClass = cl.class_code,
                                 idAdvisor = ad.advisor_code,
                                 name = pq.user_name,
                                 email = pq.email,
                                 phone = pq.phone,
                                 semester = cl.semester_name
                             }).ToList();
            return classInfo;
        }

        public bool getPermission(int id_class, string user_email)
        {
            if (db.VLClass.Where(x => x.id.Equals(id_class)).Count() == 0)
            {
                return false;
            }
            else
            {
                var user_code = db.AccountUser.FirstOrDefault(x => x.email == user_email).user_code;
                var advisor_code = db.VLClass.FirstOrDefault(x => x.id.Equals(id_class)).advisor_code;
                var role_admin = db.AccountUser.FirstOrDefault(x => x.email == user_email).id_role; 
                if (advisor_code == user_code || role_admin == 1)
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

        public bool saveStudent(String mssv, String status, CP25Team09Entities db, int classCode)
        {
            var result = false;
            try
            {
                var account_id = db.AccountUser.FirstOrDefault(x => x.user_code.Equals(mssv)).id;
                var status_id = db.StudentStatus.FirstOrDefault(x => x.status_name.Equals(status)).id;

                var item = new Student();
                item.student_code = mssv;
                item.id_class = classCode;
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

        public bool UpdateAccount(String mssv, String name, String phone, String address, String gender, CP25Team09Entities db)
        {
            var result = false;
            try
            {
                var account_id = db.AccountUser.FirstOrDefault(x => x.user_code.Equals(mssv)).id;
                AccountUser user = db.AccountUser.Find(account_id);
                if (user.user_name != name || user.phone != phone|| user.address != address || user.gender != getGender(gender))
                {
                    result = true;
                }
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
            return result;
        }

        public bool UpdateStudentImport(String mssv, String status, CP25Team09Entities db, int classCode)
        {
            var result = false;
            try
            {
                var status_id = db.StudentStatus.FirstOrDefault(x => x.status_name.Equals(status)).id;
                Student user = db.Student.Find(mssv);
                if (user.status_id != status_id)
                {
                    result = true;
                }
                user.status_id = status_id;
                user.id_class = classCode;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public string getGender(String gender)
        {
            if (gender.ToUpper() == "TRUE")
            {
                return "Nam";
            }

            return "Nữ";
        }

        public bool AddListStudent(int classCode, string mssv)
        {
            var result = false;
            try
            {
                ListStudents std = new ListStudents();
                std.id_class = classCode;
                std.student_code = mssv;
                db.ListStudents.Add(std);
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {


            }
            return result;
        }
        public int getID(string user_email)
        {
            var id_account = db.AccountUser.FirstOrDefault(x => x.email == user_email).id;
            if(id_account != null)
            {
                return id_account;
            }
            
            return id_account;
        }

    }
}
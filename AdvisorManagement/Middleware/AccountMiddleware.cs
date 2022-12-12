using AdvisorManagement.Models;
using AdvisorManagement.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace AdvisorManagement.Middleware
{
    public class AccountMiddleware
    {

        private CP25Team09Entities db = new CP25Team09Entities();
        public string getRoles(string roles)
        {
            var sql = db.AccountUser.FirstOrDefault(x => x.user_code == roles);
            if (sql != null)
            {
                return db.Role.SingleOrDefault(x => x.id == sql.id_Role).roleName;
            }
            return db.Role.SingleOrDefault(x => x.id == 4).roleName;
        }


        public object UserProfile(ClaimsIdentity logData, int roles)
        {
            var objectTest = logData;
            string[] data = objectTest.Claims.Where(x => x.Type == "name").First().Value.Split('-');
            StudentClass itemStudent = new StudentClass();
            if (roles == 2)
            {
                itemStudent.user_name = data[0].Trim();
                itemStudent.user_code = data[2].Trim() + "_CNTT";
            }
            else
            {
                itemStudent.user_code = data[0].Trim();
                itemStudent.user_name = data[1].Trim();
            }

            return itemStudent;
        }

        public int getID()
        {
            var lastRecordUser =  db.AccountUser.
               OrderByDescending(s => s.ID).
               Select(user => new
               {
                   Id = user.ID,
               }).FirstOrDefault();
            return (int)lastRecordUser.Id + 1;
        }

    }
}
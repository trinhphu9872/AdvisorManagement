using AdvisorManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvisorManagement.Middleware
{
    public class MenuMiddleware
    {
        private CP25Team09Entities db = new CP25Team09Entities();

        public object getMenu(string userMail)
        {
            var seeMenu = (from pq in db.AccountUser
                           join mnrole in db.RoleMenu on pq.id_Role equals mnrole.id_Role
                           join mn in db.Menu on mnrole.id_Menu equals mn.id
                           where pq.email == userMail && pq.id_Role == mnrole.id_Role
                           select new Models.ViewModel.MenuItem
                           {
                               menuName = mn.nameMenu
                           }).ToList();
            return seeMenu;
        }

        public object getRoleMenu()
        {
            var roleMenu = (from mnrole in db.RoleMenu
                            join mn in db.Menu on mnrole.id_Menu equals mn.id
                            join role in db.Role on mnrole.id_Role equals role.id
                            where role.id == mnrole.id_Role && mnrole.id_Menu == mn.id
                            select new Models.ViewModel.MenuRole
                            {
                                id = mnrole.id,
                                roleName = role.roleName,
                                menuName = mn.nameMenu
                            }).ToList();
            return roleMenu;
        }
    }
}
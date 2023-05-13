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
                           join mnrole in db.RoleMenu on pq.id_role equals mnrole.id_role
                           join mn in db.Menu on mnrole.id_menu equals mn.id
                           where pq.email == userMail && pq.id_role == mnrole.id_role
                           select new Models.ViewModel.MenuItem
                           {
                               menuName = mn.menu_name,
                               actionLink = mn.action_link,
                               orderID = (int)mn.order_id
                           }).OrderBy(x=>x.orderID).ToList();
            return seeMenu;
        }

        public object getRoleMenu()
        {
            var roleMenu = (from mnrole in db.RoleMenu
                            join mn in db.Menu on mnrole.id_menu equals mn.id
                            join role in db.Role on mnrole.id_role equals role.id
                            where role.id == mnrole.id_role && mnrole.id_menu == mn.id
                            select new Models.ViewModel.MenuRole
                            {
                                id = mnrole.id,
                                roleName = role.role_name_vn,
                                menuName = mn.menu_name
                            }).OrderBy(x => x.roleName).ToList();
            return roleMenu;
        }

        public List<Models.ViewModel.MenuItem> MenuItem()
        {
            var MenuItem = (from mn in db.Menu
                            select new Models.ViewModel.MenuItem
                            {
                                ID = mn.id,
                                menuName = mn.menu_name,
                                orderID =  (int)mn.order_id
                            }).OrderBy(x => x.orderID).ToList();

            return MenuItem;
        }
    }
}
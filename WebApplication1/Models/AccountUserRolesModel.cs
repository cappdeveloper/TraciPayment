using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    [Serializable] 
    public class Roles
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
   

    public class RoleDb
    {
        NYFSEntities2 oDB = new NYFSEntities2();
        public List<Roles> GetRoles()
        {
            var _list = (from p in oDB.AspNetRoles
                         select new Roles
                         {
                             Id = p.Id,
                             Name = p.Name
                         }).ToList();

              return _list;


            }

        public Roles GetRolebyId(string id)
        {
            var _list = (from p in oDB.AspNetRoles
                         where p.Id ==id
                         select new Roles
                         {

                             Id = p.Id,
                          Name = p.Name
                         }).SingleOrDefault();
            return _list;


        }


    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    [Serializable] 
    public class Users
    {
        public int Id { get; set; }
        public string Ids { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string RoleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Contact { get; set; }
        public string CompanyEmail { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Notes { get; set; }
    
        public string Zip { get; set; }
        public string Status { get; set; }

      
     public virtual AspNetRole tblRole { get; set; }

    }
    public class Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }

    }


    public class UserDB
    {
        NYFSEntities2 oDB = new NYFSEntities2();


        public List<AspNetRole> LoadRoles()
        {


            List<AspNetRole> oUser = new List<AspNetRole>();
            oUser = (from p in oDB.AspNetRoles  select p).ToList();
            return oUser;


        }
        public List<Users> GetUsers()
        {
            var data = oDB.AspNetUserRoles.ToList();
            var allRoles = oDB.AspNetRoles.ToList();
            var _list = (from p in oDB.AspNetUsers
                         where p.isActive==true 
                         select new Users
                         {
                             Ids = p.Id,
                             Name = p.Name,
                             Address = p.Address,
                             CompanyEmail = p.CompanyEmail,
                            CreatedDate = p.CreatedDate,
                             Contact = p.PhoneNumber,
                            Email=p.Email,
                         }).ToList();
            foreach(var items in _list)
            {
                var roleId = data.Where(cond => cond.UserId == items.Ids).FirstOrDefault();
                if(roleId != null)
                { 
                    items.RoleName = allRoles.Where(cond => cond.Id == roleId.RoleId.ToString()).FirstOrDefault().Name;
                }
            }



              return _list;


            }

        public Users GetUsersbyId(string id)
        {
            var _list = (from p in oDB.AspNetUsers
                         where p.Id ==id
                         select new Users
                         {

                            Ids = p.Id,
                            Name = p.Name,
                            UserName = p.UserName,
                            Address = p.Address,
                            City = p.City,
                            CompanyEmail = p.CompanyEmail,
                            Contact = p.PhoneNumber,
                            Country = p.Country,
                            Email    = p.Email,
                            Notes = p.Notes,
                            Zip = p.Zip,
                            Password=p.PasswordHash,
                         }).SingleOrDefault();
            return _list;


        }


    }
}
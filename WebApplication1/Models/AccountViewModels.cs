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
    
      //  public string ContractorName { get; set; }
        public string Zip { get; set; }
        public string Status { get; set; }

      
       // public virtual tblRole tblRole { get; set; }

    }


    public class UserDB
    {
        NYFSEntities2 oDB = new NYFSEntities2();
     

       // public tblUser GetUser(string Username, string Password)
       // {
       //    var objuser = oDB.tblUsers.Where(m => m.UserName == Username && m.Password == Password && m.IsActive == true).FirstOrDefault();
       //    return objuser;
       //}


        // public List<ContractorModel> LoadContractor()
        //{
               
          
        //List<ContractorModel> oUser = new List<ContractorModel>();
        //    oUser = (from p in oDB.tblContractors where p.Isactive == true select new ContractorModel { Id = p.Id,FirstName = p.FirstName+p.LastName }).ToList();
        //    return oUser;
            

        //}
        public List<Users> GetUsers()
        {
            var _list = (from p in oDB.AspNetUsers
                         where p.isActive==true && p.ContractorId != null
                         select new Users
                         {
                             Ids = p.Id,
                             Name = p.Name,
                           //  UserName = p.UserName,
                        //    ContractorName = p.tblContractor.FirstName,
                             Address = p.Address,
                             CompanyEmail = p.CompanyEmail,
                         CreatedDate = p.CreatedDate,
                             Contact = p.PhoneNumber






                         }).ToList();
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
                            // UserName = p.UserName,
                             Address = p.Address,
                             City = p.City,
                             CompanyEmail = p.CompanyEmail,
                             Contact = p.PhoneNumber,
                             Country = p.Country,
                             Notes = p.Notes,
                             Password=p.PasswordHash,
                             //ContractorId = p.ContractorId,
                             //Zip=p.Zip,
                            
                            









                         }).SingleOrDefault();
            return _list;


        }


    }
}
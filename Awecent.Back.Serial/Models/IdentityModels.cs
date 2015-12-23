using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Awecent.Back.Serial.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}

    public class UserandRole
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string UserGame { get; set; }
        public IEnumerable<Game> UserGameList { get; set; }

    }

    public class ResultModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }


    public class Game
    {
        public int GameId { get; set; }
        public string GameName { get; set; }

        //public int ServerId { get; set; }
        //public string ServerName { get; set; }
        
    }

    public class ReportItemCodeInput
    {
        [Required]
        public int gameId { get; set; }

        public string promotionId { get; set; }

        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }

        public int pageSize { get; set; }
        public int page { get; set; }
    }


    public class ItemCode
    {
        public long PromotionID { get; set; }
        public long LotID { get; set; }
        public string Code { get; set; }
        public string IsUsed { get; set; }
        public string FacebookID { get; set; }
        public string TimeUse { get; set; }
        public string TimeCreate { get; set; }
        public string CreateBy { get; set; }

        //page
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class ItemCodeList : ResultModel
    {
        public List<ItemCode> Data { get; set; }
        public int Total { get; set; }
    }

    public class LogModel
    {
        public Object Data { get; set; }
        public string DateTime { get; set; }
        public string Function { get; set; }
        public string Exception { get; set; }
        public string Return { get; set; }

    }

    public class PromotionsList
    {
        public List<Promotion> data { get; set; }
        public int total { get; set; }
        public bool result { get; set; }
    }

    public class Promotion
    {
        public int ID { get; set; }
        public string Name { get; set; }

    }


    public class MasterCode
    {
        [Required]
        public int? GameID { get; set; }

        public long? PromotionID { get; set; }

        public string PromotionName { get; set; }
        public string PromotionDescription { get; set; }
        public string Lot { get; set; }
        public string Code { get; set; }
        public string CreateBy { get; set; }
        public long? ItemID { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public string SerialType { get; set; }

        public string GenerateType { get; set; }
        public string SerialPrefix { get; set; }
        public string URLShared { get; set; }
        public string CreateUser { get; set; }
        public int? CodeAmount { get; set; }
        //page
        public int PageSize { get; set; }
        public int Page { get; set; }
        public bool Result { get; set; }
        public string Message { get; set; }


    }

    public class MasterCodeList : ResultModel
    {
        public int Total { get; set; }
        public List<MasterCode> Data { get; set; }
    }

    public class InputTempMaster
    {
        [Required]
        public string PromotionID { get; set; }
        [Required]
        public string Degit { get; set; }
        [Required]
        public int CodeAmount { get; set; }

        public string GenereateBy { get; set; }

    }


    public class OutputTempMaster : ResultModel
    {
        public string Key { get; set; }
        public int Amount { get; set; }
        public string GenereateBy { get; set; }

    }


    public class Lot
    {
        public long ID { get; set; }
        public long PromotionID { get; set; }
        public int Number { get; set; }
        public int Amount { get; set; }
        public string SerialRemain { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }

        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class LotList : ResultModel
    {
        public IEnumerable<Lot> Data { get; set; }
        public int Total { get; set; }
    }
}
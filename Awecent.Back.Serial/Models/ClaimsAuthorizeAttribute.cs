using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Awecent.Back.Serial.Models
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {

        private string claimType;
        private string[] claimValues;
        private string[] arryClaimValue;
        public ClaimsAuthorizeAttribute(string type, params string[] values)
        {
            this.claimType = type;
            this.claimValues = values;
            //this.arryClaimValue = value.Split(',');
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.User as ClaimsPrincipal;


            var valid = false;
            foreach (var claimValue in claimValues)
            {
                valid |= user.HasClaim(claimType , claimValue);
            }

            if (valid)
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
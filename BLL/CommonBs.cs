using BOL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web;
using System.Transactions;

namespace BLL
{
   public class CommonBs : BaseBs
    {
       public void InsertQuickUrl(QuickSubmitURLModel myQuickUrl)
       {
           using (TransactionScope Trans = new TransactionScope())
           {
               try
               {
                   tbl_User u = myQuickUrl.User;
                   u.Password = u.ConfirmPassword = "123456";
                   u.Role = "U";
                   userBs.Insert(u);

                   tbl_Url myUrl = myQuickUrl.MyUrl;
                   myUrl.UserId = u.UserId;
                   myUrl.UrlDesc = myUrl.UrlTitle;
                   myUrl.IsApproved = "P";
                   urlBs.Insert(myUrl);
                   
                   Trans.Complete();
               }
               catch(Exception E1)
               {
                   throw new Exception(E1.Message);
               }
           }
       }
    }
}

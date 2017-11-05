/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Class Title      : CookieManager.cs                  '
'  Description      : Sets the session cookies          '
'                     as we store 'custom' fields in our'
'                     SQL table and we need them on all '
'                     common web sections and by default'
'                     the custom fields are not stored  '
'                     in the normal User.Identity obj.  '
'                     If we add other custom fields to  '
'                     AspNetUsers table we only need to '
'                     update this class to affect the   '
'                     change across the app             '
'  Author           : Brian McAulay                     '
'  Creation Date    : 03 Nov 2017                       '
'  Version No       : 1.0                               '
'  Email            : g2bam2012@gmail.com               '
'  Revision         :                                   '
'  Revision Reason  :                                   '
'  Revisor          :                       		    '
'  Date Revised     :                       		    '  
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
*/
using MVCWebProject2.Models;
using System;
using System.Web;

namespace MVCWebProject2.utilities
{
    public class CookieManager 
    {

        HttpContext context = HttpContext.Current;

        //This is where the final cookies are generated and written
        public void WriteCookie(ApplicationUser currentUser)
        {
            //Now we can set our cookies
            context.Response.Cookies["userInfo"]["BootstrapTheme"] = currentUser.BootstrapTheme;
            context.Response.Cookies["userInfo"]["FirstName"] = currentUser.FirstName;
            context.Response.Cookies["userInfo"]["Surname"] = currentUser.Surname;
        }

        //Clear the cookies after a sign out
        public void ClearCookie()
        {
            //Destroy our cookies after a logout
            context.Response.Cookies["userLogin"].Expires = DateTime.Now.AddDays(-1);
        }
    }
}
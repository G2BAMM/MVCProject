/*
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'  Page Title       : global.asax                       '
'  Description      : App level settings                ' 
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
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;

namespace MVCWebProject2
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Uncomment the code below for live server to trap uncaught errors

            /* 
            // Fires when an error occurs
            MailMessage MyMailer = new MailMessage();
            SmtpClient client = new SmtpClient
                {
                    //client.Port = 587;
                    Host = WebConfigurationManager.AppSettings["MailHost"],
                    EnableSsl = true,
                    //client.Timeout = 10000;
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["MailAccount"], WebConfigurationManager.AppSettings["MailPassword"])
                };
            Exception objErr = Server.GetLastError().GetBaseException();
            string Err = "Error Caught in Application_Error event" + System.Environment.NewLine + "Error in: " + Request.Url.ToString() + System.Environment.NewLine + "Error Message: " + objErr.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + objErr.StackTrace.ToString();

            MyMailer = new MailMessage();
            MyMailer.From = new MailAddress("error@easyhire.com");
            MyMailer.To.Add(new MailAddress("g2bam2012@gmail.com"));
            MyMailer.Subject = "www.easyhire.com site error";
            MyMailer.Body = "An error has occurred on easyhire.com\n\nError is displayed below.\n\n" + Err;

            //Now send a mail to the support or dev team
            client.Send(MyMailer);
            Exception objErr = Server.GetLastError().GetBaseException();
            string Err = "Error Caught in Application_Error event" + System.Environment.NewLine + "Error in: " + Request.Url.ToString() + System.Environment.NewLine + "Error Message: " + objErr.Message.ToString() + System.Environment.NewLine + "Stack Trace:" + objErr.StackTrace.ToString();
            //Clear the error to prevent an inescapable loop
            Server.ClearError();
            //Determine which error page to display to the user
            if (Err.Contains("was not found")) 
            {
                //We have a 404 error
                Response.Redirect("~/Home/NotFoundError");
            }
            else
            {
                //We have an untrapped or general error
                Response.Redirect("~/Home/GeneralError");
            }
            */
        }


    }
}

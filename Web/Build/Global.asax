<%@ Application Language="C#" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Web.Optimization" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
        Application["softwarename"] = ConfigurationManager.AppSettings.Get("softwarename").ToString();

        PhoenixSecurityContext psc = new PhoenixSecurityContext();
        psc.UserCode = 1;
        psc.UserName = "System Admin";
        psc.UserType = "APPLICATION";
        psc.FirstName = "System";
        psc.LastName = "Admin";
        psc.MiddleName = "";
        psc.GroupList = ",1,";
        psc.ActiveYN = 1;

        PhoenixSecurityContext.SystemSecurityContext = psc;
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    void Application_BeginRequest(Object sender, EventArgs e)
    {
        switch (Request.Url.Scheme)
        {
            case "https": Response.AddHeader("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload"); break;
                //case "http": var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery; Response.Status = "301 Moved Permanently"; Response.AddHeader("Location", path); break;
        }
        HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.Now);
        Response.Cache.SetValidUntilExpires(true);

        HttpContext context = ((HttpApplication)sender).Context;
        CultureInfo ui = new CultureInfo(ConfigurationManager.AppSettings.Get("CultureToUse"));
        ui.DateTimeFormat.ShortDatePattern = ConfigurationManager.AppSettings.Get("DateFormatToUse");
        //ui.DateTimeFormat.LongDatePattern = ConfigurationManager.AppSettings.Get("CultureToCreate");
        //ui.DateTimeFormat.FullDateTimePattern = ConfigurationManager.AppSettings.Get("CultureToCreate");        
        Thread.CurrentThread.CurrentCulture = ui;
        Thread.CurrentThread.CurrentUICulture = ui;

        HttpRuntimeSection runTime = (HttpRuntimeSection)WebConfigurationManager.GetSection("system.web/httpRuntime");
        //Approx 100 Kb(for page content) size has been deducted because the maxRequestLength proprty is the page size, not only the file upload size
        int maxRequestLength = (runTime.MaxRequestLength - 100) * 1024;

        //This code is used to check the request length of the page and if the request length is greater than
        //MaxRequestLength then retrun to the same page with extra query string value action=exception


        if (context.Request.ContentLength > maxRequestLength)
        {
            IServiceProvider provider = (IServiceProvider)context;
            HttpWorkerRequest workerRequest = (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));

            // Check if body contains data
            if (workerRequest.HasEntityBody())
            {
                // get the total body length
                int requestLength = workerRequest.GetTotalEntityBodyLength();
                // Get the initial bytes loaded
                int initialBytes = 0;
                if (workerRequest.GetPreloadedEntityBody() != null)
                    initialBytes = workerRequest.GetPreloadedEntityBody().Length;
                if (!workerRequest.IsEntireEntityBodyIsPreloaded())
                {
                    byte[] buffer = new byte[512000];
                    // Set the received bytes to initial bytes before start reading
                    int receivedBytes = initialBytes;
                    while (requestLength - receivedBytes >= initialBytes)
                    {
                        // Read another set of bytes
                        initialBytes = workerRequest.ReadEntityBody(buffer, buffer.Length);

                        // Update the received bytes
                        receivedBytes += initialBytes;
                    }
                    initialBytes = workerRequest.ReadEntityBody(buffer, requestLength - receivedBytes);
                }
            }
            // Redirect the user to the same page with querystring action=exception.
            context.Response.Redirect(this.Request.Url.LocalPath + "?" + Request.QueryString.ToString().Replace("&maxreqlen=" + maxRequestLength,string.Empty) + "&maxreqlen=" + maxRequestLength);
        }
    }
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown

    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception exc = Server.GetLastError();
        // Handle HTTP errors
        if (exc == null) return;
        if (exc.GetType() == typeof(HttpException))
        {
            // The Complete Error Handling Example generates
            // some errors using URLs with "NoCatch" in them;
            // ignore these here to simulate what would happen
            // if a global.asax handler were not implemented.
            if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength") || (exc.Message.Contains("The file") &&  exc.Message.Contains("does not exist")))
                return;
            //Redirect HTTP errors to HttpError page
            //Server.Transfer("HttpErrorPage.aspx");
        }

        // Log the exception and notify system operators
        ExceptionUtility.LogException(exc, "Global");
        ExceptionUtility.NotifySystemOps(exc);
        Server.Transfer(Request.ApplicationPath + "/GenericErrorPage.aspx");
        //if (HttpContext.Current.Session == null || PhoenixSecurityContext.CurrentSecurityContext == null)
        //{
        //    Server.Transfer("/" + Request.ApplicationPath.Trim('/') + "/PhoenixLogout.aspx");
        //    return;
        //}
        //else
        //{
        //    Server.Transfer(Request.ApplicationPath + "/GenericErrorPage.aspx");
        //}
        // Clear the error from the server
        Server.ClearError();
    }
    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        // Session is Available here
        HttpContext context = HttpContext.Current;
        if (context.Session != null)
        {
            //if (PhoenixSecurityContext.CurrentSecurityContext == null && !(context.Request.Path.ToLower().Contains("default.aspx") || context.Request.Path.ToLower().Contains("/options/") ||context.Request.Path.ToLower().Contains("/purchase/")))
            //{
            //    Server.Transfer("/" + Request.ApplicationPath.Trim('/') + "/PhoenixLogout.aspx");
            //    return;
            //}
            GeneralSetting gs = PhoenixGeneralSettings.CurrentGeneralSetting;
            if (gs != null && gs.UserCulture != string.Empty)
            {
                CultureInfo ui = new CultureInfo(gs.UserCulture);
                ui.DateTimeFormat.ShortDatePattern = gs.ShortDateFormat;
                //ui.DateTimeFormat.LongDatePattern = ConfigurationManager.AppSettings.Get("CultureToCreate");
                //ui.DateTimeFormat.FullDateTimePattern = ConfigurationManager.AppSettings.Get("CultureToCreate");
                Thread.CurrentThread.CurrentCulture = ui;
                Thread.CurrentThread.CurrentUICulture = ui;
            }
        }
    }
    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
        Session["theme"] = ConfigurationManager.AppSettings.Get("theme").ToString();
        Session["skin"] = ConfigurationManager.AppSettings.Get("Telerik.Skin").ToString();

        Session["images"] = Request.ApplicationPath + "/css/" + Session["theme"] + "/images";
        string port = ConfigurationManager.AppSettings.Get("port").ToString().Trim().Equals("") ? "80" : ConfigurationManager.AppSettings.Get("port").ToString();
        if(Request.Url.Scheme.Equals("https"))
        {
            port = ConfigurationManager.AppSettings.Get("httpsport").ToString().Trim().Equals("") ? "443" : ConfigurationManager.AppSettings.Get("httpsport").ToString();
        }
        Session["sitepath"] = Request.Url.Scheme + "://" + Request.ServerVariables["SERVER_NAME"].ToString() + ":" + port + Request.ApplicationPath;
        Session["screenwidth"] = ConfigurationManager.AppSettings.Get("screenwidth").ToString();
        Session["screenheight"] = ConfigurationManager.AppSettings.Get("screenheight").ToString();
        Session["companyname"] = ConfigurationManager.AppSettings.Get("companyname").ToString();
        Session["companyshortname"] = ConfigurationManager.AppSettings.Get("companyshortname").ToString();
        // PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
        string CookieHeaders = HttpContext.Current.Request.Headers["Cookie"];

        if ((null != CookieHeaders) && (CookieHeaders.IndexOf("ASP.NET_SessionId") >= 0))
        {
            // It is existing visitor, but ASP.NET session is expired
        }
        string is30 = ConfigurationManager.AppSettings.Get("is30");
        Session["is3.0"] = string.IsNullOrEmpty(is30) ? "" : is30;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        //HttpContext.Current.Response.Redirect("~/PhoenixLogout.aspx");
    }

</script>

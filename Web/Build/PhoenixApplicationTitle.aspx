<%@ Page Language="C#" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server" language="C#">
    string username;
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixSecurityContext psc = PhoenixSecurityContext.CurrentSecurityContext;
        SessionUtil.PageAccessRights(this.ViewState);
        if (psc.FirstName.ToUpper().Equals(psc.LastName.ToUpper()))
        {
            username = psc.FirstName + " " + psc.MiddleName;
        }
        else
        {
            username = psc.FirstName + " " + psc.MiddleName + " " + psc.LastName;
        }
        if (!psc.UserShortCode.Equals(""))
        {
            username = username + " [" + psc.UserShortCode + "]";
        }

        SetDefaultVessel();

        if (Session["CURRENTDATABASE"] != null)
        {
            lblDatabase.Text = lblDatabase.Text + Session["CURRENTDATABASE"].ToString();
            lblDatabase.Visible = true;
        }

        lblVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;

        if (!(PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("USER")
            || PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("ADMIN")))
        {
            //imgDashboard.Visible = false;
            imgNews.Visible = false;
            Img1.Visible = false;
            imgEpssPhoenix.Visible = false;
            //imgPreferences.Visible = false;

            aOptions.Visible = false;
            spnOptions.Visible = false;

            aEPPSPhoenix.Visible = false;
            spnEPSSPhoenix.Visible = false;

            aEPSSHelp.Visible = false;
            spnEPSSHelp.Visible = false;

            //aDashboard.Visible = false;
            //spnDashboard.Visible = false;

            aMessage.Visible = false;
            spnMessage.Disabled = true;

            aSuptFeedback.Visible = false;
            aSuptFeedback.Disabled = true;
            spnFeedback.Disabled = true;

            aHome.Visible = false;
            aHome.Disabled = true;
            spnHome.Visible = false;

            aSearch.Visible = false;
            aSearch.Disabled = true;
            spnSearch.Visible = false;

            //aPreference.Visible = false;
            //aPreference.Disabled = true;            
        }

        aSwitch.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCH");
        spnSwitch.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCH");

        aCompanySwitch.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCHCOMPANY");
        spnCompanySwitch.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCHCOMPANY");

        aOptions.Visible = SessionUtil.CanAccess(this.ViewState, "OPTIONS");
        spnOptions.Visible = SessionUtil.CanAccess(this.ViewState, "OPTIONS");

        aHelp.Visible = SessionUtil.CanAccess(this.ViewState, "HELP");
        spnHelp.Visible = SessionUtil.CanAccess(this.ViewState, "HELP");

        aEPPSPhoenix.Visible = SessionUtil.CanAccess(this.ViewState, "EPSSPHOENIX");
        spnEPSSPhoenix.Visible = SessionUtil.CanAccess(this.ViewState, "EPSSPHOENIX");

        aEPSSHelp.Visible = SessionUtil.CanAccess(this.ViewState, "EPSSHELP");
        spnEPSSHelp.Visible = SessionUtil.CanAccess(this.ViewState, "EPSSHELP");

        aDashboard.Visible = SessionUtil.CanAccess(this.ViewState, "DASHBOARD");
        spnDashboard.Visible = SessionUtil.CanAccess(this.ViewState, "DASHBOARD");

        aMessage.Visible = SessionUtil.CanAccess(this.ViewState, "MESSAGES");
        spnMessage.Visible = SessionUtil.CanAccess(this.ViewState, "MESSAGES");

        aHome.Visible = SessionUtil.CanAccess(this.ViewState, "HOME");
        spnHome.Visible = SessionUtil.CanAccess(this.ViewState, "HOME");

        aSearch.Visible = SessionUtil.CanAccess(this.ViewState, "SEARCH");
        spnSearch.Visible = SessionUtil.CanAccess(this.ViewState, "SEARCH");

        aPhoenixAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICS");
        spnAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICS");

        aPhoenixAnalyticsReporting.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICSREPORTING");
        spnPhoenixAnalytics.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXANALYTICSREPORTING");
        //aPreference.Visible = SessionUtil.CanAccess(this.ViewState, "DASHBOARDPREFERENCES");

        string analyticsURL = null;
        if(ConfigurationManager.AppSettings["PhoenixAnalyticsUrl"] != null)
        {
            analyticsURL = ConfigurationManager.AppSettings["PhoenixAnalyticsUrl"].ToString();
        }


        string Tokenid = null;
        if (Filter.CurrentLoginToken != null)
        {
            Tokenid = Filter.CurrentLoginToken.ToString();
        }

        aPhoenixAnalyticsReporting.HRef =   analyticsURL +"/?Token="+ Tokenid;


        bool bVisible = SessionUtil.CanAccess(this.ViewState, "SUPDTEVENTFEEDBACK");

        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("OWNER") ||
                PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("SUPPLIER") ||
                PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("AGENT") ||
                 PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0
                )
        {
            aPhoenixAnalytics.Visible = false;
            aPhoenixAnalyticsReporting.Visible = false;
        }


        if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("SUPPLIER") ||
             PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("AGENT"))
        {
            imgDashboard.Visible = false;
            aDashboard.Visible = false;
            spnDashboard.Visible = false;

        }
        DataTable dt;
        dt = PhoenixDashboard.DashboardPreferenceSearch();
        if (dt.Rows.Count > 0)
        {
            string preferenceCode;
            preferenceCode = dt.Rows[0]["FLDPREFERENCECODE"].ToString();
            if (preferenceCode != "0")
            {
                imgDashboard.Visible = false;
                aDashboard.Visible = false;
                spnDashboard.Visible = false;
            }

        }


        if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback != 1
            || PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")
            || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            bVisible = false;
        }

        aSuptFeedback.Visible = bVisible;
        spnFeedback.Visible = bVisible;

    }

    protected void Set_Home(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "showVessel();OpenTaskPane('Dashboard');";
        Script += "<" + "/script>";

        DataSet dsVessel = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, "");

        if (dsVessel.Tables.Count > 0 && dsVessel.Tables[0].Rows.Count > 0)
        {
            if (dsVessel.Tables[0].Rows[0]["FLDVESSELID"].ToString().Equals("0"))
            {
                PhoenixSecurityContext.CurrentSecurityContext.VesselID = 0;
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = "--OFFICE--";
                Filter.CurrentOrderFormFilterCriteria = null;

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
                SessionUtil.ReBuildMenu();
            }
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    private void SetDefaultVessel()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselName == null || PhoenixSecurityContext.CurrentSecurityContext.VesselName == "")
        {
            PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(((GeneralSetting)PhoenixGeneralSettings.CurrentGeneralSetting).VesselID);
            PhoenixSecurityContext.CurrentSecurityContext.VesselName = ((GeneralSetting)PhoenixGeneralSettings.CurrentGeneralSetting).VesselName;
        }
    }


</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <div runat="server">
        <link rel="stylesheet" type="text/css" href="css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript">
        function OpenHelp()
        {
            //            alert(parent.document.getElementById('filterandsearch').contentDocument.location.href);
            var page = parent.document.getElementById('filterandsearch').contentDocument.location.href;
            Openpopup('help', '', 'Help/HelpDocument.aspx?page=' + (page.substring(page.lastIndexOf('/') + 1, page.length)));
            if(parent.document.getElementById('filterandsearch').contentDocument.getElementById('ifMoreInfo') != null)
            {
                //                alert(parent.document.getElementById('filterandsearch').contentDocument.getElementById('ifMoreInfo').contentWindow.location.href)
                var page = parent.document.getElementById('filterandsearch').contentDocument.getElementById('ifMoreInfo').contentWindow.location.href;
                Openpopup('help', '', 'Help/HelpDocument.aspx?page=' + (page.substring(page.lastIndexOf('/') + 1, page.length)));
            }
        }
        function checkForAlert()
        {
            var args="function=CheckAlert";    
            args = AjxPost(args, SitePath + "PhoenixWebFunctions.aspx", null, false);
            if(args == "1")
            {
                var imagePath = '<%=Session["images"] %>';
                document.getElementById("imgAlert").src = imagePath + '/51.png'
            }
        }
        
        function setTimer()
        {
            //setInterval("checkForAlert()", 60000 * 1);
        }
        setTimer();
        parent.refreshMenu();
        </script>

    </div>
</head>
<body class="pagebackground">
    <form id="Form1" runat="server">
    <div class="header">
        <div style="position: absolute; top: 0px">
            <img runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                alt="Phoenix" onclick="parent.hideMenu();" />
            <span class="title">
                <%=Application["softwarename"].ToString() %></span>
        </div>
        <div class="toolbarContent" style="position: absolute; top: 0px; right: 0px" runat="server">
            <span class="systemadmin_text">
                <%=username %>&nbsp;&nbsp;<%=DateTime.Today.Date.ToShortDateString() %>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblVessel" CssClass="activevessel_text"
                    Text="Vessel A"></asp:Label></span><p />
             
            <a href="javascript:OpenSearchPage('Options/OptionsChooseCompanyQuality.aspx');" class="applinks"
                id="aCompanySwitch" runat="server">
                <img runat="server" src="<%$ PhoenixTheme:images/61.png %>" alt="" />Switch Company</a>
            <span id="spnCompanySwitch" runat="server">|</span>
                    
           <a href="javascript:OpenSearchPage('Options/OptionsSwitch.aspx');" class="applinks"
                id="aSwitch" runat="server">
                <img runat="server" src="<%$ PhoenixTheme:images/61.png %>" alt="" />Switch</a>
            <span id="spnSwitch" runat="server">|</span> 
                                
            <a id="aOptions" href="javascript:OpenTaskPane('Options');" class="applinks" runat="server">
                <img runat="server" src="<%$ PhoenixTheme:images/45.png %>" alt="" />Options</a>
            <span id="spnOptions" runat="server">|</span> 
            
            <a href="PhoenixLogout.aspx" class="applinks">
                <img runat="server" src="<%$ PhoenixTheme:images/24.png %>" alt="Logout" title="Logout" />Logout</a>                
            <span id="spnLogout" runat="server">|</span> 
            
            <a id="aEPPSPhoenix" href="PHOENIXHELP/index.html" target="_blank" class="applinks" runat="server">
                <img id="imgEpssPhoenix" runat="server" src="<%$ PhoenixTheme:images/54.png %>" alt="" title="EPSS Phoenix" />Phoenix-Help</a>
            <span id="spnEPSSPhoenix" runat="server">|</span> 
            
            <a id="aHelp" href="javascript:OpenHelp();" class="applinks" runat="server">
                <img id="imgHelp" runat="server" src="<%$ PhoenixTheme:images/54.png %>" alt="" title="Help" />Help</a>                
            <span id="spnHelp" runat="server">|</span> 
            
            <a id="aEPSSHelp" href="EPSSHELP/index.html" target="_blank" class="applinks" runat="server">
                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/sims1.png %>" alt="" title="EPSS Help" />EPSS</a>
            <span id="spnEPSSHelp" runat="server">|</span> 
                
            <a id="aDashboard" href="javascript:OpenSearchPage('Dashboard/Dashboard.aspx');OpenTaskPane('Dashboard');"
                class="applinks" runat="server">
                <img id="imgDashboard" runat="server" src="<%$ PhoenixTheme:images/51.png %>" alt="Dashboard"
                    title="Dashboard" /></a> 
            <span id="spnDashboard" runat="server">|</span> 
            
            <a id="aMessage" onclick="javascript:Openpopup('News', 'News', 'Dashboard/DashboardBroadcast.aspx');"
                class="applinks" runat="server">
                <img id="imgNews" runat="server" src="<%$ PhoenixTheme:images/on-signer.png %>" alt="Messages"
                    title="Messages" /></a> 
            <span id="spnMessage" runat="server">|</span> 
            
            <a id="aSuptFeedback" onclick="javascript:Openpopup('Preference', '', 'Inspection/InspectionSupdtEventFeedbackBanner.aspx?sourcefrom=0');"
                class="applinks" runat="server">
                <img id="imgSupdtFeedback" runat="server" src="<%$ PhoenixTheme:images/74.png %>"
                     alt=" " title=" " /></a> 
            <span id="spnFeedback" runat="server">|</span> 
            
            <a id="aHome" href="javascript:OpenSearchPage('Dashboard/Dashboard.aspx');"
                class="applinks" runat="server" onserverclick="Set_Home">
                <img id="imgHome" runat="server" src="<%$ PhoenixTheme:images/home.png %>" alt="Office Home" title="Office Home" />
              </a>
            <span id="spnHome" runat="server">|</span> 
            
            <a id="aSearch" href="javascript:OpenSearchPage('Dashboard/DashboardSearch.aspx');"
                class="applinks" runat="server">
                <img id="imgSearch" runat="server" src="<%$ PhoenixTheme:images/search.png %>" alt="Search"
                    title="Search Crew" /></a>
            <span id="spnSearch" runat="server">|</span> 

               <%--<a id="aPreference" onclick="javascript:Openpopup('Preference', '', 'Dashboard/DashboardUserPreferences.aspx?');"
                class="applinks" runat="server">
                <img id="imgPreferences" runat="server" src="<%$ PhoenixTheme:images/tab-select.png %>"
                    alt="Dashboard Preferences" title="DashboardPreferences" /></a>--%>
                    
            <asp:Label runat="server" ID="lblDatabase" ForeColor="Red" Font-Size="Large" Visible="false"
                Text="Testing on "></asp:Label>

            <span id="spnAnalytics" runat="server">|</span>         
            <a id="aPhoenixAnalytics" href="PhoenixAnalytics.aspx" target="_blank"  class="applinks" runat="server">
                <img id="img2" runat="server" src="<%$ PhoenixTheme:images/component.png %>" alt="Work Rest Hour Analytics" title="Work Rest Hour Analytics" />
            </a>

            
            <span id="spnPhoenixAnalytics" runat="server">|</span>         
            <a id="aPhoenixAnalyticsReporting" target="_blank"  class="applinks" runat="server">
                <img id="img4" runat="server" src="<%$ PhoenixTheme:images/component-detail.png %>" alt="Phoenix Analytics" title="Phoenix Analytics" />
            </a>            
        </div>
    </div>
    <iframe width="0px" height="0px" id="ifrKeepAlive" src="PhoenixKeepAlive.aspx"></iframe>
    </form>
</body>
</html>

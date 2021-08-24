using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using Telerik.Web.Device.Detection;
using Telerik.Web.UI;

public partial class PhoenixWebDefault : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AppendHeader("Refresh", Convert.ToString(Session.Timeout * 60) + "; URL=PhoenixLogout.aspx");
            SessionUtil.PageAccessRights(this.ViewState);
            RadComboBox vessel = ((RadComboBox)mainNavigation.Nodes[2].FindControl("ddlVessel"));
            RadComboBox cpy = ((RadComboBox)mainNavigation.Nodes[3].FindControl("ddlCompany"));
            //RadComboBox fms = ((RadComboBox)mainNavigation.Nodes[4].FindControl("ddlfms"));
            if (!IsPostBack)
            {
                ViewState["modulecode"] = string.Empty;
                if (!(PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("USER")
                        || PhoenixSecurityContext.CurrentSecurityContext.UserType.Equals("ADMIN")))
                {
                    lnkEPSS.Visible = false;
                    lnkFeedback.Visible = false;
                    lnkCrewSearch.Visible = false;
                    lnkDMSSearch.Visible = false;
                }
                vessel.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCH");
                cpy.Visible = SessionUtil.CanAccess(this.ViewState, "SWITCHCOMPANY");
                //fms.Visible = SessionUtil.CanAccess(this.ViewState, "FMS");
                lnkDMSSearch.Visible = SessionUtil.CanAccess(this.ViewState, "SEARCH");
                lnkCrewSearch.Visible = SessionUtil.CanAccess(this.ViewState, "SEARCH");
                lnkFeedback.Visible = SessionUtil.CanAccess(this.ViewState, "SUPDTEVENTFEEDBACK");
                lnkEPSS.Visible = SessionUtil.CanAccess(this.ViewState, "EPSSHELP");
                lnkPhoenixHelp.Visible = SessionUtil.CanAccess(this.ViewState, "PHOENIXHELP");
                lnkHelp.Visible = SessionUtil.CanAccess(this.ViewState, "HELP");
                lnkFMSSearch.Visible = SessionUtil.CanAccess(this.ViewState, "FMS");

                DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["skin"] = ds.Tables[0].Rows[0]["FLDSKIN"].ToString();
                }
                if (Session["skin"].ToString().Equals(""))
                {
                    Session["skin"] = ConfigurationManager.AppSettings.Get("Telerik.Skin").ToString();
                }
                RadSkinManager1.Skin = Session["skin"].ToString();
                PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);                
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselName == null || PhoenixSecurityContext.CurrentSecurityContext.VesselName == "")
                {
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(((GeneralSetting)PhoenixGeneralSettings.CurrentGeneralSetting).VesselID);
                    PhoenixSecurityContext.CurrentSecurityContext.VesselName = ((GeneralSetting)PhoenixGeneralSettings.CurrentGeneralSetting).VesselName;
                }
                SessionUtil.ReBuildMenu();
                if (Session["currPage"] == null)
                {
                    Session["currPage"] = "Dashboard/Dashboard.aspx";
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    byte? assignedvessel = 1;
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
                        assignedvessel = 0;
                    //DataSet vsl = PhoenixRegistersVessel.ListAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                    DataSet vsl = PhoenixRegistersVessel.VesselListCommon(null, 1, null, assignedvessel, PhoenixVesselEntityType.VSL, null);
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() == "OWNER")
                    {
                        DataRow[] drs = vsl.Tables[0].Select("FLDVESSELID = 0");
                        if (drs.Length == 0)
                        {
                            DataRow dr = vsl.Tables[0].NewRow();
                            dr["FLDVESSELID"] = "0";
                            dr["FLDVESSELNAME"] = "-- OFFICE --";
                            vsl.Tables[0].Rows.InsertAt(dr, 0);
                        }
                    }
                        vessel.DataSource = vsl;
                    if (vsl.Tables.Count > 0)
                    {
                        DataRow[] dr = vsl.Tables[0].Select("FLDVESSELID = " + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
                        if (dr.Length > 0)
                        {
                            vessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        }
                    }

                    DataSet company = PhoenixRegistersCompany.ListAssignedCompany();
                    cpy.DataSource = company;
                    if (company.Tables.Count > 0)
                    {
                        DataRow[] dr = company.Tables[0].Select("FLDCOMPANYID = " + PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString());
                        if (dr.Length > 0)
                        {
                            cpy.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
                        }
                    }
                }
                else
                {
                    vessel.Visible = false;
                    cpy.Visible = false;
                    lnkFeedback.Visible = false;
                    spnfeedback.Visible = false;
                    lnkCrewSearch.Visible = false;
                    lnkFMSSearch.Nodes[1].Visible = false;
                    lnkFMSSearch.Nodes[2].Visible = false;
                    lnkFMSSearch.Nodes[5].Visible = false;
                    //lnkOffice.Visible = false;
                }
                //if (PhoenixSecurityContext.CurrentSecurityContext.PasswordChanged == 0)
                //{
                //    mnuNavigationBar.Visible = false;
                //}
                //mnuNavigationBar.DataTextField = "NAME";
                //mnuNavigationBar.DataNavigateUrlField = "URL";
                menuXML.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";
                XmlDataSourceModule.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";               
                LoadModule();
                SetName();                
                SetSkin();
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ShowSupdtFeedback.Equals(1)
                    && PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX")
                    && PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0))
                {
                    spnfeedback.Visible = false;
                    lnkFeedback.Visible = true;
                }
                else
                {
                    spnfeedback.Visible = false;
                    lnkFeedback.Visible = false;
                }
                
                System.Web.UI.HtmlControls.HtmlGenericControl ctrl = (System.Web.UI.HtmlControls.HtmlGenericControl)mainNavigation.FindNodeByText("Phoenix").FindControl("spnApplication");
                ctrl.InnerText = Application["softwarename"].ToString();
                ViewState["MHEIGHT"] = 500;
            }
            fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == -1 && (!PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("NEWAPPLICANT")))
            {
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "setContext", "alert('User data not found system will function as expected. Contact Office');", true);
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper().Equals("NEWAPPLICANT"))
            {
                about.Visible = false;
                (mainNavigation.Nodes[0].FindControl("menubar")).Visible = false;

            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        DeviceScreenDimensions screenDimensions = Detector.GetScreenDimensions(Request.UserAgent);
        LoadMobile(screenDimensions);
    }

    private void LoadMobile(DeviceScreenDimensions screenDimensions)
    {
        // Desktop Browser Detected
        if (screenDimensions.Height == 0 && screenDimensions.Width == 0)
        {
            //var mobileNavigation = FolderContent.FindControl("MobileNavigation");
            //if (mobileNavigation != null)
            //{
            //    mobileNavigation.Visible = false;
            //    nav.Value = "desktop";
            //}
            nav.Value = "desktop";
        }
        // Mobile Browser Detected
        else
        {
            this.form1.Attributes.Add("class", "mobile clear");
            //var desktopNavigation = FolderContent.FindControl("FolderNavigationControl");
            //if (desktopNavigation != null)
            //{
            //    desktopNavigation.Visible = false;
            //    nav.Value = "mobile";
            //}
            nav.Value = "mobile";
        }
    }
    //protected void mnuNavigationBar_ItemDataBound(object sender, Telerik.Web.UI.RadPanelBarEventArgs e)
    //{
    //    XmlElement drv = (XmlElement)e.Item.DataItem;
    //    string[] url = Regex.Replace(e.Item.NavigateUrl, "JAVASCRIPT:OPENSEARCHPAGE\\(", "", RegexOptions.IgnoreCase).Replace("'", "").Replace(");", "").Split(',');
    //    if (url.Length > 0 && url[0] != string.Empty)
    //    {
    //        e.Item.Attributes["data-url"] = "~/" + url[0];
    //        e.Item.NavigateUrl = "";
    //        //e.Item.NavigateUrl = "~/" + url[0];
    //        //e.Item.Target = "fraPhoenixApplication";
    //    }
    //    else
    //    {
    //        e.Item.NavigateUrl = "javascript:void(0); return false;";
    //    }
    //    e.Item.Value = drv.Attributes["CODE"].Value;
    //}
    protected void applicationMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("LOGOUT"))
            {
                Response.Redirect("~/PhoenixLogout.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RadSkinManager1_SkinChanged(object sender, SkinChangedEventArgs e)
    {
        Session["skin"] = e.Skin;
        fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
        SetName();
        StoreContext();
    }


    protected void ddlVessel_TextChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox vessel = (RadComboBox)sender;
        if (e.Value != string.Empty)
        {
            PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(e.Value);
            if (e.Text != string.Empty)
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = e.Text;
            Filter.CurrentOrderFormFilterCriteria = null;

            Filter.CurrentPurchaseStockType = null;
            Filter.CurrentPurchaseStockClass = null;
            Filter.CurrentPurchaseOrderIdSelection = null;

            Filter.CurrentNoonReportListFilter = null;
            Filter.CurrentArrivalReportFilter = null;
            Filter.CurrentDepartureReportFilter = null;
            Filter.CurrentShiftingReportFilter = null;
            Filter.CurrentVesselPositionReportFilter = null;
            Filter.CurrentVPRSVoyageFilter = null;

            DataSet ds = PhoenixGeneralSettings.GetUserOptions(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            PhoenixGeneralSettings.CurrentGeneralSetting = new GeneralSetting(ds);
            SessionUtil.ReBuildMenu();
            menuXML.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";
            XmlDataSourceModule.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";            
            //mnuNavigationBar.DataBind();
            radMenuTree.DataBind();
            if ((Session["currPage"].ToString().Contains("DashboardOfficeV2.aspx") || Session["currPage"].ToString().Contains("DashboardOfficeV2Crew.aspx")
                || Session["currPage"].ToString().Contains("DashboardVessel.aspx") || Session["currPage"].ToString().Contains("DashboardVesselDetails.aspx")))
                Session["currPage"] = "Dashboard/Dashboard.aspx";
            fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
            SetName();
            SetSkin();
            //if (ViewState["menucode"] != null)
            //    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "focus", "setTimeout(function(){ FocusItem('" + ViewState["menucode"].ToString() + "'); }, 500);", true);
        }
        else
        {
            vessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            vessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
        }
        StoreContext();
    }
    private void SetSkin()
    {
        //RadSkinManager s = (RadSkinManager)mainNavigation.Nodes[2].FindControl("RadSkinManager1");
        //s.Skin = Session["skin"].ToString();        
    }

    protected void ddlVessel_ItemCreated(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        //if (drv != null)
        //    e.Item.ImageUrl = Session["images"] + "/" + drv["FLDIMONUMBER"].ToString() + ".png";
    }

    protected void ddlVessel_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        //if (drv != null)
        //{
        //    var url = string.Empty;
        //    FileInfo fi = null;
        //    string imonumber = drv["FLDIMONUMBER"].ToString().Trim();
        //    if (!imonumber.Equals(""))
        //    {
        //        url = Session["images"] + "/vessel/" + imonumber + ".png";
        //        fi = new FileInfo(Server.MapPath("~/css/theme1/images/vessel/" + imonumber + ".png"));
        //    }
        //    if (imonumber.Equals("") || (fi != null && !fi.Exists))
        //    {
        //        url = Session["images"] + "/blank.png";
        //    }
        //    e.Item.ImageUrl = url;
        //}
    }
    private void SetName()
    {
        mainNavigation.Nodes[1].Text = PhoenixSecurityContext.CurrentSecurityContext.FirstName + " " + PhoenixSecurityContext.CurrentSecurityContext.LastName;
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (e.Value != string.Empty)
        {
            PhoenixSecurityContext.CurrentSecurityContext.CompanyID = int.Parse(e.Value);
            PhoenixSecurityContext.CurrentSecurityContext.CompanyName = e.Text;
            if (ViewState["modulecode"].ToString() == "QUA")
            {
                string shortcode = "QMS";

                SessionUtil.InsertQualityCompanyConfiguration(shortcode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                PhoenixSecurityContext.CurrentSecurityContext.CompanyContext.Clear();
                PhoenixSecurityContext.CurrentSecurityContext.CompanyContext.Add(shortcode, e.Value);
            }
            fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
        }
        StoreContext();
    }

    protected void ddlCompany_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {

    }
    private void StoreContext()
    {
        ViewState["module"] = Request.Form["hdnModule"];
        //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "setContext", "setTimeout(function(){setContext('"+ ViewState["module"].ToString() + "');},200);", true);
    }
    
    private string getRootMenu(RadPanelItem item)
    {
        while (item.Level > 0)
        {
            item = (RadPanelItem)item.Parent;
        }
        return item.Value;
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RadSkinManager1.Skin = Session["skin"].ToString();
        SetSkin();
    }
    protected void cmdApplication_Click(object sender, EventArgs e)
    {
        Session["currPage"] = "Dashboard/Dashboard.aspx";
        fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
        ((RadLabel)mainNavigation.Nodes[0].FindControl("lblTitle")).Text = "";
    }
    //protected void ddlModule_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    //{
    //    menuXML.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";
    //    if (ddlModule.SelectedValue != string.Empty)
    //        menuXML.XPath = "/RECORDSET/DIRECTORY[@CODE='" + ddlModule.SelectedValue + "']";
    //    else
    //        menuXML.XPath = "/RECORDSET/DIRECTORY";
    //    mnuNavigationBar.DataBind();
    //    RadMenu1.DataBind();
    //}

    protected void RadAjaxManager_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        int menuHeight;

        if (int.TryParse(e.Argument, out menuHeight))
        {
            //RadMenu1.Height = menuHeight;
            ViewState["MHEIGHT"] = menuHeight;
        }
    }
    private void LoadModule()
    {        
        string moduleXML = "<Menu><Group><Item Text=\"Phoenix\" Key=\"P\" Width=\"100%\"><Group Height=\"200\">";
        XmlDocument doc = XmlDataSourceModule.GetXmlDocument();
        if (doc.ChildNodes.Count > 0)
        {
            foreach (XmlNode node in doc.ChildNodes[0].ChildNodes)
            {
                var LeftIcon = GetIcon(node.Attributes["CODE"].InnerText);
                string icon = LeftIcon.Key != null ? "LeftLogo=\"css/Theme1/Images/"+ LeftIcon.Value + "\"" : string.Empty;
                moduleXML += "<Item Text=\"" + node.Attributes["NAME"].InnerText + "\" Value=\""+ node.Attributes["CODE"].InnerText + "\" "+ icon + "/>";
            }
        }
        moduleXML += "</Group></Item></Group></Menu>";
        PhoenixModule.LoadXml(moduleXML);
    }    
    private KeyValuePair<string, string> GetIcon(string MenuValue)
    {
        var icons = new List<KeyValuePair<string, string>>();
        icons.Add(new KeyValuePair<string, string>("ACC", "Accounts.png"));
        icons.Add(new KeyValuePair<string, string>("CSY", "Certificates.png"));
        icons.Add(new KeyValuePair<string, string>("CRW", "Crew.png"));
        icons.Add(new KeyValuePair<string, string>("DMS", "DocManagement.png"));
        icons.Add(new KeyValuePair<string, string>("DRD", "Docking.png"));
        icons.Add(new KeyValuePair<string, string>("QUA", "HSEQA.png"));
        icons.Add(new KeyValuePair<string, string>("INV", "Inventory.png"));
        icons.Add(new KeyValuePair<string, string>("PMS", "PlannedMaintenance.png"));
        icons.Add(new KeyValuePair<string, string>("PUR", "Purchase.png"));
        icons.Add(new KeyValuePair<string, string>("REG", "Register.png"));
        icons.Add(new KeyValuePair<string, string>("VAC", "VesselAcct.png"));
        icons.Add(new KeyValuePair<string, string>("VPS", "VesselPosition.png"));
        icons.Add(new KeyValuePair<string, string>("ADN", "Administration.png"));
        icons.Add(new KeyValuePair<string, string>("BGT", "Budget.png"));
        icons.Add(new KeyValuePair<string, string>("LOG", "Electronic-Log.png"));
        icons.Add(new KeyValuePair<string, string>("VPS", "IT.png"));
        icons.Add(new KeyValuePair<string, string>("OWN", "Owners.png"));
        icons.Add(new KeyValuePair<string, string>("PRT", "Portal.png"));
        icons.Add(new KeyValuePair<string, string>("PRE", "Pre-Sea.png"));
        icons.Add(new KeyValuePair<string, string>("OPT", "IT.png"));
        icons.Add(new KeyValuePair<string, string>("OFS", "Crew.png"));
        var result = icons.Find(p => p.Key == MenuValue);
        return result;
    }

    protected void PhoenixModule_ItemClick(object sender, RadMenuEventArgs e)
    {
        menuXML.DataFile = "~/js/temp/" + Session["TelerikMenuGuid"].ToString() + ".xml";
        if (!string.IsNullOrEmpty(e.Item.Value))
            menuXML.XPath = "/RECORDSET/DIRECTORY[@CODE='" + e.Item.Value + "']";
        else
            menuXML.XPath = "/RECORDSET/DIRECTORY";
        //mnuNavigationBar.DataBind();
        //RadMenu1.DataBind();
        radMenuTree.DataBind();
    }

    protected void radMenuTree_ItemDataBound(object sender, RadPanelBarEventArgs e)
    {
        XmlElement drv = (XmlElement)e.Item.DataItem;
        string[] url = Regex.Replace(e.Item.NavigateUrl, "JAVASCRIPT:OPENSEARCHPAGE\\(", "", RegexOptions.IgnoreCase).Replace("'", "").Replace(");", "").Split(',');
        if (url.Length > 0 && url[0] != string.Empty)
        {
            e.Item.Attributes["data-url"] = "~/" + url[0];
            e.Item.NavigateUrl = "";
            e.Item.Font.Bold = false;
        }
        else
        {
            e.Item.NavigateUrl = "javascript:void(0);";
        }
        e.Item.Value = drv.Attributes["CODE"].Value;
        if (e.Item.Level == 0)
        {
            var LeftIcon = GetIcon(drv.Attributes["CODE"].Value);
            string icon = LeftIcon.Key != null ? "css/Theme1/Images/" + LeftIcon.Value : string.Empty;
            if (!string.IsNullOrEmpty(icon))
                e.Item.ImageUrl = icon;
        }
        
    }

    protected void radMenuTree_ItemClick(object sender, RadPanelBarEventArgs e)
    {
        
       
        string[] menu = { "PUR", "QUA", "CSY", "INV", "VAC", "BGT", "VPS", "DRD", "PMS", "OFS", "DMR" };
        string[] cpyMenu = { "ACC", "QUA", "DMS" };
        string module = getRootMenu(e.Item);
        ViewState["modulecode"] = module;
        ((RadLabel)mainNavigation.Nodes[0].FindControl("lblTitle")).Text = "\xa0\xa0" + e.Item.Text;
        Filter.CurrentMenuCodeSelection = e.Item.Value;
        string url = e.Item.Attributes["data-url"];        
        if (!string.IsNullOrEmpty(url))
        {
            Session["currPage"] = url;
            fraPhoenixApplication.Attributes["src"] = url;
        }

        RadComboBox vessel = ((RadComboBox)mainNavigation.Nodes[2].FindControl("ddlVessel"));
        RadComboBox cpy = ((RadComboBox)mainNavigation.Nodes[3].FindControl("ddlCompany"));
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {                 
            if (module.Equals("QUA"))
            {
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (cpy != null && nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    cpy.ClearSelection();
                    cpy.SelectedValue = nvc.Get("QMS").ToString();
                }
            }
            else
            {

                cpy.ClearSelection();
                RadComboBoxItem val = cpy.FindItemByValue(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString());
                if (val != null)
                    cpy.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();

            }
        }
        ViewState["menucode"] = e.Item.Value;
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "setContext", "setTimeout(function(){telerikNav.valueChanging()},100);", true);
    }

    protected void lnkApplication_ServerClick(object sender, EventArgs e)
    {
        ((RadLabel)mainNavigation.Nodes[0].FindControl("lblTitle")).Text = "";
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            RadComboBox vessel = ((RadComboBox)mainNavigation.Nodes[2].FindControl("ddlVessel"));
            if (vessel != null)
            {
                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(vessel.SelectedItem.Value);
                PhoenixSecurityContext.CurrentSecurityContext.VesselName = vessel.SelectedItem.Text;
            }
        }
        Session["currPage"] = "Dashboard/Dashboard.aspx";
        fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
    }

    //protected void lnkOffice_ServerClick(object sender, EventArgs e)
    //{
    //    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
    //    {
    //        RadComboBox vessel = ((RadComboBox)mainNavigation.Nodes[2].FindControl("ddlVessel"));
    //        if (vessel != null)
    //        {
    //            PhoenixSecurityContext.CurrentSecurityContext.VesselID = 0;
    //            PhoenixSecurityContext.CurrentSecurityContext.VesselName = "--OFFICE--";
    //        }
    //        vessel.SelectedValue = "";
    //        vessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
    //    }
    //    Session["currPage"] = "Dashboard/Dashboard.aspx";
    //    fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
    //}

    protected void cmdVessel_Click(object sender, EventArgs e)
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            RadComboBox vessel = ((RadComboBox)mainNavigation.Nodes[2].FindControl("ddlVessel"));
            if (vessel != null)
            {
                vessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                vessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            }
        }
    }

    protected void ddlfms_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        RadComboBox dfms = ((RadComboBox)mainNavigation.Nodes[4].FindControl("ddlfms"));
        DataTable dt = PhoenixDocumentManagementFMSReports.FMSMenuConfigFetch(new Guid(dfms.SelectedValue));
        if (dfms != null)
        {
            if (dt.Rows.Count > 0)
            {
                Session["currPage"] = dt.Rows[0]["FLDURL"].ToString();
                fraPhoenixApplication.Attributes["src"] = Session["currPage"].ToString();
                ((RadLabel)mainNavigation.Nodes[0].FindControl("lblTitle")).Text = "\xa0\xa0" + dt.Rows[0]["FLDMENUNAME"].ToString();       
                        
            }
        }
        
    }

    protected void ddlfms_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {

    }
}
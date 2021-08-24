using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class PhoenixTaskPane : System.Web.UI.Page
{
    private string _defaultmodule = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
            Response.Redirect("PhoenixLogout.aspx");

        GeneralSetting gs = PhoenixGeneralSettings.CurrentGeneralSetting;
        DataSet ds = SessionUtil.ApplicationsInstalled();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Control ctl = Page.FindControl(dr["FLDAPPLICATIONLINK"].ToString());
            if (ctl != null) ctl.Visible = false;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["option"] != null)
            {
                if (Request.QueryString["option"].ToString().ToUpper().Equals("OPTIONS"))
                {
                    Filter.CurrentSelectedModule = "Options";
                    ShowModule("lnkOptions");
                }
                if (Request.QueryString["option"].ToString().ToUpper().Equals("DASHBOARD"))
                {
                    Filter.CurrentSelectedModule = "Dashboard";
                    ShowModule("lnkDashboard");
                }
            }
            else
            {
                if (Filter.CurrentSelectedModule != null)
                    ShowModule("lnk" + Filter.CurrentSelectedModule.Replace(" ", ""));
                else
                    ShowModule("");
            }

            if (Filter.CurrentSelectedModule != null)
                ShowContext("lnk" + Filter.CurrentSelectedModule.Replace(" ", ""));
            else
                ShowContext(_defaultmodule);

        }
        else
        {
            if (Filter.CurrentSelectedModule != null)
                ShowModule("lnk" + Filter.CurrentSelectedModule.Replace(" ", ""));
            else
                ShowModule("");
        }
    }

    private void ShowModule(string option)
    {        
        GeneralSetting gs = PhoenixGeneralSettings.CurrentGeneralSetting;
        DataSet ds = SessionUtil.ApplicationAccessList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        string[] modules = gs.Modules.Split(',');

        if (ds.Tables.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (string s in modules)
                {
                    if (dr["FLDAPPLICATIONLINK"].ToString().ToUpper().Equals(s.ToUpper()) && dr["FLDRIGHTS"].ToString().Equals("1"))
                    {
                        if (option.Equals(""))
                        {
                            option = s;
                            divHeading.InnerText = dr["FLDAPPLICATIONNAME"].ToString();
                            _defaultmodule = option;
                            Filter.CurrentSelectedModule = option.Replace("lnk", "");
                        }

                        if (Page.FindControl(s) != null && dr["FLDINSTALLEDYN"].ToString().Equals("1"))
                            Page.FindControl(s).Visible = true;

                        if (s.ToUpper().Equals(option.ToUpper()))
                        {
                            divHeading.InnerText = dr["FLDAPPLICATIONNAME"].ToString();
                            if (Page.FindControl(s) != null)
                                Page.FindControl(s).Visible = false;
                        }
                    }
                    
                }
            }
        }

        XmlMenu.DataFile = HttpContext.Current.Session["MenuGuid"].ToString();
        XmlMenu.XPath = "/RECORDSET/DIRECTORY[@NAME='" + divHeading.InnerText + "']";
        tvwMenu.DataSourceID = "XmlMenu";
    }

    protected void lnk_serverclick(object sender, EventArgs e)
    {
        HtmlAnchor a = (HtmlAnchor)sender;
        Filter.CurrentSelectedModule = a.InnerText;
        ShowModule(a.ID);
        ShowContext(a.ID);
    }

    protected void tvwMenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        tvwMenu.SelectedNode.Selected = true;
        tvwMenu.SelectedNodeStyle.CssClass = "navPanelMenu";
    }

    protected void tvwMenu_DataBinding(object sender, TreeNodeEventArgs e)
    {
            //e.Node.SelectAction = TreeNodeSelectAction.Expand;
            tvwMenu.SelectedNodeStyle.CssClass = "navPanelMenu";
    }

    private void ShowContext(string lnkID)
    {
        string Script = "";
        if (lnkID.Equals("lnkPurchase") || lnkID.Equals("lnkPlannedMaintenance") || lnkID.Equals("lnkInventory") || lnkID.Equals("lnkAccounts") 
            || lnkID.Equals("lnkQuality") || lnkID.Equals("lnkDocking") || lnkID.Equals("lnkVesselAccounting") || lnkID.Equals("lnkDocumentManagement")
            || lnkID.Equals("lnkVesselPosition") || lnkID.Equals("lnkOffshore") || lnkID.Equals("lnkOwners") || lnkID.Equals("lnkAdministration")
            || lnkID.Equals("lnkCrew") || lnkID.Equals("lnkDMR") || lnkID.Equals("lnkCertificatesandSurveys"))
        {
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "parent.showSwitch(true);";
            if (lnkID.Equals("lnkQuality"))
                Script += "parent.showQualityCompanySwitch(true);";
            else
                Script += "parent.showQualityCompanySwitch(false);";
            Script += "</script>" + "\n";
        }
        else
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "parent.showSwitch(false);";                
                Script += "parent.showQualityCompanySwitch(false);";
                Script += "</script>" + "\n";
            }
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

    }
}

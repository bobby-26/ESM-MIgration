using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionJHARACommentsSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Search", "Search", ToolBarDirection.Right);
        MenuSearch.AccessRights = this.ViewState;
        MenuSearch.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }
            BindSource();
            BindSection();
        }
    }
    protected void BindSource()
    {        		
        ddlSource.DataBind();
        ddlSource.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlSource.Items.Insert(1, new RadComboBoxItem("JHA", "1"));
        ddlSource.Items.Insert(2, new RadComboBoxItem("Process RA", "2"));
        ddlSource.Items.Insert(3, new RadComboBoxItem("Generic RA", "3"));
        ddlSource.Items.Insert(4, new RadComboBoxItem("Navigation RA", "4"));
        ddlSource.Items.Insert(5, new RadComboBoxItem("Machinery RA", "5"));       
        ddlSource.Items.Insert(6, new RadComboBoxItem("Cargo RA", "6"));
    }
    protected void BindSection()
    {
        DataTable dt = new DataTable();
        dt = PhoenixInspectionRiskAssessmentGenericExtn.RASectionList();

        ddlRAJHASections.DataSource = dt;
        ddlRAJHASections.DataTextField = "FLDNAME";
        ddlRAJHASections.DataValueField = "FLDRISKASSESSMENTSECTIONID";
        ddlRAJHASections.DataBind();
        ddlRAJHASections.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void MenuSearch_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection find = new NameValueCollection();

            find.Clear();

            find.Add("RefNo",txtRAJHA.Text);
            find.Add("ddlRAJHASections", ddlRAJHASections.SelectedIndex == 0? null : ddlRAJHASections.SelectedValue);
            find.Add("ddlSource", ddlSource.SelectedIndex == 0 ? null : ddlSource.SelectedValue);

            InspectionFilter.CurrentRACommentFilter = find;           
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
       
    }
}
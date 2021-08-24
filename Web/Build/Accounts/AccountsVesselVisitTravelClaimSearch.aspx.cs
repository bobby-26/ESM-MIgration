using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelClaimSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuFilterMain.AccessRights = this.ViewState;
        MenuFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {

            txtEmployeeNameSearch.Focus();
            ViewState["SelectedVesselList"] = "";
            ViewState["SelectedClaimStatus"] = "";
            ViewState["SelectedVisitType"] = "";
            ViewState["SelectedExpenseType"] = "";
            BindVesselList();
            BindClaimStatus();
        }
    }

    private void BindClaimStatus()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 132);
        chkClaimStatus.Items.Add("select");
        chkClaimStatus.DataSource = ds;
        chkClaimStatus.DataTextField = "FLDQUICKNAME";
        chkClaimStatus.DataValueField = "FLDSHORTNAME";
        chkClaimStatus.DataBind();
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkVesselList_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVesselList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVesselList"] = ViewState["SelectedVesselList"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }

    protected void chkClaimStatus_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkClaimStatus.Items)
        {
            if (item.Selected == true && !ViewState["SelectedClaimStatus"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedClaimStatus"] = ViewState["SelectedClaimStatus"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedClaimStatus"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedClaimStatus"] = ViewState["SelectedClaimStatus"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }
    protected void chkVisitType_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVisitType.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVisitType"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVisitType"] = ViewState["SelectedVisitType"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVisitType"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVisitType"] = ViewState["SelectedVisitType"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }
    protected void MenuFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();
            criteria.Add("txtEmployeeNameSearch", txtEmployeeNameSearch.Text.Trim());
            criteria.Add("txtFromDate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);
            criteria.Add("chkVesselList", ViewState["SelectedVesselList"].ToString());
            criteria.Add("txtclaimStatus", ViewState["SelectedClaimStatus"].ToString());
            criteria.Add("txtFormNumber", txtFormNumber.Text.Trim());
            criteria.Add("ddlVisitTypeSearch", ViewState["SelectedVisitType"].ToString());
            criteria.Add("ddlExpensetype", ddlExpensetype.SelectedValue);
            Filter.CurrentTravelClaimPostingFilter = criteria;
        }
       if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentTravelClaimPostingFilter = criteria;
        }
        Session["New"] = "Y";
        string script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

}

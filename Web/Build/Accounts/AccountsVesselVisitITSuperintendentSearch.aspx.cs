using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselVisitITSuperintendentSearch : PhoenixBasePage
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
            ViewState["SelectedVisitStatus"] = "";
            ViewState["SelectedClaimStatus"] = "";
            ViewState["SelectedVisitType"] = "";
            ViewState["SelectedExpenseType"] = "";
            BindVesselList();
            BindVisitStatus();
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

    private void BindVisitStatus()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(1, 252);
        chkVisitStatus.Items.Add("select");
        chkVisitStatus.DataSource = ds;
        chkVisitStatus.DataTextField = "FLDHARDNAME";
        chkVisitStatus.DataValueField = "FLDHARDCODE";
        chkVisitStatus.DataBind();
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

    protected void chkVisitStatus_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkVisitStatus.Items)
        {
            if (item.Selected == true && !ViewState["SelectedVisitStatus"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVisitStatus"] = ViewState["SelectedVisitStatus"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedVesselList"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedVisitStatus"] = ViewState["SelectedVisitStatus"].ToString().Replace("," + item.Value.ToString() + ",", ",");
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
            criteria.Add("txtFormNo", txtFormNo.Text.Trim());
            criteria.Add("txtstartFromDate",txtstartFromDate.Text);
            criteria.Add("txtstartToDate", txtstartToDate.Text);
            criteria.Add("txtendFromDate", txtendFromDate.Text);
            criteria.Add("txtendToDate", txtendToDate.Text);
            criteria.Add("chkVesselList", ViewState["SelectedVesselList"].ToString());
            criteria.Add("txtVisitStatus", ViewState["SelectedVisitStatus"].ToString());
            criteria.Add("txtclaimStatus", ViewState["SelectedClaimStatus"].ToString());
            criteria.Add("ddlVisitTypeSearch", ViewState["SelectedVisitType"].ToString());
            criteria.Add("ddlExpensetype", ddlExpensetype.SelectedValue);
            //criteria.Add("ddlbudgeted", ddlbudgeted.SelectedValue);
            criteria.Add("FIND", "FIND");
            Filter.CurrentVesselvisitSelection = criteria;
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentVesselvisitSelection = criteria;
        }
        Session["New"] = "Y";
        string script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}

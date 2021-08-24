using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelAdvanceReturnSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        MenuFilterMain.AccessRights = this.ViewState;
        MenuFilterMain.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            txtEmployeeNameSearch.Focus();
            ViewState["SelectedVesselList"] = "";
            ViewState["SelectedClaimStatus"] = "";
            ViewState["SelectedCurrency"] = "";
            BindVesselList();
            BindClaimStatus();
            BindCurrency();

            ucAdvanceStatus.QuickTypeCode = "133";
            ucAdvanceStatus.bind();
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

    private void BindCurrency()
    {
        DataSet ds = PhoenixRegistersCurrency.ListCurrency(1);
        chkCurrencyList.Items.Add("select");
        chkCurrencyList.DataSource = ds;
        chkCurrencyList.DataTextField = "FLDCURRENCYCODE";
        chkCurrencyList.DataValueField = "FLDCURRENCYID";
        chkCurrencyList.DataBind();
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListAllVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
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

    protected void chkCurrencyList_Changed(object sender, EventArgs e)
    {
        foreach (ListItem item in chkCurrencyList.Items)
        {
            if (item.Selected == true && !ViewState["SelectedCurrency"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedCurrency"] = ViewState["SelectedCurrency"].ToString() + ',' + item.Value.ToString();
            }
            if (item.Selected == false && ViewState["SelectedCurrency"].ToString().Contains("," + item.Value.ToString() + ","))
            {
                ViewState["SelectedCurrency"] = ViewState["SelectedCurrency"].ToString().Replace("," + item.Value.ToString() + ",", ",");
            }
        }
    }

    protected void MenuFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection filterCriteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            filterCriteria.Clear();
            filterCriteria.Add("txtEmployeeName", txtEmployeeNameSearch.Text.Trim());
            filterCriteria.Add("txtAdvanceNumber", txtAdvanceNumber.Text);
            filterCriteria.Add("txtVisitNo", txtVisitId.Text);
            filterCriteria.Add("chkVesselList", ViewState["SelectedVesselList"].ToString());
            filterCriteria.Add("txtPurpose", txtPurpose.Text);
            filterCriteria.Add("chkCurrencyList", ViewState["SelectedCurrency"].ToString());
            filterCriteria.Add("chkClaimStatus", ViewState["SelectedClaimStatus"].ToString());
            filterCriteria.Add("ucAdvanceStatus", ucAdvanceStatus.SelectedQuick);
            Filter.CurrentTravelAdvance = filterCriteria;
        }
       if (CommandName.ToUpper().Equals("CANCEL"))
        {
            filterCriteria.Clear();
            Filter.CurrentTravelAdvance = filterCriteria;
        }
        Session["New"] = "Y";
        string script = "javascript:fnReloadList('codehelp1');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}

using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;


public partial class OwnerBudgetProposalAddEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuAddEditProposal.AccessRights = this.ViewState;
        MenuAddEditProposal.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (Request.QueryString["proposalid"] != null)
            {
                ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
            }

            if (Request.QueryString["revisionid"] != null)
            {
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
            }
            BindYear();
            ucProposalDate.Text = DateTime.Now.Date.ToString();

            if (ViewState["PROPOSALID"] != null)
                BindData();
        }
        
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 1); i <= (DateTime.Today.Year) + 5; i++)
        {
            DropDownListItem li = new DropDownListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }
    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixOwnerBudget.BudgetProposalEdit(new Guid(ViewState["PROPOSALID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            ucOfficerWageScale.SelectedAddress = dt.Rows[0]["FLDOFFICERCBA"].ToString();
            ucRatingsWageScale.SelectedAddress = dt.Rows[0]["FLDRATINGCBA"].ToString();
            ucAnalyzeWith.SelectedVessel = dt.Rows[0]["FLDANALYZEWITH"].ToString();
            txtProposalTitle.Text = dt.Rows[0]["FLDPROPOSALTITLE"].ToString();
            ucProposalDate.Text = dt.Rows[0]["FLDPROPOSALDATE"].ToString();
            ddlYear.SelectedValue = dt.Rows[0]["FLDBUDGETYEAR"].ToString();

            if (dt.Rows[0]["FLDVESSELID"].ToString() != "")
            {
                rblst.SelectedIndex = 1;
                txtVesselName.Visible = false;
                //ucAnalyzeWith.Enabled = false;
                ucVesselName.Visible = true;
            }
            if (rblst.SelectedValue == "1")
                ucVesselName.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
            else
                txtVesselName.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
        }
    }
    protected void ddlCrewWageChanged(object sender, EventArgs e)
    {
        int vesselid;
        if (ucVesselName.SelectedVessel != "Dummy")
            vesselid = Int32.Parse(ucVesselName.SelectedVessel);

        else
            vesselid = (ucAnalyzeWith.SelectedVessel == "Dummy") ? 0 : Int32.Parse(ucAnalyzeWith.SelectedVessel);


        DataTable dt = new DataTable();
        dt = PhoenixOwnerBudget.CrewWageChanged(vesselid);

        if (dt.Rows.Count > 0)
        {
            ucOfficerWageScale.SelectedAddress = dt.Rows[0]["FLDWAGESCALE"].ToString();
            ucRatingsWageScale.SelectedAddress = dt.Rows[0]["FLDWAGESCALERATING"].ToString();
        }

        ucAnalyzeWith.SelectedVessel = ucVesselName.SelectedVessel;
    }

    protected void ucAnalyzeWith_TextChanged(object sender, EventArgs e)
    {
        int vesselid;
        if (ucVesselName.SelectedVessel != "Dummy" && !string.IsNullOrEmpty(ucVesselName.SelectedVessel))
            vesselid = Int32.Parse(ucVesselName.SelectedVessel);

        else
            vesselid = (ucAnalyzeWith.SelectedVessel == "Dummy") ? 0 : Int32.Parse(ucAnalyzeWith.SelectedVessel);


        DataTable dt = new DataTable();
        dt = PhoenixOwnerBudget.CrewWageChanged(vesselid);

        if (dt.Rows.Count > 0)
        {
            ucOfficerWageScale.SelectedAddress = dt.Rows[0]["FLDWAGESCALE"].ToString();
            ucRatingsWageScale.SelectedAddress = dt.Rows[0]["FLDWAGESCALERATING"].ToString();
        }

    }

    protected void rblst_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblst.SelectedValue == "1")
        {
            txtVesselName.Visible = false;
            ucAnalyzeWith.SelectedVessel = "Dummy";
            ucOfficerWageScale.SelectedAddress = "Dummy";
            ucRatingsWageScale.SelectedAddress = "Dummy";
            ucAnalyzeWith.Enabled = false;
            ucVesselName.Visible = true;
        }
        else
        {
            ucVesselName.Visible = false;
            ucVesselName.SelectedVessel = "Dummy";
            ucOfficerWageScale.SelectedAddress = "Dummy";
            ucRatingsWageScale.SelectedAddress = "Dummy";
            txtVesselName.Visible = true;
            ucAnalyzeWith.Enabled = true;
            ucAnalyzeWith.SelectedVessel = "Dummy";
        }
      
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        string proposaltitle = txtProposalTitle.Text;
        string proposalvessel = (txtVesselName.Text == "") ? ucVesselName.SelectedVessel : txtVesselName.Text;

        if ((proposaltitle == null) || (proposaltitle == ""))
            ucError.ErrorMessage = "Proposal title is required.";

        if ((proposalvessel == null) || (proposalvessel == "") || (proposalvessel == "Dummy"))
            ucError.ErrorMessage = "Proposal vessel is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    protected void MenuAddEditProposal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;



            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PROPOSALID"] == null)
                    InsertVesselBudget();
                else
                    UpdateVesselBudget();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void UpdateVesselBudget()
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixOwnerBudget.BudgetProposalUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , new Guid(ViewState["PROPOSALID"].ToString())
                                                                  , txtProposalTitle.Text
                                                                  , General.GetNullableDateTime(ucProposalDate.Text)
                                                                  , General.GetNullableInteger(ucVesselName.SelectedVessel.ToString())
                                                                  , txtVesselName.Text == string.Empty ? ucVesselName.SelectedVesselName : txtVesselName.Text
                                                                  , General.GetNullableInteger(ucAnalyzeWith.SelectedVessel.ToString())
                                                                  , General.GetNullableInteger(ucOfficerWageScale.SelectedAddress.ToString())
                                                                  , General.GetNullableInteger(ucRatingsWageScale.SelectedAddress.ToString())
                                                                  , General.GetNullableInteger(ddlYear.SelectedValue)
                                                                  );
        ucStatus.Text = "Budget proposal updated";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', '', null);", true);
    }

    private void InsertVesselBudget()
    {
        if (!IsValidData())
        {
            ucError.Visible = true;
            return;
        }
        string proposalid = "";
        proposalid = PhoenixOwnerBudget.BudgetProposalInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableString(txtProposalTitle.Text)
                                                             , General.GetNullableDateTime(ucProposalDate.Text)
                                                             , General.GetNullableInteger(ucVesselName.SelectedVessel.ToString())
                                                             , txtVesselName.Text == string.Empty ? ucVesselName.SelectedVesselName : txtVesselName.Text
                                                             , General.GetNullableInteger(ucAnalyzeWith.SelectedVessel.ToString())
                                                             , General.GetNullableInteger(ucOfficerWageScale.SelectedAddress.ToString())
                                                             , General.GetNullableInteger(ucRatingsWageScale.SelectedAddress.ToString())
                                                             , General.GetNullableInteger(ddlYear.SelectedValue)
                                                             );
        ViewState["PROPOSALID"] = proposalid.ToString();
        ucStatus.Text = "Budget proposal added ";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1', '', null);", true);
    }
}

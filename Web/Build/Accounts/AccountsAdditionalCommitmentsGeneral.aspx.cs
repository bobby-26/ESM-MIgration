using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsAdditionalCommitmentsGeneral : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        cmdHiddenPick.Attributes.Add("style", "display:none;");
      
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW",ToolBarDirection.Right);
        
        MenuAdvancePayment.AccessRights = this.ViewState;
        MenuAdvancePayment.MenuList = toolbar.Show();


        //btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + ucVessel.SelectedVessel.ToString() + "&budgetid=" + txtBudgetId.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

        if (!IsPostBack)
        {
            txtBudgetId.Attributes.Add("style", "display:none");
            txtBudgetgroupId.Attributes.Add("style", "display:none");
            txtOwnerBudgetgroupId.Attributes.Add("style", "display:none");
            txtOwnerBudgetId.Attributes.Add("style", "display:none");
            txtOwnerBudgetName.Attributes.Add("style", "display:none");
            txtVendorId.Attributes.Add("style", "display:none");

            txtBudgetCode.Attributes.Add("readonly", "readonly");
            txtBudgetName.Attributes.Add("readonly", "readonly");
            txtOwnerBudgetCode.Attributes.Add("readonly", "readonly");
            txtVendorCode.Attributes.Add("readonly", "readonly");


            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ADDITIONALCOMMITMENTID"] = "";

            if (Request.QueryString["ADDITIONALCOMMITMENTID"] != null && Request.QueryString["ADDITIONALCOMMITMENTID"] != string.Empty)
            {
                ViewState["ADDITIONALCOMMITMENTID"] = Request.QueryString["ADDITIONALCOMMITMENTID"].ToString();

                AdditionalCommitmentEdit();
            }

            btnShowBudget.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListBudgetRemainingBalance.aspx?framename=ifMoreInfo&budgetgroup=106&hardtypecode=30&vesselid=" + ucVessel.SelectedVessel + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?framename=ifMoreInfo', true); ");

            BindProjectCode();
        }
    }

    private void BindProjectCode()
    {
        ucProjectcode.bind(General.GetNullableInteger(ddlAccountDetails.SelectedValue),General.GetNullableInteger(txtBudgetId.Text));
    }

    private void Reset()
    {
        ucVessel.SelectedVessel = "";
        ViewState["ADDITIONALCOMMITMENTID"] = null;
        ddlAccountDetails.Items.Clear();
        ddlAccountDetails.Items.Add(new RadComboBoxItem("--Select--", ""));

        txtBudgetId.Text = "";
        txtBudgetCode.Text = "";
        txtBudgetName.Text = "";
        txtCreatedBy.Text = "";
        txtCreatedDate.Text = "";
        txtUpdatedBy.Text = "";
        txtUpdatedDate.Text = "";
        txtOwnerBudgetCode.Text = "";
        txtOwnerBudgetgroupId.Text = "";
        txtOwnerBudgetId.Text = "";
        txtOwnerBudgetName.Text = "";
        txtPONumber.Text = "";
        txtVenderName.Text = "";
        txtVendorCode.Text = "";
        txtVendorId.Text = "";
        ucPODate.Text = "";
        ucReversedDate.Text = "";
        ucAmount.Text = "";
        txtDescription.Text = "";
        ddlAccountDetails.Enabled = true;
        btnShowBudget.Visible = true;
        txtPONumber.Enabled = true;
        ImgSupplierPickList.Visible = true;
        ucPODate.Enabled = true;
        //ucAmount.en = true;
        ucVessel.Enabled = true;
        txtDescription.Enabled = true;

        ucProjectcode.SelectedProjectCode = "";
        ucProjectcode.SelectedText = "";


    }

    protected void MenuAdvancePayment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
                BindProjectCode();
                ViewState["ADDITIONALCOMMITMENTID"] = "";
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidAdditionalCommitment())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["ADDITIONALCOMMITMENTID"].ToString() == "")
                {
                    PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(ucVessel.SelectedVessel),
                        General.GetNullableInteger(ddlAccountDetails.SelectedValue),
                        General.GetNullableInteger(txtBudgetId.Text),
                        General.GetNullableGuid(txtOwnerBudgetId.Text),
                        txtPONumber.Text,
                        General.GetNullableInteger(txtVendorId.Text),
                        General.GetNullableDateTime(ucPODate.Text),
                        General.GetNullableDateTime(ucReversedDate.Text),
                        General.GetNullableDecimal(ucAmount.Text),
                        txtDescription.Text,
                        General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                        );

                    ucStatus.Text = "Additional commitment information added";
                    Reset();
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["ADDITIONALCOMMITMENTID"].ToString()),
                        General.GetNullableGuid(txtOwnerBudgetId.Text),
                        General.GetNullableDateTime(ucReversedDate.Text),
                        txtDescription.Text,
                        General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                        );
                    ucStatus.Text = "Additional commitment information updated";
                }

                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidAdditionalCommitment()
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(ddlAccountDetails.SelectedValue) == null)
            ucError.ErrorMessage = "Vessel account is required.";

        if (txtBudgetId.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Budget code is required.";
        string code = txtBudgetCode.Text;
        if (txtOwnerBudgetId.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Owner Budget Code is required.";

        if (txtPONumber.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "PO number is required.";

        if (txtVendorId.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Supplier is required.";

        if (General.GetNullableDateTime(ucPODate.Text) == null)
            ucError.ErrorMessage = "PO Date is required.";

        if (General.GetNullableDateTime(ucReversedDate.Text) != null)
        {
            if (!string.IsNullOrEmpty(ucPODate.Text)
                        && DateTime.TryParse(ucReversedDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(ucPODate.Text)) < 0)
                ucError.ErrorMessage = "Reversed date should be greater than PO date.";
        }
        if (txtDescription.Text.Trim() == string.Empty)
        {
            ucError.ErrorMessage = "Description is required.";
        }

        if (ucAmount.Text.Trim() == string.Empty)
            ucError.ErrorMessage = "Amount is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        //DataSet dsprincipal = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));

        //if (dsprincipal.Tables.Count > 0)
        //{
        //    if (dsprincipal.Tables[0].Rows.Count > 0)
        //    {
        //        ViewState["Ownerid"] = dsprincipal.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
        //    }
        //}

        btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + ucVessel.SelectedVessel + "&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");

        if (btnShowBudget != null)
        {
            BindProjectCode();
        }

    }

    protected void AdditionalCommitmentEdit()
    {
        try
        {
            if (ViewState["ADDITIONALCOMMITMENTID"] != null)
            {
                DataSet ds = PhoenixAccountsAdditionalCommitments.AccountsAdditionalCommitmentsEdit(General.GetNullableGuid(ViewState["ADDITIONALCOMMITMENTID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();

                    //ddlAccountDetails.Visible = true;
                    ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                        General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
                    ddlAccountDetails.DataBind();

                    ddlAccountDetails.SelectedValue = dr["FLDACCOUNTID"].ToString();

                    txtBudgetId.Text = dr["FLDESMBUDGETID"].ToString();
                    txtBudgetCode.Text = dr["FLDBUDGETCODE"].ToString();
                    txtBudgetName.Text = dr["FLDDESCRIPTION"].ToString();
                    txtOwnerBudgetCode.Text = dr["FLDOWNERBUDGETGROUP"].ToString();
                    txtOwnerBudgetId.Text = dr["FLDOWNERBUDGETCODE"].ToString();
                    txtPONumber.Text = dr["FLDPONUMBER"].ToString();
                    txtVendorCode.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVenderName.Text = dr["FLDSUPPLIERNAME"].ToString();
                    txtVendorId.Text = dr["FLDSUPPLIERID"].ToString();
                    ucPODate.Text = dr["FLDPODATE"].ToString();
                    ucReversedDate.Text = dr["FLDREVERSEDDATE"].ToString();
                    txtDescription.Text = dr["FLDREMARKS"].ToString();
                    ucAmount.Text = dr["FLDAMOUNT"].ToString();
                    txtCreatedDate.Text = dr["FLDCREATEDDATE"].ToString();
                    txtUpdatedDate.Text = dr["FLDMODIFIEDDATE"].ToString();
                    txtCreatedBy.Text = dr["FLDCREATEDBY"].ToString();
                    txtUpdatedBy.Text = dr["FLDMODIFIEDBY"].ToString();
                    ucProjectcode.SelectedProjectCode = dr["FLDPROJECTID"].ToString();

                    btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + dr["FLDVESSELID"].ToString() + "&Ownerid=" + ViewState["Ownerid"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetId.Text + "', true); ");
                   
                    ddlAccountDetails.Enabled = false;
                    btnShowBudget.Visible = false;
                    txtPONumber.Enabled = false;
                    ImgSupplierPickList.Visible = false;
                    ucPODate.Enabled = false;
                    //ucAmount.Enabled = false;
                    ucVessel.Enabled = false;
                    //if (General.GetNullableDateTime(ucReversedDate.Text) != null)
                    //{
                    //    ucReversedDate.Enabled = false;
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        if (ucVessel.SelectedVessel != "" && !(ucVessel.SelectedVessel.ToUpper().Equals("DUMMY")))
        {

            btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', 'Common/CommonPickListOwnerBudget.aspx?vesselid=" + ucVessel.SelectedVessel.ToString() + "&Ownerid=" + ViewState["Ownerid"] + "&budgetid=" + txtBudgetId.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");

            ddlAccountDetails.Visible = true;
            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                General.GetNullableInteger(ucVessel.SelectedVessel), 1);
            ddlAccountDetails.DataBind();
        }
    }

    protected void ddlAccountDetails_TextChanged(object sender, EventArgs e)
    {
        DataSet dsprincipal = PhoenixRegistersAccount.EditAccount(int.Parse(ddlAccountDetails.SelectedValue));

        if (dsprincipal.Tables.Count > 0)
        {
            if (dsprincipal.Tables[0].Rows.Count > 0)
            {
                ViewState["Ownerid"] = dsprincipal.Tables[0].Rows[0]["FLDPRINCIPALID"].ToString();
            }
        }

        BindProjectCode();
    }
}

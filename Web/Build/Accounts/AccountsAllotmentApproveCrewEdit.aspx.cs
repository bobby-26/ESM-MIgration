using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsAllotmentApproveCrewEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["ALLOTMENTID"] = null;
                ViewState["DTKEY"] = string.Empty;
                if (Request.QueryString["ALLOTMENTID"] != null)
                {
                    ViewState["ALLOTMENTID"] = Request.QueryString["ALLOTMENTID"].ToString();
                    ViewState["REQUESTNO"] = Request.QueryString["REQUESTNO"].ToString();
                }
                EditAllotment();
                PhoenixToolbar toolbarsub = new PhoenixToolbar();

                toolbarsub.AddButton("Approve", "APPROVED", ToolBarDirection.Right);
                toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
                //   toolbarsub.AddLinkButton("javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKEY"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS.ToString() + "'); return false;", "Attachment", "ATTACHMENT");
                MenuChecking.AccessRights = this.ViewState;
                MenuChecking.MenuList = toolbarsub.Show();
            }
            MenuChecking.Title = Request.QueryString["REQUESTNO"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditAllotment()
    {
        try
        {
            DataTable dt = PhoenixAccountsAllotmentRequest.AllotmentCrewEdit(new Guid(ViewState["ALLOTMENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewState["PBREMARKID"] = dr["FLDREMARKSID"].ToString();
                MenuChecking.Title = dr["FLDREQUESTNUMBER"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                txtFileNo.Text = dr["FLDFILENO"].ToString();
                txtRank.Text = dr["FLDRANKNAME"].ToString();
                lblAllotment.Text = dr["FLDALLOTMENTTYPENAME"].ToString();
                txtName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtName.ToolTip = dr["FLDEMPLOYEENAME"].ToString();
                txtBeneficiary.Text = dr["FLDACCOUNTNAME"].ToString();
                txtBeneficiaryBank.Text = dr["FLDBANKNAME"].ToString();
                txtAccountNo.Text = dr["FLDACCOUNTNUMBER"].ToString();
                txtBankAddress.Text = dr["FLDADDRESS1"].ToString();
                txtBankAddress.ToolTip = dr["FLDADDRESS1"].ToString();
                txtIFSCCode.Text = dr["FLDBANKIFSCCODE"].ToString();
                txtAmount.Text = dr["FLDAMOUNT"].ToString();
                txtBOW.Text = dr["FLDBOW"].ToString().Equals("") ? "N/A" : dr["FLDBOW"].ToString();
                txtBowDate.Text = dr["FLDBOW"].ToString().Equals("") ? "N/A" : General.GetDateTimeToString(dr["FLDBOWDATE"].ToString());
                ViewState["DTKEY"] = dr["FLDDTKEY"].ToString();
                ViewState["FLDATTACHMENTYN"] = dr["FLDATTACHMENTYN"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtApprovedBy.Text = dr["FLDCREWDEPTAPPROVEDBY"].ToString() + " On " + General.GetDateTimeToString(dr["FLDCREWDEPTAPPROVEDDATE"].ToString());
                txtswiftcode.Text = dr["FLDINTERMEDIATEBANKSWIFTCODE"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuChecking_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper() == "APPROVED")
            {
                PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCrewDepartmentapprove(General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString()), 1);
                EditAllotment();
                ucStatus.Visible = true;
                ucStatus.Text = "Status Changed";

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
            }
            else if (CommandName.ToUpper() == "SAVE")
            {
                if (General.GetNullableString(txtRemarks.Text.Trim()) == null)
                {
                    ucError.ErrorMessage = "Remarks is required.";
                    ucError.Visible = true;
                }
                if (General.GetNullableString(txtRemarks.Text) != null)
                {
                    if (General.GetNullableGuid(ViewState["PBREMARKID"].ToString()) == null)
                    {
                        PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksInsert(
                               General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                              , null
                              , 5
                              , General.GetNullableString(txtRemarks.Text));
                    }
                    else
                    {
                        PhoenixAccountsAllotmentRequestSystemChecking.AllotmentCheckingRemarksUpdate(
                                    General.GetNullableGuid(ViewState["ALLOTMENTID"].ToString())
                                    , General.GetNullableGuid(ViewState["PBREMARKID"].ToString())
                                    , 5
                                    , General.GetNullableString(txtRemarks.Text));
                    }
                    EditAllotment();
                    ucStatus.Visible = true;
                    ucStatus.Text = "Remarks updated.";
                    string Script = "";
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('Accounts','ifMoreInfo','keepopen');";
                    Script += "</script>" + "\n";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", Script, false);
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            EditAllotment();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

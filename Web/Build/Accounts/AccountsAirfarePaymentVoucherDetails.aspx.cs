using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Accounts_AccountsAirfarePaymentVoucherDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVendorId.Attributes.Add("style", "visibility:hidden");

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            PhoenixToolbar toolbargridCreditNotes = new PhoenixToolbar();
            
            short showcreditnotedisc = PhoenixGeneralSettings.CurrentGeneralSetting.ShowCreditNoteDiscount;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
        
            if (showcreditnotedisc == 1)
                toolbarmain.AddButton("Report", "REPORT",ToolBarDirection.Right);

            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["callfrom"] = null;
                ViewState["CURRENCYCODE"] = null;
                Title1.Text = "Invoice Payment Voucher Details";
            }
            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];
            BindDataMain();
            BindDataVessel();
            BindPendingCreditNote();
            BindCreditNote();

            //if (ViewState["PVStatuscode"].ToString() != "48")
            //{
            //    cmdApprove.Attributes.Add("onclick", "parent.Openpopup('PaymentVoucherApproval', '', '../Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=1&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            //}
            //else
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "ZEROPV")
                    Response.Redirect("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx");
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx");
            }

            if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + ViewState["voucherid"] + "&applicationcode=5&reportcode=AIRFAREPAYMENTVOUCHER&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            LinkButton lblVessel = (LinkButton)e.Item.FindControl("lblVessel");
            RadLabel lblCancelledYN = (RadLabel)e.Item.FindControl("lblCancelledyn");
            RadLabel lblNonVesselyn = (RadLabel)e.Item.FindControl("lblNonVesselyn");
            lblVessel.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsAirfarePaymentVoucherInvoiceDetails.aspx?voucherid=" + ViewState["voucherid"].ToString() + "&VesselId="
                                + lblVesselId.Text+ "&CancelledYN="
                                + lblCancelledYN.Text + "&NonVesselYN="
                                + lblNonVesselyn.Text + "');return true;");
        }


    }

    protected void gvIVList_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
        }
    }

    private void BindDataVessel()
    {
        DataSet ds = PhoenixAccountsAirfarePaymentVoucherDetails.VesselList(ViewState["voucherid"].ToString());

            gvVessel.DataSource = ds;

    }

    private void BindDataMain()
    {
        DataSet ds = PhoenixAccountsAirfarePaymentVoucherDetails.PVSearch(ViewState["voucherid"].ToString());

        DataRow dr = ds.Tables[0].Rows[0];

        txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
        txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());

        txtVendorCode.Text = dr["FLDCODE"].ToString();
        txtVendorName.Text = dr["FLDNAME"].ToString();
        txtVendorId.Text = dr["FLDPAYMENTVOUCHERID"].ToString();
        ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
        ViewState["CURRENCYCODE"] = dr["FLDCURRENCY"].ToString();
        txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
        txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
        txtAccountNumber.Text = dr["FLDBANKNAME"].ToString();
        ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
        ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
        lblBankdetails.Text = dr["FLDBANKINGDETAILS"].ToString();
        txtBankdetails.Text = dr["FLDSWIFTCODE"].ToString();
        txtAmount.Text = dr["FLDPAYABLEAMOUNT"].ToString();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindDataMain();
        BindDataVessel();
        BindPendingCreditNote();
        BindCreditNote();
    }

    private void BindPendingCreditNote()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsAirfarePaymentVoucherDetails.AirfareCreditNotePendingSearch(General.GetNullableInteger(ViewState["SuppCode"].ToString())
                                                            , General.GetNullableInteger(ViewState["CURRENCYCODE"].ToString())
                                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , General.GetNullableGuid(ViewState["voucherid"].ToString())
                                                            , chkZeroAmount.Checked == true ? 1 : 0);

            gvCreditNotePending.DataSource = ds;

    }

    private void BindCreditNote()
    {
        if (ViewState["voucherid"] != null)
        {


            DataSet ds = PhoenixAccountsAirfarePaymentVoucherDetails.PVAirfareCreditMappingSearch(new Guid(ViewState["voucherid"].ToString()));

                gvCreditNotes.DataSource = ds;
        }
    }

    protected void CheckZeroAmount(object sender, EventArgs e)
    {
        try
        {
            BindPendingCreditNote();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNotes_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            RadLabel lblCreditNoteid = (RadLabel)e.Item.FindControl("lblCreditNoteId");
            RadLabel lblPaymentVOucherId = (RadLabel)e.Item.FindControl("lblPaymentVOucherId");
        }
    }

    protected void gvCreditNotes_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel lblCreditNoteMappingid = (RadLabel)e.Item.FindControl("lblCreditMappingId");

                PhoenixAccountsAirfarePaymentVoucherDetails.PVAirfareCreditMappingDelete(General.GetNullableGuid(lblCreditNoteMappingid.Text));
            }
            BindCreditNote();
            BindPendingCreditNote();
            gvCreditNotes.Rebind();
            gvCreditNotePending.Rebind();

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                RadLabel lblCreditMappingIdEdit = (RadLabel)e.Item.FindControl("lblCreditMappingIdEdit");
                RadTextBox txtCreditAmount = (RadTextBox)e.Item.FindControl("txtCurrentUtilization");

                RadLabel lblOriginalAmount = (RadLabel)e.Item.FindControl("lblAmount");
                RadLabel lblAlreadyUtilised = (RadLabel)e.Item.FindControl("lblAlreadyUtilized");

                decimal balance = decimal.Parse(lblOriginalAmount.Text) - decimal.Parse(lblAlreadyUtilised.Text);

                if (General.GetNullableDecimal(txtCreditAmount.Text) == 0)
                {
                    ucError.ErrorMessage = "Amount to be entered";
                    ucError.Visible = true;
                    return;
                }

                if (!(balance >= decimal.Parse(txtCreditAmount.Text)))
                {
                    ucError.ErrorMessage = "Amount to be less than available amount";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixAccountsAirfarePaymentVoucherDetails.PVAirfareCreditMappingUpdate(General.GetNullableGuid(lblCreditMappingIdEdit.Text), decimal.Parse(txtCreditAmount.Text));

                  //  _gridView.EditIndex = -1;

                    BindCreditNote();
                    BindPendingCreditNote();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNotePending_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                RadLabel lblairfareCreditId = (RadLabel)e.Item.FindControl("lblairfareCreditId");
                RadLabel lblBalance = (RadLabel)e.Item.FindControl("lblBalance");

                PhoenixAccountsAirfarePaymentVoucherDetails.PVAirfareCreditMappingInsert(new Guid(ViewState["voucherid"].ToString())
                        ,new Guid(lblairfareCreditId.Text)
                        ,Convert.ToDecimal(lblBalance.Text));
            }
            BindCreditNote();
            BindPendingCreditNote();
            gvCreditNotePending.Rebind();
            gvCreditNotes.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCreditNotePending_ItemCreated(object sender, GridItemEventArgs e)
    {

    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DirectApproval(int Approvaltype)
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(Approvaltype, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), Approvaltype, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), Approvaltype, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), Approvaltype, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());

    }

    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "PAA")));
            BindDataMain();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "visibility:hidden");
            }
            //ucConfirmMessage.Visible = true;
            //ucConfirmMessage.Text = "Do you want Proceed?.";
            //return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCreditNotePending_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindPendingCreditNote();
    }

    protected void gvCreditNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindCreditNote();
    }

    protected void gvVessel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindDataVessel();
    }
}

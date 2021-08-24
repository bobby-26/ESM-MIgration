using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsCtmPaymentVoucherLineItemDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtVendorId.Attributes.Add("style", "display:none");


            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //  MenuOrderFormMain.SetTrigger(pnlStockItemEntry);

            PhoenixToolbar toolbarrevoke = new PhoenixToolbar();
            toolbarrevoke.AddButton("Revoke", "REVOKE", ToolBarDirection.Right);
            MenuRevoke.AccessRights = this.ViewState;
            MenuRevoke.MenuList = toolbarrevoke.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                Title1.Text = "Ctm Payment Voucher Details";
                ViewState["callfrom"] = null;
                BindHeaderData();
            }

            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];

            PhoenixToolbar toolbargridCreditNotes = new PhoenixToolbar();
            toolbargridCreditNotes.AddImageLink("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListCreditNote.aspx?mode=custom&CURRENCYCODE=" + ViewState["CurrencyCode"].ToString() + "&SUPPCODE=" + ViewState["SuppCode"].ToString() + "', true);", "Credit Notes List", "add.png", "ADDCREDITNOTE");

            if (ViewState["PVStatuscode"].ToString() != "48")
            {
                string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
               //mdApprove.Attributes.Add("onclick", "parent.openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            }
            else
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "display:none");
            }
            // BindData();
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
                    Response.Redirect("../Accounts/AccountsInvoiceZeroPaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
                else
                    Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?voucherid=" + voucherid
                                            + "&vesselid=null&applicationcode=5&reportcode=CTMPAYMENTVOUCHER&showexcel=no", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindHeaderData()
    {
        if (ViewState["voucherid"] != null)
        {
            DataSet ds = PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherEdit(ViewState["voucherid"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    txtAmount.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"])); ;
                    ViewState["CurrencyCode"] = dr["FLDCURRENCY"].ToString();
                    ViewState["SuppCode"] = dr["FLDSUPPLIERCODE"].ToString();
                    ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUSCODE"].ToString();
                    ViewState["PVType"] = dr["FLDPAYMENTVOUCHERTYPE"].ToString();

                    if (ViewState["PVStatuscode"].ToString() == "48")
                    {
                        cmdApprove.Attributes.Add("style", "visibility:hidden");
                    }

                    txtRevokeBy.Text = dr["FLDREVOKEDBY"].ToString();
                    if (General.GetNullableDateTime(dr["FLDREVOKEDDATE"].ToString()) != null)
                        txtRevokedDate.Text = dr["FLDREVOKEDDATE"].ToString();
                    txtRevokeRemarks.Text = dr["FLDREVOKEDREASON"].ToString();
                }

                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0];
                    txtBeneficiaryBankName.Text = dr["FLDBANKNAME"].ToString();
                    txtBeneficiaryName.Text = dr["FLDBENEFICIARYNAME"].ToString();
                    txtAccountNumber.Text = dr["FLDACCOUNTNUMBER"].ToString();
                    lblBankdetails.Text = dr["FLDBANKINGDETAILS"].ToString();
                    txtBankdetails.Text = dr["FLDSWIFTCODE"].ToString();
                }
            }
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDDATE", "FLDSEAPORTNAME", "FLDETA", "FLDSUPPLIERNAME", "FLDAMOUNT", "FLDRECEIVEDDATE", "FLDRECEIVEDAMOUNT", "FLDSTATUS" };
            string[] alCaptions = { "CTM Date", "Required Port", "ETA Date", "Port Agent", "CTM Amount", "Received Date", "Received Amount", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsCtm.CtmPaymentVoucherLineItemSearch(new Guid(ViewState["voucherid"].ToString())
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCTM", "CTM Request", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["CTMID"] == null)
                {
                    ViewState["CTMID"] = ds.Tables[0].Rows[0]["FLDCAPTAINCASHID"].ToString();
                    ViewState["ACTIVEYN"] = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString();
                    //  gvCTM.SelectedIndex = 0;
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //ShowNoRecordsFound(dt, gvCTM);
            }
            gvCTM.DataSource = ds;
            gvCTM.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton lnkreference = (LinkButton)e.Item.FindControl("lnkReferenceNumber");

            if (lnkreference != null)
                lnkreference.Attributes.Add("onclick", "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod=ACCOUNTS&U=1'); return false;");
        }
    }
    protected void gvCTM_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                string CaptainCashid = ((RadLabel)e.Item.FindControl("lblCaptainCashId")).Text;
                PhoenixAccountsCtm.CtmPaymentVoucherLineItemUpdate(new Guid(CaptainCashid));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        Response.Redirect("../Accounts/AccountsCtmPaymentVoucherLineItemDetails.aspx?voucherid=" + ViewState["voucherid"].ToString());
    }

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    try
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //        gv.DataSource = dt;
    //        gv.DataBind();

    //        int colcount = gv.Columns.Count;
    //        gv.Rows[0].Cells.Clear();
    //        gv.Rows[0].Cells.Add(new TableCell());
    //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //        gv.Rows[0].Cells[0].Font.Bold = true;
    //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    ViewState["CTMID"] = null;
    //    BindData();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvCTM.SelectedIndex = -1;
    //    gvCTM.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
    //    ViewState["CTMID"] = null;
    //    BindData();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindHeaderData();
    }
    protected void MenuRevoke_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REVOKE"))
            {
                if (General.GetNullableString(txtRevokeRemarks.Text) == null)
                {
                    ucError.ErrorMessage = "Revoke Remarks is required.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherRevoke(General.GetNullableGuid(ViewState["voucherid"].ToString()), txtRevokeRemarks.Text);
                BindHeaderData();

                if (ViewState["PVStatuscode"].ToString() != "48")
                {
                    string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
                    cmdApprove.Attributes.Add("style", "visibility:visible");
                   //mdApprove.Attributes.Add("onclick", "openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
                }
                else
                {
                    cmdApprove.Attributes.Add("style", "display:none");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DirectApproval(int ApprovalType)
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), ApprovalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), ApprovalType, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());
        ucStatus.Text = "Payment voucher Approved";
    }

    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "PAA")));
            BindHeaderData();
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "display:none");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCTM_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

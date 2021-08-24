using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using System.Net.Mail;

public partial class Accounts_AccountsPaymentVoucherAdvanceRequests : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
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

                Title1.Text = "Advance Payment Payment Voucher Details";
                ViewState["callfrom"] = null;
                BindHeaderData();
            }
            if ((Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty))
                ViewState["callfrom"] = Request.QueryString["callfrom"];
            if (ViewState["PVStatuscode"].ToString() != "48")
            {
                string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
                cmdApprove.Attributes.Add("style", "visibility:visible");
                // cmdApprove.Attributes.Add("onclick", "parent.openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
            }

            else
            if (ViewState["PVStatuscode"].ToString() == "48")
            {
                cmdApprove.Attributes.Add("style", "display:none");
            }
            //  BindInvoicePOData();
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
                string invoiceid = (ViewState["INVOICEID"] == null) ? null : (ViewState["INVOICEID"].ToString());
                string paymentvoucherinvoiceid = (ViewState["PAYMENTVOUCHERINVOICEID"] == null) ? null : (ViewState["PAYMENTVOUCHERINVOICEID"].ToString());
                string voucherid = (ViewState["voucherid"] == null) ? null : (ViewState["voucherid"].ToString());

                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());

                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?&voucherid=" + voucherid +
                                           "&applicationcode=5&reportcode=ADVANCEPAYMENTVOUCHER&showexcel=no", false);
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

    private void BindInvoicePOData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDCURRENCYNAME", "FLDTRANSACTIONAMOUNT", "FLDBASEAMOUNT", "FLDREPORTAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Sub Account", "Transaction Currency", "Transaction Amount", "Base Amount", "Report Amount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoicePaymentVoucher.AdvancePaymentVoucherSearch(new Guid(ViewState["voucherid"].ToString()), sortexpression, sortdirection
                                                 , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                 , ref iRowCount, ref iTotalPageCount);


        gvInvoicePo.DataSource = ds;
        gvInvoicePo.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvInvoice", "Voucher Line Item", alCaptions, alColumns, ds);
    }


    protected void gvInvoicePo_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }

        if (e.Item is GridDataItem)
        {
            // if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                LinkButton ll = (LinkButton)e.Item.FindControl("lblReferenceNo");
                RadLabel lb = (RadLabel)e.Item.FindControl("lblOrderId");
                RadLabel l = (RadLabel)e.Item.FindControl("lblAdvancePaymentType");
                RadLabel vesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                if (!SessionUtil.CanAccess(this.ViewState, ll.CommandName)) ll.Visible = false;
                if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "LIC")))
                    ll.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Crew/CrewLicenceRequestLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + "');return false;");
                if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "SAS")))
                    ll.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/Purchase/PurchaseFormLineItem.aspx?orderid=" + lb.Text + "&refno=" + ll.Text + "&vesselid=" + vesselid.Text + "');return false;");
                if (l.Text.Equals(PhoenixCommonRegisters.GetHardCode(1, 124, "PAR")))
                    ll.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsInvoiceDirectPurchaseOrderLineItem.aspx?ORDERID=" + lb.Text + "&vslid=" + vesselid.Text + "');return false;");

                LinkButton lnkAttachment = (LinkButton)e.Item.FindControl("lnkAttachments");
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");
                lnkAttachment.Attributes.Add("onclick", "openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text + "&mod=" + PhoenixModule.ACCOUNTS + "&U=1');return false;");

            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoicePo_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                //GridView _gridView = (GridView)sender;
                //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                DeleteInvoiceMapping(((RadLabel)e.Item.FindControl("lblPaymentVOucherId")).Text
                    , ((RadLabel)e.Item.FindControl("lblAdvancePaymentId")).Text);
                ViewState["INVOICEID"] = null;
                ViewState["ORDERID"] = null;
            }
            BindInvoicePOData();
            gvInvoicePo.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        BindInvoicePOData();
    }

    private void DeleteInvoiceMapping(string paymentvoucherid, string advancepaymentid)
    {
        PhoenixAccountsInvoicePaymentVoucher.AdvancePaymentVoucherDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(paymentvoucherid), new Guid(advancepaymentid));
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindHeaderData();
        BindInvoicePOData();
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
                gvInvoicePo.Rebind();

                if (ViewState["PVStatuscode"].ToString() != "48")
                {
                    string vouchertype = ViewState["PVType"].ToString() == "239" ? "0" : "1";
                    cmdApprove.Attributes.Add("style", "visibility:visible");
                    // cmdApprove.Attributes.Add("onclick", "parent.openNewWindow('PaymentVoucherApproval', '', '" + Session["sitepath"] + "/Common/CommonApproval.aspx?docid=" + ViewState["voucherid"].ToString() + "&mod=" + PhoenixModule.ACCOUNTS + "&type=" + 459 + "&suppliercode=" + ViewState["SuppCode"].ToString() + "&vouchertype=" + vouchertype + "&user=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "');return false;");
                }
                else
                {
                    cmdApprove.Attributes.Add("style", "visibility:hidden");
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
        SendNotificationMail();
    }
    private void SendNotificationMail()
    {
        try
        {

            string to, cc, bcc, mailsubject, mailbody, sessionid = string.Empty;
            bool htmlbody = true;
            MailPriority priority = MailPriority.Normal;
            int? iEmployeeId = null;
            DataTable dsPurchaser = PhoenixAccountsInvoicePaymentVoucher.Paymentvouchermaildetails(new Guid(ViewState["voucherid"].ToString()));
            if (dsPurchaser.Rows.Count > 0)
            {
                int count = dsPurchaser.Rows.Count;
                while (count > 0)
                {
                    DataRow drPurchaser = dsPurchaser.Rows[count - 1];
                    if (drPurchaser["FLDSOURCE"].ToString() == "APT")
                    {
                        mailbody = "Dear Purchaser," + "<br/><br/>" + "PO Number :" + drPurchaser["FLDREFERENCEDOCUMENT"].ToString() + " " + "has been" + " " + drPurchaser["FLDPAYMENTVOUCHERSTATUSNAME"].ToString() + "." + "<br/><br/>" + "Thanks & Regards" + "<br/>" + "Product Support";

                        // string comma = ",";
                        to = drPurchaser["FLDPURCHASEREMAIL"].ToString();
                        cc = "";
                        bcc = "";
                        mailsubject = "PO Number :" + drPurchaser["FLDREFERENCEDOCUMENT"].ToString() + " " + "-" + " " + drPurchaser["FLDPAYMENTVOUCHERSTATUSNAME"].ToString();
                        PhoenixMail.SendMail(to, cc, bcc, mailsubject, mailbody, htmlbody, priority, sessionid, iEmployeeId);
                    }
                    count--;
                }
                // ucStatus.Text = "Email sent successfully";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "PAA")));
            BindHeaderData();
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


    protected void gvInvoicePo_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindInvoicePOData();
    }
}

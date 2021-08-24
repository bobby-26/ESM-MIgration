using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class AccountsPaymentVoucherVesselVisitTravelAdvance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["voucherid"] != null && Request.QueryString["voucherid"] != string.Empty)
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher", "VOUCHER", ToolBarDirection.Right);
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbarLine = new PhoenixToolbar();
            toolbarLine.AddImageButton("../Accounts/AccountsPaymentVoucherVesselVisitTravelAdvance.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarLine.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbarLine.Show();

            PhoenixToolbar toolbarrevoke = new PhoenixToolbar();
            toolbarrevoke.AddButton("Revoke", "REVOKE", ToolBarDirection.Right);
            MenuRevoke.AccessRights = this.ViewState;
            MenuRevoke.MenuList = toolbarrevoke.Show();
            if (!IsPostBack)
            {
                ViewState["PVStatuscode"] = null;
                ViewState["PVType"] = null;
                ViewState["SuppCode"] = null;
                gvLineItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                PaymentVoucherEdit();
            }

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
                Response.Redirect("../Accounts/AccountsInvoicePaymentVoucherMaster.aspx?voucherid=" + ViewState["voucherid"].ToString());
            }
            if (CommandName.ToUpper().Equals("REPORT"))
            {
                PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherReportviewDetailsInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["voucherid"].ToString());
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?paymentvoucherinvoiceid=" + ViewState["voucherid"] + "&applicationcode=5&reportcode=TRAVELADVANCEPAYMENTVOUCHER&showexcel=no", false);
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
        PaymentVoucherEdit();
    }
    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void PaymentVoucherEdit()
    {
        try
        {
            DataSet ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvancePaymentVoucherEdit(new Guid(ViewState["voucherid"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVoucherNo.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                ucVoucherDate.Text = dr["FLDCREATEDDATE"].ToString();
                txtEmployeeCode.Text = dr["FLDUSERNAME"].ToString();
                txtEmployeeName.Text = dr["FLDEMPLOYEENAME"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtpaymentAmount.Text = dr["FLDPAYABLEAMOUNT"].ToString();

                if (General.GetNullableDateTime(dr["FLDREVOKEDDATE"].ToString()) != null)
                {
                    txtRevokedDate.Text = dr["FLDREVOKEDDATE"].ToString();
                    txtRevokeRemarks.Text = dr["FLDREVOKEDREASON"].ToString();
                    txtRevokeBy.Text = dr["FLDREVOKEDBY"].ToString();
                }

                ViewState["SuppCode"] = dr["FLDEMPLOYEENAME"].ToString();
                ViewState["PVStatuscode"] = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();
                ViewState["PVType"] = dr["FLDTYPE"].ToString();
                if (ViewState["PVStatuscode"].ToString() == "48")
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

    protected void gvLineItem_ItemDataBound(Object sender, GridItemEventArgs e)
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

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDPAYMENTAMOUNT", "FLDAPPROVEDBY", "FLDAPPROVEDDATE", "FLDREMARKS" };
            string[] alCaptions = { "Travel Advance No", "Vessel Name", "Amount", "Approved By", "Approved Date", "Remarks" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvancePaymentVoucherLineItem(new Guid(ViewState["voucherid"].ToString()));

            General.SetPrintOptions("gvLineItem", "Travel Advance", alCaptions, alColumns, ds);


            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDVESSELNAME", "FLDPAYMENTAMOUNT", "FLDAPPROVEDBY", "FLDAPPROVEDDATE", "FLDREMARKS" };
        string[] alCaptions = { "Travel Advance No", "Vessel Name", "Amount", "Approved By", "Approved Date", "Remarks" };

        DataSet ds = PhoenixAccountsVesselVisitTravelAdvance.TravelAdvancePaymentVoucherLineItem(new Guid(ViewState["voucherid"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelAdvance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void cmdApprove_OnClientClick(object sender, EventArgs e)
    {
        try
        {
            DirectApproval(int.Parse(PhoenixCommonRegisters.GetHardCode(1, 98, "PAA").ToString()));
            PaymentVoucherEdit();
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

    private void DirectApproval(int approvalType)
    {

        int iApprovalStatusAccounts;
        int? onbehaalf = null;
        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(approvalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

        if (dt.Rows.Count > 0)
        {
            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
        }
        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["voucherid"].ToString(), approvalType, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

        byte bAllApproved = 0;
        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["voucherid"].ToString(), approvalType, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), approvalType, ViewState["voucherid"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());
        ucStatus.Text = "Payment voucher Approved";
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
                PaymentVoucherEdit();

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
    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardRemittanceGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "Display:None");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceGenerate.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsGenerateRemittanceFilter.aspx", "Find", "search.png", "FIND");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Generate Remittance for All Payment Vouchers", "GENERATEALL", ToolBarDirection.Right);
            toolbarmain.AddButton("Generate Remittance", "NEW", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Generate Remittance";
            MenuOrderFormMain.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Session["CHECKED_ITEMS"] != null)
                    Session.Remove("CHECKED_ITEMS");

                if (Session["CHECKED_CTM"] != null)
                    Session.Remove("CHECKED_CTM");

                if (Session["CHECKED_OTHERS"] != null)
                    Session.Remove("CHECKED_OTHERS");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["voucherid"] = null;
                ViewState["PAGEURL"] = null;
                gvVoucherDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ArrayList SelectedPvs = new ArrayList();
                ArrayList SelectedPvsCtm = new ArrayList();
                ArrayList SelectedPvsOthers = new ArrayList();

                string selectedagents = ",";
                string selectedCtm = ",";
                string selectedOthers = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (Guid index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                if (Session["CHECKED_CTM"] != null)
                {
                    SelectedPvsCtm = (ArrayList)Session["CHECKED_CTM"];
                    if (SelectedPvsCtm != null && SelectedPvsCtm.Count > 0)
                    {
                        foreach (Guid index in SelectedPvsCtm)
                            selectedCtm = selectedCtm + index + ",";
                    }
                }

                if (Session["CHECKED_OTHERS"] != null)
                {
                    SelectedPvsOthers = (ArrayList)Session["CHECKED_OTHERS"];
                    if (SelectedPvsOthers != null && SelectedPvsOthers.Count > 0)
                    {
                        foreach (Guid index in SelectedPvsOthers)
                            selectedOthers = selectedOthers + index + ",";
                    }
                }

                if (SelectedPvs.Count > 0)
                {
                    if (SelectedPvsCtm.Count > 0)
                        PhoenixAccountsRemittance.GenerateRemittance(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedCtm.Length > 1 ? selectedCtm : null);
                    if (SelectedPvsOthers.Count > 0)
                        PhoenixAccountsRemittance.GenerateRemittance(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, selectedOthers.Length > 1 ? selectedOthers : null);
                    Rebind();

                }
                else
                {
                    ucError.ErrorMessage = "Please select payment voucher.";
                    ucError.Visible = true;
                    return;
                }
            }

            if (CommandName.ToUpper().Equals("GENERATEALL"))
            {
                PhoenixAccountsRemittance.AllocateAdvancePaymentVouchers(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                PhoenixAccountsRemittance.GenerateRemittance(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                Rebind();
            }
            Response.Redirect("../Accounts/AccountsRemittanceGenerate.aspx");

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvVoucherDetails.SelectedIndexes.Clear();
        gvVoucherDetails.EditIndexes.Clear();
        gvVoucherDetails.DataSource = null;
        gvVoucherDetails.Rebind();
    }
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvVoucherDetails$ctl00$ctl02$ctl01$chkAllRemittance")
        {
            ArrayList SelectedPvs = new ArrayList();
            ArrayList SelectedCTM = new ArrayList();
            ArrayList SelectedOthers = new ArrayList();
            string paymentvouchertype = "";
            string Paymentvouchertypehard = PhoenixCommonRegisters.GetHardCode(1, 221, "CTM").ToString();
            RadCheckBox chkAll = new RadCheckBox();
            foreach (GridHeaderItem headerItem in gvVoucherDetails.MasterTableView.GetItems(GridItemType.Header))
            {
                chkAll = (RadCheckBox)headerItem["Listcheckbox"].FindControl("chkAllRemittance"); // Get the header checkbox
            }
            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

            if (Session["CHECKED_CTM"] != null)
                SelectedCTM = (ArrayList)Session["CHECKED_CTM"];

            if (Session["CHECKED_OTHERS"] != null)
                SelectedOthers = (ArrayList)Session["CHECKED_OTHERS"];

            foreach (GridDataItem row in gvVoucherDetails.Items)
            {
                paymentvouchertype = ((RadLabel)row.FindControl("lblPaymentVoucherType")).Text;
                Guid index = new Guid(((RadLabel)row.FindControl("lblPaymentVoucherID")).Text);
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");

                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                    if (!SelectedPvs.Contains(index))
                    {
                        SelectedPvs.Add(index);
                    }
                    if (paymentvouchertype == Paymentvouchertypehard)
                    {
                        if (!SelectedCTM.Contains(index))
                            SelectedCTM.Add(index);
                    }
                    else
                    {
                        if (!SelectedOthers.Contains(index))
                            SelectedOthers.Add(index);
                    }
                }
                else
                {
                    cbSelected.Checked = false;
                    if (SelectedPvs.Contains(index))
                    {
                        SelectedPvs.Remove(index);
                    }
                    SelectedCTM.Remove(index);
                    SelectedOthers.Remove(index);
                }
            }
            if (SelectedPvs != null && SelectedPvs.Count > 0)
                Session["CHECKED_ITEMS"] = SelectedPvs;

            if (SelectedCTM != null && SelectedCTM.Count > 0)
                Session["CHECKED_CTM"] = SelectedCTM;

            if (SelectedOthers != null && SelectedOthers.Count > 0)
                Session["CHECKED_OTHERS"] = SelectedOthers;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (Filter.CurrentGenerateRemittenceFilterSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentGenerateRemittenceFilterSelection;

            ds = PhoenixAccountsInvoicePaymentVoucher.RemittanceGeneratePaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch").ToString().Trim())
                                                        , General.GetNullableInteger(nvc.Get("txtMakerId").ToString().Trim())
                                                        , General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim())
                                                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                        , 48
                                                        , 0
                                                        , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherFromdateSearch")) : null
                                                        , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherTodateSearch")) : null
                                                        , General.GetNullableInteger(nvc.Get("ucTechFleet").ToString().Trim())
                                                        , General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim())
                                                        , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber").ToString().Trim())
                                                        , null
                                                        , General.GetNullableInteger(nvc.Get("ddlSource").ToString().Trim())
                                                        , General.GetNullableInteger(nvc.Get("ddlType").ToString().Trim())
                                                        , sortexpression, sortdirection
                                                        , 1
                                                        , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                        , ref iRowCount, ref iTotalPageCount);
        }
        else
        {

            ds = PhoenixAccountsInvoicePaymentVoucher.RemittanceGeneratePaymentVoucherSearch("", null
                                                                             , null
                                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                             , 48
                                                                             , 0
                                                                             , null
                                                                             , null
                                                                             , null
                                                                             , null
                                                                             , null
                                                                             , null
                                                                             , null
                                                                             , null
                                                          , sortexpression, sortdirection
                                                          , 1
                                                          , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                          , ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=InvoicePaymentVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Voucher</h3></td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentGenerateRemittenceFilterSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentGenerateRemittenceFilterSelection;
            ds = PhoenixAccountsInvoicePaymentVoucher.RemittanceGeneratePaymentVoucherSearch(General.GetNullableString(nvc.Get("txtVoucherNumberSearch"))
                    , General.GetNullableInteger(nvc.Get("txtMakerId"))
                    , General.GetNullableInteger(nvc.Get("ddlCurrencyCode"))
                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                    , 48
                    , 0
                    , General.GetNullableString(nvc.Get("txtVoucherFromdateSearch"))
                    , General.GetNullableString(nvc.Get("txtVoucherTodateSearch"))
                    , General.GetNullableInteger(nvc.Get("ucTechFleet"))
                    , General.GetNullableString(nvc.Get("txtInvoiceNumber"))
                    , General.GetNullableString(nvc.Get("txtPurchaseInvoiceVoucherNumber"))
                    , null
                    , General.GetNullableInteger(nvc.Get("ddlSource"))
                    , General.GetNullableInteger(nvc.Get("ddlType"))
                    , sortexpression, sortdirection
                    , gvVoucherDetails.CurrentPageIndex + 1
                    , gvVoucherDetails.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixAccountsInvoicePaymentVoucher.RemittanceGeneratePaymentVoucherSearch("", null, null
                                       , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                       , 48
                                       , 0
                                       , string.Empty
                                       , string.Empty
                                       , null
                                       , null
                                       , null
                                       , null
                                       , null
                                       , null
                                       , sortexpression, sortdirection
                                       , gvVoucherDetails.CurrentPageIndex + 1
                                       , gvVoucherDetails.PageSize
                                       , ref iRowCount, ref iTotalPageCount);
        }

        string[] alCaptions = { "Voucher Number", "Voucher Date", "Supplier Name", "Currency", "Amount" };
        string[] alColumns = { "FLDPAYMENTVOUCHERNUMBER", "FLDCREATEDDATE", "FLDNAME", "FLDCURRENCYCODE", "FLDAMOUNT" };

        General.SetPrintOptions("gvVoucherDetails", "Invoice Voucher", alCaptions, alColumns, ds);

        {
            gvVoucherDetails.DataSource = ds;
            gvVoucherDetails.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


            if (ViewState["voucherid"] == null && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["voucherid"] = ds.Tables[0].Rows[0]["FLDPAYMENTVOUCHERID"].ToString();
            }
        }
    }

    protected void gvVoucherDetails_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkSelect");

            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (chk != null && Session["CHECKED_ITEMS"] != null)
            {
                ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                Guid index = new Guid();
                index = new Guid(dr["FLDPAYMENTVOUCHERID"].ToString());
                if (SelectedPvs.Contains(index))
                {
                    chk.Checked = true;
                }
            }
            if (chk != null && Session["CHECKED_CTM"] != null)
            {
                ArrayList SelectedPvs = (ArrayList)Session["CHECKED_CTM"];
                Guid index = new Guid();
                index = new Guid(dr["FLDPAYMENTVOUCHERID"].ToString());
                if (SelectedPvs.Contains(index))
                {
                    chk.Checked = true;
                }
            }

            if (chk != null && Session["CHECKED_OTHERS"] != null)
            {
                ArrayList SelectedPvs = (ArrayList)Session["CHECKED_OTHERS"];
                Guid index = new Guid();
                index = new Guid(dr["FLDPAYMENTVOUCHERID"].ToString());
                if (SelectedPvs.Contains(index))
                {
                    chk.Checked = true;
                }
            }

        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        ArrayList SelectedCTM = new ArrayList();
        ArrayList SelectedOthers = new ArrayList();
        Guid index = new Guid();
        string paymentvouchertype = "";
        string Paymentvouchertypehard = PhoenixCommonRegisters.GetHardCode(1, 221, "CTM").ToString();
        foreach (GridDataItem gvrow in gvVoucherDetails.MasterTableView.Items)
        {
            bool result = false;

            index = new Guid(gvrow.GetDataKeyValue("FLDPAYMENTVOUCHERID").ToString());
            paymentvouchertype = ((RadLabel)gvrow.FindControl("lblPaymentVoucherType")).Text;

            if (Session["CHECKED_ITEMS"] != null)
                SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];

            if (Session["CHECKED_CTM"] != null)
                SelectedCTM = (ArrayList)Session["CHECKED_CTM"];

            if (Session["CHECKED_OTHERS"] != null)
                SelectedOthers = (ArrayList)Session["CHECKED_OTHERS"];

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);

                if (paymentvouchertype == Paymentvouchertypehard)
                {
                    if (!SelectedCTM.Contains(index))
                        SelectedCTM.Add(index);
                }
                else
                {
                    if (!SelectedOthers.Contains(index))
                        SelectedOthers.Add(index);
                }
            }
            else
            {
                SelectedPvs.Remove(index);
                SelectedCTM.Remove(index);
                SelectedOthers.Remove(index);
            }
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
            Session["CHECKED_ITEMS"] = SelectedPvs;

        if (SelectedCTM != null && SelectedCTM.Count > 0)
            Session["CHECKED_CTM"] = SelectedCTM;

        if (SelectedOthers != null && SelectedOthers.Count > 0)
            Session["CHECKED_OTHERS"] = SelectedOthers;
    }

    private void GetSelectedPvs()
    {
        if (Session["CHECKED_ITEMS"] != null)
        {
            ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridViewRow row in gvVoucherDetails.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvVoucherDetails.MasterTableView.DataKeyValues[row.RowIndex].ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvVoucherDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
    }
}

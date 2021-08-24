using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;
using System.Web.UI;
using System.Globalization;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class OwnersSOAWithSupportingsVoucherBreakup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbargrid.AddFontAwesomeButton("../Owners/OwnersSOAWithSupportingsVoucherBreakup.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");

            if (SessionUtil.CanAccess(this.ViewState, "Excel") || SessionUtil.CanAccess(this.ViewState, "PRINT"))
            {
                MenuOrderForm.AccessRights = this.ViewState;
                MenuOrderForm.MenuList = toolbargrid.Show();
            }
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                //ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();
                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["budgetgroupid"] = Request.QueryString["budgetgroupid"].ToString();
                ViewState["VoucherDtKey"] = "";
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Owners/OwnersStatementOfAccounts.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESC", "FLDDEBITNOTEREFERENCE", "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Statement Reference", "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountVoucherListOwnerBudget(
        //    ViewState["debitnotereference"].ToString(), int.Parse(ViewState["Ownerid"].ToString()),
        //    General.GetNullableGuid(ViewState["ownerbudgetid"].ToString()), int.Parse(ViewState["accountid"].ToString()),
        //    (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);

        ds = PhoenixOwnerReportQuality.OwnersOperatingEpnsBreakup(Filter.SelectedOwnersDebitnoteReference,
            General.GetNullableInteger(ViewState["budgetgroupid"].ToString()),
            int.Parse(ViewState["accountid"].ToString()), (int)ViewState["PAGENUMBER"],
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VoucherLineItemsDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Voucher LineItems Details</center></h3></td>");
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
                if (i == 8)
                {
                    Response.Write(String.Format("{0:#,###,###.##}", dr[alColumns[i]]));
                }
                else
                {
                    Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                }
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESC", "FLDDEBITNOTEREFERENCE", "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description", "Statement Reference", "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixOwnerReportQuality.OwnersOperatingEpnsBreakup(Filter.SelectedOwnersDebitnoteReference,
            General.GetNullableInteger(ViewState["budgetgroupid"].ToString()),
            int.Parse(ViewState["accountid"].ToString()), (int)ViewState["PAGENUMBER"],
            iRowCount, ref iRowCount, ref iTotalPageCount);
        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = ds.Tables[0].Rows.Count;
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblAccountId.Text = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
            lblAccountCOdeDescription.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString();
            lnkOwnerBudgetCode.Text = ds.Tables[0].Rows[0]["FLDESMBUDGETCODE"].ToString();
            lblBudgetCodeDescription.Text = ds.Tables[0].Rows[0]["FLDBUDGETCODEDESCRIPTION"].ToString();
            lblStatementReference.Text = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
            if (ViewState["DtKey"] == null)
            {
                ViewState["DtKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ViewState["VoucherDtKey"] = ds.Tables[0].Rows[0]["FLDVOUCHERDTKEY"].ToString();
            }
            BindAttachment(ViewState["DtKey"].ToString(), ViewState["VoucherDtKey"].ToString());
            BindAttachmentLink(ViewState["DtKey"].ToString());
            BindVoucherAttachmentLink(ViewState["VoucherDtKey"].ToString());
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            lblTotal.Text = String.Format(CultureInfo.InvariantCulture, "{0:#,#.00}", ds.Tables[1].Rows[0]["FLDAMOUNT"]);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);
    }
    protected void BindAttachment(string dtkey, string voucherleveldtkey)
    {
        DataSet dsattachment = new DataSet();
        DataSet dsvoucherlevelattachment = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;

        dsvoucherlevelattachment = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(voucherleveldtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        dsattachment = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (dsvoucherlevelattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drvoucherlevelattachment = dsvoucherlevelattachment.Tables[0].Rows[0];
            string src = "../common/download.aspx?dtkey=" + drvoucherlevelattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = drvoucherlevelattachment["FLDFILEPATH"].ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
        else if (dsattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drattachment = dsattachment.Tables[0].Rows[0];

            string src = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = drattachment["FLDFILEPATH"].ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;// +"#page=" + ViewState["INVOICEBANKINFOPAGENUMBER"];
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "";
        }
    }
       protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        { }
    }
    protected void gvOwnersAccount_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridGroupHeaderItem)
        {
            (e.Item as GridGroupHeaderItem).Cells[0].Controls.Clear();
            (e.Item as GridGroupHeaderItem).Cells[0].Visible = false;
        }
    }
    protected void gvOwnersAccount_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        if (e.Column is GridGroupSplitterColumn)
        {
            GridGroupSplitterColumn sc = (GridGroupSplitterColumn)e.Column;
            sc.HeaderStyle.Width = Unit.Pixel(1);
            sc.Resizable = false;
            sc.Display.Equals("none");
        }
    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {                
                ViewState["DtKey"] = ((RadLabel)e.Item.FindControl("lblDtKey")).Text;
                ViewState["VoucherDtKey"] = ((RadLabel)e.Item.FindControl("lblVoucherDtKey")).Text;
                BindAttachment(ViewState["DtKey"].ToString(), ViewState["VoucherDtKey"].ToString());
                BindAttachmentLink(ViewState["DtKey"].ToString());
                BindVoucherAttachmentLink(ViewState["VoucherDtKey"].ToString());
            }
            if (e.CommandName == "Page")
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
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    // Attachment link..
    protected void dlAttachmentLink_RowCommand(object sender, DataListCommandEventArgs e)
    {
        DataList _gridView = (DataList)sender;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        ifMoreInfo.Attributes["src"] = null;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = e.CommandArgument.ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
    }
    private void BindAttachmentLink(string dtkey)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(
            new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        dlAttachmentLink.DataSource = ds;
        dlAttachmentLink.DataBind();
    }
    private void BindVoucherAttachmentLink(string dtkey)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(
            new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        dlVoucherAttachmentLink.DataSource = ds;
        dlVoucherAttachmentLink.DataBind();
    }
    protected void dlVoucherAttachmentLink_RowCommand(object sender, DataListCommandEventArgs e)
    {
        DataList _gridView = (DataList)sender;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        ifMoreInfo.Attributes["src"] = null;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = e.CommandArgument.ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
    }
}

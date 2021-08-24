using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsVoucherDuplicateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVoucherDuplicateList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucher')", "Print Grid", "icon_print.png", "PRINT");

            MenuForm.AccessRights = this.ViewState;
            MenuForm.MenuList = toolbargrid.Show();
            MenuForm.SetTrigger(pnlForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK");
            MenuFormLink.AccessRights = this.ViewState;
            MenuFormLink.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["INVOICECODE"] = "";
                ViewState["CALLFROM"] = "";

                if (Request.QueryString["qinvoicecode"] != null)
                    ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"].ToString();

                if(Request.QueryString["qcallfrom"] != null)
                    ViewState["CALLFROM"] = Request.QueryString["qcallfrom"].ToString();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFormLink_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Accounts/AccountsInvoiceLineItemDetails.aspx?qinvoicecode=" + ViewState["INVOICECODE"].ToString() + "&qcallfrom=" + ViewState["CALLFROM"].ToString());
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
        DataSet ds = new DataSet();

        string[] alCaptions = { 
                                "Voucher Number",
                                "Invoice Reference",
                                "Invoice Type",
                                "ERM Register Number",
                                "Voucher Date"                               
                              };

        string[] alColumns = {  "FLDVOUCHERNUMBER", 
                                "FLDREFERENCEDOCUMENTNO", 
                                "FLDINVOICETYPENAME", 
                                "FLDINVOICENUMBER",
                                "FLDVOUCHERDATE"
                             };

        ds = PhoenixAccountsVoucher.VoucherDuplicateList(new Guid(ViewState["INVOICECODE"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoice.xls");

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
    protected void MenuForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
        DataSet ds = new DataSet();
        ds = PhoenixAccountsVoucher.VoucherDuplicateList(new Guid(ViewState["INVOICECODE"].ToString()));
        string[] alCaptions = { 
                                "Voucher Number",
                                "Invoice Reference",
                                "Invoice Type",
                                "ERM Register Number",
                                "Voucher Date"                               
                              };

        string[] alColumns = {  "FLDVOUCHERNUMBER", 
                                "FLDREFERENCEDOCUMENTNO", 
                                "FLDINVOICETYPENAME", 
                                "FLDINVOICENUMBER",
                                "FLDVOUCHERDATE"
                             };

        General.SetPrintOptions("gvVoucher", "Accounts Voucher", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVoucher.DataSource = ds;
            gvVoucher.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVoucher);
        }
    }
    protected void gvVoucher_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
}

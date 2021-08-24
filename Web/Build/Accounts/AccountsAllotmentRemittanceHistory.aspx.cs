using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAllotmentRemittanceHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsAllotmentRemittanceHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
           // MenuOrderForm.SetTrigger(pnlOrderForm);
            if (!IsPostBack)
            {
                ViewState["RemittanceNumber"] = "";
                if (Request.QueryString["REMITTANCEID"] != null)
                    ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
                else
                    ViewState["REMITTANCEID"] = Guid.Empty;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                rblHistoryType.SelectedIndex = 0;  
            }
            BindRemittanceNumber();
            toolbargrid = new PhoenixToolbar();
            frmTitle.AccessRights = this.ViewState;
            frmTitle.MenuList = toolbargrid.Show();
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
        int iRowCount = 0;
        //int iTotalPageCount = 0;
        string type = "";

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsAllotmentRemittance.RemittanceHistoryList(new Guid(ViewState["REMITTANCEID"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsRemittanceHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + frmTitle.Title + "</h3></td>");
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
                BindData();
            }
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
    private void BindRemittanceNumber()
    {

        frmTitle.Title = frmTitle.Title + " - " + PhoenixAccountsVoucher.VoucherNumber + "";

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string type = "";
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        type = rblHistoryType.SelectedItem.Value;
        ds = PhoenixAccountsAllotmentRemittance.RemittanceHistoryList(new Guid(ViewState["REMITTANCEID"].ToString())
                                                     , type);
        General.SetPrintOptions("gvInvoiceHistory", "Accounts Invoice History" + "-" + ViewState["Invoicenumber"], alCaptions, alColumns, ds);

        gvInvoiceHistory.DataSource = ds;
        gvInvoiceHistory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        gvInvoiceHistory.Rebind();
    }

    protected void gvInvoiceHistory_RowDeleting(object sender, GridCommandEventArgs de)
    {
        gvInvoiceHistory.Rebind();
    }
    protected void gvInvoiceHistory_ItemDataBound(Object sender, GridItemEventArgs e)
    {
    }

    protected void gvInvoiceHistory_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvInvoiceHistory.Rebind();   
    }
    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Accounts_AccountsFinancialYearLockUnLockHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsFinancialYearLockUnLockHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvFinancialYearHistory')", "Print Grid", "icon_print.png", "PRINT");


            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                ViewState["YEAR"] = "";
                if ((Request.QueryString["COMPANYID"] != null) && (Request.QueryString["COMPANYID"] != ""))
                {
                    ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();
                }

                
                if ((Request.QueryString["YEAR"] != null) && (Request.QueryString["YEAR"] != ""))
                {
                    ViewState["YEAR"] = Request.QueryString["YEAR"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
            }

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
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

        string[] alCaptions = { "Date/Time of Change", "Type of Change", "User Name", "Field", "Old Value", "New Value", "Procedure Used" };

        string[] alColumns = { "FLDUPDATEDATE", "FLDTYPENAME", "FLDUSERNAME", "FLDFIELD", "FLDPREVIOUSVALUETEXT", "FLDCURRENTVALUETEXT", "FLDPROCEDURENAME" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsVoucherNumberSetup.FinancialYearLockUnLockHistoryList(int.Parse(ViewState["COMPANYID"].ToString())
                                                     , int.Parse(ViewState["YEAR"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=FinancialYearLockUnLockHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + frmTitle.Text + "</h3></td>");
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

    //private void BindRemittanceNumber()
    //{
    //    frmTitle.Text = frmTitle.Text + " - " + PhoenixAccountsVoucher.VoucherNumber + "";
    //}

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alCaptions = { "Date/Time of Change","Type of Change","User Name","Field","Old Value","New Value","Procedure Used"};

        string[] alColumns = {  "FLDUPDATEDATE","FLDTYPENAME","FLDUSERNAME","FLDFIELD", "FLDPREVIOUSVALUETEXT", "FLDCURRENTVALUETEXT", "FLDPROCEDURENAME"};

       //type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsVoucherNumberSetup.FinancialYearLockUnLockHistoryList(int.Parse(ViewState["COMPANYID"].ToString())
                                                     , int.Parse(ViewState["YEAR"].ToString()));

        General.SetPrintOptions("gvFinancialYearHistory", "FinancialYear History", alCaptions, alColumns, ds);

        gvFinancialYearHistory.DataSource = ds;
        gvFinancialYearHistory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            // gvInvoiceHistory.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvFinancialYearHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

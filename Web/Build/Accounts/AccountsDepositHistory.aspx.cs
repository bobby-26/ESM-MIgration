using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Accounts_AccountsDepositHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsDepositHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDepositHistory')", "Print Grid", "icon_print.png", "PRINT");
            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //MenuOrderForm.SetTrigger(pnlOrderForm);
            if (!IsPostBack)
            {
                ViewState["Depositnumber"] = "";
                if (Request.QueryString["depositid"] != null)
                    ViewState["depositcode"] = Request.QueryString["depositid"].ToString();
                else
                    ViewState["depositcode"] = Guid.Empty;

                if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"].ToString() != string.Empty)
                    ViewState["qfrom"] = Request.QueryString["qfrom"];
                else
                    ViewState["qfrom"] = "";
                SetRedirectURL();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                BindDeposit(ViewState["depositcode"].ToString());
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbarmain.AddButton("Deposit", "DEPOSIT", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;

            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SetTrigger(pnlOrderForm);

            if (ViewState["qfrom"] != null && ViewState["qfrom"].ToString() == "deposit")
                MenuOrderFormMain.SelectedMenuIndex = 0;
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;


            //BindData();
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
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DEPOSIT"))
            {
                Response.Redirect(ViewState["URL"].ToString() + ViewState["depositcode"] + "&qcallfrom=deposit");
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 0;
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

        ds = PhoenixAccountsDeposit.DepositHistoryList(new Guid(ViewState["depositcode"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsDepositHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Deposit History" + "-" + ViewState["Depositnumber"] + "</h3></td>");
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
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;                
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

    private void SetRedirectURL()
    {
        if (ViewState["qfrom"] != null)
        {
            if (ViewState["qfrom"].ToString() == "deposit")
            {
                ViewState["URL"] = "../Accounts/AccountsDepositList.aspx?depositid=";
            }
        }
    }

    private void BindDeposit(string depositid)
    {
        if (General.GetNullableGuid(depositid) != null)
        {
            DataSet dsInvoice = PhoenixAccountsDeposit.DepositEdit(new Guid(depositid));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                ViewState["Depositnumber"] = dsInvoice.Tables[0].Rows[0]["FLDDEPOSITNUMBER"];
                MenuOrderFormMain.Title = MenuOrderFormMain.Title + " - " + dsInvoice.Tables[0].Rows[0]["FLDDEPOSITNUMBER"].ToString();
            }
        }
    }

    protected void gvDepositHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
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
        ds = PhoenixAccountsDeposit.DepositHistoryList(new Guid(ViewState["depositcode"].ToString())
                                                     , type);
        General.SetPrintOptions("gvDepositHistory", "Accounts Deposit History" + "-" + ViewState["Depositnumber"], alCaptions, alColumns, ds);
        gvDepositHistory.DataSource = ds;            
        gvDepositHistory.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void ReBindData(object sender, EventArgs e)
    {
        gvDepositHistory.Rebind();
    }
}

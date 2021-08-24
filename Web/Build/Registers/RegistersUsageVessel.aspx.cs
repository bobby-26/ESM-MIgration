using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersUsageVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersUsageVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBudget')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Budget Filter','Registers/RegistersSubAccountFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbargrid.AddFontAwesomeButton("../Registers/RegistersUsageVessel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuRegistersBudget.AccessRights = this.ViewState;
        MenuRegistersBudget.MenuList = toolbargrid.Show();

        EditAccount();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BUDGETID"] = null;
            gdBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gdBudget.Rebind();
    }
    protected void gdBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    private void EditAccount()
    {
        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];

        txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
        txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
        txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
        ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
    }

    //private void Reset()
    //{
    //    ViewState["BUDGETID"] = null;
    //    txtSubAccount.Text = "";
    //    txtDescription.Text = "";
    //    chkBudgetedExpense.Checked = false;
    //    ucHard.SelectedHard = "";
    //    rblAccountType.SelectedIndex = -1;
    //}

    protected void RegistersBudgetMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gdBudget.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;
            Filter.CurrentBudgetFilterSelection = null;
            gdBudget.DataSource = null;
            gdBudget.Rebind();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCOUNTTYPENAME", "FLDBUDGETGROUPNAME", "FLDBUDGETEDEXPENSENAME" };
        string[] alCaptions = { "Sub Account", "Description", "Account Type", "Budget Group", "Budgeted Expense" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                          General.GetNullableInteger(ViewState["USAGE"].ToString())
                          , ""
                          , ""
                           , null
                           , null   // PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , null
                        , sortexpression, sortdirection
                        , gdBudget.CurrentPageIndex + 1
                        , gdBudget.PageSize
                        , ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=SubAccount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Account</h3></td>");
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        int? accountid = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDACCOUNTTYPENAME", "FLDBUDGETGROUPNAME", "FLDBUDGETEDEXPENSENAME" };
        string[] alCaptions = { "Sub Account", "Description", "Account Type", "Budget Group", "Budgeted Expense" };

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["USAGE"].ToString() == "79")
        {
            accountid = General.GetNullableInteger(Session["ACCOUNTID"].ToString());
        }

        NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                           General.GetNullableInteger(ViewState["USAGE"].ToString())
                           , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : string.Empty
                           , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : string.Empty
                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
                           , null   // PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , accountid
                           , sortexpression, sortdirection
                           , gdBudget.CurrentPageIndex + 1
                           , gdBudget.PageSize
                           , ref iRowCount
                           , ref iTotalPageCount);

        General.SetPrintOptions("gvBudget", "Sub Account", alCaptions, alColumns, ds);

        gdBudget.DataSource = ds;
        gdBudget.VirtualItemCount = iRowCount;
    }
    protected void gdBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        
    }


    protected void gdBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
 
}

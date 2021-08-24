using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class AccountsUsageCustomer : PhoenixBasePage
{
    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in gdBudget.Rows)
    //     {
    //         if (r.RowType == DataControlRowType.DataRow)
    //         {
    //             Page.ClientScript.RegisterForEventValidation
    //                     (r.UniqueID + "$ctl00");
    //             Page.ClientScript.RegisterForEventValidation
    //                     (r.UniqueID + "$ctl01");
    //         }
    //     }
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsUsageCustomer.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvBudget')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsSubAccountFilter.aspx')", "Find", "search.png", "FIND");

        MenuRegistersBudget.AccessRights = this.ViewState;
        MenuRegistersBudget.MenuList = toolbargrid.Show();
        //MenuRegistersBudget.SetTrigger(pnlBudget);

        EditAccount();

        if (!IsPostBack)
        {
            EnableControls();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BUDGETID"] = null;
            gdBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //BindData();
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

    private void EnableControls()
    {
        txtSubAccount.ReadOnly = true;
        txtSubAccount.CssClass = "readonlytextbox";
        txtDescription.CssClass = "readonlytextbox";
        txtDescription.ReadOnly = true;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void Reset()
    {
        ViewState["BUDGETID"] = null;
        txtSubAccount.Text = "";
        txtDescription.Text = "";
    }

    protected void RegistersBudgetMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION" };
        string[] alCaptions = { "Budget Code", "Description" };
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
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , null
                        , sortexpression, sortdirection
                        , (int)ViewState["PAGENUMBER"], PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
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

    public bool IsValidBudget()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSubAccount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Budget code is required.";

        if (txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        int? accountid = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION" };
        string[] alCaptions = { "Budget Code", "Description" };

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["USAGE"].ToString() == "79" || ViewState["USAGE"].ToString() == "335")
        {
            accountid = General.GetNullableInteger(Session["ACCOUNTID"].ToString());
        }

        NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                           General.GetNullableInteger(ViewState["USAGE"].ToString())
                           , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : string.Empty
                           , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : string.Empty
                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , accountid
                           , sortexpression, sortdirection
                           , (int)ViewState["PAGENUMBER"], gdBudget.PageSize
                           , ref iRowCount
                           , ref iTotalPageCount);

        General.SetPrintOptions("gvBudget", "Budget Account", alCaptions, alColumns, ds);


        gdBudget.DataSource = ds;
        gdBudget.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void BudgetEdit()
    {
        if (ViewState["BUDGETID"] != null)
        {
            DataSet dsBudget = PhoenixRegistersBudget.EditSubAccountMap
                (int.Parse(ViewState["USAGE"].ToString()), Convert.ToInt32(ViewState["BUDGETID"].ToString()));

            if (dsBudget.Tables.Count > 0)
            {
                DataRow drBudget = dsBudget.Tables[0].Rows[0];
                txtSubAccount.Text = drBudget["FLDSUBACCOUNT"].ToString();
                txtDescription.Text = drBudget["FLDDESCRIPTION"].ToString();
                lblSubAccountMapId.Text = drBudget["FLDSUBACCOUNTMAPID"].ToString();
                Rebind();
            }
        }
    }



    protected void gdBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            ViewState["BUDGETID"] = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBudgetid")).Text);
            BudgetEdit();
        }
    }

    protected void gdBudget_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gdBudget_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
    }
    protected void Rebind()
    {
        gdBudget.SelectedIndexes.Clear();
        gdBudget.EditIndexes.Clear();
        gdBudget.DataSource = null;
        gdBudget.Rebind();
    }
    protected void gdBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gdBudget.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

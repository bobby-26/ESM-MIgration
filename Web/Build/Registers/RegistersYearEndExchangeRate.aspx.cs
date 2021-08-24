using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class RegistersYearEndExchangeRate : PhoenixBasePage
{
    public int iUserCode;
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in dgExchangerate.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(dgExchangerate.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Registers/RegistersYearEndExchangeRate.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgExchangerate')", "Print Grid", "icon_print.png", "PRINT");
        MenuExchangeRate.AccessRights = this.ViewState;
        MenuExchangeRate.MenuList = toolbar.Show();
       // MenuExchangeRate.SetTrigger(pnlExchangeRate);

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Year End Exchange Rates", "EXCHANGERATES", ToolBarDirection.Right);
        toolbar1.AddButton("Period Lock", "PERIODLOCK", ToolBarDirection.Right);
        toolbar1.AddButton("Financial Years", "FINANCIALYEAR", ToolBarDirection.Right);
        MenuMain.Title = "Exchange Rates";
        MenuMain.AccessRights = this.ViewState;
        MenuMain.MenuList = toolbar1.Show();
        MenuMain.SelectedMenuIndex = 0;


        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
           
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            dgExchangerate.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
       // BindData();
    
    }
    protected void Rebind()
    {
        dgExchangerate.SelectedIndexes.Clear();
        dgExchangerate.EditIndexes.Clear();
        dgExchangerate.DataSource = null;
        dgExchangerate.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOMPANYNAME", "FLDCURRENCYCODE", "FLDBASEEXCHANGERATE", "FLDREPORTEXCHANGERATE" };
        string[] alCaptions = { "Company Name", "Currency", "Base Exchange Rate", "Report Exchange Rate" };

        
        ds = PhoenixRegistersYearEndExchangerate.YearEndExchangeRateSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ddlYear.SelectedValue)    
            );


        Response.AddHeader("Content-Disposition", "attachment; filename=ExchangeRate.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Exchange Rate History</h3></td>");
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

    protected void ExchangeRate_TabStripCommand(object sender, EventArgs e)
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

    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("PERIODLOCK"))
        {
            Response.Redirect("../Accounts/AccountsPeriodLock.aspx");
        }
        if (CommandName.Equals("FINANCIALYEAR"))
        {
            Response.Redirect("../Accounts/AccountsFinancialYearSetup.aspx");
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPANYNAME", "FLDCURRENCYCODE", "FLDBASEEXCHANGERATE", "FLDREPORTEXCHANGERATE" };
        string[] alCaptions = { "Company Name", "Currency", "Base Exchange Rate", "Report Exchange Rate" };

        DataSet ds = PhoenixRegistersYearEndExchangerate.YearEndExchangeRateSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            dgExchangerate.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ddlYear.SelectedValue)
            );

        General.SetPrintOptions("dgExchangerate", "Exchange Rate", alCaptions, alColumns, ds);

        dgExchangerate.DataSource = ds;
        dgExchangerate.VirtualItemCount = iRowCount;

      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }



    //protected void dgExchangerate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}

    protected void dgExchangerate_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }


            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidExchangeRate(((UserControlCurrency)e.Item.FindControl("ddlCurrencyCode")).SelectedCurrency, ((UserControlNumber)e.Item.FindControl("txtBaseExchangerateAdd")).Text
                                             , ((UserControlNumber)e.Item.FindControl("txtReportExchangerateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersYearEndExchangerate.YearEndExchangeRateInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                , int.Parse(((UserControlCurrency)e.Item.FindControl("ddlCurrencyCode")).SelectedCurrency)
                                                                                , General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtBaseExchangerateAdd")).Text)
                                                                                , General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtReportExchangerateAdd")).Text)
                                                                                , int.Parse(ddlYear.SelectedValue)
                                                                                );

                Rebind();
                ((UserControlCurrency)e.Item.FindControl("ddlCurrencyCode")).DataBind();

            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidExchangeRate(((RadLabel)e.Item.FindControl("lblCurrencyId")).Text, ((UserControlNumber)e.Item.FindControl("txtBaseExchangerateEdit")).Text
                                          , ((UserControlNumber)e.Item.FindControl("txtReportExchangerateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersYearEndExchangerate.YearEndExchangeRateInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                                , int.Parse(((RadLabel)e.Item.FindControl("lblCurrencyId")).Text)
                                                                                , General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtBaseExchangerateEdit")).Text)
                                                                                , General.GetNullableDecimal(((UserControlNumber)e.Item.FindControl("txtReportExchangerateEdit")).Text)
                                                                                , int.Parse(ddlYear.SelectedValue)
                                                                                );


                Rebind();

            }
           

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    //protected void dgExchangerate_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToUpper().Equals("ADD"))
    //    {
    //        if (!IsValidExchangeRate(((UserControlCurrency)_gridView.FooterRow.FindControl("ddlCurrencyCode")).SelectedCurrency, ((TextBox)_gridView.FooterRow.FindControl("txtBaseExchangerateAdd")).Text
    //                                        , ((TextBox)_gridView.FooterRow.FindControl("txtReportExchangerateAdd")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixRegistersYearEndExchangerate.YearEndExchangeRateInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
    //                                                                        , int.Parse(((UserControlCurrency)_gridView.FooterRow.FindControl("ddlCurrencyCode")).SelectedCurrency)
    //                                                                        , General.GetNullableDecimal(((TextBox)_gridView.FooterRow.FindControl("txtBaseExchangerateAdd")).Text)
    //                                                                        , General.GetNullableDecimal(((TextBox)_gridView.FooterRow.FindControl("txtReportExchangerateAdd")).Text)
    //                                                                        , int.Parse(ddlYear.SelectedValue)
    //                                                                        );

    //        BindData();
    //        ((UserControlCurrency)_gridView.FooterRow.FindControl("ddlCurrencyCode")).DataBind();
    //    }

    //    else if (e.CommandName.ToUpper().Equals("SAVE"))
    //    {
    //        if (!IsValidExchangeRate(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCurrencyId")).Text, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBaseExchangerateEdit")).Text
    //                                       , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReportExchangerateEdit")).Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixRegistersYearEndExchangerate.YearEndExchangeRateInsert(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
    //                                                                        , int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCurrencyId")).Text)
    //                                                                        , General.GetNullableDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBaseExchangerateEdit")).Text)
    //                                                                        , General.GetNullableDecimal(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtReportExchangerateEdit")).Text)
    //                                                                        ,int.Parse(ddlYear.SelectedValue)
    //                                                                        );

    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    SetPageNavigator();
    //}

    protected void dgExchangerate_SortCommand(object sender, GridSortCommandEventArgs e)
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



    //protected void dgExchangerate_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;
    //        BindData();
    //        SetPageNavigator();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void dgExchangerate_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgExchangerate, "Edit$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}

    //protected void dgExchangerate_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (ViewState["SORTEXPRESSION"] != null)
    //    {
    //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //        if (img != null)
    //        {
    //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                img.Src = Session["images"] + "/arrowUp.png";
    //            else
    //                img.Src = Session["images"] + "/arrowDown.png";

    //            img.Visible = true;
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        // Get the LinkButton control in the first cell
    //        LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
    //        // Get the javascript which is assigned to this LinkButton
    //        string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
    //        // Add this javascript to the onclick Attribute of the row
    //        e.Row.Attributes["ondblclick"] = _jsDouble;

    //        ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
    //    }

    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {

    //    }
    //}
    protected void dgExchangerate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgExchangerate.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgExchangerate_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
            if (e.Item is GridDataItem)
            {
            }
     

        }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        
    }

   
 
   
    private bool IsValidExchangeRate(string strCurrencycode, string strBaseExchangeRate,string strReportExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlYear.SelectedValue) == null)
            ucError.ErrorMessage = "Financial Year is required.";

        if (strCurrencycode.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Currency is required.";

        if (General.GetNullableDecimal(strBaseExchangeRate)==null)
            ucError.ErrorMessage = "Base Exchange Rate is required.";

        if (General.GetNullableDecimal(strReportExchangeRate) == null)
            ucError.ErrorMessage = "Report Exchange Rate is required.";
        return (!ucError.IsError);
    }


}

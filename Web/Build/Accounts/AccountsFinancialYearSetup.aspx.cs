using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.IO;
using Telerik.Web.UI;

public partial class AccountsFinancialYearSetup : PhoenixBasePage
{
    public int iUserCode;
    public string strCompanynamedisplay = string.Empty;

    //  protected override void Render(HtmlTextWriter writer)
    //  {
    //      foreach (GridViewRow r in dgFinancialYearSetup.Rows)
    //      {
    //          if (r.RowType == DataControlRowType.DataRow)
    //          {
    //              Page.ClientScript.RegisterForEventValidation(dgFinancialYearSetup.UniqueID, "Edit$" + r.RowIndex.ToString());
    //          }
    //      }
    //      base.Render(writer);
    //  }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Year End Exchange Rates", "EXCHANGERATES", ToolBarDirection.Right);
        toolbar1.AddButton("Period Lock", "PERIODLOCK", ToolBarDirection.Right);
        toolbar1.AddButton("Financial Years", "FINANCIALYEAR", ToolBarDirection.Right);
        MenuPeriodLock.AccessRights = this.ViewState;
        MenuPeriodLock.MenuList = toolbar1.Show();


        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsFinancialYearSetup.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('dgFinancialYearSetup')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageButton("../Accounts/AccountsFinancialYearSetup.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Accounts/AccountsFinancialYearSetup.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        //MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        MenuPeriodLock.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["ISFIRSTYEAR"] = "YES";
            ViewState["EXCELEXPORTYN"] = "N";
            dgFinancialYearSetup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        //BindData();
        strCompanynamedisplay = PhoenixSecurityContext.CurrentSecurityContext.CompanyName;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iCompanycode = null;
        int? iFinancialYear = null;
        int? sortdirection = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOMPANYNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR", "FLDYEAR", "FLDSTATUSNAME" };
        string[] alCaptions = { "Company Name", "Financial Start Year", "Financial End Year", "Financial Year", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (PhoenixSecurityContext.CurrentSecurityContext.CompanyID != 0)
        {
            iCompanycode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        }
        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        ds = PhoenixAccountsVoucherNumberSetup.CompanyFinancialYearList(iCompanycode, iFinancialYear, sortexpression, sortdirection,
        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
        PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
        ref iRowCount,
        ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FinanacialYear.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Financial Year</h3></td>");
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

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("PERIODLOCK"))
        {
            Response.Redirect("../Accounts/AccountsPeriodLock.aspx");
        }
        if (CommandName.ToUpper().Equals("EXCHANGERATES"))
        {
            Response.Redirect("../Registers/RegistersYearEndExchangeRate.aspx");
        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtFinancialYear.Text = "";
            Rebind();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? iCompanycode = null;
        int? iFinancialYear = null;

        string[] alColumns = { "FLDCOMPANYNAME", "FLDFINANCIALSTARTYEAR", "FLDFINANCIALENDYEAR", "FLDYEAR", "FLDSTATUSNAME" };
        string[] alCaptions = { "Company Name", "Financial Start Year", "Financial End Year", "Financial Year", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (txtFinancialYear.Text.ToString() != string.Empty)
            iFinancialYear = int.Parse(txtFinancialYear.Text.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.CompanyID != 0)
        {
            iCompanycode = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        }

        if (iCompanycode == null)
            Response.Redirect("../Options/OptionsChooseCompany.aspx");

        DataSet ds = PhoenixAccountsVoucherNumberSetup.CompanyFinancialYearList(iCompanycode, iFinancialYear, sortexpression, sortdirection,
           dgFinancialYearSetup.CurrentPageIndex + 1,
            dgFinancialYearSetup.PageSize,
            ref iRowCount,
            ref iTotalPageCount);


        ViewState["ISFIRSTYEAR"] = "NO";
        dgFinancialYearSetup.DataSource = ds;
        dgFinancialYearSetup.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("dgFinancialYearSetup", "Financial Year Setup", alCaptions, alColumns, ds);
    }


    protected void dgFinancialYearSetup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                InsertFinancialYearSetup(
                    ((UserControlDate)e.Item.FindControl("txtFinancialStartYearAdd")).Text,
                    General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtFinancialYear")).Text),
                    (((CheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked) ? 1 : 0
                );
                dgFinancialYearSetup.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateFinancialYearSetup(((RadTextBox)e.Item.FindControl("txtMapCode")).Text
                                            , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtFinancialYearEdit")).Text)
                                            , (((CheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked) ? 1 : 0
                                            , ((UserControlDate)e.Item.FindControl("txtFinancialStartYearEdit")).Text
                                            , ((UserControlDate)e.Item.FindControl("txtFinancialEndYearEdit")).Text);
                dgFinancialYearSetup.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                int iRowno;
                iRowno = e.Item.ItemIndex;
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletedFinancialYearSetup(((RadTextBox)e.Item.FindControl("txtMapCode")).Text);
                dgFinancialYearSetup.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("POSTACCURALS"))
            {
                Guid? dtkey = Guid.Empty;
                PhoenixAccountsVoucherNumberSetup.PostInvoiceAccurals(int.Parse(((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text), ref dtkey);
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "ACCOUNTS/INVOICEACCURALS";
                //string path = Server.MapPath("~/Attachments/ACCOUNTS/INVOICEACCURALS").ToString();

                //if (Directory.Exists(path))
                //    Directory.Delete(path, true);
                if (General.GetNullableGuid(dtkey.ToString()) != null)
                {
                    PhoenixAccounts2XL.Export2XLInvoiceAccuralsSave(int.Parse(((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text), path, dtkey);
                    PhoenixAccountsVoucherNumberSetup.InvoiceAccuralspostAttachment(dtkey);
                }

                ucSatus.Text = "Invoice Accurals Posted Successfully.";
                dgFinancialYearSetup.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("FOREXREVALUATION"))
            {

                PhoenixAccountsVoucherNumberSetup.PostForexRevaluation(int.Parse(((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text));
                ucSatus.Text = "Forex Revaluation Posted Successfully.";
                dgFinancialYearSetup.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("OPENINGBALANCE"))
            {

                PhoenixAccountsVoucherNumberSetup.PostOpeningBalance(int.Parse(((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text));
                ucSatus.Text = "Opening Balances Posted Successfully.";
                dgFinancialYearSetup.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UNDOPOSTING"))
            {

                PhoenixAccountsVoucherNumberSetup.UndoAllPostings(int.Parse(((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text));
                ucSatus.Text = "All Postings Reversed Successfully.";
                // _gridView.SelectedIndex = Int32.Parse(e.CommandArgument.ToString());
                dgFinancialYearSetup.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("EXCELEXPORT"))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportInvoiceAccurals('" + ((RadLabel)e.Item.FindControl("lnkFinancialYear")).Text + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "');", true);
                //_gridView.SelectedIndex = Int32.Parse(e.CommandArgument.ToString());
                dgFinancialYearSetup.Rebind();

            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgFinancialYearSetup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgFinancialYearSetup.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dgFinancialYearSetup_Sorting(object sender, GridSortCommandEventArgs e)
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

    protected void dgFinancialYearSetup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void Rebind()
    {
        dgFinancialYearSetup.SelectedIndexes.Clear();
        dgFinancialYearSetup.EditIndexes.Clear();
        dgFinancialYearSetup.DataSource = null;
        dgFinancialYearSetup.Rebind();
    }
    protected void dgFinancialYearSetup_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
        }


        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton cmdPOApprove = (ImageButton)e.Item.FindControl("cmdPOApprove");
            if (cmdPOApprove != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdPOApprove.CommandName)) cmdPOApprove.Visible = false;

            ImageButton imgAttachment = (ImageButton)e.Item.FindControl("imgAttachment");
            if (imgAttachment != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgAttachment.CommandName)) imgAttachment.Visible = false;
        }

        if (e.Item is GridDataItem)
        {
            RadLabel lblIsRecentFinancialYear = (RadLabel)e.Item.FindControl("lblIsRecentFinancialYear");

            if (lblIsRecentFinancialYear.Text == "1")
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Visible = true;
            }

            ImageButton imgAccountsTransferred = (ImageButton)e.Item.FindControl("cmdAccountsTransferred");
            if (imgAccountsTransferred != null)
            {
                imgAccountsTransferred.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','Accounts/AccountsYearEndAccountsMap.aspx?finyear=" + drv["FLDYEAR"].ToString() + "'); return false;");
            }

            ImageButton cmdExcelExport = (ImageButton)e.Item.FindControl("cmdExcelExport");
            //if (cmdExcelExport != null)
            //    cmdExcelExport.Attributes.Add("onclick", "javascript:Openpopup('Filter','','../Accounts/AccountsExport2XL.aspx?finyear=" + drv["FLDYEAR"].ToString() + "'); return false;");

            ImageButton cmdPostAccurals = (ImageButton)e.Item.FindControl("cmdPostAccurals");
            ImageButton cmdForexRevaluation = (ImageButton)e.Item.FindControl("cmdForexRevaluation");
            ImageButton cmdOpeningBalance = (ImageButton)e.Item.FindControl("cmdOpeningBalance");

            if (cmdPostAccurals != null)
                cmdPostAccurals.Visible = (General.GetNullableInteger(drv["FLDYEARENDSTATUS"].ToString()) == 1) ? true : false;

            if (cmdForexRevaluation != null)
                cmdForexRevaluation.Visible = (General.GetNullableInteger(drv["FLDYEARENDSTATUS"].ToString()) == 2) ? true : false;


            if (cmdOpeningBalance != null)
                cmdOpeningBalance.Visible = (General.GetNullableInteger(drv["FLDYEARENDSTATUS"].ToString()) == 3) ? true : false;

            ImageButton cmdHistory = (ImageButton)e.Item.FindControl("cmdHistory");
            RadLabel lblCompanyID = (RadLabel)e.Item.FindControl("lblCompanyID");
            RadLabel FinancialYear = (RadLabel)e.Item.FindControl("lnkFinancialYear");
            if (cmdHistory != null)
            {
                cmdHistory.Attributes.Add("onclick", "javascript:openNewWindow('codehelp','','Accounts/AccountsFinancialYearLockUnLockHistory.aspx?COMPANYID=" + lblCompanyID.Text + "&YEAR=" + FinancialYear.Text + "'); return false;");
            }

        }
        if (e.Item is GridDataItem)
        {
            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName)) cmdAdd.Visible = false;

        }
    }

    protected void InsertFinancialYearSetup(string strFinancialStartYear, int? iYear, int iStatus)
    {
        string strCompanyId = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
        if (!IsValidData(strCompanyId, strFinancialStartYear != null ? strFinancialStartYear : string.Empty, iYear))
        {
            ucError.Visible = true;
            return;
        }

        try
        {
            PhoenixAccountsVoucherNumberSetup.FinancialYearMapInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(strCompanyId), General.GetNullableDateTime(strFinancialStartYear), iYear, iStatus);
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void UpdateFinancialYearSetup(string strMapCode, int? iYear, int iStatus, string strFinancialYearStartDate, string strFinancialYearEndDate)
    {
        string strCompanyId = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();

        if (!IsValidDate(strFinancialYearStartDate, strFinancialYearEndDate, iYear))
        {
            ucError.Visible = true;
            return;
        }
        try
        {
            PhoenixAccountsVoucherNumberSetup.FinancialYearMapUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(strMapCode), int.Parse(strCompanyId), iYear, iStatus, DateTime.Parse(strFinancialYearEndDate));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            if (ex.Message == "VOUCHER EXISTS DURING THIS FINANCIAL YEAR, UNABLE TO MODIFY THE END DATE")
            {
                Response.Redirect("../Accounts/AccountsDeviatedEndDateVoucherList.aspx?qMapCode=" + strMapCode + "&qNewEnddate=" + strFinancialYearEndDate);
            }
            return;
        }
    }

    protected void DeletedFinancialYearSetup(string strMapCode)
    {
        try
        {
            PhoenixAccountsVoucherNumberSetup.FinancialYearMapDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(strMapCode));
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidData(string strCompanyId, string strFinancialStartYear, int? iYear)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strCompanyId.Trim().ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Company is required.";

        if (strFinancialStartYear.Trim().Equals("") && ViewState["ISFIRSTYEAR"].ToString() == "YES")
            ucError.ErrorMessage = "Financial start year is required";

        if (iYear == null)
            ucError.ErrorMessage = "Financial year is required.";

        return (!ucError.IsError);
    }


    private bool IsValidDate(string StartDate, string EndDate, int? iYear)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime? dtEndDate = null;
        if (EndDate == null)
            ucError.ErrorMessage = "Financial year end date is required.";
        else
            dtEndDate = DateTime.Parse(EndDate);
        if (dtEndDate < DateTime.Parse(StartDate))
            ucError.ErrorMessage = "Financial year end date should be later than financial year start date.";

        if (iYear == null)
            ucError.ErrorMessage = "Financial year is required.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

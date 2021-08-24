using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegistersVesselBudgetOffshore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            // toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
            toolbar1.AddButton("History", "HISTORY", ToolBarDirection.Right);

            toolbar1.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
            toolbar1.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
            toolbar1.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar1.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
            toolbar1.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
            toolbar1.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
            toolbar1.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
            toolbar1.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
            //toolbar.AddButton("List", "LIST", ToolBarDirection.Right);

            MenuVesselList.AccessRights = this.ViewState;
            MenuVesselList.MenuList = toolbar1.Show();

            MenuVesselList.SelectedMenuIndex = 2;
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERB"] = 1;
                ViewState["SORTEXPRESSIONB"] = null;
                ViewState["SORTDIRECTIONB"] = null;

                ViewState["REVISIONID"] = "";
                ViewState["SELECTEDITEM"] = "";

                gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvBudgetRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselBudgetOffshore.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersBudget.AccessRights = this.ViewState;
            MenuRegistersBudget.MenuList = toolbar.Show();
            //BindData();
            //SetPageNavigator();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SubMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersVesselBudgetOffshore.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ExcelBudget");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudget')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINTBUDGET");
        toolbar.AddFontAwesomeButton("javascript:parent.Openpopup('Register','Add','RegistersVesselBudgetAdd.aspx?revisionid=" + ViewState["REVISIONID"].ToString() + "&Budgetid=" + null + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDBUDFET");
        MenuRegistersVesslBudget.AccessRights = this.ViewState;
        MenuRegistersVesslBudget.MenuList = toolbar.Show();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvBudgetRevision.Rebind();
        BindDataBudget();
        gvBudget.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDCURRENCYCODE", "FLDOVERLAPWAGE", "FLDTANKCLEANALLOWANCE", "FLDDPALLOWANCE", "FLDOTHERALLOWANCE", "FLDREMARKS", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Currency", "Overlap Wage", "Tank Clean Allowance", "DP Allowance", "Other Allowance", "Remarks", "Revision No" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudgetRevision(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        General.ShowExcel("Budget Revision", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDCURRENCYCODE", "FLDOVERLAPWAGE", "FLDTANKCLEANALLOWANCE", "FLDDPALLOWANCE", "FLDOTHERALLOWANCE", "FLDREMARKS", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective Date", "Currency", "Overlap Wage", "Tank Clean Allowance", "DP Allowance", "Other Allowance", "Remarks", "Revision No" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(Int16.Parse(Filter.CurrentVesselMasterFilter));
        if (dsVessel.Tables[0].Rows.Count > 0)
        {
            DataRow drVessel = dsVessel.Tables[0].Rows[0];
            txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();
        }
        DataTable dt = PhoenixRegistersVesselBudget.SearchBudgetRevision(General.GetNullableInteger(Filter.CurrentVesselMasterFilter),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvBudgetRevision.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvBudgetRevision", "Budget Revision", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBudgetRevision.DataSource = ds;

        }
        else
        {
            gvBudgetRevision.DataSource = ds;
            ViewState["REVISIONID"] = "";
        }

        gvBudgetRevision.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;




    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISIONID"] = gvBudgetRevision.MasterTableView.DataKeyValues[rowindex]["FLDREVISIONID"].ToString();
            gvBudgetRevision.Items[rowindex].Selected = true;
            BindDataBudget();
            gvBudget.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsValidData(string effectivedate, string otherallowances, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        if (General.GetNullableDecimal(otherallowances) != null)
        {
            if (General.GetNullableString(remarks) == null)
                ucError.ErrorMessage = "Remarks is required.";
        }

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvBudgetRevision.Rebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    // Budget

    protected void ShowExcelB()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLDPREFERREDNATIONALITYNAME" };
        string[] alCaptions = { "Rank", "Currency", "Budgeted Wage/day", "Preferred Nationality" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONB"] == null) ? null : (ViewState["SORTEXPRESSIONB"].ToString());
        if (ViewState["SORTDIRECTIONB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONB"].ToString());

        if (ViewState["ROWCOUNTB"] == null || Int32.Parse(ViewState["ROWCOUNTB"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTB"].ToString());

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudget(int.Parse(Filter.CurrentVesselMasterFilter),
                            General.GetNullableGuid(ViewState["REVISIONID"].ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBERB"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount);

        General.ShowExcel("Budget", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersVesselBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCELBUDGET"))
            {
                ShowExcelB();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindDataBudget()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRANKNAME", "FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLDPREFERREDNATIONALITYNAME" };
        string[] alCaptions = { "Rank", "Currency", "Budgeted Wage/day", "Preferred Nationality" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONB"] == null) ? null : (ViewState["SORTEXPRESSIONB"].ToString());
        if (ViewState["SORTDIRECTIONB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONB"].ToString());

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudget(int.Parse(Filter.CurrentVesselMasterFilter),
                            General.GetNullableGuid(ViewState["REVISIONID"].ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBERB"].ToString()),
                           gvBudget.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvBudget", "Budget", alCaptions, alColumns, ds);


        gvBudget.DataSource = ds;
        gvBudget.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNTB"] = iRowCount;
        ViewState["TOTALPAGECOUNTB"] = iTotalPageCount;

    }


    private bool IsValidBudget(string rankid, string wage, string currencyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please select the revision to add budget.";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDecimal(wage) == null)
            ucError.ErrorMessage = "Budgeted Wage/day is required.";

        if (General.GetNullableInteger(currencyid) == null)
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }

    protected void gvBudget_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERB"] = ViewState["PAGENUMBERB"] != null ? ViewState["PAGENUMBERB"] : gvBudget.CurrentPageIndex + 1;
            BindDataBudget();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (Filter.CurrentVesselMasterFilter == null)
        {
            if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                if (Session["NEWMODE"] != null && Session["NEWMODE"].ToString() == "1")
                {
                    Session["NEWMODE"] = 0;
                    //Response.Redirect( "../Registers/RegistersVessel.aspx";
                    return;
                }
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselParticulars.aspx");
            }
            else if (CommandName.ToUpper().Equals("OFFICEADMIN"))
            {
                Response.Redirect("../Registers/RegistersVesselOfficeAdmin.aspx");
            }
            else if (CommandName.ToUpper().Equals("COMMUNICATIONDETAILS"))
            {
                Response.Redirect("../Registers/RegistersVesselCommunicationDetails.aspx");
            }
            else if (CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Registers/RegisterVesselCertificate.aspx");
            }
            else if (CommandName.ToUpper().Equals("MANNINGSCALE"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
                    Response.Redirect("../Registers/RegistersOffshoreVesselManningScale.aspx");
                else
                    Response.Redirect("../Registers/RegistersVesselManningScale.aspx");
            }
            else if (CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("../Registers/RegistersVesselBudget.aspx");
            }
            else if (CommandName.ToUpper().Equals("DOCUMENTSREQUIRED"))
            {
                Response.Redirect("../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=VESSEL");
            }
            else if (CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Registers/RegistersVessel.aspx");
            }
            //else if (dce.CommandName.ToUpper().Equals("CORRESPONDENCE"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselCorrespondence.aspx";
            //}
            //else if (dce.CommandName.ToUpper().Equals("CHATBOX"))
            //{
            //    Response.Redirect( "../Registers/RegistersVesselChatBox.aspx?vesselid=" + Filter.CurrentVesselMasterFilter;
            //}
            else if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
            {
                Response.Redirect("../Registers/RegistersVesselFinancialYear.aspx");
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Registers/RegistersVesselHistory.aspx");
            }
            else if (CommandName.ToUpper().Equals("VESSELSEARCH"))
            {
                Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
            }
            else
                Response.Redirect("../Registers/RegistersVesselList.aspx");
        }
    }

    protected void gvBudgetRevision_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetRevision.CurrentPageIndex + 1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudgetRevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text
                                , ((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceAdd")).Text
                                , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselBudget.InsertBudgetRevision(int.Parse(Filter.CurrentVesselMasterFilter),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text)
                    , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucTankCleanAllowanceAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucDPAllowanceAdd")).Text)
                    , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text)
                    );

                BindData();
                gvBudgetRevision.Rebind();
                ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());// _gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixRegistersVesselBudget.DeleteBudgetRevision(revisionid);
                BindData();
                gvBudgetRevision.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECTREVISION"))
            {
                ViewState["SELECTEDITEM"] = e.Item.ItemIndex;
               
                BindPageURL(e.Item.ItemIndex);
            
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                PhoenixRegistersVesselBudget.CopyPreviousRevBudget(revisionid);
                BindData();
                gvBudgetRevision.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(
                   ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                   , ((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceEdit")).Text
                   , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text
                   ))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());

                PhoenixRegistersVesselBudget.UpdateBudgetRevision(revisionid
                    , int.Parse(Filter.CurrentVesselMasterFilter)
                    , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text)
                    , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency)
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageEdit")).Text)
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceEdit")).Text)
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucTankCleanAllowanceEdit")).Text)
                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucDPAllowanceEdit")).Text)
                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text)
                    );


                BindData();
                gvBudgetRevision.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvBudgetRevision_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            LinkButton dc = (LinkButton)e.Item.FindControl("cmdCopy");
            if (dc != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dc.CommandName))
                    dc.Visible = false;

                dc.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to copy budget from previous revision?')");
            }
            UserControlCurrency ucCurrencyEdit = (UserControlCurrency)e.Item.FindControl("ucCurrencyEdit");
            if (ucCurrencyEdit != null)
            {
                ucCurrencyEdit.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                ucCurrencyEdit.DataBind();
                ucCurrencyEdit.SelectedCurrency = dr["FLDCURRENCY"].ToString();
            }

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton ab = (LinkButton)e.Item.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }
            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Item.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                ucCurrencyAdd.DataBind();
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }
        }
    }

    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("DELETEBUDGET"))
            {
                Guid budgetid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDBUDGETID"].ToString());
                PhoenixRegistersVesselBudget.DeleteBudget(budgetid);
                BindDataBudget();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBERB"] = null;
            }



        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBudget_ItemDataBound(object sender, GridItemEventArgs e)
    {


        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {

                string guid = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDBUDGETID"].ToString();
                if (guid != null || guid != "")
                    eb.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Registers/RegistersVesselBudgetAdd.aspx?revisionid=" + ViewState["REVISIONID"].ToString() + "&Budgetid=" + guid + "'); return true;");
            }

        }
    }
    protected void gvBudgetRevision_PreRender(object sender, EventArgs e)
    {
        if ((ViewState["REVISIONID"] == null || ViewState["REVISIONID"].ToString() == "") && gvBudgetRevision.Items.Count != 0)
        {
              

            gvBudgetRevision.Items[0].Selected = true;
            gvBudgetRevision.MasterTableView.Items[0].GetDataKeyValue("FLDREVISIONID").ToString();

            ViewState["REVISIONID"] = gvBudgetRevision.MasterTableView.DataKeyValues[0]["FLDREVISIONID"].ToString();
            BindDataBudget();
            gvBudget.Rebind();
        }
    }
}



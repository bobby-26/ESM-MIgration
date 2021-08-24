using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;

public partial class RegistersVesselBudget : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Contains("OFFSHORE"))
            {
                Response.Redirect("RegistersVesselBudgetOffshore.aspx", true);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //toolbar.AddButton("Vessel Search", "VESSELSEARCH", ToolBarDirection.Right);
            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);

            toolbar.AddButton("Crew Docs", "DOCUMENTSREQUIRED", ToolBarDirection.Right);
            toolbar.AddButton("Budget", "BUDGET", ToolBarDirection.Right);
            toolbar.AddButton("Manning", "MANNINGSCALE", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddButton("Office Admin", "OFFICEADMIN", ToolBarDirection.Right);
            toolbar.AddButton("Admin", "ADMIN", ToolBarDirection.Right);
            //toolbar.AddButton("Certificates", "CERTIFICATES", ToolBarDirection.Right);
            toolbar.AddButton("Commn Equipments", "COMMUNICATIONDETAILS", ToolBarDirection.Right); // Bug Id: 8910
            toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);

            MenuVesselList.AccessRights = this.ViewState;
            MenuVesselList.MenuList = toolbar.Show();
            MenuVesselList.SelectedMenuIndex = 2;

            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGENUMBERB"] = 1;
                ViewState["SORTEXPRESSIONB"] = null;
                ViewState["SORTDIRECTIONB"] = null;
                ViewState["REVISIONID"] = "";
                gvBudgetRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Registers/RegistersVesselBudget.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvBudgetRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersBudget.AccessRights = this.ViewState;
            MenuRegistersBudget.MenuList = toolbar1.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SubMenu()
    {
        PhoenixToolbar toolbar2 = new PhoenixToolbar();
        toolbar2.AddFontAwesomeButton("../Registers/RegistersVesselBudget.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ExcelBudget");
        toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvBudget')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINTBUDGET");
        toolbar2.AddFontAwesomeButton("javascript:openNewWindow('Register','Add','Registers/RegistersVesselBudgetAdd.aspx?revisionid=" + ViewState["REVISIONID"].ToString() + "&Budgetid=" + null + "')", "Add", "<i class=\"fas fa-plus\"></i>", "ADDBUDFET");
        MenuRegistersVesslBudget.AccessRights = this.ViewState;
        MenuRegistersVesslBudget.MenuList = toolbar2.Show();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        gvBudgetRevision.SelectedIndexes.Clear();
        gvBudgetRevision.EditIndexes.Clear();
        gvBudgetRevision.DataSource = null;
        gvBudgetRevision.Rebind();
        SubMenu();
    }
    protected void RebindBudget()
    {
        gvBudget.SelectedIndexes.Clear();
        gvBudget.EditIndexes.Clear();
        gvBudget.DataSource = null;
        gvBudget.Rebind(); SubMenu();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDCURRENCYCODE", "FLDREMARKS", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective From", "Currency", "Remarks", "Revision No" };
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
            UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
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

        string[] alColumns = { "FLDEFFECTIVEDATE", "FLDCURRENCYCODE", "FLDREMARKS", "FLDREVISIONNO" };
        string[] alCaptions = { "Effective From", "Currency", "Remarks", "Revision No" };

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
        DataTable dt = PhoenixRegistersVesselBudget.SearchBudgetRevision(General.GetNullableInteger(Filter.CurrentVesselMasterFilter), sortexpression,
            sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()), gvBudgetRevision.PageSize, ref iRowCount, ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvBudgetRevision", "Budget Revision", alCaptions, alColumns, ds);
        gvBudgetRevision.DataSource = ds;
        gvBudgetRevision.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["REVISIONID"] == null || ViewState["REVISIONID"].ToString() == "")
            {
                ViewState["REVISIONID"] = ds.Tables[0].Rows[0]["FLDREVISIONID"].ToString();
                lblBudget.Text = "Budget Revision No. " + ds.Tables[0].Rows[0]["FLDREVISIONNO"].ToString() + " Effective From " + General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDEFFECTIVEDATE"].ToString());
            }
            //   RebindBudget();
        }
        else
            ViewState["REVISIONID"] = "";
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        RebindBudget();
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
                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersVesselBudget.InsertBudgetRevision(int.Parse(Filter.CurrentVesselMasterFilter),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text)
                    , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency)
                    , null, null, null, null, General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text));
                Rebind();
                RebindBudget();
            }
            else if (e.CommandName.ToUpper().Equals("SELECTREVISION"))
            {
                ViewState["REVISIONID"] = ((RadLabel)e.Item.FindControl("lblrevisionid")).Text;
                string Revisionno= ((RadLabel)e.Item.FindControl("lblRevNo")).Text;
                string Effectiveon = ((LinkButton)e.Item.FindControl("lnkEffectiveDate")).Text;

                lblBudget.Text = "Budget Revision No. " + Revisionno + " Effective From " + Effectiveon;
                RebindBudget();
            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(((RadLabel)e.Item.FindControl("lblrevisionid")).Text);
                PhoenixRegistersVesselBudget.CopyPreviousRevBudget(revisionid);
                Rebind();
                RebindBudget();
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string Revisionno = ((RadLabel)e.Item.FindControl("lblRevNo")).Text;
                string Effectiveon =((RadLabel)e.Item.FindControl("lbleffectivefrom")).Text; 
                lblBudget.Text = "Budget Revision No. " + Revisionno + " Effective From " + Effectiveon;
                Rebind();
                RebindBudget();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
               
                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid revisionid = new Guid(((RadLabel)e.Item.FindControl("lblrevisionid")).Text);
                ViewState["REVISIONID"] = revisionid.ToString();
                PhoenixRegistersVesselBudget.UpdateBudgetRevision(revisionid, int.Parse(Filter.CurrentVesselMasterFilter)
                    , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text)
                    , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency)
                    , null, null, null, null, General.GetNullableString(((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text));
                string Revisionno = ((RadLabel)e.Item.FindControl("lblRevNo")).Text;
                string Effectiveon = ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text;
                lblBudget.Text = "Budget Revision No. " + Revisionno + " Effective From " + Effectiveon;
                Rebind();
                RebindBudget();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(((RadLabel)e.Item.FindControl("lblrevisionid")).Text);
                PhoenixRegistersVesselBudget.DeleteBudgetRevision(revisionid);
                Rebind();
                RebindBudget();
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
    protected void gvBudgetRevision_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            LinkButton dc = (LinkButton)e.Item.FindControl("cmdCopy");
            if (dc != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dc.CommandName)) dc.Visible = false;
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
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null) { if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false; }
            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Item.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                ucCurrencyAdd.DataBind();
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }
        }
        SubMenu();
    }
    private bool IsValidData(string effectivedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
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
            UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
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

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudget(int.Parse(Filter.CurrentVesselMasterFilter), General.GetNullableGuid(ViewState["REVISIONID"].ToString()),
                            sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBERB"].ToString()), gvBudget.PageSize, ref iRowCount, ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvBudget", "Budget", alCaptions, alColumns, ds);
        gvBudget.DataSource = ds;
        gvBudget.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNTB"] = iRowCount;
        ViewState["TOTALPAGECOUNTB"] = iTotalPageCount;
    }
    protected void gvBudget_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
            {
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
                string guid = ((RadLabel)e.Item.FindControl("lblBudgetid")).Text;
                if (guid != null || guid != "")
                    eb.Attributes.Add("onclick", "openNewWindow('Filter', '', '" + Session["sitepath"] + "/Registers/RegistersVesselBudgetAdd.aspx?revisionid=" + ViewState["REVISIONID"].ToString() + "&Budgetid=" + guid + "'); return true;");

            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null) { if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false; }
        }
    }
    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid budgetid = new Guid(((RadLabel)e.Item.FindControl("lblBudgetid")).Text);
                PhoenixRegistersVesselBudget.DeleteBudget(budgetid);
                RebindBudget();
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
    protected void gvBudget_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

}

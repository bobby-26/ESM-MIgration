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

public partial class CrewOffshore_CrewOffshoreBudget : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        if(ViewState["REVISIONID"].ToString()=="")
        BindPageURL(0);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Crew List", "CREWLIST");
            toolbarsub.AddButton("Compliance", "CHECK");
            toolbarsub.AddButton("Crew Format", "CREWLISTFORMAT");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbarsub.AddButton("Vessel Scale", "MANNINGSCALE");
                toolbarsub.AddButton("Manning", "MANNING");
                toolbarsub.AddButton("Budget", "BUDGET");
            }
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            CrewQuery.SelectedMenuIndex = 5;

          

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBERB"] = 1;
                ViewState["SORTEXPRESSIONB"] = null;
                ViewState["SORTDIRECTIONB"] = null;

                ViewState["REVISIONID"] = "";

                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    //UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    //UcVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        txtVessel.Text = ViewState["VESSELID"].ToString();
                }
                gvBudgetRevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreBudget.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudgetRevision')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreBudget.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "Add");


            MenuRegistersBudget.AccessRights = this.ViewState;
            MenuRegistersBudget.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreBudget.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ExcelBudget");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvBudget')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINTBUDGET");
            MenuRegistersVesslBudget.AccessRights = this.ViewState;
            MenuRegistersVesslBudget.MenuList = toolbar.Show();
            // BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidBudgedvessel(string vesselid)
    {
        if (General.GetNullableString(vesselid) == null)
            ucError.ErrorMessage = "Please switch the vessel";

        return (!ucError.IsError);
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CHECK"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CREWLISTFORMAT"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreReportCrewListFormat.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNINGSCALE"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreVesselManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
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

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudgetRevision(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
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
            if(CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('Budget','','" + Session["sitepath"] + "/Crewoffshore/CrewOffshoreBudgetAddEdit.aspx?vessel=" + ViewState["VESSELID"].ToString() + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Choose a vessel in crew list.";
            ucError.Visible = true;
            return;
        }

        DataSet dsVessel = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["VESSELID"].ToString()));

        if (dsVessel.Tables[0].Rows.Count > 0)
        {
            DataRow drVessel = dsVessel.Tables[0].Rows[0];

            txtVessel.Text = drVessel["FLDVESSELNAME"].ToString();

        }
        DataTable dt = PhoenixRegistersVesselBudget.SearchBudgetRevision(General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvBudgetRevision.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvBudgetRevision", "Budget Revision", alCaptions, alColumns, ds);
        gvBudgetRevision.DataSource = ds;
        gvBudgetRevision.VirtualItemCount = iRowCount;
        SetRowSelection();


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        BindDataBudget();


    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["REVISIONID"] = gvBudgetRevision.MasterTableView.DataKeyValues[rowindex]["FLDREVISIONID"].ToString();
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
       
            for (int i = 0; i < gvBudgetRevision.Items.Count; i++)
            {
                if (gvBudgetRevision.MasterTableView.DataKeyValues[i]["FLDREVISIONID"].ToString().Equals(ViewState["REVISIONID"].ToString()))
                {
                    gvBudgetRevision.MasterTableView.Items[i].Selected = true;
                    break;
                }
            }
        
    }

    protected void gvBudgetRevision_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((UserControlDate)_gridView.Rows[de.NewEditIndex].FindControl("ucEffectiveDateEdit")).Focus();

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

        string[] alColumns = { "FLDRANKNAME", "FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLD1STYEARSCALE", "FLDPREFERREDNATIONALITYNAME" };
        string[] alCaptions = { "Rank", "Currency", "Budgeted Wage/day", "1ST YEAR SCALE", "Preferred Nationality" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONB"] == null) ? null : (ViewState["SORTEXPRESSIONB"].ToString());
        if (ViewState["SORTDIRECTIONB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONB"].ToString());

        if (ViewState["ROWCOUNTB"] == null || Int32.Parse(ViewState["ROWCOUNTB"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTB"].ToString());

        DataTable dt = PhoenixRegistersVesselBudget.SearchBudget(int.Parse(ViewState["VESSELID"].ToString()),
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

        string[] alColumns = { "FLDRANKNAME", "FLDCURRENCYCODE", "FLDBUDGETEDWAGE", "FLD1STYEARSCALE", "FLDPREFERREDNATIONALITYNAME" };
        string[] alCaptions = { "Rank", "Currency", "Budgeted Wage/day", "1ST YEAR SCALE", "Preferred Nationality" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSIONB"] == null) ? null : (ViewState["SORTEXPRESSIONB"].ToString());
        if (ViewState["SORTDIRECTIONB"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONB"].ToString());
        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null)
        {
            ucError.ErrorMessage = "Choose a vessel in crew list.";
            ucError.Visible = true;
            return;
        }
        DataTable dt = PhoenixRegistersVesselBudget.SearchBudget(int.Parse(ViewState["VESSELID"].ToString()),
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



    private bool IsValidBudget(string rankid, string wage, string year1)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["REVISIONID"].ToString()) == null)
            ucError.ErrorMessage = "Please select the revision to add budget.";

        if (General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Rank is required.";

        if (General.GetNullableDecimal(wage) == null)
            ucError.ErrorMessage = "Budgeted Wage/day is required.";

        if (General.GetNullableDecimal(year1) == null)
            ucError.ErrorMessage = "1st Year scale required.";

        return (!ucError.IsError);
    }



    protected void gvBudgetRevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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
            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            string id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString();
            cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreBudgetAddEdit.aspx?id=" + id + "&vessel=" + ViewState["VESSELID"].ToString() + "'); return false;");

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

    protected void gvBudgetRevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            if (e.CommandName == "EDITS")
            {
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                string id = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString();
                cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreBudgetAddEdit.aspx?id=" + id + "&vessel=" + ViewState["VESSELID"].ToString() + "'); return false;");

            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                if (!IsValidBudgedvessel(txtVessel.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidData(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text
                                , ((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceAdd")).Text
                                , ((TextBox)e.Item.FindControl("txtRemarksAdd")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselBudget.InsertBudgetRevision(int.Parse(ViewState["VESSELID"].ToString()),
                    DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Text)
                    , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucTankCleanAllowanceAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucDPAllowanceAdd")).Text)
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtRemarksAdd")).Text)
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtOfferRemarksAdd")).Text)
                    );

                gvBudgetRevision.Rebind();
                ((UserControlDate)e.Item.FindControl("ucEffectiveDateAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["REVISIONID"] = "";
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                PhoenixRegistersVesselBudget.DeleteBudgetRevision(revisionid);
                gvBudgetRevision.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SELECTREVISION"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
                e.Item.Selected = true;
                ViewState["FLDREVISIONID"] = (e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                BindPageURL(nCurrentRow);
                SetRowSelection();
                gvBudget.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("COPY"))
            {
                Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());
                PhoenixRegistersVesselBudget.CopyPreviousRevBudget(revisionid);
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

    protected void gvBudgetRevision_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.RowIndex;

            if (!IsValidData(
                    ((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text
                    , ((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceEdit")).Text
                    , ((TextBox)e.Item.FindControl("txtRemarksEdit")).Text
                    ))
            {
                ucError.Visible = true;
                return;
            }
            Guid revisionid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDREVISIONID"].ToString());

            PhoenixRegistersVesselBudget.UpdateBudgetRevision(revisionid
                , int.Parse(ViewState["VESSELID"].ToString())
                , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucEffectiveDateEdit")).Text)
                , General.GetNullableInteger(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageEdit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceEdit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucTankCleanAllowanceEdit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucDPAllowanceEdit")).Text)
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtRemarksEdit")).Text)
                    , General.GetNullableString(((TextBox)e.Item.FindControl("txtOfferRemarksEdit")).Text)

                );


            gvBudgetRevision.Rebind();

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
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDBUDGET"))
            {
                if (!IsValidBudget(((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank,
                    ((UserControlMaskNumber)e.Item.FindControl("ucWageAdd")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucyearAdd")).Text))
                //((UserControlMaskNumber)e.Item.FindControl("ucyearAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                StringBuilder strList = new StringBuilder();
                RadCheckBoxList cblNationalityAdd = (RadCheckBoxList)e.Item.FindControl("cblNationalityAdd");

                foreach (ButtonListItem li in cblNationalityAdd.Items)
                {
                    if (li.Selected == true)
                    {
                        strList.Append("," + li.Value + ",");
                    }
                }

                string strNationality = strList.ToString().Replace(",,", ",");

                PhoenixRegistersVesselBudget.InsertBudget(int.Parse(ViewState["VESSELID"].ToString())
                    , new Guid(ViewState["REVISIONID"].ToString())
                    , int.Parse(((UserControlRank)e.Item.FindControl("ucRankAdd")).SelectedRank)
                    , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucWageAdd")).Text)
                    , null
                    , General.GetNullableString(strNationality)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageAdd")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceAdd")).Text)
                    , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucyearAdd")).Text)
                    );

                gvBudget.Rebind();
                ((UserControlRank)e.Item.FindControl("ucRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETEBUDGET"))
            {
                Guid budgetid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDBUDGETID"].ToString());

                PhoenixRegistersVesselBudget.DeleteBudget(budgetid);
                gvBudget.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidBudget(
                        ((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank,
                        ((UserControlMaskNumber)e.Item.FindControl("ucWageEdit")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucyearEdit")).Text))
                //((UserControlMaskNumber)e.Item.FindControl("ucyearEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Guid budgetid = new Guid(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["FLDBUDGETID"].ToString());

                StringBuilder strList = new StringBuilder();
                RadCheckBoxList cblNationalityEdit = (RadCheckBoxList)e.Item.FindControl("cblNationalityEdit");

                foreach (ButtonListItem li in cblNationalityEdit.Items)
                {
                    if (li.Selected == true)
                    {
                        strList.Append("," + li.Value + ",");
                    }
                }

                string strNationality = strList.ToString().Replace(",,", ",");

                PhoenixRegistersVesselBudget.UpdateBudget(budgetid, int.Parse(ViewState["VESSELID"].ToString())
                    , int.Parse(((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank)
                    , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucWageEdit")).Text)
                    , null
                    , General.GetNullableString(strNationality)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOverlapWageEdit")).Text)
                    , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucOtherAllowanceEdit")).Text)
                    , decimal.Parse(((UserControlMaskNumber)e.Item.FindControl("ucyearEdit")).Text)
                    );


                gvBudget.Rebind();
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
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            UserControlRank ucRankEdit = (UserControlRank)e.Item.FindControl("ucRankEdit");
            if (ucRankEdit != null)
            {
                ucRankEdit.RankList = PhoenixRegistersRank.ListRank();
                ucRankEdit.DataBind();
                ucRankEdit.SelectedRank = dr["FLDRANKID"].ToString();
            }
            RadLabel lblRemrks = (RadLabel)e.Item.FindControl("lblRemrks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucappremark");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblRemrks.ClientID;
            }

            RadCheckBoxList cblNationalityEdit = (RadCheckBoxList)e.Item.FindControl("cblNationalityEdit");
            if (cblNationalityEdit != null)
            {
                cblNationalityEdit.DataSource = PhoenixRegistersCountry.ListNationality();
                cblNationalityEdit.DataBindings.DataTextField = "FLDNATIONALITY";
                cblNationalityEdit.DataBindings.DataValueField = "FLDCOUNTRYCODE";
                cblNationalityEdit.DataBind();

                RadCheckBoxList chk = (RadCheckBoxList)e.Item.FindControl("cblNationalityEdit");
                foreach (ButtonListItem li in chk.Items)
                {
                    string[] slist = dr["FLDPREFERREDNATIONALITY"].ToString().Split(',');
                    foreach (string s in slist)
                    {
                        if (li.Value.Equals(s))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }

            RadLabel lblNationality = (RadLabel)e.Item.FindControl("lblNationality");
            if (lblNationality != null)
            {

                UserControlToolTip ucNationalityToolTip = (UserControlToolTip)e.Item.FindControl("ucNationalityToolTip");

                ucNationalityToolTip.Position = ToolTipPosition.TopCenter;
                ucNationalityToolTip.TargetControlId = lblNationality.ClientID;
                //UserControlToolTip ucNationalityToolTip = (UserControlToolTip)e.Item.FindControl("ucNationalityToolTip");
                //lblNationality.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucNationalityToolTip.ToolTip + "', 'visible');");
                //lblNationality.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucNationalityToolTip.ToolTip + "', 'hidden');");
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

            UserControlRank ucRankAdd = (UserControlRank)e.Item.FindControl("ucRankAdd");
            if (ucRankAdd != null)
            {
                ucRankAdd.RankList = PhoenixRegistersRank.ListRank();
                ucRankAdd.DataBind();
            }

            UserControlCurrency ucCurrencyAdd = (UserControlCurrency)e.Item.FindControl("ucCurrencyAdd");
            if (ucCurrencyAdd != null)
            {
                ucCurrencyAdd.CurrencyList = PhoenixRegistersCurrency.ListActiveCurrency(PhoenixSecurityContext.CurrentSecurityContext.UserCode, true);
                ucCurrencyAdd.DataBind();
                ucCurrencyAdd.SelectedCurrency = "10"; //USD
            }

            RadCheckBoxList cblNationalityAdd = (RadCheckBoxList)e.Item.FindControl("cblNationalityAdd");
            if (cblNationalityAdd != null)
            {
                cblNationalityAdd.DataSource = PhoenixRegistersCountry.ListNationality();
                cblNationalityAdd.DataBindings.DataTextField = "FLDNATIONALITY";
                cblNationalityAdd.DataBindings.DataValueField = "FLDCOUNTRYCODE";
                cblNationalityAdd.DataBind();
            }
        }
    }
}

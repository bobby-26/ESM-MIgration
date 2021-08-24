using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;

public partial class RegistersOwnerBudgetCodePBMapContractESM : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrew.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersContractESM.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["Rank"] = string.Empty;
                ViewState["COMPONENTID"] = string.Empty;
                ViewState["RATINGS"] = string.Empty;
                ViewState["OFFICERS"] = string.Empty;
            }
            BindData();
            BindDataSub();
            BindOwnerPBMapData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RegistersCity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME" };
                string[] alCaptions = { "Short Code", "Component Name", "Calculation Basis", "Payable Basis" };

                DataTable dt = PhoenixRegistersContract.ListESMContract(General.GetNullableInteger(ddlVessel.SelectedHard));
                General.ShowExcel("Standard Wage Components", dt, alColumns, alCaptions, null, string.Empty);
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
        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME" };
        string[] alCaptions = { "Short Code", "Component Name", "Calculation Basis", "Payable Basis" };

        DataTable dt = PhoenixRegistersContract.ListESMContract(General.GetNullableInteger(ddlVessel.SelectedHard));
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Standard Wage Components", alCaptions, alColumns, ds);

        if (dt.Rows.Count > 0)
        {
            gvCrew.DataSource = dt;
            gvCrew.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvCrew);
        }
    }
    private void BindDataSub()
    {
        Guid? g = null;
        if (ViewState["COMPONENTID"].ToString() != string.Empty)
            g = new Guid(ViewState["COMPONENTID"].ToString());
        DataTable dt = PhoenixRegistersContract.ListESMSubContract(g);

        if (dt.Rows.Count > 0)
        {
            gvCR.DataSource = dt;
            gvCR.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvCR);
        }
    }

    protected void gvCrew_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string shortcode = ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Text;
                string compname = ((TextBox)_gridView.FooterRow.FindControl("txtComponentNameAdd")).Text;
                string calbasis = ((UserControlHard)_gridView.FooterRow.FindControl("ddlCalBasisAdd")).SelectedHard;
                string paybasis = ((UserControlHard)_gridView.FooterRow.FindControl("ddlPayBasisAdd")).SelectedHard;
                string budgetcode = ((UserControlBudgetCode)_gridView.FooterRow.FindControl("ddlBudgetAdd")).SelectedBudgetCode;
                if (!IsValidCrewContract(shortcode, compname, ddlVessel.SelectedHard, calbasis, paybasis))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertESMContract(shortcode, compname, int.Parse(ddlVessel.SelectedHard)
                    , int.Parse(calbasis), int.Parse(paybasis), General.GetNullableInteger(budgetcode));
                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtShortCodeAdd")).Focus();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentId")).Text;
            PhoenixRegistersContract.DeleteESMContract(new Guid(componentid));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        gvCrew.EditIndex = -1;
        gvCrew.SelectedIndex = -1;
        if (General.GetNullableInteger(ddlVessel.SelectedHard).HasValue)
        {
            //DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(ddlVessel.SelectedHard));
            //ViewState["RATINGS"] = ds.Tables[0].Rows[0]["FLDWAGESCALERATING"].ToString();
            //ViewState["OFFICERS"] = ds.Tables[0].Rows[0]["FLDWAGESCALE"].ToString();
            //string errmsg = string.Empty;
            //if (ViewState["RATINGS"].ToString() == string.Empty) ucError.ErrorMessage = "Ratings Wage Scale Not mapped for the Vessel";
            //if (ViewState["OFFICERS"].ToString() == string.Empty) ucError.ErrorMessage = "Officers Wage Scale Not mapped for the Vessel";
            //if (ucError.IsError)
            //{                
            //    ucError.Visible = true;                
            //}
        }
        ViewState["COMPONENTID"] = string.Empty;
        gvCR.SelectedIndex = -1;
        gvCR.EditIndex = -1;
        BindData();
        BindDataSub();
    }

    protected void gvCrew_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        string componentid = ((Label)_gridView.Rows[de.NewEditIndex].FindControl("lblComponentId")).Text;
        ViewState["COMPONENTID"] = componentid;
        BindData();
        BindDataSub();
    }

    protected void gvCrew_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string componentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblComponentIdEdit")).Text;
            string shortcode = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtShortCodeEdit")).Text;
            string compname = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtComponentNameEdit")).Text;
            string calbasis = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalBasisEdit")).SelectedHard;
            string paybasis = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlPayBasisEdit")).SelectedHard;
            string budgetcode = ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ddlBudgetEdit")).SelectedBudgetCode;
            if (!IsValidCrewContract(shortcode, compname, ddlVessel.SelectedHard, calbasis, paybasis))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersContract.UpdateESMContract(new Guid(componentid), shortcode, compname, int.Parse(calbasis), int.Parse(paybasis), General.GetNullableInteger(budgetcode));

            string strPrincipal = ucOwner.SelectedAddress;
            string strBudgetId = ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ddlBudgetEdit")).SelectedBudgetCode;
            string strComponentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string strOwnerBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetCodeIdEdit")).Text;

            if (strBudgetId == "Dummy")
                strBudgetId = "";

         

            if (!IsValidOwnerBudgetCodeMap(strPrincipal, strOwnerBudgetId))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersOwnerBudgetCodePBMap.InsertOwnerBudgetCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , int.Parse(strPrincipal)
                                                                        , null
                                                                        , new Guid(strComponentId)
                                                                        , General.GetNullableInteger(strBudgetId)
                                                                        , new Guid(strOwnerBudgetId));

            _gridView.EditIndex = -1;
            BindData();
            BindOwnerPBMapData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOwnerBudgetCodeMap(string principal, string ownerbudgetid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(principal).HasValue)
        {
            ucError.ErrorMessage = "Principal is required.";
        }
        else if (!General.GetNullableGuid(ownerbudgetid).HasValue)
        {
            ucError.ErrorMessage = "Onwer Budget Code is required.";
        }

        return (!ucError.IsError);
    }

    protected void gvCrew_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ibp = (ImageButton)e.Row.FindControl("btnShowOwnerBudgetEdit");

            if (ibp != null)
                ibp.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudgetCodeTree.aspx?iframignore=false&OWNERID=" + ucOwner.SelectedAddress + "', true); ");

            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["onclick"] = _jsDouble;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            else
            {
                e.Row.Attributes["onclick"] = "";
            }

            UserControlHard calbasis = e.Row.FindControl("ddlCalBasisEdit") as UserControlHard;
            if (calbasis != null) calbasis.SelectedHard = drv["FLDCALCULATIONBASIS"].ToString();
            UserControlHard paybasis = e.Row.FindControl("ddlPayBasisEdit") as UserControlHard;
            if (paybasis != null) paybasis.SelectedHard = drv["FLDPAYABLEBASIS"].ToString();
            UserControlBudgetCode bud = (UserControlBudgetCode)e.Row.FindControl("ddlBudgetEdit");
            if (bud != null) bud.SelectedBudgetCode = drv["FLDBUDGETID"].ToString();
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvCrew_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        GridViewRow row = _gridView.Rows[e.NewSelectedIndex];
        _gridView.SelectedIndex = e.NewSelectedIndex;
        string componentid = ((Label)row.FindControl(_gridView.EditIndex > -1 ? "lblComponentIdEdit" : "lblComponentId")).Text;
        ViewState["COMPONENTID"] = componentid;
        BindData();
        BindDataSub();
    }
    protected void gvCR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindDataSub();
    }

    protected void gvCR_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            GridView _gridView = (GridView)sender;

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                Guid? g = null;
                string rank = ((UserControlRank)_gridView.FooterRow.FindControl("ddlRankAdd")).SelectedRank;
                string currency = ((UserControlCurrency)_gridView.FooterRow.FindControl("ddlCurrencyAdd")).SelectedCurrency;
                string amount = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtAmountAdd")).Text;
                string calc = string.Empty;// ((UserControlHard)_gridView.FooterRow.FindControl("ddlCalculationAdd")).SelectedHard;
                string cont = string.Empty;//((UserControlContractCBA)_gridView.FooterRow.FindControl("ddlContractAdd")).SelectedContract;
                if (cont != string.Empty)
                    g = new Guid(cont);
                if (!IsValidESMSub(rank, currency, amount, calc, cont))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertESMSubContract(new Guid(ViewState["COMPONENTID"].ToString()), int.Parse(rank), int.Parse(currency), decimal.Parse(amount)
                    , General.GetNullableInteger(calc), g);
                BindDataSub();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCR_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string subcomponentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubComponentId")).Text;
            PhoenixRegistersContract.DeleteESMSubContract(new Guid(subcomponentid));
            BindDataSub();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCR_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindDataSub();
    }

    protected void gvCR_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            Guid? g = null;
            string subcomponentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSubComponentIdEdit")).Text;
            string rank = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRank")).Text;
            string currency = ((UserControlCurrency)_gridView.Rows[nCurrentRow].FindControl("ddlCurrencyEdit")).SelectedCurrency;
            string amount = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text;
            string calc = string.Empty;//((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalculationEdit")).SelectedHard;
            string cont = string.Empty;//((UserControlContractCBA)_gridView.Rows[nCurrentRow].FindControl("ddlContractEdit")).SelectedContract;
            if (cont != string.Empty)
                g = new Guid(cont);
            if (!IsValidESMSub(rank, currency, amount, calc, cont))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersContract.UpdateESMSubContract(new Guid(subcomponentid), int.Parse(rank), int.Parse(currency), decimal.Parse(amount)
                , General.GetNullableInteger(calc), g);
            _gridView.EditIndex = -1;
            BindDataSub();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCR_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            }

            UserControlCurrency currency = e.Row.FindControl("ddlCurrencyEdit") as UserControlCurrency;
            if (currency != null) currency.SelectedCurrency = drv["FLDCURRENCYID"].ToString();
            UserControlHard cal = e.Row.FindControl("ddlCalculationEdit") as UserControlHard;
            if (cal != null) cal.SelectedHard = drv["FLDCALCULATION"].ToString();
            UserControlContractCBA contract = e.Row.FindControl("ddlContractEdit") as UserControlContractCBA;
            if (contract != null)
            {
                string rank = ((Label)e.Row.FindControl("lblRank")).Text;
                contract.ContractList = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ViewState[IsOfficer(int.Parse(rank)) ? "OFFICERS" : "RATINGS"].ToString()), null, null);
                contract.SelectedContract = drv["FLDMAINCOMPONENTID"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private bool IsValidCrewContract(string shortcode, string name, string vesselid, string calbasis, string paybasis)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Short Code is required.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component Name is required.";

        if (!General.GetNullableInteger(vesselid).HasValue)
            ucError.ErrorMessage = "Vessel is required.";

        if (!General.GetNullableInteger(calbasis).HasValue)
            ucError.ErrorMessage = "Calculation Basis is required";

        if (!General.GetNullableInteger(paybasis).HasValue)
            ucError.ErrorMessage = "Payable Basis is required";

        return (!ucError.IsError);
    }
    private bool IsValidESMSub(string rank, string currency, string amount, string calculation, string componentid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        decimal resutlDec;
        if (ViewState["COMPONENTID"].ToString() == string.Empty)
            ucError.ErrorMessage = "Select a Component to add Rank wise wages.";

        if (!General.GetNullableInteger(rank).HasValue)
            ucError.ErrorMessage = "Rank is required.";

        if (!General.GetNullableInteger(currency).HasValue)
            ucError.ErrorMessage = "Currency is required.";

        if (!decimal.TryParse(amount, out resutlDec))
            ucError.ErrorMessage = "Amount is required.";

        if (string.IsNullOrEmpty(componentid) && General.GetNullableInteger(calculation).HasValue)
            ucError.ErrorMessage = "CBA Contract is required.";

        if (!string.IsNullOrEmpty(componentid) && !General.GetNullableInteger(calculation).HasValue)
            ucError.ErrorMessage = "Calculation for CBA Contract is required.";

        return (!ucError.IsError);
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected bool IsOfficer(int RankId)
    {
        bool result = false;
        DataSet ds = PhoenixRegistersRank.EditRank(RankId);
        if (ds.Tables[0].Rows[0]["FLDOFFICECREW"].ToString() == PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 50, "OFF"))
            result = true;
        return result;
    }
    protected void ddlRankAdd_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlRank rank = sender as UserControlRank;
        ViewState["Rank"] = rank.SelectedRank;
        //UserControlContractCBA contract = (UserControlContractCBA)gvCR.FooterRow.FindControl("ddlContractAdd");
        //string selcont = contract.SelectedContract;
        //contract.ContractList = PhoenixRegistersContract.ListCBAContract(General.GetNullableInteger(ViewState[IsOfficer(int.Parse(rank.SelectedRank)) ? "OFFICERS" : "RATINGS"].ToString()), null, null);
        //contract.SelectedContract = selcont;
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    public void BindOwnerPBMapData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;


            DataSet ds = PhoenixRegistersOwnerBudgetCodePBMap.OwnerBudgetCodeSearch(2
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOwner.DataSource = ds;
                gvOwner.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvOwner);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwner_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gvOwner_Sorting(object sender, GridViewSortEventArgs se)
    {

    }
    protected void gvOwner_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {

    }
    protected void gvOwner_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindOwnerPBMapData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOwner.SelectedIndex = -1;
        gvOwner.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        BindOwnerPBMapData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
}

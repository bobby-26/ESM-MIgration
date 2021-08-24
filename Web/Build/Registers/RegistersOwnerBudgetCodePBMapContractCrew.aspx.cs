using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;

public partial class RegistersOwnerBudgetCodePBMapContractCrew : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersContractCrew.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
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
                string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDVESSELCHARGEABLENAME", "FLDSUBACCOUNT", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDINCLUDEDONBOARDYNNAME" };
                string[] alCaptions = { "Short Code", "Component Name", "Vessel Chargeable", "Budget Codes", "Calculation Basis", "Payable Basis", "Included Onboard" };

                DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
                General.ShowExcel("Components Agreed with Crew", dt, alColumns, alCaptions, null, string.Empty);
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
        string[] alColumns = { "FLDSHORTCODE", "FLDCOMPONENTNAME", "FLDVESSELCHARGEABLENAME", "FLDSUBACCOUNT", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME", "FLDINCLUDEDONBOARDYNNAME" };
        string[] alCaptions = { "Short Code", "Component Name", "Vessel Chargeable", "Budget Codes", "Calculation Basis", "Payable Basis", "Included Onboard" };

        DataTable dt = PhoenixRegistersContract.ListContractCrew(null);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Components Agreed with Crew", alCaptions, alColumns, ds);

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
                string vesselcharge = ((UserControlHard)_gridView.FooterRow.FindControl("ddlVesselChargeAdd")).SelectedHard;
                string budgetid = ((TextBox)_gridView.FooterRow.FindControl("txtBudgetIdAdd")).Text;
                string calbasis = ((UserControlHard)_gridView.FooterRow.FindControl("ddlCalBasisAdd")).SelectedHard;
                string paybasis = ((UserControlHard)_gridView.FooterRow.FindControl("ddlPayBasisAdd")).SelectedHard;
                string inconboard = ((RadioButtonList)_gridView.FooterRow.FindControl("rblYesNoAdd")).SelectedValue;
                string description = ((TextBox)_gridView.FooterRow.FindControl("txtAddDescription")).Text;
                string earningdeduction = ((RadioButtonList)_gridView.FooterRow.FindControl("rblEarningDeductionAdd")).SelectedValue;
                string activeyn = ((RadioButtonList)_gridView.FooterRow.FindControl("rblActiveYesNoAdd")).SelectedValue;

                if (!IsValidCrewContract(shortcode, compname, vesselcharge, budgetid, calbasis, paybasis, inconboard, earningdeduction))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertContractCrew(shortcode, compname, int.Parse(vesselcharge)
                   , int.Parse(budgetid), int.Parse(calbasis), int.Parse(paybasis), byte.Parse(inconboard), description, int.Parse(earningdeduction), General.GetNullableInteger(activeyn));
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
            PhoenixRegistersContract.DeleteContractCrew(new Guid(componentid));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
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
            string vesselcharge = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlVesselChargeEdit")).SelectedHard;
            string budgetid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text;
            string calbasis = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlCalBasisEdit")).SelectedHard;
            string paybasis = ((UserControlHard)_gridView.Rows[nCurrentRow].FindControl("ddlPayBasisEdit")).SelectedHard;
            string inconboard = ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblYesNoEdit")).SelectedValue;
            string description = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtEditDescription")).Text;
            string earningdeduction = ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblEarningDeductionEdit")).SelectedValue;
            string activeyn = ((RadioButtonList)_gridView.Rows[nCurrentRow].FindControl("rblActiveYesNoEdit")).SelectedValue;

            if (!IsValidCrewContract(shortcode, compname, vesselcharge, budgetid, calbasis, paybasis, inconboard, earningdeduction))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersContract.UpdateContractCrew(new Guid(componentid), shortcode, compname, int.Parse(vesselcharge)
                , int.Parse(budgetid), int.Parse(calbasis), int.Parse(paybasis), byte.Parse(inconboard), description, int.Parse(earningdeduction), General.GetNullableInteger(activeyn));

            string strPrincipal = ucOwner.SelectedAddress;
            string strBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtBudgetIdEdit")).Text;
            string strComponentId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey")).Text;
            string strOwnerBudgetId = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtOwnerBudgetCodeIdEdit")).Text;


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
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            else
            {
                TextBox tb1 = (TextBox)e.Row.FindControl("txtBudgetNameEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (TextBox)e.Row.FindControl("txtBudgetIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (TextBox)e.Row.FindControl("txtBudgetgroupIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetEdit");
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30&vesselid=', true); ");

                ((RadioButtonList)e.Row.FindControl("rblYesNoEdit")).Items.FindByValue(drv["FLDINCLUDEDONBOARDYN"].ToString()).Selected = true;

                if (General.GetNullableInteger(drv["FLDEARNINGDEDUCTION"].ToString()) != null)
                    ((RadioButtonList)e.Row.FindControl("rblEarningDeductionEdit")).Items.FindByValue(drv["FLDEARNINGDEDUCTION"].ToString()).Selected = true;
                ((RadioButtonList)e.Row.FindControl("rblActiveYesNoEdit")).Items.FindByValue(drv["FLDACTIVEYN"].ToString()).Selected = true;

            }
            UserControlHard vesselcharge = e.Row.FindControl("ddlVesselChargeEdit") as UserControlHard;
            if (vesselcharge != null) vesselcharge.SelectedHard = drv["FLDVESSELCHARGEABLE"].ToString();
            UserControlHard calbasis = e.Row.FindControl("ddlCalBasisEdit") as UserControlHard;
            if (calbasis != null) calbasis.SelectedHard = drv["FLDCALCULATIONBASIS"].ToString();
            UserControlHard paybasis = e.Row.FindControl("ddlPayBasisEdit") as UserControlHard;
            if (paybasis != null) paybasis.SelectedHard = drv["FLDPAYABLEBASIS"].ToString();
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            TextBox tb1 = (TextBox)e.Row.FindControl("txtBudgetNameAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (TextBox)e.Row.FindControl("txtBudgetgroupIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            ImageButton ib1 = (ImageButton)e.Row.FindControl("btnShowBudgetAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListTaxBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudget.aspx?budgetgroup=102&hardtypecode=30&vesselid=', true); ");

            ImageButton ad = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ad != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ad.CommandName))
                    ad.Visible = false;
            }
        }
    }

    private bool IsValidCrewContract(string shortcode, string name, string vesselcharge, string budgetid, string calbasis, string paybasis, string includeonboard, string earningdeduction)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(shortcode))
            ucError.ErrorMessage = "Short Code is required.";

        if (string.IsNullOrEmpty(name))
            ucError.ErrorMessage = "Component Name is required.";

        if (!General.GetNullableInteger(vesselcharge).HasValue)
            ucError.ErrorMessage = "Vessel Charge is required.";

        if (!General.GetNullableInteger(budgetid).HasValue)
            ucError.ErrorMessage = "Budget Codes is required.";

        if (!General.GetNullableInteger(calbasis).HasValue)
            ucError.ErrorMessage = "Calculation Basis is required";

        if (!General.GetNullableInteger(paybasis).HasValue)
            ucError.ErrorMessage = "Payable Basis is required";

        if (!General.GetNullableInteger(includeonboard).HasValue)
            ucError.ErrorMessage = "Included Onboard is required";

        if (!General.GetNullableInteger(earningdeduction).HasValue)
            ucError.ErrorMessage = "Earning/Deduction is required";

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


            DataSet ds = PhoenixRegistersOwnerBudgetCodePBMap.OwnerBudgetCodeSearch(3
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

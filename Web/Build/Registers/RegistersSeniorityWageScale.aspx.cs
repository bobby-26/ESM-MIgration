using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using Telerik.Web.UI;

public partial class RegistersSeniorityWageScale : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeniorityWageScale.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSWS')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersSeniorityWageScale.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuRegistersCity.AccessRights = this.ViewState;
            MenuRegistersCity.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvSWS.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDSCALENAME", "FLDRANKNAME", "FLDEXPERIENCE", "FLDCURRENCYCODE", "FLDAMOUNT" };
                string[] alCaptions = { "Wage Scale", "Rank", "Experience in Months", "Currency", "Amount" };
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
                DataTable dt = PhoenixRegistersContract.SearchSeniorityWageScale(General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale)
                                                                                    , General.GetNullableInteger(ucRank.SelectedRank)
                                                                                     , General.GetNullableInteger(ddlCurrencyAdd.SelectedCurrency)
                                                                                    , sortexpression, sortdirection
                                                                                    , 1
                                                                                    , iRowCount
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
                General.ShowExcel("Wage Scale", dt, alColumns, alCaptions, null, string.Empty);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlSeniority.SelectedSeniorityScale = string.Empty;
                ucRank.SelectedValue = 0;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvSWS.SelectedIndexes.Clear();
        gvSWS.EditIndexes.Clear();
        gvSWS.DataSource = null;
        gvSWS.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSCALENAME", "FLDRANKNAME", "FLDEXPERIENCE", "FLDCURRENCYCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Wage Scale", "Rank", "Experience in Months", "Currency", "Amount" };
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataTable dt = PhoenixRegistersContract.SearchSeniorityWageScale(General.GetNullableInteger(ddlSeniority.SelectedSeniorityScale)
                                                                                    , General.GetNullableInteger(ucRank.SelectedRank)
                                                                                    , General.GetNullableInteger(ddlCurrencyAdd.SelectedCurrency)
                                                                                    , sortexpression, sortdirection
                                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                    , gvSWS.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvSWS", "Wage Scale", alCaptions, alColumns, ds);
        gvSWS.DataSource = dt;
        gvSWS.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvSWS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSWS.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSWS_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        Rebind();
    }

    protected void gvSWS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

                string rank = ((UserControlRank)e.Item.FindControl("ddlRankAdd")).SelectedRank;
                string exp = ((UserControlMaskNumber)e.Item.FindControl("txtMonthsAdd")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text;
                if (!IsValidSeniority(ddlSeniority.SelectedSeniorityScale, rank, exp, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.InsertSeniorityWageScale(int.Parse(ddlSeniority.SelectedSeniorityScale), int.Parse(rank),int.Parse(ddlCurrencyAdd.SelectedCurrency), int.Parse(exp), decimal.Parse(amount));
                Rebind();
                ((UserControlRank)e.Item.FindControl("ddlRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string scaleid = ((RadLabel)e.Item.FindControl("lblScaleIdEdit")).Text;
                string rank = ((UserControlRank)e.Item.FindControl("ddlRankEdit")).SelectedRank;
                string exp = ((UserControlMaskNumber)e.Item.FindControl("txtMonthsEdit")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
                if (!IsValidSeniority(ddlSeniority.SelectedSeniorityScale, rank, exp, amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersContract.UpdateSeniorityWageScale(int.Parse(scaleid), int.Parse(ddlSeniority.SelectedSeniorityScale), int.Parse(rank), int.Parse(ddlCurrencyAdd.SelectedCurrency), int.Parse(exp), decimal.Parse(amount));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string scaleid = ((RadLabel)e.Item.FindControl("lblScaleId")).Text;
                PhoenixRegistersContract.DeleteSeniorityWageScale(int.Parse(scaleid));
                Rebind();
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
    protected void gvSWS_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ArrayList month = new ArrayList();
            for (int i = 4; i < (12 * 15); i = i + 4)
            {
                month.Add(i);
            }
            UserControlRank rnk = e.Item.FindControl("ddlRankEdit") as UserControlRank;
            if (rnk != null) rnk.SelectedRank = drv["FLDRANKID"].ToString();

        }       
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    protected void gvSWS_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void ddlSeniory_Changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        ViewState["SORTEXPRESSION"] = null;
        ViewState["SORTDIRECTION"] = null;
        ViewState["CURRENTINDEX"] = 1;
        Rebind();
    }
    private bool IsValidSeniority(string seniority, string rank, string experience, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDec;

        if (!General.GetNullableInteger(seniority).HasValue)
            ucError.ErrorMessage = "Wage Scale is required.";
        if (!General.GetNullableInteger(rank).HasValue)
            ucError.ErrorMessage = "Rank is required.";
        if (!General.GetNullableInteger(ddlCurrencyAdd.SelectedCurrency).HasValue)
            ucError.ErrorMessage = "Currency  is required.";
        if (!General.GetNullableInteger(experience).HasValue)
            ucError.ErrorMessage = "Experience is required.";
        else if (int.Parse(experience) < 0)
            ucError.ErrorMessage = "Experience is positive integer field.";
        if (!decimal.TryParse(amount, out resultDec))
            ucError.ErrorMessage = "Amount is required.";
        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

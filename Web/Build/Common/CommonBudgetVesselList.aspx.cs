using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonBudgetVesselList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Common/CommonBudgetVesselList.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Common/CommonBudgetVesselList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRegistersVesselList.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddButton("Owner Reporting Format", "OWNERREPORT", ToolBarDirection.Right);
            toolbar1.AddButton("Financial Year", "FINANCIALYEAR", ToolBarDirection.Right);
            toolbar1.AddButton("Vessel List", "VESSELLIST", ToolBarDirection.Right);

            MenuBudgetTab.AccessRights = this.ViewState;
            MenuBudgetTab.MenuList = toolbar1.Show();
            MenuBudgetTab.SelectedMenuIndex = 2;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        Filter.CurrentBudgetVesselSelection = txtSearchVesselList.Text.Trim();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELNUMBER", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Number", "IMO Number", "Flag", "Vessel Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string VesselSearch = (txtSearchVesselList.Text == null) ? "" : txtSearchVesselList.Text;

        if (Filter.CurrentBudgetVesselSelection == null)
            Filter.CurrentBudgetVesselSelection = "";
        VesselSearch = Filter.CurrentBudgetVesselSelection.ToString();

        ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselSearch(
            1
            , VesselSearch
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=VesselList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vessel List</h3></td>");
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

    protected void BudgetTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FINANCIALYEAR"))
        {
            Response.Redirect("../Common/CommonBudgetVesselFinancialYear.aspx?", false);
        }
        if (CommandName.ToUpper().Equals("OWNERREPORT"))
        {
            Response.Redirect("../Registers/RegistersOwnerReportingFormat.aspx?", false);
        }
    }

    protected void RegistersVesselList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Filter.CurrentBudgetVesselSelection = txtSearchVesselList.Text.Trim();
            BindData();
            gvVesselList.Rebind();
        }

        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            ViewState["PAGENUMBER"] = 1;
            Filter.CurrentBudgetVesselSelection = "";
            BindData();
            gvVesselList.Rebind();
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDVESSELNUMBER", "FLDIMONUMBER", "FLDVESSELFLAG", "FLDVESSELTYPE" };
        string[] alCaptions = { "Vessel Name", "Vessel Number", "IMO Number", "Flag", "Vessel Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string VesselSearch = (txtSearchVesselList.Text == null) ? "" : txtSearchVesselList.Text;

        if (Filter.CurrentBudgetVesselSelection == null)
            Filter.CurrentBudgetVesselSelection = "";
        VesselSearch = Filter.CurrentBudgetVesselSelection.ToString();

        DataSet ds = PhoenixCommonBudgetGroupAllocation.BudgetVesselSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , VesselSearch
            , sortexpression, sortdirection,
            gvVesselList.CurrentPageIndex + 1,
            gvVesselList.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvVesselList", "Registers", alCaptions, alColumns, ds);

        gvVesselList.DataSource = ds.Tables[0];
        gvVesselList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void gvVesselList_OnSortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvVesselList_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Filter.CurrentBudgetAllocationVesselFilter = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;
                string vesselaccountid = ((RadLabel)e.Item.FindControl("lblVesselAccountId")).Text;
                string accountid = ((RadLabel)e.Item.FindControl("lblaccountid")).Text;
                if (General.GetNullableInteger(vesselaccountid) != null)
                {
                    Response.Redirect("../Common/CommonBudgetGroupAllocation.aspx?vesselid="
                            + Filter.CurrentBudgetAllocationVesselFilter + "&vesselaccountid=" + vesselaccountid + "&accountid=" + accountid, false);
                }
                else
                {
                    ucError.ErrorMessage = "This Vessel is not mapped to any Account.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                DateTime? VesselTakeoverdate = null;

                VesselTakeoverdate = General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucTakeOverDateEdit")).Text);

                if (VesselTakeoverdate != null)
                {
                    PhoenixRegistersVesselAccount.VesselTakeoverHandoverdateupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselAccountId")).Text),
                       General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucTakeOverDateEdit")).Text),
                       General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucHandOverDateEdit")).Text));
                }

                else
                {
                    ucError.ErrorMessage = "Take Over Date is mandatory.";
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselList_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVesselList_OnDeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void gvVesselList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
        }
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

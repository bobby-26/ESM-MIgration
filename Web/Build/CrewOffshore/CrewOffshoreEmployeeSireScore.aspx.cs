using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
public partial class CrewOffshoreEmployeeSireScore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuSireScore.AccessRights = this.ViewState;
            toolbar.AddButton("Prosper", "PROSPER");
            toolbar.AddButton("Incident", "INCIDENT");
            toolbar.AddButton("Vetting", "SIRE");
            toolbar.AddButton("Inspection", "INSPECTION");

            MenuSireScore.AccessRights = this.ViewState;
            MenuSireScore.MenuList = toolbar.Show();
            MenuSireScore.SelectedMenuIndex = 2;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EMPID"] = Request.QueryString["empid"];
                gvSireScore.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSireScore_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("PROSPER"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployeeProsperScore.aspx?empid=" + ViewState["EMPID"].ToString(), false);
                MenuSireScore.SelectedMenuIndex = 0;
            }
            if (CommandName.ToUpper().Equals("INSPECTION"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployeeInspectionScore.aspx?empid=" + ViewState["EMPID"].ToString(), false);
                MenuSireScore.SelectedMenuIndex = 3;
            }

            if (CommandName.ToUpper().Equals("INCIDENT"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployeeIncidentScore.aspx?empid=" + ViewState["EMPID"].ToString(), false);
                MenuSireScore.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        string[] alColumns = {};
        string[] alCaptions = {};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixProsper.CrewSireScoreSearch(
                                                                General.GetNullableInteger(Request.QueryString["empid"])
                                                                , sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvSireScore.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvSireScore", "Sire", alCaptions, alColumns, ds);

        gvSireScore.DataSource = ds;
        gvSireScore.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvSireScore_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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

    protected void Rebind()
    {
        gvSireScore.SelectedIndexes.Clear();
        gvSireScore.EditIndexes.Clear();
        gvSireScore.DataSource = null;
        gvSireScore.Rebind();
    }
    protected void gvSireScore_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }


    protected void gvSireScore_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSireScore.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSireScore_SortCommand(object source, GridSortCommandEventArgs e)
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


}

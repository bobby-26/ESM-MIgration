using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
public partial class CrewOffshoreEmployeeInspectionScore : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuInspectionScore.AccessRights = this.ViewState;
            toolbar.AddButton("Prosper", "PROSPER");
            toolbar.AddButton("Incident", "INCIDENT");
            toolbar.AddButton("Vetting", "SIRE");
            toolbar.AddButton("Inspection", "INSPECTION");

            MenuInspectionScore.AccessRights = this.ViewState;
            MenuInspectionScore.MenuList = toolbar.Show();
            MenuInspectionScore.SelectedMenuIndex = 3;


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EMPID"]= Request.QueryString["empid"];
                gvInspectionScore.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void MenuInspectionScore_TabStripCommand(object sender, EventArgs e)
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
                MenuInspectionScore.SelectedMenuIndex = 0;
            }
            if (CommandName.ToUpper().Equals("INCIDENT"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployeeIncidentScore.aspx?empid=" + ViewState["EMPID"].ToString(), false);
                MenuInspectionScore.SelectedMenuIndex = 0;
            }
            if (CommandName.ToUpper().Equals("SIRE"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreEmployeeSireScore.aspx?empid=" + ViewState["EMPID"].ToString(), false);
                MenuInspectionScore.SelectedMenuIndex = 0;
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
        string[] alColumns = { "FLDSKILLORDERNO", "FLDSKILLNAME", "FLDDESCRIPTION", "FLDSTANDARDASSESSMENT", "FLDACTIVEYN" };
        string[] alCaptions = { "Order Number", "Name", "Description", "Standard Assessment", "Active Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixProsper.CrewInspectionScoreSearch(
                                                                General.GetNullableInteger(Request.QueryString["empid"])
                                                                , sortexpression
                                                                , sortdirection
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvInspectionScore.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvInspectionScore", "Inspection", alCaptions, alColumns, ds);

        gvInspectionScore.DataSource = ds;
        gvInspectionScore.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvInspectionScore_ItemCommand(object sender, GridCommandEventArgs e)
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
        gvInspectionScore.SelectedIndexes.Clear();
        gvInspectionScore.EditIndexes.Clear();
        gvInspectionScore.DataSource = null;
        gvInspectionScore.Rebind();
    }
    protected void gvInspectionScore_ItemDataBound(Object sender, GridItemEventArgs e)
    {
       
    }


    protected void gvInspectionScore_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionScore.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspectionScore_SortCommand(object source, GridSortCommandEventArgs e)
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

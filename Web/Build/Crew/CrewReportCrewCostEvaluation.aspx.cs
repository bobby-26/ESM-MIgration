using System;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Crew_CrewReportCrewCostEvaluation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuTravel.AccessRights = this.ViewState;
            MenuTravel.MenuList = toolbar.Show();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportCrewCostEvaluation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCostEvaluation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportCrewCostEvaluation.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuTravelStatus.AccessRights = this.ViewState;
            MenuTravelStatus.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvCostEvaluation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

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
        gvCostEvaluation.SelectedIndexes.Clear();
        gvCostEvaluation.EditIndexes.Clear();
        gvCostEvaluation.DataSource = null;
        gvCostEvaluation.Rebind();
    }
    protected void BindReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDNAME", "FLDCREWCHANGEDATE", "FLDNOOFJOINER", "FLDNOOFOFFSIGNER", "FLDAIRFAREAMOUNT", "FLDACTUALAMOUNT", "FLDCREWCHANGECOST" };
            string[] alCaptions = { "S.No.", "Vessel", "Port Name", "Agents Name", "Date of Crew Change", "No of on/s", "No of off/s", "Total Airfare", "Total Agency Cost", "Total Crew Change Cost" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewCostEvaluationRequest.SearchCrewCostEvaluationReport
                            (
                              General.GetNullableString(txtPoNumber.Text)
                            , General.GetNullableInteger(ucVessel.SelectedVessel)
                            , General.GetNullableInteger(ucport.SelectedSeaport)
                            , General.GetNullableInteger(txtAgentId.Text)
                            , General.GetNullableDateTime(txtStartDate.Text)
                            , General.GetNullableDateTime(txtEndDate.Text)
                            , sortexpression
                            , sortdirection
                            , (int)ViewState["PAGENUMBER"]
                            , gvCostEvaluation.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount
                            );

            General.SetPrintOptions("gvCostEvaluation", "Crew Cost Evaluation", alCaptions, alColumns, ds);

            gvCostEvaluation.DataSource = ds;
            gvCostEvaluation.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            if (!IsValidDetails())
            {
                ucError.Visible = true;
                return;
            }

            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    private bool IsValidDetails()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableDateTime(txtStartDate.Text) == null)
            ucError.ErrorMessage = "From date required";
        if (General.GetNullableDateTime(txtEndDate.Text) == null)
            ucError.ErrorMessage = "To date required";
        return (!ucError.IsError);
    }
    protected void MenuCrewCost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDROWNUMBER", "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDNAME", "FLDCREWCHANGEDATE", "FLDNOOFJOINER", "FLDNOOFOFFSIGNER", "FLDAIRFAREAMOUNT", "FLDACTUALAMOUNT", "FLDCREWCHANGECOST" };
                string[] alCaptions = { "S.No.", "Vessel", "Port Name", "Agents Name", "Date of Crew Change", "No. of on/s", "No. of off/s", "Total Airfare", "Total Agency Cost", "Total Crew Change Cost" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewCostEvaluationRequest.SearchCrewCostEvaluationReport
                           (
                             General.GetNullableString(txtPoNumber.Text)
                           , General.GetNullableInteger(ucVessel.SelectedVessel)
                           , General.GetNullableInteger(ucport.SelectedSeaport)
                           , General.GetNullableInteger(txtAgentId.Text)
                           , General.GetNullableDateTime(txtStartDate.Text)
                           , General.GetNullableDateTime(txtEndDate.Text)
                           , sortexpression
                           , sortdirection
                           , (int)ViewState["PAGENUMBER"]
                           , iRowCount
                           , ref iRowCount
                           , ref iTotalPageCount
                           );

                General.ShowExcel("Crew Cost Evaluation", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtPoNumber.Text = "";
                txtAgentName.Text = "";
                txtAgentNumber.Text = "";
                txtAgentId.Text = "";
                ucport.SelectedSeaport = "";
                ucVessel.SelectedVessel = "";
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCostEvaluation_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCostEvaluation_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvCostEvaluation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCostEvaluation.CurrentPageIndex + 1;

        BindReport();
    }

}
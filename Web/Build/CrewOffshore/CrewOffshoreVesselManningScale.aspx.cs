using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreVesselManningScale : PhoenixBasePage
{
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
            CrewQuery.SelectedMenuIndex = 3;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreVesselManningScale.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVesselManningScale')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreVesselManningScale.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuOffshoreVesselManningScale.AccessRights = this.ViewState;
            MenuOffshoreVesselManningScale.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        UcVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CHECK"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewComplianceCheck.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CREWLISTFORMAT"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreReportCrewListFormat.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCrewList.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreManningScale.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
        if (CommandName.ToUpper().Equals("BUDGET"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreBudget.aspx?vesselid=" + ViewState["VESSELID"].ToString(), true);
        }
    }

    protected void MenuOffshoreVesselManningScale_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDROW", "FLDBUDGETEDRANK", "FLDPREFERREDNATIONALITYNAME", "FLDBUDGETEDWAGE", "FLDACTUALRANK", 
                                         "FLDACTUALNATIONALITY", "FLDACTUALWAGE", "FLDVARIANCE", "FLDVARIANCEPERCENTAGE" };
                string[] alCaptions = { "No", "Budgeted Rank", "Budgeted Nationality", "Budgeted Wages", "Actual Rank", "Actual Nationality", 
                                          "Actual Wages", "Variance", "Variance %" };

                DataTable dt = PhoenixCrewOffshoreVesselBudget.SearchVesselManningScale(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

                General.ShowExcel("Vessel Manning Scale", dt, alColumns, alCaptions, null, null);
            }

            if (CommandName.ToUpper().Equals("FIND"))
            {                
               gvVesselManningScale.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = UcVessel.SelectedVessel;
        BindData();
    }

    public void BindData()
    {
        string[] alColumns = { "FLDROW", "FLDBUDGETEDRANK", "FLDPREFERREDNATIONALITYNAME", "FLDBUDGETEDWAGE", "FLDACTUALRANK", 
                                         "FLDACTUALNATIONALITY", "FLDACTUALWAGE", "FLDVARIANCE", "FLDVARIANCEPERCENTAGE" };
        string[] alCaptions = { "No", "Budgeted Rank", "Budgeted Nationality", "Budgeted Wages", "Actual Rank", "Actual Nationality", 
                                          "Actual Wages", "Variance", "Variance %" };
        try
        {
            DataTable dt = PhoenixCrewOffshoreVesselBudget.SearchVesselManningScale(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvVesselManningScale", "Vessel Manning Scale", alCaptions, alColumns, ds);
            gvVesselManningScale.DataSource = dt;
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVesselManningScale_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 1;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Budgeted";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Actual";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Variance";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvVesselManningScale.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

   

    protected void gvVesselManningScale_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVesselManningScale_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            RadGrid HeaderGrid = (RadGrid)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 1;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Budgeted";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Actual";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Variance";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvVesselManningScale.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void gvVesselManningScale_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (General.GetNullableGuid(dr["FLDBUDGETID"].ToString()) == null && General.GetNullableInteger(dr["FLDSIGNONOFFID"].ToString()) == null)
            {
                RadLabel lblNationality = (RadLabel)e.Item.FindControl("lblNationality");
                RadLabel lblWages = (RadLabel)e.Item.FindControl("lblWages");
                RadLabel lblActualNationality = (RadLabel)e.Item.FindControl("lblActualNationality");
                RadLabel lblActualWages = (RadLabel)e.Item.FindControl("lblActualWages");
                RadLabel lblVariance = (RadLabel)e.Item.FindControl("lblVariance");
                RadLabel lblVariancePercent = (RadLabel)e.Item.FindControl("lblVariancePercent");

                if (lblNationality != null) lblNationality.Font.Bold = true;
                if (lblWages != null) lblWages.Font.Bold = true;
                if (lblActualNationality != null) lblActualNationality.Font.Bold = true;
                if (lblActualWages != null) lblActualWages.Font.Bold = true;
                if (lblVariance != null) lblVariance.Font.Bold = true;
                if (lblVariancePercent != null) lblVariancePercent.Font.Bold = true;
            }
        }
    }
}

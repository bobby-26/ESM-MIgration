using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Configuration;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportOverdueMonthwise : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                drdwnFleet.DataSource = PhoenixRegistersFleet.ListFleet();
                drdwnFleet.DataBind();
                chkVessels.DataSource = PhoenixRegistersVessel.ListAssignedVesselTechFleet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, null, 1);
                chkVessels.DataBind();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceReportOverdueMonthwise.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceReportOverdueMonthwise.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            MenuTables.AccessRights = this.ViewState;
            MenuTables.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuTables_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string vessels = "";
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                DataTable dt = PhoenixPlannedMaintenanceWorkOrderReport.OverDueMonthwise(General.GetNullableDateTime(txtDateFrom.Text)
                                                                                        , General.GetNullableDateTime(txtDateTo.Text)
                                                                                        , General.GetNullableInteger(drdwnFleet.SelectedValue)
                                                                                        , General.GetNullableString(vessels));
                ShowExcel("Overdue Monthwise", dt);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        try
        {
            string vessels = "";
            foreach (ButtonListItem item in chkVessels.Items)
            {
                if (item.Selected)
                    vessels = vessels + item.Value + ",";
            }
            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReport.OverDueMonthwise(General.GetNullableDateTime(txtDateFrom.Text),
                General.GetNullableDateTime(txtDateTo.Text), General.GetNullableInteger(drdwnFleet.SelectedValue), General.GetNullableString(vessels));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            RadGrid1.DataSource = dt;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void drdwnFleet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drdwnFleet.SelectedValue != String.Empty)
            {
                chkVessels.DataSource = PhoenixRegistersVessel.ListAssignedVesselTechFleet(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, General.GetNullableInteger(drdwnFleet.SelectedValue), 1);
                chkVessels.DataBind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void drdwnFleet_DataBound(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            drdwnFleet.Items.Insert(0, new DropDownListItem("--Select--", "DUMMY"));
        }
    }

    public void ShowExcel(string strHeading, DataTable dt)
    {
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
        HttpContext.Current.Response.ContentType = "application/vnd.msexcel";
        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        HttpContext.Current.Response.Write("<tr>");
        HttpContext.Current.Response.Write("<td><img src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        HttpContext.Current.Response.Write("<td><h3>" + strHeading + "</h3></td>");
        HttpContext.Current.Response.Write("</tr>");
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br />");
        HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");

        HttpContext.Current.Response.Write("<tr>");
        foreach (DataColumn c in dt.Columns)
        {
            HttpContext.Current.Response.Write("<td><b>");
            HttpContext.Current.Response.Write(c.ColumnName);
            HttpContext.Current.Response.Write("</b></td>");
        }
        HttpContext.Current.Response.Write("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            HttpContext.Current.Response.Write("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                HttpContext.Current.Response.Write("<td>");
                HttpContext.Current.Response.Write(dr[i].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[i].ToString()) : dr[i]);
                HttpContext.Current.Response.Write("</td>");
            }
            HttpContext.Current.Response.Write("</tr>");
        }
        HttpContext.Current.Response.Write("</TABLE>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write("<br/>");
        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["softwarename"].ToString());
        HttpContext.Current.Response.End();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.CommandName == RadGrid.ExportToExcelCommandName)
        {
            string vessels = "";
            foreach (ButtonListItem item in chkVessels.Items)
            {
                if (item.Selected)
                    vessels = vessels + item.Value + ",";
            }

            RadGrid1.ExportSettings.Excel.Format = GridExcelExportFormat.Biff;
            RadGrid1.ExportSettings.IgnorePaging = true;
            RadGrid1.ExportSettings.ExportOnlyData = true;
            RadGrid1.ExportSettings.OpenInNewWindow = true;

            DataTable dt = PhoenixPlannedMaintenanceWorkOrderReport.OverDueMonthwise(General.GetNullableDateTime(txtDateFrom.Text),
                General.GetNullableDateTime(txtDateTo.Text), General.GetNullableInteger(drdwnFleet.SelectedValue), General.GetNullableString(vessels));
            ShowExcel("Overdue Monthwise", dt);
        }

        //if(e.CommandName.ToUpper().Equals("GRAPH"))
        //{

        //    string xaxis = "";
        //    string yaxis = "";
        //    string colorlist = "";
        //    GridItem header = RadGrid1.MasterTableView.GetItems(GridItemType.Header)[0];
        //    GridDataItem item = (GridDataItem)e.Item;

        //    for(int i=4; i < header.Cells.Count;i++)
        //    {
        //        xaxis = xaxis + "'" + header.Cells[i].Text + "',";
        //        yaxis = yaxis + "'" + item.Cells[i].Text + "',";
        //    }
        //}
    }
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton lnkGraph = (LinkButton)e.Item.FindControl("cmdGraph");

            if (lnkGraph != null)
            {
                lnkGraph.Attributes.Add("onclick", "javascript:openNewWindow('GRAPH', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceOverDueJobMonthWiseChart.aspx?fromdate="+txtDateFrom.Text+"&todate="+txtDateTo.Text+"&vesselid=" + drv["FLDVESSELID"]+ "'); ");
                lnkGraph.Visible = SessionUtil.CanAccess(this.ViewState, lnkGraph.CommandName);
            }
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void _grid_Created(object sender, GridItemEventArgs e)
    {
        foreach (GridColumn column in e.Item.OwnerTableView.RenderColumns)
        {
            if (column.UniqueName == "Vessel")
            {
                column.HeaderStyle.Width = Unit.Pixel(100);
                column.ItemStyle.Width = Unit.Pixel(100);
            }
            else
            {
                column.HeaderStyle.Width = Unit.Pixel(50);
                column.ItemStyle.Width = Unit.Pixel(50);
            }
            if (column.UniqueName == "FLDVESSELID")
            {
                column.Visible = false;
            }
        }
    }
}

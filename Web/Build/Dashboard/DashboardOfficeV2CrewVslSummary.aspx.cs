using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class DashboardOfficeV2CrewVslSummary : PhoenixBasePage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["MOD"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["mod"]))
            {
                ViewState["MOD"] = Request.QueryString["mod"];
            }
        }
    }

    protected void GvCrewVslSummary_NeedDataSource(object sender, PivotGridNeedDataSourceEventArgs e)
    {
        RadPivotGrid grid = (RadPivotGrid)sender;
        DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTechnicalByVessel(ViewState["MOD"].ToString(),2);
        GvCrewVslSummary.DataSource = dt;
    }

    protected void GvCrewVslSummary_CellDataBound(object sender, PivotGridCellDataBoundEventArgs e)
    {

        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;
            if (itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                bool isNumeric = true;
                if (dr.Length > 0)
                {
                    isNumeric = (dr[0]["FLDISNUMERIC"].ToString() == "1" ? true : false);
                }
                if (cell.Text.Trim() == string.Empty)
                {
                    //cell.Text = cell.DataItem.ToString();
                    //"<input type=\"image\" class=\"customIcon\" onclick=\"javascript: return Openpopup('codehelp1', '', '../Dashboard/DashboardKPI.aspx?measureid=e7fe7088-2489-e911-b585-06089601e630'); return false;\" src=\"/Phoenix/css/Theme1/images/settings.svg\" alt=\"Color\" style=\"border-width:0px;\">";
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: top.openNewWindow('color', 'Color', 'Dashboard/DashboardKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                           + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript:  top.openNewWindow('chart', 'Chart', 'Dashboard/DashboardV2Chart.aspx?mod=CREWV2&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>"
                                               + "</a>" : String.Empty) + cell.DataItem.ToString();
                }
            }
        }
        if (e.Cell is PivotGridDataCell)
        {
            PivotGridDataCell cell = (PivotGridDataCell)e.Cell;
            if (cell.CellType == PivotGridDataCellType.DataCell)
            {
                DataTable dt = (DataTable)grid.DataSource;
                string code = string.Empty;
                string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
                string vessel = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;

                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND Vessel='" + vessel + "'");
                foreach (DataRow d in dr)
                {
                    string text = cell.Text;
                    if (d["FLDISNUMERIC"].ToString() != "1" || d["FLDDETAILLINK"].ToString().Trim().Equals("") || d["FLDMEASURE"].ToString() == "0")
                    {
                        cell.Text = d["FLDMEASURE"].ToString();
                        cell.HorizontalAlign = HorizontalAlign.Right;                    
                    }
                    else
                    {
                        //cell.CssClass = "label";
                        //cell.BackColor =  ColorTranslator.FromHtml("#27727B");
                        string querystring = "?code=" + d["Code"].ToString() + " &vslname=" + vessel + "&Vesselid=" + d["FLDVESSELID"].ToString();
                        string link = d["FLDDETAILLINK"].ToString();
                        int index = link.IndexOf('?');
                        if (index > -1)
                        {
                            querystring = querystring.Replace("?", "&");
                        }
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" href=\"javascript:top.openNewWindow('wo', '" + d["Measure"].ToString() + "', '" + link + querystring + "',false);\" >" + cell.Text + "</a>";
                    }
                }
            }
        }
    }
    
}
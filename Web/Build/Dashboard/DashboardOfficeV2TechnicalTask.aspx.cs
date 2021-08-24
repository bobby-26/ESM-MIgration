using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardOfficeV2TechnicalTask : PhoenixBasePage
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
            ViewState["Due"] = "1";
        }
    }

    protected void GvPMS_NeedDataSource(object sender, Telerik.Web.UI.PivotGridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void GvPMS_CellDataBound(object sender, Telerik.Web.UI.PivotGridCellDataBoundEventArgs e)
    {
        
        RadPivotGrid grid = (RadPivotGrid)sender;
        if (e.Cell is PivotGridRowHeaderCell)
        {
            PivotGridRowHeaderCell cell = (PivotGridRowHeaderCell)e.Cell;
            PivotGridRowHeaderItem item = (PivotGridRowHeaderItem)e.Cell.DataItemContainer;
            string row = cell.ParentIndexes.Length > 0 ? cell.ParentIndexes[0].ToString() : string.Empty;
            System.Collections.ArrayList itemarray = (System.Collections.ArrayList)item.DataItem;    
            if(itemarray.Count > 1)
            {
                DataTable dt = (DataTable)grid.DataSource;
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + itemarray[1].ToString() + "'");
                bool isNumeric = true;
                if(dr.Length > 0)
                {
                    isNumeric = (dr[0]["FLDISNUMERIC"].ToString() == "1" ? true : false);
                }
                if (cell.Text.Trim() == string.Empty)
                {
                    //"<input type=\"image\" class=\"customIcon\" onclick=\"javascript: return Openpopup('codehelp1', '', '../Dashboard/DashboardKPI.aspx?measureid=e7fe7088-2489-e911-b585-06089601e630'); return false;\" src=\"/Phoenix/css/Theme1/images/settings.svg\" alt=\"Color\" style=\"border-width:0px;\">";
                    cell.Text = (isNumeric ? "<a title=\"Color\" alternatetext=\"Color\" href=\"javascript: openNewWindow('color', 'Color', 'Dashboard/DashboardKPI.aspx?measureid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"fas fa-cog\"></i></span>"
                                           + "</a>" + "<a title=\"Chart\" alternatetext=\"Chart\" href=\"javascript: openNewWindow('chart', 'Chart', 'Dashboard/DashboardV2Chart.aspx?mod="+ ViewState["MOD"].ToString() + "&mid=" + itemarray[1].ToString() + "');\" style=\"display:inline-block;height:20px;width:20px;\">"
                                        + "<span class=\"icon\"><i class=\"far fa-chart-bar\"></i></span>"
                                               + "</a>": String.Empty) + cell.DataItem.ToString();
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
                //string measure = string.Empty;                
                string measureid = cell.ParentRowIndexes.Length > 1 ? cell.ParentRowIndexes[1].ToString() : string.Empty;
                string vessel = cell.ParentColumnIndexes.Length > 0 ? cell.ParentColumnIndexes[0].ToString() : string.Empty;
                //if (row.IndexOf('~') > -1)
                //{
                //    string[] arr = row.Split('~');
                //    code = arr[0].Trim();
                //    measure = arr[1].Trim();
                //}
                DataRow[] dr = dt.Select("FLDMEASUREID = '" + measureid + "' AND Vessel='" + vessel + "'");
                //string employees = string.Empty;
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
                        cell.HorizontalAlign = HorizontalAlign.Right;
                        cell.CssClass = "label";
                        //cell.BackColor =  ColorTranslator.FromHtml("#27727B");
                        string querystring = "?code=" + d["Code"].ToString() + " &vslname=" + vessel + "&vslid=" + d["FLDVESSELID"].ToString();
                        string link = d["FLDDETAILLINK"].ToString();
                        int index = link.IndexOf('?');
                        if (index > -1)
                        {
                            querystring = querystring.Replace("?", "&");
                        }
                        cell.Text = "<a style=\"background-color: " + (d["FLDCOLOR"].ToString().Equals("") ? "#27727B" : d["FLDCOLOR"].ToString()) + ";\" class=\"mlabel\" href=\"javascript:top.openNewWindow('wo', '" + d["Measure"].ToString() + "', '" + link + querystring + "',false);\" >" + cell.Text + "</a>";
                    }
                }
            }
        }
        if (e.Cell is PivotGridHeaderCell)
        {
            PivotGridHeaderCell cell = (PivotGridHeaderCell)e.Cell;

            DataTable dt = (DataTable)grid.DataSource;
            DataRow[] dr = dt.Select("Vessel='" + cell.DataItem.ToString() + "'");

            if (dr.Length > 0)
                cell.Attributes.Add("onclick", "javascript: openNewWindow('codehelp1', '', 'Dashboard/DashboardVesselDetails.aspx?vesselid=" + dr[0]["FLDVESSELID"].ToString() + "'); return false;");

        }
    }

    protected void lstDue_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        ViewState["Due"] = e.Value;
        GvPMS.Rebind();
    }

    private void BindData()
    {
        //var unused = grid.Fields["Code"] as PivotGridRowField;
        //unused.IsHidden = true;
        //unused.CellStyle=
        DataTable dt = PhoenixDashboardTechnical.DashboardOfficeTaskByVessel(ViewState["MOD"].ToString(), General.GetNullableInteger(ViewState["Due"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        GvPMS.DataSource = dt;
    }
}
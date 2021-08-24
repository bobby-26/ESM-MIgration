using System;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using System.Globalization;
using Telerik.Web.UI.ExportInfrastructure;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Common.FormatProviders;
using System.IO;

public partial class VesselAccountsStockandVictuallingRateDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            CreateMenu();
            if (!IsPostBack)
            {
                if (Request.QueryString["type"] != null)
                    ddlType.SelectedValue = Request.QueryString["type"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuStock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidData(txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                gvStock.ExportSettings.FileName = "Stock And VictuallingRate";
                gvStock.MasterTableView.ExportToExcel();
                //if (!IsValidData(txtFromDate.Text, txtToDate.Text))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                //BindData();
                //DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStockAndVictuallingRateDetails(General.GetNullableInteger(ddlVessel.SelectedVessel)
                //                                                                       , General.GetNullableDateTime(txtFromDate.Text)
                //                                                                       , General.GetNullableDateTime(txtToDate.Text));
                //Response.ClearContent();
                //Response.ContentType = "application/ms-excel";
                //Response.AddHeader("content-disposition", "attachment;filename=VicutuallingrateDetails.xls");
                //Response.Charset = "";
                //System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    DataTable dtUniqRecords = new DataTable();
                //    dtUniqRecords = ds.Tables[0].DefaultView.ToTable(true, "FLDYEAR", "FLDMONTH");
                //    stringwriter.Write("<table><tr><td> </td></tr>");
                //    stringwriter.Write("<tr>");
                //    stringwriter.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                //    stringwriter.Write("<td colspan=5><h3>Victualling Rate - Detailed Report</h3></td>");
                //    stringwriter.Write("</tr>");
                //    stringwriter.Write("</table><table border=\"1\"><tr><td></td>");
                //    for (int i = 0; i < dtUniqRecords.Rows.Count; i++)
                //    {
                //        stringwriter.Write("<td align=\"center\" colspan=5>" + DateTime.Parse("01/" + dtUniqRecords.Rows[i]["FLDMONTH"].ToString() + "/" + dtUniqRecords.Rows[i]["FLDYEAR"].ToString()).ToString("MMM") + "-" + dtUniqRecords.Rows[i]["FLDYEAR"].ToString() + "</td>");
                //    }
                //    stringwriter.Write("</tr></table>");
                //}


                //System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                //HtmlTextWriter htw = new HtmlTextWriter(stringwriter1);
                //gvStock.RenderBeginTag(htw);
                ////gvStock.HeaderRow.RenderControl(htw);
                //foreach (GridDataItem row in gvStock.Items)
                //{
                //    row.RenderControl(htw);
                //}
                //gvStock.RenderEndTag(htw);
                //Response.Write(stringwriter + stringwriter1.ToString().Replace("table", "table border =\"1\"").Replace("td", "td valign=\"top\""));
                //Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void typechange(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue != "4")
        {
            Response.Redirect("../VesselAccounts/VesselAccountsStockandVictuallingRate.aspx?type=" + ddlType.SelectedValue, false);
        }
    }
    private bool IsValidData(string From, string To)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(From).HasValue)
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        if (!General.GetNullableDateTime(To).HasValue)
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        if (DateTime.TryParse(From, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(To)) > 0)
        {
            ucError.ErrorMessage = "From date later than To date.";
        }

        return (!ucError.IsError);
    }
    protected void Rebind()
    {
        gvStock.SelectedIndexes.Clear();
        gvStock.EditIndexes.Clear();
        gvStock.DataSource = null;
        gvStock.Rebind();
    }
    protected void gvStock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
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

            DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStockAndVictuallingRateDetails(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                          , General.GetNullableDateTime(txtFromDate.Text)
                                                                                          , General.GetNullableDateTime(txtToDate.Text));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["RECORDCOUNT"] = "1";
                DataTable dt = GetTable(ds);
                DataTable dtUniqRecords = new DataTable();
                dtUniqRecords = ds.Tables[0].DefaultView.ToTable(true, "FLDYEAR", "FLDMONTH","FLDNAME");
                for (int i = 0; i < dtUniqRecords.Rows.Count; i++)
                {
                    gvStock.MasterTableView.ColumnGroups.FindGroupByName(dtUniqRecords.Rows[i]["FLDMONTH"].ToString()).HeaderText = DateTime.Parse("01/" + dtUniqRecords.Rows[i]["FLDMONTH"].ToString() + "/" + dtUniqRecords.Rows[i]["FLDYEAR"].ToString()).ToString("MMM") + "-" + dtUniqRecords.Rows[i]["FLDYEAR"].ToString();
                }
                gvStock.DataSource = dt;
                gvStock.VirtualItemCount = dt.Rows.Count;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStockandVictuallingRateDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStockandVictuallingRateDetails.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuStock.AccessRights = this.ViewState;
        MenuStock.MenuList = toolbar.Show();
    }
    private DataTable GetTable(DataSet ds)
    {
        DataTable table = new DataTable();
        table.Columns.Add("Vessel Name"); 
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            table.Columns.Add(ds.Tables[0].Rows[i]["FLDNAME"].ToString() + "-" + ds.Tables[0].Rows[i]["FLDMONTH"].ToString() + ds.Tables[0].Rows[i]["FLDYEAR"].ToString(), typeof(string));
        }
        DataTable dtUniqRecords = new DataTable();
        dtUniqRecords = ds.Tables[1].DefaultView.ToTable(true, "FLDVESSELNAME");
        foreach (DataRow dr in dtUniqRecords.Rows)
        {
            DataRow[] result = ds.Tables[1].Select("FLDVESSELNAME ='" + dr["FLDVESSELNAME"] + "'");
            DataRow rows = table.NewRow();
            rows["Vessel Name"] = dr["FLDVESSELNAME"];
            foreach (DataRow row in result)
            {
                rows["Victualling Rate" + "-" + row["FLDMONTH"].ToString() + row["FLDYEAR"].ToString()] = row["FLDVICTUALLINGRATE"];
                rows["Provision Cost" + "-" + row["FLDMONTH"].ToString() + row["FLDYEAR"].ToString()] = row["FLDPROVISIONAMOUNT"];
                rows["Delivery Cost" + "-" + row["FLDMONTH"].ToString() + row["FLDYEAR"].ToString()] = row["FLDDELIVERYCHARGES"];
                rows["Reason for high Victualling" + "-" + row["FLDMONTH"].ToString() + row["FLDYEAR"].ToString()] = row["FLDREASON"];
                rows["Total Cost" + "-" + row["FLDMONTH"].ToString() + row["FLDYEAR"].ToString()] = row["FLDTOTAL"];
            }
            table.Rows.Add(rows);
        }
        return table;
    }
    protected void gvStock_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
    {
        try
        {
            if (e.Column.UniqueName != null && e.Column.UniqueName != "ExpandColumn")
            {
                if (e.Column.UniqueName.ToString() == "Vessel Name")
                {
                    e.Column.HeaderText = "Vessel Name";
                    e.Column.HeaderStyle.Width = Unit.Pixel(180);
                }
                else
                {
                    int index = e.Column.UniqueName.ToString().IndexOf("-");
                    if (index > 0)
                    {
                        string g = e.Column.UniqueName.ToString().Substring(index + 1).ToString();
                        int o = g.Length;
                        string p = g.Substring(0, o - 4);
                        int i = e.Column.HeaderText.Length;
                        e.Column.ColumnGroupName = p;
                        e.Column.HeaderText = e.Column.UniqueName.ToString().Substring(0, index);
                        e.Column.HeaderStyle.Width = Unit.Pixel((i * 7) + 2);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStock_InfrastructureExporting(object sender, GridInfrastructureExportingEventArgs e)
    {
        ExportStructure structure = e.ExportStructure;

        structure.Tables[0].Style.Font.Size = 10;

        XlsxRenderer renderer = new XlsxRenderer(structure);
        byte[] xlsxByteArray = renderer.Render();

        XlsxFormatProvider formatProvider = new XlsxFormatProvider();
        Workbook myWorkbook = formatProvider.Import(xlsxByteArray);

        Worksheet worksheet = myWorkbook.ActiveWorksheet;
        worksheet.WorksheetPageSetup.FitToPages = true;
        var colins = worksheet.UsedCellRange.ToIndex.ColumnIndex + 1;
        byte[] data = formatProvider.Export(myWorkbook);
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        Response.Headers.Remove("Content-Disposition");
        Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", gvStock.ExportSettings.FileName));
        Response.BinaryWrite(data);
        Response.End();

    }

    protected void gvStock_ExportCellFormatting(object sender, ExportCellFormattingEventArgs e)
    {
        GridDataItem item = e.Cell.Parent as GridDataItem;
        e.Cell.Style["border"] = "solid 0.1pt #000000";
        gvStock.ExportSettings.Excel.DefaultCellAlignment = HorizontalAlign.Center;
        gvStock.ExportSettings.ExportOnlyData = true;
        if (e.FormattedColumn.UniqueName == "ExpandColumn")
        { 
            return;
        }
    }

    protected void gvStock_GridExporting(object sender, GridExportingArgs e)
    {
        if (e.ExportType == ExportType.Excel)
        {
            string css = "<style> body { border:solid 0.1pt #CCCCCC; }</style>";
            e.ExportOutput = e.ExportOutput.Replace("</head>", css + "</head>");
        }
    }
}

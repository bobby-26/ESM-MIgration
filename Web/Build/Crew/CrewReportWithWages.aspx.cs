using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewReports;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewReportWithWages : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportWithWages.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportWithWages.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            ViewState["OFFSIGNER"] = "0";
            if (!IsPostBack)
            {
                ucTodate.Text = DateTime.Now.ToShortDateString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucVessel.SelectedVessel.ToString(), ucFromdate.Text, ucTodate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ShowReport();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowReport();

                DataSet ds = new DataSet();

                ds = PhoenixCrewList.CrewListWithWages(General.GetNullableInteger(ucVessel.SelectedVessel),
                                                  General.GetNullableDateTime(ucFromdate.Text),
                                                  General.GetNullableDateTime(ucTodate.Text));
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=CrewListWithWages.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + gvPB.Columns.Count + "\"><b>Crew List With Wages<b></td></tr>");
                stringwriter.Write("<tr><td colspan=\"" + gvPB.Columns.Count + "\"></td></tr>");
                stringwriter.Write("<tr><td><b>Vessel Name:</b></td><td>" + ds.Tables[2].Rows[0]["FLDVESSELNAME"] + "</td>");
                stringwriter.Write("<td><b>From Date:</b></td><td>" + ds.Tables[2].Rows[0]["FLDFROMDATE"] + "</td>");
                stringwriter.Write("<td><b>To Date:</b></td><td>" + ds.Tables[2].Rows[0]["FLDCLOSINGDATE"] + "</td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvPB.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVessel.SelectedVessel = "";
                ucFromdate.Text = "";
                ucTodate.Text = DateTime.Now.ToShortDateString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowReport()
    {

        int iRowCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixCrewList.CrewListWithWages(General.GetNullableInteger(ucVessel.SelectedVessel),
                                                General.GetNullableDateTime(ucFromdate.Text),
                                                General.GetNullableDateTime(ucTodate.Text));
        string[] nonaggcol = { "File No", "Staff Name", "Rank Code", "From", "To", "Days" };
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < nonaggcol.Length; i++)
            {
                if (nonaggcol[i] == "Staff Name")
                {
                    
                    GridHyperLinkColumn lnk = new GridHyperLinkColumn();
                    lnk.HeaderText = nonaggcol[i];
                    lnk.DataTextField = nonaggcol[i];
                    
                    gvPB.Columns.Add(lnk);
                }
                else
                {
                    GridBoundColumn field = new GridBoundColumn();
                    field.DataField = nonaggcol[i];
                    field.HeaderText = nonaggcol[i];
                    if (i == 3 || i == 4)
                        field.DataFormatString = "{0:dd/MM/yyyy}";
                    gvPB.Columns.Add(field);
                }
            }
            DataTable dt = ds.Tables[1];
            int ecnt = 0, dcnt = 0;
            for (int i = 0; i < dt.Rows.Count - 2; i++)
            {
                if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "1") ecnt++; else if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1") dcnt++;
                GridBoundColumn field = new GridBoundColumn();
                if (dt.Rows[i]["FLDEARNINGDEDUCTION"].ToString() == "-1" && (i == 0 || dt.Rows[i - 1]["FLDEARNINGDEDUCTION"].ToString() == "1"))
                {
                    field = new GridBoundColumn();
                    field.DataField = "FLDTOTALEAR";
                    field.HeaderText = "Total Earnings";
                    field.FooterText = dt.Rows[dt.Rows.Count - 2]["FLDAMOUNT"].ToString();
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.ItemStyle.Font.Bold = true;
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    gvPB.Columns.Add(field);
                }

                if (i == dt.Rows.Count - 3)
                {
                    field = new GridBoundColumn();
                    field.DataField = "FLDTOTALDED";
                    field.HeaderText = "Total Deduction";
                    field.FooterText = dt.Rows[dt.Rows.Count - 3]["FLDAMOUNT"].ToString();
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.ItemStyle.Font.Bold = true;
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    gvPB.Columns.Add(field);

                    

                    field = new GridBoundColumn();
                    field.DataField = "FLDGRANDTOTAL";
                    field.HeaderText = "Final Balance";
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.ItemStyle.Font.Bold = true;
                    field.FooterText = dt.Rows[dt.Rows.Count - 1]["FLDAMOUNT"].ToString();
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    gvPB.Columns.Add(field);
                }
                else
                {
                    field = new GridBoundColumn();
                    field.DataField = (i + 1).ToString();
                    field.HeaderText = dt.Rows[i]["FLDCOMPONENTNAME"].ToString();
                    field.FooterText = dt.Rows[i]["FLDAMOUNT"].ToString();
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.FooterStyle.Font.Bold = true;
                    gvPB.Columns.Add(field);
                }
            }

            gvPB.DataSource = ds;
            gvPB.VirtualItemCount = iRowCount;
            gvPB.Rebind();

                        
            GridTableView tableview = new GridTableView();

            GridDataItem row = new GridDataItem(tableview, 0, 0, GridItemType.Header);
            row.Attributes.Add("style", "position:static");
            GridTableCell cell = new GridTableCell();
            cell.ColumnSpan = 6;
            row.Cells.Add(cell);

            cell = new GridTableCell();
            cell.ColumnSpan = ecnt + 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Earnings";
            row.Cells.Add(cell);

            cell = new GridTableCell();
            cell.ColumnSpan = dcnt + 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "Deductions";
            row.Cells.Add(cell);
            cell = new GridTableCell();
            cell.ColumnSpan = 1;
            row.Cells.Add(cell);
            gvPB.Controls[0].Controls.AddAt(0, row);
        }
        else
        {
            GridBoundColumn field = new GridBoundColumn();
            field.HeaderText = "";
            gvPB.Columns.Add(field);
            DataTable dt = new DataTable();

        }

    }

    protected void gvPB_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPB.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvPB_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                if (drv.Row.Table.Columns.Count > 0 && drv["FLDSIGNOFFDATE"].ToString() != string.Empty && ViewState["OFFSIGNER"].ToString() == "0")
                {
                    ViewState["OFFSIGNER"] = "1";
                    
                    GridDataItem row = new GridDataItem(gvPB.MasterTableView, e.Item.RowIndex, (e.Item as GridDataItem).DataSetIndex);
                    row.Attributes.Add("style", "position:static");
                    GridTableCell cell = new GridTableCell();
                    cell.Text = "Off-Signers";
                    cell.Visible = true;
                    cell.ColumnSpan = gvPB.Columns.Count + 1;
                    cell.Attributes.Add("style", "font-weight:bold");
                    row.Cells.Add(cell);
                    gvPB.MasterTableView.Controls[0].Controls.AddAt(e.Item.RowIndex, row);
                    
                }
            }
        }
    }
    

    public bool IsValidFilter(string vesselid, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (vesselid.Equals("") || vesselid.Equals("Dummy") || vesselid.Equals(","))
        {
            ucError.ErrorMessage = "Vessel is Required";
        }
        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }
        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
}

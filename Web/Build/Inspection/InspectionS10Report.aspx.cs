using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionS10Report : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS10Report.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS10Report.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionS10Report.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuS10Report.AccessRights = this.ViewState;
            MenuS10Report.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                BindYear(ddlFromYear);
                BindYear(ddlToYear);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear(RadComboBox ddl)
    {
        for (int i = 2005; i <= DateTime.Now.Year; i++)
        {
            ddl.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
        }
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    private void BindData()
    {
        lblGridAuditObservation.Text = PrepareAuditObservationSummary().ToString();
        BindNavSummary();
        BindCategoryWiseBreakupObs();
        lblGridSummary.Text = PrepareNavigationAuditSummary().ToString();
        BindCategoryWiseBreakupSummary();
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDPERIODNAME", "FLDTOTALAUDITSINTERNAL", "FLDTOTALOBSERVATIONINTERNAL", "FLDAVERAGEINTERNAL", "FLDTOTALAUDITSEXTERNAL", "FLDTOTALOBSERVATIONEXTERNAL", "FLDAVERAGEEXTERNAL" };
        string[] alCaptions = { "Year", "Total No. of Audits (Int.)", "Total No. of Obs (Int.)", "Average Obs/insp (Int.)", "Total No. of Audits (Ext.)", "Total No. of Obs (Ext.)", "Average Obs/insp (Ext.)" };

        // 1)Total audits/observations

        Response.AddHeader("Content-Disposition", "attachment; filename=S10Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:1000%;border-collapse:collapse;\">");
        Response.Write("<tr><td colspan='5'><b>1) Total audits/observations</b></td></tr>");
        Response.Write("</TABLE>");
        StringBuilder sb = new StringBuilder();
        sb.Append(PrepareAuditObservationSummary().ToString());

        // 2)Navigation Audits by Internal / External Parties

        ds = PhoenixInspectionS10Report.InspectionS10ReportNavigationAuditSummary(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV");
        sb.Append(PrepareData("2) Navigation Audits by Internal / External Parties", ds.Tables[0], alColumns, alCaptions));

        // 3)Category wise break up of these observations by the external parties

        ds = PhoenixInspectionS10Report.InspectionS10CategorywiseBreakupObservations(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV"
                                                                    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
        alColumns = new string[] { "FLDCATEGORYNAME", "FLDTOTALOBS", "FLDAVERAGE" };
        alCaptions = new string[] { "Category", "Total Observations", "Observations/inspection" };
        string period = ddlFrommonth.SelectedItem + "," + ddlFromYear.SelectedValue + " - " + ddlTomonth.SelectedItem + "," + ddlToYear.SelectedValue;
        sb.Append(PrepareData("3)Category wise break up of these observations by the external parties (" + period + " )", ds.Tables[0], alColumns, alCaptions));

        // 4)Summary of all the Navigation audits        
        sb.Append("<br />");
        sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:1000%;border-collapse:collapse;\">");
        sb.Append("<tr><td><b>4)Summary of all the Navigation audits</b></td></tr>");
        sb.Append("</TABLE>");
        sb.Append(PrepareNavigationAuditSummary().ToString());

        // 5)Breakup of observations as per categories 

        ds = PhoenixInspectionS10Report.InspectionS10CategorywiseBreakupObservations(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV"
                                                                    , null);
        sb.Append(PrepareData("5)Breakup of observations as per categories (" + period + " )", ds.Tables[0], alColumns, alCaptions));

        Response.Write(sb.ToString());
        Response.End();
    }

    protected StringBuilder PrepareData(string title, DataTable dt, string[] alColumns, string[] alCaptions)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<br />");
        sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"0\" style=\"font-size:11px;width:100%;border-collapse:collapse;\">");
        sb.Append("<tr>");
        sb.Append("<td colspan='" + (alColumns.Length - 2).ToString() + "'><b>" + title + "</b></td>");
        sb.Append("</tr>");
        sb.Append("</TABLE>");
        //sb.Append("<br />");
        sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\">");
        sb.Append("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            sb.Append("<td width='20%'>");
            sb.Append("<b>" + alCaptions[i] + "</b>");
            sb.Append("</td>");
        }
        sb.Append("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr style=\"height:15px;\" >");
            for (int i = 0; i < alColumns.Length; i++)
            {
                sb.Append("<td>");
                sb.Append(dr[alColumns[i]]);
                sb.Append("</td>");

            }
            sb.Append("</tr>");
        }
        sb.Append("</TABLE>");
        return sb;
    }

    protected StringBuilder PrepareAuditObservationSummary()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionS10Report.InspectionS10ReportObservationSummary(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue));
        StringBuilder sb = new StringBuilder();
       
        if (ds.Tables[1].Rows.Count > 0)
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr class='rgHeader'><td align='CENTER' ><b></b></td>");
            //sb.Append("</tr><tr>");

            //Printing the Header


            DataTable dtColumns = ds.Tables[2];
            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<td align='CENTER' width=20% ><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("</tr>");

            //Printing the Data
            DataTable dtRows = ds.Tables[1];
            foreach (DataRow dr in dtRows.Rows)
            {
                DataTable dtData = ds.Tables[0];
                DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                sb.Append("<tr style=\"height:15px;\" ><td align='left' >" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<td align='center'>");
                    sb.Append(drTemp["FLDCOUNT"].ToString());
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            
            sb.Append("</table>");
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr style=\"height:15px;\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:15px;\"><td align=\"center\" colspan=\"6\" style=\"font-weight:bold;\">NO RECORDS FOUND</td></tr>");
            sb.Append("</table>");
        }
        return sb;

       
    }

    protected StringBuilder PrepareNavigationAuditSummary()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionS10Report.InspectionS10Summary(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV");
        StringBuilder sb = new StringBuilder();
        if (ds.Tables[1].Rows.Count > 0)
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:99%;border-collapse:collapse;\"> ");
            sb.Append("<tr class='rgHeader'><td align='CENTER' width=20%><b>Yearwise</b></td>");
            //sb.Append("</tr><tr>");

            //Printing the Header

            DataTable dtColumns = ds.Tables[2];
            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<td colspan='2' width=20% align='CENTER'><b>");
                sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                sb.Append("</b></td>");
            }
            sb.Append("</tr>");
            sb.Append("<tr class='rgHeader'><td align='CENTER' width=20%><b>Category</b></td>");
            foreach (DataRow drTempHeader in dtColumns.Rows)
            {
                sb.Append("<td align='CENTER' width=20%><b>");
                sb.Append("Total Observations");
                sb.Append("</b></td>");
                sb.Append("<td align='CENTER' width=20%><b>");
                sb.Append("Obs/insp");
                sb.Append("</b></td>");
            }
            sb.Append("</tr>");

            //Printing the Data
            DataTable dtRows = ds.Tables[1];
            foreach (DataRow dr in dtRows.Rows)
            {
                DataTable dtData = ds.Tables[0];
                DataRow[] drv = dtData.Select("FLDROWID = " + dr["FLDROWID"].ToString());

                sb.Append("<tr style=\"height:15px;\" ><td align='left' width=20%>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                foreach (DataRow drTemp in drv)
                {
                    sb.Append("<td align='center'  width=20%>");
                    sb.Append(drTemp["FLDTOTALOBS"].ToString());
                    sb.Append("</td>");
                    sb.Append("<td align='center' width=20%>");
                    sb.Append(drTemp["FLDAVERAGE"].ToString());
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:99%;border-collapse:collapse;\"> ");
            sb.Append("<tr style=\"height:15px;\" ><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:15px;\"><td align=\"center\" colspan=\"6\" style=\"font-weight:bold;\">NO RECORDS FOUND</td></tr>");
            sb.Append("</table>");
        }
        return sb;
    }

    protected void gvNavAuditSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindNavSummary();
    }

    protected void BindNavSummary()
    {
        DataSet ds = PhoenixInspectionS10Report.InspectionS10ReportNavigationAuditSummary(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV");

        gvNavAuditSummary.DataSource = ds;

    }

    protected void gvNavAuditSummary_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Navigation Audit by Superintendents";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Navigation Audit by External Parties";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvNavAuditSummary.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void gvCategorywiseBreakupObs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCategoryWiseBreakupObs();
    }

    protected void BindCategoryWiseBreakupObs()
    {
        DataSet ds = PhoenixInspectionS10Report.InspectionS10CategorywiseBreakupObservations(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV"
                                                                    , General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));

        gvCategorywiseBreakupObs.DataSource = ds;

    }

    protected void gvCategorywiseBreakupObs_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "Yearwise";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = ddlFrommonth.SelectedItem + "," + ddlFromYear.SelectedValue + " - " + ddlTomonth.SelectedItem + "," + ddlToYear.SelectedValue;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvCategorywiseBreakupObs.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    protected void gvCategorywiseBreakupSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCategoryWiseBreakupSummary();
    }

    protected void BindCategoryWiseBreakupSummary()
    {
        DataSet ds = PhoenixInspectionS10Report.InspectionS10CategorywiseBreakupObservations(General.GetNullableInteger(ddlFrommonth.SelectedValue), General.GetNullableInteger(ddlFromYear.SelectedValue)
                                                                    , General.GetNullableInteger(ddlTomonth.SelectedValue), General.GetNullableInteger(ddlToYear.SelectedValue), "NAV"
                                                                    , null);
        gvCategorywiseBreakupSummary.DataSource = ds;

    }

    protected void gvCategorywiseBreakupSummary_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.Text = "Yearwise";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = ddlFrommonth.SelectedItem + "," + ddlFromYear.SelectedValue + " - " + ddlTomonth.SelectedItem + "," + ddlToYear.SelectedValue;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvCategorywiseBreakupSummary.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    protected void MenuS10Report_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlFrommonth.SelectedIndex = 0;
                ddlFromYear.SelectedIndex = 0;
                ddlTomonth.SelectedIndex = 0;
                ddlToYear.SelectedIndex = 0;
                BindData();
                gvCategorywiseBreakupObs.Rebind();
                gvCategorywiseBreakupSummary.Rebind();
                gvNavAuditSummary.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlFrommonth.SelectedValue) == null)
            ucError.ErrorMessage = "From Month is required.";

        if (General.GetNullableString(ddlFromYear.SelectedValue) == null)
            ucError.ErrorMessage = "From Year is required.";

        if (General.GetNullableString(ddlTomonth.SelectedValue) == null)
            ucError.ErrorMessage = "To Month is required.";

        if (General.GetNullableString(ddlToYear.SelectedValue) == null)
            ucError.ErrorMessage = "To Year is required.";

        if (General.GetNullableString(ddlFromYear.SelectedValue) != null && General.GetNullableString(ddlToYear.SelectedValue) != null &&
            General.GetNullableInteger(ddlToYear.SelectedValue) < General.GetNullableInteger(ddlFromYear.SelectedValue))
        {
            ucError.ErrorMessage = "To Year should be greater than From Year.";
            return (!ucError.IsError);
        }

        if (General.GetNullableString(ddlFrommonth.SelectedValue) != null && General.GetNullableString(ddlTomonth.SelectedValue) != null &&
            General.GetNullableInteger(ddlToYear.SelectedValue) <= General.GetNullableInteger(ddlFromYear.SelectedValue) &&
            General.GetNullableInteger(ddlTomonth.SelectedValue) < General.GetNullableInteger(ddlFrommonth.SelectedValue))
        {
            ucError.ErrorMessage = "To Month should be greater than From Month.";
            return (!ucError.IsError);
        }
        return (!ucError.IsError);
    }
}

using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class InspectionWeeklyMeasure : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddFontAwesomeButton("../Inspection/InspectionWeeklyMeasure.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                MenuAuditSummaryNC.AccessRights = this.ViewState;
                MenuAuditSummaryNC.MenuList = toolbar.Show();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        lblGridISM.Text = PrepareSummaryData().ToString();
    }

    protected void ShowExcel()
    {
        string style = @"<style> TD { mso-number-format:\@; } </style> ";

        
        Response.AddHeader("Content-Disposition", "attachment; filename=WeeklyReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write(style);
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Weekly Report</h3></td>");
        Response.Write("</tr>");
        Response.Write("<tr></tr>");
        Response.Write("</TABLE>");
        StringBuilder sb = PrepareSummaryData();
        Response.Write(sb.ToString());
        Response.End();
    }

    protected StringBuilder PrepareSummaryData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionWeeklyReport.InspectionWeeklyMeasurereport();

    
        StringBuilder sb = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr class='rgHeader'>");

            //Printing the Header

            DataTable dtColumns = ds.Tables[0];
            foreach (DataColumn column in dtColumns.Columns)
            {
                if (column.ColumnName.Equals("Measure"))
                {
                    sb.Append("<b><td colspan='2' align='CENTER' width=10%'>");
                    sb.Append("<b>"+column.ColumnName+"</b>");
                    sb.Append("</td></b>");
                }
                else
                {
                    sb.Append("<b><td colspan='2' align='CENTER' width='5%'>");
                    sb.Append("<b>" + column.ColumnName + "</b>");
                    sb.Append("</td></b>");
                }
            }
            sb.Append("</tr>");

            //Printing the Column

            foreach (DataRow row in dtColumns.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dtColumns.Columns)
                {
                    sb.Append("<td colspan='2' align='CENTER' width='5%'>");
                    sb.Append(row[column.ColumnName.ToString()]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            sb.Append("</br>");
            sb.Append("<table cellspacing=\"0\" cellpadding=\"1\"> ");
            sb.Append("<tr></tr>");
            sb.Append("<tr></tr>");
            sb.Append("<tr>");
            sb.Append("<td colspan='2'>" + "<b><i> * Count Combination - Current week / Last week<i></b>");
            sb.Append("</td></tr>");
            sb.Append("</table>");
        }
        else
        {
            sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");
            sb.Append("<tr style=\"height:15px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
            sb.Append("<tr style=\"height:15px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");
            sb.Append("</table>");
        }
        return sb;
    }

    protected void MenuAuditSummaryNC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
}

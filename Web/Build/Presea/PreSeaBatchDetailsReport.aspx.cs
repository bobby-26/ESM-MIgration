using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaBatchDetailsReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Presea/PreSeaBatchDetailsReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../Presea/PreSeaBatchDetailsReport.aspx", "Clear Filter", "search.png", "SEARCH");
                MenuPreSeaScoreCradSummary.AccessRights = this.ViewState;
                MenuPreSeaScoreCradSummary.MenuList = toolbar.Show();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDROW", "FLDNAME", "FLDDATEOFBIRTH", "FLDINDOSNO", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDDATEOFJOINING", "FLDREMARKS" };
        string[] alCaptions = { "Sr.No", "Candidate Name", "Date of Birth", "INDoS No", "Passport No", "CDC No", "Date of Joining", "Remarks" };


        DataSet ds = PhoenixPreSeaReports.BatchDetailsReport(General.GetNullableInteger(ucBatch.SelectedBatch));

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCrew.DataSource = ds;
            gvCrew.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCrew);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }

        General.SetPrintOptions("gvCrew", "Batch Details", alCaptions, alColumns, ds);

    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                if (ucBatch.SelectedBatch.ToUpper().Equals("DUMMY") || ucBatch.SelectedBatch.Equals(""))
                {
                    ucError.ErrorMessage = "Batch is required.";
                    ucError.Visible = true;
                    return;
                }
            }

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDROW", "FLDNAME", "FLDDATEOFBIRTH", "FLDINDOSNO", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDDATEOFJOINING", "FLDREMARKS" };
        string[] alCaptions = { "Sr.No", "Candidate Name", "Date of Birth", "INDoS No", "Passport No", "CDC No", "Date of Joining", "Remarks" };

        DataSet ds = PhoenixPreSeaReports.BatchDetailsReport(General.GetNullableInteger(ucBatch.SelectedBatch));

        Response.AddHeader("Content-Disposition", "attachment; filename=Batch Details.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Batch Details Report</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
}

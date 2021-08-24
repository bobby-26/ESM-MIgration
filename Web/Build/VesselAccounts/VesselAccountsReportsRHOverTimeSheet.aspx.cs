using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsReportsRHOverTimeSheet : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportsRHOverTimeSheet.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRestHoursOverTimeSheet')", "Print Grid", "icon_print.png", "PRINT");
            MenuRestHoursOvertimeSheet.AccessRights = this.ViewState;
            MenuRestHoursOvertimeSheet.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLD01", "FLD02", "FLD03", "FLD04", "FLD05", "FLD06", "FLD07", "FLD08", "FLD09", "FLD10", "FLD11", "FLD12", "FLD13", "FLD14", "FLD15", "FLD16", "FLD17", "FLD18", "FLD19", "FLD20", "FLD21", "FLD22", "FLD23", "FLD24", "FLD25", "FLD26", "FLD27", "FLD28", "FLD29", "FLD30", "FLD31", "FLDTOTALHOURS", "FLDRATEPERHOUR", "FLDAMOUNTOVERDUE", "FLDAMOUNT", "FLDSIGNATURE" };
        string[] alCaptions = { "Employee Name", "Ratings", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "Total Overtime Hours", "Rate Per Hour", "Amount Overdue", "Signature" };

        ds = PhoenixVesselAccountsRH.RestHourOverTimeSheet(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            int.Parse(ddlMonth.SelectedValue),
            int.Parse(ddlYear.SelectedValue));

        Response.AddHeader("Content-Disposition", "attachment; filename=RestHourOverTimeSheet.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Monthly Overtime Record</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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

    protected void RestHoursOvertimeSheet_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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

    private void BindData()
    {
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLD01", "FLD02", "FLD03", "FLD04", "FLD05", "FLD06", "FLD07", "FLD08", "FLD09", "FLD10", "FLD11", "FLD12", "FLD13", "FLD14", "FLD15", "FLD16", "FLD17", "FLD18", "FLD19", "FLD20", "FLD21", "FLD22", "FLD23", "FLD24", "FLD25", "FLD26", "FLD27", "FLD28", "FLD29", "FLD30", "FLD31", "FLDTOTALHOURS", "FLDRATEPERHOUR", "FLDAMOUNTOVERDUE", "FLDAMOUNT", "FLDSIGNATURE" };
        string[] alCaptions = { "Employee Name", "Ratings", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "Total Overtime Hours", "Rate Per Hour", "Amount Overdue", "Signature" };

        DataSet ds = PhoenixVesselAccountsRH.RestHourOverTimeSheet(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
            int.Parse(ddlMonth.SelectedValue),
            int.Parse(ddlYear.SelectedValue));

        General.SetPrintOptions("gvRestHoursOverTimeSheet", "Designation Invoice Status", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRestHoursOverTimeSheet.DataSource = ds;
            gvRestHoursOverTimeSheet.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRestHoursOverTimeSheet);
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
        gv.Rows[0].Attributes["onclick"] = "";
    }
}

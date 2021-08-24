using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerModuleReport : PhoenixBasePage
{

    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvModuleList.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvModuleList.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarbuglist = new PhoenixToolbar();
                toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerModuleReport.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
                toolbarbuglist.AddImageButton("../DefectTracker/DefectTrackerModuleReport.aspx", "Search", "search.png", "SEARCH");
                MenuDefectTracker.AccessRights = this.ViewState;
                MenuDefectTracker.MenuList = toolbarbuglist.Show();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StrategyReport_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            BindData();
        }

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {

        DataSet ds = PhoenixDefectTracker.ModuleReport(General.GetNullableDateTime(txtToDate.Text));
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            gvModuleList.DataSource = dt;
            gvModuleList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvModuleList);
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

    protected void gvModuleList_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvModuleList, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExcel()
    {
        string[] alColumns = { "FLDMODULENAME", "FLDLOGGED", "FLDCOMPLETED", "FLDPENDING", "FLDPERCENT" };
        string[] alCaptions = { "Module Name", "Issues Logged", "Issues Completed", "Issues Pending", "% Completed" };

        DataSet ds = PhoenixDefectTracker.ModuleReport(General.GetNullableDateTime(txtToDate.Text));
        string date = txtToDate.Text != null ? txtToDate.Text : DateTime.Now.ToShortDateString();
        Response.AddHeader("Content-Disposition", "attachment; filename=\"ModuleReport.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Module Report</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr><tr></tr>");
        Response.Write("<td>Report Upto : </td>" + "<td>" + date + "</td>");
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
    long loggedtotal = 0,closedtotal=0,pendingtotal=0,newtotal=0,percenttotal=0,defferedtotal=0,bugtotal=0;
    protected void gvModuleList_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            loggedtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDLOGGED"));
            closedtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDCOMPLETED"));
            pendingtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDPENDING"));
            newtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDNEW"));
            defferedtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDDEFERRED"));
            bugtotal += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FLDBUGS"));
            percenttotal = (closedtotal * 100) / loggedtotal; 
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblloggedtotal = (Label)e.Row.FindControl("lblloggedTotal");
            lblloggedtotal.Text = loggedtotal.ToString();

            Label lblclosedtotal = (Label)e.Row.FindControl("lblCompletedTotal");
            lblclosedtotal.Text = closedtotal.ToString();

            Label lblpendingtotal = (Label)e.Row.FindControl("lblPendingTotal");
            lblpendingtotal.Text = pendingtotal.ToString();

            Label lblNewTotal = (Label)e.Row.FindControl("lblNewTotal");
            lblNewTotal.Text = newtotal.ToString();
             
            Label lblpercenttotal = (Label)e.Row.FindControl("lblPerCompletedTotal");
            lblpercenttotal.Text = percenttotal.ToString();

            Label lblnextphase = (Label)e.Row.FindControl("lblNextPhaseTotal");
            lblnextphase.Text = defferedtotal.ToString();

            Label lblBugsTotal = (Label)e.Row.FindControl("lblBugsTotal");
            lblBugsTotal.Text = bugtotal.ToString();
        }
    }
}

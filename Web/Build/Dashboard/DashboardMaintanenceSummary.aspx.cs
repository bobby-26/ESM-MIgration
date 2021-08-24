using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;


public partial class DashboardMaintanenceSummary : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvMaintanenceSummary.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvMaintanenceSummary.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            //PhoenixToolbar toolbarOnSigner = new PhoenixToolbar();
            //toolbarOnSigner.AddImageButton("../Dashboard/DashboardMaintanenceSummary.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            //toolbarOnSigner.AddImageLink("javascript:CallPrint('gvMaintanenceSummary')", "Print Grid", "icon_print.png", "PRINT");
            //MenuMaintanenceSummary.AccessRights = this.ViewState;
            //MenuMaintanenceSummary.MenuList = toolbarOnSigner.Show();

            if ((Request.QueryString["ModuleName"] != null) || (Request.QueryString["ModuleName"] != ""))
                ViewState["MODULENAME"] = Request.QueryString["ModuleName"].ToString();
        }
        BindDataMaintanenceSummary();   
    }

    protected void MaintanenceSummary_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }       
    }

    private void BindDataMaintanenceSummary()
    {
        string[] alColumns = { "FLDVESSELNAME	", "FLDFLEETCODE", "FLDJOBSCOUNT", "FLDREQUESTED", "FLDCANCELLED", "FLDPOSTPONED", "FLDCOMPLETED", "FLDISSUED", "FLDOVERDUE" };
        string[] alCaptions = { "VESSEL NAME", "FLEET CODE", "JOBS COUNT", "REQUESTED", "CANCELLED", "POSTPONED", "COMPLETED", "ISSUED", "OVERDUE" };

        DataSet ds = PhoenixCommonDashboard.DashboardSummaryAcrossModuleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["MODULENAME"].ToString());

        General.SetPrintOptions("gvMaintanenceSummary", "MaintanenceSummary List", alCaptions, alColumns, ds);

        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            gvMaintanenceSummary.DataSource = dt;
            gvMaintanenceSummary.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvMaintanenceSummary);
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
    protected void gvMaintanenceSummary_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
                && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvMaintanenceSummary, "Select$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void  ShowExcel()
    {

        string[] alColumns = {"FLDVESSELNAME","FLDFLEETCODE","FLDREQUISITION","FLDQUOTENOTRECEIVED","FLDQUOTERECEIVED","FLDQUOTEAWAITINGAPPROVAL","FLDPOWAITINGTOISSUE","FLDPOISSUED"};
        string[] alCaptions = { "Vessel Name", "Fleet Code", "Requisition", "Quote Not Received", "Quotereceived", "Quote awaiting Approval", "PO Waiting to Issue", "PO issued" };

        DataSet ds = PhoenixCommonDashboard.DashboardSummaryAcrossModuleList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["MODULENAME"].ToString());

        Response.AddHeader("Content-Disposition", "attachment; filename=\"MaintanenceSummary.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Maintanence Summary</h3></td>");
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
}

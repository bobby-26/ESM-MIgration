using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
public partial class CrewAppraisalSummary : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSummary.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSummary.aspx", "Export to Excel", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSummary.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewAppraisalSummary.aspx", "Refresh List", "<i class=\"fa - redo - refresh\"></i>", "REFRESH");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFielNo.Text = "";
                Rebind();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            if (CommandName.ToUpper().Equals("REFRESH"))
            {
                int n = PhoenixCrewAppraisalSummary.CrewAppraisalRatingUpdate();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOperationalSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                Rebind();


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
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCrewAppraisalSummary.CrewOperationalSummarySearch(General.GetNullableInteger(UcRank.SelectedRank), sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , gvCrew.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableString(txtFielNo.Text.Trim()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            appraisalfrom = "Appraisal Summary FROM " + ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
        }

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDFILENO", "FLDSTATUS", "FLDZONENAME", "FLDTRAININGBATCH", "FLDTOTALDATA", "FLDRESULT" };
        string[] alCaptions = { "Name", "Rank", "File No", "Status", "Zone", "Batch No", appraisalfrom, "Remarks" };

        General.SetPrintOptions("gvCrew", "Crew Appraisal Summary", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDFILENO", "FLDSTATUS", "FLDZONENAME", "FLDTRAININGBATCH", "FLDTOTALDATA", "FLDRESULT" };
        string[] alCaptions = { "Name", "Rank", "File No", "Status", "Zone", "Batch No", appraisalfrom, "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds;
        ds = PhoenixCrewAppraisalSummary.CrewOperationalSummarySearch(General.GetNullableInteger(UcRank.SelectedRank), sortdirection
                         , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                         , General.ShowRecords(null)
                         , ref iRowCount
                         , ref iTotalPageCount
                         , General.GetNullableString(txtFielNo.Text.Trim()));

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewAppraisalSummary.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<td><h3>Crew Appraisal Summary</h3></td>");
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


    string appraisalfrom;
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridEditableItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblSummaryHeader");
            
            if (lbl != null)
            {

                lbl.Text = "Appraisal Summary FROM " + drv["FLDYEAR"].ToString();
                appraisalfrom = "Appraisal Summary FROM " + drv["FLDYEAR"].ToString();
            }

            UserControlCommonToolTip ucCommonToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (ucCommonToolTip != null && (drv["FLDEMPLOYEENAME"] == null || drv["FLDEMPLOYEENAME"].ToString().Trim() == ""))
            {
                ucCommonToolTip.Visible = false;
            }

            RadLabel lblEmployeeID = (RadLabel)e.Item.FindControl("lblEmployeeID");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmpName");
            lb.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblEmployeeID.Text + "&appraisal=true&tabindex=1'); return false;");

        }

    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
}

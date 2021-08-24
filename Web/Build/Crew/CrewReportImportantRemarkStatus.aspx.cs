using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportImportantRemarkStatus : PhoenixBasePage
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

            PhoenixToolbar toolbarSub = new PhoenixToolbar();
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportImportantRemarkStatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarSub.AddFontAwesomeButton("javascript:CallPrint('gvImportantRemark')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportImportantRemarkStatus.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbarSub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ShowEmptyReport();
                gvImportantRemark.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucPrincipal.SelectedList = "";
                ucVessel.SelectedVessel = "";
                ucManager.SelectedList = "";
                ucRank.SelectedRankValue = "";
                ddlstatus.SelectedIndex = 1;
                lstPool.SelectedPool = "";
                lstZone.SelectedZoneValue = "";
                txtFileNo.Text = "";
                ddlstatus.SelectedIndex = -1;
                ShowReport();


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
                ViewState["PAGENUMBER"] = 1;
                ShowReport();

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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBATCH", "FLDPOSTEDBYNAME", "FLDPOSTEDDESCRIPTION", "FLDPOSTEDDATE", "FLDACTIONBYNAME", "FLDACTIONDESCRIPTION", "FLDACTIONDATE" };
        string[] alCaptions = { "File No", "Name", "Rank", "Batch", "Posted By", "Comments", "Posted Date", "Action By", "Comments", "Action Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.ImportantRemarkStatus(
                (ucManager.SelectedList) == "" ? null : General.GetNullableString(ucManager.SelectedList),
                (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList),
                (ucVessel.SelectedVessel) == "" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                (ucRank.selectedlist) == "" ? null : General.GetNullableString(ucRank.selectedlist),
                6,
                (ddlstatus.SelectedValue) == "" ? 1 : General.GetNullableInteger(ddlstatus.SelectedValue),
                General.GetNullableString(txtFileNo.Text),
                General.GetNullableString(lstPool.SelectedPool), General.GetNullableString(lstZone.selectedlist),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Important_Remark_Status.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Important Remark Status </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBATCH", "FLDPOSTEDBYNAME", "FLDPOSTEDDESCRIPTION", "FLDPOSTEDDATE", "FLDACTIONBYNAME", "FLDACTIONDESCRIPTION", "FLDACTIONDATE" };
        string[] alCaptions = { "File No", "Name", "Rank", "Batch", "Posted By", "Comments", "Posted Date", "Action By", "Comments", "Action Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.ImportantRemarkStatus(
                (ucManager.SelectedList) == "" ? null : General.GetNullableString(ucManager.SelectedList),
                (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList),
                (ucVessel.SelectedVessel) == "" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                (ucRank.selectedlist) == "" ? null : General.GetNullableString(ucRank.selectedlist),
                6,
                (ddlstatus.SelectedValue) == "" ? 1 : General.GetNullableInteger(ddlstatus.SelectedValue),
                General.GetNullableString(txtFileNo.Text),
                General.GetNullableString(lstPool.SelectedPool), General.GetNullableString(lstZone.selectedlist),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvImportantRemark.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvImportantRemark", "Important Remark Status", alCaptions, alColumns, ds);

        gvImportantRemark.DataSource = ds;
        gvImportantRemark.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void ShowEmptyReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDBATCH", "FLDPOSTEDBYNAME", "FLDPOSTEDDESCRIPTION", "FLDPOSTEDDATE", "FLDACTIONBYNAME", "FLDACTIONDESCRIPTION", "FLDACTIONDATE" };
        string[] alCaptions = { "Name", "Rank", "Batch", "Posted By", "Comments", "Posted Date", "Action By", "Comments", "Action Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.ImportantRemarkStatus(
                null,
                null,
                null,
                null,
                6,
                null, // <--(status is null) this null value is the reason for no records found..
                null,
                null,
                null,
                sortexpression,
                sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvImportantRemark", "Important Remark Status", alCaptions, alColumns, ds);

        gvImportantRemark.DataSource = ds;
        gvImportantRemark.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvImportantRemark_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvImportantRemark.CurrentPageIndex + 1;

        ShowReport();
    }

    protected void gvImportantRemark_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            RadLabel lbtnpoComment = (RadLabel)e.Item.FindControl("lblPostedComments");
            UserControlToolTip uctPosted = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (lbtnpoComment != null && uctPosted != null)
            {
                lbtnpoComment.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctPosted.ToolTip + "', 'visible');");
                lbtnpoComment.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctPosted.ToolTip + "', 'hidden');");
            }

            RadLabel lbtncomment = (RadLabel)e.Item.FindControl("lblActionComment");
            UserControlToolTip uctAction = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress1");
            if (lbtncomment != null && uctAction != null)
            {
                lbtncomment.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctAction.ToolTip + "', 'visible');");
                lbtncomment.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctAction.ToolTip + "', 'hidden');");
            }

            RadLabel lblpostedcomment = (RadLabel)e.Item.FindControl("lblPostedComments");
            UserControlToolTip ucpostedcomment = (UserControlToolTip)e.Item.FindControl("ucToolTipPostedComment");
            ucpostedcomment.Position = ToolTipPosition.TopCenter;
            ucpostedcomment.TargetControlId = lblpostedcomment.ClientID;

            RadLabel lblactioncomment = (RadLabel)e.Item.FindControl("lblActionComment");
            UserControlToolTip ucactioncomment = (UserControlToolTip)e.Item.FindControl("ucToolTipActionComment");
            ucactioncomment.Position = ToolTipPosition.TopCenter;
            ucactioncomment.TargetControlId = lblactioncomment.ClientID;
        }
        if (e.Item is GridEditableItem)
        {

            RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            lb.Attributes.Add("onclick", "javascript:Openpopup('CodeHelp1','','CrewPersonalGeneral.aspx?empid=" + lblCrewId.Text + "&familyid=" + "" + "&showimportantremarks=1'); return false;");

        }

    }

    protected void gvImportantRemark_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvImportantRemark_SortCommand(object sender, GridSortCommandEventArgs e)
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


}

using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportFamilyNOK : PhoenixBasePage
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
            toolbar1.AddButton("Supernumarary Format2", "FORMAT2", ToolBarDirection.Right);
            toolbar1.AddButton("Supernumarary Format1", "FORMAT1", ToolBarDirection.Right);

            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbar1.Show();
            MenuReport.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarSub = new PhoenixToolbar();
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportFamilyNOK.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarSub.AddFontAwesomeButton("javascript:CallPrint('gvSupernumarary')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarSub.AddFontAwesomeButton("../Crew/CrewReportFamilyNOK.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbarSub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                rblFormats.SelectedIndex = 1;
                gvSupernumarary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvSupernumarary.SelectedIndexes.Clear();
        gvSupernumarary.EditIndexes.Clear();
        gvSupernumarary.DataSource = null;
        gvSupernumarary.Rebind();
    }
    protected void MenuReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("CrewReportSupernumararyDetails.aspx", true);
                MenuReport.SelectedMenuIndex = 1;
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
                ucPrincipal.SelectedAddress = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                lstBatch.SelectedList = "";
                ucAsOndate.Text = "";
                ucRank.SelectedRankValue = "";
                ucZone.selectedlist = "";
                ucRelationship.selectedlist = "";
                ddlEmployeeStatus.SelectedIndex = 0;
                Rebind();

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
                Rebind();

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
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANK", "FLDDATEOFBIRTH", "FLDRELATIONSHIP", "FLDEMPLYEESTATUS" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", " DOB of Member", "Relation", "Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportMonthlyCrewChange.FamilyNOKDetailsReport(
                        General.GetNullableInteger(ucPrincipal.SelectedAddress),
                        General.GetNullableString(lstBatch.SelectedList),
                        General.GetNullableString(ucZone.selectedlist),
                        General.GetNullableString(ucPool.SelectedPool),
                        General.GetNullableString(ucRank.selectedlist),
                        General.GetNullableDateTime(ucAsOndate.Text),
                        General.GetNullableInteger(ddlEmployeeStatus.SelectedValue),
                        General.GetNullableString(ucRelationship.selectedlist),
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount
                        , byte.Parse(rblPrinciple.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=Supernumarary_FamilyNOK_Details.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Supernumarary Details</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>As on Date:" + ucAsOndate.Text + "</td></tr>");
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
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANK", "FLDDATEOFBIRTH", "FLDRELATIONSHIP", "FLDEMPLYEESTATUS" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", " DOB of Member", "Relation", "Status" };

        ds = PhoenixCrewReportMonthlyCrewChange.FamilyNOKDetailsReport(
                General.GetNullableInteger(ucPrincipal.SelectedAddress),
                General.GetNullableString(lstBatch.SelectedList),
                General.GetNullableString(ucZone.selectedlist),
                General.GetNullableString(ucPool.SelectedPool),
                General.GetNullableString(ucRank.selectedlist),
                General.GetNullableDateTime(ucAsOndate.Text),
                General.GetNullableInteger(ddlEmployeeStatus.SelectedValue),
                General.GetNullableString(ucRelationship.selectedlist),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvSupernumarary.PageSize,
                ref iRowCount,
                ref iTotalPageCount
                , byte.Parse(rblPrinciple.SelectedValue));

        General.SetPrintOptions("gvSupernumarary", "Family NOK Details", alCaptions, alColumns, ds);

        gvSupernumarary.DataSource = ds;
        gvSupernumarary.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSupernumarary_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpid");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");
        }

    }

    protected void gvSupernumarary_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvSupernumarary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSupernumarary.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvSupernumarary_ItemCommand(object sender, GridCommandEventArgs e)
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

    public void rblFormats_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblFormats.SelectedIndex == 0)
        {
            Response.Redirect("../Crew/CrewReportSupernumararyDetails.aspx", true);
        }
        else
        {
            Response.Redirect("../Crew/CrewReportFamilyNOK.aspx", true);
        }
    }
    protected void rblPrinciple_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPrinciple.SelectedValue == "0")
        {
            ucPrincipal.AddressList = PhoenixRegistersAddress.ListAddress("126");
            lblPrincipal.Text = "Manager";
        }
        else
        {
            ucPrincipal.AddressList = PhoenixRegistersAddress.ListAddress("128");
            lblPrincipal.Text = "Principal";
        }
    }
}

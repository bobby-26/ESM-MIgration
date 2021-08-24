using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsEx_HandInteraction : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsEx-HandInteraction.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsEx-HandInteraction.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadComboBoxItem li = new RadComboBoxItem("--All--", "DUMMY");
                ddlStatus.Items.Insert(0, li);
                ddlStatus.AppendDataBoundItems = true;
                ddlStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 54, 1, "ONB,ONL");
                ddlStatus.DataBind();
                chkIncludepastexp.Checked = true;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                if (!IsValidFilter(ucLastContactedAfter.Text, ucLastContactedPrior.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                ShowReport();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string LastContactedPrior, string LastContactedAfter)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (LastContactedPrior == null && LastContactedAfter == null)
            ucError.ErrorMessage = " Either Last contact Prior or Last contact After is mandatory";

        return (!ucError.IsError);
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
                ucRank.SelectedRankValue = "";
                ucZone.SelectedZoneValue = "";
                ddlVesselType.SelectedVesseltype = "";
                ddlStatus.SelectedIndex = 0;
                chkIncludepastexp.Checked = false;
                ucDOAFrom.Text = "";
                ucDOATo.Text = "";
                ucLastContactedAfter.Text = "";
                ucLastContactedPrior.Text = "";
                ucPrinicipal.SelectedAddress = "";
                ucPool.SelectedPool = "";
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH", "FLDZONE", "FLDNAME", "FLDRANKCODE", "FLDONBOARDVESSELNAME", "FLDONBOARDSIGNONDATE", "FLDTYPEDESCRIPTIONONBOARD", "FLDLASTSIGNOFFVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDTYPEDESCRIPTIONLASTSIGNOFFVESSEL", "FLDLASTREMARKS", "FLDLASTCONTACT", "FLDLASTCONTACTDATE", "FLDDOA" };
        string[] alCaptions = { "S.No", "File No.", "Batch", "Zone", "Name", "Rank", "OnBoard Vessel Name", "OnBoard Vessel Date", "OnBoard Vessel Type", "Last Sign Off Vessel Name", "Last Sign Off Date", "Last Sign Off Vessel Type", "Last Remarks", "Last Contact By", "Last Contact On", "DOA" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewReportsNotContacted.CrewNotContactedReport(
                  General.GetNullableString(ucRank.selectedlist),
                  General.GetNullableString(ucZone.selectedlist),
                  General.GetNullableString(ddlVesselType.SelectedVesseltype),
                  int.Parse(chkIncludepastexp.Checked ? "1" : "0"),
                  General.GetNullableDateTime(ucDOAFrom.Text),
                  General.GetNullableDateTime(ucDOATo.Text),
                  General.GetNullableDateTime(ucLastContactedPrior.Text),
                  General.GetNullableDateTime(ucLastContactedAfter.Text),
                  2,
                  General.GetNullableInteger(ddlStatus.SelectedValue),
                  sortexpression, sortdirection,
                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount,
                General.GetNullableString(ucPool.SelectedPool),
                General.GetNullableInteger(ucPrinicipal.SelectedAddress));


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewReportsEx-HandInteraction.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Seafarers Not Contacted Report</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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


        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDBATCH", "FLDZONE", "FLDNAME", "FLDRANKCODE", "FLDONBOARDVESSELNAME", "FLDONBOARDSIGNONDATE", "FLDTYPEDESCRIPTIONONBOARD", "FLDLASTSIGNOFFVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDTYPEDESCRIPTIONLASTSIGNOFFVESSEL", "FLDLASTREMARKS", "FLDLASTCONTACT", "FLDLASTCONTACTDATE", "FLDDOA" };
        string[] alCaptions = { "S.No", "File No.", "Batch", "Zone", "Name", "Rank", "OnBoard Vessel Name", "OnBoard Vessel Date", "OnBoard Vessel Type", "Last Sign Off Vessel Name", "Last Sign Off Date", "Last Sign Off Vessel Type", "Last Remarks", "Last Contact By", "Last Contact On", "DOA" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ViewState["STATUS"] = ddlStatus.SelectedValue;

        if (ViewState["STATUS"] != null)
        {
            if (ViewState["STATUS"].ToString() == "220") //on board
            {
                gvCrew.Columns[8].Visible = false;
                gvCrew.Columns[9].Visible = false;
                gvCrew.Columns[11].Visible = false;
            }
            else if (ViewState["STATUS"].ToString() == "221") //on leave
            {
                gvCrew.Columns[6].Visible = false;
                gvCrew.Columns[7].Visible = false;
                gvCrew.Columns[10].Visible = false;
            }

            else
            {
                gvCrew.Columns[6].Visible = true;
                gvCrew.Columns[7].Visible = true;
                gvCrew.Columns[10].Visible = true;
                gvCrew.Columns[8].Visible = true;
                gvCrew.Columns[9].Visible = true;
                gvCrew.Columns[11].Visible = true;


            }
        }


        DataSet ds = new DataSet();
        ds = PhoenixCrewReportsNotContacted.CrewNotContactedReport(
                General.GetNullableString(ucRank.selectedlist),
                General.GetNullableString(ucZone.selectedlist),
                General.GetNullableString(ddlVesselType.SelectedVesseltype),
                int.Parse(chkIncludepastexp.Checked ? "1" : "0"),
                General.GetNullableDateTime(ucDOAFrom.Text),
                General.GetNullableDateTime(ucDOATo.Text),
                General.GetNullableDateTime(ucLastContactedPrior.Text),
                General.GetNullableDateTime(ucLastContactedAfter.Text),
                2
                , General.GetNullableInteger(ddlStatus.SelectedValue)
                , sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount,
                General.GetNullableString(ucPool.SelectedPool),
                General.GetNullableInteger(ucPrinicipal.SelectedAddress));

        General.SetPrintOptions("gvCrew", "Seafarers Not Contacted Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 6;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "OnBoard Vessel";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Last SignOff Vessel ";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Last";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            gvCrew.Controls[0].Controls.AddAt(0, HeaderGridRow);

            GridViewRow row1 = ((GridViewRow)gvCrew.Controls[0].Controls[0]);
            row1.Attributes.Add("style", "position:static");

        }
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {



            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            RadLabel empcode = (RadLabel)e.Item.FindControl("lblEmpCode");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            if (empcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            }


            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLeaveRemarks");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}


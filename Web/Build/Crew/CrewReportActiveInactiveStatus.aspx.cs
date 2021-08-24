using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportActiveInactiveStatus : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportActiveInactiveStatus.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportActiveInactiveStatus.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;


                lstInActiveReasons.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 53, 1, "MDL,LSL,LFT,DTH,DST,EXM,TSP,TTO");
                lstInActiveReasons.DataBind();

                lstInActiveReasons.Items.Insert(0, "--Select--");

                rbtnActive.SelectedValue = "1";
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
                 Response.Redirect("../Crew/CrewReportActiveInactiveStatus.aspx");
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
                if (!IsValidFilter(ucDateFrom.Text, ucDateTo.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    ShowReport();

                }
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
        string[] alColumns = { "FLDROW", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKPOSTEDNAME", "FLDSAILEDRANK", "FLDBATCHNO", "FLDLASTSIGNOFFDATE", "FLDLASTVESSELNAME", "FLDSIGNONDATE", "FLDPRESENTVESSEL", "FLDCONTRACTTENURE", "FLDTTLSERVICE", "FLDSTATUS", "FLDNTBRREASON", "FLDDATEOFJOINING", "FLDDATE", "FLDUSER" };
        string[] alCaptions = { "S.No.", "FileNo", "Name", "Present Rank", "Sailed Rank", "Batch", "Sign-Off Date", "Last Vessel", "Sign-On Date", "Present Vessel", "No.of Tenures", "Total Service", "Status", "Inactive/NTBR Reason", "Join Date", "Active/Inactive/NTBR Date", "User" };
        string[] filtercolumns = { "FLDROW", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKPOSTEDNAME", "FLDBATCHNO", "FLDLASTSIGNOFFDATE", "FLDLASTVESSELNAME", "FLDCONTRACTTENURE", "FLDTTLSERVICE", "FLDSTATUS", "FLDNTBRREASON", "FLDDATEOFJOINING", "FLDDATE", "FLDUSER" };
        string[] filtercaptions = { "S.No.", "FileNo", "Name", "Rank", "Batch", "Sign-Off Date", "Last Vessel", "No.of Tenures", "Total Service", "Status", "Inactive/NTBR Reason", "Join Date", "Inactive/NTBR Date", "User" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());



        ds = PhoenixCrewActiveInactiveStatusReport.CrewActiveInactiveStatusReport(General.GetNullableDateTime(ucDateFrom.Text)
                                                                                 , General.GetNullableDateTime(ucDateTo.Text)
                                                                                 , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                                 , (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList)
                                                                                 , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                                 , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                                 , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                                 , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                                                                 , (InActiveReasonsSelectedList()) == "" ? null : General.GetNullableString(InActiveReasonsSelectedList())
                                                                                 , General.GetNullableString(txtFileNo.Text)
                                                                                 , General.GetNullableInteger(rbtnActive.SelectedValue)
                                                                                 , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                                                 , 1
                                                                                 , iRowCount
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , sortexpression, sortdirection);

        Response.AddHeader("Content-Disposition", "attachment; filename=SeafarersActiveInactiveStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");

        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
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
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKPOSTEDNAME", "FLDSAILEDRANK", "FLDBATCHNO", "FLDLASTSIGNOFFDATE", "FLDLASTVESSELNAME", "FLDSIGNONDATE", "FLDPRESENTVESSEL", "FLDCONTRACTTENURE", "FLDTTLSERVICE", "FLDSTATUS", "FLDNTBRREASON", "FLDDATEOFJOINING", "FLDDATE", "FLDUSER" };
        string[] alCaptions = { "Sl.No", "FileNo", "Name", "Present Rank", "Sailed Rank", "Batch", "Sign-Off Date", "Last Vessel", "Sign-On Date", "Present Vessel", "No.of Tenures", "Total Service", "Status", "Inactive/NTBR Reason", "Join Date", "Active/Inactive/NTBR Date", "User" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



        ds = PhoenixCrewActiveInactiveStatusReport.CrewActiveInactiveStatusReport(General.GetNullableDateTime(ucDateFrom.Text)
                                                                                 , General.GetNullableDateTime(ucDateTo.Text)
                                                                                 , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                                 , (ucBatchList.SelectedList) == "," ? null : General.GetNullableString(ucBatchList.SelectedList)
                                                                                 , (ucZone.selectedlist) == "Dummy" ? null : General.GetNullableString(ucZone.selectedlist)
                                                                                 , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                                 , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                                 , (ucPrincipal.SelectedList) == "" ? null : General.GetNullableString(ucPrincipal.SelectedList)
                                                                                 , (InActiveReasonsSelectedList()) == "" ? null : General.GetNullableString(InActiveReasonsSelectedList())
                                                                                 , General.GetNullableString(txtFileNo.Text)
                                                                                 , General.GetNullableInteger(rbtnActive.SelectedValue)
                                                                                 , General.GetNullableInteger(ddlCategory.SelectedValue)
                                                                                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                 , gvCrew.PageSize
                                                                                 , ref iRowCount
                                                                                 , ref iTotalPageCount
                                                                                 , sortexpression, sortdirection);

        General.SetPrintOptions("gvCrew", "Crew Active Inactive Status", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblReasonID = (RadLabel)e.Item.FindControl("lblReasonID");
            if (lblReasonID.Text.Trim() != "933")
                lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
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

    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);

    }



    private string InActiveReasonsSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstInActiveReasons.Items)
        {
            if (item.Selected == true)
            {
                if (item.Value != "--Select--")
                {
                    strlist.Append(item.Value.ToString());
                }
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }

    protected void rbtnActive_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        if (rbtnActive.SelectedValue == "2")
        {
            lstInActiveReasons.Visible = true;
            lblInActiveReasons.Visible = true;

            ucNTBRReason.Visible = false;
            lblNtbrReasons.Visible = false;

            ucNTBRReason.selectedlist = "";
        }
        if (rbtnActive.SelectedValue == "3")
        {
            ucNTBRReason.Visible = true;
            lblNtbrReasons.Visible = true;

            lstInActiveReasons.Visible = false;
            lblInActiveReasons.Visible = false;

            lstInActiveReasons.SelectedIndex = -1;
        }

        if (rbtnActive.SelectedValue == "1" || rbtnActive.SelectedValue == "4")
        {
            ucNTBRReason.Visible = false;
            lblNtbrReasons.Visible = false;

            ucNTBRReason.selectedlist = "";

            lstInActiveReasons.Visible = false;
            lblInActiveReasons.Visible = false;

            lstInActiveReasons.SelectedIndex = -1;
        }
        ShowReport();

    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }
}

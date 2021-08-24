using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportCorrespondence : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbartop = new PhoenixToolbar();
            toolbartop.AddButton("Corres", "CORRESPONDENCE", ToolBarDirection.Right);
            toolbartop.AddButton("Other Doc", "OTHERDOCUMENT", ToolBarDirection.Right);

            MenuReports.AccessRights = this.ViewState;
            MenuReports.MenuList = toolbartop.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportCorrespondence.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportCorrespondence.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuSubReport.AccessRights = this.ViewState;
            MenuSubReport.MenuList = toolbar.Show();
            MenuReports.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                lstCorrespondenceType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 11);
                lstCorrespondenceType.DataBind();
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
                ViewState["SHOWREPORT"] = null;
                ucPool.SelectedPool = "";
                ucVesselType.SelectedVesseltype = "";
                lstCorrespondenceType.Items.Clear();
                lstCorrespondenceType.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 11);
                lstCorrespondenceType.DataBind();
                lstCorrespondenceType.SelectedIndex = -1;
                ucRank.selectedlist = "";
                ucSignOnFromDate.Text = "";
                ucSignOnToDate.Text = "";
                ucSignOffFromDate.Text = "";
                ucSignOffToDate.Text = "";
                ucFromDate.Text = "";
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ucSignOnFromDate.Text = "";
                ucSignOnToDate.Text = "";
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenuSubReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text, ucSignOnFromDate.Text, ucSignOnToDate.Text, ucSignOffFromDate.Text, ucSignOffToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
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

            if (CommandName.ToUpper().Equals("OTHERDOCUMENT"))
            {
                Response.Redirect("../Crew/CrewReportOtherDocument.aspx", false);
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDVESSELNAME", "FLDSIGNONDATE",
                                                "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE","FLDSUBJECT","FLDCORRESPONDENCETYPE","FLDCREATEDDATE" };
        string[] alCaptions = { "Sl No", "Emp Code", "Name", "Rank", "Vsl on", "S/on Date", "Last Vsl", " S/Off Date", "Subject", "Corres Type", "Date Entered" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstCorrespondenceType.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }

        ds = PhoenixCrewReports.CrewCorrespondenceReport(
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                        General.GetNullableString(strlist.ToString()),
                        General.GetNullableDateTime(ucSignOnFromDate.Text),
                        General.GetNullableDateTime(ucSignOnToDate.Text),
                        General.GetNullableDateTime(ucSignOffFromDate.Text),
                        General.GetNullableDateTime(ucSignOffToDate.Text),
                        null,
                        General.GetNullableDateTime(ucFromDate.Text),
                        General.GetNullableDateTime(ucToDate.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount
                        , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist));


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewReportCorrespondence.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Crew Report Other Document</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> From Date:</b>" + ucFromDate.Text + "</td>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> To Date: </b>" + ucToDate.Text + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> Signon From Date:</b>" + ucSignOnFromDate.Text + "</td>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> Signon To Date: </b>" + ucSignOnToDate.Text + "</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> Signoff From Date:</b>" + ucSignOffFromDate.Text + "</td>");
        Response.Write("<td colspan ='4' style='font-family:Arial; font-size:10px;' align='left'><b> Signoff To Date: </b>" + ucSignOffToDate.Text + "</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDVESSELNAME", "FLDSIGNONDATE",
                                                "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE","FLDSUBJECT","FLDCORRESPONDENCETYPE","FLDCREATEDDATE" };
        string[] alCaptions = { "Sl No", "Emp Code", "Name", "Rank", "Vsl on", "S/on Date", "Last Vsl", " S/Off Date", "Subject", "Corres Type", "Date Entered" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstCorrespondenceType.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }

        DataSet ds = PhoenixCrewReports.CrewCorrespondenceReport(
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucPool.SelectedPool.ToString()) == "Dummy" ? null : General.GetNullableString(ucPool.SelectedPool.ToString()),
                        General.GetNullableString(strlist.ToString()),
                        General.GetNullableDateTime(ucSignOnFromDate.Text),
                        General.GetNullableDateTime(ucSignOnToDate.Text),
                        General.GetNullableDateTime(ucSignOffFromDate.Text),
                        General.GetNullableDateTime(ucSignOffToDate.Text),
                        null,
                        General.GetNullableDateTime(ucFromDate.Text),
                        General.GetNullableDateTime(ucToDate.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvCrew.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount
                        , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist));



        General.SetPrintOptions("gvCrew", "Employee Correspondence", alCaptions, alColumns, ds);

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
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
        }

    }
    public bool IsValidFilter(string fromdate, string todate, string SignOnfromdate, string SignOntodate, string SignOfffromdate, string SignOfftodate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";
        if ((General.GetNullableDateTime(SignOnfromdate) == null && General.GetNullableDateTime(SignOntodate) == null) &&
            (General.GetNullableDateTime(SignOfffromdate) == null && General.GetNullableDateTime(SignOfftodate) == null) &&
            (General.GetNullableDateTime(fromdate) == null && General.GetNullableDateTime(todate) == null))
        {
            ucError.ErrorMessage = "Issued Between Dates is Mandatory";
        }
        else if ((General.GetNullableDateTime(SignOnfromdate) != null && General.GetNullableDateTime(SignOntodate) == null) ||
            (General.GetNullableDateTime(SignOnfromdate) == null && General.GetNullableDateTime(SignOntodate) != null))
        {
            ucError.ErrorMessage = "Both From and To Sign on Dates is Mandatory";
        }
        else if ((General.GetNullableDateTime(SignOfffromdate) != null && General.GetNullableDateTime(SignOfftodate) == null) ||
            (General.GetNullableDateTime(SignOfffromdate) == null && General.GetNullableDateTime(SignOfftodate) != null))
        {
            ucError.ErrorMessage = "Both From and To Sign off Dates is Mandatory";
        }

        else if (DateTime.TryParse(SignOnfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Signon Date should be earlier than current date";
        }
        else if (DateTime.TryParse(SignOfffromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Sign off Date should be earlier than current date";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Issue Date should be earlier than current date";
        }
        else if (!string.IsNullOrEmpty(SignOnfromdate)
            && DateTime.TryParse(SignOntodate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(SignOnfromdate)) < 0)
        {
            ucError.ErrorMessage = "Sign On To Date should be later than 'From Date'";
        }
        else if (!string.IsNullOrEmpty(SignOfffromdate)
            && DateTime.TryParse(SignOfftodate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(SignOfffromdate)) < 0)
        {
            ucError.ErrorMessage = "Sign Off To Date should be later than 'From Date'";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "Issued To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
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

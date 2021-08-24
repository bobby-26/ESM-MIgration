using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PhoenixCrewQuickReports;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewQuickReportLicence : PhoenixBasePage
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

            toolbar1.AddFontAwesomeButton("../Crew/CrewQuickReportLicence.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewQuickReportLicence.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWREPORT"] = null;
                ucSignOnToDate.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 1, null, null);
                ds.Tables[0].Merge(PhoenixRegistersDocumentLicence.ListDocumentLicence(null, null, 3, null, null).Tables[0]);
                lstLicence.DataSource = ds;
                lstLicence.DataBind();
                lstLicence.Items.Insert(0, new RadListBoxItem("--Select--", ""));
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //  ShowReport();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string LicenceSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstLicence.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
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
                ucNationality1.SelectedNationalityValue = "";

                ucSignOnFromDate.Text = "";
                ucSignOnToDate.Text = DateTime.Now.ToShortDateString();
                ViewState["PAGENUMBER"] = 1;
                gvCrew.CurrentPageIndex = 0;
                ShowReport();
                gvCrew.Rebind();
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
                if (!IsValidFilter(ucSignOnFromDate.Text, ucSignOnToDate.Text))
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDLASTNOTESDESCRIPTION", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSTATUS", "FLDSEAFARERREQUIREMENT", "FLDLICENCENUMBER", "FLDLICENCE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDNATIONALITY" };
        string[] alCaptions = { "SNo.", "File No.", "	Name", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Remarks", "Availability", "Last Contact", "Status", "Requirement", "Licence No.", "Licence", "Date of Issue", "Date of Expire", "Place Of Issue", "Nationality" };
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewQuickReports.CrewQuickLicence(General.GetNullableString(txtname.Text.Trim())
                                                            , General.GetNullableString(txtFileNo.Text.Trim())
                                                            , General.GetNullableString(LicenceSelectedList())
                                                            , (ucNationality1.SelectedNationalityValue.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedNationalityValue.ToString())
                                                            , General.GetNullableDateTime(ucSignOnFromDate.Text)
                                                            , General.GetNullableDateTime(ucSignOnToDate.Text)
                                                            , 1
                                                            , iRowCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LicenceQuickReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Licence</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSEAFARERREQUIREMENT", "FLDLASTNOTESDESCRIPTION", "FLDSTATUS", "FLDLICENCENUMBER", "FLDLICENCE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDNATIONALITY" };
        string[] alCaptions = { "SNo.", "File No.", "	Name", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Availability", "Last Contact", "Requirement", "Remarks", "Status", "Licence No.", "Licence", "Date of Issue", "Date of Expire", "Place Of Issue", "Nationality" };

        DataSet ds = PhoenixCrewQuickReports.CrewQuickLicence(General.GetNullableString(txtname.Text.Trim())
                                                                , General.GetNullableString(txtFileNo.Text.Trim())
                                                                , General.GetNullableString(LicenceSelectedList())
                                                                , (ucNationality1.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucNationality1.SelectedList.ToString())
                                                                , General.GetNullableDateTime(ucSignOnFromDate.Text)
                                                                , General.GetNullableDateTime(ucSignOnToDate.Text)
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvCrew.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Licence", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        if (iRowCount > 0)
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        else
            ViewState["ROWSINGRIDVIEW"] = 0;
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //Label empid = (Label)e.Row.FindControl("lblEmpNo");
        //LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
        //lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

        //Image lbtn = (Image)e.Row.FindControl("imgToolTip");
        //UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTip");
        //lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
        //lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    }
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "Date is required";
        }
        if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
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
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
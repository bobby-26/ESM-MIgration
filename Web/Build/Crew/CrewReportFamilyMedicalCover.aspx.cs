using System;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Configuration;
using Telerik.Web.UI;
public partial class CrewReportFamilyMedicalCover : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilterMedical.AccessRights = this.ViewState;
            MenuReportsFilterMedical.MenuList = toolbar2.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportFamilyMedicalCover.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvMedicalInsurance')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportFamilyMedicalCover.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                gvMedicalInsurance.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvMedicalInsurance.SelectedIndexes.Clear();
        gvMedicalInsurance.EditIndexes.Clear();
        gvMedicalInsurance.DataSource = null;
        gvMedicalInsurance.Rebind();
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
                ucNationality.SelectedNationalityValue = "";
                ucRank.selectedlist = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucZone.selectedlist = "";
                ucManager.SelectedList = "";
                ucVessel.SelectedVessel = "";
                ucPrincipal.SelectedList = "";
                ddlSelectFrom.SelectedHard = "";
                ddlactiveyn.SelectedValue = "Dummy";
                ucDate.Text = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ReportsMedicalFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucDate.Text, ucDate1.Text))
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

    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        //if (ranklist.Equals("") || ranklist.Equals("Dummy"))
        //{
        //    ucError.ErrorMessage = "Rank is Required";
        //}
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDPRINCIPAL", "FLDVESSELTYPECODE", "FLDSIGNOFFDATE", "FLDACTIVE", "FLDSTATUS", "FLDFAMILYNAME", "FLDFAMILYDATEOFBIRTH", "FLDRELATIONSHIP", "FLDEMPLOYEESTATUS", "FLDZONE" };
        string[] alCaptions = { "Sr.No", "Emp Code", "Name", "Rank", "Vessel", "Principal", "Type", "Sign Off Date", "Active/Inactive", "Status", "Family Member's Name", "Family Member's D.O.B", "Family Member's Relation", "Medical Cover", "Zone" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;
        ucNationality.SelectedNationalityValue = ucNationality.SelectedNationalityValue.ToString().Contains("Dummy,") ? ucNationality.SelectedNationalityValue.ToString().Replace("Dummy,", "") : ucNationality.SelectedNationalityValue;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;

        ds = PhoenixCrewMedicalInsuranceReport.CrewMedicalInsuranceReportSearch(General.GetNullableDateTime(ucDate.Text)
            , General.GetNullableDateTime(ucDate1.Text)
            , General.GetNullableString(ucPrincipal.SelectedList)
            , General.GetNullableString(ucNationality.SelectedList)
            , General.GetNullableString(ucRank.selectedlist)
            , General.GetNullableString(ucVesselType.SelectedVesseltype)
            , General.GetNullableString(ucVessel.SelectedVessel)
            , General.GetNullableString(ucZone.selectedlist)
            , General.GetNullableInteger(ddlSelectFrom.SelectedHard)
            , General.GetNullableInteger(ddlactiveyn.SelectedValue)
            , General.GetNullableString(ucManager.SelectedList)
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        string fromdates = ucDate.Text;
        string todatess = ucDate1.Text;

        Response.AddHeader("Content-Disposition", "attachment; filename=Family_Medical_Cover.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Family Medical Cover</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
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
                Response.Write("<td align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.Write(string.IsNullOrEmpty(ConfigurationManager.AppSettings["softwarename"]) ? "" : ConfigurationManager.AppSettings["softwarename"]);
        Response.End();
    }
    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDPRINCIPAL", "FLDVESSELTYPECODE", "FLDSIGNOFFDATE", "FLDACTIVE", "FLDSTATUS", "FLDFAMILYNAME", "FLDFAMILYDATEOFBIRTH", "FLDRELATIONSHIP", "FLDEMPLOYEESTATUS", "FLDZONE" };
            string[] alCaptions = { "Sr.No", "Emp Code", "Name", "Rank", "Vessel", "Principal", "Type", "Sign Off Date", "Active/Inactive", "Status", "Family Member's Name", "Family Member's D.O.B", "Family Member's Relation", "Medical Cover", "Zone" };


            ucPrincipal.SelectedList = ucPrincipal.SelectedList.ToString().Contains("Dummy,") ? ucPrincipal.SelectedList.ToString().Replace("Dummy,", "") : ucPrincipal.SelectedList;
            ucNationality.SelectedNationalityValue = ucNationality.SelectedNationalityValue.ToString().Contains("Dummy,") ? ucNationality.SelectedNationalityValue.ToString().Replace("Dummy,", "") : ucNationality.SelectedNationalityValue;
            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
            ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
            ucManager.SelectedList = ucManager.SelectedList.ToString().Contains("Dummy,") ? ucManager.SelectedList.ToString().Replace("Dummy,", "") : ucManager.SelectedList;

            DataSet ds = new DataSet();

            //if (!IsPostBack)
            //{
            //    ds = PhoenixCrewMedicalInsuranceReport.CrewMedicalInsuranceReportSearch(null, null, null, null, null, null, null, null, null, null, null
            //        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //        , General.ShowRecords(null)
            //        , ref iRowCount
            //        , ref iTotalPageCount);

            //}
            //else
            //{
            //if (ucRank.selectedlist.ToString().Equals(""))
            //{
            //    ds = PhoenixCrewMedicalInsuranceReport.CrewMedicalInsuranceReportSearch(null, null, null, null, null, null, null, null, null, null, null
            //        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            //        , General.ShowRecords(null)
            //        , ref iRowCount
            //        , ref iTotalPageCount);
            //}
            //else
            //{
            ds = PhoenixCrewMedicalInsuranceReport.CrewMedicalInsuranceReportSearch(General.GetNullableDateTime(ucDate.Text)
                , General.GetNullableDateTime(ucDate1.Text)
                , General.GetNullableString(ucPrincipal.SelectedList)
                , General.GetNullableString(ucNationality.SelectedList)
                , General.GetNullableString(ucRank.selectedlist)
                , General.GetNullableString(ucVesselType.SelectedVesseltype)
                , General.GetNullableString(ucVessel.SelectedVessel)
                , General.GetNullableString(ucZone.selectedlist)
                , General.GetNullableInteger(ddlSelectFrom.SelectedHard)
                , General.GetNullableInteger(ddlactiveyn.SelectedValue)
                , General.GetNullableString(ucManager.SelectedList)
                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvMedicalInsurance.PageSize
                , ref iRowCount
                , ref iTotalPageCount);


            General.SetPrintOptions("gvMedicalInsurance", "Medical Insurance", alCaptions, alColumns, ds);

            gvMedicalInsurance.DataSource = ds;
            gvMedicalInsurance.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMedicalInsurance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadLabel lblEmployeeId = (RadLabel)e.Item.FindControl("lblEmployeeId");
                LinkButton lnkEmployeeName = (LinkButton)e.Item.FindControl("lnkEmployeeName");
                if (lblEmployeeId != null && lnkEmployeeName != null)
                    lnkEmployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmployeeId.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMedicalInsurance_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvMedicalInsurance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMedicalInsurance.CurrentPageIndex + 1;

        ShowReport();
    }
    protected void gvMedicalInsurance_ItemCommand(object sender, GridCommandEventArgs e)
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

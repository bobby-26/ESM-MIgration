using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;

public partial class CrewSignOnManagerWise : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOnManagerWise.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOnManagerWise.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
                ucDate1.Text = DateTime.Now.ToShortDateString();
                NameValueCollection nvc = Filter.CurrentSignOnFilter;
                if (nvc != null)
                {
                    ucManager.SelectedAddress = nvc.Get("ucManager");
                    ucDate.Text = nvc.Get("ucDate");
                    ucDate1.Text = nvc.Get("ucDate1");
                    ucZone.SelectedZoneValue = nvc.Get("ucZone");
                    if (nvc.Get("ucNationality").Equals("Dummy"))
                    {
                        ucNationality.SelectedNationalityValue = "";
                    }
                    else
                    {
                        ucNationality.SelectedNationalityValue = nvc.Get("ucNationality");
                    }
                    if (nvc.Get("ucPrincipal").Equals("Dummy"))
                    {
                        ucPrincipal.SelectedAddress = "";
                    }
                    else
                    {
                        ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal");
                    }
                    if (nvc.Get("ucVesselType").Equals("Dummy"))
                    {
                        ucVesselType.SelectedVesselTypeValue = "";
                    }
                    else
                    {
                        ucVesselType.SelectedVesselTypeValue = nvc.Get("ucVesselType");
                    }
                    if (nvc.Get("ucBatch").Equals("Dummy"))
                    {
                        ucBatch.SelectedBatch = "";
                    }
                    else
                    {
                        ucBatch.SelectedBatch = nvc.Get("ucBatch");
                    }
                    if (nvc.Get("ucEmpFleet").Equals("Dummy"))
                    {
                        ucEmpFleet.SelectedFleetValue = "";
                    }
                    else
                    {
                        ucEmpFleet.SelectedFleetValue = nvc.Get("ucEmpFleet").ToString();
                    }
                    if (nvc.Get("ucPool").Equals("Dummy"))
                    {
                        ucPool.SelectedPoolValue = "";
                    }
                    else
                    {
                        ucPool.SelectedPoolValue = nvc.Get("ucPool").ToString();
                    }
                }
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
                ucDate.Text = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucZone.selectedlist = "";
                ucNationality.SelectedNationalityValue = "";
                ucManager.SelectedAddress = "";
                ucBatch.SelectedBatch = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPoolValue = "";
                ucEmpFleet.SelectedFleetValue = "";
                Filter.CurrentSignOnFilter = null;

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
                if (!IsValidFilter(ucManager.SelectedAddress.ToString(), ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucManager", ucManager.SelectedAddress);
                    criteria.Add("ucDate", ucDate.Text);
                    criteria.Add("ucDate1", ucDate1.Text);
                    criteria.Add("ucZone", ucZone.SelectedZoneValue);
                    criteria.Add("ucNationality", ucNationality.SelectedNationalityValue);
                    criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
                    criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);
                    criteria.Add("ucBatch", ucBatch.SelectedBatch);
                    criteria.Add("ucEmpFleet", ucEmpFleet.SelectedFleetValue);
                    criteria.Add("ucPool", ucPool.SelectedPoolValue);
                    Filter.CurrentSignOnFilter = criteria;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDPASSPORTNO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDMARITALSTATUS", "FLDTRAVELDATE", "FLDSIGNONDATE", "FLDSEAPORTNAME", "FLD3AFORMYN", "FLDAGE", "FLDNATIONALITY", "FLDVESSELNAME", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "Sr.No", "EmpNo", "Passport No", "Batch", "Emp Name", "Rank", "Civil Status", "Travel to Vessel", "Signed On", "Signed On Port", "IIIA Y/N", "Age", "Nationality", "Vessel", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;

        if ((ucManager.SelectedAddress.Equals("")) || ucManager.SelectedAddress.Equals("Dummy"))
        {
            ds = PhoenixCrewSignOnReport.CrewSignOnReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   null, null,
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                   (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                   General.GetNullableDateTime(null),
                   General.GetNullableDateTime(null),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewSignOnReport.CrewSignOnReport(
                       (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                       (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                       (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                       (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                       null, null,
                       (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                       (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                       (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                       (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount);
        }

        string fromdates = ucDate.Text;
        string todatess = ucDate1.Text;
        Response.AddHeader("Content-Disposition", "attachment; filename=Sign_On_Report_ManagerWise.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign On Managerwise</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDPASSPORTNO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDTRAVELDATE", "FLDSIGNONDATE", "FLDSEAPORTNAME", "FLDAGE", "FLDNATIONALITY", "FLDVESSELNAME", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "Sr.No", "EmpNo", "Passport No", "Batch", "Emp Name", "Rank", "Travel to Vessel", "Signed On", "Signed On Port", "Age", "Nationality", "Vessel", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        if (ucEmpFleet.SelectedList.ToString().Contains("Dummy,"))
        {
            string s = ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "");
            ucEmpFleet.SelectedList = s;
        }
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;

        if ((ucManager.SelectedAddress.Equals("")) || ucManager.SelectedAddress.Equals("Dummy"))
        {
            ds = PhoenixCrewSignOnReport.CrewSignOnReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   null, null,
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                   (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                   General.GetNullableDateTime(null),
                   General.GetNullableDateTime(null),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewSignOnReport.CrewSignOnReport(
                       (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                       (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                       (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                       (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                       null, null,
                       (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                       (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                       (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                       (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvCrew.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);
        }


        General.SetPrintOptions("gvCrew", "Crew Sign On Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            if (ViewState["TTO"].ToString() != drv["FLDEMPLOYEESTATUS"].ToString())
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
            else
                lbr.Enabled = false;
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
    public bool IsValidFilter(string managerid, string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";


        if (managerid.Equals("") || managerid.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Manager is Required";
        }
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
}

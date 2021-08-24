using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewSignOffManagerWise : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOffManagerWise.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewSignOffManagerWise.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, string.Empty).Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();
                lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", ""));
                NameValueCollection nvc = Filter.CurrentSignOffFilter;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
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
                    if (nvc.Get("ChkNoReliever").Equals("0"))
                    {
                        ChkNoReliever.Checked = false;
                    }
                    else
                    {
                        ChkNoReliever.Checked = true;
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
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
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
                ucDate.Text = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucZone.SelectedZoneValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucManager.SelectedAddress = "";
                ucBatch.SelectedBatch = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucPool.SelectedPoolValue = "";
                ucEmpFleet.SelectedFleetValue = "";
                ChkNoReliever.Checked = false;
                Filter.CurrentSignOffFilter = null;

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
                    criteria.Add("ChkNoReliever", ChkNoReliever.Checked == true ? "1" : "0");
                    Filter.CurrentSignOffFilter = criteria;
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
        string[] alColumns = { "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDMARITALSTATUS", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDDURATION", "FLDAGE", "FLDNATIONALITY", "FLDRELIEVERNAME", "FLDRELIEVERRANK", "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDREASON", "FLDAVAILABLE", "FLDCOURSE", "FLDSIGNOFFREMARKS", "FLDAPPRAISALRECD" };
        string[] alCaptions = { "EmpNo", "Batch", "Emp Name", "Rank", "Civil Status", "Signed On", "Signed Off", "Duration", "Age", "Nationality", "Reliever", "Releiver Rank", "Vessel", "Signed Off Port", " Reason for Sign Off", "Available Date", "Recmd Course", "Remarks", "Appraisal Recd" };
        string[] alCaptions1 = { "", "", "", "", "", "", "", "", "", "", "Port", "Sign Off", "", "Recd" };
        string[] FilterColumns = { "FLDSELECTEDPRINCIPAL", "FLDSELECTEDMANAGER", "FLDSELECTEDNATIONALITY", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDFLEET", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDTRAININGBATCH", "FLDSELECTEDSTATUS" };
        string[] FilterCaptions = { "Principal", "Manager", "Nationality", "Zone", "Pool", "Fleet", "Vessel Type", "Batch", "Status" };

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
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        if (!IsPostBack)
        {
            ds = PhoenixCrewSignOffReport.CrewSignOffReports(null, null, null, 0, null, null, null, null, null, null, 0, null, null, null, sortexpression, sortdirection,
                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                iRowCount,
                                ref iRowCount,
                                ref iTotalPageCount, null);
        }
        else
        {
            if (ucManager.SelectedAddress.Equals("") || ucManager.SelectedAddress.Equals("Dummy"))
            {
                ds = PhoenixCrewSignOffReport.CrewSignOffReports(null, null, null, 0, null, null, null, null, null, null, 0, null, null, null, sortexpression, sortdirection,
                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                iRowCount,
                                ref iRowCount,
                                ref iTotalPageCount, null);
            }
            else
            {
                ds = PhoenixCrewSignOffReport.CrewSignOffReports(
                    (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                    (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                    (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                    (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                    null, null, null,
                    (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                    (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                    (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                    (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                    General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text),
                    (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
                     sortexpression, sortdirection,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                     iRowCount,
                     ref iRowCount,
                     ref iTotalPageCount
                     , General.GetNullableInteger((ChkNoReliever.Checked == true ? "1" : "0")));
            }
        }

        string fromdates = ucDate.Text;
        string todatess = ucDate1.Text;

        Response.AddHeader("Content-Disposition", "attachment; filename=Crew_Sign_Off_Managerwise.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Sign Off Managerwise</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions1.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions1[i] + "</b>");
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
        Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDBATCH", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDDURATION", "FLDAGE", "FLDNATIONALITY", "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDREASON", "FLDAVAILABLE", "FLDCOURSE", "FLDSIGNOFFREMARKS", "FLDAPPRAISALRECD" };
        string[] alCaptions = { "EmpNo", "Batch", "Emp Name", "Rank", "Signed On", "Signed Off", "Duration", "Age", "Nationality", "Vessel", "Signed Off", " Reason for Sign Off", "Available Date", "Recmd Course", "Remarks", "Appraisal Recd" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucNationality.SelectedList = ucNationality.SelectedList.ToString().Contains("Dummy,") ? ucNationality.SelectedList.ToString().Replace("Dummy,", "") : ucNationality.SelectedList;
        ucEmpFleet.SelectedList = ucEmpFleet.SelectedList.ToString().Contains("Dummy,") ? ucEmpFleet.SelectedList.ToString().Replace("Dummy,", "") : ucEmpFleet.SelectedList;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        DataSet ds = new DataSet();
        if (!IsPostBack)
        {
            ds = PhoenixCrewSignOffReport.CrewSignOffReports(null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                gvCrew.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount, null);
        }
        else
        {
            if (ucManager.SelectedAddress.Equals("") || ucManager.SelectedAddress.Equals("Dummy"))
            {
                ds = PhoenixCrewSignOffReport.CrewSignOffReports(null, null, null, null, null, null, null, null, null, null, null, null, null, null, sortexpression, sortdirection,
                                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                gvCrew.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount, null);
            }
            else
            {
                ds = PhoenixCrewSignOffReport.CrewSignOffReports(
                    (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                    (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                    (ucNationality.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucNationality.SelectedList),
                    (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                    null, null, null,
                    (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                    (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                    (ucEmpFleet.SelectedList.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucEmpFleet.SelectedList),
                    (ucBatch.SelectedBatch.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucBatch.SelectedBatch),
                    General.GetNullableDateTime(ucDate.Text),
                    General.GetNullableDateTime(ucDate1.Text),
                    (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList()),
                     sortexpression, sortdirection,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount
                     , General.GetNullableInteger((ChkNoReliever.Checked == true ? "1" : "0")));
            }
        }

        General.SetPrintOptions("gvCrew", "Crew Sign Off Report", alCaptions, alColumns, ds);

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

            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucRemarks");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblRemarks");
            if (lb != null)
            {
                lb.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lb.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }
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

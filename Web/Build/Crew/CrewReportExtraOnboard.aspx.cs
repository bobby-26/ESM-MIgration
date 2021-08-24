using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportExtraOnboard : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportExtraOnboard.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportExtraOnboard.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                //ucToDate.Text = DateTime.Now.ToShortDateString();
                NameValueCollection nvc = Filter.CurrentSignOnFilter;
                if (nvc != null)
                {

                    ucFromDate.Text = nvc.Get("ucFromDate");
                    ucToDate.Text = nvc.Get("ucToDate");
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
                ucFromDate.Text = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPoolValue = "";
                ucVessel.SelectedVessel= "";
                ddlReasonForSignon.SelectedValue = "";
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
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucFromDate", ucFromDate.Text);
                    criteria.Add("ucToDate", ucToDate.Text);
                    criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
                    criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFSEAPORTNAME", "FLDWAGES", "FLDDURATION", "FLDEXTRASIGNONREASON", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Vessel", "Sign On", "SignOn Port", "Relief Due", "Sign Off", "SignOff Port", " Wages in USD", "Duration(mm/dd)", "Reason for extra crew", "Sign on remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewSignOnReport.CrewReportExtraOnboardSearch(
            (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
            null,
            (ucVessel.SelectedVessel.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVessel.SelectedVessel),
            (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
            (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
            General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text),
            General.GetNullableInteger(ddlReasonForSignon.SelectedValue),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        string fromdates = ucFromDate.Text;
        string todatess = ucToDate.Text;
        Response.AddHeader("Content-Disposition", "attachment; filename=ExtraCrewOnBoard.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Extra Crew Onboard</center></h5></td></tr>");
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFSEAPORTNAME", "FLDWAGES", "FLDDURATION", "FLDEXTRASIGNONREASON", "FLDSIGNONREMARKS" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Vessel", "Sign On", "Sign on Port", "Relief Due", "Sign Off", "Sign off Port", " Wages in USD", "Duration(mm/dd)", "S/on Reason", "Sign on remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();


        ds = PhoenixCrewSignOnReport.CrewReportExtraOnboardSearch(
                (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                null,
                (ucVessel.SelectedVessel.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                General.GetNullableDateTime(ucFromDate.Text),
                General.GetNullableDateTime(ucToDate.Text),
                General.GetNullableInteger(ddlReasonForSignon.SelectedValue),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCrew.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Extra Crew Onboard Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void ucPrincipal_TextChangedEvent(object sender, EventArgs e)
    {
        StringBuilder strVesselType = new StringBuilder();
        ucPrincipal.SelectedAddress = General.GetNullableInteger(ucPrincipal.SelectedAddress) == null ? "" : ucPrincipal.SelectedAddress;
        ucVessel.VesselList = PhoenixRegistersVessel.ListVessel(null, General.GetNullableString(ucPrincipal.SelectedAddress), 0, ucVesselType.SelectedVesseltype);
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "&familyid=" + drv["FLDFAMILYID"].ToString() + "'); return false;");

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
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";


        if (string.IsNullOrEmpty(fromdate) && string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "Either FromDate or To Date is required";
        }

        else if (DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "To date should be earlier than current date";
            ucToDate.Text = DateTime.Now.ToShortDateString();
        }

        return (!ucError.IsError);
    }
}

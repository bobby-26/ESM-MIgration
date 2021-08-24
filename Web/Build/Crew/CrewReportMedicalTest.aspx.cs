using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;
public partial class CrewReportMedicalTest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportMedicalTest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportMedicalTest.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                lstMedicalTest.DataSource = PhoenixRegistersDocumentMedical.ListMedical();
                lstMedicalTest.DataTextField = "FLDNAMEOFMEDICAL";
                lstMedicalTest.DataValueField = "FLDMEDICALID";
                lstMedicalTest.DataBind();

                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                  , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 53, 1, "MDL,LSL,NTB,LFT,DTH,DST,EXM,TSP").Tables[0]);

                lstStatus.DataSource = ds;
                lstStatus.DataBind();

                ucTestToDate.Text = DateTime.Now.ToShortDateString();
                ucSignonToDate.Text = DateTime.Now.ToShortDateString();
                ucSignoffToDate.Text = DateTime.Now.ToShortDateString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;

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
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            if (!IsValidTestFilter(ucTestFromDate.Text, ucTestToDate.Text, ucSignoffFromDate.Text, ucSignoffToDate.Text, ucSignonFromDate.Text, ucSignonToDate.Text))
            {
                ucError.Visible = true;
                return;
            }
            StringBuilder strlist = new StringBuilder();
            foreach (RadListBoxItem item in lstMedicalTest.Items)
            {
                if (item.Selected == true)
                {

                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }

            }
            if (strlist.Length > 1)
            {
                strlist.Remove(strlist.Length - 1, 1);
            }

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("principalid", ucPrincipal.SelectedAddress);
            criteria.Add("managerid", ucManager.SelectedAddress);
            criteria.Add("medicaltest", strlist.ToString());
            criteria.Add("zone", ucZone.selectedlist);
            criteria.Add("pool", ucPool.SelectedPool);
            criteria.Add("testfromdate", ucTestFromDate.Text);
            criteria.Add("testtodate", ucTestToDate.Text);
            criteria.Add("signonfromdate", ucSignonFromDate.Text);
            criteria.Add("signontodate", ucSignonToDate.Text);
            criteria.Add("signofffromdate", ucSignoffFromDate.Text);
            criteria.Add("signofftodate", ucSignoffToDate.Text);
            criteria.Add("vessellist", ucVesselType.SelectedVesseltype);
            criteria.Add("status", ddlStatus.SelectedHard);
            criteria.Add("activeyn", chkArchived.Checked == true ? "0" : "1");
            criteria.Add("SeafarerStatus", StatusSelectedList());
            Filter.CurrentMedicalTestReportFilter = criteria;

            Rebind();

        }

    }

    private bool IsValidTestFilter(string testfromdate, string testtodate, string SignoffFromDate, string SignoffTodate, string SignonFromDate, string SignonTodate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;
        if (General.GetNullableDateTime(testfromdate) == null && General.GetNullableDateTime(SignoffFromDate) == null && General.GetNullableDateTime(SignonFromDate) == null)
        {
            ucError.ErrorMessage = "Any one Type of Date Filter is Required.";
        }
        else
        {
            int filterCount = 0;
            if (General.GetNullableDateTime(testfromdate) != null)
                filterCount++;
            if (General.GetNullableDateTime(SignoffFromDate) != null)
                filterCount++;
            if (General.GetNullableDateTime(SignonFromDate) != null)
                filterCount++;
            if (filterCount > 1)
            {
                ucError.ErrorMessage = "Please select Only one Type of Date Filter.";
            }
            else if (!string.IsNullOrEmpty(testfromdate))
            {
                if (DateTime.TryParse(testfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "Test From Date  should be earlier than current date.";
                }
            }
            else if (!string.IsNullOrEmpty(SignonFromDate))
            {
                if (DateTime.TryParse(SignonFromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "Sign on From Date  should be earlier than current date.";
                }
            }
            else if (!string.IsNullOrEmpty(SignoffFromDate))
            {
                if (DateTime.TryParse(SignoffFromDate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                {
                    ucError.ErrorMessage = "Sign off From Date  should be earlier than current date.";
                }
            }
        }
        

        return (!ucError.IsError);
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true)
            {

                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }

    

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = {  "FLDFILENO", "FLDBATCHNAME", "FLDEMPNAME", "FLDRANKCODE", "FLDPRESENTVESSELNAME", "FLDLASTSIGNONDATE"
                                 , "FLDSIGNOFFDATE","FLDLASTVESSELNAME", "FLDMEDICALNAME", "FLDISSUEDDATE", "FLDAFTERSIGNONOFF", "FLDRESULT", "FLDREMARKS" };
            string[] alCaptions = { "Emp No", "Batch", "Emp Name", "Rank", "Present Vessel", "Signed On", "Signed Off" ,"Last Vessel"
                                 , "Name of Test", "Date of Test", " Done after last sign off", "Result", "Remarks" };
            string sortexpression;

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            NameValueCollection nvc = Filter.CurrentMedicalTestReportFilter;

            DataSet ds = new DataSet();

            ds = PhoenixCrewReports.CrewMedicalTestReport((nvc != null ? General.GetNullableInteger(nvc.Get("principalid")) : null)
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("managerid")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("medicaltest")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("zone")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("pool")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("testfromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("testtodate")) : null)
                                    , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("signonfromdate")) : null)
                                    , (nvc != null ? General.GetNullableDateTime(nvc.Get("signontodate")) : null)
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                    , (nvc != null ? General.GetNullableInteger(nvc.Get("activeyn")) : 1)
                                    , (nvc != null ? nvc.Get("SeafarerStatus") : string.Empty)
                                     , sortexpression, sortdirection,
                                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                  gvCrew.PageSize,
                                  ref iRowCount,
                                  ref iTotalPageCount
                                    );


            General.SetPrintOptions("gvCrew", "Medical Test Report", alCaptions, alColumns, ds);

            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression;
        string[] alColumns = {  "FLDFILENO", "FLDBATCHNAME", "FLDEMPNAME", "FLDRANKCODE", "FLDPRESENTVESSELNAME", "FLDLASTSIGNONDATE"
                                 , "FLDSIGNOFFDATE","FLDLASTVESSELNAME", "FLDMEDICALNAME", "FLDISSUEDDATE", "FLDAFTERSIGNONOFF", "FLDRESULT", "FLDREMARKS" };
        string[] alCaptions = { "Emp No", "Batch", "Emp Name", "Rank", "Present Vessel", "Signed On", "Signed Off" ,"Last Vessel"
                                 , "Name of Test", "Date of Test", " Done after last sign off", "Result", "Remarks" };

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentMedicalTestReportFilter;

        DataSet ds = new DataSet();

        ds = PhoenixCrewReports.CrewMedicalTestReport((nvc != null ? General.GetNullableInteger(nvc.Get("principalid")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("managerid")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("medicaltest")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("zone")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("pool")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("testfromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("testtodate")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signonfromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signontodate")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("activeyn")) : 1)
                                , (nvc != null ? nvc.Get("SeafarerStatus") : string.Empty)
                                , sortexpression, sortdirection,
                                  Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                  iRowCount,
                                  ref iRowCount,
                                  ref iTotalPageCount
                                );

        if (General.GetNullableDateTime(nvc.Get("testfromdate")) != null)
        {
            ucTestFromDate.Text = General.GetNullableString(nvc.Get("testfromdate"));
        }
        Response.AddHeader("Content-Disposition", "attachment; filename=CrewMedicalTestReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Medical Test Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From: " + ucTestFromDate.Text + " To: " + ucTestToDate.Text + " </center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + DateTime.Now.ToShortDateString() + "</td></tr>");
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
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentMedicalTestReportFilter = null;
                ucPrincipal.SelectedAddress = "";
                ucManager.SelectedAddress = "";
                lstMedicalTest.SelectedIndex = -1;
                lstStatus.SelectedIndex = -1;
                ucZone.selectedlist = null;
                ucPool.SelectedPool = null;
                ucTestFromDate.Text = "";
                ucTestToDate.Text = DateTime.Now.ToShortDateString();
                ucSignonFromDate.Text = "";
                ucSignonToDate.Text = "";
                ucSignoffFromDate.Text = "";
                ucSignoffToDate.Text = "";
                ucVesselType.SelectedVesseltype = "";
                ddlStatus.SelectedHard = "";
                chkArchived.Checked = false;

                Rebind();

            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
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

        BindData();
    }




}

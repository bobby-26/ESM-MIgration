using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportsIceClassExperience : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsIceClassExperience.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsIceClassExperience.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ddlStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , 54, 1, "ONB,ONL");
                ddlStatus.DataBind();

                ddlStatus.Items.Insert(0, "--Select--");
                ddlStatus.Items.Insert(1, "All");
                foreach (RadComboBoxItem item in ddlStatus.Items)
                {
                    if (item.Text == "--Select--")
                        item.Value = "";
                    if (item.Text == "All")
                        item.Value = "220,221";
                }
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
                //ViewState["SHOWREPORT"] = null;
                //ucDateTo.Text = DateTime.Now.ToShortDateString();
                //ucDateFrom.Text = "";
                //ucDateTo.Text = DateTime.Now.ToShortDateString();
                //ucBatchList.SelectedValue = "";
                //ucVesselType.SelectedVesselTypeValue = "";
                //ShowReport();
                //SetPageNavigator();

                Response.Redirect("../Crew/CrewReportsIceClassExperience.aspx");
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
                ViewState["SHOWREPORT"] = 1;
                ShowReport();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadComboBoxItem item in ddlStatus.Items)
        {
            if (item.Selected == true)
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDICEEXPYN" };
        string[] alCaptions = { "Sl.No", "Employee Code", "Name", "Rank", "Last Vessel", "Sign-Off Date", "Vessel OnBoard", "SignOn Date", "Ice Experience" };
        string[] filtercolumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDICEEXPYN" };
        string[] filtercaptions = { "Sl.No", "Employee Code", "Name", "Rank", "Last Vessel", "Sign-Off Date", "Vessel OnBoard", "SignOn Date", "Ice Experience" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        ds = PhoenixCrewIceClassExperience.CrewIceClassExperienceSearch((ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                        , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                        , (ucManager.SelectedList) == "," ? null : General.GetNullableString(ucManager.SelectedList)
                                                                        , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                        , (ucZone.selectedlist) == "," ? null : General.GetNullableString(ucZone.selectedlist)
                                                                        , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                        , (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList())
                                                                        , (ddlIceExp.SelectedValue) == "" ? null : General.GetNullableInteger(ddlIceExp.SelectedValue)
                                                                        , sortexpression, sortdirection
                                                                        , 1
                                                                        , iRowCount
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewIceClassExperience.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
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
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDICEEXPYN" };
        string[] alCaptions = { "Sl.No", "Employee Code", "Name", "Rank", "Last Vessel", "Sign-Off Date", "Vessel OnBoard", "SignOn Date", "Ice Experience" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        if (ViewState["SHOWREPORT"] != null)
        {
            ds = PhoenixCrewIceClassExperience.CrewIceClassExperienceSearch((ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                            , (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                            , (ucManager.SelectedList) == "," ? null : General.GetNullableString(ucManager.SelectedList)
                                                                            , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                            , (ucZone.selectedlist) == "," ? null : General.GetNullableString(ucZone.selectedlist)
                                                                            , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                            , (StatusSelectedList()) == "" ? null : General.GetNullableString(StatusSelectedList())
                                                                            , (ddlIceExp.SelectedValue) == "" ? null : General.GetNullableInteger(ddlIceExp.SelectedValue)
                                                                            , sortexpression, sortdirection
                                                                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvCrew.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            );
        }
        else
            ds = EmptyDataSet();

        General.SetPrintOptions("gvCrew", "Crew IceClassExperience", alCaptions, alColumns, ds);

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

                LinkButton lnkIceExp = (LinkButton)e.Item.FindControl("lnkIceExp");
                RadLabel lblIceExp = (RadLabel)e.Item.FindControl("lblIceExp");

                if (drv["FLDICEEXPYN"].ToString().ToUpper() == "YES")
                {
                    lnkIceExp.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewReportsIceClassExperienceDetails.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
                    lnkIceExp.Visible = true;
                    lblIceExp.Visible = false;
                }
                else
                {
                    lnkIceExp.Visible = false;
                    lblIceExp.Visible = true;
                }
            


                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpId");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
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

    private DataSet EmptyDataSet()
    {
        DataSet EmptyDs = new DataSet();
        DataTable EmptyDt = new DataTable();

        EmptyDt.Columns.Add("FLDROWNUMBER", typeof(String));
        EmptyDt.Columns.Add("FLDEMPLOYEECODE", typeof(String));
        EmptyDt.Columns.Add("FLDEMPLOYEEID", typeof(String));
        EmptyDt.Columns.Add("FLDNAME", typeof(String));
        EmptyDt.Columns.Add("FLDRANKPOSTEDNAME", typeof(String));
        EmptyDt.Columns.Add("FLDLASTVESSELNAME", typeof(String));
        EmptyDt.Columns.Add("FLDLASTSIGNOFFDATE", typeof(String));
        EmptyDt.Columns.Add("FLDPRESENTVESSELNAME", typeof(String));
        EmptyDt.Columns.Add("FLDPRESENTSIGNONDATE", typeof(String));
        EmptyDt.Columns.Add("FLDICEEXPYN", typeof(String));

        EmptyDs.Tables.Add(EmptyDt);
        return EmptyDs;
    }
}

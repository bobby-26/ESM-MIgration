using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewContractReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewContractReportFilter.aspx" + "'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbar.AddFontAwesomeButton("../Crew/CrewContractReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        CrewContractMenu.MenuList = toolbar.Show();
        CrewContractMenu.AccessRights = this.ViewState;

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("Show Report", "SHOWREPORT");
        toolbar1.AddButton("Visual", "SHOWVISUAL", ToolBarDirection.Right);
        MenuReportsFilter.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["SUMMARY"] = "1";
            gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    // tab strip method
    protected void CrewContractMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

            if (CommandName.ToUpper().Equals("SHOWVISUAL"))
            {
                //sessionFilterValues();
                Response.Redirect("../Crew/CrewContractReportVisual.aspx");
            }
            else if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                Response.Redirect("../Crew/CrewContractReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    // grid Events
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
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

    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
        gvCrew.Rebind();
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        ViewState["SHOWREPORT"] = 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 100;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        //  if (!IsPostBack)

            // read from session filter and bind the data
            if (Filter.CurrentCrewContractFilter != null)
            {
                NameValueCollection filter = Filter.CurrentCrewContractFilter;

                ds = PhoenixCrewContractBI.CrewContract(
                        General.GetNullableString(filter.Get("ucPrincipal").ToString()),
                        General.GetNullableString(filter.Get("ucVessel").ToString()),
                        General.GetNullableString(filter.Get("txtFileNumber").ToString()),
                        General.GetNullableString(filter.Get("ucRank").ToString()),
                        General.GetNullableString(filter.Get("ucZone").ToString()),
                        //General.GetNullableString(filter.Get("ddlRankGroup").ToString()),
                        null,
                        General.GetNullableString(filter.Get("ucPool").ToString()),
                        //  General.GetNullableString(filter.Get("ucVesselType").ToString()),
                        General.GetNullableString(filter.Get("ddlYear").ToString()),
                        //General.GetNullableString(filter.Get("ddlReason").ToString()),
                        null,
                        //null,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount
                    );
            }
            else
            {
                ds = PhoenixCrewContractBI.CrewContract(
                            null,
                            null,
                            null,
                            null,
                            null,
                            //  null,
                            null,
                            null,
                            DateTime.Now.Year.ToString(),
                            null,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                       );
            }
      string[] alCaptions = getCaptions();
      string[] alColumns = getColumns();
    //
    //   Response.AddHeader("Content-Disposition", "attachment; filename=" + "CrewContract" + ".xls");
    //   Response.ContentType = "application/vnd.msexcel";
    //   Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
    //   Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
    //   Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + "CrewContract" + "</center></h5></td></tr>");
    //   Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
    //   Response.Write("</tr>");
    //   Response.Write("</TABLE>");
    //
    //   if (ViewState["SUMMARY"].ToString() == "1")
    //   {
    //       Response.Write("<br />");
    //       Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
    //       Response.Write("<tr>");
    //       for (int i = 0; i < alCaptions.Length; i++)
    //       {
    //           Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
    //           Response.Write("<b>" + alCaptions[i] + "</b>");
    //           Response.Write("</td>");
    //       }
    //       Response.Write("</tr>");
    //
    //       foreach (DataRow dr in ds.Tables[0].Rows)
    //       {
    //           Response.Write("<tr>");
    //           for (int i = 0; i < alColumns.Length; i++)
    //           {
    //               Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
    //               Response.Write(dr[alColumns[i]]);
    //               Response.Write("</td>");
    //           }
    //           Response.Write("</tr>");
    //       }
    //       Response.Write("</TABLE>");
    //       Response.End();

            General.ShowExcel("Licence Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        
    }

    private void ShowReport()
    {
        try
        {
            DataSet ds = new DataSet();

            ViewState["SHOWREPORT"] = 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alCaptions = getCaptions();
            string[] alColumns = getColumns();

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (Filter.CurrentCrewContractFilter != null)
            {
                NameValueCollection filter = Filter.CurrentCrewContractFilter;

                ds = PhoenixCrewContractBI.CrewContract(
                        General.GetNullableString(filter.Get("ucPrincipal").ToString()),
                        General.GetNullableString(filter.Get("ucVessel").ToString()),
                        General.GetNullableString(filter.Get("txtFileNumber").ToString()),
                        General.GetNullableString(filter.Get("ucRank").ToString()),
                        General.GetNullableString(filter.Get("ucZone").ToString()),
                        //General.GetNullableString(filter.Get("ddlRankGroup").ToString()),
                        null,
                        General.GetNullableString(filter.Get("ucPool").ToString()),
                      //  General.GetNullableString(filter.Get("ucVesselType").ToString()),
                        General.GetNullableString(filter.Get("ddlYear").ToString()),
                        //General.GetNullableString(filter.Get("ddlReason").ToString()),
                        null,
                        //null,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount
                    );
            }
            else
            {
                ds = PhoenixCrewContractBI.CrewContract(
                            null,
                            null,
                            null,
                            null,
                            null,
                           // null,
                            null,
                            null,
                            DateTime.Now.Year.ToString(),
                            null,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                          gvCrew.PageSize,
                           ref iRowCount,
                           ref iTotalPageCount
                       );
            }
            General.SetPrintOptions("gvCrew", "Crew Contract", alCaptions, alColumns, ds);

            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (iRowCount > 0)
                ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
            else
                ViewState["ROWSINGRIDVIEW"] = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string[] getColumns()
    {
        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDZONE", "FLDPOOL", "FLDVESSELNAME", "FLDVESSELTYPEDESCRIPTION", "FLDPRINCIPAL", "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSHORTCONTRACT", "FLDRELIEF", "FLDCONTRACTMONTHS", "FLDCONTRACTWITHOUTPLUSMINUS", "FLDSIGNOFFREASON" };
        return alColumns;
    }

    private string[] getCaptions()
    {
        string[] alCaptions = { "File No", "Employee Name", "Rank Code", "Zone", "Pool", "Vessel", "Vessel Type", "Principal", "Sign on", "Sign Off", "Short Contract", "Releif", "Contract Months", "Contract Without Plus Minus", "Sign Off Reason" };
        return alCaptions;
    }

    // popup method
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvCrew.CurrentPageIndex = 0;
        ShowReport();
        gvCrew.Rebind();         
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
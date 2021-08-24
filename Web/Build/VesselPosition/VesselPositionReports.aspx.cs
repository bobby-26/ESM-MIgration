using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Collections.Specialized;
using System.Web;
using Telerik.Web.UI;

public partial class VesselPosition_VesselPositionReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionReports.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourseList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        NameValueCollection nvc = Filter.CurrentVesselPositionReportFilter;
        if (nvc != null && nvc["LaunchFromDB"] != null)
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+ "/VesselPosition/VesselPositionReportFilter.aspx?LaunchFromDB=1'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        else
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/VesselPosition/VesselPositionReportFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionReports.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

        MenuCrewCourseList.AccessRights = this.ViewState;
        MenuCrewCourseList.MenuList = toolbar.Show();
        //MenuCrewCourseList.SetTrigger(pnlCourseListEntry);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            gvCourseList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLREPORTDATE", "FLDREPORTNAME", "FLDFROMPORT", "FLDTOPORT" };
        string[] alCaptions = { "Vessel", "Report Date", "Report Type", "Current Port", "Next Port" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentVesselPositionReportFilter;
        ds = PhoenixVesselPositionNoonReport.ReportSearch(
            General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["UcCurrentPort"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["UcNextPort"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
           , sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvCourseList.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ShipTrack.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Ship Track</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
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

    protected void CrewCourseList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Rebind();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }

    private void ClearFilter()
    {
        Filter.CurrentVesselPositionReportFilter = null;
        Session["DATAKEY"] = null;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvCourseList_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                RadLabel vesselReportid = (RadLabel)e.Item.FindControl("lblReportID");
                RadLabel vesselReportType = (RadLabel)e.Item.FindControl("lblReportType");

                PhoenixVesselPositionNoonReport.DeleteReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , General.GetNullableInteger(vesselReportType.Text)
                                                            , General.GetNullableGuid(vesselReportid.Text));
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel vesselReportid = (RadLabel)e.Item.FindControl("lblReportID");
                RadLabel vesselReportType = (RadLabel)e.Item.FindControl("lblReportType");
                Filter.CurrentNoonReportLaunchFrom = "ST";

                if (vesselReportid != null && vesselReportType.Text == "0")
                {
                    Session["DATAKEY"] = vesselReportid.Text;
                    Session["VESSELARRIVALID"] = vesselReportid.Text;
                    Response.Redirect("../VesselPosition/VesselPositionArrivalReportEdit.aspx");
                }
                else if (vesselReportid != null && vesselReportType.Text == "1")
                {
                    Session["DATAKEY"] = vesselReportid.Text;
                    Filter.CurrentVPRSDepartureReportSelection = vesselReportid.Text;
                    Response.Redirect("../VesselPosition/VesselPositionDepartureReportEdit.aspx?VESSELDEPARTUREID=" + vesselReportid.Text);
                }
                else if (vesselReportid != null && vesselReportType.Text == "4")
                {
                    Session["DATAKEY"] = vesselReportid.Text;
                    Session["NOONREPORTID"] = vesselReportid.Text;
                    Response.Redirect("VesselPositionNoonReport.aspx?NoonReportID=" + vesselReportid, false);
                }
                else if (vesselReportid != null && vesselReportType.Text == "2")
                {
                    Session["DATAKEY"] = vesselReportid.Text;
                    Filter.CurrentVPRSShiftingReportSelection = vesselReportid.Text;
                    Response.Redirect("../VesselPosition/VesselPositionShiftingReportEdit.aspx?VESSELDEPARTUREID=" + vesselReportid.Text);
                }
                else if (vesselReportid != null && vesselReportType.Text == "3")
                {
                    Session["DATAKEY"] = vesselReportid.Text;
                    Session["NOONREPORTID"] = vesselReportid.Text;
                    Response.Redirect("VesselPositionNoonReport.aspx?NoonReportID=" + vesselReportid, false);
                    //Response.Redirect("VesselPositionWedSunReport.aspx?WedSunReportID=" + vesselReportid, false);
                }
            }
            else if (e.CommandName == "Page")
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


    protected void gvCourseList_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = drv["FLDCANCELFLAG"].ToString() == "1" ? true : false;
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            LinkButton ImgSentYN = (LinkButton)e.Item.FindControl("ImgSentYN");
            if (ImgSentYN != null)
            {
                ImgSentYN.Visible = drv["FLDCONFIRMEDYN"].ToString() == "1" ? false : true;
            }               
        }

    }

    protected void gvCourseList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourseList.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLREPORTDATE", "FLDREPORTNAME", "FLDFROMPORT", "FLDTOPORT" };
        string[] alCaptions = { "Vessel", "Report Date", "Report Type", "Current Port", "Next Port" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.CurrentVesselPositionReportFilter;
        DataSet ds = PhoenixVesselPositionNoonReport.ReportSearch(
            General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
            , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
             , General.GetNullableInteger(nvc != null ? nvc["UcCurrentPort"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["UcNextPort"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
            , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
            , sortexpression, sortdirection,
             int.Parse(ViewState["PAGENUMBER"].ToString()),
             gvCourseList.PageSize,
             ref iRowCount,
             ref iTotalPageCount);

        General.SetPrintOptions("gvCourseList", "Ship Track", alCaptions, alColumns, ds);

        gvCourseList.DataSource = ds;
        gvCourseList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void Rebind()
    {
        gvCourseList.SelectedIndexes.Clear();
        gvCourseList.EditIndexes.Clear();
        gvCourseList.DataSource = null;
        gvCourseList.Rebind();
    }

    protected void gvCourseList_PreRender(object sender, EventArgs e)
    {
        if (Session["DATAKEY"] != null)
        {
            foreach (GridItem item in gvCourseList.MasterTableView.Items)
            {
                if (item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)item;
                    if (Session["DATAKEY"].Equals(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["FLDREPORTID"].ToString()))
                    {
                        dataItem.Selected = true;
                        break;
                    }
                }
            }
        }
    }
}

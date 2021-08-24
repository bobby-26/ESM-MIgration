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

public partial class VesselPositionShiftingReport : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionShiftingReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCourseList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        NameValueCollection nvc = Filter.CurrentShiftingReportFilter;
        if (nvc != null && nvc["LaunchFromDB"] != null)
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+ "/VesselPosition/VesselPositionDepartureReportFilter.aspx?LaunchFromDB=1'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        else
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/VesselPosition/VesselPositionDepartureReportFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionShiftingReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            toolbar.AddFontAwesomeButton("../VesselPosition/VesselPositionShiftingReportEdit.aspx?mode=NEW", "Add", "<i class=\"fa fa-plus-circle\"></i>", "VESSEL");


        MenuDepartureeList.AccessRights = this.ViewState;
        MenuDepartureeList.MenuList = toolbar.Show();
       // MenuDepartureeList.SetTrigger(pnlCourseListEntry);

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
            ViewState["VESSELDEPARTUREID"] = null;
            ViewState["OVERDUE"] = Request.QueryString["overdue"] != null ? "1" : "";
            ViewState["REVIEW"] = Request.QueryString["review"] != null ? "1" : "";
            gvCourseList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

	}

	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDSBE", "FLDFWE", "FLDNEXTPORTNAME" };
        string[] alCaptions = { "Vessel Name", "Port", "SBE", "FWE", "Next Port" };

		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());
        if (isoverdue != null || isreview != null)
            Filter.CurrentShiftingReportFilter = null;
        NameValueCollection nvc = Filter.CurrentShiftingReportFilter;
        ds = PhoenixVesselPositionDepartureReport.DepartureReportSearch(General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                    , General.GetNullableGuid(nvc != null ? nvc["ucVoyage"] : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["UcCurrentPort"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["UcNextPort"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
                                                                    , sortexpression, sortdirection
                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCourseList.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , 1 // 0 departure report 1 shifting report
                                                                    , isoverdue
                                                                    , isreview);

        Response.AddHeader("Content-Disposition", "attachment; filename=ShiftingReport.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='" + HttpContext.Current.Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Shifting Report</h3></td>");
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

	protected void DepartureeList_TabStripCommand(object sender, EventArgs e)
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
        Filter.CurrentShiftingReportFilter = null;
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

			if (e.CommandName.ToUpper().Equals("EDIT"))
			{
                RadLabel vesselarrivalid = (RadLabel)e.Item.FindControl("lblVesselDepartureID");

                Filter.CurrentVPRSShiftingReportSelection = vesselarrivalid.Text;
                Filter.CurrentNoonReportLaunchFrom = null;
                if (vesselarrivalid != null )
                Response.Redirect("../VesselPosition/VesselPositionShiftingReportEdit.aspx?VESSELDEPARTUREID=" + vesselarrivalid .Text);
				
			}
           else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionDepartureReport.DeleteDepartureReport(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblVesselDepartureID")).Text));
                Rebind();
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
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ImgSentYN = (LinkButton)e.Item.FindControl("ImgSentYN");
            if (ImgSentYN != null)
            {
                ImgSentYN.Visible = drv["FLDCONFIRMEDYN"].ToString() == "1" ? false : true;
            }
            LinkButton ImgReview = (LinkButton)e.Item.FindControl("cmdReview");
            if (ImgReview != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (General.GetNullableInteger(drv["FLDREVIEWDYN"].ToString()) != null && General.GetNullableInteger(drv["FLDREVIEWDYN"].ToString()) == 0)
                {
                    ImgReview.Visible = true;
                    ImgReview.Visible = SessionUtil.CanAccess(this.ViewState, ImgReview.CommandName);

                    ImgReview.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselPosition/VesselPositionShiftingReportReview.aspx?VesselId=" + drv["FLDVESSELID"].ToString() + "&NoonReportID=" + drv["FLDNOONREPORTID"].ToString() + "');");
                }
            }
        }		
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}



    protected void gvCourseList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCourseList.CurrentPageIndex + 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDSEAPORTNAME", "FLDSBE", "FLDFWE", "FLDNEXTPORTNAME" };
        string[] alCaptions = { "Vessel Name", "Port", "SBE", "FWE", "Next Port" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        int? isoverdue = General.GetNullableInteger(ViewState["OVERDUE"].ToString());
        int? isreview = General.GetNullableInteger(ViewState["REVIEW"].ToString());
        if (isoverdue != null || isreview != null)
            Filter.CurrentShiftingReportFilter = null;
        NameValueCollection nvc = Filter.CurrentShiftingReportFilter;
        DataSet ds = PhoenixVesselPositionDepartureReport.DepartureReportSearch(
           General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlFleet"] : string.Empty)
                                                                    , General.GetNullableGuid(nvc != null ? nvc["ucVoyage"] : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtReportFrom"] : string.Empty)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtReportTo"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["UcCurrentPort"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["UcNextPort"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlMonth"] : string.Empty)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ddlYear"] : string.Empty)
                                                                    , sortexpression, sortdirection
                                                                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCourseList.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , 1 // 0 departure report 1 shifting report
                                                                    , isoverdue
                                                                    , isreview); 

        General.SetPrintOptions("gvCourseList", "Shifting Report", alCaptions, alColumns, ds);

        gvCourseList.DataSource = ds;
        gvCourseList.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["VESSELDEPARTUREID"] = dr["FLDVESSELDEPARTUREID"].ToString();
        }
      

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
}

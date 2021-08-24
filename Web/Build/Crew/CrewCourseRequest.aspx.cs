using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewCourseRequest : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../Crew/CrewCourseRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("javascript: openNewWindow('Filter', '', '" + Session["sitepath"] + "/Crew/CrewCourseRequestFilter.aspx'); return false; ", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarsub.AddFontAwesomeButton("../Crew/CrewCourseRequest.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");              
            MRMenu.AccessRights = this.ViewState;
            MRMenu.MenuList = toolbarsub.Show();
            if (!Page.IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MRMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCourseRequestFilter = null;
                BindData();
                gvReq.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREFNO", "FLDFILENO", "FLDNAME", "FLDACCOMYN", "FLDRANKCODE", "FLDZONE", "FLDVESSELNAME", "FLDJOINEDVESSEL", "FLDCOURSE", "FLDFROMDATE", "FLDTODATE", "FLDREMARKS", "FLDTRAVELNO", "FLDCREATEDDATE", "FLDCREATEDBY" };
                string[] alCaptions = { "Ref Number", "File No", "Name", "Accom. Req.", "Rank", "Zone", "Vsl Pln", " Vsl Joined", "Courses to do", "Available From", "Available To", "Remarks", "Travel RefNo", "Created Date", "Created By" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                NameValueCollection nvc = Filter.CurrentCourseRequestFilter;
                DataTable dt = PhoenixCrewCourseCertificate.SearchCourseRequest(null
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                            , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkInactive"] : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                            , null
                                                            , General.GetNullableInteger(nvc != null ? nvc["ddlCourse"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                                                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                                                            , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                            , null
                                                            , sortexpression, sortdirection
                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvReq.PageSize
                                                            , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Course Request", dt, alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvReq.Rebind();
    }
  
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = 1; //default desc order
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {
            NameValueCollection nvc = Filter.CurrentCourseRequestFilter;
            DataTable dt = PhoenixCrewCourseCertificate.SearchCourseRequest(null
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlVessel"] : string.Empty)
                                                        , (byte?)General.GetNullableInteger(nvc != null ? nvc["chkInactive"] : string.Empty)
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty)
                                                        , null
                                                        , General.GetNullableInteger(nvc != null ? nvc["ddlCourse"] : string.Empty)
                                                        , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                                                        , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                                                        , General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                                                        , null
                                                        , sortexpression, sortdirection
                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                        , gvReq.PageSize
                                                        , ref iRowCount, ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                gvReq.DataSource = dt;
                gvReq.VirtualItemCount = iRowCount;
            }
            else
            {
                gvReq.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReq.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkEmployee");
            if (lb != null)
            {
                if (drv["FLDNEWAPP"].ToString() == "1")
                    lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
                else
                    lb.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
            }
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            LinkButton lnkCourse = (LinkButton)e.Item.FindControl("lnkCourse");
            UserControlToolTip uctDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDate");
            LinkButton imgEdit = (LinkButton)e.Item.FindControl("LinkButton");
            if (lnkCourse != null)
            {
                lnkCourse.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctDate.ToolTip + "', 'visible');");
                lnkCourse.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctDate.ToolTip + "', 'hidden');");
                lnkCourse.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewCourseRequestDetails.aspx?empid=" + drv["FLDEMPLOYEEID"] + "&refno=" + drv["FLDREFNO"] + "&vessel=" + drv["FLDVESSELNAME"] + "'); return false;");
            }
        }
    }
    protected void gvReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}

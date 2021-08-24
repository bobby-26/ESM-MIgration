using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;

public partial class CrewPlanEvent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEvent.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCCPlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEvent.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','Event Filter','" + Session["sitepath"] + "/Crew/CrewPlanEventFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEvent.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewPlanEvent.aspx", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");

            //toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Add','','" + Session["sitepath"] + "/Crew/CrewPlanEventAdd.aspx','','550px','400px'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuCrewChangePlan.AccessRights = this.ViewState;
            MenuCrewChangePlan.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCCPlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["VESSELID"] = "";

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["vesselid"].ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ChangePlan_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                
                string[] alColumns = { "FLDVESSELNAME", "FLDEVENTDATE", "FLDEVENTTODATE", "FLDPORTNAME", "FLDOFFSIGNERCOUNT", "FLDONSIGNERCOUNT", "FLDPORTAGENTNAME", "FLDREMARKS", "FLDSTATUSNAME" };
                string[] alCaptions = { "Vessel", "Starting Date", "Ending Date", "Port", "Off Signer count", "On Signer count", "Port Agent", "Remarks", "Status", };

                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCrewChangeEvent.CrewPlanEventSearch(
                                                                 General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                , General.GetNullableDateTime(txtEventFrom.Text)
                                                                , General.GetNullableDateTime(txtEventTo.Text)
                                                                , General.GetNullableInteger(ucport.SelectedValue)
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , sortexpression, sortdirection,
                                                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                 gvCCPlan.PageSize,
                                                                 ref iRowCount,
                                                                 ref iTotalPageCount);


                //NameValueCollection nvc = Filter.CurrentCrewPlanEventFilter;

                //if (nvc == null)
                //{
                //    nvc = new NameValueCollection();
                //    nvc.Add("ddlvessel", string.Empty);
                //    nvc.Add("txtEventFrom", string.Empty);
                //    nvc.Add("txtEventTo", string.Empty);
                //    nvc.Add("ddlStatus", string.Empty);

                //}

                //DataSet ds = PhoenixCrewChangeEvent.CrewPlanEventSearch(
                //                                                 General.GetNullableInteger(nvc != null ? nvc.Get("ddlvessel") : string.Empty)
                //                                                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEventFrom") : string.Empty)
                //                                                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEventTo") : string.Empty)
                //                                                , General.GetNullableInteger(nvc != null ? nvc.Get("ucport") : string.Empty)
                //                                                , General.GetNullableInteger(nvc != null ? nvc.Get("ddlStatus") : string.Empty)
                //                                                , sortexpression, sortdirection,
                //                                                 int.Parse(ViewState["PAGENUMBER"].ToString()),
                //                                                 gvCCPlan.PageSize,
                //                                                 ref iRowCount,
                //                                                 ref iTotalPageCount);

                if (ds.Tables.Count > 0)
                    General.ShowExcel("Crew Event", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ViewState["PAGENUMBER"] = 1;
                //Filter.CurrentCrewPlanEventFilter = null;

                BindData();
                gvCCPlan.Rebind();
            }

            else if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                //Filter.CurrentCrewPlanEventFilter = null;

                BindData();
                gvCCPlan.Rebind();
            }

            else if (CommandName.ToUpper().Equals("ADD"))
            {

                String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewPlanEventAdd.aspx?vesselid=" + ddlVessel.SelectedVessel + "',false,800,500);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDEVENTDATE", "FLDEVENTTODATE", "FLDPORTNAME", "FLDOFFSIGNERCOUNT", "FLDONSIGNERCOUNT", "FLDPORTAGENTNAME", "FLDREMARKS", "FLDSTATUSNAME" };
            string[] alCaptions = { "Vessel", "Starting Date", "Ending Date", "Port", "Off Signer count", "On Signer count", "Port Agent", "Remarks", "Status", };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewChangeEvent.CrewPlanEventSearch(
                                                                   General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                 , General.GetNullableDateTime(txtEventFrom.Text)
                                                                 , General.GetNullableDateTime(txtEventTo.Text)
                                                                 , General.GetNullableInteger(ucport.SelectedValue)
                                                                 , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                 , sortexpression, sortdirection,
                                                                  int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                  gvCCPlan.PageSize,
                                                                  ref iRowCount,
                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvCCPlan", "Crew Event", alCaptions, alColumns, ds);

            gvCCPlan.DataSource = ds;
            gvCCPlan.VirtualItemCount = iRowCount;


        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCCPlan.Rebind();
    }
    protected void gvCCPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SELECT")
            {

                string lblEventId = ((RadLabel)e.Item.FindControl("lblEventId")).Text;

                if (lblEventId != "" && lblEventId != null)
                {
                    Response.Redirect("../Crew/CrewPlanEventDetail.aspx?eventid=" + lblEventId, false);
                }
            }
            else if (e.CommandName.ToUpper() == "CLOSEEVENT")
            {
                string lblEventId = ((RadLabel)e.Item.FindControl("lblEventId")).Text;

                if (lblEventId != "" && lblEventId != null)
                {
                    PhoenixCrewChangeEvent.CloseCrewPlanEvent(new Guid(lblEventId));
                }

                BindData();
                gvCCPlan.Rebind();
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

    protected void gvCCPlan_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        gvCCPlan.Rebind();
    }


    protected void gvCCPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCPlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCCPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            LinkButton ed = (LinkButton)item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            LinkButton close = (LinkButton)item.FindControl("cmdCloseEvent");
            if (close != null)
            {
                close.Visible = SessionUtil.CanAccess(this.ViewState, close.CommandName);
                close.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to close this event?')");
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblRemarksTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

        }
    }

    protected void ddlVessel_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["VESSELID"] = ddlVessel.SelectedVessel;
    }
}
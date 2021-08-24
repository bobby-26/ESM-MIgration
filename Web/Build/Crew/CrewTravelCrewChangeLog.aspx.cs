using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewTravelCrewChangeLog : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
              
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTROW"] = null;
                ViewState["REQUESTID"] = null;
                ViewState["CANCELREQUISITION"]=null;

               
                Filter.CurrentTravelRequestFilter = null;
                gvCrewChangeLog.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelCrewChangeLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrewChangeLog')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelCrewChangeLogFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelCrewChangeLog.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            
            MenuCrewChangeLog.AccessRights = this.ViewState;
            MenuCrewChangeLog.MenuList = toolbargrid.Show();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();

        }
      
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREQUISITIONNO", "FLDNAME", "FLDRANKNAME", "FLDPORTNAME", "FLDVESSELNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDSTATUS" };
            string[] alCaptions = { "Request No.", "Name","Rank", "Crew Change Port", "Vessel","Origin", "Destination", "Departure Date", "Arrival Date","Status"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            NameValueCollection nvc = Filter.CurrentTravelRequestFilter;

            DataSet ds = PhoenixCrewTravelRequest.SearchCrewChangeLog(
                 General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ddlofficetravelyn") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("chkCanceledEmployees") : "0")
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucPort") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtPassengerName") : string.Empty)
                  , sortexpression, sortdirection
                  , (int)ViewState["PAGENUMBER"]
                  , gvCrewChangeLog.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount);

            General.SetPrintOptions("gvCrewChangeLog", "Crew Change Log", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCrewChangeLog.DataSource = ds.Tables[0];
                gvCrewChangeLog.VirtualItemCount = iRowCount;

                if (ViewState["TRAVELREQUESTID"] == null)
                    ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
            }
            else
            {
                gvCrewChangeLog.DataSource = "";
            }
            ViewState["CANCELREQUISITION"] = General.GetNullableInteger(nvc != null ? nvc.Get("chkCanceledEmployees") : "0");           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewChangeLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;              
                BindData();
                gvCrewChangeLog.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREQUISITIONNO", "FLDNAME", "FLDRANKNAME", "FLDPORTNAME", "FLDVESSELNAME", "FLDORIGINNAME", "FLDDESTINATIONNAME", "FLDTRAVELDATE", "FLDARRIVALDATE", "FLDSTATUS" };
                string[] alCaptions = { "Request No.", "Name", "Rank", "Crew Change Port", "Vessel", "Origin", "Destination", "Departure Date", "Arrival Date", "Status" };

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

                NameValueCollection nvc = Filter.CurrentTravelRequestFilter;
                DataSet ds = PhoenixCrewTravelRequest.SearchCrewChangeLog(
                  General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                   , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlofficetravelyn") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("chkCanceledEmployees") : "0")
                   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPort") : string.Empty)
                   , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                   , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                   , sortexpression, sortdirection
                   , (int)ViewState["PAGENUMBER"]
                   , General.ShowRecords(null)
                   , ref iRowCount
                   , ref iTotalPageCount);

                General.ShowExcel("Crew Change Log", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
                
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["CANCELREQUISITION"] = 0;
                Filter.CurrentTravelRequestFilter = null;
                ViewState["PAGENUMBER"] = 1;
                gvCrewChangeLog.CurrentPageIndex = 0;
                BindData();
                gvCrewChangeLog.Rebind();
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
        try
        {
            BindData();
            gvCrewChangeLog.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCrewChangeLog_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "CancelTravelRequest")
            {                
                Guid reqestid = General.GetNullableGuid((e.Item as GridEditableItem).GetDataKeyValue("FLDREQUESTID").ToString()).Value;
                if (reqestid != null)
                {
                    PhoenixCrewTravelRequest.UpdateCrewTravelReqStatus(reqestid);
                }
                BindData();
                gvCrewChangeLog.Rebind();
            }
            else if (e.CommandName == "EmailCancelTravelRequest")
            {
                Guid reqestid = General.GetNullableGuid((e.Item as GridEditableItem).GetDataKeyValue("FLDREQUESTID").ToString()).Value;
                ViewState["TRAVELREQUESTID"] = reqestid.ToString();
                if (reqestid != null)
                {
                    try
                    {
                        string selectedvendors = ",";
                        if (selectedvendors.Length <= 1)
                            selectedvendors = null;
                        DataSet dsvendor = PhoenixCrewTravelRequest.ListCrewTravelSelectedVendors(PhoenixSecurityContext.CurrentSecurityContext.UserCode, reqestid);

                        if (dsvendor.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsvendor.Tables[0].Rows)
                            {
                                selectedvendors = selectedvendors + dr["FLDAGENTID"].ToString() + ",";
                            }
                            string script = "javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelEmail.aspx?purpose=CTR&travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&selectedagent=" + selectedvendors + "');";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                        }

                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                    }
                }
                BindData();
                gvCrewChangeLog.Rebind();
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

    protected void gvCrewChangeLog_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEmail");
                if (cb != null)
                {
                    cb.Visible = true;
                    cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
                    cb.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the Travel Requisition?')");
                }

                if (ViewState["CANCELREQUISITION"] != null && ViewState["CANCELREQUISITION"].ToString() == "1")
                {
                    cb.Visible = false;
                    eb.Visible = true;
                }
                else
                {
                    cb.Visible = true;
                    eb.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewChangeLog_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewChangeLog.CurrentPageIndex + 1;
        BindData();
    }
}

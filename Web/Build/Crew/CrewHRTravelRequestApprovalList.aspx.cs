using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewHRTravelRequestApprovalList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelRequestApprovalList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTravelRequestApproval')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHRTravelRequestApprovalFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelRequestApprovalList.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuTravelRequestApproval.AccessRights = this.ViewState;
            MenuTravelRequestApproval.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["TRAVELREQUESTID"] = null;
                ViewState["PERSONALINFOSN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["travelrequestid"] != null && Request.QueryString["travelrequestid"].ToString() != "")
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();

                if (Request.QueryString["personalinfosn"] != null && Request.QueryString["personalinfosn"].ToString() != "")
                    ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"].ToString();

                gvTravelRequestApproval.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelRequestApproval_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentHRTravelRequestApprovalFilter = null;
                gvTravelRequestApproval.CurrentPageIndex = 0;
                BindData();
                gvTravelRequestApproval.Rebind();

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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTRAVELREQUESTNO", "FLDVESSELNAME", "FLDNAME", "FLDDEPATURECITY", "FLDDESTINATIONCITY", "FLDDEPATUREDATE", "FLDARRIVALDATE", "FLDTRAVELSTATUS", "FLDISAPPROVED" };
        string[] alCaptions = { "Request No.", "Vessel", "Name", "Origin", "Destination", "Departure", "Arrival", "Status", "Approved Y/N" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentHRTravelRequestApprovalFilter;

        ds = PhoenixCrewHRTravelRequest.HROfficeTravelRequestSearch(null
                   , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                   , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                   , (nvc != null ? General.GetNullableInteger(nvc["chkApprovedYN"]) : 0)
                   , null
                   , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                   , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                   , sortexpression
                   , sortdirection
                   , (int)ViewState["PAGENUMBER"]
                   , gvTravelRequestApproval.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount);


        if (ds.Tables.Count > 0)
            General.ShowExcel("Travel Approval", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELREQUESTNO", "FLDVESSELNAME", "FLDNAME", "FLDDEPATURECITY", "FLDDESTINATIONCITY", "FLDDEPATUREDATE", "FLDARRIVALDATE", "FLDTRAVELSTATUS", "FLDISAPPROVED" };
            string[] alCaptions = { "Request No.", "Vessel", "Name", "Origin", "Destination", "Departure", "Arrival", "Status", "Approved Y/N" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentHRTravelRequestApprovalFilter;

            DataSet ds = PhoenixCrewHRTravelRequest.HROfficeTravelRequestSearch(null, General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                    , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                    , (nvc != null ? General.GetNullableInteger(nvc["chkApprovedYN"]) : 0)
                    , null
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvTravelRequestApproval.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

            General.SetPrintOptions("gvTravelRequestApproval", "Travel Approval", alCaptions, alColumns, ds);

            gvTravelRequestApproval.DataSource = ds;
            gvTravelRequestApproval.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["TRAVELREQUESTID"] == null)
                {
                    ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDTRAVELREQUESTID"].ToString();
                    ViewState["PERSONALINFOSN"] = ds.Tables[0].Rows[0]["FLDPERSONALINFOSN"].ToString();                
                }

                SetRowSelection();
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void SetRowSelection()
    {
        try
        {           

            foreach (GridDataItem item in gvTravelRequestApproval.Items)
            {
                if (item.GetDataKeyValue("FLDTRAVELREQUESTID").ToString() == ViewState["TRAVELREQUESTID"].ToString())
                {
                    gvTravelRequestApproval.SelectedIndexes.Clear();

                    gvTravelRequestApproval.SelectedIndexes.Add(item.ItemIndex);                 
                }
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void gvTravelRequestApproval_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelRequestApproval.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvTravelRequestApproval_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;
            
            if (e.CommandName.ToUpper() == "SELECT")
            {             
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                ViewState["PERSONALINFOSN"] = ((RadLabel)e.Item.FindControl("lblPersonalInfosn")).Text;

                LinkButton lnkRequestNo = (LinkButton)e.Item.FindControl("lblRequestNo");
                PhoenixCrewTravelRequest.RequestNo = lnkRequestNo.Text;

                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=approval", false);

            }
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

    protected void gvTravelRequestApproval_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton select = (LinkButton)e.Item.FindControl("cmdSelect");
            if (select != null) select.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);

            LinkButton imgappove = (LinkButton)e.Item.FindControl("cmdApprove");
            if (imgappove != null)
            {
                imgappove.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?SESSIONID=" + drv["FLDSESSIONID"].ToString() + "&App=1');return false;");
            }
            LinkButton cmdReject = (LinkButton)e.Item.FindControl("cmdReject");
            if (cmdReject != null)
            {
                cmdReject.Attributes.Add("onclick", "openNewWindow('approval', '', '" + Session["sitepath"] + "/Crew/CrewHRMyTravelApproval.aspx?SESSIONID=" + drv["FLDSESSIONID"].ToString() + "&App=0');return false;");
            }

            RadLabel lblApproved = (RadLabel)e.Item.FindControl("lblApproved");
            if (lblApproved != null && lblApproved.Text == "1")
            {
                if (imgappove != null)
                    imgappove.Visible = false;
                if (cmdReject != null)
                    cmdReject.Visible = false;
            }

        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["TRAVELREQUESTID"] = null;
            ViewState["PAGENUMBER"] = 1;
            gvTravelRequestApproval.CurrentPageIndex = 0;

            BindData();
            gvTravelRequestApproval.Rebind();

            if (ViewState["TRAVELREQUESTID"] != null)
            {
                foreach (GridDataItem item in gvTravelRequestApproval.Items)
                {
                    if (item.GetDataKeyValue("FLDTRAVELREQUESTID").ToString() == ViewState["TRAVELREQUESTID"].ToString())
                    {
                        gvTravelRequestApproval.SelectedIndexes.Clear();

                        gvTravelRequestApproval.SelectedIndexes.Add(item.ItemIndex);
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


}

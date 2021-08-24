using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using SouthNests.Phoenix.CrewCommon;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewHotelBookingDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingDetails.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvHotelBooking')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingDetailsFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingDetails.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuHotelBooking.AccessRights = this.ViewState;
            MenuHotelBooking.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REQUESTID"] = null;

                gvHotelBooking.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }


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

            string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDPURPOSE", "FLDCITYNAME", "FLDCHECKINDATE", "FLDCHECKOUTDATE", "FLDGUESTSTATUS", "FLDBOOKINGSTATUS" };
            string[] alCaptions = { "Reference No.", "Name", "Rank", "Vessel", "Purpose", "City", "Checkin Date", "Checkout Date", "Guest Status", "Booking Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentHotelRequestDetailsFilter;

            DataSet ds = PhoenixCrewHotelBookingDetails.HotelBookingDetailsSearch(General.GetNullableString(nvc != null ? nvc.Get("txtReferenceNo") : string.Empty)
                , null
                , General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("txtcityid") : string.Empty)
                , null
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckinDateFrom") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckinDateTo") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckoutDateFrom") : string.Empty)
                , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckoutDateFrom") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ucPurpose") : string.Empty)
                , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("rblGuestStatus") : string.Empty)
                , General.GetNullableInteger(nvc != null ? nvc.Get("ddlBookingStatus") : string.Empty)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvHotelBooking.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvHotelBooking", "Hotel Booking Details", alCaptions, alColumns, ds);

            gvHotelBooking.DataSource = ds.Tables[0];
            gvHotelBooking.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuHotelBooking_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvHotelBooking.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREFERENCENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDPURPOSE", "FLDCITYNAME", "FLDCHECKINDATE", "FLDCHECKOUTDATE", "FLDGUESTSTATUS", "FLDBOOKINGSTATUS" };
                string[] alCaptions = { "Reference No.", "Name", "Rank", "Vessel", "Purpose", "City", "Checkin Date", "Checkout Date", "Guest Status", "Booking Status" };

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

                NameValueCollection nvc = Filter.CurrentHotelRequestDetailsFilter;

                DataSet ds = PhoenixCrewHotelBookingDetails.HotelBookingDetailsSearch(General.GetNullableString(nvc != null ? nvc.Get("txtReferenceNo") : string.Empty)
                    , null
                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucvessel") : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("txtcityid") : string.Empty)
                    , null
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckinDateFrom") : string.Empty)
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckinDateTo") : string.Empty)
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckoutDateFrom") : string.Empty)
                    , General.GetNullableDateTime(nvc != null ? nvc.Get("txtCheckoutDateFrom") : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("ucPurpose") : string.Empty)
                    , General.GetNullableString(nvc != null ? nvc.Get("txtName") : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("rblGuestStatus") : string.Empty)
                    , General.GetNullableInteger(nvc != null ? nvc.Get("ddlBookingStatus") : string.Empty)
                    , sortexpression, sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvHotelBooking.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

                General.ShowExcel("Hotel Booking Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {

                Filter.CurrentHotelRequestDetailsFilter = null;
                ViewState["PAGENUMBER"] = 1;
                gvHotelBooking.CurrentPageIndex = 0;
                BindData();
                gvHotelBooking.Rebind();
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
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvHotelBooking.Rebind();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHotelBooking_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHotelBooking.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvHotelBooking_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel the Guest?')");
            }            
        }
    }

    protected void gvHotelBooking_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "CANCELHOTELREQUEST")
            {
                string bookingid = ((RadLabel)e.Item.FindControl("lblBookingId")).Text;
                string guestid = ((RadLabel)e.Item.FindControl("lblGuestId")).Text;

                if (guestid != null)
                {
                    PhoenixCrewHotelBookingDetails.HotelBookingGuestCancellation(General.GetNullableGuid(bookingid)
                        , General.GetNullableGuid(guestid)
                        );
                }

                BindData();
                gvHotelBooking.Rebind();
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
}

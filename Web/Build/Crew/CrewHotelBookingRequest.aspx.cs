using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewHotelBookingRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvBookingDetails')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingFilter.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingRequest.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelRequestGeneral.aspx", "Add New Requisition", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuHotelBooking.AccessRights = this.ViewState;
            MenuHotelBooking.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                lblBookingId.Attributes.Add("style", "display:none");

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                ViewState["REQ"] = PhoenixCommonRegisters.GetHardCode(1, 223, "REQ");
                ViewState["QRY"] = PhoenixCommonRegisters.GetHardCode(1, 223, "QRY");
                ViewState["WBC"] = PhoenixCommonRegisters.GetHardCode(1, 223, "WBC");
                ViewState["CON"] = PhoenixCommonRegisters.GetHardCode(1, 223, "CON");
                ViewState["CND"] = PhoenixCommonRegisters.GetHardCode(1, 223, "CND");



                gvBookingDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuHotelBooking_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvBookingDetails.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["bookingid"] = null;
                ViewState["ROWCOUNT"] = null;
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentHotelBookingFilterCriteria = null;
                gvBookingDetails.CurrentPageIndex = 0;
                BindData();
                gvBookingDetails.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDHOTELNAME", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDCITYNAME", "FLDBOOKINGSTATUS" };
            string[] alCaptions = { "Reference No.", "Hotel Name", "Vessel Name", "Requested Date", "City", "Status" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentHotelBookingFilterCriteria;

            ds = PhoenixCrewHotelBooking.HotelBookingSearch(nvc != null ? nvc.Get("txtReferenceNo") : null
                                                    , General.GetNullableInteger(nvc != null ? nvc["ucVessel"] : string.Empty)
                                                    , General.GetNullableInteger(nvc != null ? nvc["txtcityid"] : string.Empty)
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , General.GetNullableInteger(nvc != null ? nvc["ucPurpose"] : string.Empty)
                                                    , null
                                                    , null
                                                    , General.GetNullableString(nvc != null ? nvc["ddlBookingStatus"] : string.Empty)
                                                    , sortexpression, sortdirection
                                                    , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                    , gvBookingDetails.PageSize,
                                                    ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Hotel Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDREFERENCENO", "FLDHOTELNAME", "FLDVESSELNAME", "FLDREQUESTEDDATE", "FLDCITYNAME", "FLDBOOKINGSTATUS" };
            string[] alCaptions = { "Reference No.", "Hotel Name", "Vessel Name", "Requested Date", "City", "Status" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentHotelBookingFilterCriteria;

            ds = PhoenixCrewHotelBooking.HotelBookingSearch(nvc != null ? nvc.Get("txtReferenceNo") : null
                                                            , General.GetNullableInteger(nvc != null ? nvc.Get("ucVessel") : string.Empty)
                                                            , General.GetNullableInteger(nvc != null ? nvc.Get("txtcityid") : string.Empty)
                                                            , null
                                                            , null
                                                            , null
                                                            , null
                                                            , null
                                                            , null
                                                            , null
                                                            , General.GetNullableInteger(nvc != null ? nvc.Get("ucPurpose") : string.Empty)
                                                            , null
                                                            , null
                                                            , General.GetNullableString(nvc != null ? nvc.Get("ddlBookingStatus") : string.Empty)
                                                            , sortexpression, sortdirection
                                                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                            , gvBookingDetails.PageSize,
                                                            ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvBookingDetails", "Hotel Booking", alCaptions, alColumns, ds);
            
            gvBookingDetails.DataSource = ds;
            gvBookingDetails.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvBookingDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBookingDetails.CurrentPageIndex + 1;
        BindData();
    }


    protected void gvBookingDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string bookingid = ((RadLabel)e.Item.FindControl("lblBookingId")).Text;
                string ReferenceNo = ((RadLabel)e.Item.FindControl("lblReferenceNo")).Text;
                PhoenixCrewHotelBookingRequest.ReferenceNumber = ReferenceNo;

                Response.Redirect("../Crew/CrewHotelRequestGeneral.aspx?bookingid=" + bookingid, false);

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
    protected void gvBookingDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblHotelName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblHotelNameTT");
            if (lbtn != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lbtn.ClientID;
            }

            LinkButton lnkReferenceNumberName = (LinkButton)e.Item.FindControl("lnkReferenceNumberName");

            RadLabel referenceno = (RadLabel)e.Item.FindControl("lblReferenceNumber");

            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatusID");
            
            LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");
            
            if (imgFlag != null)
            {
                if ((lblstatus.Text.Equals(ViewState["REQ"].ToString())) || (lblstatus.Text.Equals(ViewState["QRY"].ToString())) || (lblstatus.Text.Equals(ViewState["WBC"].ToString())))
                {
                    imgFlag.Visible = true;

                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:orange\"><i class=\"fas fa-star-yellow\"></i></span>";
                    imgFlag.Controls.Add(html);                    
                }
                else if ((lblstatus.Text.Equals(ViewState["CON"].ToString())))
                {
                    imgFlag.Visible = true;                 
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:green\"><i class=\"fas fa-star-blue\"></i></span>";
                    imgFlag.Controls.Add(html);

                }
                else if ((lblstatus.Text.Equals(ViewState["CND"].ToString())))
                {
                    imgFlag.Visible = true;                    
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:red\"><i class=\"fas fa-star-red\"></i></span>";
                    imgFlag.Controls.Add(html);
                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvBookingDetails.Rebind();
    }

}

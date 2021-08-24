using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewHotelRequestGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            txtBudgetId.Attributes.Add("style", "display:none");
            txtOwnerBudgetgroupId.Attributes.Add("style", "display:none");
            txtOwnerBudgetId.Attributes.Add("style", "display:none");
            txtOwnerBudgetName.Attributes.Add("style", "display:none");
            txtBudgetgroupId.Attributes.Add("style", "display:none");
            confirm.Attributes.Add("style", "display:none");

            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["BOOKINGID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["VESSELID"] = null;
            ViewState["CITYID"] = null;
            ViewState["ROOMTYPEID"] = null;
            ViewState["PAYABLECHARGES"] = null;
            ViewState["COMPANYPAYABLECHARGES"] = null;
            ViewState["NOOFNIGHTS"] = null;
            ViewState["CONFIRMEDREQ"] = "0";
            ViewState["EDIT"] = "0";
            ViewState["GUESTSADDEDYN"] = "0";
            
            if (Request.QueryString["bookingid"] != null)
            {
                ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                ViewState["ACTIVE"] = "1";
            }

            DataSet ds = PhoenixRegistersHotelCharges.ListHotelCharges(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder strPayableCharges = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FLDCOMPANYPAYABLEYN"].ToString() == "1")
                    {
                        strPayableCharges = strPayableCharges.Append(dr["FLDHOTELCHARGESID"].ToString());
                        strPayableCharges.Append(",");
                    }
                }
                if (strPayableCharges.Length > 1)
                {
                    strPayableCharges.Remove(strPayableCharges.Length - 1, 1);
                }
                ViewState["COMPANYPAYABLECHARGES"] = strPayableCharges.ToString();
            }
            cblComanyPayableCharges.DataSource = ds;
            cblComanyPayableCharges.DataBind();

            if (ViewState["BOOKINGID"] != null)
            {
                ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));

            }
            if (ViewState["BOOKINGID"] == null)
                SetContextCompanyCity();
            Guidlines();
        }

        if (ViewState["BOOKINGID"] != null)
        {
            RequestDetail.Visible = true;
            GuestMenu();
            DisableControls();
        }
        else
        {
            RequestDetail.Visible = false;
        }

        BindGuest();
        MainMenu();
        BindSubMenu();
        NoOfNightsCheck();
    }


    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Hotel Request", "HOTELREQUEST");
        toolbar.AddButton("Request Details", "REQUESTDETAILS");
        if (ViewState["CONFIRMEDREQ"].ToString() == "1")
        {
            toolbar.AddButton("Quotation", "QUOTATION");
        }

        MenuHotelRequest.AccessRights = this.ViewState;
        MenuHotelRequest.MenuList = toolbar.Show();

        MenuHotelRequest.SelectedMenuIndex = 1;
    }
    private void BindSubMenu()
    {
        if (ViewState["CONFIRMEDREQ"].ToString() == "0")// && ViewState["CONFIRMEDREQ"].ToString() != "1")
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (ViewState["BOOKINGID"] != null && ViewState["EDIT"].ToString() == "1")
                toolbarsub.AddButton("Confirm Request", "CONFIRMREQUEST", ToolBarDirection.Right);
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuRequest.AccessRights = this.ViewState;
            MenuRequest.MenuList = toolbarsub.Show();
        }
        else
            MenuRequest.Visible = false;
    }


    private void SetContextCompanyCity()
    {
        DataTable dt = PhoenixCrewHotelBookingRequest.EditContextCompanyCity(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (dt.Rows.Count > 0)
        {
            ViewState["CITYID"] = dt.Rows[0]["FLDCITY"].ToString();
            ucCity.SelectedValue = ViewState["CITYID"].ToString();
            ucCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
        }
    }

    protected void MenuHotelRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? bookingid = General.GetNullableGuid(ViewState["BOOKINGID"] != null ? ViewState["BOOKINGID"].ToString().ToString() : null);

            if (CommandName.ToUpper().Equals("HOTELREQUEST"))
            {
                Response.Redirect("CrewHotelBookingRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                if (ViewState["CONFIRMEDREQ"].ToString() == "1")
                {
                    Response.Redirect("CrewHotelBookingQuotationAgentDetail.aspx?bookingid=" + ViewState["BOOKINGID"] + "&cityid=" + ViewState["CITYID"].ToString(), false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void ListHotelBookingRequest(Guid bookingid)
    {
        DataSet ds = PhoenixCrewHotelBookingRequest.ListHotelBookingRequest(bookingid);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            ucVessel.SelectedVessel = dt.Rows[0]["FLDVESSELID"].ToString();
            ucCity.SelectedValue = dt.Rows[0]["FLDCITYID"].ToString();
            ucCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
            ucPurpose.SelectedReason = dt.Rows[0]["FLDPURPOSEID"].ToString();
            ddlGuestFrom.Text = "";
            ddlGuestFrom.SelectedValue = "";
            ddlGuestFrom.SelectedValue = dt.Rows[0]["FLDGUESTFROM"].ToString();

            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["CITYID"] = dt.Rows[0]["FLDCITYID"].ToString();
            ViewState["BOOKINGID"] = dt.Rows[0]["FLDBOOKINGID"].ToString();
            ViewState["GUESTFROM"] = dt.Rows[0]["FLDGUESTFROM"].ToString();

            ucHotelRoom.SelectedRoomID = dt.Rows[0]["FLDROOMTYPEID"].ToString();
            ViewState["ROOMTYPEID"] = dt.Rows[0]["FLDROOMTYPEID"].ToString();

            if (General.GetNullableString(dt.Rows[0]["FLDROOMTYPEID"].ToString()) != null)
            {
                ViewState["EDIT"] = "1";
            }
            ViewState["CONFIRMEDREQ"] = dt.Rows[0]["FLDREQCONFIRMEDYN"].ToString();

            if (dt.Rows[0]["FLDDAYUSEONLYYN"].ToString() == "1")
                chkDayUse.Checked = true;
            else
                chkDayUse.Checked = false;

            txtNoOfRooms.Text = dt.Rows[0]["FLDNOOFROOMS"].ToString();
            txtExtraBeds.Text = dt.Rows[0]["FLDEXTRABEDS"].ToString();


            if (General.GetNullableDateTime(dt.Rows[0]["FLDCHECKINDATE"].ToString()) == null)
                txtCheckinDate.Text = DateTime.Now.ToString();
            else
                txtCheckinDate.Text = dt.Rows[0]["FLDCHECKINDATE"].ToString();

            txtCheckoutDate.Text = dt.Rows[0]["FLDCHECKOUTDATE"].ToString();

            txtTimeOfCheckIn.Text = dt.Rows[0]["FLDCHECKINDATETIME"].ToString();
            txtTimeOfCheckOut.Text = dt.Rows[0]["FLDCHECKOUTDATETIME"].ToString();

            ucPaymentmode.SelectedHard = dt.Rows[0]["FLDPAYMENTBY"].ToString();
            ViewState["PAYABLECHARGES"] = dt.Rows[0]["FLDCOMPANYACCOUNTCHARGES"].ToString();
            ucPaymentmode.SelectedHard = dt.Rows[0]["FLDPAYMENTBY"].ToString();

            CheckPayment(dt.Rows[0]["FLDPAYMENTBY"].ToString());

            txtBudgetId.Text = dt.Rows[0]["FLDBUDGETID"].ToString();
            txtBudgetName.Text = dt.Rows[0]["FLDBUDGETNAME"].ToString();
            txtBudgetCode.Text = dt.Rows[0]["FLDSUBACCOUNT"].ToString();
            txtBudgetgroupId.Text = dt.Rows[0]["FLDBUDGETGROUP"].ToString();

            txtOwnerBudgetId.Text = dt.Rows[0]["FLDOWNERBUDGETID"].ToString();
            txtOwnerBudgetCode.Text = dt.Rows[0]["FLDOWNERBUDGETCODE"].ToString();
            txtOwnerBudgetgroupId.Text = dt.Rows[0]["FLDOWNERBUDGETGROUPID"].ToString();

            BudgetCode(dt.Rows[0]["FLDPAYMENTBY"].ToString());

            if (dt.Rows[0]["FLDNOOFNIGHTS"].ToString() != null && dt.Rows[0]["FLDNOOFNIGHTS"].ToString() != "")
                txtNoOfNights.Text = dt.Rows[0]["FLDNOOFNIGHTS"].ToString();
            else
                NoOfNightsCheck();
            CheckNoOfBeds();
            if (ViewState["ROOMTYPEID"] != null)
                CheckNoOfBeds();

            PhoenixCrewHotelBookingRequest.ReferenceNumber = dt.Rows[0]["FLDREFERENCENO"].ToString();

            if (btnShowOwnerBudget != null)
            {
                if (General.GetNullableInteger(dt.Rows[0]["FLDVESSELID"].ToString()) > 0)
                    btnShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudget', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListOwnerBudget.aspx?vesselid=" + dt.Rows[0]["FLDVESSELID"].ToString() + "', true); ");
            }
        }
    }
    private void DisableControls()
    {
        ucVessel.Enabled = false;
        ucCity.Enabled = false;
        ucPurpose.Enabled = false;
        ddlGuestFrom.Enabled = false;
        ucPurpose.Enabled = false;
    }
    private void Guidlines()
    {
        btnTooltipHelp.Text = "<table> <tr><td> &nbsp;1.Please fill the basic information and save.&nbsp; </td></tr> <tr><td>&nbsp;2.Please add the guests using ' + ' button under 'Requisition Guest Details'.</td> </tr> <tr><td>&nbsp;3.Next fill the Room details and clik 'Save'.<tr><td>&nbsp;4.After all the details are filled and checked, please click 'Confirm Request'. &nbsp;</td> </tr></table>";
    }

    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            Guid? bookingid = General.GetNullableGuid(ViewState["BOOKINGID"] != null ? ViewState["BOOKINGID"].ToString().ToString() : null);
            if (CommandName.ToUpper() == "SAVE")
            {
                if (ViewState["BOOKINGID"] == null)
                {
                    if (!IsValidHotelRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        PhoenixCrewHotelBookingRequest.InsertHotelBookingRequest(
                                                 int.Parse(ucVessel.SelectedVessel)
                                               , int.Parse(ucCity.SelectedValue)
                                               , General.GetNullableInteger(ucPurpose.SelectedReason)
                                               , null
                                               , General.GetNullableInteger(ddlGuestFrom.SelectedValue)
                                               , ref bookingid);
                        if (bookingid != null)
                        {
                            ViewState["BOOKINGID"] = bookingid.ToString();
                            ViewState["GUESTFROM"] = ddlGuestFrom.SelectedValue;

                            BindGuest();
                            gvGuest.Rebind();
                            RequestDetail.Visible = true;
                            DisableControls();
                        }
                        ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));

                    }
                }
                else
                {
                    if (!IsValidHotelRequest(ViewState["BOOKINGID"].ToString()))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        StringBuilder strPayableChargeList = new StringBuilder();
                        foreach (ListItem item in cblComanyPayableCharges.Items)
                        {
                            if (item.Selected == true)
                            {
                                strPayableChargeList.Append(item.Value.ToString());
                                strPayableChargeList.Append(",");
                            }

                        }
                        if (strPayableChargeList.Length > 1)
                        {
                            strPayableChargeList.Remove(strPayableChargeList.Length - 1, 1);
                        }

                        int dayuseYN;

                        if (chkDayUse.Checked == true)
                            dayuseYN = 1;
                        else
                            dayuseYN = 0;

                        PhoenixCrewHotelBookingRequest.UpdateHotelBookingRequest(
                                   General.GetNullableGuid(ViewState["BOOKINGID"] != null ? ViewState["BOOKINGID"].ToString() : null)
                                   , General.GetNullableGuid(ViewState["ROOMTYPEID"] != null ? ViewState["ROOMTYPEID"].ToString() : null)
                                   , General.GetNullableInteger(txtNoOfBeds.Text)
                                   , General.GetNullableInteger(txtExtraBeds.Text)
                                   , General.GetNullableInteger(txtNoOfRooms.Text)
                                   , DateTime.Parse(General.GetNullableDateTime(txtCheckinDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckIn.TextWithLiterals)
                                   , DateTime.Parse(General.GetNullableDateTime(txtCheckoutDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckOut.TextWithLiterals)
                                   , General.GetNullableInteger(txtNoOfNights.Text)
                                   , General.GetNullableInteger(dayuseYN.ToString())
                                   , null
                                   , General.GetNullableInteger(ucPaymentmode.SelectedHard.ToString())
                                   , strPayableChargeList.ToString()
                                   , General.GetNullableInteger(txtBudgetId.Text)
                                   , General.GetNullableGuid(txtOwnerBudgetId.Text));
                        ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));

                        BindGuest();
                        gvGuest.Rebind();
                    }
                    ucStatus.Text = "Information Saved";
                }
            }
            if (CommandName.ToUpper() == "CONFIRMREQUEST")
            {
                if (!IsValidHotelRequest(ViewState["BOOKINGID"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to confirm Hotel Request?", "confirm", 320, 150, null, "Confirm");
                }
            }

            MainMenu();
            GuestMenu();
            BindSubMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHotelRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel is required";
        }
        if (General.GetNullableInteger(ucCity.SelectedValue) == null)
        {
            ucError.ErrorMessage = "City is required";
        }
        if (General.GetNullableInteger(ucPurpose.SelectedReason.ToString()) == null)
        {
            ucError.ErrorMessage = "Purpose is required";
        }
        if (General.GetNullableInteger(ddlGuestFrom.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Guest from is required";
        }

        return (!ucError.IsError);
    }

    private void GuestMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewHotelRequestGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvGuest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        if (ViewState["GUESTFROM"] != null && ViewState["CONFIRMEDREQ"].ToString() == "0")
        {
            if (ViewState["GUESTFROM"].ToString() == "1")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHotelBookingTravelHopList.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "&cityid=" + ViewState["CITYID"].ToString() + "&bookingid=" + ViewState["BOOKINGID"].ToString() + "'); return false;", "Add Guests", "<i class=\"fas fa-plus\"></i>", "ADD");
            if (ViewState["GUESTFROM"].ToString() == "2")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHotelBookingEmployee.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "'); return false;", "Add Guests", "<i class=\"fas fa-plus\"></i>", "ADD");
            if (ViewState["GUESTFROM"].ToString() == "3")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHotelBookingOfficeUser.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "'); return false;", "Add Guests", "<i class=\"fas fa-plus\"></i>", "ADD");
            if (ViewState["GUESTFROM"].ToString() == "4")
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewHotelBookingGuestOthers.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "'); return false;", "Add Guests", "<i class=\"fas fa-plus\"></i>", "ADD");
        }

        MenuGuests.AccessRights = this.ViewState;
        MenuGuests.MenuList = toolbar.Show();
    }

    protected void MenuGuests_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                DataSet ds = new DataSet();
                string[] alColumns = { "FLDNAME", "FLDRANKCODE", "FLDPASSPORTNO" };
                string[] alCaptions = { "Name", "Rank Code", "PassportNo" };

                string sortexpression;
                int? sortdirection = null;
                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                ds = PhoenixCrewHotelBookingRequest.CrewHotelBookingGuestSearch(ViewState["BOOKINGID"] != null ? General.GetNullableGuid(ViewState["BOOKINGID"].ToString()) : null, sortexpression, sortdirection,
                                   (int)ViewState["PAGENUMBER"],
                                   gvGuest.PageSize,
                                   ref iRowCount,
                                   ref iTotalPageCount);


                General.ShowExcel("Requisition Guest Details", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvGuest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGuest.CurrentPageIndex + 1;
        BindGuest();
    }

    protected void gvGuest_ItemCommand(object sender, GridCommandEventArgs e)
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


    protected void gvGuest_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");

            if (eb != null && ViewState["GUESTFROM"] != null)
            {
                if (ViewState["GUESTFROM"].ToString() == "4") // others
                    eb.Visible = true;

                RadLabel lblGuestId = (RadLabel)e.Item.FindControl("lblGuestId");

                if (eb != null && lblGuestId != null && lblGuestId.Text != "")
                    eb.Attributes.Add("onclick", "openNewWindow('Filter', '','" + Session["sitepath"] + "/Crew/CrewHotelBookingGuestOthers.aspx?&bookingid=" + ViewState["BOOKINGID"].ToString() + "&guestid=" + lblGuestId.Text.Trim() + "')");

            }

            if (eb != null) if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName)) eb.Visible = false;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
        }
    }



    protected void gvGuest_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblGuestId = (RadLabel)e.Item.FindControl("lblGuestId");

            if (lblGuestId != null)
            {
                PhoenixCrewHotelBookingRequest.DeleteHotelBookingGuest(General.GetNullableGuid(lblGuestId.Text.ToString()));

                BindGuest();
                gvGuest.Rebind();
                ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void BindGuest()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNAME", "FLDRANKCODE", "FLDPASSPORTNO" };
            string[] alCaptions = { "Name", "Rank Code", "PassportNo" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;

            DataSet ds = new DataSet();

            ds = PhoenixCrewHotelBookingRequest.CrewHotelBookingGuestSearch(ViewState["BOOKINGID"] != null ? General.GetNullableGuid(ViewState["BOOKINGID"].ToString()) : null, sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , gvGuest.PageSize,
                                ref iRowCount,
                                ref iTotalPageCount);

            gvGuest.DataSource = ds;
            gvGuest.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                PanelDetail.Visible = true;
                ViewState["GUESTSADDEDYN"] = "1";
            }
            else
            {
                ViewState["GUESTSADDEDYN"] = "0";
                PanelDetail.Visible = false;
            }

            General.SetPrintOptions("gvGuest", "Requisition Guest Details", alCaptions, alColumns, ds);

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {
        ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));
    }


    private bool IsValidHotelRequest(string bookingid)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["GUESTSADDEDYN"].ToString() == "0")
        {
            ucError.ErrorMessage = "Please add guest";
        }
        else
        {
            if (General.GetNullableGuid(ucHotelRoom.SelectedRoomID) == null)
            {
                ucError.ErrorMessage = "Roomtype is required";
            }
            if (General.GetNullableDateTime(txtCheckinDate.Text) == null)
            {
                ucError.ErrorMessage = "Checkin date is required";
            }
            if (General.GetNullableDateTime(txtCheckoutDate.Text) == null)
            {
                ucError.ErrorMessage = "Checkout date is required";
            }
            if (txtTimeOfCheckIn.Equals(""))
                ucError.ErrorMessage = "Time of checkin date is required.";
            else
            {
                if (!DateTime.TryParse(General.GetNullableDateTime(txtCheckinDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckIn.TextWithLiterals, out resultDate))
                    ucError.ErrorMessage = "'Time of checkin date' is not a valid time.";
            }
            if (txtTimeOfCheckOut.Equals(""))
                ucError.ErrorMessage = "Time of checkout date is required.";
            else
            {
                if (!DateTime.TryParse(General.GetNullableDateTime(txtCheckoutDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckOut.TextWithLiterals, out resultDate))
                    ucError.ErrorMessage = "Time of checkout date is not a valid time.";
                else
                {
                    if (DateTime.TryParse(General.GetNullableDateTime(txtCheckinDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckIn.TextWithLiterals, out resultDate) && DateTime.Compare(resultDate, DateTime.Parse(General.GetNullableDateTime(txtCheckoutDate.Text).Value.ToShortDateString() + " " + txtTimeOfCheckOut.TextWithLiterals)) > 0)
                        ucError.ErrorMessage = "Checkin datetime should be earlier than Checkout datetime";
                }
            }
            if (General.GetNullableInteger(ucPaymentmode.SelectedHard) == null)
            {
                ucError.ErrorMessage = "Payment mode id required";
            }
        }
        return (!ucError.IsError);
    }

    protected void ucHotelRoom_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            ViewState["ROOMTYPEID"] = General.GetNullableGuid(ucHotelRoom.SelectedRoomID);

            if (ViewState["ROOMTYPEID"] != null)
                CheckNoOfBeds();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CheckNoOfBeds()
    {
        if (ViewState["ROOMTYPEID"] != null && ViewState["ROOMTYPEID"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersHotelRoom.HotelRoomList(General.GetNullableGuid(ViewState["ROOMTYPEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtNoOfBeds.Text = ds.Tables[0].Rows[0]["FLDNOOFBEDS"].ToString();
                if (txtNoOfBeds.Text != null && txtNoOfBeds.Text != "")
                {
                    int? noofbeds = General.GetNullableInteger(txtNoOfBeds.Text);
                    if (noofbeds != null)
                    {
                        if (noofbeds <= 1)
                        {
                            txtExtraBeds.Text = "0";
                            txtExtraBeds.ReadOnly = "true";
                            txtExtraBeds.CssClass = "readonlytextbox";
                        }
                        else
                        {
                            txtExtraBeds.ReadOnly = "false";
                            txtExtraBeds.CssClass = "input";
                        }
                    }
                }
            }
        }
        else
        {
            txtNoOfBeds.Text = "0";
        }
    }
    private void PayableChargesCheck(string strhotelcharges)
    {
        if (strhotelcharges.Length > 0)
        {
            string[] PayableCharges = strhotelcharges.Split(',');
            foreach (string s in PayableCharges)
            {
                foreach (ListItem item in cblComanyPayableCharges.Items)
                {
                    if (int.Parse(s) == int.Parse(item.Value))
                        item.Selected = true;
                }
            }
        }
    }

    private bool IsValidCheckInOutDate(string checkindate, string checkoutdate)
    {
        if (General.GetNullableDateTime(checkindate) == null)
        {
            ucError.ErrorMessage = "CheckIn date is required.";
        }
        if (General.GetNullableDateTime(checkoutdate) == null)
        {
            ucError.ErrorMessage = "checkoutdate date is required.";
        }

        return (!ucError.IsError);
    }

    protected void ucPaymentmode_TextChangedEvent(object sender, EventArgs e)
    {
        CheckPayment(ucPaymentmode.SelectedHard);
        BudgetCode(ucPaymentmode.SelectedHard);
    }
    private void CheckPayment(string paymentmode)
    {

        if (paymentmode.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 185, "CMP"))//company
        {
            cblComanyPayableCharges.Enabled = true;

            if (ViewState["PAYABLECHARGES"] != null && ViewState["PAYABLECHARGES"].ToString() != "")
            {
                PayableChargesCheck(ViewState["PAYABLECHARGES"].ToString());
            }
            else if (ViewState["COMPANYPAYABLECHARGES"] != null && ViewState["COMPANYPAYABLECHARGES"].ToString() != "")
            {
                PayableChargesCheck(ViewState["COMPANYPAYABLECHARGES"].ToString());
            }
        }
        else
        {
            foreach (ListItem item in cblComanyPayableCharges.Items)
            {
                item.Selected = false;
            }
            cblComanyPayableCharges.Enabled = false;
        }
        if (ViewState["PAYABLECHARGES"] != null)
        {
        }
    }
    private void NoOfNightsCheck()
    {
        if (General.GetNullableDateTime(txtCheckinDate.Text) != null)
        {
            if (General.GetNullableDateTime(txtCheckoutDate.Text) != null)
            {
                txtNoOfNights.Text = (DateTime.Parse(txtCheckoutDate.Text) - DateTime.Parse(txtCheckinDate.Text)).TotalDays.ToString();
            }
        }
    }
    private void BudgetCode(string paymentmode)
    {
        if (paymentmode.ToUpper() == PhoenixCommonRegisters.GetHardCode(1, 185, "PEL"))//company
        {
            lblBudgetCode.Visible = false;
            lblOwnerBudgetCode.Visible = false;
            tblBudgetCode.Visible = false;
            tblOwnerBudgetCode.Visible = false;
        }
        else
        {
            lblBudgetCode.Visible = true;
            lblOwnerBudgetCode.Visible = true;
            tblBudgetCode.Visible = true;
            tblOwnerBudgetCode.Visible = true;
        }
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewHotelBookingRequest.UpdateHotelBookingRquestConfirm(General.GetNullableGuid(ViewState["BOOKINGID"] == null ? "" : ViewState["BOOKINGID"].ToString()));
            ucStatus.Text = "Request Confirmed";
            ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));
            MainMenu();
            GuestMenu();
            BindSubMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


}

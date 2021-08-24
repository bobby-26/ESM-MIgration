using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Web;
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
using System.Configuration;

public partial class CrewLandTravelRequestGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            confirm.Attributes.Add("style", "display:none");
            sendemail.Attributes.Add("style", "display:none");
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["REQUESTID"] = "";
            ViewState["CONFIRMEDREQ"] = "0";
            ViewState["SENDMAILYN"] = "";
            ViewState["AGENTEMAILID"] = "";

            if (Request.QueryString["requestid"] != null && Request.QueryString["requestid"].ToString() != "")
            {
                ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
            }

            if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            {
                EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));

            }
            if (ViewState["REQUESTID"] == null || ViewState["REQUESTID"].ToString() == "")
                SetContextCompanyCity();
            Guidlines();

            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx', true); ");
            txtVendorId.Attributes.Add("style", "visibility:hidden");
        }

        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
        {
            divReqDetail.Visible = true;
            PassengerMenu();
        }
        else
            divReqDetail.Visible = false;

        MainMenu();
        BindSubMenu();
        BindPassengers();
    }


    private void PassengerMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewLandTravelRequestGeneral.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPassengers')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        if (ViewState["CONFIRMEDREQ"] != null && ViewState["CONFIRMEDREQ"].ToString() == "0")
        {
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewLandTravelEmployee.aspx?requestid=" + ViewState["REQUESTID"].ToString() + "'); return false;", "Add Passengers", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        }

        MenuPassengers.AccessRights = this.ViewState;
        MenuPassengers.MenuList = toolbar.Show();
    }



    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Land Travel Request", "LANDTRAVELREQUEST");
        toolbar.AddButton("Request Details", "REQUESTDETAILS");
        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            toolbar.AddButton("Tariff", "TARIFF");
        MenuLandTravelRequest.AccessRights = this.ViewState;
        MenuLandTravelRequest.MenuList = toolbar.Show();
        MenuLandTravelRequest.SelectedMenuIndex = 1;
    }

    private void BindSubMenu()
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Detail", "DETAIL");

        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "" && ViewState["CONFIRMEDREQ"].ToString() == "0")
        {
            toolbarsub.AddButton("Confirm Request", "CONFIRMREQUEST", ToolBarDirection.Right);
        }
        if (ViewState["CONFIRMEDREQ"].ToString() == "0")
        {
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        if (ViewState["CONFIRMEDREQ"].ToString() == "1")
        {
            toolbarsub.AddButton("Other Charges", "OTHERCHARGES");
            toolbarsub.AddButton("Send Mail", "SENDMAIL", ToolBarDirection.Right);
        }

        MenuRequest.AccessRights = this.ViewState;
        MenuRequest.MenuList = toolbarsub.Show();
        MenuRequest.SelectedMenuIndex = 0;
    }

    private void Guidlines()
    {
        ucToolTipNW.Text = "<table> <tr><td> &nbsp;1.Please fill the basic information and save.&nbsp; </td></tr> <tr><td>&nbsp;2.Please add the passengers using ' + ' button under 'Requisition Passenger Details'.</td> </tr> <tr><td>&nbsp;3.After all the details are filled and checked, please click 'Confirm Request'. &nbsp;</td> </tr></table>";
    }

    protected void MenuLandTravelRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString().ToString() : null);

            if (CommandName.ToUpper().Equals("LANDTRAVELREQUEST"))
            {
                Response.Redirect("CrewLandTravelRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("TARIFF"))
            {
                Response.Redirect("CrewLandTravelTariff.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "" ? ViewState["REQUESTID"].ToString().ToString() : null);
            if (CommandName.ToUpper() == "SAVE")
            {
                if (ViewState["REQUESTID"] == null || ViewState["REQUESTID"].ToString() == "")
                {
                    if (!IsValidRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        string fromtime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtFromTime.TextWithLiterals).ToString();
                        string totime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtToTime.TextWithLiterals).ToString();

                        PhoenixCrewLandTravelRequest.InsertLandTravelRequest(
                                                int.Parse(ucCity.SelectedValue)
                                                , General.GetNullableString(txtFromPlace.Text)
                                                , General.GetNullableString(txtToPlace.Text)
                                                , General.GetNullableDateTime(fromtime)
                                                , General.GetNullableDateTime(totime)
                                                , General.GetNullableString(txtTypeofTransport.Text)
                                                , General.GetNullableInteger(ucTypeofDuty.SelectedHard)
                                                , General.GetNullableString(txtOtherInfo.Text)
                                                , General.GetNullableString(txtMobileNumber.Text)
                                                , Int64.Parse(txtVendorId.Text)
                                                , int.Parse(ddlcompany.SelectedCompany)
                                                , General.GetNullableInteger(ddlType.SelectedValue)
                                                , ref requestid);
                        if (requestid != null)
                        {
                            ViewState["REQUESTID"] = requestid.ToString();
                            divReqDetail.Visible = true;
                            BindPassengers();
                            gvPassengers.Rebind();
                        }
                        EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
                        ucStatus.Text = "Information Saved";
                    }
                }
                else
                {
                    if (!IsValidRequest())
                    {
                        ucError.Visible = true;
                        return;
                    }
                    else
                    {
                        string fromtime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtFromTime.TextWithLiterals).ToString();
                        string totime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtToTime.TextWithLiterals).ToString();

                        PhoenixCrewLandTravelRequest.UpdateLandTravelRequest(
                                    new Guid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString() : null)
                                    , int.Parse(ucCity.SelectedValue)
                                    , General.GetNullableString(txtFromPlace.Text)
                                    , General.GetNullableString(txtToPlace.Text)
                                  //  , General.GetNullableDateTime(ucTravelDate.Text + " " + fromtime)
                                  //  , General.GetNullableDateTime(ucTravelDate.Text + " " + totime)
                                    , General.GetNullableDateTime(fromtime)
                                    , General.GetNullableDateTime(totime)
                                    , General.GetNullableString(txtTypeofTransport.Text)
                                    , General.GetNullableInteger(ucTypeofDuty.SelectedHard)
                                    , General.GetNullableString(txtOtherInfo.Text)
                                    , General.GetNullableString(txtMobileNumber.Text)
                                    , Int64.Parse(txtVendorId.Text)
                                    , int.Parse(ddlcompany.SelectedCompany)
                                    , General.GetNullableInteger(ddlType.SelectedValue));
                        EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
                    }
                    ucStatus.Text = "Information Saved";
                }
            }

            if (CommandName.ToUpper() == "CONFIRMREQUEST")
            {
                RadWindowManager1.RadConfirm("You will not be able to make any changes. Are you sure you want to confirm Land travel Request?", "confirm", 320, 150, null, "Confirm");
            }

            if (CommandName.ToUpper() == "SENDMAIL")
            {
                RadWindowManager1.RadConfirm("Are you sure you want to send Mail to Travel Agent?", "sendemail", 320, 150, null, "Send Email");
            }

            if (CommandName.ToUpper() == "OTHERCHARGES")
            {
                Response.Redirect("CrewLandTravelOtherCharges.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }

            MainMenu();
            PassengerMenu();
            BindSubMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuPassengers_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
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


    protected void ShowExcel()
    {
        try
        {
            string[] alColumns = { "FLDPASSENGERNAME", "FLDDESIGNATIONNAME", "FLDREASON", "FLDBUDGET", "FLDTENTATIVEVESSELNAME", "FLDPAYABLENAME", "FLDCOST" };
            string[] alCaptions = { "Passenger Name", "Designation", "Reason", "Budget Code", "Tentative Vessel", "Payable by", "Cost" };

            DataSet ds = new DataSet();

            ds = PhoenixCrewLandTravelPassengers.LandTravelPassengersSearch(ViewState["REQUESTID"] != null ? General.GetNullableGuid(ViewState["REQUESTID"].ToString()) : null);

            General.ShowExcel("Land Travel Passengers", ds.Tables[0], alColumns, alCaptions, null, null);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindPassengers()
    {
        try
        {
            string[] alColumns = { "FLDPASSENGERNAME", "FLDDESIGNATIONNAME", "FLDREASON", "FLDBUDGET", "FLDTENTATIVEVESSELNAME", "FLDPAYABLENAME", "FLDCOST" };
            string[] alCaptions = { "Passenger Name", "Designation", "Reason", "Budget Code", "Tentative Vessel", "Payable by", "Cost" };

            DataSet ds = new DataSet();

            ds = PhoenixCrewLandTravelPassengers.LandTravelPassengersSearch(ViewState["REQUESTID"] != null ? General.GetNullableGuid(ViewState["REQUESTID"].ToString()) : null);

            gvPassengers.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["CONFIRMEDREQ"].ToString() == "0")
                    gvPassengers.Columns[8].Visible = false;
                else
                    gvPassengers.Columns[8].Visible = true;
            }

            General.SetPrintOptions("gvPassengers", "Land Travel Passengers", alCaptions, alColumns, ds);
            MainMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPassengers_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPassengers();
    }



    protected void gvPassengers_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }


    protected void gvPassengers_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblPassengerId = (RadLabel)e.Item.FindControl("lblPassengerId");

            if (lblPassengerId != null)
            {
                PhoenixCrewLandTravelPassengers.DeleteLandTravelPassenger(General.GetNullableGuid(lblPassengerId.Text.ToString()));
                BindPassengers();
                gvPassengers.Rebind();

                EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPassengers_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (ViewState["CONFIRMEDREQ"] != null && ViewState["CONFIRMEDREQ"].ToString().Equals("1"))
                {
                    db.Visible = false;
                }
                else
                {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
            }
        }

        if (e.Item.IsInEditMode)
        {

            UserControlLandTravelReason ucTravelReason = (UserControlLandTravelReason)e.Item.FindControl("ucTravelReason");
            DataRowView dr = (DataRowView)e.Item.DataItem;
            if (ucTravelReason != null)
            {
                ucTravelReason.ReasonList = PhoenixRegistersTravelReason.ListLandTravelReason();
                ucTravelReason.DataBind();
                ucTravelReason.SelectedReason = dr["FLDREASONFORTRAVEL"].ToString();
            }

            RadTextBox txtBudgetId = (RadTextBox)e.Item.FindControl("txtBudgetId");
            if (txtBudgetId != null) txtBudgetId.Attributes.Add("style", "visibility:hidden");

            RadTextBox txtBudgetgroupId = (RadTextBox)e.Item.FindControl("txtBudgetgroupId");
            if (txtBudgetgroupId != null) txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            UserControlVessel ucTentativeVessel = (UserControlVessel)e.Item.FindControl("ucTentativeVessel");
            if (ucTentativeVessel != null)
            {
                ucTentativeVessel.bind();
                ucTentativeVessel.SelectedVessel = dr["FLDTENTATIVEVESSELID"].ToString();
            }

            UserControlHard ucPayable = (UserControlHard)e.Item.FindControl("ucPayable");
            if (ucPayable != null)
            {
                ucPayable.bind();
                ucPayable.SelectedHard = dr["FLDPAYABLE"].ToString();
            }

        }


    }

    protected void gvPassengers_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            UserControlMaskNumber uccost = (UserControlMaskNumber)e.Item.FindControl("txtCost");
            string cost = string.Empty;
            if (uccost != null)
            {
                cost = uccost.Text.Replace(",", "").Replace("_", "");
            }
            PhoenixCrewLandTravelPassengers.UpdateLandTravelPassenger(new Guid(ViewState["REQUESTID"].ToString())
                , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblPassengerId")).Text)
                , General.GetNullableInteger(((UserControlLandTravelReason)e.Item.FindControl("ucTravelReason")).SelectedReason)
                , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetId")).Text)
                , General.GetNullableInteger(((UserControlVessel)e.Item.FindControl("ucTentativeVessel")).SelectedVessel)
                , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucPayable")).SelectedHard)
                , General.GetNullableDecimal(cost));

            BindPassengers();
            gvPassengers.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRequest()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        string fromtime = "";
        string totime = "";

        if (General.GetNullableDateTime(ucTravelDate.Text) != null && txtFromTime.Text != "")
        {
            fromtime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtFromTime.TextWithLiterals).ToString();

        }
        if (General.GetNullableDateTime(ucTravelDate.Text) != null && txtToTime.Text != "")
        {
            totime = DateTime.Parse(General.GetNullableDateTime(ucTravelDate.Text).Value.ToShortDateString() + " " + txtToTime.TextWithLiterals).ToString();
        }


        if (General.GetNullableInteger(ucCity.SelectedValue) == null)
            ucError.ErrorMessage = "City required";
        if (General.GetNullableString(txtFromPlace.Text) == null)
            ucError.ErrorMessage = "From Place is required.";
        if (General.GetNullableString(txtToPlace.Text) == null)
            ucError.ErrorMessage = "To Place is required.";
        if (General.GetNullableDateTime(ucTravelDate.Text) == null)
            ucError.ErrorMessage = "Travel Date is required.";
        if (txtFromTime.Equals(""))
            ucError.ErrorMessage = "From time is required.";
        else
        {
            if (General.GetNullableDateTime(fromtime) == null)
            {
                ucError.ErrorMessage = "From time is not a valid time.";
            }
        }
        if (totime.Equals(""))
            ucError.ErrorMessage = "To time is required.";
        else
        {
            if (General.GetNullableDateTime(totime) == null)
                ucError.ErrorMessage = "To time is not a valid time.";
        }
        if (General.GetNullableString(txtTypeofTransport.Text) == null)
            ucError.ErrorMessage = "Type of transport is required.";
        if (General.GetNullableInteger(ucTypeofDuty.SelectedHard) == null)
            ucError.ErrorMessage = "Type of duty is required.";
        if (string.IsNullOrEmpty(txtMobileNumber.Text))
            ucError.ErrorMessage = "Contact No is required.";
        if (General.GetNullableInteger(txtVendorId.Text) == null)
            ucError.ErrorMessage = "Agent is required.";

        if (General.GetNullableInteger(ddlcompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
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

    private void EditLandTravelRequest(Guid requestid)
    {
        DataTable dt = PhoenixCrewLandTravelRequest.EditLandTravelRequest(requestid);

        if (dt.Rows.Count > 0)
        {
            txtReqNo.Text = dt.Rows[0]["FLDREFERENCENO"].ToString();
            ucCity.SelectedValue = dt.Rows[0]["FLDCITYID"].ToString();
            ucCity.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
            txtFromPlace.Text = dt.Rows[0]["FLDFROMPLACE"].ToString();
            txtToPlace.Text = dt.Rows[0]["FLDTOPLACE"].ToString();
            ucTravelDate.Text = dt.Rows[0]["FLDTRAVELDATE"].ToString();
            txtFromTime.Text = dt.Rows[0]["FLDFROMTIME"].ToString();
            txtToTime.Text = dt.Rows[0]["FLDTOTIME"].ToString();
            txtTypeofTransport.Text = dt.Rows[0]["FLDTYPEOFTRANSPORT"].ToString();
            ucTypeofDuty.SelectedHard = dt.Rows[0]["FLDTYPEOFDUTY"].ToString();
            txtOtherInfo.Text = dt.Rows[0]["FLDOTHERINFORMATION"].ToString();
            txtMobileNumber.Text = dt.Rows[0]["FLDCONTACTNO"].ToString();
            txtNoofPassengers.Text = dt.Rows[0]["FLDNOOFPASSENGERS"].ToString();

            txtVendorId.Text = dt.Rows[0]["FLDADDRESSCODE"].ToString();
            txtVendorCode.Text = dt.Rows[0]["FLDAGENTCODE"].ToString();
            txtVenderName.Text = dt.Rows[0]["FLDAGENTNAME"].ToString();
            ddlType.SelectedValue = dt.Rows[0]["FLDPACKAGETYPE"].ToString();

            txtCurrency.Text = dt.Rows[0]["FLDCURRENCYCODE"].ToString();
            txtAmount.Text = dt.Rows[0]["FLDAMOUNT"].ToString();

            ddlcompany.SelectedCompany = dt.Rows[0]["FLDBILLTOCOMPANY"].ToString();
            txtInvoiceNumber.Text = dt.Rows[0]["FLDINVOICENUMBER"].ToString();
            txtpayableamount.Text = dt.Rows[0]["FLDTOTALAMOUNT"].ToString();

            ViewState["CONFIRMEDREQ"] = dt.Rows[0]["FLDCONFIRMEDYN"].ToString();
            ViewState["SENDMAILYN"] = dt.Rows[0]["FLDSENDMAILYN"].ToString();
            ViewState["AGENTEMAILID"] = dt.Rows[0]["FLDAGENTEMAILID"].ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs args)
    {
        EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixCrewLandTravelRequest.ConfirmLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
            ucStatus.Text = "Request Confirmed";
            EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
            MainMenu();
            PassengerMenu();
            BindSubMenu();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["SENDMAILYN"].ToString() == "")
            {
                if (ViewState["AGENTEMAILID"].ToString() != "")
                {
                    PhoenixMail.SendMail(ViewState["AGENTEMAILID"].ToString(), "", "", "Travel Request", PrepareEmailBodyText(), false, System.Net.Mail.MailPriority.Normal, HttpContext.Current.Session.SessionID);
                    PhoenixCrewLandTravelRequest.UpdateLandTravelRequestSendMailYn(new Guid(ViewState["REQUESTID"].ToString()), 1);
                    ucStatus.Text = "Mail successfully sent.";
                    EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));
                    MainMenu();
                    PassengerMenu();
                    BindSubMenu();
                }
                else
                {
                    ucError.ErrorMessage = "Please update the Agent mailid.";
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                ucError.ErrorMessage = "Mail already sent.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string PrepareEmailBodyText()
    {
        StringBuilder sbemailbody = new StringBuilder();

        string fromemail = "";

        if (ConfigurationManager.AppSettings["fromemail"] != null)
        {
            fromemail = ConfigurationManager.AppSettings["fromemail"].ToString();
        }

        sbemailbody.Append("This is an automated message. DO NOT REPLY to " + fromemail);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the following Travel Request");
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        sbemailbody.Append("\"<" + Session["sitepath"] + "/" + "Crew/CrewLandTravelRequestQuotation.aspx?requestid=" + ViewState["REQUESTID"].ToString() + ">\"");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("This is an automated message.");
        sbemailbody.AppendLine("Please note "+fromemail+ " is NOT monitored.");

        return sbemailbody.ToString();
    }


}

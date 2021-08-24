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
using Telerik.Web.UI;
public partial class CrewTravelRequestGeneral : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EDITROW"] = "0";
                ViewState["CURRENTROW"] = null;
                ViewState["OFFICETRAVELYN"] = null;
                ViewState["REQCANCELLED"] = PhoenixCommonRegisters.GetHardCode(1, 130, "CND");
                ViewState["EDITTRAVELREQUEST"] = null;
                ViewState["TRAVELID"] = null;

                if (Request.QueryString["travelrequestedit"] != null)
                {
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();
                }
                if(Request.QueryString["travelid"] !=null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["travelid"].ToString();
                }

                BindVesselAccount();
                SetTravel();
                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();           
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");
          
            if (ViewState["TRAVELID"] != null)
            {
                toolbar.AddButton("Travel Request", "TRAVEL");
                toolbar.AddButton("Travel Plan", "TRAVELPLAN");
                toolbar.AddButton("Quotation", "QUOTATION");
                toolbar.AddButton("Ticket", "TICKET");
                //toolbar.AddButton("Invoice", "INVOICE");
                CrewMenu.AccessRights = this.ViewState;
                CrewMenu.MenuList = toolbar.Show();
                CrewMenu.SelectedMenuIndex = 1;

                toolbar = new PhoenixToolbar();
                toolbar.AddButton("Next", "NEXT",ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                Menuapprove.Title = "Travel Request (" + PhoenixCrewTravelRequest.RequestNo + ")";
                Menuapprove.AccessRights = this.ViewState;
                Menuapprove.MenuList = toolbar.Show();
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelRequestGeneral.aspx", "Send Details", "<i class=\"fas fa-envelope\"></i>", "EMAIL");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelRequestGeneral.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDTRAVELREQUEST");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewTravelRequestGeneral.aspx", "Crew Change Plan", "<i class=\"fas fa-users-crew\"></i>", "CREWCHANGE");
            Menutravel.AccessRights = this.ViewState;           
            Menutravel.MenuList = toolbargrid.Show();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetTravel()
    {
        try
        {
            DataSet ds = null;
            if (ViewState["TRAVELID"] != null)
                ds = PhoenixCrewTravelRequest.EditTravel(new Guid(ViewState["TRAVELID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ucport.SelectedValue = ds.Tables[0].Rows[0]["FLDSEAPORTID"].ToString();
                ucport.Text = ds.Tables[0].Rows[0]["FLDPORTNAME"].ToString();
                txtDateOfCrewChange.Text = ds.Tables[0].Rows[0]["FLDDATEOFCREWCHANGE"].ToString();
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                ViewState["Vesselid"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                BindVesselAccount();

                if (ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString() != null)
                {
                    ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();
                }
                else
                {
                    ddlAccountDetails.SelectedValue = "";
                }
                ddlAccountDetails.SelectedValue = ds.Tables[0].Rows[0]["FLDVESSELACCOUNTID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewTravelRequest.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                Response.Redirect("CrewTravelRequestGeneral.aspx?from=travel&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }         
            if (CommandName.ToUpper().Equals("TICKET"))
            {
                Response.Redirect("CrewTravelQuoteTicketList.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Menuapprove_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("NEXT"))
            {
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidTrvelRequest(ucport.SelectedValue, txtDateOfCrewChange.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixCrewTravelRequest.UpdateTravel(new Guid(ViewState["TRAVELID"].ToString()), int.Parse(ucport.SelectedValue), DateTime.Parse(txtDateOfCrewChange.Text), General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                    ucStatus.Text = "Information Updated";
                    SetTravel();
                }

            }
            if (CommandName.ToUpper().Equals("GENERATETRAVELREQ"))
            {
                ucConfirm.Visible = true;
                ucConfirm.Text = "You will not be able to make any changes. Are you sure you want to generate Travel Request?";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidTrvelRequest(string strPortId, string dateofcrewchange)
    {

        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(dateofcrewchange) == null)
            ucError.ErrorMessage = "Date of Crew Change is required.";

        else if (!DateTime.TryParse(dateofcrewchange.ToString(), out resultDate))
        {
            ucError.ErrorMessage = "Date of Crew Change is not Valid.";
        }
        if (General.GetNullableInteger(strPortId) == null)
            ucError.ErrorMessage = "Crew Change Port is required";

        return (!ucError.IsError);
    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["TRAVELID"] != null)
                {
                    PhoenixCrewTravelRequest.UpdateTravelRequestGeneralReqNo(
                          General.GetNullableGuid(ViewState["TRAVELID"].ToString()));

                    Response.Redirect("CrewTravelRequest.aspx", false);
                }
                else
                {
                    ucError.ErrorMessage = "Please fill up the Travel Details and then generate request";
                    ucError.Visible = true;
                    return;
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

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER","FLDFILENO", "FLDNAME", "FLDONSIGNERYESNO", "FLDDATEOFBIRTH", "FLDPASSPORTNUMBER", "FLDOTHERVISADETAILS",
                                "FLDORIGIN", "FLDDESTINATION", "FLDTRAVELDATE", "FLDARRIVALDATE","FLDISCANCELLEDYNSTATUS"};
            string[] alCaptions = { "S.No.","File No", "Name", "On/Off-Signer", "D.O.B.", "PP No.", "VISA Details", "Origin", "Destination", "Departure",
                                    "Arrival","Status"};

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelRequest.TravelRequestGeneralSearch(
                General.GetNullableGuid(ViewState["TRAVELID"] != null ? ViewState["TRAVELID"].ToString() : ""),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvCCT.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCT.DataSource = ds.Tables[0];
                gvCCT.VirtualItemCount = iRowCount;

                if (ViewState["TRAVELREQUESTID"] == null)
                    ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
                SetRowSelection();
            }
            else
            {
                gvCCT.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindVesselAccount()
    {
     
        string vesselid = (ViewState["Vesselid"] == null) ? null : (ViewState["Vesselid"].ToString());

        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            General.GetNullableInteger(vesselid) == 0 ? null : General.GetNullableInteger(vesselid), 1);
        ddlAccountDetails.DataBind();
    }
    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    private void SetRowSelection()
    {
        try
        {
            // gvCCT.SelectedIndex = -1;
            for (int i = 0; i < gvCCT.Items.Count; i++)
            {
                if (gvCCT.Items[i].GetDataKeyValue("FLDREQUESTID").ToString().Equals(ViewState["TRAVELREQUESTID"].ToString()))
                {
                    gvCCT.Items[i].Selected = true;
                }                
            }
        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }

    private bool IsValidTravelRequest(string name, string dob, string origin, string destination, string departuredate, string arrivaldate,
                                                    string paymentmode, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (string.IsNullOrEmpty(name.Trim()))
            ucError.ErrorMessage = "Name is required.";

        if (General.GetNullableDateTime(dob) == null)
            ucError.ErrorMessage = "Date of birth is required.";

        else if (DateTime.TryParse(dob, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "Date of birth should not be a future date ";

        if (string.IsNullOrEmpty(origin.Trim()))
            ucError.ErrorMessage = "Origin is required.";

        if (string.IsNullOrEmpty(destination.Trim()))
            ucError.ErrorMessage = "Destination is required.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure date is required.";

        if (General.GetNullableDateTime(arrdateoly) != null)
        {
            if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
                ucError.ErrorMessage = "Arrival date should be greater or equal to departure date";
        }

        if (paymentmode.ToString().ToUpper() == "DUMMY" || string.IsNullOrEmpty(paymentmode.Trim()))
            ucError.ErrorMessage = "Payment mode is required.";

        return (!ucError.IsError);
    }


   
    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelRequest.SearchTravelBreakUpGeneral(
                General.GetNullableGuid(ViewState["TRAVELREQUESTID"] == null ? "" : ViewState["TRAVELREQUESTID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;               
            }
            else
            {
                gvCTBreakUp.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string purpose)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableInteger(purpose) == null)
            ucError.ErrorMessage = "Purpose is required.";

        return (!ucError.IsError);
    }


    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination, string depdateoly, string arrdateoly)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        if (General.GetNullableDateTime(depdateoly) == null)
            ucError.ErrorMessage = "Departure Date is required.";

        if (General.GetNullableDateTime(arrdateoly) != null)
        {
            if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
                ucError.ErrorMessage = "Arrival date should be later than or equal departure date";
        }

        return (!ucError.IsError);
    }


    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        if (ViewState["CURRENTROW"] != null)
        {
            int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString());
            RadTextBox txtoriginname = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginNameBreakup");
            RadTextBox txtoriginid = (RadTextBox)gvCTBreakUp.Items[ncurrentrow].FindControl("txtOriginIdBreakup");
            if (txtoriginid != null && txtoriginname != null)
            {
                txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
                txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvCCT.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravel_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EMAIL"))
            {
                DataSet ds = PhoenixCrewTravelRequest.SearchTravelPassengersDetails(new Guid(ViewState["TRAVELID"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    SendMail(ds);
                }
            }
            if (CommandName.ToUpper() == "ADDTRAVELREQUEST")
            {

                if (ViewState["Vesselid"].ToString() != null && ViewState["Vesselid"].ToString() != "")
                {

                    String scriptpopup = String.Format("javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelPlanRequestAdd.aspx?VESSELID=" + ViewState["Vesselid"].ToString() + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "');");
                    //RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "codehelp1", scriptpopup, true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "", scriptpopup, true);
                }
            }
            if (CommandName.ToUpper() == "CREWCHANGE")
            {

                if (ViewState["Vesselid"].ToString() != null && ViewState["Vesselid"].ToString() != "" && ViewState["Vesselid"].ToString() != "0")
                {
                    Response.Redirect("CrewChangeTravel.aspx?vessel=" + ViewState["Vesselid"].ToString() + "&travelid=" + ViewState["TRAVELID"].ToString() + "&port=" + ucport.SelectedValue + "&date=" + txtDateOfCrewChange.Text + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString() + "&FROM=TRAVEL", false);
                }
                else
                {
                    ucError.ErrorMessage = "Not a Valid Vessel";
                    ucError.Visible = true;
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SendMail(DataSet ds)
    {

        StringBuilder sbemailbody = new StringBuilder();
        DataRow drUserDetail = ds.Tables[1].Rows[0];

        if (ds.Tables[1].Rows.Count > 0)
        {
            if (General.GetNullableString(ds.Tables[1].Rows[0]["FLDEMAIL"].ToString()) == null)
            {
                ucError.ErrorMessage = "Email id missing.";
                ucError.Visible = true;
                return;
            }

            else
            {
                sbemailbody.Append("Dear Sir/Madam");
                sbemailbody.AppendLine();
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Travel Passengers Details of" + "   " + drUserDetail["FLDREQUISITIONNO"].ToString());

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    sbemailbody.AppendLine();
                    sbemailbody.AppendLine();
                    sbemailbody.AppendLine();
                    sbemailbody.AppendLine("Passengers From     :" + " " + dr["FLDPASSENGERSFROM"].ToString());
                    sbemailbody.AppendLine("Crew Type           :" + " " + dr["FLDCREWTYPE"].ToString());
                    sbemailbody.AppendLine("Rank                :" + " " + dr["FLDRANKNAME"].ToString());
                    sbemailbody.AppendLine("Name                :" + " " + dr["FLDNAME"].ToString());
                    sbemailbody.AppendLine("Date of Birth-Place :" + " " + dr["FLDDATEOFBIRTH"].ToString() + " - " + dr["FLDPLACEOFBIRTH"].ToString());
                    sbemailbody.AppendLine("Nationality         :" + " " + dr["FLDNATIONALITY"].ToString());
                    sbemailbody.AppendLine("Passport No.        :" + " " + dr["FLDPASSPORTNO"].ToString());
                    sbemailbody.AppendLine("Issued Valid Untill-Place :" + " " + dr["FLDDATEOFISSUE"].ToString() + " - " + dr["FLDDATEOFEXPIRY"].ToString() + " - " + dr["FLDPLACEOFISSUE"].ToString());
                    sbemailbody.AppendLine("Seaman Book No.           :" + " " + dr["FLDSEAMANBOOKNO"].ToString());
                    sbemailbody.AppendLine("Issued Valid Untill-Place :" + " " + dr["FLDSDATEOFISSUE"].ToString() + " - " + dr["FLDSDATEOFEXPIRY"].ToString() + " - " + dr["FLDSPLACEOFISSUE"].ToString());
                }
                sbemailbody.AppendLine();
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Thank you,");
                string emailbody = sbemailbody.ToString();

                PhoenixMail.SendMail(drUserDetail["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',')
                    , ""
                    , ""
                    , "Travel Passengers Details of" + "   " + drUserDetail["FLDREQUISITIONNO"].ToString()
                    , emailbody, false
                    , System.Net.Mail.MailPriority.Normal
                    , ""
                    , null);
                ucStatus.Visible = true;
                ucStatus.Text = "Email Sent";
            }
        }
    }

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;
            if (e.CommandName.ToUpper() == "DELETE")
            {
                string requestid = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;

                if (General.GetNullableGuid(requestid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelRequest(new Guid(requestid));
                    BindData();
                    gvCCT.Rebind();
                    BindDataTravelBreakUp();
                    gvCTBreakUp.Rebind();
                }
            }
            else if (e.CommandName.ToUpper() == "CANCELTRAVEL")
            {
                RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblRequestId.Text != "")
                {
                    PhoenixCrewTravelRequest.CancelTravelPassenger(new Guid(lblRequestId.Text));
                }
                BindData();
                gvCCT.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string rankid = string.Empty;
                string strarrivaldateedit = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string strdeparturedateedit = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddlampmdeparture")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlampmarrival")).SelectedValue;
                rankid = ((UserControlRank)e.Item.FindControl("ddlRankEdit")).SelectedRank.ToString();

                string strdeparturedatetimeedit = strdeparturedateedit + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = strarrivaldateedit + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelRequest(
                   ((LinkButton)e.Item.FindControl("lnkName")).Text,
                   ((RadLabel)e.Item.FindControl("lblDOB")).Text,
                   ((RadTextBox)e.Item.FindControl("txtoriginIdEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtDestinationIdedit")).Text,
                   strdeparturedatetimeedit,
                   strarrivaldatetimeedit,
                   ((UserControlHard)e.Item.FindControl("ucPaymentmode")).SelectedHard
                   , strdeparturedateedit, strarrivaldateedit
                   ))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewTravelRequest.UpdateTravelRequestGeneral(
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblTravelRequestIdedit")).Text),
                       ((LinkButton)e.Item.FindControl("lnkName")).Text,
                        General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblonoffsignerid")).Text),
                       DateTime.Parse(((RadLabel)e.Item.FindControl("lblDOB")).Text),
                       ((RadLabel)e.Item.FindControl("lblppno")).Text,
                       ((RadLabel)e.Item.FindControl("lblothervisadet")).Text,
                        int.Parse(((RadTextBox)e.Item.FindControl("txtoriginIdEdit")).Text),
                        int.Parse(((RadTextBox)e.Item.FindControl("txtDestinationIdedit")).Text),
                        DateTime.Parse(((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text),
                        General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text),
                        int.Parse(((UserControlHard)e.Item.FindControl("ucPaymentmode")).SelectedHard),
                        int.Parse(departureampm),
                        General.GetNullableInteger(arrivalampm),
                        General.GetNullableInteger(rankid)
                        );
                BindData();
                gvCCT.Rebind();
                ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelRequestId")).Text;
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                RadCheckBox chkAssignedTo = (RadCheckBox)e.Item.FindControl("chkAssignedTo");
                RadLabel lblOnSignerYN = (RadLabel)e.Item.FindControl("lblOnSignerYN");
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                ViewState["TRAVELREQUESTID"] = lblTravelRequestId.Text;
                ViewState["ROWINDEX"] = e.Item.RowIndex;
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
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

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["FEPAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                LinkButton dbEdit = (LinkButton)e.Item.FindControl("cmdEdit");

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancelTravel");
                if (cancel != null)
                {
                    cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                    cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this Request?')");
                }
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblTravelRequestId");

                RadLabel lblstatus = (RadLabel)e.Item.FindControl("lbltravelstatus");

                UserControlHard ucpayment = (UserControlHard)e.Item.FindControl("ucPaymentmode");
                DataRowView drvpayment = (DataRowView)e.Item.DataItem;
                if (ucpayment != null) ucpayment.SelectedHard = drvpayment["FLDPAYMENTMODE"].ToString();

                RadComboBox AMPMDEPARTURE = (RadComboBox)e.Item.FindControl("ddlampmdeparture");
                RadComboBox AMPMARRIVAL = (RadComboBox)e.Item.FindControl("ddlampmarrival");

                if (drvpayment != null)
                {
                    if (AMPMDEPARTURE != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        AMPMDEPARTURE.SelectedValue = drvpayment["FLDDEPARTUREAMPMID"].ToString();
                    if (AMPMARRIVAL != null && (drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drvpayment["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                        AMPMARRIVAL.SelectedValue = drvpayment["FLDARRIVALAMPMID"].ToString();
                }

                if (ViewState["EDITTRAVELREQUEST"] != null && !ViewState["EDITTRAVELREQUEST"].ToString().Equals("1"))
                {
                    if (db != null) db.Visible = false;
                }
                else
                {
                    if (db != null) db.Visible = true;
                    if (dbEdit != null) dbEdit.Visible = true;
                }

                if (lblstatus.Text == "CANCELLED")
                {
                    dbEdit.Visible = false;
                    db.Visible = false;
                    cancel.Visible = false;
                }

                RadLabel lblRankIdEdit = (RadLabel)e.Item.FindControl("lblRankIdEdit");
                RadLabel lblOfficeTravel = (RadLabel)e.Item.FindControl("lblOfficeTravelYN");
                RadLabel lblFamilyTravel = (RadLabel)e.Item.FindControl("lblFamilyTravelYN");
                RadLabel lblRankName = (RadLabel)e.Item.FindControl("lblRankName");
                UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ddlRankEdit");

                if (lblOfficeTravel != null && lblOfficeTravel.Text.ToString() == "1")
                {
                    if (ucRank != null)
                    {
                        ucRank.Visible = false;
                        lblRankName.Visible = true;
                    }
                }
                else if (lblFamilyTravel != null && lblFamilyTravel.Text.ToString() == "1")
                {
                    if (ucRank != null)
                        ucRank.Visible = false;
                }
                else
                {
                    if (ucRank != null)
                        ucRank.SelectedRank = drvpayment["FLDRANKID"].ToString();
                }
            }
            //SetKeyDownScroll(sender, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpId")).Text;

                if (General.GetNullableGuid(breakupid) != null)
                {
                    PhoenixCrewTravelRequest.DeleteTravelBreakUpGeneral(new Guid(breakupid));
                    BindDataTravelBreakUp();
                }
            }
            else if (e.CommandName.ToString().ToUpper() == "ADD")
            {
                int? employeeid = null;
                string requestid;
                string vesselid;
                if (ViewState["ROWINDEX"] != null)
                {
                    int rowIndex = General.GetNullableInteger(ViewState["ROWINDEX"].ToString()).Value-2;

                    employeeid = General.GetNullableInteger(((RadLabel)gvCCT.Items[rowIndex].FindControl("lblEmpId")).Text);
                    requestid = ((RadLabel)gvCCT.Items[rowIndex].FindControl("lblTravelRequestId")).Text;
                    vesselid = ViewState["Vesselid"].ToString();

                    string orginid = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakupAdd")).Text;
                    string destinationid = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakupAdd")).Text;
                    string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateAdd")).Text;
                    string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDateAdd")).Text;
                    string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmAdd")).SelectedValue;
                    string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmAdd")).SelectedValue;
                    string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeAdd")).SelectedReason;
                    string travelclass = ((UserControlHard)e.Item.FindControl("ucClassAdd")).SelectedHard;

                    if (!IsValidTravelBreakUp(departuredate, arrivaldate, orginid, destinationid, purposeid))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixCrewTravelRequest.InsertTravelBreakUp(
                        employeeid,
                      General.GetNullableInteger(vesselid),
                       General.GetNullableInteger(orginid),
                       General.GetNullableInteger(destinationid),
                       DateTime.Parse(departuredate),
                       General.GetNullableInteger(departureampm),
                       General.GetNullableDateTime(arrivaldate),
                       General.GetNullableInteger(arrivalampm), General.GetNullableInteger(purposeid),
                       General.GetNullableGuid(requestid),
                      General.GetNullableInteger(travelclass));

                    BindDataTravelBreakUp();
                    gvCTBreakUp.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please Select the Seafarer to create a breakup";
                    ucError.Visible = true;
                }
            }
            else if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            else if (e.CommandName.ToUpper() == "UPDATE")
            {
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakUpIdEdit")).Text;
                string purposeid = ((UserControlTravelReason)e.Item.FindControl("ucPurposeOld")).SelectedReason;
                string departuredate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrivaldate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;
                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;
                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;
                string travelclass = ((UserControlHard)e.Item.FindControl("ucClassOld")).SelectedHard;

                string strdeparturedatetimeedit = departuredate + " " + "00:00" + (departureampm == "1" ? "AM" : "PM");
                string strarrivaldatetimeedit = arrivaldate + " " + "00:00" + (arrivalampm == "1" ? "AM" : "PM");

                if (!IsValidTravelBreakUp(strdeparturedatetimeedit, strarrivaldatetimeedit, origin, destination, departuredate, arrivaldate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.UpdateTravelBreakUpGeneral(
                       new Guid(breakupid),
                       General.GetNullableInteger(purposeid), DateTime.Parse(departuredate), General.GetNullableDateTime(arrivaldate),
                      General.GetNullableInteger(origin), General.GetNullableInteger(destination),
                      General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm),
                      General.GetNullableInteger(travelclass));
                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            UserControlTravelReason ucPurposeOld = (UserControlTravelReason)e.Item.FindControl("ucPurposeOld");
            if (ucPurposeOld != null) ucPurposeOld.SelectedReason = drv["FLDPURPOSEID"].ToString();

            UserControlHard ucClassOld = ((UserControlHard)e.Item.FindControl("ucClassOld"));
            if (ucClassOld != null) ucClassOld.SelectedHard = drv["FLDTRAVELCLASS"].ToString();

            RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
            RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

            if (departureampm != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                departureampm.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
            if (arrivalampm != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                arrivalampm.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();          
        }
    }
}
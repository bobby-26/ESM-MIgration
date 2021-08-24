using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
using System.Web;

public partial class CrewHotelBookingQuotationAgentDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Hotel Request", "HOTELREQUEST");
            toolbarmain.AddButton("Request Details", "REQUESTDETAILS");
            toolbarmain.AddButton("Quotation", "Quotation");
            MenuHotelBooking.AccessRights = this.ViewState;
            MenuHotelBooking.MenuList = toolbarmain.Show();
            MenuHotelBooking.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["CITYID"] = null;
                if (Request.QueryString["cityid"] != null)
                {
                    ViewState["CITYID"] = Request.QueryString["cityid"].ToString();
                }
                if (Request.QueryString["bookingid"] != null)
                {
                    ViewState["BOOKINGID"] = Request.QueryString["bookingid"].ToString();
                    ListHotelBookingRequest(new Guid(ViewState["BOOKINGID"].ToString()));
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewHotelBookingQuotationDetail.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() +
                   "&cityid=" + ViewState["CITYID"].ToString();
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "CrewHotelBookingQuotationDetail.aspx?";
                }


                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                
                ViewState["QUOTEID"] = null;
                ViewState["DTKEY"] = null;
                
                if (Request.QueryString["quoteid"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["quoteid"].ToString();
                }

                gvHBAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingQuotationAgentDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvHBAgent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewHotelBookingAgentsAdd.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "');return false;", "Add Agent", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingQuotationAgentDetail.aspx", "Send Query", "<i class=\"fas fa-envelope\"></i>", "RFQ");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingQuotationAgentDetail.aspx", "Send Reminder", "<i class=\"fas fa-bell\"></i>", "RFQREMAINDER");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewHotelBookingQuotationAgentDetail.aspx", "Create PO", "<i class=\"fas fa-file-alt\"></i>", "ORDER");

            MenuAgentList.AccessRights = this.ViewState;
            MenuAgentList.MenuList = toolbargrid.Show();

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
            if (CommandName.ToUpper().Equals("HOTELREQUEST"))
            {
                Response.Redirect("../Crew/CrewHotelBookingRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("REQUESTDETAILS"))
            {
                Response.Redirect("../Crew/CrewHotelRequestGeneral.aspx?bookingid=" + ViewState["BOOKINGID"], false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void MenuAgentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("RFQ"))
            {
                SendForQuotation();
            }
            if (CommandName.ToUpper().Equals("RFQREMAINDER"))
            {
                SendRemindorForQuotation();
            }
            if (CommandName.ToUpper().Equals("ORDER"))
            {
                if (!OrderApprove())
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=HOTELBOOKINGORDER&bookingid=" + ViewState["BOOKINGID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString() + "&cityid=" + ViewState["CITYID"].ToString() + "&showactual=1&showword=no&showexcel=no");
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

        string[] alColumns = { "FLDHOTELCODE", "FLDHOTELNAME", "FLDRECEIVEDDATE", "FLDAMOUNT", "FLDDISCOUNT", "FLDTOTALAMOUNT", "FLDQUOTESTATUS" };
        string[] alCaptions = { "Code", "Name", "Received Date", "Quoted Price", "Discount", "Total Amount", "Quoted" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewHotelBookingQuote.HotelBookingQuoteAgentsSearch(
            General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvHBAgent.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.ShowExcel("Hotel Request Quotation", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    private void ListHotelBookingRequest(Guid bookingid)
    {
        DataSet ds = PhoenixCrewHotelBookingRequest.ListHotelBookingRequest(bookingid);
        DataTable dt = ds.Tables[0];
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtReferenceNo.Text = PhoenixCrewHotelBookingRequest.ReferenceNumber;
            txtRoomType.Text = dt.Rows[0]["FLDROOMTYPENAME"].ToString();
            txtNoOfBeds.Text = dt.Rows[0]["FLDNOOFBEDS"].ToString();
            txtNoOfRooms.Text = dt.Rows[0]["FLDNOOFROOMS"].ToString();
            txtExtraBeds.Text = dt.Rows[0]["FLDEXTRABEDS"].ToString();
            txtCheckinDate.Text = dt.Rows[0]["FLDCHECKINDATE"].ToString();
            txtCheckoutDate.Text = dt.Rows[0]["FLDCHECKOUTDATE"].ToString();
            txtNoOfNights.Text = dt.Rows[0]["FLDNOOFNIGHTS"].ToString();
            ViewState["CITYID"] = dt.Rows[0]["FLDCITYID"].ToString();
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            txtTimeOfCheckIn.Text = dt.Rows[0]["FLDCHECKINDATETIME"].ToString();
            txtTimeOfCheckOut.Text = dt.Rows[0]["FLDCHECKOUTDATETIME"].ToString();
            if (dt.Rows[0]["FLDDAYUSEONLYYN"].ToString() == "1")
                txtDayUseYN.Text = "Yes";
            else
                txtDayUseYN.Text = "No";
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDHOTELCODE", "FLDHOTELNAME", "FLDRECEIVEDDATE", "FLDAMOUNT", "FLDDISCOUNT", "FLDTOTALAMOUNT", "FLDQUOTESTATUS" };
        string[] alCaptions = { "Code", "Name", "Received Date", "Quoted Price", "Discount", "Total Amount", "Quoted" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewHotelBookingQuote.HotelBookingQuoteAgentsSearch(
            General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
            , sortexpression, sortdirection
            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
            , gvHBAgent.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        gvHBAgent.DataSource = ds;
        gvHBAgent.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["QUOTEID"] == null || ViewState["QUOTEID"].ToString() == "0")
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                ifMoreInfo.Attributes["src"] = "CrewHotelBookingQuotationDetail.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString();              
            }         
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "CrewHotelBookingQuotationDetail.aspx?bookingid=" + ViewState["BOOKINGID"].ToString();
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvHBAgent", "Hotel Request Quotation", alCaptions, alColumns, ds);
    }
   
    private bool OrderApprove()
    {
        try
        {
            ucError.HeaderMessage = "Please provide the following required information";
            DataTable dt = PhoenixCrewHotelBookingRequest.HotelBookingSelectedHotel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["BOOKINGID"].ToString()));
            ViewState["QUOTEID"] = dt.Rows[0]["FLDQUOTEID"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
        return (!ucError.IsError);
    }

    protected void gvHBAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblQuoteStatus = (RadLabel)e.Item.FindControl("lblQuoteStatus");
            RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");
            RadLabel lblApprovalStatus = (RadLabel)e.Item.FindControl("lblApprovalStatus");
            RadLabel lblConfirmedYN = (RadLabel)e.Item.FindControl("lblConfirmedYN");
            RadLabel lblConfirmedStatus = (RadLabel)e.Item.FindControl("lblConfirmedStatus");

            LinkButton ibApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton ibDeApprove = (LinkButton)e.Item.FindControl("cmdDeApprove");
            LinkButton ibConfirm = (LinkButton)e.Item.FindControl("cmdConfirm");
            LinkButton imgSelectedQuote = (LinkButton)e.Item.FindControl("imgFlag");

            if (lblQuoteStatus != null && lblQuoteStatus.Text != "")
            {
                if (lblApprovalStatus != null && lblApprovedYN.Text == "1" && ibApprove != null && ibDeApprove != null)
                {
                    ibApprove.Visible = false;
                    if (ibDeApprove != null)
                        ibDeApprove.Visible = true;
                    lblApprovalStatus.Visible = true;

                    if (lblConfirmedYN != null && ibConfirm != null)
                    {
                        if (lblConfirmedYN.Text == "1")
                        {
                            ibConfirm.Visible = false;
                            ibDeApprove.Visible = false;
                            lblApprovalStatus.Visible = false;
                            lblConfirmedStatus.Visible = true;
                            imgSelectedQuote.Visible = true;                            
                        }
                        else
                        {
                            ibConfirm.Visible = true;
                            lblApprovalStatus.Visible = true;
                            lblConfirmedStatus.Visible = false;
                        }
                    }
                }
                else if (lblApprovalStatus != null && lblApprovedYN.Text == "0")
                {

                    if (ibApprove != null && ibConfirm != null)
                    {
                        ibApprove.Visible = true;
                        ibConfirm.Visible = false;
                    }
                    if (ibDeApprove != null)
                    {
                        ibDeApprove.Visible = false;
                    }
                }
            }
            else
            {
                if (ibApprove != null && ibConfirm != null && ibDeApprove != null)
                {
                    ibApprove.Visible = false;
                    ibConfirm.Visible = false;
                    ibDeApprove.Visible = false;
                }
            }
        }
    }


    protected void onHotelBookingQuotation(object sender, CommandEventArgs e)
    {
        try
        {
            ViewState["QUOTEID"] = ((RadLabel)gvHBAgent.Items[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblQuoteId")).Text;
            ViewState["DTKEY"] = ((RadLabel)gvHBAgent.Items[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDtKey")).Text;           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvHBAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            try
            {
                string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                
                PhoenixCrewHotelBookingQuote.ApproveHotelBooking(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                    , General.GetNullableGuid(quoteid));

                BindData();
                gvHBAgent.Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
        {
            try
            {
                string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                
                PhoenixCrewHotelBookingQuote.RevokeApproveHotelBooking(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                    , General.GetNullableGuid(quoteid));

                BindData();
                gvHBAgent.Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("CONFIRM"))
        {
            try
            {
                string quoteid = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
                
                PhoenixCrewHotelBookingQuote.ConfirmHotelBooking(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["BOOKINGID"].ToString())
                    , General.GetNullableGuid(quoteid));

                BindData();
                gvHBAgent.Rebind();
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuoteId")).Text;
            
            if (ViewState["QUOTEID"] != null)
            {
                ifMoreInfo.Attributes["src"] = "CrewHotelBookingQuotationDetail.aspx?bookingid=" + ViewState["BOOKINGID"].ToString() + "&quoteid=" + ViewState["QUOTEID"].ToString();
            }
        }

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvHBAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvHBAgent.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvHBAgent_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvHBAgent_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvHBAgent.Rebind();

    }

    private void SendForQuotation()
    {
        string emailid;
        try
        {
            string selectedagents = ",";
            foreach (GridDataItem gvr in gvHBAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblQuoteId"))).Text + ",";
                }
            }

            if (selectedagents.Length <= 1)
                selectedagents = null;
            DataSet dsagents = PhoenixCrewHotelBookingQuote.ListHotelBookingQuoteToSend(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["BOOKINGID"].ToString()), selectedagents, Int64.Parse(DateTime.Now.ToString("yyyyMMddhhmm")));
            if (dsagents.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsagents.Tables[0].Rows)
                {
                    string emailbodytext = "";

                    if (dr["FLDEMAIL1"].ToString().Contains(";"))
                        emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                    else
                        emailid = dr["FLDEMAIL1"].ToString();

                    if (!dr["FLDEMAIL2"].ToString().Equals(""))
                    {
                        emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                    }

                    try
                    {
                        if (dr["FLDRFQPREFERENCE"].ToString().Equals("WEB"))
                        {
                            PhoenixCrewHotelBookingQuote.InsertWebSession(new Guid(dr["FLDQUOTEID"].ToString())
                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , "HRFQ"
                                , PhoenixCrewHotelBooking.ReferenceNumber);

                            emailbodytext = PrepareEmailBodyText(new Guid(dr["FLDQUOTEID"].ToString()), dr["FLDREFERENCENO"].ToString(), dr["FLDFROMEMAIL"].ToString());

                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "HRFQ", new Guid(dr["FLDQUOTEID"].ToString())
                                , ""
                                , dr["FLDFROMEMAIL"].ToString()
                                , dr["FLDNAME"].ToString()
                                , "RFQ for " + dr["FLDREFERENCENO"].ToString() + ""
                                , emailbodytext
                                , ""
                                , ""
                                );
                            PhoenixCrewHotelBookingQuote.UpdateHotelBookingQuoteSendDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["BOOKINGID"].ToString()), General.GetNullableGuid(dr["FLDQUOTEID"].ToString()));

                        }
                        ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                ucConfirm.Visible = true;
            }
            else
            {
                ucConfirm.ErrorMessage = "Email already sent";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SendRemindorForQuotation()
    {
        string emailid;
        try
        {
            string selectedagents = ",";
            foreach (GridDataItem gvr in gvHBAgent.Items)
            {
                if (gvr.FindControl("chkSelect") != null && ((RadCheckBox)(gvr.FindControl("chkSelect"))).Checked == true)
                {
                    selectedagents = selectedagents + ((RadLabel)(gvr.FindControl("lblQuoteId"))).Text + ",";
                }
            }
            if (selectedagents.Length <= 1)
                selectedagents = null;
            DataSet dsagents = PhoenixCrewHotelBookingQuote.ListHotelBookingQuoteToSendReminder(General.GetNullableGuid(ViewState["BOOKINGID"].ToString()), selectedagents);

            if (dsagents.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in dsagents.Tables[0].Rows)
                {
                    string emailbodytext = "";

                    if (dr["FLDEMAIL1"].ToString().Contains(";"))
                        emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                    else
                        emailid = dr["FLDEMAIL1"].ToString();

                    if (!dr["FLDEMAIL2"].ToString().Equals(""))
                    {
                        emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                    }

                    try
                    {
                        if (dr["FLDRFQPREFERENCE"].ToString().Equals("WEB"))
                        {
                            PhoenixCrewHotelBookingQuote.InsertWebSession(new Guid(dr["FLDQUOTEID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, "HRFQ", PhoenixCrewHotelBooking.ReferenceNumber);
                            emailbodytext = PrepareEmailBodyTextForRemainder(new Guid(dr["FLDQUOTEID"].ToString()), dr["FLDREFERENCENO"].ToString(), dr["FLDFROMEMAIL"].ToString());
                            PhoenixCommoneProcessing.PrepareEmailMessage(
                                emailid, "HRFQ", new Guid(dr["FLDQUOTEID"].ToString())
                                , ""
                                , dr["FLDFROMEMAIL"].ToString()
                                , dr["FLDNAME"].ToString()
                                , "HRFQ Reminder for " + dr["FLDREFERENCENO"].ToString() + ""
                                , emailbodytext
                                , ""
                                , ""
                                );
                        }
                        ucConfirm.ErrorMessage = "Reminder email sent to " + dr["FLDNAME"].ToString() + "\n";
                    }
                    catch (Exception ex)
                    {
                        ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                    }
                }
                ucConfirm.Visible = true;
            }
            else
            {
                ucConfirm.ErrorMessage = "There are no hotels to whom you need to send a reminder. All the hotels have quoted";
                ucConfirm.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected string PrepareEmailBodyText(Guid quoteid, string referenceno, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewHotelBookingQuote.GetHotelBookingQuotateDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quoteid, "HRFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append(dr["FLDHOTELNAME"].ToString() + "             " + referenceno);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"]+" hereby requests you to provide your BEST quotation for the following hotel rooms in the given link");
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();
        sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Contact: " + frommailid);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }

    protected string PrepareEmailBodyTextForRemainder(Guid quotationid, string refreenceno, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixCrewHotelBookingQuote.GetHotelBookingQuotateDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "HRFQ");
        DataRow dr = ds.Tables[0].Rows[0];

        sbemailbody.Append(dr["FLDHOTELNAME"].ToString() + "             " + refreenceno);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Reminder: Awaiting your Quotation.");
        sbemailbody.AppendLine("Reply as soon as possible");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Contact: " + frommailid);
        sbemailbody.AppendLine();
        sbemailbody.Append("--------------------------------------------------------------------");

        sbemailbody.Append(dr["FLDHOTELNAME"].ToString() + "             " + refreenceno);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + " hereby requests you to provide your BEST quotation for the following hotel rooms in the given link");
        sbemailbody.AppendLine(dr["FLDVESSELNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.Append("Request your IT department to kindly allow access to this URL for submitting quotes.");
        sbemailbody.AppendLine();
        sbemailbody.Append("Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser");
        sbemailbody.AppendLine();
        string url = "\"<" + Session["sitepath"] + "/" + dr["URL"].ToString();

        sbemailbody.AppendLine(url + "?SESSIONID=" + dr["SESSIONID"].ToString());

        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.Append(dr["FLDUSERNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("For and on behalf of " + dr["FLDCOMPANYNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Contact: " + frommailid);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();

    }



}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewCommon;
using System.Text.RegularExpressions;
using Telerik.Web.UI;
public partial class CrewTravelQuotationGeneral : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Confirm Quote", "CONFIRMQUOTE", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddFontAwesomeButton("javascript:showDialog();", "Notes", "<i class=\"fas fa-info-circle\"></i>", "NOTES");
            MenuQuotationLineItem.AccessRights = this.ViewState;
            MenuQuotationLineItem.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                ViewState["finalizedyn"] = 0;//true active
                ucCurrency.SelectedCurrency = PhoenixCrewTravelQuote.DefaultINRCurrency;
                ViewState["finalizedyn"] = Request.QueryString["active"] == null ? "0" : Request.QueryString["active"];
                ViewState["TRAVELAGENTID"] = "";
                ViewState["REQUISITIONNO"] = "";

                if (Request.QueryString["SESSIONID"] != null)
                {
                    ViewState["SESSIONID"] = Request.QueryString["SESSIONID"].ToString();
                    ViewState["TRAVELAGENTID"] = Request.QueryString["SESSIONID"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                if (Request.QueryString["QUOTEID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                    CrewTravelQuotation();
                }
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                }
                if (Request.QueryString["AGENTID"] != null)
                {
                    ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
                }


            }
            if (ViewState["QUOTEID"] == null)
            {
                BindPassengerDetails();
                ViewState["SAVESTATUS"] = "NEW";

            }
            else
            {
                BindRoutingDetails();

                if (!ViewState["finalizedyn"].ToString().Equals("1"))
                {
                    PhoenixToolbar toolbar1 = new PhoenixToolbar();
                    toolbar1.AddFontAwesomeButton("../Crew/CrewTravelQuotationGeneral.aspx", "Copy Travel", "<i class=\"far fa-copy\"></i>", "COPY");
                    toolbar1.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelAgentQuotationChat.aspx?OFFICE=N&TravelId=" + ViewState["TRAVELID"] + "&TravelAgentId=" + ViewState["TRAVELAGENTID"].ToString() + "&title=" + ViewState["REQUISITIONNO"].ToString() + "')", "Quotation Chat", "<i class=\"fas fa-comments\"></i>", "");
                    MenuCopy.AccessRights = this.ViewState;
                    MenuCopy.MenuList = toolbar1.Show();
                }
                if (ViewState["finalizedyn"].ToString().Equals("1"))
                {
                    PhoenixToolbar toolbar2 = new PhoenixToolbar();
                    toolbar2.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewTravelAgentQuotationChat.aspx?OFFICE=N&TravelId=" + ViewState["TRAVELID"] + "&TravelAgentId=" + ViewState["TRAVELAGENTID"].ToString() + "&title=" + ViewState["REQUISITIONNO"].ToString() + "')", "Quotation Chat", "<i class=\"fas fa-comments\"></i>", "");
                    MenuCopy.AccessRights = this.ViewState;
                    MenuCopy.MenuList = toolbar2.Show();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                StringBuilder strrouteid = new StringBuilder();

                foreach (GridDataItem gvr in gvLineItem.Items)
                {
                    RadCheckBox chkAssignedTo = (RadCheckBox)gvr.FindControl("chkAssignedTo");

                    if (chkAssignedTo.Checked == true)
                    {
                        RadLabel lblrouteid = (RadLabel)gvr.FindControl("lblRouteID");

                        strrouteid.Append(lblrouteid.Text);
                        strrouteid.Append(",");
                    }
                }

                if (strrouteid.Length > 1)
                {
                    strrouteid.Remove(strrouteid.Length - 1, 1);
                }
                else
                {
                    ucError.ErrorMessage = "Please select passenger to copy the travel details";
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["ROUTEID"] != null && ViewState["ROUTEID"].ToString() != "")
                {
                    PhoenixCrewTravelQuoteLine.CopyQuotationLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()),
                                                new Guid(ViewState["ROUTEID"].ToString()), strrouteid.ToString());

                    String script = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "script", script, true);
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
    private void SetRowSelection()
    {
        try
        {
            for (int i = 0; i < gvLineItem.MasterTableView.Items.Count; i++)
            {
                RadCheckBox chk = (RadCheckBox)gvLineItem.MasterTableView.Items[i].FindControl("chkAssignedTo");
                if (gvLineItem.MasterTableView.Items[i].GetDataKeyValue("FLDROUTEID").ToString().Equals(ViewState["ROUTEID"].ToString()))
                {
                    gvLineItem.MasterTableView.Items[i].Selected = true;

                    if (chk != null) { chk.Checked = false; chk.Enabled = false; }
                }
                else
                {
                    if (chk != null) { chk.Enabled = true; }
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
    protected void chkAssignedTo_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            SetRowSelection();
            return;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CrewTravelQuotation()
    {
        DataSet ds = PhoenixCrewTravelQuote.EditCrewTravelQuotation(General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtQuotationReference.Text = dr["FLDQUOTATIONREFNO"].ToString();
            ucCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            chkmarinefare.Checked = dr["FLDISMARINEFAREYN"].ToString() == "0" ? false : true;
        }
    }
    protected void MenuQuotationLineItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.Equals("NEW"))
            {

                ClearTextForInserQuotation();
                ViewState["SAVESTATUS"] = "NEW";
                ViewState["QUOTEID"] = null;
            }
            if (CommandName.Equals("SAVE"))
            {
                if (!IsValidTravel("0", "1", "1", "1"))
                {
                    BindRoutingDetails();
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["SAVESTATUS"].ToString() == "NEW")
                {
                    InsertQuotationWithRout(txtQuotationReference.Text, int.Parse(ucCurrency.SelectedCurrency)
                        , General.GetNullableInteger(chkmarinefare.Checked ? "1" : "0"));

                }
                if (ViewState["SAVESTATUS"].ToString() == "EDIT")
                {
                    UpdateQuotationWithRout(txtQuotationReference.Text, int.Parse(ucCurrency.SelectedCurrency)
                        , General.GetNullableInteger(chkmarinefare.Checked ? "1" : "0"));
                }
                BindRoutingDetails();
                CrewTravelQuotation();
                ViewState["SAVESTATUS"] = "EDIT";

                String script = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", script, true);
            }
            if (CommandName.Equals("CONFIRMQUOTE"))
            {

                PhoenixCrewTravelQuote.FinalizeQuotationForAgent(General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));

                QuotationConfirmationSent();
                PhoenixCrewTravelQuote.UpdateQuotationConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), General.GetNullableInteger(ViewState["AGENTID"].ToString()), "1", "CONFIRMED"
                                , General.GetNullableGuid(ViewState["QUOTEID"] == null ? null : ViewState["QUOTEID"].ToString()));

                String script = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", script, true);

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void QuotationConfirmationSent()
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelQuote.QuotationConfirmation(new Guid(ViewState["QUOTEID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareApprovalText(ds.Tables[0]);
                DataRow dr = ds.Tables[0].Rows[0];
                PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(','),
                    null,
                    null,
                    dr["FLDSUBJECT"].ToString(),
                    emailbodytext,
                    true,
                    System.Net.Mail.MailPriority.Normal,
                    "", null,
                    null);
            }
            ucError.HeaderMessage = "";
            ucError.Text = "Quotation Confirmed Successfully.";
            ucError.Visible = true;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected string PrepareApprovalText(DataTable dt)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataRow dr = dt.Rows[0];
        sbemailbody.Append("<pre>");
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear " + dr["FLDCREATEDBY"].ToString() + ",");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Quotation is Received from  " + dr["FLDNAME"].ToString());
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");

        return sbemailbody.ToString();

    }
    private void ClearTextForInserQuotation()
    {
        chkmarinefare.Checked = false;
        txtQuotationReference.Text = "";
        txtQuotationReference.Enabled = true;
        ucCurrency.SelectedCurrency = PhoenixCrewTravelQuote.DefaultINRCurrency;
        BindPassengerDetails();

    }
    private void BindPassengerDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelPassengerSearchForAgent(new Guid(ViewState["TRAVELID"].ToString())
                                                                            , null
                                                                            , null
                                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                            , gvLineItem.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        gvLineItem.DataSource = ds.Tables[0];
        gvLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.Columns[0].Visible = false;
            for (int i = 10; i < gvLineItem.Columns.Count; i++)
            {
                gvLineItem.Columns[i].Visible = false;
            }

            ViewState["TRAVELREQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
            ViewState["REQUISITIONNO"] = ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString();

            ViewState["SAVESTATUS"] = "NEW";
            txtQuotationReference.Enabled = true;

            gvLineItem.Columns[gvLineItem.Columns.Count - 1].Visible = false;

        }

        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    private bool IsQuoteFinalized(string quoteid)
    {
        if (quoteid != null)
        {
            DataTable dt = PhoenixCrewTravelQuote.IsQuoteFinalized(new Guid(quoteid));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["FLDFINALIZEDYN"].ToString() == string.Empty)
                    return false;
                else
                    return true;
            }
        }
        return false;
    }
    private void CheckWebSessionStatus()
    {
        DataTable dt = PhoenixCommoneProcessing.GetSessionStatus(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ViewState["WEBSESSIONSTATUS"] = dt.Rows[0]["FLDACTIVE"].ToString();
        }
        else
        {
            ViewState["WEBSESSIONSTATUS"] = "N";
        }
    }

    private bool IsValidQuotation(string quotationno, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (quotationno.Equals(string.Empty))
            ucError.ErrorMessage = "Quotation Number is required.";
        Int16 resultint;
        ucError.HeaderMessage = "Please provide the following required information";
        if (currency.Equals("Dummy")) currency = string.Empty;

        if (currency.Equals("") || !Int16.TryParse(currency, out resultint))
            ucError.ErrorMessage = "Currency is required.";

        return (!ucError.IsError);
    }
    private void InsertQuotation(string quotationno, int currency)
    {
        Guid? Quoteid = new Guid();
        PhoenixCrewTravelQuote.InsertCrewTravelQuotationByAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["TRAVELREQUESTID"].ToString()), Convert.ToInt32(ViewState["AGENTID"].ToString())
            , quotationno, "Request", currency, new Guid(Request.QueryString["SESSIONID"].ToString()), 13, ref Quoteid);
        ViewState["QUOTEID"] = Quoteid;

    }

    private void InsertQuotationWithRout(string quotationno, int currency, int? marineyn)
    {
        if (ViewState["TRAVELREQUESTID"] != null)
        {
            Guid? Quoteid = new Guid();
            PhoenixCrewTravelQuote.InsertCrewTravelQuotationWithLine
                (PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["TRAVELID"].ToString()),
                new Guid(ViewState["TRAVELREQUESTID"].ToString()),
                Convert.ToInt32(ViewState["AGENTID"].ToString()),
                quotationno,
                "Request",
                currency,
                new Guid(Request.QueryString["SESSIONID"].ToString()),
                marineyn,
                ref Quoteid);


            ViewState["QUOTEID"] = Quoteid;
        }

    }

    private void UpdateQuotationWithRout(string quotationno, int currency, int? marinefareyn)
    {
        Guid? Quoteid = new Guid();
        PhoenixCrewTravelQuote.UpdateCrewTravelQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["QUOTEID"].ToString()),
             quotationno, currency, marinefareyn);


        ViewState["QUOTEID"] = Quoteid;

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        BindRoutingDetails();
        gvLineItem.Rebind();
    }
    private void BindRoutingDetails()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
            string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Amount" };
            if (ViewState["QUOTEID"] != null)
            {
                DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearchForAgent(new Guid(ViewState["TRAVELID"].ToString())
                                , new Guid(ViewState["QUOTEID"].ToString())
                                , General.GetNullableInteger(ViewState["AGENTID"] == null ? null : ViewState["AGENTID"].ToString())
                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                , gvLineItem.PageSize
                                , ref iRowCount
                                , ref iTotalPageCount);

                gvLineItem.DataSource = ds.Tables[0];
                gvLineItem.VirtualItemCount = iRowCount;

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ViewState["ROUTEID"] == null)
                        ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
                    SetRowSelection();
                    ViewState["SAVESTATUS"] = "EDIT";
                    ViewState["REQUISITIONNO"] = ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString();
                }

                General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);
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

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "VENDORDETAILS")
            {
                string lblrequestid = ((RadLabel)e.Item.FindControl("lblRequestID")).Text;
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewTravelPassengerInfo.aspx?Requestid=" + lblrequestid + "',false,700,500);");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName.ToUpper() == "SAVE")
            {
                string quoteid = ((RadLabel)e.Item.FindControl("lblQuotationIDEdit")).Text;
                string travelid = ((RadLabel)e.Item.FindControl("lblTravelIDEdit")).Text;
                string requestid = ((RadLabel)e.Item.FindControl("lblRequestIDEdit")).Text;
                string breakupid = ((RadLabel)e.Item.FindControl("lblBreakupIDEdit")).Text;
                string routeid = ((RadLabel)e.Item.FindControl("lblRouteIDEdit")).Text;

                string origin = ((RadLabel)e.Item.FindControl("txtOrigin")).Text;
                string destination = ((RadLabel)e.Item.FindControl("txtDestination")).Text;
                string departuredatetime = ((UserControlDate)e.Item.FindControl("lblDepartureDateEdit")).Text + " " + ((RadMaskedTextBox)e.Item.FindControl("txtDepartureTime")).TextWithLiterals;
                string arrivaldatetime = ((UserControlDate)e.Item.FindControl("lblArrivalDateEdit")).Text + " " + ((RadMaskedTextBox)e.Item.FindControl("txtArrivalTime")).TextWithLiterals;
                string noofstops = ((UserControlMaskNumber)e.Item.FindControl("txtNoOfStops")).Text;
                string duration = ((UserControlMaskNumber)e.Item.FindControl("txtDuration")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTax")).Text;
                string routing = ((RadTextBox)e.Item.FindControl("txtRouting")).Text;
                string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlinecode")).Text;

                if (!IsValidTravelBreakUp(departuredatetime, arrivaldatetime, noofstops, duration, amount, tax, routing))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateQuotationLineItem(travelid, quoteid, routeid, noofstops, departuredatetime, arrivaldatetime, duration, amount, tax, airlinecode);

                String script = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "script", script, true);

            }
            else if (e.CommandName.ToUpper() == "SELECT")
            {
                RadLabel routid = (RadLabel)e.Item.FindControl("lblRouteID");
                ViewState["ROUTEID"] = routid.Text;

                SetRowSelection();

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

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["FEPAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;
        BindRoutingDetails();
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadTextBox txtRouting = (RadTextBox)e.Item.FindControl("txtRouting");
            if (txtRouting != null)
                txtRouting.Attributes.Add("style", "display:none");

            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");
            LinkButton lnknostop = (LinkButton)e.Item.FindControl("lnknostop");
            if (lnknostop != null)
            {
                lnknostop.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&Requestforstop=1&ROUTEID=" + lblRouteID.Text + "',false,700,500);return false;");
            }

            RadLabel lblQuoteID = (RadLabel)e.Item.FindControl("lblQuotationID");
            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            if (lnkShowReason != null)
            {
                lnkShowReason.Visible = SessionUtil.CanAccess(this.ViewState, lnkShowReason.CommandName);
            }

            RadTextBox tx = (RadTextBox)e.Item.FindControl("txtRouting");
            if (tx != null)
                tx.Attributes.Add("style", "display:none");
            if (lblQuoteID != null && lblQuoteID.Text != "")
            {
                if (IsQuoteFinalized(lblQuoteID.Text))
                    gvLineItem.Columns[9].Visible = true;
                lnkShowReason.Attributes.Add("onclick", "javascript:openNewWindow('Filter', '','" + Session["sitepath"] + "/Crew/CrewTravelRoutingDetailsPopup.aspx?VIEWONLY=false&framename=filterandsearch&ROUTEID=" + lblRouteID.Text + "',false,700,500);return false;");
            }
            else
            {
                lnkShowReason.Visible = false;
            }

            LinkButton editbtn = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblamount = (RadLabel)e.Item.FindControl("lblAmount");
            RadLabel lblarrivaldate = (RadLabel)e.Item.FindControl("lblArrivalDate");
            RadLabel lbldeparturedate = (RadLabel)e.Item.FindControl("lblDepartureDate");

            if (lblamount != null && string.IsNullOrEmpty(lblamount.Text))
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (drv != null)
                {
                    lblarrivaldate.Text = drv["FLDARRIVALDATEONLY"].ToString();
                    lbldeparturedate.Text = drv["FLDDEPARTUREDATEONLY"].ToString();
                }
            }
        }
    }

    private void UpdateQuotationLineItem(string travelid, string quouteid, string routeid, string noofstops
                    , string departuredate, string arrivaldate, string duration, string amount
                    , string tax, string airlinecode)
    {
        PhoenixCrewTravelQuoteLine.UpdateQuotationLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(routeid),
            new Guid(ViewState["TRAVELID"].ToString()), new Guid(ViewState["QUOTEID"].ToString()),
            DateTime.Parse(departuredate), DateTime.Parse(arrivaldate), int.Parse(noofstops),
            decimal.Parse(duration), General.GetNullableDecimal(amount), General.GetNullableDecimal(tax), airlinecode);

    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string noofstops, string duration, string amount, string tax, string routing)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Proper Departure date is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "Proper Arrival date is required.";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be greater or equal to departure date";

        if (noofstops.Trim().Equals(""))
            ucError.ErrorMessage = "Stops is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }
    private bool IsValidTravel(string noofstops, string duration, string amount, string tax)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (txtQuotationReference.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Quotation Number is required.";
        if (General.GetNullableInteger(ucCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is required.";
        if (noofstops.Trim().Equals(""))
            ucError.ErrorMessage = "Stops is required.";

        if (duration.Trim().Equals(""))
            ucError.ErrorMessage = "Druation is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (amount.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }


}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewTravelQuoteTicket : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnconfirm.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");

            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            cmdHiddenPick.Attributes.Add("style", "display:none");

            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
            if (Request.QueryString["AGENTID"] != null)
                ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["EDITROW"] = "0";
            ViewState["CURRENTROW"] = null;
            ViewState["ROUTEID"] = null;
            ViewState["QUOTATIONNO"] = null;
            ViewState["CURRENCY"] = null;
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("javascript:showDialog();", "Notes", "<i class=\"fas fa-info-circle\"></i>", "NOTES");
        toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbar.AddButton("Confirm Ticket", "CONFIRMTICKET", ToolBarDirection.Right);
        MenuTicket.AccessRights = this.ViewState;
        MenuTicket.MenuList = toolbar.Show();

        PhoenixToolbar toolbarhoplistgrid = new PhoenixToolbar();
        string querystring = "TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString();
        toolbarhoplistgrid.AddFontAwesomeButton("../Crew/CrewTravelQuoteTicket.aspx?" + querystring, "Attachment", "<i class=\"fas fa-paperclip-na\"></i>", "ATTACHMENTMAPPING");
        MenuHopList.AccessRights = this.ViewState;
        MenuHopList.MenuList = toolbarhoplistgrid.Show();

    }

    protected void MenuHopList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "ATTACHMENTMAPPING")
            {

                if (ViewState["TRAVELDTKEY"].ToString() != null && ViewState["TRAVELID"].ToString() != null)
                {
                    String scriptpopup = String.Format("javascript:parent.openNewWindow('NATD','','" + Session["sitepath"] + "/Crew/CrewTravelAttachment.aspx?type=TRAVEL &dtkey= " + ViewState["TRAVELDTKEY"].ToString() + "&MOD=" + PhoenixModule.CREW + "&Travelid=" + ViewState["TRAVELID"].ToString() + "&Agentid=" + ViewState["AGENTID"].ToString() + "',false,750,500);");

                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTicket_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName == "CONFIRMTICKET")
            {
                RadWindowManager1.RadConfirm("Are you sure you want to Confirm Ticket?", "btnconfirm", 320, 150, null, "Confirm");
            }
            if (CommandName == "BACK")
            {
                Response.Redirect("../Portal/PortalTravelQuote.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {

            DataSet ds = PhoenixCrewTravelQuote.QuotationConfirmTicket(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["QUOTEID"].ToString()), 1);

            BindRoutingDetails();
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Origin", "Destination", "Departure Date", "Arrival Date", "Fare" };

        string agent = (ViewState["AGENTID"] == null) ? null : (ViewState["AGENTID"].ToString());

        string travelid = (ViewState["TRAVELID"] == null) ? null : (ViewState["TRAVELID"].ToString());

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemSearch(new Guid(ViewState["TRAVELID"].ToString())
                                                , null
                                                , int.Parse(agent)
                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                , gvLineItem.PageSize
                                                , ref iRowCount
                                                , ref iTotalPageCount
                                                , 1);

        gvLineItem.DataSource = ds.Tables[0];
        gvLineItem.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["ROUTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ViewState["ROUTEID"] = ds.Tables[0].Rows[0]["FLDROUTEID"].ToString();
            }
            ViewState["QUOTATIONNO"] = ds.Tables[0].Rows[0]["FLDQUOTATIONREFNO"].ToString();
            ViewState["CURRENCY"] = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
            ViewState["TRAVELDTKEY"] = ds.Tables[0].Rows[0]["FLDTRAVELDTKEY"].ToString();
            txtQuotationReference.Text = ds.Tables[0].Rows[0]["FLDQUOTATIONREFNO"].ToString();
            txtCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
            txtReferenceNo.Text = ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString();
            SetRowSelection();
        }

        General.SetPrintOptions("gvAgentLineItem", "Quotation Line item", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    private void SetRowSelection()
    {
        try
        {
            for (int i = 0; i < gvLineItem.MasterTableView.Items.Count; i++)
            {
                if (gvLineItem.MasterTableView.Items[i].GetDataKeyValue("FLDROUTEID").ToString().Equals(ViewState["ROUTEID"].ToString()))
                {
                    gvLineItem.MasterTableView.Items[i].Selected = true;
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

    private bool IsValidTicketNo(string amount, string tax)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }

    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLineItem.CurrentPageIndex + 1;

        BindRoutingDetails();
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");
            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            lnkShowReason.Attributes.Add("onclick", "openNewWindow('Filter', '','" + Session["sitepath"] + "/Crew/CrewTravelTicketCopy.aspx?ISTICKET=1&VIEWONLY=false&framename=ifMoreInfo&ROUTEID=" + lblRouteID.Text + "')");

            RadLabel lblBreakupID = (RadLabel)e.Item.FindControl("lblBreakupID");
            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                ViewState["VESSELID"] = ViewState["VESSELID"] == null ? "0" : ViewState["VESSELID"].ToString();
                ib.Attributes.Add("onclick", "openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&MOD=" + PhoenixModule.CREW + "&VESSELID=" + ViewState["VESSELID"].ToString() + "&u=1')");
            }

            RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");

            if (lblattachmentyn != null && lb != null)
            {
                if (!lblattachmentyn.Text.Equals("1"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    ib.Controls.Add(html);
                }

            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv != null && (drv["FLDISAPPROVED"].ToString().Equals("0") || drv["FLDCANCELTICKETYN"].ToString().Equals("1")))
            {
                if (ib != null && lnkShowReason != null)
                {
                    ib.Visible = false;
                    lnkShowReason.Visible = false;
                }
            }
            else
            {
                ib.Visible = true;
                lnkShowReason.Visible = true;
            }

            LinkButton btnupdate = (LinkButton)e.Item.FindControl("cmdSave");
            LinkButton btnsave = (LinkButton)e.Item.FindControl("cmdRowSave");
            if (ViewState["ISREQUOTE"] != null && ViewState["ISREQUOTE"].ToString().Equals("1"))
            {
                if (btnupdate != null)
                {
                    btnupdate.Visible = false;
                    btnsave.Visible = true;
                }
            }
            else
            {
                if (btnupdate != null)
                {
                    btnupdate.Visible = true;
                    btnsave.Visible = false;
                }
            }
        }
    }

    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper() == "VENDORDETAILS")
            {
                string lblrequestid = ((RadLabel)e.Item.FindControl("lblRequestID")).Text;
                String scriptpopup = String.Format("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewTravelPassengerInfo.aspx?Requestid=" + lblrequestid + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (e.CommandName.ToUpper() == "SELECT")
            {

                ViewState["ROUTEID"] = ((RadLabel)e.Item.FindControl("lblRouteID")).Text;

                ViewState["QUOTATIONNO"] = ((RadLabel)e.Item.FindControl("lblQuotationRefno")).Text;
                ViewState["CURRENCY"] = ((RadLabel)e.Item.FindControl("lblCurrency")).Text;

                txtQuotationReference.Text = ViewState["QUOTATIONNO"] != null ? ViewState["QUOTATIONNO"].ToString() : "";
                txtCurrency.Text = ViewState["CURRENCY"] != null ? ViewState["CURRENCY"].ToString() : "";

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();
            }
            if (e.CommandName.ToUpper() == "SAVE")
            {
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtamount")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txttax")).Text;

                if (!IsValidTicketNo(amount, tax))
                {
                    ucError.Visible = true;
                    return;
                }
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

    protected void gvLineItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string amount = ((UserControlMaskNumber)e.Item.FindControl("txtamount")).Text;
            string tax = ((UserControlMaskNumber)e.Item.FindControl("txttax")).Text;

            if (!IsValidTicketNo(amount, tax))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixCrewTravelQuoteLine.UpdateTicketNo(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(((RadLabel)e.Item.FindControl("lblRouteID")).Text)
                , new Guid(ViewState["QUOTEID"].ToString())
                , new Guid(((RadLabel)e.Item.FindControl("lblBreakupID")).Text)
                , null, Convert.ToDecimal(amount), Convert.ToDecimal(tax));

            BindRoutingDetails();
            gvLineItem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvLineItem_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindRoutingDetails();
        BindDataTravelBreakUp();
    }

    protected void gvCTBreakUp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTravelBreakUp();
    }

    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelhopLineItemSearch(
                new Guid(ViewState["TRAVELID"].ToString()), General.GetNullableGuid(ViewState["ROUTEID"] == null ? null : ViewState["ROUTEID"].ToString()));

            gvCTBreakUp.DataSource = ds;

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

            if (e.CommandName.ToUpper() == "SAVE")
            {
                string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemidedit")).Text;
                string depdate = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
                string arrdate = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;

                string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
                string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;

                string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
                string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

                string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlineCodeOld")).Text;
                string aclass = ((RadTextBox)e.Item.FindControl("txtClassOld")).Text;
                string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountOld")).Text;
                string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTaxold")).Text;
                string ticketno = ((RadLabel)e.Item.FindControl("txtTicketNoEditOld")).Text;
                string pnrno = ((RadTextBox)e.Item.FindControl("txtpnrnoOld")).Text;

                if (!IsValidTravelBreakUp(depdate, arrdate, origin, destination, airlinecode, aclass, amount, tax, ticketno, pnrno))
                {
                    ucError.Visible = true;
                    return;
                }

                string lblattachmentyn = ((RadLabel)e.Item.FindControl("lblattachmentyn")).Text;


                PhoenixCrewTravelQuoteLine.UpdateCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                             , new Guid(Hoplineitemid), Convert.ToInt32(origin), Convert.ToInt32(destination), Convert.ToDateTime(depdate)
                             , Convert.ToDateTime(arrdate)
                             , General.GetNullableInteger(departureampm), General.GetNullableInteger(arrivalampm)
                             , airlinecode, aclass, Convert.ToDecimal(amount), Convert.ToDecimal(tax), ticketno.Trim(), pnrno.Trim(), null, 1);

                if (gvCTBreakUp.EditIndexes.Count > 0)
                {
                    gvCTBreakUp.EditIndexes.Clear();
                }

                BindDataTravelBreakUp();
                gvCTBreakUp.Rebind();

            }

            if (e.CommandName.ToUpper() == "EDITROW")
            {
                ViewState["EDITROW"] = "1";

                DataRowView drv = (DataRowView)e.Item.DataItem;

                GridDataItem item = gvCTBreakUp.Items[e.CommandArgument.ToString()];
                item.Edit = true;
                gvCTBreakUp.MasterTableView.EditMode = GridEditMode.InPlace;
                gvCTBreakUp.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_UpdateCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemidedit")).Text;
            string depdateold = ((UserControlDate)e.Item.FindControl("txtDepartureDateOld")).Text;
            string arrdateold = ((UserControlDate)e.Item.FindControl("txtArrivalDateOld")).Text;

            string originold = ((RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup")).Text;
            string destinationold = ((RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup")).Text;

            string departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold")).SelectedValue;
            string arrivalampmold = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold")).SelectedValue;

            string departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampm")).SelectedValue;
            string arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm")).SelectedValue;

            string airlinecodeold = ((RadTextBox)e.Item.FindControl("txtAirlineCodeOld")).Text;
            string aclassold = ((RadTextBox)e.Item.FindControl("txtClassOld")).Text;
            string amountold = ((UserControlMaskNumber)e.Item.FindControl("txtAmountOld")).Text;
            string taxold = ((UserControlMaskNumber)e.Item.FindControl("txtTaxold")).Text;
            string ticketnoOld = ((RadLabel)e.Item.FindControl("txtTicketNoEditOld")).Text;
            string pnrnoOld = ((RadTextBox)e.Item.FindControl("txtpnrnoOld")).Text;


            string depdate = ((UserControlDate)e.Item.FindControl("txtDepartureDate")).Text;
            string arrdate = ((UserControlDate)e.Item.FindControl("txtArrivalDate")).Text;

            string origin = ((RadTextBox)e.Item.FindControl("txtOriginIdBreakup")).Text;
            string destination = ((RadTextBox)e.Item.FindControl("txtDestinationIdBreakup")).Text;

            string airlinecode = ((RadTextBox)e.Item.FindControl("txtAirlineCode")).Text;
            string aclass = ((RadTextBox)e.Item.FindControl("txtClass")).Text;
            string amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text;
            string tax = ((UserControlMaskNumber)e.Item.FindControl("txtTax")).Text;
            string ticketno = ((RadLabel)e.Item.FindControl("txtTicketNoEdit")).Text;
            string pnrno = ((RadTextBox)e.Item.FindControl("txtpnrno")).Text.Trim();

            if (!IsValidTravelBreakUp(depdateold, depdate, arrdateold, arrdate, originold, origin, destinationold, destination,
                                         airlinecodeold, airlinecode,
                                         aclassold, aclass, amountold, amount, taxold, tax, ticketnoOld, ticketno, pnrnoOld, pnrno))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixCrewTravelQuoteLine.InsertCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["TRAVELID"].ToString()), new Guid(Hoplineitemid)
                                                    , Convert.ToInt32(originold), Convert.ToInt32(origin), Convert.ToInt32(destinationold)
                                                    , Convert.ToInt32(destination)
                                                    , Convert.ToDateTime(depdateold)
                                                    , Convert.ToDateTime(depdate)
                                                    , Convert.ToDateTime(arrdateold)
                                                    , Convert.ToDateTime(arrdate)
                                                    , Convert.ToInt32(departureampmold)
                                                    , Convert.ToInt32(departureampm)
                                                    , Convert.ToInt32(arrivalampmold)
                                                    , Convert.ToInt32(arrivalampm)
                                                    , airlinecodeold.Trim()
                                                    , airlinecode.Trim()
                                                    , aclassold.Trim(), aclass.Trim(), Convert.ToDecimal(amountold), Convert.ToDecimal(amount), Convert.ToDecimal(taxold)
                                                    , Convert.ToDecimal(tax), General.GetNullableString(ticketnoOld), General.GetNullableString(ticketno), pnrnoOld.Trim(), pnrno.Trim());

            BindDataTravelBreakUp();
            gvCTBreakUp.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCTBreakUp_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        string Hoplineitemid = ((RadLabel)e.Item.FindControl("lblhoplineitemid")).Text;

        PhoenixCrewTravelQuoteLine.DeleteCrewTravelHopLineItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(Hoplineitemid));
        BindDataTravelBreakUp();
        gvCTBreakUp.Rebind();
    }

    protected void gvCTBreakUp_EditCommand(object sender, GridCommandEventArgs e)
    {
        ViewState["EDITROW"] = "0";
    }

    protected void gvCTBreakUp_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            ViewState["CURRENTROW"] = editedItem.RowIndex.ToString();

        }

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton imgTravelBreakUp = (LinkButton)e.Item.FindControl("cmdTravelBreakUp");

            RadLabel lblTravelstatus = (RadLabel)e.Item.FindControl("lblHPtravelstatus");
            RadLabel lblticketstatus = (RadLabel)e.Item.FindControl("lblTicketStatus");

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                ib.Attributes.Add("onclick", "openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&MOD=" + PhoenixModule.CREW + "')");
            }
            RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");

            if (ib != null)
            {
                if (lblattachmentyn != null && lb != null)
                {
                    if (!lblattachmentyn.Text.Equals("1"))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        ib.Controls.Add(html);
                    }

                }
            }
            LinkButton ibm = (LinkButton)e.Item.FindControl("cmdAttachmentMapping");
            RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachment");
            RadLabel ticket = (RadLabel)e.Item.FindControl("lblTicketNo");
            if (ibm != null && lbm.Text != null && ticket.Text != null)
            {
                ibm.Attributes.Add("onclick", "openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAttachmentView.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "')");
            }

            RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");

            if (ibm != null)
            {
                if (lblattachmentmappingyn != null && ibm != null)
                {
                    if (!lblattachmentmappingyn.Text.Equals("1"))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        ib.Controls.Add(html);
                    }
                }
            }

            if (lblticketstatus != null && lblticketstatus.Text.ToString() == "YES" && imgTravelBreakUp != null && eb != null)
            {
                db.Visible = false;
                imgTravelBreakUp.Visible = false;
                eb.Visible = false;
            }

            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (e.Item.ItemIndex == 0)
            {
                if (db != null)
                    db.Visible = false;
            }

            RadLabel lblTicketCancelApproved = (RadLabel)e.Item.FindControl("lblTicketCancelApproved");
            LinkButton imgTicketCancelApproved = (LinkButton)e.Item.FindControl("imgTicketCancelApproved");
            LinkButton ImgApproved = (LinkButton)e.Item.FindControl("ImgApproved");

            if (lblTicketCancelApproved != null && lblTicketCancelApproved.Text == "0")
            {
                if (imgTicketCancelApproved != null)
                    imgTicketCancelApproved.Visible = true;
                if (eb != null)
                    eb.Visible = false;
            }
            if (lblTicketCancelApproved != null && lblTicketCancelApproved.Text == "1")
            {
                if (ImgApproved != null)
                    ImgApproved.Visible = true;
            }

        }

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (ViewState["EDITROW"].ToString() == "1")
            {
                RadTextBox txtorigin = (RadTextBox)e.Item.FindControl("txtOriginIdBreakup");
                RadTextBox txtdestination = (RadTextBox)e.Item.FindControl("txtDestinationIdBreakup");
                LinkButton btnorigin = (LinkButton)e.Item.FindControl("btnShowOriginbreakup");
                LinkButton btndestination = (LinkButton)e.Item.FindControl("btnShowDestinationbreakup");
                RadTextBox txtoriginname = (RadTextBox)e.Item.FindControl("txtOriginNameBreakup");
                RadTextBox txtdestinationname = (RadTextBox)e.Item.FindControl("txtDestinationNameBreakup");

                UserControlDate txtDepartureDate = (UserControlDate)e.Item.FindControl("txtDepartureDate");
                UserControlDate txtArrivalDate = (UserControlDate)e.Item.FindControl("txtArrivalDate");


                RadTextBox txtoriginold = (RadTextBox)e.Item.FindControl("txtOriginIdOldBreakup");
                RadTextBox txtdestinationold = (RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup");
                RadTextBox txtoriginoldname = (RadTextBox)e.Item.FindControl("txtOriginNameOldBreakup");
                LinkButton btnoriginold = (LinkButton)e.Item.FindControl("btnShowOriginoldbreakup");
                LinkButton btndestinationold = (LinkButton)e.Item.FindControl("btnShowDestinationOldbreakup");

                UserControlDate txtDepartureDateOld = (UserControlDate)e.Item.FindControl("txtDepartureDateOld");
                UserControlDate txtArrivalDateOld = (UserControlDate)e.Item.FindControl("txtArrivalDateOld");

                RadComboBox departureampm = ((RadComboBox)e.Item.FindControl("ddldepartureampm"));
                RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

                RadComboBox departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
                RadComboBox arrivalampmold = ((RadComboBox)e.Item.FindControl("ddlarrivalampmold"));

                UserControlMaskNumber txtamount = (UserControlMaskNumber)e.Item.FindControl("txtAmount");
                UserControlMaskNumber txttax = (UserControlMaskNumber)e.Item.FindControl("txtTax");

                RadTextBox txtairlinecode = (RadTextBox)e.Item.FindControl("txtAirlineCode");
                RadTextBox txtclass = (RadTextBox)e.Item.FindControl("txtClass");
                RadLabel txtticket = (RadLabel)e.Item.FindControl("txtTicketNoEdit");
                RadTextBox txtpnrs = (RadTextBox)e.Item.FindControl("txtpnrno");

                LinkButton cmdRowSave = (LinkButton)e.Item.FindControl("cmdRowSave");
                LinkButton cmdSave = (LinkButton)e.Item.FindControl("cmdSave");


                if (txtorigin != null)
                {
                    txtorigin.Visible = false;
                    txtArrivalDate.Visible = false;
                    txtDepartureDate.Visible = false;
                    txtdestination.Visible = false;
                    btnorigin.Visible = false;
                    btndestination.Visible = false;
                    txtoriginname.Visible = false;
                    txtdestinationname.Visible = false;


                    if (departureampm != null)
                        departureampm.Visible = false;
                    if (arrivalampm != null)
                        arrivalampm.Visible = false;


                    txtoriginoldname.CssClass = "input_mandatory";
                    txtoriginold.Visible = false;
                    btnoriginold.Visible = false;
                    cmdRowSave.Visible = true;
                    cmdSave.Visible = false;

                    txtamount.Visible = false;
                    txttax.Visible = false;
                    txtairlinecode.Visible = false;
                    txtclass.Visible = false;
                    txtticket.Visible = false;
                    txtpnrs.Visible = false;

                    if (drv != null)
                    {
                        txtArrivalDateOld.Text = drv["FLDARRIVALDATE"].ToString();
                        txtDepartureDateOld.Text = drv["FLDDEPARTUREDATE"].ToString();

                        if (departureampmold != null && (drv["FLDDEPARTUREAMPMID"].ToString().Equals("1") || drv["FLDDEPARTUREAMPMID"].ToString().Equals("2")))
                            departureampmold.SelectedValue = drv["FLDDEPARTUREAMPMID"].ToString();
                        if (arrivalampmold != null && (drv["FLDARRIVALAMPMID"].ToString().Equals("1") || drv["FLDARRIVALAMPMID"].ToString().Equals("2")))
                            arrivalampmold.SelectedValue = drv["FLDARRIVALAMPMID"].ToString();
                    }

                }
            }

            if (ViewState["EDITROW"].ToString() == "0")
            {
                LinkButton btnoriginold = (LinkButton)e.Item.FindControl("btnShowOriginoldbreakup");
                LinkButton btnorigin = (LinkButton)e.Item.FindControl("btnShowOriginbreakup");
                LinkButton btndestination = (LinkButton)e.Item.FindControl("btnShowDestinationbreakup");
                LinkButton btndestinationold = (LinkButton)e.Item.FindControl("btnShowDestinationOldbreakup");
                UserControlDate txtDepartureDateOld = (UserControlDate)e.Item.FindControl("txtDepartureDateOld");
                UserControlDate txtArrivalDate = (UserControlDate)e.Item.FindControl("txtArrivalDate");

                RadComboBox departureampmold = ((RadComboBox)e.Item.FindControl("ddldepartureampmold"));
                RadComboBox arrivalampm = ((RadComboBox)e.Item.FindControl("ddlarrivalampm"));

                if (btnoriginold != null)
                    btnoriginold.Visible = false;
                if (btndestination != null)
                    btndestination.Visible = false;
                if (btnorigin != null)
                    btnorigin.Visible = false;

                if (txtDepartureDateOld != null)
                    txtDepartureDateOld.Enabled = false;
                if (txtArrivalDate != null)
                    txtArrivalDate.Enabled = false;


                RadTextBox txtdestinationoldname = (RadTextBox)e.Item.FindControl("txtDestinationNameOldBreakup");
                RadTextBox txtdestinationoldid = (RadTextBox)e.Item.FindControl("txtDestinationIdOldBreakup");

                if (txtdestinationoldname != null)
                    txtdestinationoldname.Text = "";
                if (txtdestinationoldid != null)
                    txtdestinationoldid.Text = "";

                UserControlMaskNumber txtamount = (UserControlMaskNumber)e.Item.FindControl("txtAmountold");
                UserControlMaskNumber txttax = (UserControlMaskNumber)e.Item.FindControl("txtTaxold");

                RadTextBox txtairlinecode = (RadTextBox)e.Item.FindControl("txtAirlineCodeold");
                RadTextBox txtclass = (RadTextBox)e.Item.FindControl("txtClassold");
                RadLabel txtticketno = (RadLabel)e.Item.FindControl("txtTicketNoEditOld");
                RadTextBox txtpnr = (RadTextBox)e.Item.FindControl("txtpnrnoOld");

                if (txtamount != null) txtamount.Text = "";
                if (txttax != null) txttax.Text = "";
                if (txtairlinecode != null) txtairlinecode.Text = "";
                if (txtclass != null) txtclass.Text = "";
                if (txtticketno != null) txtticketno.Text = "";
                if (txtpnr != null) txtpnr.Text = "";

            }

        }

    }

    private bool IsValidTravelBreakUp(string departuredateold, string departuredate, string arrivaldateold, string arrivaldate, string originold, string origin,
                                       string destinationold, string destination,
                                       string airlinecodeold, string airlinecode, string aclassold, string aclass, string amountold, string amount, string taxold, string tax,
                                       string ticketnoold, string ticketno, string pnrnoold, string pnrno)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (originold.Trim() == "" || origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";
        if (destinationold.Trim() == "" || destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";
        if (destinationold.Trim().ToString().Equals(originold.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in Ist sector.";
        if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same in 2nd sector.";

        if (General.GetNullableDateTime(departuredateold) == null || General.GetNullableDateTime(departuredateold) == null)
            ucError.ErrorMessage = "Departure Date is required.";


        if (General.GetNullableDateTime(departuredate) == null || General.GetNullableDateTime(departuredate) == null)
            ucError.ErrorMessage = "Depature Date is required";

        if (General.GetNullableDateTime(arrivaldateold) == null || General.GetNullableDateTime(arrivaldateold) == null)
            ucError.ErrorMessage = "Arrival Date is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date in 2nd sector";

        if (string.IsNullOrEmpty(pnrno.Trim()) || string.IsNullOrEmpty(pnrnoold.Trim()))
            ucError.ErrorMessage = "PNR No is required.";

        if (string.IsNullOrEmpty(airlinecode.Trim()) || string.IsNullOrEmpty(airlinecodeold.Trim()))
            ucError.ErrorMessage = "Airline code  is required.";

        if (amount.Trim().Equals("") || amountold.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (amount.Equals("0") || amountold.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";

        if (tax.Trim().Equals("") || taxold.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }

    private bool IsValidTravelBreakUp(string departuredate, string arrivaldate, string origin, string destination
                                     , string airlinecode, string aclass, string amount, string tax, string ticketno, string pnr)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (origin.Trim() == "")
            ucError.ErrorMessage = "Origin is required.";

        if (destination.Trim() == "")
            ucError.ErrorMessage = "Destination is required.";

        else if (destination.Trim().ToString().Equals(origin.Trim().ToString()))
            ucError.ErrorMessage = "Origin and Destination can not be same.";

        else if (General.GetNullableDateTime(departuredate) == null || string.IsNullOrEmpty(departuredate))
            ucError.ErrorMessage = "Departure Date is required.";

        else if (General.GetNullableDateTime(arrivaldate) == null || string.IsNullOrEmpty(arrivaldate))
            ucError.ErrorMessage = "Arrival Date is required";

        else if (DateTime.TryParse(departuredate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(arrivaldate)) > 0)
            ucError.ErrorMessage = "Arrival date should be later than departure date";

        if (string.IsNullOrEmpty(ticketno.Trim()))
            ucError.ErrorMessage = "Ticket No  is required.";

        if (string.IsNullOrEmpty(pnr.Trim()))
            ucError.ErrorMessage = "PNR No  is required.";

        if (string.IsNullOrEmpty(airlinecode.Trim()))
            ucError.ErrorMessage = "Airline code  is required.";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";

        if (amount.Equals("0"))
            ucError.ErrorMessage = "Amount must be greater than 0.";

        if (tax.Trim().Equals(""))
            ucError.ErrorMessage = "Tax is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentPickListSelection == null)
                return;

            if (ViewState["CURRENTROW"] != null)
            {
                int ncurrentrow = ViewState["CURRENTROW"] == null ? 0 : int.Parse(ViewState["CURRENTROW"].ToString());

                ncurrentrow = ncurrentrow - 2;

                for (int i = 0; i < gvLineItem.MasterTableView.Items.Count; i++)
                {
                    if (i == ncurrentrow)
                    {
                        RadTextBox txtoriginname = (RadTextBox)gvCTBreakUp.MasterTableView.Items[i].FindControl("txtOriginNameBreakup");
                        RadTextBox txtoriginid = (RadTextBox)gvCTBreakUp.MasterTableView.Items[i].FindControl("txtOriginIdBreakup");
                        if (txtoriginid != null && txtoriginname != null)
                        {
                            txtoriginname.Text = Filter.CurrentPickListSelection.Get(1);
                            txtoriginid.Text = Filter.CurrentPickListSelection.Get(2);
                        }
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

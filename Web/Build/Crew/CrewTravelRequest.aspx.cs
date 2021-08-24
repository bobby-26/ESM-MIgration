using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;

public partial class CrewTravelRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Travel Request", "TRAVEL");
            toolbar.AddButton("Travel Plan", "TRAVELPLAN");
            toolbar.AddButton("Quotation", "QUOTATION");
            toolbar.AddButton("Ticket", "TICKET");            
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbar.Show();
            CrewMenu.SelectedMenuIndex = 0;
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCCT')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','Travel Request Filter','" + Session["sitepath"] + "/Crew/CrewTravelRequestFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequest.aspx" , "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelRequest.aspx", "New Travel Request", "<i class=\"fa fa-plus-circle\"></i>", "ADD");            
            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirm.Visible = false;
                cmdHiddenSubmit.Attributes.Add("style", "display:none");


                ViewState["TRAVELID"] = null;
                ViewState["OFFICEREQUESTYN"] = null;
                ViewState["OFFICEAPPROVEDYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                
                ViewState["REQUSERCANCELLED"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 130, "CND");
                ViewState["REQUSERISSUED"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 130, "ISS");
                ViewState["REQUSERCOURSES"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 88, "CUT");
                ViewState["REQISSUED"] = PhoenixCommonRegisters.GetHardCode(1, 130, "ISS");
                ViewState["REQPENDING"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TRQ");
                ViewState["REQORDER"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TPO");
                ViewState["REQQUERY"] = PhoenixCommonRegisters.GetHardCode(1, 130, "TQY");
                ViewState["REQCANCELLED"] = PhoenixCommonRegisters.GetHardCode(1, 130, "CND");

                ViewState["EDITTRAVELREQUEST"] = "1";
                ViewState["EDITROW"] = "0";


                if (Request.QueryString["TRAVELID"] != null)
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();

                if (Request.QueryString["travelrequestedit"] != null)
                    ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();

                if (Request.QueryString["offficeapprovalreq"] != null)
                {
                    ucError.ErrorMessage = "Office Travel Request needs to be Confirmed";
                    ucError.Visible = true;
                }
                gvCCT.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

            if (ViewState["TRAVELID"] == null)
            {
                ucError.ErrorMessage = "Please select the travel request before proceed further.";
                ucError.Visible = true;
                return;
            }
            else
            {
                if (ViewState["OFFICEREQUESTYN"] != null && ViewState["OFFICEAPPROVEDYN"] != null)
                {
                    if (ViewState["OFFICEREQUESTYN"].ToString() == "1" && ViewState["OFFICEAPPROVEDYN"].ToString() == "0")
                    {
                        ucError.ErrorMessage = "Office Travel Request needs to be Confirmed";
                        ucError.Visible = true;
                        return;
                    }
                }
            }
            if (CommandName.ToUpper().Equals("CREWCHANGE"))
            {
                Response.Redirect("CrewChangeRequest.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
            }

            if (CommandName.ToUpper().Equals("VESSELLIST"))
            {
                Response.Redirect("CrewChangePlanFilter.aspx" + (Request.QueryString["access"] != null ? "?access=1" : string.Empty), false);
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

    protected void MenutravelList_TabStripCommand(object sender, EventArgs e)
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
                Filter.CurrentTravelRequestFilter = null;
                gvCCT.CurrentPageIndex = 0;         
                BindData();
                gvCCT.Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {           
                Response.Redirect("../Crew/CrewTravelRequestAdd.aspx", false);
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

        string[] alColumns = { "FLDREQUISITIONNO", "FLDVESSELNAME", "FLDDATEOFCREWCHANGE", "FLDPORTNAME", "FLDPURPOSE", "FLDSTATUS", "FLDOFFICETRAVEL", "FLDREQUESTEDDATE" };
        string[] alCaptions = { "Request No", "Vessel", "Crew Change", "Crew Change Port", "Purpose", "Status", "Office/Crew Change", "Requested" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentTravelRequestFilter;

        ds = PhoenixCrewTravelRequest.SearchTravel(
                    General.GetNullableInteger(nvc != null ? nvc.Get("vesselid") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ddlofficetravelyn") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtPassengerName") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtFileNo") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucPort") : string.Empty)
                  , sortexpression, sortdirection
                  , int.Parse(ViewState["PAGENUMBER"].ToString())
                  , gvCCT.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelRequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Request</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREQUISITIONNO", "FLDVESSELNAME", "FLDDATEOFCREWCHANGE", "FLDPORTNAME", "FLDPURPOSE", "FLDSTATUS", "FLDOFFICETRAVEL", "FLDREQUESTEDDATE" };
            string[] alCaptions = { "Request No", "Vessel", "Crew Change", "Crew Change Port", "Purpose", "Status", "Office/Crew Change", "Requested" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentTravelRequestFilter;

            DataSet ds = PhoenixCrewTravelRequest.SearchTravel(
                    General.GetNullableInteger(nvc != null ? nvc.Get("vesselid") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucpurpose") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtStartDate") : string.Empty)
                  , General.GetNullableDateTime(nvc != null ? nvc.Get("txtEndDate") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("uctravelstatus") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtOrigin") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("txtDestination") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ddlofficetravelyn") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtTravelRequestNo") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtPassengerName") : string.Empty)
                  , General.GetNullableString(nvc != null ? nvc.Get("txtFileNo") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucZone") : string.Empty)
                  , General.GetNullableInteger(nvc != null ? nvc.Get("ucPort") : string.Empty)
                  , sortexpression, sortdirection
                  , int.Parse(ViewState["PAGENUMBER"].ToString())
                  , gvCCT.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount);

            General.SetPrintOptions("gvCCT", "Travel Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCCT.DataSource = ds;
                gvCCT.VirtualItemCount = iRowCount;
                if (ViewState["TRAVELID"] == null)
                {
                    ViewState["TRAVELID"] = ds.Tables[0].Rows[0]["FLDTRAVELID"].ToString();
                    Filter.CurrentTraveltoVesselName = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                    PhoenixCrewTravelRequest.RequestNo = ds.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString();

                    ViewState["OFFICEREQUESTYN"] = ds.Tables[0].Rows[0]["FLDOFFICEREQUESTYN"].ToString();
                    ViewState["OFFICEAPPROVEDYN"] = ds.Tables[0].Rows[0]["FLDREQUESTAPPROVE"].ToString();

                    if (ds.Tables[0].Rows[0]["FLDTRAVELSTATUS"].ToString() == ViewState["REQUSERCANCELLED"].ToString() || ds.Tables[0].Rows[0]["FLDTRAVELSTATUS"].ToString() == ViewState["REQUSERISSUED"].ToString())
                        ViewState["EDITTRAVELREQUEST"] = "0";
                    else
                        ViewState["EDITTRAVELREQUEST"] = "1";
                }
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

    private void SetRowSelection()
    {
        try
        {
            for (int i = 0; i < gvCCT.MasterTableView.Items.Count; i++)
            {
                if (gvCCT.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELID").ToString().Equals(ViewState["TRAVELID"].ToString()))
                {
                    gvCCT.MasterTableView.Items[i].Selected = true;
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

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            if (ucCM.confirmboxvalue == 1)
            {
               SendMail(General.GetNullableGuid(ViewState["Travelid"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected string PrepareEmailBodyText(string formno, string sendto, string username, string passengers)
    {

        StringBuilder sbemailbody = new StringBuilder();
        sbemailbody.Append("Dear  " + sendto + " ,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(HttpContext.Current.Session["companyname"] + "  hereby inform you that, travel Request[Requisition NO : " + formno + "] has been initiated for the below crew members");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine(passengers + "<br/>");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine("<br/>");
        sbemailbody.Append(username);
        sbemailbody.AppendLine("<br/>");
        sbemailbody.AppendLine("For and on behalf of "+ HttpContext.Current.Session["companyname"]);
        sbemailbody.AppendLine("(As Agents for Owners)");
        sbemailbody.AppendLine("<br/>");

        return sbemailbody.ToString();
    }

    private void SendMail(Guid? Crewtravelid)
    {
        try
        {
            string emailbodytext = "";
            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelRequest.travelrequestmailsearch(Crewtravelid);
            DataRow dr = ds.Tables[0].Rows[0];
            if (ds.Tables[0].Rows.Count > 0)
            {
                emailbodytext = PrepareEmailBodyText(dr["FLDREQUISITIONNO"].ToString(), dr["FLDSENDTO"].ToString()
                                                    , dr["FLDSENDBY"].ToString(), dr["FLDPASSENGERS"].ToString());

                PhoenixMail.SendMail(ds.Tables[0].Rows[0]["FLDTRAVELPICEMAIL"].ToString(), ds.Tables[0].Rows[0]["FLDEMAIL2"].ToString().TrimEnd(','), null, ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString()
                                            , emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
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
            ViewState["TRAVELID"] = null;
            BindData();
            gvCCT.Rebind();
            if (ViewState["TRAVELID"] != null)
            {
                for (int i = 0; i < gvCCT.Items.Count; i++)
                {
                    if (gvCCT.Items[i].GetDataKeyValue("FLDTRAVELID").ToString() == ViewState["TRAVELID"].ToString())
                    {
                        gvCCT.Items[i].Selected = true;
                        break;
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

    protected void gvCCT_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT")
                return;

            else if (e.CommandName.ToUpper() == "SELECT")
            {
               
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                RadLabel lblTravelRequeststatusId = (RadLabel)e.Item.FindControl("lblStatusID");
                RadLabel lblVesselname = (RadLabel)e.Item.FindControl("lnkName");
                LinkButton lnkRequestNo = (LinkButton)e.Item.FindControl("lnkReqNo");
                RadLabel lblOfficeTravelYN = (RadLabel)e.Item.FindControl("lblOfficeTravelYN");
                RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");

                ViewState["TRAVELREQUESTID"] = lblTravelRequestId.Text;
                ViewState["TRAVELID"] = lblTravelRequestId.Text;
                ViewState["REQUESTID"] = lblTravelRequestId.Text;
                ViewState["OFFICEREQUESTYN"] = lblOfficeTravelYN.Text;
                ViewState["OFFICEAPPROVEDYN"] = lblApprovedYN.Text;

                Filter.CurrentTraveltoVesselName = lblVesselname.Text;
                PhoenixCrewTravelRequest.RequestNo = lnkRequestNo.Text;

                if (lblTravelRequeststatusId.Text == ViewState["REQUSERCANCELLED"].ToString() || lblTravelRequeststatusId.Text == ViewState["REQUSERISSUED"].ToString())
                    ViewState["EDITTRAVELREQUEST"] = "0";
                else
                    ViewState["EDITTRAVELREQUEST"] = "1";

                if (lblOfficeTravelYN != null && lblOfficeTravelYN.Text == "1")
                {
                    if (lblApprovedYN != null && lblApprovedYN.Text == "0")
                    {
                        ucError.ErrorMessage = "Office Travel Request needs to be Confirmed";
                        ucError.Visible = true;
                        return;
                    }
                    else
                        Response.Redirect("CrewTravelRequestGeneral.aspx?from=travel&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
                }
                else
                    Response.Redirect("CrewTravelRequestGeneral.aspx?from=travel&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);

                BindData();
                gvCCT.Rebind();
            }
            else if (e.CommandName.ToUpper() == "APPROVE")
            {
                RadLabel lblTravelId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblTravelId.Text != "")
                {
                    PhoenixCrewTravelRequest.OfficeTravelApprove(new Guid(lblTravelId.Text));
                }
                BindData();
                gvCCT.Rebind();
            }
            else if (e.CommandName.ToUpper() == "EDIT")
            {
                RadLabel lblTravelId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblTravelId.Text != "")
                {
                    Response.Redirect("../Crew/CrewTravelRequestAdd.aspx?travelid=" + lblTravelId.Text, false);
                }
            }
            else if (e.CommandName.ToUpper() == "CANCELTRAVEL")
            {
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblTravelRequestId.Text != "")
                {
                    PhoenixCrewTravelRequest.CancelTravel(new Guid(lblTravelRequestId.Text));
                }
                BindData();
                gvCCT.Rebind();
            }
            else if (e.CommandName.ToUpper() == "PICEMAIL")
            {
                RadLabel lblTravelRequestId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
                if (lblTravelRequestId.Text != "")
                {
                    ViewState["Travelid"] = lblTravelRequestId.Text;

                    ucConfirm.Visible = true;
                    ucConfirm.Text = "Are you sure you want sent an Email for Travel Initiated?";
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
            return;
        }
    }

    protected void gvCCT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCCT.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvCCT_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton email = (LinkButton)e.Item.FindControl("cmdPICEmail");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancelTravel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
                cancel.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to cancel this Request?')");
            }
            LinkButton ticket = (LinkButton)e.Item.FindControl("cmdShowTicket");

            RadLabel lblstatus = (RadLabel)e.Item.FindControl("lblStatusID");
            RadLabel lblTravelId = (RadLabel)e.Item.FindControl("lblTravelRequestId");
            LinkButton imgFlag = (LinkButton)e.Item.FindControl("imgFlag");

            RadLabel lblOfficeTravelYN = (RadLabel)e.Item.FindControl("lblOfficeTravelYN");
            RadLabel lblApprovedYN = (RadLabel)e.Item.FindControl("lblApprovedYN");
            LinkButton approve = (LinkButton)e.Item.FindControl("cmdApprove");
            if (approve != null)
            {
                approve.Visible = SessionUtil.CanAccess(this.ViewState, approve.CommandName);
                approve.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Are you sure you want to Approve this Request?')");
            }

            if (lblstatus.Text.Equals(ViewState["REQISSUED"].ToString()))
                ticket.Visible = true;
            else
                ticket.Visible = false;

            if ((lblstatus.Text.Equals(ViewState["REQPENDING"].ToString())) || (lblstatus.Text.Equals(ViewState["REQORDER"].ToString())) || (lblstatus.Text.Equals(ViewState["REQQUERY"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:orange\"><i class=\"fas fa-star-yellow\"></i></span>";
                imgFlag.Controls.Add(html);
                approve.Visible = false;
                cmdEdit.Visible = false;
            }
            else if ((lblstatus.Text.Equals(ViewState["REQISSUED"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:green\"><i class=\"fas fa-star-blue\"></i></span>";
                imgFlag.Controls.Add(html);
                cancel.Visible = false;
                approve.Visible = false;
                cmdEdit.Visible = false;
            }
            else if ((lblstatus.Text.Equals(ViewState["REQCANCELLED"].ToString())))
            {
                imgFlag.Visible = true;
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:red\"><i class=\"fas fa-star-red\"></i></span>";
                imgFlag.Controls.Add(html);

                cancel.Visible = false;
                email.Visible = false;
                approve.Visible = false;
                cmdEdit.Visible = false;
            }
            if (lblstatus.Text.Equals(ViewState["REQPENDING"].ToString()) && lblOfficeTravelYN.Text.Equals("1"))
            {
                if (lblApprovedYN != null && lblApprovedYN.Text == "1")
                {
                    approve.Visible = false;
                    cmdEdit.Visible = false;
                }
                else
                {
                    approve.Visible = true;
                    cmdEdit.Visible = true;
                }
            }

            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblPurpose");
            RadLabel lbltravelpurpose = (RadLabel)e.Item.FindControl("lblTravelPurpose");
            if (lbltravelpurpose.Text == ViewState["REQUSERCOURSES"].ToString())
            {
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            if (ticket != null)
                ticket.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewTravelTicket.aspx?TRAVELID=" + lblTravelId.Text + "',false,700,500); return false;");

            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.TRAVELCOSTANALYSIS + "'); return false;");
            } 
        }
    }

    protected void gvCCT_DataBound(object sender, EventArgs e)
    {
        if (gvCCT.MasterTableView.Items.Count > 0)
            gvCCT.MasterTableView.Items[0].Selected = true;
    }
}

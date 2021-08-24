using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewTravelTicket : PhoenixBasePage
{
    DataSet dsEmployeeDetails = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {        
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["EDITROW"] = "0";
            ViewState["CURRENTROW"] = null;
            ViewState["ROUTEID"] = null;

            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
            gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        BindRoutingDetails();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.Title = lblTitle.Text;
        CrewMenu.MenuList = toolbar.Show();

    }
    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDORIGIN", "FLDDESTINATION", "FLDDEPARTUREDATE", "FLDARRIVALDATE", "FLDNOOFSTOPS", "FLDDURATION", "FLDCURRENTAMOUNT", "FLDCURRENTTAX", "FLDTICKETNO" };
        string[] alCaptions = { "Name", "Origin", "Destination", "Departure Date", "Arrival Date", "Stops", "Duration (Hrs)", "Fare", "Tax", "Ticket No" };


         dsEmployeeDetails = PhoenixCrewTravelQuoteLine.CrewTravelhopLineItemSearchTicket(
               new Guid(ViewState["TRAVELID"].ToString()), (int)ViewState["PAGENUMBER"],
                gvCrewSearch.PageSize,//10, 
                ref iRowCount,
                ref iTotalPageCount);

        if (dsEmployeeDetails.Tables[0].Rows.Count > 0)
        {
            gvCrewSearch.DataSource = dsEmployeeDetails.Tables[0];
            gvCrewSearch.VirtualItemCount = iRowCount;
            lblTitle.Text = "Travel Request (" + dsEmployeeDetails.Tables[0].Rows[0]["FLDREQUISITIONNO"].ToString() + " )";
        }
        else
        {
            gvCrewSearch.DataSource = "";
        }
    }
  
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EMAILTOFIELDOFFCES"))
            {
                SendMailToFieldOffice();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SendMailToFieldOffice()
    {
        DataTable dtEmployee = PhoenixCrewTravelQuoteLine.CrewTravelEmployeeDetailsForTicket(new Guid(ViewState["TRAVELID"].ToString())
            ,PhoenixSecurityContext.CurrentSecurityContext.UserCode);

        if (dtEmployee.Rows.Count > 0)
        {
            bool check = false;
            foreach (DataRow dr in dtEmployee.Rows)
            {
                Guid requestid;
                string allattachments = string.Empty;
                string ticketno = string.Empty;
                int attachmentyn = 0;
                string[] arrAttachments = new string[100];

                requestid = new Guid(dr["FLDREQUESTID"].ToString());

                PhoenixCrewTravelQuoteLine.CrewTravelEmployeeTicketNoAttachmentsGet(new Guid(ViewState["TRAVELID"].ToString()), requestid, ref ticketno, ref allattachments, ref attachmentyn);

                StringBuilder sbemailbody = new StringBuilder();

                sbemailbody.Append("Dear Sir/Madam");
                sbemailbody.AppendLine();
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Please find attached travel ticket for " + dr["FLDEMPLOYEENAME"].ToString() + " - " + dr["FLDRANKNAME"].ToString());
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Ticket Number :" + ticketno);
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Crew Change Date : " + dr["FLDDATEOFCREWCHANGE"].ToString());
                sbemailbody.AppendLine("Crew Change Port : " + dr["FLDCREWCHANGEPORT"].ToString());
                sbemailbody.AppendLine();
                sbemailbody.AppendLine("Thank you,");
                string emailbody = sbemailbody.ToString();

                if (General.GetNullableString(dr["FLDEMAIL"].ToString()) != null && General.GetNullableString(ticketno) != null)
                {
                    if (attachmentyn == 0)
                    {
                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',')
                           , ""
                           , ""
                           , dr["FLDREQUISITIONNO"].ToString() + " - " + dr["FLDVESSELNAME"].ToString() + " - " + dr["FLDEMPLOYEENAME"].ToString() 
                           , emailbody, false
                           , System.Net.Mail.MailPriority.Normal
                           , ""
                           );
                    }
                    else
                    {
                        arrAttachments = allattachments.Split(',');

                        PhoenixMail.SendMail(dr["FLDEMAIL"].ToString().Replace(";", ",").TrimEnd(',')
                          , ""
                          , ""
                          , dr["FLDREQUISITIONNO"].ToString() + " - " + dr["FLDVESSELNAME"].ToString() + " - " + dr["FLDEMPLOYEENAME"].ToString()
                          , emailbody, false
                          , System.Net.Mail.MailPriority.Normal
                          , ""
                          , arrAttachments
                          , null
                          );
                    }
                    check = true;
                }
            }
            if (check == true)
            {
                ucStatus.Visible = true;
                ucStatus.Text = "Email Sent";
            }
        }
    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblagentid = (RadLabel)e.Item.FindControl("lblAgentId");
            RadLabel lblquotationid = (RadLabel)e.Item.FindControl("lblQuotationID");
            RadLabel lblRouteID = (RadLabel)e.Item.FindControl("lblRouteID");

            LinkButton lnkShowReason = (LinkButton)e.Item.FindControl("cmdShowReason");
            if (lnkShowReason != null)
                lnkShowReason.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Crew/CrewTravelTicketCopy.aspx?ISTICKET=1&VIEWONLY=true&framename=ifMoreInfo&ROUTEID=" + lblRouteID.Text + "',false,700,500); return false;");

            LinkButton ib = (LinkButton)e.Item.FindControl("cmdAttachment");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblDtKey");
            if (ib != null && lb != null)
            {
                ib.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1',''," + "'" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?type=TRAVEL&DTKEY=" + lb.Text + "&MOD=" + PhoenixModule.CREW + "',false,700,500); return false;");
                RadLabel lblattachmentyn = (RadLabel)e.Item.FindControl("lblattachmentyn");
                if (lblattachmentyn != null)
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
                //att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lbldtkey.Text + "&mod=" + PhoenixModule.CREW + "','medium'); return false;");
                ibm.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1',''," + "'" + Session["sitepath"] + "/Crew/CrewTravelAttachmentView.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "',false,700,500); return false;");
                RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");
                if (lblattachmentmappingyn != null && lblattachmentmappingyn.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    ibm.Controls.Add(html);
                }
            }
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
        BindRoutingDetails();
    }
}

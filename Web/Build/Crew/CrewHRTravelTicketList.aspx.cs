using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CrewHRTravelTicketList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);          

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Details", "PASSENGERS");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Tickets", "TICKETS");

            MenuTravelTicketMain.AccessRights = this.ViewState;
            MenuTravelTicketMain.MenuList = toolbarmain.Show();
            MenuTravelTicketMain.SelectedMenuIndex = 3;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewHRTravelTicketList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvTravelTicket')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuTravelTicket.AccessRights = this.ViewState;
            MenuTravelTicket.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TRAVELPASSENGERID"] = "";
                ViewState["TRAVELHOPLINEID"] = "";

                if (Request.QueryString["travelrequestid"] != null && Request.QueryString["travelrequestid"].ToString() != "")
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
                else
                    ViewState["TRAVELREQUESTID"] = "";

                if (Request.QueryString["personalinfosn"] != null && Request.QueryString["personalinfosn"].ToString() != "")
                    ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"].ToString();
                else
                    ViewState["PERSONALINFOSN"] = "";

                if (Request.QueryString["page"] != null && Request.QueryString["page"].ToString() != "")
                    ViewState["PAGE"] = Request.QueryString["page"].ToString();
                else
                    ViewState["PAGE"] = "";

                lblTitle.Text = "Ticket (" + PhoenixCrewTravelRequest.RequestNo + ")";
                gvTravelTicket.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.Title = lblTitle.Text;
            MenuTitle.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelTicketMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["PAGE"].ToString() == "approval")
                {
                    Response.Redirect("../Crew/CrewHRTravelRequestApprovalList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
                }
                else
                {
                    Response.Redirect("../Crew/CrewHRTravelRequestList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString(), false);
                }
            }
            else if (CommandName.ToUpper().Equals("PASSENGERS"))
            {
                Response.Redirect("../Crew/CrewHRTravelPassengerList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("../Crew/CrewHRTravelQuotationAgentDetail.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("TICKETS"))
            {
                Response.Redirect("../Crew/CrewHRTravelTicketList.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"].ToString() + "&personalinfosn=" + ViewState["PERSONALINFOSN"].ToString() + "&page=" + ViewState["PAGE"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TravelTicket_TabStripCommand(object sender, EventArgs e)
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNAME", "FLDDEPATURECITYNAME", "FLDDESTINATIONCITYNAME", "FLDDEPARTUREDATETIME", "FLDARRIVALDATETIME", "FLDTICKETNO", "FLDTOTALAMOUNT", "FLDTICKETSTATUS", "FLDDEPATUREDATEPASSEDYN" };
        string[] alCaptions = { "Name", "Origin", "Destination", "Departure", "Arrival", "Ticket No", "Amount", "Travel Status", "Obsolete Ticket" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewHRTravelRequest.HRTravelTicketSearch(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString())
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvTravelTicket.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelTicketList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + Request.Url.Scheme + "://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Ticket List</h3></td>");
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

            string[] alColumns = { "FLDNAME", "FLDDEPATURECITYNAME", "FLDDESTINATIONCITYNAME", "FLDDEPARTUREDATETIME",  "FLDARRIVALDATETIME", "FLDTICKETNO", "FLDTOTALAMOUNT", "FLDTICKETSTATUS", "FLDDEPATUREDATEPASSEDYN" };
            string[] alCaptions = { "Name", "Origin", "Destination", "Departure", "Arrival", "Ticket No", "Amount", "Travel Status", "Obsolete Ticket" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewHRTravelRequest.HRTravelTicketSearch(General.GetNullableGuid(ViewState["TRAVELREQUESTID"].ToString())
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , gvTravelTicket.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount);

            General.SetPrintOptions("gvTravelTicket", "Travel Ticket List", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelTicket.DataSource = ds;
                gvTravelTicket.VirtualItemCount = iRowCount;
                if (ViewState["TRAVELHOPLINEID"] == null)
                {
                    ViewState["TRAVELHOPLINEID"] = ds.Tables[0].Rows[0]["FLDTRAVELHOPLINEID"].ToString();
                    //gvTravelTicket.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                gvTravelTicket.DataSource = "";
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
            for (int i = 0; i < gvTravelTicket.MasterTableView.Items.Count; i++)
            {
                if (gvTravelTicket.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELHOPLINEID").ToString().Equals(ViewState["TRAVELHOPLINEID"].ToString()))
                {
                    gvTravelTicket.MasterTableView.Items[i].Selected = true;
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
    protected void gvTravelTicket_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;
        else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["TRAVELHOPLINEID"] = ((RadLabel)e.Item.FindControl("lblTravelHopLineId")).Text;            
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvTravelTicket_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelTicket.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTravelTicket_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton select = (LinkButton)e.Item.FindControl("cmdSelect");
            if (select != null) select.Visible = SessionUtil.CanAccess(this.ViewState, select.CommandName);

            LinkButton ibm = (LinkButton)e.Item.FindControl("cmdAttachmentMapping");
            RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachment");
            RadLabel ticket = (RadLabel)e.Item.FindControl("lblTicketNo");
            if (ibm != null && lbm.Text != null && ticket.Text != null)
            {
                RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");
                ibm.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAttachmentView.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "')");
                HtmlGenericControl html = new HtmlGenericControl();
                html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                ibm.Controls.Add(html);
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["TRAVELHOPLINEID"] = null;
            BindData();
            gvTravelTicket.Rebind();
            if (ViewState["TRAVELHOPLINEID"] != null)
            {
                for (int i = 0; i < gvTravelTicket.MasterTableView.Items.Count; i++)
                {
                    if (gvTravelTicket.MasterTableView.Items[i].GetDataKeyValue("FLDTRAVELHOPLINEID").ToString() == ViewState["TRAVELHOPLINEID"].ToString())
                    {
                        gvTravelTicket.MasterTableView.Items[i].Selected = true;
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
}

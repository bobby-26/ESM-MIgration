using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewHRTravelQuotationAgentDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "LIST");
            toolbarmain.AddButton("Details", "PASSENGERS");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Tickets", "TICKETS");
            MenuAgent.AccessRights = this.ViewState;
            MenuAgent.MenuList = toolbarmain.Show();
            MenuAgent.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["REQUISITIONNO"] = "";
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                if (Request.QueryString["travelrequestid"] != null)
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"];

                if (Request.QueryString["personalinfosn"] != null)
                    ViewState["PERSONALINFOSN"] = Request.QueryString["personalinfosn"];

                if (Request.QueryString["page"] != null && Request.QueryString["page"].ToString() != "")
                    ViewState["PAGE"] = Request.QueryString["page"].ToString();
                else
                    ViewState["PAGE"] = "";
                SetInformation();

                ViewState["SETCURRENTNAVIGATIONTAB"] = null;
                if (Request.QueryString["travelrequestid"] != null)
                {
                    ViewState["TRAVELREQUESTID"] = Request.QueryString["travelrequestid"].ToString();
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewHRTravelQuotationAgentLineItem.aspx?travelrequestid=";
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelQuotationAgentLineItem.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"];
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = "../Crew/CrewHRTravelQuotationAgentLineItem.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"] +
                    "&AGENTID=" + ViewState["AGENTID"].ToString();
                  
                }
                gvAgent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                lblTitle.Text = "Quotation (" + PhoenixCrewTravelRequest.RequestNo + ")";
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewTravelQuotationAgentDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAgent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");            
            MenuAgentList.AccessRights = this.ViewState;
            MenuAgentList.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.Title = lblTitle.Text;
            MenuTitle.MenuList = toolbar.Show();

            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetInformation()
    {
        DataSet ds = PhoenixCrewTravelRequest.EditTravel(new Guid(Request.QueryString["travelrequestid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {

            DataRow dr = ds.Tables[0].Rows[0];
            string vsl = "";
            if (Filter.CurrentTraveltoVesselName != null)
                vsl = Filter.CurrentTraveltoVesselName.ToString();
            lblTitle.Text = "Quotation (" + PhoenixCrewTravelRequest.RequestNo + " ) ";            
            ViewState["REQUISITIONNO"] = " [" + dr["FLDREQUISITIONNO"].ToString() + " ] ";
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };
            string travelid = null;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (ViewState["TRAVELREQUESTID"] != null)
                travelid = ViewState["TRAVELREQUESTID"].ToString();

            DataSet ds = PhoenixCrewTravelQuote.CrewTravelAgentSearch(new Guid(travelid), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvAgent", "Travel Agents", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvAgent.DataSource = ds;
                gvAgent.VirtualItemCount = iRowCount;
                if (ViewState["SETCURRENTNAVIGATIONTAB"] == null)
                    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewHRTravelQuotationAgentLineItem.aspx?travelrequestid=" + ViewState["TRAVELREQUESTID"];


                if (ViewState["AGENTID"] == null)
                {
                    ViewState["AGENTID"] = ds.Tables[0].Rows[0]["FLDAGENTID"].ToString();
                    PhoenixCrewTravelQuote.Agent = ds.Tables[0].Rows[0]["FLDAGENTNAME"].ToString();
                  //  gvAgent.SelectedIndex = 0;
                }
                if (ViewState["TRAVELAGENTID"] == null)
                {
                    ViewState["TRAVELAGENTID"] = ds.Tables[0].Rows[0]["FLDTRAVELAGENTID"].ToString();
                    PhoenixCrewTravelQuote.Agent = ds.Tables[0].Rows[0]["FLDAGENTNAME"].ToString();
                    //gvAgent.SelectedIndex = 0;
                }

                if (ViewState["SETCURRENTNAVIGATIONTAB"] != null)
                {
                    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELREQUESTID"] +
                        "&AGENTID=" + ViewState["AGENTID"].ToString()
                        + "&TRAVELAGENTID=" + ViewState["TRAVELAGENTID"].ToString(); 
                }
            }
            else
            {
                gvAgent.DataSource = "";
                ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELREQUESTID"].ToString();
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
            BindData();
            gvAgent.Rebind();
            String script = String.Format("javascript:resize();");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage = "E-mail sent to ,<BR/>" + Session["emailsend"].ToString();
                ucConfirm.Visible = true;
            }
            Session["emailsend"] = null;
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuAgent_TabStripCommand(object sender, EventArgs e)
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDAGENTNAME", "FLDSENDDATE", "FLDRECEIVEDDATE" };
        string[] alCaptions = { "Agent Name", "Send Date", "Received Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewTravelQuote.CrewTravelAgentSearch(new Guid(ViewState["TRAVELREQUESTID"].ToString()), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvAgent.PageSize, ref iRowCount, ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=Travel Agents.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Agents</h3></td>");
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
    protected void gvAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        else if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["TRAVELREQUESTID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
            ViewState["TRAVELAGENTID"] = ((RadLabel)e.Item.FindControl("lblTravelAgentId")).Text;

            PhoenixCrewTravelQuote.Agent = ((LinkButton)e.Item.FindControl("lnkAgentName")).Text;
            ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString() + ViewState["TRAVELREQUESTID"] +
                    "&AGENTID=" + ViewState["AGENTID"].ToString()
                    + "&TRAVELAGENTID=" + ViewState["TRAVELAGENTID"].ToString();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAgent.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            // {
            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnPickAgent");
            RadTextBox txtagentname = (RadTextBox)e.Item.FindControl("txtAgentName");
            RadTextBox txtagentcode = (RadTextBox)e.Item.FindControl("txtAgentCode");
            RadTextBox txtagentid = (RadTextBox)e.Item.FindControl("txtAgentID");
            if (txtagentid != null) txtagentid.Attributes.Add("style", "visibility:hidden");

            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?producttype=63&framename=filterandsearch', true);");
            //}
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            LinkButton dbc = (LinkButton)e.Item.FindControl("cmdCommunication");
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblTravelID");
            RadLabel lb2 = (RadLabel)e.Item.FindControl("lblAgentID");
            RadLabel lb3 = (RadLabel)e.Item.FindControl("lblTravelAgentId");
            LinkButton lbn = (LinkButton)e.Item.FindControl("lnkAgentName");
            RadLabel isfinalized = (RadLabel)e.Item.FindControl("lblIsFinalized");
            RadLabel isSelectedfororder = (RadLabel)e.Item.FindControl("lblIsSelectedForOrder");

            if (dbc != null && ViewState["REQUISITIONNO"].ToString() != null)
            {
                dbc.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationChat.aspx?TRAVELID=" + lbl.Text + "&AGENTID=" + lb2.Text + "&TRAVELAGENTID=" + lb3.Text + "&AGENTNAMEOLY=" + lbn.Text.Replace('&', '~').ToString() + "&AGENTNAME=" + lbn.Text.Replace('&', '~').ToString() + ViewState["REQUISITIONNO"].ToString() + "&ISOFFICE=1" + "');return false;");
            }
            RadLabel lblSendDate = (RadLabel)e.Item.FindControl("lblSendDate");
            RadLabel lblRecievedDate = (RadLabel)e.Item.FindControl("lblRecievedDate");

            if (lblSendDate != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblSendDate.Text);
                if (dt != null)
                {
                    lblSendDate.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblSendDate.Visible = true;
                }
            }

            if (lblRecievedDate != null)
            {
                DateTime? dt = General.GetNullableDateTime(lblRecievedDate.Text);
                if (dt != null)
                {
                    lblRecievedDate.Text = String.Concat(String.Format("{0:dd/MM/yyyy}", dt).ToString() + " " + String.Format("{0:t}", dt).ToString());
                    lblRecievedDate.Visible = true;
                }
            }
            //}
        }
    }
}

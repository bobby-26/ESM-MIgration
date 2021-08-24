using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Portal;
using System.Collections.Generic;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class PortalTravelQuote : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toobargrid = new PhoenixToolbar();
            toobargrid.AddFontAwesomeButton("../Portal/PortalTravelQuote.aspx", "Find", " <i class=\"fas fa-search\"></i>", "FIND");
            toobargrid.AddFontAwesomeButton("../Portal/PortalTravelQuote.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenutravelList.AccessRights = this.ViewState;
            MenutravelList.MenuList = toobargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                RadComboBoxItem item = new RadComboBoxItem("--Select--", "0");

                ddlStatus.Items.Add(item);
                ddlStatus.AppendDataBoundItems = true;
                ddlStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , 130, 1, "TQY,TPO,ISS,CND");


                if (Filter.PortalTravelQuoteFilter != null)
                {
                    NameValueCollection nvc = Filter.PortalTravelQuoteFilter;
                    txtReqNo.Text = nvc.Get("REQUISTIONNUMBER").ToString();
                    ddlVessel.SelectedVessel = nvc.Get("VESSELID").ToString();
                    txtStartDate.Text = nvc.Get("STARTDATE").ToString();
                    txtEndDate.Text = nvc.Get("ENDDATE").ToString();
                    txtQuoteSentFromDate.Text = nvc.Get("QUOTESENTFROMDATE").ToString();
                    txtQuoteSentToDate.Text = nvc.Get("QUOTESENTTODATE").ToString();
                    if ((nvc.Get("STATUS").ToString() != "0"))
                        ddlStatus.SelectedValue = nvc.Get("STATUS").ToString();

                    ViewState["PAGENUMBER"] = General.GetNullableInteger(nvc.Get("PAGENUMBER").ToString());
                    ViewState["SORTEXPRESSION"] = nvc.Get("SORTEXPRESSION").ToString();
                    ViewState["SORTDIRECTION"] = General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString());
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();

                    criteria.Clear();

                    criteria.Add("REQUISTIONNUMBER", txtReqNo.Text == null ? "" : txtReqNo.Text);
                    criteria.Add("VESSELID", ddlVessel.SelectedVessel == "--Dummy--" ? "" : ddlVessel.SelectedVessel);
                    criteria.Add("STARTDATE", txtStartDate.Text == null ? "" : txtStartDate.Text);
                    criteria.Add("ENDDATE", txtEndDate.Text == null ? "" : txtEndDate.Text);
                    criteria.Add("QUOTESENTFROMDATE", txtQuoteSentFromDate.Text == null ? "" : txtQuoteSentFromDate.Text);
                    criteria.Add("QUOTESENTTODATE", txtQuoteSentToDate.Text == null ? "" : txtQuoteSentToDate.Text);
                    criteria.Add("STATUS", ddlStatus.SelectedValue == "--Select--" ? "0" : ddlStatus.SelectedValue);
                    criteria.Add("PAGENUMBER", ViewState["PAGENUMBER"] == null ? "1" : ViewState["PAGENUMBER"].ToString());
                    criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                    criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());

                    Filter.PortalTravelQuoteFilter = criteria;
                }
                ddlStatus.DataBind();
                gvTravelQuote.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
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
            if (CommandName.ToUpper().Equals("FIND"))
            {

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("REQUISTIONNUMBER", txtReqNo.Text == null ? "" : txtReqNo.Text);
                criteria.Add("VESSELID", ddlVessel.SelectedVessel == "--Dummy--" ? "" : ddlVessel.SelectedVessel);
                criteria.Add("STARTDATE", txtStartDate.Text == null ? "" : txtStartDate.Text);
                criteria.Add("ENDDATE", txtEndDate.Text == null ? "" : txtEndDate.Text);
                criteria.Add("QUOTESENTFROMDATE", txtQuoteSentFromDate.Text == null ? "" : txtQuoteSentFromDate.Text);
                criteria.Add("QUOTESENTTODATE", txtQuoteSentToDate.Text == null ? "" : txtQuoteSentToDate.Text);
                criteria.Add("STATUS", ddlStatus.SelectedValue == "--Select--" ? "0" : ddlStatus.SelectedValue);
                criteria.Add("PAGENUMBER", "1");
                criteria.Add("SORTEXPRESSION", ViewState["SORTEXPRESSION"] == null ? "" : ViewState["SORTEXPRESSION"].ToString());
                criteria.Add("SORTDIRECTION", ViewState["SORTDIRECTION"] == null ? "" : ViewState["SORTDIRECTION"].ToString());
                Filter.PortalTravelQuoteFilter = criteria;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvTravelQuote.Rebind();

            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtReqNo.Text = "";
                ddlVessel.SelectedVessel = "";
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtQuoteSentFromDate.Text = "";
                txtQuoteSentToDate.Text = "";
                ddlStatus.SelectedValue = "";
                ddlStatus.Text = "";

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();

                criteria.Add("REQUISTIONNUMBER", "");
                criteria.Add("VESSELID", "");
                criteria.Add("STARTDATE", "");
                criteria.Add("ENDDATE", "");
                criteria.Add("QUOTESENTFROMDATE", "");
                criteria.Add("QUOTESENTTODATE", "");
                criteria.Add("STATUS", "0");
                criteria.Add("PAGENUMBER", "1");
                criteria.Add("SORTEXPRESSION", "");
                criteria.Add("SORTDIRECTION", "");
                Filter.PortalTravelQuoteFilter = criteria;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvTravelQuote.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelQuote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelQuote.CurrentPageIndex + 1;
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.PortalTravelQuoteFilter;

            DataSet ds = PhoenixPortalTravelQuote.TravelRequisitionSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableString(nvc.Get("REQUISTIONNUMBER").ToString())
                , General.GetNullableInteger(nvc.Get("VESSELID").ToString())
                , null
                , General.GetNullableDateTime(nvc.Get("STARTDATE").ToString())
                , General.GetNullableDateTime(nvc.Get("ENDDATE").ToString())
                , General.GetNullableDateTime(nvc.Get("QUOTESENTFROMDATE").ToString())
                , General.GetNullableDateTime(nvc.Get("QUOTESENTTODATE").ToString())
                , General.GetNullableInteger(nvc.Get("STATUS").ToString() == "0" ? "" : nvc.Get("STATUS").ToString())
                , General.GetNullableString(nvc.Get("SORTEXPRESSION").ToString())
                , General.GetNullableInteger(nvc.Get("SORTDIRECTION").ToString())
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , gvTravelQuote.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvTravelQuote.DataSource = ds;
            gvTravelQuote.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelQuote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("QUOTE"))
            {
                string sessionid = ((RadLabel)e.Item.FindControl("lblTravelAgentID")).Text;

                Response.Redirect("../Crew/CrewTravelQuotation.aspx?SESSIONID=" + sessionid, false);
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                string sessionid = ((RadLabel)e.Item.FindControl("lblTravelAgentID")).Text;

                Response.Redirect("../Crew/CrewTravelQuotation.aspx?SESSIONID=" + sessionid, false);
            }
            if (e.CommandName.ToUpper().Equals("ISSUE"))
            {
                string travelid = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
                string agentid = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
                string querystring = "TRAVELID=" + travelid + "&AGENTID=" + agentid;
                Response.Redirect("../Crew/CrewTravelQuoteTicket.aspx?" + querystring, false);
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

    protected void gvTravelQuote_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel finalizedYN = (RadLabel)e.Item.FindControl("lblFinalizedYN");
            LinkButton ib = (LinkButton)e.Item.FindControl("cmdIssue");

            if (finalizedYN != null && finalizedYN.Text.ToString() == "1" && ib != null)
            {
                ib.Visible = true;
                ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
            }
            else if (ib != null)
            {
                ib.Visible = false;
            }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton cm = (LinkButton)e.Item.FindControl("cmdQuote");

            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (cm != null) cm.Visible = SessionUtil.CanAccess(this.ViewState, cm.CommandName);

        }
    }
    
    protected void gvTravelQuote_EditCommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    ((RadTextBox)e.Item.FindControl("txthandledby")).Focus();
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }

    protected void gvTravelQuote_UpdateCommand(object sender, GridCommandEventArgs e)
    {
    
        string travelagentid = ((RadLabel)e.Item.FindControl("lblTravelAgentID")).Text;

        PhoenixPortalTravelQuote.TravelRequisitionAgentUpdate(new Guid(travelagentid), ((RadTextBox)e.Item.FindControl("txthandledby")).Text);
        
        BindData();
        gvTravelQuote.Rebind();
    }

    protected void gvTravelQuote_SortCommand(object sender, GridSortCommandEventArgs e)
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


}

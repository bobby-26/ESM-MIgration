using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewTravelQuotation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");

                if (Request.QueryString["SESSIONID"] != null)
                {
                    bindTravals();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["QUOTEPAGENUMBER"] = 1;
                ViewState["QUOTESORTEXPRESSION"] = null;
                ViewState["QUOTESORTDIRECTION"] = null;

                ViewState["CURRENTINDEX"] = 1;
                ViewState["EDITROW"] = "0";
                CheckWebSessionStatus();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuQuotation.AccessRights = this.ViewState;
            if (ViewState["PageTitle"] != null && ViewState["PageTitle"].ToString() != "")
            {
                MenuQuotation.Title = ViewState["PageTitle"].ToString();
            }
            MenuQuotation.MenuList = toolbar.Show();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void bindTravals()
    {
        ViewState["SESSIONID"] = Request.QueryString["SESSIONID"].ToString();
        DataSet ds = PhoenixCrewTravelQuote.CrewQuotationDetails(new Guid(Request.QueryString["SESSIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["TRAVELID"] = dr["FLDTRAVELID"].ToString();
            ViewState["AGENTID"] = dr["FLDAGENTID"].ToString();
            ViewState["PORT"] = dr["FLDPORTOFCREWCHANGE"].ToString();
            ViewState["DATE"] = dr["FLDDATEOFCREWCHANGE"].ToString();
            ViewState["VESSEL"] = dr["FLDVESSELID"].ToString();
            ViewState["FINALIZEYN"] = dr["FLDSESSIONSTATUS"].ToString();
            ViewState["AGENTNAMEOLY"] = dr["FLDUSERNAME"].ToString();

            ViewState["Title"] = dr["FLDAGENTNAME"].ToString() + " [ " + dr["FLDREQUISITIONNO"].ToString() + " ]";

            ViewState["PageTitle"] = "Quotation Details [ " + dr["FLDREQUISITIONNO"].ToString() + " ]";

        }
    }
    protected void MenuQuotation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
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

    protected void gvAgent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["QUOTEPAGENUMBER"] = ViewState["QUOTEPAGENUMBER"] != null ? ViewState["QUOTEPAGENUMBER"] : gvAgent.CurrentPageIndex + 1;
        BindQuotationDetails();
    }


    private void BindQuotationDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDQUOTATIONREFNO", "FLDAMOUNT" };
        string[] alCaptions = { "Agent Name", "Amount" };
        string travelid = null;
        int agentid = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["QUOTESORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["QUOTESORTDIRECTION"].ToString());
        if (ViewState["TRAVELID"] != null)
            travelid = ViewState["TRAVELID"].ToString();
        if (ViewState["AGENTID"] != null)
            agentid = int.Parse(ViewState["AGENTID"].ToString());
        DataSet ds = PhoenixCrewTravelQuote.CrewTravelQuotationSearchForAgent(new Guid(travelid)
                                        , new Guid(ViewState["SESSIONID"].ToString())
                                        , sortexpression
                                        , sortdirection
                                        , (int)ViewState["QUOTEPAGENUMBER"]
                                        , gvAgent.PageSize
                                        , ref iRowCount
                                        , ref iTotalPageCount);

        gvAgent.DataSource = ds.Tables[0];
        gvAgent.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {

            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ifMoreInfo.Attributes["src"] = "CrewTravelQuotationGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&QUOTEID=" + ViewState["QUOTEID"] + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
           + "&date=" + ViewState["DATE"].ToString()
           + "&vessel=" + ViewState["VESSEL"].ToString()
           + "&active=" + ViewState["FINALIZEYN"].ToString() + "&confirmed=" + ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
            }

        }
        else
        {
            ifMoreInfo.Attributes["src"] = "CrewTravelQuotationGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
          + "&date=" + ViewState["DATE"].ToString()
          + "&vessel=" + ViewState["VESSEL"].ToString()
          + "&active=0&confirmed=request";
        }

        ViewState["QUOTETOTALPAGECOUNT"] = iTotalPageCount;
        ViewState["QUOTEROWCOUNT"] = iRowCount;

    }

    protected void gvAgent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "SORT")
            return;

        if (e.CommandName.ToUpper() == "SELECT")
        {
            ViewState["QUOTEID"] = ((RadLabel)e.Item.FindControl("lblQuotationID")).Text;
            ViewState["TRAVELID"] = ((RadLabel)e.Item.FindControl("lblTravelID")).Text;
            ViewState["AGENTID"] = ((RadLabel)e.Item.FindControl("lblAgentID")).Text;
            string finalizeyn = ((RadLabel)e.Item.FindControl("lblfinalizeyn")).Text;
            string confirmedyn = ((RadLabel)e.Item.FindControl("lblconfirmedyn")).Text;

            ifMoreInfo.Attributes["src"] = "CrewTravelQuotationGeneral.aspx?SESSIONID=" + ViewState["SESSIONID"].ToString() + "&QUOTEID=" + ViewState["QUOTEID"] + "&TRAVELID=" + ViewState["TRAVELID"] + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&port=" + ViewState["PORT"].ToString()
               + "&date=" + ViewState["DATE"].ToString()
               + "&vessel=" + ViewState["VESSEL"].ToString()
               + "&active=" + ViewState["FINALIZEYN"].ToString() + "&confirmed=" + confirmedyn;


        }
    }

    protected void gvAgent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton imgbtn = (LinkButton)e.Item.FindControl("cmdChat");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblQuotationID");
            if (imgbtn != null)
                imgbtn.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2','','" + Session["sitepath"] + "/Crew/CrewTravelQuotationChat.aspx?QUOTEID=" + lb.Text + "&TRAVELID=" + ViewState["TRAVELID"].ToString() + "&AGENTID=" + ViewState["AGENTID"].ToString() + "&AGENTNAMEOLY=" + ViewState["AGENTNAMEOLY"].ToString() + "&ACTIVE=" + ViewState["FINALIZEYN"].ToString() + "&AGENTNAME=" + ViewState["Title"].ToString().Replace('&', '~') + "&VIEWONLY=Y'); return false;");
            if (imgbtn != null && Request.QueryString["OFFICE"] != null)
                imgbtn.Visible = false;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["QUOTEID"] = null;
        BindQuotationDetails();
        gvAgent.Rebind();

    }

}

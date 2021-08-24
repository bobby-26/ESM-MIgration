using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewTravelAgentQuotationChat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        try
        {
            HtmlMeta meta = new HtmlMeta();
            meta.HttpEquiv = "Refresh";
            Response.AppendHeader("Refresh", "60");

            if (!IsPostBack)
            {
                ViewState["TRAVELID"] = "";
                ViewState["TRAVELAGENTID"] = "";
                ViewState["OFFICE"] = "";

                if (Request.QueryString["TravelId"] != null)
                    ViewState["TRAVELID"] = Request.QueryString["TravelId"].ToString();

                if (Request.QueryString["TravelAgentId"] != null)
                    ViewState["TRAVELAGENTID"] = Request.QueryString["TravelAgentId"].ToString();

                if (Request.QueryString["OFFICE"] != null)
                    ViewState["OFFICE"] = Request.QueryString["OFFICE"].ToString();

                if (Request.QueryString["title"] != null)
                    lblNote.Text = "Chat Details " + Request.QueryString["title"].ToString();
                BindData();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post Remarks", "POSTCOMMENT", ToolBarDirection.Right);
            MenuChat.AccessRights = this.ViewState;
            MenuChat.Title = lblNote.Text;
            MenuChat.MenuList = toolbar.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();        
        dt = PhoenixCrewTravelQuote.CrewTravelAgentQuotationChatSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            ,new Guid(ViewState["TRAVELID"].ToString())
            ,General.GetNullableGuid(ViewState["TRAVELAGENTID"].ToString()));       

        if (dt.Rows.Count > 0)
        {
            gvDiscussion.DataSource = dt;
            //gvDiscussion.VirtualItemCount = iRowCount;
        }
        else
        {
            gvDiscussion.DataSource = "";
        }
    }
    protected void MenuChat_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtChatMessage.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["OFFICE"].ToString() == "Y")
            PhoenixCrewTravelQuote.CrewTravelAgentQuotationChatInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["TRAVELID"].ToString())
                , General.GetNullableGuid(ViewState["TRAVELAGENTID"].ToString())
                , txtChatMessage.Text.Trim());

            if (ViewState["OFFICE"].ToString() == "N")
                PhoenixCrewTravelQuote.CrewTravelAgentQuotationChatInsert(0
               , new Guid(ViewState["TRAVELID"].ToString())
               , General.GetNullableGuid(ViewState["TRAVELAGENTID"].ToString())
               , txtChatMessage.Text.Trim());
            BindData();
            gvDiscussion.Rebind();
            txtChatMessage.Text = "";
        }
    }

    private bool IsCommentValid(string strComment)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";       

        return (!ucError.IsError);
    }

    protected void gvDiscussion_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

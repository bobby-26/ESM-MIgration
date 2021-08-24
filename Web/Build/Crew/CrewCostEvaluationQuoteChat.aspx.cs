using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCostEvaluationQuoteChat : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                btnPost.Attributes.Add("style", "display:none");

                ViewState["VIWEONLY"] = "0";

                ViewState["SESSIONID"] = null;
                ViewState["ISOFFICE"] = 0;
                ViewState["QUOTEIDVNDOR"] = null;
                ViewState["defauttext"] = null;
                ViewState["Title"] = "";

                if (Request.QueryString["VIEWONLY"] == null)
                {
                    ViewState["VIWEONLY"] = "1";
                }

                if (Request.QueryString["REQUESTID"] != null)
                {
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                }
                if (Request.QueryString["QUOTEID"] != null)
                {
                    ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
                    ViewState["SESSIONID"] = Request.QueryString["QUOTEID"].ToString();
                }
                if (Request.QueryString["AGENTID"] != null)
                {
                    ViewState["AGENTID"] = Request.QueryString["AGENTID"].ToString();
                }
                if (Request.QueryString["ISOFFICE"] != null)
                {
                    ViewState["ISOFFICE"] = Request.QueryString["ISOFFICE"].ToString();
                    if (Request.QueryString["ISOFFICE"].ToString().Equals("1"))
                    {
                        ViewState["ISOFFICE"] = "1";
                    }
                }

                if (Request.QueryString["AGENTNAME"] != null)
                {
                    ViewState["Title"] = Request.QueryString["AGENTNAME"].ToString().Replace('~', '&');
                }
                if (Request.QueryString["AGENTNAMEOLY"] != null)
                {
                    ViewState["defauttext"] = "Type a message here";

                    Page.ClientScript.RegisterHiddenField("defauttext", ViewState["defauttext"].ToString());
                    txtChat.Text = ViewState["defauttext"].ToString();
                    txtChat.ForeColor = System.Drawing.Color.Gray;
                }
                if (ViewState["QUOTEID"] != null)
                {
                    ifMoreInfoQuote.Attributes["src"] = "../Crew/CrewCostEvaluationChatQuoteDetails.aspx?REQUESTID=" + ViewState["REQUESTID"] + "&QUOTEID=" + ViewState["QUOTEID"];
                    ifMoreInfoChat.Attributes["src"] = "../Crew/CrewCostEvaluationChatDetails.aspx?QUOTEID=" + ViewState["QUOTEID"];
                }

            }
            if (IsPostBack)
                this.txtChat.Focus();


            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuTab.AccessRights = this.ViewState;
            MenuTab.Title = ViewState["Title"].ToString();
            MenuTab.MenuList = toolbar.Show();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Chat();

    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        Chat();
    }
    private void Chat()
    {
        if (ViewState["QUOTEID"] != null && txtChat.Text != "" && !ViewState["defauttext"].ToString().Equals(txtChat.Text))
        {
            if (Request.QueryString["VIEWONLY"] == null)
            {

                PhoenixCrewCostEvaluationQuoteChat.CrewCostEvaluationQuoteChatInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["QUOTEID"].ToString())
                , General.GetNullableInteger(ViewState["AGENTID"].ToString())
                , txtChat.Text
                , int.Parse(ViewState["ISOFFICE"].ToString() == "1" ? "0" : "1"));
            }
            txtChat.Text = "";
        }
    }

}

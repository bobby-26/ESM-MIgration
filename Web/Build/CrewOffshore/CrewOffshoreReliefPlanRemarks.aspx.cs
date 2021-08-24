using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewOffshoreReliefPlanRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("SAVE", "SAVE",ToolBarDirection.Right);
        MenuRemarks.AccessRights = this.ViewState;
        MenuRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
           

            if (Request.QueryString["crewplanid"] != null && Request.QueryString["crewplanid"].ToString() != string.Empty)
            {
                ViewState["crewplanid"] = Request.QueryString["crewplanid"].ToString();
            }

            if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != string.Empty)
            {
                ViewState["empid"] = Request.QueryString["empid"].ToString();
            }
            ProposalRemarksEdit();
        }
    }

    public void ProposalRemarksEdit()
    {

        DataTable dt = PhoenixCrewOffshoreCrewChange.EditCrewPlanProposalRemarks(new Guid(ViewState["crewplanid"].ToString())
                                                                  , int.Parse(ViewState["empid"].ToString())
                                                                  );
        if (dt.Rows.Count > 0)
        {
            txtRemarks.Text = General.GetNullableString (dt.Rows[0]["FLDPROPOSALREMARKS"].ToString());
        }
    }

    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (General.GetNullableString(txtRemarks.Text) == null)
            {
                lblMessage.Text = "Remarks required.";
                return;
            }

            string Script = "";

            if (ViewState["crewplanid"] != null && ViewState["crewplanid"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixCrewOffshoreCrewChange.UpdateCrewPlanProposalRemarks(new Guid(ViewState["crewplanid"].ToString())
                                                                   , int.Parse(ViewState["empid"].ToString())
                                                                   , txtRemarks.Text);

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('CI','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}

using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRAClose : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            MenuCloseRemarks.AccessRights = this.ViewState;
            MenuCloseRemarks.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                {
                    ViewState["RAID"] = Request.QueryString["RAID"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }

    protected void MenuCloseRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtCloseRemarks.Text) == null)
            {
                lblMessage.Text = " Remarks is required.";
                return;
            }

            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != string.Empty)
            {
                if (CommandName.ToUpper().Equals("CLOSE"))
                {
                    PhoenixInspectionPEARSRiskAssessment.CloseRiskAssessment(new Guid(ViewState["RAID"].ToString()), General.GetNullableString(txtCloseRemarks.Text));
                }

                String script = String.Format("javascript:fnReloadList('Close','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using System.Text;
using Telerik.Web.UI;

public partial class RegisterCrewTravelAgentConfigurationEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["ID"] = "";
            
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() != "")
                {
                    ViewState["ID"] = Request.QueryString["id"].ToString();
                }
                if (!String.IsNullOrEmpty(ViewState["ID"].ToString()))
                    BindFields();
                else
                {
                    chkactive.Checked = true;
                }

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
            
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveMatrix();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindFields()
    {
        DataTable dt = PhoenixRegisterCrewTravelAgentConfiguration.CrewAgentConfigurationEdit(new Guid(ViewState["ID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            
            if (dt.Rows[0]["FLDISACTIVE"].ToString() == "1")
            {
                chkactive.Checked = true;

            }
            ucAddrAgent.SelectedValue = dt.Rows[0]["FLDAGENTID"].ToString();
            ucAddrAgent.Text = dt.Rows[0]["FLDAGENTNAME"].ToString();

        }
    }

    protected void SaveMatrix()
    {
        if (!IsValidConfig())
        {
            ucError.Visible = true;
            return;
        }

        if (ViewState["ID"].ToString() == null || ViewState["ID"].ToString() == "")
        {
            Guid? id = null;
            PhoenixRegisterCrewTravelAgentConfiguration.CrewAgentConfigurationInsert(                
                  General.GetNullableInteger(ucAddrAgent.SelectedValue)
                  , General.GetNullableInteger(chkactive.Checked.Value ? "1" : "0") 
                  , ref id                 
           );

            ViewState["ID"] = id;
        }
        else
        {
            PhoenixRegisterCrewTravelAgentConfiguration.CrewAgentConfigurationUpdate(new Guid(ViewState["ID"].ToString())
                , General.GetNullableInteger(ucAddrAgent.SelectedValue)
                , General.GetNullableInteger(chkactive.Checked.Value ? "1" : "0")
              );
        }

       // ucStatus.Text = "Saved updated.";
        BindFields();

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);

    }

    private bool IsValidConfig()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucAddrAgent.SelectedValue) == null)
            ucError.ErrorMessage = "Agent is required.";

        return (!ucError.IsError);
    }


}
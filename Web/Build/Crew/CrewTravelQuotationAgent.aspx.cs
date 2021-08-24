using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewTravelQuotationAgent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        try
        {
            toolbarsave.AddButton("New", "NEW");
            toolbarsave.AddButton("Save", "SAVE");
            MenuAgent.AccessRights = this.ViewState;
            MenuAgent.MenuList = toolbarsave.Show();
            txtAgentID.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["SaveStatus"] = "New";
                if (Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                }
                BindAgentInfo();
            }
            
        }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindAgentInfo()
    {
        DataSet dsAgent = PhoenixCrewTravelQuote.CrewTravelAgentHeaderSearch(new Guid(ViewState["TRAVELID"].ToString()),null);
        if (dsAgent.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsAgent.Tables[0].Rows[0];
            txtAgentCode.Text = dr["FLDAGENTCODE"].ToString();
            txtAgentID.Text = dr["FLDAGENTID"].ToString();
            txtAgentName.Text = dr["FLDNAME"].ToString();
            txtAgentAddress.Text = dr["FLDAGENTADDRESS"].ToString();
          
            txtEmail.Text = dr["FLDAGENTEMAIL"].ToString();
            txtFax.Text = dr["FLDAGENTFAX"].ToString();
        }
    }
    
    protected void MenuAgent_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                txtAgentAddress.Visible = true;
                txtEmail.Visible = true;
                txtFax.Visible = true;
                lblAddress.Visible = true;
                lblEmail.Visible = true;
                lblFax.Visible = true;

                if (IsValidAgent(txtAgentID.Text))
                {
                    if (ViewState["SaveStatus"].ToString().Equals("New"))
                    {

                        InsertTravelAgent();
                    }
                  
                    String script = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                ClearTextBox();
                ViewState["SaveStatus"] = "New";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
  

    private bool IsValidAgent(string agentid)
    {
     
        ucError.HeaderMessage = "Please provide the following required information";

      

        if (agentid.Trim().Equals(""))
                ucError.ErrorMessage = "Agent is required";
        return (!ucError.IsError);

    }
    private void ClearTextBox()
    {
        txtAgentCode.Text = "";
        txtAgentName.Text = "";
        txtAgentReference.Text = "";
        txtAgentAddress.Text = "";
        txtEmail.Text = "";
        txtFax.Text = "";
        txtAgentAddress.Visible = false;
        txtEmail.Visible = false;
        txtFax.Visible = false;
        lblAddress.Visible = false;
        lblEmail.Visible = false;
        lblFax.Visible = false;
         
       
    }
    private void InsertTravelAgent()
    {
        PhoenixCrewTravelQuote.InsertCrewTravelAgent(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["TRAVELID"].ToString()), Convert.ToInt32(txtAgentID.Text));
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}

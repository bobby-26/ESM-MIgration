using System;
using System.Data;
using System.Web;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
public partial class CrewOffshoreReliefRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
         

            if (Request.QueryString["SIGNONOFFID"] != null && Request.QueryString["SIGNONOFFID"].ToString() != string.Empty)
            {
                ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
                EditReliefRemarks();
            }
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRemarks.AccessRights = this.ViewState;
        MenuRemarks.MenuList = toolbarmain.Show();
    }

    protected void EditReliefRemarks()
    {
        DataTable dt = PhoenixVesselAccountsEmployee.EditSignOffDetails(int.Parse(ViewState["SIGNONOFFID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ucSignOffReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();
            txtReliefRemarks.Text = dt.Rows[0]["FLDSIGNOFFREMARKS"].ToString();
        }
    }

    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            string errormsg = "";

            if (General.GetNullableInteger(ucSignOffReason.SelectedSignOffReason) == null)
            {
                errormsg = "Sign Off Reason is required.";
            }
            if (General.GetNullableString(txtReliefRemarks.Text) == null)
            {
                errormsg = errormsg + "</br>" + "Relief remarks is required.";                
            }            

            if (!string.IsNullOrEmpty(errormsg))
            {
                lblMessage.Text = errormsg;
                return;
            }

            string Script = "";

            if (ViewState["SIGNONOFFID"] != null && ViewState["SIGNONOFFID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixCrewOffshoreReliefRequest.RaiseReliefRequest(int.Parse(ViewState["SIGNONOFFID"].ToString()), 
                        General.GetNullableString(txtReliefRemarks.Text),
                        General.GetNullableInteger(ucSignOffReason.SelectedSignOffReason));
                    
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
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

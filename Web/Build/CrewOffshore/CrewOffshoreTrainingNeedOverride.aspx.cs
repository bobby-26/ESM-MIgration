using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewOffshoreTrainingNeedOverride : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            confirm.Attributes.Add("style", "display:none");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {

                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuTrainingNeedOverride.AccessRights = this.ViewState;
                MenuTrainingNeedOverride.MenuList = toolbarmain.Show();
                MenuTrainingNeedOverride.Title = "Training Need Override";
            }

            if (!IsPostBack)
            {
                ViewState["employeeid"] = "";
                ViewState["trainingneedid"] = "";

                if (Request.QueryString["empid"] != null && Request.QueryString["empid"].ToString() != "")
                    ViewState["employeeid"] = Request.QueryString["empid"].ToString();

                if (Request.QueryString["trainingneedid"] != null && Request.QueryString["trainingneedid"].ToString() != "")
                    ViewState["trainingneedid"] = Request.QueryString["trainingneedid"].ToString();
              
                EditTrainingNeedOverride();
            }
              
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }   
    protected void EditTrainingNeedOverride()
    {
        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.EditTrainingNeed(new Guid(ViewState["trainingneedid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            //ChkOverrideYN.Checked = (dt.Rows[0]["FLDOVERRIDEYN"].ToString() == "1" ? true : false);
            txtOverrideRemarks.Text = dt.Rows[0]["FLDOVERRIDEREASON"].ToString();
            txtOverrideDate.Text = dt.Rows[0]["FLDOVERRIDEDATE"].ToString();
            txtOverrideBy.Text = dt.Rows[0]["FLDOVERRIDEBYNAME"].ToString();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                //ChkOverrideYN.Enabled = false;
                txtOverrideRemarks.Enabled = false;
                txtOverrideRemarks.CssClass = "readonlytextbox";
            }

        }
    }
    protected void MenuTrainingNeedOverride_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (!IsValidOverride())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["trainingneedid"] != null && ViewState["trainingneedid"].ToString() != "")
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
                    //ucConfirmCrew.Visible = true;
                    //ucConfirmCrew.Text = "Do you want to override this course?";
                    RadWindowManager1.RadConfirm("Do you want to override this course?", "confirm", 320, 150, null, "Confirm");


                }                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidOverride()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtOverrideRemarks.Text) == null)
            ucError.ErrorMessage = "Override remark is required.";

        return (!ucError.IsError);
    }

    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;
            //if (ucCM.confirmboxvalue == 1)
            //{
                PhoenixCrewOffshoreTrainingNeeds.UpdateTrainingNeedOverride(General.GetNullableGuid(ViewState["trainingneedid"].ToString())
                        , 1//General.GetNullableInteger(ChkOverrideYN.Checked == true ? "1" : "0")                       
                        , General.GetNullableString(txtOverrideRemarks.Text));

                ucStatus.Text = "Training Need Overrided successfully.";

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                EditTrainingNeedOverride();


            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowActionTypeEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkFlowActionTypeEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            Guid? Id = General.GetNullableGuid(Request.QueryString["ACTIONTYPEID"].ToString());
            DataTable ds4;
            ds4 = PhoenixWorkForm.ActionTypeEdit(Id);
            if (ds4.Rows.Count > 0)
            {
                DataRow dr = ds4.Rows[0];
                txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            }
        }

    }

    protected void MenuWorkFlowActionTypeEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Id = General.GetNullableGuid(Request.QueryString["ACTIONTYPEID"].ToString());
            string Name = General.GetNullableString(txtName.Text);
            string Description = General.GetNullableString(txtDescription.Text);
                   
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidActionType(Name, Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkForm.ActionTypeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id, Name, Description);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                              "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidActionType(string Name, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }



}
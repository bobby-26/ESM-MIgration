using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowStateTypeEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowStateTypeEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            int? Id = General.GetNullableInteger(Request.QueryString["STATETYPEID"].ToString());
            DataTable ds1;
            ds1 = PhoenixWorkForm.StateTypeEdit(Id);
            if (ds1.Rows.Count > 0)
            {

                DataRow dr = ds1.Rows[0];
                txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            }
        }
    }

    protected void MenuWorkflowStateTypeEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            int? Id = General.GetNullableInteger(Request.QueryString["STATETYPEID"].ToString());
            string Name = General.GetNullableString(txtName.Text);
            string Description = General.GetNullableString(txtDescription.Text);

          
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStateType(Name, Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkForm.StateTypeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id, Name, Description);

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

    private bool IsValidStateType(string Name, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        return (!ucError.IsError);
    }



}
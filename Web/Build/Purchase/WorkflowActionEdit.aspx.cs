using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class WorkflowActionEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkflowActionEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            Guid? Id = General.GetNullableGuid(Request.QueryString["ACTIONID"].ToString());
            DataTable ds6;
            ds6 = PhoenixWorkForm.ActionEdit(Id);
            if (ds6.Rows.Count > 0)
            {

                DataRow dr = ds6.Rows[0];
                txtActiontype.Text = dr["NAME"].ToString();
                txtShortCode.Text = dr["FLDSHORTCODE"].ToString();
                txtName.Text = dr["FLDNAME"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            }
        }

    }

    protected void MenuWorkflowActionEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Id = General.GetNullableGuid(Request.QueryString["ACTIONID"].ToString());
            string Name = General.GetNullableString(txtName.Text);
            string Description = General.GetNullableString(txtDescription.Text);

           


            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidAction(Name, Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkForm.ActionUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Id, Name, Description);

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

    private bool IsValidAction(string Name, string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";


        return (!ucError.IsError);
    }



}
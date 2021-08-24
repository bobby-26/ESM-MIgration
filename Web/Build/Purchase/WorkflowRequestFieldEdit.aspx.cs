using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowRequestFieldEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWFRequestFieldEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            Guid? Id = General.GetNullableGuid(Request.QueryString["PROCESSREQUESTFIELDID"].ToString());
            DataTable ds;
            ds = PhoenixWorkflowRequest.ProcessRequestFieldEdit(Id);
            if (ds.Rows.Count > 0)
            {

                DataRow dr = ds.Rows[0];
                txtprocess.Text = dr["PROCESSNAME"].ToString();
                txtFieldName.Text = dr["FLDFIELDNAME"].ToString();
                txtDataType.Text = dr["FLDDATATYPE"].ToString();
                txtLength.Text = dr["FLDLENGTH"].ToString();
                txtDefault.Text = dr["FLDDEFAULT"].ToString();

            }
        }
    }

    protected void MenuWFRequestFieldEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Id = General.GetNullableGuid(Request.QueryString["PROCESSREQUESTFIELDID"].ToString());
            string FieldName = General.GetNullableString(txtFieldName.Text);
            string DataType = General.GetNullableString(txtDataType.Text);
            int? Length = General.GetNullableInteger(txtLength.Text);
            int? Default = General.GetNullableInteger(txtDefault.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixWorkflowRequest.ProcessRequestFieldUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Id,FieldName,DataType,Length,Default);


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

}

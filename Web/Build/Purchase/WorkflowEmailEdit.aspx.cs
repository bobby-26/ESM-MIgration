using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class WorkflowEmailEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["DTKEY"] = General.GetNullableGuid(Request.QueryString["DTKEY"].ToString());

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuWorkEmailEdit.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                Guid? Id = General.GetNullableGuid(Request.QueryString["DTKEY"].ToString());
                DataTable ds;
                ds = PhoenixWorkflow.WorkflowemailEdit(Id);
                if (ds.Rows.Count > 0)
                {

                    DataRow dr = ds.Rows[0];
                    lblProcessTransitionActivity.Text = dr["FLDPROCESSTRANSITIONACTIVITYID"].ToString();
                    lblActivity.Text = dr["ACTIVITYNAME"].ToString();
                    lblDescription.Text = dr["FLDDESCRIPTION"].ToString();
                    lblEmailId.Text = dr["FLDEMAILID"].ToString();
                    lblTo.Text = dr["FLDTO"].ToString();
                    lblCC.Text = dr["FLDCC"].ToString();
                    lblSubject.Text = dr["FLDSUBJECT"].ToString();
                    lblBody.Text = dr["FLDBODY"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkEmailEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? ProcessTransitionActivity = General.GetNullableGuid(lblProcessTransitionActivity.Text);
            string Description = General.GetNullableString(lblDescription.Text);
            Guid? EmailId = General.GetNullableGuid(lblEmailId.Text);
            string To = General.GetNullableString(lblTo.Text);
            string CC = General.GetNullableString(lblCC.Text);
            string Subject = General.GetNullableString(lblSubject.Text);
            string Body = General.GetNullableString(lblBody.Text);


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidEmail(Description,To, CC, Subject, Body))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixWorkflow.WorkflowEmailUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    ProcessTransitionActivity,
                                                    Description,
                                                    EmailId,
                                                    To,
                                                    CC,
                                                    Subject,
                                                    Body                                                
                                                    );

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


    private bool IsValidEmail(string Description,string To, string CC, string Subject, string Body)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";

        if (To == null)
            ucError.ErrorMessage = "To is required.";

        if (CC == null)
            ucError.ErrorMessage = "CC is required.";

        if (Subject == null)
            ucError.ErrorMessage = "Subject is required.";

        if (Body == null)
            ucError.ErrorMessage = "Body is required.";

        return (!ucError.IsError);
    }


}
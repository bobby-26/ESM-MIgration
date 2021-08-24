using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Inspection_InspectionBulkPreventiveTaskCompletion : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            BindComments();
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddImageLink("javascript:openNewWindow('attachment','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"]
                    + "&mod=" + PhoenixModule.QUALITY
                    + "&type=SHIPBOARDEVIDENCE"
                    + "&cmdname=SHIPBOARDEVIDENCEUPLOAD"
                    + "&VESSELID=" + ViewState["VESSELID"]
                    + "'); return false;", "Attachments", "", "ATTACHMENT", ToolBarDirection.Right);
            //toolbarmain.AddImageLink("javascript:Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["ATTACHMENTCODE"] + "&mod=" + PhoenixModule.QUALITY + "&type=AUDITINSPECTION" + "&cmdname=AUDITINSPECTIONUPLOAD" + "'); return false;", "Attachments", "", "ATTACHMENT");
            toolbarmain.AddButton("Complete", "SAVE",ToolBarDirection.Right);
            MenuOfficeRemarks.AccessRights = this.ViewState;
            MenuOfficeRemarks.MenuList = toolbarmain.Show();
        }
    }

    protected void MenuOfficeRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableString(txtOfficeRemarks.Text) == null && General.GetNullableString(txtOfficeRemarks.Text) == string.Empty)
                {
                    ucError.ErrorMessage = "Completion remarks is required.";
                    ucError.Visible = true;
                    return;
                }
                if (General.GetNullableDateTime(ucCompletionDate.Text) == null)
                {
                    ucError.ErrorMessage = "Completion Date is required.";
                    ucError.Visible = true;
                    return;
                }
                if (Filter.CurrentSelectedOfficeTasks != null)
                {
                    ArrayList selectedofficetask = (ArrayList)Filter.CurrentSelectedOfficeTasks;
                    if (selectedofficetask != null && selectedofficetask.Count > 0)
                    {
                        foreach (Guid preventiveactionid in selectedofficetask)
                        {
                            PhoenixInspectionLongTermAction.OfficeTaskBulkCompletionUpdate(
                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                     , new Guid(preventiveactionid.ToString())                                                     
                                                     , txtOfficeRemarks.Text
                                                     , General.GetNullableDateTime(ucCompletionDate.Text)   
                                                     );
                        }
                    }
                }
            }
            Filter.CurrentSelectedOfficeTasks = null;
            ucStatus.Text = "Completion remarks updated for selected tasks.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindComments()
    {
        if (Filter.CurrentSelectedOfficeTasks != null)
        {
            ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedOfficeTasks;

            if (selectedcomments != null && selectedcomments.Count > 0)
            {
                foreach (Guid commentid in selectedcomments)
                {
                    DataTable dt = PhoenixInspectionLongTermAction.OfficeTaskBulkCompletionEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(commentid.ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["ATTACHMENTCODE"] = dt.Rows[0]["FLDDTKEY"].ToString();
                        ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
                    }
                }
            }
        }
    }
}

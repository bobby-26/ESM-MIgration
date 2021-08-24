using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagement_DocumentManagementBulkReviewOfficeRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuOfficeRemarks.AccessRights = this.ViewState;
        MenuOfficeRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
            {
                ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();
            }
            if (Request.QueryString["COMMENTID"] != null && Request.QueryString["COMMENTID"].ToString() != string.Empty)
            {
                ViewState["COMMENTID"] = Request.QueryString["COMMENTID"].ToString();
            }

            BindComments();
        }


        if (rblbtn1.SelectedValue == "0")
        {
            ucCommentsDueDate.Enabled = false;
            ucCommentsDueDate.CssClass = "input";
            //ucCommentsDueDate.ReadOnly = true;
        }
        else
        {
            ucCommentsDueDate.Enabled = true;
            ucCommentsDueDate.CssClass = "input_mandatory";           
        }
    }

    protected void MenuOfficeRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (General.GetNullableString(txtOfficeRemarks.Text) == null)
            //{
            //    lblMessage.Text = "Office comments is required.";
            //    return;
            //}

            if (rblbtn1.SelectedIndex == -1)
            {
                ucError.ErrorMessage = "Please select option accepted Y/N";
                ucError.Visible = true;
                return;
            }
                        
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Filter.CurrentSelectedComments != null)
                {
                    ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;
                    if (selectedcomments != null && selectedcomments.Count > 0)
                    {
                        foreach (Guid commentid in selectedcomments)
                        {
                            PhoenixDocumentManagementDocument.DMSCommentReviewUpdate(
                                                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                     , new Guid(commentid.ToString())
                                                     , General.GetNullableInteger(rblbtn1.SelectedItem.Value)
                                                     , General.GetNullableDateTime(ucCommentsDueDate.Text)
                                                     , txtOfficeRemarks.Text
                                                     );                            
                        }
                    }
                }
            }
            Filter.CurrentSelectedComments = null;
            ucStatus.Text = "Office comments updated for selected comments.";
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
        if (Filter.CurrentSelectedComments != null)
        {
            ArrayList selectedcomments = (ArrayList)Filter.CurrentSelectedComments;    
            int rowcount = 0;        
            txtOfficeRemarks.Text = null;
            DataTable dt1 = new DataTable();

            if (selectedcomments != null && selectedcomments.Count > 0)
            {
                foreach (Guid commentid in selectedcomments)
                {
                    DataTable dt = PhoenixDocumentManagementDocumentSectionRevison.DocumentSectionRevisonRemarks(new Guid(commentid.ToString()));
                    if (dt.Rows.Count > 0 && (dt.Rows[0]["FLDCOMMENTS"].ToString() != "" || dt.Rows[0]["FLDCOMMENTS"].ToString() != null))
                    {
                        dt1.Merge(dt);
                        dt1.DefaultView.Sort = "FLDPOSTEDDATE DESC";
                        repDiscussion.DataSource = dt1;                        
                        repDiscussion.DataBind();
                        dt1.PrimaryKey = new[] { dt1.Columns["FLDCOMMENTS"] };
                        dt.PrimaryKey = new[] { dt.Columns["FLDCOMMENTS"] };
                    }
                }
                for (rowcount = 0; rowcount < dt1.Rows.Count; rowcount++)
                {
                    txtOfficeRemarks.Text = txtOfficeRemarks.Text + "\r\n\r\n" + dt1.Rows[rowcount]["FLDCOMMENTS"].ToString();
                }
            }
        }
    }
}

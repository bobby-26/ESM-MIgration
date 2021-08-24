using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Post", "POSTCOMMENT", ToolBarDirection.Right);

        MenuBugDiscussion.AccessRights = this.ViewState;
        MenuBugDiscussion.MenuList = toolbaredit.Show();

        if (!IsPostBack)
        {
            ViewState["REFERENCEID"] = null;

            if (Filter.CurrentDMSDocumentSelection != null && General.GetNullableGuid(Filter.CurrentDMSDocumentSelection.ToString()) != null)
                ViewState["REFERENCEID"] = Filter.CurrentDMSDocumentSelection.ToString();

            //if (Request.QueryString["REFERENCEID"] != null && Request.QueryString["REFERENCEID"].ToString() != string.Empty)
            //    ViewState["REFERENCEID"] = Request.QueryString["REFERENCEID"].ToString();


            if (chkReviewedYN.Checked == true)
                txtNotesDescription.Text = "";
        }
        else
        {
            if (chkReviewedYN.Checked == true)
                txtNotesDescription.Text = "This Section has been reviewed and no changes required";
        }

        BindData();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        if (ViewState["REFERENCEID"] != null && ViewState["REFERENCEID"].ToString() != "")
        {
            dt = PhoenixDocumentManagementDocument.ListDMSComments(new Guid(ViewState["REFERENCEID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                //if (dt.Rows[0]["FLDACCEPTEDDOCUMENTYN"].ToString() == "1")
                //    chkReviewedYN.Checked = true;
                repDiscussion.DataSource = dt;
                repDiscussion.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, repDiscussion);
            }
        }
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }

    protected void MenuBugDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string notes = null;
        
        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (chkReviewedYN.Checked == true)
            {                
                notes = chkReviewedYN.Text;
            }
            else
            {
                notes = txtNotesDescription.Text;
            }

            if (ViewState["REFERENCEID"] != null && ViewState["REFERENCEID"].ToString() != "")
                PhoenixDocumentManagementDocument.DMSCommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                                new Guid(ViewState["REFERENCEID"].ToString()), 
                                General.GetNullableString (notes),
                                chkReviewedYN.Checked == true ? 1 : 0,
                                null,
                                null);

            txtNotesDescription.Text = "";
            BindData();
        }
    }

    private bool IsCommentValid(string strComment)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (strComment.Trim() == "")
            ucError.ErrorMessage = "Comments is required";

        return (!ucError.IsError);
    }
}

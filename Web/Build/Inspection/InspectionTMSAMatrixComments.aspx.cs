using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class Inspection_InspectionTMSAMatrixComments : PhoenixBasePage
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
            ViewState["CATEGORYID"] = null;
            ViewState["CONTENTID"] = null;

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != string.Empty)
                ViewState["CONTENTID"] = Request.QueryString["contentid"].ToString();

        }
        BindData();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        if (!string.IsNullOrEmpty(ViewState["CATEGORYID"].ToString()) && !string.IsNullOrEmpty(ViewState["CONTENTID"].ToString()))
        {
            dt = PhoenixInspectionTMSAMatrix.ListTMSAComments(new Guid(ViewState["CATEGORYID"].ToString()), new Guid(ViewState["CONTENTID"].ToString()));

            if (dt.Rows.Count > 0)
            {
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

        //string notes = null;

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(ViewState["CATEGORYID"].ToString()) && !string.IsNullOrEmpty(ViewState["CONTENTID"].ToString()))
            { 
                PhoenixInspectionTMSAMatrix.InspectionTMSACommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["CATEGORYID"].ToString()),
                                new Guid(ViewState["CONTENTID"].ToString()),
                                General.GetNullableString(txtNotesDescription.Text)                                
                                );
            }

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
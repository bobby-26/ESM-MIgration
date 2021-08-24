using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;
public partial class Inspection_InspectionCDISIREMatrixComments : PhoenixBasePage
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
            ViewState["INSPECTIONID"] = "";

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != string.Empty)
                ViewState["CONTENTID"] = Request.QueryString["contentid"].ToString();

            if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

        }
        BindData();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        if (!string.IsNullOrEmpty(ViewState["CATEGORYID"].ToString()) && !string.IsNullOrEmpty(ViewState["CONTENTID"].ToString()))
        {
            dt = PhoenixInspectionCDISIREMatrix.ListCDISIREComments(new Guid(ViewState["CATEGORYID"].ToString()), new Guid(ViewState["CONTENTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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
                PhoenixInspectionCDISIREMatrix.InspectionCDISIRECommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["CATEGORYID"].ToString()),
                                new Guid(ViewState["CONTENTID"].ToString()),
                                General.GetNullableString(txtNotesDescription.Text),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                new Guid(ViewState["INSPECTIONID"].ToString())
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
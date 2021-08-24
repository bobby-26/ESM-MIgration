using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionRAComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Post", "POSTCOMMENT", ToolBarDirection.Right);

        MenuRAComments.AccessRights = this.ViewState;
        MenuRAComments.MenuList = toolbaredit.Show();

        if (!IsPostBack)
        {
            ViewState["RAID"] = "";
            ViewState["RATYPEID"] = "0";
            ViewState["SECTIONID"] = "0";

            if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                ViewState["RAID"] = Request.QueryString["RAID"].ToString();

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

            if (Request.QueryString["RATYPEID"] != null && Request.QueryString["RATYPEID"].ToString() != string.Empty)
                ViewState["RATYPEID"] = Request.QueryString["RATYPEID"].ToString();

        }

        BindData();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
        {
            dt = PhoenixInspectionRiskAssessmentJobHazardExtn.ListRAComments(new Guid(ViewState["RAID"].ToString()),int.Parse(ViewState["RATYPEID"].ToString()),int.Parse(ViewState["SECTIONID"].ToString()));

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

    protected void MenuRAComments_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != "")
                PhoenixInspectionRiskAssessmentJobHazardExtn.RACommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                new Guid(ViewState["RAID"].ToString()), int.Parse(ViewState["RATYPEID"].ToString()), int.Parse(ViewState["SECTIONID"].ToString()),
                                General.GetNullableString(txtNotesDescription.Text.Trim()),
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
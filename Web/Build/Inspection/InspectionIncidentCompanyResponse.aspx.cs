using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionIncidentCompanyResponse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["Vesselid"] = "";

        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Post", "POSTCOMMENT", ToolBarDirection.Right);
        MenuBugDiscussion.AccessRights = this.ViewState;
        MenuBugDiscussion.MenuList = toolbaredit.Show();
        
        if (!IsPostBack)
        {            
            BindData();
        }
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }   

    private void BindData()
    {
        DataTable dt = new DataTable();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (Filter.CurrentIncidentID != null && Filter.CurrentIncidentID.ToString() != "")
        {
            dt = PhoenixInspectionIncident.IncidentComments(new Guid(Filter.CurrentIncidentID.ToString()));

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

        if (CommandName.ToUpper().Equals("POSTCOMMENT"))
        {
            if (!IsCommentValid(txtNotesDescription.Text))
            {
                ucError.Visible = true;
                return;
            }
            if (Filter.CurrentIncidentID != null && Filter.CurrentIncidentID.ToString() != "")
                PhoenixInspectionIncident.IncidentCommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Filter.CurrentIncidentID.ToString()), txtNotesDescription.Text);

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

    protected void GetRemarks(object sender, EventArgs e)
    {
        BindData();
    }
}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;


public partial class Inspection_InspectionCDISIREMatrixCommentsRevisionView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        MenuBugDiscussion.AccessRights = this.ViewState;
        MenuBugDiscussion.MenuList = toolbaredit.Show();

        if (!IsPostBack)
        {
            ViewState["CATEGORYID"] = null;
            ViewState["REVISIONID"] = null;

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["revisionid"] != null && Request.QueryString["revisionid"].ToString() != string.Empty)
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();

        }
        BindData();

        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "DivResize", "resizediv();", true);
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        if (ViewState["CATEGORYID"] != null && ViewState["CATEGORYID"].ToString() != "")
        {
            dt = PhoenixInspectionCDISIREMatrix.ListCDISIRERevisionComments(new Guid(ViewState["CATEGORYID"].ToString()), new Guid(ViewState["REVISIONID"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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
}
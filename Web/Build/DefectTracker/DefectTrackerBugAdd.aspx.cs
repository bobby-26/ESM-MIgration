using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerBugAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbar = new PhoenixToolbar();
        PhoenixToolbar toolbarbugadd = new PhoenixToolbar();
        toolbarbugadd.AddButton("Save", "SAVE");
        if (!IsPostBack)
        {
            string projname = Request.QueryString["projectname"].ToString();
            ViewState["PROJECTNAME"] = projname;
            BindProjectList();
            ddlProject.SelectedValue = ViewState["PROJECTNAME"].ToString();
            BindVesselList();
            MenuBugAdd.AccessRights = this.ViewState;
            MenuBugAdd.MenuList = toolbarbugadd.Show();
            MenuBugAdd.SetTrigger(pnlBugEntry);
            BindDefaultVessel();
            ddlSEPBugStatus.SelectedValue = "24"; //New Status             

        }

    }

    private void BindProjectList()
    {
        DataTable dt = PhoenixDefectTracker.ProjectList(null);
        if (dt.Rows.Count > 0)
        {
            ddlProject.DataSource = dt;
            ddlProject.DataValueField = "FLDPROJECTID";
            ddlProject.DataTextField = "FLDPROJECTNAME";
            ddlProject.DataBind();
        }
    }

    private void BindVesselList()
    {
        if (ddlProject.SelectedValue.Trim() != string.Empty)
        {
            DataTable dt = PhoenixDefectTracker.vessellist(int.Parse(ddlProject.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                ddlvesselcode.DataSource = dt;
                ddlvesselcode.DataValueField = "FLDVESSELID";
                ddlvesselcode.DataTextField = "FLDVESSELNAME";
                ddlvesselcode.DataBind();
                ddlvesselcode.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
        }
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindVesselList();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDefaultVessel()
    {
        DataTable dt = PhoenixDefectTracker.DefaultVessel();

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            int nVessel = int.Parse(dr["FLDINSTALLCODE"].ToString());

            if (nVessel > 0)
            {
                ddlvesselcode.SelectedValue = dr["FLDINSTALLCODE"].ToString();
                ddlvesselcode.Enabled = false;
            }
        }
    }

    protected void MenuBugAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (!IsValidBug())
            {
                ucError.Visible = true;
                return;
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid dtkey = Guid.NewGuid();

                PhoenixDefectTracker.BugSave(-1,
                    int.Parse(ddlModuleList.SelectedValue), 15
                    , txtSubject.Text
                    , txtDescription.Text
                    , ddlSEPBugStatus.SelectedValue
                    , ddlBugType.SelectedValue
                    , ddlBugPriority.SelectedValue
                    , ddlBugSeverity.SelectedValue, PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , "1.0", "Phoenix", null, null, null, "sep@southnests.com", General.GetNullableInteger(ddlvesselcode.SelectedValue)
                    , txtReportedBy.Text, null, int.Parse(ddlProject.SelectedValue), ref dtkey);

                ucStatus.Text = "Issue saved";
                String script = "javascript:fnDefectTrackerBugEdit('" + dtkey.ToString() + "'); javascript:fnReloadList('code1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                //Response.Redirect("DefectTrackerBugEdit.aspx?dtkey=" + dtkey.ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        ddlModuleList.SelectedValue = "";
        ddlBugPriority.SelectedValue = "";
        ddlBugSeverity.SelectedValue = "";
        txtDescription.Text = "";
        txtSubject.Text = "";
        ddlBugType.SelectedValue = "";
        ddlSEPBugStatus.BugId = -1;
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {

    }

    private bool IsValidBug()
    {
        if (General.GetNullableInteger(ddlModuleList.SelectedValue) == null)
            ucError.ErrorMessage = "Module is required";

        if (General.GetNullableInteger(ddlBugPriority.SelectedValue) == null)
            ucError.ErrorMessage = "Priority is required";

        if (General.GetNullableInteger(ddlBugSeverity.SelectedValue) == null)
            ucError.ErrorMessage = "Severity is required";

        if (General.GetNullableString(ddlProject.SelectedValue) == null)
            ucError.ErrorMessage = "Project is required";

        if (General.GetNullableInteger(ddlBugType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";

        if (General.GetNullableString(txtSubject.Text) == null)
            ucError.ErrorMessage = "Subject is required";

        if (General.GetNullableString(txtDescription.Text) == null)
            ucError.ErrorMessage = "Description is required";

        if (General.GetNullableInteger(ddlSEPBugStatus.SelectedValue) == null)
            ucError.ErrorMessage = "Status is required";

        return !ucError.IsError;
    }

}

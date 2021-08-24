using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;


public partial class Dashboard_DashboardBSCIssue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            Guid? kpiid = General.GetNullableGuid(Request.QueryString["kpiid"]);

            DataTable dt = PhoenixDashboardBSC.issuepilist(kpiid);
            Radcombopilist.DataSource = dt;

            Radcombopilist.DataBind();
            radcbmonth.SelectedMonth = DateTime.Now.Month.ToString();
            radcbyear.SelectedYear = DateTime.Now.Year;

            DataTable dt1 = PhoenixDashboardBSC.officestafflist();
            Radcombodesignationlist.DataSource = dt1;

            Radcombodesignationlist.DataBind();

            if (General.GetNullableString(Request.QueryString["edit"]) == "yes")
            {
                Guid? issueid = General.GetNullableGuid(Request.QueryString["issueid"]);
                ViewState["ISSUEID"] = issueid;
                DataTable dt2 = PhoenixDashboardBSC.IssueEditList(General.GetNullableGuid(Request.QueryString["issueid"]));

                radcbmonth.SelectedMonth = dt2.Rows[0]["FLDMONTH"].ToString();
                radcbyear.SelectedYear = Int32.Parse(dt2.Rows[0]["FLDYEAR"].ToString());
                Radcombopilist.Value = dt2.Rows[0]["FLDPIID"].ToString();
                radtbissueentry.Text = dt2.Rows[0]["FLDISSUE"].ToString();
                radtbimplicationentry.Text = dt2.Rows[0]["FLDIMPLICATION"].ToString();
                radtbactionentry.Text = dt2.Rows[0]["FLDACTIONS"].ToString();
                Radcombodesignationlist.Value = dt2.Rows[0]["FLDASSIGNEDTO"].ToString();
                radtargetdate.Text = dt2.Rows[0]["FLDTARGETDATE"].ToString();

            }

        }
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        if (General.GetNullableString(Request.QueryString["edit"]) == "yes")
        { toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right); }
        else
        {
            toolbargrid.AddButton("Save", "Save", ToolBarDirection.Right);
        }

        TabstripMenu.MenuList = toolbargrid.Show();
    }

    protected void TabstripMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            { int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? month = General.GetNullableInteger(radcbmonth.SelectedMonth.ToString());
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? Piid = General.GetNullableGuid(Radcombopilist.Value);
                string issue = General.GetNullableString(radtbissueentry.Text);
                string implication = General.GetNullableString(radtbimplicationentry.Text);
                string action = General.GetNullableString(radtbactionentry.Text);
                int? assignedto = General.GetNullableInteger(Radcombodesignationlist.Value);
                DateTime? targetdate = General.GetNullableDateTime(radtargetdate.Text);
                PhoenixDashboardBSC.IssueInsert(rowusercode, Piid, month, year, issue, implication, action, assignedto, targetdate);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? month = General.GetNullableInteger(radcbmonth.SelectedMonth.ToString());
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? Piid = General.GetNullableGuid(Radcombopilist.Value);
                string issue = General.GetNullableString(radtbissueentry.Text);
                string implication = General.GetNullableString(radtbimplicationentry.Text);
                string action = General.GetNullableString(radtbactionentry.Text);
                int? assignedto = General.GetNullableInteger(Radcombodesignationlist.Value);
                DateTime? targetdate = General.GetNullableDateTime(radtargetdate.Text);
                Guid? issueid = General.GetNullableGuid(ViewState["ISSUEID"].ToString());
                PhoenixDashboardBSC.IssueUpdate(rowusercode, Piid, month, year, issue, implication, action, assignedto, targetdate,issueid);
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
}
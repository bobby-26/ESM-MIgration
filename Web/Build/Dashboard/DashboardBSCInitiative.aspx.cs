using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class Dashboard_DashboardBSCInitiative : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            Guid? kpiid = General.GetNullableGuid(Request.QueryString["kpiid"]);

            
            DataTable dt = PheonixDashboardSKKPI.KPIList();
            radcobkpi.DataSource = dt;
            radcobkpi.DataBind();
            radcobkpi.Value = kpiid.ToString();
            if (kpiid == null)
            {
                radcobkpi.Enable = true;
            }
            //radcbmonth.SelectedMonth = DateTime.Now.Month.ToString();
            //radcbyear.SelectedYear = DateTime.Now.Year;

            DataTable dt1 = PhoenixDashboardBSC.officestafflist();
            Radcombodesignationlist.DataSource = dt1;

            Radcombodesignationlist.DataBind();

            if (General.GetNullableString(Request.QueryString["edit"]) == "yes")
            {
                Guid? initiativeid = General.GetNullableGuid(Request.QueryString["initiativeid"]);
                ViewState["INITIATIVEID"] = initiativeid;
                DataTable dt2 = PhoenixDashboardBSCInitiative.InitiativeEditList(General.GetNullableGuid(Request.QueryString["initiativeid"]));

                radcbmonth.SelectedMonth = dt2.Rows[0]["FLDMONTH"].ToString();
                radcbyear.SelectedYear = Int32.Parse(dt2.Rows[0]["FLDYEAR"].ToString());
                radcobkpi.Value = dt2.Rows[0]["FLDKPIID"].ToString();
                radtbinitiativeentry.Text = dt2.Rows[0]["FLDINITIATIVE"].ToString();
                Radcombodesignationlist.Value = dt2.Rows[0]["FLDASSIGNEDTO"].ToString();
                radtargetdate.Text = dt2.Rows[0]["FLDTARGRETDATE"].ToString();

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
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? month = General.GetNullableInteger(radcbmonth.SelectedMonth.ToString());
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);
                string initiative = General.GetNullableString(radtbinitiativeentry.Text);
               
                int? assignedto = General.GetNullableInteger(Radcombodesignationlist.Value);
                DateTime? targetdate = General.GetNullableDateTime(radtargetdate.Text);
                PhoenixDashboardBSCInitiative.InitiativeInsert(rowusercode, initiative, assignedto, targetdate,kpiid, month, year);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                int? month = General.GetNullableInteger(radcbmonth.SelectedMonth.ToString());
                int? year = General.GetNullableInteger(radcbyear.SelectedYear.ToString());
                Guid? kpiid = General.GetNullableGuid(radcobkpi.Value);
                string initiative = General.GetNullableString(radtbinitiativeentry.Text);
                Guid? initiativeid = General.GetNullableGuid(ViewState["INITIATIVEID"].ToString());
                int? assignedto = General.GetNullableInteger(Radcombodesignationlist.Value);
                DateTime? targetdate = General.GetNullableDateTime(radtargetdate.Text);
                PhoenixDashboardBSCInitiative.InitiativeUpdate(rowusercode, initiative, assignedto, targetdate, kpiid, month, year, initiativeid);
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
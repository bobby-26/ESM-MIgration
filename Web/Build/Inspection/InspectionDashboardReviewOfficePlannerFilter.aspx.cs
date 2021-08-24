using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionDashboardReviewOfficePlannerFilter : PhoenixBasePage
{
   protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuDashboardScheduleFilter.AccessRights = this.ViewState;
        MenuDashboardScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            Bind_UserControls(sender, new EventArgs());
        }
    }

    protected void MenuDashboardScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucAudit", ddlAuditInspection.SelectedValue);
            criteria.Add("ucCompany", ucCompany.SelectedCompany);
            criteria.Add("ucFrom", ucFrom.Text);
            criteria.Add("ucTo", ucTo.Text);

            InspectionFilter.CurrentAuditInspectionSchedulrOfficeDashboardFilter = criteria;
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
    }
    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAuditInspection.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(null)
                                        , null
                                        , 0
                                        , General.GetNullableInteger(ucCompany.SelectedCompany));
        ddlAuditInspection.DataTextField = "FLDSHORTCODE";
        ddlAuditInspection.DataValueField = "FLDINSPECTIONID";
        ddlAuditInspection.DataBind();

        ddlAuditInspection.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ucCompany_TextChangedEvent(object sender, EventArgs e)
    {
        Bind_UserControls(sender, new EventArgs());
    }
}
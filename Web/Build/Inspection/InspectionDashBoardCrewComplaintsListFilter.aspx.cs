using System;
using System.Collections.Specialized;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class InspectionDashBoardCrewComplaintsListFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuOpenReportsFilter.AccessRights = this.ViewState;
        MenuOpenReportsFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

        }
    }

    protected void MenuOpenReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();

            criteria.Add("ucReviewCategory", ucReviewCategory.SelectedQuick);
            criteria.Add("ucDept", ucDept.SelectedDepartment);

            InspectionFilter.CurrentOpenReportCrewComplaintsDashboardFilter = criteria;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Filter', 'wo');", true);
        }

    }

}

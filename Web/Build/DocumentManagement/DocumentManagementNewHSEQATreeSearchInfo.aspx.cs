using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class DocumentManagementNewHSEQATreeSearchInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        lblInfo.Text = "- Used for advance searching. It will display matched title from Document Section, Forms and Checklist, Job Hazard Analysis and Process Risk Assessment.";
    }
}
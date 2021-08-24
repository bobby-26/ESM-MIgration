using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;

public partial class OwnersMonthlyReportModuleInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["CODE"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["CODE"]))
            {
                ViewState["CODE"] = Request.QueryString["CODE"];
            }
            Edit();
        }
    }
    private void Edit()
    {
        if (ViewState["CODE"].ToString() != null || ViewState["CODE"].ToString() != string.Empty)
        {
            DataTable dt = PhoenixOwnerReportComments.ListOwnerReportsInfo(ViewState["CODE"].ToString());
            if (dt.Rows.Count > 0)
            {
                lblInfo.Text = dt.Rows[0]["FLDINFO"].ToString();
            }
        }
    }
}
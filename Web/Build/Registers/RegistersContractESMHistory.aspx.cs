using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersContractESMHistory : PhoenixBasePage
{
    string ComponentId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ComponentId = Request.QueryString["subComponentId"];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataTable dt = PhoenixRegistersContract.ESMStandardWageHistorySearch(new Guid(ComponentId));
        gvHistory.DataSource = dt;
        gvHistory.VirtualItemCount = dt.Rows.Count;
    }
}

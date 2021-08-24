using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class DryDock_DrydockCostIncurredDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       

        if (!IsPostBack)
        {
            radtbdate.Text = Request.QueryString["date"];
            gvprogress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["ORDERID"] = General.GetNullableGuid(Request.QueryString["orderid"]);
            ViewState["VESSELID"] = General.GetNullableInteger(Request.QueryString["vslid"]);
            ViewState["QUOTATIONID"] = General.GetNullableGuid(Request.QueryString["quotationid"]);
        }
    }

    protected void gvprogress_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
       

        }

    protected void gvprogress_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Guid? orderid = General.GetNullableGuid(ViewState["ORDERID"].ToString());
        int? vesselid = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        Guid? Quotationid = General.GetNullableGuid(ViewState["QUOTATIONID"].ToString());
        DateTime? date = General.GetNullableDateTime(radtbdate.Text);
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataTable dt = PhoenixDryDockOrder.DrydockProgressSearch(orderid, vesselid, Quotationid, date, gvprogress.CurrentPageIndex + 1, gvprogress.PageSize, ref iRowCount, ref iTotalPageCount);
        gvprogress.DataSource = dt;
        gvprogress.VirtualItemCount = iRowCount;
    }

    protected void gvprogress_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
}
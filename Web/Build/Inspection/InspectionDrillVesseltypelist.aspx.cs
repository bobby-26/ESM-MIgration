using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
public partial class Registers_vesseltypelist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? drillid = General.GetNullableGuid( Request.QueryString["drillid"].ToString());
        SessionUtil.PageAccessRights(this.ViewState);
        ViewState["DRILLID"] = drillid;

        if (!IsPostBack)
        {
          
            gvvesseltypelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void gvvesseltypelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowcount = 0;
        Guid? drillid = General.GetNullableGuid(ViewState["DRILLID"].ToString());

        DataTable dt = PhoenixRegisterDrill.vesseltypelist(drillid, gvvesseltypelist.CurrentPageIndex + 1,
                                                gvvesseltypelist.PageSize,
                                                ref iRowcount);
        gvvesseltypelist.DataSource = dt;
        gvvesseltypelist.VirtualItemCount = iRowcount;

    }
}
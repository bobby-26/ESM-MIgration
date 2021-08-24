using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionVesselTypeSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Guid? trainingid = General.GetNullableGuid(Request.QueryString["trainingid"].ToString());

        ViewState["TRAININGID"] = trainingid;

        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            gvvesseltypelist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void gvvesseltypelist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowcount = 0;
        Guid? trainingid = General.GetNullableGuid(ViewState["TRAININGID"].ToString());

        DataTable dt = PhoenixRegisterTraining.vesseltypelist(trainingid, gvvesseltypelist.CurrentPageIndex + 1,
                                                gvvesseltypelist.PageSize,
                                                ref iRowcount);
        gvvesseltypelist.DataSource = dt;
        gvvesseltypelist.VirtualItemCount = iRowcount;

    }
}
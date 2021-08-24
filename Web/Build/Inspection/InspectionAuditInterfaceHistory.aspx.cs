using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionAuditInterfaceHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["REVIEWSCHEDULEID"] != null)
                {
                    ViewState["REVIEWSCHEDULEID"] = Request.QueryString["REVIEWSCHEDULEID"].ToString();
                }

                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                }

                if (Request.QueryString["MAPPINGID"] != null)
                {
                    ViewState["MAPPINGID"] = Request.QueryString["MAPPINGID"].ToString();
                }

            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvInspectionhistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
           ds = PhoenixInspectionAuditInterfaceDetails.InterfacevesselCheckitemHistoryList(
                                General.GetNullableGuid(ViewState["REVIEWSCHEDULEID"].ToString())
                                , General.GetNullableGuid(ViewState["MAPPINGID"].ToString())
                                , General.GetNullableInteger(ViewState["VESSELID"].ToString())

                );
            gvInspectionhistory.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
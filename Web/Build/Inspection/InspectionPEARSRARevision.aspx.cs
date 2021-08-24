using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionPEARSRARevision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["RAID"] = "";

                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                    ViewState["RAID"] = Request.QueryString["RAID"].ToString();

                gvPEARSRARevision.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvPEARSRARevision_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixInspectionPEARSRiskAssessment.RiskAssessmentRevisionList(new Guid(ViewState["RAID"].ToString()));

        gvPEARSRARevision.DataSource = ds;

    }

    protected void gvPEARSRARevision_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");
            LinkButton lnkJobActivity = (LinkButton)e.Item.FindControl("lnkJobActivity");

            if (lnkJobActivity != null)
            {
                lnkJobActivity.Attributes.Add("onclick", "openNewWindow('Details', '', '" + Session["sitepath"] + "/Inspection/InspectionPEARSRiskAssessmentDetails.aspx?RAID=" + lblRAid.Text + "&RevYN=1"+"'); return true;");
            }
        }
    }

    protected void gvPEARSRARevision_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadLabel lblRAid = (RadLabel)e.Item.FindControl("lblRAID");

            //if (e.CommandName.ToUpper().Equals("EDITROW"))
            //{
            //    Response.Redirect("../Inspection/InspectionPEARSRiskAssessmentDetails.aspx?RAID=" + lblRAid.Text + "&Rev=1", false);
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
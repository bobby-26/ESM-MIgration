using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement; 

public partial class DocumentManagementJHAView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["DOCUMENTID"] = "";

            ViewState["CURRENTINDEXSECTION"] = 1;
            ViewState["SECTIONID"] = "";

            ViewState["CURRENTINDEXFORM"] = 1;
            ViewState["FORMID"] = "";

            ViewState["CURRENTINDEXRISKASSESSMENT"] = 1;
            ViewState["PROCESSID"] = "";

            ViewState["CURRENTINDEXJOBHAZARD"] = 1;
            ViewState["JOBHAZARDID"] = "";

            if (Request.QueryString["keyword"] != null && Request.QueryString["keyword"].ToString() != "")
                ViewState["keyword"] = Request.QueryString["keyword"].ToString();
            else
                ViewState["keyword"] = "";
        }
    }

    private void BindJobHazard()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewJHA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );

            gvJobHazard.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
                {
                    ViewState["JOBHAZARDID"] = ds.Tables[0].Rows[0]["FLDJOBHAZARDID"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindJobHazard();
    }

    protected void gvJobHazard_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucJob = (UserControlToolTip)e.Item.FindControl("ucJob");
            RadLabel lblJob = (RadLabel)e.Item.FindControl("lblJob");
            if (lblJob != null)
            {
                ucJob.Position = ToolTipPosition.TopCenter;
                ucJob.TargetControlId = lblJob.ClientID;
            }

            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            RadLabel lblJobHazardId = (RadLabel)e.Item.FindControl("lblJobHazardId");
            if (lnkRefNo != null)
            {
                lnkRefNo.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefNo.CommandName);
                lnkRefNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisExtn.aspx?DMSYN=1&jobhazardid=" + lblJobHazardId.Text + "',false,900,600);return false;");
            }
        }

    }
}
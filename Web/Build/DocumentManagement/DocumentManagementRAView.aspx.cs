using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementRAView : PhoenixBasePage
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

    private void BindRiskAssessment()
    {
        try
        {
            int iRowCount = 0;

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

            DataSet ds = PhoenixDocumentManagementDocument.KeyWordSearchResultInNewRA(
                                                                  General.GetNullableString(ViewState["keyword"].ToString())
                                                                , ref iRowCount
                                                                , companyid
                                                                );

            gvRiskAssessment.DataSource = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableGuid(ViewState["PROCESSID"].ToString()) == null)
                {
                    ViewState["PROCESSID"] = ds.Tables[0].Rows[0]["FLDPROCESSID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessment_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            UserControlToolTip ucActivityCondition = (UserControlToolTip)e.Item.FindControl("ucActivityCondition");
            RadLabel lblActivityCondition = (RadLabel)e.Item.FindControl("lblActivityCondition");
            if (lblActivityCondition != null)
            {
                //lblActivityCondition.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucActivityCondition.ToolTip + "', 'visible');");
                //lblActivityCondition.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucActivityCondition.ToolTip + "', 'hidden');");
                ucActivityCondition.Position = ToolTipPosition.TopCenter;
                ucActivityCondition.TargetControlId = lblActivityCondition.ClientID;
            }

            UserControlToolTip ucProcessName = (UserControlToolTip)e.Item.FindControl("ucProcessName");
            RadLabel lblProcessName = (RadLabel)e.Item.FindControl("lblProcessName");
            if (lblProcessName != null)
            {
                //lblProcessName.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'visible');");
                //lblProcessName.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucProcessName.ToolTip + "', 'hidden');");
                ucProcessName.Position = ToolTipPosition.TopCenter;
                ucProcessName.TargetControlId = lblProcessName.ClientID;
            }

            LinkButton lnkRefNo = (LinkButton)e.Item.FindControl("lnkRefNo");
            RadLabel lblProcessId = (RadLabel)e.Item.FindControl("lblProcessId");
            if (lnkRefNo != null)
            {
                lnkRefNo.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefNo.CommandName);
                lnkRefNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAProcessExtn.aspx?DMSYN=1&processid=" + lblProcessId.Text + "',false,900,600);return false;");
            }
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindRiskAssessment();
    }
}
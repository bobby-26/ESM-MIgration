using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;
public partial class Inspection_InspectionTMSAMatrixMappedProcedure : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["contentid"] = "";
            ViewState["revisionid"] = "";

            if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != "")
                ViewState["contentid"] = Request.QueryString["contentid"].ToString();

            if (Request.QueryString["revisionid"] != null && Request.QueryString["revisionid"].ToString() != "")
                ViewState["revisionid"] = Request.QueryString["revisionid"].ToString();
        }
    }

    protected void gvprocedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        if (ViewState["revisionid"].ToString() != "")
        {
            DataSet dss = PhoenixInspectionTMSAMatrix.ArchivedFormPosterList(ViewState["contentid"] == null ? null : General.GetNullableGuid(ViewState["contentid"].ToString()),
                                                    null, 0, General.GetNullableGuid(ViewState["revisionid"].ToString()));
            gvprocedure.DataSource = dss;
        }
        else
        {
            DataSet dss = PhoenixInspectionTMSAMatrix.FormPosterList(ViewState["contentid"] == null ? null : General.GetNullableGuid(ViewState["contentid"].ToString()),
                                                    null, 0);
            gvprocedure.DataSource = dss;
        }
    }

    protected void gvprocedure_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblFormId != null)
            {
                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(lblFormId.Text), ref type);
                if (type == 2)
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + lblFormId.Text + "',false,800,500);return false;");
                else if (type == 3)
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblFormId.Text + "',false,800,500);return false;");
                else if (type == 5)
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "');return false;");
                }
                else if (type == 6)
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "',false,800,500);return false;");
                }

            }
        }
    }
}
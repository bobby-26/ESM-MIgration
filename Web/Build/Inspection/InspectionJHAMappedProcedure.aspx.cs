using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Common;

public partial class InspectionJHAMappedProcedure : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["JHAID"] = "";

            if (Request.QueryString["JHAID"] != null && Request.QueryString["JHAID"].ToString() != "")
                ViewState["JHAID"] = Request.QueryString["JHAID"].ToString();

            ViewState["JHAYN"] = "0";
            ViewState["JHAYN"] = Request.QueryString["JHAYN"];
        }
    }

    protected void gvprocedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["JHAYN"].ToString() == "1")
        {
            DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterList(ViewState["JHAID"] == null ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), General.GetNullableInteger("1"));
            gvprocedure.DataSource = dss;
        }
        else
        {
            DataSet dss = PhoenixInspectionRiskAssessmentProcessExtn.FormPosterList(ViewState["JHAID"] == null ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), General.GetNullableInteger("1"));
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
                if (lbltype.Text == "2")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + lblFormId.Text + "',false,700,700);return false;");
                else if (lbltype.Text == "3")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblFormId.Text + "',false,700,700);return false;");

            }
        }
    }
}
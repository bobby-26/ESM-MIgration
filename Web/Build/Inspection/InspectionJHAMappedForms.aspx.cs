using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionJHAMappedForms : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["JHAID"] = "";

            ViewState["JHAYN"] = "0";

            if (Request.QueryString["JHAID"] != null && Request.QueryString["JHAID"].ToString() != "")
                ViewState["JHAID"] = Request.QueryString["JHAID"].ToString();

            if (Request.QueryString["JHAYN"] != null && Request.QueryString["JHAYN"].ToString() != "")
                ViewState["JHAYN"] = Request.QueryString["JHAYN"].ToString();
        }
    }

    protected void gvForms_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text == "5") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "',false,700,700);return false;");
                }
                if ((lbltype.Text == "6") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "',false,700,700);return false;");
                }
            }
        }
    }

    protected void gvForms_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if (ViewState["JHAYN"].ToString() == "1")
        {
            DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.FormPosterList(ViewState["JHAID"] == null ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), General.GetNullableInteger("0"));
            gvForms.DataSource = dss;
        }
        else
        {
            DataSet dss = PhoenixInspectionRiskAssessmentProcessExtn.FormPosterList(ViewState["JHAID"] == null ? null : General.GetNullableGuid(ViewState["JHAID"].ToString()), General.GetNullableInteger("0"));
            gvForms.DataSource = dss;
        }
        
    }
}
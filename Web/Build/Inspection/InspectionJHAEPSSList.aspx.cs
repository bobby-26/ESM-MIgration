using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class InspectionJHAEPSSList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        if (ViewState["JHAYN"].ToString() == "1")
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListJHAEPPSWithIcons(new Guid(ViewState["JHAID"].ToString()));
            gvRAMiscellaneous.DataSource = DT;
        }
        else
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAEPPSWithIcons(new Guid(ViewState["JHAID"].ToString()));
            gvRAMiscellaneous.DataSource = DT;
        }
    }
    protected void gvRAMiscellaneous_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblFormId != null)
            {

                if ((lbltype.Text != "1") && (lblName.Text != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "',false,700,700);return false;");
                }
                if ((lbltype.Text == "1") && (lblName.Text != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "',false,700,700);return false;");
                }
            }
        }
    }

    protected void gvRAMiscellaneous_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
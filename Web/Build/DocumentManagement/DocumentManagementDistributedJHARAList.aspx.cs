using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class DocumentManagementDistributedJHARAList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["PAGENUMBERF"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCUMENTID"] = "";

                if (Request.QueryString["CATEGORYID"] != null && Request.QueryString["CATEGORYID"].ToString() != "")
                    ViewState["CATEGORYID"] = Request.QueryString["CATEGORYID"].ToString();
                else
                    ViewState["CATEGORYID"] = "";

                if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != "")
                    ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
                else
                    ViewState["TYPE"] = "";

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                else
                    ViewState["VESSELID"] = "";

                if (Request.QueryString["NEW"] != null && Request.QueryString["NEW"].ToString() != "")
                    ViewState["NEW"] = Request.QueryString["NEW"].ToString();
                else
                    ViewState["TYPE"] = "";

                gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
            BindCategory();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void BindCategory()
    {
        DataSet ds = PhoenixIntegrationDocumentManagement.EditRiskAssessmentActivity(int.Parse(ViewState["CATEGORYID"].ToString()));
        txtCategoryName.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
    }

    protected void gvDocument_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDREVISIONDETAILS" };
        string[] alCaptions = { "S.No", "Name", "Revision" };

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;


        if (ViewState["NEW"].ToString().Equals("1"))
        {
            ds = PheonixDocumentManagementDistributionExtn.DistributedJHARAListByCategory(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , General.GetNullableInteger(ViewState["CATEGORYID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                                 , gvDocument.CurrentPageIndex + 1
                                                                 , gvDocument.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixDocumentManagementDocument.DistributedJHARAListByCategory(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , General.GetNullableInteger(ViewState["CATEGORYID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                                 , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                                 , gvDocument.CurrentPageIndex + 1
                                                                 , gvDocument.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);
        }
        General.SetPrintOptions("gvDocument", "Documents", alCaptions, alColumns, ds);
        gvDocument.DataSource = ds;
        gvDocument.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvDocument_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {


            DataRowView dv = (DataRowView)e.Item.DataItem;

            RadLabel lblReferencid = (RadLabel)e.Item.FindControl("lblReferencid");

            UserControlToolTip ucWorkActivity = (UserControlToolTip)e.Item.FindControl("ucWorkActivity");
            RadLabel lblWorkActivity = (RadLabel)e.Item.FindControl("lblWorkActivity");
            if (lblWorkActivity != null)
            {
                //lblWorkActivity.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucWorkActivity.ToolTip + "', 'visible');");
                //lblWorkActivity.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucWorkActivity.ToolTip + "', 'hidden');");
                ucWorkActivity.Position = ToolTipPosition.TopCenter;
                ucWorkActivity.TargetControlId = lblWorkActivity.ClientID;
            }
        }
    }  
}



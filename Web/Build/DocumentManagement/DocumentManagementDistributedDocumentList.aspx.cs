using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class DocumentManagementDistributedDocumentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

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

                if (Request.QueryString["COMPANYID"] != null && Request.QueryString["COMPANYID"].ToString() != "")
                    ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();
                else
                    ViewState["COMPANYID"] = "";

                if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                else
                    ViewState["VESSELID"] = "";

                BindCategory();
                BindData();
                BindForm();

                gvDocument.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvForm.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void BindCategory()
    {
        DataSet ds = PhoenixDocumentManagementCategory.DocumentCategoryEdit(new Guid(ViewState["CATEGORYID"].ToString()));
        txtCategoryName.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
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

        ds = PhoenixDocumentManagementDocument.DistributedDocumentListByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                             , gvDocument.CurrentPageIndex + 1
                                                             , gvDocument.PageSize
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);
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

            RadLabel lblDocumentId = (RadLabel)e.Item.FindControl("lblDocumentId");

            UserControlToolTip ucDocumentName = (UserControlToolTip)e.Item.FindControl("ucDocumentName");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");
            if (lblDocumentName != null)
            {
                ucDocumentName.Position = ToolTipPosition.BottomCenter;
                ucDocumentName.TargetControlId = lblDocumentName.ClientID;

            }
        }
    }

    /*form grid */

    protected void gvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindForm();
    }


    private void BindForm()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSERIALNUMBER", "FLDDOCUMENTNAME", "FLDREVISIONDETAILS" };
        string[] alCaptions = { "S.No", "Name", "Revision" };

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementDocument.DistributedFormListByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , General.GetNullableGuid(ViewState["CATEGORYID"].ToString())
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                                                             , gvForm.CurrentPageIndex + 1
                                                             , gvForm.PageSize
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);
        General.SetPrintOptions("gvForm", "Form", alCaptions, alColumns, ds);
        gvForm.DataSource = ds;
        gvForm.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNTF"] = iRowCount;
        ViewState["TOTALPAGECOUNTF"] = iTotalPageCount;
    }
    protected void gvForm_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            DataRowView dr = (DataRowView)e.Item.DataItem;

            UserControlToolTip ucPurpose = (UserControlToolTip)e.Item.FindControl("ucPurpose");
            RadLabel lblPurpose = (RadLabel)e.Item.FindControl("lblPurpose");
            if (lblPurpose != null)
            {
                ucPurpose.Position = ToolTipPosition.BottomCenter;
                ucPurpose.TargetControlId = lblPurpose.ClientID;

            }
            DataRowView dv = (DataRowView)e.Item.DataItem;
            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            if (ucCompanyEdit != null)
            {
                ucCompanyEdit.CompanyList = PhoenixRegistersCompany.ListCompany();
                ucCompanyEdit.SelectedCompany = dv["FLDCOMPANYID"].ToString();
            }
            UserControlToolTip ucFilenameTT = (UserControlToolTip)e.Item.FindControl("ucFilenameTT");
            RadLabel lblDocumentName = (RadLabel)e.Item.FindControl("lblDocumentName");

            if (lblDocumentName != null)
            {
                ucFilenameTT.Position = ToolTipPosition.BottomCenter;
                ucFilenameTT.TargetControlId = lblDocumentName.ClientID;
            }

        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvForm.CurrentPageIndex = 0;
        gvDocument.CurrentPageIndex = 0;
        gvForm.Rebind();
        gvDocument.Rebind();

    }
}


using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersCertificateCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersCertificateCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersCategory.AccessRights = this.ViewState;
            MenuRegistersCategory.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                gvCategory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDORDER", "FLDSHORTCODE", "FLDCATEGORYNAME", "FLDCAPTION", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Order", "Code", "Name", "Caption", "Active Y/N" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersCertificatesCategory.CertificatesCategorySearch(
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Category", ds.Tables[0], alColumns, alCaptions, null, "");
    }

    protected void MenuRegistersCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDORDER", "FLDSHORTCODE", "FLDCATEGORYNAME", "FLDCAPTION", "FLDACTIVEYNSTATUS" };
        string[] alCaptions = { "Order", "Code", "Name", "Caption", "Active Y/N" };

        DataSet ds = PhoenixRegistersCertificatesCategory.CertificatesCategorySearch(
            (int)ViewState["PAGENUMBER"],
            gvCategory.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvCategory", "Category", alCaptions, alColumns, ds);

        gvCategory.DataSource = ds;
        gvCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void InsertCertificatesCategory(string ShortCode, string CategoryName, int? ActiveYN, int? Order, string Caption)
    {
        PhoenixRegistersCertificatesCategory.InsertCertificatesCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , ShortCode
             , CategoryName
             , ActiveYN
             , Order
             , Caption);
    }

    private void UpdateCertificatesCategory(string ShortCode, string CategoryName, int? ActiveYN, int? CategoryId,int?Order,string Caption)
    {
        PhoenixRegistersCertificatesCategory.UpdateCertificatesCategory(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , ShortCode
             , CategoryName
             , ActiveYN
             , CategoryId
             , Order
             , Caption);
    }
    private void DeleteCertificatesCategory(int? CategoryId)
    {
        PhoenixRegistersCertificatesCategory.DeleteCertificatesCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode, CategoryId);
    }

    private bool IsValidCategory(string ShortCode, string Category)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ShortCode == "")
            ucError.ErrorMessage = "Code is required.";

        if (Category == "")
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }
    protected void gvCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCategory.CurrentPageIndex + 1;
        BindData();
    }
    protected void Rebind()
    {
        gvCategory.SelectedIndexes.Clear();
        gvCategory.EditIndexes.Clear();
        gvCategory.DataSource = null;
        gvCategory.Rebind();
    }
    protected void gvCategory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvCategory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvCategory.MasterTableView.GetItems(GridItemType.Footer)[0];
                if (!IsValidCategory(((RadTextBox)footerItem.FindControl("txtShortCodeAdd")).Text.Trim()
                , ((RadTextBox)footerItem.FindControl("txtCategoryAdd")).Text.Trim()

                ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertCertificatesCategory(
                    ((RadTextBox)footerItem.FindControl("txtShortCodeAdd")).Text.Trim()
                    , ((RadTextBox)footerItem.FindControl("txtCategoryAdd")).Text.Trim()
                    , General.GetNullableInteger(((RadCheckBox)footerItem.FindControl("chkActiveYNAdd")).Checked ==  true ? "1" : "0")
                    , General.GetNullableInteger(((UserControlMaskNumber)footerItem.FindControl("ucOrderAdd")).Text)
                    , General.GetNullableString(((RadTextBox)footerItem.FindControl("txtCaptionAdd")).Text.Trim())
                );
                
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CategoryId"] = ((RadLabel)e.Item.FindControl("lblCategoryId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                Response.Redirect("../Registers/RegistersVesselSurveyPlanConfiguration.aspx?TemplateId=" + ((RadLabel)e.Item.FindControl("lblTemplateId")).Text);
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCategory_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidCategory(((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim()
                , ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text.Trim()
                ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateCertificatesCategory(
                     ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text.Trim()
                     , ((RadTextBox)e.Item.FindControl("txtCategoryEdit")).Text.Trim()
                     , General.GetNullableInteger(((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true ? "1" : "0")
                     , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCategoryId")).Text)
                     , General.GetNullableInteger(((UserControlMaskNumber)e.Item.FindControl("ucOrderEdit")).Text)
                     , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtCaptionEdit")).Text.Trim())
                 );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCategory_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteCertificatesCategory(General.GetNullableInteger(ViewState["CategoryId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class DocumentManagement_DocumentManagementFMSManualAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["FILENOID"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["FileNoID"] = Request.QueryString["FileNoID"].ToString();
                FileNoEdit(Request.QueryString["FileNoID"].ToString());
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();
            BindCategory();
        }
    }

    protected void BindCategory()
    {
        string category = Request.QueryString["categoryid"];

        ddlCategory.DataSource = PhoenixRegisterFMSManual.FMSManualCategoryList();
        ddlCategory.DataTextField = "FLDMANUALCATEGORY";
        ddlCategory.DataValueField = "FLDFMSMANUALCATEGORY";
        ddlCategory.DataBind();
        ddlCategory.SelectedValue = category;
    }

    private void BindSubCategory(int? catagory)
    {
        ddlSubCategory.DataSource = PhoenixRegisterFMSManual.FMSManualSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubCategory.DataValueField = "FLDFMSMANUALSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.ClearSelection();
        BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));
    }


    protected void MenuFMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidFileNo())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["FileNoID"] != null)
                {
                    DataSet ds = PhoenixDocumentManagementFMSManual.FMSManualInsert(txtManualno.Text
                                                        , new Guid(ViewState["FileNoID"].ToString())
                                                        , new Guid(ddlCategory.SelectedValue)
                                                        , new Guid(ddlSubCategory.SelectedValue)
                                                        , txtManualName.Text);
                    ucStatus.Text = "Manual No Updated";
                }
                else
                {
                    DataSet ds = PhoenixDocumentManagementFMSManual.FMSManualInsert(txtManualno.Text
                                                        , null
                                                        , new Guid(ddlCategory.SelectedValue)
                                                        , new Guid(ddlSubCategory.SelectedValue)
                                                        , txtManualName.Text);
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

            String script = String.Format("javascript:fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }
    private bool IsValidFileNo()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtManualno.Text.Equals(""))
            ucError.ErrorMessage = "Manual No is required.";

        if (txtManualName.Text.Equals(""))
            ucError.ErrorMessage = "Manual Name is required.";

        return (!ucError.IsError);
    }

    private void FileNoEdit(string FileNoID)
    {
        try
        {
            DataSet ds = PhoenixDocumentManagementFMSManual.EditFMSManualNo(new Guid(FileNoID));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtManualno.Text = dr["FLDMANUALNUMBER"].ToString();
                txtManualName.Text = dr["FLDMANUALNAME"].ToString();
                BindCategory();
                ddlCategory.SelectedValue = dr["FLDFMSMANUALCATEGORYID"].ToString();
                BindSubCategory(General.GetNullableInteger(ddlCategory.SelectedValue));
                ddlSubCategory.SelectedValue = dr["FLDFMSMANUALSUBCATEGORYID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
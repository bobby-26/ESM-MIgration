using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections;
using Telerik.Web.UI;

public partial class DocumentManagementFormCategoryBulkUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCategoryUpdate.AccessRights = this.ViewState;
        MenuCategoryUpdate.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {         
            BindFormCategory();
        }
    }

    protected void MenuCategoryUpdate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            string Script = "";

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidInput())
                {
                    lblMessage.Visible = true;
                    return;
                }

                string selectedforms = ",";
                ArrayList SelectedForms = (ArrayList)Filter.CurrentSelectedForms;
                if (SelectedForms != null && SelectedForms.Count > 0)
                {
                    foreach (Guid index in SelectedForms)
                    {
                        selectedforms = selectedforms + index + ",";
                    }

                    PhoenixDocumentManagementForm.FormCategoryBulkUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableString(selectedforms), General.GetNullableGuid(ddlFormCategory.SelectedValue));
                }

                Filter.CurrentSelectedForms = null;

                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                lblMessage.Visible = true;
                lblMessage.Text = "Category is updated.";
            }            
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }

    private bool IsValidInput()
    {
        lblMessage.Text = "";
        bool result = true;

        if (Filter.CurrentSelectedForms == null)
        {
            lblMessage.Text += "*Select the form to update the category." + "<br/>";
            result = false; 
        }
        if (General.GetNullableGuid(ddlFormCategory.SelectedValue) == null)
        {
            lblMessage.Text += "*Category is required." + "<br/>";
            result = false;
        }
        return result;
    }

    protected void BindFormCategory()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ddlFormCategory.Items.Clear();
        ddlFormCategory.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory(companyid);
        ddlFormCategory.DataTextField = "FLDCATEGORYNAME";
        ddlFormCategory.DataValueField = "FLDCATEGORYID";
        ddlFormCategory.DataBind();
        ddlFormCategory.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }
}

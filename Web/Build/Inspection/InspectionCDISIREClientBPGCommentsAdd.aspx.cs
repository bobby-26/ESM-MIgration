using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Inspection_InspectionCDISIREClientBPGCommentsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        // toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["CATEGORYID"] = null;
            ViewState["CONTENTID"] = null;

            if (Request.QueryString["categoryid"] != null && Request.QueryString["categoryid"].ToString() != string.Empty)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["contentid"] != null && Request.QueryString["contentid"].ToString() != string.Empty)
                ViewState["CONTENTID"] = Request.QueryString["contentid"].ToString();
            BindCompany();
        }

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
    }

    protected void BindCompany()
    {
        DataSet ds = PhoenixInspectionOilMajorComany.ListOilMajorCompany(1, null);
        ddlCompany.DataSource = ds;
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidDepartment(txtCommentsAdd.Text
                , ddlCompany.SelectedValue))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionCDISIREMatrix.InspectionCDISIREClientBPGCommentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        new Guid(ViewState["CATEGORYID"].ToString()),
                                        new Guid(ViewState["CONTENTID"].ToString()),
                                        new Guid(ddlCompany.SelectedValue),
                                        txtCommentsAdd.Text);
            ucStatus.Text = "Saved successfully.";
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('Edit','');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }

    }

    private bool IsValidDepartment(string comments, string company)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (comments.Trim().Equals(""))
            ucError.ErrorMessage = "BPG is required.";

        if (string.IsNullOrEmpty(company))
            ucError.ErrorMessage = "Client is required.";

        return (!ucError.IsError);
    }
}
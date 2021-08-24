using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class DocumentManagementCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblCategoryId.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CATEGORYID"] = null;

            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();

            BindDocumentCategoryTree();
            DocumentCategoryEdit();
            BindType();
            BindVesselTypeList();
        }
    }
    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }
    private void BindDocumentCategoryTree()
    {
        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        ds = PhoenixDocumentManagementCategory.ListTreeDocumentCategory(companyid);

        tvwDocumentCategory.DataTextField = "FLDCATEGORYNAME";
        tvwDocumentCategory.DataValueField = "FLDCATEGORYID";
        tvwDocumentCategory.DataFieldParentID = "FLDPARENTGROUPID";
        tvwDocumentCategory.RootText = "ROOT";
        tvwDocumentCategory.PopulateTree(ds.Tables[0]);
    }

    private void BindType()
    {
        DataSet ds = new DataSet();
        ds = PhoenixDocumentManagementCategory.ListRACategory();

        ddlType.DataSource = ds;
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDSNO";
        ddlType.DataBind();
        ddlType.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        lblCategoryId.Text = tvsne.Node.Text.ToString();
        chkCheckAll.Checked = false;

        if (lblSelectedNode.Text == "_Root")
        {
            lblSelectedNode.Text = "";
            lblCategoryId.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            lblCategoryId.Text = tvsne.Node.Text.ToString();
            DocumentCategoryEdit();
        }
    }
    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidDocumentCategory())
            {
                ucError.Visible = true;
                return;
            }
            if (lblCategoryId.Text == "")
            {
                try
                {
                    PhoenixDocumentManagementCategory.InsertDocumentCategory(
                                                         General.GetNullableGuid(lblSelectedNode.Text),
                                                         txtDocumentCategory.Text.Trim(),
                                                         chkActiveyn.Checked == true ? General.GetNullableInteger("1") : General.GetNullableInteger("0"),
                                                         txtCategoryNumber.Text,
                                                         General.GetNullableGuid(null),
                                                         General.GetNullableInteger(ddlType.SelectedValue),
                                                         General.GetNullableInteger(ucCompany.SelectedCompany),
                                                         General.GetNullableString(General.RadCheckBoxList(chkVesselType))
                                                         );
                    Reset();
                    ucStatus.Text = "Category is added.";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                try
                {
                    //update
                    PhoenixDocumentManagementCategory.UpdateDocumentCategory(
                                                        new Guid(lblSelectedNode.Text),
                                                        txtDocumentCategory.Text.Trim(),
                                                        chkActiveyn.Checked == true ? General.GetNullableInteger("1") : General.GetNullableInteger("0"),
                                                        txtCategoryNumber.Text,
                                                        General.GetNullableGuid(null),
                                                        General.GetNullableInteger(ddlType.SelectedValue),
                                                        General.GetNullableInteger(ucCompany.SelectedCompany),
                                                        General.GetNullableString(General.RadCheckBoxList(chkVesselType))
                                                        );
                    ucStatus.Text = "Category updated.";
                    chkCheckAll.Checked = false;
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            BindDocumentCategoryTree();
        }
        if (CommandName.ToUpper().Equals("DELETE"))
        {
            try
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "Root cannot be deleted.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixDocumentManagementCategory.DeleteDocumentCategory(new Guid(lblSelectedNode.Text));
                    Reset();
                    lblSelectedNode.Text = "";
                    ucStatus.Text = "Category deleted.";
                }
                BindDocumentCategoryTree();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }
    public bool IsValidDocumentCategory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtDocumentCategory.Text.Trim()))
            ucError.ErrorMessage = "Category name is required.";

        if (string.IsNullOrEmpty(txtCategoryNumber.Text.Trim()))
            ucError.ErrorMessage = "Category number is required.";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }
    protected void DocumentCategoryEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "_ROOT")
        {
            ucCompany.SelectedCompany = "";
            DataSet ds = PhoenixDocumentManagementCategory.DocumentCategoryEdit(new Guid(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();

                txtDocumentCategory.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNAME"].ToString();
                chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                lblCategoryId.Text = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                txtCategoryNumber.Text = ds.Tables[0].Rows[0]["FLDCATEGORYNUMBER"].ToString();
                //rListAdd.SelectedValue = ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString();

                ddlType.SelectedValue = ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString() != null ? ds.Tables[0].Rows[0]["FLDACCESSLEVEL"].ToString() : "Dummy";

                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();

                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;

                General.RadBindCheckBoxList(chkVesselType, ds.Tables[0].Rows[0]["FLDVESSELTYPELIST"].ToString());
            }
        }
        else
        {
            txtDocumentCategory.Text = "";
            chkActiveyn.Checked = false;
            lblCategoryId.Text = "";
            ucCompany.SelectedCompany = "";
            txtCategoryNumber.Text = "";
        }
    }
    private void Reset()
    {
        txtDocumentCategory.Text = "";
        chkActiveyn.Checked = false;
        lblCategoryId.Text = "";
        txtCategoryNumber.Text = "";
        ucCompany.SelectedCompany = "";
        chkCheckAll.Checked = false;
        //chkVesselType.SelectedValue = null;
        ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
        BindType();

        foreach (ButtonListItem li in chkVesselType.Items)
            li.Selected = false;
    }

    protected void chkCheckAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCheckAll.Checked == true)
        {
            foreach (ButtonListItem item in chkVesselType.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem item in chkVesselType.Items)
            {
                item.Selected = false;
            }
        }
    }
}

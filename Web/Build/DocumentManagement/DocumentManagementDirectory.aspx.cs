using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementDirectory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblDirectoryId.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("New", "NEW");
        toolbarmain.AddButton("Save", "SAVE");
        toolbarmain.AddButton("Delete", "DELETE");

        MenuDirectory.AccessRights = this.ViewState;
        MenuDirectory.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["DIRECTORYID"] = null;

            BindDocumentCategoryTree();
            DocumentCategoryEdit();
        }
    }
    private void BindDocumentCategoryTree()
    {

        DataSet ds = new DataSet();
        ds = PhoenixDocumentManagementDirectory.DirectoryTreeList();

        tvwDirectory.DataTextField = "FLDDIRECTORYNAME";
        tvwDirectory.DataValueField = "FLDDIRECTORYID";
        tvwDirectory.ParentNodeField = "FLDPARENTGROUPID";
        tvwDirectory.XPathField = "XPATH";

        if (ds.Tables[0].Rows.Count > 0)
        {
            tvwDirectory.PopulateTree(ds);
        }
        else
        {
            tvwDirectory.RootText = "";
            tvwDirectory.PopulateTree(ds);
        }
    }
    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
        lblDirectoryId.Text = tvsne.SelectedNode.Text.ToString();

        if (lblSelectedNode.Text == "Root")
        {

            lblSelectedNode.Text = "";
            lblDirectoryId.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
            lblDirectoryId.Text = tvsne.SelectedNode.Text.ToString();
            DocumentCategoryEdit();
        }


    }
    protected void MenuDirectory_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidDirectory())
                {
                    ucError.Visible = true;
                    return;
                }
                if (lblDirectoryId.Text == "")
                {                    
                    PhoenixDocumentManagementDirectory.DirectoryInsert(
                                             General.GetNullableGuid(lblSelectedNode.Text),
                                             txtDirectoryName.Text.Trim(), 
                                             null,
                                             txtDirectoryNumber.Text
                                             );
                    Reset();
                }
                else
                {
                    
                    PhoenixDocumentManagementDirectory.DirectoryUpdate(
                                                new Guid(lblSelectedNode.Text),
                                                txtDirectoryName.Text.Trim(),
                                                null,
                                                txtDirectoryNumber.Text
                                                );
                }

            }
            if (dce.CommandName.ToUpper().Equals("DELETE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "Root cannot be deleted.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixDocumentManagementDirectory.DirectoryDelete(new Guid(lblSelectedNode.Text));
                    Reset();
                    lblSelectedNode.Text = "";
                }
            }
            BindDocumentCategoryTree();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidDirectory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtDirectoryName.Text.Trim()))  
            ucError.ErrorMessage = "Directory Name is required.";

        if (string.IsNullOrEmpty(txtDirectoryNumber.Text.Trim()))
            ucError.ErrorMessage = "Directory Number is required.";

        return (!ucError.IsError);
    }
    protected void DocumentCategoryEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            DataSet ds = PhoenixDocumentManagementDirectory.DirectoryEdit(new Guid(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["DIRECTORYID"] = ds.Tables[0].Rows[0]["FLDDIRECTORYID"].ToString();
                
                txtDirectoryName.Text = ds.Tables[0].Rows[0]["FLDDIRECTORYNAME"].ToString();
                lblDirectoryId.Text = ds.Tables[0].Rows[0]["FLDDIRECTORYID"].ToString();
                txtDirectoryNumber.Text = ds.Tables[0].Rows[0]["FLDDIRECTORYNUMBER"].ToString();
            }
        }
        else
        {
            Reset();
        }
    }
    private void Reset()
    {
        txtDirectoryName.Text = txtDirectoryNumber.Text = "";        
        lblDirectoryId.Text = "";
    }
}

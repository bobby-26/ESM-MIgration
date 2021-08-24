using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionVIRItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblItemId.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("New", "NEW");
        toolbarmain.AddButton("Save", "SAVE");
        toolbarmain.AddButton("Delete", "DELETE");

        MenuInspectionVIRItemMain.AccessRights = this.ViewState;
        MenuInspectionVIRItemMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ITEMID"] = null;

            BindInspectionVIRItemTree();
            InspectionVIRItemEdit();
        }
    }

    private void BindInspectionVIRItemTree()
    {

        DataSet ds = new DataSet();

        ds = PhoenixInspectionVIRItem.InspectionVIRItemTree();

        tvwInspectionVIRItem.DataTextField = "FLDITEMNAME";
        tvwInspectionVIRItem.DataValueField = "FLDITEMID";
        tvwInspectionVIRItem.ParentNodeField = "FLDPARENTITEMID";
        tvwInspectionVIRItem.XPathField = "XPATH";

        if (ds.Tables[0].Rows.Count > 0)
        {
            tvwInspectionVIRItem.PopulateTree(ds);
        }
        else
        {
            tvwInspectionVIRItem.RootText = "";
            tvwInspectionVIRItem.PopulateTree(ds);
        }
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
        lblItemId.Text = tvsne.SelectedNode.Text.ToString();

        if (lblSelectedNode.Text == "Root")
        {
            lblSelectedNode.Text = "";
            lblItemId.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
            lblItemId.Text = tvsne.SelectedNode.Text.ToString();
            InspectionVIRItemEdit();
        }
    }
    protected void MenuInspectionVIRItemMain_TabStripCommand(object sender, EventArgs e)
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
                if (!IsValidInspectionVIRItem())
                {
                    ucError.Visible = true;
                    return;
                }
                if (lblItemId.Text == "")
                {
                    //insert
                    PhoenixInspectionVIRItem.InsertInspectionVIRItem(
                                                         General.GetNullableGuid(lblSelectedNode.Text),
                                                         General.GetNullableString(txtItemName.Text),
                                                         chkActiveyn.Checked ? General.GetNullableInteger("1") : General.GetNullableInteger("0"),
                                                         General.GetNullableString(txtItemNumber.Text)
                                                         );
                    Reset();
                }
                else
                {

                    //update
                    PhoenixInspectionVIRItem.UpdateInspectionVIRItem(
                                                        new Guid(lblSelectedNode.Text),
                                                        General.GetNullableString(txtItemName.Text),
                                                        chkActiveyn.Checked ? 1 : 0,
                                                        General.GetNullableString(txtItemNumber.Text)
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
                    PhoenixInspectionVIRItem.DeleteInspectionVIRItem(new Guid(lblSelectedNode.Text));
                    Reset();
                    lblSelectedNode.Text = "";
                }
            }
            BindInspectionVIRItemTree();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidInspectionVIRItem()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtItemName.Text.Trim()))
            ucError.ErrorMessage = "Item name is required.";

        return (!ucError.IsError);
    }
    protected void InspectionVIRItemEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {            
            DataSet ds = PhoenixInspectionVIRItem.InspectionVIRItemEdit(new Guid(lblSelectedNode.Text));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ITEMID"] = ds.Tables[0].Rows[0]["FLDITEMID"].ToString();

                txtItemName.Text = ds.Tables[0].Rows[0]["FLDITEMNAME"].ToString();
                chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                lblItemId.Text = ds.Tables[0].Rows[0]["FLDITEMID"].ToString();
                txtItemNumber.Text = ds.Tables[0].Rows[0]["FLDITEMNUMBER"].ToString();
            }
        }
        else
        {
            txtItemName.Text = "";
            txtItemNumber.Text = "";
            chkActiveyn.Checked = true;
            lblItemId.Text = "";
        }
    }
    private void Reset()
    {
        txtItemName.Text = "";
        chkActiveyn.Checked = true;
        lblItemId.Text = "";
        txtItemNumber.Text = "";
    }
}

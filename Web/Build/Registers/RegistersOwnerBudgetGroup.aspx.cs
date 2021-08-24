using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersOwnerBudgetGroup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersOwnerBudgetGroup.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOwnerBudgetGroup')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuOwnerBGroupExport.AccessRights = this.ViewState;
        //MenuOwnerBGroupExport.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Copy", "COPY", ToolBarDirection.Right);
        toolbar1.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("Add", "ADD", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);

        MenuOwnerBGroup.AccessRights = this.ViewState;
        MenuOwnerBGroup.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            lblParentGroupId.Visible = false;
            lblOwnerBGroupId.Visible = false;
            lblSelectedNode.Visible = false;
            //MenuOwnerBGroupExport.SetTrigger(tvwOwnerBGroup);

            ViewState["PAGENUMBER"] = 1;
            lblParentGroupId.Text = "_Root";
            lblSelectedNode.Text = "_Root";
            BindData();
            
        }

    }

    protected void BindData()
    {
        string[] alColumns = { "FLDOWNERBUDGETGROUP" };
        string[] alCaptions = { "Owner Budget Group" };

        DataSet ds = new DataSet();

        ds = PhoenixRegistersBudget.ListTreeOwnerBudgetGroup(
            General.GetNullableInteger(ucOwner.SelectedAddress) == null ? 0 : int.Parse(ucOwner.SelectedAddress));

        General.SetPrintOptions("gvOwnerBGroup", "Owner Budget Group", alCaptions, alColumns, ds);

        tvwOwnerBGroup.DataTextField = "FLDOWNERBUDGETGROUP";
        tvwOwnerBGroup.DataValueField = "FLDOWNERBUDGETGROUPID";
        tvwOwnerBGroup.DataFieldParentID = "FLDPARENTGROUPID";
        //tvwOwnerBGroup.XPathField = "XPATH";

        DataSet ds1 = PhoenixRegistersAddress.EditAddress(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(ucOwner.SelectedAddress) == null ? 0 : long.Parse(ucOwner.SelectedAddress));

        if (ds1.Tables[0].Rows.Count > 0)
        {
            tvwOwnerBGroup.RootText = ds1.Tables[0].Rows[0]["FLDCODE"].ToString();
            tvwOwnerBGroup.PopulateTree(ds.Tables[0]);
            //TreeNode tn = new TreeNode(lblSelectedNode.Text);
            //TreeView tv = (TreeView)tvwOwnerBGroup.ThisTreeView;
            //tv.SelectedNode = tn;
        }
        else
        {
            tvwOwnerBGroup.RootText = "";
            tvwOwnerBGroup.PopulateTree(ds.Tables[0]);
        }
    }

    protected void ucOwner_Changed(object sender, EventArgs e)
    {
        BindData();
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs args)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)args;
        Session["SELECTEDBUDGETGROUPCODE"] = tvsne.Node;
        ViewState["ParentGroupId"] = tvsne.Node.Value.ToString();
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        lblOwnerBGroupId.Text = tvsne. Node.Value.ToString();
        lblParentGroupId.Text = tvsne.Node.Value.ToString();

        if (lblSelectedNode.Text == "_Root")
        {
            txtOwnerBGroupName.Text = "";
            lblOwnerBGroupId.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            EditBudgetGroup();
        }
        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
        if (e.Node.Value.ToLower() != "_root")
        {
            //lblOwner(e.Node.Value);
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "treeheight", "PaneResized();", true);
        }
        else
        {
            Reset();
        }
    }



    //private void ExpandTreeToNode()
    //{
    //    if (Session["SELECTEDBUDGETGROUPCODE"] == null)
    //        return;

    //    TreeNode tn = (TreeNode)Session["SELECTEDBUDGETGROUPCODE"];
    //    tvwOwnerBGroup.ThisTreeView.ExpandDepth = tn.Depth;
    //    BindData();
    //    tn = tvwOwnerBGroup.ThisTreeView.FindNode(tn.ValuePath);
    //    while (tn != null && tn.Depth > 1)
    //        tn = tn.Parent;
    //    tn.ExpandAll();
    //    tvwOwnerBGroup.ThisTreeView.FindNode(((TreeNode)Session["SELECTEDBUDGETGROUPCODE"]).ValuePath).Select();
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDOWNERBUDGETGROUP" };
        string[] alCaptions = { "Owner Budget Group" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersBudget.OwnerBudgetGroupSearch(
            General.GetNullableInteger(ucOwner.SelectedAddress)
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OwnerBudgetGroup.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Owner Budget Group</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void OwnerBGroupExport_TabStripCommand(object sender, EventArgs e)
       {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
         {
            ShowExcel();
         }
      }

    protected void OwnerBGroup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            //To add sub groups under selected parent

            if (CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidOwnerBudgetCode(txtOwnerBGroupName.Text,txtSortingOrder.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (lblSelectedNode.Text != null && lblSelectedNode.Text.ToString() != "")
                {
                    PhoenixRegistersBudget.InsertOwnerBudgetGroup(
                        int.Parse(ucOwner.SelectedAddress)
                        , txtOwnerBGroupName.Text
                        , General.GetNullableGuid(lblSelectedNode.Text)
                        , ""
                        , 0
                        , 1
                        , int.Parse(txtSortingOrder.Text));
                }
                BindData();
                Reset();
                //ExpandTreeToNode();
            } 
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                //To update the selected budget group

                if (lblSelectedNode.Text == "_Root")
                {
                    ucError.ErrorMessage = "You can not update the top most group";
                    ucError.Visible = true;
                }

                if (!IsValidOwnerBudgetCode(txtOwnerBGroupName.Text,txtSortingOrder.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (lblOwnerBGroupId.Text != null && lblOwnerBGroupId.Text != "")
                {
                    PhoenixRegistersBudget.UpdateOwnerBudgetGroup(
                        int.Parse(ucOwner.SelectedAddress),
                        txtOwnerBGroupName.Text,
                        new Guid(lblOwnerBGroupId.Text), "", 1,int.Parse(txtSortingOrder.Text));
                    BindData();
                    //ExpandTreeToNode();
                }

            }
            else if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (lblSelectedNode.Text == "_Root")
                {
                    ucError.ErrorMessage = "You can not delete the top most group";
                    ucError.Visible = true;
                }
                else
                {
                    try
                    {
                        PhoenixRegistersBudget.DeleteOwnerBudgetGroup(new Guid(lblSelectedNode.Text));
                        Session["SELECTEDBUDGETGROUPCODE"] = null;
                        lblParentGroupId.Text = "_Root";
                        lblSelectedNode.Text = "_Root";
                        BindData();
                        Reset();
                        //tvwOwnerBGroup.ThisTreeView.FindNode("Root").Select();
                        //ExpandTreeToNode();
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                    }
                }
            }
            else if (CommandName.ToUpper().Equals("COPY"))
            {
                if (General.GetNullableInteger(ucOwner.SelectedAddress) == null)
                {
                    ucError.ErrorMessage = "Please select the owner.";
                    ucError.Visible = true;
                }
                else
                {
                    try
                    {
                        PhoenixRegistersBudget.CopyOwnerBudgetGroupFromESM(int.Parse(ucOwner.SelectedAddress));
                        Session["SELECTEDBUDGETGROUPCODE"] = null;
                        lblParentGroupId.Text = "_Root";
                        lblSelectedNode.Text = "_Root";
                        BindData();
                        Reset();
                        //tvwOwnerBGroup.ThisTreeView.FindNode("Root").Select();
                        //ExpandTreeToNode();
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                    }
                }
            }   
            else if (CommandName.ToUpper().Equals("NEW"))
            {
               
                Reset();
             
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        //lblParentGroupId.Text = "";
        txtOwnerBGroupName.Text = "";
        lblOwnerBGroupId.Text = "";
        txtSortingOrder.Text = "";
        txtOwnerBGroupName.Focus();
        //ucOwner.SelectedAddress = "";
    }

    protected void EditBudgetGroup()
    {
        if((lblSelectedNode.Text != "") && (lblSelectedNode.Text != "_Root"))
         {
            DataSet ds = PhoenixRegistersBudget.EditOwnerBudgetGroup(new Guid(lblSelectedNode.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtOwnerBGroupName.Text = dr["FLDOWNERBUDGETGROUP"].ToString();
                lblParentGroupId.Text = dr["FLDPARENTGROUPID"].ToString();
                txtSortingOrder.Text = dr["FLDSORTINGORDER"].ToString();
            }
        }
    }

    private bool IsValidOwnerBudgetCode(string ownerbudgetgroup,string sortingorder)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucOwner.SelectedAddress) == null)
            ucError.ErrorMessage = "Owner is required.";

        if (ownerbudgetgroup.Trim().Equals(""))
            ucError.ErrorMessage = "Owner Budget Group is required.";

        if (sortingorder.Trim().Equals(""))
            ucError.ErrorMessage = "Sorting Order is required.";

  

        return (!ucError.IsError);
    }

    private void DeleteOwnerBudgetGroup(string ownerbudgetgroupid)
    {
        PhoenixRegistersBudget.DeleteOwnerBudgetGroup(new Guid(ownerbudgetgroupid));
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}

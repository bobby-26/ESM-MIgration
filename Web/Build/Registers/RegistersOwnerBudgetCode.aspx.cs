using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOwnerBudgetCode : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersOwnerBudgetCode.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvOwnerBudgetGroup')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuOwnerBudgetCodeExport.AccessRights = this.ViewState;
        //MenuOwnerBudgetCodeExport.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        

        MenuOwnerBudgetCodeMain.AccessRights = this.ViewState;
        MenuOwnerBudgetCodeMain.MenuList = toolbarmain.Show();       
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ISBUDGETCODE"] = null;

           

            BindOwnerBudgetCodeTree();
            BudgetCodeEdit();
        }
    }
    private void BindOwnerBudgetCodeTree()
    {        

        DataSet ds = new DataSet();
        ds = PhoenixRegistersBudget.ListTreeOwnerBudgetCodeGroup((General.GetNullableInteger(ucOwner.SelectedAddress))==null? 0: int.Parse(ucOwner.SelectedAddress) );

        tvwOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETGROUP";
        tvwOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETGROUPID";
        tvwOwnerBudgetCode.DataFieldParentID = "FLDPARENTGROUPID";
        //tvwOwnerBudgetCode.XPathField = "XPATH";
        DataSet ds1=new DataSet();       
        ds1  = PhoenixRegistersAddress.EditAddress(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,(General.GetNullableInteger(ucOwner.SelectedAddress))==null?0: long.Parse (ucOwner.SelectedAddress));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            tvwOwnerBudgetCode.RootText = ds1.Tables[0].Rows[0]["FLDCODE"].ToString();
            tvwOwnerBudgetCode.PopulateTree(ds.Tables[0]);
        }
        else
        {
            tvwOwnerBudgetCode.RootText = "";
            tvwOwnerBudgetCode.PopulateTree(ds.Tables[0]);
        }        
    }
    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        BudgetCodeEdit();        
    }
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
    protected void MenuOwnerBudgetCodeExport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void MenuOwnerBudgetCodeMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "You can not add budget code under root";
                    ucError.Visible = true;
                    return;
                }
               if(!IsValidOwnerBudgetCode())
                {
                    ucError.Visible = true;
                    return;
                }
               if (ViewState["ISBUDGETCODE"] != null && ViewState["ISBUDGETCODE"].ToString() == "1")
               {
                   //update
                   PhoenixRegistersBudget.UpdateOwnerBudgetCode(int.Parse(ucOwner.SelectedAddress), txtOwnerBudgetcode.Text.Trim(),
                    new Guid(lblSelectedNode.Text), txtbudgetdesc.Text.Trim(),(bool)chkActiveyn.Checked ? 1 : 0, int.Parse(txtSortingOrder.Text));

               }
               else
               {//insert
                   PhoenixRegistersBudget.InsertOwnerBudgetCode(int.Parse(ucOwner.SelectedAddress), txtOwnerBudgetcode.Text.Trim(),
                    General.GetNullableGuid(lblSelectedNode.Text ), txtbudgetdesc.Text.Trim(), 1,(bool)chkActiveyn.Checked ? General.GetNullableInteger("1") : General.GetNullableInteger("0")
                    ,int.Parse(txtSortingOrder.Text));

               }

            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "_ROOT")
                {
                    ucError.ErrorMessage = "You can not delete root";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ISBUDGETCODE"] != null && ViewState["ISBUDGETCODE"].ToString() == "1")
                {
                    PhoenixRegistersBudget.DeleteOwnerBudgetGroup(new Guid(lblSelectedNode.Text));
                }
                else
                {
                    ucError.ErrorMessage = "you can not delete  budget group";
                    ucError.Visible = true;
                    return;
                }
            }
            BindOwnerBudgetCodeTree();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
  }
    public bool IsValidOwnerBudgetCode()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(txtOwnerBudgetcode.Text.Trim()))
            ucError.ErrorMessage = "Owner budget code required.";
        if (string.IsNullOrEmpty(txtbudgetdesc.Text.Trim()))
            ucError.ErrorMessage = "Owner budget description required.";
        if (string.IsNullOrEmpty(txtSortingOrder.Text.Trim()))
            ucError.ErrorMessage = "Sorting Order is required.";
        return (!ucError.IsError);
    }
    protected void BudgetCodeEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "_ROOT")
        {
            if((lblSelectedNode.Text!="")&&(lblSelectedNode.Text!="_ROOT"))
            { 
            DataSet ds = PhoenixRegistersBudget.EditOwnerBudgetGroup(new Guid(lblSelectedNode.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["ISBUDGETCODE"] = ds.Tables[0].Rows[0]["FLDISBUDGETCODE"].ToString();
                    if (ViewState["ISBUDGETCODE"].ToString() == "1")
                    {
                        txtOwnerBudgetcode.Text = ds.Tables[0].Rows[0]["FLDOWNERBUDGETGROUP"].ToString();
                        txtbudgetdesc.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
                        chkActiveyn.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                        txtSortingOrder.Text = ds.Tables[0].Rows[0]["FLDSORTINGORDER"].ToString();

                    }
                }
                else
                {
                    Reset();
                }
            }
        }
        else
        {
            Reset();
            lblSelectedNode.Text = "_Root";
        }

    }
    private void Reset()
    {
        txtbudgetdesc.Text = "";
        txtOwnerBudgetcode.Text = "";
        chkActiveyn.Checked = false;
        txtSortingOrder.Text = "";
    }
    protected void ucOwner_Onchange(object sender,EventArgs e)
    {
        BindOwnerBudgetCodeTree();
    }
   
}

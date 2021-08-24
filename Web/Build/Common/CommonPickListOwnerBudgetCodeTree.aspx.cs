using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;

public partial class CommonPickListOwnerBudgetCodeTree : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Done", "DONE");        
        MenuBudget.MenuList = toolbarmain.Show();

        lblSelectedNode.Visible = false;

        if (!IsPostBack)
        {
            if (Request.QueryString["OWNERID"] != null)            
                ViewState["OWNERID"] = Request.QueryString["OWNERID"].ToString();
            
           
            BindOwnerBudgetCodeTree();
            BudgetCodeEdit();
        }
    }
    private void BindOwnerBudgetCodeTree()
    {
        DataSet ds = new DataSet();
        ds = PhoenixRegistersBudget.ListTreeOwnerBudgetCodeGroup(General.GetNullableInteger(ViewState["OWNERID"] == null ? "0" : ViewState["OWNERID"].ToString()), 1);

        tvwOwnerBudgetCode.DataTextField = "FLDOWNERBUDGETGROUP";
        tvwOwnerBudgetCode.DataValueField = "FLDOWNERBUDGETGROUPID";
        tvwOwnerBudgetCode.ParentNodeField = "FLDPARENTGROUPID";
        tvwOwnerBudgetCode.XPathField = "XPATH";
        DataSet ds1 = new DataSet();
        ds1 = PhoenixRegistersAddress.EditAddress(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, long.Parse(ViewState["OWNERID"] == null ? "0" : ViewState["OWNERID"].ToString()));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            tvwOwnerBudgetCode.RootText = ds1.Tables[0].Rows[0]["FLDCODE"].ToString();
            tvwOwnerBudgetCode.PopulateTree(ds);
        }
        else
        {
            tvwOwnerBudgetCode.RootText = "";
            tvwOwnerBudgetCode.PopulateTree(ds);
        }
    }
    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        TreeViewSelectNodeEvent tvsne = (TreeViewSelectNodeEvent)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.SelectedNode.Value.ToString();
        BudgetCodeEdit();
        
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {       

            nvc = new NameValueCollection();
            
            nvc.Add("txtOwnerBudgetCodeName", tvsne.SelectedNode.Text.ToString());
            nvc.Add("txtOwnerBudgetCodeId", tvsne.SelectedNode.Value.ToString());
            nvc.Add("txtParentgroupid", ViewState["PARENTGROUPID"].ToString());
        }
        else
        {

            nvc = Filter.CurrentPickListSelection;

            nvc.Set(nvc.GetKey(1), tvsne.SelectedNode.Text.ToString());
            nvc.Set(nvc.GetKey(2), tvsne.SelectedNode.Value.ToString());           
            nvc.Set(nvc.GetKey(3), ViewState["PARENTGROUPID"].ToString());

        }
        Filter.CurrentPickListSelection = nvc;       
    }      
    protected void BudgetCodeEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            DataSet ds = PhoenixRegistersBudget.EditOwnerBudgetGroup(new Guid(lblSelectedNode.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ISBUDGETCODE"] = ds.Tables[0].Rows[0]["FLDISBUDGETCODE"].ToString();
                ViewState["PARENTGROUPID"] = ds.Tables[0].Rows[0]["FLDPARENTGROUPID"].ToString();
            }            
        }
        else
        {           
            lblSelectedNode.Text = "Root";
        }

    }
    protected void MenuBudget_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
           
            if (dce.CommandName.ToUpper().Equals("DONE"))
            {
                if (ViewState["ISBUDGETCODE"] != null && ViewState["ISBUDGETCODE"].ToString() == "1")
                {
                    string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                        Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                    else
                        Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
                else
                {
                    ucError.ErrorMessage = "Please select Owner budget code to proceed.";
                    ucError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;


public partial class Registers_RegisterPOSHProjectCode : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        lblSelectedNode.Visible = false;
        lblProjectId.Visible = false;

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);        

        MenuDocumentCategoryMain.AccessRights = this.ViewState;
        MenuDocumentCategoryMain.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {

            ViewState["PROJECTID"] = null;

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

            if (Request.QueryString["projectid"] != null)
                lblSelectedNode.Text = Request.QueryString["projectid"].ToString();

            BindProjectCodeTree();
            ProjectCodeEdit();
            subaccount();
        }
    }

    private void subaccount()
    {

        DataSet ds = new DataSet();
        ds = PhoenixCommonRegisters.SubAccountPOSHList();

        ds.Tables[0].Columns.Add("Acoount", typeof(string), "FLDSUBACCOUNTCODE + ' - ' + FLDDESCRIPTION");

        chksubaccounts.DataTextField = "Acoount";
        chksubaccounts.DataValueField = "FLDBUDGETID";
        chksubaccounts.DataSource = ds;
        chksubaccounts.DataBind();
        // imgaccountcode.Attributes.Add("onclick", "return showPickList('spnPickListAccountCode', 'codehelp1', '', '../Common/CommonPickListAccount.aspx', true); ");
    }
    private void BindProjectCodeTree()
    {

        DataSet ds = new DataSet();

        ds = PhoenixRegisterPOSHProjectCode.ListTreeProjectCode();

        tvwDocumentCategory.DataTextField = "FLDPROJECTTITLE";
        tvwDocumentCategory.DataValueField = "FLDPROJECTID";
        tvwDocumentCategory.DataFieldParentID = "FLDPARENTID";
        tvwDocumentCategory.RootText = "ROOT";

        if (ds.Tables[0].Rows.Count > 0)
        {
            tvwDocumentCategory.PopulateTree(ds.Tables[0]);
            tvwDocumentCategory.SelectNodeByValue = lblSelectedNode.Text;
        }
        else
        {
            tvwDocumentCategory.RootText = "";
            tvwDocumentCategory.PopulateTree(ds.Tables[0]);
        }
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        lblProjectId.Text = tvsne.Node.Text.ToString();

        if (lblSelectedNode.Text == "_Root")
        {

            lblSelectedNode.Text = "";
            lblProjectId.Text = "";
            txtProjectTitle.Text = "";
            txtProjectCode.Text = "";
        }
        else
        {
            lblSelectedNode.Text = tvsne.Node.Value.ToString();
            lblProjectId.Text = tvsne.Node.Text.ToString();
            Session["PROJECTID"] = lblSelectedNode.Text;
            ProjectCodeEdit();
        }
    }

    protected void MenuDocumentCategoryMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (lblProjectId.Text == "")
                {
                    //insert
                    if (!IsValidDocumentCategory())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegisterPOSHProjectCode.InsertProjectCode(
                                                         General.GetNullableInteger(lblSelectedNode.Text),
                                                         txtseqno.Text.Trim(),
                                                         txtProjectTitle.Text,
                                                         General.GetNullableInteger(ddlstatus.SelectedValue),
                                                         General.ReadCheckBoxList(chksubaccounts)
                                                         );
                    Reset();
                    ucStatus.Text = "Project is added.";
                    BindProjectCodeTree();
                }
                else
                {
                    PhoenixRegisterPOSHProjectCode.UpdateProjectCode(
                                                         General.GetNullableInteger(lblSelectedNode.Text),
                                                         txtseqno.Text.Trim(),
                                                         txtProjectTitle.Text,
                                                         General.GetNullableInteger(ddlstatus.SelectedValue),
                                                         General.ReadCheckBoxList(chksubaccounts)
                                                         );
                    
                    ucStatus.Text = "Project updated.";
                    BindProjectCodeTree(); 
                }

            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                if (string.IsNullOrEmpty(lblSelectedNode.Text) || lblSelectedNode.Text.ToString().ToUpper() == "ROOT")
                {
                    ucError.ErrorMessage = "Root cannot be deleted.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixRegisterPOSHProjectCode.DeleteProjectCode(General.GetNullableInteger(lblSelectedNode.Text));
                    Reset();
                    lblSelectedNode.Text = "";
                    ucStatus.Text = "Project code deleted.";
                }
                BindProjectCodeTree();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidDocumentCategory()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtseqno.Text.Trim()))
            ucError.ErrorMessage = "Sequence number is required.";

        if (string.IsNullOrEmpty(txtProjectTitle.Text.Trim()))
            ucError.ErrorMessage = "Project title is required.";

        //if (General.GetNullableInteger(txtnoofcolumns.Text) == null)
        //    ucError.ErrorMessage = "No of column is required.";

        //if (General.GetNullableInteger(txtnoofstages.Text) == null)
        //    ucError.ErrorMessage = "No of Stage is required.";

        return (!ucError.IsError);
    }
    protected void ProjectCodeEdit()
    {
        if (!string.IsNullOrEmpty(lblSelectedNode.Text) && lblSelectedNode.Text.ToString().ToUpper() != "ROOT")
        {
            DataTable dt = PhoenixRegisterPOSHProjectCode.ProjectEdit(General.GetNullableInteger(lblSelectedNode.Text));
            if (dt.Rows.Count > 0)
            {
                ViewState["PROJECTID"] = dt.Rows[0]["FLDID"].ToString();
                txtProjectTitle.Text = dt.Rows[0]["FLDTITLE"].ToString();  
                lblProjectId.Text = dt.Rows[0]["FLDID"].ToString();
                txtseqno.Text = dt.Rows[0]["FLDPROJECTSEQNUMBER"].ToString();
                txtProjectCode.Text = dt.Rows[0]["FLDPROJECTCODE"].ToString();
                General.BindCheckBoxList(chksubaccounts, dt.Rows[0]["FLDSUBACCOUNTLIST"].ToString());
                ddlstatus.SelectedValue = dt.Rows[0]["FLDSTATUS"].ToString();                
            }

        }
        else
        {
            txtProjectTitle.Text = "";           
            lblProjectId.Text = "";            
        }
    }
    private void Reset()
    {
        txtProjectTitle.Text = "";       
        lblProjectId.Text = "";
        txtseqno.Text = "";
        txtProjectCode.Text = "";
        chksubaccounts.ClearSelection();
        ddlstatus.SelectedValue = "";
        Session["PROJECTID"] = null;
    }
}
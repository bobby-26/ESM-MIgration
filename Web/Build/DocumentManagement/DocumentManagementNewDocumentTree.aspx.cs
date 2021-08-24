using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementNewDocumentTree : PhoenixBasePage
{
    NameValueCollection nvc = new NameValueCollection();
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["DOCUMENTID"] = "";
            ViewState["selectednode"] = "";
            setMenu();
            BindDocument();            
        }
        BtnTreeSearchInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('HSEQAInfo','More Info','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementNewHSEQATreeSearchInfo.aspx" + "',false, 350, 220,'','',options); return false;");
    }
    private void setMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["viewonly"] != null)
            toolbar.AddButton("List", "LIST");
        toolbar.AddImageLink("javascript:parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementComments.aspx?REFERENCEID=" + ViewState["selectednode"].ToString() + "'); return false;", "Comments", "", "COMMENTS", ToolBarDirection.Right);
        MenuDiscussion.AccessRights = this.ViewState;
        MenuDiscussion.MenuList = toolbar.Show();
        MenuDiscussion.SelectedMenuIndex = 1;
    }

    protected void BindDocument()
    {
        try
        {
            DataSet ds = PhoenixDocumentManagementDocument.TreeDocumentList(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()));
            tvwComponent.DataTextField = "FLDDOCUMENTNAME";
            tvwComponent.DataValueField = "FLDDOCUMENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTDOCUMENTID";
            tvwComponent.DataFieldID = "FLDDOCUMENTID";

            tvwComponent.DataSource = ds.Tables[0];
            tvwComponent.DataBind();

            RadTreeNode rootNode = new RadTreeNode();
            rootNode.Text = "HSEQA Document"; ;
            rootNode.Value = "_Root";
            rootNode.Expanded = true;
            rootNode.AllowEdit = false;
            rootNode.Font.Bold = true;

            while (tvwComponent.Nodes.Count > 0)
            {
                ViewState["DOCUMENTID"].ToString();
                RadTreeNode node = tvwComponent.Nodes[0];
                rootNode.Nodes.Add(node);
            }

            tvwComponent.Nodes.Add(rootNode);
        }


        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void ShoudAutoExpand(RadTreeNode tn)
    {
        if ((tn.Level == 1) || (tn.Level == 0))
            tn.Expanded = true;
        else
            tn.Expanded = false;
    }




    //protected void tvwComponent_NodeDataBoundEvent(object sender, EventArgs args)
    //{
    //    try
    //    {
    //        RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
    //        DataRowView drv = (DataRowView)e.Node.DataItem;
    //        e.Node.Attributes["NAME"] = drv["FLDDOCUMENTNAME"].ToString();
    //        e.Node.Attributes["URL"] = drv["FLDURL"].ToString();
    //        e.Node.Attributes["TYPE"] = drv["FLDTYPE"].ToString();

    //        if (ViewState["DOCUMENTID"] != null && ViewState["DOCUMENTID"].ToString() == drv["FLDDOCUMENTID"].ToString())
    //            e.Node.Selected = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuDiscussion_TabStripCommand(object sender, EventArgs e)
    {
        Session["CustomTreeViewKey"] = this.Session.SessionID;
        string fileId = Session["CustomTreeViewKey"].ToString();

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../DocumentManagement/DocumentManagementCommentsList.aspx", false);
        }

        fileId = Session["CustomTreeViewKey"].ToString();

    }

    protected void tvwComponent_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            Session["CustomTreeViewKey"] = this.Session.SessionID;
            string fileId = Session["CustomTreeViewKey"].ToString();

            ViewState["ISTREENODECLICK"] = true;

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                string selectednode = e.Node.Value.ToString();
                ///string selectedvalue = e.Node.Text.ToString();
                
                //RadTreeNode parent = e.Node.ParentNode;

                DataSet DS = PhoenixDocumentManagementDocument.TreeDocumentEdit(General.GetNullableGuid(selectednode), PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    if (DS.Tables[0].Rows[0]["FLDTYPE"].ToString().Equals("4") || DS.Tables[0].Rows[0]["FLDTYPE"].ToString().Equals("5"))
                    {
                        string url = Session["sitepath"]+ DS.Tables[0].Rows[0]["FLDURL"].ToString().Replace("..","");
                        String script = String.Format("javascript:openNewWindow('spnPickListVendor', '"+DS.Tables[0].Rows[0]["FLDDOCUMENTNAME"].ToString() + "', '"+ url + "','false','950px','650px');");
                        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    }
                    else
                    {
                        if (DS.Tables[0].Rows[0]["FLDURL"].ToString() != "")
                        {
                            ifMoreInfo.Attributes["src"] = DS.Tables[0].Rows[0]["FLDURL"].ToString() + "&NEWDMSYN=1";
                        }
                    }
                }

                fileId = Session["CustomTreeViewKey"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }
    protected void cmdButton_Click(object sender, EventArgs e)
    {
        if (General.GetNullableString(treeViewSearch.Text) != null)
            ifMoreInfo.Attributes["src"] = "../DocumentManagement/DocumentManagementSearchResultsNew.aspx?keyword=" + treeViewSearch.Text;
    }
    protected void imgSearch_Click(object sender, EventArgs e)
    {
        if (General.GetNullableString(treeViewSearch.Text) != null)
            ifMoreInfo.Attributes["src"] = "../DocumentManagement/DocumentManagementSearchResultsNew.aspx?keyword=" + treeViewSearch.Text;
    }

}
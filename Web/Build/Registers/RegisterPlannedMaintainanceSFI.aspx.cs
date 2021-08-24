using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Data;

public partial class Registers_RegisterPlannedMaintainanceSFI : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar Menu = new PhoenixToolbar();
        Menu.AddButton("Delete", "DELETE", ToolBarDirection.Right);
        Menu.AddButton("Save", "SAVE", ToolBarDirection.Right);
        Menu.AddButton("New", "NEW", ToolBarDirection.Right);
        Menu_SFI.MenuList = Menu.Show();
        if (!Page.IsPostBack)
        {

            ViewState["HCode"] = Request.QueryString["Code"];
            ViewState["SFIId"] = Request.QueryString["SFIId"];
            ViewState["Type"] = Request.QueryString["Name"];
            ddsfitype.SelectedHard = ViewState["HCode"].ToString();
            BindDropDown();
            
        }
        BindTreeData();
    }
    protected void BindTreeData()
    {
        try
        {
            DataTable dt = PhoenixRegistersSFI.SFITreeList(General.GetNullableInteger(ViewState["HCode"].ToString()));
            

            tvwSFI.DataTextField = "TEXT";
            tvwSFI.DataValueField = "FLDSFIID";
            tvwSFI.DataFieldParentID = "FLDPARENTID";
            tvwSFI.DataFieldID = "FLDSFIID";

            tvwSFI.DataSource = dt;
            tvwSFI.DataBind();

            RadTreeNode rootNode = new RadTreeNode();
            rootNode.Text = ViewState["Type"].ToString();
            rootNode.Value = "_Root";
            rootNode.Expanded = true;
            rootNode.AllowEdit = false;
            rootNode.Font.Bold = true;
            while (tvwSFI.Nodes.Count > 0)
            {
                RadTreeNode node = tvwSFI.Nodes[0];
                rootNode.Nodes.Add(node);
            }
            tvwSFI.Nodes.Add(rootNode);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    public void BindDropDown()
    {
        DataTable dt = PhoenixRegistersSFI.SFICategoryList(General.GetNullableInteger(ViewState["HCode"].ToString()));
        radcbsficategory.DataSource = dt;
        radcbsficategory.DataValueField = "FLDCATEGORYID";
        radcbsficategory.DataTextField = "FLDCATEGORY";
        radcbsficategory.DataBind();

    }
    //public void BindFields()
    //{
    //    if (ViewState["SFIId"] != null)
    //    { 
    //    DataTable dt = PhoenixRegistersSFI.SFIDetails(General.GetNullableGuid(ViewState["SFIId"].ToString()));
    //    tbsfinumber.Text = dt.Rows[0]["FLDSFINUMBER"].ToString();
    //    tbradsfiname.Text = dt.Rows[0]["FLDSFINAME"].ToString();
    //    tbradsfiparentnumber.Text = dt.Rows[0]["FLDSFIPARENTNUMBER"].ToString();
    //    radcbsficategory.SelectedValue = dt.Rows[0]["FLDCATEGORYID"].ToString();
    //    }
    //}

    public void ClearFields()
    {

        tbsfinumber.Text = "";
        tbradsfiname.Text = "";
        tbradsfiparentnumber.Text = "";
        radcbsficategory.ClearSelection();
        ViewState["SFIId"] = "";

    }
    protected void Menu_SFI_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            Guid? SFIID = General.GetNullableGuid(ViewState["SFIId"].ToString());
        
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (SFIID == null)
                {
                    PhoenixRegistersSFI.SFIInsert(General.GetNullableString(tbsfinumber.Text)
                                                  , General.GetNullableString(tbradsfiname.Text)
                                                  , General.GetNullableString(tbradsfiparentnumber.Text)
                                                  , General.GetNullableInteger(ViewState["HCode"].ToString())
                                                  , General.GetNullableInteger(radcbsficategory.SelectedValue)
                                                  , ref SFIID
                                                  );
                    ViewState["SFIId"] = SFIID;
                    radnotificationstatus.Text = "SFI Succesfully Saved.";
                    BindTreeData();
                }
                else
                {
                    PhoenixRegistersSFI.SFIUpdate(General.GetNullableString(tbsfinumber.Text)
                                                  , General.GetNullableString(tbradsfiname.Text)
                                                  , General.GetNullableString(tbradsfiparentnumber.Text)
                                                  , General.GetNullableInteger(ViewState["HCode"].ToString())
                                                  , General.GetNullableInteger(radcbsficategory.SelectedValue)
                                                  , SFIID);
                    radnotificationstatus.Text = "SFI Succesfully Updated.";
                    BindTreeData();
                }


            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ClearFields();

            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersSFI.SFIDelete(SFIID);
                ClearFields();
                radnotificationstatus.Text = "SFI Succesfully Deleted.";
                BindTreeData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }



    protected void tvwSFI_NodeClickEvent(object sender, EventArgs args)
    {
        try
        {
            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            if (e.Node.Value.ToLower() != "_root")
            {
                tbsfinumber.Text = e.Node.Attributes["NUMBER"].ToString();
                tbradsfiname.Text = e.Node.Attributes["NAME"].ToString();
                tbradsfiparentnumber.Text = e.Node.Attributes["PARENTNUMBER"].ToString();
                radcbsficategory.SelectedValue = e.Node.Attributes["CATEGORY"].ToString();
                ViewState["SFIId"] = e.Node.Attributes["ID"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {

    }

    protected void Menusearch_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void tvwSFI_NodeDataBound(object sender, RadTreeNodeEventArgs args)
    {
        try
        {

            RadTreeNodeEventArgs e = (RadTreeNodeEventArgs)args;
            DataRowView drv = (DataRowView)e.Node.DataItem;
            e.Node.Attributes["NUMBER"] = drv["FLDSFINUMBER"].ToString();
            e.Node.Attributes["NAME"] = drv["FLDSFINAME"].ToString();
            e.Node.Attributes["PARENTNUMBER"] = drv["PARENTNUMBER"].ToString();
            e.Node.Attributes["CATEGORY"] = drv["CATEGORY"].ToString();
            e.Node.Attributes["ID"] = drv["FLDSFIID"].ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
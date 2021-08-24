using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Registers;

public partial class Options_OptionsHSEQAUserAccess : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Defaults", "USERACCESS");
        toolbar.AddButton("Zone", "USERZONE");
        toolbar.AddButton("Rank", "USERRANK");
        toolbar.AddButton("Vessel", "USERVESSEL");
        toolbar.AddButton("Vessel Type", "USERVESSELTYPE");
        toolbar.AddButton("Fleet", "USERFLEET");
        toolbar.AddButton("Pool", "USERPOOL");
        toolbar.AddButton("Alert", "USERALERT");
        toolbar.AddButton("Company", "USERCOMPANY");
        toolbar.AddButton("Department", "USERDEPARTMENT");
        toolbar.AddButton("Sub Department", "USERSUBDEPARTMENT");
        toolbar.AddButton("HSEQA", "HSEQAACCESS");

        MenuUserAccessList.MenuList = toolbar.Show();
        MenuUserAccessList.SelectedMenuIndex = 11;

        if (Request.QueryString["GroupId"] != null)
            ViewState["GroupId"] = Request.QueryString["GroupId"].ToString();

        if (Request.QueryString["accessid"] != null)
            ViewState["ACCESSID"] = Request.QueryString["accessid"].ToString();
        if (!IsPostBack)
        {
            //if (Request.QueryString["GroupId"] != null)
            //    ViewState["GroupId"] = Request.QueryString["GroupId"].ToString();

            BindCompany();
            BindCategory();
        }
        BindGroupName();
        BindData();

    }
    private void BindGroupName()
    {
        int GroupId = int.Parse(ViewState["GroupId"].ToString());
        DataSet ds = PhoenixDocumentManagementUserGroupConfiguration.UserGroupDataList(GroupId);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            txtGroupName.Text = dt.Rows[0]["FLDGROUPNAME"].ToString();
        }
    }
    protected void UserAccessList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("USERACCESS"))
        {
            if (ViewState["ACCESSID"] != null)
            {
                if (Request.QueryString["usercode"] != null)
                    Response.Redirect("OptionsUserAccess.aspx?usercode=" + Request.QueryString["usercode"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString());
                else
                    Response.Redirect("OptionsUserAccess.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString());
            }
        }
        else if (dce.CommandName.ToUpper().Equals("HSEQAACCESS"))
        {
            if (ViewState["ACCESSID"] != null)
            {
                Response.Redirect("OptionsHSEQAUserAccess.aspx?GroupId=" + Request.QueryString["GroupId"].ToString() + "&accessid=" + Request.QueryString["accessid"].ToString());
            }
        }
        else
        {
            if (ViewState["ACCESSID"] != null)
            {
                if (Request.QueryString["usercode"] != null)
                    Response.Redirect("OptionsUserAccessList.aspx?usercode=" + Request.QueryString["usercode"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString() + "&menuindex=" + dce.Item.ItemIndex + "&commandname=" + dce.CommandName);
                else
                    Response.Redirect("OptionsUserAccessList.aspx?accessid=" + ViewState["ACCESSID"].ToString() + "&GroupId=" + Request.QueryString["GroupId"].ToString() + "&menuindex=" + dce.Item.ItemIndex + "&commandname=" + dce.CommandName);
            }
        }
    }

    private void BindData()
    {
        Guid? Category = new Guid();
        Category = General.GetNullableGuid(ddlDMSCategoryList.SelectedValue);
        //int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        int? companyid = General.GetNullableInteger(ddlCompany.SelectedValue);
        if (companyid != null && Category!=null)
        {
            gvMenu.DataSource = PhoenixDocumentManagementUserGroupConfiguration.DMSCateoryTree(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            companyid
              , int.Parse(ViewState["GroupId"].ToString())
              , General.GetNullableGuid(ddlDMSCategoryList.SelectedValue)
               );
            gvMenu.DataBind();
        }
    }
    protected void gvMenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string CategoryId = ((Label)_gridview.Rows[nCurrentRow].FindControl("lblCategoryId")).Text;
        }
        BindData();
    }


    protected void gvMenu_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }


    protected void ddlDMSCategoryList_TextChanged(object sender, EventArgs e)
    {
        Guid? Category = new Guid();
        Category = General.GetNullableGuid(ddlDMSCategoryList.SelectedValue);

        if (Category != null)
        {
            BindData();

        }
    }



    protected void gvMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox cb = (CheckBox)e.Row.FindControl("chkMenuRights");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;

            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(cb, drv["FLDROOTID"].ToString());
            // Add this javascript to the onclick Attribute of the row
            //cb.Attributes["onclick"] = "return fnStopPropagation(event, '" + e.Row.FindControl("lnkCheck").ClientID + "');";
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int nCurrentRow = Int32.Parse(cb.Text);
        string menucode = ((Label)gvMenu.Rows[nCurrentRow].FindControl("lblCategoryId")).Text;
        //int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
        int? companyid = General.GetNullableInteger(ddlCompany.SelectedValue);
        if (companyid != null)
        {
            PhoenixDocumentManagementUserGroupConfiguration.DMSUserAccessInsert(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , companyid
                 , General.GetNullableGuid(menucode)
                , int.Parse(ViewState["GroupId"].ToString())
                );
        }
        BindData();

    }


    protected void BindCategory()
    {

        int? companyid = General.GetNullableInteger(ddlCompany.SelectedValue);
        
        if (companyid != null)
        {
            ddlDMSCategoryList.Items.Clear();
            ddlDMSCategoryList.DataSource = PhoenixDocumentManagementUserGroupConfiguration.DMSUserAccessCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, companyid);
            ddlDMSCategoryList.DataBind();
            ddlDMSCategoryList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            ddlDMSCategoryList.Visible = true;
        }
    }
    protected void BindCompany()
    {
        ddlCompany.Items.Clear();
        //ddlCompany.DataSource = PhoenixRegistersCompany.ListCompany();
        ddlCompany.DataSource = PhoenixDocumentManagementUserGroupConfiguration.GroupCompanyList(int.Parse(ViewState["GroupId"].ToString()));
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("--Select--", "Dummy"));
        ddlCompany.Visible = true;
        //ddlCompany.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
    }
    protected void ddlCompany_TextChanged(object sender, EventArgs e)
    {
        BindCategory();
        BindData();
    }

}

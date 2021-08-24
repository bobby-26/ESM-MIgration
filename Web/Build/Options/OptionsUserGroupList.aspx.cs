using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class Options_OptionsUserGroupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Identity", "USERIDENTITY", ToolBarDirection.Right);
        toolbar.AddButton("Permissions", "USERPERMISSIONS", ToolBarDirection.Right);
        toolbar.AddButton("User Group", "USERGROUP", ToolBarDirection.Right);
        toolbar.AddButton("User", "USER", ToolBarDirection.Right);


        MenuUserAdmin.MenuList = toolbar.Show();
        MenuUserAdmin.SelectedMenuIndex = 2;

        if (Request.QueryString["usercode"] != null)
            ViewState["USERCODE"] = Request.QueryString["usercode"].ToString();
        if (Request.QueryString["accessid"] != null)
            ViewState["ACCESSID"] = Request.QueryString["accessid"].ToString();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            dgUserGroups.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void Rebind()
    {
        dgUserGroups.SelectedIndexes.Clear();
        dgUserGroups.EditIndexes.Clear();
        dgUserGroups.DataSource = null;
        dgUserGroups.Rebind();
    }
    protected void UserAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("USER"))
        {
            Response.Redirect("OptionsUser.aspx?usercode=" + ViewState["USERCODE"].ToString());
        }
        if (CommandName.ToUpper().Equals("USERGROUP"))
        {
            if (ViewState["USERCODE"] != null)
                Response.Redirect("OptionsUserGroupList.aspx?usercode=" + ViewState["USERCODE"].ToString());
        }
        if (CommandName.ToUpper().Equals("USERPERMISSIONS"))
        {
            if (ViewState["USERCODE"] != null)
                Response.Redirect("OptionsUserAccess.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
        }
        if (CommandName.ToUpper().Equals("USERIDENTITY"))
        {
            if (ViewState["USERCODE"] != null)
                Response.Redirect("OptionsUserIdentity.aspx?usercode=" + ViewState["USERCODE"].ToString() + "&accessid=" + ViewState["ACCESSID"].ToString());
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = SessionUtil.User2UserGroupSearch(Int32.Parse(ViewState["USERCODE"].ToString()), sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            dgUserGroups.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        string[] alColumns = { "FLDGROUPCODE", "FLDGROUPNAME" };
        string[] alCaptions = { "Group Code", "Group name" };

        General.SetPrintOptions("dgUserGroups", "User Groups Assigned", alCaptions, alColumns, ds);
        dgUserGroups.DataSource = ds;
        dgUserGroups.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
 protected void dgUserGroups_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    //protected void CheckBoxClicked(object sender, EventArgs e)
    //{

    //    foreach (GridDataItem row in dgUserGroups.MasterTableView.Items)
    //    {
    //      // RadLabel groupcode = ((RadLabel)row.FindControl("lblGroupcode"));
    //        string groupcode = ((RadLabel)row.FindControl("lblGroupcode")).Text;
    //        //string chkGroupRights = ((RadCheckBox)row.FindControl("lblGroupcode")).Text;
    //        //if (chkGroupRights.Checked == true )
    //        {
    //            SessionUtil.MapUser2UserGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(ViewState["USERCODE"].ToString()), int.Parse(groupcode.ToString()));
    //            Rebind();
    //        } 
    //    }


    //}
   

// protected void CheckBoxClicked(object sender, EventArgs e)
// {
//     CheckBox cb = (CheckBox)sender;
//     int nCurrentRow = Int32.Parse(cb.Text);
//     string groupcode = ((Label)dgUserGroups.Rows[nCurrentRow].FindControl("lblGroupcode")).Text;
//     SessionUtil.MapUser2UserGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(ViewState["USERCODE"].ToString()), int.Parse(groupcode));
//     BindData();
//     SetPageNavigator();
// }


protected void dgUserGroups_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
            return;
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //int GroupRights = ((RadCheckBox)e.Item.FindControl("chkGroupRights")).Checked == true ? 1 : 0;
                string groupcode = ((RadLabel)e.Item.FindControl("lblGroupcode")).Text;
                SessionUtil.MapUser2UserGroup(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Int32.Parse(ViewState["USERCODE"].ToString()), int.Parse(groupcode));
                Rebind();
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void dgUserGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUserGroups.CurrentPageIndex + 1;
        BindData();
    }

    protected void dgUserGroups_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkGroupRights");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (cb != null)
                cb.Checked = drv["FLDRIGHTS"].ToString().Equals("1") ? true : false;
        }
    }

   
}

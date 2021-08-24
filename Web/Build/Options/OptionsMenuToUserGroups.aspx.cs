using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class OptionsMenuToUserGroups : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string menucode = Request.QueryString["menucode"].ToString();
            DataSet ds = SessionUtil.Menu2UserGroupMap(Int32.Parse(menucode));
            DataTable dt = SessionUtil.MenuPath(Int32.Parse(menucode));

            if (dt.Rows.Count > 0)
            {
                lblMenuPath.Text = dt.Rows[0]["FLDPATH"].ToString();    
            }

            if (ds.Tables.Count > 0)
            {
                dgUserGroups.DataSource = ds.Tables[0];
                dgUserGroups.DataBind();
            }
            else
            {
                Response.Write("User is not assigned to a group.");
            }
        }
    }

  
    protected void dgUserGroups_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
       
        RadLabel lblGroupCode = (RadLabel)e.Item.FindControl("lblGroupCode");
        string menucode = Request.QueryString["menucode"].ToString();
        if (e.CommandName.ToUpper().Equals("ASSIGNRIGHTS"))
        {

            SessionUtil.MenuAccessAssign(int.Parse(lblGroupCode.Text), Int32.Parse(menucode));
        }

        DataSet ds = SessionUtil.Menu2UserGroupMap(Int32.Parse(menucode));
        //dgUserGroups.DataSource = ds;

        if (ds.Tables.Count > 0)
        {
            dgUserGroups.DataSource = ds.Tables[0];
            dgUserGroups.DataBind();
        }
    }

    protected void dgUserGroups_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image ib = (Image)e.Item.FindControl("imgGroupAssigned");
            RadLabel lb = (RadLabel)e.Item.FindControl("lblGroupAssignedBlank");

            if (ib != null && lb != null && drv["FLDASSIGNED"].ToString().Equals("1"))
            {
                ib.Visible = true;
                lb.Visible = false;
            }
            else
            {
                lb.Visible = true;
                ib.Visible = false;
            }
        }
    }

    protected void dgUserGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dgUserGroups.CurrentPageIndex + 1;
        string menucode = Request.QueryString["menucode"].ToString();
        DataSet ds = SessionUtil.Menu2UserGroupMap(Int32.Parse(menucode));
        dgUserGroups.DataSource = ds;
        if (ds.Tables.Count > 0)
        {
            dgUserGroups.DataSource = ds.Tables[0];
           
        }

    }
}

using System;
using System.Data;
using SouthNests.Phoenix.Framework;

public partial class OptionsMoreInfoGroupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void dgUserGroups_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void BindData()
    {
        string usercode = Request.QueryString["usercode"].ToString();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = SessionUtil.User2UserGroupMap(Int32.Parse(usercode), null, 0, 1, 100, ref iRowCount, ref iTotalPageCount);
        DataSet ds1 = new DataSet();
        ds1.Merge(ds.Tables[0].Select("FLDRIGHTS=1"));
        if (ds1.Tables.Count > 0)
        {
            dgUserGroups.DataSource = ds1.Tables[0];
            dgUserGroups.VirtualItemCount = iRowCount;
        }
        else
        {
            Response.Write("User is not assigned to a group.");
        }
    }
}

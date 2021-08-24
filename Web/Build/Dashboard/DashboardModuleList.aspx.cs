using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;


public partial class DashboardModuleList : PhoenixBasePage
{
    int usercode;
    protected void Page_Load(object sender, EventArgs e)
    {

        usercode = (Request.QueryString["usercode"] != null) ? int.Parse(Request.QueryString["usercode"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        try
        {
            DataSet ds = PhoenixDashboardOption.dashboardModuleConfigureList(usercode);
            gvModule.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvModule.SelectedIndexes.Clear();
        gvModule.EditIndexes.Clear();
        gvModule.DataSource = null;
        gvModule.Rebind();
    }
  

    protected void gvModule_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
        
                CheckBox cb = (CheckBox)e.Item.FindControl("chkSelectedYN");
                RadLabel moduleId = (RadLabel)e.Item.FindControl("lblModuleId");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (cb != null)
                    cb.Checked = drv["FLDVERIFIEDYN"].ToString().Equals("0") ? false : true;
        
                string jvscript = "";
             
                if (cb != null) jvscript = "javascript:ModuleUpdate(" + usercode + ",'" + moduleId.Text + "',this);";
             
                if (cb != null) cb.Attributes.Add("onclick", jvscript);
        
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvModule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvModule.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvModule_ItemCommand(object sender, GridCommandEventArgs e)
    {
      
    }
}




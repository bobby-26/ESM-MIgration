using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Text;
using SouthNests.Phoenix.Dashboard;
using Telerik.Web.UI;

public partial class DashboardMeasureList : PhoenixBasePage
{
    int usercode;
   
    protected void Page_Load(object sender, EventArgs e)
    {

        usercode = (Request.QueryString["usercode"] != null) ? int.Parse(Request.QueryString["usercode"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
            BindModuleList();
           
        }
     }

    private void BindData()
    {
		try
		{
			DataSet ds = PhoenixDashboardOption.UserMeasureListConfigure(General.GetNullableString(ddlModulelist.SelectedValue), usercode);
            gvMeasures.DataSource = ds;           
        }
		catch(Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
    }

    protected void Rebind()
    {
        gvMeasures.SelectedIndexes.Clear();
        gvMeasures.EditIndexes.Clear();
        gvMeasures.DataSource = null;
        gvMeasures.Rebind();
    }
    private void BindModuleList()
    {
        DataSet ds = PhoenixDashboardOption.dashboardUserModuleList(usercode);
        ddlModulelist.Items.Clear();
        ddlModulelist.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlModulelist.DataSource = ds.Tables[0];
        ddlModulelist.DataBind();
    }
 
    protected void ddlModulelist_DataBound(object sender, EventArgs e)
    {
        //ddlModulelist.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }
  
    protected void gvMeasures_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
          //  if (e.CommandName.ToUpper().Equals("UPDATE"))
          //  {
          //      RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkSelectedYN");
          //      RadLabel measurecode = (RadLabel)e.Item.FindControl("lblMeasureCode");
          //      //DataRowView drv = (DataRowView)e.Item.DataItem;
          //      //cb.Checked = drv["FLDVERIFIEDYN"].ToString().Equals("0") ? false : true;
          //
          //      string jvscript = "";
          //
          //      if (cb != null) jvscript = "javascript:MeasureUpdate(" + usercode + ",'" + measurecode.Text + "',this);";
          //
          //      if (cb != null) cb.Attributes.Add("onclick", jvscript);
          //
          //      Rebind();
          //  }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMeasures_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                CheckBox cb = (CheckBox)e.Item.FindControl("chkSelectedYN");
                RadLabel measurecode = (RadLabel)e.Item.FindControl("lblMeasureCode");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (cb != null)
                    cb.Checked = drv["FLDVERIFIEDYN"].ToString().Equals("1") ? true : false;

                string jvscript = "";
           
                if (cb != null) jvscript = "javascript:MeasureUpdate(" + usercode + ",'" + measurecode.Text + "',this);";
           
                if (cb != null) cb.Attributes.Add("onclick", jvscript);
           
               // BindData();
           
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvMeasures_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMeasures.CurrentPageIndex + 1;
        BindData();
    }

    protected void ddlModulelist_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {        
        gvMeasures.Rebind();
    }
}




using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;


public partial class InspectionCheckitemChildComponentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["COMPONENTID"] = "";

                if (Request.QueryString["COMPONENTID"] != null && Request.QueryString["COMPONENTID"].ToString() != string.Empty)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                                
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();
       
        ds = PhoenixInspectionRegisterCheckItems.ListCheckItemsChildComponents(new Guid(ViewState["COMPONENTID"].ToString()));        

        gvComponent.DataSource = ds;
    }
    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
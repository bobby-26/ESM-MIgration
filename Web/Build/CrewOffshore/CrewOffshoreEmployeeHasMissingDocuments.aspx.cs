using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;

public partial class CrewOffshoreEmployeeHasMissingDocuments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
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
        DataSet ds = PhoenixCrewOffshoreDocument.DocumentHasMissingSearch(int.Parse(Request.QueryString["empid"].ToString()), General.GetNullableInteger(Request.QueryString["sid"].ToString()));

        gvHasDocuments.DataSource = ds.Tables[0];

        gvMissingDocuments.DataSource = ds.Tables[1];

    
    }

  
    protected void gvHasDocuments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvMissingDocuments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

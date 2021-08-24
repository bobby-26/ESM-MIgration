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
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Text;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewRegisteredBankAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            PhoenixToolbar toolbarAddress = new PhoenixToolbar();
            toolbarAddress.AddButton("Bank", "BANK");
            toolbarAddress.AddButton("Address", "ADDRESS");
            PhoenixToolbar toolbarMain = new PhoenixToolbar();
           
        }
        BindData();
    }

    private void BindData()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt = PhoenixCrewBankAccount.EditCrewDefaultBankAccount(General.GetNullableInteger(Filter.CurrentCrewSelection));               

        ds.Tables.Add(dt.Copy());
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvBankInformation.DataSource = ds;
     
        }

    }
    protected void gvBankInformation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
           
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



}

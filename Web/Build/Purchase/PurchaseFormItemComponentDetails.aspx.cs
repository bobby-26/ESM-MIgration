using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;  
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Inventory;

public partial class PurchaseFormItemComponentDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (!IsPostBack)
            {
                if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
                {
                    BindComponetDetails(Request.QueryString["COMPONENTID"].ToString(), Request.QueryString["VESSELID"].ToString());
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindComponetDetails(string componentid ,string vesselid)
    {
        DataSet ds = new DataSet();

        ds = PhoenixInventoryComponent.ListComponent(new Guid(componentid), Convert.ToInt32(Request.QueryString["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtComponentDetails.Text = ds.Tables[0].Rows[0]["FLDMISCELLANEOUS1"].ToString();
        }

    }


}

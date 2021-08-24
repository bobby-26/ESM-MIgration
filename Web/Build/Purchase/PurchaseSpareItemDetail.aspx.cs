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
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Common;

public partial class PurchaseSpareItemDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
            {
                DataSet ds=new DataSet(); 
                if (Request.QueryString["STOCKTYPE"].ToString().ToUpper().Equals("STORE"))
                {
                    ds = PhoenixInventoryStoreItem.ListStoreItem(new Guid(Request.QueryString["SPAREITEMID"].ToString()), Int32.Parse(Request.QueryString["VESSELID"].ToString()));
                }
                else
                {
                   ds = PhoenixInventorySpareItem.ListSpareItem(new Guid(Request.QueryString["SPAREITEMID"].ToString()), Int32.Parse(Request.QueryString["VESSELID"].ToString()));
                }
                if (ds.Tables[0].Rows.Count  > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtItemDetails.Content = dr["FLDDETAIL"].ToString();
                    ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
                    //Title1.Text += "    (Item Details - " + dr["FLDNUMBER"].ToString() + " )";
                }
                //else
                //    Title1.Text += " Item Details ";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void btnInsertPic_Click(object sender, EventArgs e)
    { }

   
}

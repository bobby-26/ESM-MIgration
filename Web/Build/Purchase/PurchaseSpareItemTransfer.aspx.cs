using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class PurchaseSpareItemTransfer : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            MenuFormDetail.Title =  PhoenixPurchaseOrderForm.FormNumber;
            if (Request.QueryString["launchedfrom"] == null)
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuFormDetail.AccessRights = this.ViewState;
                MenuFormDetail.MenuList = toolbarmain.Show();
            }
            if (!IsPostBack)
            {
                
                if ((Request.QueryString["orderlineid"] != null) && (Request.QueryString["orderlineid"] != ""))
                {
                    ViewState["orderlineid"] = Request.QueryString["orderlineid"].ToString();
                    BindData(Request.QueryString["orderlineid"].ToString());
                   
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string orderidline)
    {
        DataSet ds = PhoenixPurchaseOrderLine.OrderLineSpareItemDetail(new Guid(orderidline), Filter.CurrentPurchaseVesselSelection);
        if(ds.Tables[1].Rows.Count > 0)
        {
            ddlvessel.DataSource = ds.Tables[1];
            ddlvessel.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlvessel.DataBind();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
            txtPartNumber.Text = dr["FLDNUMBER"].ToString();
            txtPartName.Text = dr["FLDSPARENAME"].ToString();
            txtMaker.Text = dr["FLDMAKERNAME"].ToString();
            txtPreferedVendor.Text = dr["FLDPREFERDVENDOR"].ToString();
            txtQuantity.Text = dr["FLDQUANTITY"].ToString();
            lblvesselid.Text = dr["FLDVESSELID"].ToString();
            if(General.GetNullableInteger(dr["FLDFROMVESSEL"].ToString()) != null)
            {
                ddlvessel.SelectedValue = dr["FLDFROMVESSEL"].ToString();
            }
        }
       
    }

    protected void MenuFormDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["orderlineid"] != null) && (Request.QueryString["orderlineid"] != ""))
                {
                    PhoenixPurchaseOrderLine.SpareTransferInsert(
                        General.GetNullableGuid(Request.QueryString["orderlineid"])
                        , General.GetNullableInteger(ddlvessel.SelectedValue)
                        , General.GetNullableInteger(lblvesselid.Text)
                        , General.GetNullableString(txtPartNumber.Text)
                        , General.GetNullableDecimal(txtQuantity.Text));

                    //string Script = "";
                    //Script += "<script language=JavaScript id='codehelp1'>" + "\n";
                    //Script += "fnReloadList();";
                    //Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "codehelp1", Script);
                    ucStatus.Text = "Transfer Success.";
                    ucStatus.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}

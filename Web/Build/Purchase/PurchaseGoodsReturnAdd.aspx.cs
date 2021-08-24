using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;


public partial class PurchaseGoodsReturnAdd: PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
     
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddFontAwesomeButton("../Purchase/PurchaseGoodsReturnAdd.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbarmain.AddFontAwesomeButton("../Purchase/PurchaseGoodsReturnAdd.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR"); 
                   
            MenuOrder.AccessRights = this.ViewState;
            MenuOrder.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void MenuOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                UcVessel.SelectedVessel = "";
                ddlStockType.SelectedValue = "";
                txtVendorNumber.Text = "";
                txtVenderName.Text = "";
                txtVendorId.Text = "";
                txtTitle.Text = "";
                txtFormNo.Text = "";
                BindData();
                gvOrder.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
           
            int? vessel = General.GetNullableInteger(UcVessel.SelectedVessel);
            
            DataSet ds = PhoenixCommonOrder.OrderSearch(vessel
                , General.GetNullableInteger(txtVendorId.Text)
                , General.GetNullableString(ddlStockType.SelectedValue)
                , txtTitle.Text.Trim() 
                , txtFormNo.Text.Trim() 
                , sortexpression
                , sortdirection
                , gvOrder.CurrentPageIndex + 1
                , gvOrder.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            gvOrder.DataSource = ds;
            gvOrder.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void Rebind()
    {
        gvOrder.SelectedIndexes.Clear();
        gvOrder.EditIndexes.Clear();
        gvOrder.DataSource = null;
        gvOrder.Rebind();
    }

    protected void gvOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOrder.CurrentPageIndex + 1;
        BindData();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;
            Guid? id = General.GetNullableGuid(((RadLabel)item.FindControl("lblOrderId")).Text);
            int? Vesselid = General.GetNullableInteger(((RadLabel)item.FindControl("lblvesselId")).Text); 


            LinkButton edit = ((LinkButton)item.FindControl("cmdCreate"));
            if (edit != null)
            {
                edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','OrderItem','Purchase/PurchaseGoodsReturnLineAdd.aspx?ORDERID=" + id + "&VESSELID="+ Vesselid + "');return false");

            }

            string StockType = General.GetNullableString(((RadLabel)item.FindControl("lblStockType")).Text);

            if (StockType == "SERVICE" )
            {
                edit.Visible = false;
            }
            else
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }


        }




    }


  

}








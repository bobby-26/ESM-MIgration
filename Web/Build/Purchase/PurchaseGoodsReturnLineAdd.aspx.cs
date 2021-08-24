using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseGoodsReturnLineAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bind();

            if (!IsPostBack)
            {
                DataSet ds2 = PhoenixCommonOrder.GRNEdit(General.GetNullableGuid(ViewState["GRNID"].ToString()));
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds2.Tables[0].Rows[0];
                    txtvessel.Text = dr["FLDVESSELNAME"].ToString();
                    txtvedor.Text = dr["Address"].ToString();
                    txtstocktype.Text = dr["FLDSTOCKTYPE"].ToString();
                    ViewState["STOCKTYPE"] = dr["FLDSTOCKTYPE"].ToString();
                    txtOrder.Text = dr["FLDTITLE"].ToString();
                    txtformno.Text = dr["FLDFORMNO"].ToString();
                    txtComment.Text = dr["FLDCOMMENT"].ToString();
                    txtGRNNumber.Text = dr["FLDREFERENCENUMBER"].ToString();
                    txtInvoiceNo.Text = dr["FLDINVOICENUMBER"].ToString();
                    txtSupplierReference.Text = dr["FLDINVOICESUPPLIERREFERENCE"].ToString();
                    ViewState["VENDORID"] = dr["FLDVENDORID"].ToString();
                    ViewState["CURRENCYID"] = dr["FLDCURRENCYID"].ToString();
                    ViewState["INVOICECODE"] = dr["FLDINVOICECODE"].ToString();
                    ViewState["CREDITDEBITNOTEID"] = dr["FLDCREDITDEBITNOTEID"].ToString();
                    txtCNRegisterNo.Text = dr["FLDCNREGISTERNO"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                gvItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();          
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuItem.AccessRights = this.ViewState;
            MenuItem.MenuList = toolbarmain.Show();
           
            PhoenixToolbar tool = new PhoenixToolbar();
            if (General.GetNullableGuid(ViewState["INVOICECODE"].ToString()) != null)
                tool.AddButton("Credit Note", "CREDITNOTE", ToolBarDirection.Right);
            tool.AddButton("Return", "RETURN", ToolBarDirection.Right);
            MenuReturn.MenuList = tool.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void bind()
    {
        try
        {
            Guid? id = null;
            if (Request.QueryString["ORDERID"] != null && Request.QueryString["VESSELID"] != null)
            {
                Guid? OrderId = General.GetNullableGuid((Request.QueryString["ORDERID"]).ToString());
                int? VesselId = General.GetNullableInteger((Request.QueryString["VESSELID"]).ToString());

                DataSet ds = PhoenixCommonOrder.POItem(PhoenixSecurityContext.CurrentSecurityContext.UserCode, OrderId, VesselId, ref id);             
                ViewState["GRNID"] = id.ToString();
                
            }

            if (ViewState["GRNID"] == null && Request.QueryString["GRNID"] != null)
            {
                ViewState["GRNID"] = Request.QueryString["GRNID"].ToString();
            }

            PhoenixCommonOrder.GRNLineAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["GRNID"].ToString()));

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void MenuItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                Guid? id = General.GetNullableGuid(ViewState["GRNID"].ToString());
                string Comment = General.GetNullableString(txtComment.Text);

                if (!IsValidGRNUpdate(Comment))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonOrder.GRNUpdate(rowusercode, id, Comment);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                  "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
                   
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvItem.SelectedIndexes.Clear();
        gvItem.EditIndexes.Clear();
        gvItem.DataSource = null;
        gvItem.Rebind();
    }

    protected void gvItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = PhoenixCommonOrder.GRNLineList
                (
                General.GetNullableGuid(ViewState["GRNID"].ToString()),
                gvItem.CurrentPageIndex + 1,
                gvItem.PageSize,
                ref iRowCount,
                ref iTotalPageCount

                );
            gvItem.DataSource = ds;
            gvItem.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidateGRNLine((((UserControlDecimal)e.Item.FindControl("txtReturnQty")).Text),
                    (((UserControlDecimal)e.Item.FindControl("txtAcceptedQty")).Text),
                    //(((UserControlDecimal)e.Item.FindControl("txtDamagedQty")).Text),
                    (((UserControlDecimal)e.Item.FindControl("txtReturnPrice")).Text),
                    (((UserControlQuick)e.Item.FindControl("ucReason")).SelectedQuick)
                    ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixCommonOrder.GRNLineUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblGRNId")).Text),
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblGRNLineId")).Text),
                    General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtReturnQty")).Text),
                    General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtAcceptedQty")).Text),
                    //General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtDamagedQty")).Text),
                    General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtReturnPrice")).Text),
                    General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucReason")).SelectedQuick)

                    );


                PhoenixCommonOrder.GRNAmount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                    General.GetNullableGuid(ViewState["GRNID"].ToString()));

                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }



    private bool IsValidGRNUpdate(string Comment)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (Comment == null)
        {
            ucError.ErrorMessage = "Comment Is Required.";
        }

        return (!ucError.IsError);
    }


    private bool IsValidateGRNLine(string ReturnQty, string AcceptedQty, string ReturnPrice, string Reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableDecimal(ReturnQty) == null)
            ucError.ErrorMessage = "ReturnQty is required.";

        if (General.GetNullableDecimal(AcceptedQty) == null)
            ucError.ErrorMessage = "AcceptedQty is required.";
     
        if (General.GetNullableDecimal(ReturnPrice) == null)
            ucError.ErrorMessage = "ReturnPrice is required.";

        if (General.GetNullableInteger(Reason) == null)
            ucError.ErrorMessage = "Reason is required.";

        return (!ucError.IsError);
    }




    protected void gvItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControlQuick Reason = (UserControlQuick)e.Item.FindControl("ucReason");
            if (Reason != null)
            {
                Reason.QuickList = PhoenixRegistersQuick.ListQuick(1, 181);
                Reason.SelectedQuick = drv["FLDQUICKCODE"].ToString();
            }
        }
    }


    protected void MenuReturn_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        
            if (CommandName.ToUpper().Equals("RETURN"))
            {

                if (ViewState["STOCKTYPE"].ToString() == "SPARE" || ViewState["STOCKTYPE"].ToString() == "SERVICE")
                {
                    Guid? id = General.GetNullableGuid(ViewState["GRNID"].ToString());

                    PurchaseGoodsReturn.SpareItemReturnPO(PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                }
                if (ViewState["STOCKTYPE"].ToString() == "STORE")
                {

                    Guid? id = General.GetNullableGuid(ViewState["GRNID"].ToString());

                    PurchaseGoodsReturn.StoreItemReturnPO(PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                }
            }

            if (CommandName.ToUpper().Equals("CREDITNOTE"))
            {
                Guid? id = General.GetNullableGuid(ViewState["GRNID"].ToString());
                int? Vendor = General.GetNullableInteger(ViewState["VENDORID"].ToString());
                int? Currency = General.GetNullableInteger(ViewState["CURRENCYID"].ToString());

                Guid? CreditId = General.GetNullableGuid(ViewState["CREDITDEBITNOTEID"].ToString());

                Response.Redirect("PurchaseGoodsReturnCreditNoteList.aspx?VENDORID=" + Vendor + "&CURRENCYID=" + Currency + "&GRNID=" + id + "&CREDITDEBITNOTEID=" + CreditId + " ");
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

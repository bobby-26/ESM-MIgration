using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;

using SouthNests.Phoenix.Framework;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseSupplierPortPOReasonlist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["orderid"] != null)
            {
                ViewState["ORDERID"] = Request.QueryString["orderid"].ToString();
            }


        }
    }
    protected void Rebind()
    {
        gvSupplierlist.SelectedIndexes.Clear();
        gvSupplierlist.EditIndexes.Clear();
        gvSupplierlist.DataSource = null;
        gvSupplierlist.Rebind();
    }
    private void BindData()
    {

        DataSet ds = PhoenixPurchasePOReasons.POReasonSupplierList(Guid.Parse(ViewState["ORDERID"].ToString()));

        gvSupplierlist.DataSource = ds;
      //  gvSupplierlist.DataBind();


    }
    protected void gvSupplierlist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvSupplierlist_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancle = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancle != null) cancle.Visible = SessionUtil.CanAccess(this.ViewState, cancle.CommandName);

            
        }

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlQuick ddl = (UserControlQuick)e.Item.FindControl("ddlQuicktype");
            if (ddl != null)
            {
                ddl.SelectedQuick = drv["FLDREASON"].ToString();
            }

        }

        if (e.Item.IsInEditMode && e.Item is GridDataItem)
        {
            UserControlQuick ddl = (UserControlQuick)e.Item.FindControl("ddlQuicktype");
            RadTextBox reason = (RadTextBox)e.Item.FindControl("txtreasontext");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ddl != null)
            {
               
                if (drv["FLDSHORTNAME"].ToString() == "OTH")
                {
                    reason.Visible = true;
                }
            }
        }
    }
    protected void ddlQuicktype_OnTextchanged(object sender, EventArgs e)
    {
        try
        {
            UserControlQuick ucc = (UserControlQuick)sender;
            GridDataItem item = (GridDataItem)ucc.Parent.Parent;

            int? ddl = General.GetNullableInteger(((UserControlQuick)item.FindControl("ddlQuicktype")).SelectedQuick);
            RadTextBox reason = (((RadTextBox)item.FindControl("txtreasontext")));
            Guid id = (Guid.Parse(((RadLabel)item.FindControl("lblreasonidedit")).Text));
            int? OTH = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(191, "OTH"));

            if (ddl != null)
            {
                if (ddl == OTH)
                {

                    //if (reason.Text == "")
                    //{
                    //    ucError.ErrorMessage = "Reason Message is required";
                    //    ucError.Visible = true;
                    //}
                    reason.Visible = true;
                }
                else
                {
                    reason.Text = "";
                    reason.Visible = false;

                }
            }

            PhoenixPurchasePOReasons.POReasonSupplierReasonupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                              id, ddl, General.GetNullableString(reason.Text));
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    private void POReasonSupplierReasonupdate(Guid reasonid, int? ddlquick, string reasontext)
    {
        PhoenixPurchasePOReasons.POReasonSupplierReasonupdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                reasonid,ddlquick, reasontext);
    }

    protected void gvSupplierlist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                int? ddl = General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ddlQuicktype")).SelectedQuick);
                RadTextBox reason = (((RadTextBox)e.Item.FindControl("txtreasontext")));
                int? OTH = General.GetNullableInteger(PhoenixCommonRegisters.GetQuickCode(191, "OTH"));
                if (ddl != null)
                {
                    if (ddl == OTH)
                    {
                       if (reason.Text == "")
                        {
                            ucError.ErrorMessage = "Reason Message is required";
                            e.Canceled = true;
                            ucError.Visible = true;
                            return;

                        }
                    }
                }

             POReasonSupplierReasonupdate((Guid.Parse(((RadLabel)e.Item.FindControl("lblreasonidedit")).Text)),
                   (General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ddlQuicktype")).SelectedQuick)),
                   (General.GetNullableString(((RadTextBox)e.Item.FindControl("txtreasontext")).Text))
                    );
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
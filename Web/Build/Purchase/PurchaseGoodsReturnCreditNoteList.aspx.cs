using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;


public partial class PurchaseGoodsReturnCreditNoteList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuGoodsReturnCreditNote.AccessRights = this.ViewState;
            MenuGoodsReturnCreditNote.MenuList = toolbar.Show();

            if (!IsPostBack)
            {              
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvGoodsReturnCreditNote.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }     
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGoodsReturnCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvGoodsReturnCreditNote.Items)
                {
                    if (((RadCheckBox)(gvr.FindControl("ChkCN"))).Checked == true)
                    {
                        Guid? CreditDebitNote = General.GetNullableGuid(((RadLabel)gvr.FindControl("lblCreditDebitNote")).Text);
                        Guid? GrnId = General.GetNullableGuid(Request.QueryString["GRNID"].ToString());

                        PurchaseGoodsReturn.GrnCnUpdate(GrnId, CreditDebitNote);
                      
                    }
                }
            }


            if (CommandName.ToUpper().Equals("BACK"))
            {
                Guid? GrnId = General.GetNullableGuid(Request.QueryString["GRNID"].ToString());
                Response.Redirect("PurchaseGoodsReturnLineAdd.aspx?GRNID="+ GrnId + "");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvGoodsReturnCreditNote_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGoodsReturnCreditNote.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PurchaseGoodsReturn.GoodsReturnCreditNoteSearch(
                General.GetNullableInteger(Request.QueryString["VENDORID"].ToString()),
                 General.GetNullableInteger(Request.QueryString["CURRENCYID"].ToString()),
                gvGoodsReturnCreditNote.CurrentPageIndex + 1,
                gvGoodsReturnCreditNote.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvGoodsReturnCreditNote.DataSource = ds;
            gvGoodsReturnCreditNote.VirtualItemCount = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGoodsReturnCreditNote_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {

                if (Request.QueryString["CREDITDEBITNOTEID"] != null)
                {
                    ViewState["CreditNoteId"] = (Request.QueryString["CREDITDEBITNOTEID"]).ToString();

                    GridDataItem item = e.Item as GridDataItem;

                    RadCheckBox cb = (RadCheckBox)e.Item.FindControl("ChkCN");
                    cb.Enabled = SessionUtil.CanAccess(this.ViewState, "ACTIVE");
                    cb.Checked = DataBinder.Eval(e.Item.DataItem, "FLDCREDITDEBITNOTEID").ToString().Equals(ViewState["CreditNoteId"]) ? true : false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
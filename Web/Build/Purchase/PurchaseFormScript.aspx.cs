using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseFormScript : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");

                //PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Search", "SEARCH");
                //MenuOrderFormMain.AccessRights = this.ViewState;
                //MenuOrderFormMain.MenuList = toolbarmain.Show();

                //MenuOrderFormMain.SelectedMenuIndex = 0;
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
        DataSet ds = PhoenixPurchaseOrderForm.ListOrderFormForDataCorrection(
                General.GetNullableString(txtFormNo.Text));

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvFormDetails.DataSource = ds;
        //    gvFormDetails.DataBind();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvFormDetails);
        //}

        rgPurchaseForm.DataSource = ds;
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FORM"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseFormGeneral.aspx";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void txtFormNo_Changed(object sender, EventArgs e)
    {
        rgPurchaseForm.Rebind();
    }

    //protected void gvFormDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
    //           && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
    //        {
    //            DataRowView drv = (DataRowView)e.Row.DataItem;

    //            Label lbtn = (Label)e.Row.FindControl("lblStockItemCode");
    //            UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("uclblStockItemCodeTT");
    //            if (lbtn != null)
    //            {
    //                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
    //                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
    //            }
    //        }
    //    }
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    //protected void gvFormDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;

    //    if (e.CommandName.ToUpper().Equals("SORT"))
    //        return;

    //    if (e.CommandName.ToUpper().Equals("REQSTATUS"))
    //    {
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        Label lnkFormNumberName = (Label)_gridView.Rows[nCurrentRow].FindControl("lnkFormNumberName");

    //        if (lnkFormNumberName != null)
    //        {
    //            PhoenixPurchaseOrderForm.UpdateOrderFormStatus(
    //            General.GetNullableString(lnkFormNumberName.Text));

    //            BindData();
    //        }
    //    }
    //    if (e.CommandName.ToUpper().Equals("REMOVEDUPLICATE"))
    //    {
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        Label lblVesselId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId");
    //        Label lblOrderId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderId");

    //        if (lblOrderId != null)
    //        {
    //            PhoenixPurchaseOrderForm.DeleteFormExtnDuplicate(
    //                General.GetNullableGuid(lblOrderId.Text),
    //                General.GetNullableInteger(lblVesselId.Text));

    //            BindData();
    //        }
    //    }
    //    if (e.CommandName.ToUpper().Equals("SPLITPOOVERWRITE"))
    //    {
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        Label lblOrderId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderId");

    //        if (lblOrderId != null)
    //        {
    //            PhoenixPurchaseOrderForm.UpdateFormLineOverwrite(
    //                General.GetNullableGuid(lblOrderId.Text));

    //            BindData();
    //        }
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void Rebind()
    {
        rgPurchaseForm.SelectedIndexes.Clear();
        rgPurchaseForm.EditIndexes.Clear();
        rgPurchaseForm.DataSource = null;
        rgPurchaseForm.Rebind();
    }
    protected void rgPurchaseForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void rgPurchaseForm_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit)
               && !e.Item.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                //DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblStockItemCode");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblStockItemCodeTT");
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
            }
        }
    }

    protected void rgPurchaseForm_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("REQSTATUS"))
        {
            RadLabel lnkFormNumberName = (RadLabel)e.Item.FindControl("lnkFormNumberName");

            if (lnkFormNumberName != null)
            {
                PhoenixPurchaseOrderForm.UpdateOrderFormStatus(
                General.GetNullableString(lnkFormNumberName.Text));

                rgPurchaseForm.Rebind();
            }
        }
        if (e.CommandName.ToUpper().Equals("REMOVEDUPLICATE"))
        {
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");

            if (lblOrderId != null)
            {
                PhoenixPurchaseOrderForm.DeleteFormExtnDuplicate(
                    General.GetNullableGuid(lblOrderId.Text),
                    General.GetNullableInteger(lblVesselId.Text));

                rgPurchaseForm.Rebind();
            }
        }
        if (e.CommandName.ToUpper().Equals("SPLITPOOVERWRITE"))
        {
            RadLabel lblOrderId = (RadLabel)e.Item.FindControl("lblOrderId");

            if (lblOrderId != null)
            {
                PhoenixPurchaseOrderForm.UpdateFormLineOverwrite(
                    General.GetNullableGuid(lblOrderId.Text));

                rgPurchaseForm.Rebind();
            }
        }
    }
}

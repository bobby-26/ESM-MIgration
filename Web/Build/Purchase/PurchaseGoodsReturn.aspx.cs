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

public partial class Purchase_PurchaseGoodsReturn : PhoenixBasePage
{
    //Guid OrderId = new Guid("62B71400-E430-E311-8EDA-0023AEB26001");
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Purchase/PurchaseGoodsReturn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvGoodsReturn')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseGoodsReturn.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/PurchaseGoodsReturn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");                 
            toolbar.AddFontAwesomeButton("javascript:parent.openNewWindow('Add','','Purchase/PurchaseGoodsReturnAdd.aspx')", "Add", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            
            MenuGoodsReturn.AccessRights = this.ViewState;
            MenuGoodsReturn.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvGoodsReturn.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            int? vesselid = General.GetNullableInteger(UcVessel.SelectedVessel);

            DataSet ds = PurchaseGoodsReturn.GoodsReturnSearch(txtReferenceNumber.Text.Trim(), General.GetNullableDateTime(ucFromDate.Text),
                General.GetNullableDateTime(ucToDate.Text), General.GetNullableString(ddlStockType1.SelectedValue), General.GetNullableInteger(txtVendorId.Text),
                General.GetNullableInteger(ucCurrency.SelectedCurrency), txtformno.Text.Trim(), txtordertitle.Text.Trim(), vesselid,
                sortexpression, sortdirection, gvGoodsReturn.CurrentPageIndex + 1,
                gvGoodsReturn.PageSize, ref iRowCount, ref iTotalPageCount);

            string[] alColumns = { "FLDVESSELNAME", "FLDCODE", "FLDNAME", "FLDTITLE", "FLDFORMNO", "FLDREFERENCENUMBER", "FLDGRNDATE", "FLDSTOCKTYPE", "FLDCURRENCYCODE", "FLDAMOUNT" };
            string[] alCaptions = { "Vessel", "Code", "Name", "Order", "FormNo", "Reference Number", "Grndate", "Stock Type",  "Currency", "Amount" };

            General.SetPrintOptions("gvGoodsReturn", "Goods Return Note", alCaptions, alColumns, ds);

            gvGoodsReturn.DataSource = ds;
            gvGoodsReturn.VirtualItemCount = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGoodsReturn_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvGoodsReturn.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCODE", "FLDNAME", "FLDTITLE", "FLDFORMNO", "FLDREFERENCENUMBER", "FLDGRNDATE", "FLDSTOCKTYPE", "FLDCURRENCYCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Vessel", "Code", "Name", "Order", "FormNo", "Reference Number", "Grndate", "Stock Type", "Currency", "Amount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? vesselid = General.GetNullableInteger(UcVessel.SelectedVessel);

        DataSet ds = PurchaseGoodsReturn.GoodsReturnSearch(txtReferenceNumber.Text.Trim(), General.GetNullableDateTime(ucFromDate.Text),
            General.GetNullableDateTime(ucToDate.Text), General.GetNullableString(ddlStockType1.SelectedValue), General.GetNullableInteger(txtVendorId.Text),
            General.GetNullableInteger(ucCurrency.SelectedCurrency), txtformno.Text.Trim(), txtordertitle.Text.Trim(), vesselid,
            sortexpression, sortdirection, gvGoodsReturn.CurrentPageIndex + 1,
            gvGoodsReturn.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=GoodsReturnNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Goods Return Note</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuGoodsReturn_TabStripCommand(object sender, EventArgs e)
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

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {           
                ClearFilter();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void ClearFilter()
    {
        txtReferenceNumber.Text = "";
        UcVessel.SelectedVessel = "";
        ucFromDate.Text = "";
        ucToDate.Text = "";
        ddlStockType1.SelectedValue = "";
        txtVendorNumber.Text = "";
        txtVenderName.Text = "";
        txtVendorId.Text = "";
        ucCurrency.SelectedCurrency = "";
        txtformno.Text = "";
        txtordertitle.Text = "";
        Rebind();
    }


    protected void Rebind()
    {
        gvGoodsReturn.SelectedIndexes.Clear();
        gvGoodsReturn.EditIndexes.Clear();
        gvGoodsReturn.DataSource = null;
        gvGoodsReturn.Rebind();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvGoodsReturn_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblGrnId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PurchaseGoodsReturn.DeleteGoodsReturn(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["ID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvGoodsReturn_ItemDataBound(object sender, GridItemEventArgs e)
    { 
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? id = General.GetNullableGuid(item.GetDataKeyValue("FLDGRNID").ToString());

                Guid? OrderId = General.GetNullableGuid(((RadLabel)item.FindControl("lblOrder")).Text);
                int? Vesselid = General.GetNullableInteger(((RadLabel)item.FindControl("lblVesselId")).Text);
              
                LinkButton edit = ((LinkButton)item.FindControl("cmdEdit"));
                if (edit != null)
                {                    
                    edit.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','OrderItem','Purchase/PurchaseGoodsReturnLineAdd.aspx?GRNID=" + id + "');return false");
                }

                LinkButton lb = ((LinkButton)item.FindControl("lbReferenceNumber"));

                if (lb != null)
                {
                    lb.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','','Purchase/PurchaseGoodsReturnLineAdd.aspx?GRNID=" + id + " ');return false");
                }

                LinkButton Form = ((LinkButton)item.FindControl("lblFormNo"));

                if (Form != null)
                {                
                    Form.Attributes.Add("onclick", "openNewWindow('Filter', '', 'Purchase/PurchaseFormDetails.aspx?orderid=" + OrderId + "&VesselId=" + Vesselid + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvGoodsReturn.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}







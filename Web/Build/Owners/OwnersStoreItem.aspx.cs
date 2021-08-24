using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class OwnersStoreItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersStoreItem.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dvStoreItemControl')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersStoreItem.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Owners/OwnersStoreItem.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
        MenuStockItemControl.AccessRights = this.ViewState;
        MenuStockItemControl.MenuList = toolbargrid.Show();
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            chkROB.Checked = true;
            dvStoreItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        dvStoreItemControl.SelectedIndexes.Clear();
        dvStoreItemControl.EditIndexes.Clear();
        dvStoreItemControl.DataSource = null;
        dvStoreItemControl.Rebind();
    }
    protected void MenuStockItemControl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtNumber.Text = "";
                txtName.Text = "";
                ddlStockClass.SelectedHard = "";
                chkROB.Checked = true;
                txtMekerRef.Text = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Unit", "In Stock", "Actual" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixOwnersInventory.StoreItemForOwnerSearch(
                null,
                General.GetNullableString(txtNumber.Text),
                General.GetNullableString(txtName.Text),
                General.GetNullableInteger(ucVessel.SelectedVessel),
                General.GetNullableInteger(ddlStockClass.SelectedHard)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], dvStoreItemControl.PageSize
                , ref iRowCount, ref iTotalPageCount,
                General.GetNullableInteger(chkROB.Checked == true ? "1" : "0")
                , txtMekerRef.Text);

            General.SetPrintOptions("dvStoreItemControl", "Store Item", alCaptions, alColumns, ds);
            dvStoreItemControl.DataSource = ds;
            dvStoreItemControl.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Unit", "In Stock", "Actual" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixOwnersInventory.StoreItemForOwnerSearch(
                null,
                General.GetNullableString(txtNumber.Text),
                General.GetNullableString(txtName.Text),
                General.GetNullableInteger(ucVessel.SelectedVessel),
                General.GetNullableInteger(ddlStockClass.SelectedHard)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], iRowCount
                , ref iRowCount, ref iTotalPageCount,
                General.GetNullableInteger(chkROB.Checked == true ? "1" : "0")
                , txtMekerRef.Text);

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItem.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Store Item</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void dvStoreItemControl_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {

        }
    }
    protected void dvStoreItemControl_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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
    protected void dvStoreItemControl_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : dvStoreItemControl.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

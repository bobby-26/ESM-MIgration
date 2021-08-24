using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemComponent : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvComponent.Items)
        {
            if (r.ItemType == GridItemType.Item)
            {
                Page.ClientScript.RegisterForEventValidation(gvComponent.UniqueID, "Edit$" + r.RowIndex.ToString());                
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);            
            if (!IsPostBack)
            {
                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["SPAREITEMID"].ToString() != null && Request.QueryString["SPAREITEMID"].ToString() != "")
                    ViewState["SPAREITEMID"] = Request.QueryString["SPAREITEMID"].ToString();
                else
                    ViewState["SPAREITEMID"] = null;
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemComponent.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"], "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('location','','" + Session["sitepath"] + "/Common/CommonPickListMultipleComponents.aspx?spareitemid=" + ViewState["SPAREITEMID"] + "&ifr=1');return true;", "Component", "<i class=\"fas fa-boxes\"></i>", "ADDCOMPONENT");
            MenuGridStockItemComponent.AccessRights = this.ViewState;
            MenuGridStockItemComponent.MenuList = toolbargrid.Show();
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

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Component Name", "In Use", "Drawing Number", "Position", "Alternative Number" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixInventorySpareItemComponent.SpareItemComponentSearch(Request.QueryString["SPAREITEMID"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , sortexpression, sortdirection
                            , gvComponent.CurrentPageIndex + 1
                            , gvComponent.PageSize
                            , ref iRowCount, ref iTotalPageCount);



            General.SetPrintOptions("gvComponent", "Component", alCaptions, alColumns, ds);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void StockItemComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void ShowExcel()
    {

        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Component Name", "In Use", "Drawing Number", "Position", "Alternative Number" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInventorySpareItemComponent.SpareItemComponentSearch(Request.QueryString["SPAREITEMID"].ToString(), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , sortexpression, sortdirection
                            , gvComponent.CurrentPageIndex + 1
                            , gvComponent.PageSize
                            , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StockItemComponent.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory StockItem Component</h3></td>");
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

    private void UpdateStockItemComponent(string stockitemcomponentid, string quantityincomponent, string drawingnumber, string position, string oldpartnumber)
    {
        try
        {

            if (!IsValidComponentEdit(quantityincomponent))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInventorySpareItemComponent.UpdateSpareItemComponent
            (
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
                  , new Guid(Request.QueryString["SPAREITEMID"]), General.GetNullableDecimal(quantityincomponent), drawingnumber, position, oldpartnumber
            );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteStockItemComponent(string stockitemcomponentid)
    {

        try
        {
            PhoenixInventorySpareItemComponent.DeleteSpareItemComponent
            (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
            );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidComponentEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvComponent;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvComponent.Rebind();         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            DeleteStockItemComponent(((Label)item.FindControl("lblStockItemComponentId")).Text);
            BindData();
        }
    }

    protected void gvComponent_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        GridDataItem item = (GridDataItem)e.Item;

        UpdateStockItemComponent
             (
                 ((Label)item.FindControl("lblStockItemComponentId")).Text,
                 ((UserControlNumber)item.FindControl("txtQuantityUseInComponentEdit")).Text,
                 ((TextBox)item.FindControl("txtDrawingNumberEdit")).Text,
                 ((TextBox)item.FindControl("txtItemPositionEdit")).Text,
                 ((TextBox)item.FindControl("txtItemOldNumberEdit")).Text

            );
        ucStatus.Text = "Component information updated.";

        BindData();
    }

    protected void gvComponent_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}

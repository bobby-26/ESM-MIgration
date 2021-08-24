using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class InventoryStoreItemControl : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvStoreItemControl.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation(gvStoreItemControl.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemControl.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dvStoreItemControl')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemControl.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemControl.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemControl.aspx", "Export to Sub Location", "<i class=\"fas fa-file-excel\"></i>", "SUBLOCATION EXCEL");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemControl.aspx", "Export To Quantity Update", "<i class=\"fas fa-file-excel\"></i>", "UPDATE EXCEL");
        MenuStockItemControl.AccessRights = this.ViewState;   
        MenuStockItemControl.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            gvStoreItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
 
            ViewState["LOCATIONID"] = null;

            txtLocationName.Text = "--ALL Location--";
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            divLocationTree.Visible = true;
            divLocationList.Visible = false;

            BindCount();
            BindTreeData();
            BindLocationListData();
        }
    }

    protected void BindCount()
    {

        PhoenixInventoryStoreItemControl.StoreItemCountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                              PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                              null);
    }

    protected void BindTreeData()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInventoryStoreItemControl.StoreLocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        tvwLocation.DataTextField = "FLDLOCATIONNAME";
        tvwLocation.DataValueField = "FLDLOCATIONID";
        tvwLocation.DataFieldParentID = "FLDPARENTLOCATIONID";
        tvwLocation.RootText = "Locations";
        tvwLocation.PopulateTree(ds.Tables[0]);
    }

    protected void tvwLocation_NodeClickEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs args = (RadTreeNodeEventArgs)e;
        if (args.Node.Value.ToLower() != "_root")
        {
            locationcode(args.Node.Value, args.Node.Text);
            BindData();
            gvStoreItemControl.Rebind();
        }
        else
        {
            Reset();
        }
    }
    protected void locationcode(string locationid, string locationname)
    {
        ViewState["LOCATIONID"] = locationid;
        txtLocationName.Text = locationname;
        ViewState["LOCATIONNAME"] = locationname;
    }

    private void Reset()
    {
        txtLocationName.Text = "";
    }

    protected void MenuStockItemControl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                gvStoreItemControl.Rebind();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
             if (CommandName.ToUpper().Equals("SUBLOCATION EXCEL"))
            {
                ShowSubLocationExcel();
            }

            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtLocationName.Text = "";
                txtNumber.Text = "";
                txtName.Text = "";
                ddlStockClass.SelectedHard = "";
                chkROB.Checked = false;
                BindData();
                gvStoreItemControl.Rebind();
            }
             if (CommandName.ToUpper().Equals("UPDATE EXCEL"))
             {
                 UpdateExcelDownload();
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

            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Unit", "In Stock", "Actual" };
        
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string locationid =(ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());


            DataSet ds = PhoenixInventoryStoreItemControl.StoreItemLocationControlSearch(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text), 
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,General.GetNullableInteger(ddlStockClass.SelectedHard)   
                , sortexpression, sortdirection
                , gvStoreItemControl.CurrentPageIndex + 1, gvStoreItemControl.PageSize
                , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"));


            General.SetPrintOptions("dvStoreItemControl", "Store Item Control", alCaptions, alColumns, ds);

            gvStoreItemControl.DataSource = ds;
            gvStoreItemControl.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Unit" ,"In Stock" , "Actual" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvStoreItemControl.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            DataSet ds = PhoenixInventoryStoreItemControl.StoreItemLocationControlSearch(General.GetNullableInteger(locationid), null, null,
                           PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ddlStockClass.SelectedHard)  
                           , sortexpression, sortdirection
                           , 1, iRowCount
                           , ref iRowCount, ref iTotalPageCount,General.GetNullableInteger(chkROB.Checked == true? "1":"0"));

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemControl.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Store Item Control</h3></td>");
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

    protected void ShowSubLocationExcel()
    {
        try
        {

            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Unit", "In Stock", "Actual" };

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            DataSet ds = PhoenixInventoryStoreItemControl.StoreItemLocationExcel(General.GetNullableInteger(locationid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemControl.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Store Item Control</h3></td>");
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

    private void BindLocationListData()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInventoryStoreItemControl.StoreLocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                repLocationList.DataSource = ds.Tables[0];
                repLocationList.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItemControl_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvStoreItemControl_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
            LinkButton cmdMove = (LinkButton)e.Item.FindControl("cmdMove");
            if (cmdMove != null)
            {
                cmdMove.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, cmdMove.CommandName)) cmdMove.Visible = false;

                RadLabel lblStoreItemId = (RadLabel)e.Item.FindControl("lblStoreItemId");
                RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                cmdMove.Visible = SessionUtil.CanAccess(this.ViewState, cmdMove.CommandName);

                cmdMove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inventory/InventoryStoreItemMove.aspx?Storeitemid=" + lblStoreItemId.Text + "&locationid=" + lblLocationId.Text + "')");
            }
        }
    }

    protected void gvStoreItemControl_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            UpdateStoreItemControl
                (
                    ((RadLabel)eeditedItem.FindControl("lblStoreItemLocationId")).Text,
                    ((RadLabel)eeditedItem.FindControl("lblStoreItemId")).Text,
                    ((RadLabel)eeditedItem.FindControl("lblInStockQuantity")).Text,
                    ((UserControlDecimal)eeditedItem.FindControl("txtActualQuantityEdit")).Text
               );
            gvStoreItemControl.SelectedIndexes.Clear();
            BindData();
            gvStoreItemControl.Rebind();
            ucStatus.Text = "Actual Quantity updated.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkLocationName(object sender, CommandEventArgs e)
    {
        try
        {
            string[] locationvalue = e.CommandArgument.ToString().Split(',');

            foreach (RepeaterItem item in repLocationList.Items)
            {
                LinkButton previous = (LinkButton)item.FindControl("lnkLocationName");
                previous.ForeColor = System.Drawing.Color.Blue;
                previous.BackColor = System.Drawing.Color.White;

            }

            LinkButton current = (LinkButton)sender;
            current.ForeColor = System.Drawing.Color.Crimson;
            current.BackColor = System.Drawing.Color.FromArgb(0xFFE88C);
            
            ViewState["LOCATIONID"] = locationvalue[0];
            txtLocationName.Text = locationvalue[1];

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            BindData();
            gvStoreItemControl.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateStoreItemControl(string Storeitemlocationid, string Storeitemid, string stockquantity, string actualquantity)
    {
        try
        {
            if (!IsValidStoreItemControlEdit(actualquantity))
            {
                ucError.Visible = true;
                return;
            }
            if (stockquantity == "")
                stockquantity = "0";
            if (actualquantity == "")
                actualquantity = "0";

            int iMessageCode = 0;
            string iMessageText = "";
            string DisPositionHeaderId = "";
            Guid iDisPositionHeaderId = new Guid();

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            decimal transactionqty = 0;
            int transactiontype;

            /* TRANSACTION TYPE : LOST =9   FOUND =8 */

            if (Convert.ToDecimal(actualquantity) > Convert.ToDecimal(stockquantity))
            {
                transactiontype = 8;
                transactionqty = Convert.ToDecimal(actualquantity) - Convert.ToDecimal(stockquantity);

                PhoenixInventoryStoreItemDisposition.InsertStoreItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       transactiontype, Convert.ToDateTime(System.DateTime.UtcNow),
                      null, null,
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", ref iDisPositionHeaderId);

                DisPositionHeaderId = iDisPositionHeaderId.ToString();

                PhoenixInventoryStoreItemDisposition.InsertStoreItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Storeitemlocationid),
                    new Guid(DisPositionHeaderId),
                    new Guid(Storeitemid),
                    General.GetNullableDecimal(transactionqty.ToString()), null, "",
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(locationid), 0, ref iMessageCode, ref iMessageText
                    );
            }

            if (Convert.ToDecimal(actualquantity) < Convert.ToDecimal(stockquantity))
            {
                transactiontype = 9;
                transactionqty = Convert.ToDecimal(stockquantity) - Convert.ToDecimal(actualquantity);

                PhoenixInventoryStoreItemDisposition.InsertStoreItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         transactiontype, Convert.ToDateTime(System.DateTime.UtcNow),
                        null, null,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", ref iDisPositionHeaderId);

                DisPositionHeaderId = iDisPositionHeaderId.ToString();

                PhoenixInventoryStoreItemDisposition.InsertStoreItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Storeitemlocationid),
                    new Guid(DisPositionHeaderId),
                    new Guid(Storeitemid),
                    General.GetNullableDecimal(transactionqty.ToString()), null, "",
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(locationid), 0, ref iMessageCode, ref iMessageText
                    );
            }



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStoreItemControlEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvStoreItemControl;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Enter numeric decimal";
        }
        return (!ucError.IsError);
    }

    protected void ddlLocationAdd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtLocationName.Text = "--ALL Location--";
            ViewState["LOCATIONID"] = null;

            if (ddlLocationAdd.SelectedValue == "1")
            {
                divLocationList.Visible = false;
                divLocationTree.Visible = true;
                BindTreeData();
            }
            if (ddlLocationAdd.SelectedValue == "2")
            {
                divLocationTree.Visible = false;
                divLocationList.Visible = true;
                BindLocationListData();
            }
            BindData();
            gvStoreItemControl.Rebind();
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
            BindTreeData();
            BindLocationListData();

            foreach (RepeaterItem item in repLocationList.Items)
            {
                LinkButton previous = (LinkButton)item.FindControl("lnkLocationName");
                if (previous.CommandArgument == ViewState["LOCATIONID"] + "," + txtLocationName.Text)
                {
                    previous.ForeColor = System.Drawing.Color.Crimson;
                    previous.BackColor = System.Drawing.Color.FromArgb(0xFFE88C);
                }
            }
            BindData();
            gvStoreItemControl.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void UpdateExcelDownload()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());
            PhoenixInventory2XL.PopulateInventoryStoreItemQtyUpdateXl(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                         PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ddlStockClass.SelectedHard)
                        , sortexpression, sortdirection
                        , 1, iRowCount
                        , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"));



        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStoreItemControl_SortCommand(object sender, GridSortCommandEventArgs e)
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

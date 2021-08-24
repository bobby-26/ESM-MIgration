using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InventoryBulkStoreItemMove : PhoenixBasePage
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
        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Clear", "CLEAR", ToolBarDirection.Right);
        toolbarmain.AddButton("Move", "MOVE",ToolBarDirection.Right);
        MenuStoreControl.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryBulkStoreItemMove.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvStoreItemControl')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryBulkStoreItemMove.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventoryBulkStoreItemMove.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuStockItemControl.AccessRights = this.ViewState;
        MenuStockItemControl.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            gvStoreItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["LOCATIONID"] = null;
            lblSelectedNode.Text = "Root";

            txtLocationName.Text = "--ALL Location--";
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            divLocationTree.Visible = true;
            divLocationList.Visible = false;

            BindCount();
            BindTreeData();
            BindLocationListData();
            BindMoveLocation();
        }
    }

    protected void BindMoveLocation()
    {
        ddlMoveLocation.DataSource = PhoenixInventorySpareBulkControl.StockItemLocationList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        ddlMoveLocation.DataTextField = "FLDLOCATIONNAME";
        ddlMoveLocation.DataValueField = "FLDLOCATIONID";
        ddlMoveLocation.Items.Insert(0, new DropDownListItem("--Select--", ""));
        ddlMoveLocation.DataBind();
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
        tvwLocation.RootText = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
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
    protected void MenuStoreControl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MOVE"))
            {
                if (ViewState["BATCHID"].ToString() != "")
                {
                    if (!IsValidlocation(ddlMoveLocation.SelectedValue))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixInventoryStoreBulkControl.StoreItemBulkMove(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         PhoenixSecurityContext.CurrentSecurityContext.VesselID, Convert.ToInt16(ddlMoveLocation.SelectedValue), new Guid(ViewState["BATCHID"].ToString()));
                    BindData();
                    gvStoreItemControl.Rebind();
                    BindCount();
                    BindTreeData();
                    BindLocationListData();
                }
                ddlMoveLocation.SelectedValue = "Dummy";
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                if (ViewState["BATCHID"].ToString() != "")
                {
                    PhoenixInventoryStoreBulkControl.StoreItemBulkClear(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["BATCHID"].ToString()));
                }
                BindData();
                gvStoreItemControl.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                BindData();
                gvStoreItemControl.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtNumber.Text = "";
                txtName.Text = "";
                ddlStockClass.SelectedHard = "";
                chkROB.Checked = false;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStoreItemControl.Rebind();
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

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());


            DataSet ds = PhoenixInventoryStoreBulkControl.StoreItemBulkControlSearch(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                General.GetNullableInteger(ddlStockClass.SelectedHard),General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,sortexpression, sortdirection
                , gvStoreItemControl.CurrentPageIndex + 1, gvStoreItemControl.PageSize
                , ref iRowCount, ref iTotalPageCount);


            General.SetPrintOptions("gvStoreItemControl", "Store Item Control", alCaptions, alColumns, ds);

            gvStoreItemControl.DataSource = ds;
            gvStoreItemControl.VirtualItemCount = iRowCount;

            if (ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                ViewState["BATCHID"] = dr["FLDBATCHID"].ToString();
                lblbatchdate.Text = dr["FLDMODIFIEDDATE"].ToString();
            }
            else
            {
                ViewState["BATCHID"] = "";
                lblbatchdate.Text = "";
            }
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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Unit", "In Stock", "Actual" };
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

            DataSet ds = PhoenixInventoryStoreBulkControl.StoreItemBulkControlSearch(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                General.GetNullableInteger(ddlStockClass.SelectedHard), General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemBulkControl.xls");
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
        }
    }

    protected void gvStoreItemControl_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            Guid? batchid = General.GetNullableGuid(ViewState["BATCHID"].ToString());
            string quantity = ((UserControlDecimal)eeditedItem.FindControl("txtMoveQuantityEdit")).Text;
            String instock = ((RadLabel)eeditedItem.FindControl("lblInStockQuantity")).Text;

            if (!IsValidStoreItemBulkControl(instock, quantity))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixInventoryStoreBulkControl.StoreItemBulkInsert
            (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                Convert.ToInt16(((RadLabel)eeditedItem.FindControl("lblLocationId")).Text),
                ref batchid,
                new Guid(((RadLabel)eeditedItem.FindControl("lblStoreItemId")).Text),
                General.GetNullableDecimal(quantity)
           );
            gvStoreItemControl.SelectedIndexes.Clear();
            BindData();
            gvStoreItemControl.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidStoreItemBulkControl(string instock,string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        decimal result;

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Enter numeric value";

            if(General.GetNullableDecimal(quantity)>General.GetNullableDecimal(instock))
                ucError.ErrorMessage = "Moving Quantity cannot be greater than Instock quantity ";
        }
        return (!ucError.IsError);
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    private bool IsValidlocation(string location)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(location) == null)
        {
            ucError.ErrorMessage = "Select Move To Location";
        }
        return (!ucError.IsError);
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
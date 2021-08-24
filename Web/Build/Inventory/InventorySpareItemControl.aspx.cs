using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class InventorySpareItemControl : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvSpareItemControl.Items)
        {
            if (r is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation(gvSpareItemControl.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemControl.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareItemControl')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemControl.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemControl.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemControl.aspx", "Export to Sub Location", "<i class=\"fas fa-file-excel\"></i>", "SUBLOCATION EXCEL");
        toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemControl.aspx", "Export to Quantity Update", "<i class=\"fas fa-file-excel\"></i>", "UPDATE EXCEL");
        MenuStockItemControl.AccessRights = this.ViewState;  
        MenuStockItemControl.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            gvSpareItemControl.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            cmdShowComponent.Attributes.Add("onclick", "return showPickList('spnPickComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx',true);");

            ViewState["LOCATIONID"] = null;

            txtLocationName.Text = "--ALL Location--";

            divLocationTree.Visible = true;
            divLocationList.Visible = false;

            BindCount();
            BindTreeData();
            BindLocationListData();
        }
    }

    protected void BindCount()
    {
      
      PhoenixInventorySpareItemControl.SpareItemCountUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                            null);
    }


    protected void BindTreeData()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInventorySpareItemControl.SpareLocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
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
            locationcode(args.Node.Value,args.Node.Text);
            BindData();
            gvSpareItemControl.Rebind();
        }
        else
        {
            Reset();
        }
    }
    protected void locationcode(string locationid,string locationname)
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
                gvSpareItemControl.Rebind();
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
                txtMekerRef.Text = "";
                txtComponentID.Text = "";
                txtComponentName.Text = "";
                txtComponent.Text = "";
                chkROB.Checked = false;
                BindData();
                gvSpareItemControl.Rebind();
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

            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNAME", "FLDMAKERREFERENCE", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Component Name", "Makers Reference", "Unit", "In Stock", "Actual" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            DataSet ds = PhoenixInventorySpareItemControl.SpareItemLocationControlSearch(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text),General.GetNullableString(txtName.Text),
               General.GetNullableString(txtMekerRef.Text), General.GetNullableGuid(txtComponentID.Text), General.GetNullableInteger(chkCritical.Checked == true ? "1" : null), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , sortexpression, sortdirection
                , gvSpareItemControl.CurrentPageIndex + 1, gvSpareItemControl.PageSize
                , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(chkROB.Checked == true ? "1":"0"));

            General.SetPrintOptions("gvSpareItemControl", "Spare Item Control", alCaptions, alColumns, ds);

            gvSpareItemControl.DataSource = ds;
            gvSpareItemControl.VirtualItemCount = iRowCount;

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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNAME", "FLDMAKERREFERENCE", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Component Name", "Makers Reference", "Unit", "In Stock", "Actual" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvSpareItemControl.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            DataSet ds = PhoenixInventorySpareItemControl.SpareItemLocationControlSearch(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                            General.GetNullableString(txtMekerRef.Text), General.GetNullableGuid(txtComponentID.Text),  General.GetNullableInteger(chkCritical.Checked == true ? "1" : null), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , sortexpression, sortdirection
                        , 1, iRowCount
                        , ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(chkROB.Checked == true ? "1" : "0"));

            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItemControl.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Spare Item Control</h3></td>");
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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNAME", "FLDMAKERREFERENCE", "FLDUNITNAME", "FLDQUANTITY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Component Name", "Makers Reference", "Unit", "In Stock", "Actual" };

            string locationid = (ViewState["LOCATIONID"] == null) ? null : (ViewState["LOCATIONID"].ToString());

            DataSet ds = PhoenixInventorySpareItemControl.SpareItemLocationExcel(General.GetNullableInteger(locationid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItemControl.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Spare Item Control</h3></td>");
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
            ds = PhoenixInventorySpareItemControl.SpareLocationTree(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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


    protected void gvSpareItemControl_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvSpareItemControl_ItemDataBound(object sender, GridItemEventArgs e)
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

                RadLabel lblSpareItemId = (RadLabel)e.Item.FindControl("lblSpareItemId");
                RadLabel lblLocationId = (RadLabel)e.Item.FindControl("lblLocationId");
                cmdMove.Visible = SessionUtil.CanAccess(this.ViewState, cmdMove.CommandName);

                cmdMove.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inventory/InventorySpareItemMove.aspx?spareitemid=" + lblSpareItemId.Text + "&locationid=" + lblLocationId.Text + "')");
            }
        }
    }
    protected void gvSpareItemControl_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            UpdateSpareItemControl
                (
                    ((RadLabel)eeditedItem.FindControl("lblSpareItemLocationId")).Text,
                    ((RadLabel)eeditedItem.FindControl("lblSpareItemId")).Text,
                    ((RadLabel)eeditedItem.FindControl("lblStockComponentId")).Text,
                    ((RadLabel)eeditedItem.FindControl("lblInStockQuantity")).Text,
                    ((UserControlNumber)eeditedItem.FindControl("txtActualQuantityEdit")).Text
               );
            gvSpareItemControl.SelectedIndexes.Clear();
            BindData();
            gvSpareItemControl.Rebind();
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
            ViewState["LOCATIONNAME"] = locationvalue[1];

            BindData();
            gvSpareItemControl.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void UpdateSpareItemControl(string spareitemlocationid, string spareitemid, string componentid, string stockquantity, string actualquantity)
    {
        try
        {
            if (!IsValidSpareItemControlEdit(actualquantity))
            {
                ucError.Visible = true;
                return;
            }

            if (actualquantity == "")
                actualquantity = "0";
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

                PhoenixInventorySpareItemDisposition.InsertSpareItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       transactiontype, Convert.ToDateTime(System.DateTime.UtcNow),
                      null, componentid,
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", ref iDisPositionHeaderId);

                DisPositionHeaderId = iDisPositionHeaderId.ToString();

                PhoenixInventorySpareItemDisposition.InsertSpareItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(spareitemlocationid),
                    new Guid(DisPositionHeaderId),
                    new Guid(spareitemid),
                    General.GetNullableDecimal(transactionqty.ToString()), null ,"",
                     PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(locationid), 0, ref iMessageCode, ref iMessageText
                    );
            }

            if (Convert.ToDecimal(actualquantity) < Convert.ToDecimal(stockquantity))
            {
                transactiontype = 9;
                transactionqty = Convert.ToDecimal(stockquantity) - Convert.ToDecimal(actualquantity);

                PhoenixInventorySpareItemDisposition.InsertSpareItemDispositionHeader(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         transactiontype, Convert.ToDateTime(System.DateTime.UtcNow),
                        null, componentid,
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID, "", ref iDisPositionHeaderId);

                DisPositionHeaderId = iDisPositionHeaderId.ToString();

                PhoenixInventorySpareItemDisposition.InsertSpareItemDisposition(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(spareitemlocationid),
                    new Guid(DisPositionHeaderId),
                    new Guid(spareitemid),
                    General.GetNullableDecimal(transactionqty.ToString()),null,"",
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

    private bool IsValidSpareItemControlEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvSpareItemControl;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Enter numeric value";
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
            gvSpareItemControl.Rebind();
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
                if (previous.CommandArgument == ViewState["LOCATIONID"]+","+txtLocationName.Text)
                {
                    previous.ForeColor = System.Drawing.Color.Crimson;
                    previous.BackColor = System.Drawing.Color.FromArgb(0xFFE88C);

                }
            }
            BindData();
            gvSpareItemControl.Rebind();

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
            PhoenixInventory2XL.PopulateInventorySpareItemQtyUpdateXl(General.GetNullableInteger(locationid), General.GetNullableString(txtNumber.Text), General.GetNullableString(txtName.Text),
                            General.GetNullableString(txtMekerRef.Text), General.GetNullableGuid(txtComponentID.Text), General.GetNullableInteger(chkCritical.Checked == true ? "1" : null), PhoenixSecurityContext.CurrentSecurityContext.VesselID
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

    protected void gvSpareItemControl_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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

    protected void cmdClear_Click(object sender, EventArgs e)
    {
        txtComponent.Text = "";
        txtComponentName.Text = "";
        txtComponentID.Text = "";
    }
}

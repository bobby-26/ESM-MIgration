using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventoryComponentSpareItem : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in dgComponent.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(dgComponent.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryComponentSpareItem.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('dgComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListStockItem.aspx?mode=multi&componentid=" + Request.QueryString["componentid"] + "', true);", "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItem.aspx?mode=multi&componentid=" + Request.QueryString["componentid"] + "', true);", "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");


            MenuGridComponentStockItem.AccessRights = this.ViewState;
            MenuGridComponentStockItem.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["COMPONENTID"] = null;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                BindComponentData();
                dgComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindComponentData()
    {
        if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
        {
            DataSet ds = PhoenixInventoryComponent.ListComponent(new Guid(Request.QueryString["COMPONENTID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataRow dr = ds.Tables[0].Rows[0];
            //Title1.Text += "    (" + dr["FLDCOMPONENTNUMBER"].ToString() + " - " + dr["FLDCOMPONENTNAME"].ToString() + ")";
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDQUANTITY", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION" };
            string[] alCaptions = { "Number", "Name", "Maker's Reference", "In Stock", "In use", "Drawing No.", "Position" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixInventoryComponentSpareItem.ComponentStockItemSearch(ViewState["COMPONENTID"].ToString()
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , null
                                                                                    , null
                                                                                    , null
                                                                                    , null
                                                                                    , sortexpression, sortdirection
                                                                                    , dgComponent.CurrentPageIndex + 1, dgComponent.PageSize
                                                                                    , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("dgComponent", "Parts", alCaptions, alColumns, ds);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            dgComponent.DataSource = ds;
            dgComponent.VirtualItemCount = iRowCount;
            //}
            //else
            //{
            //    DataTable dt = ds.Tables[0];
            //    dgComponent.DataSource = "";
            //}
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
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDQUANTITY", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION" };
            string[] alCaptions = { "Number", "Name", "Maker's Reference", "In Stock", "In use", "Drawing No.", "Position" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixInventoryComponentSpareItem.ComponentStockItemSearch(ViewState["COMPONENTID"].ToString()
                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, null, null
                            , sortexpression, sortdirection
                            , dgComponent.CurrentPageIndex + 1, dgComponent.PageSize
                            , ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Component Spare Item", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuGridComponentStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                dgComponent.CurrentPageIndex = 0;
                BindData();
            }
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

    protected void dgComponent_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgComponent, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}
    }

    //protected void dgComponent_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    try
    //    {

    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void dgComponent_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    try
    //    {

    //        GridView _gridView = (GridView)sender;

    //        _gridView.EditIndex = de.NewEditIndex;
    //        _gridView.SelectedIndex = de.NewEditIndex;


    //        BindData();
    //        ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantityUseInComponentEdit")).Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void UpdateStockItemComponent(string stockitemcomponentid, string quantityincomponent, string drawingnumber, string position)
    {
        try
        {
            if (!IsValidComponentStockItemEdit(quantityincomponent))
            {
                ucError.Visible = true;
                return;
            }
            if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
            {
                PhoenixInventoryComponentSpareItem.UpdateComponentSpareItem
                (
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
                      , Convert.ToDecimal(quantityincomponent), drawingnumber, position
                );
            }

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
            PhoenixInventoryComponentSpareItem.DeleteComponentSpareItem
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

    private bool IsValidComponentStockItemEdit(string quantityincomponent)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //GridView _gridView = dgComponent;
        decimal result;

        if (quantityincomponent.Trim() != "")
        {
            if (decimal.TryParse(quantityincomponent, out result) == false)
                ucError.ErrorMessage = "Enter numeric decimal";
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
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblStockItemId").ToString();
            Guid guidstockitemid = new Guid(stockitemid);

            if ((Request.QueryString["COMPONENTID"] != null) && (Request.QueryString["COMPONENTID"] != ""))
            {
                PhoenixInventoryComponentSpareItem.InsertComponentStockItem
                (
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(Request.QueryString["COMPONENTID"])
                     , guidstockitemid
                     , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                );
            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void dgComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
              
                UpdateStockItemComponent
                (
                    ((RadLabel)e.Item.FindControl("lblStockItemComponentId")).Text,
                    ((RadMaskedTextBox)e.Item.FindControl("txtQuantityUseInComponentEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDrawingNumberEdit")).Text,
                    ((RadTextBox)e.Item.FindControl("txtItemPositionEdit")).Text
               );
                dgComponent.EditIndexes.Clear();

                ucStatus.Text = "Part information updated";

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStockItemComponent
                (
                   ((RadLabel)e.Item.FindControl("lblStockItemComponentId")).Text

                );
                //BindData();
                //dgComponent.Rebind();
            }
            dgComponent.Rebind();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void dgComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void dgComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null)
                {
                    ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                }
                Image img = (Image)e.Item.FindControl("imgFlag");
                string minQTYFlag = ((RadLabel)e.Item.FindControl("lblminqt")).Text;
                if (minQTYFlag == "1")
                {
                    img.Visible = true;
                    img.ToolTip = "ROB is less than Minimum Level";
                }
                else
                    img.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}

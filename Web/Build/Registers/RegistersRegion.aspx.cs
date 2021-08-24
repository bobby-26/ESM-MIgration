using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegistersRegion : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersRegion.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('RadGrid1')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersRegion.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersRegion.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuRegistersRegion.AccessRights = this.ViewState;
            MenuRegistersRegion.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                RadGrid1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuRegistersRegion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                RadGrid1.CurrentPageIndex = 0;
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("ADDADDRESS"))
            {
                RadGrid1.Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAddressFilterCriteria = null;
                txtregion.Text = "";
                RadGrid1.Rebind();
            }
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

        string[] alColumns = { "FLDREGIONNAME" };
        string[] alCaptions = { "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = int.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || int.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = int.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixRegistersRegion.SearchRegion(txtregion.Text, sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        General.ShowExcel("Region", dt, alColumns, alCaptions, null, null);
    }
    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREGIONNAME" };
        string[] alCaptions = { "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixRegistersRegion.SearchRegion(General.GetNullableString(txtregion.Text), sortexpression, sortdirection,
            RadGrid1.CurrentPageIndex + 1,
            RadGrid1.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("RadGrid1", "Region", alCaptions, alColumns, ds);


        RadGrid1.DataSource = dt;
        RadGrid1.VirtualItemCount = iRowCount;
    }

    protected void RadGrid1_InsertCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            var editableItem = ((GridEditableItem)e.Item);
            string name = (editableItem.FindControl("txtregionNameedit") as RadTextBox).Text;

            if (!IsValidRegion(name))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersRegion.InsertRegion(name.Trim());
            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        if (e.CommandName.ToUpper().Equals("ADD"))

        {
            try
            {
                GridFooterItem item = (GridFooterItem)RadGrid1.MasterTableView.GetItems(GridItemType.Footer)[0];

                string name = (item.FindControl("txtregionNameAdd") as RadTextBox).Text;

                if (!IsValidRegion(name))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersRegion.InsertRegion(name.Trim());
                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        else
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                string RegionIdEdit = ((RadLabel)e.Item.FindControl("lblregionIDEdit")).Text;
                {
                    string RegionId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDREGIONID"].ToString();
                    string RegionName = (eeditedItem.FindControl("txtregionNameedit") as RadTextBox).Text;

                    if (!IsValidRegion(RegionName))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixRegistersRegion.UpdateRegion(RegionName.Trim()
                        , Int32.Parse(RegionId));
                }

                RadGrid1.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");
        if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

        LinkButton imgApprove = (LinkButton)e.Item.FindControl("cmdRelation");
        if (imgApprove != null)
        {
            string RegionId = item.OwnerTableView.DataKeyValues[item.ItemIndex]["FLDREGIONID"].ToString();
            imgApprove.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Region', '" + Session["sitepath"] + "/Registers/RegistersRegionCountry.aspx?regionid=" + RegionId + "');return true;");
        }

        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

    }
    protected void RadGrid1_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        string RegionId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDREGIONID"].ToString();

        PhoenixRegistersRegion.DeleteRegion(Int32.Parse(RegionId));
        RadGrid1.Rebind();
    }

    protected void RadGrid1_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            int nCurrentRow = e.Item.RowIndex;

            string RegionId = eeditedItem.OwnerTableView.DataKeyValues[eeditedItem.ItemIndex]["FLDREGIONID"].ToString();
            string RegionName = (eeditedItem.FindControl("txtregionNameedit") as RadTextBox).Text;
            string RegionIdEdit = (eeditedItem.FindControl("lblregionIDEdit") as RadTextBox).Text;

            if (RegionIdEdit == string.Empty || RegionIdEdit == "")
            {
                try
                {
                    var editableItem = ((GridEditableItem)e.Item);
                    string name = (editableItem.FindControl("txtregionNameedit") as RadTextBox).Text;

                    if (!IsValidRegion(name))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixRegistersRegion.InsertRegion(name.Trim());
                    RadGrid1.Rebind();
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            else
            {
                if (!IsValidRegion(RegionName))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersRegion.UpdateRegion(RegionName.Trim()
                    , Int32.Parse(RegionId));
            }

            RadGrid1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvRegion_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{
    //    ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //    if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //    ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
    //    if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

    //    ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //    if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //    ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //    if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

    //    ImageButton add = (ImageButton)e.Row.FindControl("cmdAdd");
    //    if (add != null) add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (ViewState["SORTEXPRESSION"] != null)
    //        {
    //            HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //            if (img != null)
    //            {
    //                if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                    img.Src = Session["images"] + "/arrowUp.png";
    //                else
    //                    img.Src = Session["images"] + "/arrowDown.png";

    //                img.Visible = true;
    //            }
    //        }
    //    }
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton imgApprove = (ImageButton)e.Row.FindControl("cmdRelation");
    //        if (imgApprove != null)
    //        {
    //            imgApprove.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','../Registers/RegistersRegionCountry.aspx?regionid=" + drv["FLDREGIONID"].ToString() + "'); return false;");
    //        }
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
    //    }
    //}

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvRegion.EditIndex = -1;
    //    gvRegion.SelectedIndex = -1;
    //    BindData();
    //    SetPageNavigator();
    //}
    private bool IsValidRegion(string regionname)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (regionname.Trim().Equals(""))
            ucError.ErrorMessage = "Region is required.";
        return (!ucError.IsError);
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {

    }

    protected void RadGrid1_SortCommand(object sender, GridSortCommandEventArgs e)
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

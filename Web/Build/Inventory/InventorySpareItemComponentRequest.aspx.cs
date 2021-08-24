using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using Telerik.Web.UI;

public partial class InventorySpareItemComponentRequest : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvComponent.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvComponent.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
          

            if (!IsPostBack)
            {
                ViewState["APPROVEDYN"] = "0";
                SpareRequestEdit();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareItemComponentRequest.aspx?SPAREITEMID=" + Request.QueryString["SPAREITEMID"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (General.GetNullableInteger(ViewState["APPROVEDYN"].ToString()) != 1)
                toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?mode=custom', true);", "Component", "<i class=\"fas fa-cogs\"></i>", "ADDCOMPONENT");

            MenuGridStockItemComponent.AccessRights = this.ViewState;
            MenuGridStockItemComponent.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SpareRequestEdit()
    {

        if (Request.QueryString["SPAREITEMID"] != null && Request.QueryString["SPAREITEMID"].ToString() != "")
        {

            DataTable dt = PhoenixInventorySpareItemRequest.EditSpareItemRequest(
                new Guid(Request.QueryString["SPAREITEMID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                ViewState["APPROVEDYN"] = dr["FLDAPPROVEDYN"].ToString();
            }
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Name", "In Use", "Drawing No.", "Position", "Alternative No." };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInventorySpareItemRequest.SpareItemComponentRequestSearch(General.GetNullableGuid(Request.QueryString["SPAREITEMID"].ToString())
                ,PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , sortexpression
                , sortdirection
                , gvComponent.CurrentPageIndex + 1
                , gvComponent.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StockItemComponent.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Component</h3></td>");
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDQUANTITYINCOMPONENT", "FLDDRAWINGNUMBER", "FLDPOSITION", "FLDOLDPARTNUMBER" };
            string[] alCaptions = { "Number", "Name", "In Use", "Drawing No.", "Position", "Alternative No." };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventorySpareItemRequest.SpareItemComponentRequestSearch(General.GetNullableGuid(Request.QueryString["SPAREITEMID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , gvComponent.CurrentPageIndex + 1
                                                                                        , gvComponent.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);

            General.SetPrintOptions("gvComponent", "Component", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponent.DataSource = ds;
                gvComponent.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvComponent.DataSource = "";
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

    protected void gvComponent_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvComponent, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvComponent_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            UpdateStockItemComponent
                 (
                     ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text,
                     ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemOldNumberEdit")).Text

                );
            ucStatus.Text = "Component information updated.";

            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateStockItemComponent
                (
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtQuantityUseInComponentEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDrawingNumberEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemPositionEdit")).Text,
                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemOldNumberEdit")).Text

               );
                _gridView.EditIndex = -1;
                BindData();
                ucStatus.Text = "Component information updated.";

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteStockItemComponent
                (
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStockItemComponentId")).Text

                );
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantityUseInComponentEdit")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ViewState["SORTEXPRESSION"] != null)
                {
                    HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                    if (img != null)
                    {
                        if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                            img.Src = Session["images"] + "/arrowUp.png";
                        else
                            img.Src = Session["images"] + "/arrowDown.png";

                        img.Visible = true;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }
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
            if (General.GetNullableInteger(ViewState["APPROVEDYN"].ToString()) != 1)
            {
                PhoenixInventorySpareItemRequest.UpdateSpareItemComponentRequest(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid)
                      , new Guid(Request.QueryString["SPAREITEMID"]), General.GetNullableDecimal(quantityincomponent), drawingnumber, position, oldpartnumber);
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
            if (General.GetNullableInteger(ViewState["APPROVEDYN"].ToString()) != 1)
            {
                PhoenixInventorySpareItemRequest.DeleteSpareItemComponentRequest(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemcomponentid));
            }
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
        //GridView _gridView = gvComponent;
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
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string componentid = nvc.Get("lblComponentId").ToString();
            Guid guidcomponentid = new Guid(componentid);

            if ((Request.QueryString["SPAREITEMID"] != null) && (Request.QueryString["SPAREITEMID"] != ""))
            {
                PhoenixInventorySpareItemRequest.InsertSpareItemComponentRequest(
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , guidcomponentid
                     , new Guid(Request.QueryString["SPAREITEMID"])
                     , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }

            BindData();
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
    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
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
    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
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

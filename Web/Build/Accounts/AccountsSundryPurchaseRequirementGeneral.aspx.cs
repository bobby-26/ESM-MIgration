using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;


public partial class AccountsSundryPurchaseRequirementGeneral : PhoenixBasePage
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            menugeneral.Title = "General";
            menugeneral.AccessRights = this.ViewState;
            menugeneral.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                cmdHiddenSubmit1.Attributes.Add("style", "visibility:hidden");
                cmdHiddenSubmit2.Attributes.Add("style", "visibility:hidden");

                ViewState["REQID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            if (Request.QueryString["REQID"] != null)
            {
                ViewState["REQID"] = Request.QueryString["REQID"].ToString();
                EditRequirement();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void menugeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("NEW"))
            {
               
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreItemMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidRequirement())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixAccountsSundryPurchase.InsertSundryPurchaseRequirement(
                                          General.GetNullableInteger(ucVessel.SelectedVessel.ToString())
                                        , General.GetNullableInteger(ddlStockType.SelectedHard.ToString())
                                        , General.GetNullableInteger(ddlDepartment.SelectedDepartment.ToString()));
            ucstatus.Text = "Requirement Information Saved Successfully...";
            ucstatus.Visible = true;
            String script = String.Format("javascript:parent.document.getElementById('cmdHiddenSubmit').click();");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStoreItemMain1_TabStripCommand(object sender, EventArgs e)
    {
        ddlStockType.SelectedHard = "";
        ddlDepartment.SelectedDepartment = "";
        ucVessel.SelectedVessel = "";
        ddlStockType.Enabled = true;
        ucVessel.Enabled = true;
        ((RadComboBox)ddlDepartment.FindControl("ddldepartment")).Enabled = true;
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
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
    protected bool IsValidRequirement()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlStockType.SelectedHard.ToUpper().Trim() == "DUMMY")
            ucError.ErrorMessage = "Stock type is required.";
        if (ddlDepartment.SelectedDepartment.ToUpper().Trim() == "DUMMY")
            ucError.ErrorMessage = "Department is required.";
        if (ucVessel.SelectedVessel.ToUpper().Trim() == "DUMMY")
            ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaserequirementLineItem(General.GetNullableGuid(ViewState["REQID"] != null ? ViewState["REQID"].ToString() : string.Empty)
                 , sortexpression, sortdirection,
                Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                ref iRowCount,
                ref iTotalPageCount);
            string title = "Sundry Purchase Requirement Items";
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            DataSet ds = null;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixAccountsSundryPurchase.SearchSundryPurchaserequirementLineItem(General.GetNullableGuid(ViewState["REQID"] != null ? ViewState["REQID"].ToString() : string.Empty)
               , sortexpression, sortdirection,
              Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), 200,
              ref iRowCount,
              ref iTotalPageCount);
            string title = "Sundry Purchase Requirements";

            General.SetPrintOptions("gvCrewSearch", title, alCaptions, alColumns, ds);

            gvCrewSearch.DataSource = ds;
            gvCrewSearch.VirtualItemCount = iRowCount;

            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblrequirementlineid")).Text);
                PhoenixAccountsSundryPurchase.DeleteSundryPurchaseRequirementLineItem(id);


            }
            if (e.CommandName.ToUpper().Equals("Update"))
            {
                Guid id = Guid.Parse(((RadLabel)e.Item.FindControl("lblrequirementlineid")).Text);
                string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantity")).Text;
                if (!IsValidRequiremtLineItem(quantity))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsSundryPurchase.UpdateSundryPurchaseRequirementLineItem(id, decimal.Parse(quantity));
                ucstatus.Text = "Quantity Updated Successfully...";
                ucstatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
          
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (e.Item is GridDataItem)
                {
                    ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                  ImageButton ed = (ImageButton)e.Item.FindControl("cmdEdit");
                  if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

        }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        PhoenixAccountsSundryPurchase.DeleteSundryPurchaseRequirementLineItem(id);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}
    //protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

    //        ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
    //    }
    //}
  
    //protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
    //        if (!IsValidRequiremtLineItem(quantity))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixAccountsSundryPurchase.UpdateSundryPurchaseRequirementLineItem(id, decimal.Parse(quantity));
    //        ucstatus.Text = "Quantity Updated Successfully...";
    //        ucstatus.Visible = true;
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    _gridView.EditIndex = -1;
    //    BindData();
    //}
    private bool IsValidRequiremtLineItem(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).HasValue && General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }

        return (!ucError.IsError);
    }
   
    //private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    //{
    //    int nextRow = 0;
    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
    //    {
    //        int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

    //        String script = "var keyValue = SelectSibling(event); ";
    //        script += " if(keyValue == 38) {";  //Up Arrow
    //        nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";

    //        script += " if(keyValue == 40) {";  //Down Arrow
    //        nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

    //        script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
    //        script += "}";
    //        script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
    //        e.Row.Attributes["onkeydown"] = script;
    //    }
    //}
    private void ResetMenu(string stocktype)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsSundryPurchaseRequirementGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
        if (ViewState["REQID"] != null && ViewState["REQID"].ToString() != string.Empty)
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSundryPurchaseRequirementItem.aspx?storeclass=" + stocktype + "&REQUIREMENTID=" + ViewState["REQID"] + "', true);", "Store Item", "add.png", "ADDSTORE");
        MenuStoreItem.AccessRights = this.ViewState;
        MenuStoreItem.MenuList = toolbar.Show();
      //  MenuStoreItem.SetTrigger(pnlNTBRManager);
    }
    private void EditRequirement()
    {
        DataTable dt = PhoenixAccountsSundryPurchase.EditSundryPurchaseRequirement(new Guid(ViewState["REQID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            ddlStockType.SelectedHard = dt.Rows[0]["FLDSTOCKTYPEID"].ToString();
            ddlDepartment.SelectedDepartment = dt.Rows[0]["FLDDEPARTMENTID"].ToString();
            ucVessel.SelectedVessel = dt.Rows[0]["FLDVESSELOFFICEID"].ToString();
            ddlStockType.Enabled = false;
            ucVessel.Enabled = false;
            ((RadComboBox)ddlDepartment.FindControl("ddldepartment")).Enabled = false;
            ResetMenu(dt.Rows[0]["FLDSTOCKTYPEID"].ToString());
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}

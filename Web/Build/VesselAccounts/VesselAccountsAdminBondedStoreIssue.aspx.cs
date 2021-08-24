using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Generic;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class VesselAccountsAdminBondedStoreIssue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState); CreateMenu();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Store Item Opening", "OPENING");
            toolbar.AddButton("Store Disposition", "DISPOSITION");
            toolbar.AddButton("Purchase Order UnConfirm", "UCPROVISIONANDBOND");
            toolbar.AddButton("Issue of Bonded Stores", "BONDEDSTOREISSUE");
            //toolbar.AddButton("Round Off", "ROUNDOFF");
            //  toolbar.AddButton("Rob Initialize", "ROB");
            MenuStoreAdmin.AccessRights = this.ViewState;
            MenuStoreAdmin.MenuList = toolbar.Show();
            MenuStoreAdmin.SelectedMenuIndex = 3;
            if (!IsPostBack)
            {
                DataTable dt = PhoenixVesselAccountsBondSubsidy.FetchVesselBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                //if (dt.Rows.Count > 0 && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).HasValue
                //    && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).Value > 0)
                //{
                //    Response.Redirect("VesselAccountsBondSubsidyIssue.aspx", true);
                //}
                ddlHard.HardTypeCode = "97";
                ddlHard.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, 0, "BND");
                ddlHard.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, "BND");
                ViewState["ROB"] = string.Empty;
                ViewState["UNITPRICE"] = string.Empty;
                ViewState["UNITNAME"] = string.Empty;
                ViewState["AMOUNT"] = string.Empty;
                ViewState["TOTALAMOUNT"] = string.Empty;
                ViewState["STOREITEMID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
    }
    protected void MenuStoreAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("OPENING"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreItemOpening.aspx";
            }
            else if (CommandName.ToUpper().Equals("DISPOSITION"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsStoreDisposition.aspx";
            }
            else if (CommandName.ToUpper().Equals("UCPROVISIONANDBOND"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOrderFormUnConfirm.aspx";
            }
            else if (CommandName.ToUpper().Equals("BONDEDSTOREISSUE"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROUNDOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsRoundOffUpdate.aspx";
            }
            else if (CommandName.ToUpper().Equals("ROB"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsOpeningRobExcelUpload.aspx";
            }
            Response.Redirect(ViewState["CURRENTTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
                string[] alCaptions = { "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStoreIssue(General.GetNullableInteger(ddlEmployee.SelectedEmployee), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , General.GetNullableInteger(ddlHard.SelectedHard)
                                                                        , General.GetNullableDateTime(txtFromDate.Text)
                                                                        , General.GetNullableDateTime(txtToDate.Text)
                                                                        , sortexpression, sortdirection
                                                                        , 1, iRowCount
                                                                        , ref iRowCount, ref iTotalPageCount);
                General.ShowExcel("Issue of Bonded Stores", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }
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
            string[] alColumns = { "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
            string[] alCaptions = { "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStoreIssue(General.GetNullableInteger(ddlEmployee.SelectedEmployee), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableInteger(ddlHard.SelectedHard), General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text), sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvCrewSearch.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Issue of Bonded Stores", alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                ViewState["TOTALAMOUNT"] = ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString();
            else ViewState["TOTALAMOUNT"] = "";
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
    protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
          
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
            ((Label)e.Row.FindControl("lblAmount")).Text = ViewState["TOTALAMOUNT"].ToString();
        }
    }
    private bool IsValidIssue(string Amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(Amount).HasValue)
        {
            ucError.ErrorMessage = "Amount is required.";
        }

        return (!ucError.IsError);
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>" , "PRINT");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsAdminBondedStoreIssue.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();
    }
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void gvCrewSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");          
        }
    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblstoreitemid")).Text);             
                string Amount = ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text;
                if (!IsValidIssue(Amount))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsCorrections.UpdateBondedStoreItemAmount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id, decimal.Parse(Amount));
                Rebind();
            }  if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid id = new Guid(((RadLabel)e.Item.FindControl("lblstoreitemid")).Text);
                PhoenixVesselAccountsStoreIssue.DeleteStoreIssue(id);
                ViewState["ROB"] = string.Empty;
                ViewState["UNITPRICE"] = string.Empty;
                ViewState["UNITNAME"] = string.Empty;
                ViewState["AMOUNT"] = string.Empty;
                Rebind();
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
}

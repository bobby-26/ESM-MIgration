using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsStoreIssue : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                DataTable dt = PhoenixVesselAccountsBondSubsidy.FetchVesselBondSubsidy(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (dt.Rows.Count > 0 && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).HasValue
                    && General.GetNullableDecimal(dt.Rows[0]["FLDBONDSUBSIDYAMOUNT"].ToString()).Value > 0)
                {
                    Response.Redirect("VesselAccountsBondSubsidyIssue.aspx", true);
                }

                ddlHard.HardTypeCode = "97";
                ddlHard.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, 0, "BND");
                ddlHard.SelectedHard = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 97, "BND");
                ViewState["TOTALAMOUNT"] = string.Empty;
                ViewState["STOREITEMID"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            CreateMenu();
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
    protected void MenuBondIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDEMPLOYEENAME", "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
                string[] alCaptions = { "On Account For", "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

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
            string[] alColumns = { "FLDEMPLOYEENAME", "FLDSTORENAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDAMOUNT", "FLDISSUEDATE", "FLDREMARKS" };
            string[] alCaptions = { "On Account For", "Item Name", "Unit", "Sold Quantity", "Unit Price", "Amount", "Issue Date", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsStoreIssue.SearchStoreIssue(General.GetNullableInteger(ddlEmployee.SelectedEmployee), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , General.GetNullableInteger(ddlHard.SelectedHard)
                            , General.GetNullableDateTime(txtFromDate.Text)
                            , General.GetNullableDateTime(txtToDate.Text)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvCrewSearch.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvCrewSearch", "Issue of Bonded Stores", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = ds;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                ViewState["TOTALAMOUNT"] = ds.Tables[0].Rows[0]["FLDTOTALAMOUNT"].ToString();
            else
                ViewState["TOTALAMOUNT"] = "";
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
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "UPDATE")
            {

                string id = ((RadLabel)e.Item.FindControl("lblIssueItemid")).Text;

                string quantity = ((UserControlMaskNumber)e.Item.FindControl("txtQuantityEdit")).Text;
                string date = ((UserControlDate)e.Item.FindControl("txtDateEdit")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text;
                string oldqty = ((RadLabel)e.Item.FindControl("lblOldQty")).Text;
                string EmpId = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                if (!IsValidIssue(EmpId.ToString(), id.ToString(), date, quantity, oldqty))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsStoreIssue.UpdateStoreIssue(new Guid(id), DateTime.Parse(date), decimal.Parse(quantity), remarks
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                string id = ((RadLabel)e.Item.FindControl("lblIssueItemid")).Text;
                PhoenixVesselAccountsStoreIssue.DeleteStoreIssue(new Guid(id));
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

    protected void gvCrewSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            e.Item.HorizontalAlign = HorizontalAlign.Right;
            e.Item.Font.Bold = true;
            ((RadLabel)e.Item.FindControl("lblAmount")).Text = ViewState["TOTALAMOUNT"].ToString();
        }
    }

    private bool IsValidIssue(string employeeid, string storeitem, string date, string quantity, string oldqty)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        //decimal resultDecimal;
        decimal varience = decimal.Parse(String.IsNullOrEmpty(oldqty) ? "0" : oldqty) - decimal.Parse(String.IsNullOrEmpty(quantity) ? "0" : quantity);

        if (!General.GetNullableInteger(employeeid).HasValue)
        {
            ucError.ErrorMessage = "Staff Name is required.";
        }
        if (string.IsNullOrEmpty(storeitem))
            ucError.ErrorMessage = "Item is required.";
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Sold Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Sold Quantity should not be zero or negative";
        }
        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("StoreItemId").ToString();
            string qty = nvc.Get("Quantity");
            if (General.GetNullableInteger(ddlEmployee.SelectedEmployee).HasValue && stockitemid.Trim() != string.Empty)
            {
                PhoenixVesselAccountsStoreIssue.InsertStoreIssue(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(ddlEmployee.SelectedEmployee)
                        , stockitemid.Replace('¿', ','), General.GetNullableDateTime(nvc.Get("IssueDate")), qty.Replace('¿', ','), nvc.Get("Remarks").ToString());
                Rebind();
            }
            else
            {
                string message = "Select a Staff to add Items";
                if (stockitemid.Trim() == string.Empty)
                    message = "No Items selected.";
                ucError.ErrorMessage = message;
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void CreateMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStoreIssue.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:showPickList('spnPickListStore', 'codehelp1', '', '../VesselAccounts/VesselAccountsStoreIssueStoreItemSelection.aspx?storeclass=" + ddlHard.SelectedHard + "&accountfor=" + ddlEmployee.SelectedEmployee + "', true); return false;", "Store Item", "<i class=\"fa fa-plus-circle\"></i>", "ADDSTORE");
        toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsStoreIssue.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        MenuBondIssue.AccessRights = this.ViewState;
        MenuBondIssue.MenuList = toolbar.Show();
    }
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        CreateMenu();
        Rebind();
    }
}

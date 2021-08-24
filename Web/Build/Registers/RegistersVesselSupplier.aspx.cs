using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselSupplier : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplier.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAddress')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Registers/RegistersVesselSupplierFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSupplier.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('AddAddress','','" + Session["sitepath"] + "/Registers/RegistersVesselSupplierList.aspx'); return false;", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDADDRESS");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAddress.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvAddress.SelectedIndexes.Clear();
        gvAddress.EditIndexes.Clear();
        gvAddress.DataSource = null;
        gvAddress.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITYNAME", "FLDCOUNTRYNAME", "FLDHARDNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status", "Reported By" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;
        DataSet ds = PhoenixCommonRegisters.VesselSupplierSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                    nvc != null ? nvc.Get("txtCode").ToString() : string.Empty,
                    nvc != null ? nvc.Get("txtName").ToString() : string.Empty,
                    General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty),
                    General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty),
                    General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvAddress.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvAddress", "Vessel Supplier", alCaptions, alColumns, ds);
        gvAddress.DataSource = ds;
        gvAddress.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCODE", "FLDNAME", "FLDPHONE1", "FLDEMAIL1", "FLDCITYNAME", "FLDCOUNTRYNAME", "FLDHARDNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "Code", "Name", "Phone1", "Email", "City", "Country", "Status", "Reported By" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAddressFilterCriteria;
        DataSet ds = PhoenixCommonRegisters.VesselSupplierSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                   nvc != null ? nvc.Get("txtCode").ToString() : string.Empty,
                   nvc != null ? nvc.Get("txtName").ToString() : string.Empty,
                   General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty),
                   General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty),
                   General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   General.ShowRecords(null),
                   ref iRowCount,
                   ref iTotalPageCount);
        General.ShowExcel("Vessel Supplier", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvAddress_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAddress.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAddress_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string lblsuppliercode = ((RadLabel)e.Item.FindControl("lblsuppliercode")).Text;
                PhoenixRegistersVesselSupplier.DeleteVesselSupplier(new Guid(lblsuppliercode));
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
    protected void gvAddress_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            string lblsuppliercode = ((RadLabel)e.Item.FindControl("lblsuppliercode")).Text;
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkAddressName");
            lbtn.Attributes.Add("onclick", "openNewWindow('AddAddress', '', '" + Session["sitepath"] + "/Registers/RegistersVesselSupplierList.aspx?SupplierCode=" + lblsuppliercode + "'); return false;");

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            db1.Attributes.Add("onclick", "openNewWindow('AddAddress', '', '" + Session["sitepath"] + "/Registers/RegistersVesselSupplierList.aspx?SupplierCode=" + lblsuppliercode + "'); return false;");

            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            lbtn.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lbtn.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");

        }
    }
    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
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
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentAddressFilterCriteria = null;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

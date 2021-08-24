using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseZonePortMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        MenuPurchaseVendor.Title = PhoenixPurchaseContractVendor.VendorName + "-" + PhoenixPurchaseVendorZone.ZoneName;
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Vendors", "VENDOR");
        toolbarmain.AddButton("Zone", "ZONE");
        toolbarmain.AddButton("Product", "PRODUCT");
        MenuPurchaseVendor.AccessRights = this.ViewState;
        MenuPurchaseVendor.MenuList = toolbarmain.Show();
        MenuPurchaseVendor.SelectedMenuIndex = 1;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Purchase/PurchaseZonePortMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('rgvPort')", "Print Grid", "icon_print.png", "PRINT");
        MenuPurchaseZonePortMapping.AccessRights = this.ViewState;
        MenuPurchaseZonePortMapping.MenuList = toolbar.Show();


        if (Request.QueryString["vendorid"] != null)
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("List", "ZONELIST");
            toolbarsub.AddButton("Port", "PORT");
            MenuPurchaseVendorZoneMain.AccessRights = this.ViewState;
            MenuPurchaseVendorZoneMain.MenuList = toolbarsub.Show();
            MenuPurchaseVendorZoneMain.SelectedMenuIndex = 1;

            //ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        if (Request.QueryString["vendorid"] != null)
        {
            ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
        }
        if (Request.QueryString["zoneid"] != null)
            ViewState["zoneid"] = Request.QueryString["zoneid"].ToString();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDCOUNTRYNAME", "FLDSEAPORTNAME", "FLDISKEYPORTDESC", "FLDPORTGROUP" };
        string[] alCaptions = { "Country", "Seaport Name", "Key Port","Port Group" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseZonePortMapping.ZonePortMappingSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               General.GetNullableInteger(ViewState["vendorid"].ToString()), General.GetNullableInteger(ViewState["zoneid"].ToString()),
            sortexpression, sortdirection, 1, rgvPort.VirtualItemCount > 0 ? rgvPort.VirtualItemCount : 10, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ZonePortMapping.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Zone Port Mapping</h3></td>");
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

    protected void MenuPurchaseVendor_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VENDOR"))
            {
                Response.Redirect("../Purchase/PurchaseContractVendor.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }
            if (CommandName.ToUpper().Equals("PRODUCT"))
            {
                Response.Redirect("../Purchase/PurchaseVendorProduct.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPurchaseVendorZoneMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ZONELIST"))
            {
                Response.Redirect("../Purchase/PurchaseVendorZone.aspx?vendorid=" + ViewState["vendorid"].ToString() + "&zoneid=" + ViewState["zoneid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PurchaseZonePortMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidVendorPort(ViewState["vendorid"].ToString(), ViewState["zoneid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                rgvPort.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertZonePort(string vendorid,string vendorzoneid,string countrycode,string seaportcode,string iskeyport,string portgroup)
    {

        PhoenixPurchaseZonePortMapping.ZonePortMappingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                General.GetNullableInteger(vendorid), General.GetNullableInteger(vendorzoneid),General.GetNullableInteger(countrycode)
                                  ,General.GetNullableInteger(seaportcode),General.GetNullableInteger(iskeyport),portgroup);
    }

    private void UpdateZonePort(string vendorzoneportid,string vendorid,string vendorzoneid,string countrycode,string seaportid,string iskeyport,string portgroup)
    {
        if (!IsValidZonePortDetails(vendorid,vendorzoneid,countrycode,seaportid))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixPurchaseZonePortMapping.ZonePortMappingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(vendorzoneportid),
                              General.GetNullableInteger(vendorid),General.GetNullableInteger(vendorzoneid),
                              General.GetNullableInteger(countrycode),General.GetNullableInteger(seaportid),
                              General.GetNullableInteger(iskeyport),portgroup);
        ucStatus.Show("Information updated");
    }

    private bool IsValidZonePortDetails(string vendor,string vendorzoneid,string countrycode,string seaportid)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";

        if (vendor.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required.";

        if (vendorzoneid.Trim().Equals("") || vendorzoneid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Zone is required.";

        if (countrycode.Trim().Equals("") || countrycode.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Country is required.";

        if (seaportid.Trim().Equals("") || seaportid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "SeaPort is Required";

        return (!ucError.IsError);
    }

    private void ZonePortDelete(int zonemappingid)
    {
        PhoenixPurchaseZonePortMapping.ZonePortMappingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, zonemappingid);
    }

    private bool IsValidVendorPort(string vendor, string zone)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";

        if (vendor.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required.";

        if (zone.Trim().Equals("") || zone.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Zone is required.";

        return (!ucError.IsError);
    }

    protected void rgvPort_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOUNTRYNAME", "FLDSEAPORTNAME", "FLDISKEYPORTDESC", "FLDPORTGROUP" };
        string[] alCaptions = { "Country", "Seaport Name", "Key Port", "Port Group" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseZonePortMapping.ZonePortMappingSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              General.GetNullableInteger(ViewState["vendorid"].ToString()), General.GetNullableInteger(ViewState["zoneid"].ToString()),
           sortexpression, sortdirection, rgvPort.CurrentPageIndex+1, rgvPort.PageSize, ref iRowCount, ref iTotalPageCount);


        rgvPort.DataSource = ds;
        rgvPort.VirtualItemCount = iRowCount;

        General.SetPrintOptions("rgvPort", "Zone Port Mapping", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvPort_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            ImageButton db = (ImageButton)item["ACTION"].FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)item["ACTION"].FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)item["ACTION"].FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)item["ACTION"].FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadCheckBox chk = (RadCheckBox)item["ACTION"].FindControl("chkKeyPortEdit");
            DataRowView drv = (DataRowView)item.DataItem;

            if (drv["FLDISKEYPORT"].ToString() == "1")
            {
                if (chk != null)
                    chk.Checked = true;
            }
        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            ImageButton db = (ImageButton)item["ACTION"].FindControl("cmdAdd");
            if (db != null)
            {
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlSeaport s = (UserControlSeaport)item.FindControl("ucSeaPort");
            s.SeaportList = PhoenixRegistersSeaport.ListSeaport();

            UserControlCountry c = (UserControlCountry)item.FindControl("ucCountry");
            c.CountryList = PhoenixRegistersCountry.ListCountry(1);

        }
    }

    protected void rgvPort_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
    }

    protected void rgvPort_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidZonePortDetails(ViewState["vendorid"].ToString(), ViewState["zoneid"].ToString(),
                    ((UserControlCountry)item.FindControl("ucCountry")).SelectedCountry,
                    ((UserControlSeaport)item.FindControl("ucSeaPort")).SelectedSeaport))

                {
                    ucError.Visible = true;
                    return;
                }
                InsertZonePort(ViewState["vendorid"].ToString(), ViewState["zoneid"].ToString(),
                    ((UserControlCountry)item.FindControl("ucCountry")).SelectedCountry,
                    ((UserControlSeaport)item.FindControl("ucSeaPort")).SelectedSeaport,
                    ((RadCheckBox)item.FindControl("chkKeyPortAdd")).Checked == true ? "1" : "0",
                    ((RadTextBox)item.FindControl("txtPortGroupAdd")).Text
                 );
                rgvPort.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                UpdateZonePort(
                    item.GetDataKeyValue("FLDVENDORZONEPORTID").ToString(),
                   ViewState["vendorid"].ToString(), ViewState["zoneid"].ToString(),
                    item.GetDataKeyValue("FLDCOUNTRYCODE").ToString(),
                    item.GetDataKeyValue("FLDSEAPORTID").ToString(),
                    ((RadCheckBox)item.FindControl("chkKeyPortEdit")).Checked == true ? "1" : "0",
                    ((RadTextBox)item.FindControl("txtPortGroupEdit")).Text
                    );
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ZonePortDelete(Int32.Parse(item.GetDataKeyValue("FLDVENDORZONEPORTID").ToString()));
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseVendorZone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (PhoenixPurchaseContractVendor.VendorName != null)
            {
                ViewState["vendorname"] = PhoenixPurchaseContractVendor.VendorName;
                MenuPurchaseVendor.Title = PhoenixPurchaseContractVendor.VendorName;
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vendors", "VENDOR",ToolBarDirection.Left);
            toolbarmain.AddButton("Zone", "ZONE", ToolBarDirection.Left);
            toolbarmain.AddButton("Product", "PRODUCT", ToolBarDirection.Left);
            MenuPurchaseVendor.AccessRights = this.ViewState;
            MenuPurchaseVendor.MenuList = toolbarmain.Show();
            MenuPurchaseVendor.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseVendorZone.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvZone')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersVendorZone.AccessRights = this.ViewState;
            MenuRegistersVendorZone.MenuList = toolbar.Show();

            if (Request.QueryString["vendorid"].ToString() != null)
            {
                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("List", "ZONELIST",ToolBarDirection.Left);
                toolbarsub.AddButton("Port", "PORT",ToolBarDirection.Left);
                MenuPurchaseVendorZoneMain.AccessRights = this.ViewState;
                MenuPurchaseVendorZoneMain.MenuList = toolbarsub.Show();
                MenuPurchaseVendorZoneMain.SelectedMenuIndex = 0;

                //ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                

                if (Request.QueryString["vendorid"].ToString() != null)
                {
                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                }

                if (Request.QueryString["zoneid"] != null)
                    ViewState["zoneid"] = Request.QueryString["zoneid"].ToString();
                else
                    ViewState["zoneid"] = "";
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

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDZONECODE", "FLDZONEDESCRIPTION" };
        string[] alCaptions = { "Zone Code", "Zone Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseVendorZone.VendorZoneSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["vendorid"].ToString()), sortexpression, sortdirection,
                                                                 1,rgvZone.VirtualItemCount > 0 ? rgvZone.VirtualItemCount : 0, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ContractZone.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Contract Zone</h3></td>");
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


    protected void RegistersVendorZone_TabStripCommand(object sender, EventArgs e)
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

    protected void MenuPurchaseVendorZoneMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("PORT"))
            {
                if (!IsValidZone(ViewState["zoneid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Purchase/PurchaseZonePortMapping.aspx?vendorid=" + ViewState["vendorid"].ToString() + "&zoneid=" + ViewState["zoneid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    private void InsertContractZone(string vendorid, string code, string name)
    {

        PhoenixPurchaseVendorZone.VendorZoneInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                 General.GetNullableInteger(vendorid), code, name);
    }

    private void UpdateContractZone(string vendorid, int zoneid, string code, string name)
    {
        if (!IsValidContractZone(vendorid, code, name))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixPurchaseVendorZone.VendorZoneUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["vendorid"].ToString()),
                                zoneid, code, name);
        ucStatus.Show("Contract Zone information updated");
    }

    private bool IsValidContractZone(string vendorid, string code, string description)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";


        if (vendorid.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (code.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";


        return (!ucError.IsError);
    }

    private void ContractZoneDelete(int zoneid)
    {
        PhoenixPurchaseVendorZone.VendorZoneDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, zoneid);
    }

    private bool IsValidZone(string zone)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (zone.Trim().Equals(""))
            ucError.ErrorMessage = "Zone is required";

        return (!ucError.IsError);
    }

    protected void rgvZone_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDZONECODE", "FLDZONEDESCRIPTION" };
        string[] alCaptions = { "Zone Code", "Zone Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseVendorZone.VendorZoneSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["vendorid"].ToString()), sortexpression, sortdirection,
                                                                 rgvZone.CurrentPageIndex+1,rgvZone.PageSize, ref iRowCount, ref iTotalPageCount);


        General.SetPrintOptions("rgvZone", "Contract Zone", alCaptions, alColumns, ds);
        rgvZone.DataSource = ds;
        rgvZone.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["zoneid"].ToString() == "")
            {
                ViewState["zoneid"] = ds.Tables[0].Rows[0]["FLDVENDORZONEID"].ToString();

                PhoenixPurchaseVendorZone.ZoneName = ds.Tables[0].Rows[0]["FLDZONECODE"].ToString();
                rgvZone.SelectedIndexes.Add(0);
            }

        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvZone_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            ImageButton db = (ImageButton)item["ACTION"].FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
    }

    protected void rgvZone_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
    }

    protected void rgvZone_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidContractZone(ViewState["vendorid"].ToString(), ((RadTextBox)item.FindControl("txtZoneCodeAdd")).Text,
                    ((RadTextBox)item.FindControl("txtZoneNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertContractZone(ViewState["vendorid"].ToString(),
                    ((RadTextBox)item.FindControl("txtZoneCodeAdd")).Text,
                    ((RadTextBox)item.FindControl("txtZoneNameAdd")).Text
                );
                rgvZone.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["zoneid"] = item.GetDataKeyValue("FLDVENDORZONEID").ToString();

                PhoenixPurchaseVendorZone.ZoneName = ((RadLabel)item.FindControl("lblZoneCode")).Text;
            }

            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidContractZone(ViewState["vendorid"].ToString(), ((RadTextBox)item.FindControl("txtZoneCodeEdit")).Text,
                   ((RadTextBox)item.FindControl("txtZoneNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateContractZone(ViewState["vendorid"].ToString(),
                     Int32.Parse(item.GetDataKeyValue("FLDVENDORZONEID").ToString()),
                     ((RadTextBox)item.FindControl("txtZoneCodeEdit")).Text,
                     ((RadTextBox)item.FindControl("txtZoneNameEdit")).Text);

                rgvZone.Rebind();
                
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ContractZoneDelete(Int32.Parse(item.GetDataKeyValue("FLDVENDORZONEID").ToString()));
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

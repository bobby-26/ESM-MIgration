using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseContract : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            toolbarmain.AddButton("General", "GENERAL");
            toolbarmain.AddButton("Items", "ITEMS");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            MenuPurchaseContractMain.MenuList = toolbarmain.Show();
            MenuPurchaseContractMain.SelectedMenuIndex = 0;
                
            if (!IsPostBack)
            {
                gvPurchaseContract.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (Request.QueryString["contractid"] != null)
                {
                    ViewState["contractid"] = Request.QueryString["contractid"].ToString();

                    ViewState["PAGEURL"] = "PurchaseContractGeneral.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseContractGeneral.aspx?contractid=" + Request.QueryString["contractid"].ToString();
                }
                else
                {
                    ViewState["contractid"] = "0";
                    ViewState["PAGEURL"] = "PurchaseContractGeneral.aspx";
                    ifMoreInfo.Attributes["src"] = "PurchaseContractGeneral.aspx";
                }

                if (Request.QueryString["vendorid"] != null)
                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                else
                    ViewState["vendorid"] = "";
            }


            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseContract.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPurchaseContract')", "Print Grid", "icon_print.png", "");         
            MenuPurchaseContract.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                ViewState["PAGEURL"] = "../Purchase/PurchaseContractGeneral.aspx"; ;
                if (ViewState["contractid"].ToString() == "0")
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractGeneral.aspx";
                else
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseContractGeneral.aspx?contractid=" + ViewState["contractid"].ToString();
            }
            if (CommandName.ToUpper().Equals("ITEMS"))
            {
                if (ViewState["contractid"].ToString() == "0")
                   Response.Redirect("../Purchase/PurchaseVendorProductZone.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseVendorProductZone.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString());
            }
            if (CommandName.ToUpper().Equals("DLOCATION"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseContractDeliveryLocation.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseContractDeliveryLocation.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString());
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
        string[] alColumns = { "FLDCONTRACTNO", "FLDCONTRACTDESCRIPTION", "FLDCOMMENTS", "FLDVENDORNAME", "FLDCONTRACTDATE", "FLDEXPIRYDATE" };
        string[] alCaptions = { "Contract Number","Description ","Comments", "Vendor","Contract Date","Contract Exp Date"};
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixPurchaseContract.ContractSearch(General.GetNullableGuid(""), General.GetNullableInteger(""), null, null, null, General.GetNullableInteger(""), General.GetNullableInteger(""), General.GetNullableDateTime(""), General.GetNullableDateTime(""), sortexpression,
                       sortdirection, (int)ViewState["PAGENUMBER"], gvPurchaseContract.PageSize, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PurchaseContract.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.jpg' /></td>");
        Response.Write("<td><h3>Purchase Contract</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvPurchaseContract.Rebind();
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

    private void BindData()
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseContract.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCONTRACTNO", "FLDCONTRACTDESCRIPTION", "FLDCOMMENTS", "FLDVENDORNAME", "FLDCONTRACTDATE", "FLDEXPIRYDATE" };
        string[] alCaptions = { "Contract Number", "Description ", "Comments", "Vendor", "Contract Date", "Contract Exp Date" };
        

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseContract.ContractSearch(General.GetNullableGuid(""),General.GetNullableInteger(""),null,null,null,General.GetNullableInteger(""),General.GetNullableInteger(""),General.GetNullableDateTime(""),General.GetNullableDateTime(""),  sortexpression, 
                        sortdirection, (int)ViewState["PAGENUMBER"], gvPurchaseContract.PageSize, ref iRowCount, ref iTotalPageCount);

        gvPurchaseContract.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["contractid"].ToString() == "0")
            {
                ViewState["contractid"] = ds.Tables[0].Rows[0]["FLDcontractid"].ToString();
                ifMoreInfo.Attributes["src"] = "PurchaseContractGeneral.aspx?contractid=" + ViewState["contractid"].ToString();
                ViewState["PAGEURL"] = "PurchaseContractGeneral.aspx";
                PhoenixPurchaseContract.VendorName = ds.Tables[0].Rows[0]["FLDVENDORNAME"].ToString();
                //gvPurchaseContract.SelectedIndex = 0;
            }
            if (ViewState["vendorid"].ToString() == "")
                ViewState["vendorid"] = ds.Tables[0].Rows[0]["FLDVENDORID"].ToString();
            
            SetRowSelection();

        }
        else
        {
            PhoenixPurchaseContract.VendorName = "";
        }

        gvPurchaseContract.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvPurchaseContract", "Quotation Line item", alCaptions, alColumns, ds);    
    }

    private void SetRowSelection()
    {
        gvPurchaseContract.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvPurchaseContract.Items)
        {
            if (item.GetDataKeyValue("FLDCONTRACTID").ToString().Equals(ViewState["contractid"].ToString()))
            {
                gvPurchaseContract.SelectedIndexes.Add(item.ItemIndex);
                ifMoreInfo.Attributes["src"] = "PurchaseContractGeneral.aspx?contractid=" + ViewState["contractid"].ToString();
            }
        }
    }

    protected void gvPurchaseContract_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");            
        }

    }

    protected void gvPurchaseContract_RowCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
           
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseContract.DeleteContract(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblContractId")).Text));
                gvPurchaseContract.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["contractid"] = item.GetDataKeyValue("FLDCONTRACTID").ToString();

                ViewState["vendorid"] = item.GetDataKeyValue("FLDVENDORID").ToString();

                PhoenixPurchaseContract.VendorName = ((RadLabel)e.Item.FindControl("lblVendor")).Text;

                SetRowSelection();
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvPurchaseContract.Rebind();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }   

    protected void gvPurchaseContract_Sorting(object sender, GridSortCommandEventArgs e)
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

        gvPurchaseContract.Rebind();
    }
    protected void gvPurchaseContract_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

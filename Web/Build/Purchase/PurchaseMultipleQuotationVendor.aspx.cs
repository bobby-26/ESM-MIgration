using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseMultipleQuotationVendor : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Vessel Form", "FORMS", ToolBarDirection.Left);
            toolbarmain.AddButton("Vendor Quotations", "GENERAL",ToolBarDirection.Left);
            toolbarmain.AddButton("Multiple Reqs", "MULTIPLEREQUISITION",ToolBarDirection.Left);
            

            MenuMultipleQuotationMain.AccessRights = this.ViewState;
            MenuMultipleQuotationMain.MenuList = toolbarmain.Show();
            MenuMultipleQuotationMain.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                VesselConfiguration();
                Session["New"] = "N";
                
                if (Request.QueryString["pageno"] != null)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["orderid"] = null;
                ViewState["quotationid"] = null;
                ViewState["vendorid"] = "";

                ViewState["PAGEURL"] = null;

                ViewState["FormNumber"] = PhoenixPurchaseOrderForm.FormNumber;
                ViewState["CurrentPurchaseVesselSelection"] = Filter.CurrentPurchaseVesselSelection;
                ViewState["CurrentPurchaseStockType"] = Filter.CurrentPurchaseStockType;
                ViewState["CurrentPurchaseStockClass"] = Filter.CurrentPurchaseStockClass;

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    hdnorderId.Value= ViewState["orderid"].ToString();

                    if (Request.QueryString["pageno"] != null)
                    {
                        ViewState["pageno"] = Request.QueryString["pageno"].ToString();
                    }
                    else
                    {
                        ViewState["pageno"] = "1";
                    }
                }

                gvQuotationFormDetails.PageSize = General.ShowRecords(null);

                if (Request.QueryString["quotationid"] != null && Request.QueryString["orderid"] != null)
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();

                    if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
                        Filter.CurrentPurchaseStockType = "STORE";
                    if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SPARE"))
                        Filter.CurrentPurchaseStockType = "SPARE";
                    if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
                        Filter.CurrentPurchaseStockType = "SERVICE";

                    DataSet ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(ViewState["quotationid"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["vendorid"] = ds.Tables[0].Rows[0]["FLDVENDORID"].ToString();
                        hdnvendorid.Value= ViewState["vendorid"].ToString();

                        DataSet dsAddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(hdnvendorid.Value));

                        if (dsAddress.Tables[0].Rows.Count > 0)
                        {
                            ViewState["vendorname"] = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
                            txtVendorName.Text = ViewState["vendorname"].ToString();
                        }   

                        ViewState["vesselid"] = Filter.CurrentPurchaseVesselSelection;
                        hdnvesselid.Value= ViewState["vesselid"].ToString();
                    }
                }

                if (hdnvendorid.Value!= "" && hdnvesselid.Value!= "")
                {
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseMultipleQuotation.aspx?vendorid=" + hdnvendorid.Value + "&vesselid=" + hdnvesselid.Value;
                }
                
            }

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseMultipleQuotationVendor.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvQuotationFormDetails')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuMultipleQuotation.AccessRights = this.ViewState;
            MenuMultipleQuotation.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void MenuMultipleQuotationMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                PhoenixPurchaseOrderForm.FormNumber = ViewState["FormNumber"].ToString();
                Filter.CurrentPurchaseVesselSelection = int.Parse(ViewState["CurrentPurchaseVesselSelection"].ToString());
                Filter.CurrentPurchaseStockType = ViewState["CurrentPurchaseStockType"].ToString();
                Filter.CurrentPurchaseStockClass = ViewState["CurrentPurchaseStockClass"].ToString();
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + ViewState["pageno"].ToString());
                }
                //if (ViewState["orderid"] != null)
                //    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                //else
                //    Response.Redirect("../Purchase/PurchaseQuotationVendorDetail.aspx?pageno=" + ViewState["pageno"].ToString());

            }
            if (CommandName.ToUpper().Equals("FORMS"))
            {
                if (ViewState["orderid"] != null)
                    Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
            }
            if (CommandName.ToUpper().Equals("MULTIPLEREQUISITION"))
            {
                if (ViewState["orderid"] != null)
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString() + "&orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
                }
                else
                {
                    if (ViewState["quotationid"] != null)
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx?quotationid=" + ViewState["quotationid"].ToString());
                    else
                        Response.Redirect("../Purchase/PurchaseMultipleQuotationVendor.aspx");
                }

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
        int vesselid = -1;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDFORMTYPENAME", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDVESSELCODE" };
        string[] alCaptions = { "Number", "Form Title", "Form Type", "Type", "Component Class/Store Type ", "Vessel" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        ds = PhoenixPurchaseOrderForm.MultipleRequisitionSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, General.GetNullableInteger(ViewState["vendorid"].ToString()), "61,62", "53",
            sortexpression, sortdirection, 1, iRowCount,
               ref iRowCount, ref iTotalPageCount);


        //ds = PhoenixPurchaseOrderForm.OrderFormSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, null,
        //   null, null, null, null, null, null, null, "61", "53", null, null, null, null, null, null, null, null, null, null, null,
        //   sortexpression, sortdirection, 1, iRowCount,
        //       ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=MultipleQuotation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase order form - " + vesselname + "</center></h3></td>");
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

    protected void MenuMultipleQuotation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
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
    private void InsertOrderVender()
    {
        PhoenixPurchaseQuotation.InsertQuotation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(""),
            Int32.Parse(hdnvendorid.Value), new Guid(hdnoid.Value),
            General.GetNullableInteger(""), General.GetNullableDateTime(""),
            General.GetNullableDateTime(""), General.GetNullableDateTime(""),
            General.GetNullableDateTime(""), General.GetNullableDecimal(""),
            General.GetNullableInteger(PhoenixPurchaseOrderForm.DefaultCurrency), General.GetNullableDecimal(""),
            General.GetNullableInteger(""), "",
            General.GetNullableDateTime(""),
            General.GetNullableInteger(""),General.GetNullableInteger(""));
    }

    private bool IsValidVender()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (hdnvendorid.Value.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required. Please Select a Vendor from Quotation Page";
        if (General.GetDateTimeToString("") != null && Convert.ToDateTime("") <= Convert.ToDateTime(DateTime.Now.ToString()))
            ucError.ErrorMessage = "Expiry  date should be greater than today's date.";
        if (hdnoid.Value.Trim() == "")
            ucError.ErrorMessage = "Please select a form to assign a vendor";
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvQuotationFormDetails.Rebind();
        if (Session["New"].ToString() == "Y")
        {
            gvQuotationFormDetails.SelectedIndexes.Add(0);
            Session["New"] = "N";
        }
    }  
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if(ds.Tables[0].Rows.Count >0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void gvQuotationFormDetails_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
    }

    protected void gvQuotationFormDetails_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        gvQuotationFormDetails.Rebind();
    }

    protected void gvQuotationFormDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDFORMTYPENAME", "FLDSTOCKTYPE", "FLDSTOCKCLASS", "FLDVESSELCODE" };
        string[] alCaptions = { "Number", "Form Title", "Form Type", "Type", "Component Class/Store Type ", "Vessel" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = new DataSet();

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        ds = PhoenixPurchaseOrderForm.MultipleRequisitionSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, vesselid, General.GetNullableInteger(ViewState["vendorid"].ToString()), "61,62", "53",
            sortexpression, sortdirection, gvQuotationFormDetails.CurrentPageIndex+1, gvQuotationFormDetails.PageSize,
               ref iRowCount, ref iTotalPageCount);

        gvQuotationFormDetails.DataSource = ds;
        gvQuotationFormDetails.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            vesselname = dr["FLDVESSELNAME"].ToString();
            if (ViewState["orderid"] == null)
            {
                ViewState["orderid"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                hdnorderId.Value = ViewState["orderid"].ToString();
                gvQuotationFormDetails.SelectedIndexes.Add(0);
                MenuMultipleQuotationMain.Visible = true;
            }
        }
        General.SetPrintOptions("gvQuotationFormDetails", "Order Form List -  " + vesselname, alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvQuotationFormDetails_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvQuotationFormDetails_EditCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvQuotationFormDetails_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            LinkButton db = (LinkButton)item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

    }

    protected void gvQuotationFormDetails_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                hdnoid.Value = item.GetDataKeyValue("FLDORDERID").ToString();
                Filter.CurrentPurchaseStockType = ((RadLabel)item.FindControl("lblStockType")).Text;
                if (!IsValidVender())
                {
                    ucError.Visible = true;
                    return;
                }

                int iRowCount = 0;
                int iTotalPageCount = 0;
                DataSet quotationidds = PhoenixPurchaseOrderForm.QuotationSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["vesselid"].ToString()),
                                                                                           int.Parse(ViewState["vendorid"].ToString()), new Guid(hdnoid.Value),
                                                                                           null, null, 1, 100,
                                                                                               ref iRowCount, ref iTotalPageCount);
                if (quotationidds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = quotationidds.Tables[0].Rows[0];
                    string qid = dr["FLDQUOTATIONID"].ToString();
                }
                else
                {
                    InsertOrderVender();
                }
                gvQuotationFormDetails.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvQuotationFormDetails_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }
}

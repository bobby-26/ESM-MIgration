using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class PurchaseDeliveryDetail : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
       // SetRowSelection();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            toolbar.AddButton("General", "GENERAL");            
            toolbar.AddButton("Forms", "FORMLIST");
            //toolbar.AddButton("Correction", "CORRECTION");

            MenuDelivery.AccessRights = this.ViewState;  
            MenuDelivery.MenuList = toolbar.Show();
            MenuDelivery.SelectedMenuIndex = 0;
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            
            if (!IsPostBack)
            {
                gvDelivery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["deliveryid"] = null; 
                if (Request.QueryString["deliveryid"] != null)
                {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseDelivery.aspx?deliveryid=" + Request.QueryString["deliveryid"].ToString();
                    SetRowSelection();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;             
            }

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseDeliveryDetail.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDelivery')", "Print Grid", "icon_print.png", "Print");
            toolbar.AddImageButton("../Purchase/PurchaseDeliveryFilter.aspx", "Find", "search.png", "FIND");
            //toolbar.AddImageLink("javascript:Openpopup('codehelp1','','../Purchase/PurchaseForwarderExcelUpload.aspx')", "Upload", "icon_xls.png", "UPLOAD");
            //toolbar.AddImageButton("../Purchase/PurchaseForwarderExcelUpload.aspx", "Find", "icon_xls.png", "UPLOAD");            
            MenuDeliveryList.AccessRights = this.ViewState; 
            MenuDeliveryList.MenuList = toolbar.Show();
     
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

        string[] alColumns = {"FLDFORMNO","FLDDELIVERYNUMBER", "FLDVESSELNAME", "FLDFORWARDERNAME", "FLDFORWARDERRECEIVEDDATE", "FLDAGENTNAME"
                                 };
        string[] alCaptions = {"PO Number","Delivery Number", "Vessel", "Forwarder", "Received Date", "Agent"
                                 };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (Filter.CurrentDeliveryFormFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentDeliveryFormFilterCriteria;
            ds = PhoenixPurchaseDelivery.DeliverySearch(General.GetNullableInteger(nvc.Get("ucVessel").ToString()), nvc.Get("txtDeliveryNumber").ToString()
                , nvc.Get("txtDocumentNumber").ToString(), General.GetNullableInteger(nvc.Get("ucFormType").ToString())
                , General.GetNullableDateTime(nvc.Get("txtFromDate")), General.GetNullableDateTime(nvc.Get("txtToDate"))
                , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                , iRowCount, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseDelivery.DeliverySearch(
                vesselid, null, null, General.GetNullableInteger(null), null,
                null
                , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                , iRowCount, ref iRowCount, ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Delivery List</h3></td>");
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
                //Response.Write(dr[alColumns[i]]);
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuDeliveryList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvDelivery.Rebind();
                
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDelivery.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = {"FLDFORMNO","FLDDELIVERYNUMBER", "FLDVESSELNAME", "FLDFORWARDERNAME", "FLDFORWARDERRECEIVEDDATE", "FLDAGENTNAME"
                                 };
        string[] alCaptions = {"PO Number","Delivery Number", "Vessel", "Forwarder", "Received Date", "Agent"
                                 };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;


        DataSet ds;
        if (Filter.CurrentDeliveryFormFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.CurrentDeliveryFormFilterCriteria;
            ds = PhoenixPurchaseDelivery.DeliverySearch(General.GetNullableInteger( nvc.Get("ucVessel").ToString()), nvc.Get("txtDeliveryNumber").ToString()
                , nvc.Get("txtDocumentNumber").ToString(), General.GetNullableInteger(nvc.Get("ucFormType").ToString())
                , General.GetNullableDateTime(nvc.Get("txtFromDate")), General.GetNullableDateTime(nvc.Get("txtToDate"))
                , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                , gvDelivery.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixPurchaseDelivery.DeliverySearch(vesselid, null, null, null, null,
                null
                , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"]
                , gvDelivery.PageSize, ref iRowCount, ref iTotalPageCount);
        }
        gvDelivery.DataSource = ds;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["deliveryid"] == null || ViewState["deliveryid"].ToString() == "0")
            {
                ViewState["deliveryid"] = ds.Tables[0].Rows[0]["FLDDELIVERYID"].ToString();
                ifMoreInfo.Attributes["src"] = "PurchaseDelivery.aspx?deliveryid=" + ViewState["deliveryid"].ToString();
            }
        }
        else
        {
            ifMoreInfo.Attributes["src"] = "PurchaseDelivery.aspx";
        }
        gvDelivery.VirtualItemCount = iRowCount;
         ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvDelivery", "Delivery List", alCaptions, alColumns, ds);
    }

    protected void gvDelivery_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        gvDelivery.Rebind();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            gvDelivery.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvDelivery_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixPurchaseDelivery.Deletedelivery(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                new Guid(item.GetDataKeyValue("FLDDELIVERYID").ToString()));
            gvDelivery.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("ONDELIVERYNUMBER") || e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            gvDelivery.SelectedIndexes.Add(e.Item.ItemIndex);
            ViewState["deliveryid"] = item.GetDataKeyValue("FLDDELIVERYID").ToString();
            ifMoreInfo.Attributes["src"] = "PurchaseDelivery.aspx?deliveryid=" + ViewState["deliveryid"].ToString();
            // gvDelivery.Rebind();
            SetRowSelection();
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        

    }

    protected void gvDelivery_RowDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridHeaderItem)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}

        if (e.Item is GridEditableItem)
        {

                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlCommonToolTip ucToolTip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (General.GetNullableInteger(drv["FLDAGENTID"].ToString()) == null && ucToolTip != null)
            {
                ucToolTip.Visible = false;
            }
        }
    }

    protected void MenuDelivery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                    if (ViewState["deliveryid"] != null && ViewState["deliveryid"].ToString() != "")
                        ifMoreInfo.Attributes["src"] = "PurchaseDelivery.aspx?deliveryid=" + ViewState["deliveryid"].ToString();
                    else
                        ifMoreInfo.Attributes["src"] = "PurchaseDelivery.aspx";  
            }
            if (CommandName.ToUpper().Equals("FORMLIST"))
            {
                if (ViewState["deliveryid"] != null)
                    Response.Redirect("../Purchase/PurchaseDeliveredForms.aspx?deliveryid=" + ViewState["deliveryid"].ToString());
                else
                {
                     ucError.ErrorMessage="No delivery entrys, Please add delivery";
                     ucError.Visible = true;                     
                }
            }
            if (CommandName.ToUpper().Equals("CORRECTION"))
            {
                Response.Redirect("../Purchase/PurchaseDeliveryDetailTemp.aspx");                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvDelivery.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDelivery.Items)
        {
            if (item.GetDataKeyValue("FLDDELIVERYID").ToString() == ViewState["deliveryid"].ToString())
            {
                gvDelivery.SelectedIndexes.Add(item.ItemIndex);
                PhoenixPurchaseOrderForm.FormNumber = item.GetDataKeyValue("FLDDELIVERYID").ToString();
                Filter.CurrentPurchaseVesselSelection = int.Parse(item.GetDataKeyValue("FLDVESSELID").ToString());
                Filter.CurrentPurchaseStockType = item.GetDataKeyValue("FLDSTOCKTYPE").ToString();
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["deliveryid"] = null;
            gvDelivery.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDelivery_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
        SetRowSelection();
    }
}

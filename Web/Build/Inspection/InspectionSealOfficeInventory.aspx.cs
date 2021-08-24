using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionSealOfficeInventory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealOfficeInventory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealInventory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSeal.AccessRights = this.ViewState;
            MenuSeal.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
               
                gvSealInventory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSeal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDPURCHASEDQTY", "FLDISSUEDQTY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Purchased", "Issued to vessel", "In Stock" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionSealInventory.SealOfficeInventorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableInteger(ucSealType.SelectedQuick),
                                sortexpression, sortdirection,
                                Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                                iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Office Inventory of seals", ds.Tables[0], alColumns, alCaptions, null, "");
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
            string[] alColumns = { "FLDLOCATIONNAME", "FLDNUMBER", "FLDNAME", "FLDPURCHASEDQTY" ,"FLDISSUEDQTY", "FLDQUANTITY" };
            string[] alCaptions = { "Location", "Number", "Name", "Purchased", "Issued to vessel", "In Stock" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionSealInventory.SealOfficeInventorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                General.GetNullableInteger(ucSealType.SelectedQuick),
                                sortexpression, sortdirection,
                                gvSealInventory.CurrentPageIndex+1,
                               gvSealInventory.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSealInventory", "Office Inventory of seals", alCaptions, alColumns, ds);
            gvSealInventory.DataSource = ds;
            gvSealInventory.VirtualItemCount = iRowCount;
           

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvSealInventory_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            LinkButton lnkIssuedQty = (LinkButton)e.Row.FindControl("lnkIssuedQty");
    //            Label lblLocationid = (Label)e.Row.FindControl("lblLocationid");
    //            if (lnkIssuedQty != null && lblLocationid != null)
    //                lnkIssuedQty.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealIssuedtoVessel.aspx?LOCATIOND=" + lblLocationid.Text + "',null)");

    //            LinkButton lnkPurchasedQty = (LinkButton)e.Row.FindControl("lnkPurchasedQty");
    //            Label lblStoreItemId = (Label)e.Row.FindControl("lblStoreItemId");
    //            if (lnkPurchasedQty != null && lblLocationid != null && lblStoreItemId != null)
    //                lnkPurchasedQty.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','', '../Inspection/InspectionSealsPurchased.aspx?LOCATIOND=" + lblLocationid.Text + "&STOREITEMID=" + lblStoreItemId.Text + "',null)");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            //SetPageNavigator();
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
            ViewState["PAGENUMBER"] = 1;
            BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealInventory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }


    protected void gvSealInventory_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
          
           
            if (e.Item is GridDataItem)
            {
                LinkButton lnkIssuedQty = (LinkButton)e.Item.FindControl("lnkIssuedQty");
                RadLabel lblLocationid = (RadLabel)e.Item.FindControl("lblLocationid");
                if (lnkIssuedQty != null && lblLocationid != null)
                    lnkIssuedQty.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealIssuedtoVessel.aspx?LOCATIOND=" + lblLocationid.Text + "',null)");

                LinkButton lnkPurchasedQty = (LinkButton)e.Item.FindControl("lnkPurchasedQty");
                RadLabel lblStoreItemId = (RadLabel)e.Item.FindControl("lblStoreItemId");
                if (lnkPurchasedQty != null && lblLocationid != null && lblStoreItemId != null)
                    lnkPurchasedQty.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSealsPurchased.aspx?LOCATIOND=" + lblLocationid.Text + "&STOREITEMID=" + lblStoreItemId.Text + "',null)");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

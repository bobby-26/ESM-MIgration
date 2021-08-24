using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Purchase_PurchaseSpareItemSearchAcrossVessel : PhoenixBasePage
{
    string partno = null;
    int Vesselid;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (Request.QueryString["ItemNo"] != null)
                MenuSpareItemList.Title = "Spare Item [ " + Request.QueryString["ItemNo"].ToString() + " ]";
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Purchase/PurchaseSpareItemSearchAcrossVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareItemList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "Print");
            MenuSpareItemList.AccessRights = this.ViewState;
            MenuSpareItemList.MenuList = toolbargrid.Show();


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["partno"] = "";
                ViewState["Vesselid"] = "";
                if (Request.QueryString["ItemNo"] != null && Request.QueryString["ItemNo"] != "")
                {
                    ViewState["partno"] = Request.QueryString["ItemNo"].ToString();
                }
                if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"] != "")
                {
                    ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
                }
                
                gvSpareItemList.PageSize = General.ShowRecords(null);
            }

            partno = ViewState["partno"].ToString();
            Vesselid = Int32.Parse(ViewState["Vesselid"].ToString());
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSpareItemList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvSpareItemList.Rebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
 
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        DataTable dt;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDPARTNUMBER", "FLDVENDORNAME", "FLDREQUESTEDQUANTITY","FLDSTATUS"};
        string[] alCaptions = { "Form No", "Title", "Part No", "Vendor Name", "Requested Qty","Requisition Status"};
        int iRowCount;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPurchaseOrderLine.SpareItemSearchByReference(partno,Vesselid ,sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                               iRowCount, ref iRowCount, ref iTotalPageCount);
        dt = ds.Tables[0];
        General.ShowExcel("Critical Spare Item", dt, alColumns, alCaptions, null, null);

    }
    protected void gvSpareItemList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDPARTNUMBER", "FLDVENDORNAME", "FLDREQUESTEDQUANTITY", "FLDSTATUS" };
        string[] alCaptions = { "Form No", "Title", "Part No", "Vendor Name", "Requested Qty", "Requisition Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderLine.SpareItemSearchByReference(partno, Vesselid, sortexpression, sortdirection, gvSpareItemList.CurrentPageIndex+1,
                                                               gvSpareItemList.PageSize, ref iRowCount, ref iTotalPageCount);

        gvSpareItemList.DataSource = ds;
        General.SetPrintOptions("gvSpareItemList", "Critical Spare Item", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSpareItemList_ItemDataBound1(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
                LinkButton lbtn = (LinkButton)item.FindControl("lnkVendorName");
            if (lbtn != null)
                lbtn.Attributes.Add("onclick", "openNewWindow('AddAddress', '', 'Registers/Registersoffice.aspx?addresscode=" + item.GetDataKeyValue("FLDADDRESSCODE").ToString() + "'); return false;");
        }
    }
}

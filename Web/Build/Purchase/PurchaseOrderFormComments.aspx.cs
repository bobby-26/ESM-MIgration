using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PurchaseOrderFormComments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Vessel Form", "FORM",ToolBarDirection.Left);
        toolbarmain.AddButton("Comments", "COMMENTS",ToolBarDirection.Left);
    
        MenuMainFilter.AccessRights = this.ViewState;
        MenuMainFilter.MenuList = toolbarmain.Show();

        if (Request.QueryString["frmreport"] != null)
        {
            MenuMainFilter.Visible = false;
            Filter.CurrentPurchaseVesselSelection = int.Parse(Request.QueryString["vesselid"].ToString());
        }
        PhoenixToolbar toolbarsave = new PhoenixToolbar();
        toolbarsave.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuSaveFilter.AccessRights = this.ViewState;
        MenuSaveFilter.MenuList = toolbarsave.Show();

        //PhoenixToolbar toolbarsub = new PhoenixToolbar();
        //toolbarsub.AddImageButton("../Purchase/PurchaseOrderFormRemarks.aspx?orderid=" + Request.QueryString["orderid"], "Export to Excel", "icon_xls.png", "Excel");
        //toolbarsub.AddImageLink("javascript:CallPrint('gvPurchaseRemarks')", "Print Grid", "icon_print.png", "PRINT");
        //MenuPurchaseRemarks.AccessRights = this.ViewState;
        //MenuPurchaseRemarks.MenuList = toolbarsub.Show();

        if (!IsPostBack)
        {
            rgvPurchaseRemarks.PageSize = General.ShowRecords(null);
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["orderid"] = null;
            if (Request.QueryString["frmreport"] == null)
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();

        }

        if (Request.QueryString["orderid"].ToString() != null)
        {
            ViewState["orderid"] = Request.QueryString["orderid"];
        }

        MenuMainFilter.SelectedMenuIndex = 1;

    }

    protected void MenuMainFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
             RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FORM"))
            {
                if (ViewState["orderid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseForm.aspx?pageno=" + ViewState["pageno"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseForm.aspx?orderid=" + ViewState["orderid"].ToString() + "&pageno=" + ViewState["pageno"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSaveFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                InsertComments();
            }

            txtNotesDescription.Text = "";
            rgvPurchaseRemarks.Rebind();
            

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvPurchaseRemarks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPOSTEDBY", "FLDREMARKS", "FLDPOSTEDDATE" };
        string[] alCaptions = { "Posted By", "Comments", "Posted Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseComments.CommentsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
              General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()),
           sortexpression, sortdirection, rgvPurchaseRemarks.CurrentPageIndex+1,rgvPurchaseRemarks.PageSize , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvPurchaseRemarks", "Comments", alCaptions, alColumns, ds);

        rgvPurchaseRemarks.DataSource = ds;
        rgvPurchaseRemarks.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void rgvPurchaseRemarks_PreRender(object sender, EventArgs e)
    {
    }
    private void InsertComments()
    {
        if (!IsValidComments())
        {
            ucError.Visible = true;
            return;
        }

        PhoenixPurchaseComments.InsertComments(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                General.GetNullableGuid(ViewState["orderid"].ToString()),
                                                General.GetNullableInteger(Filter.CurrentPurchaseVesselSelection.ToString()),
                                                txtNotesDescription.Text);

    }

    private bool IsValidComments()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtNotesDescription.Text == null || txtNotesDescription.Text == "")
            ucError.ErrorMessage = "Please Enter Comments";

        return (!ucError.IsError);
    }
}

using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonPickListComponentPurchase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ViewState["showcritical"] = "";
        if (Request.QueryString["showcritical"] != null && Request.QueryString["showcritical"].ToString() != "")
        {
            ViewState["showcritical"] = Request.QueryString["showcritical"].ToString();
        }

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvComponent.PageSize = General.ShowRecords(null);
        }
        
    }

    protected void gvComponent_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCommonInventory.ComponentPicklistSearch(
                vesselid, txtNumberSearch.Text, txtComponentNameSearch.Text,
                General.GetNullableByte(ViewState["showcritical"].ToString()),
                sortexpression, sortdirection,
                gvComponent.CurrentPageIndex + 1,
                gvComponent.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            gvComponent.DataSource = ds;
            gvComponent.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT") || e.CommandName.ToUpper().Equals("ROWCLICK"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            
            NameValueCollection nvc;
            string Script = "";
            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                RadLabel lblComponentNumber = (RadLabel)item.FindControl("lblComponentNumber");
                nvc.Add(lblComponentNumber.ID, lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)item.FindControl("lnkComponentName");
                nvc.Add(lbComponentName.ID, lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)item.FindControl("lblComponentId");
                nvc.Add(lblComponentId.ID, lblComponentId.Text);
            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                RadLabel lblComponentNumber = (RadLabel)item.FindControl("lblComponentNumber");
                nvc.Set(nvc.GetKey(1), lblComponentNumber.Text.ToString());
                LinkButton lbComponentName = (LinkButton)item.FindControl("lnkComponentName");
                nvc.Set(nvc.GetKey(2), lbComponentName.Text.ToString());
                RadLabel lblComponentId = (RadLabel)item.FindControl("lblComponentId");
                nvc.Set(nvc.GetKey(3), lblComponentId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page,typeof(Page), "BookMarkScript", Script,false);
        }
    }
}

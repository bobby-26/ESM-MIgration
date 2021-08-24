using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonPickListVessel : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);
        MenuVessel.MenuList = toolbarmain.Show();
        //MenuVessel.SetTrigger(pnlVessel);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        gvVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
    }

    protected void MenuVessel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            ds = PhoenixCommonRegisters.VesselSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , txtVesselNameSearch.Text
            , sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvVessel.PageSize,
            ref iRowCount,
            ref iTotalPageCount, null);

            gvVessel.DataSource = ds;
            gvVessel.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {

        string Script = "";
        NameValueCollection nvc;

        if (Request.QueryString["mode"] == "custom")
        {

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = new NameValueCollection();

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblVesselId");
            nvc.Add(lbl.ID, lbl.Text);
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVesselName");
            nvc.Add(lb.ID, lb.Text.ToString());
        }
        else
        {

            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";

            nvc = Filter.CurrentPickListSelection;

            RadLabel lbl = (RadLabel)e.Item.FindControl("lblVesselId");
            nvc.Set(nvc.GetKey(1), lbl.Text);
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkVesselName");
            nvc.Set(nvc.GetKey(2), lb.Text.ToString());
        }

        //Filter.CurrentPickListSelection = nvc;
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        Filter.CurrentPickListSelection = nvc;
        RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);

    }

    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

    protected void gvVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CommonPickListVesselFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvVessel.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

            ds = PhoenixCommonRegisters.VesselFilterSearch(
                                                             General.GetNullableInteger(ucVessel.SelectedVessel),
                                                             General.GetNullableInteger(ucVesselType.SelectedVesseltype),
                                                             General.GetNullableInteger(ucEngineType.SelectedEngineName),
                                                             General.GetNullableInteger(ucPrincipal.SelectedAddress)
                                                             , sortexpression, sortdirection
                                                             , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                             , gvVessel.PageSize
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);
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

    protected void gvVessel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVessel.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;   
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
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

                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkVesselName");
                    nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblVesselId");
                    nvc.Set(nvc.GetKey(2), lbl.Text);
                }

                Filter.CurrentPickListSelection = nvc;
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript1", Script, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVessel_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }

}

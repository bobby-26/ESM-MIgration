using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class CommonPickListVoyage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            gvVoyage.PageSize = General.ShowRecords(null);

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            else
                ViewState["VESSELID"] = Request.QueryString["vesselid"] != null ? Request.QueryString["vesselid"].ToString() : "0";

            if (Request.QueryString["voyageid"] != null)
                ViewState["VOYAGEID"] = Request.QueryString["voyageid"].ToString();
            else
                ViewState["VOYAGEID"] = "";   
        }

    }

    protected void gvVoyage_ItemDataBound(Object sender, GridItemEventArgs e)
    {

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionVoyageData.NewVoyageDataSearch(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            gvVoyage.DataSource = ds;

        gvVoyage.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvVoyage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVoyage_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnReloadList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkVoyageId");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblVoyageId");
                nvc.Add(lbl.ID, lbl.Text);
            }
            else
            {

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkVoyageId");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblVoyageId");
                nvc.Set(nvc.GetKey(2), lbl.Text);


                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                if (Request.QueryString["iframignore"] != null && Request.QueryString["iframignore"] == "true")
                    Script += "fnClosePickList('codehelp1','ifMoreInfo',true);";
                else
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }
    }

}

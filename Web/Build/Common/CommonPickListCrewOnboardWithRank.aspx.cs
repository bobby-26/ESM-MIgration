using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CommonPickListCrewOnboardWithRank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
            gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    private void BindData()
    {
        try
        {
            DataSet ds;
            int? vesselid = (Request.QueryString["VesselId"] != null) ? General.GetNullableInteger(Request.QueryString["VesselId"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            DateTime? date = (Request.QueryString["date"] != null) ? General.GetNullableDateTime(Request.QueryString["date"].ToString()) : General.GetNullableDateTime("");
            ds = PhoenixCrewManagement.ListCrewOnboard(
                vesselid == null ? 0 : vesselid
                , General.GetNullableInteger(ucRank.SelectedRank)
                , date
                , null);
            gvCrewList.DataSource = ds;
            gvCrewList.VirtualItemCount = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        string Script = "";
        NameValueCollection nvc;
        if (Request.QueryString["mode"] == "custom")
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            nvc = new NameValueCollection();
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            nvc.Add(lb.ID, lb.Text.ToString());
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            nvc.Add(lblRank.ID, lblRank.Text);
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblCrewId");
            nvc.Add(lbl.ID, lbl.Text);
        }
        else
        {
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1','ifMoreInfo');";
            Script += "</script>" + "\n";
            nvc = Filter.CurrentPickListSelection;
            LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
            nvc.Set(nvc.GetKey(1), lb.Text.ToString());
            RadLabel lblRank = (RadLabel)e.Item.FindControl("lblRank");
            nvc.Set(nvc.GetKey(2), lblRank.Text.ToString());
            RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");
            nvc.Set(nvc.GetKey(3), lblCrewId.Text.ToString());
        }
        Filter.CurrentPickListSelection = nvc;
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
    }
    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

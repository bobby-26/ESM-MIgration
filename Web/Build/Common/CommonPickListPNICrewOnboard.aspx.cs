using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
using System.Collections;
using System.Configuration;
using System.Web.Security;
using System.Text;

public partial class CommonPickListPNICrewOnboard : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["VesselId"] != null)
                ViewState["VesselId"] = Request.QueryString["VesselId"].ToString();
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
        if (e.CommandName.ToUpper().Equals("PICKLIST"))
        {
            if (Request.QueryString["mode"] == "custom")
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";

                    if (ViewState["framename"] != null)
                        Script += "fnReloadList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnReloadList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = new NameValueCollection();

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
                nvc.Add(lb.ID, lb.Text.ToString());
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblCrewId");
                nvc.Add(lbl.ID, lbl.Text.ToString());
                RadLabel lblRank = (RadLabel)e.Item.FindControl("lblCrewRank");
                nvc.Add(lblRank.ID, lblRank.Text.ToString());
            }
            else
            {
                if (Request.QueryString["ignoreiframe"] != null)
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo', true);";
                    Script += "</script>" + "\n";
                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    if (ViewState["framename"] != null)
                        Script += "fnClosePickList('codehelp1','" + ViewState["framename"].ToString() + "');";
                    else
                        Script += "fnClosePickList('codehelp1');";
                    Script += "</script>" + "\n";
                }

                nvc = Filter.CurrentPickListSelection;

                LinkButton lb = (LinkButton)e.Item.FindControl("lnkCrew");
                nvc.Set(nvc.GetKey(1), lb.Text.ToString());
                RadLabel lblCrewRank = (RadLabel)e.Item.FindControl("lblCrewRank");
                nvc.Set(nvc.GetKey(2), lblCrewRank.Text.ToString());
                RadLabel lblCrewId = (RadLabel)e.Item.FindControl("lblCrewId");
                nvc.Set(nvc.GetKey(3), lblCrewId.Text.ToString());
            }

            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
        }
    }
    protected void Rebind()
    {
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
    protected void gvCrewList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }




}

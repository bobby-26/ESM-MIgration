using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class CrewOffshoreTrainingNeedsCompletedSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Search", "SEARCH");
            toolbarsub.AddButton("Clear", "CLEAR");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarsub.Show();
            if (!IsPostBack)
            {
                ViewState["PENDINGNEEDSYN"] = "";
                ViewState["OVERRIDEYN"] = "";
                ViewState["VESSELYN"] = "";
                if (Request.QueryString["PendingNeedsYN"] != null && Request.QueryString["PendingNeedsYN"].ToString() != "")
                    ViewState["PENDINGNEEDSYN"] = Request.QueryString["PendingNeedsYN"].ToString();
                if (Request.QueryString["Vessel"] != null && Request.QueryString["Vessel"].ToString() != "")
                    ViewState["VESSELYN"] = Request.QueryString["Vessel"].ToString();
                if (Request.QueryString["Override"] != null && Request.QueryString["Override"].ToString() != "")
                    ViewState["OVERRIDEYN"] = Request.QueryString["Override"].ToString();

                //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                //{
                //    ucVessel.Enabled = false;
                //    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //}


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

      private void Clear()
    {
        txtName.Text = "";
        txtFileNo.Text = "";
        ddlRank.SelectedRank = "";
        //ucVessel.SelectedVessel = "";
        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        //{
        //    ucVessel.Enabled = false;
        //    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        //}
        // ddlstatus.SelectedIndex = 0;

    }
    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SEARCH"))
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("txtName", txtName.Text);
            nvc.Add("txtFileNo", txtFileNo.Text);
            nvc.Add("ddlRank", ddlRank.SelectedRank);
           // nvc.Add("ucVessel", ucVessel.SelectedVessel);


            Filter.CurrentOffshoreTrainingNeedSearch = nvc;
            if (ViewState["PENDINGNEEDSYN"].ToString() == "1" && ViewState["VESSELYN"].ToString() == "0" && ViewState["OVERRIDEYN"].ToString() == "0")
                Response.Redirect("CrewOffshoreTrainingNeeds.aspx", true);
            else if (ViewState["PENDINGNEEDSYN"].ToString() == "1" && ViewState["VESSELYN"].ToString() == "1" && ViewState["OVERRIDEYN"].ToString() == "0")
                Response.Redirect("CrewOffshoreTrainingNeedsVessel.aspx", true);
            else if (ViewState["PENDINGNEEDSYN"].ToString() == "0" && ViewState["VESSELYN"].ToString() == "0" && ViewState["OVERRIDEYN"].ToString() == "0")
                Response.Redirect("CrewOffshoreCompletedTrainingNeed.aspx", true);
            else if (ViewState["PENDINGNEEDSYN"].ToString() == "0" && ViewState["VESSELYN"].ToString() == "1" && ViewState["OVERRIDEYN"].ToString() == "0")
                Response.Redirect("CrewOffshoreCompletedTrainingNeedVessel.aspx", true);
            else if (ViewState["OVERRIDEYN"].ToString() == "1")
                Response.Redirect("CrewOffshoreOverrideTrainingNeed.aspx", true);
            //else if (ViewState["OVERRIDEYN"].ToString() == "1")
            //    Response.Redirect("CrewOffshoreOverrideTrainingNeed.aspx", true);

        }
        else if (dce.CommandName.ToUpper().Equals("CLEAR"))
        {
            Clear();
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;

public partial class Dashboard_DashboardVesselSynchronizationStatus : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
        //{
        //    toolbar.AddButton("Alerts", "ALERTS");
        //    toolbar.AddButton("Dashboard", "NEW");
        //    toolbar.AddButton("Sync Status", "OLD");
        //    //MenuVesselList.AccessRights = this.ViewState;
        //    MenuVesselList.MenuList = toolbar.Show();
        //    MenuVesselList.SelectedMenuIndex = 2;
        //}
        //else
        //{
        //    toolbar.AddButton("Vessel Overview", "PARTICULARS");
        //    toolbar.AddButton("Crew List", "CREWLIST");
        //    toolbar.AddButton("Certificates", "CERTIFICATES");
        //    toolbar.AddButton("Admin", "ADMIN");
        //    toolbar.AddButton("Office Admin", "OFFICE");
        //    toolbar.AddButton("Manning", "MANNING");
        //    toolbar.AddButton("Travel", "TRAVEL");
        //    toolbar.AddButton("Attachments", "ATTACHMENTS");
        //    toolbar.AddButton("Summary", "SUMMARY");
        //    toolbar.AddButton("Sync Status", "OLD");
        //    toolbar.AddButton("Alerts", "ALERTS");
        //    //MenuVesselList.AccessRights = this.ViewState;
        //    MenuVesselList.MenuList = toolbar.Show();
        //    MenuVesselList.SelectedMenuIndex = 9;
        //}

        BindToolBar();

        BindData();
    }

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            int index = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                toolbar.AddButton(dr["FLDURLDESCRIPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='OLD'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuVesselList.MenuList = toolbar.Show();
            MenuVesselList.SelectedMenuIndex = index;
        }
    }

    public void BindData()
    {
        DataSet ds = new DataSet();

        List<SqlParameter> ParameterList = new List<SqlParameter>();

        ds = DataAccess.ExecSPReturnDataSet("PRALERTDATASYNCHRONIZERVESSELS", ParameterList);

        gvVesselSynchronizationStatus.DataSource = ds;
        gvVesselSynchronizationStatus.DataBind();

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";
    }

    protected void MenuVesselList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    Response.Redirect("../Dashboard/DashboardOffice.aspx", true);
                }
                else
                {
                    Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
                }
            }

            if (dce.CommandName.ToUpper().Equals("OLD"))
            {
                Response.Redirect("../Dashboard/DashboardPhoenix.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("ALERTS"))
            {
                Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
            }

            if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
            {
                Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("CREWLIST"))
            {
                Response.Redirect("../Dashboard/DashboardVesselCrewList.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
            {
                Response.Redirect("../Dashboard/DashboardVesselCertificates.aspx", true);
            }
            if (dce.CommandName.ToUpper().Equals("ADMIN"))
            {
                Response.Redirect("../Dashboard/DashboardAdmin.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("OFFICE"))
            {
                Response.Redirect("../Dashboard/DashboardOfficeAdmin.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("MANNING"))
            {
                Response.Redirect("../Dashboard/DashboardManning.aspx", true);
            }

            if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
            {
                Response.Redirect("../Dashboard/DashboardAttachments.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS, true);
            }

            if (dce.CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("../Dashboard/DashboardTravelSingOnOffList.aspx", true);
            }
            if (dce.CommandName.ToUpper().Equals("SUMMARY"))
            {
                Response.Redirect("../Dashboard/DashboardSummary.aspx", true);
            }
            if (dce.CommandName.ToUpper().Equals("OLD"))
            {
                Response.Redirect("../Dashboard/DashboardVesselSynchronizationStatus.aspx", false);
            }

            if (dce.CommandName.ToUpper().Equals("ALERTS"))
            {
                Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

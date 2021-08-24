using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;

public partial class Dashboard_DashboardSummary : PhoenixBasePage
{
    int vesselid;

    protected void Page_Load(object sender, EventArgs e)
    {
        //PhoenixToolbar toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddButton("Particulars", "PARTICULARS");
        //toolbarmain.AddButton("Crew List", "CREWLIST");
        //toolbarmain.AddButton("Certificates", "CERTIFICATES");
        //toolbarmain.AddButton("Admin", "ADMIN");
        //toolbarmain.AddButton("Office Admin", "OFFICE");
        //toolbarmain.AddButton("Manning", "MANNING");
        //toolbarmain.AddButton("Travel", "TRAVEL");
        //toolbarmain.AddButton("Attachments", "ATTACHMENTS");
        //toolbarmain.AddButton("Summary", "SUMMARY");
        //toolbarmain.AddButton("Sync Status", "OLD");
        //toolbarmain.AddButton("Alerts", "ALERTS");

        if (Request.QueryString["launchedfrom"] != null)
        {
            Attachment.ShowMenu = "false";
        }

        if (Request.QueryString["vesselid"] != null)
        {
            ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
            vesselid = int.Parse(ViewState["vesselid"].ToString());
        }
        else
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            BindToolBar();

            //MenuDdashboradVesselParticulrs.AccessRights = this.ViewState;
            //MenuDdashboradVesselParticulrs.MenuList = toolbarmain.Show();
            //MenuDdashboradVesselParticulrs.SelectedMenuIndex = 8;
        }

        BindData();
    }

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , vesselid
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

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='SUMMARY'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuDdashboradVesselParticulrs.MenuList = toolbar.Show();
            MenuDdashboradVesselParticulrs.SelectedMenuIndex = index;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardSummaryAcrossModuleEdit(vesselid);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    //gvAccounts.DataSource = ds.Tables[0];
        //    //gvAccounts.DataBind();
        //}
        //else
        //{
        //    //DataTable dt = ds.Tables[0];
        //    //ShowNoRecordsFound(dt, gvAccounts);
        //}

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPlannedMaintenance.DataSource = ds.Tables[0];
            gvPlannedMaintenance.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPlannedMaintenance);
        }

        if (ds.Tables[1].Rows.Count > 0)
        {
            gvAccountingSummary.DataSource = ds.Tables[1];
            gvAccountingSummary.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[1];
            ShowNoRecordsFound(dt, gvAccountingSummary);
        }

        //if (ds.Tables[2].Rows.Count > 0)
        //{
        //    //gvPurchase.DataSource = ds.Tables[2];
        //    //gvPurchase.DataBind();
        //}
        //else
        //{
        //    //DataTable dt = ds.Tables[2];
        //    //ShowNoRecordsFound(dt, gvPurchase);
        //}

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(vesselid);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void MenuDdashboradVesselParticulrs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

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
        if (dce.CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../Dashboard/DashboardManning.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("OFFICE"))
        {
            Response.Redirect("../Dashboard/DashboardOfficeAdmin.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Dashboard/DashboardAttachments.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS, true);
        }
        if (dce.CommandName.ToUpper().Equals("TRAVEL"))
        {
            Response.Redirect("../Dashboard/DashboardTravelSingOnOffList.aspx", true);
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
}

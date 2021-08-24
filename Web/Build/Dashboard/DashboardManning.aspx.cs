using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;

public partial class Dashboard_DashboardManning : PhoenixBasePage
{
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
        //MenuRegistersManningScale.MenuList = toolbarmain.Show();
        //MenuRegistersManningScale.SelectedMenuIndex = 5;

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

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='MANNING'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuRegistersManningScale.MenuList = toolbar.Show();
            MenuRegistersManningScale.SelectedMenuIndex = index;
        }
    }

    protected void RegistersManningScale_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardVesselManningScaleList(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvManningScale.DataSource = ds;
            gvManningScale.DataBind();

            DataRow dr = ds.Tables[0].Rows[0];

            txtOwnerScaleTotal.Text = dr["FLDOWNERSCALETOTAL"].ToString();
            txtSafeScaleTotal.Text = dr["FLDSAFESCALETOTAL"].ToString();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvManningScale);
        }

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);


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
}

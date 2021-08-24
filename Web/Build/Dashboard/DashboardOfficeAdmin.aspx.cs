using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;

public partial class Dashboard_DashboardOfficeAdmin : PhoenixBasePage
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
        ////MenuDdashboradVesselParticulrs.AccessRights = this.ViewState;
        //MenuVesselOfficeAdmin.MenuList = toolbarmain.Show();
        //MenuVesselOfficeAdmin.SelectedMenuIndex = 4;

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

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='OFFICE'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuVesselOfficeAdmin.MenuList = toolbar.Show();
            MenuVesselOfficeAdmin.SelectedMenuIndex = index;
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixCommonDashboard.DashboardOfficeAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            
            txtAccountsExecName.Text = ds.Tables[0].Rows[0]["FLDACCOUNTSEXECUTIVENAME"].ToString();
            txtAccountsExecDesignation.Text = ds.Tables[0].Rows[0]["FLDACCOUNTSEXECUTIVEDESIGNATION"].ToString();
            txtAccountsExecEmail.Text = ds.Tables[0].Rows[0]["FLDACCOUNTSEXECUTIVEEMAIL"].ToString();
            
            txtPurchaseSupdtName.Text = ds.Tables[0].Rows[0]["FLDPURCHASESUPDTNAME"].ToString();
            txtPurchaseSupdtDesignation.Text = ds.Tables[0].Rows[0]["FLDPURCHASESUPDTDESIGNATION"].ToString();
            txtPurchaseSupdtEmail.Text = ds.Tables[0].Rows[0]["FLDPURCHASESUPDTEMAIL"].ToString();
            
            txtQualitySupdtName.Text = ds.Tables[0].Rows[0]["FLDQUALITYSUPDTNAME"].ToString();
            txtQualitySupdtDesignation.Text = ds.Tables[0].Rows[0]["FLDQUALITYSUPDTDESIGNATION"].ToString();
            txtQualitySupdtEmail.Text = ds.Tables[0].Rows[0]["FLDQUALITYSUPDTEMAIL"].ToString();
            
            txtQualityDirectorName.Text = ds.Tables[0].Rows[0]["FLDQUALITYDIRECTORNAME"].ToString();
            txtQualityDirectorDesignation.Text = ds.Tables[0].Rows[0]["FLDQUALITYDIRECTORDESIGNATION"].ToString();
            txtQualityDirectorEmail.Text = ds.Tables[0].Rows[0]["FLDQUALITYDIRECTOREMAIL"].ToString();

            txtTravelPIC2Name.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC2NAME"].ToString();
            txtTravelPIC2Designation.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC2DESIGNATION"].ToString();
            txtTravelPIC2Email.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC2EMAIL"].ToString();

            txtTravelPIC3Name.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC3NAME"].ToString();
            txtTravelPIC3Designation.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC3DESIGNATION"].ToString();
            txtTravelPIC3Email.Text = ds.Tables[0].Rows[0]["FLDTRAVELPIC3EMAIL"].ToString();

            if (ds.Tables[0].Rows[0]["FLDUSESUPPLIERCONFIGYN"].ToString() == "0")
                chkSupplierConfig.Checked = false;
            else
                chkSupplierConfig.Checked = true;
        }

        DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (dsedit.Tables[0].Rows.Count > 0)
            ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
        else
            ViewState["FLDDTKEY"] = "";

    }

    protected void MenuVesselOfficeAdmin_TabStripCommand(object sender, EventArgs e)
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
}

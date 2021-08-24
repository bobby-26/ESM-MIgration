using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportRHRecordNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Crew List", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportRHRecord.AccessRights = this.ViewState;
            MenuReportRHRecord.MenuList = toolbar.Show();
            MenuReportRHRecord.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                if (Request.QueryString.Count > 0)
                {
                    ddlVessel.SelectedVessel = "33";
                    ddlVessel.bind();
                    ddlVessel.Enabled = true;
                }
                else
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.bind();
                    ddlVessel.Enabled = false;
                }

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlYear.DataBind();
    }
    protected void BindData()
    {
        ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsReportRHNonComplianceListNew.aspx?vesselid="
                            + ddlVessel.SelectedVessel
                            + "&month=" + ddlMonth.SelectedValue
                            + "&year=" + ddlYear.SelectedItem.Text
                            + "&Report=RESTHOURSRECORDNEW";
    }

    protected void MenuReportRHRecord_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsReportRHNonComplianceListNew.aspx?vesselid="
                    + ddlVessel.SelectedVessel.ToString()
                    + "&month=" + ddlMonth.SelectedValue
                    + "&year=" + ddlYear.SelectedItem.Text
                    + "&Report=RESTHOURSRECORDNEW";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ddlYear.Items.Sort();
    }
}
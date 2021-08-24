using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsMissingVesselSignOff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Crew List", "SIGNONOFFLIST");
            toolbar.AddButton("Missing SignOff", "MISSINGSIGNOFF");
            MenuCrewAdmin.AccessRights = this.ViewState;
            MenuCrewAdmin.MenuList = toolbar.Show();
            MenuCrewAdmin.SelectedMenuIndex = 1;
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSignOff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindDataSignOff()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = PhoenixVesselAccountsCorrections.SearchVesselSignOffMissingData(PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection,
                                                               Convert.ToInt16(ViewState["PAGENUMBER"]), gvSignOff.PageSize, ref iRowCount, ref iTotalPageCount);
            gvSignOff.DataSource = ds;
            gvSignOff.VirtualItemCount = ds.Tables[0].Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewAdmin_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SIGNONOFFLIST"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsAdminSignOnOffList.aspx";
            }
            else if (CommandName.ToUpper().Equals("MISSINGSIGNOFF"))
            {
                ViewState["CURRENTTAB"] = "../VesselAccounts/VesselAccountsMissingVesselSignOff.aspx";
            }
            Response.Redirect(ViewState["CURRENTTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSignOff_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
    }
    protected void Rebind()
    {
        gvSignOff.SelectedIndexes.Clear();
        gvSignOff.EditIndexes.Clear();
        gvSignOff.DataSource = null;
        gvSignOff.Rebind();
    }
    protected void gvSignOff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
          
            BindDataSignOff();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignOff_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                int signonoffid = int.Parse(((RadLabel)e.Item.FindControl("lblsignonoffid")).Text);
                string Employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                string SignonVesselid = ((RadLabel)e.Item.FindControl("lblSignonVesselid")).Text;
                PhoenixVesselAccountsCorrections.InsertVesselSignOffMissingData(int.Parse(SignonVesselid), int.Parse(Employeeid), signonoffid);
                Rebind();
                Ucsstats.Text = "Successfully Saved.";
                Ucsstats.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSignOff_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}

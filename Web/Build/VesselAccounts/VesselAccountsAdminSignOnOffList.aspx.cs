using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class VesselAccountsAdminSignOnOffList : PhoenixBasePage
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
            MenuCrewAdmin.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ddlStatus.SelectedValue = "1";
                gvCrewList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                vessellist();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ddlVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                else
                {
                    ddlVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
            }

            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("Style", "display:none;");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void vessellist()
    {
        ddlVessel.DataSource = PhoenixRegistersVessel.ListAllVessel();
        ddlVessel.DataBind();
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
    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsCorrections.ListVesselAdminSignOnOff(
                int.Parse(ddlVessel.SelectedValue)
                , byte.Parse(ddlStatus.SelectedValue));
            gvCrewList.DataSource = dt;
            gvCrewList.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewList.SelectedIndexes.Clear();
        gvCrewList.EditIndexes.Clear();
        gvCrewList.DataSource = null;
        gvCrewList.Rebind();
    }
    protected void gvCrewList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("RESEND"))
            {

                string signonoffid = ((RadLabel)e.Item.FindControl("lblSignonoffid")).Text;
                string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeid")).Text;
                PhoenixVesselAccountsCorrections.ResendCrew(int.Parse(signonoffid), int.Parse(ddlVessel.SelectedValue));
                PhoenixVesselAccountsCorrections.ResendEmployeeData(int.Parse(employeeid), int.Parse(ddlVessel.SelectedValue));
                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void gvCrewList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                ed.Attributes.Add("onclick", "javascript:openNewWindow('signonoff','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsAdminSignOnOffEdit.aspx?SIGNONOFFID=" + drv["FLDSIGNONOFFID"].ToString() +"&vesselid="+ddlVessel.SelectedValue + "');return false;");
            }
            LinkButton lnkbtn = (LinkButton)e.Item.FindControl("lnkEployeeName");
            if (lnkbtn != null)
            {
                lnkbtn.Enabled = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                lnkbtn.Attributes.Add("onclick", "javascript:openNewWindow('signonoff','','" + Session["sitepath"] + "/VesselAccounts/VesselAccountsAdminSignOnOffEdit.aspx?SIGNONOFFID=" + drv["FLDSIGNONOFFID"].ToString() + "&vesselid=" + ddlVessel.SelectedValue + "');return false;");
            }
            RadLabel EmpId = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel SignOffDate = (RadLabel)e.Item.FindControl("lblSignOffDate");
            LinkButton re = (LinkButton)e.Item.FindControl("cmdResend");
            if (re != null)
            {
                re.Visible = SessionUtil.CanAccess(this.ViewState, re.CommandName);
                re.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want Resend ?')");
            }
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
    protected void ddlCrewList_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }

}

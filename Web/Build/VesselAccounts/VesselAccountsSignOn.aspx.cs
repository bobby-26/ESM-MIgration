using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsSignOn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsSignOn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewSignOn.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsSignOn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSignOff')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            CrewSignOff.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvSignOff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void CrewSignOn_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign On", "Sign On Port" };
                DataTable dt = PhoenixVesselAccountsEmployee.ListVesselSignOn(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                General.ShowExcel("Sign On", dt, alColumns, alCaptions, null, string.Empty);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewSignOff_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFSEAPORTNAME", "FLDSIGNOFFREASON" };
                string[] alCaptions = { "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign On", "Relief Due", "Sign Off", "Sign Off Port", "Sign Off Reason" };
                DataTable dt = PhoenixVesselAccountsEmployee.ListVesselSignOff(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                General.ShowExcel("Sign Off", dt, alColumns, alCaptions, null, string.Empty);
            }
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
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDSIGNONSEAPORTNAME" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign On", "Sign On Port" };
            DataTable dt = PhoenixVesselAccountsEmployee.ListVesselSignOn(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Sign On", alCaptions, alColumns, ds);
            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = dt.Rows.Count;
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
            string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDSIGNONRANKNAME", "FLDNATIONALITYNAME", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDSIGNOFFDATE", "FLDSIGNOFFSEAPORTNAME", "FLDSIGNOFFREASON" };
            string[] alCaptions = { "File No.", "Name", "Rank", "Nationality", "Passport", "CDC No.", "Sign On", "Relief Due", "Sign Off", "Sign Off Port", "Sign Off Reason" };
            DataTable dt = PhoenixVesselAccountsEmployee.ListVesselSignOff(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvSignOff", "Sign Off", alCaptions, alColumns, ds);
            gvSignOff.DataSource = dt;
            gvSignOff.VirtualItemCount = dt.Rows.Count;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrewSearch.SelectedIndexes.Clear();
        gvCrewSearch.EditIndexes.Clear();
        gvCrewSearch.DataSource = null;
        gvCrewSearch.Rebind();
        gvSignOff.SelectedIndexes.Clear();
        gvSignOff.EditIndexes.Clear();
        gvSignOff.DataSource = null;
        gvSignOff.Rebind();
    }
    protected void gvCrewSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvCrewSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridItem)
        {
            LinkButton de = (LinkButton)e.Item.FindControl("cmdDelete");
            if (de != null && PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0) de.Visible = false;
            else if (de != null)
            {
                de.Visible = SessionUtil.CanAccess(this.ViewState, de.CommandName);
                de.Attributes.Add("onclick", "return fnConfirmDelete();");
            }

            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            LinkButton db = (LinkButton)e.Item.FindControl("cmdApprove");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm sign-on ?')");
            }
            LinkButton con = (LinkButton)e.Item.FindControl("cmdContract");
            if (con != null)
            {
                con.Visible = SessionUtil.CanAccess(this.ViewState, con.CommandName);
                con.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsReportContract.aspx?EmpId=" + drv["FLDEMPLOYEEID"].ToString() + "&SignonoffId=" + drv["FLDSIGNONOFFID"].ToString() + "');return false;");
            }
            UserControlSeaport ddlSeaPort = (UserControlSeaport)e.Item.FindControl("ddlSeaPort");
            if (ddlSeaPort != null) ddlSeaPort.SelectedSeaport = drv["FLDSIGNONSEAPORTID"].ToString();
        }

    }
    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string signonid = ((RadLabel)e.Item.FindControl("lblSignOnOffIdAdd")).Text;
                string signondate = ((RadLabel)e.Item.FindControl("lblSignOnDate")).Text;
                string port = ((RadLabel)e.Item.FindControl("lblSeaPortId")).Text;
                if (!IsValidSignOn(signondate, port))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEmployee.UpdateVesselSignOn(int.Parse(signonid), DateTime.Parse(signondate), int.Parse(port), 1);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string signonid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                string signondate = ((UserControlDate)e.Item.FindControl("txtSignOnDate")).Text;
                string port = ((UserControlSeaport)e.Item.FindControl("ddlSeaPort")).SelectedSeaport;
                if (!IsValidSignOn(signondate, port))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEmployee.UpdateVesselSignOn(int.Parse(signonid), DateTime.Parse(signondate), int.Parse(port), null);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string ids = ((RadLabel)e.Item.FindControl("lblSignonoffid")).Text.Trim();
                int id = int.Parse(ids);
                PhoenixVesselAccountsEmployee.DeleteVesselSignOn(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSignOff_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                string signonoffid = ((RadLabel)e.Item.FindControl("lblSignonoffid")).Text;
                int signonid = int.Parse(signonoffid);
                string signoffdate = ((RadLabel)e.Item.FindControl("lblSignOffDate")).Text;
                string port = ((RadLabel)e.Item.FindControl("lblSeaPort")).Text;
                string reason = ((RadLabel)e.Item.FindControl("lblReason")).Text;
                string remarks = ((RadLabel)e.Item.FindControl("lblSignOffRemarks")).Text;
                string paidwages = ((RadLabel)e.Item.FindControl("lblCancelWagesEdit")).Text;
                string negbal = ((RadLabel)e.Item.FindControl("lblrecovertooffice")).Text;
                if (!IsValidSignOff(signoffdate, port, reason))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixVesselAccountsEmployee.UpdateVesselSignOff(signonid, DateTime.Parse(signoffdate), int.Parse(port), int.Parse(reason), remarks, paidwages.TrimEnd(','), 0, byte.Parse(negbal));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSignOff_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkEployeeName");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onclick", "openNewWindow('codehelp1', '', \"" + Session["sitepath"] + "/VesselAccounts/VesselAccountsSignOffDetails.aspx?SIGNONID=" + drv["FLDSIGNONOFFID"].ToString() + "&EMPID=" + drv["FLDEMPLOYEEID"].ToString() + "&EMPNAME=" + drv["FLDNAME"].ToString() + "\");return true;");
                lbtn.Enabled = SessionUtil.CanAccess(this.ViewState, lbtn.CommandName);
            }
            if (ed != null)
            {

                ed.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/VesselAccounts/VesselAccountsSignOffDetails.aspx?SIGNONID=" + drv["FLDSIGNONOFFID"].ToString() + "&EMPID=" + drv["FLDEMPLOYEEID"].ToString() + "&EMPNAME=" + drv["FLDNAME"].ToString() + "');return true;");
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdApprove");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm sign-off ?')");
            }
            RadLabel EmpId = (RadLabel)e.Item.FindControl("lblEmployeeid");
            LinkButton con = (LinkButton)e.Item.FindControl("cmdGenReport");
            RadLabel SignOffDate = (RadLabel)e.Item.FindControl("lblSignOffDate");

            if (con != null) con.Visible = SessionUtil.CanAccess(this.ViewState, con.CommandName);
            if (SignOffDate != null && con != null)
            {
                if (!string.IsNullOrEmpty(EmpId.Text) && !string.IsNullOrEmpty(SignOffDate.Text))
                {
                    con.Attributes.Add("onclick", "parent.openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=7&showexcel=no&showword=no&reportcode=BOW&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "&employee=" + EmpId.Text + "&date=" + SignOffDate.Text + "');return false;");
                }
                else con.Visible = false;
            }
            if (drv["FLDAPPRAISALREQUIREDYN"].ToString() == "1" && con != null)
                con.Visible = false;


        }

    }
 
    private bool IsValidSignOn(string date, string SeaPort)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign On Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Sign On Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign On Port is required.";
        }

        return (!ucError.IsError);
    }
    private bool IsValidSignOff(string date, string SeaPort, string reason)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Today.AddDays(1)) > 0)
        {
            ucError.ErrorMessage = "Sign Off Date should be earlier than current date +1 day";
        }
        if (!General.GetNullableInteger(SeaPort).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Port is required.";
        }
        if (!General.GetNullableInteger(reason).HasValue)
        {
            ucError.ErrorMessage = "Sign Off Reason is required.";
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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

}

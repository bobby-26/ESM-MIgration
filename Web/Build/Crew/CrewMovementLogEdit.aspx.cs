using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class Crew_CrewMovementLogEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);        

        MenuMovementLog.AccessRights = this.ViewState;
        MenuMovementLog.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            string mlid = Request.QueryString["mlid"];
            ViewState["ID"] = string.IsNullOrEmpty(mlid) ? "" : mlid;
            string empid = Request.QueryString["empid"];
            ViewState["EID"] = string.IsNullOrEmpty(empid) ? "" : empid;
            Edit();
        }
    }
    protected void MenuMovementLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string VesselId = ddlVessel.SelectedVessel;
                string MovementId = ddlMovement.SelectedMovement;
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                Guid? MovementLogId = null;
                if (ViewState["ID"].ToString() != string.Empty)
                {
                    MovementLogId = Guid.Parse(ViewState["ID"].ToString());
                }
                if(!IsValidLog(VesselId, MovementId, fromDate, toDate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewMovementLog.Insert(int.Parse(VesselId), int.Parse(ViewState["EID"].ToString())
                    , int.Parse(MovementId), DateTime.Parse(fromDate), DateTime.Parse(toDate), ref MovementLogId, General.GetNullableInteger(ddlUnion.SelectedAddress));
            }            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Edit()
    {
        if (ViewState["ID"].ToString().Equals(string.Empty)) return;

        DataTable dt = PhoenixCrewMovementLog.Edit(new Guid(ViewState["ID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ddlMovement.SelectedMovement = dr["FLDMOVEMENTID"].ToString();
            txtFromDate.Text = dr["FLDFROMDATE"].ToString();
            txtToDate.Text = dr["FLDTODATE"].ToString();
            ddlUnion.SelectedAddress = dr["FLDUNIONID"].ToString();
            txtAmount.Text = dr["FLDCURRENCYCODE"].ToString() + " " + dr["FLDTOTALAMOUNT"].ToString();
        }
    }
    private bool IsValidLog(string VesselId, string MovementId, string FromDate, string ToDate)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (General.GetNullableInteger(VesselId) == null)
            ucError.ErrorMessage = "Vessel is required";
        if (General.GetNullableInteger(MovementId) == null)
            ucError.ErrorMessage = "Movement is required";
        if (!General.GetNullableDateTime(FromDate).HasValue)
            ucError.ErrorMessage = "From is required.";

        if (!General.GetNullableDateTime(ToDate).HasValue)
            ucError.ErrorMessage = "To is required.";
        else if (General.GetNullableDateTime(FromDate).HasValue && DateTime.TryParse(ToDate, out resultDate) 
            && DateTime.Compare(DateTime.Parse(FromDate), resultDate) > 0)
        {
            ucError.ErrorMessage = "To should be later than From";
        }
        return (!ucError.IsError);
    }
}
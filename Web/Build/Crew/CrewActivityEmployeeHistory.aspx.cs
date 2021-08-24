using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewEmployeeActivity;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewActivityEmployeeHistory : PhoenixBasePage
{
    private string strEmployeeId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                strEmployeeId = Filter.CurrentCrewSelection;
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                SetEmployeePrimaryDetails();
                gvEmployeeHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewEmployeeActivity.SearchEmployeeActivity(int.Parse(strEmployeeId),
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvEmployeeHistory.PageSize,
                ref iRowCount,
                ref iTotalPageCount);
            gvEmployeeHistory.DataSource = ds;
            DataTable dt = ds.Tables[0];
            gvEmployeeHistory.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEmployeeHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvEmployeeHistory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(strEmployeeId));

            if (dt.Rows.Count > 0)
            {
                txtFileNo.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtPayRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtEmployeeName.Text = dt.Rows[0]["FLDLASTNAME"].ToString() + " " + dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["VESSELID"] = dt.Rows[0]["FLDPRESENTVESSELID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvEmployeeHistory.SelectedIndexes.Clear();
        gvEmployeeHistory.EditIndexes.Clear();
        gvEmployeeHistory.DataSource = null;
        gvEmployeeHistory.Rebind();
    }
    protected void gvEmployeeHistory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                int Rank = int.Parse(((UserControlRank)e.Item.FindControl("ucRankEdit")).SelectedRank);
                string fromdate = ((UserControlDate)e.Item.FindControl("ucFromdateEdit")).Text;
                string todate = ((UserControlDate)e.Item.FindControl("ucTodateEdit")).Text;
                Guid Dtkey = new Guid(((RadLabel)e.Item.FindControl("lblDtkey")).Text);

                try
                {
                    PhoenixCrewEmployeeActivity.UpdateEmployeeActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                       int.Parse(strEmployeeId),
                                                                       Rank,
                                                                       int.Parse(((UserControlVessel)e.Item.FindControl("ucVesselEdit")).SelectedVessel),
                                                                       DateTime.Parse(fromdate),
                                                                       General.GetNullableDateTime(todate),
                                                                       int.Parse(((UserControlEmployeeActivity)e.Item.FindControl("ucActivityEdit")).SelectedEmpActivity),
                                                                       int.Parse(((UserControlEmployeeActivityReason)e.Item.FindControl("ucActivityReasonEdit")).SelectedEmpActivityReason),
                                                                       Dtkey
                                                                       );

                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
                Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string fromdate = ((RadLabel)e.Item.FindControl("lblFromDate")).Text;
                Guid Dtkey = new Guid(((RadLabel)e.Item.FindControl("lblDtkey")).Text);

                PhoenixCrewEmployeeActivity.DeleteEmployeeActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , int.Parse(strEmployeeId)
                                                                   , DateTime.Parse(fromdate)
                                                                   , Dtkey);
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
    protected void gvEmployeeHistory_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            UserControlRank ra = (UserControlRank)e.Item.FindControl("ucRankEdit");
            if (ra != null) ra.SelectedRank = drv["FLDRANKID"].ToString();

            UserControlEmployeeActivity ea = (UserControlEmployeeActivity)e.Item.FindControl("ucActivityEdit");
            if (ea != null) ea.SelectedEmpActivity = drv["FLDACTIVITY"].ToString();

            UserControlVessel v = (UserControlVessel)e.Item.FindControl("ucVesselEdit");
            if (v != null) v.SelectedVessel = drv["FLDVESSELID"].ToString();

            UserControlEmployeeActivityReason ar = (UserControlEmployeeActivityReason)e.Item.FindControl("ucActivityReasonEdit");
            //ar.CssClass = "Input_mandatory";
            if (ar != null) ar.SelectedEmpActivityReason = drv["FLDACTIVITYREASONID"].ToString();
        }
    }
  
}

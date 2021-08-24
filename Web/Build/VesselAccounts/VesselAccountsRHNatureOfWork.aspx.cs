using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;
using Telerik.Web.UI;
public partial class VesselAccountsRHNatureOfWork : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuRHGeneral.AccessRights = this.ViewState;
                MenuRHGeneral.MenuList = toolbar.Show();

                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["CALENDERID"] = null;

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["CalenderId"] != null)
                    ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();

                if (Request.QueryString["RHStartId"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RHStartId"].ToString();

                if (ViewState["CALENDERID"] != null)
                    BindDetails();
                bindChechBox();
                SetDetails();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void bindChechBox()
    {
        try
        {
            chknatureofwork.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 84);
            chknatureofwork.DataBindings.DataTextField = "FLDQUICKNAME";
            chknatureofwork.DataBindings.DataValueField = "FLDQUICKCODE";
            chknatureofwork.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RHGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string reason = string.Empty;
                string caction = string.Empty;
                string natureofwork = string.Empty;
                string officereason = string.Empty;
                foreach (ButtonListItem li in chknatureofwork.Items)
                {
                    natureofwork += (li.Selected ? li.Value + "," : string.Empty);
                }
                natureofwork.TrimEnd(',');

                WorkCalenderRemarksSave(natureofwork);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool ValidReason(string reason, string caction, string natureofwork)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(natureofwork))
            ucError.ErrorMessage = "Nature of Work is required.";

        return (!ucError.IsError);
    }
    private void BindDetails()
    {

        DataSet ds = PhoenixVesselAccountsRH.RHWorkCalenderEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtEmpName.Text = dr["FLDNAME"].ToString();
            ucRank.SelectedRank = dr["FLDRANKID"].ToString();
            txtDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDDATE"].ToString());
            txtHour.Text = dr["FLDHOURS"].ToString();
            txtReportingday.Text = dr["FLDREPORTINGDAY"].ToString();

            rbnhourchange.SelectedValue = dr["FLDADVANCERETARD"].ToString();
            if (dr["FLDHALFHOURYN"].ToString() != "0" && dr["FLDHALFHOURYN"].ToString() != "")
                rbnhourvalue.SelectedValue = dr["FLDHALFHOURYN"].ToString();
            rbtnadvanceretard.SelectedValue = dr["FLDCLOCK"].ToString();
            rbtnworkplace.SelectedValue = dr["FLDWORKPLACE"].ToString();
        }
    }
    protected void WorkCalenderRemarksSave(string natureofwork)
    {
        try
        {
            PhoenixVesselAccountsRH.UpdateRestHourNatureOfWork(General.GetNullableGuid(ViewState["CALENDERID"].ToString()),
                                                                    General.GetNullableString(natureofwork));
            SetDetails();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('nature', 'code1', null);", true);
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    protected void SetDetails()
    {
        DataSet ds = PhoenixVesselAccountsRH.RHWorkCalenderEdit(new Guid(ViewState["CALENDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            string natureofwork = dr["FLDNATUREOFWORK"].ToString();

            if (!string.IsNullOrEmpty(natureofwork))
            {
                foreach (string val in natureofwork.Split(','))
                {
                    if (val.Trim() != "")
                    {
                        chknatureofwork.SelectedValue = val;
                    }
                }
            }
        }
    }
}

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class CrewNewApplicantInActive : PhoenixBasePage
{    
    string strEmployeeId = string.Empty;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();            
            toolbar.AddButton("Save", "SAVE");
            MenuInActive.AccessRights = this.ViewState;
            MenuInActive.MenuList = toolbar.Show();

            if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = string.Empty;
            else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                strEmployeeId = Request.QueryString["empid"];
            else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
                strEmployeeId = Filter.CurrentNewApplicantSelection;

            if (!IsPostBack)
            {
                if (General.GetNullableInteger(strEmployeeId) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }

                SetEmployeePrimaryDetails();
                ViewState["ACTIVEID"] = string.Empty;
                CrewInActiveEdit();
                ddlReason_TextChanged(null, null);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //private void BindData()
    //{
    //    DataSet ds;
    //    ds = PhoenixCrewActive.CrewInActiveList(General.GetNullableInteger(strEmployeeId));

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        gvCrewInActive.DataSource = ds;
    //        gvCrewInActive.DataBind();
    //    }
    //    else
    //    {
    //        DataTable dt = ds.Tables[0];
    //        ShowNoRecordsFound(dt, gvCrewInActive);
    //    }
    //}
    
    protected void CrewInActive_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidActive())
                {
                    ucError.Visible = true;
                    return;

                }
                else
                {
                    PhoenixCrewActive.NewApplicantActiveInactiveInsert(General.GetNullableInteger(ViewState["ACTIVEID"].ToString()), General.GetNullableInteger(strEmployeeId),
                        General.GetNullableDateTime(txtInActiveDate.Text), txtInActiveRemarks.Text, int.Parse(ddlInactiveReason.SelectedHard), byte.Parse(rblInActive.SelectedValue));

                    ucStatus.Text = "Status Information Updated";
                    //BindData();
                    ViewState["ACTIVEID"] = string.Empty;
                    CrewInActiveEdit();

                }
            }
            else
            {
                 ViewState["ACTIVEID"] = 0;
               CrewInActiveEdit();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidActive()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;
        int resultInt;
        if (General.GetNullableInteger(strEmployeeId) == null)
        {
            ucError.ErrorMessage = "Select a Employee from Query Activity";

        }

        if (string.IsNullOrEmpty(txtInActiveDate.Text) && txtInActiveDate.CssClass == "input_mandatory")
            ucError.ErrorMessage = "Relief Due is required.";
        else if (!string.IsNullOrEmpty(txtInActiveDate.Text) 
            && DateTime.TryParse(txtInActiveDate.Text, out resultdate))
        {
            if (txtInActiveDate.CssClass == "input_mandatory" && DateTime.Compare(resultdate, DateTime.Now) < 0)
                ucError.ErrorMessage = "Relief Due should be later than current date";
            else if (txtInActiveDate.CssClass == "input" && DateTime.Compare(resultdate, DateTime.Now) > 0)
                ucError.ErrorMessage = "Date should be earlier than current date";
        }

        if (!int.TryParse(ddlInactiveReason.SelectedHard, out resultInt))
            ucError.ErrorMessage = "Reason is required.";

        return (!ucError.IsError);
    }

    private void CrewInActiveEdit()
    {
        DataTable dt;
        dt = PhoenixCrewActive.CrewNewApplicantActiveInactiveList(General.GetNullableInteger(strEmployeeId).Value);
        string status = string.Empty;
        if (dt.Rows.Count > 0)
        {
            status = dt.Rows[0]["FLDACTIVEINACTIVE"].ToString();
            rblInActive.SelectedValue = dt.Rows[0]["FLDACTIVEINACTIVE"].ToString();
            //txtInActiveDate.Text = General.GetDateTimeToString(dt.Rows[0][(status == "0" ? "FLDINACTIVEDATE" : "FLDACTIVEDATE")].ToString());
            ViewState["ACTIVEID"] = dt.Rows[0]["FLDACTIVEID"].ToString();
            txtInActiveRemarks.Text = dt.Rows[0][(status == "0" ? "FLDINACTIVEREMARKS" : "FLDACTIVEREMARKS")].ToString();
            ActiveRemarks(null, null);
            //ddlInactiveReason.SelectedHard = dt.Rows[0]["FLDREASONID"].ToString();
        }
        else
        {
            ActiveRemarks(null, null);
        }
    }
    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(strEmployeeId));
            if (dt.Rows.Count > 0)
            {              
                txtEmployeeFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtEmployeeMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtEmployeeLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ActiveRemarks(object sender, EventArgs e)
    {        
        if (rblInActive.SelectedValue == "1")
        {
            ddlInactiveReason.ShortNameFilter = "ONB,ONL";
            ddlInactiveReason.HardTypeCode = "54";
            ddlInactiveReason.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 54, 1, "ONB,ONL");
        }
        else
        {
            //ddlInactiveReason.ShortNameFilter = "DTH,MDL,NTB";
            ddlInactiveReason.HardTypeCode = "96";
            ddlInactiveReason.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                        , 96, 1, string.Empty);
        }
    }
    protected void ddlReason_TextChanged(object sender, EventArgs e)
    {
        if (rblInActive.SelectedValue == "1" && ((DropDownList)ddlInactiveReason.FindControl("ddlHard")).SelectedItem.Text.ToLower().Contains("onboard"))
        {
            lblDate.Text = "Relief Due";
            txtInActiveDate.CssClass = "input_mandatory";
        }
        else
        {
            lblDate.Text = "Date";
            txtInActiveDate.CssClass = "input";
        }
    }
}

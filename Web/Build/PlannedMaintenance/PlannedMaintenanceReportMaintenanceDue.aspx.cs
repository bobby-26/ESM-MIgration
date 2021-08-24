using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportMaintenanceDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuReportMaintenanceDue.AccessRights = this.ViewState;
            MenuReportMaintenanceDue.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ckPlaning.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.PLANNING));
                ckPlaning.DataBind();
                chkClasses.DataSource = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixQuickTypeCode.JOBCLASS));
                chkClasses.DataBind();
                chkStatus.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)(PhoenixHardTypeCode.WORKORDERSTATUS), 0, "REQ,CAN,POP,PND,ISS");
                chkStatus.DataBind();
                chkDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
                chkDiscipline.DataBind();
                ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
                ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
                ucMainCause.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCAUSE)).ToString();

                ViewState["VIEWCOMPONENTID"] = "";

                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["VIEWCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected bool IsValidDateRange(DateTime? FromDate, DateTime? ToDate)
    {
        if (FromDate == null || ToDate == null)
        {
            return true;
        }
        else if (FromDate > ToDate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void MenuReportMaintenanceDue_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string status = "";
                string planning = "";
                string jobclass = "";
                string discipline = "";
                foreach (ButtonListItem item in chkStatus.Items)
                {
                    if (item.Selected)
                        status = status + item.Value + ",";
                }
                foreach (ButtonListItem item in ckPlaning.Items)
                {
                    if (item.Selected)
                        planning = planning + item.Value + ",";
                }
                foreach (ButtonListItem item in chkClasses.Items)
                {
                    if (item.Selected)
                        jobclass = jobclass + item.Value + ",";
                }
                foreach (ButtonListItem item in chkDiscipline.Items)
                {
                    if (item.Selected)
                        discipline = discipline + item.Value + ",";
                }
                status = status.TrimEnd(new Char[] { ',' });

                if (IsValidDateRange(General.GetNullableDateTime(txtDateFrom.Text), General.GetNullableDateTime(txtDateTo.Text)))
                {
                    string prams = "";
                    string unexp = "";
                    if (chkUnexpected.Checked == true)
                        unexp = "1";
                    else
                        unexp = "";

                    string Compno = "";
                    if (txtComponentNumber.TextWithLiterals.Length > 2)
                    {
                        Compno = txtComponentNumber.TextWithLiterals;
                    }

                    prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    prams += "&workorderno=" + General.GetNullableString(txtWorkOrderNumber.Text);
                    prams += "&workordername=" + General.GetNullableString(txtTitle.Text);
                    prams += "&Compid=" + General.GetNullableGuid(ViewState["VIEWCOMPONENTID"].ToString());
                    prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.TrimEnd("000.00.00".ToCharArray()));
                    //prams += "&compno=" + General.GetNullableString(Compno.ToString());
                    prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                    prams += "&plan=" + General.GetNullableString(planning.TrimEnd(new Char[] { ',' }));
                    prams += "&jobclass=" + General.GetNullableString(jobclass.TrimEnd(new Char[] { ',' }));
                    prams += "&dtfrom=" + General.GetNullableDateTime(txtDateFrom.Text);
                    prams += "&dtto=" + General.GetNullableDateTime(txtDateTo.Text);
                    prams += "&maintype=" + General.GetNullableInteger(ucMainType.SelectedQuick);
                    prams += "&mainclass=" + General.GetNullableInteger(ucMaintClass.SelectedQuick);
                    prams += "&maincause=" + General.GetNullableInteger(ucMainCause.SelectedQuick);
                    prams += "&workunexp=" + unexp;
                    prams += "&status=" + General.GetNullableString(status.TrimEnd(new Char[] { ',' }));
                    prams += "&priority=" + General.GetNullableInteger(txtPriority.Text);
                    prams += "&rank=" + General.GetNullableString(discipline.TrimEnd(new Char[] { ',' }));
                    prams += "&classcode=" + General.GetNullableString(txtClassCode.Text);
                    prams += exceloptions();

                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=MAINTENANCEDUE" + prams);
                }
                else
                {
                    ucError.ErrorMessage = "From Date should not be greater than To Date";
                    ucError.Visible = true;
                }
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }


}

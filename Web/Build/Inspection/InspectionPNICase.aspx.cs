using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using SouthNests.Phoenix.Registers;

public partial class InspectionPNICase : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Medical Case", "MEDICAL");
            toolbar.AddButton("PNI Case", "PNI");
            MenuPNIGeneral.AccessRights = this.ViewState;
            MenuPNIGeneral.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");
            PNIListMain.AccessRights = this.ViewState;
            PNIListMain.MenuList = toolbar.Show();

            MenuPNIGeneral.SelectedMenuIndex = 1;
            ViewState["NEW"] = null;
            ViewState["PNICASEID"] = null;
            if (Request.QueryString["PNIId"] != null && Request.QueryString["PNIId"].ToString() != "")
                ViewState["PNIID"] = Request.QueryString["PNIId"].ToString();
            if (Request.QueryString["PNICASEID"] != null && !string.IsNullOrEmpty(Request.QueryString["PNICASEID"].ToString()))
                ViewState["PNICASEID"] = Request.QueryString["PNICASEID"].ToString();           

            EditPNI();            
        }
        lnkPNIChecklist.Visible = false;
        if (ViewState["PNICASEID"] != null && !string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
        {
            lnkPNIChecklist.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionPNIChecklist.aspx?&PNICASEID=" + ViewState["PNICASEID"].ToString() + "&DEPARTMENTID=1');return true;");
            lnkPNIChecklist.Visible = true;
        }
    }

    protected void EditPNI()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.EditInspectionPNI(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                txtMedicalCaseNo.Text = dr["FLDCASENUMBER"].ToString();
                txtPNIClub.Text = dr["FLDPNICLUBNAME"].ToString();
                txtPNIClubid.Text = dr["FLDPNICLUB"].ToString();
                txtDeductible.Text = dr["FLDCREWDEDUCTIBLE"].ToString();
                txtQualityincharge.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
                txtQualityinchargeid.Text = dr["FLDQUALITYIC"].ToString();
                txtReportedDate.Text = dr["FLDREPORTEDDATE"].ToString();
                ucESMOwner.SelectedHard = dr["FLDESMOROWNER"].ToString();
                txtRefNo.Text = dr["FLDPNIREFNO"].ToString();
                ViewState["PNICASEID"] = dr["FLDPNICASEID"].ToString();
                if (!string.IsNullOrEmpty(dr["FLDPNICASEID"].ToString()))
                    ViewState["NEW"] = 0;
                else
                    ViewState["NEW"] = 1;
                lnkPNIChecklist.Visible = false;
                if (ViewState["PNICASEID"] != null && !string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
                {
                    lnkPNIChecklist.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Inspection/InspectionPNIChecklist.aspx?&PNICASEID=" + ViewState["PNICASEID"].ToString() + "&DEPARTMENTID=1');return true;");
                    lnkPNIChecklist.Visible = true;
                }
            }
            if (ViewState["NEW"].ToString() == "0")
            {
                if(ViewState["PNICASEID"]!= null && !string.IsNullOrEmpty(ViewState["PNICASEID"].ToString()))
                {
                    ds = PhoenixInspectionPNI.EditPNICase(new Guid(ViewState["PNICASEID"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                        ucVessel.Enabled = false;
                        txtMedicalCaseNo.Text = dr["FLDMEDICALCASENO"].ToString();
                        txtPNIClub.Text = dr["FLDPNICLUBNAME"].ToString();
                        txtPNIClubid.Text = dr["FLDPNICLUB"].ToString();
                        txtDeductible.Text = dr["FLDCREWDEDUCTIBLE"].ToString();
                        txtQualityincharge.Text = dr["FLDQUALITYICNAME"].ToString();
                        txtQualityinchargeid.Text = dr["FLDQUALITYINCHARGE"].ToString();
                        txtReportedDate.Text = dr["FLDREPORTEDDATE"].ToString();
                        ucESMOwner.SelectedHard = dr["FLDESMOWNER"].ToString();
                        txtRefNo.Text = dr["FLDPNIREFNUMBER"].ToString();
                        ViewState["PNICASEID"] = dr["FLDPNICASEID"].ToString();
                    }
                }
            }
        }
    }
    protected void MenuPNIGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("MEDICAL"))
        {
            Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIId=" + ViewState["PNIID"], true);
        }
    }
    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["PNIID"] != null && !string.IsNullOrEmpty(ViewState["PNIID"].ToString()))
            {
                if (isValidPNICase())
                {
                    if (ViewState["NEW"].ToString() == "1")
                    {
                        PhoenixInspectionPNI.PNICaseInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            int.Parse(ucESMOwner.SelectedHard), int.Parse(ucVessel.SelectedVessel), General.GetNullableInteger(txtPNIClubid.Text),
                            General.GetNullableDecimal(txtDeductible.Text), DateTime.Parse(txtReportedDate.Text),
                            General.GetNullableInteger(txtQualityinchargeid.Text),
                            new Guid(ViewState["PNIID"].ToString()));

                        ucStatus.Text = "PNI Case is registered.";
                    }
                    else
                    {
                        PhoenixInspectionPNI.PNICaseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["PNICASEID"].ToString()), int.Parse(ucESMOwner.SelectedHard), int.Parse(ucVessel.SelectedVessel),
                            General.GetNullableInteger(txtPNIClubid.Text), General.GetNullableDecimal(txtDeductible.Text),
                            DateTime.Parse(txtReportedDate.Text), General.GetNullableInteger(txtQualityinchargeid.Text),
                            new Guid(ViewState["PNIID"].ToString()));

                        ucStatus.Text = "PNI Case is registered.";
                    }
                    EditPNI();
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else
            {
                ucError.ErrorMessage = "Please record the medical case to register it as PNI Case.";
                ucError.Visible = true;
                return;
            }
        }
    }
    private bool isValidPNICase()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(txtReportedDate.Text) == null)
            ucError.ErrorMessage = "Reported Date is required.";

        if (General.GetNullableInteger(ucESMOwner.SelectedHard) == null)
            ucError.ErrorMessage = "Company/Owner is required.";

        return (!ucError.IsError);
    }

    protected void ucESMOwner_Changed(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPNIClub.Text) || string.IsNullOrEmpty(txtDeductible.Text) || string.IsNullOrEmpty(txtQualityincharge.Text))
        {
            DataSet ds = PhoenixInspectionPNI.EditPNIDetails(int.Parse(ucVessel.SelectedVessel));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (string.IsNullOrEmpty(txtPNIClub.Text))
                {
                    txtPNIClubid.Text = dr["FLDPANDICLUB"].ToString();
                    txtPNIClub.Text = dr["FLDPNICLUBNAME"].ToString();
                }
                if(string.IsNullOrEmpty(txtDeductible.Text))
                    txtDeductible.Text = dr["FLDDEDUCTIBLES"].ToString();
                if (string.IsNullOrEmpty(txtQualityincharge.Text))
                {
                    txtQualityinchargeid.Text = dr["FLDQUALITYINCHARGE"].ToString();
                    txtQualityincharge.Text = dr["FLDQUALITYINCHARGENAME"].ToString();
                }
            }
        }
    }
}

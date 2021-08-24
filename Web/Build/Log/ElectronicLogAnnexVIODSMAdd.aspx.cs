using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectronicLogAnnexVIODSMAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["RECORDID"] = Request.QueryString["recordid"];

            txtDate.SelectedDate = DateTime.Now.Date;
            txtTime.SelectedTime = DateTime.Now.TimeOfDay;
            if (ViewState["RECORDID"] != null)
            {
                BindFields();
                trreason.Visible = true;
            }
        }
        ShowToolBar();

    }
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        Tabstrip.MenuList = toolbar.Show();
        Tabstrip.AccessRights = this.ViewState;
    }
    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbportsea.Text, tbgas.Text, tbequipment.Text, tbmaintenance.Text, tbreason.Text, lblincsign.Text,null))
                {
                    ucError.Visible = true;
                    return;
                }
                DateTime D = DateTime.Parse(txtDate.SelectedDate.ToString());

                DateTime? Date = D.Add(txtTime.SelectedTime.Value);
                string Location = General.GetNullableString(tbportsea.Text);
                string Gas = General.GetNullableString(tbgas.Text);
                string Equipment = General.GetNullableString(tbequipment.Text);
                string ChargingReason = General.GetNullableString(tbcharging.Text);
                string Maintenance = General.GetNullableString(tbmaintenance.Text);
                string Reason = General.GetNullableString(tbreason.Text);
                int? Status = General.GetNullableInteger(tbstatus.Text);
                Guid? RecordId = null;
                if (ViewState["RECORDID"] != null)
                {
                    RecordId = General.GetNullableGuid(ViewState["RECORDID"].ToString());
                }

                if (RecordId != null)
                {
                    PhoenixMarpolLogODS.ODSMaintRecordUpdate(Date, Location, Gas, Equipment, ChargingReason, Maintenance,  RecordId, Reason, Status, lblincRank.Text, lblinchName.Text);
                }
                else
                {
                    PhoenixMarpolLogODS.ODSMaintRecordInsert(Date, Location, Gas, Equipment, ChargingReason, Maintenance, lblinchName.Text, lblincRank.Text);
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "closeTelerikWindow('code1', 'Log', false);", true);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSubmmision(DateTime? Date, string Time, string location, string Gas, string Quantity, string Maintenacne, string Reason, string Sign , int? validiationbeforesign)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Date == null)
        {
            ucError.ErrorMessage = "Date is required.";

        }

        if (string.IsNullOrEmpty(Time))
            ucError.ErrorMessage = "Time  is required.";

        if (string.IsNullOrEmpty(location))
            ucError.ErrorMessage = "Location Port / At Sea is required.";

        if (string.IsNullOrEmpty(Gas))
            ucError.ErrorMessage = "Name of Gas is required.";
        if (string.IsNullOrEmpty(Quantity))
            ucError.ErrorMessage = "Quantity used / Equipment is required.";
        if (string.IsNullOrEmpty(Maintenacne))
            ucError.ErrorMessage = "Maintenacne done is required.";
        if (ViewState["RECORDID"] != null)
        {
            if (string.IsNullOrEmpty(Reason))
                ucError.ErrorMessage = "Reason is required.";
        }
        if (validiationbeforesign == null)
        {
            if (string.IsNullOrEmpty(Sign))
                ucError.ErrorMessage = "Sign is required.";
           
        }
        return (!ucError.IsError);
    }
    private void BindFields()
    {
        try
        {
            DataTable dt = PhoenixMarpolLogODS.ODSMaintRecordEdit(General.GetNullableGuid(ViewState["RECORDID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DateTime? D = General.GetNullableDateTime(dt.Rows[0]["FLDDATE"].ToString());

                txtTime.SelectedDate = D;
                txtDate.SelectedDate = D;
                txtTime.Enabled = false;
                txtDate.Enabled = false;
                tbportsea.Text = dt.Rows[0]["FLDLOCATION"].ToString();
                tbgas.Text = dt.Rows[0]["FLDGASNAME"].ToString();
                tbequipment.Text = dt.Rows[0]["FLDQOREUSED"].ToString();
                tbcharging.Text = dt.Rows[0]["FLDCHARGINGREASON"].ToString();
                tbmaintenance.Text = dt.Rows[0]["FLDMAINTDONE"].ToString();
                
                tbstatus.Text = dt.Rows[0]["FLDSTATUS"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        string scriptpopup = "";
        string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        

        if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbportsea.Text, tbgas.Text, tbequipment.Text, tbmaintenance.Text, tbreason.Text, lblincsign.Text, 1))
        {
            ucError.Visible = true;
            return;
        }
        scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer in Charge Signature" + "&rankName=" + Rank + "&LogBookId=" + "" + "&popupname=code1" + "','false','400','190');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;


            string Name = nvc.Get("name");
            string Rank = nvc.Get("rank");

            lblinchName.Text = Name;
            lblincRank.Text = Rank;
            lblincsign.Text = "1";
            btnInchargeSign.Visible = false;
            lblinchName.Visible = true;
            //lblincRank.Visible = true;
            Filter.DutyEngineerSignatureFilterCriteria = null;

        }
    }
}
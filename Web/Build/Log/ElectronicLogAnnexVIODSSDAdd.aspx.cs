using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;

public partial class Log_ElectronicLogAnnexVIODSSDAdd : System.Web.UI.Page
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
    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        string scriptpopup = "";
        if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbsystemtype.Text, tbreason.Text, lblincsign.Text, 1))
        {
            ucError.Visible = true;
            return;
        }
        string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        


        scriptpopup = String.Format("javascript:parent.openNewWindow('engineersign','','Log/ElectricLogOfficerSignature.aspx?popuptitle=" + "Officer in Charge Signature" + "&rankName=" + Rank + "&LogBookId=" + "" + "&popupname=code1" + "','false','400','190');");
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
    }
    private void BindFields()
    {
        try
        {
            DataTable dt = PhoenixMarpolLogODS.ODSSDRecordEdit(General.GetNullableGuid(ViewState["RECORDID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DateTime? D = General.GetNullableDateTime(dt.Rows[0]["FLDDATE"].ToString());

                txtTime.SelectedDate = D;
                txtDate.SelectedDate = D;
                txtTime.Enabled = false;
                txtDate.Enabled = false;
                tbsystemtype.Text = dt.Rows[0]["FLDSYSTYPE"].ToString();
                tbsupplytotheship.Text = dt.Rows[0]["FLDODSSUPPLY"].ToString();
                tbdeliberate.Text = dt.Rows[0]["FLDDELIBDIS"].ToString();
                tbndeliberate.Text = dt.Rows[0]["FLDNDELIBDIS"].ToString();
                tbdischargeatshore.Text = dt.Rows[0]["FLDODSDISCHARGEATSHORE"].ToString();
                tbremarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                tbstatus.Text = dt.Rows[0]["FLDSTATUS"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbsystemtype.Text, tbreason.Text,lblincsign.Text,null))
                {
                    ucError.Visible = true;
                    return;
                }
                DateTime D = DateTime.Parse(txtDate.SelectedDate.ToString());

                DateTime? Date = D.Add(txtTime.SelectedTime.Value);
                string SystemType = General.GetNullableString(tbsystemtype.Text);
                string SupplyToShip = General.GetNullableString(tbsupplytotheship.Text);
                string Deliberate = General.GetNullableString(tbdeliberate.Text);
                string NDeliberate = General.GetNullableString(tbndeliberate.Text);
                string DischargeatShore = General.GetNullableString(tbdischargeatshore.Text);
                string Remarks = General.GetNullableString(tbremarks.Text);
                string Reason = General.GetNullableString(tbreason.Text);
                int? Status = General.GetNullableInteger(tbstatus.Text);
                Guid? RecordId = null;
                if (ViewState["RECORDID"] != null)
                {
                    RecordId = General.GetNullableGuid(ViewState["RECORDID"].ToString());
                }

                if (RecordId != null)
                {
                    PhoenixMarpolLogODS.ODSSDRecordUpdate(Date, SystemType, SupplyToShip, Deliberate, NDeliberate, DischargeatShore, Remarks, RecordId, Reason, Status, lblincRank.Text, lblinchName.Text);
                }
                else
                {
                    PhoenixMarpolLogODS.ODSSDRecordInsert(Date, SystemType, SupplyToShip, Deliberate, NDeliberate, DischargeatShore, Remarks, lblinchName.Text, lblincRank.Text);
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

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

        Tabstrip.MenuList = toolbar.Show();
        Tabstrip.AccessRights = this.ViewState;
    }
    private bool IsValidSubmmision(DateTime? Date, string Time, string SystemType , string Reason, string Sign , int? PreSignValidation)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Date == null)
        {
            ucError.ErrorMessage = "Date is required.";

        }

        if (string.IsNullOrEmpty(Time))
            ucError.ErrorMessage = "Time is required.";

        if (string.IsNullOrEmpty(SystemType))
            ucError.ErrorMessage = "System Type is required.";
        
        if (ViewState["RECORDID"] != null)
        {
            if (string.IsNullOrEmpty(Reason))
                ucError.ErrorMessage = "Reason is required.";
        }
        if (PreSignValidation == null)
        {

            if (string.IsNullOrEmpty(Sign))
                ucError.ErrorMessage = "Sign is required.";
        }
       
        return (!ucError.IsError);
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
           // lblincRank.Visible = true;
            Filter.DutyEngineerSignatureFilterCriteria = null;

        }
    }
}
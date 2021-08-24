using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
using System.Collections.Specialized;


public partial class Log_ElectronicLogAnnexVIODSAdd : PhoenixBasePage
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

    private void BindFields()
    {
        try
        {
            DataTable dt = PhoenixMarpolLogODS.ODSRecordEdit(General.GetNullableGuid(ViewState["RECORDID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DateTime? D = General.GetNullableDateTime(dt.Rows[0]["FLDDDATE"].ToString());

                txtTime.SelectedDate = D;
                txtTime.Enabled = false;
                txtDate.Enabled = false;
                txtDate.SelectedDate = D;

                tbequipmentname.Text = dt.Rows[0]["FLDEQUIPMENT"].ToString();
                tbmaker.Text = dt.Rows[0]["FLDMAKER"].ToString();
                tbmodel.Text = dt.Rows[0]["FLDMODEL"].ToString();
                tblocation.Text = dt.Rows[0]["FLDLOCATION"].ToString();
                tbods.Text = dt.Rows[0]["FLDODSNAME"].ToString();
                tbmass.Text = dt.Rows[0]["FLDMASS"].ToString();
                tbstatus.Text = dt.Rows[0]["FLDSTATUS"].ToString();
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

    protected void Tabstrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbequipmentname.Text, tbods.Text, tbmass.Text, tbreason.Text , lblincsign.Text,null))
                {
                    ucError.Visible = true;
                    return;
                }
                DateTime D = DateTime.Parse(txtDate.SelectedDate.ToString());

                DateTime? Date = D.Add(txtTime.SelectedTime.Value);
                string Equipment = General.GetNullableString(tbequipmentname.Text);
                string Maker = General.GetNullableString(tbmaker.Text);
                string Model = General.GetNullableString(tbmodel.Text);
                string Location = General.GetNullableString(tblocation.Text);
                string ODS = General.GetNullableString(tbods.Text);
                string Mass = General.GetNullableString(tbmass.Text);
                string Reason = General.GetNullableString(tbreason.Text);
                int? Status = General.GetNullableInteger(tbstatus.Text);
                Guid? RecordId = null;
                if (ViewState["RECORDID"] != null)
                {
                    RecordId = General.GetNullableGuid(ViewState["RECORDID"].ToString());
                }

                if (RecordId != null)
                {
                    PhoenixMarpolLogODS.ODSRecordUpdate(Date, Equipment, Maker, Model, Location, ODS, Mass, RecordId, Reason, Status,lblincRank.Text,lblinchName.Text);
                }
                else
                {
                    PhoenixMarpolLogODS.ODSRecordInsert(Date, Equipment, Maker, Model, Location, ODS, Mass,lblinchName.Text,lblincRank.Text);
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

    private bool IsValidSubmmision(DateTime? Date, string Time, string Equipment, string ODS, string Mass, string Reason , string Sign , int? Validationbeforesign)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Date == null)
        {
            ucError.ErrorMessage = "Date .";

        }

        if (string.IsNullOrEmpty(Time))
            ucError.ErrorMessage = "Time .";

        if (string.IsNullOrEmpty(Equipment))
            ucError.ErrorMessage = "Equipment .";
        if (string.IsNullOrEmpty(ODS))
            ucError.ErrorMessage = "ODS .";
        if (string.IsNullOrEmpty(Mass))
            ucError.ErrorMessage = "Mass .";
        if (ViewState["RECORDID"] != null)
        {
            if (string.IsNullOrEmpty(Reason))
                ucError.ErrorMessage = "Reason .";
        }
        if (Validationbeforesign == null)
        {
            if (string.IsNullOrEmpty(Sign))
                ucError.ErrorMessage = "Sign .";
        }
       
        return (!ucError.IsError);
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        string scriptpopup = "";
        string Rank = PhoenixMarpolLogODS.GetRankName(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        

        if (!IsValidSubmmision(General.GetNullableDateTime(txtDate.SelectedDate.ToString()), txtTime.SelectedTime.ToString(), tbequipmentname.Text, tbods.Text, tbmass.Text, tbreason.Text, lblincsign.Text,1))
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
            
            Filter.DutyEngineerSignatureFilterCriteria = null;

        }
    }
}
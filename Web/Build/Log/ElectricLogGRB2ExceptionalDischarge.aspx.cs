using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogGRB2ExceptionalDischarge : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int logId = 0;
    Guid logBookId = Guid.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        confirm.Attributes.Add("style", "display:none;");
        logId = PhoenixMarpolLogGRB2.GetGRB2LogId(usercode, "ExcepDischarge");

        if (string.IsNullOrWhiteSpace(Request.QueryString["LogBookId"]) == false)
        {
            logBookId = Guid.Parse(Request.QueryString["LogBookId"]);
        }

        ShowToolBar();

        if (IsPostBack == false)
        {
            dtPicker.SelectedDate = DateTime.Now;
            dtPicker.MaxDate = DateTime.Now;
            //GetLastTranscationDate();
            LoadNotes();
            LoadCategory();
            BindData();
        }
    }


    private void LoadCategory()
    {
        ddlCategory.DataSource = PhoenixMarpolLogGRB2.GetCategoryList();
        ddlCategory.DataValueField = "FLDCODEID";
        ddlCategory.DataTextField = "FLDSHORTCODE";
        ddlCategory.DataBind();
    }

    private void LoadNotes()
    {
        DataTable dt = PhoenixMarpolLogGRB2.GRB2LogRegisterEdit(usercode, logId);
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }
        else
        {
            lblnotes.Text = "No Notes";
        }
    }


    private void BindData()
    {
        if (logBookId != Guid.Empty)
        {
            DataTable dt = PhoenixMarpolLogGRB2.BookEntryReportSearchById(usercode, vesselId, logBookId);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                dtPicker.SelectedDate = Convert.ToDateTime(row["FLDDATE"]);
                txtVesselName.Text = (string)row["FLDPORTPOSITION"];
                txtAmountDischarge.Text = row["FLDDISCHARGEACCIDENT"] == DBNull.Value ? null : Convert.ToDecimal(row["FLDDISCHARGEACCIDENT"]).ToString();
                txtRemarks.Text = (string)row["FLDREMARKS"];

                RadComboBoxItem categoryItem = ddlCategory.Items.FindItem(x => x.Value == row["FLDCATEGORY"].ToString());
                if (categoryItem != null)
                {
                    categoryItem.Selected = true;
                }

                dtPicker.Enabled = false;
                ddlCategory.Enabled = false;
            }
        }
    }


    private void OnChiefOfficerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null && Filter.DutyEngineerSignatureFilterCriteria["rank"].ToLower() == "co")
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincsign.Text = string.Format("{0} {1}",  nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
            lblinchName.Text = nvc.Get("name");
            lblincRank.Text = nvc.Get("rank");
            lblinchId.Text = nvc.Get("id");
            lblincSignDate.Text = signedDate.ToString();
            Filter.DutyEngineerSignatureFilterCriteria = null;

            btnInchargeSign.Visible = false;
            lblincsign.Visible = true;

        }
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarpolLogGRB2.GetLastTransactionDate(vesselId, usercode);
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                if (logBookId == Guid.Empty)
                {
                    logBookEntryInsert();
                }
                else
                {
                    logBookEntryUpdate();
                }

                // close the popup and refresh the list
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
            }


        }
        catch (Exception ex)
        {
            if (ex.Message == "EXCEED85")
                RadWindowManager1.RadConfirm("Warning. Tank Reached its 85 percentage capacity", "confirm", 320, 150, null, "Exceed");
            else
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void logBookEntryUpdate()
    {
        Decimal amountDischarge = string.IsNullOrWhiteSpace(txtAmountDischarge.Text) == false ? Convert.ToDecimal(txtAmountDischarge.Text) : 0;

        PhoenixMarpolLogGRB2.BookEntryUpdate(usercode, vesselId, logBookId, logId, dtPicker.SelectedDate.Value,
            txtVesselName.Text, ddlCategory.SelectedValue, null, null, null, txtRemarks.Text, amountDischarge);

        PhoenixMarpolLogGRB2.LogGRB2BookEntryStatusUpdate(usercode, logId, logBookId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), null, null, null, null);
    }

    private void logBookEntryInsert()
    {
        Guid logBookTransferId = Guid.Empty;
        Decimal amountDischarge = string.IsNullOrWhiteSpace(txtAmountDischarge.Text) == false ? Convert.ToDecimal(txtAmountDischarge.Text) : 0;

        PhoenixMarpolLogGRB2.BookEntryInsert(usercode, vesselId, logId, dtPicker.SelectedDate.Value,
            txtVesselName.Text, ddlCategory.SelectedValue, null, null, null, txtRemarks.Text, amountDischarge,
            false, null, true, ref logBookTransferId);

        //// status insert
        PhoenixMarpolLogGRB2.LogGRB2BookEntryStatusInsert(usercode, logId, logBookTransferId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), null, null, null, null);
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidInput() == false)
            {
                ucError.Visible = true;
                return;
            }

            //DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            // close the popup and refresh the list
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (dtPicker.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtVesselName.Text))
        {
            ucError.ErrorMessage = "Position or Port / Vessel is Required ";
        }

        if (ddlCategory.SelectedItem == null)
        {
            ucError.ErrorMessage = "Category is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtAmountDischarge.Text))
        {
            ucError.ErrorMessage = "Amount Discharge is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtAmountDischarge.Text) == false && (Convert.ToDecimal(txtAmountDischarge.Text) <= 0 ))
        {
            ucError.ErrorMessage = "Amount Discharge cannot be negative or zero";
        }

        if (string.IsNullOrWhiteSpace(txtRemarks.Text))
        {
            ucError.ErrorMessage = "Remarks is Required ";
        }

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Incharge Sign is Required ";
        }


        return (!ucError.IsError);
    }

    public bool isValidSignature()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (dtPicker.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtVesselName.Text))
        {
            ucError.ErrorMessage = "Position or Port / Vessel is Required ";
        }

        if (ddlCategory.SelectedItem == null)
        {
            ucError.ErrorMessage = "Category is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtAmountDischarge.Text))
        {
            ucError.ErrorMessage = "Amount Discharge is Required ";
        }

        if (string.IsNullOrWhiteSpace(txtAmountDischarge.Text) == false && (Convert.ToDecimal(txtAmountDischarge.Text) <= 0))
        {
            ucError.ErrorMessage = "Amount Discharge cannot be negative or zero";
        }

        if (string.IsNullOrWhiteSpace(txtRemarks.Text))
        {
            ucError.ErrorMessage = "Remarks is Required ";
        }


        return (!ucError.IsError);
    }

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        //
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnChiefOfficerSign();
    }


    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if(isValidSignature()==false)
        {
            ucError.Visible = true;
            return;
        }
        string popupName = "Log";
        string popupTitle = "Chief Officer Sign";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName=co&popuptitle={1}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, popupTitle);
        RadScriptManager.RegisterStartupScript(this, this.GetType(), popupName, script, true);

    }
}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBInternalCargoTransfer : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "3";
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "InternalTransfer");
        confirm.Attributes.Add("style", "display:none;");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
            ddlTransferTo.Enabled = false;
            txtOperationDate.Enabled = false;
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["isMissedOperation"]) == false)
        {
            isMissedOperation = Convert.ToBoolean(Request.QueryString["isMissedOperation"]);
        }

        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationEdit"]) == false)
        {
            isMissedOperationEdit = Convert.ToBoolean(Request.QueryString["missedOperationEdit"]);
        }

        ShowToolBar();

        if (IsPostBack == false)
        {
            ddlFromPopulate();
            ddlToPopulate();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
        }
    }

    private void BindData()
    {
        if (txid != Guid.Empty)
        {
            DataTable dt = PhoenixMarpolLogCRB.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 3)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 4)
                    {
                        txtcagoname.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 5)
                    {
                        txtcategory.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 6)
                    {
                        RadComboBoxItem fromItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 7)
                    {
                        RadComboBoxItem toItem = ddlTransferTo.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (toItem != null)
                        {
                            toItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 8)
                    {
                        txtBfrTrnsROBFrom.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 9)
                    {
                        txtBfrTrnsROBTo.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 10)
                    {
                        txtAfrTrnsROBFrom.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 11)
                    {
                        txtAfrTrnsROBTo.Text = row["FLDVALUE"].ToString();
                    }
                }
                SetDefaultData();
            }
        }
    }

    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;

            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincsign.Text = string.Format("{0}-{1} {2}", nvc.Get("rank"), nvc.Get("name"), signedDate.ToString("dd-MM-yyyy"));
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
        ViewState["lastTranscationDate"] = PhoenixMarpolLogCRB.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, "CCRO");
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }
    private void ddlToPopulate()
    {
        ddlTransferTo.DataSource = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, "CCRO");
        ddlTransferTo.DataBind();
        if (ddlTransferTo.Items.Count > 0)
        {
            ddlTransferTo.Items[0].Selected = true;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;

       
    }

    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        ReportCode = "B";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "4.1";
        txtItemNo2.Text = "4.2";
        txtItemNo3.Text = "5";
        txtItemNo4.Text = "6";
        lblRecord.Text = string.Format("From  <b> {0} Category : {1}</b>", txtcagoname.Text, txtcategory.Text);
        lblrecord1.Text = string.Format("From  <b> {0}</b>", ddlTransferFrom.SelectedItem.Text);
        lblrecord2.Text = string.Format("To  <b> {0}</b>", ddlTransferTo.SelectedItem.Text);

        if(txtAfrTrnsROBFrom.Text != "" && decimal.Parse(txtAfrTrnsROBFrom.Text) > 0)
        {

            lblrecord3.Text = "<b>NO</b>";
            lblrecord4.Text = decimal.Parse(txtAfrTrnsROBFrom.Text).ToString() + " m3 Retained";
        }
        else
        { 
            lblrecord3.Text = "<b>YES</b>";
            lblrecord4.Text = "<b>N/A</b>";
        }
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == false && isValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                TransactionValidation();
                SaveTxin();
                
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

    private void TransactionValidation()
    {
        PhoenixMarpolLogCRB.InsertTransactionCRBValidation(usercode
                                            , new Guid(ddlTransferTo.SelectedItem.Value)
                                            , null
                                            , txtAfrTrnsROBTo.Text
                                            , vesselId
                                            , General.GetNullableInteger(lblinchId.Text)
                                            );
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidInput() == false || isValidSignature() == false)
            {
                ucError.Visible = true;
                return;
            }

            SaveTxin();
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

    private void SaveTxin()
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(entrydate, logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate(logId);
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId);
            LogBookUpdate(logId);
        }
        else
        {
            Guid logTxId = Guid.NewGuid();

            TranscationInsert(entrydate, logTxId, logId);

            // book entry insert
            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            txtCode.Text,
                            txtItemNo.Text,
                            lblRecord.Text,
                            1
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo1.Text,
                            lblrecord1.Text,
                            2
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo2.Text,
                            lblrecord2.Text,
                            3
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo3.Text,
                            lblrecord3.Text,
                            4
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo4.Text,
                            lblrecord4.Text,
                            5
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            null,
                            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), false),
                            6,
                            true
                );

            // history insert
            PhoenixMarpolLogCRB.LogCRBBookEntryStatusInsert(usercode
                            , vesselId
                            , logId
                            , logTxId
                            , General.GetNullableInteger(lblinchId.Text)
                            , General.GetNullableString(lblincRank.Text)
                            , General.GetNullableString(lblincsign.Text)
                            , General.GetNullableDateTime(lblincSignDate.Text)
                        );


            PhoenixMarpolLogCRB.LogCRBHistoryInsert(usercode
                                     , vesselId
                                     , logId
                                     , logTxId
                                     , General.GetNullableString(lblincRank.Text)
                                     , General.GetNullableDateTime(lblincSignDate.Text)
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , 1
                                     , lblRecord.Text
                                     , lblRecord.Text
                                     , lblrecord1.Text
                                     , lblrecord2.Text
                                     , lblrecord3.Text
                                     , lblrecord4.Text
                                     , ""
                                     , ""
                                     , ""
                               );
        }
    }

    private void MissedOperationalEntryTemplateUpdate(Guid logTxId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // store transaction 
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);
        nvc.Add("Record5", lblrecord4.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "5");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord3.Text,
            4
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord4.Text,
            5
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            6
        );

        PhoenixMarpolLogCRB.LogCRBBookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , txid
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

        PhoenixMarpolLogCRB.LogCRBHistoryUpdate(usercode
                                  , vesselId
                                  , logId
                                  , txid
                                  , General.GetNullableString(lblincRank.Text)
                                  , General.GetNullableDateTime(lblincSignDate.Text)
                                  , txtCode.Text
                                  , txtItemNo.Text
                                  , 1
                                  , lblRecord.Text
                                  , lblRecord.Text
                                  , lblrecord1.Text
                                  , lblrecord2.Text
                                  , lblrecord3.Text
                                  , lblrecord4.Text
                                  , ""
                                  , ""
                                  , ""
                            );

    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                3,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                4,
                txid,
                entrydate,
                txtcagoname.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                5,
                txid,
                entrydate,
                txtcategory.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                6,
                txid,
                entrydate,
                ddlTransferFrom.SelectedItem.Text);


        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                7,
                txid,
                entrydate,
                ddlTransferTo.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                8,
                txid,
                entrydate,
                txtBfrTrnsROBFrom.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                9,
                txid,
                entrydate,
                txtBfrTrnsROBTo.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                10,
                txid,
                entrydate,
                txtAfrTrnsROBFrom.Text);


        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                11,
                txid,
                entrydate,
                txtAfrTrnsROBTo.Text);

    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            3,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            4,
                            logTxId,
                            entrydate,
                            txtcagoname.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            5,
                            logTxId,
                            entrydate,
                            txtcategory.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            6,
                            logTxId,
                            entrydate,
                            ddlTransferFrom.SelectedItem.Text
            );


        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            7,
                            logTxId,
                            entrydate,
                            ddlTransferTo.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            8,
                            logTxId,
                            entrydate,
                            txtBfrTrnsROBFrom.Text
            );


        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            9,
                            logTxId,
                            entrydate,
                            txtBfrTrnsROBTo.Text
            );


        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            10,
                            logTxId,
                            entrydate,
                            txtAfrTrnsROBFrom.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            11,
                            logTxId,
                            entrydate,
                            txtAfrTrnsROBTo.Text
            );
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "From Tank Before Transfer ROB is required";
        }

        if (string.IsNullOrWhiteSpace(txtcagoname.Text))
        {
            ucError.ErrorMessage = "Cargo Name is required";
        }

        if (string.IsNullOrWhiteSpace(txtcategory.Text))
        {
            ucError.ErrorMessage = "Category is required";
        }

        if (string.IsNullOrWhiteSpace(txtBfrTrnsROBTo.Text))
        {
            ucError.ErrorMessage = "To Tank Before Transfer ROB is required";
        }


        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "From Tank After Transfer ROB is required";
        }

        if (ddlTransferFrom.SelectedItem.Text == ddlTransferTo.SelectedItem.Text)
        {
            ucError.ErrorMessage = "Transfer From and To Tank cannot be the same";
        }
        else if (string.IsNullOrWhiteSpace(txtAfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "To Tank After Transfer ROB is required";
        }
        else if (Convert.ToDecimal(txtAfrTrnsROBFrom.Text) > Convert.ToDecimal(txtBfrTrnsROBFrom.Text))
        {
            ucError.ErrorMessage = "After ROB cannot be greater than Before ROB";
        }

        if (string.IsNullOrWhiteSpace(txtAfrTrnsROBTo.Text))
        {
            ucError.ErrorMessage = "To Tank After Transfer ROB is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    private bool isValidSignature()
    {
        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void Refresh_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        string popupName = isMissedOperation ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Sign";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMark", script, true);
    }
}
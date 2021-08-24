using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBMandatoryPrewash : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "12";
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
        logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "MandatoryPrewash");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
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
        LoadCargoTank();

        if (IsPostBack == false)
        {
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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 19)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 20)
                    {
                        txtwashingmethod.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 21)
                    {
                        txtnomachine.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 22)
                    {
                        txtdurationwash.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 23)
                    {
                        txthotcold.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 24)
                    {
                        txtunloadingport.Text = row["FLDVALUE"].ToString();
                        if(txtunloadingport.Text != "")
                        {
                            txtotherwise.Enabled = false;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 25)
                    {
                        txtotherwise.Text = row["FLDVALUE"].ToString();
                        if (txtotherwise.Text != "")
                        {
                            txtunloadingport.Enabled = false;
                        }
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
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
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

        ReportCode = "D";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "13";
        txtItemNo2.Text = "13.1";
        txtItemNo3.Text = "13.2";
        txtItemNo4.Text = "13.3";
        if (txtunloadingport.Text != "")
        {
            txtItemNo5.Text = "14.1";
        }
        else
        {
            txtItemNo5.Text = "14.2";
        }

        ViewState["TankName"] = string.Empty;
        ShowCargoTank(rptrTank);
        ShowCargoTank(rptrTank20);
        ShowCargoTank(rptrTank30);
        ShowCargoTank(rptrTank40);

        lblRecord.Text = string.Format("<span class='left'><b>{0}</b></span>", ViewState["TankName"].ToString().TrimEnd(','));
        lblrecord1.Text = string.Format("<b>{0}</b>", txtwashingmethod.Text);
        lblrecord2.Text = string.Format("<b>{0}</b>", txtnomachine.Text);
        lblrecord3.Text = string.Format("<b>{0}</b>", txtdurationwash.Text);
        lblrecord4.Text = string.Format("<b>{0}</b>", txthotcold.Text);
        if (txtItemNo5.Text == "14.1")
        {
            lblrecord5.Text = string.Format("<b>{0}</b>", txtunloadingport.Text);
        }
        else
        {
            lblrecord5.Text = string.Format("<b>{0}</b>", txtotherwise.Text);
        }
    }

    private void LoadCargoTank()
    {
        // form the dynamic tank
        //DataSet ds = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, "CCRO");
        Guid? TransactionId = txid == Guid.Empty ? null : (Guid?)txid;

        DataTable tankList = PhoenixMarpolLogCRB.CRBTankList(usercode, vesselId, TransactionId);
        DataTable dt = tankList;

        if (dt.Rows.Count == 0)
        {
            return;
        }

        DataTable dtload = SelectTopDataRow(dt, 0, 9);


        if (dtload.Rows.Count > 0)
        {
            rptrTank.DataSource = dtload;
            rptrTank.DataBind();
        }

        DataTable dt2ndload = SelectTopDataRow(dt, 10, 19);

        if (dt2ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt2ndload.Rows.Count;
            rptrTank20.DataSource = dt2ndload;
            rptrTank20.DataBind();
        }

        DataTable dt3ndload = SelectTopDataRow(dt, 20, 29);

        if (dt3ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt3ndload.Rows.Count;
            rptrTank30.DataSource = dt3ndload;
            rptrTank30.DataBind();
        }

        DataTable dt4ndload = SelectTopDataRow(dt, 30, 39);

        if (dt4ndload.Rows.Count > 0)
        {
            ViewState["TankCount"] = dt4ndload.Rows.Count;
            rptrTank40.DataSource = dt4ndload;
            rptrTank40.DataBind();
        }
    }

    public DataTable SelectTopDataRow(DataTable dt, int from, int to)
    {
        DataTable dtn = dt.Clone();
        for (int i = from; (i <= to && i < dt.Rows.Count); i++)
        {
            dtn.ImportRow(dt.Rows[i]);
        }

        return dtn;
    }

    private void ShowCargoTank(Repeater repeater)
    {
        ViewState["TankName"] = (string)ViewState["TankName"] == string.Empty ? string.Empty : ViewState["TankName"];

        foreach (RepeaterItem item in repeater.Items)
        {
            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadTextBox txtnamesub = (RadTextBox)item.FindControl("txtSubstance");
            RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");

            if (string.IsNullOrWhiteSpace(txtnamesub.Text) == false)
            {
                string tankCargo = string.Format("{0} / {1} / Cat {2}", lblTankName.Text, txtnamesub.Text, txtCategory.Text);
                ViewState["TankName"] += tankCargo + ", ";
            }
            
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
                if (isValidInput() == false || IsValidSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

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
                            (txtunloadingport.Text != "") ? "14.1" : "14.2",
                            txtItemNo5.Text == "14.1" ? txtunloadingport.Text : txtotherwise.Text,
                            6
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
                            7,
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
                                     , lblrecord5.Text
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
        nvc.Add("Record6", lblrecord5.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);
        nvc.Add("ItemNo6", txtItemNo5.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "6");
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
            lblrecord5.Text,
            6
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            7
        );

        // history update
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
                                , lblrecord5.Text
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
                19,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                20,
                txid,
                entrydate,
                txtwashingmethod.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                21,
                txid,
                entrydate,
                txtnomachine.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                22,
                txid,
                entrydate,
                txtdurationwash.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                23,
                txid,
                entrydate,
                txthotcold.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                24,
                txid,
                entrydate,
                txtunloadingport.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                25,
                txid,
                entrydate,
                txtotherwise.Text);


        UpdateCargoTankValues(rptrTank);
        UpdateCargoTankValues(rptrTank20);
        UpdateCargoTankValues(rptrTank30);
        UpdateCargoTankValues(rptrTank40);
    }


    private void UpdateCargoTankValues(Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");
            RadTextBox txtnamesub = (RadTextBox)item.FindControl("txtSubstance");
            RadLabel lblTankId = (RadLabel)item.FindControl("lblTankId");


            PhoenixMarpolLogCRB.CargoLoadingUpdate(
                    usercode,
                    vesselId,
                    txid,
                    Guid.Parse(lblTankId.Text),
                    0,
                    (txtCategory.Text),
                    txtnamesub.Text,
                    true
              );
        }
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            19,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            20,
                            logTxId,
                            entrydate,
                            txtwashingmethod.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            21,
                            logTxId,
                            entrydate,
                            txtnomachine.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            22,
                            logTxId,
                            entrydate,
                            txtdurationwash.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            23,
                            logTxId,
                            entrydate,
                            txthotcold.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            24,
                            logTxId,
                            entrydate,
                            txtunloadingport.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            25,
                            logTxId,
                            entrydate,
                            txtotherwise.Text
            );


        InsertCargoTankValues(logTxId, rptrTank);
        InsertCargoTankValues(logTxId, rptrTank20);
        InsertCargoTankValues(logTxId, rptrTank30);
        InsertCargoTankValues(logTxId, rptrTank40);
    }

    private void InsertCargoTankValues(Guid logTxId, Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");
            RadTextBox txtnamesub = (RadTextBox)item.FindControl("txtSubstance");
            RadLabel lblTankId = (RadLabel)item.FindControl("lblTankId");


            PhoenixMarpolLogCRB.CargoLoadingInsert(
                    usercode,
                    vesselId,
                    logTxId,
                    Guid.Parse(lblTankId.Text),
                    0,
                    (txtCategory.Text),
                    txtnamesub.Text,
                    true
                );
        }
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtwashingmethod.Text))
        {
            ucError.ErrorMessage = "Washing Method is required";
        }

        if (string.IsNullOrWhiteSpace(txtnomachine.Text))
        {
            ucError.ErrorMessage = "Number of Cleaning machines is required";
        }

        if (string.IsNullOrWhiteSpace(txtdurationwash.Text))
        {
            ucError.ErrorMessage = "Duration of Wash/Washing Cycles is required";
        }

        if (string.IsNullOrWhiteSpace(txthotcold.Text))
        {
            ucError.ErrorMessage = "Hot/Cold Wash is required";
        }

        if (string.IsNullOrWhiteSpace(txtunloadingport.Text) && string.IsNullOrWhiteSpace(txtotherwise.Text))
        {
            ucError.ErrorMessage = "Reception Facility is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        ViewState["validTankCount"] = 0;
        int totalTankCount = rptrTank.Items.Count + rptrTank20.Items.Count + rptrTank30.Items.Count + rptrTank40.Items.Count;

        CheckValidTank(rptrTank);
        CheckValidTank(rptrTank20);
        CheckValidTank(rptrTank30);
        CheckValidTank(rptrTank40);

        if ((int)ViewState["validTankCount"] == totalTankCount)
        {
            ucError.ErrorMessage = "Identity of Tanks is required";
        }


        return (!ucError.IsError);
    }


    private void CheckValidTank(Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadTextBox txtnamesub = (RadTextBox)item.FindControl("txtSubstance");
            RadTextBox txtCategory = (RadTextBox)item.FindControl("txtCategory");
            RadLabel lblTank = (RadLabel)item.FindControl("lblTank");

            if (string.IsNullOrWhiteSpace(txtnamesub.Text) && string.IsNullOrWhiteSpace(txtCategory.Text) == false)
            {
                ucError.ErrorMessage = "Name of Substance is Required";
                break;
            }

            if (string.IsNullOrWhiteSpace(txtCategory.Text) && string.IsNullOrWhiteSpace(txtnamesub.Text) == false)
            {
                ucError.ErrorMessage = "Category is Required";
                break;
            }

            if (string.IsNullOrWhiteSpace(txtCategory.Text) && string.IsNullOrWhiteSpace(txtnamesub.Text))
            {
                ViewState["validTankCount"] = (int)ViewState["validTankCount"] + 1;
            }

        }
    }

    private bool IsValidSignature()
    {
        ucError.HeaderMessage = "Please provide the following required information";

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


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTxin();
        }
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlTank_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        SetDefaultData();
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

    protected void txtSubstance_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}
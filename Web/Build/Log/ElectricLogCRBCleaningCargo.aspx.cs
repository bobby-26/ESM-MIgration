using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogCRBCleaningCargo : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "15";
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
        logId = PhoenixMarpolLogCRB.GetCRBLogId(usercode, "CleaningCargoTank");

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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 26)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 27)
                    {
                        txtStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 28)
                    {
                        txtEndTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 29)
                    {
                        txtwashingmethod.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 30)
                    {
                        txtCleaningAgents.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 31)
                    {
                        txtventilationproc.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 32)
                    {
                        txtIntoSea.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 33)
                    {
                        txtReception.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 34)
                    {
                        txtSlops.Text = row["FLDVALUE"].ToString();
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
        ReportCode = "E";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "15.1";
        txtItemNo2.Text = "15.2";
        txtItemNo3.Text = "15.3";
        txtItemNo4.Text = "16.1";
        txtItemNo5.Text = "16.2";
        txtItemNo6.Text = "16.3";

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan endTime = txtEndTime.SelectedTime.HasValue == false ? new TimeSpan() : txtEndTime.SelectedTime.Value;

        ViewState["TankName"] = string.Empty;
        ShowCargoTank(rptrTank);
        ShowCargoTank(rptrTank20);
        ShowCargoTank(rptrTank30);
        ShowCargoTank(rptrTank40);


        lblRecord.Text = string.Format("<span class='left'><b>From {0} To {1} </b></span> <span class='Right'><b>{2}</b></span>", startTime.ToString(),endTime.ToString(), ViewState["TankName"].ToString().TrimEnd(','));
        lblrecord1.Text = string.Format("<b>{0}</b>", txtwashingmethod.Text);
        lblrecord2.Text = string.Format("<b>{0}</b>", txtCleaningAgents.Text);
        lblrecord3.Text = string.Format("<b>{0}</b>", txtventilationproc.Text);
        if (txtIntoSea.Text == "")
        {
            lblrecord4.Text = "<b>N/A</b>";
        }
        else
        {
            lblrecord4.Text = string.Format("<b>{0}</b>", txtIntoSea.Text);
        }
        if (txtReception.Text == "")
        {
            lblrecord5.Text = "<b>N/A</b>";
        }
        else
        {
            lblrecord5.Text = string.Format("<b>{0}</b>", txtReception.Text);
        }
        if (txtSlops.Text == "")
        {
            lblrecord6.Text = "<b>N/A</b>";
        }
        else
        {
            lblrecord6.Text = string.Format("<b>{0}</b>", txtSlops.Text);
        }
    }

    private void LoadCargoTank()
    {

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
                            txtItemNo5.Text,
                            lblrecord5.Text,
                            6
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo6.Text,
                            lblrecord6.Text,
                            7
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
                            8,
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
        nvc.Add("Record7", lblrecord6.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);
        nvc.Add("ItemNo6", txtItemNo5.Text);
        nvc.Add("ItemNo7", txtItemNo6.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "7");
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
            lblrecord6.Text,
            7
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            8
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
    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan endTime = txtEndTime.SelectedTime.HasValue == false ? new TimeSpan() : txtEndTime.SelectedTime.Value;

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                26,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                27,
                txid,
                entrydate,
                startTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                28,
                txid,
                entrydate,
                endTime.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                29,
                txid,
                entrydate,
                txtwashingmethod.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                30,
                txid,
                entrydate,
                txtCleaningAgents.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                31,
                txid,
                entrydate,
                txtventilationproc.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                32,
                txid,
                entrydate,
                txtIntoSea.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                33,
                txid,
                entrydate,
                txtReception.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                34,
                txid,
                entrydate,
                txtSlops.Text);

        UpdateCargoTankValues(rptrTank);
        UpdateCargoTankValues(rptrTank20);
        UpdateCargoTankValues(rptrTank30);
        UpdateCargoTankValues(rptrTank40);

        //for (int i = 0; i < tankCount; i++)
        //{
        //    RadTextBox txtnamesub = (RadTextBox)tblUserControl.FindControl("txtnamesub" + i);
        //    RadComboBox ddlTank = (RadComboBox)tblUserControl.FindControl("ddlTank" + i);
        //    RadTextBox txtCategory = (RadTextBox)tblUserControl.FindControl("txtCategory" + i);

        //    if (ddlTank.SelectedItem == null)
        //        continue;

        //    PhoenixMarpolLogCRB.CargoLoadingUpdate(
        //            usercode,
        //            vesselId,
        //            txid,
        //            Guid.Parse(ddlTank.SelectedItem.Value),
        //            0,
        //            (txtCategory.Text),
        //            txtnamesub.Text,
        //            true
        //      );
        //}
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

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan endTime = txtEndTime.SelectedTime.HasValue == false ? new TimeSpan() : txtEndTime.SelectedTime.Value;

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            26,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            27,
                            logTxId,
                            entrydate,
                            startTime.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            28,
                            logTxId,
                            entrydate,
                            endTime.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            29,
                            logTxId,
                            entrydate,
                            txtwashingmethod.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            30,
                            logTxId,
                            entrydate,
                            txtCleaningAgents.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            31,
                            logTxId,
                            entrydate,
                            txtventilationproc.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            32,
                            logTxId,
                            entrydate,
                            txtIntoSea.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            33,
                            logTxId,
                            entrydate,
                            txtReception.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            34,
                            logTxId,
                            entrydate,
                            txtSlops.Text
            );

        InsertCargoTankValues(logTxId, rptrTank);
        InsertCargoTankValues(logTxId, rptrTank20);
        InsertCargoTankValues(logTxId, rptrTank30);
        InsertCargoTankValues(logTxId, rptrTank40);

        // insert the tank list
        //for (int i = 0; i < tankCount; i++)
        //{
        //    RadTextBox txtCategory = (RadTextBox)tblUserControl.FindControl("txtCategory" + i);
        //    RadTextBox txtnamesub = (RadTextBox)tblUserControl.FindControl("txtnamesub" + i);
        //    RadComboBox ddlTank = (RadComboBox)tblUserControl.FindControl("ddlTank" + i);

        //    if (ddlTank.SelectedItem == null)
        //        continue;

        //    PhoenixMarpolLogCRB.CargoLoadingInsert(
        //            usercode,
        //            vesselId,
        //            logTxId,
        //            Guid.Parse(ddlTank.SelectedItem.Value),
        //            0,
        //            (txtCategory.Text),
        //            txtnamesub.Text,
        //            true
        //        );
        //}
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

        if (string.IsNullOrWhiteSpace(txtCleaningAgents.Text))
        {
            ucError.ErrorMessage = "Cleaning Agent(s) is required";
        }

        if (string.IsNullOrWhiteSpace(txtventilationproc.Text))
        {
            ucError.ErrorMessage = "Ventilation Procedure Used is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }

        if (txtEndTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Completed Time is required";
        }

        if (string.IsNullOrWhiteSpace(txtIntoSea.Text) && string.IsNullOrWhiteSpace(txtReception.Text) && string.IsNullOrWhiteSpace(txtSlops.Text))
        {
            ucError.ErrorMessage = "Tank Washing Transferred to is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }


        ViewState["invalidTankCount"] = 0;
        int totalTankCount = rptrTank.Items.Count + rptrTank20.Items.Count + rptrTank30.Items.Count + rptrTank40.Items.Count;

        CheckValidTank(rptrTank);
        CheckValidTank(rptrTank20);
        CheckValidTank(rptrTank30);
        CheckValidTank(rptrTank40);

        if ((int)ViewState["invalidTankCount"] == totalTankCount)
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
                ViewState["invalidTankCount"] = (int)ViewState["invalidTankCount"] + 1;
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

    protected void txtStartTime_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
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
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Log_ElectricLogCRBUnloadingCargo : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "7";
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
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "UnloadingCargo");
        confirm.Attributes.Add("style", "display:none;");

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

                    if (Convert.ToInt32(row["FLDITEMID"]) == 12)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 13)
                    {
                        txtUnloadTerminal.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 14)
                    {
                        ddlReception.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 15)
                    {
                        ddlfailure.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 16)
                    {
                        txtnaturefailure.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 17)
                    {
                        txtreasonfailure.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 18)
                    {
                        txttimeoperational.Text = row["FLDVALUE"].ToString();
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
        ReportCode = "C";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        txtItemNo1.Text = "8";
        txtItemNo2.Text = "9";
        txtItemNo3.Text = "9.1";
        txtItemNo4.Text = "9.2";
        txtItemNo5.Text = "10";
        txtItemNo6.Text = "11";
        txtItemNo7.Text = "11.1";
        txtItemNo8.Text = "11.2";
        txtItemNo9.Text = "11.3";

        ViewState["TankName"] = string.Empty;
        ViewState["TankLoaded"] = string.Empty;
        ViewState["SumQuantity"] = 0;

        ShowCargoTank(rptrTank);
        ShowCargoTank(rptrTank20);
        ShowCargoTank(rptrTank30);
        ShowCargoTank(rptrTank40);

        lblRecord.Text = string.Format("From  <b> {0}</b>", txtUnloadTerminal.Text);
        lblrecord1.Text = string.Format("<b>{0}</b>", ViewState["TankName"].ToString().TrimEnd(','));

        if (ViewState["TankLoaded"].ToString() != "")
        {
            if (decimal.Parse(ViewState["SumQuantity"].ToString()) > 0)
            {
                lblrecord2.Text = "No";
                lblrecord4.Text = string.Format("<span class='right'><b> {0}</b></span>", ViewState["TankLoaded"].ToString().TrimEnd(','));
            }
            else
            {
                lblrecord2.Text = "Yes";
                lblrecord4.Text = "NA";
            }
        }
        if (lblrecord2.Text == "No")
        {
            lblrecord3.Text = "NA";
        }
        else
        {
            lblrecord3.Text = "Yes, Procedure for emptying and stripping has been performed in accordance with the ship's Procedures and Arrangements Manual";
        }

        if (ddlReception.SelectedValue != "")
        {
            lblrecord5.Text = ddlReception.SelectedItem.Text;
        }

        if (ddlfailure.SelectedValue != "")
        {
            lblrecord6.Text = ddlfailure.SelectedItem.Text;
        }

        if (ddlfailure.SelectedValue != "" && ddlfailure.SelectedItem.Text == "Yes")
        {
            lblrecord7.Text = txtnaturefailure.Text;
            lblrecord8.Text = txtreasonfailure.Text;
            lblrecord9.Text = txttimeoperational.Text;
        }
        else
        {
            lblrecord7.Text = "NA";
            lblrecord8.Text = "NA";
            lblrecord9.Text = "NA";
        }
    }

    private void LoadCargoTank()
    {
        // form the dynamic tank
        DataSet ds = PhoenixMarpolLogCRB.ElogLocationDropDown(vesselId, usercode, "CCRO");
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
        foreach (RepeaterItem item in repeater.Items)
        {
            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadNumericTextBox txtROB = (RadNumericTextBox)item.FindControl("txtROB");
            string tankQty = string.IsNullOrWhiteSpace(txtROB.Text) ? "0" : txtROB.Text;

            if (string.IsNullOrWhiteSpace(txtROB.Text) == false)
            {
                string tankCargo = string.Format("{0} = {1} m3,", lblTankName.Text, tankQty);
                ViewState["TankName"] += lblTankName.Text + " ,";
                ViewState["TankLoaded"] += " " + tankCargo;
                ViewState["SumQuantity"] = decimal.Parse(ViewState["SumQuantity"].ToString()) + decimal.Parse(tankQty);
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

                TransactionValdation();
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
        SetDefaultData();

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
                            txtItemNo7.Text,
                            lblrecord7.Text,
                            8
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo8.Text,
                            lblrecord8.Text,
                            9
                );

            PhoenixMarpolLogCRB.BookEntryInsert(
                            usercode,
                            vesselId,
                            logTxId,
                            logId,
                            entrydate,
                            null,
                            txtItemNo9.Text,
                            lblrecord9.Text,
                            10
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
                            11,
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
                                     , lblrecord6.Text
                                     , lblrecord7.Text
                               );

        }
    }

    private void TransactionValdation()
    {
        validateTanks(rptrTank);
        validateTanks(rptrTank20);
        validateTanks(rptrTank30);
        validateTanks(rptrTank40);
    }

    private void validateTanks(Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadNumericTextBox txtROB = (RadNumericTextBox)item.FindControl("txtROB");
            RadLabel lblTankId = (RadLabel)item.FindControl("lblTankId");

            PhoenixMarpolLogCRB.InsertTransactionCRBValidation(usercode
                                , new Guid(lblTankId.Text)
                                , null
                                , txtROB.Text
                                , vesselId
                                , General.GetNullableInteger(lblinchId.Text)
                                );
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
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
        nvc.Add("Record8", lblrecord7.Text);
        nvc.Add("Record9", lblrecord8.Text);
        nvc.Add("Record10", lblrecord9.Text);

        nvc.Add("ReportCode1", txtCode.Text);

        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("ItemNo5", txtItemNo4.Text);
        nvc.Add("ItemNo6", txtItemNo5.Text);
        nvc.Add("ItemNo7", txtItemNo6.Text);
        nvc.Add("ItemNo8", txtItemNo7.Text);
        nvc.Add("ItemNo9", txtItemNo8.Text);
        nvc.Add("ItemNo10", txtItemNo9.Text);

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "10");
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
            lblrecord7.Text,
            8
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord8.Text,
            9
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord9.Text,
            10
        );

        PhoenixMarpolLogCRB.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            11
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
                                  , lblrecord6.Text
                                  , lblrecord7.Text
                            );
    }

    private void TransactionUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                12,
                txid,
                entrydate,
                entrydate.ToString());

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                13,
                txid,
                entrydate,
                txtUnloadTerminal.Text);


        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                14,
                txid,
                entrydate,
                ddlReception.SelectedItem.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                15,
                txid,
                entrydate,
                ddlfailure.SelectedItem.Text);


        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                16,
                txid,
                entrydate,
                txtnaturefailure.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                17,
                txid,
                entrydate,
                txtreasonfailure.Text);

        PhoenixMarpolLogCRB.TransactionUpdate(usercode,
                vesselId,
                logId,
                18,
                txid,
                entrydate,
                txttimeoperational.Text);

        UpdateCargoTankValues(rptrTank);
        UpdateCargoTankValues(rptrTank20);
        UpdateCargoTankValues(rptrTank30);
        UpdateCargoTankValues(rptrTank40);

    }

    private void UpdateCargoTankValues(Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadNumericTextBox txtROB = (RadNumericTextBox)item.FindControl("txtROB");
            RadLabel lblTankId = (RadLabel)item.FindControl("lblTankId");

            PhoenixMarpolLogCRB.CargoLoadingUpdate(
                    usercode,
                    vesselId,
                    txid,
                    Guid.Parse(lblTankId.Text),
                    Convert.ToDecimal(txtROB.Text),
                    false
              );
        }
    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            12,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            13,
                            logTxId,
                            entrydate,
                            txtUnloadTerminal.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            14,
                            logTxId,
                            entrydate,
                            ddlReception.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            15,
                            logTxId,
                            entrydate,
                            ddlfailure.SelectedItem.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            16,
                            logTxId,
                            entrydate,
                            txtnaturefailure.Text
            );


        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            17,
                            logTxId,
                            entrydate,
                            txtreasonfailure.Text
            );

        PhoenixMarpolLogCRB.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            18,
                            logTxId,
                            entrydate,
                            txttimeoperational.Text
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
            RadNumericTextBox txtROB = (RadNumericTextBox)item.FindControl("txtROB");
            RadLabel lblTankId = (RadLabel)item.FindControl("lblTankId");

            PhoenixMarpolLogCRB.CargoLoadingInsert(
                    usercode,
                    vesselId,
                    logTxId,
                    Guid.Parse(lblTankId.Text),
                    Convert.ToDecimal(txtROB.Text),
                    false
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

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        if (string.IsNullOrWhiteSpace(txtUnloadTerminal.Text))
        {
            ucError.ErrorMessage = "Unload Terminal is Required Before Save";
        }

        if (string.IsNullOrWhiteSpace(ddlfailure.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Failure of Pumping is Required";
        }

        if (string.IsNullOrWhiteSpace(ddlReception.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Disposal to reception facilities is Required";
        }

        if (string.IsNullOrWhiteSpace(txtnaturefailure.Text))
        {
            ucError.ErrorMessage = "Time and nature of failure is Required Before Save";
        }

        if (string.IsNullOrWhiteSpace(txtreasonfailure.Text))
        {
            ucError.ErrorMessage = "Reason for Failure is Required Before Save";
        }

        if (string.IsNullOrWhiteSpace(txttimeoperational.Text))
        {
            ucError.ErrorMessage = "Time when the system has been made operational is Required Before Save";
        }

        ViewState["invalidTankCount"] = 0;
        int totalTankCount = rptrTank.Items.Count + rptrTank20.Items.Count + rptrTank30.Items.Count + rptrTank40.Items.Count;

        CheckValidTank(rptrTank);
        CheckValidTank(rptrTank20);
        CheckValidTank(rptrTank30);
        CheckValidTank(rptrTank40);


        if ((int)ViewState["invalidTankCount"] == totalTankCount)
        {
            ucError.ErrorMessage = "Tanks Loaded with Quantity is required";
        }

        return (!ucError.IsError);
    }

    private void CheckValidTank(Repeater repeater)
    {
        foreach (RepeaterItem item in repeater.Items)
        {
            RadNumericTextBox txtROB = (RadNumericTextBox)item.FindControl("txtROB");
            if (string.IsNullOrWhiteSpace(txtROB.Text))
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

    protected void Refresh_TextChangedEvent(object sender, EventArgs e)
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

    protected void ddlfailure_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtSubstance_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

}
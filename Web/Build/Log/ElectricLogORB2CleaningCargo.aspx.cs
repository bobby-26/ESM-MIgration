using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogORB2CleaningCargo : PhoenixBasePage
{

    string ReportCode;
    string ItemNo = "27";
    int usercode = 0;
    int vesselId = 0;
    Guid txid = Guid.Empty;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "CleaningCargoTank");
        confirm.Attributes.Add("style", "display:none;");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlCargoTankCleaned.Enabled = false;
            ddlWaterTransferedFrom.Enabled = false;
            ddlWashWaterTransferedTo.Enabled = false;
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
        if (string.IsNullOrWhiteSpace(Request.QueryString["missedOperationDate"]) == false)
        {
            missedOperationDate = Convert.ToDateTime(Request.QueryString["missedOperationDate"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            ViewState["lastTranscationDate"] = null;
            ddlCargoPopulate();
            ddlWaterTransferedFromPoulate();
            ddlWashWaterTransferedToPopulate();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
            ShowHideControls();
            LoadNotes();
        }
    }

    private void LoadNotes()
    {
        DataTable dt = PhoenixMarbolLogORB2.ORB2LogRegisterEdit(usercode, General.GetNullableInteger(logId.ToString()));
        if (dt.Rows.Count > 0)
        {
            lblnotes.Text = dt.Rows[0]["FLDNOTES"].ToString();
        }
    }

    private void BindData()
    {
        if (txid != Guid.Empty)
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, txid);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 33)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 34)
                    {
                        RadComboBoxItem fromItem = ddlCargoTankCleaned.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (fromItem != null)
                        {
                            fromItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 35)
                    {
                        txtCOWStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                        
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 36)
                    {
                        txtCOWStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 37)
                    {
                        txtWashWaterTransfered.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 38)
                    {
                        RadComboBoxItem toItem = ddlWaterTransferedFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (toItem != null)
                        {
                            toItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 39)
                    {
                        txtMethodCleaning.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 40)
                    {
                        txtStartPosistionLat.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 41)
                    {
                        txtStartPosistionLog.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 42)
                    {
                        txtStopPosistionLat.Text = (string)row["FLDVALUE"];
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 43)
                    {
                        txtStopPosistionLog.Text = (string)row["FLDVALUE"];
                    }

                    if (ddlWaterTransferedFrom.SelectedItem != null && ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
                    {

                        if (Convert.ToInt32(row["FLDITEMID"]) == 125)
                        {
                            RadComboBoxItem toItem = ddlWashWaterTransferedTo.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                            if (toItem != null)
                            {
                                toItem.Selected = true;
                            }
                        }

                        if (Convert.ToInt32(row["FLDITEMID"]) == 126)
                        {
                            txtTotalROB.Text = (string)row["FLDVALUE"];
                        }

                    } else
                    {
                        if (Convert.ToInt32(row["FLDITEMID"]) == 125)
                        {
                            txtfacility.Text = (string)row["FLDVALUE"];
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

    private void ddlCargoPopulate()
    {
        ddlCargoTankCleaned.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlCargoTankCleaned.DataBind();
        if (ddlCargoTankCleaned.Items.Count > 0)
        {
            ddlCargoTankCleaned.Items[0].Selected = true;
        }
    }

    private void ddlWashWaterTransferedToPopulate()
    {
        ddlWashWaterTransferedTo.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlWashWaterTransferedTo.DataBind();
        if (ddlWashWaterTransferedTo.Items.Count > 0)
        {
            ddlWashWaterTransferedTo.Items[0].Selected = true;
        }
    }

    private void ddlWaterTransferedFromPoulate()
    {
        List<string> list = new List<string>();
        list.Add("Slops");
        list.Add("Reception");
        ddlWaterTransferedFrom.DataSource = list;
        ddlWaterTransferedFrom.DataBind();
        if (ddlWaterTransferedFrom.Items.Count > 0)
        {
            ddlWaterTransferedFrom.Items[0].Selected = true;
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
        if (ddlCargoTankCleaned.SelectedItem == null) return;

        ReportCode = "G";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtCode.Text = ReportCode;

        txtItemNo.Text = ItemNo;
        txtItemNo1.Text = "28";
        txtItemNo2.Text = "29";
        txtItemNo3.Text = "30";

        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
        {
            txtItemNo4.Text = "31.2";
        } else
        {
            txtItemNo4.Text = "31.1";
        }


        TimeSpan startTime = txtCOWStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtCOWStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtCOWStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtCOWStopTime.SelectedTime.Value;
        lblRecord.Text = string.Format("<b>{0}</b>", ddlCargoTankCleaned.SelectedItem.Text);
        lblrecord1.Text = string.Format("Start:  <b>{0},{1}</b>, <br/> Stop: <b>{2},{3}</b>", txtStartPosistionLat.Text, txtStartPosistionLog.Text, txtStopPosistionLat.Text, txtStopPosistionLog.Text);
        lblrecord2.Text = string.Format("<b>{0}</b> Hours", Math.Abs(stopTime.Hours - startTime.Hours));
        lblrecord3.Text = string.Format("<b>{0}</b>", txtMethodCleaning.Text);

        string slopTemplate = string.Format("<b>{0} quantity transferred {1} m3 / Total Quantity  {2} m3</b>", ddlWashWaterTransferedTo.SelectedItem.Text, txtWashWaterTransfered.Text, txtTotalROB.Text);
        string facilityTemplate = string.Format("<b>Reception Facilities {0} {1} m3 </b>", txtfacility.Text, txtWashWaterTransfered.Text);
        string record4 = ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops" ? slopTemplate: facilityTemplate ;
        lblrecord4.Text = record4;
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

                if (isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                //Validate transaction Capacity
                TransactionValidation();
                SaveTransaction();

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
        PhoenixMarbolLogORB2.InsertTransactionORB2Validation(usercode
                            , new Guid(ddlWashWaterTransferedTo.SelectedItem.Value)
                            , null
                            , txtTotalROB.Text
                            , vesselId
                            , General.GetNullableInteger(lblinchId.Text)
                            );
    }

    private void SaveTransaction()
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
            PhoenixMarbolLogORB2.BookEntryInsert(
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

            PhoenixMarbolLogORB2.BookEntryInsert(
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

            PhoenixMarbolLogORB2.BookEntryInsert(
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

            PhoenixMarbolLogORB2.BookEntryInsert(
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

            PhoenixMarbolLogORB2.BookEntryInsert(
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

            PhoenixMarbolLogORB2.BookEntryInsert(
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
            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
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

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (isValidInput() == false)
            {
                ucError.Visible = true;
                return;
            }

            SaveTransaction();

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


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblRecord.Text,
            1
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord1.Text,
            2
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord2.Text,
            3
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord3.Text,
            4
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            lblrecord4.Text,
            5
        );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode,
            vesselId,
            txid,
            logId,
            entrydate,
            PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true),
            6
        );

        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
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

        PhoenixMarbolLogORB2.TransactionUpdate(
                           usercode,
                           vesselId,
                           logId,
                           33,
                           txid,
                           entrydate,
                           entrydate.ToString()
           );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            34,
                            txid,
                            entrydate,
                            ddlCargoTankCleaned.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            35,
                            txid,
                            entrydate,
                            txtCOWStartTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            36,
                            txid,
                            entrydate,
                            txtCOWStopTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            37,
                            txid,
                            entrydate,
                            txtWashWaterTransfered.Text
            );


        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            38,
                            txid,
                            entrydate,
                            ddlWaterTransferedFrom.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            39,
                            txid,
                            entrydate,
                            txtMethodCleaning.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            40,
                            txid,
                            entrydate,
                            txtStartPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            41,
                            txid,
                            entrydate,
                            txtStartPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            42,
                            txid,
                            entrydate,
                            txtStopPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            43,
                            txid,
                            entrydate,
                            txtStopPosistionLog.Text
            );

        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
        {

            PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            125,
                            txid,
                            entrydate,
                            ddlWashWaterTransferedTo.SelectedItem.Text
            );

            PhoenixMarbolLogORB2.TransactionUpdate(
                                usercode,
                                vesselId,
                                logId,
                                126,
                                txid,
                                entrydate,
                                txtTotalROB.Text
                );

        }
        else
        {

            PhoenixMarbolLogORB2.TransactionUpdate(
                            usercode,
                            vesselId,
                            logId,
                            125,
                            txid,
                            entrydate,
                            txtfacility.Text
            );

        }

    }

    private void TranscationInsert(DateTime entrydate, Guid logTxId, int logId)
    {
        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            33,
                            logTxId,
                            entrydate,
                            entrydate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            34,
                            logTxId,
                            entrydate,
                            ddlCargoTankCleaned.SelectedItem.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            35,
                            logTxId,
                            entrydate,
                            txtCOWStartTime.SelectedDate.ToString()
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            36,
                            logTxId,
                            entrydate,
                            txtCOWStopTime.SelectedDate.ToString()
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            37,
                            logTxId,
                            entrydate,
                            txtWashWaterTransfered.Text
            );


        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            38,
                            logTxId,
                            entrydate,
                            ddlWaterTransferedFrom.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            39,
                            logTxId,
                            entrydate,
                            txtMethodCleaning.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            40,
                            logTxId,
                            entrydate,
                            txtStartPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            41,
                            logTxId,
                            entrydate,
                            txtStartPosistionLog.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            42,
                            logTxId,
                            entrydate,
                            txtStopPosistionLat.Text
            );

        PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            43,
                            logTxId,
                            entrydate,
                            txtStopPosistionLog.Text
            );

        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
        {

            PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            125,
                            logTxId,
                            entrydate,
                            ddlWashWaterTransferedTo.SelectedItem.Text
            );

            PhoenixMarbolLogORB2.TransactionInsert(
                                usercode,
                                vesselId,
                                logId,
                                126,
                                logTxId,
                                entrydate,
                                txtTotalROB.Text
                );

        } else
        {

            PhoenixMarbolLogORB2.TransactionInsert(
                            usercode,
                            vesselId,
                            logId,
                            125,
                            logTxId,
                            entrydate,
                            txtfacility.Text
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

        if (string.IsNullOrWhiteSpace(ddlCargoTankCleaned.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Cargo Tank value is required";
        }

        if (txtCOWStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }

        if (txtCOWStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }


        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position Longitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text))
        {
            ucError.ErrorMessage = "Stop Position Latitude is Required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position Longitude is Required";
        }

        if (string.IsNullOrWhiteSpace(ddlWaterTransferedFrom.SelectedItem.Text))
        {
            ucError.ErrorMessage = "Water Transferred from is required";
        }

        if (string.IsNullOrWhiteSpace(txtMethodCleaning.Text))
        {
            ucError.ErrorMessage = "Method of Cleaning is required";
        }

        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops" && string.IsNullOrWhiteSpace(txtTotalROB.Text))
        {
            ucError.ErrorMessage = "Total ROB is required";
        }

        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() != "slops" && string.IsNullOrWhiteSpace(txtfacility.Text))
        {
            ucError.ErrorMessage = "Name of Reception Facility/Port is required";
        }

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    protected void txtOperationDate_SelectedDateChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void txtAfrTrnsROBFrom_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            SaveTransaction();
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        //string popupName = "Log";
        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log";
        string rank = "co";
        string popupTitle = "Chief Officer Signature";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogOfficerSignature.aspx?popupname={0}&rankName={1}&popupTitle={2}&isMissedOperation={3}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, rank, popupTitle, isMissedOperation);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }

    private bool isValidateSignature()
    {

        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Chief Officer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    protected void ddlWaterTransferedFrom_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ShowHideControls();
        SetDefaultData();
    }

    private void ShowHideControls()
    {
        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
        {
            lblFacility.Visible = false;
            txtfacility.Visible = false;

            lblWashWaterTransferred.Visible = true;
            ddlWashWaterTransferedTo.Visible = true;
            lblTotalROB.Visible = true;
            txtTotalROB.Visible = true;
            lblrobunit.Visible = true;
        }
        else
        {
            lblTotalROB.Visible = false;
            txtTotalROB.Visible = false;
            lblrobunit.Visible = false;
            lblWashWaterTransferred.Visible = false;
            ddlWashWaterTransferedTo.Visible = false;

            lblFacility.Visible = true;
            txtfacility.Visible = true;
        }
    }
}
using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


public partial class Log_ElectricLogORB2BallastingCargoTanks : PhoenixBasePage
{

    string ReportCode;
    string ItemNo = "18";
    //string ItemName = "Sludge";
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    Guid txid = Guid.Empty;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "BallastingCargoTank");
        confirm.Attributes.Add("style", "display:none;");


        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
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
            ddlFromPopulate();
            SetDefaultData();
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            GetLastTranscationDate();
            BindData();
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



    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }
    private void ddlFromPopulate()
    {
        ddlTransferFrom.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlTransferFrom.DataBind();
        if (ddlTransferFrom.Items.Count > 0)
        {
            ddlTransferFrom.Items[0].Selected = true;
        }
    }
    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }

    private void BindData()
    {
        if (txid.ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(txid.ToString()));

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
                        ddlTransferFrom.SelectedValue = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 28)
                    {
                        txtStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 29)
                    {
                        txtStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 30)
                    {
                        txtquantity.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 31)
                    {
                        txtStartPosistionLat.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 32)
                    {
                        txtStopPosistionLat.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 123)
                    {
                        txtStartPosistionLog.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 124)
                    {
                        txtStopPosistionLog.Text = row["FLDVALUE"].ToString();
                    }

                }
                SetDefaultData();
            }
        }
    }
    private void SetDefaultData()
    {
        ReportCode = "E";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
    
        txtItemNo1.Text = "19.1";
        txtItemNo2.Text = "19.2";
        txtItemNo3.Text = "19.3";

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;

        lblRecord.Text = string.Format("Start: <b> {0},{1}</b>  Stop:  <b> {2},{3}</b>", txtStartPosistionLat.Text, txtStartPosistionLog.Text, txtStopPosistionLat.Text, txtStopPosistionLog.Text);
        lblrecord1.Text = string.Format("<b>{0}</b>", ddlTransferFrom.Text);
        lblrecord2.Text = string.Format("Start at  <b>{0}</b>, Stop at <b>{1}</b>", startTime.ToString(),stopTime.ToString());
        lblrecord3.Text = string.Format("Total Quantity received in <b>{0}</b> = <b>{1}</b> m3 ", ddlTransferFrom.Text,txtquantity.Text);
        
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }
           if (string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position Longitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text))
        {
            ucError.ErrorMessage = "Start Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text))
        {
            ucError.ErrorMessage = "Stop Position Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position Longitude is required";
        }
       
        if (string.IsNullOrWhiteSpace(txtquantity.Text))
        {
            ucError.ErrorMessage = "Total Quantity of Ballast Received in above tank is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }
        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }

        if (PhoenixMarbolLogORB2.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && txid == Guid.Empty)
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    private void TransactionUpdate()
    {

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 26
                                    , txid
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 27
                                    , txid
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(ddlTransferFrom.SelectedValue)
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 28
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(startTime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 29
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(stopTime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 30
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtquantity.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 31
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 32
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStopPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 123
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 124
                            , txid
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStopPosistionLog.Text)

                        );
    }


    private void TranscationInsert(Guid logTxId, int logId)
    {
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 26
                                    , logTxId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 27
                                    , logTxId
                                    , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                    , General.GetNullableString(ddlTransferFrom.SelectedValue)

                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 28
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(startTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 29
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(stopTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 30
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtquantity.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 31
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 32
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStopPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 123
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 124
                            , logTxId
                            , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                            , General.GetNullableString(txtStopPosistionLog.Text)

                        );

    }


    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string rank = PhoenixElog.GetRankName(usercode);

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
                            , new Guid(ddlTransferFrom.SelectedItem.Value)
                            , null
                            , txtquantity.Text
                            , vesselId
                            , General.GetNullableInteger(lblinchId.Text)
                            );
    }

    private void SaveTransaction()
    {
        if (isMissedOperation && isMissedOperationEdit == false)
        {
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(logTxId, logId);
            MissedOperationalEntryTemplateUpdate(logTxId);            
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            TransactionUpdate();
            MissedOperationalEntryTemplateUpdate(txid);
        }
        else if (txid != Guid.Empty && isMissedOperationEdit == false)
        {
            TransactionUpdate();
            LogBookUpdate();
        }
        else
        {

            DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
            Guid logTxId = Guid.NewGuid();
            TranscationInsert(logTxId, logId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , logTxId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , logTxId
                                     , logId
                                     , entrydate
                                     , null
                                     , txtItemNo1.Text
                                     , lblrecord1.Text
                                     , 2
                                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                    , vesselId
                                    , logTxId
                                    , logId
                                    , entrydate
                                    , null
                                    , txtItemNo2.Text
                                    , lblrecord2.Text
                                    , 3
                                );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , logTxId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo3.Text
                     , lblrecord3.Text
                     , 4
                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , logTxId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 5
                                      , true
                                );

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
        nvc.Add("Record1", lblRecord.Text);
        nvc.Add("Record2", lblrecord1.Text);
        nvc.Add("Record3", lblrecord2.Text);
        nvc.Add("Record4", lblrecord3.Text);

        nvc.Add("ReportCode1", txtCode.Text);
        nvc.Add("ItemNo1", txtItemNo.Text);
        nvc.Add("ItemNo2", txtItemNo1.Text);
        nvc.Add("ItemNo3", txtItemNo2.Text);
        nvc.Add("ItemNo4", txtItemNo3.Text);
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "4");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void LogBookUpdate()
    {

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , txid
                                  , logId
                                  , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                  , lblRecord.Text
                                  , 1
                              );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , txid
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord1.Text
                                 , 2
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , txid
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord2.Text
                                 , 3
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , txid
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , lblrecord3.Text
                                 , 4
                             );



        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , txid
                                 , logId
                                 , General.GetNullableDateTime(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 5
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
    private void OnDutyEngineerSign()
    {
        if (Filter.DutyEngineerSignatureFilterCriteria != null)
        {
            NameValueCollection nvc = Filter.DutyEngineerSignatureFilterCriteria;
            DateTime signedDate = DateTime.Parse(nvc.Get("date"));
            lblincRank.Text = nvc.Get("rank");
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
   
    protected void txt_selectedindexchaged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
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



    protected void txt_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtAfrTrnsROBFrom_TextChangedEvent(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void txtFailureReason_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
}
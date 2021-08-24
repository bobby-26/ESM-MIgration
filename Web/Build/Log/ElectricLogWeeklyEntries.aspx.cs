using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;


public partial class Log_ElectricLogWeeklyEntries : PhoenixBasePage
{
    int usercode = 0;
    int vesselid = 0;
    Guid? txid = null;
    Guid? logbookid = null;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    DateTime? missedOperationDate = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        confirm.Attributes.Add("style", "display:none;");

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            txid = Guid.Parse(Request.QueryString["TxnId"]);
            txtOperationDate.Enabled = false;
        }
        if (string.IsNullOrWhiteSpace(Request.QueryString["LogBookId"]) == false)
        {
            logbookid = Guid.Parse(Request.QueryString["LogBookId"]);
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
            txtOperationDate.SelectedDate = DateTime.Now;
            txtOperationDate.MaxDate = DateTime.Now;
            SetDefaultData();
            GetLastTranscationDate(); // for validation 
            OnDutyEngineerSign();

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

    public void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false: true;
    }
    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                // validation and proccedd
                if (isValidInput() == false || isValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                TransactionValidation();
                saveTxin();

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
        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {

            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            PhoenixElog.InsertTransactionValidation(usercode
                                , new Guid(lblLocationId.Text)
                                    , lblBeforeROB.Text
                                    , lblROB.Text
                                    , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                    , General.GetNullableInteger(lblinchId.Text)
                                    , General.GetNullableInteger(null)
                                    , null
                                    , null
                                );
        }
    }

    private void MissedOperationInsert(Guid logId, string logBookName)
    {
        Guid TranscationId = Guid.Empty;
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixElog.MissedOperationalEntryInsert(usercode
                                            , vesselid
                                            , logId
                                            , logBookName
                                            , ref TranscationId
                                            , entrydate
                                            , Convert.ToDateTime(missedOperationDate)
                                            , false
                        );
    }

    private void saveTxin()
    {
        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {

            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            PhoenixElog.InsertTransactionValidation(usercode
                                , new Guid(lblLocationId.Text)
                                    , lblBeforeROB.Text
                                    , lblROB.Text
                                    , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                    , General.GetNullableInteger(lblinchId.Text)
                                    , General.GetNullableInteger(null)
                                , null
                                , null
                                );
        }

        Guid WeeklyId = PhoenixElog.GetProcessId(usercode, "WEL");
        string logName = "Weekly Entries";


        if (isMissedOperation && isMissedOperationEdit == false)
        {
            AddMissedOperation(logName, WeeklyId);
        }
        else if (isMissedOperation && isMissedOperationEdit)
        {
            UpdateMissedOperation(logName, WeeklyId);
        }
        else if ((txid != Guid.Empty && txid != null) && isMissedOperationEdit == false)
        {
            TranscationUpdate();
            LogBookUpdate();
        }
        else
        {
            Guid TranscationId = Guid.Empty;
            foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
            {
                Guid logId = Guid.NewGuid();
                int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselid);
                RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
                RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
                RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
                RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
                RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
                RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

                RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
                RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
                RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
                RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

                RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
                RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

                RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
                RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
                DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);



                PhoenixElog.InsertTransactionNew(usercode
                        , lblCode.Text
                        , new Guid(lblLocationId.Text)
                        , lblTankName.Text
                        , WeeklyId
                        , "Weekly Entry"
                        , lblCode.Text
                        , lblItemNo1.Text
                        , "Sludge"
                        , lblBeforeROB.Text
                        , lblROB.Text
                        , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                        , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , General.GetNullableInteger(lblinchId.Text)
                        , General.GetNullableString(lblincRank.Text)
                        , General.GetNullableString(lblincsign.Text)
                        , General.GetNullableDateTime(lblincSignDate.Text)
                        , General.GetNullableInteger(null)
                        , General.GetNullableString(null)
                        , General.GetNullableString(null)
                        , General.GetNullableDateTime(null)
                        , null
                        , ref TranscationId
                        , null
                        , logId
                        , "Weekly Entries"
                        );


                PhoenixElog.LogBookEntryInsert(usercode
                        , entryDate
                        , lblItemNo1.Text
                        , "Name of tank : " + lblTankName.Text
                        , lblCode.Text
                        , TranscationId
                        , 1
                        , null
                        , null
                        , General.GetNullableString(lblincRank.Text)
                        , vesselid
                        , true
                        , null
                        , logName
                         , false
                         , entryNo
                         , logId
                        );

                PhoenixElog.LogBookEntryInsert(usercode
                , entryDate
                , lblItemNo2.Text
                , "Capacity " + lblCapacity.Text + " m3"
                , null
                , TranscationId
                , 2
                , null
                , null
                , General.GetNullableString(lblincRank.Text)
                , vesselid
                , true
                , null
                , logName
                 , false
                 , entryNo
                 , logId
                );

                PhoenixElog.LogBookEntryInsert(usercode
                , entryDate
                , lblItemNo3.Text
                , "Total retained on board " + lblROB.Text + " m3"
                , null
                , TranscationId
                , 3
                , null
                , null
                , General.GetNullableString(lblincRank.Text)
                , vesselid
                , true
                , null
                , logName
                 , false
                 , entryNo
                 , logId
                );

                PhoenixElog.LogBookEntryInsert(usercode
                , entryDate
                , null
                , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                , null
                , TranscationId
                , 4
                , false
                , null
                , General.GetNullableString(lblincRank.Text)
                , vesselid
                , true
                , null
                , logName
                 , false
                 , entryNo
                 , logId
                );

                PhoenixElog.LogBookEntryInsert(usercode
                , entryDate
                , null
                , PhoenixElog.GetSignatureName("", "", null, true)
                , null
                , TranscationId
                , 5
                , true
                , null
                , General.GetNullableString(lblincRank.Text)
                , vesselid
                , true
                , null
                , logName
                 , false
                 , entryNo
                 , logId
                );

            }


            foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
            {
                Guid logId = Guid.NewGuid();
                int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselid);
                RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
                RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
                RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
                RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
                RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
                RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
                RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

                RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
                RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
                RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
                RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

                RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
                RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

                RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
                RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
                DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


                PhoenixElog.InsertTransactionNew(usercode
                                                , lblCode.Text
                                                , new Guid(lblLocationId.Text)
                                                , lblTankName.Text
                                                , WeeklyId
                                                , "Weekly Entry"
                                                , lblCode.Text
                                                , null
                                                , "Bilge"
                                                , lblBeforeROB.Text
                                                , lblROB.Text
                                                , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                                , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                , General.GetNullableInteger(lblinchId.Text)
                                                , General.GetNullableString(lblincRank.Text)
                                                , General.GetNullableString(lblincsign.Text)
                                                , General.GetNullableDateTime(lblincSignDate.Text)
                                                , General.GetNullableInteger(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableString(null)
                                                , General.GetNullableDateTime(null)
                                                , null
                                                , ref TranscationId
                                                , null
                                                , logId
                                                , "Weekly Entries"
                                            );


                PhoenixElog.LogBookEntryInsert(usercode
                                           , entryDate
                                           , lblItemNo1.Text
                                           , lblRecord.Text
                                           , lblCode.Text
                                           , TranscationId
                                           , 1
                                           , null
                                           , null
                                           , General.GetNullableString(lblincRank.Text)
                                           , vesselid
                                           , true
                                           , null
                                           , logName
                                            , false
                                            , entryNo
                                            , logId
                                       );



                PhoenixElog.LogBookEntryInsert(usercode
                                           , entryDate
                                           , null
                                           , "Name of tank : " + lblTankName.Text
                                           , null
                                           , TranscationId
                                           , 2
                                           , null
                                           , null
                                           , General.GetNullableString(lblincRank.Text)
                                           , vesselid
                                           , true
                                           , null
                                           , logName
                                            , false
                                            , entryNo
                                            , logId
                                       );

                PhoenixElog.LogBookEntryInsert(usercode
                                   , entryDate
                                   , lblItemNo2.Text
                                   , "Capacity " + lblCapacity.Text + " m3"
                                   , null
                                   , TranscationId
                                   , 3
                                   , null
                                   , null
                                   , General.GetNullableString(lblincRank.Text)
                                   , vesselid
                                   , true
                                   , null
                                   , logName
                                    , false
                                    , entryNo
                                    , logId
                               );

                PhoenixElog.LogBookEntryInsert(usercode
                                        , entryDate
                                        , lblItemNo3.Text
                                        , "Total retained on board " + lblROB.Text + " m3"
                                        , null
                                        , TranscationId
                                        , 4
                                        , null
                                   , null
                                   , General.GetNullableString(lblincRank.Text)
                                   , vesselid
                                   , true
                                   , null
                                   , logName
                                    , false
                                    , entryNo
                                    , logId
                                  );


                PhoenixElog.LogBookEntryInsert(usercode
                          , entryDate
                          , null
                          , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                          , null
                          , TranscationId
                          , 5
                          , false
                          , null
                           , General.GetNullableString(lblincRank.Text)
                           , vesselid
                           , true
                           , null
                           , logName
                           , false
                           , entryNo
                           , logId
                    );

                PhoenixElog.LogBookEntryInsert(usercode
                                          , entryDate
                                          , null
                                          , PhoenixElog.GetSignatureName("", "", null, true)
                                          , null
                                          , TranscationId
                                          , 6
                                          , true
                                          , null
                                        , General.GetNullableString(lblincRank.Text)
                                        , vesselid
                                        , true
                                        , null
                                        , logName
                                        , false
                                        , entryNo
                                        , logId
                                    );
            }
        }
    }

    private void AddMissedOperation(string logName, Guid WeeklyId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        Guid logId = Guid.Empty;
        Guid TranscationId = Guid.Empty;

        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselid);
        int counter = 1;
        for (int i = 0; i < gvWeeklyEntrySludge.Items.Count; i++)
        {
            logId = Guid.NewGuid();

            MissedOperationInsert(logId, logName);

            GridDataItem item = gvWeeklyEntrySludge.Items[i];
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            PhoenixElog.InsertTransactionNew(usercode
                    , lblCode.Text
                    , new Guid(lblLocationId.Text)
                    , lblTankName.Text
                    , WeeklyId
                    , "Weekly Entry"
                    , lblCode.Text
                    , lblItemNo1.Text
                    , "Sludge"
                    , lblBeforeROB.Text
                    , lblROB.Text
                    , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                    , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableInteger(lblinchId.Text)
                    , General.GetNullableString(lblincRank.Text)
                    , General.GetNullableString(lblincsign.Text)
                    , General.GetNullableDateTime(lblincSignDate.Text)
                    , General.GetNullableInteger(null)
                    , General.GetNullableString(null)
                    , General.GetNullableString(null)
                    , General.GetNullableDateTime(null)
                    , null
                    , ref TranscationId
                    , null
                    , logId
                    , null
                    , null
                    , "Weekly Entries"
                    , isMissedOperation
                    );


            nvc.Add("ReportCode" + counter, lblCode.Text);
            nvc.Add("Record" + counter, "Name of tank : " + lblTankName.Text);
            nvc.Add("Record" + (counter + 1), "Capacity " + lblCapacity.Text + " m3");
            nvc.Add("Record" + (counter + 2), "Total retained on board " + lblROB.Text + " m3");

            nvc.Add("ItemNo" + counter, lblItemNo1.Text);
            nvc.Add("ItemNo" + (counter + 1), lblItemNo2.Text);
            nvc.Add("ItemNo" + (counter + 2), lblItemNo3.Text);
            nvc.Add("logId" + counter, logId.ToString());


            nvc.Add("txId" + counter, TranscationId.ToString());

            counter = counter + 3;
        }


        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            logId = Guid.NewGuid();
            MissedOperationInsert(logId, logName);

            TranscationId = Guid.Empty;
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            PhoenixElog.InsertTransactionNew(usercode
                                            , lblCode.Text
                                            , new Guid(lblLocationId.Text)
                                            , "Name of tank : " + lblTankName.Text
                                            , WeeklyId
                                            , "Weekly Entry"
                                            , lblCode.Text
                                            , null
                                            , "Bilge"
                                            , lblBeforeROB.Text
                                            , lblROB.Text
                                            , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                            , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , General.GetNullableInteger(lblinchId.Text)
                                            , General.GetNullableString(lblincRank.Text)
                                            , General.GetNullableString(lblincsign.Text)
                                            , General.GetNullableDateTime(lblincSignDate.Text)
                                            , General.GetNullableInteger(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableDateTime(null)
                                            , null
                                            , ref TranscationId
                                            , null
                                            , logId
                                            , null
                                            , null
                                            , "Weekly Entries"
                                            , isMissedOperation
                                        );

            nvc.Add("ReportCode" + counter, lblCode.Text);
            nvc.Add("Record" + counter, "Name of tank : " + lblTankName.Text);
            nvc.Add("Record" + (counter + 1), "Capacity " + lblCapacity.Text + " m3");
            nvc.Add("Record" + (counter + 2), "Total retained on board " + lblROB.Text + " m3");
            nvc.Add("txId" + counter, TranscationId.ToString());
            nvc.Add("logId" + counter, logId.ToString());

            nvc.Add("ItemNo" + counter, lblItemNo1.Text);
            nvc.Add("ItemNo" + (counter + 1), lblItemNo2.Text);
            nvc.Add("ItemNo" + (counter + 2), lblItemNo3.Text);

            counter = counter + 3;
        }


        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", (gvSludgeTank.Items.Count + gvBilgeTank.Items.Count).ToString());
        nvc.Add("sludgeTankCount", (gvSludgeTank.Items.Count).ToString());
        nvc.Add("bilgeTankCount", (gvBilgeTank.Items.Count).ToString());
        nvc.Add("logBookName", logName);
        nvc.Add("isTransaction", "true");
        nvc.Add("isWeeklyEntry", "true");
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void UpdateMissedOperation(string logName, Guid WeeklyId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        Guid logId = Guid.NewGuid();
        Guid TranscationId = Guid.Empty;

        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselid);
        int counter = 1;
        for (int i = 0; i < gvWeeklyEntrySludge.Items.Count; i++)
        {

            GridDataItem item = gvWeeklyEntrySludge.Items[i];
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            decimal tnxqty = 0;

            tnxqty = ((General.GetNullableDecimal(lblBeforeROB.Text) != null ? decimal.Parse(lblBeforeROB.Text) : 0)
                        - (General.GetNullableDecimal(lblROB.Text) != null ? decimal.Parse(lblROB.Text) : 0));

            PhoenixElog.UpdateTransactionWeekly(txid.Value
                                  , usercode
                                  , new Guid(lblLocationId.Text)
                                  , lblBeforeROB.Text
                                  , lblROB.Text
                                  , tnxqty.ToString()
                                  , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                  , vesselid
                                  , General.GetNullableInteger(lblinchId.Text)
                                  , General.GetNullableString(lblincRank.Text)
                                  , General.GetNullableString(lblincsign.Text)
                                  , General.GetNullableDateTime(lblincSignDate.Text)
                                  , General.GetNullableInteger(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableDateTime(null)
                                  , null
                              );


            nvc.Add("ReportCode" + counter, lblCode.Text);
            nvc.Add("Record" + counter, "Name of tank : " + lblTankName.Text);
            nvc.Add("Record" + (counter + 1), "Capacity " + lblCapacity.Text + " m3");
            nvc.Add("Record" + (counter + 2), "Total retained on board " + lblROB.Text + " m3");

            nvc.Add("ItemNo" + counter, lblItemNo1.Text);
            nvc.Add("ItemNo" + (counter + 1), lblItemNo2.Text);
            nvc.Add("ItemNo" + (counter + 2), lblItemNo3.Text);


            nvc.Add("txId" + counter, TranscationId.ToString());

            counter = counter + 3;
        }


        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            TranscationId = Guid.Empty;
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            decimal tnxqty = 0;

            tnxqty = ((General.GetNullableDecimal(lblBeforeROB.Text) != null ? decimal.Parse(lblBeforeROB.Text) : 0)
                        - (General.GetNullableDecimal(lblROB.Text) != null ? decimal.Parse(lblROB.Text) : 0));

            PhoenixElog.UpdateTransactionWeekly(txid.Value
                                  , usercode
                                  , new Guid(lblLocationId.Text)
                                  , lblBeforeROB.Text
                                  , lblROB.Text
                                  , tnxqty.ToString()
                                  , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                  , vesselid
                                  , General.GetNullableInteger(lblinchId.Text)
                                  , General.GetNullableString(lblincRank.Text)
                                  , General.GetNullableString(lblincsign.Text)
                                  , General.GetNullableDateTime(lblincSignDate.Text)
                                  , General.GetNullableInteger(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableDateTime(null)
                                  , null
                              );

            nvc.Add("ReportCode" + counter, lblCode.Text);
            nvc.Add("Record" + counter, "Name of tank : " + lblTankName.Text);
            nvc.Add("Record" + (counter + 1), "Capacity " + lblCapacity.Text + " m3");
            nvc.Add("Record" + (counter + 2), "Total retained on board " + lblROB.Text + " m3");
            nvc.Add("txId" + counter, TranscationId.ToString());

            nvc.Add("ItemNo" + counter, lblItemNo1.Text);
            nvc.Add("ItemNo" + (counter + 1), lblItemNo2.Text);
            nvc.Add("ItemNo" + (counter + 2), lblItemNo3.Text);

            counter = counter + 3;
        }


        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", (gvSludgeTank.Items.Count + gvBilgeTank.Items.Count).ToString());
        nvc.Add("sludgeTankCount", (gvSludgeTank.Items.Count).ToString());
        nvc.Add("bilgeTankCount", (gvBilgeTank.Items.Count).ToString());
        nvc.Add("logBookName", logName);
        nvc.Add("isTransaction", "true");
        nvc.Add("isWeeklyEntry", "true");
        nvc.Add("logId", logId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            // validation and proccedd
            if (isValidInput() == false || isValidateSignature() == false)
            {
                ucError.Visible = true;
                return;
            }

            saveTxin();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Log', 'oilLog');", true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void MissedOperationEntryTemplateUpdate(string logName, Guid logId)
    {
        NameValueCollection nvc = new NameValueCollection();
        // add for logbook
        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        for (int i = 0; i < gvWeeklyEntrySludge.Items.Count; i++)
        {
            GridDataItem item = gvWeeklyEntrySludge.Items[i];
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");


            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

            nvc.Add("ReportCode" + i, lblCode.Text);
            nvc.Add("Record" + i, "Name of tank : " + lblTankName.Text);
            nvc.Add("Record" + (i +1), "Capacity " + lblCapacity.Text + " m3");
            nvc.Add("Record" + (i + 2), "Total retained on board " + lblROB.Text + " m3");
            nvc.Add("ReportCode" + i, lblCode.Text);

        }

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text)));
        // add meta data for the log
        nvc.Add("logBookEntry", (gvWeeklyEntrySludge.Items.Count * 3).ToString());
        nvc.Add("logBookName", logName);
        nvc.Add("isTransaction", "true");
        nvc.Add("isWeeklyEntry", "true");
        nvc.Add("logId", logId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }

    private void TranscationUpdate()
    {
        Guid TranscationId = Guid.Empty;
        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");

            decimal tnxqty = 0;

            tnxqty = ((General.GetNullableDecimal(lblBeforeROB.Text) != null ? decimal.Parse(lblBeforeROB.Text) : 0)
                        - (General.GetNullableDecimal(lblROB.Text) != null ? decimal.Parse(lblROB.Text) : 0));

            PhoenixElog.UpdateTransactionWeekly(txid.Value
                                  , usercode
                                  , new Guid(lblLocationId.Text)
                                  , lblBeforeROB.Text
                                  , lblROB.Text
                                  , tnxqty.ToString()
                                  , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                  , vesselid
                                  , General.GetNullableInteger(lblinchId.Text)
                                  , General.GetNullableString(lblincRank.Text)
                                  , General.GetNullableString(lblincsign.Text)
                                  , General.GetNullableDateTime(lblincSignDate.Text)
                                  , General.GetNullableInteger(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableString(null)
                                  , General.GetNullableDateTime(null)
                                  , null
                              );

        }

        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");

            PhoenixElog.UpdateTransactionWeekly(txid.Value
                                 , usercode
                                 , new Guid(lblLocationId.Text)
                                 , lblBeforeROB.Text
                                 , lblROB.Text
                                 , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                 , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                 , vesselid
                                 , General.GetNullableInteger(lblinchId.Text)
                                 , General.GetNullableString(lblincRank.Text)
                                 , General.GetNullableString(lblincsign.Text)
                                 , General.GetNullableDateTime(lblincSignDate.Text)
                                 , General.GetNullableInteger(null)
                                 , General.GetNullableString(null)
                                 , General.GetNullableString(null)
                                 , General.GetNullableDateTime(null)
                                 , null
                             );

        }
    }

    private void TranscationInsert(Guid WeeklyId, Guid logId)
    {
        Guid TranscationId = Guid.Empty;
        int entryNo = PhoenixElog.GetLogBookNextEntryNo(usercode, vesselid);
        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            PhoenixElog.InsertTransactionNew(usercode
                    , lblCode.Text
                    , new Guid(lblLocationId.Text)
                    , lblTankName.Text
                    , WeeklyId
                    , "Weekly Entry"
                    , lblCode.Text
                    , lblItemNo1.Text
                    , "Sludge"
                    , lblBeforeROB.Text
                    , lblROB.Text
                    , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                    , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , General.GetNullableInteger(lblinchId.Text)
                    , General.GetNullableString(lblincRank.Text)
                    , General.GetNullableString(lblincsign.Text)
                    , General.GetNullableDateTime(lblincSignDate.Text)
                    , General.GetNullableInteger(null)
                    , General.GetNullableString(null)
                    , General.GetNullableString(null)
                    , General.GetNullableDateTime(null)
                    , null
                    , ref TranscationId
                    , null
                    , logId
                    , null
                    , null
                    , "Weekly Entries"
                    , isMissedOperation
                    );

        }


        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");
            DateTime entryDate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);


            PhoenixElog.InsertTransactionNew(usercode
                                            , lblCode.Text
                                            , new Guid(lblLocationId.Text)
                                            , "Name of tank : " + lblTankName.Text
                                            , WeeklyId
                                            , "Weekly Entry"
                                            , lblCode.Text
                                            , null
                                            , "Bilge"
                                            , lblBeforeROB.Text
                                            , lblROB.Text
                                            , (Convert.ToDecimal(lblBeforeROB.Text) - Convert.ToDecimal(lblROB.Text)).ToString()
                                            , txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy")
                                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                            , General.GetNullableInteger(lblinchId.Text)
                                            , General.GetNullableString(lblincRank.Text)
                                            , General.GetNullableString(lblincsign.Text)
                                            , General.GetNullableDateTime(lblincSignDate.Text)
                                            , General.GetNullableInteger(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableString(null)
                                            , General.GetNullableDateTime(null)
                                            , null
                                            , ref TranscationId
                                            , null
                                            , logId
                                            , null
                                            , null
                                            , "Weekly Entries"
                                            , isMissedOperation
                                        );
        }
    }

    private void LogBookUpdate()
    {
        Guid TranscationId = Guid.Empty;
        Guid WeeklyId = PhoenixElog.GetProcessId(usercode, "WEL");

        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");

            decimal tnxqty = 0;

            tnxqty = ((General.GetNullableDecimal(lblBeforeROB.Text) != null ? decimal.Parse(lblBeforeROB.Text) : 0)
                        - (General.GetNullableDecimal(lblROB.Text) != null ? decimal.Parse(lblROB.Text) : 0));


            PhoenixElog.LogBookEntryUpdate(usercode
                    , "Name of tank : " + lblTankName.Text
                    , txid.Value
                    , 1
                    , null
                    , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , "Capacity " + lblCapacity.Text + " m3"
                            , txid.Value
                            , 2
                            , null
                            , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , "Total retained on board " + lblROB.Text + "m3"
                            , txid.Value
                            , 3
                            , null
                            , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                            , txid.Value
                            , 4
                            , false
                            , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName("", "", null, true)
                            , txid.Value
                            , 5
                            , true
                            , logbookid);

        }

        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            RadLabel lblRecord = (RadLabel)item.FindControl("lblRecord");
            RadLabel lblDate = (RadLabel)item.FindControl("lblDate");
            RadLabel lblItemNo1 = (RadLabel)item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)item.FindControl("lblItemNo3");
            RadLabel lblCode = (RadLabel)item.FindControl("lblCode");
            RadLabel lblLocationId = (RadLabel)item.FindControl("lblLocationId");

            RadLabel lblTankName = (RadLabel)item.FindControl("lblTankName");
            RadLabel lblCapacity = (RadLabel)item.FindControl("lblCapacity");
            RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
            RadLabel lblBeforeROB = (RadLabel)item.FindControl("lblBeforeROB");

            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            RadLabel lblInchargeSigndate = (RadLabel)item.FindControl("lblInchargeSigndate");

            RadLabel lblCheifOfficerInchargeSign = (RadLabel)item.FindControl("lblCheifOfficerInchargeSign");
            RadLabel lblCheifOfficerSigndate = (RadLabel)item.FindControl("lblCheifOfficerSigndate");


            PhoenixElog.LogBookEntryUpdate(usercode
                    , lblRecord.Text
                    , txid.Value
                    , 1
                    , null
                    , logbookid);



            PhoenixElog.LogBookEntryUpdate(usercode
                    , "Name of tank : " + lblTankName.Text
                    , txid.Value
                    , 2
                    , null
                    , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , "Capacity " + lblCapacity.Text + " m3"
                            , txid.Value
                            , 3
                            , null
                            , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , "Total retained on board " + lblROB.Text + " m3"
                            , txid.Value
                            , 4
                            , null
                            , logbookid);



            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                            , txid.Value
                            , 5
                            , false
                            , logbookid);

            PhoenixElog.LogBookEntryUpdate(usercode
                            , PhoenixElog.GetSignatureName("", "", null, true)
                            , txid.Value
                            , 6
                            , true
                            , logbookid);


        }
    }

    private bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (PhoenixElog.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false && isMissedOperation == false)
        {
            ucError.ErrorMessage = "Date of Operation should be greater or less than Last Transaction date and not greater than current date";
        }

        //foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        //{
        //    RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
        //    if (string.IsNullOrWhiteSpace(lblROB.Text))
        //    {
        //        ucError.ErrorMessage = "ROB is cannot be empty on Sludge Tanks";
        //    }
        //}


        //foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        //{
        //    RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
        //    if (string.IsNullOrWhiteSpace(lblROB.Text))
        //    {
        //        ucError.ErrorMessage = "ROB is cannot be empty on Bilge Tank";
        //    }
        //}

        foreach (GridDataItem item in gvSludgeTank.Items)
        {
            RadNumericTextBox txtSludgeRob = (RadNumericTextBox)item.FindControl("txtSludgeRob");
            if (string.IsNullOrWhiteSpace(txtSludgeRob.Text))
            {
                ucError.ErrorMessage = "ROB is cannot be empty on Sludge Tanks";
                break;
            }
        }

        foreach (GridDataItem item in gvBilgeTank.Items)
        {
            RadNumericTextBox txtBilgeRob = (RadNumericTextBox)item.FindControl("txtBilgeRob");
            if (string.IsNullOrWhiteSpace(txtBilgeRob.Text))
            {
                ucError.ErrorMessage = "ROB is cannot be empty on Bilge Tank";
                break;
            }
        }


        return (!ucError.IsError);
    }

    private bool isValidateSignature()
    {
        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Duty Engineer Signature is Required Before Save";
        }

        return (!ucError.IsError);
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixElog.GetLogLastTranscationDate(vesselid, usercode);
    }

    protected void txtOperationDate_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlTransferTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    private void SetDefaultData()
    {

        if (txid != null)
        {
            DataSet ds = PhoenixElog.TankTypeWiseList(usercode, vesselid, "SLG", txid, logbookid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtOperationDate.SelectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FLDDATE"]);
            }
        }
        else
            txtOperationDate.SelectedDate = DateTime.Now;

        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            lblOfficerInchargeSign.Text = lblincsign.Text;

        }

        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {
            RadLabel lblOfficerInchargeSign = (RadLabel)item.FindControl("lblOfficerInchargeSign");
            lblOfficerInchargeSign.Text = lblincsign.Text;

        }

    }

    #region Grid Releated Events

    protected void gvWeeklyEntrySludge_NeedDataSource(object sender, EventArgs e)
    {
        // coz if we combine into single method then every sp will get hit 8 times
        gvWeeklyEntrySludge.DataSource = PhoenixElog.TankTypeWiseList(usercode, vesselid, "SLG", txid, logbookid);
    }
    protected void gvWeeklyEntrySludge_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            RadLabel lblInchargeSignedDate = (RadLabel)e.Item.FindControl("lblInchargeSigndate");
            RadLabel lblCheifOfficerSignedDate = (RadLabel)e.Item.FindControl("lblCheifOfficerSigndate");
            RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
            RadLabel lblOfficerInchargeSign = (RadLabel)e.Item.FindControl("lblOfficerInchargeSign");
            RadLabel lblCheifOfficerInchargeSign = (RadLabel)e.Item.FindControl("lblCheifOfficerInchargeSign");
            RadTextBox txtSludgeRob = (RadTextBox)e.Item.FindControl("txtSludgeRob");


            RadLabel lblItemNo1 = (RadLabel)e.Item.FindControl("lblItemNo1");
            RadLabel lblItemNo2 = (RadLabel)e.Item.FindControl("lblItemNo2");
            RadLabel lblItemNo3 = (RadLabel)e.Item.FindControl("lblItemNo3");

            lblItemNo1.Text = "11.1";
            lblItemNo2.Text = "11.2";
            lblItemNo3.Text = "11.3";

            if (txtOperationDate.SelectedDate.HasValue)
            {
                DateTime operationDate = txtOperationDate.SelectedDate.Value;
                if (lblInchargeSignedDate != null)
                {
                    lblInchargeSignedDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                if (lblCheifOfficerSignedDate != null)
                {
                    lblCheifOfficerSignedDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                if (lblDate != null)
                {
                    lblDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                //if (lblOfficerInchargeSign != null && string.IsNullOrWhiteSpace(lblincsign.Text) == false)
                //{
                //    lblOfficerInchargeSign.Text = lblincsign.Text;
                //}

                //if (lblCheifOfficerInchargeSign != null && string.IsNullOrWhiteSpace(lblChiefSign.Text) == false)
                //{
                //    lblCheifOfficerInchargeSign.Text = lblChiefSign.Text;
                //}

            }
        }
    }
    protected void gvWeeklyEntrySludge_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

            }
            if (e.CommandName == "VIEW")
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWeeklyEntryBilge_NeedDataSource(object sender, EventArgs e)
    {
        gvWeeklyEntryBilge.DataSource = PhoenixElog.TankTypeWiseList(usercode, vesselid, "BLG", txid, logbookid);
    }


    protected void gvWeeklyEntryBilge_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && e.Item.IsInEditMode == false)
        {
            RadLabel lblInchargeSignedDate = (RadLabel)e.Item.FindControl("lblInchargeSigndate");
            RadLabel lblCheifOfficerSignedDate = (RadLabel)e.Item.FindControl("lblCheifOfficerSigndate");
            RadLabel lblDate = (RadLabel)e.Item.FindControl("lblDate");
            RadLabel lblOfficerInchargeSign = (RadLabel)e.Item.FindControl("lblOfficerInchargeSign");
            RadLabel lblCheifOfficerInchargeSign = (RadLabel)e.Item.FindControl("lblCheifOfficerInchargeSign");


            if (txtOperationDate.SelectedDate.HasValue)
            {
                DateTime operationDate = txtOperationDate.SelectedDate.Value;
                if (lblInchargeSignedDate != null)
                {
                    lblInchargeSignedDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                if (lblCheifOfficerSignedDate != null)
                {
                    lblCheifOfficerSignedDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                if (lblDate != null)
                {
                    lblDate.Text = PhoenixElog.GetElogDateFormat(operationDate);
                }

                //if (lblOfficerInchargeSign != null && string.IsNullOrWhiteSpace(lblincsign.Text) == false)
                //{
                //    lblOfficerInchargeSign.Text = lblincsign.Text;
                //}

                //if (lblCheifOfficerInchargeSign != null && string.IsNullOrWhiteSpace(lblChiefSign.Text) == false)
                //{
                //    lblCheifOfficerInchargeSign.Text = lblChiefSign.Text;
                //}
            }
        }
    }
    protected void gvWeeklyEntryBilge_ItemCommand(object sender, GridCommandEventArgs e)
    {


    }

    #endregion
    protected void gvSludgeTank_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void gvSludgeTank_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvSludgeTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvSludgeTank.DataSource = PhoenixElog.TankTypeWiseList(usercode, vesselid, "SLG", txid, logbookid);
    }

    protected void gvBilgeTank_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
    protected void gvBilgeTank_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void gvBilgeTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        gvBilgeTank.DataSource = PhoenixElog.TankTypeWiseList(usercode, vesselid, "BLG", txid, logbookid);
    }




    protected void txtOperationDate_TextChangedEvent(object sender, EventArgs e)
    {
        gvWeeklyEntryBilge.Rebind();
        gvWeeklyEntrySludge.Rebind();
    }

    protected void txtBilgeRob_TextChangedEvent(object sender, EventArgs e)
    {
        RadNumericTextBox uc = sender as RadNumericTextBox;
        GridDataItem row = (GridDataItem)uc.NamingContainer;
        RadLabel lblLocationId = (RadLabel)row.FindControl("lblLocationId");

        foreach (GridDataItem item in gvWeeklyEntryBilge.Items)
        {
            RadLabel lblWeeklyLocationId = (RadLabel)item.FindControl("lblLocationId");
            if (lblLocationId.Text == lblWeeklyLocationId.Text)
            {
                RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
                lblROB.Text = uc.Text;
            }
        }
    }

    protected void txtSludgeRob_TextChangedEvent(object sender, EventArgs e)
    {
        RadNumericTextBox uc = sender as RadNumericTextBox;
        GridDataItem row = (GridDataItem)uc.NamingContainer;
        RadLabel lblLocationId = (RadLabel)row.FindControl("lblLocationId");

        foreach (GridDataItem item in gvWeeklyEntrySludge.Items)
        {
            RadLabel lblWeeklyLocationId = (RadLabel)item.FindControl("lblLocationId");
            if (lblLocationId.Text == lblWeeklyLocationId.Text)
            {
                RadLabel lblROB = (RadLabel)item.FindControl("lblROB");
                lblROB.Text = uc.Text;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
        SetDefaultData();
        if (isMissedOperation || isMissedOperationEdit)
        {
            saveTxin();
        }
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        string popupName = isMissedOperation || isMissedOperationEdit ? "Log~iframe" : "Log"; ;
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}&isMissedOperation={1}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName, isMissedOperation);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", script, true);
    }
}
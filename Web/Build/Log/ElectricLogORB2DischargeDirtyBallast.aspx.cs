using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class Log_ElectricLogORB2DischargeDirtyBallast : PhoenixBasePage
{
    string ReportCode;
    string ItemNo = "32";
    // string ItemName = "Sludge";
    int usercode = 0;
    int vesselId = 0;
    bool isMissedOperation = false;
    bool isMissedOperationEdit = false;
    int logId = 0;
    DateTime? missedOperationDate = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        logId = PhoenixMarbolLogORB2.GetORB2LogId(usercode, "DischargeDirtyBallast");
        confirm.Attributes.Add("style", "display:none;");

        ViewState["TXID"] = "";

        if (string.IsNullOrWhiteSpace(Request.QueryString["TxnId"]) == false)
        {
            ViewState["TXID"] = Guid.Parse(Request.QueryString["TxnId"]);
            ddlTransferFrom.Enabled = false;
            ddlWaterTransferedFrom.Enabled = false;
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
        if (!IsPostBack)
        {
            ViewState["lastTranscationDate"] = null;
            ddlFromPopulate();
            ddlWaterTransferedFromPoulate();
            ddlwashwater1Populate();
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
    private void SetDefaultData()
    {
        if (ddlTransferFrom.SelectedItem == null) return;

        ReportCode = "H";
        txtDate.Text = txtOperationDate.SelectedDate.HasValue ? PhoenixElog.GetElogDateFormat(txtOperationDate.SelectedDate.Value) : PhoenixElog.GetElogDateFormat(DateTime.Now);
        string StartPosistion = txtStartPosistionLat.Text + " ,  " + txtStartPosistionLog.Text;
        string StopPosistion = txtStopPosistionLat.Text + " ,  " + txtStopPosistionLog.Text;
        txtItemNo.Text = ItemNo;
        txtCode.Text = ReportCode;
        lblRecord.Text = string.Format("<b> {0}</b> ", ddlTransferFrom.SelectedItem.Text);
        txtItemNo1.Text = "33";
        lblrecord1.Text = string.Format("Start:  <b>{0}</b> ", StartPosistion);
        txtItemNo2.Text = "34";
        lblrecord2.Text = string.Format("Stop:  <b>{0}</b>", StopPosistion);
        txtItemNo3.Text = "35";
        lblrecord3.Text = string.Format("<b>{0}</b> m3", txttotquantity.Text);
        txtItemNo4.Text = "36";
        lblrecord4.Text = string.Format("<b>{0}</b> Knots", txtspeed.Text);
        txtItemNo5.Text = "37";
        if (ddldischargemonitoring.SelectedValue != null)
        {
            lblrecord5.Text = string.Format("<b>{0}</b>", ddldischargemonitoring.SelectedValue);
        }
        txtItemNo6.Text = "38";
        if (ddlregularcheck.SelectedValue != null)
        {
            lblrecord6.Text = string.Format("<b>{0}</b>", ddlregularcheck.SelectedValue);
        }
        txtItemNo7.Text = "39";
        //lblrecord7.Text = string.Format("<b>{0}</b> m3 to <b>{1}</b> ", txtqtytransfer.Text, ddlwashwaterto1.Text);
        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "slops")
        {
            lblrecord7.Text = string.Format("<b>{0}</b> m3 to <b>{1}</b> ", txtqtytransfer.Text, ddlwashwaterto1.Text);
            txtreceptionport.Text = "NA";
        }
        else
        {
            lblrecord7.Text = "NA";
        }
        txtItemNo8.Text = "40";
        if (ddlWaterTransferedFrom.SelectedItem.Text.ToLower() == "reception")
        {
            lblrecord8.Text = string.Format("Dischaged to shore reception facility at Port <b>{0}</b> quantity transferred <b>{1}</b> m3", txtreceptionport.Text, txtquantitydischarge.Text);
        }
        else
        {
            lblrecord8.Text = "NA";
        }
    }
    protected void MenuOperationApproval_TabStripCommand(object sender, EventArgs e)
    {

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
    private void ddlwashwater1Populate()
    {
        ddlwashwaterto1.DataSource = PhoenixMarbolLogORB2.ElogLocationDropDown(vesselId, usercode, "CRO");
        ddlwashwaterto1.DataBind();
        if (ddlwashwaterto1.Items.Count > 0)
        {
            ddlwashwaterto1.Items[0].Selected = true;
        }
    }

    public void GetLastTranscationDate()
    {
        ViewState["lastTranscationDate"] = PhoenixMarbolLogORB2.GetLogLastTranscationDate(vesselId, usercode);
    }


    private void BindData()
    {
        if (ViewState["TXID"].ToString() != "")
        {
            DataTable dt = PhoenixMarbolLogORB2.TransactionEdit(usercode, new Guid(ViewState["TXID"].ToString()));

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    if (Convert.ToInt32(row["FLDITEMID"]) == 44)
                    {
                        txtOperationDate.SelectedDate = Convert.ToDateTime(row["FLDVALUE"]);
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 45)
                    {
                        //ddlTransferFrom.SelectedValue = row["FLDVALUE"].ToString();
                        RadComboBoxItem toItem = ddlTransferFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (toItem != null)
                        {
                            toItem.Selected = true;
                        }
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 46)
                    {
                        txtStartTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 47)
                    {
                        txtStartPosistionLat.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 48)
                    {
                        txtStartPosistionLog.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 49)
                    {
                        txtStopTime.SelectedDate = Convert.ToDateTime(row["FLDVALUE"].ToString());
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 50)
                    {
                        txtStopPosistionLat.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 51)
                    {
                        txtStopPosistionLog.Text = row["FLDVALUE"].ToString();
                    }

                    if (Convert.ToInt32(row["FLDITEMID"]) == 52)
                    {
                        txttotquantity.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 53)
                    {
                        txtreceptionport.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 54)
                    {
                        txtquantitydischarge.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 55)
                    {
                        RadComboBoxItem toItem = ddlWaterTransferedFrom.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (toItem != null)
                        {
                            toItem.Selected = true;
                        }
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 56)
                    {
                        //ddlTransferFrom.SelectedValue = row["FLDVALUE"].ToString();
                        RadComboBoxItem toItem = ddlwashwaterto1.Items.FindItem(x => x.Text == row["FLDVALUE"].ToString());
                        if (toItem != null)
                        {
                            toItem.Selected = true;
                        }
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 57)
                    {
                        txtspeed.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 58)
                    {
                        txttotalrob.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 127)
                    {
                        txtqtytransfer.Text = row["FLDVALUE"].ToString();
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 128)
                    {
                        if (row["FLDVALUE"].ToString() != "")
                        {
                            ddldischargemonitoring.SelectedValue = row["FLDVALUE"].ToString();
                        }
                    }
                    if (Convert.ToInt32(row["FLDITEMID"]) == 129)
                    {
                        if (row["FLDVALUE"].ToString() != "")
                        {
                            ddlregularcheck.SelectedValue = row["FLDVALUE"].ToString();
                        }
                    }

                }
                SetDefaultData();
            }
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.Visible = isMissedOperation || isMissedOperationEdit ? false : true;
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
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
                            , new Guid(ddlwashwaterto1.SelectedItem.Value)
                            , null
                            , txttotalrob.Text
                            , vesselId
                            , General.GetNullableInteger(lblinchId.Text)
                            );
    }

    private void SaveTransaction()
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
            MissedOperationalEntryTemplateUpdate(new Guid(ViewState["TXID"].ToString()));
        }
        else if (ViewState["TXID"].ToString() != "" && isMissedOperationEdit == false)
        {
            TransactionUpdate(logId);
            LogBookUpdate(logId);
        }
        else
        {
            Guid TranscationId = Guid.NewGuid();
            TranscationInsert(entrydate, TranscationId, logId);

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , txtCode.Text
                                     , txtItemNo.Text
                                     , lblRecord.Text
                                     , 1
                                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                     , vesselId
                                     , TranscationId
                                     , logId
                                     , entrydate
                                     , null
                                     , txtItemNo1.Text
                                     , lblrecord1.Text
                                     , 2
                                 );


            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                    , vesselId
                                    , TranscationId
                                    , logId
                                    , entrydate
                                    , null
                                    , txtItemNo2.Text
                                    , lblrecord2.Text
                                    , 3
                                );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo3.Text
                     , lblrecord3.Text
                     , 4
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo4.Text
                     , lblrecord4.Text
                     , 5
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo5.Text
                     , lblrecord5.Text
                     , 6
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo6.Text
                     , lblrecord6.Text
                     , 7
                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo7.Text
                     , lblrecord7.Text
                     , 8

                 );

            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                     , vesselId
                     , TranscationId
                     , logId
                     , entrydate
                     , null
                     , txtItemNo8.Text
                     , lblrecord8.Text
                     , 9
                 );
            PhoenixMarbolLogORB2.BookEntryInsert(usercode
                                      , vesselId
                                      , TranscationId
                                      , logId
                                      , entrydate
                                      , null
                                      , null
                                      , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                      , 10
                                      , true
                                );


            PhoenixMarbolLogORB2.LogORB2BookEntryStatusInsert(usercode
                                          , vesselId
                                          , logId
                                          , TranscationId
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
        nvc.Add("Record5", lblrecord4.Text);
        nvc.Add("Record6", lblrecord5.Text);
        nvc.Add("Record7", lblrecord6.Text);
        nvc.Add("Record8", lblrecord7.Text);
        nvc.Add("Record9", lblrecord8.Text);

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

        nvc.Add("Date", txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"));

        nvc.Add("inchId", lblinchId.Text);
        nvc.Add("inchRank", lblincRank.Text);
        nvc.Add("inchName", lblinchName.Text);
        nvc.Add("inchSignDate", lblincSignDate.Text);
        nvc.Add("InchargeSignature", PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text), true));

        // add meta data for the log
        nvc.Add("logBookEntry", "9");
        nvc.Add("logId", logId.ToString());
        nvc.Add("logTxId", logTxId.ToString());
        Filter.MissedOperationalEntryCriteria = nvc;
    }


    private void TransactionUpdate(int logId)
    {

        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStopTime.SelectedTime.Value;

        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 44
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                                    , vesselId
                                    , logId
                                    , 45
                                    , General.GetNullableGuid(ViewState["TXID"].ToString())
                                    , entrydate
                                    , General.GetNullableString(ddlTransferFrom.SelectedItem.Text)
                                );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 46
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(startTime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 47
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 48
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLog.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 49
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 50
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLat.Text)
                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 51
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 52
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txttotquantity.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 53
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtreceptionport.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 54
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtquantitydischarge.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 55
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(ddlWaterTransferedFrom.SelectedValue)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 56
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(ddlwashwaterto1.SelectedItem.Text)
                            );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 57
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtspeed.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 58
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txttotalrob.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 127
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(txtqtytransfer.Text)

                        );

        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 128
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(ddldischargemonitoring.SelectedValue)

                        );
        PhoenixMarbolLogORB2.TransactionUpdate(usercode
                            , vesselId
                            , logId
                            , 129
                            , General.GetNullableGuid(ViewState["TXID"].ToString())
                            , entrydate
                            , General.GetNullableString(ddlregularcheck.SelectedValue)

                        );

    }

    private void LogBookUpdate(int logId)
    {
        DateTime entrydate = PhoenixElog.GetLogBookEntryDate(txtOperationDate.SelectedDate.Value);
        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                  , vesselId
                                  , General.GetNullableGuid(ViewState["TXID"].ToString())
                                  , logId
                                  , entrydate
                                  , lblRecord.Text
                                  , 1
                              );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord1.Text
                                 , 2
                             );


        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord2.Text
                                 , 3
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord3.Text
                                 , 4
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord4.Text
                                 , 5
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord5.Text
                                 , 6
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord6.Text
                                 , 7
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord7.Text
                                 , 8
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , lblrecord8.Text
                                 , 9
                             );

        PhoenixMarbolLogORB2.BookEntryUpdate(usercode
                                 , vesselId
                                 , General.GetNullableGuid(ViewState["TXID"].ToString())
                                 , logId
                                 , entrydate
                                 , PhoenixElog.GetSignatureName(General.GetNullableString(lblinchName.Text), General.GetNullableString(lblincRank.Text), General.GetNullableDateTime(lblincSignDate.Text))
                                 , 10
                             );



        PhoenixMarbolLogORB2.LogORB2BookEntryStatusUpdate(usercode
                                      , vesselId
                                      , logId
                                      , General.GetNullableGuid(ViewState["TXID"].ToString())
                                      , General.GetNullableInteger(lblinchId.Text)
                                      , General.GetNullableString(lblincRank.Text)
                                      , General.GetNullableString(lblincsign.Text)
                                      , General.GetNullableDateTime(lblincSignDate.Text)
                                        );

    }
    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtOperationDate.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date of Operation is required";
        }

        if (string.IsNullOrWhiteSpace(txtStartPosistionLat.Text) || string.IsNullOrWhiteSpace(txtStartPosistionLog.Text))
        {
            ucError.ErrorMessage = "Start Position	is required";
        }

        if (string.IsNullOrWhiteSpace(txtStopPosistionLat.Text) || string.IsNullOrWhiteSpace(txtStopPosistionLog.Text))
        {
            ucError.ErrorMessage = "Stop Position is required";
        }

        if (txtStartTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Start Time is required";
        }
        if (txtStopTime.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Stop Time is required";
        }

        if (string.IsNullOrWhiteSpace(txttotquantity.Text))
        {
            ucError.ErrorMessage = "Total Quantity is required";
        }
        if (string.IsNullOrWhiteSpace(txtreceptionport.Text))
        {
            ucError.ErrorMessage = "Reception port is required";
        }

        if (string.IsNullOrWhiteSpace(txtquantitydischarge.Text))
        {
            ucError.ErrorMessage = "Quantity discharged to shore reception facility is required";
        }

        if (ddlTransferFrom.SelectedItem.Text == ddlwashwaterto1.SelectedItem.Text)
        {
            ucError.ErrorMessage = "Transfer From and To Tank cannot be the same";
        }

        if (string.IsNullOrWhiteSpace(txtspeed.Text))
        {
            ucError.ErrorMessage = "Speed is required";
        }

        if (string.IsNullOrWhiteSpace(txttotalrob.Text))
        {
            ucError.ErrorMessage = "Total ROB in Tank or Shore is required";
        }

        if (string.IsNullOrWhiteSpace(txtqtytransfer.Text))
        {
            ucError.ErrorMessage = "Was the discharge monitoring and control system in operation during discharge?";
        }

        if (ddlregularcheck.SelectedItem  == null)
        {
            ucError.ErrorMessage = "Was a regular check carried out on the effluent and the surface of the water in the locality of the discharge?";
        }

        if (string.IsNullOrWhiteSpace(txtqtytransfer.Text))
        {
            ucError.ErrorMessage = "Quantity of oily-water is required";
        }

        if (PhoenixMarbolLogORB2.IsValidTranscationDate(General.GetNullableDateTime((string)ViewState["lastTranscationDate"]), txtOperationDate.SelectedDate.Value) == false
            && isMissedOperation == false && string.IsNullOrWhiteSpace(ViewState["TXID"].ToString()))
        {
            ucError.ErrorMessage = "Date of Operation should be greater than Last Transaction date and not greater than current date";
        }

        return (!ucError.IsError);
    }

    private void TranscationInsert(DateTime entrydate, Guid TranscationId, int logId)
    {
        TimeSpan startTime = txtStartTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;
        TimeSpan stopTime = txtStopTime.SelectedTime.HasValue == false ? new TimeSpan() : txtStartTime.SelectedTime.Value;

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 44
                                    , TranscationId
                                    , entrydate
                                    , General.GetNullableString(txtOperationDate.SelectedDate.Value.ToString("dd-MM-yyyy"))
                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                                    , vesselId
                                    , logId
                                    , 45
                                    , TranscationId
                                    , entrydate
                                    , ddlTransferFrom.SelectedItem.Text

                                );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 46
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(startTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 47
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 48
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStartPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 49
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(stopTime.ToString())

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 50
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLat.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 51
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtStopPosistionLog.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 52
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txttotquantity.Text)

                        );
        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 53
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtreceptionport.Text)

                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 54
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtquantitydischarge.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 55
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(ddlWaterTransferedFrom.SelectedValue)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 56
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(ddlwashwaterto1.SelectedItem.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 57
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtspeed.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 58
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txttotalrob.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 127
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(txtqtytransfer.Text)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 128
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(ddldischargemonitoring.SelectedValue)
                        );

        PhoenixMarbolLogORB2.TransactionInsert(usercode
                            , vesselId
                            , logId
                            , 129
                            , TranscationId
                            , entrydate
                            , General.GetNullableString(ddlregularcheck.SelectedValue)
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

    protected void txtrefresh_TextChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }

    protected void ddlTransferFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDefaultData();
    }
    protected void ddlWaterTransferedFrom_SelectedIndexChanged(object sender, EventArgs e)
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

}
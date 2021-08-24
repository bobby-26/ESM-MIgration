using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Log_ElectricLogNOX : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    Guid logBookId = Guid.Empty;
    int logId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        ShowToolBar();
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        logId = PhoenixMarpolLogGRB2.GetGRB2LogId(usercode, "NOX");

        if (string.IsNullOrWhiteSpace(Request.QueryString["LogBookId"]) == false)
        {
            logBookId = Guid.Parse(Request.QueryString["LogBookId"]);
            dtPicker.Enabled = false;
        }


        if (IsPostBack == false)
        {
            dtPicker.SelectedDate = DateTime.Now;
            dtPicker.MaxDate = DateTime.Now;
            ShowHideEngineDetails();
            LoadStatusChange();
            LoadAllTiers();
            LoadAllStatus();
            BindData();
        }
    }

    private void ShowHideEngineDetails()
    {
        int mainEngineCount = EngineConfigCount("MainEngine");
        int auxEngineCount = EngineConfigCount("AuxEngine");
        int harbourGenCount = EngineConfigCount("HarbourGenerator");

        ShowHideControls(mainEngineCount, "ddlTier", "ddlEngineStatus", "lblMainEngine");
        ShowHideControls(auxEngineCount, "ddlAuxTier", "ddlAuxEngineStatus", "lblAuxEngine");
        ShowHideControls(harbourGenCount, "ddlHarbourTier", "ddlHarbourStatus", "lblHarbour");


    }

    private void ShowHideControls(int engineCount, string tier, string status, string labelName)
    {
        for (int i = 1; i <= engineCount; i++)
        {
            RadComboBox ddlTier = (RadComboBox)tblEngineDetails.FindControl(tier + i);
            RadComboBox ddlEngineStatus = (RadComboBox)tblEngineDetails.FindControl(status + i);
            RadLabel lblEngineTitle = (RadLabel)tblEngineDetails.FindControl(labelName + i);
            ddlTier.Visible = true;
            ddlEngineStatus.Visible = true;
            lblEngineTitle.Visible = true;
        }
    }

    private int EngineConfigCount(string engineDetail)
    {
        int engineCount = 0;
        DataTable dt = PhoenixMarpolLogNOX.AnnexureEngineCount(usercode, vesselId, engineDetail);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            engineCount = Convert.ToInt32(row["FLDENGINECOUNT"]);
        }
        return engineCount;
    }

    private void LoadStatusChange()
    {
        ddlStatus.DataSource = PhoenixMarpolLogNOX.LogNOXtTierStatus("Status Change");
        ddlStatus.DataTextField = "FLDNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
    }

    private void LoadAllTiers()
    {
        DataSet ds = PhoenixMarpolLogNOX.LogNOXtTierStatus("Tier");
        string nameField = "FLDNAME";
        string valueField = "FLDSTATUSID";
        LoadComboBox(ddlTier2, ds, nameField, valueField, "7");
        LoadComboBox(ddlTier1, ds, nameField, valueField, "7");
        LoadComboBox(ddlAuxTier1, ds, nameField, valueField, "7");
        LoadComboBox(ddlAuxTier2, ds, nameField, valueField, "7");
        LoadComboBox(ddlAuxTier3, ds, nameField, valueField, "7");
        LoadComboBox(ddlAuxTier4, ds, nameField, valueField, "7");
        LoadComboBox(ddlHarbourTier1, ds, nameField, valueField, "7");
    }

    private void LoadAllStatus()
    {
        DataSet ds = PhoenixMarpolLogNOX.LogNOXtTierStatus("Tier Status");
        string nameField = "FLDNAME";
        string valueField = "FLDSTATUSID";
        LoadComboBox(ddlEngineStatus1, ds, nameField, valueField, "10");
        LoadComboBox(ddlEngineStatus2, ds, nameField, valueField, "10");
        LoadComboBox(ddlAuxEngineStatus1, ds, nameField, valueField, "10");
        LoadComboBox(ddlAuxEngineStatus2, ds, nameField, valueField, "10");
        LoadComboBox(ddlAuxEngineStatus3, ds, nameField, valueField, "10");
        LoadComboBox(ddlAuxEngineStatus4, ds, nameField, valueField, "10");
        LoadComboBox(ddlHarbourStatus1, ds, nameField, valueField, "10");
    }

    private void LoadComboBox(RadComboBox combobox, DataSet ds, string nameField, string valueField, string defaultValue = null)
    {
        combobox.DataSource = ds;
        combobox.DataTextField = nameField;
        combobox.DataValueField = valueField;
        combobox.DataBind();
        setComboBoxValue(combobox, defaultValue);
    }    

    private void BindData()
    {
        if (logBookId != Guid.Empty)
        {
            DataTable dt = PhoenixMarpolLogNOX.BookEntryReportSearchById(usercode, vesselId, logBookId);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                dtPicker.SelectedDate = Convert.ToDateTime(row["FLDDATE"]);

                setComboBoxValue(ddlStatus, row["FLDSTATUSCHANGEID"].ToString());
                setComboBoxValue(ddlTier1, row["FLDMAINENGINE1TIERID"].ToString());
                setComboBoxValue(ddlTier2, row["FLDMAINENGINE2TIERID"].ToString());
                setComboBoxValue(ddlAuxTier1, row["FLDAE1TIERID"].ToString());
                setComboBoxValue(ddlAuxTier2, row["FLDAE1TIERID"].ToString());
                setComboBoxValue(ddlAuxTier3, row["FLDAE1TIERID"].ToString());
                setComboBoxValue(ddlAuxTier4, row["FLDAE1TIERID"].ToString());
                setComboBoxValue(ddlHarbourTier1, row["FLDHARBOURGENTIERID"].ToString());

                setComboBoxValue(ddlEngineStatus1, row["FLDMAINENGINE1STATUSID"].ToString());
                setComboBoxValue(ddlEngineStatus2, row["FLDMAINENGINE2STATUSID"].ToString());
                setComboBoxValue(ddlAuxEngineStatus1, row["FLDAE1STATUSID"].ToString());
                setComboBoxValue(ddlAuxEngineStatus2, row["FLDAE2STATUSID"].ToString());
                setComboBoxValue(ddlAuxEngineStatus3, row["FLDAE3STATUSID"].ToString());
                setComboBoxValue(ddlAuxEngineStatus4, row["FLDAE4STATUSID"].ToString());
                setComboBoxValue(ddlHarbourStatus1, row["FLDHARBOURGENSTATUSID"].ToString());

                txtLatitude.Text = (string)row["FLDLATITUDE"];
                txtLongitude.Text = (string)row["FLDLONGITUDE"];

            }

        }
    }

    private void setComboBoxValue(RadComboBox ddlComboBox, string data)
    {
        RadComboBoxItem item = ddlComboBox.Items.FindItem(x => x.Value == data);
        if (item != null)
        {
            item.Selected = true;
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
                if (isValidInput() == false || ValidateSignature() == false)
                {
                    ucError.Visible = true;
                    return;
                }

                if (logBookId == Guid.Empty)
                {
                    LogbookInsert();
                } else
                {
                    LogbookUpdate();
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

    private void LogbookInsert()
    {
        int statusId = Convert.ToInt32(ddlStatus.SelectedItem.Value);
        Guid logBookId = Guid.Empty;

        int engineTierId1 = Convert.ToInt32(ddlTier1.SelectedItem.Value);
        int engineStatusId1 = Convert.ToInt32(ddlEngineStatus1.SelectedItem.Value);

        int engineTierId2 = Convert.ToInt32(ddlTier2.SelectedItem.Value);
        int engineStatusId2 = Convert.ToInt32(ddlEngineStatus2.SelectedItem.Value);

        int auxEngineTierId1 = Convert.ToInt32(ddlAuxTier1.SelectedItem.Value);
        int auxEngineStatusId1 = Convert.ToInt32(ddlAuxEngineStatus1.SelectedItem.Value);

        int auxEngineTierId2 = Convert.ToInt32(ddlAuxTier2.SelectedItem.Value);
        int auxEngineStatusId2 = Convert.ToInt32(ddlAuxEngineStatus2.SelectedItem.Value);

        int auxEngineTierId3 = Convert.ToInt32(ddlAuxTier3.SelectedItem.Value);
        int auxEngineStatusId3 = Convert.ToInt32(ddlAuxEngineStatus3.SelectedItem.Value);

        int auxEngineTierId4 = Convert.ToInt32(ddlAuxTier4.SelectedItem.Value);
        int auxEngineStatusId4 = Convert.ToInt32(ddlAuxEngineStatus4.SelectedItem.Value);

        int harbourTierId = Convert.ToInt32(ddlHarbourTier1.SelectedItem.Value);
        int harbourStatusId = Convert.ToInt32(ddlHarbourStatus1.SelectedItem.Value);

        PhoenixMarpolLogNOX.BookEntryInsert(usercode, vesselId, statusId, ddlStatus.SelectedItem.Text, dtPicker.SelectedDate.Value, txtLatitude.Text, txtLongitude.Text,
            engineTierId1, ddlTier1.SelectedItem.Text, engineStatusId1, ddlEngineStatus1.SelectedItem.Text,
            engineTierId2, ddlTier2.SelectedItem.Text, engineStatusId2, ddlEngineStatus2.SelectedItem.Text,
            auxEngineTierId1, ddlAuxTier1.SelectedItem.Text, auxEngineStatusId1, ddlAuxEngineStatus1.SelectedItem.Text,
            auxEngineTierId2, ddlAuxTier2.SelectedItem.Text, auxEngineStatusId2, ddlAuxEngineStatus2.SelectedItem.Text,
            auxEngineTierId3, ddlAuxTier3.SelectedItem.Text, auxEngineStatusId3, ddlAuxEngineStatus3.SelectedItem.Text,
            auxEngineTierId4, ddlAuxTier4.SelectedItem.Text, auxEngineStatusId4, ddlAuxEngineStatus4.SelectedItem.Text,
            harbourTierId, ddlHarbourTier1.SelectedItem.Text, harbourStatusId, ddlHarbourStatus1.SelectedItem.Text,
            true, null, ref logBookId
            );

        PhoenixMarpolLogNOX.LogNOXBookEntryStatusInsert(usercode, logBookId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), false);
    }

    private void LogbookUpdate()
    {
        int statusId = Convert.ToInt32(ddlStatus.SelectedItem.Value);

        int engineTierId1 = Convert.ToInt32(ddlTier1.SelectedItem.Value);
        int engineStatusId1 = Convert.ToInt32(ddlEngineStatus1.SelectedItem.Value);

        int engineTierId2 = Convert.ToInt32(ddlTier2.SelectedItem.Value);
        int engineStatusId2 = Convert.ToInt32(ddlEngineStatus2.SelectedItem.Value);

        int auxEngineTierId1 = Convert.ToInt32(ddlAuxTier1.SelectedItem.Value);
        int auxEngineStatusId1 = Convert.ToInt32(ddlAuxEngineStatus1.SelectedItem.Value);

        int auxEngineTierId2 = Convert.ToInt32(ddlAuxTier2.SelectedItem.Value);
        int auxEngineStatusId2 = Convert.ToInt32(ddlAuxEngineStatus2.SelectedItem.Value);

        int auxEngineTierId3 = Convert.ToInt32(ddlAuxTier3.SelectedItem.Value);
        int auxEngineStatusId3 = Convert.ToInt32(ddlAuxEngineStatus3.SelectedItem.Value);

        int auxEngineTierId4 = Convert.ToInt32(ddlAuxTier4.SelectedItem.Value);
        int auxEngineStatusId4 = Convert.ToInt32(ddlAuxEngineStatus4.SelectedItem.Value);

        int harbourTierId = Convert.ToInt32(ddlHarbourTier1.SelectedItem.Value);
        int harbourStatusId = Convert.ToInt32(ddlHarbourStatus1.SelectedItem.Value);

        PhoenixMarpolLogNOX.BookEntryUpdate(usercode, vesselId, logBookId, statusId,  dtPicker.SelectedDate.Value, ddlStatus.SelectedItem.Text 
        ,txtLatitude.Text, txtLongitude.Text,
        engineTierId1, ddlTier1.SelectedItem.Text, engineStatusId1, ddlEngineStatus1.SelectedItem.Text,
        engineTierId2, ddlTier2.SelectedItem.Text, engineStatusId2, ddlEngineStatus2.SelectedItem.Text,
        auxEngineTierId1, ddlAuxTier1.SelectedItem.Text, auxEngineStatusId1, ddlAuxEngineStatus1.SelectedItem.Text,
        auxEngineTierId2, ddlAuxTier2.SelectedItem.Text, auxEngineStatusId2, ddlAuxEngineStatus2.SelectedItem.Text,
        auxEngineTierId3, ddlAuxTier3.SelectedItem.Text, auxEngineStatusId3, ddlAuxEngineStatus3.SelectedItem.Text,
        auxEngineTierId4, ddlAuxTier4.SelectedItem.Text, auxEngineStatusId4, ddlAuxEngineStatus4.SelectedItem.Text,
        harbourTierId, ddlHarbourTier1.SelectedItem.Text, harbourStatusId, ddlHarbourStatus1.SelectedItem.Text);

        PhoenixMarpolLogNOX.LogNOXBookEntryStatusUpdate(usercode, logBookId, Convert.ToInt32(lblinchId.Text), lblinchName.Text, lblincRank.Text, Convert.ToDateTime(lblincSignDate.Text), false);
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (dtPicker.SelectedDate.HasValue == false)
        {
            ucError.ErrorMessage = "Date is required";
        }

        if (string.IsNullOrWhiteSpace(txtLatitude.Text))
        {
            ucError.ErrorMessage = "Latitude is required";
        }

        if (string.IsNullOrWhiteSpace(txtLongitude.Text))
        {
            ucError.ErrorMessage = "Longitude is required";
        }

        return (!ucError.IsError);
    }

    private bool ValidateSignature()
    {
        if (string.IsNullOrWhiteSpace(lblinchName.Text))
        {
            ucError.ErrorMessage = "Incharge Sign is Required ";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        OnDutyEngineerSign();
    }

    protected void btnInchargeSign_Click(object sender, EventArgs e)
    {
        if (isValidInput() == false)
        {
            ucError.Visible = true;
            return;
        }

        string popupName = "Log";
        string script = string.Format("javascript: parent.openNewWindow('engineersign', '', `Log/ElectricLogDutyEngineerSignature.aspx?popupname={0}`, 'false', '370', '170', null, null, {{ 'disableMinMax': true }})", popupName);
        btnInchargeSign.Attributes.Add("onclick", script);

    }
}
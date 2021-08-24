using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.Web.UI;

public partial class Log_ElectricLogAnnexureConfig : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        ShowToolBar();
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        if (IsPostBack == false)
        {
            PopulateEngineStatus();
            LoadVesselParticulars();
            LoadTankList();
            LoadEngineDetail();
            LoadAuxillaryEngineDetail();
            LoadHarbourGenDetail();
        }
    }

    private void PopulateEngineStatus()
    {
        Dictionary<string, bool> engineStatus = new Dictionary<string, bool>();
        engineStatus.Add("Yes", true);
        engineStatus.Add("No", false);

        AssignStatus(ddlMainEngineStatus1, engineStatus);
        AssignStatus(ddlMainEngineStatus2, engineStatus);
        AssignStatus(ddlAuxEngineStatus1, engineStatus);
        AssignStatus(ddlAuxEngineStatus2, engineStatus);
        AssignStatus(ddlAuxEngineStatus3, engineStatus);
        AssignStatus(ddlAuxEngineStatus4, engineStatus);
        AssignStatus(ddlHarbourGenerator, engineStatus);
    }
    private void AssignStatus(RadComboBox ddlStatus, Dictionary<string, bool> data)
    {
        ddlStatus.DataSource = data;
        ddlStatus.DataTextField = "Key";
        ddlStatus.DataValueField = "Value";
        ddlStatus.DataBind();
    }
    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
    }
    private void LoadVesselParticulars()
    {
        DataTable dt= PhoenixMarpolLogNOX.AnnexureVesselDetails(usercode, vesselId);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            lblVesselName.Text = (string)row["FLDVESSELNAME"];
            lblShipType.Text = (string)row["FLDTYPEDESCRIPTION"];
            lblIMO.Text = (string)row["FLDIMONUMBER"];
            lblFlag.Text = (string)row["FLDCOUNTRYNAME"];
            lblCompany.Text = (string)row["FLDPRIMARYMANAGERNAME"];
            lblClassfication.Text = (string)row["FLDCLASSNAMEVALUE"];

        }
    }

    private void LoadTankList()
    {
        DataTable dt = PhoenixMarpolLogNOX.AnnexureTankDetails(usercode, vesselId);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                RadTextBox txtTankId              = (RadTextBox)tblTankList.FindControl("txtTankId" + row["FLDORDERNO"]);
                RadTextBox txtTankName            = (RadTextBox)tblTankList.FindControl("txtTankName" + row["FLDORDERNO"]);
                RadNumericTextBox txtTankCapacity = (RadNumericTextBox)tblTankList.FindControl("txtTankCapacity" + row["FLDORDERNO"]);

                txtTankName.Text     = (string)row["FLDTANKNAME"];
                txtTankCapacity.Text = row["FLDCAPACITY"].ToString();
                txtTankId.Text      = row["FLDID"].ToString();
            }
        }
    }

    private void LoadEngineDetail()
    {
        DataTable dt = PhoenixMarpolLogNOX.AnnexureEngineDetails(usercode, vesselId, "MainEngine");
        if (dt.Rows.Count > 0)
        {

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                int controlId = i + 1;
                RadComboBox  ddlMainEngineStatus = (RadComboBox)tblEngine.FindControl("ddlMainEngineStatus" + controlId);
                RadLabel   lblEngineId = (RadLabel)tblEngine.FindControl("lblMainEngineId" + controlId);

                lblEngineId.Text = row["FLDID"].ToString();
                bool status = row["FLDISONBOARD"].ToString() == "1" ? true : false;

                RadComboBoxItem mainengineStatus = ddlMainEngineStatus.Items.FindItem(x => x.Value == status.ToString());
                if (mainengineStatus != null)
                {
                    mainengineStatus.Selected = true;
                }
            }
        }
    }

    private void LoadAuxillaryEngineDetail()
    {
        DataTable dt = PhoenixMarpolLogNOX.AnnexureEngineDetails(usercode, vesselId, "AuxEngine");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                int controlId = i + 1;
                RadComboBox ddlAuxEngineStatus = (RadComboBox)tblEngine.FindControl("ddlAuxEngineStatus" + controlId);
                RadLabel lblEngineId = (RadLabel)tblEngine.FindControl("lblAuxEngineId" + controlId);
                bool status = row["FLDISONBOARD"].ToString() == "1" ? true : false;

                lblEngineId.Text = row["FLDID"].ToString();

                RadComboBoxItem auxengineStatus = ddlAuxEngineStatus.Items.FindItem(x => x.Value == status.ToString());
                if (auxengineStatus != null)
                {
                    auxengineStatus.Selected = true;
                }
            }
        }
    }

    private void LoadHarbourGenDetail()
    {
        DataTable dt = PhoenixMarpolLogNOX.AnnexureEngineDetails(usercode, vesselId, "HarbourGenerator");
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                RadComboBox ddlEngineStatus = (RadComboBox)tblEngine.FindControl("ddlHarbourGenerator");
                RadLabel lblHarbourId = (RadLabel)tblEngine.FindControl("lblHarbourId");
                bool status = row["FLDISONBOARD"].ToString() == "1" ? true : false;

                lblHarbourId.Text = row["FLDID"].ToString();

                RadComboBoxItem engineStatus = ddlEngineStatus.Items.FindItem(x => x.Value == status.ToString());
                if (engineStatus != null)
                {
                    engineStatus.Selected = true;
                }
            }
        }
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if (isValidInput() == false)
                //{
                //    ucError.Visible = true;
                //    return;
                //}

                TankListInsertUpdate();
                EngineInsertUpdate();

                ucStatus.Text = "Saved Successfully";
                ucStatus.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtTankName1.Text) || string.IsNullOrWhiteSpace(txtTankCapacity1.Text))
        {
            ucError.ErrorMessage = "Tank 1 details is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankName2.Text) || string.IsNullOrWhiteSpace(txtTankCapacity2.Text))
        {
            ucError.ErrorMessage = "Tank 2 details is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankName3.Text) || string.IsNullOrWhiteSpace(txtTankCapacity3.Text))
        {
            ucError.ErrorMessage = "Tank 3 details is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankName4.Text) || string.IsNullOrWhiteSpace(txtTankCapacity4.Text))
        {
            ucError.ErrorMessage = "Tank 4 details is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankName5.Text) || string.IsNullOrWhiteSpace(txtTankCapacity5.Text))
        {
            ucError.ErrorMessage = "Tank 5 details is required";
        }

        if (string.IsNullOrWhiteSpace(txtTankName6.Text) || string.IsNullOrWhiteSpace(txtTankCapacity6.Text))
        {
            ucError.ErrorMessage = "Tank 6 details is required";
        }


        return (!ucError.IsError);
    }

    private void EngineInsertUpdate()
    {
        string engineShortCode = string.Empty;


        for (int i = 1; i <= 2; i++)
        {
            RadComboBox ddlEngineStatus = (RadComboBox)tblEngine.FindControl("ddlMainEngineStatus" + i);
            RadLabel lblEngineId = (RadLabel)tblEngine.FindControl("lblMainEngineId" + i);
            RadLabel lblEngineName = (RadLabel)tblEngine.FindControl("lblEngineName" + i);

            Guid? id = General.GetNullableGuid(lblEngineId.Text);
            engineShortCode = "MainEngine";
           
            PhoenixMarpolLogNOX.AnnexureEngineInsertupdate(usercode, vesselId, lblEngineName.Text, Convert.ToBoolean(ddlEngineStatus.SelectedItem.Value), engineShortCode, id);
        }

        for (int i = 1; i <= 4; i++)
        {
            RadComboBox ddlEngineStatus = (RadComboBox)tblEngine.FindControl("ddlAuxEngineStatus" + i);
            RadLabel lblEngineId = (RadLabel)tblEngine.FindControl("lblAuxEngineId" + i);
            RadLabel lblEngineName = (RadLabel)tblEngine.FindControl("lblAuxEngineName" + i);

            Guid? id = General.GetNullableGuid(lblEngineId.Text);
            engineShortCode = "AuxEngine";

            PhoenixMarpolLogNOX.AnnexureEngineInsertupdate(usercode, vesselId, lblEngineName.Text, Convert.ToBoolean(ddlEngineStatus.SelectedItem.Value), engineShortCode, id);
        }


        Guid? harbourGenId = General.GetNullableGuid(lblHarbourId.Text);
        engineShortCode = "HarbourGenerator";
        PhoenixMarpolLogNOX.AnnexureEngineInsertupdate(usercode, vesselId, lblHarbourGen.Text, Convert.ToBoolean(ddlHarbourGenerator.SelectedItem.Value), engineShortCode, harbourGenId);


    }

    private void TankListInsertUpdate()
    {
        for (int i = 1; i <= 6; i++)
        {
            RadTextBox txtTankName = (RadTextBox)tblTankList.FindControl("txtTankName" + i);
            RadTextBox txtTankId = (RadTextBox)tblTankList.FindControl("txtTankId" + i);
            RadNumericTextBox txtTankCapacity = (RadNumericTextBox)tblTankList.FindControl("txtTankCapacity" + i);

            if (string.IsNullOrWhiteSpace(txtTankName.Text) == false && string.IsNullOrWhiteSpace(txtTankCapacity.Text) == false)
            {
                decimal capacity = Convert.ToDecimal(txtTankCapacity.Text);
                Guid? id = General.GetNullableGuid(txtTankId.Text);

                PhoenixMarpolLogNOX.AnnexureTankInsert(usercode, vesselId, txtTankName.Text, capacity, id);
            }
        }
    }
}
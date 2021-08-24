using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersNOX : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuNOx.AccessRights = this.ViewState;
            MenuNOx.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
                {
                    ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVesselName.Enabled = false;
                }
                else
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    {
                        ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        ucVesselName.Enabled = false;
                    }
                    else
                    {
                        if (Request.QueryString["VesselId"] != null)
                        {
                            ucVesselName.SelectedVessel = Request.QueryString["VesselId"].ToString();
                            ucVesselName.Enabled = false;
                        }
                    }
                }

                if (Request.QueryString["NOxId"] != null)
                {
                    ViewState["NOxId"] = Request.QueryString["NOxId"].ToString();

                    NOxEdit(new Guid(Request.QueryString["NOxId"].ToString()));
                }
                else
                {
                    NOxEdit(null);
                }
            }
        
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidNOx()
    {
        if (General.GetNullableInteger(ucVesselName.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel name is required.";

        if (General.GetNullableDateTime(txtBuiltDate.Text) == null)
            ucError.ErrorMessage = "Built Date is required.";

        if(General.GetNullableDecimal(txtRPM.Text) == null)
            ucError.ErrorMessage = "Rated Speed(rpm) (ME) is required.";

        if (General.GetNullableDecimal(txtAERPM.Text) == null)
            ucError.ErrorMessage = "Rated Speed(rpm) (AE) is required.";

        if (General.GetNullableDecimal(txtMEPowerOutput.Text) == null)
            ucError.ErrorMessage = "Rated Power(kW) (ME) is required.";

        if (General.GetNullableDecimal(txtAEPowerOutput.Text) == null)
            ucError.ErrorMessage = "Rated Power(kW) (AE) is required.";

        if (General.GetNullableDecimal(txtMENoOfUnits.Text) == null)
            ucError.ErrorMessage = "No. of Units (ME) is required.";

        if (General.GetNullableDecimal(txtAENoOfUnits.Text) == null)
            ucError.ErrorMessage = "No. of Units (AE) is required.";

        return (!ucError.IsError);
    }

    protected void NOx_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if ((CommandName.ToUpper().Equals("SAVE")))
        {
            try
            {
                if (!IsValidNOx())
                {
                    ucError.Visible = true;
                    return;
                }
                 
                if (ViewState["NOxId"] != null)
                {
                    PhoenixRegistersNOX.UpdateNOX(
                        new Guid((ViewState["NOxId"].ToString())),
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ucVesselName.SelectedVessel),
                        txtMEMaker.Text, txtMEModel.Text, ddlMEUse.SelectedValue,
                        General.GetNullableDecimal(txtMEPowerOutput.Text),
                        General.GetNullableInteger(txtMENoOfUnits.Text),
                        General.GetNullableDecimal(txtMEAllowedEmission.Text),
                        General.GetNullableDecimal(txtMEActualEmission.Text),
                        txtAEMaker.Text, txtAEModel.Text, ddlAEUse.SelectedValue,
                        General.GetNullableDecimal(txtAEPowerOutput.Text),
                        General.GetNullableInteger(txtAENoOfUnits.Text),
                        General.GetNullableDecimal(txtAEAllowedEmission.Text),
                        General.GetNullableDecimal(txtAEActualEmission.Text),
                        chkEEOI.Checked==true ? 1 : 0,
                        chkOPS.Checked == true ? 1 : 0,
                        General.GetNullableDecimal(txtRPM.Text),
                        General.GetNullableDateTime(txtBuiltDate.Text),
                        null,
                        General.GetNullableDecimal(txtAERPM.Text),
                        General.GetNullableInteger(chkTier1Yn.Checked == true ? "1" : "")
                    );
                }
                else
                {
                    //NOX
                    PhoenixRegistersNOX.InsertNOX(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ucVesselName.SelectedVessel),
                        txtMEMaker.Text, txtMEModel.Text, ddlMEUse.SelectedValue,
                        General.GetNullableDecimal(txtMEPowerOutput.Text),
                        General.GetNullableInteger(txtMENoOfUnits.Text),
                        General.GetNullableDecimal(txtMEAllowedEmission.Text),
                        General.GetNullableDecimal(txtMEActualEmission.Text),
                        txtAEMaker.Text, txtAEModel.Text, ddlAEUse.SelectedValue,
                        General.GetNullableDecimal(txtAEPowerOutput.Text),
                        General.GetNullableInteger(txtAENoOfUnits.Text),
                        General.GetNullableDecimal(txtAEAllowedEmission.Text),
                        General.GetNullableDecimal(txtAEActualEmission.Text),
                        chkEEOI.Checked == true ? 1 : 0,
                        chkOPS.Checked == true ? 1 : 0,
                        General.GetNullableDecimal(txtRPM.Text),
                        General.GetNullableDateTime(txtBuiltDate.Text),
                        null,
                        General.GetNullableDecimal(txtAERPM.Text),
                        General.GetNullableInteger(chkTier1Yn.Checked == true ? "1" : "")
                        );
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
           // Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

            String script = "javascript:fnReloadList('codehelp1',null);";
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }
    
    private void NOxEdit(Guid? noxid)
    {
        DataSet ds;
        if(Request.QueryString["VesselId"] != null)
            ds = PhoenixRegistersNOX.EditNOX(noxid, General.GetNullableInteger(Request.QueryString["VesselId"].ToString()));
        else
            ds = PhoenixRegistersNOX.EditNOX(noxid, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtMEActualEmission.Text = dr["FLDMEACTUALEMISSION"].ToString();
            txtMEAllowedEmission.Text = dr["FLDMEALLOWEDEMISSION"].ToString();
            txtMEMaker.Text = dr["FLDMEMAKER"].ToString();
            txtMEModel.Text = dr["FLDMEMODEL"].ToString();
            txtMENoOfUnits.Text = dr["FLDMENOOFUNIT"].ToString();
            txtMEPowerOutput.Text = dr["FLDMEPOWEROUTPUT"].ToString();
            ddlMEUse.SelectedValue = dr["FLDMEUSE"].ToString();
            lblMEUse.Text = ddlMEUse.Text; 

            txtAEActualEmission.Text = dr["FLDAEACTUALEMISSION"].ToString();
            txtAEAllowedEmission.Text = dr["FLDAEALLOWEDEMISSION"].ToString();
            txtAEMaker.Text = dr["FLDAEMAKER"].ToString();
            txtAEModel.Text = dr["FLDAEMODEL"].ToString();
            txtAENoOfUnits.Text = dr["FLDAENOOFUNIT"].ToString();
            txtAEPowerOutput.Text = dr["FLDAEPOWEROUTPUT"].ToString();
            ddlAEUse.SelectedValue = dr["FLDAEUSE"].ToString();
            lblAEUse.Text = ddlAEUse.Text; 
            txtESINox.Text = dr["FLDNOX"].ToString();

            txtRPM.Text = dr["FLDRPM"].ToString();
            txtAERPM.Text = dr["FLDAERPM"].ToString();
            txtBuiltDate.Text = dr["FLDDATEOFBUILT"].ToString();
            txtTierName.Text = dr["FLDTIERNAME"].ToString();
            lblTierId.Text = dr["FLDTIER"].ToString();

            if (dr["FLDUSENOXTIER1YN"].ToString() == "1")
                chkTier1Yn.Checked = true;

            ViewState["Tier1SOxId"] = dr["FLDTIER1SOXID"].ToString();
            ViewState["Tier2SOxId"] = dr["FLDTIER2SOXID"].ToString();
            ViewState["Tier3SOxId"] = dr["FLDTIER3SOXID"].ToString();

            if (dr["FLDEEOIREPORTED"].ToString() == "1")
                chkEEOI.Checked = true;
            else
                chkEEOI.Checked = false;
            if (dr["FLDOPSONBOARD"].ToString() == "1")
                chkOPS.Checked = true;
            else
                chkOPS.Checked = false;

            ucVesselName.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucVesselName.Enabled = false;
        }
    }
    //decimal bsout = 0, bsin = 0, bsberth = 0;
    protected void gvSOX_RowDataBound(object sender, GridItemEventArgs e)
    {
        //DataRowView drv = (DataRowView)e.Item.DataItem;

        //if (e.Item is GridEditableItem)
        //{
        //    decimal.TryParse(drv["FLDOUTWEIGHTAGE"].ToString(), out bsout);
        //    decimal.TryParse(drv["FLDINWEIGHTAGE"].ToString(), out bsin);
        //    decimal.TryParse(drv["FLDBERTHWEIGHTAGE"].ToString(), out bsberth);
        //}
        //else if (e.Item is GridFooterItem)
        //{
        //    e.Item.Cells[0].Text = "Weightage %";
        //    e.Item.Cells[1].Text = Convert.ToInt32(bsout).ToString();
        //    e.Item.Cells[2].Text = Convert.ToInt32(bsin).ToString();
        //    e.Item.Cells[3].Text = Convert.ToInt32(bsberth).ToString();
        //    PivotGridRowField footer = e.Item;
        //    int cellcnt = GridFooterItem.Cells.Count;

        //    GridViewRow newrow = new GridViewRow(footer.RowIndex + 1, -1, footer.RowType, footer.RowState);
        //    TableCell cell = null;
        //    newrow.HorizontalAlign = HorizontalAlign.Center;
        //    for (int i = 0; i <= cellcnt - 1; i++)
        //    {
        //        cell = new TableCell();
        //        cell.ApplyStyle(((GridView)sender).Columns[i].FooterStyle);
        //        newrow.Cells.Add(cell);
        //    }
        //}
    }

    protected void ddlMEUse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMEUse.SelectedValue == "")
            lblMEUse.Text = "";
        else
            lblMEUse.Text = ddlMEUse.SelectedItem.Text + ":" + ddlMEUse.SelectedValue;
    }

    protected void ddlAEUse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAEUse.SelectedValue == "")
            lblAEUse.Text = "";
        else
            lblAEUse.Text = ddlAEUse.SelectedItem.Text + ":" + ddlAEUse.SelectedValue;  
    }

    decimal bsout = 0, bsin = 0, bsberth = 0;
    protected void gvSOX_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds;
        ds = PhoenixRegistersSOX.ListSOX();
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count>0)
        {
            decimal.TryParse(dt.Rows[0]["FLDOUTWEIGHTAGE"].ToString(), out bsout);
            decimal.TryParse(dt.Rows[0]["FLDINWEIGHTAGE"].ToString(), out bsin);
            decimal.TryParse(dt.Rows[0]["FLDBERTHWEIGHTAGE"].ToString(), out bsberth);

            dt.Rows.Add(null, bsout, bsin, bsberth,null,null,null,null,null,null, "Weightage %");
        }
        gvSOX.DataSource = dt;
    }
}

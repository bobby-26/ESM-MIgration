using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseFalLevelMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuFalLevelMappingEdit.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            vessellist();
          
            Guid? Id = General.GetNullableGuid(Request.QueryString["LEVELMAPPINGID"].ToString());
            DataTable dT;
            dT = PhoenixPurchaseFalLevel.FalLevelMappingEdit(Id);
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];
                txtLevelName.Text = dr["FLDLEVELNAME"].ToString();
                ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
                lblInMinimum.Text = dr["FLDINBUDGETMINIMUM"].ToString();
                lblInMaximum.Text = dr["FLDINBUDGETMAXIMUM"].ToString();
                lblExcMinimum.Text = dr["FLDEXCBUDGETMINIMUM"].ToString();
                lblExcMaximum.Text = dr["FLDEXCBUDGETMAXIMUM"].ToString();                           
                SetCsvValue(ddlVessel, dr["FLDVESSELLIST"].ToString());
            

               
            }

        }

    }

  


    private void vessellist()
    {
        ddlVessel.DataSource = PhoenixPurchaseRules.PurchaseConfigVesselList();
        ddlVessel.DataBind();
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }

    protected void MenuFalLevelMappingEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String StockType = General.GetNullableString(ddlStockType.SelectedValue);
            decimal? InMinimum = General.GetNullableDecimal(lblInMinimum.Text);
            decimal? InMaximum = General.GetNullableDecimal(lblInMaximum.Text);
            decimal? ExcMinimum = General.GetNullableDecimal(lblExcMinimum.Text);
            decimal? ExcMaximum = General.GetNullableDecimal(lblExcMaximum.Text);
            string Vessel = General.GetNullableString(GetCsvValue(ddlVessel));
         

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidLevel(StockType, InMinimum, InMaximum, ExcMinimum, ExcMaximum))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPurchaseFalLevel.FalLevelMappingUpdate(
                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , StockType
                  , InMinimum
                  , InMaximum
                  , ExcMinimum
                  , ExcMaximum
                  , Vessel              
                  , General.GetNullableGuid(Request.QueryString["LEVELMAPPINGID"].ToString())
                  );


            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                          "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLevel(string StockType, decimal? InMinimum, decimal? InMaximum, decimal? ExcMinimum, decimal? ExcMaximum)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (StockType == null)
            ucError.ErrorMessage = "StockType is required.";

        if (InMinimum == null)
            ucError.ErrorMessage = "In Minimum is required.";

        if (InMaximum == null)
            ucError.ErrorMessage = "In Maximum is required.";

        if (ExcMinimum == null)
            ucError.ErrorMessage = "Exc Minimum is required.";

        if (ExcMaximum == null)
            ucError.ErrorMessage = "Exc Maximum is required.";

        return (!ucError.IsError);
    }

}
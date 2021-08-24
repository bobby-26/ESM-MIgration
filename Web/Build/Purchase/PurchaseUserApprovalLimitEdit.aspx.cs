using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseUserApprovalLimitEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuUserApprovalEdit.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            vessellist();
            BindUser();

            Guid? Id = General.GetNullableGuid(Request.QueryString["USERAPPROVALLIMITID"].ToString());
            DataTable dT;

            dT = PhoenixPurchaseUserApprovalLimit.PurchaseUserApproveEdit(Id);         
            if (dT.Rows.Count > 0)
            {
                DataRow dr = dT.Rows[0];

                ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
                lblInMinimum.Text = dr["FLDINBUDGETMINIMUM"].ToString();
                lblInMaximum.Text = dr["FLDINBUDGETMAXIMUM"].ToString();
                lblExcMinimum.Text = dr["FLDEXCBUDGETMINIMUM"].ToString();
                lblExcMaximum.Text = dr["FLDEXCBUDGETMAXIMUM"].ToString();
                SetCsvValue(ddlVessel, dr["VESSELID"].ToString());            
                SetCsvValue(ddluser, dr["USERCODE"].ToString());

               // ddlVessel.SelectedValue = dr["FLDVESSELID"].ToString();
               // ddluser.SelectedValue = dr["FLDUSERCODE"].ToString().Trim(',');
               
            }
        }

    }

    private void vessellist()
    {
        ddlVessel.DataSource = PhoenixPurchaseUserApprovalLimit.PurchaseConfigVesselList();
        ddlVessel.DataBind();
    }

    private void BindUser()
    {
        ddluser.DataSource = PhoenixPurchaseUserApprovalLimit.PurchaseUserList();
        ddluser.DataBind();
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
    protected void MenuUserApprovalEdit_TabStripCommand(object sender, EventArgs e)
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
            string UserCode = General.GetNullableString(GetCsvValue(ddluser));

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidApproval(StockType, InMinimum, InMaximum, ExcMinimum, ExcMaximum))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPurchaseUserApprovalLimit.PurchaseUserApprovalUpdate
                    (
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , StockType
                       , InMinimum
                       , InMaximum
                       , ExcMinimum
                       , ExcMaximum
                       , Vessel
                       , UserCode
                       , General.GetNullableGuid(Request.QueryString["USERAPPROVALLIMITID"].ToString())
                    );

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                             "BookMarkScript", "fnReloadList('Add', 'ifMoreInfo', null);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidApproval(string StockType, decimal? InMinimum, decimal? InMaximum, decimal? ExcMinimum, decimal? ExcMaximum)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (StockType == null)
            ucError.ErrorMessage = "StockType is required.";

        if (InMinimum == null)
            ucError.ErrorMessage = "InMinimum is required.";

        if (InMaximum == null)
            ucError.ErrorMessage = "InMaximum is required.";

        if (ExcMinimum == null)
            ucError.ErrorMessage = "ExcMinimum is required.";

        if (ExcMaximum == null)
            ucError.ErrorMessage = "ExcMaximum is required.";

        if (General.GetNullableString(ddluser.Text) == null)
            ucError.ErrorMessage = "User is required.";

        if (General.GetNullableString(ddlVessel.Text) == null)
            ucError.ErrorMessage = "Vessel is required.";


        return (!ucError.IsError);
    }


}


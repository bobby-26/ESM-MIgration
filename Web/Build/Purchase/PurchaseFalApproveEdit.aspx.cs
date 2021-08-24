using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseFalApproveEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            ViewState["APPROVALID"] = General.GetNullableGuid(Request.QueryString["APPROVALID"].ToString());

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);           
            MenuPurchaseFalApproveEdit.MenuList = toolbarmain.Show();
       
            if (!IsPostBack)
            {

                vessellist();
                Guid? Id = General.GetNullableGuid(Request.QueryString["APPROVALID"].ToString());
                DataTable dT;
                dT = PhoenixPurchaseFalApprove.PurchaseFalApproveEdit(Id);
                if (dT.Rows.Count > 0)
                {
                    DataRow dr = dT.Rows[0];

                    lbllevel.Text = dr["FLDLEVEL"].ToString();
                    lblname.Text = dr["FLDLEVELNAME"].ToString();

                    ddlStockType.SelectedValue = dr["FLDSTOCKTYPE"].ToString();
                    if (dr["FLDREQUIRED"].ToString() == "1")
                        chkRequiredYN.Checked = true;
               
                    lblMaximum.Text = dr["FLDMAXIMUM"].ToString();
                    UcGroup.SelectedGroup = dr["FLDGROUP"].ToString();
                    UcTarget.SelectedTarget = dr["FLDTARGET"].ToString();
                    SetCsvValue(ddlVessel, dr["FLDAPPLIESTO"].ToString());

                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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


    protected void MenuPurchaseFalApproveEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String StockType = General.GetNullableString(ddlStockType.SelectedValue);
            String LevelName = General.GetNullableString(lblname.Text);
            decimal? Maximum = General.GetNullableDecimal(lblMaximum.Text);
            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Tatrget = General.GetNullableInteger(UcTarget.SelectedTarget);
            string Vessel = General.GetNullableString(GetCsvValue(ddlVessel));


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidApprove(StockType, LevelName, Maximum, Group, Tatrget, Vessel))
                {
                    ucError.Visible = true;
                    return;
                }


                PhoenixPurchaseFalApprove.PurchaseFalApproveUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , StockType
                    , LevelName
                    , chkRequiredYN.Checked.Equals(true) ? 1 : 0               
                    , Maximum
                    , Group
                    , Tatrget
                    , Vessel
                    , "IN-BGT"
                    , General.GetNullableGuid(Request.QueryString["APPROVALID"].ToString())

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



    private bool IsValidApprove(string StockType,string Name, decimal? Maximum, Guid? Group, int? Tatrget, string Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((StockType) == null)
            ucError.ErrorMessage = "StockType is required.";

        if ((Name) == null)
            ucError.ErrorMessage = "LevelName is required.";

        if (Maximum == null)
            ucError.ErrorMessage = "Maximum is required.";

        if (Group == null)
            ucError.ErrorMessage = "Group is required.";

        if (Tatrget == null)
            ucError.ErrorMessage = "Tatrget is required.";

        if (Vessel == null)
            ucError.ErrorMessage = "Vessel is required.";
        return (!ucError.IsError);
    }



}
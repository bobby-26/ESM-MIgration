using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseFalLevelEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuFalLevelEdit.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                Guid? Id = General.GetNullableGuid(Request.QueryString["LEVELID"].ToString());
                DataTable dT;

                dT = PhoenixPurchaseFalLevel.PurchaseFalLevelEdit(Id);
                if (dT.Rows.Count > 0)
                {
                    DataRow dr = dT.Rows[0];

                    txtLevel.Text = dr["FLDLEVEL"].ToString();
                    txtLevelName.Text = dr["FLDLEVELNAME"].ToString();
                    if (dr["FLDREQUIRED"].ToString() == "1")
                        chkRequiredYN.Checked = true;

                    lblInMinimum.Text = dr["FLDINBUDGETMINIMUM"].ToString();
                    lblInMaximum.Text = dr["FLDINBUDGETMAXIMUM"].ToString();
                    lblExcMinimum.Text = dr["FLDEXCBUDGETMINIMUM"].ToString();
                    lblExcMaximum.Text = dr["FLDEXCBUDGETMAXIMUM"].ToString();
                    UcGroup.SelectedGroup = dr["FLDGROUPID"].ToString();
                    UcTarget.SelectedTarget = dr["FLDTARGETID"].ToString();
                    UcFormType.SelectedQuick = dr["FLDFORMTYPE"].ToString();

                }


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuFalLevelEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            int? Level = General.GetNullableInteger(txtLevel.Text);
            string Name = General.GetNullableString(txtLevelName.Text);
            decimal? InMinimum = General.GetNullableDecimal(lblInMinimum.Text);
            decimal? InMaximum = General.GetNullableDecimal(lblInMaximum.Text);
            decimal? ExcMinimum = General.GetNullableDecimal(lblExcMinimum.Text);
            decimal? ExcMaximum = General.GetNullableDecimal(lblExcMaximum.Text);
            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Tatrget = General.GetNullableInteger(UcTarget.SelectedTarget);
            int? FormType = General.GetNullableInteger(UcFormType.SelectedQuick);
            Guid? LevelId = General.GetNullableGuid(Request.QueryString["LEVELID"].ToString());
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidLevel(Level, Name, InMinimum, InMaximum, ExcMinimum, ExcMaximum, Group, Tatrget, FormType))
                {
                    ucError.Visible = true;
                    return;
                }

          
                PhoenixPurchaseFalLevel.PurchaseFalLevelUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , Level
                , Name
                , chkRequiredYN.Checked.Equals(true) ? 1 : 0
                , InMinimum
                , InMaximum
                , ExcMinimum
                , ExcMaximum
                , Group
                , Tatrget
                , FormType
                , LevelId

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

    private bool IsValidLevel(int? Level,string Name, decimal? InMinimum, decimal? InMaximum, decimal? ExcMinimum, decimal? ExcMaximum, Guid? Group, int? Tatrget, int? FormType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Level == null)
            ucError.ErrorMessage = "Level is required.";

        if (Name == null)
            ucError.ErrorMessage = "Name is required.";

        if (InMinimum == null)
            ucError.ErrorMessage = "InMinimum is required.";

        if (InMaximum == null)
            ucError.ErrorMessage = "InMaximum is required.";

        if (ExcMinimum == null)
            ucError.ErrorMessage = "ExcMinimum is required.";

        if (ExcMaximum == null)
            ucError.ErrorMessage = "ExcMaximum is required.";

        if (Group == null)
            ucError.ErrorMessage = "Group is required.";

        if (Tatrget == null)
            ucError.ErrorMessage = "Tatrget is required.";

        if (FormType == null)
            ucError.ErrorMessage = "FormType is required.";
        return (!ucError.IsError);
    }

}
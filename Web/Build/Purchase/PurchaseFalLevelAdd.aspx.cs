using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseFalLevelAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuFalLevelAdd.MenuList = toolbarmain.Show();
      
    }

    protected void MenuFalLevelAdd_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            
                       
            int? Level = General.GetNullableInteger(lblLevel.Text);
            string Name = General.GetNullableString(txtLevelName.Text);
            decimal? InMinimum = General.GetNullableDecimal(lblInMinimum.Text);
            decimal? InMaximum = General.GetNullableDecimal(lblInMaximum.Text);
            decimal? ExcMinimum = General.GetNullableDecimal(lblExcMinimum.Text);
            decimal? ExcMaximum = General.GetNullableDecimal(lblExcMaximum.Text);
            Guid? Group = General.GetNullableGuid(UcGroup.SelectedGroup);
            int? Tatrget = General.GetNullableInteger(UcTarget.SelectedTarget);
            int? FormType = General.GetNullableInteger(UcFormType.SelectedQuick);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidLevel(Level,Name, InMinimum, InMaximum, ExcMinimum, ExcMaximum, Group,Tatrget,FormType))
                {
                    ucError.Visible = true;
                    return;
                }
          
                    PhoenixPurchaseFalLevel.PurchaseFalLevelInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
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

    private bool IsValidLevel(int? Level,string Name,decimal? InMinimum,decimal? InMaximum,decimal? ExcMinimum,decimal? ExcMaximum, Guid? Group, int? Tatrget,int? FormType)
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
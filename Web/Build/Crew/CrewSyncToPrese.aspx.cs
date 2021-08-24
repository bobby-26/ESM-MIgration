using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewSyncToPrese : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Sync", "SYNC",ToolBarDirection.Right); 
            SyncToPresea.MenuList = toolbar.Show();
        }
    }

    protected void SyncToPresea_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SYNC"))
        {
            if (General.GetNullableInteger(ucBatch.SelectedBatch.ToString())==null)
            {
                ucError.Text = "Batch is required";
                ucError.Visible = true;
                return;
            }

            PhoenixNewApplicantManagement.NewApplicantSyncDataToPresea(null, ucBatch.SelectedValue);
            ucStatus.Text = "Data Sync Successfully";
        }
       
    }
}

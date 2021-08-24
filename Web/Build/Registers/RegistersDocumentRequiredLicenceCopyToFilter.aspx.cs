using System;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersDocumentRequiredLicenceCopyToFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddButton("Copy", "COPY",ToolBarDirection.Right);
        MenuCopy.AccessRights = this.ViewState;
        MenuCopy.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();
            ucVesselType.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
            ucVesselType.DataBind();
            BindVessel(null, null);
        }
    }

    protected void BindVessel(object sender, EventArgs e)
    {
        cblVessel.DataSource = PhoenixRegistersVessel.ListVessel(
            General.GetNullableInteger(Request.QueryString["flag"].ToString()), ucPrincipal.SelectedAddress == "Dummy" ? "" : ucPrincipal.SelectedAddress, 1,
            ucVesselType.SelectedVesseltype == "Dummy"?"0":ucVesselType.SelectedVesseltype);
        cblVessel.DataTextField = "FLDVESSELNAME";
        cblVessel.DataValueField = "FLDVESSELID";
        cblVessel.DataBind();
    }

    protected void Copy_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("COPY"))
        {
            StringBuilder strVessel = new StringBuilder();

            foreach (ListItem item in cblVessel.Items)
            {
                if (item.Selected == true)
                {
                    strVessel.Append(item.Value.ToString());
                    strVessel.Append(",");
                }
            }
            if (strVessel.Length > 1)
            {
                strVessel.Remove(strVessel.Length - 1, 1);
            }

            if (IsValidCopy(strVessel.ToString()))
            {
                PhoenixRegistersVesselDocumentsRequired.CopyDocumentsRequired(
                      strVessel.ToString()
                      , Convert.ToInt32(Request.QueryString["flag"].ToString())
                      , Convert.ToInt32(Request.QueryString["vesseltype"].ToString())
                      , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                ucStatus.Text = "Licence Details successfully copied";
                ucStatus.Visible = true;
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
    }

    private bool IsValidCopy(string strVessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (strVessel.Trim().Equals(""))
            ucError.ErrorMessage = "Select atleast one vessel to copy.";

        if(General.GetNullableInteger(Request.QueryString["vesseltype"].ToString())==null)
            ucError.ErrorMessage="Copying from Vessel type is not specified.";
        
        return (!ucError.IsError);
    }

    protected void SelectAllVessel(object sender, EventArgs e)
    {
        if (chkChkAllVessel.Checked == true)
        {
            foreach (ListItem item in cblVessel.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            foreach (ListItem item in cblVessel.Items)
            {
                item.Selected = false;
            }
        }
    }
}

using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Inspection_InspectionSPIEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
       
        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {

            ViewState["SHIPPINGSPIID"] = General.GetNullableGuid(Request.QueryString["shippingspiid"]);
            Guid? shippingspiid = General.GetNullableGuid(ViewState["SHIPPINGSPIID"].ToString());

            DataTable dt = PheonixDashboardSKSPI.SPIEditlist(shippingspiid);
            if (dt.Rows.Count > 0)
            {
                Radspiidentry.Text = dt.Rows[0]["FLDSPIID"].ToString();
                Radspishortcodeentry.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
                Radspinameentry.Text = dt.Rows[0]["FLDSPITITLE"].ToString();
                if (dt.Rows[0]["FLDDESCRIPTION"] != null)
                {
                    Radspidescriptionentry.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
                }

            }
        }



    }
    protected void spiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string id = General.GetNullableString(Radspiidentry.Text);
                string shortcode = General.GetNullableString(Radspishortcodeentry.Text);
                string title = General.GetNullableString(Radspinameentry.Text);
                string description = General.GetNullableString(Radspidescriptionentry.Text);
                Guid? shippingspiid = General.GetNullableGuid(ViewState["SHIPPINGSPIID"].ToString());
                if (!IsValidShippingSPIDetails(id, shortcode, title))
                {
                    ucError.Visible = true;
                    return;
                }


                PheonixDashboardSKSPI.SPIUpdate(rowusercode, id, shortcode, title, description, shippingspiid);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }

        }



        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    private bool IsValidShippingSPIDetails(string id, string shortcode, string title)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (id == null)
        {
            ucError.ErrorMessage = "SPI ID.";
        }
        if (shortcode == null)
        {
            ucError.ErrorMessage = "SPI Short Code.";
        }
        if (title == null)
        {
            ucError.ErrorMessage = "SPI Title.";
        }
        return (!ucError.IsError);
    }



}
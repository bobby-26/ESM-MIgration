using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;

public partial class Registers_RegistersCBAVesselMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            //chkIsactive.Checked = true;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCBAVesselMapping.AccessRights = this.ViewState;
            MenuCBAVesselMapping.MenuList = toolbar.Show();
        //    ucVessel.SelectedVessel= PhoenixRegistersVessel.VesselList();

        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Request.QueryString["MappingID"] != null)
        {
          
            ViewState["MappingID"] = Request.QueryString["MappingID"].ToString();
            EditVesselMapping(new Guid(Request.QueryString["MappingID"].ToString()));
        }
    }


    private void EditVesselMapping(Guid Mappingid)
    {
        try
        {
            DataSet ds = PhoenixRegistersContractCBARevisionMapping.EditVesselMapping(Mappingid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlUnion.SelectedAddress = dr["FLDCBAID"].ToString();
                //chkIsactive.Checked = dr["FLDISACTIVE"].ToString() == "1" ? true : false;
                ucVessel.SelectedVessel = dr["FLDVESSELLIST"].ToString();
 
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCBAVesselMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='Vessel Mapping'>" + "\n";
            scriptClosePopup += "fnReloadList('Vessel Mapping');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='VesselMappingNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('OfficeStaff', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["MappingID"] != null)
                {
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersContractCBARevisionMapping.UpdateCBAVesselMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["MappingID"].ToString())
                                                    , ucVessel.SelectedVessel
                                                    //, chkIsactive.Checked.Equals(true) ? 1 : 0
                                                    );

                    ucStatus.Text = "Information Updated";
               
                }
                else
                {
                    if (!IsValidMapping())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersContractCBARevisionMapping.InsertCBAVesselMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(ddlUnion.SelectedAddress)
                                                    , ddlUnion.SelectedText
                                                    , ucVessel.SelectedVessel
                                                    //, chkIsactive.Checked.Equals(true) ? 1 : 0
                                                    );

                    ucStatus.Text = "Information Added";
                   
                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "VesselMappingNew", scriptClosePopup);
            }
            if (CommandName.ToUpper().Equals("NEW"))
                Reset();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidMapping()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlUnion.SelectedAddress) == null)
            ucError.ErrorMessage = "Union is required";

        if (General.GetNullableString(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required";


        return (!ucError.IsError);

    }

    private void Reset()
    {
        ViewState["MappingID"] = null;
        ddlUnion.SelectedAddress = "";
        ucVessel.SelectedVessel = "";
        //chkIsactive.Checked = true;
    }

}
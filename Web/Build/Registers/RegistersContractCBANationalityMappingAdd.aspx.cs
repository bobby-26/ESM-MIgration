using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Data;
using Telerik.Web.UI;

public partial class Registers_RegistersContractCBANationalityMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

        //    chkIsactive.Checked = true;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCBANationalityMapping.AccessRights = this.ViewState;
            MenuCBANationalityMapping.MenuList = toolbar.Show();
            lstNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
        }     

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Request.QueryString["MappingID"] != null)
        {
            ViewState["MappingID"] = Request.QueryString["MappingID"].ToString();
            NationalityMappingEdit(new Guid(Request.QueryString["MappingID"].ToString()));
        }
    }

    private void NationalityMappingEdit(Guid Mappingid)
    {
        try
        {
            DataSet ds = PhoenixRegistersContractCBARevisionMapping.EditNaionalityMapping(Mappingid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ddlUnion.SelectedAddress = dr["FLDCBAID"].ToString();
            //    chkIsactive.Checked = dr["FLDISACTIVE"].ToString() == "1" ? true : false;
                lstNationality.SelectedList = dr["FLDNATIONALITY"].ToString();

                
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCBANationalityMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='Nationality Mapping'>" + "\n";
            scriptClosePopup += "fnReloadList('City');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='NationalityMappingNew'>" + "\n";
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

                    PhoenixRegistersContractCBARevisionMapping.UpdateCBANationalityMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(ViewState["MappingID"].ToString())
                                                    , lstNationality.SelectedList
                                                  //  , chkIsactive.Checked.Equals(true) ? 1 : 0
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

                    PhoenixRegistersContractCBARevisionMapping.InsertCBANationalityMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , int.Parse(ddlUnion.SelectedAddress)
                                                    , ddlUnion.SelectedText
                                                    , lstNationality.SelectedList
                                                   // , chkIsactive.Checked.Equals(true) ? 1 : 0
                                                   );

                    ucStatus.Text = "Information Added";
                   
                }
                Page.ClientScript.RegisterStartupScript(typeof(Page), "NationalityMapping", scriptClosePopup);

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

        if (General.GetNullableString(lstNationality.SelectedList) == null)
            ucError.ErrorMessage = "Nationality is required";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["MappingID"] = null;
        ddlUnion.SelectedAddress = "";
        lstNationality.SelectedList = "";
      //  chkIsactive.Checked = true;
    }

}
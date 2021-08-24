using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class VesselOtherCompanyList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["COMPANYID"] != null)
            {

                toolbar.AddButton("Save", "SAVE");
                ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();
                VesselOtherCompanyEdit(Int32.Parse(Request.QueryString["COMPANYID"].ToString()));
            }
            else
            {
                toolbar.AddButton("New", "NEW");
                toolbar.AddButton("Save", "SAVE");

            }
            MenuSecurityVesselOtherCompanyList.AccessRights = this.ViewState;
            MenuSecurityVesselOtherCompanyList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

            }
        }

        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }

    }

    protected void SecurityVesselOtherCompanyList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='VesselcompanyEdit'>" + "\n";
            scriptClosePopup += "fnReloadList('VesselOtherCompany');";
            scriptClosePopup += "</script>" + "\n";

            string scriptRefreshDontClose = "";
            scriptRefreshDontClose += "<script language='javaScript' id='VesselcompanyAddNew'>" + "\n";
            scriptRefreshDontClose += "fnReloadList('VesselOtherCompany', null, 'yes');";
            scriptRefreshDontClose += "</script>" + "\n";
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                if (ViewState["COMPANYID"] != null)
                {
                    if (!IsValidVesselOtherCompany())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersVesselOtherCompany.UpdateVesselOtherCompany(1,
                        Int32.Parse(ViewState["COMPANYID"].ToString()),
                        txtCompanyName.Text,
                        txtVesselName.Text,
                        Convert.ToInt32(ucVesselType.SelectedVesseltype),
                        txtDWT.Text,
                        txtGRT.Text,
                        General.GetNullableInteger(ucEngineType.SelectedEngineName),
                        txtModel.Text,
                        txtKW.Text,
                        txtBHP.Text,
                        General.GetNullableInteger(txtNoOfUnits.Text),
                        txtFlag.Text,
                        General.GetNullableInteger(ucNationality.SelectedNationality),
                        txtUMS.Text,
                        txtIMONumber.Text);
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "VesselcompanyEdit", scriptClosePopup);
                }
                else
                {
                    if (!IsValidVesselOtherCompany())
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixRegistersVesselOtherCompany.InsertVesselOtherCompany(1, txtCompanyName.Text,
                        txtVesselName.Text,
                        Convert.ToInt32(ucVesselType.SelectedVesseltype),
                        txtDWT.Text,
                        txtGRT.Text,
                        General.GetNullableInteger(ucEngineType.SelectedEngineName),
                        txtModel.Text,
                        txtKW.Text,
                        txtBHP.Text,
                        General.GetNullableInteger(txtNoOfUnits.Text),
                        txtFlag.Text,
                        General.GetNullableInteger(ucNationality.SelectedNationality),
                        txtUMS.Text,
                        txtIMONumber.Text);
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "VesselcompanyAddNew", scriptRefreshDontClose);
                    Reset();

                }
            }
            if (dce.CommandName.ToUpper().Equals("NEW"))
                Reset();
        }

        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }

    }
    private bool IsValidVesselOtherCompany()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        Int16 result;
        
        if (txtCompanyName.Text.Equals(""))
            ucError.ErrorMessage = "Company name is required.";

        if (txtVesselName.Text.Equals(""))
            ucError.ErrorMessage = "Vessel Name is required.";

        if (ucVesselType.SelectedVesseltype == "" || !Int16.TryParse(ucVesselType.SelectedVesseltype, out result))
            ucError.ErrorMessage = "Vessel Type is required.";

        if (txtDWT.Text.Equals(""))
            ucError.ErrorMessage = "DWT is required.";

        if (txtIMONumber.Text.Equals(""))
            ucError.ErrorMessage = "IMO Number is required.";

        return (!ucError.IsError);
             
        
    }
    private void Reset()
    {
            ViewState["COMPANYID"] = null;
            txtCompanyName.Text="";
            txtVesselName.Text="";
            ucVesselType.SelectedVesseltype=null;
            txtDWT.Text="";
            txtGRT.Text="";
            ucEngineType.SelectedEngineName=null;
            ucNationality.SelectedNationality = null;    
            txtModel.Text="";
            txtKW.Text="";
            txtBHP.Text="";
            txtNoOfUnits.Text="";
            txtFlag.Text="";
            txtUMS.Text="";
            txtIMONumber.Text = "";
    }

    private void VesselOtherCompanyEdit(int companyid)
    {
        try
        {
            DataSet ds = PhoenixRegistersVesselOtherCompany.EditVesselOtherCompany(companyid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtCompanyName.Text = dr["FLDCOMPANYNAME"].ToString();
                txtVesselName.Text = dr["FLDVESSELNAME"].ToString();
                ucVesselType.SelectedVesseltype = dr["FLDVESSELTYPEID"].ToString();
                txtDWT.Text = dr["FLDDWT"].ToString();
                txtGRT.Text = dr["FLDGRT"].ToString();
                ucEngineType.SelectedEngineName = dr["FLDENGINETYPEID"].ToString();
                txtModel.Text = dr["FLDMODEL"].ToString();
                txtKW.Text = dr["FLDKW"].ToString();
                txtBHP.Text = dr["FLDBHP"].ToString();
                txtNoOfUnits.Text = dr["FLDNOOFUNITS"].ToString();
                txtFlag.Text = dr["FLDFLAG"].ToString();
                ucNationality.SelectedNationality = dr["FLDNATIONALITYOFCREW"].ToString();
                txtUMS.Text = dr["FLDUMS"].ToString();
                txtIMONumber.Text = dr["FLDIMONUMBER"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }
    }
}

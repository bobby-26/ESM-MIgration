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
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionVesselInspectionMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Map", "MAP",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["inspectionid"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();                    
                }
                BindInspection();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuInspectionCompanyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("../Inspection/InspectionCompanyVessel.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindInspection()
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        DataSet ds = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , null
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        cblInspection.DataSource = ds;
        cblInspection.DataBindings.DataTextField = "FLDSHORTCODE";
        cblInspection.DataBindings.DataValueField = "FLDINSPECTIONID";
        cblInspection.DataBind();
    }

    protected void BindMapping()
    {
        string inspectionlist = null;
        BindInspection();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            PhoenixInspectionInspectingCompany.GetInspectionVesselMappingList(int.Parse(ucVessel.SelectedVessel), ref inspectionlist);

            General.RadBindCheckBoxList(cblInspection, inspectionlist);
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MAP"))
            {
                StringBuilder strinspectionid = new StringBuilder();
                foreach (ButtonListItem item in cblInspection.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strinspectionid.Append(item.Value.ToString());
                        strinspectionid.Append(",");
                    }
                }

                if (strinspectionid.Length > 1)
                {
                    strinspectionid.Remove(strinspectionid.Length - 1, 1);
                }
                string inspectionid = strinspectionid.ToString();

                if (!IsValidMapping(ucVessel.SelectedVessel, inspectionid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionInspectingCompany.MapInspectionVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucVessel.SelectedVessel), inspectionid);

                ucStatus.Text = "Inspection mapped successfully.";
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMapping(string vesselid, string inspectionid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(inspectionid) == null)
            ucError.ErrorMessage = "Inspection is required.";

        return (!ucError.IsError);
    }

    protected void SelectAll(object sender, EventArgs e)
    {

    }

    protected void ClearAll(object sender, EventArgs e)
    {
        ucVessel.SelectedVessel = "";
        BindInspection();
    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        BindMapping();
    }
}

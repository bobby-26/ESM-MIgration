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

public partial class InspectionCompanyVesselMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Map", "MAP");
                MenuTemplateMapping.AccessRights = this.ViewState;
                MenuTemplateMapping.MenuList = toolbar.Show();

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                BindCompanies();    
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("VESSEL"))
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

    protected void BindCompanies()
    {
        DataSet ds = PhoenixInspectionInspectingCompany.ListInspectionCompany();
        cblCompany.DataSource = ds;
        cblCompany.DataTextField = "FLDCOMPANYINSPECTION";
        cblCompany.DataValueField = "FLDCOMPANYID";
        cblCompany.DataBind();
    }

    protected void BindMapping()
    {
        string companylist = null;
        BindCompanies();
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            PhoenixInspectionInspectingCompany.GetMappingList(int.Parse(ucVessel.SelectedVessel), ref companylist);

            foreach (string item in companylist.Split(','))
            {
                if (item.Trim() != "")
                {
                    if (cblCompany.Items.FindByValue(item) != null)
                        cblCompany.Items.FindByValue(item).Selected = true;
                }
            }
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("MAP"))
            {
                StringBuilder strcompanyid = new StringBuilder();
                foreach (ListItem item in cblCompany.Items)
                {
                    if (item.Selected == true && item.Enabled == true)
                    {
                        strcompanyid.Append(item.Value.ToString());
                        strcompanyid.Append(",");
                    }
                }

                if (strcompanyid.Length > 1)
                {
                    strcompanyid.Remove(strcompanyid.Length - 1, 1);
                }
                string companyid = strcompanyid.ToString();

                if (!IsValidMapping(ucVessel.SelectedVessel, companyid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionInspectingCompany.MapInspectionCompanyForVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucVessel.SelectedVessel), companyid);

                ucStatus.Text = "Companies mapped successfully.";
                //BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    } 

    private bool IsValidMapping(string vesselid, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";        

        if (General.GetNullableInteger(vesselid)==null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }

    protected void SelectAll(object sender, EventArgs e)
    {
        //if (chkCheckAll.Checked == true)
        //{
        //    foreach (ListItem item in cblCompany.Items)
        //    {
        //        item.Selected = true;
        //    }
        //}
        //else
        //{
        //    foreach (ListItem item in cblCompany.Items)
        //    {
        //        item.Selected = false;
        //    }
        //    //BindData();
        //}
    }

    protected void ClearAll(object sender, EventArgs e)
    {
        ucVessel.SelectedVessel = "";
        BindCompanies();
    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        BindMapping();
    }
}

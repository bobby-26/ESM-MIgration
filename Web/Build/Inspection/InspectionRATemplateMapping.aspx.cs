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
public partial class InspectionRATemplateMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["RISKASSESSMENTID"] = "";
                ViewState["TYPE"] = "";
				PhoenixToolbar toolbarsearcg = new PhoenixToolbar();
				toolbarsearcg.AddImageButton("../Inspection/InspectionRATemplateMapping.aspx", "Find", "search.png", "FIND");
                MenuTemplateMapping.AccessRights = this.ViewState;
				MenuRATemplate.MenuList = toolbarsearcg.Show();
				ucAddrOwner.AddressType = ((int)PhoenixAddressType.OWNER).ToString();
				ucPrincipal.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

				PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Distribute", "PUBLISH");
                MenuTemplateMapping.AccessRights = this.ViewState;
				MenuTemplateMapping.MenuList = toolbar.Show();

                if (Request.QueryString["RISKASSESSMENTID"] != null)
                {
                    ViewState["RISKASSESSMENTID"] = Request.QueryString["RISKASSESSMENTID"].ToString();
                    ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
                    string raid = ViewState["RISKASSESSMENTID"].ToString();
                    string type = ViewState["TYPE"].ToString();
                }
				BindData();
                chkVeselTypeList.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
                chkVeselTypeList.DataTextField = "FLDHARDNAME";
                chkVeselTypeList.DataValueField = "FLDHARDCODE";
                chkVeselTypeList.DataBind();
            }

			
        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRATemplate_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            BindData();
            
        }
        
    }
	protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("PUBLISH"))
			{
				StringBuilder strvesselid = new StringBuilder();
				foreach (ListItem item in cblVesselName.Items)
				{
					if (item.Selected == true && item.Enabled == true)
					{
						strvesselid.Append(item.Value.ToString());
						strvesselid.Append(",");
					}
				}

				if (strvesselid.Length > 1)
				{
					strvesselid.Remove(strvesselid.Length - 1, 1);
				}
				string vesselid = strvesselid.ToString();

				if (!IsValidTemplateMapping(vesselid))
				{
					ucError.Visible = true;
					return;
				}

                if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() == "1") //Generic
                {
                    PhoenixInspectionRATemplateMapping.InsertTemplateMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        vesselid, new Guid(ViewState["RISKASSESSMENTID"].ToString()));
                }
                else if (ViewState["TYPE"] != null && ViewState["TYPE"].ToString() == "4") //Process
                {
                    PhoenixInspectionRATemplateMapping.InsertProcessTemplateMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        vesselid, new Guid(ViewState["RISKASSESSMENTID"].ToString()));
                }
                ucStatus.Text = "Template has been distributed successfully.";
				BindData();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
    private void BindData()
    {
        string strVesselType = GetSelectedVesselType();
        DataSet ds = PhoenixInspectionRATemplateMapping.RATemplateMappingVesselSearch(
                                                           txtName .Text,
                                                           General.GetNullableString(strVesselType),
                                                           General .GetNullableInteger (ucPrincipal .SelectedAddress),
                                                           General .GetNullableInteger (ucAddrOwner .SelectedAddress),
														   General.GetNullableGuid(ViewState["RISKASSESSMENTID"].ToString()),
														   General.GetNullableInteger(ViewState["TYPE"].ToString())
														   );

			cblVesselName.DataSource = ds.Tables[0];
			cblVesselName.DataTextField = "FLDVESSELNAME";
			cblVesselName.DataValueField = "FLDVESSELID";
			cblVesselName.DataBind();

			if (ds.Tables[1].Rows.Count > 0)
			{
				string[] vesselid = ds.Tables[1].Rows[0]["FLDVESSELID"].ToString().Split(',');
				foreach (string item in vesselid)
				{
					if (item.Trim() != "")
					{
						if (cblVesselName.Items.FindByValue(item) != null)
						{
							cblVesselName.Items.FindByValue(item).Selected = true;
							cblVesselName.Items.FindByValue(item).Enabled = false;
						}
					}
				}
			}
    }
	protected void BindCheckBox(object sender, EventArgs e)
	{
		BindData();
	}

	private bool IsValidTemplateMapping(string vesselid)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (vesselid.Equals(""))
            ucError.ErrorMessage = "Atleast select 1 vessel to distribute.";

		return (!ucError.IsError);
	}
	
	protected void SelectAll(object sender, EventArgs e)
	{
		if (chkCheckAll.Checked == true)
		{
			foreach (ListItem item in cblVesselName.Items)
			{
				item.Selected = true;
			}
		}
		else
		{
			foreach (ListItem item in cblVesselName.Items)
			{
				item.Selected = false;
			}
			BindData();
		}
		
	}

    protected string GetSelectedVesselType()
    {
        StringBuilder strVesselType = new StringBuilder();
        foreach (ListItem item in chkVeselTypeList.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strVesselType.Append(item.Value.ToString());
                strVesselType.Append(",");
            }
        }

        if (strVesselType.Length > 1)
        {
            strVesselType.Remove(strVesselType.Length - 1, 1);
        }

        return strVesselType.ToString();
    }
}

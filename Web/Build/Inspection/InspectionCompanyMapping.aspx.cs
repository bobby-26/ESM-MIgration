using System;
using System.Data;
using System.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;
using Telerik.Web.UI;

public partial class InspectionCompanyMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuTemplateMapping.AccessRights = this.ViewState;
            MenuTemplateMapping.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["INSPECTIONID"] = "";
                if (Request.QueryString["INSPECTIONID"] != null && Request.QueryString["INSPECTIONID"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["INSPECTIONID"].ToString();
              
                EditInspection();
                BindCompanies();
                BindMapping();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void EditInspection()
    {
        DataSet ds = PhoenixInspection.EditInspection(new Guid(ViewState["INSPECTIONID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
            txtAuditInspection.Text = ds.Tables[0].Rows[0]["FLDINSPECTIONNAME"].ToString();
    }

    protected void BindCompanies()
    {
        DataSet ds = PhoenixRegistersCompany.ListCompany();
        cblCompany.DataSource = ds;
        cblCompany.DataBindings.DataTextField = "FLDSHORTCODE";
        cblCompany.DataBindings.DataValueField = "FLDCOMPANYID";
        cblCompany.DataBind();
    }

    protected void BindMapping()
    {
        string companylist = null;
        BindCompanies();
        if (General.GetNullableGuid(ViewState["INSPECTIONID"].ToString()) != null)
        {
            PhoenixInspection.GetMappingCompanyList(new Guid(ViewState["INSPECTIONID"].ToString()), ref companylist);
            string[] list = companylist.Split(',');

            for (int i = 0; i <= list.Count()-1; i++)
            {
                if (list[i] != "")
                {
                    cblCompany.SelectedValue = list[i];
                }
                //if (cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()) != null)
                //    cblvesseltype.Items.FindByValue(ds.Tables[0].Rows[i]["FLDVESSELTYPE"].ToString()).Selected = true;
            }
            
        }
    }

    protected void MenuTemplateMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                StringBuilder strcompanyid = new StringBuilder();
                foreach (ButtonListItem item in cblCompany.Items)
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

                if (!IsValidMapping(companyid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspection.InsertInspectionCompanyMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["INSPECTIONID"].ToString()), companyid);

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

    private bool IsValidMapping(string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }
}

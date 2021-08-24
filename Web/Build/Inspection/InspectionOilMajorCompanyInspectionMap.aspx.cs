using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionOilMajorCompanyInspectionMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuMapping.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }                

                ViewState["OILMAJORCOMPANYID"] = "";                

                if (Request.QueryString["oilmajorcompanyid"] != null)
                    ViewState["OILMAJORCOMPANYID"] = Request.QueryString["oilmajorcompanyid"].ToString();

                if (Request.QueryString["oilmajorcompany"] != null)
                    txtOilMajorCompany.Text = Request.QueryString["oilmajorcompany"].ToString();

                BindCheckbox();
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string source = ReadCheckBoxList(cblInspection);

                PhoenixInspectionOilMajorComany.UpdateOilMajorCompanyInspections(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString())
                    , source);
                ucStatus.Text = "Inspections are mapped successfully.";

                BindData();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindCheckbox()
    {
        cblInspection.Items.Clear();
        cblInspection.DataSource = PhoenixInspection.ListInspectionByCompany(null, null, null, 1, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        cblInspection.DataBindings.DataTextField = "FLDINSPECTIONNAMEWITHCODE";
        cblInspection.DataBindings.DataValueField = "FLDINSPECTIONID";
        cblInspection.DataBind();
    }
    private void BindData()
    {
        BindCheckbox();
        DataTable dt = PhoenixInspectionOilMajorComany.ListOilMajorCompanyInspections(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString()), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = (DataRow)dt.Rows[i];
            string s = dr["FLDINSPECTIONID"].ToString();
            if (cblInspection.Items[i].Value == s)
                cblInspection.Items[i].Selected = true;
            //if (cblInspection.Items.FindByValue(s) != null)
            //    cblInspection.Items.FindByValue(s).Selected = true;
        }
    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
}

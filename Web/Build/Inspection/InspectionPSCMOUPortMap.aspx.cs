using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class Inspection_InspectionPSCMOUPortMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
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
                string source = ReadCheckBoxList(cblport);

                PhoenixInspectionPSCMOUMatrix.UpdatePSCMOUMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString()), General.GetNullableInteger(ddlCountrySearch.SelectedCountry)
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
        cblport.Items.Clear();
        cblport.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUSeaport(General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString()),General.GetNullableInteger(ddlCountrySearch.SelectedCountry) == null ? 0 : General.GetNullableInteger(ddlCountrySearch.SelectedCountry));
        cblport.DataBindings.DataTextField = "FLDSEAPORTNAME";
        cblport.DataBindings.DataValueField = "FLDSEAPORTID";
        cblport.DataBind();
    }
    private void BindData()
    {
        BindCheckbox();
        DataTable dt = PhoenixInspectionPSCMOUMatrix.ListPSCMOUSeaportMappedList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                       , General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString()), General.GetNullableInteger(ddlCountrySearch.SelectedCountry));


        if (dt.Rows.Count > 0)
        {
            //ddlCountrySearch.SelectedCountry = dt.Rows[0]["FLDCOUNTRYID"].ToString();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = (DataRow)dt.Rows[i];
                string s = dr["FLDMAPPEDSEAPORT"].ToString();
                if (s != "")
                {
                    if (cblport.Items[i].Value == s)
                        cblport.Items[i].Selected = true;
                }
                //if (cblInspection.Items.FindByValue(s) != null)
                //    cblInspection.Items.FindByValue(s).Selected = true;
            }
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

    protected void ddlCountrySearch_TextChangedEvent(object sender, EventArgs e)
    {
        //BindCheckbox();
        BindData();
    }

}
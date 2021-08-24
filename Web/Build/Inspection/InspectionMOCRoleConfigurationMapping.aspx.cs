using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionMOCRoleConfigurationMapping : PhoenixBasePage
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
                //ViewState["COMPANYID"] = "";
                //NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                //if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                //{
                //    ViewState["COMPANYID"] = nvc.Get("QMS");
                //}

                ViewState["ROLEAPPROVERID"] = "";
                ViewState["MID"] = "";

                if (Request.QueryString["roleapproverroleid"] != null)
                    ViewState["ROLEAPPROVERID"] = Request.QueryString["roleapproverroleid"].ToString();

                if (Request.QueryString["Mid"] != null)
                    ViewState["MID"] = Request.QueryString["Mid"].ToString();

                if (Request.QueryString["Role"] != null)
                    txtRole.Text = Request.QueryString["Role"].ToString();

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
                if (ViewState["ROLEAPPROVERID"].ToString() != "" && ViewState["MID"].ToString() != "")
                {
                    string source = ReadCheckBoxList(cblInspection);

                    PhoenixInspectionMOCApproverRole.UpdateProcess(General.GetNullableGuid(ViewState["ROLEAPPROVERID"].ToString())
                         , txtRole.Text
                         , source);

                    ucStatus.Text = "Information Updated successfully.";
                }
                else
                {
                    string source = ReadCheckBoxList(cblInspection);

                    PhoenixInspectionMOCApproverRole.MOCApproverConfigurationRoleProcessInsert(source, General.GetNullableGuid(ViewState["ROLEAPPROVERID"].ToString()));
                    ucStatus.Text = "MOC Role mapped successfully.";

                    BindData();
                }
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
        //cblInspection.Items.Clear();
        cblInspection.DataSource = PhoenixInspectionMOCApproverRole.MOCRoleConfigurationProcessList(277);
        cblInspection.DataBindings.DataTextField = "FLDHARDNAME";
        cblInspection.DataBindings.DataValueField = "FLDHARDCODE";
        cblInspection.DataBind();
    }
    private void BindData()
    {
        if (ViewState["ROLEAPPROVERID"].ToString() == null)
        {
            BindCheckbox();
        }
        else
        {
            DataSet ds = PhoenixInspectionMOCApproverRole.EditProcess(General.GetNullableGuid(ViewState["ROLEAPPROVERID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRole.Text = dr["FLDMOCAPPROVERROLE"].ToString();
                General.RadBindCheckBoxList(cblInspection, dr["FLDHARDPROCESSID"].ToString());
            }
            //DataTable dt = PhoenixInspectionOilMajorComany.ListOilMajorCompanyInspections(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //               , General.GetNullableGuid(ViewState["OILMAJORCOMPANYID"].ToString()), General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = (DataRow)dt.Rows[i];
            //    string s = dr["FLDINSPECTIONID"].ToString();
            //    if (cblInspection.Items[i].Value == s)
            //        cblInspection.Items[i].Selected = true;
            //    //if (cblInspection.Items.FindByValue(s) != null)
            //    //    cblInspection.Items.FindByValue(s).Selected = true;
            //}
        }
    }
    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                //list = list + item.Value.ToString() + ",";
                list += item.Value + ",";
            }
        }
        if (list != "")
        {
            list = "," + list;
        }
        //list = list.TrimEnd(',');
        return list;
    }
}

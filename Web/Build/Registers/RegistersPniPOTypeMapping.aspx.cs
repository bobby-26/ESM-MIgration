using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Registers_RegistersPniPOTypeMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersPniPOTypeMapping.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvpnimapping')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../Registers/RegistersPniPOTypeMapping.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        //toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelpactivity','Company','Registers/RegistersCompany.aspx')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADDCOMPANY");
        toolbar.AddFontAwesomeButton("../Registers/RegistersPniPOTypeMapping.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuRegistersPNI.AccessRights = this.ViewState;
        MenuRegistersPNI.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        if (!IsPostBack)
        {

            ViewState["PAGENUMBER"] = 1;
       
            gvpnimapping.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTYPE", "FLDNAME", "FLDSIGNERYN" };
            string[] alCaptions = { "Type", "Name", "Signer" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

          

            DataSet ds = PhoenixRegistersPniPoMapping.PniPoMappingSearch(
                  cmbtype.SelectedValue.ToString()               
                , int.Parse(ViewState["PAGENUMBER"].ToString())
                , gvpnimapping.PageSize
                , ref iRowCount
                , ref iTotalPageCount
                );

         
            General.SetPrintOptions("gvpnimapping", "PNI Mapping", alCaptions, alColumns, ds);

            gvpnimapping.DataSource = ds;
            gvpnimapping.VirtualItemCount = iRowCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvpnimapping_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvpnimapping.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvpnimapping_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if(e.CommandName.ToUpper()=="ADD")
        {
            try
            {

                RadComboBox cmbsubtypeadd = (RadComboBox)e.Item.FindControl("cmbsubtypeadd");
                UserControlQuick cmdmappingtypeadd = (UserControlQuick)e.Item.FindControl("cmdmappingtypeadd");
                RadComboBox cmbsigneradd = (RadComboBox)e.Item.FindControl("cmbsigneradd");
                if(!IsValidation(cmdmappingtypeadd.SelectedValue.ToString(),General.GetNullableGuid(cmbsubtypeadd.SelectedValue.ToString()),
                            General.GetNullableInteger(cmbsigneradd.SelectedValue.ToString())))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersPniPoMapping.PniPoMappingInsert(cmdmappingtypeadd.SelectedValue, new Guid(cmbsubtypeadd.SelectedValue), int.Parse(cmbsigneradd.SelectedValue));
            }
            catch(Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
    }

    protected void MenuRegistersPNI_TabStripCommand(object sender, EventArgs e)
    {

    }
    private bool IsValidation(string type, Guid? subtype, int? signer)
    {
        if(type=="" || type ==null)
            ucError.ErrorMessage = "Select type";
        if (subtype == null)
            ucError.ErrorMessage = "Select heading";
        if (signer == null)
            ucError.ErrorMessage = "select on/off signer";
        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvpnimapping_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridFooterItem)
        {
            RadComboBox cmbsubtypeadd = (RadComboBox)e.Item.FindControl("cmbsubtypeadd");

            DataTable dt = PhoenixRegistersPniPoMapping.PniSubtypeList();
            cmbsubtypeadd.DataSource = dt;
            cmbsubtypeadd.DataBind();
            cmbsubtypeadd.DataTextField = "FLDPARTNAME";
            cmbsubtypeadd.DataValueField = "FLDID";
        }
    }
}
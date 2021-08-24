using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;

public partial class DocumentManagement_DocumentManagementFMSManualVesselsAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (Request.QueryString["FILENOID"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["FileNoID"] = Request.QueryString["FileNoID"].ToString();
            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuFMSFileNo.AccessRights = this.ViewState;
            MenuFMSFileNo.MenuList = toolbar.Show();

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            BindVesselTypeList();
            BindVesselList();
        }
    }
    protected void FMSFileNo_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                if (ViewState["FileNoID"] != null)
                {
                    string vessellist = GetSelectedVessels();
                    DataSet ds = PhoenixDocumentManagementFMSManual.FMSVesselMap(new Guid(ViewState["FileNoID"].ToString()), vessellist);
                    ucStatus.Text = "Vessels Updated";
                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

        }
    }

    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataBindings.DataTextField = "FLDHARDNAME";
        chkVesselType.DataBindings.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }

    protected void BindVesselList()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        chkVessel.Items.Clear();
        chkVessel.DataSource = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , null, companyid, null, 1);
        chkVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVessel.DataBindings.DataValueField = "FLDVESSELID";
        chkVessel.DataBind();
    }

    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        foreach (ButtonListItem item in chkVessel.Items)
            item.Selected = false;

        int? companyid = General.GetNullableInteger(ViewState["COMPANYID"].ToString());
        DataSet ds = PhoenixRegistersVessel.ListVessel(null, null, null, null, null, companyid, General.GetNullableString(selectedcategorylist));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ButtonListItem item in chkVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected && General.GetNullableString(selectedcategorylist) != null)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
    }

    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = true;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ButtonListItem li in chkVesselType.Items)
                    li.Selected = false;

                foreach (ButtonListItem li in chkVessel.Items)
                    li.Selected = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected string GetSelectedVesselType()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ButtonListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);

        string vesseltype = strvesseltype.ToString();
        return vesseltype;
    }
    protected string GetSelectedVessels()
    {
        StringBuilder strVessel = new StringBuilder();
        foreach (ButtonListItem item in chkVessel.Items)
        {
            if (item.Selected == true)
            {
                strVessel.Append(item.Value.ToString());
                strVessel.Append(",");
            }
        }

        if (strVessel.Length > 1)
            strVessel.Remove(strVessel.Length - 1, 1);

        string categoryList = strVessel.ToString();
        return categoryList;
    }
}


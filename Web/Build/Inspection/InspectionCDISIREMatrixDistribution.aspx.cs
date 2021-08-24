using System;
using System.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionCDISIREMatrixDistribution : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            confirm.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Distribute", "DISTRIBUTE", ToolBarDirection.Right);
            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["SelectedVesselList"] = "";
                ViewState["VesselSelection"] = "";
                ViewState["INSPECTIONID"] = "";

                if (Request.QueryString["inspectionid"] != null && Request.QueryString["inspectionid"].ToString() != string.Empty)
                    ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();

                BindVesselTypeList();
                BindVesselList();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        chkVessel.DataSource = PhoenixDocumentManagementDocument.DMSVesselList(null, companyid);
        chkVessel.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVessel.DataBindings.DataValueField = "FLDVESSELID";
        chkVessel.DataBind();
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


    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DISTRIBUTE"))
            {
                ViewState["DISTRIBUTEYN"] = "0";
                RadWindowManager1.RadConfirm("Do you want to distribute.? Y/N", "confirm", 320, 150, null, "Distribute");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        //ViewState["SelectedVesselList"] = "";
        foreach (ButtonListItem item in chkVessel.Items)
            item.Selected = false;

        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        DataSet ds = PhoenixDocumentManagementDocument.DMSVesselList(selectedcategorylist, companyid);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ButtonListItem item in chkVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected)
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

            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {
            string script = "resizeDiv(divManual);resizeDiv(divFormCategory);resizeDiv(divJHACategory);resizeDiv(divVesselType);resizeDiv(divVesselType);resizeDiv(divVessel);\r\n;";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

            string strVessels = GetSelectedVessels();
            int? distributionyn = General.GetNullableInteger(ViewState["DISTRIBUTEYN"].ToString());

            PhoenixInspectionCDISIREMatrix.CDISIREDistribution(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                            new Guid(ViewState["INSPECTIONID"].ToString()),
                                            distributionyn,
                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                            strVessels);

            ucStatus.Text = "CDI/SIRE Matrix distributed successfully.";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
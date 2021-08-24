using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class PlannedMaintenanceReportComponent : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show Report", "GO",ToolBarDirection.Right);
            MenuReportComponent.AccessRights = this.ViewState;
            MenuReportComponent.MenuList = toolbarmain.Show();

            txtVendorId.Attributes.Add("style", "visibility:hidden");
            txtMakerId.Attributes.Add("style", "visibility:hidden");

            string ScreenName = Request.QueryString["report"].ToString();

            if (!IsPostBack)
            {
               
                ViewState["ISTREENODECLICK"] = false;
                ViewState["CURRENTINDEX"] = 1;
                if (Request.QueryString["COMPONENTID"] != null)
                    ViewState["WIEVCOMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                {
                    ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();
                }

                //if (Request.QueryString["report"].ToString() == "code")
                //{
                //    Title1.Text = "Component Code";
                //}
                //else if (Request.QueryString["report"].ToString() == "maker")
                //{
                //    Title1.Text = "Component Maker";
                //}
            }
            imgShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            cmdShowMaker.Attributes.Add("onclick", "javascript:return showPickList('spnPickListVendor', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=130,131', true);");
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuReportComponent_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper() == "GO")
            {
                string prams = "";

                prams += "&vesselid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                prams += "&compno=" + General.GetNullableString(txtComponentNumber.Text.Trim());
                prams += "&compname=" + General.GetNullableString(txtComponentName.Text.Trim());
                prams += "&comptype=" + General.GetNullableString(txtComponentType.Text);
                prams += "&loccode=" + General.GetNullableString(txtLocation.Text); 
                prams += "&vendid=" + General.GetNullableInteger(txtVendorId.Text);
                prams += "&makeid=" + General.GetNullableInteger(txtMakerId.Text);
                prams += "&classcode=" + General.GetNullableString(txtClassCode.Text);
                prams += exceloptions();

                if (Request.QueryString["report"].ToString() == "code")
                {
                    prams += "&sortby=FLDCOMPONENTNUMBER";
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=COMPONENTCODE" + prams);
                }
                else if (Request.QueryString["report"].ToString() == "maker")
                {
                    prams += "&sortby=FLDMAKERNAME";
                    Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=6&reportcode=COMPONENTMAKER" + prams);
                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected string exceloptions()
    {
        DataSet ds = new DataSet();

        ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 181);

        string options = "";

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["FLDSHORTNAME"].ToString().Equals("EXL"))
                options += "&showexcel=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("WRD"))
                options += "&showword=" + dr["FLDHARDNAME"].ToString();

            if (dr["FLDSHORTNAME"].ToString().Equals("PDF"))
                options += "&showpdf=" + dr["FLDHARDNAME"].ToString();
        }
        return options;
    }

    protected void cmdMakerClear_Click(object sender, ImageClickEventArgs e)
    {
        txtMakerCode.Text = "";
        txtMakerName.Text = "";
        txtMakerId.Text = "";
    }

    protected void cmdVendorClear_Click(object sender, ImageClickEventArgs e)
    {
        txtVendorNumber.Text = "";
        txtVenderName.Text = "";
        txtVendorId.Text = "";
    }
}

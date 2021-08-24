using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Common;

public partial class VesselPositionEUMRVProcedureDetailViewE2 : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PROCEDUREID"] = "";
                ProcedureDetailEdit();
               
                    PhoenixToolbar toolbarmain = new PhoenixToolbar();
                    if (Request.QueryString["Lanchfrom"].ToString() == "0")
                        toolbarmain.AddButton("Company Procedure", "PROCEDUREDETAIL");
                    if (Request.QueryString["Lanchfrom"].ToString() == "1")
                        toolbarmain.AddButton("Ship Specific Procedure", "PROCEDUREDETAIL");
                   
                    MenuProcedureDetailList.AccessRights = this.ViewState;
                    MenuProcedureDetailList.MenuList = toolbarmain.Show();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void TabProcedure_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
           
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ProcedureDetailEdit()
    {
        DataSet ds = PhoenixVesselPositionEUMRV.EUMRVProcedureDetailEdit(General.GetNullableGuid(Request.QueryString["DetailID"].ToString()));
        DataTable dt=ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            txtReferencetoExisting.Text = dt.Rows[0]["FLDPROCEDUREREFERENCE"].ToString();
            txtVersion.Text = dt.Rows[0]["FLDPROCEDUREVERSION"].ToString();
            txteuprocedure.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            txtxpersonreponsible.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            txtlocation.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            txtSystemUsed.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
            ViewState["PROCEDUREID"] = dt.Rows[0]["FLDPROCEDUREID"].ToString();
            txtDocumentNameEdit.InnerText = dt.Rows[0]["FLDFORMNAME"].ToString();
            string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
            txtProcedure.Text = dt.Rows[0]["FLDCODE"].ToString() + "-" + dt.Rows[0]["FLDPROCEDURE"].ToString() + Guidance;
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("FROM"))
                txtDocumentNameEdit.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + dt.Rows[0]["FLDFILEPATH"].ToString() + "'); return false;");
            if (dt.Rows[0]["FLDRECORDTYPE"].ToString().ToUpper().Equals("DOCUMENT"))
                txtDocumentNameEdit.Attributes.Add("onclick", "javascript:Openpopup('codehelp','','../DocumentManagement/DocumentManagementDocumentSectionRevisonGeneral.aspx?DOCUMENTID=" + dt.Rows[0]["FLDDOCUMENTID"].ToString() + "&SECTIONID=" + dt.Rows[0]["FLDSECTIONID"].ToString() + "&REVISONID=" + dt.Rows[0]["FLDREVISIONID"].ToString() + "'); return false;");
            txtlistexistingmgntsystem.Text = dt.Rows[0]["FLDLISTMGNTSYSTEM"].ToString();

            if (Request.QueryString["Lanchfrom"].ToString() == "0")
                Title1.Text = "Company Procedure View";
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Title1.Text = "Ship Specific Procedure View";
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            if (Request.QueryString["Lanchfrom"].ToString() == "1")
                Response.Redirect("../VesselPosition/VesselPositionEUMRVShipSpecificProcedure.aspx?Lanchfrom=1");
            else
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx?Lanchfrom=0");
        }   
    }
}

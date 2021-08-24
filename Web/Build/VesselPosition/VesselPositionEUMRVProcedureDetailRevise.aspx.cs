using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;

public partial class VesselPositionEUMRVProcedureDetailRevise : PhoenixBasePage
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

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Procedure", "PROCEDURE");
            toolbarmain.AddButton("Company Procedure", "PROCEDUREDETAIL");
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbartab = new PhoenixToolbar();
            toolbartab.AddButton("Save", "SAVE");
            TabProcedure.AccessRights = this.ViewState;
            TabProcedure.MenuList = toolbartab.Show();

            if (!IsPostBack)
            {
                ViewState["PROCEDUREID"] = "";
                ProcedureDetailEdit();
                
            }
            int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTreeForAllNodes.aspx?iframignore=true&companyid=" + companyid + "', true); ");
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
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {

                //PhoenixVesselPositionEUMRV.InsertEUMRVProcedureDetail(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //                                                        General.GetNullableGuid(ViewState["PROCEDUREID"].ToString()),
                //                                                        General.GetNullableString(txtReferencetoExisting.Text.Trim()),
                //                                                        General.GetNullableInteger(txtVersion.Text.Trim()),
                //                                                        General.GetNullableString(txteuprocedure.Text.Trim()),
                //                                                        General.GetNullableString(txtxpersonreponsible.Text.Trim()),
                //                                                        General.GetNullableString(txtlocation.Text.Trim()),
                //                                                        General.GetNullableString(txtSystemUsed.Text.Trim()),
                //                                                        "REVISE",General.GetNullableGuid(txtDocumentId.Text));
                Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailList.aspx");
            }
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
            string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
            txtProcedure.Text = dt.Rows[0]["FLDCODE"].ToString() + "-" + dt.Rows[0]["FLDPROCEDURE"].ToString() + Guidance;
            txtReferencetoExisting.Text = dt.Rows[0]["FLDPROCEDUREREFERENCE"].ToString();
            txtVersion.Text = (int.Parse( dt.Rows[0]["FLDPROCEDUREVERSION"].ToString())+1).ToString();
            txteuprocedure.Text = dt.Rows[0]["FLDEUMRVPROCEDURE"].ToString();
            txtxpersonreponsible.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            txtlocation.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            txtSystemUsed.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
        }
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        //if (dce.CommandName.ToUpper().Equals("PROCEDURE"))
        //{
        //    Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedure.aspx");
        //}
        if (dce.CommandName.ToUpper().Equals("PROCEDUREDETAIL"))
        {
            Response.Redirect("../VesselPosition/VesselPositionEUMRVProcedureDetailList.aspx");
        }   
    }
}

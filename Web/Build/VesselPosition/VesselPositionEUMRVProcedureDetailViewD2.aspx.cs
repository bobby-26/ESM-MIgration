using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Common;

public partial class VesselPositionEUMRVProcedureDetailViewD2 : PhoenixBasePage
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
            txtVersion.Text = dt.Rows[0]["FLDPROCEDUREVERSION"].ToString();
            txtxpersonreponsible.Text = dt.Rows[0]["FLDRESPONSIBLEPERSION"].ToString();
            txtlocation.Text = dt.Rows[0]["FLDRECORDLOCATION"].ToString();
            txtSystemUsed.Text = dt.Rows[0]["FLDITSYSTEMNAME"].ToString();
            ViewState["PROCEDUREID"] = dt.Rows[0]["FLDPROCEDUREID"].ToString();
            txtFormulae.Text = dt.Rows[0]["FLDFORMULAE"].ToString();
            txtDatasource.Text = dt.Rows[0]["FLDDATASOURCE"].ToString();
            string Guidance = General.GetNullableString(dt.Rows[0]["FLDGUIDANCE"].ToString()) != null ? " (" + dt.Rows[0]["FLDGUIDANCE"].ToString() + ")" : "";
            txtProcedure.Text = dt.Rows[0]["FLDCODE"].ToString() + "-" + dt.Rows[0]["FLDPROCEDURE"].ToString() + Guidance;
            txtMethodTreat.Text = dt.Rows[0]["FLDTREATDATAGAPS"].ToString();

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

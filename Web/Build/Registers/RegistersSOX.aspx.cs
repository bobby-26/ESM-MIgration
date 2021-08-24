using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersSOX : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
            //{
            //    ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            //    ucVesselName.Enabled = false;
            //}
            //else
            //{
            //    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            //    {
            //        ucVesselName.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            //        ucVesselName.Enabled = false;
            //    }
            //}

            if (Request.QueryString["SOxId"] != null)
            {
                toolbar.AddButton("Save", "SAVE");
                ViewState["SOxId"] = Request.QueryString["SOxId"].ToString();

                SOxEdit(new Guid(Request.QueryString["SOxId"].ToString()));
            }
            else
            {
                toolbar.AddButton("Save", "SAVE");
            }
            MenuSOx.AccessRights = this.ViewState;
            MenuSOx.MenuList = toolbar.Show();
        }
    }

    //private bool IsValidSOx()
    //{
    //    if (General.GetNullableInteger(ucVesselName.SelectedVessel) == null)
    //        ucError.ErrorMessage = "Vessel name is required.";

    //    return (!ucError.IsError);
    //}

    protected void SOx_TabStripCommand(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";

        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            try
            {
                //if (!IsValidSOx())
                //{
                //    ucError.Visible = true;
                //    return;
                //}

                if (ViewState["SOxId"] != null)
                {
                    PhoenixRegistersSOX.UpdateSOX(
                        new Guid((ViewState["SOxId"].ToString())),
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        null,
                        General.GetNullableDecimal(rblOutsideECA.SelectedValue),
                        General.GetNullableDecimal(rblInsideECA.SelectedValue),
                        General.GetNullableDecimal(rblAtBerth.SelectedValue),
                        General.GetNullableDecimal(txtOutWeightage.Text),
                        General.GetNullableDecimal(txtInWeightage.Text),
                        General.GetNullableDecimal(txtAtBerthWeightage.Text));
                }
                else
                {
                    PhoenixRegistersSOX.InsertSOX(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        null,
                        General.GetNullableDecimal(rblOutsideECA.SelectedValue),
                        General.GetNullableDecimal(rblInsideECA.SelectedValue),
                        General.GetNullableDecimal(rblAtBerth.SelectedValue),
                        General.GetNullableDecimal(txtOutWeightage.Text),
                        General.GetNullableDecimal(txtInWeightage.Text),
                        General.GetNullableDecimal(txtAtBerthWeightage.Text), 1);
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
    }

    private void SOxEdit(Guid soxid)
    {
        DataSet ds = PhoenixRegistersSOX.EditSOX(soxid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtOutWeightage.Text = dr["FLDOUTWEIGHTAGE"].ToString();
            txtInWeightage.Text = dr["FLDINWEIGHTAGE"].ToString();
            txtAtBerthWeightage.Text = dr["FLDBERTHWEIGHTAGE"].ToString();

            rblOutsideECA.SelectedValue = dr["FLDBSOUT"].ToString();
            rblInsideECA.SelectedValue = dr["FLDBSIN"].ToString();
            rblAtBerth.SelectedValue = dr["FLDBSBERTH"].ToString();

            //ucVesselName.SelectedVessel = dr["FLDVESSELID"].ToString();
            //ucVesselName.Enabled = false;
        }
    }
}

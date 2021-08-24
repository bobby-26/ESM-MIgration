using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsAdminSignOnOffEdit :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuMainList.AccessRights = this.ViewState;
            MenuMainList.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["vesselid"] = 0;
                if (Request.QueryString["SIGNONOFFID"] != null)
                    ViewState["SIGNONOFFID"] = Request.QueryString["SIGNONOFFID"].ToString();
                if (Request.QueryString["vesselid"] != null)
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMainList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='signonoff'>" + "\n";
            scriptClosePopup += "fnReloadList('signonoff');";
            scriptClosePopup += "</script>" + "\n";


            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidsignonoff())
                {
                    ucError.Visible = true;
                    return;
                }

                int SignOnOffid = int.Parse(ViewState["SIGNONOFFID"].ToString());
                string Rank = ddlRank.SelectedRank;
                string signondate = txtSignOnDate.Text;
                string signoffdate = txtSignOffDate.Text;
                string ReliefDuedate = txtReliefDueDate.Text;
                string Etodate = txtEtoDate.Text;
                string Btodate = txtBtoDate.Text;
                string reason = ddlReason.SelectedSignOffReason;
                string SeaPort = ddlSeaPort.SelectedValue;
                string Status = ddlSignOnOffStatus.SelectedValue;
                if (reason == "Dummy")
                {
                    reason = null;
                }
                if (SeaPort == "Dummy")
                {
                    SeaPort = null;
                }
                PhoenixVesselAccountsCorrections.UpdateVesselAdminSignOnOff(int.Parse(ViewState["vesselid"].ToString()), SignOnOffid, General.GetNullableInteger(Rank)
                                                                              , General.GetNullableDateTime(signondate), General.GetNullableDateTime(signoffdate)
                                                                              , General.GetNullableDateTime(ReliefDuedate), General.GetNullableDateTime(Btodate)
                                                                              , General.GetNullableDateTime(Etodate), General.GetNullableInteger(reason)
                                                                              , General.GetNullableInteger(SeaPort), int.Parse(Status));


                BindData();
                Ucsstats.Text = "Updated Sucessfully.";
                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "signonoff", scriptClosePopup, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidsignonoff()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        if (ddlRank.SelectedRank.Equals("Dummy"))
            ucError.ErrorMessage = "Rank is required.";
        if ((General.GetNullableDateTime(txtSignOnDate.Text) == null))
            ucError.ErrorMessage = "Sign on is required.";
        if (ddlSignOnOffStatus.SelectedValue == "0")
        {
            if ((General.GetNullableDateTime(txtSignOffDate.Text) == null))
                ucError.ErrorMessage = "Sign off is required.";
            if (General.GetNullableInteger(ddlReason.SelectedSignOffReason) == null)
                ucError.ErrorMessage = "Sign off reason is required.";
            if (!int.TryParse(ddlSeaPort.SelectedValue, out resultInt))
                ucError.ErrorMessage = "Sign off Port is required.";

        }
        return (!ucError.IsError);
    }

    public void BindData()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsCorrections.EditVesselAdminSignOnOff(int.Parse(ViewState["vesselid"].ToString()), int.Parse(ViewState["SIGNONOFFID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtFile.Text = dt.Rows[0]["FLDFILENO"].ToString();
                txtpassportno.Text = dt.Rows[0]["FLDPASSPORTNO"].ToString();
                txtCDC.Text = dt.Rows[0]["FLDSEAMANBOOKNO"].ToString();
                ViewState["SIGNONOFFID"] = dt.Rows[0]["FLDSIGNONOFFID"].ToString();
                txtName.Text = dt.Rows[0]["FLDNAME"].ToString();
                ddlRank.SelectedRank = dt.Rows[0]["FLDSIGNONRANKID"].ToString();
                txtSignOnDate.Text = dt.Rows[0]["FLDSIGNONDATE"].ToString();
                txtSignOffDate.Text = dt.Rows[0]["FLDSIGNOFFDATE"].ToString();
                txtReliefDueDate.Text = dt.Rows[0]["FLDRELIEFDUEDATE"].ToString();
                txtEtoDate.Text = dt.Rows[0]["FLDETOD"].ToString();
                txtBtoDate.Text = dt.Rows[0]["FLDBTOD"].ToString();
                ddlReason.SelectedSignOffReason = dt.Rows[0]["FLDSIGNOFFREASONID"].ToString();

                ddlSeaPort.Text = dt.Rows[0]["FLDSIGNOFFSEAPORTNAME"].ToString();
                ddlSeaPort.SelectedValue = dt.Rows[0]["FLDSIGNOFFSEAPORTID"].ToString();
                ddlSignOnOffStatus.SelectedValue = dt.Rows[0]["FLDSTATUS"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

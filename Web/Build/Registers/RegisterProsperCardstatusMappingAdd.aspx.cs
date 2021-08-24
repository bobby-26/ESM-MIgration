using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class RegisterProsperCardstatusMappingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["cardstatusid"] = null;
            if(Request.QueryString["cardstatusid"].ToString() != null)
            {
                ViewState["cardstatusid"] = Request.QueryString["cardstatusid"].ToString();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                BindCardstatus();
                ddlcardstatus.SelectedValue = ViewState["cardstatusid"].ToString();
                txtcardstatus.Text = ddlcardstatus.SelectedItem.Text.ToString();
            }
            MenuCardstatusMapping.AccessRights = this.ViewState;
            MenuCardstatusMapping.MenuList = toolbar.Show();
        }
    }

    protected void BindCardstatus()
    {
        ddlcardstatus.Items.Clear();
        ddlcardstatus.DataSource = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusList();
        ddlcardstatus.DataTextField = "FLDCARDSTATUSNAME";
        ddlcardstatus.DataValueField = "FLDCARDSTATUSID";
        ddlcardstatus.DataBind();
        ddlcardstatus.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddlcardstatus.SelectedIndex = 0;
    }

    protected void MenuCardstatusMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='Cardstatus'>" + "\n";
            scriptClosePopup += "fnReloadList('Cardstatus');";
            scriptClosePopup += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                
                string MinPointsRequired = txtminpointsrequired.Text.ToString();
                string MaxPointsRequired = txtmaxpointsrequired.Text.ToString();
                string ucRankid = ucRank.SelectedRank.ToString();

                if (!IsValidCardstatusMapping(ucRankid,MinPointsRequired,MaxPointsRequired))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusMappingInsert(
                    General.GetNullableGuid(ddlcardstatus.SelectedValue.ToString())
                    , General.GetNullableInteger(ucRank.SelectedRank.ToString())
                    , General.GetNullableInteger(MinPointsRequired.ToString())
                    , General.GetNullableInteger(MaxPointsRequired.ToString())
                    );

                ucStatus.Text = "Information Added";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "CardstatusNew", scriptClosePopup);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidCardstatusMapping(string Rank, string MinPointsRequired, string MaxPointsRequired)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Rank) == null)
            ucError.ErrorMessage = "Rank is required.";
        if (General.GetNullableInteger(MinPointsRequired) == null)
            ucError.ErrorMessage = "Min Points Required is required.";
        if (General.GetNullableInteger(MaxPointsRequired) == null)
            ucError.ErrorMessage = "Max Points Required is required.";

        return (!ucError.IsError);

    }
    protected void ddlcardstatus_OnTextChanged(object sender, EventArgs e)
    {
        if (ViewState["CardstatusID"] != null)
        {
            ddlcardstatus.SelectedValue = ViewState["CardstatusID"].ToString();
        }
    }
}
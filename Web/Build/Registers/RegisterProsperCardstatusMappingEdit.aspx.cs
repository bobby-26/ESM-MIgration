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

public partial class RegisterProsperCardstatusMappingEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["cardstatusmappingid"] = null;
            if (Request.QueryString["cardstatusmappingid"].ToString() != null)
            {
                ViewState["cardstatusmappingid"] = Request.QueryString["cardstatusmappingid"].ToString();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                BindCardstatus();
                CardstatusMapppingEdit(ViewState["cardstatusmappingid"].ToString());
            }
            MenuCardstatusMapping.AccessRights = this.ViewState;
            MenuCardstatusMapping.MenuList = toolbar.Show();
        }
    }
    private void CardstatusMapppingEdit(string CardstatusMappingid)
    {
        try
        {
            DataTable dt = PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusMappingEdit(Guid.Parse(CardstatusMappingid));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtcardstatus.Text = dr["FLDCARDSTATUSNAME"].ToString();
                ddlcardstatus.SelectedValue = dr["FLDCARDSTATUSID"].ToString();
                ucRank.SelectedRank = dr["FLDRANKID"].ToString();
                txtrank.Text = dr["FLDRANKNAME"].ToString();
                txtminpointsrequired.Text = dr["FLDMINPOINTSREQUIRED"].ToString();
                txtmaxpointsrequired.Text = dr["FLDMAXPOINTSREQUIRED"].ToString();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
            scriptClosePopup += "<script language='javaScript' id='CardStatus'>" + "\n";
            scriptClosePopup += "fnReloadList('CardstatusCode', true);";
            scriptClosePopup += "</script>" + "\n";

            //string scriptClosePopup = "";
            //scriptClosePopup += "<script language='javaScript' id='Cardstatus'>" + "\n";
            //scriptClosePopup += "fnReloadList('Cardstatus');";
            //scriptClosePopup += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string MinPointsRequired = txtminpointsrequired.Text.ToString();
                string MaxPointsRequired = txtmaxpointsrequired.Text.ToString();
                string ucRankid = ucRank.SelectedRank.ToString();

                if (!IsValidCardstatusMapping(ucRankid, MinPointsRequired, MaxPointsRequired))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegisterProsperCardstatusMapping.ProsperCardstatusMappingUpdate(
                    Guid.Parse(ViewState["cardstatusmappingid"].ToString())
                    , Guid.Parse(ddlcardstatus.SelectedValue.ToString())
                    , Int32.Parse(ucRankid)
                    , Int32.Parse(MinPointsRequired)
                    , Int32.Parse(MaxPointsRequired)
                    );

                ucStatus.Text = "Information Updated";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "CardStatus", scriptClosePopup);
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